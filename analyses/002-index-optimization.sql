/*
================================================================================
 Index Optimization Script for Lab Patient Accounting Database
================================================================================
 Generated: 2026-02-03
 Purpose: Implements index optimizations identified through cross-referencing
          the database schema with actual application query patterns.

 Target: SQL Server 2014+
 Database: Lab Patient Accounting (LabBilling)

 IMPORTANT NOTES:
 - Each operation is independent and can be run incrementally.
 - All DROP statements include IF EXISTS checks.
 - All CREATE statements include IF NOT EXISTS checks.
 - Suboptimal index fixes require DROP + CREATE (cannot ALTER INCLUDE columns).
 - Test in a non-production environment first.
 - Monitor query plan changes after each modification.
 - Consider running during a maintenance window for large tables (chrg).

 Priority Order:
   1. Create missing index for HL7 message processing (HIGH impact)
   2. Enhance chrg.account_index with additional INCLUDE columns (MEDIUM impact)
   3. Create missing index for billing history date range queries (MEDIUM impact)
   4. Enhance chrg.credited_account_status_Includes with additional INCLUDEs (MEDIUM impact)
   5. Create missing index for message date range queries (LOW-MEDIUM impact)
   6. Drop redundant index on acc_location (LOW impact)
================================================================================
*/

SET NOCOUNT ON;
GO

PRINT '=== Lab Patient Accounting Index Optimization Script ===';
PRINT 'Started: ' + CONVERT(varchar(30), GETDATE(), 121);
GO

--------------------------------------------------------------------------------
-- SECTION 1: CREATE MISSING INDEXES
--------------------------------------------------------------------------------

PRINT '';
PRINT '--- Section 1: Creating Missing Indexes ---';
GO

/*
 Priority 1: infce.messages_inbound - Index for GetUnprocessedMessages()

 Rationale: The HL7ProcessorService.ProcessMessages() method runs continuously
 as a background service, calling GetUnprocessedMessages() which filters on
 processFlag = 'N' ORDER BY msgDate. The existing IX_PROC_DFT index has
 processFlag as the 2nd column (after msgType), requiring an index scan
 across all msgType values. A dedicated index with processFlag as the leading
 column will convert this to an efficient index seek.

 Query pattern: WHERE processFlag = 'N' ORDER BY msgDate
 Expected impact: HIGH - This is the most frequently executed query in the
 background processing service, running in a continuous loop.
*/
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_messages_inbound_processFlag_msgDate'
    AND object_id = OBJECT_ID('infce.messages_inbound')
)
BEGIN
    PRINT 'Creating IX_messages_inbound_processFlag_msgDate on infce.messages_inbound...';
    CREATE NONCLUSTERED INDEX [IX_messages_inbound_processFlag_msgDate]
    ON [infce].[messages_inbound]
    (
        [processFlag] ASC,
        [msgDate] ASC
    )
    INCLUDE (
        [account_cerner],
        [sourceMsgId],
        [sourceInfce],
        [msgType],
        [processStatusMsg]
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
          DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON,
          ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY];
    PRINT 'Created successfully.';
END
ELSE
    PRINT 'IX_messages_inbound_processFlag_msgDate already exists - skipping.';
GO

/*
 Priority 3: dbo.data_billing_history - Index for GetByRunDate()

 Rationale: BillingActivityRepository.GetByRunDate() queries
 WHERE run_date BETWEEN @fromDate AND @thruDate. The clustered PK is on
 (account, run_date) which does not support standalone run_date range queries
 efficiently. A nonclustered index on run_date enables efficient date range
 scans for billing activity reports.

 Query pattern: WHERE run_date BETWEEN @0 AND @1
 Expected impact: MEDIUM - Used for billing activity reports and auditing.
*/
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_data_billing_history_run_date'
    AND object_id = OBJECT_ID('dbo.data_billing_history')
)
BEGIN
    PRINT 'Creating IX_data_billing_history_run_date on dbo.data_billing_history...';
    CREATE NONCLUSTERED INDEX [IX_data_billing_history_run_date]
    ON [dbo].[data_billing_history]
    (
        [run_date] ASC
    )
    INCLUDE (
        [account],
        [ins_abc],
        [fin_code],
        [batch],
        [ebill_status],
        [claim_amount]
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
          DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON,
          ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY];
    PRINT 'Created successfully.';
END
ELSE
    PRINT 'IX_data_billing_history_run_date already exists - skipping.';
GO

/*
 Priority 5: infce.messages_inbound - Index for GetByDateRange()

 Rationale: MessagesInboundRepository.GetByDateRange() and GetByMessageType()
 filter on msgDate. The existing IX_PROC_DFT has msgDate as the 3rd column
 which is not efficient for standalone date range queries. A simple index on
 msgDate supports the date range browsing pattern.

 Query pattern: WHERE msgDate BETWEEN @0 AND @1
 Expected impact: LOW-MEDIUM - Used for message browsing/auditing UI.
*/
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_messages_inbound_msgDate'
    AND object_id = OBJECT_ID('infce.messages_inbound')
)
BEGIN
    PRINT 'Creating IX_messages_inbound_msgDate on infce.messages_inbound...';
    CREATE NONCLUSTERED INDEX [IX_messages_inbound_msgDate]
    ON [infce].[messages_inbound]
    (
        [msgDate] ASC
    )
    INCLUDE (
        [account_cerner],
        [msgType],
        [processFlag],
        [sourceMsgId],
        [sourceInfce]
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
          DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON,
          ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY];
    PRINT 'Created successfully.';
END
ELSE
    PRINT 'IX_messages_inbound_msgDate already exists - skipping.';
GO

--------------------------------------------------------------------------------
-- SECTION 2: ENHANCE SUBOPTIMAL INDEXES (DROP + CREATE)
--------------------------------------------------------------------------------

PRINT '';
PRINT '--- Section 2: Enhancing Suboptimal Indexes ---';
GO

/*
 Priority 2: dbo.chrg.account_index - Add fin_code, cl_mnem, fin_type to INCLUDE

 Rationale: ChrgRepository.GetByAccount() is the most frequent charge query,
 filtering on account with optional additional filters. The current index
 INCLUDEs status, service_date, cdm, qty, net_amt, credited, chrg_num, invoice
 but is MISSING fin_code, cl_mnem, and fin_type. This forces key lookups back
 to the clustered index for these columns, which are commonly needed by:
   - InvoiceSelectRepository.GetByClientAndDate() (needs cl_mnem, fin_code, fin_type)
   - ClaimGeneratorService (needs fin_type)
   - Multiple stored procedures

 Adding these 3 columns to INCLUDE makes this a covering index for nearly all
 charge-by-account queries, eliminating expensive key lookups.

 Current: (account) INCLUDE(status, service_date, cdm, qty, net_amt, credited, chrg_num, invoice)
 New:     (account) INCLUDE(status, service_date, cdm, qty, net_amt, credited, chrg_num, invoice, fin_code, cl_mnem, fin_type)

 Expected impact: MEDIUM - Eliminates key lookups for the most common charge query.
*/
IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'account_index'
    AND object_id = OBJECT_ID('dbo.chrg')
)
BEGIN
    PRINT 'Dropping existing account_index on dbo.chrg...';
    DROP INDEX [account_index] ON [dbo].[chrg];
    PRINT 'Dropped.';
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'account_index'
    AND object_id = OBJECT_ID('dbo.chrg')
)
BEGIN
    PRINT 'Creating enhanced account_index on dbo.chrg with additional INCLUDE columns...';
    CREATE NONCLUSTERED INDEX [account_index] ON [dbo].[chrg]
    (
        [account] ASC
    )
    INCLUDE (
        [status],
        [service_date],
        [cdm],
        [qty],
        [net_amt],
        [credited],
        [chrg_num],
        [invoice],
        [fin_code],
        [cl_mnem],
        [fin_type]
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
          DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON,
          ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY];
    PRINT 'Created successfully.';
END
ELSE
    PRINT 'account_index already exists on dbo.chrg - skipping.';
GO

/*
 Priority 4: dbo.chrg.credited_account_status_Includes - Add cdm, service_date, net_amt to INCLUDE

 Rationale: This index supports queries filtering on credited = 0 AND account = @account
 AND status = @status, which is a frequent pattern in charge processing. The current
 INCLUDE of (chrg_num, qty) is insufficient for queries that also need cdm, service_date,
 and net_amt (e.g., the charge balance views and stored procedures like sp_GetClaim837Inst).

 Current: (credited, account, status) INCLUDE(chrg_num, qty)
 New:     (credited, account, status) INCLUDE(chrg_num, qty, cdm, service_date, net_amt)

 Expected impact: MEDIUM - Reduces key lookups for charge summary and billing queries.
*/
IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'credited_account_status_Includes'
    AND object_id = OBJECT_ID('dbo.chrg')
)
BEGIN
    PRINT 'Dropping existing credited_account_status_Includes on dbo.chrg...';
    DROP INDEX [credited_account_status_Includes] ON [dbo].[chrg];
    PRINT 'Dropped.';
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'credited_account_status_Includes'
    AND object_id = OBJECT_ID('dbo.chrg')
)
BEGIN
    PRINT 'Creating enhanced credited_account_status_Includes on dbo.chrg with additional INCLUDE columns...';
    CREATE NONCLUSTERED INDEX [credited_account_status_Includes] ON [dbo].[chrg]
    (
        [credited] ASC,
        [account] ASC,
        [status] ASC
    )
    INCLUDE (
        [chrg_num],
        [qty],
        [cdm],
        [service_date],
        [net_amt]
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
          DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON,
          ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY];
    PRINT 'Created successfully.';
END
ELSE
    PRINT 'credited_account_status_Includes already exists on dbo.chrg - skipping.';
GO

--------------------------------------------------------------------------------
-- SECTION 3: DROP REDUNDANT INDEXES
--------------------------------------------------------------------------------

PRINT '';
PRINT '--- Section 3: Dropping Redundant Indexes ---';
GO

/*
 Priority 6: dbo.acc_location.IX_acc_location - Redundant with covering index

 Rationale: This index has key columns (location, surveydate) which are
 identical to the wider index [ix_location, surveydate INCLUDE account,
 pt_type, mod_date, mod_user, mod_prg, mod_host, ov_acct, ov_mri]. The wider
 index can serve all queries that the narrow index handles, plus provides
 covering index benefits. The narrow index adds write overhead and storage
 with no query benefit.

 Verification:
 - This index is NOT a unique constraint.
 - This index is NOT referenced by any foreign key.
 - The wider covering index with the same key columns exists on the same table.

 Expected impact: LOW - Reduces write overhead and storage on acc_location table.
*/
IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_acc_location'
    AND object_id = OBJECT_ID('dbo.acc_location')
)
BEGIN
    PRINT 'Dropping redundant IX_acc_location on dbo.acc_location...';
    PRINT '  (Redundant with wider covering index [ix_location, surveydate INCLUDE ...])';
    DROP INDEX [IX_acc_location] ON [dbo].[acc_location];
    PRINT 'Dropped successfully.';
END
ELSE
    PRINT 'IX_acc_location does not exist on dbo.acc_location - skipping.';
GO

--------------------------------------------------------------------------------
-- COMPLETION
--------------------------------------------------------------------------------

PRINT '';
PRINT '=== Index Optimization Script Complete ===';
PRINT 'Finished: ' + CONVERT(varchar(30), GETDATE(), 121);
PRINT '';
PRINT 'POST-IMPLEMENTATION STEPS:';
PRINT '1. Update statistics on modified tables: UPDATE STATISTICS dbo.chrg; UPDATE STATISTICS infce.messages_inbound; UPDATE STATISTICS dbo.data_billing_history; UPDATE STATISTICS dbo.acc_location;';
PRINT '2. Monitor query plans for key queries (HL7 processing, charge lookups, billing activity)';
PRINT '3. Check sys.dm_db_index_usage_stats after 1 week to verify new indexes are being used';
PRINT '4. Review sys.dm_db_missing_index_details for any additional suggestions from the optimizer';
GO
