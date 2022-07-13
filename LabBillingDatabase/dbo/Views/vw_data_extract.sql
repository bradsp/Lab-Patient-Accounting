CREATE VIEW dbo.vw_data_extract
AS
SELECT     dbo.acc.account, dbo.acc.cl_mnem, dbo.client.cli_nme, dbo.acc.fin_code AS 'accFinCode', dbo.acc.pat_name, dbo.acc.ssn, dbo.acc.trans_date, dbo.pat.city_st_zip, 
                      dbo.pat.dob_yyyy, COALESCE (dbo.pat.h1500_date, dbo.pat.ub_date) AS last_bill_date, insA.ins_code AS 'PrimaryInsCode', insA.plan_nme AS 'PrimaryInsName', 
                      insB.ins_code AS 'SecondaryInsCode', insB.plan_nme AS 'SecondaryInsName', insB.ins_code AS 'TertiaryInsCode', insC.plan_nme AS 'TertiaryInsName', 
                      dbo.chrg.cdm, dbo.amt.cpt4, dbo.chrg.qty, dbo.chrg.fin_code AS 'chrgFinCode', dbo.amt.amount, dbo.chrg.qty * dbo.amt.amount AS NetAmount, 
                      dbo.amt.mod_date AS 'chrgPostedDate'
FROM         dbo.acc LEFT OUTER JOIN
                      dbo.chrg ON dbo.chrg.account = dbo.acc.account LEFT OUTER JOIN
                      dbo.amt ON dbo.amt.chrg_num = dbo.chrg.chrg_num LEFT OUTER JOIN
                      dbo.pat ON dbo.pat.account = dbo.acc.account LEFT OUTER JOIN
                      dbo.ins AS insA ON insA.account = dbo.acc.account AND insA.ins_a_b_c = 'A' AND insA.deleted = 0 LEFT OUTER JOIN
                      dbo.ins AS insB ON insB.account = dbo.acc.account AND insB.ins_a_b_c = 'B' AND insB.deleted = 0 LEFT OUTER JOIN
                      dbo.ins AS insC ON insC.account = dbo.acc.account AND insC.ins_a_b_c = 'C' AND insC.deleted = 0 LEFT OUTER JOIN
                      dbo.client ON dbo.acc.cl_mnem = dbo.client.cli_mnem

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
         Begin Table = "acc"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "chrg"
            Begin Extent = 
               Top = 6
               Left = 252
               Bottom = 125
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "amt"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 125
               Right = 636
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "insA"
            Begin Extent = 
               Top = 6
               Left = 674
               Bottom = 125
               Right = 850
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "insB"
            Begin Extent = 
               Top = 126
               Left = 300
               Bottom = 245
               Right = 476
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "insC"
            Begin Extent = 
               Top = 126
               Left = 514
               Bottom = 245
               Right = 690
            End
            DisplayFlags = 280
            TopColumn = 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_data_extract';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'
         End
         Begin Table = "client"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 365
               Right = 233
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
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_data_extract';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_data_extract';

