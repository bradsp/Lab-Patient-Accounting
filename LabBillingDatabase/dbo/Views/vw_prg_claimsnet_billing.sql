CREATE VIEW dbo.vw_prg_claimsnet_billing
AS
SELECT     TOP (100) PERCENT dbo.acc.account, dbo.acc.pat_name, dbo.acc.trans_date, dbo.acc.fin_code, dbo.acc.cl_mnem, dbo.acc.status, dbo.pat.dbill_date, 
                      dbo.pat.ub_date, dbo.pat.h1500_date, dbo.pat.batch_date, dbo.pat.ebill_batch_date, dbo.pat.mailer, dbo.fin.h1500, dbo.fin.ub92, dbo.pat.phy_id, 
                      dbo.pat.ebill_batch_1500, dbo.pat.last_dm, dbo.pat.bd_list_date, dbo.pat.claimsnet_1500_batch_date, dbo.vw_chrg_bal.total AS [Chrg Total], 
                      dbo.vw_chk_bal.total AS [Chk Total], dbo.insc.claimsnet_payer_id, dbo.ins.plan_nme AS INS_Plan_nme, dbo.insc.name AS INSC_name, 
                      dbo.ins.ins_code AS INS_ins_code
FROM         dbo.insc RIGHT OUTER JOIN
                      dbo.fin RIGHT OUTER JOIN
                      dbo.acc RIGHT OUTER JOIN
                      dbo.ins ON dbo.acc.fin_code = dbo.ins.fin_code AND dbo.acc.account = dbo.ins.account LEFT OUTER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account ON dbo.fin.fin_code = dbo.acc.fin_code LEFT OUTER JOIN
                      dbo.vw_chrg_bal ON dbo.pat.account = dbo.vw_chrg_bal.account LEFT OUTER JOIN
                      dbo.vw_chk_bal ON dbo.pat.account = dbo.vw_chk_bal.account ON dbo.insc.code = dbo.ins.ins_code
WHERE     (NOT (dbo.acc.status IN ('SSIUBOP', 'UBOP', 'PAID_OUT', 'CLOSED', 'SSIUB', 'SSI1500', 'UB', '1500'))) AND (ISNULL(dbo.vw_chrg_bal.total, 0) 
                      - ISNULL(dbo.vw_chk_bal.total, 0) > 0) AND (dbo.pat.ub_date IS NULL) AND (dbo.pat.h1500_date IS NULL) AND (dbo.pat.batch_date IS NULL) AND 
                      (dbo.pat.ebill_batch_date IS NULL) AND (dbo.fin.h1500 <> 'N') AND (dbo.pat.ebill_batch_1500 IS NULL) AND (dbo.acc.fin_code IS NOT NULL) AND 
                      (NOT (dbo.insc.claimsnet_payer_id IN ('PAPER', 'EXCLUDED'))) AND (dbo.pat.claimsnet_1500_batch_date IS NULL) AND (dbo.fin.ub92 = 'N')
ORDER BY dbo.acc.pat_name

GO
EXECUTE sp_addextendedproperty @name = N'Description', @value = N'11/10/2008 wdk Moved LIVE. Used for billing via claimsnet.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_claimsnet_billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[14] 2[37] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
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
         Begin Table = "insc"
            Begin Extent = 
               Top = 186
               Left = 710
               Bottom = 301
               Right = 883
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "fin"
            Begin Extent = 
               Top = 212
               Left = 227
               Bottom = 327
               Right = 379
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "acc"
            Begin Extent = 
               Top = 14
               Left = 13
               Bottom = 318
               Right = 181
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "ins"
            Begin Extent = 
               Top = 150
               Left = 433
               Bottom = 313
               Right = 585
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "pat"
            Begin Extent = 
               Top = 17
               Left = 310
               Bottom = 132
               Right = 496
            End
            DisplayFlags = 280
            TopColumn = 41
         End
         Begin Table = "vw_chrg_bal"
            Begin Extent = 
               Top = 69
               Left = 592
               Bottom = 154
               Right = 744
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vw_chk_bal"
            Begin Extent = 
               Top = 3
               Left = 590
               Bottom = 88
               Right = 742
            End
            DisplayFlags = 280
          ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_claimsnet_billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'  TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
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
         Width = 2025
         Width = 1500
         Width = 1500
         Width = 1005
         Width = 1575
         Width = 1035
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2460
         Alias = 1515
         Table = 1470
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 450
         SortOrder = 585
         GroupBy = 1350
         Filter = 2805
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_claimsnet_billing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_claimsnet_billing';

