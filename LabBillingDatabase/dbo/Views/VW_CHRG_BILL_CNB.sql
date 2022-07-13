CREATE VIEW dbo.VW_CHRG_BILL_CNB
AS
SELECT     dbo.acc.account, dbo.acc.trans_date, SUM(dbo.chrg.qty) AS qty, SUM(dbo.amt.amount * dbo.chrg.qty) AS amount, dbo.chrg.cdm, dbo.amt.cpt4, 
                      dbo.amt.type, CASE WHEN dbo.amt.modi = '' THEN NULL ELSE dbo.amt.modi END AS modi, dbo.amt.revcode, dbo.vw_cdm.cpt4_desc, 
                      dbo.amt.uri
FROM         dbo.vw_cdm INNER JOIN
                      dbo.amt ON dbo.vw_cdm.cpt4 = dbo.amt.cpt4 LEFT OUTER JOIN
                      dbo.acc INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account ON dbo.amt.chrg_num = dbo.chrg.chrg_num
WHERE     (dbo.amt.type <> 'N/A') AND (dbo.chrg.credited = 0) AND (dbo.chrg.cdm <> 'CBILL') AND (dbo.chrg.status <> 'CBILL') AND (dbo.chrg.invoice IS NULL)
GROUP BY dbo.acc.account, dbo.acc.trans_date, dbo.chrg.cdm, dbo.amt.cpt4, dbo.amt.type, dbo.amt.modi, dbo.amt.revcode, dbo.vw_cdm.cpt4_desc, 
                      dbo.amt.uri
HAVING      (SUM(dbo.chrg.qty) <> 0)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[61] 4[4] 2[4] 3) )"
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
               Top = 143
               Left = 398
               Bottom = 258
               Right = 582
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 242
               Left = 214
               Bottom = 357
               Right = 382
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "amt"
            Begin Extent = 
               Top = 0
               Left = 0
               Bottom = 115
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_cdm"
            Begin Extent = 
               Top = 23
               Left = 510
               Bottom = 138
               Right = 678
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_CHRG_BILL_CNB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'VW_CHRG_BILL_CNB';

