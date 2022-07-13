CREATE VIEW dbo.View_acc_chrg_blist_cnb
AS
SELECT     TOP (100) PERCENT dbo.acc.account, dbo.acc.pat_name, dbo.acc.cl_mnem, dbo.acc.fin_code, dbo.acc.trans_date, SUM(dbo.chrg.qty) AS Expr1, 
                      dbo.chrg.cdm, dbo.cdm.descript, dbo.chrg.chrg_num, dbo.chrg.net_amt, dbo.chrg.retail
FROM         dbo.client RIGHT OUTER JOIN
                      dbo.acc RIGHT OUTER JOIN
                      dbo.chrg INNER JOIN
                      dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm ON dbo.acc.account = dbo.chrg.account ON dbo.client.cli_mnem = dbo.acc.cl_mnem
GROUP BY dbo.acc.account, dbo.acc.pat_name, dbo.acc.cl_mnem, dbo.acc.fin_code, dbo.acc.trans_date, dbo.chrg.cdm, dbo.chrg.chrg_num, dbo.cdm.descript, 
                      dbo.chrg.net_amt, dbo.chrg.retail
HAVING      (dbo.acc.fin_code NOT IN ('CLIENT', 'W')) AND (dbo.acc.trans_date BETWEEN CONVERT(DATETIME, '2015-09-01 00:00:00', 102) AND 
                      CONVERT(DATETIME, '2015-09-25 23:59:00', 102))
ORDER BY dbo.acc.cl_mnem, dbo.chrg.cdm, dbo.acc.account

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[21] 4[27] 2[2] 3) )"
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
               Top = 0
               Left = 0
               Bottom = 115
               Right = 183
            End
            DisplayFlags = 280
            TopColumn = 19
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 31
               Left = 321
               Bottom = 146
               Right = 538
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 6
               Left = 642
               Bottom = 121
               Right = 821
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 188
               Left = 293
               Bottom = 303
               Right = 496
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
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
         SortType = 1350
         SortOrder = 1410
         GroupBy = ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_acc_chrg_blist_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'1350
         Filter = 3960
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_acc_chrg_blist_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'View_acc_chrg_blist_cnb';

