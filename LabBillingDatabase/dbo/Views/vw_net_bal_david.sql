CREATE VIEW dbo.vw_net_bal_david
AS
SELECT     TOP (100) PERCENT dbo.vw_net_charge_david.account, SUM(CAST(dbo.vw_net_charge_david.Total_chg AS money)) AS Charges, 
                      SUM(CAST(dbo.vw_net_pay_david.TOTAL_CHK AS money)) AS Payments, 
                      CAST(dbo.vw_net_charge_david.Total_chg - CAST(dbo.vw_net_pay_david.TOTAL_CHK AS money) AS money) AS Balance, 
                      dbo.vw_net_charge_david.service_date, dbo.vw_net_pay_david.last_pay_date_rec
FROM         dbo.vw_net_charge_david LEFT OUTER JOIN
                      dbo.vw_net_pay_david ON dbo.vw_net_charge_david.account = dbo.vw_net_pay_david.account
GROUP BY dbo.vw_net_charge_david.account, CAST(dbo.vw_net_charge_david.Total_chg - CAST(dbo.vw_net_pay_david.TOTAL_CHK AS money) AS money), 
                      dbo.vw_net_charge_david.service_date, dbo.vw_net_pay_david.last_pay_date_rec
HAVING      (SUM(CAST(dbo.vw_net_charge_david.Total_chg AS money)) > 0)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[21] 4[41] 2[17] 3) )"
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
         Begin Table = "vw_net_charge_david"
            Begin Extent = 
               Top = 8
               Left = 14
               Bottom = 106
               Right = 182
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_net_pay_david"
            Begin Extent = 
               Top = 5
               Left = 269
               Bottom = 111
               Right = 437
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
         Column = 6960
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 885
         SortOrder = 930
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_net_bal_david';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_net_bal_david';

