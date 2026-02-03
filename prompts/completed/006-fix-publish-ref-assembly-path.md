<objective>
Diagnose and fix a ClickOnce publish error where MSBuild looks for the LabBilling Core ref assembly at the WRONG target framework path (`net8.0-windows7.0`) instead of the library's actual target framework (`net8.0`).

The error is:
```
Metadata file 'C:\Users\bpowers\source\repos\Lab-Patient-Accounting\LabBilling Library\obj\x64\Release\net8.0-windows7.0\ref\LabBilling Core.dll' could not be found
```

The obj folders have been completely cleared, so there are no stale artifacts — MSBuild is actively resolving the wrong path during publish. The fix must ensure the publish process references the correct `net8.0` ref assembly path for LabBilling Core.
</objective>

<context>
This is a .NET 8.0 WinForms medical billing application using ClickOnce deployment.

Read the CLAUDE.md file for project conventions and build commands.

Key facts:
- **LabBilling Core** (`LabBilling Library/LabBilling Core.csproj`) targets `net8.0` — NOT `net8.0-windows7.0`
- **Lab PA WinForms UI** (`Lab PA WinForms UI/Lab PA WinForms UI.csproj`) targets `net8.0-windows7.0` and references LabBilling Core via ProjectReference
- The ClickOnce publish profile is at `Lab PA WinForms UI/Properties/PublishProfiles/ClickOnceProfile.pubxml`
- The publish profile specifies `RuntimeIdentifier=win-x64`, `SelfContained=True`, `PublishReadyToRun=True`
- Both projects specify `<Platforms>x64</Platforms>`

The problem: When MSBuild publishes the WinForms UI with RuntimeIdentifier=win-x64, it appears to resolve all ProjectReference dependencies under the consuming project's TFM (`net8.0-windows7.0`) rather than each library's own TFM (`net8.0`). Since the obj folders are now clean, MSBuild cannot find the ref assembly at the incorrect `net8.0-windows7.0` path.

Key files to examine:
- @"LabBilling Library/LabBilling Core.csproj" — targets `net8.0`
- @"Lab PA WinForms UI/Lab PA WinForms UI.csproj" — targets `net8.0-windows7.0`
- @"Lab PA WinForms UI/Properties/PublishProfiles/ClickOnceProfile.pubxml" — ClickOnce publish profile
- @"Utilities/Utilities.csproj" — another dependency library, check its TFM
- @"j4jayant.HL7.Parser/j4jayant.HL7.Parser.csproj" — another dependency, check its TFM
- @"LabBilling Winforms Library/LabBilling Winforms Library.csproj" — check its TFM
- @"WinFormsLibrary/WinFormsLibrary.csproj" — check its TFM
- @"Lab Billing.sln" — solution file for project relationships
</context>

<diagnosis_approach>
Thoroughly investigate these potential root causes:

1. **Target framework mismatch during publish**: When MSBuild publishes with RuntimeIdentifier, it can force dependent projects to build under the consuming project's TFM. The LabBilling Core library targets `net8.0` but the consuming WinForms UI targets `net8.0-windows7.0`. MSBuild may be attempting to build/resolve LabBilling Core under `net8.0-windows7.0`, which it doesn't support.

2. **Review ALL project TFMs in the solution**: Check every .csproj file for their TargetFramework values. Determine if there's a pattern — some libraries may already target `net8.0-windows7.0` while others target `net8.0`. Understand which approach is correct for each.

3. **Consider whether LabBilling Core should target `net8.0-windows7.0`**: The library uses `System.Drawing.Common` and `NLog.WindowsEventLog` which are Windows-specific packages. If the library is ONLY used by Windows projects, changing its TFM to `net8.0-windows7.0` may be the correct fix and would align it with its consumers.

4. **Check if other non-Windows-TFM libraries have the same issue**: If Utilities or j4jayant.HL7.Parser also target plain `net8.0`, check whether MSBuild also fails to find their ref assemblies during publish.

5. **Publish profile configuration**: Review the ClickOnce profile for any settings that might force TFM resolution across all dependencies.

To validate hypotheses:
- First examine all .csproj files to understand the TFM landscape
- Then try a publish from the command line using MSBuild (NOT dotnet CLI — the project uses TextTemplating targets that require full MSBuild):
  ```
  "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Lab PA WinForms UI\Lab PA WinForms UI.csproj" -t:Publish -p:Configuration=Release -p:Platform=x64 -p:RuntimeIdentifier=win-x64 -p:SelfContained=true -p:PublishProfile=ClickOnceProfile
  ```
- If the TFM change approach is chosen, rebuild and test both normal build and publish
</diagnosis_approach>

<requirements>
1. Identify the exact root cause of why MSBuild resolves LabBilling Core's ref assembly under `net8.0-windows7.0` instead of `net8.0` during publish
2. Review ALL .csproj files in the solution to understand the TFM landscape and determine the best configuration
3. Apply the minimal, correct fix — likely one of:
   a. Change LabBilling Core's TargetFramework from `net8.0` to `net8.0-windows7.0` (if it's Windows-only and all consumers are Windows)
   b. Adjust the publish profile or project reference to prevent TFM coercion
   c. Some other MSBuild configuration that properly resolves cross-TFM project references during publish
4. Verify the fix by running both:
   - Normal build: `"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Lab Billing.sln" -p:Configuration=Release -p:Platform=x64`
   - Publish: `"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "Lab PA WinForms UI\Lab PA WinForms UI.csproj" -t:Publish -p:Configuration=Release -p:Platform=x64 -p:RuntimeIdentifier=win-x64 -p:SelfContained=true -p:PublishProfile=ClickOnceProfile`
</requirements>

<constraints>
- Do NOT remove or modify business logic, models, or service code
- Do NOT change the ClickOnce deployment strategy (it must remain ClickOnce)
- Do NOT remove any existing project references
- If changing a library's TFM, ensure all its package references are compatible
- The fix must work for BOTH Visual Studio publish (GUI) and command-line publish via MSBuild.exe
- Use MSBuild.exe (NOT dotnet CLI) for testing — the project uses TextTemplating targets that require full Visual Studio MSBuild
- Keep changes minimal and focused on the build/publish configuration
</constraints>

<verification>
Before declaring complete:
1. Confirm normal solution build succeeds with 0 errors
2. Confirm ClickOnce publish succeeds with 0 errors using MSBuild.exe command line
3. If any .csproj files were modified, verify they are valid XML
4. Verify no business logic was changed
</verification>

<success_criteria>
- The `Metadata file could not be found` error no longer appears during publish
- Normal solution build still succeeds
- The ClickOnce publish produces a valid deployment package
- All project TFMs are consistent and appropriate for their usage
- No unrelated code changes were made
</success_criteria>
