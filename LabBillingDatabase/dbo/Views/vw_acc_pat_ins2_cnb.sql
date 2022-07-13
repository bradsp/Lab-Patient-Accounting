CREATE VIEW dbo.vw_acc_pat_ins2_cnb
AS
SELECT     TOP (100) PERCENT dbo.acc.account, dbo.acc.trans_date, dbo.ins.policy_num, dbo.pat.dob_yyyy, dbo.acc.pat_name, dbo.pat.phy_id, 
                      dbo.phy.first_name, dbo.phy.last_name, dbo.pat.icd9_1, dbo.pat.icd9_2, dbo.pat.icd9_3, dbo.pat.icd9_4, dbo.ins.ins_a_b_c, dbo.acc.fin_code, 
                      dbo.phy.tnh_num, dbo.phy.upin
FROM         dbo.phy INNER JOIN
                      dbo.ins INNER JOIN
                      dbo.acc INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account ON dbo.ins.account = dbo.acc.account ON dbo.phy.upin = dbo.pat.phy_id
WHERE     (dbo.acc.fin_code = 'D') OR
                      (dbo.acc.fin_code = 'B')
ORDER BY dbo.acc.pat_name, dbo.acc.trans_date

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[9] 4[34] 2[34] 3) )"
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
         Begin Table = "phy"
            Begin Extent = 
               Top = 19
               Left = 675
               Bottom = 134
               Right = 827
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 247
               Left = 340
               Bottom = 362
               Right = 492
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 40
               Left = 52
               Bottom = 224
               Right = 220
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 13
               Left = 342
               Bottom = 128
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 27
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 18
         Width = 284
         Width = 1500
         Width = 2835
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
         Width = 720
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
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_ins2_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 3285
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_ins2_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_ins2_cnb';

