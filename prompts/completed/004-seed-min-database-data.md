<objective>
Generate an idempotent T-SQL seed script that populates the **minimum data the application needs to actually run** — the system-parameters table (`system`) and the users table (`emp`) — derived from what the code actually reads. Without this, the app connects but is unusable: `system` has 0 rows (so `ApplicationParameters` is all defaults) and `emp` has 0 rows (so the `DatabaseUser` authorization policy blocks every page).

Why this matters: the local dev database (`LabBillingTest` on the dockerized SQL Server) has the full schema (163 tables) but no seed data. The immediate consumer is plan **21-02** (the runtime DB-object Extended Events trace), which requires logging in and exercising real Blazor/service/scheduler workflows — impossible with no users and no parameters. This seed makes the app login-able and functional for that work.

End state: a single re-runnable `.sql` script that, applied to a schema-complete billing database, yields a working login (for both the current Windows dev user and a generic admin) and a fully-populated `system` parameter set with safe defaults.
</objective>

<context>
- Read `./CLAUDE.md` first for project conventions and the dual `UnitOfWorkMain` / `UnitOfWorkSystem` data layer.
- This is a HIPAA-billing app; the seed is for a **local dev/test database only**. Use synthetic, non-PHI values. Do not put real patient data in the script.

Local DB environment (verified working):
- Dockerized SQL Server 2022, container `mssql_server`, `localhost:1433`. Target database: **`LabBillingTest`**. App login: SQL login `sysapp` (password is in `LabOutreachUI/appsettings.json` working tree — `AppSettings:DatabasePassword`). `sysapp` is `db_owner` on `LabBillingTest`.
- Run sqlcmd inside the container via the **PowerShell tool** (Git Bash mangles `/opt/...` paths):
  `docker exec mssql_server /opt/mssql-tools18/bin/sqlcmd -S localhost -U sysapp -P '<password>' -d LabBillingTest -C -Q "..."`

The two target tables and how the app uses them:
- **`system`** — PetaPoco model `SysParameter` (`LabBilling Library/Models/SysParameter.cs`, `[TableName("system")]`). It's a key/value parameter store. On first request the app calls `SystemService.LoadSystemParameters()` (via `UnitOfWorkSystem` → `SystemParametersRepository`) which materializes the `ApplicationParameters` object. `ApplicationParameters` is a large model split across partial classes: `LabBilling Library/Models/ApplicationParameters-*.cs` (Accounting, Billing, Charging, etc.). **Every property there is loaded from a `system` row** keyed by some parameter name.
- **`emp`** — PetaPoco model `UserAccount` (`LabBilling Library/Models/UserAccount.cs`, `[TableName("emp")]`). The auth/authorization path: `LabOutreachUI/Authentication/DevelopmentAuthenticationHandler.cs` resolves the dev user from `AppSettings:DevelopmentUser`, falling back to `Environment.UserName` (currently `brads`); then the `DatabaseUser` policy (`LabOutreachUI/Authorization/DatabaseUserAuthorizationHandler.cs`) and `CustomAuthenticationStateProvider`/`UserAccountRepository.GetByUsername` validate that username against `emp`.
</context>

<research>
Thoroughly analyze the code to derive exactly what must be seeded — do not guess at parameter names or columns. For maximum efficiency, batch independent reads/greps in parallel.

1. **System parameters:** Enumerate the COMPLETE set of parameter keys the app reads.
   - Read every `LabBilling Library/Models/ApplicationParameters-*.cs` partial and the `SysParameter` model.
   - Read `SystemService.LoadSystemParameters()` and `SystemParametersRepository` to learn exactly how a property maps to a `system` row (the key column name, how the value is read, the data type, and any key-name source — attribute, property name, or an explicit string key).
   - Produce the full list of parameter keys + the .NET type each is parsed into, so you can assign a type-safe safe default to every one (e.g. bool→0/false, int→0, decimal→0, string→'' or a sensible literal, date→a valid date, fin-code/path-like keys→reasonable dev defaults).
2. **Users:** Determine what a *valid, authorizable* user row requires.
   - Read `UserAccount` model for columns, and query the LIVE `emp` table schema (`INFORMATION_SCHEMA.COLUMNS` for `emp`) to get every NOT NULL column, its type, and any defaults — the INSERT must satisfy all NOT NULL constraints.
   - Trace `DatabaseUserAuthorizationHandler`, `CustomAuthenticationStateProvider`, `UserAccountRepository.GetByUsername`, and `AuthenticationService` to learn what makes a user pass: the username match, an active/enabled flag, and whether authorization checks a permission level / role / claim. Note whether a password (and what hash) is required, or whether dev/Windows auth bypasses password entirely.
   - If a user genuinely requires a supporting row in another table to authenticate or pass the `DatabaseUser` policy (e.g. a permissions/role/profile row), identify that table+row and include it — that is part of "minimum to run." Otherwise stay focused on `system` + `emp` only.
3. Also inspect the live `system` table schema (`INFORMATION_SCHEMA.COLUMNS` for `system`) so the inserts match the real columns/nullability, not just the model.

After each discovery step, reflect: is the parameter/column list now complete and type-correct? Adjust before writing the script.
</research>

<requirements>
1. Produce ONE idempotent T-SQL seed script (SQL Server dialect). Re-running it must be safe and must not create duplicates — use `IF NOT EXISTS (...) INSERT ...` per row, or `MERGE`, keyed on the natural key (parameter key for `system`, username for `emp`). Do not `TRUNCATE`/`DELETE` existing data.
2. **`system` table:** seed a row for **every parameter key the app reads** (from the research), each with a type-appropriate safe default. Group/comment the inserts by the `ApplicationParameters-*` partial they come from so the script is auditable. Where a parameter clearly needs a meaningful dev value (e.g. a default fin-code, a temp path, a feature flag), use a sensible one and add an inline comment; otherwise use the type's neutral default.
3. **`emp` table:** seed **two** active admin-capable users:
   - `brads` — matches the current Windows dev user (`Environment.UserName`) so dev-auth login works with no config change.
   - a generic admin (e.g. `admin`) — portable across machines.
   Both must satisfy every NOT NULL column and be granted the **highest available access/permission level** so they can reach all modules (needed to exercise workflows for the 21-02 trace). If the schema requires a password hash, use the app's own hashing scheme/algorithm (trace `AuthenticationService`); if dev/Windows auth bypasses password, set a valid placeholder and note it. Include any directly-required supporting rows found in research.
4. Make the script self-documenting: a header comment stating purpose, target (a schema-complete billing DB such as `LabBillingTest`), that it is idempotent/re-runnable and dev-only (synthetic data), and how to run it (sqlcmd against the container). Do NOT hardcode credentials in the script.
5. Keep it focused on `system` + `emp` (+ any strictly-required supporting row). Do not seed unrelated business tables.
</requirements>

<implementation>
- Derive columns and NOT NULL constraints from the LIVE table schema, not just the model — the model may omit columns or computed/defaulted columns, and a missing NOT NULL column will make the INSERT fail. WHY: the script must run clean on the first try against the real schema.
- Prefer the natural key for idempotency (`system` parameter-key column; `emp` username column). WHY: lets the script be re-run after partial application without duplicate-key errors.
- Use type-safe literals matching each parameter's parsed .NET type. WHY: `LoadSystemParameters` parses values; a bad literal (e.g. non-numeric string into an int param) will throw at startup and the app falls back to defaults or errors.
- Do not alter `appsettings.json`, the schema, or any app code — this task only produces a data script. Do not commit secrets.
- Run all DB checks via `docker exec mssql_server /opt/mssql-tools18/bin/sqlcmd ...` through the PowerShell tool.
</implementation>

<output>
Create one file (relative path):
- `./Database/Seed/seed-min-data.sql` — the idempotent seed script for `system` + `emp` (+ any strictly-required supporting row). Create the `Database/Seed/` folder if needed.

No app-code, schema, or config changes.
</output>

<verification>
Before declaring complete, verify against the live container:
- [ ] Apply the script once: `docker exec ... sqlcmd -U sysapp -P '<pw>' -d LabBillingTest -C -i <script>` (or `-Q` with the contents) → completes with no errors.
- [ ] Re-apply the SAME script a second time → completes with no errors and creates **no** duplicate rows (idempotency proven).
- [ ] `SELECT COUNT(*) FROM [system]` > 0 and equals the number of parameter keys you seeded; spot-check 3-4 keys return type-valid values.
- [ ] `SELECT COUNT(*) FROM emp WHERE <username-col> IN ('brads','admin')` = 2, and each row is active/enabled with the admin access level set.
- [ ] Confirm the seeded parameter keys cover every property across the `ApplicationParameters-*.cs` partials (no missing keys) — list any intentionally omitted with a reason.
- [ ] (Best effort) Confirm the values are shaped such that `SystemService.LoadSystemParameters()` would parse them without throwing (type alignment check).
</verification>

<success_criteria>
- `./Database/Seed/seed-min-data.sql` exists, is idempotent, dev-only, and uses only synthetic data.
- Running it against a schema-complete billing DB yields: every app-read `system` parameter populated with a type-safe default, and two active admin users (`brads` + `admin`) that satisfy all NOT NULL constraints and pass the `DatabaseUser` authorization policy.
- The local `LabBillingTest` database is left seeded and ready, so the app can log in and be exercised for plan 21-02.
- No app code, schema, config, or secrets were changed/committed.
</success_criteria>

<!--
Completed: 2026-06-16
Executed via: /taches-cc-resources:run-prompt 004
Result: Created Database/Seed/seed-min-data.sql (idempotent). Seeds 188 system parameters (all ApplicationParameters-*.cs properties, type-safe defaults, keyed on KeyName) + 2 admin users (brads, admin; access=ENTER/EDIT, reserve4=1 admin, password Password1 hashed via app SHA256 scheme). Validated against LabBillingTest container: apply-once + apply-twice idempotent, 188 params / 2 users. Note: UserAccount.CanAccessRandomDrugScreen maps to emp.access_random_drug_screen which is absent from the live schema - possible login mapping issue (schema/model mismatch, not the seed).
-->
