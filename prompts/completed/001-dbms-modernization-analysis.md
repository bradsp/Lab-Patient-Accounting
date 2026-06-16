<objective>
Produce a single planning-grade analysis document that answers two coupled modernization questions for the Lab Patient Accounting application:

1. Which open-source DBMS should replace Microsoft SQL Server, given performance, stability, HIPAA suitability, and ecosystem support?
2. Where in this specific application would adding a Redis cache yield meaningful performance improvement?

This document will feed downstream planning work (migration roadmap, effort sizing, vendor selection, security review). It is not the migration plan itself — it is the input that justifies the choices the migration plan will make. Treat it as a document a senior engineer, an architect, and a compliance reviewer will all read. Be concrete, opinionated where the evidence supports it, and explicit about uncertainty where it doesn't.

The user has selected the following framing — honor it:

- **DBMS conclusion style: single recommendation.** Pick one DBMS, justify it deeply, and treat the other candidates as runner-ups with a short rationale for why they lost. Do **not** produce a non-committal "decision framework" — make the call.
- **Migration depth: code-level inventory.** Enumerate the SQL Server-specific features actually used by this codebase (T-SQL stored procs, views, functions, identity columns, datetime/datetime2/datetimeoffset, computed columns, `MERGE`, `OUTPUT`, `ROWVERSION`, table-valued parameters, etc.), and name the specific files/services/repositories that will need rework. PetaPoco provider compatibility must be checked explicitly.
- **HIPAA scope: feature comparison.** Compare each finalist DBMS's encryption-at-rest, TDE/equivalent, audit logging, RBAC/row-level security, TLS, and key management against the HIPAA technical safeguards. Practical depth — not a full §164.312 controls map.
- **Redis scope: identify hotspots only.** Survey this codebase and list the specific places caching would pay off (dictionary lookups, HL7 dedupe, claim state, etc.), with rough hit-rate / latency-impact reasoning. No design sketches, no key-shape proposals, no TTL recommendations — that comes later.
</objective>

<context>
- Codebase root: the current working directory.
- **Read `./CLAUDE.md` first.** It is the source of truth for project layout, the layered architecture, the dual `UnitOfWorkMain` / `UnitOfWorkSystem` databases, the active WinForms→Blazor UI migration on branch `billing-ui-to-blazor`, and the deprecated `MCL` data access layer. Honor those facts in the analysis; do not contradict them.
- The application is a medical billing system for outreach laboratories. It processes HL7 ADT/DFT/MFN messages, generates X12 837i/837p claims, and handles patient billing. This is a HIPAA-regulated workload — PHI lives in this database.
- Current persistence stack: Microsoft SQL Server 2014+, PetaPoco micro-ORM, repository + UnitOfWork pattern, 500+ tables, schema in `LabPatientAccounting DB Build.sql` and the `LabBillingDatabase` / `Lab PA Database` SQL Server Database Projects.
- The WinForms UI is being deprecated in favor of `LabOutreachUI` (Blazor Server, .NET 8). DBMS choice should consider Blazor/.NET 8 ergonomics, not WinForms.
- Background workloads exist: `LabBillingService` (Topshelf Windows service), `Lab Patient Accounting Job Scheduler` (Quartz.NET), `LabBillingConsole` (batch). Any DBMS choice must support these access patterns, not just interactive web traffic.
- HL7 parsing lives in `j4jayant.HL7.Parser`; X12 generation uses `EdiTools`. Neither is database-coupled, so they constrain the choice less than the schema does.
</context>

<research>
You must do BOTH codebase research and external research. Do not skip either.

**Codebase research — gather evidence before recommending anything:**

1. Read `./CLAUDE.md` end-to-end.
2. Inventory SQL Server-specific surface area:
   - Glob the `LabBillingDatabase` and `Lab PA Database` projects for `.sql` files. Sample representative stored procedures, functions, views, and triggers — enough to characterize what T-SQL features are in use. You do NOT need to read all 500+ tables; you DO need to identify which T-SQL features (CTEs, window functions, `MERGE`, `OUTPUT INTO`, `ROWVERSION`/`TIMESTAMP`, computed columns, `IDENTITY`, table-valued parameters, `XML`/`JSON` columns, full-text search, SQLCLR, linked servers, SQL Server Agent jobs) appear and how heavily.
   - Read `Lab Patient Accounting SQL Agent Jobs.sql` to inventory anything that depends on SQL Server Agent specifically (since open-source DBMSs lack a direct equivalent).
3. Inventory the .NET data access layer:
   - Glob `LabBilling Library/Repositories/**/*.cs` and sample several repositories. Note any raw SQL strings, T-SQL-flavored syntax (square brackets, `TOP`, `OFFSET ... FETCH`, `NOLOCK`, `OUTPUT`), and PetaPoco features in use.
   - Find every `UnitOfWork`/connection-construction site and identify the SqlClient provider in use (`Microsoft.Data.SqlClient` vs `System.Data.SqlClient`).
   - Confirm whether PetaPoco is used with a provider abstraction or hard-coded to SQL Server. Check the PetaPoco config/initialization for explicit provider selection.
4. Inventory the access pattern, since it drives the Redis hotspot analysis:
   - Identify reference-data lookups (CPT codes, diagnosis codes, fee schedules, provider/payer dictionaries, client master) that are read-heavy and change rarely. Grep for `DictionaryService` usage and similar reference-data services.
   - Identify hot read paths: account loads (`AccountService` ~2500 lines per CLAUDE.md), claim status checks, validation lookups during HL7 ingestion.
   - Identify idempotency/dedupe candidates: HL7 message processing (control IDs), claim submission tracking.
   - Identify session/auth state: how does `LabOutreachUI` cache user/role/policy lookups? Look at `LabOutreachUI/Authentication/` and `LabOutreachUI/Authorization/`.
   - Identify anything currently cached in-memory inside a service (those are candidates to move to Redis if they need to be shared across `LabOutreachUI` web nodes, `LabBillingService`, and the job scheduler).
5. Inventory the existing schema's data shape at a high level — count rough table cardinality (e.g., "~500 relational tables, heavily normalized, foreign-key-rich, no obvious document-store fit"). This is the evidence that decides whether MongoDB is even in the running.

For efficiency, use parallel tool calls when you need to perform multiple independent searches (e.g., multiple Globs, multiple Greps).

**External research — current information matters, your training data is stale:**

Use `WebSearch` and `WebFetch` (and the context7 MCP server when fetching library docs like Npgsql, MariaDB Connector/NET, MongoDB.Driver, StackExchange.Redis, or PetaPoco provider notes) to gather **current** evidence on:

- PostgreSQL, MariaDB, MySQL Community Edition, and MongoDB Community for the criteria below. Include each candidate's current stable major version, license posture (especially MariaDB vs. MySQL post-Oracle, and MongoDB's SSPL implications for commercial hosting), and recent (last ~24 months) stability/security track record.
- HIPAA-relevant features per DBMS: encryption at rest (native vs. filesystem-only), TLS in transit, audit logging (native vs. extension), row-level security / fine-grained RBAC, key management integration (KMS, HSM), and **which major managed offerings will sign a BAA** for each (AWS RDS, Azure Database for X, GCP Cloud SQL, Aiven, Crunchy Bridge, MongoDB Atlas, etc.). The BAA point is decisive — a DBMS no managed provider will sign a BAA for is effectively self-hosted-only for HIPAA, which is a material operational burden.
- .NET 8 driver ecosystem: maturity of Npgsql, MySqlConnector, MongoDB.Driver. PetaPoco provider support for each (confirm via PetaPoco docs/repo — does it ship a Postgres provider, or does the community use a fork/adapter?). Async/cancellation support and connection-pool maturity matter for Blazor Server.
- Migration tooling from SQL Server: pgloader, AWS SCT/DMS, Babelfish for Aurora Postgres (T-SQL compatibility layer — is it production-viable?), and equivalent paths for MariaDB/MySQL.

Do not skim. Cite primary sources (official docs, vendor BAA pages, CVE databases, release notes) where possible. Note publication dates when relevance is time-sensitive.
</research>

<analysis_requirements>
Thoroughly analyze the evidence and produce an opinionated, decision-quality document. Where the codebase and external evidence point clearly to one answer, say so plainly. Where they don't, name the uncertainty and the smallest experiment that would resolve it.

The DBMS section must:

- **Recommend exactly one DBMS** for replacing SQL Server in this application. Defend the choice on each criterion (performance, stability, HIPAA suitability, support/ecosystem, .NET driver and PetaPoco fit, migration cost from the current T-SQL surface). Quantify where possible ("PostgreSQL 17 supports X; SQL Server uses Y; conversion cost: ~N stored procedures touch this feature").
- For each runner-up (cover at minimum Postgres, MariaDB/MySQL, and MongoDB — explicitly include MongoDB even if you reject it, since the user named it), give a short, specific reason it lost. "MongoDB lost because the schema is 500+ relational tables with heavy FK use; document remodeling cost dwarfs any benefit" is a real answer. "MongoDB lost because relational is better" is not.
- Map the T-SQL feature inventory you built in research onto the recommended DBMS: for each SQL Server feature in use, name the equivalent (or the workaround) in the chosen DBMS, and flag the items that have no clean equivalent. This is the migration-effort signal the user asked for.
- Name the PetaPoco provider story explicitly: does it work as-is, work with a community provider, or require ORM replacement? If replacement is on the table, name 1–2 candidate ORMs that fit a .NET 8 + repository-pattern + Blazor Server stack.
- Identify the SQL Server Agent replacement strategy implied by the choice (extend existing Quartz.NET, use pgAgent, etc.).

The HIPAA section must:

- Compare encryption-at-rest support (native TDE-equivalent vs. filesystem-only), TLS-in-transit, audit logging (native vs. extension), RBAC granularity, row-level security, and key-management integration across the finalist DBMSs.
- Be a **feature comparison**, not a full §164.312 controls map. The user explicitly chose feature comparison over full mapping.
- Note BAA availability per major managed offering for the recommended DBMS, since hosting choice is the dominant HIPAA risk surface and self-hosting in this org may not be realistic.

The Redis section must:

- Be **hotspot identification only** — no design sketches, no key shapes, no TTL recommendations, no invalidation patterns. The user was explicit.
- For each hotspot, name the specific service/file/path involved (e.g., "`DictionaryService` CPT/dx lookups in `LabBilling Library/Services/DictionaryService.cs`"), why it's a candidate (read-heavy, low cardinality, rarely-changing, called from multiple processes, etc.), and a qualitative expected impact (e.g., "high — called on every charge entry across web + service + scheduler; current path round-trips to SQL Server").
- Also identify hotspots that look tempting but are bad Redis candidates and say why (e.g., "patient account snapshot looks cacheable, but it's both PHI and high-churn — caching it externally adds a HIPAA surface for marginal latency gain").
- Distinguish between *application-level data caches* and *shared state caches* (HL7 dedupe / idempotency tokens, distributed locks for the Topshelf service + scheduler + web). The latter is a Redis win even when the former isn't.

Use the parallel-tool-call rule throughout research: when you need multiple independent file reads or web fetches, batch them in one message.

After each tool result, briefly reflect — does this change the recommendation? Adjust before proceeding.
</analysis_requirements>

<output>
Save the analysis to: `./analyses/dbms-modernization-and-redis-hotspots.md`

Use this structure exactly (the document feeds downstream planning, so a predictable shape matters):

```
# DBMS Modernization & Redis Hotspot Analysis

## 1. Executive Summary
- Recommended DBMS and one-sentence justification
- Top 3 Redis hotspots in priority order
- The single biggest risk to the migration
- The single biggest unknown that should be resolved before committing

## 2. Current State Inventory
### 2.1 SQL Server feature surface in use
### 2.2 Data access layer & PetaPoco binding
### 2.3 Background workloads & SQL Agent dependence
### 2.4 Schema shape (relational vs. document fit)

## 3. DBMS Recommendation
### 3.1 Recommendation (the single chosen DBMS)
### 3.2 Why this DBMS wins on each criterion
  - Performance
  - Stability & maturity
  - HIPAA feature suitability (summary — full comparison in §4)
  - .NET driver & PetaPoco fit
  - Ecosystem & support
  - Migration cost from current T-SQL surface
### 3.3 Runner-ups and why they lost
  - PostgreSQL (if not chosen)
  - MariaDB / MySQL
  - MongoDB
  - Any other candidate considered
### 3.4 T-SQL feature → target DBMS feature map
  (Table: SQL Server feature in use | Codebase locations | Target equivalent | Effort signal)
### 3.5 ORM strategy
  (PetaPoco-as-is / PetaPoco + community provider / ORM replacement + candidates)
### 3.6 SQL Server Agent replacement
### 3.7 Migration tooling shortlist

## 4. HIPAA Feature Comparison
  (Table: criterion | SQL Server (today) | Recommended DBMS | Runner-up DBMS)
  Criteria: encryption at rest, TLS in transit, audit logging, RBAC granularity,
  row-level security, key management integration, BAA availability across major
  managed offerings.

## 5. Redis Hotspot Survey
### 5.1 High-value hotspots (table: hotspot | location in code | why cacheable | expected impact)
### 5.2 Shared-state caches across web + service + scheduler
### 5.3 Tempting-but-bad candidates (with reasons)

## 6. Open Questions & Recommended Spikes
  (Bulleted list: the things that should be answered or prototyped before the
  migration plan is written. Each item: question, why it matters, smallest
  experiment that would resolve it.)
```

Tone rules for the document itself:

- No filler. No "in this section we will discuss." Lead each section with the conclusion or the data.
- Cite codebase paths with `file_path:line_number` when naming a specific call site.
- Cite external sources by URL inline for any claim that depends on current vendor state.
- Tables over prose when comparing N items on M criteria.
- Use precise language: "T-SQL `MERGE` appears in 14 stored procedures; PostgreSQL added `MERGE` in v15 with minor syntax differences" beats "MERGE may need attention."
</output>

<verification>
Before declaring the document complete, verify all of the following. If any item is "no" or "unsure", fix it before finishing:

- [ ] The recommendation section names exactly one DBMS — not a tie, not "it depends."
- [ ] Every runner-up DBMS (Postgres if not chosen, MariaDB/MySQL, MongoDB) has a specific rejection rationale grounded in this codebase, not a generic one.
- [ ] The T-SQL feature map is built from actual evidence in `LabBillingDatabase` / `Lab PA Database` / SQL Agent jobs files — not from a generic SQL Server feature list.
- [ ] PetaPoco's provider story for the recommended DBMS is stated explicitly with a source (PetaPoco docs, repo issue, or community provider link).
- [ ] The HIPAA section addresses BAA availability for at least one realistic managed hosting option per finalist DBMS.
- [ ] Every Redis hotspot names a concrete service/file, not a generic category like "reference data."
- [ ] At least one "tempting but bad" Redis candidate is listed and rejected, to prove the survey is critical, not just enthusiastic.
- [ ] Open Questions section has at least 3 items, each with a "smallest experiment" framing.
- [ ] No claim that contradicts `./CLAUDE.md` (e.g., do not assume WinForms is the target UI; do not assume `MCL` is still the data layer).
- [ ] Document saves to `./analyses/dbms-modernization-and-redis-hotspots.md` (create the `analyses/` directory if it doesn't exist).
</verification>

<success_criteria>
The document is successful if a competent architect could read it and, without further conversation, draft a migration plan that picks the right DBMS, scopes the T-SQL conversion work to the right order of magnitude, knows which hosting options preserve HIPAA posture, and knows where the first Redis investments should land. If any of those four are still ambiguous after reading, the analysis isn't done.
</success_criteria>

<!--
Completed: 2026-06-16
Executed via: /taches-cc-resources:run-prompt 001
Output: ./analyses/dbms-modernization-and-redis-hotspots.md
Result: PostgreSQL recommended (single call); MariaDB runner-up; MongoDB rejected. Top Redis hotspots: DictionaryService reference data, AccountService.GetAccount reference fan-out, Blazor auth/identity lookups.
-->
