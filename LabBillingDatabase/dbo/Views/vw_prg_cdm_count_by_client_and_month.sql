CREATE VIEW dbo.vw_prg_cdm_count_by_client_and_month
AS
SELECT     TOP (100) PERCENT dbo.chrg.cdm, dbo.chrg.qty, dbo.acc.trans_date, dbo.client.cli_nme, dbo.cdm.descript, dbo.client.cli_mnem, dbo.chrg.service_date,
                       dbo.chrg.account
FROM         dbo.chrg INNER JOIN
                      dbo.acc ON dbo.chrg.account = dbo.acc.account INNER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem INNER JOIN
                      dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm INNER JOIN
                      dbo.chk ON dbo.chrg.account = dbo.chk.account
GROUP BY dbo.chrg.cdm, dbo.chrg.qty, dbo.acc.trans_date, dbo.client.cli_nme, dbo.cdm.descript, dbo.client.cli_mnem, dbo.chrg.service_date, 
                      dbo.chrg.account
HAVING      (dbo.acc.trans_date > CONVERT(DATETIME, '2007-01-01 00:00:00', 102)) AND (dbo.chrg.cdm <> 'CBILL') AND 
                      (dbo.chrg.service_date > CONVERT(DATETIME, '2007-01-01 00:00:00', 102)) AND (SUM(dbo.chrg.qty) > 0)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[43] 2[3] 3) )"
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
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 6
               Left = 228
               Bottom = 209
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 6
               Left = 434
               Bottom = 121
               Right = 599
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 4
               Left = 635
               Bottom = 119
               Right = 787
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chk"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 197
            End
            DisplayFlags = 280
            TopColumn = 8
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1830
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
     ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_cdm_count_by_client_and_month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'    Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 450
         GroupBy = 2070
         Filter = 4110
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_cdm_count_by_client_and_month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_cdm_count_by_client_and_month';

