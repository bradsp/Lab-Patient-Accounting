# Phase 21 Finding: MARS-Dependency Audit

> Answers §6 open question: *"Does `Microsoft.Data.SqlClient`'s `MultipleActiveResultSets=true`
> hide places where code holds an open reader while issuing nested queries?"*
> Npgsql has **no MARS**, so any code that iterates one query's reader while issuing a second query
> on the same connection breaks on PostgreSQL.
> Branch: `billing-ui-to-blazor`. Method: static analysis of `LabBilling Library/**/*.cs` (repositories
> AND services) + Read-verification of every candidate loop. **No source was modified.**

---

## 1. Where & why MARS is enabled

`MultipleActiveResultSets = true` is set in the `SqlConnectionStringBuilder` in **two** places, both in
`LabBilling Library/Repositories/AppEnvironment.cs`:

| Site | Connection | file:line |
|---|---|---|
| Main/system connection string | `ConnectionString` (billing + system DBs) | `AppEnvironment.cs:83` |
| Log connection string | `LogConnectionString` | `AppEnvironment.cs:132` |

A solution-wide grep for `MultipleActiveResultSets`/`MARS` (case-insensitive, `*.cs`) found **no other
occurrences** — every other "MARS" hit was a false positive (`MarshalAs`, `Marshal.*`, a city named
"Marshall" in `Dictionaries.cs`). So MARS is enabled in exactly these two connection-string builders
and nowhere else.

**Why it's there:** it is part of a boilerplate `SqlConnectionStringBuilder` block (alongside
`Encrypt=false`, pooling limits, `ConnectTimeout`). There is no comment or code path indicating it was
added to satisfy a specific nested-reader requirement — it appears to be a defensive default.

---

## 2. The PetaPoco streaming-vs-buffering distinction (the crux)

PetaPoco exposes two materialization modes, and which one a repository uses decides MARS risk:

- **`Query<T>` — streamed / lazy.** Returns `IEnumerable<T>` backed by an **open `IDataReader`**;
  the reader stays open until the enumeration completes. Issuing a second DB call on the same
  connection *during* that enumeration is exactly what requires MARS. **This is the HIGH-risk shape.**
- **`Fetch<T>` / `FetchAsync<T>` — buffered / eager.** Drains the reader fully into a `List<T>` and
  **closes the reader before returning**. Iterating the returned list and issuing further DB calls
  opens a *new* command *after* the reader is closed — **no MARS needed.** This is LOW/no risk.

### Usage counts across `LabBilling Library`

| Mode | Occurrences | Files | MARS implication |
|---|---|---|---|
| `Fetch<>` / `FetchAsync<>` (buffered) | **77** | 42 | None — readers closed before return |
| `Query<>` (streamed) | **2** | 2 | Potential — but both immediately materialized (see §3) |

The data-access layer is overwhelmingly **buffered**. The repository base methods confirm this:
`RepositoryBase.GetAll()` (`RepositoryBase.cs:53`) and `GetAllAsync()` (`:67`) use `Fetch`/`FetchAsync`;
`Find()` (`:226`) is a stub returning `null`; no base method returns a deferred `Query<>` enumerable.

---

## 3. Suspect-site table (every `Query<>` site + representative nested-loop sites)

| Site (file:line) | Pattern | Query vs Fetch | Risk | Refactor note |
|---|---|---|---|---|
| `Repositories/PatientStatementRepository.cs:38` | `Context.Query<PatientStatement>(...)` then `return statements.ToList();` (line 40) | **Query** (streamed) | **L** | Reader is fully drained by `.ToList()` on the very next line, in the same method, with no DB call between. No nested call → no MARS dependency. (Tidy-up: switch to `Fetch<>` for clarity.) |
| `Repositories/MessagesInboundRepository.cs:29` | `Context.Query<MessageQueueCount>(sql).ToList();` | **Query** (streamed) | **L** | `.ToList()` chained inline; reader drained immediately, no nested call. No MARS dependency. |
| `Services/AccountService.cs:1216` | `foreach (var chrg in charges)` with nested `RevenueCodeRepository.GetByCode()` / `CdmRepository.GetCdm()` inside | **Fetch** (`charges` ← `ChrgRepository.GetClaimCharges()` → `Fetch<>` at `ChrgRepository.cs:38`) | **L** | Loop iterates a materialized `List<ClaimChargeView>`; reader already closed. Nested calls run on a fresh command. No MARS. (This is the §5.1 account-assembly fan-out.) |
| `Services/HL7ProcessorService.cs:171` | `foreach (var msg in msgsToProcess)` → `ProcessMessage(msg)` (issues many nested DB calls) | **Fetch** (`msgsToProcess` ← `GetUnprocessedMessages()` → `Fetch<>` at `MessagesInboundRepository.cs:42`) | **L** | Loop iterates a materialized `List<MessageInbound>`; reader closed before the loop. No MARS. |
| `Services/AccountService.cs:1218-1219`, `:323`, `:382`, `:1968-1970`; `HL7ProcessorService.cs:323,382` (nested dictionary/repo lookups inside `foreach`) | Nested DB lookups inside `foreach` over `_accountRecord.Insurances` / `.Pat.Diagnoses` / `charges` | **Fetch** / in-memory POCO collections | **L** | All outer collections are either `Fetch<>`-materialized lists or already-loaded in-memory POCO navigation collections (`account.Pat.Diagnoses`, `_accountRecord.Insurances`). No open reader is held across the nested calls. |

**HIGH list: empty.** No site holds an open `Query<>` reader open while issuing a nested DB call.
The two streamed `Query<>` sites both `.ToList()` immediately; every nested-loop-with-DB-call pattern
iterates a buffered list or an in-memory object graph.

---

## 4. Verdict

**MARS is INCIDENTAL, not load-bearing.**

The application's data-access convention is buffer-then-iterate (`Fetch<>` everywhere, 77 sites), and
the two streamed `Query<>` call sites both materialize via `.ToList()` on the same line with no
intervening DB call. The many "loop over a result, query inside the loop" patterns in `AccountService`
and `HL7ProcessorService` — the very places one would expect a MARS dependency — all iterate
**already-materialized `List<T>`** (or in-memory POCO navigation collections), so the reader is closed
before any nested command runs. PostgreSQL/Npgsql will execute these identically without MARS.

### Refactor-site count: **0**

- **0** code paths require a fetch-then-iterate or separate-connection refactor — the code is already
  fetch-then-iterate by convention.
- The only required change is **mechanical**: remove `MultipleActiveResultSets = true` from the two
  `SqlConnectionStringBuilder` sites (`AppEnvironment.cs:83` and `:132`) when `AppEnvironment` is
  rebuilt for Npgsql in **Phase 22-02** (Npgsql's connection-string builder has no such key). This is a
  deletion of two lines, not a logic refactor.

### Recommended confirmation (cheap, optional)

Per the analysis's "smallest experiment": run the existing test suite against a SQL Server connection
string with `MultipleActiveResultSets=false` to empirically confirm zero failures before Phase 22. The
static evidence above predicts a clean pass.
