CREATE PROCEDURE [dbo].[sp_create_cctemp] @begin_date datetime, @end_date datetime AS

select chrg.cdm,chrg.chrg_num,chrg.fin_type as fintype,acc.cl_mnem,chrg.inp_price,
chrg.retail,amt.amount,chrg.qty,amt.type as amttype,chrg.status,client.print_cc
INTO tempdb..cc_temp

from chrg JOIN acc on chrg.account = acc.account
JOIN amt on chrg.chrg_num = amt.chrg_num
JOIN client on acc.cl_mnem = client.cli_mnem

where amt.mod_date between @begin_date and @end_date
AND chrg.cdm <> 'CBILL'
