CREATE VIEW dbo.vw_pat_ins3_cnb
AS
SELECT     TOP (100) PERCENT dbo.pat.account, dbo.ins.policy_num, dbo.pat.dob_yyyy, dbo.pat.phy_id, dbo.phy.upin, dbo.phy.first_name, dbo.phy.last_name, 
                      dbo.pat.icd9_1, dbo.pat.icd9_2, dbo.pat.icd9_3, dbo.pat.icd9_4, dbo.phy.tnh_num, dbo.ins.ins_a_b_c, dbo.chrg.cdm, dbo.cpt4.cpt4
FROM         dbo.phy RIGHT OUTER JOIN
                      dbo.chrg INNER JOIN
                      dbo.pat INNER JOIN
                      dbo.ins ON dbo.pat.account = dbo.ins.account ON dbo.chrg.account = dbo.pat.account RIGHT OUTER JOIN
                      dbo.cpt4 ON dbo.chrg.cdm = dbo.cpt4.cdm ON dbo.phy.tnh_num = dbo.pat.phy_id

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[41] 2[5] 3) )"
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
         Left = -206
      End
      Begin Tables = 
         Begin Table = "cpt4"
            Begin Extent = 
               Top = 117
               Left = 680
               Bottom = 232
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 82
               Left = 221
               Bottom = 197
               Right = 390
            End
            DisplayFlags = 280
            TopColumn = 27
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 121
               Left = 527
               Bottom = 236
               Right = 679
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 2
               Left = 532
               Bottom = 116
               Right = 684
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "phy"
            Begin Extent = 
               Top = 244
               Left = 533
               Bottom = 359
               Right = 685
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
      Begin ColumnWidths = 19
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
   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_pat_ins3_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'      Width = 1500
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
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_pat_ins3_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_pat_ins3_cnb';

