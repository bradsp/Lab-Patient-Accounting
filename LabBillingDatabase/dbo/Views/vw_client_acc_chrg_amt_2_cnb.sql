﻿CREATE VIEW dbo.vw_client_acc_chrg_amt_2_cnb
AS
SELECT     dbo.client.cli_mnem, dbo.client.cli_nme, dbo.acc.account, dbo.acc.fin_code, dbo.amt.mod_date, dbo.chrg.qty
FROM         dbo.amt INNER JOIN
                      dbo.chrg ON dbo.amt.chrg_num = dbo.chrg.chrg_num INNER JOIN
                      dbo.client INNER JOIN
                      dbo.acc ON dbo.client.cli_mnem = dbo.acc.cl_mnem ON dbo.chrg.account = dbo.acc.account
WHERE     (dbo.chrg.cdm <> 'CBILL') AND (NOT (dbo.client.cli_mnem IN ('BCH', 'CGH', 'COM', 'GGH', 'HG')))

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[43] 2[9] 3) )"
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
         Begin Table = "amt"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 121
               Right = 402
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 203
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 126
               Left = 241
               Bottom = 241
               Right = 409
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
      Begin ColumnWidths = 9
         Width = 284
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
         SortOrder = 75
         GroupBy = 1350
         Filter = 3630
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_client_acc_chrg_amt_2_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_client_acc_chrg_amt_2_cnb';

