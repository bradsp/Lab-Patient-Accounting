/****** Object:  View dbo.vw_chrgcode    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_chrgcode] AS
SELECT acc.fin_code,chrg.status,amt.mod_date,chrg.cdm,chrg.qty,chrg.retail,chrg.inp_price,
	amt.cpt4,amt.type,amt.amount,chrg.chrg_num,cpt4.descript,client.print_cc,acc.cl_mnem,
	fin.type as fintype
FROM acc
JOIN chrg on (acc.account = chrg.account)
JOIN amt on (chrg.chrg_num = amt.chrg_num)
LEFT OUTER JOIN fin on (acc.fin_code = fin.fin_code) 
LEFT OUTER JOIN client on (acc.cl_mnem = client.cli_mnem)
LEFT OUTER JOIN cpt4 on (chrg.cdm = cpt4.cdm and amt.cpt4 = cpt4.cpt4)
