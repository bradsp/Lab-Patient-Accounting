/* ============================================================================
   REFERENCE T-SQL: HL7 XML-shred behavior to match (Phase 21-03 bake-off)
   ----------------------------------------------------------------------------
   Source 1: SQL Agent job "PROD Daily AM Run", step "CERNER DAILY CDM NOT IN
             LEGACY BILLING"  ->  Lab Patient Accounting SQL Agent Jobs.sql:483-655
   Source 2: usp_prg_Xml_Import.sql (the postingbatch/encounter charge shred is the
             same family; the CDM step below is the cleaner, focused representative
             that the plan calls out).

   This file is the REFERENCE behavior the two prototypes must reproduce. It is
   NOT meant to be executed standalone; it is the verbatim shredding core lifted
   from the agent step, lightly trimmed to one representative shred (charge/CDM
   element set) and de-escaped (the agent stores it as an @command N'...' string
   with doubled single-quotes; those are unescaped here for readability).

   The HL7 payload is stored as an XML envelope in infce.messages_inbound.msgContent.
   Element names are the dotted HL7 field/component paths (PV1.3.4, PID.18.1,
   FT1.1.1, FT1.7.1, FT1.7.2, FT1.10.1). The SQL Server XML methods used are:
     .nodes()      -> CROSS APPLY to expand the HL7Message and the repeating FT1 nodes
     .value()      -> project a scalar from an XPath
     .query()      -> carry the HL7Message subtree forward between CTEs
   plus a FOR XML PATH('tr') used only to build the HTML email body (NOT part of the
   data shred; reproduced at the bottom for completeness but excluded from parity).
   ============================================================================ */


/* ---------------------------------------------------------------------------
   STEP 1 - Pull the raw message and cast msgContent to XML
   (the real step buffers into a #temp table first; collapsed here)
   --------------------------------------------------------------------------- */
; WITH cteTemp AS
(
    SELECT  mi.systemMsgId                                              AS systemMsgId,
            CAST(REPLACE(mi.msgContent, '"', '') AS XML)               AS xmlContent
    FROM    infce.messages_inbound mi
    WHERE   mi.msgType = 'DFT-P03'
      AND   mi.msgDate BETWEEN @startDate AND @endDate
)

/* ---------------------------------------------------------------------------
   STEP 2 - cteData: shred the header-level CLIENT and ACCOUNT scalars off each
   HL7Message node, and carry the HL7Message subtree forward via .query()
   --------------------------------------------------------------------------- */
, cteData AS
(
    SELECT  r.systemMsgId,
            COALESCE(NULLIF(dbo.GetMappingValue('CLIENT','CERNER',
                COALESCE(
                    NULLIF(LTRIM(a.alias.value('(//PV1.3.4/text())[1]', 'varchar(50)')),''),
                    NULLIF(LTRIM(a.alias.value('(//PV1.3.1/text())[1]', 'varchar(50)')),''),
                    NULLIF(LTRIM(a.alias.value('(//PV1.3.7/text())[1]', 'varchar(50)')),''),
                    NULLIF(LTRIM(a.alias.value('(//PV1.6.4/text())[1]', 'varchar(50)')),''),
                    NULLIF(LTRIM(a.alias.value('(//PV1.6.1/text())[1]', 'varchar(50)')),''),
                    NULLIF(LTRIM(a.alias.value('(//PV1.6.7/text())[1]', 'varchar(50)')),'')
                )),'K')
                , a.alias.value('(//PV1.3.4/text())[1]', 'varchar(50)'))            AS [CLIENT],
            'L' + a.alias.value('(//PID.18.1/text())[1]', 'varchar(15)')            AS [ACCOUNT],
            a.alias.query('//HL7Message')                                          AS xmlContent
    FROM    (SELECT cteTemp.systemMsgId, xmlContent AS rep_xml FROM cteTemp) r
    CROSS APPLY r.rep_xml.nodes('HL7Message') a(alias)
)

/* ---------------------------------------------------------------------------
   STEP 3 - xCDM: shred the repeating FT1 charge lines into one row per charge.
   THIS IS THE CORE DATA SHRED the prototypes must reproduce.
   --------------------------------------------------------------------------- */
, xCDM AS
(
    SELECT
        'L' + a.alias.value('(//PID.18.1/text())[1]', 'varchar(15)')     AS [ACCOUNT],
        ft1.cdm.value('(./FT1.1/FT1.1.1/text())[1]',  'int')             AS [SET ID],
        ft1.cdm.value('(./FT1.7/FT1.7.1/text())[1]',  'varchar(7)')      AS [CDM],
        ft1.cdm.value('(./FT1.10/FT1.10.1/text())[1]','int')             AS [CDM QTY],
        ft1.cdm.value('(./FT1.7/FT1.7.2/text())[1]',  'varchar(50)')     AS [CERNER DESCRIPTION]
    FROM   (SELECT xmlContent AS rep_xml FROM cteData) r
    CROSS APPLY r.rep_xml.nodes('HL7Message')  a(alias)
    CROSS APPLY r.rep_xml.nodes('//FT1')       ft1(cdm)
)

/* ---------------------------------------------------------------------------
   STEP 4 - join shredded charges to the CDM dictionary and aggregate. The agent
   step keeps only the charges whose CDM is NOT in the cdm dictionary (the
   "not in legacy billing" report), summing QTY per (account, cdm).
   --------------------------------------------------------------------------- */
SELECT  xCDM.ACCOUNT,
        xCDM.CDM,
        ISNULL(cdm.descript, xCDM.CDM + ' Not in Billing')   AS [DESCRIPTION],
        SUM(xCDM.[CDM QTY])                                  AS [QTY],
        CASE WHEN cdm.descript IS NULL
             THEN xCDM.[CERNER DESCRIPTION] ELSE '' END      AS [CERNER DESCRIPTION]
FROM    xCDM
LEFT OUTER JOIN cdm ON dbo.cdm.cdm = xCDM.CDM
WHERE   cdm.descript IS NULL                 -- "not in legacy billing" filter
GROUP BY xCDM.ACCOUNT, xCDM.CDM, cdm.descript, xCDM.[CERNER DESCRIPTION]
ORDER BY xCDM.CDM;


/* ===========================================================================
   EXPECTED SHREDDED OUTPUT (parity target for the prototypes)
   ===========================================================================

   Given sample-message.xml (one HL7Message, account 9000001234, client CLNT01,
   three FT1 charge lines), the CORE SHRED (xCDM CTE, STEP 3) produces exactly:

     ACCOUNT      | SET ID | CDM     | CDM QTY | CERNER DESCRIPTION
     -------------+--------+---------+---------+----------------------
     L9000001234  |   1    | 8000010 |   1     | SYNTHETIC PANEL A
     L9000001234  |   2    | 8000020 |   2     | SYNTHETIC TEST B
     L9000001234  |   3    | 8000099 |   1     | SYNTHETIC UNMAPPED C

   And the header scalars (cteData, STEP 2) produce:
     systemMsgId | CLIENT | ACCOUNT
     ------------+--------+-------------
     (msg id)    | CLNT01 | L9000001234

   This 5-column charge projection (ACCOUNT, SET ID, CDM, CDM QTY, CERNER
   DESCRIPTION) is the agreed PARITY COLUMN SET for the bake-off. The dictionary
   join / SUM / "not in billing" filter (STEP 4) and the FOR XML PATH email
   rendering are downstream of the shred and are intentionally OUT OF SCOPE for
   the parity comparison (they are ordinary relational SQL / presentation, not
   XML shredding). With the synthetic dictionary assumption that 8000010/8000020
   exist and 8000099 does not, STEP 4 would emit a single report row:
     L9000001234 | 8000099 | "8000099 Not in Billing" | 1 | SYNTHETIC UNMAPPED C
   =========================================================================== */


/* ---------------------------------------------------------------------------
   (reference only - NOT part of the data-shred parity) HTML email rendering.
   The agent step wraps the STEP 4 result in FOR XML PATH('tr') to build an HTML
   table body for sp_send_dbmail. On PostgreSQL this becomes app-tier rendering
   (Quartz job + SMTP), not an XML method. Shown to document the full original.
   --------------------------------------------------------------------------- */
-- SELECT @tableHtml =
--   N'<table>...' +
--   CAST((SELECT td = ROW_NUMBER() OVER (ORDER BY xCDM.ACCOUNT, xCDM.CDM), '',
--                td = xCDM.ACCOUNT, '', td = xCDM.CDM, '', ...
--         FROM xCDM ...
--         FOR XML PATH('tr'), TYPE) AS NVARCHAR(MAX)) + N'</table>';
