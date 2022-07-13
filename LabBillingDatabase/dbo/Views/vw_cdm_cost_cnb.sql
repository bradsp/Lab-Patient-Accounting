CREATE VIEW dbo.vw_cdm_cost_cnb
AS
SELECT     dbo.chrg_pa.perform_site, dbo.acc_location.pt_type, SUM(dbo.chrg.qty) AS SumOfqty, dbo.chrg.service_date, dbo.cdm_cost_cnb.THE_COST, 
                      dbo.chrg.cdm, dbo.acc.cl_mnem
FROM         dbo.acc LEFT OUTER JOIN
                      dbo.acc_location ON dbo.acc.account = dbo.acc_location.account INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account LEFT OUTER JOIN
                      dbo.chrg_pa ON dbo.chrg.chrg_num = dbo.chrg_pa.chrg_num LEFT OUTER JOIN
                      dbo.cdm_cost_cnb ON dbo.chrg.cdm = dbo.cdm_cost_cnb.THE_CDM
WHERE     (dbo.chrg.cdm <> 'CBILL')
GROUP BY dbo.chrg_pa.perform_site, dbo.acc_location.pt_type, dbo.chrg.service_date, dbo.cdm_cost_cnb.THE_COST, dbo.chrg.cdm, dbo.acc.cl_mnem
HAVING      (dbo.chrg_pa.perform_site = 'ML') AND (dbo.acc_location.pt_type = 'REF' OR
                      dbo.acc_location.pt_type IS NULL)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[54] 2[6] 3) )"
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
               Top = 17
               Left = 51
               Bottom = 132
               Right = 219
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "acc_location"
            Begin Extent = 
               Top = 174
               Left = 325
               Bottom = 289
               Right = 477
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 10
               Left = 333
               Bottom = 125
               Right = 485
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg_pa"
            Begin Extent = 
               Top = 3
               Left = 586
               Bottom = 118
               Right = 738
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cdm_cost_cnb"
            Begin Extent = 
               Top = 163
               Left = 587
               Bottom = 278
               Right = 739
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
      Begin ColumnWidths = 10
         Width = 284
         Width = 2595
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
   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cdm_cost_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'      Alias = 900
         Table = 1920
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 3135
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cdm_cost_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cdm_cost_cnb';

