# Phase 21 Plan 02: Runtime DB-Object Surface Summary

**Runtime path touches ~7 procedural objects (4 procs + 3 scalar UDFs) + ~10 views of ~378 catalog objects; ~271 procs/UDFs and ~90 views are deferrable to post-cutover — confirming the runtime-blocking procedural surface is tiny.**

## Accomplishments
- **Runtime-critical surface:** 7 procedural entry points — scalar UDFs `GetAccBalance`/`GetAccBalByDate`/`GetAccClientBalance`, procs `GetNextNumber`, `usp_cerner_chrg_reprocess`, `usp_prg_pat_bill_update_flags`, `usp_prg_pat_bill_compile` — plus ~8–10 runtime views (`AccountSearchView`, `vw_cbill_select`, `vw_chrg_bill`, `cpt4`×6, +`StatementChargeView`/`InvoiceChargeView`). Each mapped to a Phase 23 plan (UDFs/GetNextNumber→23-01, cerner/pat-bill procs→23-02, views→23-04).
- **Deferrable surface:** ~154 procs, ~117 functions (scalar/TVF), ~92 views never invoked directly from .NET.
- **Background executables** (Console/Service/Scheduler) call **no** procs/UDFs directly — all funnel through `LabBilling Library`.
- **Confidence:** STATIC-ONLY (no live trace). HIPAA-safe XE capture+analysis scripts committed and ready to upgrade this to traced confidence when a data-rich environment exists.

## Files Created/Modified
- `.planning/phases/21-derisking-spikes/spikes/xe-runtime-surface-session.sql` — XE capture (module_start, object-identity-only, no statement text/PHI)
- `.planning/phases/21-derisking-spikes/spikes/xe-analyze-captured-objects.sql` — executed-vs-deferrable analysis/export
- `.planning/phases/21-derisking-spikes/21-findings-runtime-surface.md` — findings
- `.planning/ROADMAP.md` — Phase 21 → 2/4

## Decisions Made
- Resolved the human-action checkpoint via the **static call-graph fallback** (user choice) — the dev DB has no business data and the service/scheduler were not running, so a representative trace was not feasible now.

## Issues Encountered
- **No representative env** → static-only confidence. Mitigations: committed trace scripts for later; flagged transitive in-DB dependency closure as a required Phase 23 step.
- **Test-DB schema drift** reconfirmed: several model-referenced objects (`acc_lock`, `cpt4_ama`, `chrg_diag_pointer`, `remittance_*`, `StatementChargeView`, `InvoiceChargeView`, `LMRPRuleDefinition`) are absent from `LabBillingTest` though they exist in production. Size Phase 23 against the production catalog, not this snapshot.

## Next Step
Ready for 21-03-PLAN.md (XML-shredding approach bake-off).
