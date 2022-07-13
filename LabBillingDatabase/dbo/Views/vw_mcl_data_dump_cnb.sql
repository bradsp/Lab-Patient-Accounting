CREATE VIEW dbo.vw_mcl_data_dump_cnb
AS
SELECT     dbo.acc.cl_mnem, dbo.acc.mri, dbo.acc.pat_name, dbo.pat.pat_addr1, dbo.pat.city_st_zip, dbo.pat.dob_yyyy, dbo.pat.sex, dbo.acc.ssn, 
                      dbo.pat.guarantor, dbo.pat.guar_addr, dbo.pat.g_city_st, dbo.pat.guar_phone, dbo.pat.relation, dbo.pat.phy_id, dbo.ins.ins_a_b_c, dbo.ins.plan_nme, 
                      dbo.ins.ins_code, dbo.ins.holder_nme, dbo.ins.holder_dob, dbo.ins.policy_num, dbo.ins.grp_num, dbo.pat.mod_date, dbo.pat.deleted, 
                      dbo.acc.ov_pat_id
FROM         dbo.pat INNER JOIN
                      dbo.acc ON dbo.pat.account = dbo.acc.account INNER JOIN
                      dbo.ins ON dbo.pat.account = dbo.ins.account
WHERE     (dbo.pat.mod_date >= { fn NOW() } - 365) AND (dbo.pat.deleted = 0) AND (NOT (dbo.acc.mri IS NULL))

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[22] 4[26] 2[5] 3) )"
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
         Top = -5144
         Left = 0
      End
      Begin Tables = 
         Begin Table = "pat"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 1
               Left = 412
               Bottom = 116
               Right = 580
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 30
               Left = 324
               Bottom = 145
               Right = 476
            End
            DisplayFlags = 280
            TopColumn = 20
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 25
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2565
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_mcl_data_dump_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
         Filter = 2655
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_mcl_data_dump_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_mcl_data_dump_cnb';

