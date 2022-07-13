--
--SELECT * FROM dbo.vw_prg_report_by_plan_name
--
--SET QUOTED_IDENTIFIER ON
--SET ANSI_NULLS ON
--GO
CREATE VIEW [dbo].[vw_prg_report_by_plan_name]
AS
SELECT     distinct dbo.ins.ins_code
, dbo.ins.plan_nme
, dbo.acc.cl_mnem
, dbo.acc.account
, dbo.acc.pat_name
, dbo.acc.trans_date
, dbo.ins.ins_a_b_c
, dbo.acc.fin_code
, dbo.pat.mailer
, dbo.pat.ub_date
, dbo.pat.h1500_date
, dbo.ins.policy_num
, case when dbo.client.outpatient_billing = 1 and trans_date >= '04/01/2012 00:00'
	then 'OUTPATIENT'
	else 'REF LAB'
	end as [BILL TYPE]
FROM         dbo.acc 
RIGHT OUTER JOIN      dbo.chrg ON dbo.acc.account = dbo.chrg.account AND dbo.chrg.chrg_num IS NOT NULL 
LEFT OUTER JOIN       dbo.ins ON dbo.acc.account = dbo.ins.account 
LEFT OUTER JOIN       dbo.pat ON dbo.acc.account = dbo.pat.account
right outer join	  dbo.client on dbo.client.cli_mnem = dbo.acc.cl_mnem
WHERE    (NOT (dbo.acc.status IN ('PAID_OUT', 'CLOSED')))
GROUP BY client.outpatient_billing
,dbo.acc.fin_code, dbo.ins.plan_nme, dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.acc.trans_date, dbo.ins.ins_a_b_c, 
                      dbo.pat.batch_date, dbo.pat.mailer, dbo.acc.cl_mnem
, dbo.pat.ub_date
, dbo.pat.h1500_date
, dbo.ins.policy_num
, dbo.ins.ins_code
HAVING      (dbo.acc.trans_date > CONVERT(DATETIME, '2010-01-01 00:00:00', 102))

GO
EXECUTE sp_addextendedproperty @name = N'Description', @value = N'This is a test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_report_by_plan_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[44] 4[24] 2[19] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "acc"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 121
               Right = 412
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 113
               Left = 183
               Bottom = 253
               Right = 335
            End
            DisplayFlags = 280
            TopColumn = 16
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 6
               Left = 482
               Bottom = 160
               Right = 660
            End
            DisplayFlags = 280
            TopColumn = 37
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 17
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
    ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_report_by_plan_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'     SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 2460
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_report_by_plan_name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_report_by_plan_name';

