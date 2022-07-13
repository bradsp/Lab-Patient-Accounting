CREATE VIEW dbo.vw_wc_billing_list_exc_cnb
AS
SELECT        TOP (100) PERCENT dbo.acc.cl_mnem, dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.acc.trans_date, dbo.chrg.cdm, dbo.cdm.descript, dbo.chrg.qty, 
                         dbo.chrg.retail, dbo.chrg.net_amt
FROM            dbo.chrg INNER JOIN
                         dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm INNER JOIN
                         dbo.acc ON dbo.chrg.account = dbo.acc.account
WHERE        (dbo.acc.cl_mnem = 'WC') AND (dbo.acc.fin_code = 'Y') AND (dbo.chrg.cdm = '6322134' OR
                         dbo.chrg.cdm = '5565222' OR
                         dbo.chrg.cdm = '5527276' OR
                         dbo.chrg.cdm = '5565248' OR
                         dbo.chrg.cdm = '5529617' OR
                         dbo.chrg.cdm = '5527821' OR
                         dbo.chrg.cdm = '5528570' OR
                         dbo.chrg.cdm = '5528384' OR
                         dbo.chrg.cdm = '5565170' OR
                         dbo.chrg.cdm = '5687030' OR
                         dbo.chrg.cdm = '5527961' OR
                         dbo.chrg.cdm = '5529120' OR
                         dbo.chrg.cdm = '5687032' OR
                         dbo.chrg.cdm = '5949016' OR
                         dbo.chrg.cdm = '5527418' OR
                         dbo.chrg.cdm = '5525259' OR
                         dbo.chrg.cdm = '5565120' OR
                         dbo.chrg.cdm = '5325020' OR
                         dbo.chrg.cdm = '5529165' OR
                         dbo.chrg.cdm = '5687042' OR
                         dbo.chrg.cdm = '5527628' OR
                         dbo.chrg.cdm = '5525300' OR
                         dbo.chrg.cdm = '6322114' OR
                         dbo.chrg.cdm = '6321004' OR
                         dbo.chrg.cdm = '5687064' OR
                         dbo.chrg.cdm = '5525970' OR
                         dbo.chrg.cdm = '5529605' OR
                         dbo.chrg.cdm = '5529345' OR
                         dbo.chrg.cdm = '5528195' OR
                         dbo.chrg.cdm = '5529341' OR
                         dbo.chrg.cdm = '5642260' OR
                         dbo.chrg.cdm = '5642326' OR
                         dbo.chrg.cdm = '5642264' OR
                         dbo.chrg.cdm = '5642262' OR
                         dbo.chrg.cdm BETWEEN '5929160' AND '5929116' OR
                         dbo.chrg.cdm BETWEEN '5939106' AND '5939117' OR
                         dbo.chrg.cdm = '5687026' OR
                         dbo.chrg.cdm = '5527058' OR
                         dbo.chrg.cdm = '5525298' OR
                         dbo.chrg.cdm = '5545096' OR
                         dbo.chrg.cdm = '5527759' OR
                         dbo.chrg.cdm = '5527080' OR
                         dbo.chrg.cdm = '5525299' OR
                         dbo.chrg.cdm = '5528965' OR
                         dbo.chrg.cdm = '5529230' OR
                         dbo.chrg.cdm = '5325094' OR
                         dbo.chrg.cdm = '5322126' OR
                         dbo.chrg.cdm = '5325048' OR
                         dbo.chrg.cdm = '5526060' OR
                         dbo.chrg.cdm = '5565184' OR
                         dbo.chrg.cdm = '5565188' OR
                         dbo.chrg.cdm = '5525600' OR
                         dbo.chrg.cdm = '5949338')
ORDER BY dbo.acc.pat_name, dbo.chrg.cdm, dbo.acc.account

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[33] 4[12] 2[28] 3) )"
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
         Begin Table = "chrg"
            Begin Extent = 
               Top = 0
               Left = 327
               Bottom = 115
               Right = 479
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 233
               Left = 381
               Bottom = 348
               Right = 533
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 1
               Left = 62
               Bottom = 116
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1260
         Width = 1500
         Width = 1500
         Width = 2685
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
         Output = 1365
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 47070
         Or = 1350
         Or = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_wc_billing_list_exc_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_wc_billing_list_exc_cnb';

