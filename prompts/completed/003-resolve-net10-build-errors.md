<objective>
All projects in `Lab Billing.sln` were just retargeted from .NET 8 to **.NET 10** (`net10.0` / `net10.0-windows7.0`). That retarget surfaced build errors. Review and resolve every build error so the whole solution compiles cleanly again, and — as part of the fix — upgrade all NuGet packages to their latest stable versions.

Why this matters: the `billing-ui-to-blazor` branch needs a green build on .NET 10 to continue the Blazor (`LabOutreachUI`) migration. A retarget like this typically breaks the build in two ways — (1) framework-bound packages (ASP.NET Core, EF, test SDKs) whose major version must match the .NET 10 runtime, and (2) code that used APIs changed/removed between .NET 8 and .NET 10. Both must be cleared to zero errors.

End state: `Lab Billing.sln` builds with **zero errors** on .NET 10, all NuGet packages at latest stable, no behavior changes beyond what the upgrade requires.
</objective>

<context>
- Read `./CLAUDE.md` first for project layout, the active WinForms→Blazor migration, and conventions.
- The installed SDK is .NET 10 GA (`dotnet --version` ≈ `10.0.204`). Target **stable** packages only — this is a HIPAA-regulated billing system, so do NOT introduce `-preview`/`-rc`/`-beta` package versions.

Build tooling — CRITICAL, do not fight this:
- This solution **cannot** be built with `dotnet build`. `Utilities/Utilities.csproj` has a `VBIDE` COM reference and the .NET Core MSBuild engine fails with MSB4803 (`ResolveComReference` unsupported). That is NOT a real error to "fix" — it is a tooling limitation.
- Build and get the authoritative error list with **Visual Studio's Framework MSBuild**. Locate it:
  `& "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe"`
  (Known-good from prior sessions: `C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe` — verify it exists.) Invoke with `/p:Platform=x64`. Use the PowerShell tool for MSBuild.
- `dotnet restore` and `dotnet list package --outdated` DO work per-project (restore is unaffected by the COM ref) and are useful for discovering current/latest package versions. Use them for discovery, but treat **VS MSBuild as the source of truth** for whether the build passes.

Scope — the in-solution .NET projects now on .NET 10 (these are the ONLY projects to touch):
1. `LabBilling Library/LabBilling Core.csproj` (`net10.0`)
2. `Utilities/Utilities.csproj` (`net10.0`)
3. `j4jayant.HL7.Parser/j4jayant.HL7.Parser.csproj` (`net10.0`)
4. `LabOutreachUI/LabOutreachUI.csproj` (`net10.0`, Blazor Server)
5. `LabBillingService/LabBillingService.csproj` (`net10.0-windows7.0`)
6. `LabBillingConsole/LabBillingConsole.csproj` (`net10.0-windows7.0`)
7. `Lab Patient Accounting Job Scheduler/Lab Patient Accounting Job Scheduler.csproj` (`net10.0-windows7.0`)
8. `LabBillingCore.UnitTests/LabBillingCore.UnitTests.csproj` (`net10.0-windows7.0`)

Out of scope — DO NOT touch:
- The WinForms projects (`Lab PA WinForms UI`, `LabBilling Winforms Library`, `WinFormsLibrary`) and any other `.csproj` still on `net8.0` — they were intentionally removed from the solution in a prior step and remain on disk. They are NOT part of this build. Do not re-add them, retarget them, or upgrade their packages.
- `Lab PA Database/*.sqlproj` (SQL database project — no TFM/NuGet concern).
- The `TargetFramework` values themselves — the retarget to .NET 10 is already done and is correct. Do not change TFMs.
</context>

<requirements>
1. Establish the baseline: build the full solution with VS MSBuild and capture the complete, deduplicated list of build errors (group by project and by root cause). For maximum efficiency, when running independent discovery steps (e.g. `dotnet list package --outdated` across several projects, multiple greps), batch them in parallel.
2. **Upgrade all NuGet packages to latest stable** across the 8 in-solution projects (per the chosen dependency policy):
   - Align framework-bound Microsoft packages to the **10.x** line to match the .NET 10 runtime: `Microsoft.AspNetCore.*`, `Microsoft.Extensions.*`, `Microsoft.EntityFrameworkCore.*` (if present), and the test stack (`Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `coverlet.*`).
   - Upgrade all other third-party packages to their **latest stable** version (e.g. PetaPoco.Compiled, Microsoft.Data.SqlClient, Autofac, FluentValidation, NLog/log4net, Quartz, Topshelf, PDFsharp/MigraDoc, NPOI/ClosedXML, SSH.NET, Newtonsoft.Json, EdiTools, etc. — whatever each project actually references).
   - Edit the `<PackageReference Version="…">` values in-place in each `.csproj`. Do NOT introduce Central Package Management or restructure the projects.
3. Fix all code-level build errors caused by the .NET 10 retarget and the package upgrades — removed/renamed/changed APIs, altered signatures, ambiguous overloads, etc. Make the **minimal behavior-preserving** edit at each call site. Do NOT pin packages back to old versions to dodge an API break — the decision is upgrade-to-latest, so adapt the code instead.
4. **Errors only.** Get to zero build errors; do NOT chase warnings. Leave new analyzer/nullable/style warnings and obsolete-API deprecation warnings in place, but record the significant ones (especially obsoletions that will become errors later) in your final summary so they can be triaged separately.
5. Keep the change surface limited to the 8 in-solution projects' `.csproj` files (package versions) and their source files where an error requires a code fix.
</requirements>

<implementation>
- Iterate: build with VS MSBuild → read errors → fix a root cause (package bump or code change) → rebuild. After each build, carefully reflect on the new error set before the next change; package upgrades often resolve or reshape multiple errors at once, so re-baseline rather than blindly working an outdated list.
- Latest **stable** only. If a package's latest stable still has no `net10`-compatible build, upgrade to the newest stable that restores/builds and note it in the summary (do not use a prerelease to force it).
- Prefer fixing the root cause once (e.g. a changed API in a shared helper) over patching many call sites.
- Benign/pre-existing diagnostics you can ignore (they are warnings, not errors): `NU1902`/`NU1903` advisory warnings, and `MSB3270` processor-architecture-mismatch warnings. Do not spend effort suppressing them.
- Do not alter `appsettings.json`, connection strings, NLog config, or runtime behavior. This task is "make .NET 10 compile," not a feature or config change.
- Watch specifically for: ASP.NET Core 10 minimal-hosting/Blazor API changes in `LabOutreachUI`; test-SDK/xunit major bumps in `LabBillingCore.UnitTests`; and any analyzer that became an error by default in .NET 10.
</implementation>

<output>
Modify only within the 8 in-solution projects:
- Their `.csproj` files — updated `<PackageReference>` versions.
- Source files (`.cs`/`.razor`) where a .NET 10 / package break requires a code fix.

No new projects, no TFM edits, no changes to out-of-scope (WinForms / sqlproj) files.
</output>

<verification>
Before declaring complete, verify all of the following and fix anything that fails:
- [ ] Full-solution restore + build with VS MSBuild succeeds with **zero errors**:
      `& "<MSBuild.exe path>" "Lab Billing.sln" /p:Platform=x64 /t:Restore`
      `& "<MSBuild.exe path>" "Lab Billing.sln" /p:Platform=x64 /t:Build /v:m`  → exit 0, 0 Error(s)
- [ ] `LabOutreachUI` (Blazor) is among the projects that build.
- [ ] No prerelease package versions were introduced: grep the 8 `.csproj` files for `Version=` and confirm no `-preview`/`-rc`/`-beta`/`-alpha` values.
- [ ] `git diff --name-only` is limited to files under the 8 in-solution projects (their `.csproj` + touched source). No WinForms project files, no `.sqlproj`, no TFM changes, no out-of-scope files.
- [ ] (Best effort) Build the unit test project and, if a VS `vstest.console.exe` is available, run `LabBillingCore.UnitTests`; report pass/fail. If tests can't be run in this environment (e.g. require a database), say so — a clean build is the hard requirement, running tests is best-effort.
</verification>

<success_criteria>
- `Lab Billing.sln` builds on .NET 10 with zero errors via VS MSBuild.
- All NuGet packages across the 8 in-solution projects are at their latest stable versions, with framework-bound packages on the 10.x line.
- Only error-level issues were fixed; warnings were left intact and the significant new ones are listed in the summary.
- The diff is limited to package-version bumps and the code edits required to compile; no behavior, config, or out-of-scope changes.
- The branch is ready to continue Blazor migration on .NET 10.
</success_criteria>

<!--
Completed: 2026-06-16
Executed via: /taches-cc-resources:run-prompt 003
Result: Solution builds on .NET 10 with 0 errors (VS MSBuild). Fixed C# 14 `field` contextual-keyword breaks in j4jayant.HL7.Parser (Segment.cs, Field.cs); reverted SixLabors Fonts 3.0.0->2.1.3 and ImageSharp 4.0.0->3.1.11 (3.x/4.x enforce commercial license and fail build); added Microsoft.NET.Test.Sdk 18.6.0 to unit tests. Packages were already at latest stable from the user retarget. Warnings left for triage (NPOI OSMF EULA, nullable CS86xx, CS0114/CS8981).
-->
