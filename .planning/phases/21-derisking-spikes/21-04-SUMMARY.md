# Phase 21 Plan 04: Hosting Validation & FINDINGS Consolidation Summary

**AWS RDS for PostgreSQL 17 chosen as the HIPAA hosting target (validation deferred as an open risk); all four Phase 21 spikes consolidated into FINDINGS.md — Phase 21 complete.**

## Accomplishments
- Built a current (2026), URL-cited managed-PostgreSQL HIPAA matrix across AWS RDS/Aurora, Azure Flexible Server, GCP Cloud SQL, Crunchy Bridge, Aiven. All five sign BAAs for managed PG; only Aiven gates CMK (limited-availability).
- **Recommended SKU:** AWS RDS for PostgreSQL 17 (KMS CMK + `rds.force_ssl` + pgaudit, all self-serve in one SKU); fallback Azure Flexible Server. Npgsql TLS: `SSL Mode=VerifyFull` + `Root Certificate=<RDS CA>` + `Channel Binding=Require`.
- **SKU validation: DEFERRED** (user decision) — no cloud instance provisioned; carried as open risk R1 into Phase 22.
- Consolidated all five findings + three SUMMARYs into `FINDINGS.md`: locked decisions, per-phase sizing (22–25), and 6 carried-forward risks.

## Files Created/Modified
- `.planning/phases/21-derisking-spikes/21-findings-hosting.md`
- `.planning/phases/21-derisking-spikes/FINDINGS.md`
- `.planning/phases/21-derisking-spikes/21-04-SUMMARY.md`
- `.planning/ROADMAP.md` — Phase 21 → Complete (4/4)

## Decisions Made
- **XML-shredding:** C#/j4jayant (app tier) — from 21-03.
- **Hosting target:** AWS RDS for PostgreSQL 17, fallback Azure Flexible Server — validation deferred.
- **MARS:** not load-bearing (0 refactor sites) — from 21-01.

## Issues Encountered
- Managed-SKU validation deferred (procurement/legal, not engineering) → recorded as risk R1, to validate early in Phase 22.
- Research sub-agent reported it couldn't read the prior analysis file (read glitch) and verified all hosting claims directly against live vendor pages instead — deliverable stands on primary sources.

## Next Step
**Phase 21 complete.** Ready to detail-plan Phase 22 (PostgreSQL Schema & Data-Access Foundation) from `FINDINGS.md`.
