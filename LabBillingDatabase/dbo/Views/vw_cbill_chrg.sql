﻿/****** Object:  View dbo.vw_cbill_chrg    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW dbo.vw_cbill_chrg
AS
SELECT     dbo.chrg.account, dbo.acc.trans_date, SUM(dbo.chrg.qty) AS qty, SUM(dbo.chrg.inp_price * dbo.chrg.qty) AS inp_amt, 
                      SUM(dbo.chrg.retail * dbo.chrg.qty) AS retail, SUM(dbo.chrg.net_amt * dbo.chrg.qty) AS amount, dbo.chrg.cdm
FROM         dbo.chrg INNER JOIN
                      dbo.acc ON dbo.acc.account = dbo.chrg.account
WHERE     (dbo.chrg.status NOT IN ('CBILL', 'N/A')) AND (dbo.chrg.invoice IS NULL OR
                      dbo.chrg.invoice = '')
GROUP BY dbo.chrg.account, dbo.acc.trans_date, dbo.chrg.cdm
HAVING      (SUM(dbo.chrg.qty * dbo.chrg.net_amt) <> 0)

GO
EXECUTE sp_addextendedproperty @name = N'DESCRIPTION', @value = N'rgc/wdk 20090715 added [ * dbo.chrg.net_amt] to the having clause of this view to correct a problem with changing prices between the time we post the charge and we bill the charge. Reverse Charge gets the new correct price but the count of the qty''s is zero so they would never have shown on the next bill.
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cbill_chrg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 6
               Left = 244
               Bottom = 121
               Right = 428
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cbill_chrg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cbill_chrg';
