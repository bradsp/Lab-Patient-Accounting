-- =============================================
-- Author:		Bradley Powers
-- Create date: 9/29/2019
-- Description:	Gets summary information for the specified account
-- =============================================
CREATE PROCEDURE [dbo].[GetAccountSummary] 
	-- Add the parameters for the stored procedure here
	@accno varchar(15) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select acc.*, 
		client.cli_nme as 'ClientName', 
		dbo.GetAccTotalCharges(@accno) as 'TotalCharges',
        dbo.GetAmtPaidByAccount(@accno) as 'TotalPayments',
        dbo.GetContractualByAccount(@accno) as 'TotalContractual',
        dbo.GetWriteOffByAccount(@accno) as 'TotalWriteoff',
        dbo.GetBadDebtByAccount(@accno) as 'TotalBadDebt',
		dbo.GetAccBalByDate(@accno,GETDATE()) as 'Balance'
    from acc left outer join client on acc.cl_mnem = client.cli_mnem 
	where account = @accno;
                    
END
