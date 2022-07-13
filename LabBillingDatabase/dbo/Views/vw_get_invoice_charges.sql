CREATE VIEW dbo.vw_get_invoice_charges
AS
SELECT     TOP (100) PERCENT CONVERT(VARCHAR(10), dbo.chrg.service_date, 101) AS DATE, dbo.chrg.account, dbo.acc.pat_name AS [PATIENT NAME], 
                      dbo.chrg.cdm AS [CHARGE CODE], dt.QTY, dbo.cdm.descript AS [CHARGE DESCRIPTION], SUM(dbo.chrg.qty * dbo.amt.amount) AS AMOUNT, 
                      dbo.chrg.invoice
FROM         dbo.chrg INNER JOIN
                      dbo.amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num INNER JOIN
                      dbo.acc ON dbo.acc.account = dbo.chrg.account LEFT OUTER JOIN
                      dbo.cdm ON dbo.cdm.cdm = dbo.chrg.cdm AND dbo.cdm.deleted = 0 INNER JOIN
                          (SELECT     account, cdm, SUM(qty) AS QTY
                            FROM          dbo.chrg AS chrg_1
                            WHERE      (status NOT IN ('N/A', 'CBILL')) AND (credited = 0) AND (cdm <> 'CBILL') AND (NULLIF (invoice, '') IS NOT NULL)
                            GROUP BY cdm, account) AS dt ON dt.account = dbo.chrg.account AND dt.cdm = dbo.chrg.cdm
WHERE     (dbo.chrg.status NOT IN ('N/A', 'CBILL')) AND (dbo.chrg.credited = 0) AND (dbo.chrg.cdm <> 'CBILL') AND (NULLIF (dbo.chrg.invoice, '') IS NOT NULL)
GROUP BY dbo.chrg.service_date, dbo.chrg.account, dbo.acc.pat_name, dbo.chrg.cdm, dbo.cdm.descript, dbo.chrg.invoice, dt.QTY
HAVING      (SUM(dbo.chrg.qty) <> 0)
ORDER BY dbo.chrg.account, [CHARGE CODE]

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[32] 4[22] 2[37] 3) )"
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
               Top = 6
               Left = 38
               Bottom = 121
               Right = 255
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "amt"
            Begin Extent = 
               Top = 6
               Left = 293
               Bottom = 121
               Right = 483
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 6
               Left = 521
               Bottom = 121
               Right = 705
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 6
               Left = 743
               Bottom = 121
               Right = 922
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "dt"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 226
               Right = 206
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
         ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_get_invoice_charges';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'Output = 720
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_get_invoice_charges';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_get_invoice_charges';

