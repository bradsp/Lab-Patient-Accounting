<objective>
Remove the legacy WinForms presentation layer from the Visual Studio solution file `Lab Billing.sln` so the `billing-ui-to-blazor` branch can proceed as a Blazor-only solution (per `.planning/ROADMAP.md` and `LabOutreachUI/IMPLEMENTATION_PLAN.md`).

Specifically, remove these four projects — and ONLY these four — from the solution:
1. `Lab PA WinForms UI` (the WinForms desktop client being deprecated)
2. `LabBilling Winforms Library` (WinForms-specific; referenced only by the WinForms UI)
3. `WinFormsLibrary` (WinForms-specific; referenced only by the WinForms UI)
4. `DataGridViewGrouper` (external `..\..\#Utilities\...` control; referenced only by the WinForms UI, and currently a build blocker because the project file does not exist on this machine)

Why this matters: the WinForms UI references an external project (`#Utilities/DataGridViewGrouper`) that is absent from this environment, which hard-fails whole-solution restore (MSB3202). The migration deprecates WinForms in favor of `LabOutreachUI` (Blazor Server). Dropping these four projects from the solution unblocks the build and focuses the solution on the active Blazor surface.
</objective>

<context>
- Read `./CLAUDE.md` first for project conventions, the active WinForms→Blazor migration, and the project layout.
- The only file you may modify is `./Lab Billing.sln`.
- @Lab Billing.sln

Reference graph (already verified — confirm it yourself before acting, do not blindly trust it):
- `Lab PA WinForms UI` references: `LabBilling Core`, `Utilities`, `LabBilling Winforms Library`, `WinFormsLibrary`, `DataGridViewGrouper`.
- `LabBilling Winforms Library` and `WinFormsLibrary` are referenced by NOTHING except `Lab PA WinForms UI` → safe to drop from the solution.
- `DataGridViewGrouper` is referenced only by `Lab PA WinForms UI`.
- Shared projects that MUST remain in the solution: `LabBilling Core`, `Utilities`, `j4jayant.HL7.Parser`, `LabBillingService`, `LabBillingConsole`, `Lab Patient Accounting Job Scheduler`, `LabBillingCore.UnitTests`, `Lab PA Database`, `LabOutreachUI`, plus the `Libraries` and `Solution Items` solution folders.

Project GUIDs to remove (verify each against the current `.sln` before deleting its lines):
- `Lab PA WinForms UI` → `{ED4EDC70-4C69-4B69-94BB-9D0954730B69}`
- `DataGridViewGrouper` → `{DAD6DDA8-324C-54A8-F85E-24563407C529}`
- `WinFormsLibrary` → `{9FABCEDD-BF62-4769-A79E-72891CE3EF4A}`
- `LabBilling Winforms Library` → `{B25BC3DE-0F08-482F-BFC8-4E00A7FCBB02}`

Build tooling note (important — do not waste time on `dotnet build`): this solution CANNOT be built with `dotnet build`. `Utilities.csproj` has a `VBIDE` COM reference, and the .NET Core MSBuild engine fails with MSB4803 (`ResolveComReference` unsupported). You must build with Visual Studio's Framework MSBuild. Locate it with:
`& "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe"`
and invoke it with `/p:Platform=x64`.
</context>

<requirements>
This is a solution-file-only change. Per the explicit decision driving this task:
1. Edit ONLY `./Lab Billing.sln`. Do NOT modify any `.csproj` file. Do NOT delete, move, or `git rm` any project folder or source file from disk — the WinForms project directories stay on disk untouched; they are simply no longer part of the solution.
2. For each of the four projects above, remove from `Lab Billing.sln`:
   a. its `Project("{...}") = "<name>", "<path>", "{GUID}"` … `EndProject` block (including everything between, e.g. nested `ProjectSection` lines if present);
   b. every line in `GlobalSection(ProjectConfigurationPlatforms) = postSolution` keyed by that project's `{GUID}` (all Debug/Release × platform rows);
   c. any line in `GlobalSection(NestedProjects) = preSolution` that maps that project's `{GUID}` to a solution folder.
3. Leave all other projects, the `SolutionConfigurationPlatforms` section, the `Libraries`/`Solution Items` solution folders, and the solution GUID intact.
4. Preserve the file's existing encoding/formatting (UTF-8 BOM, CRLF line endings, tabs/indentation). Make surgical line removals only — do not reflow or reorder unrelated content.
</requirements>

<implementation>
- Work from the GUIDs, not just the display names — `ProjectConfigurationPlatforms` and `NestedProjects` rows reference projects by GUID only, and missing one leaves a dangling reference that Visual Studio flags. Search the `.sln` for each GUID and account for every occurrence.
- Do NOT touch the `.csproj` `<ProjectReference>` entries (e.g. `Lab PA WinForms UI.csproj` still referencing `DataGridViewGrouper`). Leaving them is intentional and correct here: once the projects are out of the solution, the solution build never restores them, so the dangling external reference no longer blocks the build. Editing `.csproj` files is out of scope for this task and was explicitly excluded.
- Avoid `dotnet sln remove` as your editing mechanism unless you confirm it runs without the COM-reference/missing-project errors; a direct, surgical text edit of the `.sln` is the reliable path. If you do use `dotnet sln`, verify the resulting file afterward against requirement #2 (it must also clean up the config/nested sections).
</implementation>

<output>
Modify exactly one file:
- `./Lab Billing.sln` — four `Project…EndProject` blocks removed, plus their `ProjectConfigurationPlatforms` and `NestedProjects` GUID rows.

No other files created or modified.
</output>

<verification>
Before declaring complete, verify all of the following and fix anything that fails:
- [ ] `git diff --stat` shows `Lab Billing.sln` as the ONLY changed file (no `.csproj`, no source, no deletions of files on disk).
- [ ] None of the four removed GUIDs (`ED4EDC70…`, `DAD6DDA8…`, `9FABCEDD…`, `B25BC3DE…`) appears anywhere in `Lab Billing.sln` afterward (grep to confirm zero matches each).
- [ ] All nine keeper projects plus the two solution folders still have their `Project…EndProject` blocks and config rows intact.
- [ ] The previously-failing whole-solution restore now succeeds — the MSB3202 "DataGridViewGrouper.csproj was not found" error is gone. Run: `& "<MSBuild.exe path from vswhere>" "Lab Billing.sln" /p:Platform=x64 /t:Restore /v:m`
- [ ] The full solution BUILDS with VS MSBuild: `& "<MSBuild.exe path>" "Lab Billing.sln" /p:Platform=x64 /t:Build /v:m` exits 0 (warnings are acceptable; zero errors). This is now expected to succeed end-to-end since the only blocker was the missing external project.
- [ ] `LabOutreachUI` (the Blazor app) is among the projects that build.
</verification>

<success_criteria>
- `Lab Billing.sln` contains the nine keeper projects + two solution folders and none of the four removed WinForms projects (no dangling GUID references in any section).
- The whole solution restores and builds cleanly with VS MSBuild (no MSB3202, zero build errors).
- No `.csproj` files were edited and no files were deleted from disk; `git diff` is limited to `Lab Billing.sln`.
- The branch is ready to continue Blazor-only UI development per the plans.
</success_criteria>

<!--
Completed: 2026-06-16
Executed via: /taches-cc-resources:run-prompt 002
Result: Removed 4 WinForms projects (Lab PA WinForms UI, LabBilling Winforms Library, WinFormsLibrary, DataGridViewGrouper) from Lab Billing.sln (43 lines). Solution-file-only change; no .csproj edited, no disk files deleted. Full solution now builds clean with VS MSBuild (exit 0, zero errors); LabOutreachUI builds.
-->
