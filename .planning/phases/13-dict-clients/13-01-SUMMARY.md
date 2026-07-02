# Phase 13 Plan 01: Dictionary Authorization + Navigation Summary

**Added an enforceable `EditDictionary` authorization policy and a permission-gated "Dictionary" nav section with a Clients link — the entry point for Client dictionary maintenance.**

## Accomplishments
- Added `EditDictionaryRequirement` and `EditDictionaryAuthorizationHandler`, mirroring the existing Random Drug Screen handler: access granted when the user is DB-validated AND (is an administrator OR carries the `Permission=EditDictionary` claim).
- Registered the `EditDictionary` policy in the `AddAuthorization` block and its handler in DI (Scoped, matching the other auth handlers).
- Added a "DICTIONARY" nav section to `NavMenu.razor` with a "Clients" link (`dictionary/clients`), wrapped in `<AuthorizeView Policy="EditDictionary">` so it only renders for authorized users.
- Confirmed via human verification: section visible to CanEditDictionary users/admins, hidden otherwise.

## Files Created/Modified
- `LabOutreachUI/Authorization/DatabaseUserAuthorizationHandler.cs` — added `EditDictionaryRequirement` + `EditDictionaryAuthorizationHandler`.
- `LabOutreachUI/Program.cs` — registered `EditDictionary` policy + handler.
- `LabOutreachUI/Shared/NavMenu.razor` — added gated DICTIONARY nav section with Clients link.

## Decisions Made
- Consumed the existing `Permission=EditDictionary` claim (already emitted by `WindowsAuthenticationMiddleware` from `emp.access_edit_dictionary`) rather than adding new claims or DB/middleware changes. No Core/DB/middleware changes were needed.
- Gated the nav via the policy (`<AuthorizeView Policy="EditDictionary">`) rather than a hand-rolled `HasClaim` check, for consistency with the RDS section and to reuse the admin-override logic.

## Issues Encountered
- **Build tool (Rule 3 — blocker, auto-resolved):** The plan's verify step used `dotnet build`, which fails on this solution because `Utilities.csproj` contains a COM reference (`ResolveComReference`) that only the .NET Framework MSBuild can process (error MSB4803). Resolved by building with Visual Studio's MSBuild: `C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe "LabOutreachUI/LabOutreachUI.csproj" -p:Configuration=Debug -p:Platform=x64`. Build succeeded (only pre-existing warnings). **Remaining plans (13-02..13-04) should verify with VS MSBuild, not `dotnet build`.**

## Next Step
Ready for 13-02-PLAN.md (Client list/search page).
