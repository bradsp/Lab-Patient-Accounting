# Phase 21 Plan 01: Static-Analysis Spikes Summary

**10 of 67 repository files (≈15%) carry hand-written T-SQL — about 1/3 of the analysis's 40–50% estimate; MARS is NOT load-bearing (0 refactor sites, only 2 connection-string lines to delete).**

## Accomplishments
- **Repository T-SQL backlog:** 18 live port sites across 10 files (+3 deletion sites in the deleted `CustomSqlDatabaseProvider`), out of 67 repository `.cs` files. Corrected backlog = **10/67 ≈ 15%** (or 10/51 ≈ 20% scoped to files containing any SQL). Concentrated in **5 P1 runtime-critical sites**: the 3 `dbo.GetAcc*` UDFs (`ClientRepository.cs:85,97,117`), `GetNextNumber` (`NumberRepository.cs:28`), and `usp_cerner_chrg_reprocess` (`MessagesInboundRepository.cs:90`). All 5 analysis-named sites confirmed present and Read-verified.
- **MARS audit:** Verdict = **incidental, not load-bearing**. **0 refactor sites.** The data layer is buffered (`Fetch<>` ×77 across 42 files); both streamed `Query<>` sites `.ToList()` immediately; all nested-loop-with-DB-call patterns (AccountService fan-out, HL7 processor) iterate materialized lists. Only required change: delete `MultipleActiveResultSets=true` from `AppEnvironment.cs:83` and `:132` during the Phase 22 Npgsql rebuild.

## Files Created/Modified
- `.planning/phases/21-derisking-spikes/21-findings-repo-tsql-backlog.md` - Counted, file:line-cited T-SQL porting backlog with per-file table, prioritized site list, corrected percentage, and analysis-site confirmation.
- `.planning/phases/21-derisking-spikes/21-findings-mars-audit.md` - MARS enable-site confirmation, Query-vs-Fetch counts, suspect-site table, load-bearing verdict + refactor-site count.
- `.planning/ROADMAP.md` - Phase 21 progress 0/4 → 1/4.

## Decisions Made
- T-SQL rewrite surface is materially smaller than estimated and concentrated in 5 runtime-critical UDF/proc call sites — Phase 23 can scope `23-03 Repository T-SQL rewrite` directly from §3 of the backlog.
- No MARS refactor work is needed in Phase 22/23 beyond two-line connection-string cleanup; the optional empirical confirmation is to run the test suite with `MARS=false`.

## Issues Encountered
- `InvoiceSelectRepository.cs:14-19` matched the `dbo.` grep but is a **dead `/* … */` comment**, not live SQL — excluded from the backlog (documented in §6 of the findings).
- `ReportingRepository.cs` is the only repository using raw `SqlConnection`/`SqlDataAdapter` (bypassing PetaPoco); it needs a full Npgsql repoint on top of the date-function rewrite — flagged High.
- Bracket-identifier T-SQL (`[x]`) was **not** found in repository source (PetaPoco emits brackets at the provider layer, auto-handled by the PG provider); the analysis's "brackets pervasive" note applies to the DB project, not the repo `.cs` files.

## Next Step
Ready for 21-02-PLAN.md (runtime DB-object surface trace).
