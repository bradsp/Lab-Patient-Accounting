CREATE VIEW dbo.vw_acc_by_cl_cnb
AS
SELECT     TOP (100) PERCENT dbo.acc.cl_mnem, dbo.client.cli_nme, dbo.client.type, dbo.chrg.status, dbo.chrg.cdm, SUM(dbo.chrg.qty) AS sumofqty, 
                      SUM(dbo.chrg.inp_price) AS grosscharge, dbo.chrg.mod_date, dbo.chrg.net_amt, dbo.chrg.inp_price, dbo.chrg.retail, dbo.cdm.descript
FROM         dbo.acc INNER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account INNER JOIN
                      dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm INNER JOIN
                      dbo.chrg_pa ON dbo.chrg.chrg_num = dbo.chrg_pa.chrg_num
GROUP BY dbo.acc.cl_mnem, dbo.client.cli_nme, dbo.client.type, dbo.chrg.status, dbo.chrg.cdm, dbo.chrg.mod_date, dbo.chrg.net_amt, dbo.chrg.inp_price, 
                      dbo.chrg.retail, dbo.cdm.descript, dbo.chrg.fin_type, dbo.chrg_pa.perform_site
HAVING      (dbo.chrg.cdm <> 'CBILL') AND (dbo.chrg.status <> 'N/A') AND (dbo.chrg.fin_type <> 'M') AND (dbo.chrg_pa.perform_site = 'ML' OR
                      dbo.chrg_pa.perform_site = 'MAYO')
ORDER BY dbo.acc.cl_mnem

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[30] 4[37] 2[4] 3) )"
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
         Left = -288
      End
      Begin Tables = 
         Begin Table = "acc"
            Begin Extent = 
               Top = 7
               Left = 257
               Bottom = 177
               Right = 425
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 3
               Left = 14
               Bottom = 171
               Right = 179
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 463
               Bottom = 176
               Right = 615
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 105
               Left = 703
               Bottom = 220
               Right = 871
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg_pa"
            Begin Extent = 
               Top = 0
               Left = 831
               Bottom = 214
               Right = 999
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
      Begin ColumnWidths = 13
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
      End
   End
   Begin CriteriaPane = 
     ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_by_cl_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' Begin ColumnWidths = 12
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_by_cl_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_by_cl_cnb';

