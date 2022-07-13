CREATE VIEW [dbo].[vw_prg_client_ClientPaid_by_year_2007]
AS
SELECT     TOP (100) PERCENT dbo.client.cli_nme AS cli_name, SUM(dbo.chk.amt_paid) AS PAID, SUM(dbo.chk.amt_paid + dbo.chk.contractual + dbo.chk.write_off)
                       AS CHARGED, DATEPART(Year, dbo.chk.date_rec) AS Year, dbo.acc.cl_mnem
FROM         dbo.acc INNER JOIN
                      dbo.chk ON dbo.acc.account = dbo.chk.account INNER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem
WHERE     (dbo.acc.fin_code = 'client')
GROUP BY DATEPART(Year, dbo.chk.date_rec), dbo.acc.cl_mnem, dbo.client.cli_nme
HAVING      (DATEPART(Year, dbo.chk.date_rec) = '2007')
ORDER BY cli_name


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[13] 4[29] 2[24] 3) )"
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
         Begin Table = "chk"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 197
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 11
               Left = 243
               Bottom = 126
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 6
               Left = 449
               Bottom = 121
               Right = 614
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
         Width = 2490
         Width = 1500
         Width = 3075
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 3150
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 390
         SortOrder = 660
         GroupBy = 1350
         Filter = 4725
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_client_ClientPaid_by_year_2007';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_client_ClientPaid_by_year_2007';

