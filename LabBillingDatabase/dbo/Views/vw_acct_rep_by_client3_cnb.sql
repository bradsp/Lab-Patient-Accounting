CREATE VIEW dbo.vw_acct_rep_by_client3_cnb
AS
SELECT     TOP (100) PERCENT dbo.client.cli_mnem, dbo.client.cli_nme, dbo.chrg.cdm, dbo.cdm.descript, SUM(dbo.chrg.qty) AS Expr1, dbo.chrg.inp_price, 
                      dbo.chrg.retail, dbo.chrg.net_amt, dbo.acc.status, dbo.chrg.mod_date, dbo.chk.amt_paid, dbo.acc.account, dbo.chk.date_rec
FROM         dbo.client INNER JOIN
                      dbo.acc ON dbo.client.cli_mnem = dbo.acc.cl_mnem INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account INNER JOIN
                      dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm INNER JOIN
                      dbo.chrg_pa ON dbo.chrg.chrg_num = dbo.chrg_pa.chrg_num INNER JOIN
                      dbo.chk ON dbo.acc.account = dbo.chk.account
GROUP BY dbo.client.cli_mnem, dbo.client.cli_nme, dbo.cdm.descript, dbo.chrg.net_amt, dbo.chrg.retail, dbo.chrg.inp_price, dbo.chrg.cdm, dbo.chrg.mod_date, 
                      dbo.acc.status, dbo.chrg_pa.perform_site, dbo.chrg.fin_type, dbo.chk.amt_paid, dbo.acc.account, dbo.chk.date_rec
HAVING      (dbo.chrg.cdm <> 'CBILL') AND (dbo.acc.status <> 'N/A') AND (dbo.chrg_pa.perform_site = 'ML' OR
                      dbo.chrg_pa.perform_site = 'MAYO') AND (dbo.chrg.fin_type <> 'M')
ORDER BY dbo.client.cli_mnem, dbo.chrg.cdm

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[54] 4[5] 2[8] 3) )"
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
            TopColumn = 1
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 0
               Left = 246
               Bottom = 115
               Right = 430
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 16
               Left = 600
               Bottom = 131
               Right = 768
            End
            DisplayFlags = 280
            TopColumn = 12
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 163
               Left = 45
               Bottom = 278
               Right = 213
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg_pa"
            Begin Extent = 
               Top = 166
               Left = 276
               Bottom = 281
               Right = 444
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chk"
            Begin Extent = 
               Top = 155
               Left = 588
               Bottom = 270
               Right = 763
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 16
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acct_rep_by_client3_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
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
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acct_rep_by_client3_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acct_rep_by_client3_cnb';

