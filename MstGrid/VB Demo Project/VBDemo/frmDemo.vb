Imports System.IO
Imports System.Drawing.Text
Imports BaiqiSoft.GridControl


Public Class frmDemo

    Dim m_FileName As String = ""
    Dim m_LineStyle As Integer = 1
    Dim m_LineColor As Color = Color.Black

    Private Sub CboAllowBigSelection_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboAllowBigSelection.SelectedIndexChanged
        GrdView1.AllowBigSelection = CBool(cboAllowBigSelection.Text)
    End Sub


    Private Sub CboAllowSelection_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboAllowSelection.SelectedIndexChanged
        GrdView1.AllowSelection = CBool(cboAllowSelection.Text)
    End Sub


    Private Sub CboAllowUserReorder_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboAllowUserReorder.SelectedIndexChanged
        GrdView1.AllowUserReorder = cboAllowUserReorder.SelectedIndex
    End Sub


    Private Sub CboAllowUserResizing_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboAllowUserResizing.SelectedIndexChanged
        GrdView1.AllowUserResizing = cboAllowUserResizing.SelectedIndex
    End Sub


    Private Sub CboAllowUserSort_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboAllowUserSort.SelectedIndexChanged
        GrdView1.AllowUserSort = CBool(cboAllowUserSort.Text)
    End Sub


    Private Sub CboBorderStyle_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboBorderStyle.SelectedIndexChanged
        GrdView1.BorderStyle = cboBorderStyle.SelectedIndex
    End Sub


    Private Sub cboGridLines_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboGridLines.SelectedIndexChanged
        GrdView1.GridLines = cboGridLines.SelectedIndex
    End Sub


    Private Sub CboDateFormat_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboDateFormat.SelectedIndexChanged
        GrdView1.DateFormat = cboDateFormat.SelectedIndex
    End Sub


    Private Sub CboButtonLocked_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboButtonLocked.SelectedIndexChanged
        GrdView1.ButtonLocked = CBool(cboButtonLocked.Text)
    End Sub


    Private Sub CboEditable_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboEditable.SelectedIndexChanged
        GrdView1.Editable = CBool(cboEditable.Text)
    End Sub


    Private Sub CboEllipsis_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboEllipsis.SelectedIndexChanged
        GrdView1.Ellipsis = CBool(cboEllipsis.Text)
    End Sub


    Private Sub CboEnterKeyBehavior_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboEnterKeyBehavior.SelectedIndexChanged
        GrdView1.EnterKeyBehavior = cboEnterKeyBehavior.SelectedIndex
    End Sub


    Private Sub CboExtendLastCol_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboExtendLastCol.SelectedIndexChanged
        GrdView1.ExtendLastCol = CBool(cboExtendLastCol.Text)
    End Sub


    Private Sub cboSelectionMode_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSelectionMode.SelectedIndexChanged
        GrdView1.SelectionMode = cboSelectionMode.SelectedIndex
    End Sub


    Private Sub cboHighLight_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboHighLight.SelectedIndexChanged
        GrdView1.HighLight = cboHighLight.SelectedIndex
    End Sub


    Private Sub cboFocusRect_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboFocusRect.SelectedIndexChanged
        GrdView1.FocusRect = cboFocusRect.SelectedIndex
    End Sub


    Private Sub cboScrollBars_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboScrollBars.SelectedIndexChanged
        GrdView1.ScrollBars = cboScrollBars.SelectedIndex
    End Sub


    Private Sub cboScrollBarStyle_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboScrollBarStyle.SelectedIndexChanged
        GrdView1.ScrollBarStyle = cboScrollBarStyle.SelectedIndex
    End Sub


    Private Sub cboPictureOver_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboPictureOver.SelectedIndexChanged
        GrdView1.PicturesOver = CBool(cboPictureOver.Text)
    End Sub


    Private Sub cboShowComboButton_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboShowComboButton.SelectedIndexChanged
        GrdView1.ShowComboButton = cboShowComboButton.SelectedIndex
    End Sub


    Private Sub cboShowContextMenu_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboShowContextMenu.SelectedIndexChanged
        GrdView1.ShowContextMenu = CBool(cboShowContextMenu.Text)
    End Sub


    Private Sub cboApplySelectionToImage_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboApplySelectionToImage.SelectedIndexChanged
        GrdView1.ApplySelectionToImage = CBool(cboApplySelectionToImage.Text)
    End Sub


    Private Sub TxtRowCount_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtRowCount.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.RowCount = CInt(txtRowCount.Text)
            End If
        End If
    End Sub


    Private Sub TxtColCount_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtColCount.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.ColCount = CInt(txtColCount.Text)
            End If
        End If
    End Sub


    Private Sub txtFixedRows_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtFixedRows.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.FixedRows = CInt(txtFixedRows.Text)
            End If
        End If
    End Sub


    Private Sub txtFixedCols_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtFixedCols.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.FixedCols = CInt(txtFixedCols.Text)
            End If
        End If
    End Sub


    Private Sub txtFrozenRows_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtFrozenRows.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.FrozenRows = CInt(txtFrozenRows.Text)
            End If
        End If
    End Sub


    Private Sub txtFrozenCols_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtFrozenCols.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.FrozenCols = CInt(txtFrozenCols.Text)
            End If
        End If
    End Sub


    Private Sub TxtDefaultColWidth_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDefaultColWidth.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.DefaultColWidth = CInt(txtDefaultColWidth.Text)
            End If
        End If
    End Sub


    Private Sub TxtDefaultRowHeight_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDefaultRowHeight.KeyPress
        If eventArgs.KeyChar < "0" Or eventArgs.KeyChar > "9" Then
            If Asc(eventArgs.KeyChar) <> System.Windows.Forms.Keys.Return Then
                eventArgs.Handled = True
            Else
                'Enter Key
                GrdView1.DefaultRowHeight = CInt(txtDefaultRowHeight.Text)
            End If
        End If
    End Sub


    Private Sub picBackColor_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picBackColor.Click
        ColorDialog1.Color = picBackColor.BackColor
        ColorDialog1.ShowDialog()
        picBackColor.BackColor = ColorDialog1.Color
        GrdView1.BackColor = picBackColor.BackColor
    End Sub


    Private Sub picBackColorAlternate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picBackColorAlternate.Click
        ColorDialog1.Color = picBackColorAlternate.BackColor
        ColorDialog1.ShowDialog()
        picBackColorAlternate.BackColor = ColorDialog1.Color
        GrdView1.BackColorAlternate = picBackColorAlternate.BackColor
    End Sub


    Private Sub picBackColorBkg_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picBackColorBkg.Click
        ColorDialog1.Color = picBackColorBkg.BackColor
        ColorDialog1.ShowDialog()
        picBackColorBkg.BackColor = ColorDialog1.Color
        GrdView1.BackColorBkg = picBackColorBkg.BackColor
    End Sub


    Private Sub picBackColorFrozen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picBackColorFrozen.Click
        ColorDialog1.Color = picBackColorFrozen.BackColor
        ColorDialog1.ShowDialog()
        picBackColorFrozen.BackColor = ColorDialog1.Color
        GrdView1.BackColorFrozen = picBackColorFrozen.BackColor
    End Sub


    Private Sub picBackColorSel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picBackColorSel.Click
        ColorDialog1.Color = picBackColorSel.BackColor
        ColorDialog1.ShowDialog()
        picBackColorSel.BackColor = ColorDialog1.Color
        GrdView1.BackColorSel = picBackColorSel.BackColor
    End Sub


    Private Sub cboUseBackColorAlternate_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboUseBackColorAlternate.SelectedIndexChanged
        GrdView1.UseBackColorAlternate = CBool(cboUseBackColorAlternate.Text)
    End Sub


    Private Sub picForeColor_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picForeColor.Click
        ColorDialog1.Color = picForeColor.BackColor
        ColorDialog1.ShowDialog()
        picForeColor.BackColor = ColorDialog1.Color
        GrdView1.ForeColor = picForeColor.BackColor
    End Sub


    Private Sub picForeColorFixed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picForeColorFixed.Click
        ColorDialog1.Color = picForeColorFixed.BackColor
        ColorDialog1.ShowDialog()
        picForeColorFixed.BackColor = ColorDialog1.Color
        GrdView1.ForeColorFixed = picForeColorFixed.BackColor
    End Sub


    Private Sub picForeColorFrozen_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picForeColorFrozen.Click
        ColorDialog1.Color = picForeColorFrozen.BackColor
        ColorDialog1.ShowDialog()
        picForeColorFrozen.BackColor = ColorDialog1.Color
        GrdView1.ForeColorFrozen = picForeColorFrozen.BackColor
    End Sub


    Private Sub picForeColorSel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picForeColorSel.Click
        ColorDialog1.Color = picForeColorSel.BackColor
        ColorDialog1.ShowDialog()
        picForeColorSel.BackColor = ColorDialog1.Color
        GrdView1.ForeColorSel = picForeColorSel.BackColor
    End Sub


    Private Sub picGridColor_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picGridColor.Click
        ColorDialog1.Color = picGridColor.BackColor
        ColorDialog1.ShowDialog()
        picGridColor.BackColor = ColorDialog1.Color
        GrdView1.GridColor = picGridColor.BackColor
    End Sub


    Private Sub cboSheetBorder_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSheetBorder.SelectedIndexChanged
        GrdView1.SheetBorder = CBool(cboSheetBorder.Text)
    End Sub


    Private Sub picSheetBorderColor_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picSheetBorderColor.Click
        ColorDialog1.Color = picSheetBorderColor.BackColor
        ColorDialog1.ShowDialog()
        picSheetBorderColor.BackColor = ColorDialog1.Color
        GrdView1.SheetBorderColor = picSheetBorderColor.BackColor
    End Sub


    Private Sub cboThemeStyle_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboThemeStyle.SelectedIndexChanged
        GrdView1.ThemeStyle = cboThemeStyle.SelectedIndex
    End Sub


    Private Sub cboThemeColor_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboThemeColor.SelectedIndexChanged
        GrdView1.ThemeColor = cboThemeColor.SelectedIndex
        picThemeCustomColorFrom.BackColor = GrdView1.ThemeCustomColorFrom
        picThemeCustomColorTo.BackColor = GrdView1.ThemeCustomColorTo
    End Sub


    Private Sub picThemeCustomColorFrom_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picThemeCustomColorFrom.Click
        ColorDialog1.Color = picThemeCustomColorFrom.BackColor
        ColorDialog1.ShowDialog()
        picThemeCustomColorFrom.BackColor = ColorDialog1.Color
        GrdView1.ThemeCustomColorFrom = picThemeCustomColorFrom.BackColor
    End Sub


    Private Sub picThemeCustomColorTo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles picThemeCustomColorTo.Click
        ColorDialog1.Color = picThemeCustomColorTo.BackColor
        ColorDialog1.ShowDialog()
        picThemeCustomColorTo.BackColor = ColorDialog1.Color
        GrdView1.ThemeCustomColorTo = picThemeCustomColorTo.BackColor
    End Sub


    Private Sub btnFont_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFont.Click
        FontDialog1.Font = GrdView1.Font.Clone
        FontDialog1.ShowDialog()
        GrdView1.Font = FontDialog1.Font.Clone
    End Sub


    Private Sub cboShowRowHeaderImage_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboShowRowHeaderImage.SelectedIndexChanged
        GrdView1.ShowRowHeaderImage = CBool(cboShowRowHeaderImage.Text)
    End Sub


    Private Sub cboHighlightHeaders_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboHighlightHeaders.SelectedIndexChanged
        GrdView1.HighlightHeaders = CBool(cboHighlightHeaders.Text)
    End Sub


    Private Sub ReadProperty()
        'Page1
        cboAllowBigSelection.SelectedIndex = IIf(GrdView1.AllowBigSelection, 0, 1)
        cboAllowSelection.SelectedIndex = IIf(GrdView1.AllowSelection, 0, 1)
        cboAllowUserReorder.SelectedIndex = GrdView1.AllowUserReorder
        cboAllowUserResizing.SelectedIndex = GrdView1.AllowUserResizing
        cboAllowUserSort.SelectedIndex = IIf(GrdView1.AllowUserSort, 0, 1)
        cboBorderStyle.SelectedIndex = GrdView1.BorderStyle
        cboGridLines.SelectedIndex = GrdView1.GridLines
        cboDateFormat.SelectedIndex = GrdView1.DateFormat
        cboButtonLocked.SelectedIndex = IIf(GrdView1.ButtonLocked, 0, 1)
        cboEditable.SelectedIndex = IIf(GrdView1.Editable, 0, 1)
        cboEllipsis.SelectedIndex = IIf(GrdView1.Ellipsis, 0, 1)
        cboEnterKeyBehavior.SelectedIndex = GrdView1.EnterKeyBehavior
        cboExtendLastCol.SelectedIndex = IIf(GrdView1.ExtendLastCol, 0, 1)
        cboSelectionMode.SelectedIndex = GrdView1.SelectionMode
        cboHighLight.SelectedIndex = GrdView1.HighLight
        cboFocusRect.SelectedIndex = GrdView1.FocusRect
        cboScrollBars.SelectedIndex = GrdView1.ScrollBars
        cboScrollBarStyle.SelectedIndex = GrdView1.ScrollBarStyle
        cboPictureOver.SelectedIndex = IIf(GrdView1.PicturesOver, 0, 1)
        cboShowComboButton.SelectedIndex = GrdView1.ShowComboButton
        cboShowContextMenu.SelectedIndex = IIf(GrdView1.ShowContextMenu, 0, 1)
        cboApplySelectionToImage.SelectedIndex = IIf(GrdView1.ApplySelectionToImage, 0, 1)
        'Page2
        txtRowCount.Text = CStr(GrdView1.RowCount)
        txtColCount.Text = CStr(GrdView1.ColCount)
        txtFixedRows.Text = CStr(GrdView1.FixedRows)
        txtFixedCols.Text = CStr(GrdView1.FixedCols)
        txtFrozenRows.Text = CStr(GrdView1.FrozenRows)
        txtFrozenCols.Text = CStr(GrdView1.FrozenCols)
        txtDefaultColWidth.Text = CStr(GrdView1.DefaultColWidth)
        txtDefaultRowHeight.Text = CStr(GrdView1.DefaultRowHeight)
        picBackColor.BackColor = GrdView1.BackColor
        picBackColorAlternate.BackColor = GrdView1.BackColorAlternate
        picBackColorBkg.BackColor = GrdView1.BackColorBkg
        picBackColorFrozen.BackColor = GrdView1.BackColorFrozen
        picBackColorSel.BackColor = GrdView1.BackColorSel
        cboUseBackColorAlternate.SelectedIndex = IIf(GrdView1.UseBackColorAlternate, 0, 1)
        picForeColor.BackColor = GrdView1.ForeColor
        picForeColorFixed.BackColor = GrdView1.ForeColorFixed
        picForeColorFrozen.BackColor = GrdView1.ForeColorFrozen
        picForeColorSel.BackColor = GrdView1.ForeColorSel
        picGridColor.BackColor = GrdView1.GridColor
        cboSheetBorder.SelectedIndex = IIf(GrdView1.SheetBorder, 0, 1)
        picSheetBorderColor.BackColor = GrdView1.SheetBorderColor
        'Page3
        cboThemeStyle.SelectedIndex = GrdView1.ThemeStyle
        cboThemeColor.SelectedIndex = GrdView1.ThemeColor
        picThemeCustomColorFrom.BackColor = GrdView1.ThemeCustomColorFrom
        picThemeCustomColorTo.BackColor = GrdView1.ThemeCustomColorTo
        cboShowRowHeaderImage.SelectedIndex = IIf(GrdView1.ShowRowHeaderImage, 0, 1)
        cboShowHeaderAutoText.SelectedIndex = GrdView1.ShowHeaderAutoText
        cboHighlightHeaders.SelectedIndex = IIf(GrdView1.HighlightHeaders, 0, 1)
        cboAutoClipboard.SelectedIndex = IIf(GrdView1.AutoClipboard, 0, 1)
        cboTabKeyBehavior.SelectedIndex = GrdView1.TabKeyBehavior
        cboAllowDragDrop.SelectedIndex = IIf(GrdView1.AllowDragDrop, 0, 1)
        cboDragDropMode.SelectedIndex = GrdView1.DragDropMode
    End Sub


    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        bt_imagealign.Tag = 1
        bt_cellborder.Tag = -1
        bt_textalign.Tag = 1
        bt_cellcolor.ForeColor = Color.White
        bt_textcolor.ForeColor = Color.Black
        'Font Name
        Dim i As Integer
        Dim fontFamilies() As FontFamily
        Dim installedFontCollection As New InstalledFontCollection()
        fontFamilies = installedFontCollection.Families
        For i = 0 To fontFamilies.Length - 1
            cboFontName.Items.Add(fontFamilies(i).Name)
        Next i
        cboFontName.Text = GrdView1.Font.Name
        'FontSize
        For i = 5 To 72
            cboFontSize.Items.Add(CStr(i))
        Next i
        cboFontSize.Text = CStr(GrdView1.Font.SizeInPoints)
        'Init Grid
        GrdView1.NewFile()
        GrdView1.ImageList = ImageList1
        GrdView1.AllowDragDrop = True
        'Get Property Value
        ReadProperty()
    End Sub


    Private Sub Main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        tabProperty.Left = Me.ClientRectangle.Width - tabProperty.Width - 15
        tabProperty.Top = Me.ToolStrip2.Bottom + 3

        GrdView1.Left = 0
        GrdView1.Top = tabProperty.Top
        If tabProperty.Left - 12 > 0 Then
            GrdView1.Width = tabProperty.Left - 12
        End If
        If Me.Height - Me.MenuStrip1.Height - Me.ToolStrip1.Height - Me.ToolStrip2.Height - Me.StatusStrip1.Height - 45 > 0 Then
            GrdView1.Height = Me.Height - Me.MenuStrip1.Height - Me.ToolStrip1.Height - Me.ToolStrip2.Height - Me.StatusStrip1.Height - 45
            If GrdView1.Height < tabProperty.Height Then
                GrdView1.Height = tabProperty.Height
            End If
        End If
    End Sub


    Private Sub mi_file_new_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_new.Click
        m_FileName = ""
        GrdView1.NewFile()
        ReadProperty()
    End Sub


    Private Sub mi_file_open_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_open.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "XML File (*.xml)|*.xml"
        OpenFileDialog1.Title = "Open"
        OpenFileDialog1.ShowDialog()
        If Len(OpenFileDialog1.FileName) > 0 Then
            m_FileName = OpenFileDialog1.FileName
            GrdView1.OpenFile(m_FileName)
            ReadProperty()
        End If
    End Sub


    Private Sub mi_file_save_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_save.Click
        If Len(m_FileName) > 0 Then
            GrdView1.SaveFile(m_FileName)
        Else
            SaveFileDialog1.Filter = "XML File (*.xml)|*.xml"
            SaveFileDialog1.Title = "Save"
            SaveFileDialog1.ShowDialog()
            If Len(SaveFileDialog1.FileName) > 0 Then
                m_FileName = SaveFileDialog1.FileName
                GrdView1.SaveFile(m_FileName)
            End If
        End If
    End Sub


    Private Sub mi_file_saveas_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_saveas.Click
        SaveFileDialog1.OverwritePrompt = True
        SaveFileDialog1.Filter = "XML File (*.xml)|*.xml"
        SaveFileDialog1.Title = "Save As"
        SaveFileDialog1.ShowDialog()
        If Len(SaveFileDialog1.FileName) > 0 Then
            m_FileName = SaveFileDialog1.FileName
            GrdView1.SaveFile(m_FileName)
        End If
    End Sub


    Private Sub mi_file_pagesetup_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_pagesetup.Click
        GrdView1.ShowPageSetupDialog()
    End Sub


    Private Sub mi_file_printpreview_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_printpreview.Click
        GrdView1.PrintPreview()
    End Sub


    Private Sub mi_file_print_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_print.Click
        GrdView1.PrintOut()
    End Sub


    Private Sub mi_file_exit_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_file_exit.Click
        frmColumn.Close()
        frmSetCellImage.Close()
        Me.Close()
    End Sub

    Private Sub mi_edit_cut_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_cut.Click
        GrdView1.Selection.Cut()
    End Sub

    Private Sub mi_edit_copy_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_copy.Click
        GrdView1.Selection.Copy()
    End Sub

    Private Sub mi_edit_paste_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_paste.Click
        GrdView1.Selection.Paste()
    End Sub

    Private Sub mi_edit_hiderow_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_hiderow.Click
        Dim i As Integer
        GrdView1.AutoRedraw = False
        For i = GrdView1.Selection.FirstRow To GrdView1.Selection.LastRow
            If i >= 0 Then
                GrdView1.Row(i).Hidden = True
            End If
        Next i
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub mi_edit_hidecolumn_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_hidecolumn.Click
        Dim j As Integer
        GrdView1.AutoRedraw = False
        For j = GrdView1.Selection.FirstCol To GrdView1.Selection.LastCol
            If j >= 0 Then
                GrdView1.Column(j).Hidden = True
            End If
        Next j
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub mi_edit_unhiderow_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_unhiderow.Click
        Dim i As Integer
        GrdView1.AutoRedraw = False
        For i = GrdView1.Selection.FirstRow To GrdView1.Selection.LastRow
            If i >= 0 Then
                GrdView1.Row(i).Hidden = False
            End If
        Next i
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub mi_edit_unhidecolumn_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_edit_unhidecolumn.Click
        Dim j As Integer
        GrdView1.AutoRedraw = False
        For j = GrdView1.Selection.FirstCol To GrdView1.Selection.LastCol
            If j >= 0 Then
                GrdView1.Column(j).Hidden = False
            End If
        Next j
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub mi_help_manual_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_help_manual.Click
        System.Diagnostics.Process.Start("http://www.baiqisoft.com/help/mstgridnet")
    End Sub

    Private Sub mi_help_about_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_help_about.Click
        GrdView1.AboutBox()
    End Sub


    Private Sub bt_new_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_new.Click
        mi_file_new.PerformClick()
    End Sub

    Private Sub bt_open_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_open.Click
        mi_file_open.PerformClick()
    End Sub

    Private Sub bt_save_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_save.Click
        mi_file_save.PerformClick()
    End Sub

    Private Sub bt_print_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_print.Click
        mi_file_print.PerformClick()
    End Sub

    Private Sub bt_printpreview_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_printpreview.Click
        mi_file_printpreview.PerformClick()
    End Sub

    Private Sub bt_cut_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cut.Click
        mi_edit_cut.PerformClick()
    End Sub

    Private Sub bt_copy_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_copy.Click
        mi_edit_copy.PerformClick()
    End Sub

    Private Sub bt_paste_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_paste.Click
        mi_edit_paste.PerformClick()
    End Sub

    Private Sub bt_mergecell_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_mergecell.Click
        GrdView1.Selection.Merge()
    End Sub

    Private Sub bt_unmergecell_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_unmergecell.Click
        GrdView1.Selection.UnMerge()
    End Sub

    Private Sub bt_insertrow_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_insertrow.Click
        GrdView1.Selection.InsertRows()
    End Sub

    Private Sub bt_insertcolumn_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_insertcolumn.Click
        GrdView1.Selection.InsertCols()
    End Sub

    Private Sub bt_deleterow_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_deleterow.Click
        GrdView1.Selection.DeleteRows()
    End Sub

    Private Sub bt_deletecolumn_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_deletecolumn.Click
        GrdView1.Selection.DeleteCols()
    End Sub

    Private Sub bt_image_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_image.Click
        mi_cell_image.PerformClick()
    End Sub

    Private Sub bt_help_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_help.Click
        mi_help_manual.PerformClick()
    End Sub

    Private Sub bt_fontbold_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_fontbold.Click
        If GrdView1.CurrentRowIndex > -1 And GrdView1.CurrentColIndex > -1 Then
            GrdView1.Selection.FontBold = Not GrdView1.ActiveCell.FontBold
        End If
    End Sub

    Private Sub bt_fontitalic_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_fontitalic.Click
        If GrdView1.CurrentRowIndex > -1 And GrdView1.CurrentColIndex > -1 Then
            GrdView1.Selection.FontItalic = Not GrdView1.ActiveCell.FontItalic
        End If
    End Sub

    Private Sub bt_fontunderline_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_fontunderline.Click
        If GrdView1.CurrentRowIndex > -1 And GrdView1.CurrentColIndex > -1 Then
            GrdView1.Selection.FontUnderline = Not GrdView1.ActiveCell.FontUnderline
        End If
    End Sub

    Private Sub bt_fontstrikeout_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_fontstrikeout.Click
        If GrdView1.CurrentRowIndex > -1 And GrdView1.CurrentColIndex > -1 Then
            GrdView1.Selection.FontStrikethrough = Not GrdView1.ActiveCell.FontStrikethrough
        End If
    End Sub

    Private Sub bt_imagealign_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign.ButtonClick
        GrdView1.Selection.PictureAlignment = bt_imagealign.Tag
    End Sub

    Private Sub bt_imagealign_lefttop_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_lefttop.Click
        bt_imagealign.Tag = 0
        bt_imagealign.Image = bt_imagealign_lefttop.Image
        GrdView1.Selection.PictureAlignment = 0
    End Sub

    Private Sub bt_imagealign_leftcenter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_leftcenter.Click
        bt_imagealign.Tag = 1
        bt_imagealign.Image = bt_imagealign_leftcenter.Image
        GrdView1.Selection.PictureAlignment = 1
    End Sub

    Private Sub bt_imagealign_leftbottom_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_leftbottom.Click
        bt_imagealign.Tag = 2
        bt_imagealign.Image = bt_imagealign_leftbottom.Image
        GrdView1.Selection.PictureAlignment = 2
    End Sub

    Private Sub bt_imagealign_centertopm_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_centertop.Click
        bt_imagealign.Tag = 3
        bt_imagealign.Image = bt_imagealign_centertop.Image
        GrdView1.Selection.PictureAlignment = 3
    End Sub

    Private Sub bt_imagealign_centercenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_centercenter.Click
        bt_imagealign.Tag = 4
        bt_imagealign.Image = bt_imagealign_centercenter.Image
        GrdView1.Selection.PictureAlignment = 4
    End Sub

    Private Sub bt_imagealign_centerbottom_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_centerbottom.Click
        bt_imagealign.Tag = 5
        bt_imagealign.Image = bt_imagealign_centerbottom.Image
        GrdView1.Selection.PictureAlignment = 5
    End Sub

    Private Sub bt_imagealign_righttop_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_righttop.Click
        bt_imagealign.Tag = 6
        bt_imagealign.Image = bt_imagealign_righttop.Image
        GrdView1.Selection.PictureAlignment = 6
    End Sub

    Private Sub bt_imagealign_rightcenter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_rightcenter.Click
        bt_imagealign.Tag = 7
        bt_imagealign.Image = bt_imagealign_rightcenter.Image
        GrdView1.Selection.PictureAlignment = 7
    End Sub

    Private Sub bt_imagealign_rightbottom_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_rightbottom.Click
        bt_imagealign.Tag = 8
        bt_imagealign.Image = bt_imagealign_rightbottom.Image
        GrdView1.Selection.PictureAlignment = 8
    End Sub

    Private Sub bt_imagealign_stretch_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_imagealign_stretch.Click
        bt_imagealign.Tag = 9
        bt_imagealign.Image = bt_imagealign_stretch.Image
        GrdView1.Selection.PictureAlignment = 9
    End Sub

    Private Sub bt_textalign_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign.ButtonClick
        GrdView1.Selection.Alignment = bt_textalign.Tag
    End Sub

    Private Sub bt_textalign_lefttop_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_lefttop.Click
        bt_textalign.Tag = 0
        bt_textalign.Image = bt_textalign_lefttop.Image
        GrdView1.Selection.Alignment = 0
    End Sub

    Private Sub bt_textalign_leftcenter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_leftcenter.Click
        bt_textalign.Tag = 1
        bt_textalign.Image = bt_textalign_leftcenter.Image
        GrdView1.Selection.Alignment = 1
    End Sub

    Private Sub bt_textalign_leftbottom_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_leftbottom.Click
        bt_textalign.Tag = 2
        bt_textalign.Image = bt_textalign_leftbottom.Image
        GrdView1.Selection.Alignment = 2
    End Sub

    Private Sub bt_textalign_centertop_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_centertop.Click
        bt_textalign.Tag = 3
        bt_textalign.Image = bt_textalign_centertop.Image
        GrdView1.Selection.Alignment = 3
    End Sub

    Private Sub bt_textalign_centercenter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_centercenter.Click
        bt_textalign.Tag = 4
        bt_textalign.Image = bt_textalign_centercenter.Image
        GrdView1.Selection.Alignment = 4
    End Sub

    Private Sub bt_textalign_centerbottom_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_centerbottom.Click
        bt_textalign.Tag = 5
        bt_textalign.Image = bt_textalign_centerbottom.Image
        GrdView1.Selection.Alignment = 5
    End Sub

    Private Sub bt_textalign_righttop_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_righttop.Click
        bt_textalign.Tag = 6
        bt_textalign.Image = bt_textalign_righttop.Image
        GrdView1.Selection.Alignment = 6
    End Sub

    Private Sub bt_textalign_rightcenter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_rightcenter.Click
        bt_textalign.Tag = 7
        bt_textalign.Image = bt_textalign_rightcenter.Image
        GrdView1.Selection.Alignment = 7
    End Sub

    Private Sub bt_textalign_rightbottom_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textalign_rightbottom.Click
        bt_textalign.Tag = 8
        bt_textalign.Image = bt_textalign_rightbottom.Image
        GrdView1.Selection.Alignment = 8
    End Sub

    Private Sub bt_cellcolor_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellcolor.ButtonClick
        GrdView1.Selection.BackColor = bt_cellcolor.ForeColor
    End Sub

    Private Sub bt_cellcolor_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles bt_cellcolor.DropDownOpening
        ColorDialog1.Color = bt_cellcolor.ForeColor
        ColorDialog1.ShowDialog()
        bt_cellcolor.ForeColor = ColorDialog1.Color
        GrdView1.Selection.BackColor = bt_cellcolor.ForeColor
    End Sub

    Private Sub bt_textcolor_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_textcolor.ButtonClick
        GrdView1.Selection.ForeColor = bt_textcolor.ForeColor
    End Sub

    Private Sub bt_textcolor_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles bt_textcolor.DropDownOpening
        ColorDialog1.Color = bt_textcolor.ForeColor
        ColorDialog1.ShowDialog()
        bt_textcolor.ForeColor = ColorDialog1.Color
        GrdView1.Selection.ForeColor = bt_textcolor.ForeColor
    End Sub

    Private Sub cboFontName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFontName.SelectedIndexChanged
        GrdView1.Selection.FontName = cboFontName.Text
    End Sub

    Private Sub cboFontSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFontSize.SelectedIndexChanged
        GrdView1.Selection.FontSize = Val(cboFontSize.Text)
    End Sub

    Private Sub cboFontSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFontSize.TextChanged
        GrdView1.Selection.FontSize = Val(cboFontSize.Text)
    End Sub

    Private Sub bt_cellborder_thin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_thin.Click
        m_LineStyle = 1
        bt_cellborder_thin.Checked = True
        bt_cellborder_thick.Checked = False
        bt_cellborder_dot.Checked = False
    End Sub

    Private Sub bt_cellborder_thick_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_thick.Click
        m_LineStyle = 2
        bt_cellborder_thin.Checked = False
        bt_cellborder_thick.Checked = True
        bt_cellborder_dot.Checked = False
    End Sub

    Private Sub bt_cellborder_dot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_dot.Click
        m_LineStyle = 3
        bt_cellborder_thin.Checked = False
        bt_cellborder_thick.Checked = False
        bt_cellborder_dot.Checked = True
    End Sub

    Private Sub bt_cellborder_color_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_color.Click
        ColorDialog1.Color = m_LineColor
        ColorDialog1.ShowDialog()
        m_LineColor = ColorDialog1.Color
    End Sub

    Private Sub bt_cellborder_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder.ButtonClick
        If bt_cellborder.Tag = -1 Then
            GrdView1.AutoRedraw = False
            GrdView1.Selection.Borders(CellBorders.Around).LineStyle = LineStyle.None
            GrdView1.Selection.Borders(CellBorders.Inside).LineStyle = LineStyle.None
            GrdView1.Selection.Borders(CellBorders.DiagonalUp).LineStyle = LineStyle.None
            GrdView1.Selection.Borders(CellBorders.DiagonalDown).LineStyle = LineStyle.None
            GrdView1.AutoRedraw = True
        Else
            GrdView1.AutoRedraw = False
            GrdView1.Selection.Borders(bt_cellborder.Tag).LineStyle = m_LineStyle
            GrdView1.Selection.Borders(bt_cellborder.Tag).Color = m_LineColor
            GrdView1.AutoRedraw = True
        End If
    End Sub

    Private Sub bt_cellborder_none_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_none.Click
        bt_cellborder.Tag = -1
        bt_cellborder.Image = bt_cellborder_none.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Around).LineStyle = LineStyle.None
        GrdView1.Selection.Borders(CellBorders.Inside).LineStyle = LineStyle.None
        GrdView1.Selection.Borders(CellBorders.DiagonalUp).LineStyle = LineStyle.None
        GrdView1.Selection.Borders(CellBorders.DiagonalDown).LineStyle = LineStyle.None
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_around_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_around.Click
        bt_cellborder.Tag = 0
        bt_cellborder.Image = bt_cellborder_around.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Around).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.Around).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_left_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_left.Click
        bt_cellborder.Tag = 1
        bt_cellborder.Image = bt_cellborder_left.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Left).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.Left).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_top_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_top.Click
        bt_cellborder.Tag = 2
        bt_cellborder.Image = bt_cellborder_top.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Top).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.Top).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_right_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_right.Click
        bt_cellborder.Tag = 3
        bt_cellborder.Image = bt_cellborder_right.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Right).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.Right).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_bottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_bottom.Click
        bt_cellborder.Tag = 4
        bt_cellborder.Image = bt_cellborder_bottom.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Bottom).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.Bottom).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_diagonalup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_diagonalup.Click
        bt_cellborder.Tag = 5
        bt_cellborder.Image = bt_cellborder_diagonalup.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.DiagonalUp).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.DiagonalUp).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_diagonaldown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_diagonaldown.Click
        bt_cellborder.Tag = 6
        bt_cellborder.Image = bt_cellborder_diagonaldown.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.DiagonalDown).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.DiagonalDown).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_inside_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_inside.Click
        bt_cellborder.Tag = 7
        bt_cellborder.Image = bt_cellborder_inside.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.Inside).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.Inside).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_insidevertical_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_insidevertical.Click
        bt_cellborder.Tag = 8
        bt_cellborder.Image = bt_cellborder_insidevertical.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.InsideVertical).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.InsideVertical).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub bt_cellborder_insidehorizontal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bt_cellborder_insidehorizontal.Click
        bt_cellborder.Tag = 9
        bt_cellborder.Image = bt_cellborder_insidehorizontal.Image
        GrdView1.AutoRedraw = False
        GrdView1.Selection.Borders(CellBorders.InsideHorizontal).LineStyle = m_LineStyle
        GrdView1.Selection.Borders(CellBorders.InsideHorizontal).Color = m_LineColor
        GrdView1.AutoRedraw = True
    End Sub

    Private Sub GrdView1_ErrorInfo(ByVal sender As Object, ByVal e As ErrorInfoEventArgs) Handles GrdView1.ErrorInfo
        MsgBox(e.LastErrorNumber.ToString & "   " & e.LastErrorMessage, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Warning")
    End Sub

    Private Sub mi_help_updates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_help_updates.Click
        System.Diagnostics.Process.Start("http://www.baiqisoft.com")
    End Sub

    Private Sub mi_cell_setwraptext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_cell_setwraptext.Click
        GrdView1.Selection.WrapText = True
    End Sub

    Private Sub mi_cell_cancelwraptext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_cell_cancelwraptext.Click
        GrdView1.Selection.WrapText = False
    End Sub

    Private Sub mi_cell_lock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_cell_lock.Click
        GrdView1.Selection.Locked = True
    End Sub

    Private Sub mi_cell_unlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_cell_unlock.Click
        GrdView1.Selection.Locked = False
    End Sub

    Private Sub mi_cell_image_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_cell_image.Click
        frmSetCellImage.ShowDialog()
        GrdView1.Refresh()
    End Sub

    Private Sub mi_column_celltype_textbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_celltype_textbox.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.TextBox
        End If
    End Sub

    Private Sub mi_column_celltype_combobox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_celltype_combobox.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.ComboBox
        End If
    End Sub

    Private Sub mi_column_celltype_checkbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_celltype_checkbox.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.CheckBox
        End If
    End Sub

    Private Sub mi_column_celltype_calendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_celltype_calendar.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.Calendar
        End If
    End Sub

    Private Sub mi_column_celltype_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_celltype_button.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.Button
        End If
    End Sub

    Private Sub mi_column_celltype_hypelink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_celltype_hypelink.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.HyperLink
        End If
    End Sub

    Private Sub mi_column_editmask_any_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_any.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Any
        End If
    End Sub

    Private Sub mi_column_editmask_numeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_numeric.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Numeric
        End If
    End Sub

    Private Sub mi_column_editmask_positivenumeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_positivenumeric.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.PositiveNumeric
        End If
    End Sub

    Private Sub mi_column_editmask_integers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_integers.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Integers
        End If
    End Sub

    Private Sub mi_column_editmask_positiveintegers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_positiveintegers.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.PositiveIntegers
        End If
    End Sub

    Private Sub mi_column_editmask_letter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_letter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Letter
        End If
    End Sub

    Private Sub mi_column_editmask_letternumeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_letternumeric.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.LetterNumeric
        End If
    End Sub

    Private Sub mi_column_editmask_upper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_upper.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Upper
        End If
    End Sub

    Private Sub mi_column_editmask_uppernumeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_uppernumeric.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.UpperNumeric
        End If
    End Sub

    Private Sub mi_column_editmask_lower_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_lower.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Lower
        End If
    End Sub

    Private Sub mi_column_editmask_lowernumeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_lowernumeric.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.LowerNumeric
        End If
    End Sub

    Private Sub mi_column_editmask_chqno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_editmask_chqno.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.ChqNo
        End If
    End Sub

    Private Sub mi_column_textalign_lefttop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_lefttop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.LeftTop
        End If
    End Sub

    Private Sub mi_column_textalign_leftcenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_leftcenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.LeftCenter
        End If
    End Sub

    Private Sub mi_column_textalign_leftbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_leftbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.LeftBottom
        End If
    End Sub

    Private Sub mi_column_textalign_centertop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_centertop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.CenterTop
        End If
    End Sub

    Private Sub mi_column_textalign_centercenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_centercenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.CenterCenter
        End If
    End Sub

    Private Sub mi_column_textalign_centerbotom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_centerbotom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.CenterBottom
        End If
    End Sub

    Private Sub mi_column_textalign_righttop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_righttop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.RightTop
        End If
    End Sub

    Private Sub mi_column_textalign_rightcenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_rightcenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.RightCenter
        End If
    End Sub

    Private Sub mi_column_textalign_rightbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_textalign_rightbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.RightBottom
        End If
    End Sub

    Private Sub mi_column_titlealign_lefttop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_lefttop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.LeftTop
        End If
    End Sub

    Private Sub mi_column_titlealign_leftcenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_leftcenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.LeftCenter
        End If
    End Sub

    Private Sub mi_column_titlealign_leftbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_leftbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.LeftBottom
        End If
    End Sub

    Private Sub mi_column_titlealign_centertop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_centertop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.CenterTop
        End If
    End Sub

    Private Sub mi_column_titlealign_centercenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_centercenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.CenterCenter
        End If
    End Sub

    Private Sub mi_column_titlealign_centerbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_centerbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.CenterBottom
        End If
    End Sub

    Private Sub mi_column_titlealign_righttop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_righttop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.RightTop
        End If
    End Sub

    Private Sub mi_column_titlealign_rightcenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_rightcenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.RightCenter
        End If
    End Sub

    Private Sub mi_column_titlealign_rightbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_titlealign_rightbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.RightBottom
        End If
    End Sub

    Private Sub mi_column_picalign_lefttop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_lefttop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.LeftTop
        End If
    End Sub

    Private Sub mi_column_picalign_leftcenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_leftcenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.LeftCenter
        End If
    End Sub

    Private Sub mi_column_picalign_leftbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_leftbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.LeftBottom
        End If
    End Sub

    Private Sub mi_column_picalign_centertop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_centertop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.CenterTop
        End If
    End Sub

    Private Sub mi_column_picalign_centercenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_centercenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.CenterCenter
        End If
    End Sub

    Private Sub mi_column_picalign_centerbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_centerbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.CenterBottom
        End If
    End Sub

    Private Sub mi_column_picalign_righttop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_righttop.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.RightTop
        End If
    End Sub

    Private Sub mi_column_picalign_rightcenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_rightcenter.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.RightCenter
        End If
    End Sub

    Private Sub mi_column_picalign_rightbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_rightbottom.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.RightBottom
        End If
    End Sub

    Private Sub mi_column_picalign_streth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_picalign_streth.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.Stretch
        End If
    End Sub

    Private Sub mi_column_sort_bystring_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_sort_bystring.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByString
        End If
    End Sub

    Private Sub mi_column_sort_bystringnocase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_sort_bystringnocase.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByStringNoCase
        End If
    End Sub

    Private Sub mi_column_sort_byboolean_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_sort_byboolean.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByBoolean
        End If
    End Sub

    Private Sub mi_column_sort_bydate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_sort_bydate.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByDate
        End If
    End Sub

    Private Sub mi_column_sort_bynumeric_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_sort_bynumeric.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByNumeric
        End If
    End Sub

    Private Sub mi_column_datestyle_date_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_datestyle_date.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).DateStyle = DateStyle.flexDate
        End If
    End Sub

    Private Sub mi_column_datestyle_time_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_datestyle_time.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).DateStyle = DateStyle.flexTime
        End If
    End Sub

    Private Sub mi_column_datestyle_datetime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_datestyle_datetime.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).DateStyle = DateStyle.flexDateTime
        End If
    End Sub

    Private Sub mi_column_lock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_lock.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Locked = True
        End If
    End Sub

    Private Sub mi_column_unlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_unlock.Click
        If GrdView1.CurrentColIndex <> -1 Then
            GrdView1.Column(GrdView1.CurrentColIndex).Locked = False
        End If
    End Sub

    Private Sub mi_column_title_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mi_column_title.Click
        If GrdView1.CurrentRowIndex <> -1 And GrdView1.CurrentColIndex <> -1 Then
            frmColumn.ShowDialog()
        End If
    End Sub

    Private Sub cboShowHeaderAutoText_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboShowHeaderAutoText.SelectedIndexChanged
        GrdView1.ShowHeaderAutoText = cboShowHeaderAutoText.SelectedIndex
    End Sub

    Private Sub cboAutoClipboard_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAutoClipboard.SelectedIndexChanged
        GrdView1.AutoClipboard = CBool(cboAutoClipboard.Text)
    End Sub

    Private Sub cboTabKeyBehavior_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTabKeyBehavior.SelectedIndexChanged
        GrdView1.TabKeyBehavior = cboTabKeyBehavior.SelectedIndex
    End Sub

    Private Sub cboAllowDragDrop_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboAllowDragDrop.SelectedIndexChanged
        GrdView1.AllowDragDrop = CBool(cboAllowDragDrop.Text)
    End Sub

    Private Sub cboDragDropMode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboDragDropMode.SelectedIndexChanged
        GrdView1.DragDropMode = cboDragDropMode.SelectedIndex
    End Sub
End Class