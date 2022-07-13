-- =============================================
-- Author:		David
-- Create date: 05/28/2013
-- Description:	Select invoice
-- =============================================
CREATE PROCEDURE [dbo].[usp_prg_InvoiceReprint] 
	-- Add the parameters for the stored procedure here
--	@Client varchar(15) = '', 
--	@Invoice varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT @Client, @Invoice

	select cl_mnem, invoice,service_date as [date], chrg.account, pat_name, chrg.cdm as [Charge Code]
		, qty, descript as [Charge Description] 
        , sum(qty*net_amt) over (partition by chrg.account, chrg.cdm) as [Amount] 
        from chrg 
        left outer join acc on acc.account = chrg.account 
        left outer  join cdm on cdm.cdm = chrg.cdm 
        where chrg.cdm <> 'CBILL' and coalesce(invoice,'') <> ''
END
