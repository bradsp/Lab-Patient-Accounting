/*───────────────────────────────────────────────────────────────────────────
  xe-runtime-surface-session.sql
  Phase 21-02 — Runtime DB-object surface trace (capture side)

  PURPOSE
    Capture which in-database MODULES (stored procedures, scalar/multi-statement
    UDFs, triggers) the live application actually executes during a representative
    workload, so Phase 23 can port the runtime-critical procedural surface first
    and defer the batch/reporting surface.

  HIPAA — IDENTITY ONLY, NO PHI
    This session captures the `module_start` event, whose payload is object
    IDENTITY only (object_id / object_name / object_type). It deliberately does
    NOT add the `sql_text` or any `statement` action, so NO query text and NO
    parameter values (which could contain PHI) are ever written. Do not add
    sql_text/statement actions to this session.

  COVERAGE NOTE
    `module_start` fires for procedures, scalar functions, multi-statement TVFs,
    and triggers — i.e. the procedural surface that is the real porting cost.
    VIEWS and INLINE TVFs are expanded into the calling query and do NOT raise
    module_start; view usage is therefore enumerated separately via static
    reference analysis (repositories/services SQL), not by this trace.

  HOW TO RUN (representative window)
    1. Run section [1] to create + start the session (scoped to LabBillingTest).
    2. Exercise a full business cycle against this DB: log into LabOutreachUI and
       do account search/load/edit, charge entry, payment posting, claim
       generation, HL7 ADT/DFT/MFN ingestion; run the LabBillingService and at
       least one Quartz scheduler cycle. The richer the data + paths, the better.
    3. Run section [3] to stop the session.
    4. Run xe-analyze-captured-objects.sql to produce executed-vs-deferrable lists
       and export captured-runtime-objects.csv.
    5. Run section [4] to drop the session when done.

  TARGET
    event_file inside the container at /var/opt/mssql/xe/ (writable by the mssql
    service account). For a short ad-hoc capture you may instead use the
    ring_buffer target (see commented block) — no filesystem path needed.

  CONTAINER RUN EXAMPLE (PowerShell, from host)
    docker exec mssql_server /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa `
      -P '<sa-pw>' -C -i /path/in/container/xe-runtime-surface-session.sql
───────────────────────────────────────────────────────────────────────────*/

------------------------------------------------------------------------------
-- [1] CREATE + START  (run once at the start of the capture window)
------------------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM sys.server_event_sessions WHERE name = N'RuntimeSurface')
    DROP EVENT SESSION [RuntimeSurface] ON SERVER;
GO

CREATE EVENT SESSION [RuntimeSurface] ON SERVER
ADD EVENT sqlserver.module_start
(
    -- object identity only; scope to the application database
    ACTION (sqlserver.database_name)
    WHERE  sqlserver.database_name = N'LabBillingTest'
)
ADD TARGET package0.event_file
(
    SET filename         = N'/var/opt/mssql/xe/RuntimeSurface.xel',
        max_file_size    = 50,   -- MB per rollover file
        max_rollover_files = 4
)
WITH
(
    MAX_DISPATCH_LATENCY = 5 SECONDS,
    TRACK_CAUSALITY      = OFF,
    STARTUP_STATE        = OFF
);
GO

ALTER EVENT SESSION [RuntimeSurface] ON SERVER STATE = START;
GO
PRINT 'RuntimeSurface session STARTED. Exercise the application now, then run section [3].';
GO

/*  ── Alternative in-memory target (use INSTEAD of the event_file above for a
    short capture with no filesystem path) ──
ADD TARGET package0.ring_buffer (SET max_memory = 8192)   -- 8 MB
*/

------------------------------------------------------------------------------
-- [2] LIVE PEEK  (optional — distinct modules seen so far, while running)
------------------------------------------------------------------------------
-- See xe-analyze-captured-objects.sql for the full read/analysis.

------------------------------------------------------------------------------
-- [3] STOP  (run at the end of the capture window)
------------------------------------------------------------------------------
-- ALTER EVENT SESSION [RuntimeSurface] ON SERVER STATE = STOP;
-- GO
-- PRINT 'RuntimeSurface session STOPPED. Run xe-analyze-captured-objects.sql.';
-- GO

------------------------------------------------------------------------------
-- [4] DROP  (run once the capture is exported/analyzed)
------------------------------------------------------------------------------
-- IF EXISTS (SELECT 1 FROM sys.server_event_sessions WHERE name = N'RuntimeSurface')
--     DROP EVENT SESSION [RuntimeSurface] ON SERVER;
-- GO
