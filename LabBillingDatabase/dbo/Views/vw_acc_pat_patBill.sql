﻿CREATE VIEW dbo.vw_acc_pat_patBill
AS
SELECT  dbo.acc.account, dbo.acc.pat_name, dbo.acc.trans_date, dbo.acc.fin_code, dbo.acc.cl_mnem, dbo.acc.status, dbo.pat.dbill_date, dbo.pat.ub_date, dbo.pat.h1500_date, 
               dbo.pat.batch_date, dbo.pat.ebill_batch_date, dbo.pat.mailer, dbo.fin.h1500, dbo.fin.ub92, dbo.pat.phy_id, dbo.pat.ebill_batch_1500, dbo.pat.last_dm, dbo.pat.bd_list_date, 
               dbo.pat.claimsnet_1500_batch_date
FROM     dbo.acc LEFT OUTER JOIN
               dbo.pat ON dbo.acc.account = dbo.pat.account INNER JOIN
               dbo.fin ON dbo.acc.fin_code = dbo.fin.fin_code
WHERE  (NOT (dbo.acc.status IN ('PAID_OUT', 'CLOSED'))) AND (dbo.acc.trans_date < CONVERT(varchar(10), GETDATE(), 101)) AND (dbo.GetAccBalByDate(dbo.acc.account, GETDATE()) <> 0.00) 
               AND (COALESCE (NULLIF (dbo.pat.last_dm, ''), dbo.acc.trans_date) < DATEADD(mm, DATEDIFF(m, 0, GETDATE()) + 0, - .000003)) AND (dbo.acc.fin_code NOT IN ('W', 'X', 'Y', 'Z', 
               'CLIENT')) AND (dbo.pat.mailer <> 'N')

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[17] 4[21] 2[44] 3) )"
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
         Top = -96
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
            TopColumn = 0
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 121
               Right = 460
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "fin"
            Begin Extent = 
               Top = 6
               Left = 498
               Bottom = 121
               Right = 650
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 20
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
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_patBill';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_patBill';
