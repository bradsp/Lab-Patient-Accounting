Imports BaiqiSoft.GridControl

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDemo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDemo))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.mi_file = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_file_new = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_file_open = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_file_save = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_file_saveas = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_file_pagesetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_file_printpreview = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_file_print = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_file_exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_edit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_edit_cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_edit_copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_edit_paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_edit_hiderow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_edit_unhiderow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_edit_hidecolumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_edit_unhidecolumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_format = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype_textbox = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype_combobox = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype_checkbox = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype_calendar = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype_button = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_celltype_hypelink = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_any = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_numeric = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_positivenumeric = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_integers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_positiveintegers = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_letter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_letternumeric = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_upper = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_uppernumeric = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_lower = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_lowernumeric = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_editmask_chqno = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_lefttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_leftcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_leftbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_centertop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_centercenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_centerbotom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_righttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_rightcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_textalign_rightbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_lefttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_leftcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_leftbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_centertop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_centercenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_centerbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_righttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_rightcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_titlealign_rightbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_lefttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_leftcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_leftbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_centertop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_centercenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_centerbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_righttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_rightcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_rightbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_picalign_streth = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_sort = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_sort_bystring = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_sort_bystringnocase = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_sort_byboolean = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_sort_bydate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_sort_bynumeric = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_datestyle = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_datestyle_date = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_datestyle_time = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_datestyle_datetime = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_column_lock = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_column_unlock = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator27 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_column_title = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_cell = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_cell_setwraptext = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_cell_cancelwraptext = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator28 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_cell_lock = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_cell_unlock = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_cell_image = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_help = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_help_manual = New System.Windows.Forms.ToolStripMenuItem()
        Me.mi_help_updates = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.mi_help_about = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.bt_new = New System.Windows.Forms.ToolStripButton()
        Me.bt_open = New System.Windows.Forms.ToolStripButton()
        Me.bt_save = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_print = New System.Windows.Forms.ToolStripButton()
        Me.bt_printpreview = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_cut = New System.Windows.Forms.ToolStripButton()
        Me.bt_copy = New System.Windows.Forms.ToolStripButton()
        Me.bt_paste = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_mergecell = New System.Windows.Forms.ToolStripButton()
        Me.bt_unmergecell = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_insertrow = New System.Windows.Forms.ToolStripButton()
        Me.bt_insertcolumn = New System.Windows.Forms.ToolStripButton()
        Me.bt_deleterow = New System.Windows.Forms.ToolStripButton()
        Me.bt_deletecolumn = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_image = New System.Windows.Forms.ToolStripButton()
        Me.bt_help = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.cboFontName = New System.Windows.Forms.ToolStripComboBox()
        Me.cboFontSize = New System.Windows.Forms.ToolStripComboBox()
        Me.bt_fontbold = New System.Windows.Forms.ToolStripButton()
        Me.bt_fontitalic = New System.Windows.Forms.ToolStripButton()
        Me.bt_fontunderline = New System.Windows.Forms.ToolStripButton()
        Me.bt_fontstrikeout = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.bt_imagealign = New System.Windows.Forms.ToolStripSplitButton()
        Me.bt_imagealign_lefttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_leftcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_leftbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_centertop = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_centercenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_centerbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_righttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_rightcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_rightbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_imagealign_stretch = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.bt_textalign = New System.Windows.Forms.ToolStripSplitButton()
        Me.bt_textalign_lefttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_leftcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_leftbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_centertop = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_centercenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_centerbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_righttop = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_rightcenter = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_textalign_rightbottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel5 = New System.Windows.Forms.ToolStripLabel()
        Me.bt_cellcolor = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel6 = New System.Windows.Forms.ToolStripLabel()
        Me.bt_textcolor = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabel7 = New System.Windows.Forms.ToolStripLabel()
        Me.bt_cellborder = New System.Windows.Forms.ToolStripSplitButton()
        Me.bt_cellborder_none = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_around = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_left = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_top = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_right = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_bottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_diagonalup = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_diagonaldown = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_inside = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_insidevertical = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_insidehorizontal = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_cellborder_thin = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_thick = New System.Windows.Forms.ToolStripMenuItem()
        Me.bt_cellborder_dot = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.bt_cellborder_color = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tabProperty = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.cboApplySelectionToImage = New System.Windows.Forms.ComboBox()
        Me.cboShowContextMenu = New System.Windows.Forms.ComboBox()
        Me.cboShowComboButton = New System.Windows.Forms.ComboBox()
        Me.cboPictureOver = New System.Windows.Forms.ComboBox()
        Me.cboScrollBarStyle = New System.Windows.Forms.ComboBox()
        Me.cboScrollBars = New System.Windows.Forms.ComboBox()
        Me.cboFocusRect = New System.Windows.Forms.ComboBox()
        Me.cboHighLight = New System.Windows.Forms.ComboBox()
        Me.cboSelectionMode = New System.Windows.Forms.ComboBox()
        Me.cboExtendLastCol = New System.Windows.Forms.ComboBox()
        Me.cboEnterKeyBehavior = New System.Windows.Forms.ComboBox()
        Me.cboEllipsis = New System.Windows.Forms.ComboBox()
        Me.cboEditable = New System.Windows.Forms.ComboBox()
        Me.cboButtonLocked = New System.Windows.Forms.ComboBox()
        Me.cboDateFormat = New System.Windows.Forms.ComboBox()
        Me.cboGridLines = New System.Windows.Forms.ComboBox()
        Me.cboBorderStyle = New System.Windows.Forms.ComboBox()
        Me.cboAllowUserSort = New System.Windows.Forms.ComboBox()
        Me.cboAllowUserResizing = New System.Windows.Forms.ComboBox()
        Me.cboAllowUserReorder = New System.Windows.Forms.ComboBox()
        Me.cboAllowSelection = New System.Windows.Forms.ComboBox()
        Me.cboAllowBigSelection = New System.Windows.Forms.ComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.cboSheetBorder = New System.Windows.Forms.ComboBox()
        Me.picSheetBorderColor = New System.Windows.Forms.PictureBox()
        Me.picGridColor = New System.Windows.Forms.PictureBox()
        Me.picForeColorSel = New System.Windows.Forms.PictureBox()
        Me.picForeColorFrozen = New System.Windows.Forms.PictureBox()
        Me.picForeColorFixed = New System.Windows.Forms.PictureBox()
        Me.picForeColor = New System.Windows.Forms.PictureBox()
        Me.picBackColorSel = New System.Windows.Forms.PictureBox()
        Me.picBackColorFrozen = New System.Windows.Forms.PictureBox()
        Me.picBackColorBkg = New System.Windows.Forms.PictureBox()
        Me.picBackColorAlternate = New System.Windows.Forms.PictureBox()
        Me.picBackColor = New System.Windows.Forms.PictureBox()
        Me.btnFont = New System.Windows.Forms.Button()
        Me.txtDefaultRowHeight = New System.Windows.Forms.TextBox()
        Me.txtDefaultColWidth = New System.Windows.Forms.TextBox()
        Me.txtFrozenCols = New System.Windows.Forms.TextBox()
        Me.txtFrozenRows = New System.Windows.Forms.TextBox()
        Me.txtFixedCols = New System.Windows.Forms.TextBox()
        Me.txtFixedRows = New System.Windows.Forms.TextBox()
        Me.txtColCount = New System.Windows.Forms.TextBox()
        Me.cboUseBackColorAlternate = New System.Windows.Forms.ComboBox()
        Me.txtRowCount = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.cboTabKeyBehavior = New System.Windows.Forms.ComboBox()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.cboAutoClipboard = New System.Windows.Forms.ComboBox()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.cboHighlightHeaders = New System.Windows.Forms.ComboBox()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.cboShowHeaderAutoText = New System.Windows.Forms.ComboBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.cboShowRowHeaderImage = New System.Windows.Forms.ComboBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.picThemeCustomColorTo = New System.Windows.Forms.PictureBox()
        Me.picThemeCustomColorFrom = New System.Windows.Forms.PictureBox()
        Me.cboThemeColor = New System.Windows.Forms.ComboBox()
        Me.cboThemeStyle = New System.Windows.Forms.ComboBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.GrdView1 = New BaiqiSoft.GridControl.MstGrid()
        Me.cboDragDropMode = New System.Windows.Forms.ComboBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.cboAllowDragDrop = New System.Windows.Forms.ComboBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.tabProperty.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.picSheetBorderColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picGridColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picForeColorSel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picForeColorFrozen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picForeColorFixed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picForeColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBackColorSel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBackColorFrozen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBackColorBkg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBackColorAlternate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBackColor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.picThemeCustomColorTo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picThemeCustomColorFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_file, Me.mi_edit, Me.mi_format, Me.mi_help})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(976, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'mi_file
        '
        Me.mi_file.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_file_new, Me.mi_file_open, Me.mi_file_save, Me.mi_file_saveas, Me.ToolStripSeparator25, Me.mi_file_pagesetup, Me.mi_file_printpreview, Me.mi_file_print, Me.ToolStripSeparator2, Me.mi_file_exit})
        Me.mi_file.Name = "mi_file"
        Me.mi_file.Size = New System.Drawing.Size(37, 20)
        Me.mi_file.Text = "&File"
        '
        'mi_file_new
        '
        Me.mi_file_new.Name = "mi_file_new"
        Me.mi_file_new.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mi_file_new.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_new.Text = "New"
        '
        'mi_file_open
        '
        Me.mi_file_open.Name = "mi_file_open"
        Me.mi_file_open.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mi_file_open.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_open.Text = "Open..."
        '
        'mi_file_save
        '
        Me.mi_file_save.Name = "mi_file_save"
        Me.mi_file_save.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_save.Text = "Save"
        '
        'mi_file_saveas
        '
        Me.mi_file_saveas.Name = "mi_file_saveas"
        Me.mi_file_saveas.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_saveas.Text = "Save As..."
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(152, 6)
        '
        'mi_file_pagesetup
        '
        Me.mi_file_pagesetup.Name = "mi_file_pagesetup"
        Me.mi_file_pagesetup.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_pagesetup.Text = "Page Setup..."
        '
        'mi_file_printpreview
        '
        Me.mi_file_printpreview.Name = "mi_file_printpreview"
        Me.mi_file_printpreview.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_printpreview.Text = "Print Preview..."
        '
        'mi_file_print
        '
        Me.mi_file_print.Name = "mi_file_print"
        Me.mi_file_print.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.mi_file_print.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_print.Text = "Print"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(152, 6)
        '
        'mi_file_exit
        '
        Me.mi_file_exit.Name = "mi_file_exit"
        Me.mi_file_exit.Size = New System.Drawing.Size(155, 22)
        Me.mi_file_exit.Text = "Exit"
        '
        'mi_edit
        '
        Me.mi_edit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_edit_cut, Me.mi_edit_copy, Me.mi_edit_paste, Me.ToolStripSeparator3, Me.mi_edit_hiderow, Me.mi_edit_unhiderow, Me.ToolStripSeparator8, Me.mi_edit_hidecolumn, Me.mi_edit_unhidecolumn})
        Me.mi_edit.Name = "mi_edit"
        Me.mi_edit.Size = New System.Drawing.Size(39, 20)
        Me.mi_edit.Text = "&Edit"
        '
        'mi_edit_cut
        '
        Me.mi_edit_cut.Name = "mi_edit_cut"
        Me.mi_edit_cut.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_cut.Text = "Cut"
        '
        'mi_edit_copy
        '
        Me.mi_edit_copy.Name = "mi_edit_copy"
        Me.mi_edit_copy.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_copy.Text = "Copy"
        '
        'mi_edit_paste
        '
        Me.mi_edit_paste.Name = "mi_edit_paste"
        Me.mi_edit_paste.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_paste.Text = "Paste"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(155, 6)
        '
        'mi_edit_hiderow
        '
        Me.mi_edit_hiderow.Name = "mi_edit_hiderow"
        Me.mi_edit_hiderow.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_hiderow.Text = "Hide Row"
        '
        'mi_edit_unhiderow
        '
        Me.mi_edit_unhiderow.Name = "mi_edit_unhiderow"
        Me.mi_edit_unhiderow.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_unhiderow.Text = "Unhide Row"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(155, 6)
        '
        'mi_edit_hidecolumn
        '
        Me.mi_edit_hidecolumn.Name = "mi_edit_hidecolumn"
        Me.mi_edit_hidecolumn.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_hidecolumn.Text = "Hide Column"
        '
        'mi_edit_unhidecolumn
        '
        Me.mi_edit_unhidecolumn.Name = "mi_edit_unhidecolumn"
        Me.mi_edit_unhidecolumn.Size = New System.Drawing.Size(158, 22)
        Me.mi_edit_unhidecolumn.Text = "Unhide Column"
        '
        'mi_format
        '
        Me.mi_format.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column, Me.ToolStripSeparator10, Me.mi_cell})
        Me.mi_format.Name = "mi_format"
        Me.mi_format.Size = New System.Drawing.Size(57, 20)
        Me.mi_format.Text = "&Format"
        '
        'mi_column
        '
        Me.mi_column.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_celltype, Me.mi_column_editmask, Me.mi_column_textalign, Me.mi_column_titlealign, Me.mi_column_picalign, Me.mi_column_sort, Me.mi_column_datestyle, Me.ToolStripSeparator26, Me.mi_column_lock, Me.mi_column_unlock, Me.ToolStripSeparator27, Me.mi_column_title})
        Me.mi_column.Name = "mi_column"
        Me.mi_column.Size = New System.Drawing.Size(117, 22)
        Me.mi_column.Text = "Column"
        '
        'mi_column_celltype
        '
        Me.mi_column_celltype.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_celltype_textbox, Me.mi_column_celltype_combobox, Me.mi_column_celltype_checkbox, Me.mi_column_celltype_calendar, Me.mi_column_celltype_button, Me.mi_column_celltype_hypelink})
        Me.mi_column_celltype.Name = "mi_column_celltype"
        Me.mi_column_celltype.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_celltype.Text = "Cell Type"
        '
        'mi_column_celltype_textbox
        '
        Me.mi_column_celltype_textbox.Name = "mi_column_celltype_textbox"
        Me.mi_column_celltype_textbox.Size = New System.Drawing.Size(150, 22)
        Me.mi_column_celltype_textbox.Text = "0 - TextBox"
        '
        'mi_column_celltype_combobox
        '
        Me.mi_column_celltype_combobox.Name = "mi_column_celltype_combobox"
        Me.mi_column_celltype_combobox.Size = New System.Drawing.Size(150, 22)
        Me.mi_column_celltype_combobox.Text = "1 - ComboBox"
        '
        'mi_column_celltype_checkbox
        '
        Me.mi_column_celltype_checkbox.Name = "mi_column_celltype_checkbox"
        Me.mi_column_celltype_checkbox.Size = New System.Drawing.Size(150, 22)
        Me.mi_column_celltype_checkbox.Text = "2 - CheckBox"
        '
        'mi_column_celltype_calendar
        '
        Me.mi_column_celltype_calendar.Name = "mi_column_celltype_calendar"
        Me.mi_column_celltype_calendar.Size = New System.Drawing.Size(150, 22)
        Me.mi_column_celltype_calendar.Text = "3 - Calendar"
        '
        'mi_column_celltype_button
        '
        Me.mi_column_celltype_button.Name = "mi_column_celltype_button"
        Me.mi_column_celltype_button.Size = New System.Drawing.Size(150, 22)
        Me.mi_column_celltype_button.Text = "4 - Button"
        '
        'mi_column_celltype_hypelink
        '
        Me.mi_column_celltype_hypelink.Name = "mi_column_celltype_hypelink"
        Me.mi_column_celltype_hypelink.Size = New System.Drawing.Size(150, 22)
        Me.mi_column_celltype_hypelink.Text = "5 - Hypelink"
        '
        'mi_column_editmask
        '
        Me.mi_column_editmask.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_editmask_any, Me.mi_column_editmask_numeric, Me.mi_column_editmask_positivenumeric, Me.mi_column_editmask_integers, Me.mi_column_editmask_positiveintegers, Me.mi_column_editmask_letter, Me.mi_column_editmask_letternumeric, Me.mi_column_editmask_upper, Me.mi_column_editmask_uppernumeric, Me.mi_column_editmask_lower, Me.mi_column_editmask_lowernumeric, Me.mi_column_editmask_chqno})
        Me.mi_column_editmask.Name = "mi_column_editmask"
        Me.mi_column_editmask.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_editmask.Text = "Edit Mask"
        '
        'mi_column_editmask_any
        '
        Me.mi_column_editmask_any.Name = "mi_column_editmask_any"
        Me.mi_column_editmask_any.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_any.Text = "0 - Any"
        '
        'mi_column_editmask_numeric
        '
        Me.mi_column_editmask_numeric.Name = "mi_column_editmask_numeric"
        Me.mi_column_editmask_numeric.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_numeric.Text = "1 - Numeric"
        '
        'mi_column_editmask_positivenumeric
        '
        Me.mi_column_editmask_positivenumeric.Name = "mi_column_editmask_positivenumeric"
        Me.mi_column_editmask_positivenumeric.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_positivenumeric.Text = "2 - PositiveNumeric"
        '
        'mi_column_editmask_integers
        '
        Me.mi_column_editmask_integers.Name = "mi_column_editmask_integers"
        Me.mi_column_editmask_integers.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_integers.Text = "3 - Integers"
        '
        'mi_column_editmask_positiveintegers
        '
        Me.mi_column_editmask_positiveintegers.Name = "mi_column_editmask_positiveintegers"
        Me.mi_column_editmask_positiveintegers.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_positiveintegers.Text = "4 - PositiveIntegers"
        '
        'mi_column_editmask_letter
        '
        Me.mi_column_editmask_letter.Name = "mi_column_editmask_letter"
        Me.mi_column_editmask_letter.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_letter.Text = "5 - Letter"
        '
        'mi_column_editmask_letternumeric
        '
        Me.mi_column_editmask_letternumeric.Name = "mi_column_editmask_letternumeric"
        Me.mi_column_editmask_letternumeric.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_letternumeric.Text = "6 - LetterNumeric"
        '
        'mi_column_editmask_upper
        '
        Me.mi_column_editmask_upper.Name = "mi_column_editmask_upper"
        Me.mi_column_editmask_upper.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_upper.Text = "7 - Upper"
        '
        'mi_column_editmask_uppernumeric
        '
        Me.mi_column_editmask_uppernumeric.Name = "mi_column_editmask_uppernumeric"
        Me.mi_column_editmask_uppernumeric.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_uppernumeric.Text = "8 - UpperNumeric"
        '
        'mi_column_editmask_lower
        '
        Me.mi_column_editmask_lower.Name = "mi_column_editmask_lower"
        Me.mi_column_editmask_lower.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_lower.Text = "9 - Lower"
        '
        'mi_column_editmask_lowernumeric
        '
        Me.mi_column_editmask_lowernumeric.Name = "mi_column_editmask_lowernumeric"
        Me.mi_column_editmask_lowernumeric.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_lowernumeric.Text = "10 - LowerNumeric"
        '
        'mi_column_editmask_chqno
        '
        Me.mi_column_editmask_chqno.Name = "mi_column_editmask_chqno"
        Me.mi_column_editmask_chqno.Size = New System.Drawing.Size(178, 22)
        Me.mi_column_editmask_chqno.Text = "11 - ChqNo"
        '
        'mi_column_textalign
        '
        Me.mi_column_textalign.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_textalign_lefttop, Me.mi_column_textalign_leftcenter, Me.mi_column_textalign_leftbottom, Me.mi_column_textalign_centertop, Me.mi_column_textalign_centercenter, Me.mi_column_textalign_centerbotom, Me.mi_column_textalign_righttop, Me.mi_column_textalign_rightcenter, Me.mi_column_textalign_rightbottom})
        Me.mi_column_textalign.Name = "mi_column_textalign"
        Me.mi_column_textalign.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_textalign.Text = "Column Alignment"
        '
        'mi_column_textalign_lefttop
        '
        Me.mi_column_textalign_lefttop.Name = "mi_column_textalign_lefttop"
        Me.mi_column_textalign_lefttop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_lefttop.Text = "0 - LeftTop"
        '
        'mi_column_textalign_leftcenter
        '
        Me.mi_column_textalign_leftcenter.Name = "mi_column_textalign_leftcenter"
        Me.mi_column_textalign_leftcenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_leftcenter.Text = "1 - LeftCenter"
        '
        'mi_column_textalign_leftbottom
        '
        Me.mi_column_textalign_leftbottom.Name = "mi_column_textalign_leftbottom"
        Me.mi_column_textalign_leftbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_leftbottom.Text = "2 - LeftBottom"
        '
        'mi_column_textalign_centertop
        '
        Me.mi_column_textalign_centertop.Name = "mi_column_textalign_centertop"
        Me.mi_column_textalign_centertop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_centertop.Text = "3 - CenterTop"
        '
        'mi_column_textalign_centercenter
        '
        Me.mi_column_textalign_centercenter.Name = "mi_column_textalign_centercenter"
        Me.mi_column_textalign_centercenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_centercenter.Text = "4 - CenterCenter"
        '
        'mi_column_textalign_centerbotom
        '
        Me.mi_column_textalign_centerbotom.Name = "mi_column_textalign_centerbotom"
        Me.mi_column_textalign_centerbotom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_centerbotom.Text = "5 - CenterBottom"
        '
        'mi_column_textalign_righttop
        '
        Me.mi_column_textalign_righttop.Name = "mi_column_textalign_righttop"
        Me.mi_column_textalign_righttop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_righttop.Text = "6 - RightTop"
        '
        'mi_column_textalign_rightcenter
        '
        Me.mi_column_textalign_rightcenter.Name = "mi_column_textalign_rightcenter"
        Me.mi_column_textalign_rightcenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_rightcenter.Text = "7 - RightCenter"
        '
        'mi_column_textalign_rightbottom
        '
        Me.mi_column_textalign_rightbottom.Name = "mi_column_textalign_rightbottom"
        Me.mi_column_textalign_rightbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_textalign_rightbottom.Text = "8 - RightBottom"
        '
        'mi_column_titlealign
        '
        Me.mi_column_titlealign.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_titlealign_lefttop, Me.mi_column_titlealign_leftcenter, Me.mi_column_titlealign_leftbottom, Me.mi_column_titlealign_centertop, Me.mi_column_titlealign_centercenter, Me.mi_column_titlealign_centerbottom, Me.mi_column_titlealign_righttop, Me.mi_column_titlealign_rightcenter, Me.mi_column_titlealign_rightbottom})
        Me.mi_column_titlealign.Name = "mi_column_titlealign"
        Me.mi_column_titlealign.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_titlealign.Text = "Title Alignment"
        '
        'mi_column_titlealign_lefttop
        '
        Me.mi_column_titlealign_lefttop.Name = "mi_column_titlealign_lefttop"
        Me.mi_column_titlealign_lefttop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_lefttop.Text = "0 - LeftTop"
        '
        'mi_column_titlealign_leftcenter
        '
        Me.mi_column_titlealign_leftcenter.Name = "mi_column_titlealign_leftcenter"
        Me.mi_column_titlealign_leftcenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_leftcenter.Text = "1 - LeftCenter"
        '
        'mi_column_titlealign_leftbottom
        '
        Me.mi_column_titlealign_leftbottom.Name = "mi_column_titlealign_leftbottom"
        Me.mi_column_titlealign_leftbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_leftbottom.Text = "2 - LeftBottom"
        '
        'mi_column_titlealign_centertop
        '
        Me.mi_column_titlealign_centertop.Name = "mi_column_titlealign_centertop"
        Me.mi_column_titlealign_centertop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_centertop.Text = "3 - CenterTop"
        '
        'mi_column_titlealign_centercenter
        '
        Me.mi_column_titlealign_centercenter.Name = "mi_column_titlealign_centercenter"
        Me.mi_column_titlealign_centercenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_centercenter.Text = "4 - CenterCenter"
        '
        'mi_column_titlealign_centerbottom
        '
        Me.mi_column_titlealign_centerbottom.Name = "mi_column_titlealign_centerbottom"
        Me.mi_column_titlealign_centerbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_centerbottom.Text = "5 - CenterBottom"
        '
        'mi_column_titlealign_righttop
        '
        Me.mi_column_titlealign_righttop.Name = "mi_column_titlealign_righttop"
        Me.mi_column_titlealign_righttop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_righttop.Text = "6 - RightTop"
        '
        'mi_column_titlealign_rightcenter
        '
        Me.mi_column_titlealign_rightcenter.Name = "mi_column_titlealign_rightcenter"
        Me.mi_column_titlealign_rightcenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_rightcenter.Text = "7 - RightCenter"
        '
        'mi_column_titlealign_rightbottom
        '
        Me.mi_column_titlealign_rightbottom.Name = "mi_column_titlealign_rightbottom"
        Me.mi_column_titlealign_rightbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_titlealign_rightbottom.Text = "8 - RightBottom"
        '
        'mi_column_picalign
        '
        Me.mi_column_picalign.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_picalign_lefttop, Me.mi_column_picalign_leftcenter, Me.mi_column_picalign_leftbottom, Me.mi_column_picalign_centertop, Me.mi_column_picalign_centercenter, Me.mi_column_picalign_centerbottom, Me.mi_column_picalign_righttop, Me.mi_column_picalign_rightcenter, Me.mi_column_picalign_rightbottom, Me.mi_column_picalign_streth})
        Me.mi_column_picalign.Name = "mi_column_picalign"
        Me.mi_column_picalign.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_picalign.Text = "Picture Alignment"
        '
        'mi_column_picalign_lefttop
        '
        Me.mi_column_picalign_lefttop.Name = "mi_column_picalign_lefttop"
        Me.mi_column_picalign_lefttop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_lefttop.Text = "0 - LeftTop"
        '
        'mi_column_picalign_leftcenter
        '
        Me.mi_column_picalign_leftcenter.Name = "mi_column_picalign_leftcenter"
        Me.mi_column_picalign_leftcenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_leftcenter.Text = "1 - LeftCenter"
        '
        'mi_column_picalign_leftbottom
        '
        Me.mi_column_picalign_leftbottom.Name = "mi_column_picalign_leftbottom"
        Me.mi_column_picalign_leftbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_leftbottom.Text = "2 - LeftBottom"
        '
        'mi_column_picalign_centertop
        '
        Me.mi_column_picalign_centertop.Name = "mi_column_picalign_centertop"
        Me.mi_column_picalign_centertop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_centertop.Text = "3 - CenterTop"
        '
        'mi_column_picalign_centercenter
        '
        Me.mi_column_picalign_centercenter.Name = "mi_column_picalign_centercenter"
        Me.mi_column_picalign_centercenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_centercenter.Text = "4 - CenterCenter"
        '
        'mi_column_picalign_centerbottom
        '
        Me.mi_column_picalign_centerbottom.Name = "mi_column_picalign_centerbottom"
        Me.mi_column_picalign_centerbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_centerbottom.Text = "5 - CenterBottom"
        '
        'mi_column_picalign_righttop
        '
        Me.mi_column_picalign_righttop.Name = "mi_column_picalign_righttop"
        Me.mi_column_picalign_righttop.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_righttop.Text = "6 - RightTop"
        '
        'mi_column_picalign_rightcenter
        '
        Me.mi_column_picalign_rightcenter.Name = "mi_column_picalign_rightcenter"
        Me.mi_column_picalign_rightcenter.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_rightcenter.Text = "7 - RightCenter"
        '
        'mi_column_picalign_rightbottom
        '
        Me.mi_column_picalign_rightbottom.Name = "mi_column_picalign_rightbottom"
        Me.mi_column_picalign_rightbottom.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_rightbottom.Text = "8 - RightBottom"
        '
        'mi_column_picalign_streth
        '
        Me.mi_column_picalign_streth.Name = "mi_column_picalign_streth"
        Me.mi_column_picalign_streth.Size = New System.Drawing.Size(166, 22)
        Me.mi_column_picalign_streth.Text = "9 - Streth"
        '
        'mi_column_sort
        '
        Me.mi_column_sort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_sort_bystring, Me.mi_column_sort_bystringnocase, Me.mi_column_sort_byboolean, Me.mi_column_sort_bydate, Me.mi_column_sort_bynumeric})
        Me.mi_column_sort.Name = "mi_column_sort"
        Me.mi_column_sort.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_sort.Text = "Sort Type"
        '
        'mi_column_sort_bystring
        '
        Me.mi_column_sort_bystring.Name = "mi_column_sort_bystring"
        Me.mi_column_sort_bystring.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_sort_bystring.Text = "0 - ByString"
        '
        'mi_column_sort_bystringnocase
        '
        Me.mi_column_sort_bystringnocase.Name = "mi_column_sort_bystringnocase"
        Me.mi_column_sort_bystringnocase.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_sort_bystringnocase.Text = "1 - ByStringNoCase"
        '
        'mi_column_sort_byboolean
        '
        Me.mi_column_sort_byboolean.Name = "mi_column_sort_byboolean"
        Me.mi_column_sort_byboolean.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_sort_byboolean.Text = "2 - ByBoolean"
        '
        'mi_column_sort_bydate
        '
        Me.mi_column_sort_bydate.Name = "mi_column_sort_bydate"
        Me.mi_column_sort_bydate.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_sort_bydate.Text = "3 - ByDate"
        '
        'mi_column_sort_bynumeric
        '
        Me.mi_column_sort_bynumeric.Name = "mi_column_sort_bynumeric"
        Me.mi_column_sort_bynumeric.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_sort_bynumeric.Text = "4 - ByNumeric"
        '
        'mi_column_datestyle
        '
        Me.mi_column_datestyle.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_column_datestyle_date, Me.mi_column_datestyle_time, Me.mi_column_datestyle_datetime})
        Me.mi_column_datestyle.Name = "mi_column_datestyle"
        Me.mi_column_datestyle.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_datestyle.Text = "Date Style"
        '
        'mi_column_datestyle_date
        '
        Me.mi_column_datestyle_date.Name = "mi_column_datestyle_date"
        Me.mi_column_datestyle_date.Size = New System.Drawing.Size(160, 22)
        Me.mi_column_datestyle_date.Text = "0 - flexDate"
        '
        'mi_column_datestyle_time
        '
        Me.mi_column_datestyle_time.Name = "mi_column_datestyle_time"
        Me.mi_column_datestyle_time.Size = New System.Drawing.Size(160, 22)
        Me.mi_column_datestyle_time.Text = "1 - flexTime"
        '
        'mi_column_datestyle_datetime
        '
        Me.mi_column_datestyle_datetime.Name = "mi_column_datestyle_datetime"
        Me.mi_column_datestyle_datetime.Size = New System.Drawing.Size(160, 22)
        Me.mi_column_datestyle_datetime.Text = "2 - flexDateTime"
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(173, 6)
        '
        'mi_column_lock
        '
        Me.mi_column_lock.Name = "mi_column_lock"
        Me.mi_column_lock.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_lock.Text = "Lock Column"
        '
        'mi_column_unlock
        '
        Me.mi_column_unlock.Name = "mi_column_unlock"
        Me.mi_column_unlock.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_unlock.Text = "Unlock Column"
        '
        'ToolStripSeparator27
        '
        Me.ToolStripSeparator27.Name = "ToolStripSeparator27"
        Me.ToolStripSeparator27.Size = New System.Drawing.Size(173, 6)
        '
        'mi_column_title
        '
        Me.mi_column_title.Name = "mi_column_title"
        Me.mi_column_title.Size = New System.Drawing.Size(176, 22)
        Me.mi_column_title.Text = "Column Title..."
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(114, 6)
        '
        'mi_cell
        '
        Me.mi_cell.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_cell_setwraptext, Me.mi_cell_cancelwraptext, Me.ToolStripSeparator28, Me.mi_cell_lock, Me.mi_cell_unlock, Me.ToolStripSeparator29, Me.mi_cell_image})
        Me.mi_cell.Name = "mi_cell"
        Me.mi_cell.Size = New System.Drawing.Size(117, 22)
        Me.mi_cell.Text = "Cell"
        '
        'mi_cell_setwraptext
        '
        Me.mi_cell_setwraptext.Name = "mi_cell_setwraptext"
        Me.mi_cell_setwraptext.Size = New System.Drawing.Size(163, 22)
        Me.mi_cell_setwraptext.Text = "Set WrapText"
        '
        'mi_cell_cancelwraptext
        '
        Me.mi_cell_cancelwraptext.Name = "mi_cell_cancelwraptext"
        Me.mi_cell_cancelwraptext.Size = New System.Drawing.Size(163, 22)
        Me.mi_cell_cancelwraptext.Text = "Cancel WrapText"
        '
        'ToolStripSeparator28
        '
        Me.ToolStripSeparator28.Name = "ToolStripSeparator28"
        Me.ToolStripSeparator28.Size = New System.Drawing.Size(160, 6)
        '
        'mi_cell_lock
        '
        Me.mi_cell_lock.Name = "mi_cell_lock"
        Me.mi_cell_lock.Size = New System.Drawing.Size(163, 22)
        Me.mi_cell_lock.Text = "Lock Cell"
        '
        'mi_cell_unlock
        '
        Me.mi_cell_unlock.Name = "mi_cell_unlock"
        Me.mi_cell_unlock.Size = New System.Drawing.Size(163, 22)
        Me.mi_cell_unlock.Text = "Unlock Cell"
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(160, 6)
        '
        'mi_cell_image
        '
        Me.mi_cell_image.Name = "mi_cell_image"
        Me.mi_cell_image.Size = New System.Drawing.Size(163, 22)
        Me.mi_cell_image.Text = "Set Cell Image..."
        '
        'mi_help
        '
        Me.mi_help.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mi_help_manual, Me.mi_help_updates, Me.ToolStripSeparator24, Me.mi_help_about})
        Me.mi_help.Name = "mi_help"
        Me.mi_help.Size = New System.Drawing.Size(44, 20)
        Me.mi_help.Text = "&Help"
        '
        'mi_help_manual
        '
        Me.mi_help_manual.Name = "mi_help_manual"
        Me.mi_help_manual.Size = New System.Drawing.Size(171, 22)
        Me.mi_help_manual.Text = "User Manual"
        '
        'mi_help_updates
        '
        Me.mi_help_updates.Name = "mi_help_updates"
        Me.mi_help_updates.Size = New System.Drawing.Size(171, 22)
        Me.mi_help_updates.Text = "Check for Updates"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(168, 6)
        '
        'mi_help_about
        '
        Me.mi_help_about.Name = "mi_help_about"
        Me.mi_help_about.Size = New System.Drawing.Size(171, 22)
        Me.mi_help_about.Text = "About ..."
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bt_new, Me.bt_open, Me.bt_save, Me.ToolStripSeparator12, Me.bt_print, Me.bt_printpreview, Me.ToolStripSeparator13, Me.bt_cut, Me.bt_copy, Me.bt_paste, Me.ToolStripSeparator14, Me.bt_mergecell, Me.bt_unmergecell, Me.ToolStripSeparator15, Me.bt_insertrow, Me.bt_insertcolumn, Me.bt_deleterow, Me.bt_deletecolumn, Me.ToolStripSeparator16, Me.bt_image, Me.bt_help})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(976, 25)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'bt_new
        '
        Me.bt_new.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_new.Image = CType(resources.GetObject("bt_new.Image"), System.Drawing.Image)
        Me.bt_new.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_new.Name = "bt_new"
        Me.bt_new.Size = New System.Drawing.Size(23, 22)
        Me.bt_new.Text = "ToolStripButton1"
        Me.bt_new.ToolTipText = "New"
        '
        'bt_open
        '
        Me.bt_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_open.Image = CType(resources.GetObject("bt_open.Image"), System.Drawing.Image)
        Me.bt_open.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_open.Name = "bt_open"
        Me.bt_open.Size = New System.Drawing.Size(23, 22)
        Me.bt_open.Text = "ToolStripButton2"
        Me.bt_open.ToolTipText = "Open"
        '
        'bt_save
        '
        Me.bt_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_save.Image = CType(resources.GetObject("bt_save.Image"), System.Drawing.Image)
        Me.bt_save.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_save.Name = "bt_save"
        Me.bt_save.Size = New System.Drawing.Size(23, 22)
        Me.bt_save.Text = "ToolStripButton3"
        Me.bt_save.ToolTipText = "Save"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 25)
        '
        'bt_print
        '
        Me.bt_print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_print.Image = CType(resources.GetObject("bt_print.Image"), System.Drawing.Image)
        Me.bt_print.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_print.Name = "bt_print"
        Me.bt_print.Size = New System.Drawing.Size(23, 22)
        Me.bt_print.Text = "ToolStripButton4"
        Me.bt_print.ToolTipText = "Print"
        '
        'bt_printpreview
        '
        Me.bt_printpreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_printpreview.Image = CType(resources.GetObject("bt_printpreview.Image"), System.Drawing.Image)
        Me.bt_printpreview.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_printpreview.Name = "bt_printpreview"
        Me.bt_printpreview.Size = New System.Drawing.Size(23, 22)
        Me.bt_printpreview.Text = "ToolStripButton5"
        Me.bt_printpreview.ToolTipText = "Print Preview"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(6, 25)
        '
        'bt_cut
        '
        Me.bt_cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_cut.Image = CType(resources.GetObject("bt_cut.Image"), System.Drawing.Image)
        Me.bt_cut.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_cut.Name = "bt_cut"
        Me.bt_cut.Size = New System.Drawing.Size(23, 22)
        Me.bt_cut.Text = "ToolStripButton6"
        Me.bt_cut.ToolTipText = "Cut"
        '
        'bt_copy
        '
        Me.bt_copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_copy.Image = CType(resources.GetObject("bt_copy.Image"), System.Drawing.Image)
        Me.bt_copy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_copy.Name = "bt_copy"
        Me.bt_copy.Size = New System.Drawing.Size(23, 22)
        Me.bt_copy.Text = "ToolStripButton7"
        Me.bt_copy.ToolTipText = "Copy"
        '
        'bt_paste
        '
        Me.bt_paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_paste.Image = CType(resources.GetObject("bt_paste.Image"), System.Drawing.Image)
        Me.bt_paste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_paste.Name = "bt_paste"
        Me.bt_paste.Size = New System.Drawing.Size(23, 22)
        Me.bt_paste.Text = "ToolStripButton8"
        Me.bt_paste.ToolTipText = "Paste"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 25)
        '
        'bt_mergecell
        '
        Me.bt_mergecell.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_mergecell.Image = CType(resources.GetObject("bt_mergecell.Image"), System.Drawing.Image)
        Me.bt_mergecell.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_mergecell.Name = "bt_mergecell"
        Me.bt_mergecell.Size = New System.Drawing.Size(23, 22)
        Me.bt_mergecell.Text = "ToolStripButton9"
        Me.bt_mergecell.ToolTipText = "Merge"
        '
        'bt_unmergecell
        '
        Me.bt_unmergecell.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_unmergecell.Image = CType(resources.GetObject("bt_unmergecell.Image"), System.Drawing.Image)
        Me.bt_unmergecell.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_unmergecell.Name = "bt_unmergecell"
        Me.bt_unmergecell.Size = New System.Drawing.Size(23, 22)
        Me.bt_unmergecell.Text = "ToolStripButton10"
        Me.bt_unmergecell.ToolTipText = "Unmerge"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 25)
        '
        'bt_insertrow
        '
        Me.bt_insertrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_insertrow.Image = CType(resources.GetObject("bt_insertrow.Image"), System.Drawing.Image)
        Me.bt_insertrow.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_insertrow.Name = "bt_insertrow"
        Me.bt_insertrow.Size = New System.Drawing.Size(23, 22)
        Me.bt_insertrow.Text = "ToolStripButton11"
        Me.bt_insertrow.ToolTipText = "Insert Rows"
        '
        'bt_insertcolumn
        '
        Me.bt_insertcolumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_insertcolumn.Image = CType(resources.GetObject("bt_insertcolumn.Image"), System.Drawing.Image)
        Me.bt_insertcolumn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_insertcolumn.Name = "bt_insertcolumn"
        Me.bt_insertcolumn.Size = New System.Drawing.Size(23, 22)
        Me.bt_insertcolumn.Text = "ToolStripButton12"
        Me.bt_insertcolumn.ToolTipText = "Insert Columns"
        '
        'bt_deleterow
        '
        Me.bt_deleterow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_deleterow.Image = CType(resources.GetObject("bt_deleterow.Image"), System.Drawing.Image)
        Me.bt_deleterow.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_deleterow.Name = "bt_deleterow"
        Me.bt_deleterow.Size = New System.Drawing.Size(23, 22)
        Me.bt_deleterow.Text = "ToolStripButton13"
        Me.bt_deleterow.ToolTipText = "Delete Rows"
        '
        'bt_deletecolumn
        '
        Me.bt_deletecolumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_deletecolumn.Image = CType(resources.GetObject("bt_deletecolumn.Image"), System.Drawing.Image)
        Me.bt_deletecolumn.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_deletecolumn.Name = "bt_deletecolumn"
        Me.bt_deletecolumn.Size = New System.Drawing.Size(23, 22)
        Me.bt_deletecolumn.Text = "ToolStripButton14"
        Me.bt_deletecolumn.ToolTipText = "Delete Columns"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(6, 25)
        '
        'bt_image
        '
        Me.bt_image.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_image.Image = CType(resources.GetObject("bt_image.Image"), System.Drawing.Image)
        Me.bt_image.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_image.Name = "bt_image"
        Me.bt_image.Size = New System.Drawing.Size(23, 22)
        Me.bt_image.Text = "ToolStripButton15"
        Me.bt_image.ToolTipText = "Set Cell Image"
        '
        'bt_help
        '
        Me.bt_help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_help.Image = CType(resources.GetObject("bt_help.Image"), System.Drawing.Image)
        Me.bt_help.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_help.Name = "bt_help"
        Me.bt_help.Size = New System.Drawing.Size(23, 22)
        Me.bt_help.Text = "ToolStripButton16"
        Me.bt_help.ToolTipText = "Help"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cboFontName, Me.cboFontSize, Me.bt_fontbold, Me.bt_fontitalic, Me.bt_fontunderline, Me.bt_fontstrikeout, Me.ToolStripSeparator17, Me.ToolStripLabel1, Me.bt_imagealign, Me.ToolStripSeparator18, Me.ToolStripLabel2, Me.bt_textalign, Me.ToolStripSeparator19, Me.ToolStripLabel5, Me.bt_cellcolor, Me.ToolStripSeparator20, Me.ToolStripLabel6, Me.bt_textcolor, Me.ToolStripSeparator21, Me.ToolStripLabel7, Me.bt_cellborder})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 49)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(976, 25)
        Me.ToolStrip2.TabIndex = 0
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'cboFontName
        '
        Me.cboFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFontName.Name = "cboFontName"
        Me.cboFontName.Size = New System.Drawing.Size(121, 25)
        Me.cboFontName.ToolTipText = "Font Name"
        '
        'cboFontSize
        '
        Me.cboFontSize.Name = "cboFontSize"
        Me.cboFontSize.Size = New System.Drawing.Size(80, 25)
        Me.cboFontSize.ToolTipText = "Font Size"
        '
        'bt_fontbold
        '
        Me.bt_fontbold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_fontbold.Image = CType(resources.GetObject("bt_fontbold.Image"), System.Drawing.Image)
        Me.bt_fontbold.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_fontbold.Name = "bt_fontbold"
        Me.bt_fontbold.Size = New System.Drawing.Size(23, 22)
        Me.bt_fontbold.Text = "ToolStripButton17"
        Me.bt_fontbold.ToolTipText = "Bold"
        '
        'bt_fontitalic
        '
        Me.bt_fontitalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_fontitalic.Image = CType(resources.GetObject("bt_fontitalic.Image"), System.Drawing.Image)
        Me.bt_fontitalic.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_fontitalic.Name = "bt_fontitalic"
        Me.bt_fontitalic.Size = New System.Drawing.Size(23, 22)
        Me.bt_fontitalic.Text = "ToolStripButton18"
        Me.bt_fontitalic.ToolTipText = "Italic"
        '
        'bt_fontunderline
        '
        Me.bt_fontunderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_fontunderline.Image = CType(resources.GetObject("bt_fontunderline.Image"), System.Drawing.Image)
        Me.bt_fontunderline.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_fontunderline.Name = "bt_fontunderline"
        Me.bt_fontunderline.Size = New System.Drawing.Size(23, 22)
        Me.bt_fontunderline.Text = "ToolStripButton19"
        Me.bt_fontunderline.ToolTipText = "Underline"
        '
        'bt_fontstrikeout
        '
        Me.bt_fontstrikeout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_fontstrikeout.Image = CType(resources.GetObject("bt_fontstrikeout.Image"), System.Drawing.Image)
        Me.bt_fontstrikeout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_fontstrikeout.Name = "bt_fontstrikeout"
        Me.bt_fontstrikeout.Size = New System.Drawing.Size(23, 22)
        Me.bt_fontstrikeout.Text = "ToolStripButton20"
        Me.bt_fontstrikeout.ToolTipText = "Strikeout"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(68, 22)
        Me.ToolStripLabel1.Text = "ImageAlign"
        '
        'bt_imagealign
        '
        Me.bt_imagealign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_imagealign.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bt_imagealign_lefttop, Me.bt_imagealign_leftcenter, Me.bt_imagealign_leftbottom, Me.bt_imagealign_centertop, Me.bt_imagealign_centercenter, Me.bt_imagealign_centerbottom, Me.bt_imagealign_righttop, Me.bt_imagealign_rightcenter, Me.bt_imagealign_rightbottom, Me.bt_imagealign_stretch})
        Me.bt_imagealign.Image = CType(resources.GetObject("bt_imagealign.Image"), System.Drawing.Image)
        Me.bt_imagealign.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_imagealign.Name = "bt_imagealign"
        Me.bt_imagealign.Size = New System.Drawing.Size(32, 22)
        Me.bt_imagealign.Text = "ToolStripSplitButton1"
        Me.bt_imagealign.ToolTipText = "Picture Alignment"
        '
        'bt_imagealign_lefttop
        '
        Me.bt_imagealign_lefttop.Image = CType(resources.GetObject("bt_imagealign_lefttop.Image"), System.Drawing.Image)
        Me.bt_imagealign_lefttop.Name = "bt_imagealign_lefttop"
        Me.bt_imagealign_lefttop.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_lefttop.Text = "0 - Left Top"
        '
        'bt_imagealign_leftcenter
        '
        Me.bt_imagealign_leftcenter.Image = CType(resources.GetObject("bt_imagealign_leftcenter.Image"), System.Drawing.Image)
        Me.bt_imagealign_leftcenter.Name = "bt_imagealign_leftcenter"
        Me.bt_imagealign_leftcenter.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_leftcenter.Text = "1 - Left Center"
        '
        'bt_imagealign_leftbottom
        '
        Me.bt_imagealign_leftbottom.Image = CType(resources.GetObject("bt_imagealign_leftbottom.Image"), System.Drawing.Image)
        Me.bt_imagealign_leftbottom.Name = "bt_imagealign_leftbottom"
        Me.bt_imagealign_leftbottom.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_leftbottom.Text = "2 - Left Bottom"
        '
        'bt_imagealign_centertop
        '
        Me.bt_imagealign_centertop.Image = CType(resources.GetObject("bt_imagealign_centertop.Image"), System.Drawing.Image)
        Me.bt_imagealign_centertop.Name = "bt_imagealign_centertop"
        Me.bt_imagealign_centertop.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_centertop.Text = "3 - Center Top"
        '
        'bt_imagealign_centercenter
        '
        Me.bt_imagealign_centercenter.Image = CType(resources.GetObject("bt_imagealign_centercenter.Image"), System.Drawing.Image)
        Me.bt_imagealign_centercenter.Name = "bt_imagealign_centercenter"
        Me.bt_imagealign_centercenter.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_centercenter.Text = "4 - Center Center"
        '
        'bt_imagealign_centerbottom
        '
        Me.bt_imagealign_centerbottom.Image = CType(resources.GetObject("bt_imagealign_centerbottom.Image"), System.Drawing.Image)
        Me.bt_imagealign_centerbottom.Name = "bt_imagealign_centerbottom"
        Me.bt_imagealign_centerbottom.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_centerbottom.Text = "5 - Center Bottom"
        '
        'bt_imagealign_righttop
        '
        Me.bt_imagealign_righttop.Image = CType(resources.GetObject("bt_imagealign_righttop.Image"), System.Drawing.Image)
        Me.bt_imagealign_righttop.Name = "bt_imagealign_righttop"
        Me.bt_imagealign_righttop.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_righttop.Text = "6 - Right Top"
        '
        'bt_imagealign_rightcenter
        '
        Me.bt_imagealign_rightcenter.Image = CType(resources.GetObject("bt_imagealign_rightcenter.Image"), System.Drawing.Image)
        Me.bt_imagealign_rightcenter.Name = "bt_imagealign_rightcenter"
        Me.bt_imagealign_rightcenter.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_rightcenter.Text = "7 - Right Center"
        '
        'bt_imagealign_rightbottom
        '
        Me.bt_imagealign_rightbottom.Image = CType(resources.GetObject("bt_imagealign_rightbottom.Image"), System.Drawing.Image)
        Me.bt_imagealign_rightbottom.Name = "bt_imagealign_rightbottom"
        Me.bt_imagealign_rightbottom.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_rightbottom.Text = "8 - Right Bottom"
        '
        'bt_imagealign_stretch
        '
        Me.bt_imagealign_stretch.Image = CType(resources.GetObject("bt_imagealign_stretch.Image"), System.Drawing.Image)
        Me.bt_imagealign_stretch.Name = "bt_imagealign_stretch"
        Me.bt_imagealign_stretch.Size = New System.Drawing.Size(169, 22)
        Me.bt_imagealign_stretch.Text = "9 - Stretch"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(57, 22)
        Me.ToolStripLabel2.Text = "TextAlign"
        '
        'bt_textalign
        '
        Me.bt_textalign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.bt_textalign.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bt_textalign_lefttop, Me.bt_textalign_leftcenter, Me.bt_textalign_leftbottom, Me.bt_textalign_centertop, Me.bt_textalign_centercenter, Me.bt_textalign_centerbottom, Me.bt_textalign_righttop, Me.bt_textalign_rightcenter, Me.bt_textalign_rightbottom})
        Me.bt_textalign.Image = CType(resources.GetObject("bt_textalign.Image"), System.Drawing.Image)
        Me.bt_textalign.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_textalign.Name = "bt_textalign"
        Me.bt_textalign.Size = New System.Drawing.Size(32, 22)
        Me.bt_textalign.Text = "ToolStripSplitButton1"
        Me.bt_textalign.ToolTipText = "Text Alignment"
        '
        'bt_textalign_lefttop
        '
        Me.bt_textalign_lefttop.Image = CType(resources.GetObject("bt_textalign_lefttop.Image"), System.Drawing.Image)
        Me.bt_textalign_lefttop.Name = "bt_textalign_lefttop"
        Me.bt_textalign_lefttop.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_lefttop.Text = "0 - Left Top"
        '
        'bt_textalign_leftcenter
        '
        Me.bt_textalign_leftcenter.Image = CType(resources.GetObject("bt_textalign_leftcenter.Image"), System.Drawing.Image)
        Me.bt_textalign_leftcenter.Name = "bt_textalign_leftcenter"
        Me.bt_textalign_leftcenter.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_leftcenter.Text = "1 - Left Center"
        '
        'bt_textalign_leftbottom
        '
        Me.bt_textalign_leftbottom.Image = CType(resources.GetObject("bt_textalign_leftbottom.Image"), System.Drawing.Image)
        Me.bt_textalign_leftbottom.Name = "bt_textalign_leftbottom"
        Me.bt_textalign_leftbottom.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_leftbottom.Text = "2 - Left Bottom"
        '
        'bt_textalign_centertop
        '
        Me.bt_textalign_centertop.Image = CType(resources.GetObject("bt_textalign_centertop.Image"), System.Drawing.Image)
        Me.bt_textalign_centertop.Name = "bt_textalign_centertop"
        Me.bt_textalign_centertop.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_centertop.Text = "3 - Center Top"
        '
        'bt_textalign_centercenter
        '
        Me.bt_textalign_centercenter.Image = CType(resources.GetObject("bt_textalign_centercenter.Image"), System.Drawing.Image)
        Me.bt_textalign_centercenter.Name = "bt_textalign_centercenter"
        Me.bt_textalign_centercenter.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_centercenter.Text = "4 - Center Center"
        '
        'bt_textalign_centerbottom
        '
        Me.bt_textalign_centerbottom.Image = CType(resources.GetObject("bt_textalign_centerbottom.Image"), System.Drawing.Image)
        Me.bt_textalign_centerbottom.Name = "bt_textalign_centerbottom"
        Me.bt_textalign_centerbottom.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_centerbottom.Text = "5 - Center Bottom"
        '
        'bt_textalign_righttop
        '
        Me.bt_textalign_righttop.Image = CType(resources.GetObject("bt_textalign_righttop.Image"), System.Drawing.Image)
        Me.bt_textalign_righttop.Name = "bt_textalign_righttop"
        Me.bt_textalign_righttop.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_righttop.Text = "6 - Right Top"
        '
        'bt_textalign_rightcenter
        '
        Me.bt_textalign_rightcenter.Image = CType(resources.GetObject("bt_textalign_rightcenter.Image"), System.Drawing.Image)
        Me.bt_textalign_rightcenter.Name = "bt_textalign_rightcenter"
        Me.bt_textalign_rightcenter.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_rightcenter.Text = "7 - Right Center"
        '
        'bt_textalign_rightbottom
        '
        Me.bt_textalign_rightbottom.Image = CType(resources.GetObject("bt_textalign_rightbottom.Image"), System.Drawing.Image)
        Me.bt_textalign_rightbottom.Name = "bt_textalign_rightbottom"
        Me.bt_textalign_rightbottom.Size = New System.Drawing.Size(169, 22)
        Me.bt_textalign_rightbottom.Text = "8 - Right Bottom"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel5
        '
        Me.ToolStripLabel5.Name = "ToolStripLabel5"
        Me.ToolStripLabel5.Size = New System.Drawing.Size(59, 22)
        Me.ToolStripLabel5.Text = "Cell Color"
        '
        'bt_cellcolor
        '
        Me.bt_cellcolor.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.bt_cellcolor.Name = "bt_cellcolor"
        Me.bt_cellcolor.Size = New System.Drawing.Size(35, 22)
        Me.bt_cellcolor.Text = "[]"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel6
        '
        Me.ToolStripLabel6.Name = "ToolStripLabel6"
        Me.ToolStripLabel6.Size = New System.Drawing.Size(61, 22)
        Me.ToolStripLabel6.Text = "Text Color"
        '
        'bt_textcolor
        '
        Me.bt_textcolor.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.bt_textcolor.Name = "bt_textcolor"
        Me.bt_textcolor.Size = New System.Drawing.Size(35, 22)
        Me.bt_textcolor.Text = "[]"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabel7
        '
        Me.ToolStripLabel7.Name = "ToolStripLabel7"
        Me.ToolStripLabel7.Size = New System.Drawing.Size(65, 22)
        Me.ToolStripLabel7.Text = "Cell Border"
        '
        'bt_cellborder
        '
        Me.bt_cellborder.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bt_cellborder_none, Me.bt_cellborder_around, Me.bt_cellborder_left, Me.bt_cellborder_top, Me.bt_cellborder_right, Me.bt_cellborder_bottom, Me.bt_cellborder_diagonalup, Me.bt_cellborder_diagonaldown, Me.bt_cellborder_inside, Me.bt_cellborder_insidevertical, Me.bt_cellborder_insidehorizontal, Me.ToolStripSeparator22, Me.bt_cellborder_thin, Me.bt_cellborder_thick, Me.bt_cellborder_dot, Me.ToolStripSeparator23, Me.bt_cellborder_color})
        Me.bt_cellborder.Image = CType(resources.GetObject("bt_cellborder.Image"), System.Drawing.Image)
        Me.bt_cellborder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.bt_cellborder.Name = "bt_cellborder"
        Me.bt_cellborder.Size = New System.Drawing.Size(32, 22)
        '
        'bt_cellborder_none
        '
        Me.bt_cellborder_none.Image = CType(resources.GetObject("bt_cellborder_none.Image"), System.Drawing.Image)
        Me.bt_cellborder_none.Name = "bt_cellborder_none"
        Me.bt_cellborder_none.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_none.Text = "None"
        '
        'bt_cellborder_around
        '
        Me.bt_cellborder_around.Image = CType(resources.GetObject("bt_cellborder_around.Image"), System.Drawing.Image)
        Me.bt_cellborder_around.Name = "bt_cellborder_around"
        Me.bt_cellborder_around.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_around.Text = "flexEdgeAround"
        '
        'bt_cellborder_left
        '
        Me.bt_cellborder_left.Image = CType(resources.GetObject("bt_cellborder_left.Image"), System.Drawing.Image)
        Me.bt_cellborder_left.Name = "bt_cellborder_left"
        Me.bt_cellborder_left.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_left.Text = "flexEdgeLeft"
        '
        'bt_cellborder_top
        '
        Me.bt_cellborder_top.Image = CType(resources.GetObject("bt_cellborder_top.Image"), System.Drawing.Image)
        Me.bt_cellborder_top.Name = "bt_cellborder_top"
        Me.bt_cellborder_top.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_top.Text = "flexEdgeTop"
        '
        'bt_cellborder_right
        '
        Me.bt_cellborder_right.Image = CType(resources.GetObject("bt_cellborder_right.Image"), System.Drawing.Image)
        Me.bt_cellborder_right.Name = "bt_cellborder_right"
        Me.bt_cellborder_right.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_right.Text = "flexEdgeRight"
        '
        'bt_cellborder_bottom
        '
        Me.bt_cellborder_bottom.Image = CType(resources.GetObject("bt_cellborder_bottom.Image"), System.Drawing.Image)
        Me.bt_cellborder_bottom.Name = "bt_cellborder_bottom"
        Me.bt_cellborder_bottom.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_bottom.Text = "flexEdgeBottom"
        '
        'bt_cellborder_diagonalup
        '
        Me.bt_cellborder_diagonalup.Image = CType(resources.GetObject("bt_cellborder_diagonalup.Image"), System.Drawing.Image)
        Me.bt_cellborder_diagonalup.Name = "bt_cellborder_diagonalup"
        Me.bt_cellborder_diagonalup.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_diagonalup.Text = "flexDiagonalUp"
        '
        'bt_cellborder_diagonaldown
        '
        Me.bt_cellborder_diagonaldown.Image = CType(resources.GetObject("bt_cellborder_diagonaldown.Image"), System.Drawing.Image)
        Me.bt_cellborder_diagonaldown.Name = "bt_cellborder_diagonaldown"
        Me.bt_cellborder_diagonaldown.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_diagonaldown.Text = "flexDiagonalDown"
        '
        'bt_cellborder_inside
        '
        Me.bt_cellborder_inside.Image = CType(resources.GetObject("bt_cellborder_inside.Image"), System.Drawing.Image)
        Me.bt_cellborder_inside.Name = "bt_cellborder_inside"
        Me.bt_cellborder_inside.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_inside.Text = "flexInside"
        '
        'bt_cellborder_insidevertical
        '
        Me.bt_cellborder_insidevertical.Image = CType(resources.GetObject("bt_cellborder_insidevertical.Image"), System.Drawing.Image)
        Me.bt_cellborder_insidevertical.Name = "bt_cellborder_insidevertical"
        Me.bt_cellborder_insidevertical.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_insidevertical.Text = "flexInsideVertical"
        '
        'bt_cellborder_insidehorizontal
        '
        Me.bt_cellborder_insidehorizontal.Image = CType(resources.GetObject("bt_cellborder_insidehorizontal.Image"), System.Drawing.Image)
        Me.bt_cellborder_insidehorizontal.Name = "bt_cellborder_insidehorizontal"
        Me.bt_cellborder_insidehorizontal.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_insidehorizontal.Text = "flexInsideHorizontal"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(175, 6)
        '
        'bt_cellborder_thin
        '
        Me.bt_cellborder_thin.Checked = True
        Me.bt_cellborder_thin.CheckState = System.Windows.Forms.CheckState.Checked
        Me.bt_cellborder_thin.Name = "bt_cellborder_thin"
        Me.bt_cellborder_thin.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_thin.Text = "flexLineStyleThin"
        '
        'bt_cellborder_thick
        '
        Me.bt_cellborder_thick.Name = "bt_cellborder_thick"
        Me.bt_cellborder_thick.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_thick.Text = "flexLineStyleThick"
        '
        'bt_cellborder_dot
        '
        Me.bt_cellborder_dot.Name = "bt_cellborder_dot"
        Me.bt_cellborder_dot.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_dot.Text = "flexLineStyleDot"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(175, 6)
        '
        'bt_cellborder_color
        '
        Me.bt_cellborder_color.Name = "bt_cellborder_color"
        Me.bt_cellborder_color.Size = New System.Drawing.Size(178, 22)
        Me.bt_cellborder_color.Text = "Cell Border Color"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 759)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(976, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(961, 17)
        Me.ToolStripStatusLabel1.Spring = True
        Me.ToolStripStatusLabel1.Text = "Ready"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabProperty
        '
        Me.tabProperty.Controls.Add(Me.TabPage1)
        Me.tabProperty.Controls.Add(Me.TabPage2)
        Me.tabProperty.Controls.Add(Me.TabPage3)
        Me.tabProperty.Location = New System.Drawing.Point(635, 83)
        Me.tabProperty.Name = "tabProperty"
        Me.tabProperty.SelectedIndex = 0
        Me.tabProperty.Size = New System.Drawing.Size(329, 647)
        Me.tabProperty.TabIndex = 6
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.cboApplySelectionToImage)
        Me.TabPage1.Controls.Add(Me.cboShowContextMenu)
        Me.TabPage1.Controls.Add(Me.cboShowComboButton)
        Me.TabPage1.Controls.Add(Me.cboPictureOver)
        Me.TabPage1.Controls.Add(Me.cboScrollBarStyle)
        Me.TabPage1.Controls.Add(Me.cboScrollBars)
        Me.TabPage1.Controls.Add(Me.cboFocusRect)
        Me.TabPage1.Controls.Add(Me.cboHighLight)
        Me.TabPage1.Controls.Add(Me.cboSelectionMode)
        Me.TabPage1.Controls.Add(Me.cboExtendLastCol)
        Me.TabPage1.Controls.Add(Me.cboEnterKeyBehavior)
        Me.TabPage1.Controls.Add(Me.cboEllipsis)
        Me.TabPage1.Controls.Add(Me.cboEditable)
        Me.TabPage1.Controls.Add(Me.cboButtonLocked)
        Me.TabPage1.Controls.Add(Me.cboDateFormat)
        Me.TabPage1.Controls.Add(Me.cboGridLines)
        Me.TabPage1.Controls.Add(Me.cboBorderStyle)
        Me.TabPage1.Controls.Add(Me.cboAllowUserSort)
        Me.TabPage1.Controls.Add(Me.cboAllowUserResizing)
        Me.TabPage1.Controls.Add(Me.cboAllowUserReorder)
        Me.TabPage1.Controls.Add(Me.cboAllowSelection)
        Me.TabPage1.Controls.Add(Me.cboAllowBigSelection)
        Me.TabPage1.Controls.Add(Me.Label22)
        Me.TabPage1.Controls.Add(Me.Label21)
        Me.TabPage1.Controls.Add(Me.Label20)
        Me.TabPage1.Controls.Add(Me.Label19)
        Me.TabPage1.Controls.Add(Me.Label18)
        Me.TabPage1.Controls.Add(Me.Label17)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.Label14)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.Label10)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(321, 621)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Page1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'cboApplySelectionToImage
        '
        Me.cboApplySelectionToImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboApplySelectionToImage.FormattingEnabled = True
        Me.cboApplySelectionToImage.Items.AddRange(New Object() {"True", "False"})
        Me.cboApplySelectionToImage.Location = New System.Drawing.Point(142, 582)
        Me.cboApplySelectionToImage.Name = "cboApplySelectionToImage"
        Me.cboApplySelectionToImage.Size = New System.Drawing.Size(168, 21)
        Me.cboApplySelectionToImage.TabIndex = 43
        '
        'cboShowContextMenu
        '
        Me.cboShowContextMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShowContextMenu.FormattingEnabled = True
        Me.cboShowContextMenu.Items.AddRange(New Object() {"True", "False"})
        Me.cboShowContextMenu.Location = New System.Drawing.Point(142, 555)
        Me.cboShowContextMenu.Name = "cboShowContextMenu"
        Me.cboShowContextMenu.Size = New System.Drawing.Size(168, 21)
        Me.cboShowContextMenu.TabIndex = 42
        '
        'cboShowComboButton
        '
        Me.cboShowComboButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShowComboButton.FormattingEnabled = True
        Me.cboShowComboButton.Items.AddRange(New Object() {"0-Focus", "1-Editing", "2-MouseMove"})
        Me.cboShowComboButton.Location = New System.Drawing.Point(142, 528)
        Me.cboShowComboButton.Name = "cboShowComboButton"
        Me.cboShowComboButton.Size = New System.Drawing.Size(168, 21)
        Me.cboShowComboButton.TabIndex = 41
        '
        'cboPictureOver
        '
        Me.cboPictureOver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPictureOver.FormattingEnabled = True
        Me.cboPictureOver.Items.AddRange(New Object() {"True", "False"})
        Me.cboPictureOver.Location = New System.Drawing.Point(142, 501)
        Me.cboPictureOver.Name = "cboPictureOver"
        Me.cboPictureOver.Size = New System.Drawing.Size(168, 21)
        Me.cboPictureOver.TabIndex = 40
        '
        'cboScrollBarStyle
        '
        Me.cboScrollBarStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScrollBarStyle.FormattingEnabled = True
        Me.cboScrollBarStyle.Items.AddRange(New Object() {"0-Classic", "1-Flat", "2-Themed"})
        Me.cboScrollBarStyle.Location = New System.Drawing.Point(142, 473)
        Me.cboScrollBarStyle.Name = "cboScrollBarStyle"
        Me.cboScrollBarStyle.Size = New System.Drawing.Size(168, 21)
        Me.cboScrollBarStyle.TabIndex = 39
        '
        'cboScrollBars
        '
        Me.cboScrollBars.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboScrollBars.FormattingEnabled = True
        Me.cboScrollBars.Items.AddRange(New Object() {"0-None", "1-Horizontal", "2-Vertical", "3-Both"})
        Me.cboScrollBars.Location = New System.Drawing.Point(142, 446)
        Me.cboScrollBars.Name = "cboScrollBars"
        Me.cboScrollBars.Size = New System.Drawing.Size(168, 21)
        Me.cboScrollBars.TabIndex = 38
        '
        'cboFocusRect
        '
        Me.cboFocusRect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFocusRect.FormattingEnabled = True
        Me.cboFocusRect.Items.AddRange(New Object() {"0-None", "1-ByCell", "2-ByRow"})
        Me.cboFocusRect.Location = New System.Drawing.Point(142, 419)
        Me.cboFocusRect.Name = "cboFocusRect"
        Me.cboFocusRect.Size = New System.Drawing.Size(168, 21)
        Me.cboFocusRect.TabIndex = 37
        '
        'cboHighLight
        '
        Me.cboHighLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHighLight.FormattingEnabled = True
        Me.cboHighLight.Items.AddRange(New Object() {"0-Never", "1-Always"})
        Me.cboHighLight.Location = New System.Drawing.Point(142, 392)
        Me.cboHighLight.Name = "cboHighLight"
        Me.cboHighLight.Size = New System.Drawing.Size(168, 21)
        Me.cboHighLight.TabIndex = 36
        '
        'cboSelectionMode
        '
        Me.cboSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSelectionMode.FormattingEnabled = True
        Me.cboSelectionMode.Items.AddRange(New Object() {"0-Free", "1-ByRow", "2-ByColumn", "3-ListBox"})
        Me.cboSelectionMode.Location = New System.Drawing.Point(142, 365)
        Me.cboSelectionMode.Name = "cboSelectionMode"
        Me.cboSelectionMode.Size = New System.Drawing.Size(168, 21)
        Me.cboSelectionMode.TabIndex = 35
        '
        'cboExtendLastCol
        '
        Me.cboExtendLastCol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboExtendLastCol.FormattingEnabled = True
        Me.cboExtendLastCol.Items.AddRange(New Object() {"True", "False"})
        Me.cboExtendLastCol.Location = New System.Drawing.Point(142, 338)
        Me.cboExtendLastCol.Name = "cboExtendLastCol"
        Me.cboExtendLastCol.Size = New System.Drawing.Size(168, 21)
        Me.cboExtendLastCol.TabIndex = 34
        '
        'cboEnterKeyBehavior
        '
        Me.cboEnterKeyBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEnterKeyBehavior.FormattingEnabled = True
        Me.cboEnterKeyBehavior.Items.AddRange(New Object() {"0-NextCol", "1-NextRow", "2-EnterKeyDefault"})
        Me.cboEnterKeyBehavior.Location = New System.Drawing.Point(142, 311)
        Me.cboEnterKeyBehavior.Name = "cboEnterKeyBehavior"
        Me.cboEnterKeyBehavior.Size = New System.Drawing.Size(168, 21)
        Me.cboEnterKeyBehavior.TabIndex = 33
        '
        'cboEllipsis
        '
        Me.cboEllipsis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEllipsis.FormattingEnabled = True
        Me.cboEllipsis.Items.AddRange(New Object() {"True", "False"})
        Me.cboEllipsis.Location = New System.Drawing.Point(142, 284)
        Me.cboEllipsis.Name = "cboEllipsis"
        Me.cboEllipsis.Size = New System.Drawing.Size(168, 21)
        Me.cboEllipsis.TabIndex = 32
        '
        'cboEditable
        '
        Me.cboEditable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEditable.FormattingEnabled = True
        Me.cboEditable.Items.AddRange(New Object() {"True", "False"})
        Me.cboEditable.Location = New System.Drawing.Point(142, 257)
        Me.cboEditable.Name = "cboEditable"
        Me.cboEditable.Size = New System.Drawing.Size(168, 21)
        Me.cboEditable.TabIndex = 31
        '
        'cboButtonLocked
        '
        Me.cboButtonLocked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboButtonLocked.FormattingEnabled = True
        Me.cboButtonLocked.Items.AddRange(New Object() {"True", "False"})
        Me.cboButtonLocked.Location = New System.Drawing.Point(142, 230)
        Me.cboButtonLocked.Name = "cboButtonLocked"
        Me.cboButtonLocked.Size = New System.Drawing.Size(168, 21)
        Me.cboButtonLocked.TabIndex = 30
        '
        'cboDateFormat
        '
        Me.cboDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDateFormat.FormattingEnabled = True
        Me.cboDateFormat.Items.AddRange(New Object() {"0-YMD", "1-MDY", "2-DMY"})
        Me.cboDateFormat.Location = New System.Drawing.Point(142, 203)
        Me.cboDateFormat.Name = "cboDateFormat"
        Me.cboDateFormat.Size = New System.Drawing.Size(168, 21)
        Me.cboDateFormat.TabIndex = 29
        '
        'cboGridLines
        '
        Me.cboGridLines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGridLines.FormattingEnabled = True
        Me.cboGridLines.Items.AddRange(New Object() {"0-None", "1-Horizontal", "2-Vertical", "3-Both"})
        Me.cboGridLines.Location = New System.Drawing.Point(142, 176)
        Me.cboGridLines.Name = "cboGridLines"
        Me.cboGridLines.Size = New System.Drawing.Size(168, 21)
        Me.cboGridLines.TabIndex = 28
        '
        'cboBorderStyle
        '
        Me.cboBorderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBorderStyle.FormattingEnabled = True
        Me.cboBorderStyle.Items.AddRange(New Object() {"0-None", "1-FixedSingle", "2-Fixed3D"})
        Me.cboBorderStyle.Location = New System.Drawing.Point(142, 148)
        Me.cboBorderStyle.Name = "cboBorderStyle"
        Me.cboBorderStyle.Size = New System.Drawing.Size(168, 21)
        Me.cboBorderStyle.TabIndex = 27
        '
        'cboAllowUserSort
        '
        Me.cboAllowUserSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowUserSort.FormattingEnabled = True
        Me.cboAllowUserSort.Items.AddRange(New Object() {"True", "False"})
        Me.cboAllowUserSort.Location = New System.Drawing.Point(142, 121)
        Me.cboAllowUserSort.Name = "cboAllowUserSort"
        Me.cboAllowUserSort.Size = New System.Drawing.Size(168, 21)
        Me.cboAllowUserSort.TabIndex = 26
        '
        'cboAllowUserResizing
        '
        Me.cboAllowUserResizing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowUserResizing.FormattingEnabled = True
        Me.cboAllowUserResizing.Items.AddRange(New Object() {"0-None", "1-Columns", "2-Rows", "3-Both", "4-BothUniform"})
        Me.cboAllowUserResizing.Location = New System.Drawing.Point(142, 94)
        Me.cboAllowUserResizing.Name = "cboAllowUserResizing"
        Me.cboAllowUserResizing.Size = New System.Drawing.Size(168, 21)
        Me.cboAllowUserResizing.TabIndex = 25
        '
        'cboAllowUserReorder
        '
        Me.cboAllowUserReorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowUserReorder.FormattingEnabled = True
        Me.cboAllowUserReorder.Items.AddRange(New Object() {"0-None", "1-Rows", "2-Columns", "3-Both"})
        Me.cboAllowUserReorder.Location = New System.Drawing.Point(142, 67)
        Me.cboAllowUserReorder.Name = "cboAllowUserReorder"
        Me.cboAllowUserReorder.Size = New System.Drawing.Size(168, 21)
        Me.cboAllowUserReorder.TabIndex = 24
        '
        'cboAllowSelection
        '
        Me.cboAllowSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowSelection.FormattingEnabled = True
        Me.cboAllowSelection.Items.AddRange(New Object() {"True", "False"})
        Me.cboAllowSelection.Location = New System.Drawing.Point(142, 40)
        Me.cboAllowSelection.Name = "cboAllowSelection"
        Me.cboAllowSelection.Size = New System.Drawing.Size(168, 21)
        Me.cboAllowSelection.TabIndex = 23
        '
        'cboAllowBigSelection
        '
        Me.cboAllowBigSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowBigSelection.FormattingEnabled = True
        Me.cboAllowBigSelection.Items.AddRange(New Object() {"True", "False"})
        Me.cboAllowBigSelection.Location = New System.Drawing.Point(142, 13)
        Me.cboAllowBigSelection.Name = "cboAllowBigSelection"
        Me.cboAllowBigSelection.Size = New System.Drawing.Size(168, 21)
        Me.cboAllowBigSelection.TabIndex = 22
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(11, 586)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(119, 13)
        Me.Label22.TabIndex = 21
        Me.Label22.Text = "ApplySelectionToImage"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(11, 559)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(97, 13)
        Me.Label21.TabIndex = 20
        Me.Label21.Text = "ShowContextMenu"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(11, 532)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(98, 13)
        Me.Label20.TabIndex = 19
        Me.Label20.Text = "ShowComboButton"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(11, 505)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(63, 13)
        Me.Label19.TabIndex = 18
        Me.Label19.Text = "PictureOver"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(11, 478)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(72, 13)
        Me.Label18.TabIndex = 17
        Me.Label18.Text = "ScrollBarStyle"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(11, 451)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(54, 13)
        Me.Label17.TabIndex = 16
        Me.Label17.Text = "ScrollBars"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(11, 424)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(59, 13)
        Me.Label16.TabIndex = 15
        Me.Label16.Text = "FocusRect"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(11, 397)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(52, 13)
        Me.Label15.TabIndex = 14
        Me.Label15.Text = "HighLight"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(11, 369)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(78, 13)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "SelectionMode"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(11, 342)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(75, 13)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "ExtendLastCol"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 315)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(92, 13)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "EnterKeyBehavior"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 288)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(38, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Ellipsis"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(11, 261)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 13)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "Editable"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 234)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "ButtonLocked"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 207)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "DateFormat"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 180)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "GridLines"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 153)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "BorderStyle"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 126)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "AllowUserSort"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "AllowUserResizing"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "AllowUserReorder"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "AllowSelection"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "AllowBigSelection"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.cboSheetBorder)
        Me.TabPage2.Controls.Add(Me.picSheetBorderColor)
        Me.TabPage2.Controls.Add(Me.picGridColor)
        Me.TabPage2.Controls.Add(Me.picForeColorSel)
        Me.TabPage2.Controls.Add(Me.picForeColorFrozen)
        Me.TabPage2.Controls.Add(Me.picForeColorFixed)
        Me.TabPage2.Controls.Add(Me.picForeColor)
        Me.TabPage2.Controls.Add(Me.picBackColorSel)
        Me.TabPage2.Controls.Add(Me.picBackColorFrozen)
        Me.TabPage2.Controls.Add(Me.picBackColorBkg)
        Me.TabPage2.Controls.Add(Me.picBackColorAlternate)
        Me.TabPage2.Controls.Add(Me.picBackColor)
        Me.TabPage2.Controls.Add(Me.btnFont)
        Me.TabPage2.Controls.Add(Me.txtDefaultRowHeight)
        Me.TabPage2.Controls.Add(Me.txtDefaultColWidth)
        Me.TabPage2.Controls.Add(Me.txtFrozenCols)
        Me.TabPage2.Controls.Add(Me.txtFrozenRows)
        Me.TabPage2.Controls.Add(Me.txtFixedCols)
        Me.TabPage2.Controls.Add(Me.txtFixedRows)
        Me.TabPage2.Controls.Add(Me.txtColCount)
        Me.TabPage2.Controls.Add(Me.cboUseBackColorAlternate)
        Me.TabPage2.Controls.Add(Me.txtRowCount)
        Me.TabPage2.Controls.Add(Me.Label23)
        Me.TabPage2.Controls.Add(Me.Label24)
        Me.TabPage2.Controls.Add(Me.Label25)
        Me.TabPage2.Controls.Add(Me.Label26)
        Me.TabPage2.Controls.Add(Me.Label27)
        Me.TabPage2.Controls.Add(Me.Label28)
        Me.TabPage2.Controls.Add(Me.Label29)
        Me.TabPage2.Controls.Add(Me.Label30)
        Me.TabPage2.Controls.Add(Me.Label31)
        Me.TabPage2.Controls.Add(Me.Label32)
        Me.TabPage2.Controls.Add(Me.Label33)
        Me.TabPage2.Controls.Add(Me.Label34)
        Me.TabPage2.Controls.Add(Me.Label35)
        Me.TabPage2.Controls.Add(Me.Label36)
        Me.TabPage2.Controls.Add(Me.Label37)
        Me.TabPage2.Controls.Add(Me.Label38)
        Me.TabPage2.Controls.Add(Me.Label39)
        Me.TabPage2.Controls.Add(Me.Label40)
        Me.TabPage2.Controls.Add(Me.Label41)
        Me.TabPage2.Controls.Add(Me.Label42)
        Me.TabPage2.Controls.Add(Me.Label43)
        Me.TabPage2.Controls.Add(Me.Label44)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(321, 621)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Page2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'cboSheetBorder
        '
        Me.cboSheetBorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSheetBorder.FormattingEnabled = True
        Me.cboSheetBorder.Items.AddRange(New Object() {"True", "False"})
        Me.cboSheetBorder.Location = New System.Drawing.Point(142, 555)
        Me.cboSheetBorder.Name = "cboSheetBorder"
        Me.cboSheetBorder.Size = New System.Drawing.Size(168, 21)
        Me.cboSheetBorder.TabIndex = 65
        '
        'picSheetBorderColor
        '
        Me.picSheetBorderColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picSheetBorderColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picSheetBorderColor.Location = New System.Drawing.Point(142, 582)
        Me.picSheetBorderColor.Name = "picSheetBorderColor"
        Me.picSheetBorderColor.Size = New System.Drawing.Size(34, 23)
        Me.picSheetBorderColor.TabIndex = 64
        Me.picSheetBorderColor.TabStop = False
        '
        'picGridColor
        '
        Me.picGridColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picGridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picGridColor.Location = New System.Drawing.Point(142, 528)
        Me.picGridColor.Name = "picGridColor"
        Me.picGridColor.Size = New System.Drawing.Size(34, 23)
        Me.picGridColor.TabIndex = 63
        Me.picGridColor.TabStop = False
        '
        'picForeColorSel
        '
        Me.picForeColorSel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picForeColorSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picForeColorSel.Location = New System.Drawing.Point(142, 501)
        Me.picForeColorSel.Name = "picForeColorSel"
        Me.picForeColorSel.Size = New System.Drawing.Size(34, 23)
        Me.picForeColorSel.TabIndex = 62
        Me.picForeColorSel.TabStop = False
        '
        'picForeColorFrozen
        '
        Me.picForeColorFrozen.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picForeColorFrozen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picForeColorFrozen.Location = New System.Drawing.Point(142, 473)
        Me.picForeColorFrozen.Name = "picForeColorFrozen"
        Me.picForeColorFrozen.Size = New System.Drawing.Size(34, 23)
        Me.picForeColorFrozen.TabIndex = 61
        Me.picForeColorFrozen.TabStop = False
        '
        'picForeColorFixed
        '
        Me.picForeColorFixed.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picForeColorFixed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picForeColorFixed.Location = New System.Drawing.Point(142, 446)
        Me.picForeColorFixed.Name = "picForeColorFixed"
        Me.picForeColorFixed.Size = New System.Drawing.Size(34, 23)
        Me.picForeColorFixed.TabIndex = 60
        Me.picForeColorFixed.TabStop = False
        '
        'picForeColor
        '
        Me.picForeColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picForeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picForeColor.Location = New System.Drawing.Point(142, 419)
        Me.picForeColor.Name = "picForeColor"
        Me.picForeColor.Size = New System.Drawing.Size(34, 23)
        Me.picForeColor.TabIndex = 59
        Me.picForeColor.TabStop = False
        '
        'picBackColorSel
        '
        Me.picBackColorSel.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picBackColorSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBackColorSel.Location = New System.Drawing.Point(142, 365)
        Me.picBackColorSel.Name = "picBackColorSel"
        Me.picBackColorSel.Size = New System.Drawing.Size(34, 23)
        Me.picBackColorSel.TabIndex = 58
        Me.picBackColorSel.TabStop = False
        '
        'picBackColorFrozen
        '
        Me.picBackColorFrozen.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picBackColorFrozen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBackColorFrozen.Location = New System.Drawing.Point(142, 338)
        Me.picBackColorFrozen.Name = "picBackColorFrozen"
        Me.picBackColorFrozen.Size = New System.Drawing.Size(34, 23)
        Me.picBackColorFrozen.TabIndex = 57
        Me.picBackColorFrozen.TabStop = False
        '
        'picBackColorBkg
        '
        Me.picBackColorBkg.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picBackColorBkg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBackColorBkg.Location = New System.Drawing.Point(142, 311)
        Me.picBackColorBkg.Name = "picBackColorBkg"
        Me.picBackColorBkg.Size = New System.Drawing.Size(34, 23)
        Me.picBackColorBkg.TabIndex = 56
        Me.picBackColorBkg.TabStop = False
        '
        'picBackColorAlternate
        '
        Me.picBackColorAlternate.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picBackColorAlternate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBackColorAlternate.Location = New System.Drawing.Point(142, 284)
        Me.picBackColorAlternate.Name = "picBackColorAlternate"
        Me.picBackColorAlternate.Size = New System.Drawing.Size(34, 23)
        Me.picBackColorAlternate.TabIndex = 55
        Me.picBackColorAlternate.TabStop = False
        '
        'picBackColor
        '
        Me.picBackColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picBackColor.Location = New System.Drawing.Point(142, 257)
        Me.picBackColor.Name = "picBackColor"
        Me.picBackColor.Size = New System.Drawing.Size(34, 23)
        Me.picBackColor.TabIndex = 54
        Me.picBackColor.TabStop = False
        '
        'btnFont
        '
        Me.btnFont.Location = New System.Drawing.Point(142, 230)
        Me.btnFont.Name = "btnFont"
        Me.btnFont.Size = New System.Drawing.Size(34, 23)
        Me.btnFont.TabIndex = 53
        Me.btnFont.Text = "..."
        Me.btnFont.UseVisualStyleBackColor = True
        '
        'txtDefaultRowHeight
        '
        Me.txtDefaultRowHeight.Location = New System.Drawing.Point(142, 203)
        Me.txtDefaultRowHeight.Name = "txtDefaultRowHeight"
        Me.txtDefaultRowHeight.Size = New System.Drawing.Size(168, 20)
        Me.txtDefaultRowHeight.TabIndex = 52
        '
        'txtDefaultColWidth
        '
        Me.txtDefaultColWidth.Location = New System.Drawing.Point(142, 176)
        Me.txtDefaultColWidth.Name = "txtDefaultColWidth"
        Me.txtDefaultColWidth.Size = New System.Drawing.Size(168, 20)
        Me.txtDefaultColWidth.TabIndex = 51
        '
        'txtFrozenCols
        '
        Me.txtFrozenCols.Location = New System.Drawing.Point(142, 148)
        Me.txtFrozenCols.Name = "txtFrozenCols"
        Me.txtFrozenCols.Size = New System.Drawing.Size(168, 20)
        Me.txtFrozenCols.TabIndex = 50
        '
        'txtFrozenRows
        '
        Me.txtFrozenRows.Location = New System.Drawing.Point(142, 121)
        Me.txtFrozenRows.Name = "txtFrozenRows"
        Me.txtFrozenRows.Size = New System.Drawing.Size(168, 20)
        Me.txtFrozenRows.TabIndex = 49
        '
        'txtFixedCols
        '
        Me.txtFixedCols.Location = New System.Drawing.Point(142, 94)
        Me.txtFixedCols.Name = "txtFixedCols"
        Me.txtFixedCols.Size = New System.Drawing.Size(168, 20)
        Me.txtFixedCols.TabIndex = 48
        '
        'txtFixedRows
        '
        Me.txtFixedRows.Location = New System.Drawing.Point(142, 67)
        Me.txtFixedRows.Name = "txtFixedRows"
        Me.txtFixedRows.Size = New System.Drawing.Size(168, 20)
        Me.txtFixedRows.TabIndex = 47
        '
        'txtColCount
        '
        Me.txtColCount.Location = New System.Drawing.Point(142, 40)
        Me.txtColCount.Name = "txtColCount"
        Me.txtColCount.Size = New System.Drawing.Size(168, 20)
        Me.txtColCount.TabIndex = 46
        '
        'cboUseBackColorAlternate
        '
        Me.cboUseBackColorAlternate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUseBackColorAlternate.FormattingEnabled = True
        Me.cboUseBackColorAlternate.Items.AddRange(New Object() {"True", "False"})
        Me.cboUseBackColorAlternate.Location = New System.Drawing.Point(142, 393)
        Me.cboUseBackColorAlternate.Name = "cboUseBackColorAlternate"
        Me.cboUseBackColorAlternate.Size = New System.Drawing.Size(168, 21)
        Me.cboUseBackColorAlternate.TabIndex = 45
        '
        'txtRowCount
        '
        Me.txtRowCount.Location = New System.Drawing.Point(142, 13)
        Me.txtRowCount.Name = "txtRowCount"
        Me.txtRowCount.Size = New System.Drawing.Size(168, 20)
        Me.txtRowCount.TabIndex = 44
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(11, 586)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(90, 13)
        Me.Label23.TabIndex = 43
        Me.Label23.Text = "SheetBorderColor"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(11, 559)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(66, 13)
        Me.Label24.TabIndex = 42
        Me.Label24.Text = "SheetBorder"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(11, 532)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(50, 13)
        Me.Label25.TabIndex = 41
        Me.Label25.Text = "GridColor"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(11, 505)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(67, 13)
        Me.Label26.TabIndex = 40
        Me.Label26.Text = "ForeColorSel"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(11, 478)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(84, 13)
        Me.Label27.TabIndex = 39
        Me.Label27.Text = "ForeColorFrozen"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(11, 451)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(77, 13)
        Me.Label28.TabIndex = 38
        Me.Label28.Text = "ForeColorFixed"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(11, 424)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(52, 13)
        Me.Label29.TabIndex = 37
        Me.Label29.Text = "ForeColor"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(11, 397)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(117, 13)
        Me.Label30.TabIndex = 36
        Me.Label30.Text = "UseBackColorAlternate"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(11, 369)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(71, 13)
        Me.Label31.TabIndex = 35
        Me.Label31.Text = "BackColorSel"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(11, 342)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(88, 13)
        Me.Label32.TabIndex = 34
        Me.Label32.Text = "BackColorFrozen"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(11, 315)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(75, 13)
        Me.Label33.TabIndex = 33
        Me.Label33.Text = "BackColorBkg"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(11, 288)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(98, 13)
        Me.Label34.TabIndex = 32
        Me.Label34.Text = "BackColorAlternate"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(11, 261)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(56, 13)
        Me.Label35.TabIndex = 31
        Me.Label35.Text = "BackColor"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(11, 234)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(28, 13)
        Me.Label36.TabIndex = 30
        Me.Label36.Text = "Font"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(11, 207)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(94, 13)
        Me.Label37.TabIndex = 29
        Me.Label37.Text = "DefaultRowHeight"
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(11, 180)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(84, 13)
        Me.Label38.TabIndex = 28
        Me.Label38.Text = "DefaultColWidth"
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(11, 153)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(59, 13)
        Me.Label39.TabIndex = 27
        Me.Label39.Text = "FrozenCols"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(11, 126)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(66, 13)
        Me.Label40.TabIndex = 26
        Me.Label40.Text = "FrozenRows"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(11, 99)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(52, 13)
        Me.Label41.TabIndex = 25
        Me.Label41.Text = "FixedCols"
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(11, 72)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(59, 13)
        Me.Label42.TabIndex = 24
        Me.Label42.Text = "FixedRows"
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(11, 44)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(50, 13)
        Me.Label43.TabIndex = 23
        Me.Label43.Text = "ColCount"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(11, 17)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(57, 13)
        Me.Label44.TabIndex = 22
        Me.Label44.Text = "RowCount"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.cboDragDropMode)
        Me.TabPage3.Controls.Add(Me.Label54)
        Me.TabPage3.Controls.Add(Me.cboAllowDragDrop)
        Me.TabPage3.Controls.Add(Me.Label55)
        Me.TabPage3.Controls.Add(Me.cboTabKeyBehavior)
        Me.TabPage3.Controls.Add(Me.Label53)
        Me.TabPage3.Controls.Add(Me.cboAutoClipboard)
        Me.TabPage3.Controls.Add(Me.Label52)
        Me.TabPage3.Controls.Add(Me.cboHighlightHeaders)
        Me.TabPage3.Controls.Add(Me.Label51)
        Me.TabPage3.Controls.Add(Me.cboShowHeaderAutoText)
        Me.TabPage3.Controls.Add(Me.Label50)
        Me.TabPage3.Controls.Add(Me.cboShowRowHeaderImage)
        Me.TabPage3.Controls.Add(Me.Label49)
        Me.TabPage3.Controls.Add(Me.picThemeCustomColorTo)
        Me.TabPage3.Controls.Add(Me.picThemeCustomColorFrom)
        Me.TabPage3.Controls.Add(Me.cboThemeColor)
        Me.TabPage3.Controls.Add(Me.cboThemeStyle)
        Me.TabPage3.Controls.Add(Me.Label45)
        Me.TabPage3.Controls.Add(Me.Label46)
        Me.TabPage3.Controls.Add(Me.Label47)
        Me.TabPage3.Controls.Add(Me.Label48)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(321, 621)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Page3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'cboTabKeyBehavior
        '
        Me.cboTabKeyBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTabKeyBehavior.FormattingEnabled = True
        Me.cboTabKeyBehavior.Items.AddRange(New Object() {"0-NextCol", "1-NextRow", "2-StandardTab"})
        Me.cboTabKeyBehavior.Location = New System.Drawing.Point(142, 238)
        Me.cboTabKeyBehavior.Name = "cboTabKeyBehavior"
        Me.cboTabKeyBehavior.Size = New System.Drawing.Size(168, 21)
        Me.cboTabKeyBehavior.TabIndex = 68
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Location = New System.Drawing.Point(11, 243)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(86, 13)
        Me.Label53.TabIndex = 67
        Me.Label53.Text = "TabKeyBehavior"
        '
        'cboAutoClipboard
        '
        Me.cboAutoClipboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAutoClipboard.FormattingEnabled = True
        Me.cboAutoClipboard.Items.AddRange(New Object() {"True", "False"})
        Me.cboAutoClipboard.Location = New System.Drawing.Point(142, 208)
        Me.cboAutoClipboard.Name = "cboAutoClipboard"
        Me.cboAutoClipboard.Size = New System.Drawing.Size(168, 21)
        Me.cboAutoClipboard.TabIndex = 65
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Location = New System.Drawing.Point(11, 212)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(73, 13)
        Me.Label52.TabIndex = 64
        Me.Label52.Text = "AutoClipboard"
        '
        'cboHighlightHeaders
        '
        Me.cboHighlightHeaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHighlightHeaders.FormattingEnabled = True
        Me.cboHighlightHeaders.Items.AddRange(New Object() {"True", "False"})
        Me.cboHighlightHeaders.Location = New System.Drawing.Point(142, 179)
        Me.cboHighlightHeaders.Name = "cboHighlightHeaders"
        Me.cboHighlightHeaders.Size = New System.Drawing.Size(168, 21)
        Me.cboHighlightHeaders.TabIndex = 63
        '
        'Label51
        '
        Me.Label51.AutoSize = True
        Me.Label51.Location = New System.Drawing.Point(11, 182)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(88, 13)
        Me.Label51.TabIndex = 62
        Me.Label51.Text = "HighlightHeaders"
        '
        'cboShowHeaderAutoText
        '
        Me.cboShowHeaderAutoText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShowHeaderAutoText.FormattingEnabled = True
        Me.cboShowHeaderAutoText.Items.AddRange(New Object() {"0-None", "1-RowHeader", "2-ColHeader", "3-Both"})
        Me.cboShowHeaderAutoText.Location = New System.Drawing.Point(142, 150)
        Me.cboShowHeaderAutoText.Name = "cboShowHeaderAutoText"
        Me.cboShowHeaderAutoText.Size = New System.Drawing.Size(168, 21)
        Me.cboShowHeaderAutoText.TabIndex = 61
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(11, 154)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(112, 13)
        Me.Label50.TabIndex = 60
        Me.Label50.Text = "ShowHeaderAutoText"
        '
        'cboShowRowHeaderImage
        '
        Me.cboShowRowHeaderImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboShowRowHeaderImage.FormattingEnabled = True
        Me.cboShowRowHeaderImage.Items.AddRange(New Object() {"True", "False"})
        Me.cboShowRowHeaderImage.Location = New System.Drawing.Point(142, 121)
        Me.cboShowRowHeaderImage.Name = "cboShowRowHeaderImage"
        Me.cboShowRowHeaderImage.Size = New System.Drawing.Size(168, 21)
        Me.cboShowRowHeaderImage.TabIndex = 59
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(11, 126)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(120, 13)
        Me.Label49.TabIndex = 58
        Me.Label49.Text = "ShowRowHeaderImage"
        '
        'picThemeCustomColorTo
        '
        Me.picThemeCustomColorTo.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picThemeCustomColorTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picThemeCustomColorTo.Location = New System.Drawing.Point(142, 94)
        Me.picThemeCustomColorTo.Name = "picThemeCustomColorTo"
        Me.picThemeCustomColorTo.Size = New System.Drawing.Size(34, 23)
        Me.picThemeCustomColorTo.TabIndex = 57
        Me.picThemeCustomColorTo.TabStop = False
        '
        'picThemeCustomColorFrom
        '
        Me.picThemeCustomColorFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picThemeCustomColorFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picThemeCustomColorFrom.Location = New System.Drawing.Point(142, 67)
        Me.picThemeCustomColorFrom.Name = "picThemeCustomColorFrom"
        Me.picThemeCustomColorFrom.Size = New System.Drawing.Size(34, 23)
        Me.picThemeCustomColorFrom.TabIndex = 56
        Me.picThemeCustomColorFrom.TabStop = False
        '
        'cboThemeColor
        '
        Me.cboThemeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboThemeColor.FormattingEnabled = True
        Me.cboThemeColor.Items.AddRange(New Object() {"0-Blue", "1-Silver", "2-Olive", "3-Visual2005", "4-CustomColor"})
        Me.cboThemeColor.Location = New System.Drawing.Point(142, 40)
        Me.cboThemeColor.Name = "cboThemeColor"
        Me.cboThemeColor.Size = New System.Drawing.Size(168, 21)
        Me.cboThemeColor.TabIndex = 31
        '
        'cboThemeStyle
        '
        Me.cboThemeStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboThemeStyle.FormattingEnabled = True
        Me.cboThemeStyle.Items.AddRange(New Object() {"0-Flat", "1-Light3D", "2-Windows", "3-Office"})
        Me.cboThemeStyle.Location = New System.Drawing.Point(142, 13)
        Me.cboThemeStyle.Name = "cboThemeStyle"
        Me.cboThemeStyle.Size = New System.Drawing.Size(168, 21)
        Me.cboThemeStyle.TabIndex = 30
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(11, 99)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(112, 13)
        Me.Label45.TabIndex = 29
        Me.Label45.Text = "ThemeCustomColorTo"
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(11, 72)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(122, 13)
        Me.Label46.TabIndex = 28
        Me.Label46.Text = "ThemeCustomColorFrom"
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(11, 44)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(64, 13)
        Me.Label47.TabIndex = 27
        Me.Label47.Text = "ThemeColor"
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(11, 17)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(63, 13)
        Me.Label48.TabIndex = 26
        Me.Label48.Text = "ThemeStyle"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "s1.ico")
        Me.ImageList1.Images.SetKeyName(1, "s2.ico")
        Me.ImageList1.Images.SetKeyName(2, "s3.ico")
        Me.ImageList1.Images.SetKeyName(3, "s4.ico")
        Me.ImageList1.Images.SetKeyName(4, "s5.ico")
        Me.ImageList1.Images.SetKeyName(5, "s6.ico")
        Me.ImageList1.Images.SetKeyName(6, "s7.ico")
        Me.ImageList1.Images.SetKeyName(7, "s8.ico")
        '
        'GrdView1
        '
        Me.GrdView1.AutoRedraw = True
        Me.GrdView1.CurrentColIndex = 1
        Me.GrdView1.CurrentRowIndex = 1
        Me.GrdView1.EditSelectedText = Nothing
        Me.GrdView1.EditSelectionLength = 0
        Me.GrdView1.EditSelectionStart = 0
        Me.GrdView1.EditText = Nothing
        Me.GrdView1.FormatProvider = New System.Globalization.CultureInfo("zh-CN")
        Me.GrdView1.LanguageConfig = Nothing
        Me.GrdView1.LeftCol = 1
        Me.GrdView1.LicenseKey = Nothing
        Me.GrdView1.Location = New System.Drawing.Point(12, 99)
        Me.GrdView1.Name = "GrdView1"
        Me.GrdView1.Size = New System.Drawing.Size(600, 626)
        Me.GrdView1.TabIndex = 7
        Me.GrdView1.ThemeCustomColorFrom = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.GrdView1.ThemeCustomColorTo = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(221, Byte), Integer))
        Me.GrdView1.TopRow = 1
        '
        'cboDragDropMode
        '
        Me.cboDragDropMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDragDropMode.FormattingEnabled = True
        Me.cboDragDropMode.Items.AddRange(New Object() {"0-Content", "1-FormatAndContent"})
        Me.cboDragDropMode.Location = New System.Drawing.Point(142, 298)
        Me.cboDragDropMode.Name = "cboDragDropMode"
        Me.cboDragDropMode.Size = New System.Drawing.Size(168, 21)
        Me.cboDragDropMode.TabIndex = 72
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(11, 303)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(80, 13)
        Me.Label54.TabIndex = 71
        Me.Label54.Text = "DragDropMode"
        '
        'cboAllowDragDrop
        '
        Me.cboAllowDragDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAllowDragDrop.FormattingEnabled = True
        Me.cboAllowDragDrop.Items.AddRange(New Object() {"True", "False"})
        Me.cboAllowDragDrop.Location = New System.Drawing.Point(142, 268)
        Me.cboAllowDragDrop.Name = "cboAllowDragDrop"
        Me.cboAllowDragDrop.Size = New System.Drawing.Size(168, 21)
        Me.cboAllowDragDrop.TabIndex = 70
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Location = New System.Drawing.Point(11, 272)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(78, 13)
        Me.Label55.TabIndex = 69
        Me.Label55.Text = "AllowDragDrop"
        '
        'frmDemo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(976, 781)
        Me.Controls.Add(Me.GrdView1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.tabProperty)
        Me.Controls.Add(Me.ToolStrip2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmDemo"
        Me.Text = "MstGrid Designer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.tabProperty.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.picSheetBorderColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picGridColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picForeColorSel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picForeColorFrozen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picForeColorFixed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picForeColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBackColorSel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBackColorFrozen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBackColorBkg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBackColorAlternate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBackColor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.picThemeCustomColorTo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picThemeCustomColorFrom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents mi_file As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_new As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_open As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_save As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_saveas As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_pagesetup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_printpreview As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_file_print As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_file_exit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_edit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_edit_cut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_edit_copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_edit_paste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_edit_hiderow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_edit_hidecolumn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_edit_unhiderow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_edit_unhidecolumn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_format As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_help As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_help_manual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_help_about As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents bt_new As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_open As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_save As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_print As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_printpreview As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_cut As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_copy As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_paste As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_mergecell As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_unmergecell As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_insertrow As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_insertcolumn As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_deleterow As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_deletecolumn As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_image As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_help As System.Windows.Forms.ToolStripButton
    Friend WithEvents cboFontName As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents cboFontSize As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents bt_fontbold As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_fontitalic As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_fontunderline As System.Windows.Forms.ToolStripButton
    Friend WithEvents bt_fontstrikeout As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents bt_imagealign As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents bt_imagealign_lefttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_leftcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_leftbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_imagealign_centertop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_centercenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_centerbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_righttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_rightcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_imagealign_rightbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents bt_textalign_lefttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_leftcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_leftbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_centertop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_centercenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_centerbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_righttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_rightcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_textalign_rightbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tabProperty As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents cboApplySelectionToImage As System.Windows.Forms.ComboBox
    Friend WithEvents cboShowContextMenu As System.Windows.Forms.ComboBox
    Friend WithEvents cboShowComboButton As System.Windows.Forms.ComboBox
    Friend WithEvents cboPictureOver As System.Windows.Forms.ComboBox
    Friend WithEvents cboScrollBarStyle As System.Windows.Forms.ComboBox
    Friend WithEvents cboScrollBars As System.Windows.Forms.ComboBox
    Friend WithEvents cboFocusRect As System.Windows.Forms.ComboBox
    Friend WithEvents cboHighLight As System.Windows.Forms.ComboBox
    Friend WithEvents cboSelectionMode As System.Windows.Forms.ComboBox
    Friend WithEvents cboExtendLastCol As System.Windows.Forms.ComboBox
    Friend WithEvents cboEnterKeyBehavior As System.Windows.Forms.ComboBox
    Friend WithEvents cboEllipsis As System.Windows.Forms.ComboBox
    Friend WithEvents cboEditable As System.Windows.Forms.ComboBox
    Friend WithEvents cboButtonLocked As System.Windows.Forms.ComboBox
    Friend WithEvents cboDateFormat As System.Windows.Forms.ComboBox
    Friend WithEvents cboGridLines As System.Windows.Forms.ComboBox
    Friend WithEvents cboBorderStyle As System.Windows.Forms.ComboBox
    Friend WithEvents cboAllowUserSort As System.Windows.Forms.ComboBox
    Friend WithEvents cboAllowUserResizing As System.Windows.Forms.ComboBox
    Friend WithEvents cboAllowUserReorder As System.Windows.Forms.ComboBox
    Friend WithEvents cboAllowSelection As System.Windows.Forms.ComboBox
    Friend WithEvents cboAllowBigSelection As System.Windows.Forms.ComboBox
    Friend WithEvents txtDefaultRowHeight As System.Windows.Forms.TextBox
    Friend WithEvents txtDefaultColWidth As System.Windows.Forms.TextBox
    Friend WithEvents txtFrozenCols As System.Windows.Forms.TextBox
    Friend WithEvents txtFrozenRows As System.Windows.Forms.TextBox
    Friend WithEvents txtFixedCols As System.Windows.Forms.TextBox
    Friend WithEvents txtFixedRows As System.Windows.Forms.TextBox
    Friend WithEvents txtColCount As System.Windows.Forms.TextBox
    Friend WithEvents cboUseBackColorAlternate As System.Windows.Forms.ComboBox
    Friend WithEvents txtRowCount As System.Windows.Forms.TextBox
    Friend WithEvents btnFont As System.Windows.Forms.Button
    Friend WithEvents picBackColor As System.Windows.Forms.PictureBox
    Friend WithEvents cboSheetBorder As System.Windows.Forms.ComboBox
    Friend WithEvents picSheetBorderColor As System.Windows.Forms.PictureBox
    Friend WithEvents picGridColor As System.Windows.Forms.PictureBox
    Friend WithEvents picForeColorSel As System.Windows.Forms.PictureBox
    Friend WithEvents picForeColorFrozen As System.Windows.Forms.PictureBox
    Friend WithEvents picForeColorFixed As System.Windows.Forms.PictureBox
    Friend WithEvents picForeColor As System.Windows.Forms.PictureBox
    Friend WithEvents picBackColorSel As System.Windows.Forms.PictureBox
    Friend WithEvents picBackColorFrozen As System.Windows.Forms.PictureBox
    Friend WithEvents picBackColorBkg As System.Windows.Forms.PictureBox
    Friend WithEvents picBackColorAlternate As System.Windows.Forms.PictureBox
    Friend WithEvents cboThemeColor As System.Windows.Forms.ComboBox
    Friend WithEvents cboThemeStyle As System.Windows.Forms.ComboBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents picThemeCustomColorTo As System.Windows.Forms.PictureBox
    Friend WithEvents picThemeCustomColorFrom As System.Windows.Forms.PictureBox
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents bt_imagealign_stretch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_cellcolor As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents bt_textcolor As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents ToolStripLabel5 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel6 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel7 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents bt_cellborder As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents bt_cellborder_around As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_none As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_left As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_top As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_right As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_bottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_diagonalup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_diagonaldown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_inside As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_insidevertical As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_insidehorizontal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator22 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_cellborder_thin As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_thick As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents bt_cellborder_dot As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator23 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bt_cellborder_color As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboShowRowHeaderImage As System.Windows.Forms.ComboBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents mi_help_updates As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator24 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_column_celltype As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_celltype_textbox As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_celltype_combobox As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_celltype_checkbox As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_celltype_calendar As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_celltype_button As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_celltype_hypelink As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_any As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_numeric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_positivenumeric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_integers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_positiveintegers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_letter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_letternumeric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_upper As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_uppernumeric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_lower As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_lowernumeric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_editmask_chqno As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_lefttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_leftcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_leftbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_centertop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_centercenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_centerbotom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_righttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_rightcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_textalign_rightbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_lefttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_leftcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_leftbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_centertop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_centercenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_centerbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_righttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_rightcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_titlealign_rightbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_lefttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_leftcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_leftbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_centertop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_centercenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_centerbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_righttop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_rightcenter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_rightbottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_picalign_streth As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_sort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_sort_bystring As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_sort_bystringnocase As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_sort_byboolean As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_sort_bydate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_sort_bynumeric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_datestyle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_datestyle_date As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_datestyle_time As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_datestyle_datetime As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator26 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_column_lock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_column_unlock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator27 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_column_title As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_cell As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_cell_setwraptext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_cell_cancelwraptext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator28 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_cell_lock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mi_cell_unlock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator29 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mi_cell_image As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboShowHeaderAutoText As System.Windows.Forms.ComboBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents cboHighlightHeaders As System.Windows.Forms.ComboBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents cboAutoClipboard As System.Windows.Forms.ComboBox
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents cboTabKeyBehavior As System.Windows.Forms.ComboBox
    Friend WithEvents GrdView1 As MstGrid
    Friend WithEvents cboDragDropMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents cboAllowDragDrop As System.Windows.Forms.ComboBox
    Friend WithEvents Label55 As System.Windows.Forms.Label


End Class
