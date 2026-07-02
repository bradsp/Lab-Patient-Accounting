<objective>
Diagnose and fix two compilation errors that occur ONLY when publishing (ClickOnce) the Lab PA WinForms UI project, but NOT during normal builds. The errors are:

- `'UserAccount' does not contain a definition for 'CanAccessRandomDrugScreen'` at line 86 and line 251 of `UserSecurity.cs`

The property `CanAccessRandomDrugScreen` definitely exists in the `UserAccount` model class unconditionally (no conditional compilation). The fact that it builds fine but fails on publish points to a build configuration or platform mismatch during the ClickOnce publish process.
</objective>

<context>
This is a .NET 8.0 WinForms medical billing application using ClickOnce deployment.

Key files to examine:
- @"Lab PA WinForms UI/Forms/UserSecurity.cs" - the file with the errors (lines 86 and 251 reference `CanAccessRandomDrugScreen`)
- @"LabBilling Library/Models/UserAccount.cs" - the model class that DOES contain the property (line 35-36)
- @"Lab PA WinForms UI/Lab PA WinForms UI.csproj" - WinForms project targeting `net8.0-windows7.0` with `<Platforms>x64</Platforms>`
- @"LabBilling Library/LabBilling Core.csproj" - library project targeting `net8.0` with `<Platforms>x64</Platforms>`
- @"Lab PA WinForms UI/Properties/PublishProfiles/ClickOnceProfile.pubxml" - publish profile specifying `<Platform>Any CPU</Platform>`

Read the CLAUDE.md file for project conventions and build commands.
</context>

<diagnosis_approach>
Thoroughly investigate these potential root causes:

1. **Platform mismatch during publish**: The ClickOnce publish profile (`ClickOnceProfile.pubxml`) specifies `<Platform>Any CPU</Platform>`, but the `.csproj` files only define `<Platforms>x64</Platforms>`. When MSBuild resolves the publish, it may use `AnyCPU` configuration which has no matching `PropertyGroup` in the library project, potentially causing assembly resolution issues where the LabBilling Core library is not properly built or referenced.

2. **Target framework mismatch**: The WinForms UI targets `net8.0-windows7.0` while the LabBilling Library targets `net8.0`. This is normally fine, but during publish with ReadyToRun (`<PublishReadyToRun>True</PublishReadyToRun>`) and self-contained deployment (`<SelfContained>True</SelfContained>`), framework resolution may differ.

3. **Stale intermediate build artifacts**: The publish process may be using cached/stale build outputs where an older version of the LabBilling Core assembly (before `CanAccessRandomDrugScreen` was added) is being referenced.

4. **RuntimeIdentifier impact**: The publish profile specifies `<RuntimeIdentifier>win-x64</RuntimeIdentifier>` which changes how dependencies are resolved compared to a normal build. Combined with `<Platform>Any CPU</Platform>`, this can create conflicts.

To validate hypotheses, try:
- Run `dotnet publish "Lab PA WinForms UI/Lab PA WinForms UI.csproj" -c Release` from the command line and check if the same error occurs
- Run `dotnet clean "Lab Billing.sln"` followed by the publish to rule out stale artifacts
- Check if changing the publish profile's `<Platform>` from `Any CPU` to `x64` resolves the issue
</diagnosis_approach>

<requirements>
1. Identify the exact root cause of why `CanAccessRandomDrugScreen` is not found during publish but works during build
2. Apply the minimal fix needed — do NOT refactor unrelated code, do NOT add features, do NOT modify the UserAccount model
3. Verify the fix by attempting a publish from the command line using:
   ```
   dotnet publish "Lab PA WinForms UI/Lab PA WinForms UI.csproj" -c Release -r win-x64 --self-contained
   ```
4. Ensure normal builds still work after the fix:
   ```
   dotnet build "Lab Billing.sln" -c Release -p:Platform=x64
   ```
</requirements>

<constraints>
- Do NOT modify the `UserAccount` model — the property exists and is correct
- Do NOT remove or rename `CanAccessRandomDrugScreen` usage from `UserSecurity.cs` unless the property truly needs to be accessed differently
- Do NOT change the ClickOnce deployment strategy (it must remain ClickOnce)
- Keep the fix scoped to project/publish configuration — avoid changing business logic
- The solution must work for both Visual Studio publish (GUI) and command-line publish
</constraints>

<verification>
Before declaring complete:
1. Confirm `dotnet build "Lab Billing.sln"` succeeds
2. Confirm `dotnet publish "Lab PA WinForms UI/Lab PA WinForms UI.csproj" -c Release -r win-x64 --self-contained` succeeds without the `CanAccessRandomDrugScreen` errors
3. If the fix involved changing the publish profile, verify the ClickOnce profile is still valid XML
</verification>

<success_criteria>
- The two `CanAccessRandomDrugScreen` errors no longer appear during publish
- Normal solution build still succeeds
- The ClickOnce publish profile produces a valid deployment package
- No unrelated code changes were made
</success_criteria>
