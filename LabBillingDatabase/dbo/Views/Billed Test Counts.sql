CREATE VIEW dbo.[Billed Test Counts]
AS
SELECT     TOP (100) PERCENT dbo.ClientFacility_wdk.facility, dbo.chrg_pa.perform_site, dbo.acc_location.pt_type, SUM(dbo.chrg.qty) AS SumOfqty, 
                      dbo.amt.mod_date, dbo.chrg.service_date
FROM         dbo.acc LEFT OUTER JOIN
                      dbo.acc_location ON dbo.acc.account = dbo.acc_location.account INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num LEFT OUTER JOIN
                      dbo.chrg_pa ON dbo.chrg.chrg_num = dbo.chrg_pa.chrg_num LEFT OUTER JOIN
                      dbo.ClientFacility_wdk ON dbo.acc.cl_mnem = dbo.ClientFacility_wdk.cli_mnem
WHERE     (dbo.chrg.cdm <> 'CBILL')
GROUP BY dbo.ClientFacility_wdk.facility, dbo.chrg_pa.perform_site, dbo.acc_location.pt_type, dbo.amt.mod_date, dbo.chrg.service_date
HAVING      (dbo.chrg.service_date BETWEEN '05/01/2010' AND '05/31/2010')
ORDER BY dbo.ClientFacility_wdk.facility

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
               Right = 222
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc_location"
            Begin Extent = 
               Top = 6
               Left = 260
               Bottom = 121
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 466
               Bottom = 121
               Right = 634
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "amt"
            Begin Extent = 
               Top = 6
               Left = 672
               Bottom = 121
               Right = 862
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg_pa"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 241
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ClientFacility_wdk"
            Begin Extent = 
               Top = 126
               Left = 244
               Bottom = 211
               Right = 412
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
    ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Billed Test Counts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'     Width = 1500
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
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Billed Test Counts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Billed Test Counts';

