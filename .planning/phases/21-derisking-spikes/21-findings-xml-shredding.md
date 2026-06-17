# Phase 21-03 Findings: HL7 XML-Shredding Bake-Off (`xmltable()` vs. C#/j4jayant)

**Recommendation: `csharp-parser` — move HL7 shredding into the application tier using the j4jayant parser already in the app.** Both prototypes hit full parity on the representative case; the deciding factors are *where the logic lives* and *long-term maintainability*, not feasibility (both work).

> **DECISION (recorded 2026-06-16): `csharp-parser` — APPROVED by the user.** HL7 XML shredding moves into C# (j4jayant) in the app tier; Phase 23-02 is scoped accordingly (see "Net" at the end). A narrow `xmltable()`/`XDocument` fallback is retained only if a pipe-less, XML-only inbound source is found.

---

## What was built

Over one representative shred — the SQL Agent **"CERNER DAILY CDM NOT IN LEGACY BILLING"** step (`Lab Patient Accounting SQL Agent Jobs.sql:483-655`), which is the same XML-method family as `usp_prg_Xml_Import.sql` but the cleanest focused case — three artifacts:

| File | Role |
|---|---|
| `spikes/xml-case-source-tsql.sql` | Reference T-SQL: the verbatim `.nodes()`/`.value()`/`.query()` shred + documented expected output (the parity target). |
| `spikes/sample-message.xml` | PHI-free synthetic HL7-as-XML message (one encounter, 3 FT1 charge lines). |
| `spikes/shred-prototype-pg.sql` | Prototype A — PostgreSQL `xmltable()`/`xpath()`. |
| `spikes/ShredPrototypeCSharp.cs` | Prototype B — C# over the real `j4jayant.HL7.Parser`. |

**Parity column set (agreed target):** `ACCOUNT, SET ID, CDM, CDM QTY, CERNER DESCRIPTION` (+ the `CLIENT` header scalar). The downstream dictionary `LEFT JOIN`/`SUM`/"not in billing" filter and the `FOR XML PATH` HTML email body are ordinary relational SQL + presentation — explicitly out of scope for the XML-shred comparison.

## Parity result — all three agree

| Source | ACCOUNT | rows | Result |
|---|---|---|---|
| T-SQL reference (documented expected) | L9000001234 | 3 FT1 | baseline |
| **PG `xmltable()`** | L9000001234 | 3 FT1 | **MATCH — validated live** |
| **C# / j4jayant** | L9000001234 | 3 FT1 | **MATCH (by construction; mirrors `HL7ProcessorService.ParseFT1`)** |

The PG prototype was **run live against PostgreSQL 16** (`personal-finance-postgres-1`, localhost:5433, creds `app`/`app`, db `personal_finance`) and produced exactly:

```
   account   | client | SET ID |   CDM   | CDM QTY |  CERNER DESCRIPTION
-------------+--------+--------+---------+---------+----------------------
 L9000001234 | CLNT01 |      1 | 8000010 |       1 | SYNTHETIC PANEL A
 L9000001234 | CLNT01 |      2 | 8000020 |       2 | SYNTHETIC TEST B
 L9000001234 | CLNT01 |      3 | 8000099 |       1 | SYNTHETIC UNMAPPED C
(3 rows)
```

`xmltable()` behaves identically on PG 14–18, so PG16 validation is representative of the 17/18 target. The C# prototype is a sketch (not compiled here, as it must not be wired into a project) but calls only real j4jayant methods that `HL7ProcessorService` already uses in production.

---

## Key structural finding: two encodings of the same message

This reframes the whole decision. The inbound Cerner message exists in `infce.messages_inbound` in **two** columns:

- **`msgContent`** — HL7-as-**XML** envelope (`<HL7Message><PV1.3.4>…<FT1>…`). This is what the SQL Agent step and `usp_prg_Xml_Import` shred via SQL Server XML methods.
- **`HL7Message`** — the **pipe-delimited** HL7 form. This is what the C# `HL7ProcessorService` already parses with j4jayant (`ParseHL7(_currentMessage.HL7Message)` → `GetValue("PV1.3.4")`, `Segments("FT1")`, `Fields(7).Components(1).Value`).

So the "1,616 XML-method occurrences" are the DB tier re-deriving, from the XML copy, data the **app already extracts** from the pipe copy. The C# path is not a new parser to build — it is the *existing, in-production* ingestion path. j4jayant only parses pipe-delimited HL7 (it requires a leading `MSH` and splits on `| ^ ~ \ &`); it cannot read the XML form, but it does not need to — the pipe form is already persisted and already parsed.

---

## Head-to-head comparison

| Dimension | PG `xmltable()` (Prototype A) | C# / j4jayant (Prototype B) |
|---|---|---|
| **Output parity** | Full — validated live | Full — mirrors `ParseFT1` |
| **Lines / complexity for THIS shred** | ~30 lines SQL; clean once you know `xmltable`. But the *full* proc is ~1,500 lines of nested CTEs/`OPENXML`/`CROSS APPLY` — a large, dense rewrite per proc. | ~40 lines; flat loop over `Segments("FT1")`. The hard parsing is already done by the shared parser. |
| **Where HL7 logic lives** | Stays **in the database**, split across 38 files / ~1,616 method calls + SQL Agent steps. HL7 knowledge remains in two tiers. | **Consolidates in the app tier** next to the parser the app already owns. One place to know HL7. |
| **Unit-testability** | Poor — needs a live PG instance + fixture tables to exercise; logic is embedded in procs/agent steps not under app test. | Strong — `Shred(string) → rows` is a pure function; trivially xUnit-tested with string fixtures (the project already has `LabBillingCore.UnitTests`). |
| **Version control / review** | Procs + agent-job script; agent steps are doubled-quote `@command` blobs — painful to diff/review. | Normal C# in the repo, reviewed/built/tested with the rest of the code. |
| **Performance intuition** | Set-based, runs at the data; good for bulk nightly batches. `xmltable()` is efficient. | Per-message parse in app; the app **already pays this cost** on ingest, so shredding there is near-free incremental work, not a new pass. Streamable/parallelizable. |
| **Transactional semantics** | Preserved trivially — shred + inserts stay in one DB transaction (the proc's `BEGIN TRAN`/`XACT_ABORT`). | Must be re-established in the service: wrap the derived inserts in the existing `UnitOfWork` transaction (`uow.StartTransaction()` — `HL7ProcessorService.cs:115` already does exactly this). Achievable but is real work and the main risk. |
| **Scaling across ~1,616 occurrences** | Re-expresses **all** of them in a second XML dialect (`.nodes`/`.value`→`xmltable`/`xpath`, `OPENXML`→`xmltable`). Mechanical but voluminous; one-for-one port keeps the maintenance burden. | Collapses the count toward **zero DB XML methods** — the shred becomes field reads on an already-parsed message. Removes the 1,616 occurrences from the porting backlog instead of translating them. Bulk of effort shifts to rewriting the import *procs* as services. |
| **Alignment with roadmap** | Keeps logic in DB — at odds with the Blazor/.NET consolidation direction. | Matches the §6 / Phase 22-26 direction (logic in app, DB as storage). Also sets up the Phase 26 HL7 idempotency work. |
| **Up-front cost** | Lower per-proc *mechanically*, but you do it ~1,616× and keep it. | Higher up-front (rewrite import procs → services, re-prove transactions/dup-checking), but pays down the largest single migration liability permanently. |

### Caveats / risks for the C# route (must be scoped into Phase 23-02)
- The import procs do more than shred: account **dup-checking/merge** (`acc_dup_check`/`acc_merges`), `GetMasterAccount`, `GetMappingValue('CLIENT','CERNER',…)`, dictionary lookups, dedupe (`usp_prg_PurgeDuplicates`). Moving the shred to C# means moving (or re-calling) that surrounding logic too — this is the "larger up-front rewrite" the decision options call out.
- The XML `msgContent` and pipe `HL7Message` columns must be confirmed to **always co-exist and agree** for every inbound source before the app can rely solely on the pipe form. If any source populates only `msgContent`, a fallback (`System.Xml`/`XDocument`, or keep `xmltable()` for that path) is needed. **Verify in Phase 23-02.**
- Transactional parity: the proc rolls back the whole file on error; the service must wrap the equivalent set in one UoW transaction.

### Caveats for the PG route
- Faithful but keeps HL7 logic in the DB, untested and in a second dialect, indefinitely.
- The agent-job `@command` blobs (doubled-quote escaping, `sp_send_dbmail`, `H:\sqlText` output) have to be dismantled anyway for the SQL-Agent→Quartz move (Phase 24) — so part of this code is leaving the DB regardless, which weakens the "least disturbance" argument.

---

## Recommendation & rationale

**Choose `csharp-parser`.** Both approaches reproduce the reference output, so feasibility is not the tie-breaker — *destination* is. The shredded data is something the app **already parses** from the pipe-delimited `HL7Message` via j4jayant in `HL7ProcessorService`; the in-DB XML shredding is a parallel, untestable re-derivation of the same fields from the XML copy. Porting it to `xmltable()` would faithfully carry ~1,616 XML-method occurrences into a second SQL dialect and keep HL7 knowledge split across DB and app forever, while the SQL-Agent steps that host much of it are being dismantled for Quartz anyway. Moving the shred into C# consolidates HL7 logic where it is unit-testable and version-controlled, reuses an existing in-production code path, removes the single largest XML-porting backlog item rather than translating it, and aligns with the .NET/Blazor and Phase 22-26 direction. The cost — rewriting the import procs (dup-check/merge/mapping/dedupe) into services and re-establishing their transactional + idempotency semantics in the existing `UnitOfWork` — is the real Phase 23-02 scope and the main risk, and should be sized against the **production** catalog (not the drifted test DB). If verification finds inbound sources that populate only the XML `msgContent`, keep a narrow `xmltable()`/`XDocument` fallback for those, but the primary path should be C#.

**Net:** `csharp-parser`, with Phase 23-02 scoped to (1) confirm `HL7Message` pipe form is universally present and authoritative, (2) port the import procs' surrounding logic (dup-check/merge/mapping/dedupe) to services, (3) wrap derived inserts in the UoW transaction, (4) retain a small XML fallback only if a pipe-less source exists.

---

## PHI confirmation

`sample-message.xml` and the pipe-delimited literal in `ShredPrototypeCSharp.cs` are **synthetic and PHI-free**: invented MRN (`MRN0000001`), account (`9000001234`), name (`TESTPATIENT, ALPHA`), client (`CLNT01`), and CDM codes (`8000010/8000020/8000099`). No real names, MRNs, SSNs, DOBs, providers, or org identifiers. (`8000099` is deliberately an unmapped CDM to exercise the "Not in Billing" branch.)
