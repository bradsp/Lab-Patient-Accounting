CREATE VIEW dbo.vw_age_rep_cnb
AS
SELECT     TOP (100) PERCENT dbo.aging_history.account, dbo.acc.fin_code, dbo.fin.res_party, DATEDIFF(dd, dbo.acc.trans_date, dbo.aging_history.datestamp) 
                      AS DaysOld, dbo.aging_history.balance, dbo.acc.pat_name, dbo.acc.trans_date, dbo.aging_history.datestamp, dbo.client.cli_nme, dbo.client.type, 
                      dbo.pat.mailer
FROM         dbo.aging_history INNER JOIN
                      dbo.acc ON dbo.aging_history.account = dbo.acc.account INNER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem LEFT OUTER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account LEFT OUTER JOIN
                      dbo.fin ON dbo.acc.fin_code = dbo.fin.fin_code

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[54] 4[5] 2[4] 3) )"
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
         Left = -509
      End
      Begin Tables = 
         Begin Table = "aging_history"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 106
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 2
               Left = 252
               Bottom = 163
               Right = 420
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 56
               Left = 691
               Bottom = 171
               Right = 856
            End
            DisplayFlags = 280
            TopColumn = 16
         End
         Begin Table = "fin"
            Begin Extent = 
               Top = 124
               Left = 818
               Bottom = 239
               Right = 970
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 4
               Left = 805
               Bottom = 119
               Right = 1021
            End
            DisplayFlags = 280
            TopColumn = 23
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
   Begin CriteriaPane =', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_age_rep_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' 
      Begin ColumnWidths = 11
         Column = 2595
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_age_rep_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_age_rep_cnb';

