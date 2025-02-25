﻿CREATE VIEW dbo.VW_acct_chrg_rollup_cnb
AS
SELECT     TOP (10) PERCENT dbo.client.cli_mnem, SUM(dbo.chrg.net_amt) AS Expr1, dbo.acc.fin_code, dbo.chrg.fin_type, dbo.chrg.account, 
                      dbo.chrg.service_date
FROM         dbo.client INNER JOIN
                      dbo.acc ON dbo.client.cli_mnem = dbo.acc.cl_mnem INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account
WHERE     (dbo.chrg.service_date BETWEEN CONVERT(DATETIME, '2009-06-01 00:00:00', 102) AND CONVERT(DATETIME, '2009-06-30 00:00:00', 102)) AND 
                      (dbo.acc.status <> 'N/A') AND (dbo.chrg.cdm <> 'CBILL')
GROUP BY dbo.client.cli_mnem, dbo.chrg.fin_type, dbo.acc.fin_code, dbo.chrg.service_date, dbo.chrg.account, dbo.chrg.service_date WITH CUBE
HAVING      (NOT (dbo.client.cli_mnem IS NULL)) AND (NOT (dbo.chrg.account IS NULL)) AND (NOT (SUM(dbo.chrg.net_amt) IS NULL)) AND 
                      (NOT (dbo.acc.fin_code IS NULL)) AND (NOT (dbo.chrg.fin_type IS NULL)) AND (NOT (dbo.chrg.service_date IS NULL))
ORDER BY dbo.client.cli_mnem, dbo.chrg.account

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[18] 4[21] 2[4] 3) )"
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
         Begin Table = "client"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 40
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 6
               Left = 257
               Bottom = 121
               Right = 441
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 479
               Bottom = 121
               Right = 647
            End
            DisplayFlags = 280
            TopColumn = 16
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2040
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
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_acct_chrg_rollup_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_acct_chrg_rollup_cnb';

