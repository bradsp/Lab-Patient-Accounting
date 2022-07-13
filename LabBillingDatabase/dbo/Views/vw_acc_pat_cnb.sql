CREATE VIEW dbo.vw_acc_pat_cnb
AS
SELECT     dbo.acc.account, dbo.acc.pat_name, dbo.acc.trans_date, dbo.acc.cl_mnem, dbo.pat.phy_id, dbo.acc.deleted, dbo.pat.ssn, dbo.pat.pat_addr1, 
                      dbo.pat.city_st_zip, dbo.pat.dob_yyyy, dbo.pat.sex, dbo.pat.relation, dbo.pat.guarantor, dbo.pat.guar_addr, dbo.pat.g_city_st, dbo.pat.pat_marital, 
                      dbo.pat.icd9_1, dbo.pat.icd9_2, dbo.pat.icd9_3, dbo.pat.icd9_4, dbo.pat.icd9_8, dbo.pat.icd9_7, dbo.pat.icd9_6, dbo.pat.icd9_5, dbo.pat.icd9_9, 
                      dbo.acc.mri, dbo.acc.ov_pat_id, dbo.pat.rowguid, dbo.ins.ins_a_b_c
FROM         dbo.acc INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account LEFT OUTER JOIN
                      dbo.ins ON dbo.acc.account = dbo.ins.account
WHERE     (dbo.ins.ins_a_b_c IS NULL)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[47] 4[10] 2[9] 3) )"
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
         Begin Table = "acc"
            Begin Extent = 
               Top = 3
               Left = 23
               Bottom = 210
               Right = 191
            End
            DisplayFlags = 280
            TopColumn = 12
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 0
               Left = 230
               Bottom = 206
               Right = 446
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 6
               Left = 484
               Bottom = 121
               Right = 636
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
      Begin ColumnWidths = 30
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
         Append =', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' 1400
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_acc_pat_cnb';

