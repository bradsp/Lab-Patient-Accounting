-- =============================================
-- Author:		Bradley Powers
-- Create date: 09/06/2013
-- Description:	Generate the aging accounts report for a particular date.
--		Contains a list of accounts with balance, date of service, patient name,
--		client name, primary payor name, last payment date and amount, and 
--		days since last payment.
-- =============================================
CREATE PROCEDURE [dbo].[usp_aging_accounts_report] 
	-- Add the parameters for the stored procedure here
	@ardate DATETIME,
	@fincode VARCHAR(5) = NULL,
	@inscode VARCHAR(10) = NULL,
	@daysold NUMERIC = -1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	; with cteChk ([account],[pay_no])
	AS
	(
		select account, MAX(pay_no) as pay_no
		from chk
		where mod_date < @ardate+1 and amt_paid <> 0.00
		group by account
	)
	select ah.account,
		acc.mri,
		acc.pat_name,
		CONVERT(VARCHAR(10),acc.trans_date,101) AS DateOfService, 
		ah.fin_code,
		insc.name, 
		client.cli_nme, 
		ah.balance,
		DATEDIFF(day,acc.trans_date,@ardate) as DaysOld,
		ISNULL(chk.amt_paid,0.00) as LastPmtAmt,
		CONVERT(VARCHAR(10),chk.date_rec,101) as LastPmtDate,
		ISNULL(DATEDIFF(day,chk.date_rec,@ardate),0) as DaysSinceLastPayment
	from aging_history ah
	INNER JOIN acc on ah.account = acc.account
	LEFT OUTER JOIN insc on ah.ins_code = insc.code
	LEFT OUTER JOIN client on acc.cl_mnem = client.cli_mnem
	LEFT OUTER JOIN cteChk on ah.account = cteChk.account
	LEFT OUTER JOIN chk on cteChk.pay_no = chk.pay_no
	where datestamp = @ardate
		AND (ah.fin_code = @fincode OR @fincode IS NULL)
		AND (ah.ins_code = @inscode OR @inscode IS NULL)
		AND (DATEDIFF(day,acc.trans_date,@ardate) > @daysold OR @daysold = -1)
	order by acc.pat_name, ah.account

END
