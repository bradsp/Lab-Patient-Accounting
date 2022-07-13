CREATE VIEW dbo.vw_hp_cnb
AS
SELECT     TOP (100) PERCENT dbo.acc.cl_mnem, dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.ins.ins_a_b_c, dbo.ins.ins_code, dbo.chrg.cdm, 
                      dbo.chrg.qty, dbo.acc.trans_date, dbo.cpt4.cpt4, dbo.cpt4.mprice, dbo.cpt4.modi, dbo.chrg.fin_type
FROM         dbo.acc INNER JOIN
                      dbo.ins ON dbo.acc.account = dbo.ins.account INNER JOIN
                      dbo.chrg ON dbo.ins.account = dbo.chrg.account RIGHT OUTER JOIN
                      dbo.cpt4 ON dbo.chrg.cdm = dbo.cpt4.cdm
WHERE     (dbo.ins.ins_code = 'HP') AND (dbo.acc.trans_date BETWEEN CONVERT(DATETIME, '2009-04-01 00:00:00', 102) AND CONVERT(DATETIME, 
                      '2009-06-30 23:59:59', 102)) AND (dbo.acc.fin_code = 'H') AND (dbo.chrg.cdm <> 'CBILL')
ORDER BY dbo.cpt4.cpt4

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[25] 4[37] 2[4] 3) )"
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
            TopColumn = 2
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 121
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 136
               Left = 154
               Bottom = 251
               Right = 306
            End
            DisplayFlags = 280
            TopColumn = 16
         End
         Begin Table = "cpt4"
            Begin Extent = 
               Top = 89
               Left = 507
               Bottom = 204
               Right = 659
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
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_hp_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'1350
         Filter = 3780
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_hp_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_hp_cnb';

