# Phase 13 Plan 04: Preferences/MRO/Comments + New Client Summary

**Client dictionary maintenance is feature-complete (minus the deferred Discounts + Interface Mappings grids): the edit form now covers preferences/MRO/comments and new clients can be created with duplicate-mnemonic protection.**

## Accomplishments
- Added three sections to `ClientEdit.razor`: **Preferences** (Print in Date Order, Include on Charge Code Report, Bill at Discount, Do NOT Bill, Print CPT on Invoice, plus Default Discount % via `InputNumber<double>`), **Medical Review Officer** (name, address 1/2, city, state, zip), and **Comments** (`InputTextArea`). All bind into the same `client` model and persist through the existing save path.
- Implemented the new-client flow: for the `new` route the mnemonic is editable; an `@onblur` check calls `GetClient` and surfaces an "already exists — edit that client instead" warning with a jump link; `SaveAsync` also re-checks and blocks inserting over an existing mnemonic.
- Verified by human: preferences/MRO/comment load, edit, and persist; duplicate mnemonic is detected on new; a brand-new client can be created and re-opened.

## Files Created/Modified
- `LabOutreachUI/Pages/Dictionary/ClientEdit.razor` — added Preferences/MRO/Comments sections and the new-client duplicate-mnemonic flow.

## Decisions Made
- Reused the existing `EditForm`/`SaveAsync`/validation from 13-03 rather than a separate add form — the same page serves both edit and new via the `{Mnem}` route (`new` sentinel).
- Skipped the legacy non-persisted "Collection Site on Chain of Custody" checkbox (it maps to no column).

## Issues Encountered
- **Full-solution build (environmental, out of scope — not fixed):** `MSBuild "Lab Billing.sln"` fails in the legacy `Lab PA WinForms UI.csproj` with MSB4226 — it imports `Microsoft.TextTemplating.targets` for VS v17.0, absent from the VS 18 install. This is pre-existing and unrelated to Phase 13 (only LabOutreachUI files were changed). The phase deliverable — `LabOutreachUI` and its dependency chain (LabBilling Core, Utilities) — builds cleanly via VS MSBuild. See [[build-requires-vs-msbuild]] for the build-tool context.

## Next Step
**Phase 13 complete** (13-01..13-04). Deferred: Discounts + Interface Mappings grids — see `.planning/ISSUES.md` ISS-001 (a future 13-05). Ready to plan the next phase, or ship this branch.
