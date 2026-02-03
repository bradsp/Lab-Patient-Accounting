/*
==============================================================================
 Stored Procedure & Function Optimization Script
 Database: LabPatientAccounting
 Date: 2026-02-03

 This script contains optimized versions of Critical and High severity
 stored procedures and functions identified in the optimization report.

 IMPORTANT: Test each change in a staging environment before deploying
 to production. Verify output matches the original for representative
 test cases.

 Changes are organized by priority:
   1. Critical scalar function fixes (non-sargable dates, nested calls)
   2. Critical procedure inlining (patient billing, HL7)
   3. High correctness fixes (@@IDENTITY, transactions)
   4. High performance fixes (scalar function inlining)
==============================================================================
*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

PRINT '============================================================'
PRINT 'Starting Stored Procedure Optimization Script'
PRINT 'Date: ' + CONVERT(VARCHAR(20), GETDATE(), 120)
PRINT '============================================================'
GO

-- ============================================================================
-- PRIORITY 1: CRITICAL - Fix non-sargable date comparisons
-- ============================================================================

PRINT ''
PRINT '--- Priority 1: Fixing GetAccBalByDate (non-sargable dates) ---'
GO

/*
 Original anti-pattern:
   CONVERT(DATETIME,convert(varchar(10),chrg_details.mod_date,101)) <=
   CONVERT(DATETIME,convert(varchar(10),@effDate,101))

 This wraps the indexed column chrg_details.mod_date in CONVERT(), preventing
 index seeks. Fix: Use date boundary comparison that is sargable.

 Input/Output contract: UNCHANGED
   @account varchar(15), @effDate datetime -> NUMERIC(18,2)
*/
CREATE OR ALTER FUNCTION [dbo].[GetAccBalByDate]
(
    @account varchar(15),
    @effDate datetime
)
RETURNS NUMERIC(18,2)
AS
BEGIN
    DECLARE @Balance NUMERIC(18,2);

    -- Calculate the sargable date boundary once
    -- Original compared date-only portions, so we use < next day
    DECLARE @dateBoundary DATETIME;
    SET @dateBoundary = DATEADD(DAY, 1, CAST(@effDate AS DATE));

    WITH cteChrg ([account], [ChargeTotal])
    AS
    (
        SELECT [chrg].[account], SUM([qty]*[amount]) AS 'chrgtotal'
        FROM [chrg]
        JOIN [chrg_details] ON [chrg].[chrg_num] = [chrg_details].[chrg_num]
        WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A')
            AND [chrg_details].[mod_date] < @dateBoundary  -- SARGABLE
            AND [chrg].[account] = @account
        GROUP BY [chrg].[account]
    ),
    cteChk ([account], [ChkTotal])
    AS
    (
        SELECT [chk].[account],
            SUM(ISNULL([amt_paid],0.00) + ISNULL([write_off],0.00) + ISNULL([contractual],0.00)) AS 'chktotal'
        FROM chk
        WHERE [chk].[status] <> 'CBILL'
            AND [chk].[mod_date] < @dateBoundary  -- SARGABLE
            AND [chk].[account] = @account
        GROUP BY [chk].[account]
    )

    SELECT @Balance = ISNULL(cteChrg.ChargeTotal,0.00) - ISNULL(cteChk.ChkTotal,0.00)
    FROM cteChrg
    FULL JOIN cteChk ON cteChrg.account = cteChk.account;

    RETURN ISNULL(@Balance,0.00)
END
GO

PRINT '--- Priority 1: Fixing GetAccBalByTransDate (non-sargable dates) ---'
GO

/*
 Same non-sargable pattern on service_date.
 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER FUNCTION [dbo].[GetAccBalByTransDate]
(
    @account varchar(15),
    @effDate datetime
)
RETURNS NUMERIC(18,2)
AS
BEGIN
    DECLARE @Balance NUMERIC(18,2);

    DECLARE @dateBoundary DATETIME;
    SET @dateBoundary = DATEADD(DAY, 1, CAST(@effDate AS DATE));

    WITH cteChrg ([account], [ChargeTotal])
    AS
    (
        SELECT [chrg].[account], SUM([qty]*[amount]) AS 'chrgtotal'
        FROM [chrg]
        JOIN [chrg_details] ON [chrg].[chrg_num] = [chrg_details].[chrg_num]
        WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A')
            AND [chrg].[service_date] < @dateBoundary  -- SARGABLE (was CONVERT wrapped)
            AND [chrg].[account] = @account
        GROUP BY [chrg].[account]
    ),
    cteChk ([account], [ChkTotal])
    AS
    (
        SELECT [chk].[account],
            SUM(ISNULL([amt_paid],0.00) + ISNULL([write_off],0.00) + ISNULL([contractual],0.00)) AS 'chktotal'
        FROM chk
        WHERE [chk].[status] <> 'CBILL'
            AND [chk].[mod_date] < @dateBoundary  -- SARGABLE
            AND [chk].[account] = @account
        GROUP BY [chk].[account]
    )

    SELECT @Balance = ISNULL(cteChrg.ChargeTotal,0.00) - ISNULL(cteChk.ChkTotal,0.00)
    FROM cteChrg
    FULL JOIN cteChk ON cteChrg.account = cteChk.account;

    RETURN ISNULL(@Balance,0.00)
END
GO

PRINT '--- Priority 1: Fixing GetAccTotalCharges (nested scalar function) ---'
GO

/*
 Original anti-pattern:
   SELECT @chrgTotal = SUM(chrg.qty * dbo.GetChrgDetailTotal(chrg.chrg_num))

 GetChrgDetailTotal is: SELECT SUM(amount) FROM chrg_details WHERE chrg_num = @chrg_num
 This creates O(n^2) - for each chrg row, a separate scalar query runs against chrg_details.

 Fix: Inline the join to chrg_details.
 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER FUNCTION [dbo].[GetAccTotalCharges]
(
    @account varchar(15)
)
RETURNS NUMERIC(18,2)
AS
BEGIN
    DECLARE @chrgTotal NUMERIC(18,2);

    -- Inline the GetChrgDetailTotal function directly as a JOIN
    SELECT @chrgTotal = SUM(chrg.qty * cd.amount)
    FROM chrg
    JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
    WHERE chrg.account = @account
        AND chrg.status NOT IN ('CBILL','CAP','N/A');

    RETURN ISNULL(@chrgTotal, 0.00)
END
GO

PRINT '--- Priority 1: Fixing GetAccBalByServiceDate (non-sargable dates) ---'
GO

/*
 Same non-sargable pattern.
 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER FUNCTION [dbo].[GetAccBalByServiceDate]
(
    @account varchar(15),
    @effDate datetime
)
RETURNS NUMERIC(18,2)
AS
BEGIN
    DECLARE @Balance NUMERIC(18,2);

    DECLARE @dateBoundary DATETIME;
    SET @dateBoundary = DATEADD(DAY, 1, CAST(@effDate AS DATE));

    WITH cteChrg ([account], [ChargeTotal])
    AS
    (
        SELECT [chrg].[account], SUM([qty]*[amount]) AS 'chrgtotal'
        FROM [chrg]
        JOIN [chrg_details] ON [chrg].[chrg_num] = [chrg_details].[chrg_num]
        WHERE [chrg].[status] NOT IN ('CBILL','CAP','N/A')
            AND [chrg].[service_date] < @dateBoundary  -- SARGABLE
            AND [chrg].[account] = @account
        GROUP BY [chrg].[account]
    ),
    cteChk ([account], [ChkTotal])
    AS
    (
        SELECT [chk].[account],
            SUM(ISNULL([amt_paid],0.00) + ISNULL([write_off],0.00) + ISNULL([contractual],0.00)) AS 'chktotal'
        FROM chk
        WHERE [chk].[status] <> 'CBILL'
            AND [chk].[mod_date] < @dateBoundary  -- SARGABLE
            AND [chk].[account] = @account
        GROUP BY [chk].[account]
    )

    SELECT @Balance = ISNULL(cteChrg.ChargeTotal,0.00) - ISNULL(cteChk.ChkTotal,0.00)
    FROM cteChrg
    FULL JOIN cteChk ON cteChrg.account = cteChk.account;

    RETURN ISNULL(@Balance,0.00)
END
GO

-- ============================================================================
-- PRIORITY 2: CRITICAL - Optimize GetAccountSummary (7 scalar calls for 1 row)
-- ============================================================================

PRINT ''
PRINT '--- Priority 2: Optimizing GetAccountSummary (inline scalar functions) ---'
GO

/*
 Original calls 7 scalar functions for a single account row.
 Each scalar function independently queries chrg, chk, chrg_details tables.

 Fix: Inline all calculations into a single query with pre-aggregated CTEs.
 Input/Output contract: UNCHANGED (same column names and types)
*/
CREATE OR ALTER PROCEDURE [dbo].[GetAccountSummary]
    @accno varchar(15) = null
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH cteTotalCharges AS
    (
        -- Replaces GetAccTotalCharges (which called GetChrgDetailTotal)
        SELECT chrg.account,
            SUM(chrg.qty * cd.amount) AS TotalCharges
        FROM chrg
        JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
        WHERE chrg.account = @accno
            AND chrg.status NOT IN ('CBILL','CAP','N/A')
        GROUP BY chrg.account
    ),
    ctePayments AS
    (
        -- Replaces GetAmtPaidByAccount, GetContractualByAccount, GetWriteOffByAccount
        SELECT chk.account,
            SUM(ISNULL(chk.amt_paid, 0.00)) AS TotalPayments,
            SUM(ISNULL(chk.contractual, 0.00)) AS TotalContractual,
            SUM(ISNULL(chk.write_off, 0.00)) AS TotalWriteoff
        FROM chk
        WHERE chk.account = @accno
            AND chk.status <> 'CBILL'
        GROUP BY chk.account
    ),
    cteBadDebt AS
    (
        -- Replaces GetBadDebtByAccount
        SELECT chk.account,
            SUM(ISNULL(chk.amt_paid, 0.00)) AS TotalBadDebt
        FROM chk
        WHERE chk.account = @accno
            AND chk.status = 'BADDEBT'
        GROUP BY chk.account
    ),
    cteBalance AS
    (
        -- Replaces GetAccBalByDate(@accno, GETDATE()) with sargable date logic
        SELECT chrg.account,
            SUM(chrg.qty * cd.amount) AS ChrgTotal
        FROM chrg
        JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
        WHERE chrg.account = @accno
            AND chrg.status NOT IN ('CBILL','CAP','N/A')
            AND cd.mod_date < DATEADD(DAY, 1, CAST(GETDATE() AS DATE))
        GROUP BY chrg.account
    ),
    cteBalChk AS
    (
        SELECT chk.account,
            SUM(ISNULL(chk.amt_paid, 0.00) + ISNULL(chk.write_off, 0.00) + ISNULL(chk.contractual, 0.00)) AS ChkTotal
        FROM chk
        WHERE chk.account = @accno
            AND chk.status <> 'CBILL'
            AND chk.mod_date < DATEADD(DAY, 1, CAST(GETDATE() AS DATE))
        GROUP BY chk.account
    )
    SELECT acc.*,
        client.cli_nme AS 'ClientName',
        ISNULL(tc.TotalCharges, 0.00) AS 'TotalCharges',
        ISNULL(pmt.TotalPayments, 0.00) AS 'TotalPayments',
        ISNULL(pmt.TotalContractual, 0.00) AS 'TotalContractual',
        ISNULL(pmt.TotalWriteoff, 0.00) AS 'TotalWriteoff',
        ISNULL(bd.TotalBadDebt, 0.00) AS 'TotalBadDebt',
        ISNULL(bal.ChrgTotal, 0.00) - ISNULL(balchk.ChkTotal, 0.00) AS 'Balance'
    FROM acc
    LEFT OUTER JOIN client ON acc.cl_mnem = client.cli_mnem
    LEFT JOIN cteTotalCharges tc ON tc.account = acc.account
    LEFT JOIN ctePayments pmt ON pmt.account = acc.account
    LEFT JOIN cteBadDebt bd ON bd.account = acc.account
    LEFT JOIN cteBalance bal ON bal.account = acc.account
    LEFT JOIN cteBalChk balchk ON balchk.account = acc.account
    WHERE acc.account = @accno;

    EXEC utility.Log_ProcedureCall @ObjectID = @@PROCID;
END
GO

-- ============================================================================
-- PRIORITY 3: HIGH - Fix @@IDENTITY and add transactions
-- ============================================================================

PRINT ''
PRINT '--- Priority 3: Fixing usp_prg_Credit_Charge (@@IDENTITY, transaction, PRINT) ---'
GO

/*
 Original anti-patterns:
   1. Uses @@IDENTITY (line 26752) - should be SCOPE_IDENTITY()
   2. No transaction wrapping 3-step operation (INSERT chrg, INSERT amt, UPDATE chrg)
   3. PRINT statements in production code
   4. SELECT * for existence check

 Input/Output contract: UNCHANGED (same parameters, same logical behavior)
*/
CREATE OR ALTER PROCEDURE [dbo].[usp_prg_Credit_Charge]
    @chrgNum numeric(18,0) = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @chrgNumNew NUMERIC(18,0);

    -- Use SELECT 1 instead of SELECT * for existence check
    IF (NOT EXISTS(SELECT 1 FROM chrg WHERE chrg_num = @chrgNum AND credited = 0))
    BEGIN
        RETURN;
    END

    BEGIN TRANSACTION;
    BEGIN TRY

        -- 1. INSERT the new credited record into the charge table
        INSERT INTO dbo.chrg
        (
            credited, account, status, service_date, hist_date,
            cdm, qty, retail, inp_price, comment,
            mod_date, mod_user, mod_prg, mod_host,
            net_amt, fin_type, mt_req_no, fin_code,
            order_site, pat_ssn, unitno, location,
            responsiblephy, mt_mnem, action, facility, referencereq,
            pat_dob, chrg_err
        )
        SELECT 1, account, status, service_date, GETDATE(),
            cdm, (qty * -1) AS [qty], retail, inp_price,
            'dbo.usp_prg_Credit_Charge',
            GETDATE() AS [mod_date],
            RIGHT(SUSER_SNAME(), 50) AS [mod_user],
            RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                'SQL PROC ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50) AS [mod_prg],
            RIGHT(HOST_NAME(), 50) AS [mod_host],
            net_amt, fin_type, mt_req_no, fin_code,
            order_site, pat_ssn, unitno, location,
            responsiblephy, mt_mnem, action, facility, referencereq,
            pat_dob, chrg_err
        FROM chrg
        WHERE chrg_num = @chrgNum
            AND credited = 0;

        -- 2. Get the new chrg_num - use SCOPE_IDENTITY() instead of @@IDENTITY
        SET @chrgNumNew = SCOPE_IDENTITY();

        -- 3. Insert the amount records for the credited charge
        INSERT INTO amt
        (chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted,
         modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code,
         bill_type, bill_method, pointer_set)
        SELECT @chrgNumNew, cpt4, type, amount,
            GETDATE() AS [mod_date],
            RIGHT(SUSER_SNAME(), 50) AS [mod_user],
            RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                'SQL PROC ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50) AS [mod_prg],
            deleted, modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code,
            bill_type, bill_method, 1 AS [pointer_set]
        FROM amt
        WHERE chrg_num = @chrgNum;

        -- 4. Update the old charge as credited
        UPDATE dbo.chrg
        SET credited = 1,
            comment = 'dbo.usp_prg_Credit_Charge'
        WHERE chrg_num = @chrgNum;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Re-raise the error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

PRINT '--- Priority 3: Fixing usp_prg_Drug_Screens (add transaction) ---'
GO

/*
 Original anti-patterns:
   1. No transaction wrapping multi-table INSERT/UPDATE operations
   2. Potential for orphaned amt records if chrg INSERT fails

 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER PROCEDURE [dbo].[usp_prg_Drug_Screens]
    @OrigChrgNum numeric(18,0) = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @chrgRowguid uniqueidentifier;
    SET @chrgRowguid = NEWID();
    DECLARE @chrg_num numeric(18,0);

    DECLARE @credited int;
    SET @credited = (SELECT credited FROM chrg WHERE chrg_num = @OrigChrgNum);

    IF (@credited = 0)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY

            -- Credit the original charge
            UPDATE chrg
            SET credited = 1
            WHERE chrg_num = @OrigChrgNum;

            -- Insert counter-charge (reversal)
            INSERT INTO chrg
            (rowguid, credited, account, [status], service_date, hist_date, cdm, qty, retail, inp_price, comment,
             mod_date, mod_user, mod_prg, net_amt, fin_type, mod_host, mt_req_no, fin_code, performing_site, bill_method)
            SELECT
                @ChrgRowguid, credited, account, [status], service_date, GETDATE(), cdm, qty * -1, retail, inp_price, comment,
                GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT('usp_prg_Drug_Screen ' + CONVERT(VARCHAR(10), GETDATE(), 112), 50),
                net_amt, fin_type, RIGHT(mod_host, 50), mt_req_no, fin_code, performing_site, bill_method
            FROM chrg AS chrg_1
            WHERE chrg_1.chrg_num = @OrigChrgNum;

            -- Get new chrg_num via rowguid
            SET @chrg_num = (SELECT chrg_num FROM chrg WHERE rowguid = @ChrgRowguid);

            -- Insert amt records for counter-charge
            INSERT INTO amt
            (chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted, modi, revcode, modi2,
             diagnosis_code_ptr, mt_req_no, order_code, bill_type, bill_method)
            SELECT
                @chrg_num, cpt4, [type], amount, GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT('usp_prg_Drug_Screen ' + CONVERT(VARCHAR(10), GETDATE(), 112), 50),
                deleted, modi, revcode, modi2,
                diagnosis_code_ptr, mt_req_no, order_code, bill_type, bill_method
            FROM amt AS amt_1
            WHERE amt_1.chrg_num = @OrigChrgNum;

            -- Now add the medicare drug screen charge
            SET @ChrgRowguid = NEWID();

            INSERT INTO chrg
            (rowguid, credited, account, [status], service_date, hist_date, cdm, qty, retail, inp_price, comment,
             mod_date, mod_user, mod_prg, net_amt, fin_type, mod_host, mt_req_no, fin_code, performing_site, bill_method)
            SELECT
                @ChrgRowguid, 0, account, 'NEW', service_date, GETDATE(), '5362566', 1,
                (SELECT SUM(mprice) FROM cpt4 WHERE cdm = '5362566'),
                (SELECT SUM(zprice) FROM cpt4 WHERE cdm = '5362566'),
                'G0143 Conversion',
                GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT('usp_prg_Drug_Screen ' + CONVERT(VARCHAR(10), GETDATE(), 112), 50),
                (SELECT SUM(mprice) FROM cpt4 WHERE cdm = '5362566'),
                fin_type, RIGHT(chrg_1.mod_host, 50), mt_req_no, fin_code, performing_site, bill_method
            FROM chrg AS chrg_1
            INNER JOIN cpt4 ON cpt4.cdm = '5362566'
            WHERE chrg_1.chrg_num = @OrigChrgNum;

            -- Add the new amt record
            SET @chrg_num = (SELECT chrg_num FROM chrg WHERE rowguid = @ChrgRowguid);

            INSERT INTO amt
            (chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted, modi, revcode)
            SELECT
                @chrg_num, cpt4, [type], mprice, GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT('usp_prg_Drug_Screen ' + CONVERT(VARCHAR(10), GETDATE(), 112), 50),
                deleted, modi, rev_code
            FROM cpt4
            WHERE cpt4.cdm = '5362566';

            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
            DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
            DECLARE @ErrorState INT = ERROR_STATE();
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        END CATCH
    END
END
GO

PRINT '--- Priority 3: Fixing usp_prg_Drug_Screens_Reversal (@@IDENTITY, transaction) ---'
GO

/*
 Original anti-patterns:
   1. @@IDENTITY on line 27006
   2. No transaction wrapping
   3. String parsing with CHARINDEX for CDM extraction

 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER PROCEDURE [dbo].[usp_prg_Drug_Screens_Reversal]
    @OrigChrgNum numeric(18,0) = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @chrgRowguid uniqueidentifier;
    SET @chrgRowguid = NEWID();
    DECLARE @chrg_num numeric(18,0);

    DECLARE @credited int;
    SET @credited = (SELECT credited FROM chrg WHERE chrg_num = @OrigChrgNum);

    IF (@credited = 0)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY

            -- Credit the original charge
            UPDATE chrg
            SET credited = 1
            WHERE chrg_num = @OrigChrgNum;

            -- 1. Insert the counter charge into the table
            INSERT INTO chrg
            (rowguid, credited, account, [status], service_date, hist_date, cdm, qty, retail, inp_price, comment,
             mod_date, mod_user, mod_prg, net_amt, fin_type, mod_host, mt_req_no, fin_code, performing_site, bill_method)
            SELECT
                @ChrgRowguid, credited, account, [status], service_date, GETDATE(), cdm, qty * -1, retail, inp_price, comment,
                GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                    'SQL TRIGGER ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50),
                net_amt, fin_type, RIGHT(mod_host, 50), mt_req_no, fin_code, performing_site, bill_method
            FROM chrg AS chrg_1
            WHERE chrg_1.chrg_num = @OrigChrgNum;

            -- Use SCOPE_IDENTITY() instead of @@IDENTITY
            SET @chrg_num = SCOPE_IDENTITY();

            -- Insert amt records for counter-charge
            INSERT INTO amt
            (chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted, modi, revcode, modi2,
             diagnosis_code_ptr, mt_req_no, order_code, bill_type, bill_method)
            SELECT
                @chrg_num, cpt4, [type], amount, GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                    'SQL TRIGGER ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50),
                deleted, modi, revcode, modi2,
                diagnosis_code_ptr, mt_req_no, order_code, bill_type, bill_method
            FROM amt AS amt_1
            WHERE amt_1.chrg_num = @OrigChrgNum;

            -- Now add old drug screen back
            SET @ChrgRowguid = NEWID();
            DECLARE @origCDM VARCHAR(7);
            SET @origCDM = (SELECT CAST(SUBSTRING(comment, CHARINDEX('[', comment) + 1, 7) AS VARCHAR(7))
                           FROM chrg WHERE chrg_num = @OrigChrgNum);

            INSERT INTO chrg
            (rowguid, credited, account, [status], service_date, hist_date, cdm, qty, retail, inp_price, comment,
             mod_date, mod_user, mod_prg, net_amt, fin_type, mod_host, mt_req_no, fin_code, performing_site)
            SELECT
                @ChrgRowguid, 0, account, 'NEW', service_date, GETDATE(), @origCDM, qty,
                (SELECT SUM(mprice) FROM cpt4 WHERE cdm = @origCDM),
                (SELECT SUM(zprice) FROM cpt4 WHERE cdm = @origCDM),
                'G0143 ReConversion [' + @origCdm + ']',
                GETDATE(),
                RIGHT(SUSER_SNAME(), 50),
                RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                    'SQL TRIGGER ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50),
                (SELECT SUM(mprice) FROM cpt4 WHERE cdm = @origCDM), fin_type,
                RIGHT(HOST_NAME(), 50), mt_req_no, fin_code, performing_site
            FROM chrg AS chrg_1
            WHERE chrg_1.chrg_num = @OrigChrgNum;

            -- Now add the new amt record
            SET @chrg_num = (SELECT chrg_num FROM chrg WHERE rowguid = @ChrgRowguid);

            INSERT INTO amt
            (chrg_num, cpt4, [type], amount, mod_date, mod_user, mod_prg, deleted, modi, revcode)
            SELECT
                @chrg_num, cpt4, [type], mprice, GETDATE(), RIGHT(SUSER_SNAME(), 50),
                RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                    'SQL TRIGGER ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50),
                deleted, modi, rev_code
            FROM cpt4
            WHERE cpt4.cdm = @origCDM;

            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
            DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
            DECLARE @ErrorState INT = ERROR_STATE();
            RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        END CATCH
    END
END
GO

PRINT '--- Priority 3: Fixing usp_prg_CreateAccount (transaction, race condition) ---'
GO

/*
 Original anti-patterns:
   1. No transaction wrapping UPDATE number + INSERT acc (race condition on sequence)
   2. No TRY/CATCH error handling
   3. SELECT @acc without assignment clarity

 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER PROCEDURE [dbo].[usp_prg_CreateAccount]
    @client varchar(10),
    @dos datetime
AS
BEGIN
    SET NOCOUNT ON;

    SET @client = COALESCE(NULLIF(@client, ''), 'TBL');
    SET @dos = COALESCE(
        CAST(NULLIF(@dos, '') AS DATE),
        CAST(GETDATE() AS DATE));

    DECLARE @acc VARCHAR(15);

    BEGIN TRANSACTION;
    BEGIN TRY

        -- Atomic read-and-increment of sequence number
        -- This UPDATE with variable assignment is atomic within a transaction
        UPDATE number
        SET @acc = cnt,
            cnt = cnt + 1
        WHERE dbo.number.keyfield = 'account';

        SET @acc = STUFF(CAST(@acc AS VARCHAR(15)), 1, 0, 'D');

        INSERT INTO dbo.acc
        (
            deleted, account, pat_name, cl_mnem, fin_code,
            trans_date, cbill_date, status, ssn, num_comments,
            meditech_account, original_fincode, oereqno, mri,
            post_date, ov_order_id, ov_pat_id,
            mod_date, mod_user, mod_prg, mod_host,
            bill_priority, guarantorID, HNE_NUMBER,
            trans_date_time, tdate_update
        )
        VALUES
        (
            0, @acc, '', @client, '',
            @dos, '', '', '', 0,
            '', '', '', '',
            NULL, '', '',
            GETDATE(),
            RIGHT(SUSER_SNAME(), 50),
            COALESCE(RIGHT(APP_NAME(), 50), RIGHT(ISNULL(OBJECT_NAME(@@PROCID),
                    'SQL PROC ' + CONVERT(VARCHAR(10), GETDATE(), 112)), 50), 'NO APP IDENTIFIED'),
            RIGHT(HOST_NAME(), 50),
            0, '', '',
            NULL, NULL
        );

        -- Return the created account number
        SELECT @acc AS [account];

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

-- ============================================================================
-- PRIORITY 4: HIGH - Fix GetClientsToBill (scalar functions in WHERE clause)
-- ============================================================================

PRINT ''
PRINT '--- Priority 4: Optimizing GetClientsToBill (inline scalar from WHERE) ---'
GO

/*
 Original anti-patterns:
   1. GetAccClientBalance scalar function called per-row in SUM()
   2. GetAccBalance scalar function called per-row in WHERE clause (2x)

 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER PROCEDURE [dbo].[GetClientsToBill]
    @thruDate datetime
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH cteClientBalance AS
    (
        -- Pre-calculate client balances instead of scalar per-row
        SELECT acc.account,
            SUM(ISNULL(chrg.qty * cd.amount, 0.00)) AS ChrgTotal,
            SUM(ISNULL(chk_agg.ChkTotal, 0.00)) AS ChkTotal
        FROM acc
        LEFT JOIN chrg ON chrg.account = acc.account
            AND chrg.status NOT IN ('CBILL','CAP','N/A')
        LEFT JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
        LEFT JOIN (
            SELECT account,
                SUM(ISNULL(amt_paid, 0.00) + ISNULL(write_off, 0.00) + ISNULL(contractual, 0.00)) AS ChkTotal
            FROM chk
            WHERE status <> 'CBILL'
            GROUP BY account
        ) chk_agg ON chk_agg.account = acc.account
        GROUP BY acc.account
    ),
    cteAccBalance AS
    (
        SELECT account, (ChrgTotal - ChkTotal) AS Balance
        FROM cteClientBalance
    ),
    cteUnbilledByClient AS
    (
        -- Replaces GetAccClientBalance scalar function
        SELECT vbs.cl_mnem,
            SUM(
                ISNULL(
                    (SELECT SUM(chrg.qty * cd.amount)
                     FROM chrg
                     JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
                     WHERE chrg.account = vbs.account
                        AND chrg.cl_mnem = vbs.cl_mnem
                        AND chrg.status = 'CBILL'
                        AND chrg.credited = 0), 0.00)
            ) AS UnbilledAmount
        FROM vw_cbill_select vbs
        WHERE vbs.trans_date < @thruDate
        GROUP BY vbs.cl_mnem
    )
    SELECT client.cli_mnem AS 'ClientMnem',
        client.cli_nme AS 'ClientName',
        ct.description AS 'ClientType',
        ISNULL(ab.Balance, 0.00) AS 'PriorBalance',
        COALESCE(vbs.UnbilledAmount, 0.00) AS 'UnbilledAmount'
    FROM client
    JOIN acc ON client.cli_mnem = acc.account
    LEFT OUTER JOIN cteUnbilledByClient vbs ON vbs.cl_mnem = client.cli_mnem
    LEFT JOIN cteAccBalance ab ON ab.account = acc.account
    JOIN dictionary.clienttype ct ON ct.type = client.[type]
    WHERE (vbs.UnbilledAmount <> 0.00 OR ISNULL(ab.Balance, 0.00) <> 0.00);

    EXEC utility.Log_ProcedureCall @ObjectID = @@PROCID;
END
GO

-- ============================================================================
-- PRIORITY 5: CRITICAL - Optimize usp_prg_pat_bill_update_flags
-- ============================================================================

PRINT ''
PRINT '--- Priority 5: Optimizing usp_prg_pat_bill_update_flags (scalar in WHERE) ---'
GO

/*
 Original anti-patterns:
   1. GetAccBalByDate called 2x per row in the initial SELECT INTO #MailerTemp
      (once in the WHERE clause, once in the SELECT list)
   2. GetAccBalByDate called again later for bad_debt insert
   3. No transaction wrapping multiple UPDATEs and INSERTs

 Fix: Pre-compute balances set-based and use throughout.
 Note: Adding transaction wrapping to the existing TRY/CATCH.
 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER PROCEDURE [dbo].[usp_prg_pat_bill_update_flags]
    @thrudate datetime
AS
    DECLARE @today DATETIME;
    SET @today = DATEADD(dd, DATEDIFF(dd, 0, GETDATE()), 0);

    -- Pre-compute the sargable date boundary
    DECLARE @dateBoundary DATETIME;
    SET @dateBoundary = DATEADD(DAY, 1, CAST(@today AS DATE));

    DECLARE @smallBalAmt FLOAT;
    SET @smallBalAmt = CAST(dbo.GetSystemValue('small_balance_amt') AS FLOAT);

    -- Compute balances set-based instead of scalar function per row
    ;WITH cteCharges AS
    (
        SELECT chrg.account, SUM(chrg.qty * cd.amount) AS ChargeTotal
        FROM chrg
        JOIN chrg_details cd ON cd.chrg_num = chrg.chrg_num
        WHERE chrg.status NOT IN ('CBILL','CAP','N/A')
            AND cd.mod_date < @dateBoundary
        GROUP BY chrg.account
    ),
    cteChecks AS
    (
        SELECT chk.account,
            SUM(ISNULL(chk.amt_paid, 0.00) + ISNULL(chk.write_off, 0.00) + ISNULL(chk.contractual, 0.00)) AS ChkTotal
        FROM chk
        WHERE chk.status <> 'CBILL'
            AND chk.mod_date < @dateBoundary
        GROUP BY chk.account
    ),
    cteBalances AS
    (
        SELECT COALESCE(c.account, k.account) AS account,
            ISNULL(c.ChargeTotal, 0.00) - ISNULL(k.ChkTotal, 0.00) AS Balance
        FROM cteCharges c
        FULL JOIN cteChecks k ON c.account = k.account
    )
    -- Select the accounts eligible to receive a statement, or will be sent to bad_debt
    SELECT vw.account, vw.mailer, vw.bd_list_date, vw.last_dm, vw.fin_code,
        ISNULL(bal.Balance, 0.00) AS [Balance]
    INTO #MailerTemp
    FROM vw_acc_pat vw
    LEFT JOIN cteBalances bal ON bal.account = vw.account
    WHERE vw.mailer <> 'N'
        AND vw.trans_date <= @thrudate
        AND vw.fin_code NOT IN ('X','Y','W','Z','CLIENT')
        AND ISNULL(bal.Balance, 0.00) > @smallBalAmt
        AND vw.bd_list_date IS NULL;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Add accounts to bad debt list if mailer is 3 or more
        UPDATE pat SET
            bd_list_date = @today,
            mailer = CAST(CAST(mailer AS INT) + 1 AS VARCHAR),
            mod_date = GETDATE(),
            mod_host = RIGHT(HOST_NAME(), 50),
            mod_prg = 'usp_prg_pat_bill_update_flags',
            mod_user = RIGHT(SUSER_SNAME(), 50)
        WHERE pat.account IN (SELECT account FROM #MailerTemp WHERE mailer IN ('3','4','5','6') AND bd_list_date IS NULL);

        -- Add entries to system-log for updates
        INSERT INTO system_log (log_date, log_text, account)
        SELECT GETDATE(), 'Account added to Bad Debt List and mailer flag incremented', account
        FROM #MailerTemp WHERE mailer IN ('3','4','5','6') AND bd_list_date IS NULL;

        -- Add accounts going to collection to bad_debt table
        -- Use pre-computed balance from #MailerTemp instead of calling GetAccBalByDate again
        INSERT INTO bad_debt
            (account_no, debtor_first_name, debtor_last_name, st_addr_1, st_addr_2, city, state_zip, spouse,
            phone, soc_security, license_number, employment, remarks, patient_name, remarks2,
            misc, service_date, payment_date, balance, date_entered, date_sent)
        SELECT temp.account AS account_no,
            CASE pat.relation
                WHEN '03' THEN LEFT(pat.guar_first_name, 15)
                ELSE LEFT(acc.first_name, 15) END AS debtor_first_name,
            CASE pat.relation
                WHEN '03' THEN LEFT(pat.guar_last_name, 20)
                ELSE LEFT(acc.last_name, 20) END AS debtor_last_name,
            CASE pat.relation
                WHEN '03' THEN LEFT(pat.guar_addr, 25)
                ELSE LEFT(pat.pat_addr1, 25) END AS st_addr_1,
            CASE pat.relation
                WHEN '03' THEN ''
                ELSE LEFT(pat.pat_addr2, 25) END AS st_addr_2,
            CASE pat.relation
                WHEN '03' THEN LEFT(pat.guar_city, 18)
                ELSE LEFT(pat.pat_city, 18) END AS city,
            CASE pat.relation
                WHEN '03' THEN LEFT(CONCAT(pat.guar_state, ' ', LEFT(pat.guar_zip, 5)), 15)
                ELSE LEFT(CONCAT(pat.pat_state, ' ', LEFT(pat.pat_zip, 5)), 15) END AS state_zip,
            CASE ins.relation WHEN '02' THEN LEFT(ins.holder_nme, 15) ELSE NULL END AS spouse,
            REPLACE(REPLACE(REPLACE(pat.pat_phone, '(', ''), ')', ''), '-', '') AS phone,
            REPLACE(pat.ssn, '-', '') AS soc_security,
            NULL AS license_number,
            LEFT(ins.employer, 35) AS employment,
            NULL AS remarks,
            LEFT(acc.pat_name, 20) AS patient_name,
            NULL AS remarks2,
            CONCAT(CONVERT(VARCHAR, acc.birthdate, 101), ' dob') AS misc,
            acc.trans_date AS service_date,
            NULL AS payment_date,
            temp.Balance AS balance,  -- Use pre-computed balance instead of scalar function
            @today AS date_entered,
            NULL AS date_sent
        FROM #MailerTemp temp
        LEFT OUTER JOIN acc ON acc.account = temp.account
        LEFT OUTER JOIN pat ON pat.account = temp.account
        LEFT OUTER JOIN ins ON ins.account = temp.account AND ins.ins_a_b_c = 'A'
        LEFT OUTER JOIN (SELECT account, MAX(chk_date) AS LastPmtDate FROM chk GROUP BY account) chk ON chk.account = temp.account
        WHERE temp.mailer IN ('3','4','5','6') AND temp.bd_list_date IS NULL;

        -- Update flag on accounts receiving a mailer - update last_dm date
        UPDATE pat SET
            last_dm = @today,
            mailer = CAST(CAST(mailer AS INT) + 1 AS VARCHAR),
            mod_date = @today,
            mod_host = RIGHT(HOST_NAME(), 50),
            mod_prg = 'usp_prg_pat_bill_update_flags',
            mod_user = RIGHT(SUSER_SNAME(), 50)
        WHERE pat.account IN (SELECT account FROM #MailerTemp WHERE mailer IN ('1','2'));

        INSERT INTO system_log (log_date, log_text, account)
        SELECT GETDATE(), 'Mailer flag incremented and last_dm date updated', account
        FROM #MailerTemp WHERE mailer IN ('1','2');

        -- Update flag on accounts receiving a mailer for the first time
        UPDATE pat SET
            first_dm = @today,
            last_dm = @today,
            mailer = '1',
            mod_date = GETDATE(),
            mod_host = RIGHT(HOST_NAME(), 50),
            mod_prg = 'usp_prg_pat_bill_update_flags',
            mod_user = RIGHT(SUSER_SNAME(), 50)
        WHERE pat.account IN (SELECT account FROM #MailerTemp WHERE mailer = 'Y');

        INSERT INTO system_log (log_date, log_text, account)
        SELECT GETDATE(), 'Mailer flag set to 1 and first_dm, last_dm updated', account
        FROM #MailerTemp WHERE mailer = 'Y';

        -- Update last_dm date for accounts on payment plan
        UPDATE pat SET
            last_dm = @today,
            mod_date = GETDATE(),
            mod_host = RIGHT(HOST_NAME(), 50),
            mod_prg = 'usp_prg_pat_bill_update_flags',
            mod_user = RIGHT(SUSER_SNAME(), 50)
        WHERE pat.account IN (SELECT account FROM #MailerTemp WHERE mailer = 'P');

        INSERT INTO system_log (log_date, log_text, account)
        SELECT GETDATE(), 'Last_DM updated for payment plan', account
        FROM #MailerTemp WHERE mailer = 'P';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_SEVERITY() AS ErrorSeverity,
            ERROR_STATE() AS ErrorState,
            ERROR_PROCEDURE() AS ErrorProcedure,
            ERROR_LINE() AS ErrorLine,
            ERROR_MESSAGE() AS ErrorMessage;
    END CATCH

    IF OBJECT_ID('tempdb..#MailerTemp') IS NOT NULL
        DROP TABLE #MailerTemp;

    RETURN 0;
GO

-- ============================================================================
-- PRIORITY 6: HIGH - Inline GetChrgDetailTotal
-- ============================================================================

PRINT ''
PRINT '--- Priority 6: Fixing GetChrgDetailTotal (optimize for inline usage) ---'
GO

/*
 This function is very simple but is called inside SUM() in GetAccTotalCharges,
 causing O(n^2) behavior. The function itself is fine; the anti-pattern is
 how it's called. We've already fixed GetAccTotalCharges to inline this.

 For any remaining callers, ensure they also inline the logic.
 Input/Output contract: UNCHANGED
*/
CREATE OR ALTER FUNCTION [dbo].[GetChrgDetailTotal]
(
    @chrg_num numeric(18,0)
)
RETURNS NUMERIC(18,2)
AS
BEGIN
    DECLARE @Result NUMERIC(18,2);

    SELECT @Result = SUM(amount)
    FROM chrg_details
    WHERE chrg_num = @chrg_num;

    RETURN ISNULL(@Result, 0.00)
END
GO

-- ============================================================================
-- PRIORITY 7: MEDIUM - Fix usp_account_receivable_daily_reconciliation
-- ============================================================================

PRINT ''
PRINT '--- Priority 7: Fixing usp_account_receivable_daily_reconciliation (non-sargable GROUP BY) ---'
GO

/*
 Original anti-pattern:
   GROUP BY CONVERT(CHAR(8),amt.mod_date,10) - wraps column in function

 Fix: Use CAST(... AS DATE) which is sargable and cleaner.
 Input/Output contract: UNCHANGED
*/
-- Note: This procedure would need full source to rewrite. The key fix is:
-- Replace: GROUP BY CONVERT(CHAR(8),amt.mod_date,10)
-- With:    GROUP BY CAST(amt.mod_date AS DATE)

-- ============================================================================
-- SUMMARY
-- ============================================================================

PRINT ''
PRINT '============================================================'
PRINT 'Optimization Script Complete'
PRINT ''
PRINT 'Objects modified:'
PRINT '  - GetAccBalByDate (sargable dates)'
PRINT '  - GetAccBalByTransDate (sargable dates)'
PRINT '  - GetAccBalByServiceDate (sargable dates)'
PRINT '  - GetAccTotalCharges (inline nested scalar)'
PRINT '  - GetChrgDetailTotal (preserved, callers fixed)'
PRINT '  - GetAccountSummary (inline 7 scalar functions)'
PRINT '  - GetClientsToBill (inline scalar from WHERE)'
PRINT '  - usp_prg_Credit_Charge (SCOPE_IDENTITY, transaction, cleanup)'
PRINT '  - usp_prg_Drug_Screens (transaction wrapping)'
PRINT '  - usp_prg_Drug_Screens_Reversal (SCOPE_IDENTITY, transaction)'
PRINT '  - usp_prg_CreateAccount (transaction, race condition fix)'
PRINT '  - usp_prg_pat_bill_update_flags (set-based balance, transaction)'
PRINT ''
PRINT 'IMPORTANT: Test each procedure with representative data before'
PRINT 'deploying to production. Verify output matches the original.'
PRINT '============================================================'
GO
