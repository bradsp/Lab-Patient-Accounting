# Phase 21 Finding: Repository T-SQL Porting Backlog

> Static, read-only inventory of hand-written T-SQL embedded in `LabBilling Library/Repositories/**/*.cs`.
> Replaces the analysis's "~40–50% of repositories" estimate (§2.2 / §6 of
> `analyses/dbms-modernization-and-redis-hotspots.md`) with a counted, file:line-cited backlog.
> Branch: `billing-ui-to-blazor`. Method: ripgrep per marker + Read-verification of every cited site.
> **No source was modified** — this is inventory only.

---

## 1. Scope & method

- **Universe:** 67 `.cs` files under `LabBilling Library/Repositories/` (incl. interfaces, base classes,
  the mapper, and `AppEnvironment.cs`). Of these, 51 contain a `Sql.Builder` chain, raw SQL string,
  or `ExecuteScalar`/`ExecuteNonQuery`/`Query`/`Fetch` call.
- **Markers scanned** (the constructs Npgsql/PostgreSQL reject or that require rewriting):
  bracket identifiers `[x]` inside SQL strings; ` TOP `; `dbo.`; `OUTPUT`; `EXEC` /
  `ExecuteNonQueryProc`; `NOLOCK`; `OFFSET … FETCH`; single-quoted column aliases
  (`as 'X'`); and T-SQL scalar functions `LEFT(`/`RIGHT(`/`ISNULL(`/`GETDATE(`/`LEN(`/
  `DATEADD(`/`DATEDIFF(`/`nullif(`.
- **False-positive filtering:** C# `Substring()`, attribute brackets (`[Obsolete]`), `SqlParameter`
  type usage, and `Marshal*` were excluded. `dbo.` appearing only inside a `/* … */` comment block
  was excluded from the live-port list (see Issues).

### Marker hit summary (live sites only)

| Marker | Live sites | Files |
|---|---|---|
| ` TOP ` (→ `LIMIT`) | 1 | UserProfileRepository |
| `EXEC … OUTPUT` proc call | 1 | NumberRepository |
| `ExecuteNonQueryProc` (proc call) | 1 | MessagesInboundRepository |
| inline scalar-UDF `dbo.GetAcc*` | 3 | ClientRepository |
| `dbo.<table>` schema-qualified | 4 | PatientStatement{Encounter,EncounterActivity,Repository} |
| `LEFT(` + single-quoted alias | 3 | MessagesInboundRepository |
| single-quoted alias `as 'X'` (other) | 5 | InvoiceHistory, PatientStatement, Reporting (×2 blocks) |
| `GETDATE()` | 2 | RandomDrugScreenPersonRepository |
| `DATEADD`/`DATEDIFF`/`GetDate()` | 2 blocks | ReportingRepository |
| `nullif(` (portable but T-SQL idiom) | 2 | PatientStatement{Encounter,EncounterActivity} |
| `OUTPUT INSERTED.[pk]` synthesis | 3 | CustomSqlDatabaseProvider (class **deleted** in Phase 22) |
| bracket identifiers in SQL strings | 0 | — (none found; PetaPoco emits brackets via the provider, not in repo code) |
| `NOLOCK` | 0 | — |
| `OFFSET … FETCH` | 0 | — |

---

## 2. Per-file backlog table

Only files with live T-SQL are listed; all other repository files are auto-CRUD / `Fetch<>`-only and
port for free (see §4 for the full accounting). Complexity = the porting effort for that file's T-SQL.

| File | Markers found (file:line) | Count | Complexity |
|---|---|---|---|
| `ClientRepository.cs` | inline UDF `dbo.GetAccBalance` (`:85`), `dbo.GetAccBalByDate` (`:97`), `dbo.GetAccClientBalance` (`:117`) | 3 | **High** — 3 scalar-UDF call sites; depends on Phase 23 PL/pgSQL port of the `GetAcc*` family |
| `NumberRepository.cs` | `;EXEC GetNextNumber @0, @1 OUTPUT` (`:28`) | 1 | **High** — proc + `OUTPUT` param; rewrite to PL/pgSQL `OUT`-param fn or PG `SEQUENCE` |
| `MessagesInboundRepository.cs` | `left(...) as 'MessageType' … as 'QueueCount'` (`:23`), `GroupBy(left(...))` (`:26`), `OrderBy(left(...))` (`:27`), `ExecuteNonQueryProc("usp_cerner_chrg_reprocess", …)` (`:90`) | 4 | **High** — `LEFT`→`left`(ok) + quoted aliases→`"…"` + proc call to a proc ported in Phase 23 |
| `ReportingRepository.cs` | quoted aliases + `DATEADD(Day,-1,DATEDIFF(Day,0,GetDate()))` in two raw verbatim SQL blocks (`:20-23`, `:41-45`); built on raw `SqlConnection`/`SqlDataAdapter` (`:25-35`, `:47-53`) | 2 blocks | **High** — date arithmetic rewrite + the only repo bypassing PetaPoco entirely (raw ADO.NET `SqlConnection`) must be repointed to Npgsql |
| `PatientStatementEncounterActivityRepository.cs` | sub-select `from dbo.patbill_acc` + `nullif(date_sent,'')` (`:23-24`) | 1 | **Medium** — drop `dbo.` schema qualifier; `nullif` is portable |
| `PatientStatementEncounterRepository.cs` | sub-select `from dbo.patbill_acc` + `nullif(date_sent,'')` (`:23-24`) | 1 | **Medium** — same as above |
| `PatientStatementRepository.cs` | `count(*) as 'Cnt'` (`:47`), `.From("dbo.patbill_stmt")` (`:48`) | 2 | **Medium** — quoted alias→`"…"`; drop `dbo.` |
| `UserProfileRepository.cs` | `"TOP " + numEntries + " *"` string interpolation (`:35`) | 1 | **Medium** — relocate `TOP n` to a trailing `LIMIT n` (PetaPoco's PG provider does not auto-translate a `TOP` baked into the SELECT list) |
| `RandomDrugScreenPersonRepository.cs` | `GETDATE()` in UPDATE (`:126`, `:149`) | 2 | **Low** — `GETDATE()`→`now()`/`CURRENT_TIMESTAMP` |
| `InvoiceHistoryRepository.cs` | `client.cli_nme as 'ClientName'` (`:28`) | 1 | **Low** — single-quoted alias→double-quote |
| `CustomSqlDatabaseProvider.cs` | `OUTPUT INSERTED.[pk] into @result` synthesis (`:18`, `:29`, `:36`) | 3 | **Negative** — **entire class deleted** in Phase 22 (PG uses native `RETURNING`); listed for completeness, not a rewrite |

---

## 3. Flat prioritized call-site list (every concrete site, file:line)

Ordered by porting priority (runtime-critical inline UDF/proc calls first, then schema/alias/date funcs).

**P1 — runtime-critical inline UDF / stored-proc coupling (Phase 23 blockers):**
1. `LabBilling Library/Repositories/ClientRepository.cs:85` — `select dbo.GetAccBalance(@0)`
2. `LabBilling Library/Repositories/ClientRepository.cs:97` — `select dbo.GetAccBalByDate(@0, @1)`
3. `LabBilling Library/Repositories/ClientRepository.cs:117` — `dbo.GetAccClientBalance(...)` inline in SELECT list
4. `LabBilling Library/Repositories/NumberRepository.cs:28` — `;EXEC GetNextNumber @0, @1 OUTPUT`
5. `LabBilling Library/Repositories/MessagesInboundRepository.cs:90` — `ExecuteNonQueryProc("usp_cerner_chrg_reprocess", …)`

**P2 — T-SQL syntax / functions / schema qualifiers:**
6. `LabBilling Library/Repositories/UserProfileRepository.cs:35` — `"TOP " + numEntries + " *"`
7. `LabBilling Library/Repositories/MessagesInboundRepository.cs:23` — `left(col,3) as 'MessageType', count(*) as 'QueueCount'`
8. `LabBilling Library/Repositories/MessagesInboundRepository.cs:26` — `GroupBy("left(col, 3)")`
9. `LabBilling Library/Repositories/MessagesInboundRepository.cs:27` — `OrderBy("left(col, 3)")`
10. `LabBilling Library/Repositories/ReportingRepository.cs:20-23` — `... as 'Financial Class' ... DATEADD(Day,-1,DATEDIFF(Day,0,GetDate()))` (+ raw `SqlConnection` at `:25`)
11. `LabBilling Library/Repositories/ReportingRepository.cs:41-45` — same date arithmetic + aliases (+ raw `SqlConnection` at `:47`)
12. `LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs:126` — `mod_date = GETDATE(),`
13. `LabBilling Library/Repositories/RandomDrugScreenPersonRepository.cs:149` — `mod_date = GETDATE(),`
14. `LabBilling Library/Repositories/PatientStatementEncounterActivityRepository.cs:23-24` — `... from dbo.patbill_acc ... nullif(date_sent,'') is null`
15. `LabBilling Library/Repositories/PatientStatementEncounterRepository.cs:23-24` — `... from dbo.patbill_acc ... nullif(date_sent,'') is null`
16. `LabBilling Library/Repositories/PatientStatementRepository.cs:47` — `count(*) as 'Cnt'`
17. `LabBilling Library/Repositories/PatientStatementRepository.cs:48` — `.From("dbo.patbill_stmt")`
18. `LabBilling Library/Repositories/InvoiceHistoryRepository.cs:28` — `client.cli_nme as 'ClientName'`

**P0 — provider removal (not a rewrite, a deletion):**
19. `LabBilling Library/Repositories/CustomSqlDatabaseProvider.cs:18,29,36` — `OUTPUT INSERTED.[pk]` synthesis; whole class deleted in Phase 22.

---

## 4. Final tally & corrected percentage

- **Repository `.cs` files total:** 67
- **Files carrying live, port-requiring hand-written T-SQL:** **10**
  (ClientRepository, NumberRepository, MessagesInboundRepository, ReportingRepository,
  PatientStatementEncounterActivityRepository, PatientStatementEncounterRepository,
  PatientStatementRepository, UserProfileRepository, RandomDrugScreenPersonRepository,
  InvoiceHistoryRepository)
- **Plus 1 provider file deleted, not ported:** `CustomSqlDatabaseProvider.cs` → **11 files touched** in total.
- **Total concrete call sites to rewrite:** **18** live sites (P1+P2) + 3 deletion sites in the provider = **21**.

**Corrected backlog percentage (replaces the 40–50% estimate):**

> **10 of 67 repository files (≈ 15%) embed hand-written T-SQL that must be ported** — about
> **1/3** of the analysis's eyeballed "40–50%". The overwhelming majority of repositories use
> PetaPoco auto-CRUD / `Fetch<>` and port for free. If the count is scoped to the ~51 files that
> actually contain any SQL builder/raw-SQL, the T-SQL-bearing share is **10/51 ≈ 20%**. Either way
> the .NET-side rewrite surface is materially smaller than estimated, and is concentrated in
> **5 P1 runtime-critical sites** (the 3 `dbo.GetAcc*` UDFs + `GetNextNumber` + `usp_cerner_chrg_reprocess`).

The 40–50% figure appears to have counted *files containing any `Sql.Builder` chain* (38 files via
the `Sql.Builder` grep ≈ 57%, or 51 with raw SQL ≈ 76%) rather than files containing *non-portable
T-SQL*; most `Sql.Builder` usage is portable parameterized SQL with column names resolved through
`GetRealColumn`, which Npgsql/PetaPoco-PG handle unchanged.

---

## 5. Analysis-named sites — confirmation (all 5 present, Read-verified)

| Analysis citation | Marker | Verified at | Status |
|---|---|---|---|
| `UserProfileRepository.cs:35` (TOP) | `"TOP " + numEntries + " *"` | line 35 | ✅ present |
| `NumberRepository.cs:28` (EXEC GetNextNumber OUTPUT) | `;EXEC GetNextNumber @0, @1 OUTPUT` | line 28 | ✅ present |
| `ClientRepository.cs:85,97,117` (GetAccBalance family) | `dbo.GetAccBalance` / `dbo.GetAccBalByDate` / `dbo.GetAccClientBalance` | lines 85 / 97 / 117 | ✅ all three present |
| `MessagesInboundRepository.cs:23` (LEFT + quoted alias) | `left(...) as 'MessageType'` | line 23 | ✅ present |
| `MessagesInboundRepository.cs:90` (usp_cerner_chrg_reprocess) | `ExecuteNonQueryProc("usp_cerner_chrg_reprocess", …)` | line 90 | ✅ present |

---

## 6. Issues / clarifications

- **`InvoiceSelectRepository.cs:14-19`** matched the `dbo.` grep, but those lines are a **dead
  `/* … */` comment block** documenting the original query shape. The *live* code (`:29-57`) uses
  `Chrg.TableName`/`Account.TableName` constants and parameterized `Sql.Builder` — **no live T-SQL**,
  so it is **not** a port site. Excluded from §2/§3.
- **`ReportingRepository.cs`** is the only repository that bypasses PetaPoco entirely and uses raw
  `SqlConnection`/`SqlDataAdapter`/`SqlCommand` (`:25-35`, `:47-53`). Beyond the date-function
  rewrite, this whole class must be repointed from `Microsoft.Data.SqlClient` to `Npgsql` — a larger
  change than a `Sql.Builder` edit. Flagged High accordingly.
- **`UserProfileRepository.cs:35`** bakes `TOP` into the SELECT-column string (`"TOP n *"`). PetaPoco's
  PG provider rewrites `db.Page`/`SkipTake` paging but will not translate a `TOP` literal embedded in
  the projection — this needs an explicit `LIMIT` move, hence Medium not Low.
- Bracket-identifier SQL (`[x]`) was **not** found in repository source; PetaPoco emits brackets at the
  *provider* layer, which the PG provider replaces with double-quotes automatically. So the
  "brackets pervasive" note in §2.1 of the analysis applies to the DB project (views/procs), not the
  repository `.cs` files.
