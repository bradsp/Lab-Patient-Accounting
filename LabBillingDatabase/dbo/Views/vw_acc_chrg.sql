/* Microsoft SQL Server - Scripting			*/
/* Server: MCLBILL					*/
/* Database: MCLLIVE					*/
/* Creation Date 9/19/2001 10:47:04 AM 			*/

/****** Object:  View dbo.vw_acc_chrg    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_acc_chrg] AS

select chrg.chrg_num,chrg.account,acc.pat_name,chrg.retail,chrg.cdm,chrg.inp_price,
	chrg.net_amt,acc.fin_code
from chrg JOIN acc on chrg.account = acc.account
