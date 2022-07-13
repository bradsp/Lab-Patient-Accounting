CREATE VIEW [dbo].[vw_paperwise_index]
AS
SELECT     account, pat_name, cl_mnem, fin_code, trans_date, ssn, meditech_account, mri, status
FROM         dbo.acc
