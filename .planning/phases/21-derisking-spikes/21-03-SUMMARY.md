# Phase 21 Plan 03: XML-Shredding Bake-Off Summary

**Chose `csharp-parser` over `xmltable()` — move HL7 shredding into the app tier (j4jayant); eliminates the ~1,616-occurrence XML backlog rather than translating it, and reuses the in-production pipe-HL7 parse path.**

## Accomplishments
- Built two working prototypes over one PHI-free synthetic HL7 case and the verbatim T-SQL reference shred (from the SQL-Agent "CERNER DAILY CDM" step).
- **PG `xmltable()` prototype validated LIVE** against the running PG16 container (`personal-finance-postgres-1`, localhost:5433) — produced the exact expected rows. `xmltable()` is identical on PG14–18, so PG16 is representative of the 17/18 target.
- **Parity:** all three agree (T-SQL reference = PG `xmltable()` = C#/j4jayant) — same 5-column set (`ACCOUNT, SET ID, CDM, CDM QTY, CERNER DESCRIPTION` + CLIENT), 3 charge rows.
- **Decisive insight:** the inbound message exists in two columns — `msgContent` (XML the DB shreds) and `HL7Message` (pipe-delimited, which `HL7ProcessorService` already parses via j4jayant in production). The in-DB XML shredding re-derives data the app already extracts.

## Files Created/Modified
- `.planning/phases/21-derisking-spikes/spikes/xml-case-source-tsql.sql`
- `.planning/phases/21-derisking-spikes/spikes/sample-message.xml` (PHI-free synthetic)
- `.planning/phases/21-derisking-spikes/spikes/shred-prototype-pg.sql` (validated live)
- `.planning/phases/21-derisking-spikes/spikes/ShredPrototypeCSharp.cs`
- `.planning/phases/21-derisking-spikes/21-findings-xml-shredding.md`
- `.planning/ROADMAP.md` — Phase 21 → 3/4

## Decisions Made
- **`csharp-parser` (user-approved):** HL7 XML shredding moves into C# (j4jayant) in the app tier. Phase 23-02 scope: (1) confirm the pipe `HL7Message` column is universally present/authoritative, (2) port the import procs' surrounding logic (dup-check/merge, GetMasterAccount, GetMappingValue, dedupe) into services, (3) wrap derived inserts in the `UnitOfWork` transaction for idempotency, (4) retain a narrow `xmltable()`/`XDocument` fallback only if a pipe-less XML-only source exists.

## Issues Encountered
- **Message is XML on the DB side, not pipe-HL7.** j4jayant parses only pipe-delimited HL7 (needs leading `MSH`); the C# prototype therefore parses the pipe `HL7Message` column the app already ingests — the real in-production path. Documented in the prototype and findings.
- The C# route's true cost is the surrounding proc logic (not the shred itself) — folded into the Phase 23-02 scope above.

## Next Step
Ready for 21-04-PLAN.md (managed-PostgreSQL HIPAA SKU validation + FINDINGS consolidation).
