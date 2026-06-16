# Lab Patient Accounting - Blazor Migration Roadmap

## Milestone: v1.0 - Core Billing Application

Complete migration of WinForms billing application to Blazor Server.

---

## Phase 01: Foundation & Shared Components
**Status:** In Progress
**Directory:** `.planning/phases/01-foundation/`

Create the foundational UI components and layout structure that all subsequent phases will use.

**Deliverables:**
- Extended MainLayout with full navigation structure
- Shared form input components (CurrencyInput, DateInput, PhoneInput)
- Lookup/autocomplete components (ClientLookup, InsuranceLookup, ProviderLookup, CDMLookup)
- DataGrid component with sorting, filtering, pagination
- Modal dialog component for edit forms
- Banner component for account header display
- Loading/progress indicators

**Plans:**
- 01-01: Navigation and layout structure
- 01-02: Form input components
- 01-03: Lookup components
- 01-04: DataGrid component
- 01-05: Modal and utility components

---

## Phase 02: Account Management Core
**Status:** Not Started
**Directory:** `.planning/phases/02-account-core/`

Implement the Account page - the central hub for patient billing.

**Deliverables:**
- Account search and navigation
- Account banner with patient info
- Summary tab
- Demographics tab with patient/guarantor editing
- Tab navigation structure for account details

**Plans:**
- 02-01: Account search and routing
- 02-02: Account banner and summary
- 02-03: Demographics tab

---

## Phase 03: Account Details - Diagnosis & Insurance
**Status:** Not Started
**Directory:** `.planning/phases/03-account-diagnosis-insurance/`

Implement diagnosis and insurance tabs for the Account page.

**Deliverables:**
- Diagnosis tab with ICD code search and assignment
- DX pointer management
- Primary Insurance tab
- Secondary Insurance tab
- Tertiary Insurance tab
- Insurance plan lookup and editing

**Plans:**
- 03-01: Diagnosis tab
- 03-02: Primary insurance tab
- 03-03: Secondary and tertiary insurance tabs

---

## Phase 04: Account Details - Charges & Payments
**Status:** Not Started
**Directory:** `.planning/phases/04-account-charges-payments/`

Implement charges and payments tabs for the Account page.

**Deliverables:**
- Charges tab with charge listing
- Charge entry form (add/edit)
- Charge maintenance (modify, void)
- Payments tab with payment listing
- Payment/adjustment entry form

**Plans:**
- 04-01: Charges tab and listing
- 04-02: Charge entry form
- 04-03: Payments tab
- 04-04: Payment entry form

---

## Phase 05: Account Details - Notes & Billing Activity
**Status:** Not Started
**Directory:** `.planning/phases/05-account-notes-billing/`

Implement notes and billing activity tabs.

**Deliverables:**
- Notes tab with note listing and alerts
- Add note functionality
- Billing Activity tab
- Claim status and validation display
- Statement history display
- Generate claim functionality

**Plans:**
- 05-01: Notes tab
- 05-02: Billing activity tab

---

## Phase 06: Work List
**Status:** Not Started
**Directory:** `.planning/phases/06-worklist/`

Implement the work list page for queue-based account processing.

**Deliverables:**
- Work queue tree view (recreate TreeView in Blazor)
- Account grid with filtering
- Filter options (name, client, account, insurance)
- Context menu actions (hold, change fin class, etc.)
- Quick navigation to account

**Plans:**
- 06-01: Work queue navigation
- 06-02: Account grid and filtering
- 06-03: Context menu actions

---

## Phase 07: Charge Entry
**Status:** Not Started
**Directory:** `.planning/phases/07-charge-entry/`

Implement standalone charge entry pages.

**Deliverables:**
- Account Charge Entry page
- Batch Charge Entry page
- CDM lookup and validation
- Charge posting workflow

**Plans:**
- 07-01: Account charge entry
- 07-02: Batch charge entry

---

## Phase 08: Remittance Processing
**Status:** Not Started
**Directory:** `.planning/phases/08-remittance/`

Implement remittance posting and processing.

**Deliverables:**
- Batch Remittance page
- Post Remittance page
- Process Remittance page
- Remittance file import
- Payment posting workflow

**Plans:**
- 08-01: Batch remittance
- 08-02: Post remittance
- 08-03: Process remittance

---

## Phase 09: Claims Management
**Status:** Not Started
**Directory:** `.planning/phases/09-claims/`

Implement claims batch management.

**Deliverables:**
- Claims Management page
- Claim batch listing
- Claim batch detail view
- Generate claims (institutional/professional)
- Regenerate/clear batch functions

**Plans:**
- 09-01: Claims management page
- 09-02: Claim generation workflow

---

## Phase 10: Client Invoices
**Status:** Not Started
**Directory:** `.planning/phases/10-invoices/`

Implement client invoice generation.

**Deliverables:**
- Client Invoice page
- Client selection
- Invoice generation workflow
- Invoice preview/download

**Plans:**
- 10-01: Client invoice page

---

## Phase 11: Patient Collections
**Status:** Not Started
**Directory:** `.planning/phases/11-collections/`

Implement bad debt/patient collections management.

**Deliverables:**
- Patient Collections page
- Collections listing
- Collection status management
- Collections wizard

**Plans:**
- 11-01: Patient collections page

---

## Phase 12: Dictionary - Charge Master
**Status:** Not Started
**Directory:** `.planning/phases/12-dict-chargemaster/`

Implement Charge Master (CDM) maintenance.

**Deliverables:**
- Charge Master list page
- Charge Master edit form
- Add/edit/delete functionality
- CPT/Revenue code management

**Plans:**
- 12-01: Charge master list
- 12-02: Charge master edit

---

## Phase 13: Dictionary - Clients (Extended)
**Status:** Not Started
**Directory:** `.planning/phases/13-dict-clients/`

Extend existing Client Viewer with full maintenance capabilities.

**Deliverables:**
- Extend ClientList with edit capability
- Client edit form (full detail)
- Add new client functionality

**Plans:**
- 13-01: Client edit form
- 13-02: Add new client

---

## Phase 14: Dictionary - Insurance Plans
**Status:** Not Started
**Directory:** `.planning/phases/14-dict-insurance/`

Implement Insurance Plan maintenance.

**Deliverables:**
- Health Plan list page
- Health Plan edit form
- Insurance company management
- Plan codes and settings

**Plans:**
- 14-01: Health plan list
- 14-02: Health plan edit

---

## Phase 15: Dictionary - Physicians
**Status:** Not Started
**Directory:** `.planning/phases/15-dict-physicians/`

Implement Physician and Pathologist maintenance.

**Deliverables:**
- Physician list page
- Physician edit form
- Pathologist list page
- NPI and credential management

**Plans:**
- 15-01: Physician maintenance
- 15-02: Pathologist maintenance

---

## Phase 16: Dictionary - Audit Reports
**Status:** Not Started
**Directory:** `.planning/phases/16-dict-audit/`

Implement Audit Report configuration.

**Deliverables:**
- Audit Report list page
- Audit Report edit form
- Report configuration

**Plans:**
- 16-01: Audit report maintenance

---

## Phase 17: System Administration - Users
**Status:** Not Started
**Directory:** `.planning/phases/17-admin-users/`

Implement user security management.

**Deliverables:**
- User list page
- User edit form
- Permission management
- Password reset functionality

**Plans:**
- 17-01: User management

---

## Phase 18: System Administration - Parameters & Interface
**Status:** Not Started
**Directory:** `.planning/phases/18-admin-system/`

Implement system parameters and interface monitoring.

**Deliverables:**
- System Parameters page
- Interface Mapping page
- Interface Monitor page
- Log Viewer page
- Account Locks viewer

**Plans:**
- 18-01: System parameters
- 18-02: Interface mapping
- 18-03: Interface monitor and logs

---

## Phase 19: Dashboard & Navigation Polish
**Status:** Not Started
**Directory:** `.planning/phases/19-dashboard/`

Implement dashboard and finalize navigation.

**Deliverables:**
- Dashboard with summary widgets
- Recent accounts quick access
- Navigation refinements
- About page

**Plans:**
- 19-01: Dashboard implementation
- 19-02: Navigation polish

---

## Phase 20: Integration Testing & Polish
**Status:** Not Started
**Directory:** `.planning/phases/20-integration/`

End-to-end testing and final polish.

**Deliverables:**
- Integration testing
- UI/UX refinements
- Performance optimization
- Bug fixes

**Plans:**
- 20-01: Integration testing
- 20-02: Final polish

---

## Milestone: v1.1 - Platform Modernization (PostgreSQL + Redis)

Replace Microsoft SQL Server with PostgreSQL and introduce Redis caching for the
hotspots identified in `analyses/dbms-modernization-and-redis-hotspots.md`. This
milestone is **independent of the v1.0 Blazor UI work** and can proceed in parallel;
it touches `LabBilling Library` (data access), the database projects, the Quartz
scheduler, and `LabOutreachUI` (caching/auth state).

Source of truth for all decisions: `analyses/dbms-modernization-and-redis-hotspots.md`.

---

### Phase 21: De-Risking Spikes
**Status:** Not Started
**Directory:** `.planning/phases/21-derisking-spikes/`
**Depends on:** none (gates everything below)

Resolve the open questions from §6 of the analysis before committing migration effort.
These are time-boxed investigations that produce decisions + effort sizing, not production code.

**Deliverables:**
- Runtime DB-object surface trace (Extended Events) → confirmed thin runtime proc/UDF/view set
- MARS-dependency audit (Npgsql has no MARS) → list of nested-reader code paths to refactor
- XML-shredding approach bake-off: `xmltable()` PL/pgSQL vs. C# (j4jayant parser) on a sample message
- Managed-PostgreSQL BAA + at-rest + TLS + `pgaudit` SKU validation (e.g. AWS RDS)
- Exact repository T-SQL porting backlog (scripted scan of `Sql.Builder` usages)
- FINDINGS.md sizing Phases 22-25

**Plans:**
- 21-01: Static-analysis spikes — repository T-SQL backlog + MARS-dependency audit (autonomous)
- 21-02: Runtime DB-object surface trace (live SQL Server env)
- 21-03: XML-shredding approach bake-off + decision (`xmltable()` vs. C#)
- 21-04: Managed-PostgreSQL HIPAA SKU validation + decision, consolidate FINDINGS

---

### Phase 22: PostgreSQL Schema & Data-Access Foundation
**Status:** Not Started
**Directory:** `.planning/phases/22-postgres-foundation/`
**Depends on:** Phase 21

Stand up the PostgreSQL schema and repoint the .NET data-access layer, without porting
in-database procedural logic yet (Phase 23).

**Deliverables:**
- PostgreSQL schema: ~500 tables, IDENTITY→`GENERATED ... AS IDENTITY`, computed columns→`GENERATED ALWAYS AS ... STORED`, 7 schemas, ~209 views (bracket→double-quote)
- PetaPoco swapped to built-in `PostgreSQLDatabaseProvider`; `CustomSqlDatabaseProvider` deleted (native `RETURNING`)
- `Microsoft.Data.SqlClient` → `Npgsql`; `AppEnvironment` connection construction rebuilt
- **TLS-in-transit enabled** (`SSL Mode=Require`) — closes the `Encrypt=false` HIPAA gap
- Both UoW construction sites (`UnitOfWorkMain`, `UnitOfWorkSystem`) repointed

**Plans:**
- 22-01: Schema generation & type/identity/computed-column mapping
- 22-02: Provider swap, Npgsql, connection rebuild + TLS
- 22-03: Auto-CRUD repository smoke verification

---

### Phase 23: In-Database Logic & Repository Port
**Status:** Not Started
**Directory:** `.planning/phases/23-logic-repo-port/`
**Depends on:** Phase 22

Port the procedural surface the runtime actually uses, plus the repositories carrying
hand-written T-SQL. Batch/reporting procs not on the runtime path are deferred to incremental follow-up.

**Deliverables:**
- Runtime UDFs ported to PL/pgSQL: `GetAccBalance`/`GetAccBalByDate`/`GetAccClientBalance`, `GetNextNumber`
- `usp_cerner_chrg_reprocess` and XML-shredding import procs ported (per Phase 21 decision)
- Repository T-SQL rewritten: `[brackets]`→`"quotes"`, `TOP`→`LIMIT`, inline UDF/proc call sites
- Inline scalar-UDF call sites updated (`ClientRepository.cs:85,97,117`, `NumberRepository.cs:28`, `MessagesInboundRepository.cs:90`)

**Plans:**
- 23-01: Runtime scalar UDFs + GetNextNumber port
- 23-02: XML-shredding / cerner import procs port
- 23-03: Repository T-SQL rewrite backlog
- 23-04: View definitions port

---

### Phase 24: Background Jobs → Quartz.NET
**Status:** Not Started
**Directory:** `.planning/phases/24-jobs-to-quartz/`
**Depends on:** Phase 23

Consolidate SQL Server Agent jobs into the existing Quartz.NET scheduler and remove
the `sp_send_dbmail` / Windows-path dependencies.

**Deliverables:**
- 4 SQL Agent jobs (Accounts Aging Payout, BadDebt Writeoff, Daily AM Run, nLog Purge) as Quartz jobs
- `sp_send_dbmail` replaced by app-layer SMTP email service
- `H:\sqlText` output coupling retired

**Plans:**
- 24-01: Quartz job scaffolding + SMTP email service
- 24-02: Port aging/writeoff/purge jobs
- 24-03: Port Daily AM Run (multi-step reconciliation)

---

### Phase 25: Data Migration & Cutover
**Status:** Not Started
**Directory:** `.planning/phases/25-data-cutover/`
**Depends on:** Phase 24

Move data to PostgreSQL and validate parity before cutover.

**Deliverables:**
- Bulk data load (pgloader / AWS DMS) with FK/index integrity
- Row-count + checksum parity validation vs. SQL Server
- Dual-run / shadow verification of critical workflows
- Cutover runbook

**Plans:**
- 25-01: Bulk load tooling + run
- 25-02: Parity validation harness
- 25-03: Cutover runbook + dry run

---

### Phase 26: Redis Caching
**Status:** Not Started
**Directory:** `.planning/phases/26-redis-caching/`
**Depends on:** Phase 22 (stable data layer); ideally after Phase 25 cutover

Introduce Redis for the §5 hotspots. Explicitly skips the §5.3 tempting-but-bad targets
(live balances, full account aggregate, transactional writes, raw PHI).

**Deliverables:**
- Redis infrastructure + `StackExchange.Redis` / `IDistributedCache` DI wiring
- Reference-data cache for `DictionaryService` (CPT/dx/CDM/revenue/fin codes, client/insurance/mapping)
- Distributed auth/identity cache (replaces process-local `UserCircuitHandler` dictionary)
- HL7 idempotency guard (recently-processed control-IDs) + distributed number/lock coordination

**Plans:**
- 26-01: Redis infra + DI wiring + health check
- 26-02: DictionaryService reference-data cache
- 26-03: Distributed auth/identity cache
- 26-04: HL7 idempotency + distributed coordination

---

## Future Milestones

### v1.2 - Reports Integration
- SQL Reporting Services integration
- Report viewer component
- Standard report access

### v1.3 - Advanced Features
- Mobile-responsive optimizations
- Advanced search capabilities
- Batch operations enhancements

---

## Progress Tracking

| Phase | Status | Plans Complete |
|-------|--------|----------------|
| 01 Foundation | In Progress | 2/5 |
| 02 Account Core | Not Started | 0/3 |
| 03 Diagnosis/Insurance | Not Started | 0/3 |
| 04 Charges/Payments | Not Started | 0/4 |
| 05 Notes/Billing | Not Started | 0/2 |
| 06 Work List | Not Started | 0/3 |
| 07 Charge Entry | Not Started | 0/2 |
| 08 Remittance | Not Started | 0/3 |
| 09 Claims | Not Started | 0/2 |
| 10 Invoices | Not Started | 0/1 |
| 11 Collections | Not Started | 0/1 |
| 12 Dict-CDM | Not Started | 0/2 |
| 13 Dict-Clients | Not Started | 0/2 |
| 14 Dict-Insurance | Not Started | 0/2 |
| 15 Dict-Physicians | Not Started | 0/2 |
| 16 Dict-Audit | Not Started | 0/1 |
| 17 Admin-Users | Not Started | 0/1 |
| 18 Admin-System | Not Started | 0/3 |
| 19 Dashboard | Not Started | 0/2 |
| 20 Integration | Not Started | 0/2 |
| **v1.1 Platform Modernization** | | |
| 21 De-Risking Spikes | Not Started | 0/4 |
| 22 Postgres Foundation | Not Started | 0/3 |
| 23 Logic & Repo Port | Not Started | 0/4 |
| 24 Jobs → Quartz | Not Started | 0/3 |
| 25 Data Cutover | Not Started | 0/3 |
| 26 Redis Caching | Not Started | 0/4 |
