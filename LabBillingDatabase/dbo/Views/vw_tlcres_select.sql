CREATE VIEW [dbo].[vw_tlcres_select]
AS
SELECT     dbo.acc.account, dbo.acc.pat_name, dbo.acc.cl_mnem, dbo.acc.fin_code, dbo.acc.trans_date, dbo.acc.status, dbo.acc.ssn, dbo.acc.meditech_account, 
                      dbo.acc.original_fincode, dbo.acc.mri, dbo.acc_track.event_date, dbo.acc_track.event_user, dbo.acc_track.event_prg, dbo.acc_track.event_host
FROM         dbo.acc LEFT OUTER JOIN
                      dbo.acc_track ON dbo.acc.account = dbo.acc_track.account
WHERE     (dbo.acc_track.track_code = 'TLCRES') AND (dbo.acc_track.event_date IS NULL)
