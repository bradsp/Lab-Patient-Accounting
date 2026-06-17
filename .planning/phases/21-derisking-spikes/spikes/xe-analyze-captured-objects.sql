/*───────────────────────────────────────────────────────────────────────────
  xe-analyze-captured-objects.sql
  Phase 21-02 — Runtime DB-object surface trace (analysis side)

  PURPOSE
    Read the RuntimeSurface capture, resolve the distinct modules that actually
    executed, and LEFT JOIN them against the full static catalog of LabBillingTest
    to produce two lists:
      (A) EXECUTED AT RUNTIME      — procs/UDFs/triggers the live app called
      (B) PRESENT BUT NEVER SEEN   — catalog objects never observed (deferrable)

  PREREQUISITE
    Run against the SAME database the session was scoped to (LabBillingTest).
    The capture only records object IDENTITY (no statement text / no PHI).

  NOTE ON XML
    The .xel read below shreds the event XML purely to extract object_name —
    this is event metadata, not application data; no PHI is involved.

  EXPORT
    Result set (D) is the deduplicated executed-vs-deferrable catalog. Export it
    to .planning/phases/21-derisking-spikes/spikes/captured-runtime-objects.csv
    e.g.:
      docker exec mssql_server /opt/mssql-tools18/bin/sqlcmd -S localhost \
        -U sysapp -P '<pw>' -d LabBillingTest -C -s "," -W -h -1 \
        -Q "<paste section [D] query>" > captured-runtime-objects.csv

  VIEWS
    module_start does not fire for views/inline-TVFs. Enumerate view usage from
    the .NET layer separately (repositories/services SQL) — see the findings doc.
───────────────────────────────────────────────────────────────────────────*/

SET NOCOUNT ON;

------------------------------------------------------------------------------
-- [A] Distinct modules captured (from the event_file target)
------------------------------------------------------------------------------
-- Adjust the path glob if rollover files were produced (RuntimeSurface*.xel).
IF OBJECT_ID('tempdb..#executed') IS NOT NULL DROP TABLE #executed;

;WITH raw AS
(
    SELECT CAST(event_data AS XML) AS x
    FROM   sys.fn_xe_file_target_read_file('/var/opt/mssql/xe/RuntimeSurface*.xel', NULL, NULL, NULL)
)
SELECT DISTINCT
       x.value('(event/data[@name="object_name"]/value)[1]', 'sysname')      AS object_name,
       x.value('(event/data[@name="object_type"]/text)[1]',  'varchar(30)')  AS object_type_captured
INTO   #executed
FROM   raw
WHERE  x.value('(event/data[@name="object_name"]/value)[1]', 'sysname') IS NOT NULL;

/*  ── If you used the ring_buffer target instead, replace the CTE above with: ──
;WITH raw AS
(
    SELECT CAST(t.target_data AS XML) AS x
    FROM   sys.dm_xe_sessions s
    JOIN   sys.dm_xe_session_targets t ON t.event_session_address = s.address
    WHERE  s.name = N'RuntimeSurface' AND t.target_name = 'ring_buffer'
)
SELECT DISTINCT
       n.value('(data[@name="object_name"]/value)[1]', 'sysname')     AS object_name,
       n.value('(data[@name="object_type"]/text)[1]', 'varchar(30)')  AS object_type_captured
INTO #executed
FROM raw CROSS APPLY x.nodes('//RingBufferTarget/event') AS q(n)
WHERE n.value('(data[@name="object_name"]/value)[1]', 'sysname') IS NOT NULL;
*/

------------------------------------------------------------------------------
-- [B] Full static catalog of callable modules (procs, scalar/TVF UDFs, triggers)
------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#catalog') IS NOT NULL DROP TABLE #catalog;

SELECT  s.name  AS [schema_name],
        o.name  AS object_name,
        CASE o.type
             WHEN 'P'  THEN 'SQL_STORED_PROCEDURE'
             WHEN 'FN' THEN 'SQL_SCALAR_FUNCTION'
             WHEN 'IF' THEN 'SQL_INLINE_TABLE_VALUED_FUNCTION'
             WHEN 'TF' THEN 'SQL_TABLE_VALUED_FUNCTION'
             WHEN 'TR' THEN 'SQL_TRIGGER'
             ELSE o.type_desc
        END     AS object_type
INTO    #catalog
FROM    sys.objects o
JOIN    sys.schemas s ON s.schema_id = o.schema_id
WHERE   o.type IN ('P','FN','IF','TF','TR')
        AND o.is_ms_shipped = 0;

------------------------------------------------------------------------------
-- [C] Headline counts
------------------------------------------------------------------------------
SELECT 'catalog_modules'      AS metric, COUNT(*) AS value FROM #catalog
UNION ALL
SELECT 'executed_distinct',          COUNT(*)        FROM #executed
UNION ALL
SELECT 'runtime_critical_in_catalog',
       COUNT(*) FROM #catalog c
       WHERE EXISTS (SELECT 1 FROM #executed e WHERE e.object_name = c.object_name)
UNION ALL
SELECT 'deferrable_never_executed',
       COUNT(*) FROM #catalog c
       WHERE NOT EXISTS (SELECT 1 FROM #executed e WHERE e.object_name = c.object_name);

------------------------------------------------------------------------------
-- [D] EXPORT SET — every catalog module flagged executed vs deferrable
--     (this is the captured-runtime-objects.csv payload)
------------------------------------------------------------------------------
SELECT  c.[schema_name],
        c.object_name,
        c.object_type,
        CASE WHEN e.object_name IS NOT NULL
             THEN 'RUNTIME_CRITICAL' ELSE 'DEFERRABLE' END AS classification
FROM    #catalog c
LEFT JOIN #executed e ON e.object_name = c.object_name
ORDER BY classification, c.object_type, c.[schema_name], c.object_name;

------------------------------------------------------------------------------
-- [E] Anything executed but NOT in the catalog (sanity — should be empty;
--     non-empty would indicate cross-DB calls or ms-shipped helpers)
------------------------------------------------------------------------------
SELECT e.object_name, e.object_type_captured
FROM   #executed e
WHERE  NOT EXISTS (SELECT 1 FROM #catalog c WHERE c.object_name = e.object_name);
