CREATE VIEW dbo.vw_Jama_PC_Billing
AS
SELECT     TOP (100) PERCENT dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.acc.trans_date, dbo.acc.mod_date, dbo.chrg.cdm, dbo.cpt4.cpt4, 
                      dbo.cpt4.type, SUM(dbo.chrg.qty) AS Expr1, dbo.pwise_cnb.TheDate
FROM         dbo.acc INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account INNER JOIN
                      dbo.cpt4 ON dbo.chrg.cdm = dbo.cpt4.cdm LEFT OUTER JOIN
                      dbo.pwise_cnb ON dbo.acc.account = dbo.pwise_cnb.TheAcct
GROUP BY dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.acc.trans_date, dbo.acc.mod_date, dbo.chrg.cdm, dbo.cpt4.cpt4, dbo.cpt4.type, 
                      dbo.pwise_cnb.TheDate
HAVING      (dbo.cpt4.type = 'pc') AND (dbo.chrg.cdm <> 'cbill') OR
                      (dbo.cpt4.type = 'tc')
ORDER BY dbo.acc.trans_date, dbo.acc.pat_name, dbo.acc.account

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[3] 2[5] 3) )"
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
         Begin Table = "chrg"
            Begin Extent = 
               Top = 0
               Left = 375
               Bottom = 115
               Right = 543
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "cpt4"
            Begin Extent = 
               Top = 21
               Left = 666
               Bottom = 136
               Right = 834
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pwise_cnb"
            Begin Extent = 
               Top = 142
               Left = 523
               Bottom = 242
               Right = 691
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
      Begin ColumnWidths = 11
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
         Or = 13', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Jama_PC_Billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'50
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Jama_PC_Billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_Jama_PC_Billing';

