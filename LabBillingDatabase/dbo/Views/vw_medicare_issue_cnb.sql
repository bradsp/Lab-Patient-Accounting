CREATE VIEW dbo.vw_medicare_issue_cnb
AS
SELECT     dbo.chrg.account, dbo.acc.pat_name, dbo.chrg.cdm, dbo.acc.fin_code, dbo.acc.trans_date, dbo.cdm.descript, dbo.pat.icd9_1, dbo.pat.icd9_2, 
                      dbo.pat.icd9_3, dbo.pat.icd9_4, dbo.pat.icd9_5, dbo.pat.icd9_6, dbo.pat.icd9_7, dbo.pat.icd9_8, dbo.pat.icd9_9
FROM         dbo.chrg INNER JOIN
                      dbo.acc ON dbo.chrg.account = dbo.acc.account INNER JOIN
                      dbo.cdm ON dbo.chrg.cdm = dbo.cdm.cdm INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account
WHERE     (dbo.acc.fin_code = 'A') AND (dbo.chrg.cdm <> 'cbill')

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[24] 2[5] 3) )"
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
               Top = 1
               Left = 0
               Bottom = 116
               Right = 152
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 0
               Left = 184
               Bottom = 115
               Right = 352
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cdm"
            Begin Extent = 
               Top = 149
               Left = 196
               Bottom = 264
               Right = 348
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 5
               Left = 370
               Bottom = 120
               Right = 586
            End
            DisplayFlags = 280
            TopColumn = 22
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 17
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
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_medicare_issue_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_medicare_issue_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_medicare_issue_cnb';

