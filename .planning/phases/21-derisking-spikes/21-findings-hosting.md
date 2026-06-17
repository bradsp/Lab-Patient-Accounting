# 21-04 / Task 1 — Managed PostgreSQL HIPAA Hosting Evaluation (current, 2026)

**De-risking spike for the SQL Server → PostgreSQL migration of the Lab Patient Accounting billing application.**

> Status: desk research only. Nothing provisioned, no BAA executed. All claims below carry a primary-source vendor URL with an **access date of 2026-06-16**. Where a vendor "what's new" / changelog post fixes a date-sensitive fact (version availability), the publication date is noted inline.

## Context / what this answers

The prior DBMS analysis already chose PostgreSQL as the target DBMS and noted that BAAs are broadly available across the hyperscalers. This task makes the **SKU-level call concrete and current**. The workload is a HIPAA-regulated medical billing app (PHI), ~500 relational tables migrated off SQL Server, running on .NET 8/10 via **Npgsql**, and it needs all three of **pgaudit + customer-managed key (CMK) + forced TLS** in a *single* managed SKU, plus a current stable major version (PostgreSQL 17.x, ideally 18.x available).

The five leading managed offerings were evaluated: AWS RDS for PostgreSQL, AWS Aurora PostgreSQL, Azure Database for PostgreSQL (Flexible Server), GCP Cloud SQL for PostgreSQL, Crunchy Bridge, and Aiven for PostgreSQL.

---

## Comparison matrix

| Provider / SKU | 1. BAA (sign + tier/conditions) | 2. Encryption at rest + CMK | 3. TLS in transit (force knob) | 4. pgaudit on managed plane | 5. Major version (17.x / 18.x) | 6. Feature carve-outs |
|---|---|---|---|---|---|---|
| **AWS RDS for PostgreSQL** | Yes — RDS is a HIPAA-eligible service under the AWS BAA; PostgreSQL engine explicitly named. Requires an executed AWS BAA (via AWS Artifact). [1] | AES-256 at rest; CMK via **AWS KMS** (customer-managed keys). [2] | Yes — `rds.force_ssl` parameter; **default `1` (on) for PG 15+**. [4] | Yes — `pgaudit` extension installable; needs `rds_superuser`; `pgaudit 18.0` ships for PG 18. [5] | **18.4, 17.10, 16.14, 15.18, 14.23** (minors as of 2026-05-14); major **18** GA since 18.1. [3] | No true superuser; `rds_superuser` is a constrained role; extensions limited to an RDS allow-list. [5] |
| **AWS Aurora PostgreSQL** | Yes — Amazon Aurora is listed as a HIPAA-eligible service under the AWS BAA (no engine carve-out noted). [1] | AES-256 at rest; CMK via **AWS KMS**. [2] | Yes — `rds.force_ssl` (same Aurora/RDS PG parameter family). [4] | Yes — `pgaudit` available; setup via `rds_superuser`. [5] | Aurora tracks community PG; 17.x line supported (Aurora PostgreSQL updates page). [3a] | Same managed-plane restrictions as RDS (`rds_superuser`, extension allow-list). [5] |
| **Azure Database for PostgreSQL — Flexible Server** | Yes — HIPAA BAA is offered **by default** via the Microsoft **Product Terms** (formerly Online Services Terms) + DPA; **no separate contract to sign** — execution of the volume licensing agreement includes the BAA. Flexible Server is an in-scope service. [6] | Encrypted at rest by default; CMK via **Azure Key Vault** (or Key Vault Managed HSM). CMK mode is **set at server-create time and immutable** for the server's life. [7] | Yes — `require_secure_transport` server parameter forces TLS. [8] | Yes — `pgaudit` extension: allowlist → load (`shared_preload_libraries`) → `CREATE EXTENSION`; `pgaudit.log` set via server parameters. [9] | **17.x supported** (17.10), plus 16/15/14/13; in-place major-version upgrade supported. [10] | Managed roles (`azure_pg_admin`), no OS superuser; extensions must be **allowlisted** before load. CMK choice immutable post-create. [7][9] |
| **GCP Cloud SQL for PostgreSQL** | Yes — Google Cloud BAA covers **Cloud SQL** (explicitly listed in covered products); customer executes the BAA via the Cloud console (Privacy compliance & records). [11] | AES-256 at rest (Google-managed by default); CMK via **Cloud KMS (CMEK)**. [12] | Yes — connection `ssl_mode` = `ENCRYPTED_ONLY` (rejects unencrypted) or `TRUSTED_CLIENT_CERTIFICATE_REQUIRED`. [13] | Yes — `pgaudit` via flag `cloudsql.enable_pgaudit=on` + `pgaudit.log`. [14] | **18 (default), 17**, 16, 15, 14, 13… (18 GA 2025-09-25; 17 GA 2024-10-22). [15] | No customer superuser; databases owned by `cloudsqlsuperuser`; managed-service extension/flag model. [13a] |
| **Crunchy Bridge** | Yes — HIPAA supported; **BAA available on request** (contact Crunchy). No published tier gate, but HIPAA is a contractual/contact path, not self-serve. [16] | AES-256 at rest; CMK supported via "Crunchy Bridge Encryption Keys" leveraging the underlying cloud's KMS (AWS/Azure/GCP). [16] | Yes — all instances **require TLS 1.2+** (enforced by default, not optional). [16] | Yes — auditing implemented via **pgAudit**; full auditing enabled on several role types by default. [17] | Tracks current community PG (17/18 line on Postgres-version availability; confirm exact minor at provision time). [16] | Real superuser is *not* exposed on the managed plane, but Crunchy is closest to "stock Postgres" — fewer extension restrictions than the hyperscalers; you create your own roles. [16] |
| **Aiven for PostgreSQL** | Yes — HIPAA compliant since Dec 2018; **BAA on request** (contact Aiven). No published self-serve tier, but data-plane features below have caveats. [18] | Data disk + backups + replication stream encrypted (AES). **CMK / BYOK is a limited-availability feature — must request access** (RSA-2048/4096 keys in AWS/Azure/GCP/OCI KMS). [19][20] | Yes — TLS enforced; deprecated TLS 1.0/1.1 removed; `verify-ca`/`verify-full` require the **Aiven project CA cert**. [21] | Yes — `pgaudit` available and documented for Aiven for PostgreSQL. [22] | **17 is the default for new services**; 18.x minors shipped (18.2/17.8…) per changelog. [23] | No superuser; managed extension list; **CMK is gated behind limited-availability access request** — the key carve-out vs hyperscalers. [20] |

---

## Per-provider notes (things that don't fit a cell)

### AWS RDS / Aurora PostgreSQL
- BAA mechanism: the HIPAA-eligible-services list explicitly enumerates RDS engines — *"Amazon RDS [SQL Server, MySQL, Oracle, PostgreSQL, Db2 and MariaDB engines only]"* — and Aurora separately. The list was **Last Updated 2026-05-22**. [1] The BAA itself is accepted through AWS Artifact.
- `rds.force_ssl=1` is already the **default on PG 15+**, so a current 17.x/18.x instance forces TLS out of the box; flipping it also sets the engine `ssl=1` and rewrites `pg_hba.conf`. [4]
- pgaudit requires the `rds_superuser` role and is set up via a custom parameter group (`shared_preload_libraries=pgaudit`, then `CREATE EXTENSION pgaudit`). [5]
- This is the **only one of the five where pgaudit + KMS CMK + forced-TLS are all first-class, GA, self-serve, single-SKU** with no "contact us / limited availability" gate.

### Azure Flexible Server
- Strongest BAA story contractually: **no separate BAA to sign** — it is incorporated into the Product Terms/DPA on licensing-agreement execution. [6]
- The CMK constraint is operationally significant: **CMK must be elected at create time and is immutable for the server's lifetime**. [7] Provision the production server CMK-on from day one; you cannot retrofit.
- pgaudit param quirk: Azure does **not** accept the `-` (minus) shortcut in `pgaudit.log`; enumerate each statement class. Also, a major-version upgrade **drops and recreates** pgaudit and does *not* preserve `pgaudit.log` settings — re-apply them post-upgrade. [9]

### GCP Cloud SQL
- BAA covers Cloud SQL explicitly; the covered-products page was **Last Updated 2026-05-15**. [11]
- Most current version line of the five: **PG 18 is the default**, 17 GA. [15]
- TLS enforcement is via the instance `ssl_mode` (`ENCRYPTED_ONLY`), not a Postgres GUC. [13]

### Crunchy Bridge
- Closest to "stock PostgreSQL" semantics — fewest extension/role surprises during a 500-table migration, and pgAudit is on by default for compliance roles. [17]
- HIPAA is a **contact-Crunchy/BAA-on-request** path. SOC 2 Type 2 complete. CMK uses the underlying cloud KMS. [16] Confirm the exact supported PG **minor** at provisioning (the security page does not pin a version).

### Aiven
- pgaudit and forced TLS are fine, but **CMK/BYOK is a limited-availability feature requiring an access request** — it is *not* a self-serve checkbox like AWS KMS / Azure Key Vault / Cloud KMS. [20] This is the one material gap against the "pgaudit + CMK + forced TLS in one SKU, today" requirement. PG 17 is the new-service default. [23]

---

## Recommendation

### Primary: **AWS RDS for PostgreSQL** (Multi-AZ, PostgreSQL 17.x, KMS CMK, `rds.force_ssl=1`, pgaudit enabled)

Rationale: RDS for PostgreSQL is the only evaluated SKU where all three hard requirements — **pgaudit** (`rds_superuser` + custom parameter group), **customer-managed keys** (AWS KMS, self-serve and GA), and **forced TLS** (`rds.force_ssl`, already the default on PG 15+) — are first-class, generally available, single-SKU, and self-serve, with **no "contact us / limited-availability" gate**. RDS-for-PostgreSQL is explicitly named in the AWS HIPAA-eligible-services list (BAA via AWS Artifact), PG 17.10/18.4 are current, and the constrained-`rds_superuser`/extension-allow-list model is well-documented and well-trodden for migrations off SQL Server. For a ~500-table PHI billing workload on .NET 8/10 + Npgsql, this is the lowest-risk path to "compliant on day one." Use **Aurora PostgreSQL** instead of vanilla RDS only if the read-scaling/failover profile justifies it — it carries the same BAA/KMS/force_ssl/pgaudit story.

### Fallback: **Azure Database for PostgreSQL — Flexible Server** (CMK elected at create, `require_secure_transport=on`, pgaudit allowlisted)

Rationale: Azure matches all three requirements (Key Vault CMK, `require_secure_transport`, pgaudit) and has the cleanest **BAA-by-default** posture (no separate signature; baked into Product Terms/DPA). Two operational gotchas keep it as fallback rather than primary: **CMK is immutable after server create** (must be set on day one), and **major-version upgrades drop/recreate pgaudit and lose `pgaudit.log` settings** (must be re-applied). If the org is Azure-first / already has an EA, Flexible Server is an entirely viable primary. GCP Cloud SQL is an equally-capable third option (PG 18 default, CMEK, `ssl_mode=ENCRYPTED_ONLY`, pgaudit flag) if the org is GCP-aligned. **Crunchy Bridge** is the strongest "stock Postgres" boutique option (pgAudit-on-by-default, KMS-backed CMK, TLS 1.2+ enforced) where HIPAA is a BAA-on-request path. **Aiven is *not* recommended for this workload** as-is, solely because **CMK/BYOK is limited-availability/request-gated** rather than self-serve — it cannot meet "pgaudit + CMK + forced TLS in one self-serve SKU, today" without a special access grant.

---

## Npgsql TLS connection-string settings (TLS-required)

For a forced-TLS managed Postgres with full server-identity verification (recommended for PHI), the Npgsql (3.2+/8.x) connection string should be:

```
Host=<endpoint>;Port=5432;Database=<db>;Username=<user>;Password=<secret>;
SSL Mode=VerifyFull;
Root Certificate=<path-to-provider-CA-bundle.pem>;
Channel Binding=Require;
Trust Server Certificate=false
```

Settings explained / how they map to each provider's CA:

- **`SSL Mode=VerifyFull`** — encrypts *and* verifies the server certificate chain **and** that the hostname matches the cert (defends against MITM). `Require` (encrypt only, no cert validation) is the minimum to satisfy a forced-TLS server but does **not** validate identity — for PHI use `VerifyFull`. (`VerifyCA` validates the chain but not the hostname — acceptable only if the endpoint hostname isn't in the cert.)
- **`Trust Server Certificate=false`** — must be `false` for `VerifyFull`/`VerifyCA` to actually validate. Setting it `true` disables validation and downgrades you to encrypt-only; do **not** use `true` in production.
- **`Root Certificate=<...pem>`** — points Npgsql at the provider's CA bundle used to validate the server cert. Per provider:
  - **AWS RDS / Aurora**: the Amazon RDS regional/global CA bundle (e.g. `rds-ca-rsa2048-g1` / global `global-bundle.pem`).
  - **Azure Flexible Server**: the DigiCert Global Root CA chain Azure uses for Flexible Server TLS.
  - **GCP Cloud SQL**: the per-instance **server CA** downloaded from the instance (Cloud SQL → Connections → server certificate).
  - **Crunchy Bridge**: connect with the provided connection string; their cert chains to a public CA (TLS 1.2+ enforced).
  - **Aiven**: the **Aiven project CA certificate** (required for `verify-ca`/`verify-full`), downloaded from the Aiven console/API. Map Aiven `verify-full` → Npgsql `VerifyFull`.
- **`Channel Binding=Require`** — enforces SCRAM channel binding (binds the SCRAM auth to the TLS channel; extra MITM hardening). Use `Require` where the server supports it (modern PG + SCRAM, all five do); fall back to `Prefer` if a provider/version rejects `Require`.

> Note: as a defense-in-depth backstop, the server-side force knob (`rds.force_ssl=1` / `require_secure_transport=on` / Cloud SQL `ssl_mode=ENCRYPTED_ONLY`) should also be on — so a misconfigured client that omits SSL Mode is still rejected by the server, not silently allowed in cleartext.

---

## Sources (accessed 2026-06-16)

- [1] AWS HIPAA Eligible Services Reference (Last Updated 2026-05-22): https://aws.amazon.com/compliance/hipaa-eligible-services-reference/
- [2] Amazon RDS Security features (encryption at rest / KMS): https://aws.amazon.com/rds/features/security/
- [3] AWS What's New — RDS for PostgreSQL minors 18.4/17.10/16.14/15.18/14.23 (2026-05): https://aws.amazon.com/about-aws/whats-new/2026/05/amazon-rds-postgresql/ ; major 18 GA: https://aws.amazon.com/about-aws/whats-new/2025/11/amazon-rds-postgresql-major-version-18
- [3a] Amazon Aurora PostgreSQL updates / release notes: https://docs.aws.amazon.com/AmazonRDS/latest/AuroraPostgreSQLReleaseNotes/AuroraPostgreSQL.Updates.html
- [4] Using SSL/TLS with an RDS PostgreSQL DB instance (`rds.force_ssl`, default 1 on PG15+): https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/PostgreSQL.Concepts.General.SSL.html
- [5] Setting up the pgAudit extension (RDS) + rds_superuser role: https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.PostgreSQL.CommonDBATasks.pgaudit.basic-setup.html ; https://docs.aws.amazon.com/AmazonRDS/latest/AuroraUserGuide/Appendix.PostgreSQL.CommonDBATasks.Roles.rds_superuser.html
- [6] Azure HIPAA compliance offering — BAA via Product Terms, no separate signature, in-scope services: https://learn.microsoft.com/en-us/azure/compliance/offerings/offering-hipaa-us ; BAA: https://aka.ms/baa ; Product Terms: https://www.microsoft.com/licensing/docs/view/Product-Terms
- [7] Azure Database for PostgreSQL — Data encryption / CMK (Key Vault, immutable at create): https://learn.microsoft.com/en-us/azure/postgresql/security/security-data-encryption
- [8] Azure Flexible Server security concepts — `require_secure_transport` (transport encryption): https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-security
- [9] Azure Flexible Server audit logging via pgaudit (allowlist/load/create, param quirks): https://learn.microsoft.com/en-us/azure/postgresql/flexible-server/concepts-audit
- [10] Azure supported PostgreSQL versions (17.x + in-place upgrade): https://learn.microsoft.com/en-us/azure/postgresql/configure-maintain/concepts-supported-versions
- [11] Google Cloud HIPAA compliance — BAA covers Cloud SQL (Last Updated 2026-05-15): https://cloud.google.com/security/compliance/hipaa ; BAA terms: https://cloud.google.com/terms/hipaa-baa
- [12] Cloud SQL for PostgreSQL — CMEK / Cloud KMS: https://cloud.google.com/sql/docs/postgres/cmek ; https://cloud.google.com/sql/docs/postgres/configure-cmek
- [13] Cloud SQL — SSL/TLS enforcement (`ssl_mode=ENCRYPTED_ONLY`): https://cloud.google.com/sql/docs/postgres/configure-ssl-instance
- [13a] Cloud SQL — superuser restrictions / known issues: https://docs.cloud.google.com/sql/docs/postgres/users (managed superuser model) ; https://docs.cloud.google.com/sql/docs/postgres/known-issues
- [14] Cloud SQL — pgAudit (`cloudsql.enable_pgaudit`): https://docs.cloud.google.com/sql/docs/postgres/pg-audit
- [15] Cloud SQL for PostgreSQL supported DB versions (18 default, 17 GA): https://docs.cloud.google.com/sql/docs/postgres/db-versions ; PG17 launch: https://cloud.google.com/blog/products/databases/postgresql-17-now-available-on-cloud-sql/
- [16] Crunchy Bridge — Security (HIPAA BAA on request, AES-256, CMK via cloud KMS, TLS 1.2+): https://docs.crunchybridge.com/concepts/security
- [17] Crunchy Bridge — Security & team management (pgAudit auditing): https://www.crunchydata.com/blog/security-and-team-management-in-crunchy-bridge
- [18] Aiven — HIPAA compliant (BAA on request; compliant since 2018, pub. 2019-05-13): https://aiven.io/blog/aiven-is-hipaa-compliant ; security/compliance overview: https://aiven.io/security-compliance
- [19] Aiven — encryption (disk/backup/replication encrypted): https://aiven.io/postgresql
- [20] Aiven — Bring Your Own Key (BYOK) is limited availability, request access; RSA-2048/4096 in AWS/Azure/GCP/OCI KMS: https://aiven.io/docs/platform/howto/bring-your-own-key
- [21] Aiven — TLS/SSL certificates (project CA for verify-ca/verify-full): https://aiven.io/docs/platform/concepts/tls-ssl-certificates ; deprecated TLS removed: https://aiven.io/docs/products/postgresql/reference/use-of-deprecated-tls-versions
- [22] Aiven — pgaudit logging: https://aiven.io/docs/products/postgresql/howto/list-pgaudit
- [23] Aiven — PostgreSQL 17 now default for new services / 18.x minors: https://aiven.io/changelog/1567c641-ac7e-4801-85a5-4a15ccc3653f ; https://aiven.io/changelog/65b55699-6536-465c-a655-8b7c544053a5
