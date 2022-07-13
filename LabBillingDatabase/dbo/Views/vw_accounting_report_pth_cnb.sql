CREATE VIEW dbo.vw_accounting_report_pth_cnb
AS
SELECT     dbo.chrg.chrg_num, dbo.chrg.status, dbo.acc.cl_mnem, dbo.acc.fin_code, dbo.chrg.cdm, dbo.chrg.qty, dbo.chrg.retail, dbo.chrg.inp_price, 
                      dbo.chrg.net_amt, dbo.chrg.fin_type, dbo.chrg_pa.pa_amount, dbo.client.print_cc, dbo.client.cli_nme, dbo.chrg_pa.perform_site, dbo.chrg.mod_date, 
                      dbo.chrg.service_date, dbo.chrg.hist_date, dbo.amt.cpt4, dbo.amt.type, dbo.cdm.descript
FROM         dbo.acc INNER JOIN
                      dbo.chrg ON dbo.acc.account = dbo.chrg.account INNER JOIN
                      dbo.chrg_pa ON dbo.chrg.chrg_num = dbo.chrg_pa.chrg_num INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num INNER JOIN
                      dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm LEFT OUTER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem
WHERE     (dbo.chrg.cdm <> 'CBILL') AND (dbo.chrg.cdm BETWEEN '5920000' AND '5949999') AND (dbo.chrg.status <> 'N/A') AND (dbo.amt.type <> 'PC')

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[58] 4[2] 2[26] 3) )"
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
         Top = -51
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
            TopColumn = 18
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 121
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "chrg_pa"
            Begin Extent = 
               Top = 139
               Left = 453
               Bottom = 254
               Right = 605
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "amt"
            Begin Extent = 
               Top = 0
               Left = 511
               Bottom = 115
               Right = 685
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 246
               Left = 572
               Bottom = 361
               Right = 724
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 143
               Left = 193
               Bottom = 258
               Right = 358
            End
            DisplayFlags = 280
            TopColumn = 11
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 21
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_accounting_report_pth_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'500
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
         GroupBy = 1350
         Filter = 1545
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_accounting_report_pth_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_accounting_report_pth_cnb';

