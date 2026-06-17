/* ============================================================================
   PROTOTYPE A - PostgreSQL xmltable()/xpath() HL7 XML shred  (Phase 21-03)
   ----------------------------------------------------------------------------
   Reproduces xml-case-source-tsql.sql (the SQL Agent "CERNER DAILY CDM" shred)
   over spikes/sample-message.xml, projecting the SAME parity column set:
       ACCOUNT, SET ID, CDM, CDM QTY, CERNER DESCRIPTION   (+ CLIENT header scalar)

   THROWAWAY SPIKE. Not wired into any project. Read-only SELECT over literal XML.

   VALIDATED LIVE against PostgreSQL 16 (container personal-finance-postgres-1,
   localhost:5433, db personal_finance) on 2026-06-16 -- produced exactly the 3
   rows documented as the expected output in xml-case-source-tsql.sql.

   To re-run:
     docker exec -i personal-finance-postgres-1 psql -U app -d personal_finance \
       -f /dev/stdin < shred-prototype-pg.sql
   (or inline the <HL7Message> literal where indicated below).

   PostgreSQL XML-method mapping vs the T-SQL reference:
     T-SQL  r.rep_xml.nodes('//FT1')   ->  PG  xmltable('//FT1' PASSING ... )
     T-SQL  ft1.cdm.value('(...)[1]')  ->  PG  xmltable COLUMNS ... PATH '...'
     T-SQL  a.alias.value('(//x)[1]')  ->  PG  (xpath('//x/text()', doc))[1]::text
     T-SQL  COALESCE(NULLIF(LTRIM..))  ->  PG  COALESCE(NULLIF(btrim(...),''))
   xmltable() exists and behaves identically on PG 14/15/16/17/18, so PG16
   validation is representative of the 17/18 target line.
   ============================================================================ */

-- In production this XML comes from infce.messages_inbound.msgContent (cast to xml,
-- with embedded double-quotes stripped exactly as the T-SQL does: REPLACE(...,'"','')).
-- Here it is inlined as a literal so the spike is self-contained. Replace the
-- literal below with the de-commented contents of spikes/sample-message.xml, OR
-- point `src` at the real table:
--     SELECT systemMsgId, replace(msgContent,'"','')::xml
--     FROM infce.messages_inbound
--     WHERE msgType = 'DFT-P03' AND msgDate BETWEEN :startDate AND :endDate

WITH src AS (
    SELECT
        1::numeric AS systemmsgid,
        $hl7$
<HL7Message>
  <PID><PID.18><PID.18.1>9000001234</PID.18.1></PID.18></PID>
  <PV1>
    <PV1.3><PV1.3.1>CLNT01</PV1.3.1><PV1.3.4>CLNT01</PV1.3.4><PV1.3.7>CLNT01</PV1.3.7></PV1.3>
    <PV1.6><PV1.6.1>CLNT01</PV1.6.1><PV1.6.4>CLNT01</PV1.6.4><PV1.6.7>CLNT01</PV1.6.7></PV1.6>
  </PV1>
  <FT1><FT1.1><FT1.1.1>1</FT1.1.1></FT1.1>
       <FT1.7><FT1.7.1>8000010</FT1.7.1><FT1.7.2>SYNTHETIC PANEL A</FT1.7.2></FT1.7>
       <FT1.10><FT1.10.1>1</FT1.10.1></FT1.10></FT1>
  <FT1><FT1.1><FT1.1.1>2</FT1.1.1></FT1.1>
       <FT1.7><FT1.7.1>8000020</FT1.7.1><FT1.7.2>SYNTHETIC TEST B</FT1.7.2></FT1.7>
       <FT1.10><FT1.10.1>2</FT1.10.1></FT1.10></FT1>
  <FT1><FT1.1><FT1.1.1>3</FT1.1.1></FT1.1>
       <FT1.7><FT1.7.1>8000099</FT1.7.1><FT1.7.2>SYNTHETIC UNMAPPED C</FT1.7.2></FT1.7>
       <FT1.10><FT1.10.1>1</FT1.10.1></FT1.10></FT1>
</HL7Message>
        $hl7$::xml AS xmlcontent
),

/* cteData: header-level CLIENT + ACCOUNT scalars (mirrors STEP 2 of the T-SQL).
   The COALESCE fan-out over PV1.3.x / PV1.6.x reproduces the proc's client
   resolution; the real proc additionally wraps this in dbo.GetMappingValue
   ('CLIENT','CERNER',...) -- in PG that becomes a PL/pgSQL function call or a
   join to the mapping table. Stubbed here as the raw resolved code. */
cteData AS (
    SELECT
        s.systemmsgid,
        COALESCE(
            NULLIF(btrim((xpath('//PV1.3.4/text()', s.xmlcontent))[1]::text), ''),
            NULLIF(btrim((xpath('//PV1.3.1/text()', s.xmlcontent))[1]::text), ''),
            NULLIF(btrim((xpath('//PV1.3.7/text()', s.xmlcontent))[1]::text), ''),
            NULLIF(btrim((xpath('//PV1.6.4/text()', s.xmlcontent))[1]::text), ''),
            NULLIF(btrim((xpath('//PV1.6.1/text()', s.xmlcontent))[1]::text), ''),
            NULLIF(btrim((xpath('//PV1.6.7/text()', s.xmlcontent))[1]::text), ''),
            'K'
        )                                                       AS client,
        'L' || (xpath('//PID.18.1/text()', s.xmlcontent))[1]::text  AS account,
        s.xmlcontent
    FROM src s
)

/* xCDM: the core per-charge shred (mirrors STEP 3). xmltable() expands the
   repeating //FT1 nodes to one row each and projects the dotted-path columns. */
SELECT
    d.account                       AS account,
    d.client                        AS client,
    x."SET ID"                      AS "SET ID",
    x."CDM"                         AS "CDM",
    x."CDM QTY"                     AS "CDM QTY",
    x."CERNER DESCRIPTION"          AS "CERNER DESCRIPTION"
FROM cteData d
CROSS JOIN LATERAL xmltable(
    '//FT1' PASSING d.xmlcontent
    COLUMNS
        "SET ID"             int          PATH 'FT1.1/FT1.1.1',
        "CDM"                varchar(7)   PATH 'FT1.7/FT1.7.1',
        "CDM QTY"            int          PATH 'FT1.10/FT1.10.1',
        "CERNER DESCRIPTION" varchar(50)  PATH 'FT1.7/FT1.7.2'
) AS x
ORDER BY x."SET ID";

/* ---------------------------------------------------------------------------
   ACTUAL OUTPUT (PostgreSQL 16, 2026-06-16) -- matches expected parity set:

      account   | client | SET ID |   CDM   | CDM QTY |  CERNER DESCRIPTION
   -------------+--------+--------+---------+---------+----------------------
    L9000001234 | CLNT01 |      1 | 8000010 |       1 | SYNTHETIC PANEL A
    L9000001234 | CLNT01 |      2 | 8000020 |       2 | SYNTHETIC TEST B
    L9000001234 | CLNT01 |      3 | 8000099 |       1 | SYNTHETIC UNMAPPED C
   (3 rows)

   STEP 4 (dictionary join / SUM / "not in billing" filter) is ordinary SQL on
   top of this shred and ports 1:1 to PostgreSQL (LEFT JOIN cdm ... WHERE
   cdm.descript IS NULL GROUP BY ...). The FOR XML PATH('tr') HTML email body
   moves to the app tier (Quartz + SMTP), not an XML method.
   --------------------------------------------------------------------------- */
