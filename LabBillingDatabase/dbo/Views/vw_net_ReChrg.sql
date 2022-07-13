/****** Object:  View dbo.vw_net_ReChrg    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_net_ReChrg] AS
select account, cdm, SUM(qty) sum_qty, SUM(qty * amount) sum_amt, host_name from ReChrg
group by cdm, host_name, account
