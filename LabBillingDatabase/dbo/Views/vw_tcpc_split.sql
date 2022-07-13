CREATE VIEW [dbo].[vw_tcpc_split]
AS
SELECT dbo.chrg.chrg_num, dbo.chrg.cdm, dbo.chrg.qty, 
    dbo.chrg.retail, dbo.chrg.inp_price, dbo.chrg.fin_type, 
    dbo.amt.type, dbo.amt.amount, dbo.amt.mod_date
FROM dbo.amt INNER JOIN
    dbo.chrg ON dbo.amt.chrg_num = dbo.chrg.chrg_num
WHERE (NOT (dbo.chrg.cdm IN ('5949040', '5949044', '5949018', 
    '5959041'))) AND (dbo.amt.type IN ('TC', 'PC'))
