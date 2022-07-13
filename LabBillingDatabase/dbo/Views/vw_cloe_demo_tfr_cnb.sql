CREATE VIEW dbo.vw_cloe_demo_tfr_cnb
AS
SELECT     TOP (100) PERCENT dbo.acc.cl_mnem, dbo.acc.pat_name, dbo.pat.dob_yyyy, dbo.pat.sex, dbo.acc.ssn, dbo.pat.pat_addr1, dbo.pat.city_st_zip, 
                      dbo.ins.plan_nme, dbo.ins.policy_num, dbo.pat.guarantor, dbo.pat.guar_addr, dbo.pat.g_city_st, dbo.pat.phy_id, dbo.phy.last_name, 
                      dbo.phy.first_name, dbo.ins.holder_nme, dbo.ins.holder_sex, dbo.ins.fin_code, dbo.ins.ins_code, dbo.pat.relation
FROM         dbo.ins INNER JOIN
                      dbo.acc INNER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account ON dbo.ins.account = dbo.acc.account INNER JOIN
                      dbo.phy ON dbo.pat.phy_id = dbo.phy.tnh_num
WHERE     (dbo.ins.ins_a_b_c = 'A') AND (dbo.pat.guarantor IS NOT NULL) AND (dbo.pat.guar_addr IS NOT NULL) AND (dbo.pat.g_city_st IS NOT NULL) AND 
                      (dbo.acc.trans_date >= { fn NOW() } - 730)

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[22] 4[34] 3[24] 2) )"
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
         Begin Table = "ins"
            Begin Extent = 
               Top = 145
               Left = 454
               Bottom = 260
               Right = 606
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 17
               Left = 0
               Bottom = 132
               Right = 168
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 0
               Left = 176
               Bottom = 115
               Right = 392
            End
            DisplayFlags = 280
            TopColumn = 41
         End
         Begin Table = "phy"
            Begin Extent = 
               Top = 0
               Left = 474
               Bottom = 115
               Right = 626
            End
            DisplayFlags = 280
            TopColumn = 14
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 22
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
         Width = 2070
         Width = 2235
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
       ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cloe_demo_tfr_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 345
         GroupBy = 1350
         Filter = 2520
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cloe_demo_tfr_cnb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_cloe_demo_tfr_cnb';

