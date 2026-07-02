# Phase 13 Plan 03: Client Edit — Core Fields Summary

**Added a persisting Client edit page at `/dictionary/clients/{mnem}` with the core scalar fields, reference-data dropdowns, required-field validation, save, and Active→soft-delete.**

## Accomplishments
- Created `Pages/Dictionary/ClientEdit.razor`, policy-gated with `[Authorize(Policy = "EditDictionary")]`, routed by `{Mnem}` (handles both an existing mnemonic and the literal `new`).
- Loads an existing client via `DictionaryService.GetClient`; for `new`, initializes a blank `Client` with `Type = -1` so the required client-type selection is meaningful.
- Core fields grouped in Identity / Address / Contact / Business Classification cards inside an `EditForm`.
- Dropdowns: State, County, Client Type, Fee Schedule, EMR Type from `Dictionaries`; Cost Center from `DictionaryService.GetGLCodes` (binds `GLCode.level_1`, matching the legacy form); Bill Method from the fixed list INVOICE/PATIENT/PER ACCOUNT.
- Client Type binds directly as `InputSelect<int>` to `Client.Type` — no manual string↔int conversion needed.
- Required-field validation (ClientMnem, Client Type ≥ 0, Fee Schedule, Cost Center, Bill Method) blocks save with inline per-field messages; ClientMnem is read-only when editing an existing client.
- "Active" checkbox bound to the inverse of `IsDeleted` (soft delete); save via `DictionaryService.SaveClient`, then navigate back to the list.
- Verified by human: existing-client edit loads/persists, required-field validation blocks empty saves, Active toggle soft-deletes.

## Files Created/Modified
- `LabOutreachUI/Pages/Dictionary/ClientEdit.razor` — new client edit page.

## Decisions Made
- Used manual validation (a `fieldErrors` dictionary + inline messages) rather than adding DataAnnotations to the shared Core `Client` model, keeping Core untouched.
- Cost Center dropdown displays `level_1 - description` for readability but stores `level_1` as the value (the column the legacy form persisted).
- New-client `Type = -1` sentinel makes "client type required" enforceable, since `0` is a valid existing type ("Affiliate Hospital").

## Issues Encountered
- **Razor build error (Rule 1 — auto-fixed):** Blazor input components (`InputText`/`InputSelect`) reject `class` attributes that mix literal text with a `@expression` (error RZ9986). Fixed by switching the five conditional-class attributes to a single interpolated expression (`class="@($"form-control ... {Invalid("X")}")"`). Build clean afterward (VS MSBuild).

## Next Step
Ready for 13-04-PLAN.md (preferences/MRO/comments sections + new-client flow with duplicate-mnem check).
