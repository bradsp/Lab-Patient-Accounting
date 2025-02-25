﻿CREATE VIEW dbo.vw_prg_cpt4_by_account
AS
SELECT     TOP (100) PERCENT dbo.chrg.account, dbo.chrg.cdm, SUM(DISTINCT dbo.chrg.qty) AS cdm_qty, dbo.amt.cpt4, dbo.chrg.service_date, dbo.cpt4.descript, 
                      dbo.amt.diagnosis_code_ptr, dbo.amt.chrg_num, dbo.amt.amount AS total
FROM         dbo.amt INNER JOIN
                      dbo.chrg ON dbo.amt.chrg_num = dbo.chrg.chrg_num LEFT OUTER JOIN
                      dbo.cpt4 ON dbo.chrg.cdm = dbo.cpt4.cdm AND dbo.amt.cpt4 = dbo.cpt4.cpt4 LEFT OUTER JOIN
                      dbo.acc ON dbo.chrg.account = dbo.acc.account
WHERE     (dbo.acc.status NOT IN ('closed', 'paid_out')) AND (dbo.chrg.credited = 0)
GROUP BY dbo.chrg.account, dbo.chrg.cdm, dbo.amt.cpt4, dbo.chrg.service_date, dbo.acc.account, dbo.cpt4.descript, dbo.amt.diagnosis_code_ptr, 
                      dbo.amt.chrg_num, dbo.amt.amount
HAVING      (NOT (dbo.acc.account IN
                          (SELECT     cli_mnem
                            FROM          dbo.client))) AND (NOT (dbo.chrg.cdm IN ('CBILL'))) AND (SUM(DISTINCT dbo.chrg.qty) <> 0)
ORDER BY dbo.acc.account

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[23] 4[47] 2[25] 3) )"
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
         Begin Table = "amt"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 228
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 266
               Bottom = 121
               Right = 434
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cpt4"
            Begin Extent = 
               Top = 10
               Left = 490
               Bottom = 125
               Right = 658
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 9
               Left = 737
               Bottom = 124
               Right = 921
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
         Filter = 3765
         Or = 1350
         Or = 1350
  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_cpt4_by_account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'       Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_cpt4_by_account';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_cpt4_by_account';

