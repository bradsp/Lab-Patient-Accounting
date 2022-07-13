CREATE VIEW dbo.vw_prg_chrg_bill
AS
WITH cteChrgAmt(account, chrg_numm, qty, chrgnet, amtnet, cdm, cpt4, [type], modi, revcode, modi2) AS (SELECT     dbo.chrg.account, dbo.chrg.chrg_num, 
                                                                                                                                                                                                                                                          dbo.chrg.qty, 
                                                                                                                                                                                                                                                          dbo.chrg.qty * dbo.chrg.net_amt AS net, 
                                                                                                                                                                                                                                                          dbo.chrg.qty * dbo.amt.amount AS Expr1, 
                                                                                                                                                                                                                                                          dbo.chrg.cdm, dbo.amt.cpt4, dbo.amt.type, 
                                                                                                                                                                                                                                                          dbo.amt.modi, dbo.amt.revcode, 
                                                                                                                                                                                                                                                          dbo.amt.modi2
                                                                                                                                                                                                                                   FROM         dbo.chrg INNER JOIN
                                                                                                                                                                                                                                                          dbo.amt ON 
                                                                                                                                                                                                                                                          dbo.amt.chrg_num = dbo.chrg.chrg_num
                                                                                                                                                                                                                                   WHERE     (dbo.chrg.cdm <> 'CBILL') AND 
                                                                                                                                                                                                                                                          (dbo.chrg.status <> 'CBILL') AND 
                                                                                                                                                                                                                                                          (dbo.chrg.credited = 0))
    SELECT     dbo.acc.account, dbo.acc.trans_date, SUM(cteChrgAmt_1.qty) AS qty, cteChrgAmt_1.chrgnet, cteChrgAmt_1.amtnet, cteChrgAmt_1.cpt4, 
                            cteChrgAmt_1.[type], cteChrgAmt_1.modi, cteChrgAmt_1.revcode, cteChrgAmt_1.modi2
     FROM         cteChrgAmt AS cteChrgAmt_1 INNER JOIN
                            dbo.acc ON dbo.acc.account = cteChrgAmt_1.account
     GROUP BY cteChrgAmt_1.cpt4, dbo.acc.account, cteChrgAmt_1.chrgnet, cteChrgAmt_1.amtnet, dbo.acc.trans_date, cteChrgAmt_1.[type], cteChrgAmt_1.modi, 
                            cteChrgAmt_1.revcode, cteChrgAmt_1.modi2

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[45] 2[12] 3) )"
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
               Left = 244
               Bottom = 121
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cteChrgAmt_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 206
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
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_chrg_bill';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vw_prg_chrg_bill';

