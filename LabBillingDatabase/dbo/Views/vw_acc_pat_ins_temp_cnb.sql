CREATE VIEW dbo.vw_acc_pat_ins_temp_cnb
AS
SELECT     dbo.acc.account, dbo.acc.fin_code, dbo.acc.pat_name, dbo.pat.pat_addr1, dbo.pat.city_st_zip, dbo.pat.dob_yyyy, dbo.pat.sex, dbo.pat.relation, 
                      dbo.Ins_temp_cnb.InsCode1, dbo.Ins_temp_cnb.InsName1, dbo.Ins_temp_cnb.InsPol1, dbo.Ins_temp_cnb.InsGrp1, dbo.Ins_temp_cnb.InsCode2, 
                      dbo.Ins_temp_cnb.InsPol2, dbo.Ins_temp_cnb.InsName2, dbo.Ins_temp_cnb.InsGrp2, dbo.Ins_temp_cnb.InsCode3, dbo.Ins_temp_cnb.InsName3, 
                      dbo.Ins_temp_cnb.InsPol3, dbo.Ins_temp_cnb.InsGrp3, dbo.acc.deleted, dbo.pat.guarantor, dbo.pat.guar_addr, dbo.pat.g_city_st
FROM         dbo.acc INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account INNER JOIN
                      dbo.Ins_temp_cnb ON dbo.acc.account = dbo.Ins_temp_cnb.Account
WHERE     (dbo.acc.deleted = 0)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[21] 4[4] 2[4] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1[50] 4[25] 3) )"
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
      ActivePaneConfig = 1
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
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
            TopColumn = 0
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 17
               Left = 271
               Bottom = 132
               Right = 487
            End
            DisplayFlags = 280
            TopColumn = 11
         End
         Begin Table = "Ins_temp_cnb"
            Begin Extent = 
               Top = 6
               Left = 498
               Bottom = 121
               Right = 650
            End
            DisplayFlags = 280
            TopColumn = 12
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 21
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
         Width = 2145
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
         Filter = 1350
         Or = 1350
         Or = 1350
  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_ins_temp_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'       Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_ins_temp_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_ins_temp_cnb';

