

using BaiqiSoft.GridControl;
public partial class frmDemo : System.Windows.Forms.Form
{
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDemo));
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mi_file = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_file_new = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_file_open = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_file_save = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_file_saveas = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator25 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_file_pagesetup = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_file_printpreview = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_file_print = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_file_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit_cut = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_edit_hiderow = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit_unhiderow = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_edit_hidecolumn = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_edit_unhidecolumn = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_format = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_format_column = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype_textbox = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype_combobox = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype_checkbox = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype_calendar = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype_button = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_celltype_hypelink = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_any = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_numeric = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_positivenumeric = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_integers = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_positiveintegers = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_letter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_letternumeric = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_upper = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_uppernumeric = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_lower = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_lowernumeric = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_editmask_chqno = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_lefttop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_leftcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_leftbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_centertop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_centercenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_centerbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_righttop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_rightcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_textalign_rightbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_lefttop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_leftcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_leftbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_centertop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_centercenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_centerbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_righttop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_rightcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_titlealign_rightbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_lefttop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_leftcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_leftbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_centertop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_centercenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_centerbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_righttop = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_rightcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_rightbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_picalign_streth = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_sort = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_sort_bystring = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_sort_bystringnocase = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_sort_byboolean = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_sort_bydate = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_sort_bynumeric = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_datestyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_datestyle_date = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_datestyle_time = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_datestyle_datetime = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator26 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_columnformat_lock = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_columnformat_unlock = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator27 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_columnformat_title = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_cellformat = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_cellformat_setwraptext = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_cellformat_cancelwrap = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator28 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_cellformat_lock = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_cellformat_unlock = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator29 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_cellformat_iamge = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_help = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_help_manual = new System.Windows.Forms.ToolStripMenuItem();
            this.mi_help_updates = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this.mi_help_about = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bt_new = new System.Windows.Forms.ToolStripButton();
            this.bt_open = new System.Windows.Forms.ToolStripButton();
            this.bt_save = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_print = new System.Windows.Forms.ToolStripButton();
            this.bt_printpreview = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_cut = new System.Windows.Forms.ToolStripButton();
            this.bt_copy = new System.Windows.Forms.ToolStripButton();
            this.bt_paste = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_merge = new System.Windows.Forms.ToolStripButton();
            this.bt_unmerge = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_insertrow = new System.Windows.Forms.ToolStripButton();
            this.bt_insertcolumn = new System.Windows.Forms.ToolStripButton();
            this.bt_deleterow = new System.Windows.Forms.ToolStripButton();
            this.bt_deletecolumn = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_image = new System.Windows.Forms.ToolStripButton();
            this.bt_help = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip2 = new System.Windows.Forms.ToolStrip();
            this.cboFontName = new System.Windows.Forms.ToolStripComboBox();
            this.cboFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.bt_fontbold = new System.Windows.Forms.ToolStripButton();
            this.bt_fontitalic = new System.Windows.Forms.ToolStripButton();
            this.bt_fontunderline = new System.Windows.Forms.ToolStripButton();
            this.bt_FontStrikethrough = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.bt_imagealign = new System.Windows.Forms.ToolStripSplitButton();
            this.bt_imagealign_lefttop = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_leftcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_leftbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_centertop = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_centercenter = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_centerbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_righttop = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_rightcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_rightbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_imagealign_stretch = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.bt_textalign = new System.Windows.Forms.ToolStripSplitButton();
            this.bt_textalign_lefttop = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_leftcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_leftbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_centertop = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_centercenter = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_centerbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_righttop = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_rightcenter = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_textalign_rightbottom = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.bt_cellcolor = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.bt_textcolor = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.bt_cellborder = new System.Windows.Forms.ToolStripSplitButton();
            this.bt_cellborder_none = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_around = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_left = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_top = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_right = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_diagonalup = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_diagonaldown = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_inside = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_insidevertical = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_insidehorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_cellborder_thin = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_thick = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_cellborder_dot = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_cellborder_color = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabProperty = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.cboApplySelectionToImage = new System.Windows.Forms.ComboBox();
            this.cboShowContextMenu = new System.Windows.Forms.ComboBox();
            this.cboShowComboButton = new System.Windows.Forms.ComboBox();
            this.cboPictureOver = new System.Windows.Forms.ComboBox();
            this.cboScrollBarStyle = new System.Windows.Forms.ComboBox();
            this.cboScrollBars = new System.Windows.Forms.ComboBox();
            this.cboFocusRect = new System.Windows.Forms.ComboBox();
            this.cboHighLight = new System.Windows.Forms.ComboBox();
            this.cboSelectionMode = new System.Windows.Forms.ComboBox();
            this.cboExtendLastCol = new System.Windows.Forms.ComboBox();
            this.cboEnterKeyBehavior = new System.Windows.Forms.ComboBox();
            this.cboEllipsis = new System.Windows.Forms.ComboBox();
            this.cboEditable = new System.Windows.Forms.ComboBox();
            this.cboButtonLocked = new System.Windows.Forms.ComboBox();
            this.cboDateFormat = new System.Windows.Forms.ComboBox();
            this.cboGridLines = new System.Windows.Forms.ComboBox();
            this.cboBorderStyle = new System.Windows.Forms.ComboBox();
            this.cboAllowUserSort = new System.Windows.Forms.ComboBox();
            this.cboAllowUserResizing = new System.Windows.Forms.ComboBox();
            this.cboAllowUserReorder = new System.Windows.Forms.ComboBox();
            this.cboAllowSelection = new System.Windows.Forms.ComboBox();
            this.cboAllowBigSelection = new System.Windows.Forms.ComboBox();
            this.Label22 = new System.Windows.Forms.Label();
            this.Label21 = new System.Windows.Forms.Label();
            this.Label20 = new System.Windows.Forms.Label();
            this.Label19 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.cboSheetBorder = new System.Windows.Forms.ComboBox();
            this.picSheetBorderColor = new System.Windows.Forms.PictureBox();
            this.picGridColor = new System.Windows.Forms.PictureBox();
            this.picForeColorSel = new System.Windows.Forms.PictureBox();
            this.picForeColorFrozen = new System.Windows.Forms.PictureBox();
            this.picForeColorFixed = new System.Windows.Forms.PictureBox();
            this.picForeColor = new System.Windows.Forms.PictureBox();
            this.picBackColorSel = new System.Windows.Forms.PictureBox();
            this.picBackColorFrozen = new System.Windows.Forms.PictureBox();
            this.picBackColorBkg = new System.Windows.Forms.PictureBox();
            this.picBackColorAlternate = new System.Windows.Forms.PictureBox();
            this.picBackColor = new System.Windows.Forms.PictureBox();
            this.btnFont = new System.Windows.Forms.Button();
            this.txtDefaultRowHeight = new System.Windows.Forms.TextBox();
            this.txtDefaultColWidth = new System.Windows.Forms.TextBox();
            this.txtFrozenCols = new System.Windows.Forms.TextBox();
            this.txtFrozenRows = new System.Windows.Forms.TextBox();
            this.txtFixedCols = new System.Windows.Forms.TextBox();
            this.txtFixedRows = new System.Windows.Forms.TextBox();
            this.txtColCount = new System.Windows.Forms.TextBox();
            this.cboUseBackColorAlternate = new System.Windows.Forms.ComboBox();
            this.txtRowCount = new System.Windows.Forms.TextBox();
            this.Label23 = new System.Windows.Forms.Label();
            this.Label24 = new System.Windows.Forms.Label();
            this.Label25 = new System.Windows.Forms.Label();
            this.Label26 = new System.Windows.Forms.Label();
            this.Label27 = new System.Windows.Forms.Label();
            this.Label28 = new System.Windows.Forms.Label();
            this.Label29 = new System.Windows.Forms.Label();
            this.Label30 = new System.Windows.Forms.Label();
            this.Label31 = new System.Windows.Forms.Label();
            this.Label32 = new System.Windows.Forms.Label();
            this.Label33 = new System.Windows.Forms.Label();
            this.Label34 = new System.Windows.Forms.Label();
            this.Label35 = new System.Windows.Forms.Label();
            this.Label36 = new System.Windows.Forms.Label();
            this.Label37 = new System.Windows.Forms.Label();
            this.Label38 = new System.Windows.Forms.Label();
            this.Label39 = new System.Windows.Forms.Label();
            this.Label40 = new System.Windows.Forms.Label();
            this.Label41 = new System.Windows.Forms.Label();
            this.Label42 = new System.Windows.Forms.Label();
            this.Label43 = new System.Windows.Forms.Label();
            this.Label44 = new System.Windows.Forms.Label();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.cboTabKeyBehavior = new System.Windows.Forms.ComboBox();
            this.Label53 = new System.Windows.Forms.Label();
            this.cboAutoClipboard = new System.Windows.Forms.ComboBox();
            this.Label52 = new System.Windows.Forms.Label();
            this.cboHighlightHeaders = new System.Windows.Forms.ComboBox();
            this.Label51 = new System.Windows.Forms.Label();
            this.cboShowHeaderAutoText = new System.Windows.Forms.ComboBox();
            this.Label50 = new System.Windows.Forms.Label();
            this.cboShowRowHeaderImage = new System.Windows.Forms.ComboBox();
            this.Label49 = new System.Windows.Forms.Label();
            this.picThemeCustomColorTo = new System.Windows.Forms.PictureBox();
            this.picThemeCustomColorFrom = new System.Windows.Forms.PictureBox();
            this.cboThemeColor = new System.Windows.Forms.ComboBox();
            this.cboThemeStyle = new System.Windows.Forms.ComboBox();
            this.Label45 = new System.Windows.Forms.Label();
            this.Label46 = new System.Windows.Forms.Label();
            this.Label47 = new System.Windows.Forms.Label();
            this.Label48 = new System.Windows.Forms.Label();
            this.ColorDialog1 = new System.Windows.Forms.ColorDialog();
            this.FontDialog1 = new System.Windows.Forms.FontDialog();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.GrdView1 = new BaiqiSoft.GridControl.MstGrid();
            this.cboDragDropMode = new System.Windows.Forms.ComboBox();
            this.Label54 = new System.Windows.Forms.Label();
            this.cboAllowDragDrop = new System.Windows.Forms.ComboBox();
            this.Label55 = new System.Windows.Forms.Label();
            this.MenuStrip1.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.ToolStrip2.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.tabProperty.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSheetBorderColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGridColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColorSel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColorFrozen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColorFixed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorSel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorFrozen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorBkg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorAlternate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColor)).BeginInit();
            this.TabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThemeCustomColorTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThemeCustomColorFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_file,
            this.mi_edit,
            this.mi_format,
            this.mi_help});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(976, 24);
            this.MenuStrip1.TabIndex = 3;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // mi_file
            // 
            this.mi_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_file_new,
            this.mi_file_open,
            this.mi_file_save,
            this.mi_file_saveas,
            this.ToolStripSeparator25,
            this.mi_file_pagesetup,
            this.mi_file_printpreview,
            this.mi_file_print,
            this.ToolStripSeparator2,
            this.mi_file_exit});
            this.mi_file.Name = "mi_file";
            this.mi_file.Size = new System.Drawing.Size(37, 20);
            this.mi_file.Text = "&File";
            // 
            // mi_file_new
            // 
            this.mi_file_new.Name = "mi_file_new";
            this.mi_file_new.Size = new System.Drawing.Size(152, 22);
            this.mi_file_new.Text = "New";
            this.mi_file_new.Click += new System.EventHandler(this.mi_file_new_click);
            // 
            // mi_file_open
            // 
            this.mi_file_open.Name = "mi_file_open";
            this.mi_file_open.Size = new System.Drawing.Size(152, 22);
            this.mi_file_open.Text = "Open...";
            this.mi_file_open.Click += new System.EventHandler(this.mi_file_open_click);
            // 
            // mi_file_save
            // 
            this.mi_file_save.Name = "mi_file_save";
            this.mi_file_save.Size = new System.Drawing.Size(152, 22);
            this.mi_file_save.Text = "Save";
            this.mi_file_save.Click += new System.EventHandler(this.mi_file_save_click);
            // 
            // mi_file_saveas
            // 
            this.mi_file_saveas.Name = "mi_file_saveas";
            this.mi_file_saveas.Size = new System.Drawing.Size(152, 22);
            this.mi_file_saveas.Text = "Save As...";
            this.mi_file_saveas.Click += new System.EventHandler(this.mi_file_saveas_click);
            // 
            // ToolStripSeparator25
            // 
            this.ToolStripSeparator25.Name = "ToolStripSeparator25";
            this.ToolStripSeparator25.Size = new System.Drawing.Size(149, 6);
            // 
            // mi_file_pagesetup
            // 
            this.mi_file_pagesetup.Name = "mi_file_pagesetup";
            this.mi_file_pagesetup.Size = new System.Drawing.Size(152, 22);
            this.mi_file_pagesetup.Text = "Page Setup...";
            this.mi_file_pagesetup.Click += new System.EventHandler(this.mi_file_pagesetup_click);
            // 
            // mi_file_printpreview
            // 
            this.mi_file_printpreview.Name = "mi_file_printpreview";
            this.mi_file_printpreview.Size = new System.Drawing.Size(152, 22);
            this.mi_file_printpreview.Text = "Print Preview...";
            this.mi_file_printpreview.Click += new System.EventHandler(this.mi_file_printpreview_click);
            // 
            // mi_file_print
            // 
            this.mi_file_print.Name = "mi_file_print";
            this.mi_file_print.Size = new System.Drawing.Size(152, 22);
            this.mi_file_print.Text = "Print";
            this.mi_file_print.Click += new System.EventHandler(this.mi_file_print_click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // mi_file_exit
            // 
            this.mi_file_exit.Name = "mi_file_exit";
            this.mi_file_exit.Size = new System.Drawing.Size(152, 22);
            this.mi_file_exit.Text = "Exit";
            this.mi_file_exit.Click += new System.EventHandler(this.mi_file_exit_click);
            // 
            // mi_edit
            // 
            this.mi_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_edit_cut,
            this.mi_edit_copy,
            this.mi_edit_paste,
            this.ToolStripSeparator3,
            this.mi_edit_hiderow,
            this.mi_edit_unhiderow,
            this.ToolStripSeparator8,
            this.mi_edit_hidecolumn,
            this.mi_edit_unhidecolumn});
            this.mi_edit.Name = "mi_edit";
            this.mi_edit.Size = new System.Drawing.Size(39, 20);
            this.mi_edit.Text = "&Edit";
            // 
            // mi_edit_cut
            // 
            this.mi_edit_cut.Name = "mi_edit_cut";
            this.mi_edit_cut.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_cut.Text = "Cut";
            this.mi_edit_cut.Click += new System.EventHandler(this.mi_edit_cut_click);
            // 
            // mi_edit_copy
            // 
            this.mi_edit_copy.Name = "mi_edit_copy";
            this.mi_edit_copy.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_copy.Text = "Copy";
            this.mi_edit_copy.Click += new System.EventHandler(this.mi_edit_copy_click);
            // 
            // mi_edit_paste
            // 
            this.mi_edit_paste.Name = "mi_edit_paste";
            this.mi_edit_paste.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_paste.Text = "Paste";
            this.mi_edit_paste.Click += new System.EventHandler(this.mi_edit_paste_click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(155, 6);
            // 
            // mi_edit_hiderow
            // 
            this.mi_edit_hiderow.Name = "mi_edit_hiderow";
            this.mi_edit_hiderow.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_hiderow.Text = "Hide Row";
            this.mi_edit_hiderow.Click += new System.EventHandler(this.mi_edit_hiderow_click);
            // 
            // mi_edit_unhiderow
            // 
            this.mi_edit_unhiderow.Name = "mi_edit_unhiderow";
            this.mi_edit_unhiderow.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_unhiderow.Text = "Unhide Row";
            this.mi_edit_unhiderow.Click += new System.EventHandler(this.mi_edit_unhiderow_click);
            // 
            // ToolStripSeparator8
            // 
            this.ToolStripSeparator8.Name = "ToolStripSeparator8";
            this.ToolStripSeparator8.Size = new System.Drawing.Size(155, 6);
            // 
            // mi_edit_hidecolumn
            // 
            this.mi_edit_hidecolumn.Name = "mi_edit_hidecolumn";
            this.mi_edit_hidecolumn.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_hidecolumn.Text = "Hide Column";
            this.mi_edit_hidecolumn.Click += new System.EventHandler(this.mi_edit_hidecolumn_click);
            // 
            // mi_edit_unhidecolumn
            // 
            this.mi_edit_unhidecolumn.Name = "mi_edit_unhidecolumn";
            this.mi_edit_unhidecolumn.Size = new System.Drawing.Size(158, 22);
            this.mi_edit_unhidecolumn.Text = "Unhide Column";
            this.mi_edit_unhidecolumn.Click += new System.EventHandler(this.mi_edit_unhidecolumn_click);
            // 
            // mi_format
            // 
            this.mi_format.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_format_column,
            this.ToolStripSeparator10,
            this.mi_cellformat});
            this.mi_format.Name = "mi_format";
            this.mi_format.Size = new System.Drawing.Size(57, 20);
            this.mi_format.Text = "&Format";
            // 
            // mi_format_column
            // 
            this.mi_format_column.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_celltype,
            this.mi_columnformat_editmask,
            this.mi_columnformat_textalign,
            this.mi_columnformat_titlealign,
            this.mi_columnformat_picalign,
            this.mi_columnformat_sort,
            this.mi_columnformat_datestyle,
            this.ToolStripSeparator26,
            this.mi_columnformat_lock,
            this.mi_columnformat_unlock,
            this.ToolStripSeparator27,
            this.mi_columnformat_title});
            this.mi_format_column.Name = "mi_format_column";
            this.mi_format_column.Size = new System.Drawing.Size(117, 22);
            this.mi_format_column.Text = "Column";
            // 
            // mi_columnformat_celltype
            // 
            this.mi_columnformat_celltype.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_celltype_textbox,
            this.mi_columnformat_celltype_combobox,
            this.mi_columnformat_celltype_checkbox,
            this.mi_columnformat_celltype_calendar,
            this.mi_columnformat_celltype_button,
            this.mi_columnformat_celltype_hypelink});
            this.mi_columnformat_celltype.Name = "mi_columnformat_celltype";
            this.mi_columnformat_celltype.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_celltype.Text = "Cell Type";
            // 
            // mi_columnformat_celltype_textbox
            // 
            this.mi_columnformat_celltype_textbox.Name = "mi_columnformat_celltype_textbox";
            this.mi_columnformat_celltype_textbox.Size = new System.Drawing.Size(150, 22);
            this.mi_columnformat_celltype_textbox.Text = "0 - TextBox";
            this.mi_columnformat_celltype_textbox.Click += new System.EventHandler(this.mi_column_celltype_textbox_click);
            // 
            // mi_columnformat_celltype_combobox
            // 
            this.mi_columnformat_celltype_combobox.Name = "mi_columnformat_celltype_combobox";
            this.mi_columnformat_celltype_combobox.Size = new System.Drawing.Size(150, 22);
            this.mi_columnformat_celltype_combobox.Text = "1 - ComboBox";
            this.mi_columnformat_celltype_combobox.Click += new System.EventHandler(this.mi_column_celltype_combobox_click);
            // 
            // mi_columnformat_celltype_checkbox
            // 
            this.mi_columnformat_celltype_checkbox.Name = "mi_columnformat_celltype_checkbox";
            this.mi_columnformat_celltype_checkbox.Size = new System.Drawing.Size(150, 22);
            this.mi_columnformat_celltype_checkbox.Text = "2 - CheckBox";
            this.mi_columnformat_celltype_checkbox.Click += new System.EventHandler(this.mi_column_celltype_checkbox_click);
            // 
            // mi_columnformat_celltype_calendar
            // 
            this.mi_columnformat_celltype_calendar.Name = "mi_columnformat_celltype_calendar";
            this.mi_columnformat_celltype_calendar.Size = new System.Drawing.Size(150, 22);
            this.mi_columnformat_celltype_calendar.Text = "3 - Calendar";
            this.mi_columnformat_celltype_calendar.Click += new System.EventHandler(this.mi_column_celltype_calendar_click);
            // 
            // mi_columnformat_celltype_button
            // 
            this.mi_columnformat_celltype_button.Name = "mi_columnformat_celltype_button";
            this.mi_columnformat_celltype_button.Size = new System.Drawing.Size(150, 22);
            this.mi_columnformat_celltype_button.Text = "4 - Button";
            this.mi_columnformat_celltype_button.Click += new System.EventHandler(this.mi_column_celltype_button_click);
            // 
            // mi_columnformat_celltype_hypelink
            // 
            this.mi_columnformat_celltype_hypelink.Name = "mi_columnformat_celltype_hypelink";
            this.mi_columnformat_celltype_hypelink.Size = new System.Drawing.Size(150, 22);
            this.mi_columnformat_celltype_hypelink.Text = "5 - Hypelink";
            this.mi_columnformat_celltype_hypelink.Click += new System.EventHandler(this.mi_column_celltype_hypelink_click);
            // 
            // mi_columnformat_editmask
            // 
            this.mi_columnformat_editmask.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_editmask_any,
            this.mi_columnformat_editmask_numeric,
            this.mi_columnformat_editmask_positivenumeric,
            this.mi_columnformat_editmask_integers,
            this.mi_columnformat_editmask_positiveintegers,
            this.mi_columnformat_editmask_letter,
            this.mi_columnformat_editmask_letternumeric,
            this.mi_columnformat_editmask_upper,
            this.mi_columnformat_editmask_uppernumeric,
            this.mi_columnformat_editmask_lower,
            this.mi_columnformat_editmask_lowernumeric,
            this.mi_columnformat_editmask_chqno});
            this.mi_columnformat_editmask.Name = "mi_columnformat_editmask";
            this.mi_columnformat_editmask.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_editmask.Text = "Edit Mask";
            // 
            // mi_columnformat_editmask_any
            // 
            this.mi_columnformat_editmask_any.Name = "mi_columnformat_editmask_any";
            this.mi_columnformat_editmask_any.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_any.Text = "0 - Any";
            this.mi_columnformat_editmask_any.Click += new System.EventHandler(this.mi_column_editmask_any_click);
            // 
            // mi_columnformat_editmask_numeric
            // 
            this.mi_columnformat_editmask_numeric.Name = "mi_columnformat_editmask_numeric";
            this.mi_columnformat_editmask_numeric.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_numeric.Text = "1 - Numeric";
            this.mi_columnformat_editmask_numeric.Click += new System.EventHandler(this.mi_column_editmask_numeric_click);
            // 
            // mi_columnformat_editmask_positivenumeric
            // 
            this.mi_columnformat_editmask_positivenumeric.Name = "mi_columnformat_editmask_positivenumeric";
            this.mi_columnformat_editmask_positivenumeric.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_positivenumeric.Text = "2 - PositiveNumeric";
            this.mi_columnformat_editmask_positivenumeric.Click += new System.EventHandler(this.mi_column_editmask_positivenumeric_click);
            // 
            // mi_columnformat_editmask_integers
            // 
            this.mi_columnformat_editmask_integers.Name = "mi_columnformat_editmask_integers";
            this.mi_columnformat_editmask_integers.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_integers.Text = "3 - Integers";
            this.mi_columnformat_editmask_integers.Click += new System.EventHandler(this.mi_column_editmask_integers_click);
            // 
            // mi_columnformat_editmask_positiveintegers
            // 
            this.mi_columnformat_editmask_positiveintegers.Name = "mi_columnformat_editmask_positiveintegers";
            this.mi_columnformat_editmask_positiveintegers.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_positiveintegers.Text = "4 - PositiveIntegers";
            this.mi_columnformat_editmask_positiveintegers.Click += new System.EventHandler(this.mi_column_editmask_positiveintegers_click);
            // 
            // mi_columnformat_editmask_letter
            // 
            this.mi_columnformat_editmask_letter.Name = "mi_columnformat_editmask_letter";
            this.mi_columnformat_editmask_letter.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_letter.Text = "5 - Letter";
            this.mi_columnformat_editmask_letter.Click += new System.EventHandler(this.mi_column_editmask_letter_click);
            // 
            // mi_columnformat_editmask_letternumeric
            // 
            this.mi_columnformat_editmask_letternumeric.Name = "mi_columnformat_editmask_letternumeric";
            this.mi_columnformat_editmask_letternumeric.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_letternumeric.Text = "6 - LetterNumeric";
            this.mi_columnformat_editmask_letternumeric.Click += new System.EventHandler(this.mi_column_editmask_letternumeric_click);
            // 
            // mi_columnformat_editmask_upper
            // 
            this.mi_columnformat_editmask_upper.Name = "mi_columnformat_editmask_upper";
            this.mi_columnformat_editmask_upper.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_upper.Text = "7 - Upper";
            this.mi_columnformat_editmask_upper.Click += new System.EventHandler(this.mi_column_editmask_upper_click);
            // 
            // mi_columnformat_editmask_uppernumeric
            // 
            this.mi_columnformat_editmask_uppernumeric.Name = "mi_columnformat_editmask_uppernumeric";
            this.mi_columnformat_editmask_uppernumeric.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_uppernumeric.Text = "8 - UpperNumeric";
            this.mi_columnformat_editmask_uppernumeric.Click += new System.EventHandler(this.mi_column_editmask_uppernumeric_click);
            // 
            // mi_columnformat_editmask_lower
            // 
            this.mi_columnformat_editmask_lower.Name = "mi_columnformat_editmask_lower";
            this.mi_columnformat_editmask_lower.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_lower.Text = "9 - Lower";
            this.mi_columnformat_editmask_lower.Click += new System.EventHandler(this.mi_column_editmask_lower_click);
            // 
            // mi_columnformat_editmask_lowernumeric
            // 
            this.mi_columnformat_editmask_lowernumeric.Name = "mi_columnformat_editmask_lowernumeric";
            this.mi_columnformat_editmask_lowernumeric.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_lowernumeric.Text = "10 - LowerNumeric";
            this.mi_columnformat_editmask_lowernumeric.Click += new System.EventHandler(this.mi_column_editmask_lowernumeric_click);
            // 
            // mi_columnformat_editmask_chqno
            // 
            this.mi_columnformat_editmask_chqno.Name = "mi_columnformat_editmask_chqno";
            this.mi_columnformat_editmask_chqno.Size = new System.Drawing.Size(178, 22);
            this.mi_columnformat_editmask_chqno.Text = "11 - ChqNo";
            this.mi_columnformat_editmask_chqno.Click += new System.EventHandler(this.mi_column_editmask_chqno_click);
            // 
            // mi_columnformat_textalign
            // 
            this.mi_columnformat_textalign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_textalign_lefttop,
            this.mi_columnformat_textalign_leftcenter,
            this.mi_columnformat_textalign_leftbottom,
            this.mi_columnformat_textalign_centertop,
            this.mi_columnformat_textalign_centercenter,
            this.mi_columnformat_textalign_centerbottom,
            this.mi_columnformat_textalign_righttop,
            this.mi_columnformat_textalign_rightcenter,
            this.mi_columnformat_textalign_rightbottom});
            this.mi_columnformat_textalign.Name = "mi_columnformat_textalign";
            this.mi_columnformat_textalign.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_textalign.Text = "Column Alignment";
            // 
            // mi_columnformat_textalign_lefttop
            // 
            this.mi_columnformat_textalign_lefttop.Name = "mi_columnformat_textalign_lefttop";
            this.mi_columnformat_textalign_lefttop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_lefttop.Text = "0 - LeftTop";
            this.mi_columnformat_textalign_lefttop.Click += new System.EventHandler(this.mi_column_textalign_lefttop_click);
            // 
            // mi_columnformat_textalign_leftcenter
            // 
            this.mi_columnformat_textalign_leftcenter.Name = "mi_columnformat_textalign_leftcenter";
            this.mi_columnformat_textalign_leftcenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_leftcenter.Text = "1 - LeftCenter";
            this.mi_columnformat_textalign_leftcenter.Click += new System.EventHandler(this.mi_column_textalign_leftcenter_click);
            // 
            // mi_columnformat_textalign_leftbottom
            // 
            this.mi_columnformat_textalign_leftbottom.Name = "mi_columnformat_textalign_leftbottom";
            this.mi_columnformat_textalign_leftbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_leftbottom.Text = "2 - LeftBottom";
            this.mi_columnformat_textalign_leftbottom.Click += new System.EventHandler(this.mi_column_textalign_leftbottom_click);
            // 
            // mi_columnformat_textalign_centertop
            // 
            this.mi_columnformat_textalign_centertop.Name = "mi_columnformat_textalign_centertop";
            this.mi_columnformat_textalign_centertop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_centertop.Text = "3 - CenterTop";
            this.mi_columnformat_textalign_centertop.Click += new System.EventHandler(this.mi_column_textalign_centertop_click);
            // 
            // mi_columnformat_textalign_centercenter
            // 
            this.mi_columnformat_textalign_centercenter.Name = "mi_columnformat_textalign_centercenter";
            this.mi_columnformat_textalign_centercenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_centercenter.Text = "4 - CenterCenter";
            this.mi_columnformat_textalign_centercenter.Click += new System.EventHandler(this.mi_column_textalign_centercenter_click);
            // 
            // mi_columnformat_textalign_centerbottom
            // 
            this.mi_columnformat_textalign_centerbottom.Name = "mi_columnformat_textalign_centerbottom";
            this.mi_columnformat_textalign_centerbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_centerbottom.Text = "5 - CenterBottom";
            this.mi_columnformat_textalign_centerbottom.Click += new System.EventHandler(this.mi_column_textalign_centerbottom_click);
            // 
            // mi_columnformat_textalign_righttop
            // 
            this.mi_columnformat_textalign_righttop.Name = "mi_columnformat_textalign_righttop";
            this.mi_columnformat_textalign_righttop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_righttop.Text = "6 - RightTop";
            this.mi_columnformat_textalign_righttop.Click += new System.EventHandler(this.mi_column_textalign_righttop_click);
            // 
            // mi_columnformat_textalign_rightcenter
            // 
            this.mi_columnformat_textalign_rightcenter.Name = "mi_columnformat_textalign_rightcenter";
            this.mi_columnformat_textalign_rightcenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_rightcenter.Text = "7 - RightCenter";
            this.mi_columnformat_textalign_rightcenter.Click += new System.EventHandler(this.mi_column_textalign_rightcenter_click);
            // 
            // mi_columnformat_textalign_rightbottom
            // 
            this.mi_columnformat_textalign_rightbottom.Name = "mi_columnformat_textalign_rightbottom";
            this.mi_columnformat_textalign_rightbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_textalign_rightbottom.Text = "8 - RightBottom";
            this.mi_columnformat_textalign_rightbottom.Click += new System.EventHandler(this.mi_column_textalign_rightbottom_click);
            // 
            // mi_columnformat_titlealign
            // 
            this.mi_columnformat_titlealign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_titlealign_lefttop,
            this.mi_columnformat_titlealign_leftcenter,
            this.mi_columnformat_titlealign_leftbottom,
            this.mi_columnformat_titlealign_centertop,
            this.mi_columnformat_titlealign_centercenter,
            this.mi_columnformat_titlealign_centerbottom,
            this.mi_columnformat_titlealign_righttop,
            this.mi_columnformat_titlealign_rightcenter,
            this.mi_columnformat_titlealign_rightbottom});
            this.mi_columnformat_titlealign.Name = "mi_columnformat_titlealign";
            this.mi_columnformat_titlealign.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_titlealign.Text = "Title Alignment";
            // 
            // mi_columnformat_titlealign_lefttop
            // 
            this.mi_columnformat_titlealign_lefttop.Name = "mi_columnformat_titlealign_lefttop";
            this.mi_columnformat_titlealign_lefttop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_lefttop.Text = "0 - LeftTop";
            this.mi_columnformat_titlealign_lefttop.Click += new System.EventHandler(this.mi_column_titlealign_lefttop_click);
            // 
            // mi_columnformat_titlealign_leftcenter
            // 
            this.mi_columnformat_titlealign_leftcenter.Name = "mi_columnformat_titlealign_leftcenter";
            this.mi_columnformat_titlealign_leftcenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_leftcenter.Text = "1 - LeftCenter";
            this.mi_columnformat_titlealign_leftcenter.Click += new System.EventHandler(this.mi_column_titlealign_leftcenter_click);
            // 
            // mi_columnformat_titlealign_leftbottom
            // 
            this.mi_columnformat_titlealign_leftbottom.Name = "mi_columnformat_titlealign_leftbottom";
            this.mi_columnformat_titlealign_leftbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_leftbottom.Text = "2 - LeftBottom";
            this.mi_columnformat_titlealign_leftbottom.Click += new System.EventHandler(this.mi_column_titlealign_leftbottom_click);
            // 
            // mi_columnformat_titlealign_centertop
            // 
            this.mi_columnformat_titlealign_centertop.Name = "mi_columnformat_titlealign_centertop";
            this.mi_columnformat_titlealign_centertop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_centertop.Text = "3 - CenterTop";
            this.mi_columnformat_titlealign_centertop.Click += new System.EventHandler(this.mi_column_titlealign_centertop_click);
            // 
            // mi_columnformat_titlealign_centercenter
            // 
            this.mi_columnformat_titlealign_centercenter.Name = "mi_columnformat_titlealign_centercenter";
            this.mi_columnformat_titlealign_centercenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_centercenter.Text = "4 - CenterCenter";
            this.mi_columnformat_titlealign_centercenter.Click += new System.EventHandler(this.mi_column_titlealign_centercenter_click);
            // 
            // mi_columnformat_titlealign_centerbottom
            // 
            this.mi_columnformat_titlealign_centerbottom.Name = "mi_columnformat_titlealign_centerbottom";
            this.mi_columnformat_titlealign_centerbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_centerbottom.Text = "5 - CenterBottom";
            this.mi_columnformat_titlealign_centerbottom.Click += new System.EventHandler(this.mi_column_titlealign_centerbottom_click);
            // 
            // mi_columnformat_titlealign_righttop
            // 
            this.mi_columnformat_titlealign_righttop.Name = "mi_columnformat_titlealign_righttop";
            this.mi_columnformat_titlealign_righttop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_righttop.Text = "6 - RightTop";
            this.mi_columnformat_titlealign_righttop.Click += new System.EventHandler(this.mi_column_titlealign_righttop_click);
            // 
            // mi_columnformat_titlealign_rightcenter
            // 
            this.mi_columnformat_titlealign_rightcenter.Name = "mi_columnformat_titlealign_rightcenter";
            this.mi_columnformat_titlealign_rightcenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_rightcenter.Text = "7 - RightCenter";
            this.mi_columnformat_titlealign_rightcenter.Click += new System.EventHandler(this.mi_column_titlealign_rightcenter_click);
            // 
            // mi_columnformat_titlealign_rightbottom
            // 
            this.mi_columnformat_titlealign_rightbottom.Name = "mi_columnformat_titlealign_rightbottom";
            this.mi_columnformat_titlealign_rightbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_titlealign_rightbottom.Text = "8 - RightBottom";
            this.mi_columnformat_titlealign_rightbottom.Click += new System.EventHandler(this.mi_column_titlealign_rightbottom_click);
            // 
            // mi_columnformat_picalign
            // 
            this.mi_columnformat_picalign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_picalign_lefttop,
            this.mi_columnformat_picalign_leftcenter,
            this.mi_columnformat_picalign_leftbottom,
            this.mi_columnformat_picalign_centertop,
            this.mi_columnformat_picalign_centercenter,
            this.mi_columnformat_picalign_centerbottom,
            this.mi_columnformat_picalign_righttop,
            this.mi_columnformat_picalign_rightcenter,
            this.mi_columnformat_picalign_rightbottom,
            this.mi_columnformat_picalign_streth});
            this.mi_columnformat_picalign.Name = "mi_columnformat_picalign";
            this.mi_columnformat_picalign.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_picalign.Text = "Picture Alignment";
            // 
            // mi_columnformat_picalign_lefttop
            // 
            this.mi_columnformat_picalign_lefttop.Name = "mi_columnformat_picalign_lefttop";
            this.mi_columnformat_picalign_lefttop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_lefttop.Text = "0 - LeftTop";
            this.mi_columnformat_picalign_lefttop.Click += new System.EventHandler(this.mi_column_picalign_lefttop_click);
            // 
            // mi_columnformat_picalign_leftcenter
            // 
            this.mi_columnformat_picalign_leftcenter.Name = "mi_columnformat_picalign_leftcenter";
            this.mi_columnformat_picalign_leftcenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_leftcenter.Text = "1 - LeftCenter";
            this.mi_columnformat_picalign_leftcenter.Click += new System.EventHandler(this.mi_column_picalign_leftcenter_click);
            // 
            // mi_columnformat_picalign_leftbottom
            // 
            this.mi_columnformat_picalign_leftbottom.Name = "mi_columnformat_picalign_leftbottom";
            this.mi_columnformat_picalign_leftbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_leftbottom.Text = "2 - LeftBottom";
            this.mi_columnformat_picalign_leftbottom.Click += new System.EventHandler(this.mi_column_picalign_leftbottom_click);
            // 
            // mi_columnformat_picalign_centertop
            // 
            this.mi_columnformat_picalign_centertop.Name = "mi_columnformat_picalign_centertop";
            this.mi_columnformat_picalign_centertop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_centertop.Text = "3 - CenterTop";
            this.mi_columnformat_picalign_centertop.Click += new System.EventHandler(this.mi_column_picalign_centertop_click);
            // 
            // mi_columnformat_picalign_centercenter
            // 
            this.mi_columnformat_picalign_centercenter.Name = "mi_columnformat_picalign_centercenter";
            this.mi_columnformat_picalign_centercenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_centercenter.Text = "4 - CenterCenter";
            this.mi_columnformat_picalign_centercenter.Click += new System.EventHandler(this.mi_column_picalign_centercenter_click);
            // 
            // mi_columnformat_picalign_centerbottom
            // 
            this.mi_columnformat_picalign_centerbottom.Name = "mi_columnformat_picalign_centerbottom";
            this.mi_columnformat_picalign_centerbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_centerbottom.Text = "5 - CenterBottom";
            this.mi_columnformat_picalign_centerbottom.Click += new System.EventHandler(this.mi_column_picalign_centerbottom_click);
            // 
            // mi_columnformat_picalign_righttop
            // 
            this.mi_columnformat_picalign_righttop.Name = "mi_columnformat_picalign_righttop";
            this.mi_columnformat_picalign_righttop.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_righttop.Text = "6 - RightTop";
            this.mi_columnformat_picalign_righttop.Click += new System.EventHandler(this.mi_column_picalign_righttop_click);
            // 
            // mi_columnformat_picalign_rightcenter
            // 
            this.mi_columnformat_picalign_rightcenter.Name = "mi_columnformat_picalign_rightcenter";
            this.mi_columnformat_picalign_rightcenter.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_rightcenter.Text = "7 - RightCenter";
            this.mi_columnformat_picalign_rightcenter.Click += new System.EventHandler(this.mi_column_picalign_rightcenter_click);
            // 
            // mi_columnformat_picalign_rightbottom
            // 
            this.mi_columnformat_picalign_rightbottom.Name = "mi_columnformat_picalign_rightbottom";
            this.mi_columnformat_picalign_rightbottom.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_rightbottom.Text = "8 - RightBottom";
            this.mi_columnformat_picalign_rightbottom.Click += new System.EventHandler(this.mi_column_picalign_rightbottom_click);
            // 
            // mi_columnformat_picalign_streth
            // 
            this.mi_columnformat_picalign_streth.Name = "mi_columnformat_picalign_streth";
            this.mi_columnformat_picalign_streth.Size = new System.Drawing.Size(166, 22);
            this.mi_columnformat_picalign_streth.Text = "9 - Streth";
            this.mi_columnformat_picalign_streth.Click += new System.EventHandler(this.mi_column_picalign_stretch_click);
            // 
            // mi_columnformat_sort
            // 
            this.mi_columnformat_sort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_sort_bystring,
            this.mi_columnformat_sort_bystringnocase,
            this.mi_columnformat_sort_byboolean,
            this.mi_columnformat_sort_bydate,
            this.mi_columnformat_sort_bynumeric});
            this.mi_columnformat_sort.Name = "mi_columnformat_sort";
            this.mi_columnformat_sort.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_sort.Text = "Sort Type";
            // 
            // mi_columnformat_sort_bystring
            // 
            this.mi_columnformat_sort_bystring.Name = "mi_columnformat_sort_bystring";
            this.mi_columnformat_sort_bystring.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_sort_bystring.Text = "0 - ByString";
            this.mi_columnformat_sort_bystring.Click += new System.EventHandler(this.mi_column_sort_bystring_click);
            // 
            // mi_columnformat_sort_bystringnocase
            // 
            this.mi_columnformat_sort_bystringnocase.Name = "mi_columnformat_sort_bystringnocase";
            this.mi_columnformat_sort_bystringnocase.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_sort_bystringnocase.Text = "1 - ByStringNoCase";
            this.mi_columnformat_sort_bystringnocase.Click += new System.EventHandler(this.mi_column_sort_bystringnocase_click);
            // 
            // mi_columnformat_sort_byboolean
            // 
            this.mi_columnformat_sort_byboolean.Name = "mi_columnformat_sort_byboolean";
            this.mi_columnformat_sort_byboolean.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_sort_byboolean.Text = "2 - ByBoolean";
            this.mi_columnformat_sort_byboolean.Click += new System.EventHandler(this.mi_column_sort_byboolean_click);
            // 
            // mi_columnformat_sort_bydate
            // 
            this.mi_columnformat_sort_bydate.Name = "mi_columnformat_sort_bydate";
            this.mi_columnformat_sort_bydate.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_sort_bydate.Text = "3 - ByDate";
            this.mi_columnformat_sort_bydate.Click += new System.EventHandler(this.mi_column_sort_bydate_click);
            // 
            // mi_columnformat_sort_bynumeric
            // 
            this.mi_columnformat_sort_bynumeric.Name = "mi_columnformat_sort_bynumeric";
            this.mi_columnformat_sort_bynumeric.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_sort_bynumeric.Text = "4 - ByNumeric";
            this.mi_columnformat_sort_bynumeric.Click += new System.EventHandler(this.mi_column_sort_bynumeric_click);
            // 
            // mi_columnformat_datestyle
            // 
            this.mi_columnformat_datestyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_columnformat_datestyle_date,
            this.mi_columnformat_datestyle_time,
            this.mi_columnformat_datestyle_datetime});
            this.mi_columnformat_datestyle.Name = "mi_columnformat_datestyle";
            this.mi_columnformat_datestyle.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_datestyle.Text = "Date Style";
            // 
            // mi_columnformat_datestyle_date
            // 
            this.mi_columnformat_datestyle_date.Name = "mi_columnformat_datestyle_date";
            this.mi_columnformat_datestyle_date.Size = new System.Drawing.Size(160, 22);
            this.mi_columnformat_datestyle_date.Text = "0 - flexDate";
            this.mi_columnformat_datestyle_date.Click += new System.EventHandler(this.mi_column_datestyle_date_click);
            // 
            // mi_columnformat_datestyle_time
            // 
            this.mi_columnformat_datestyle_time.Name = "mi_columnformat_datestyle_time";
            this.mi_columnformat_datestyle_time.Size = new System.Drawing.Size(160, 22);
            this.mi_columnformat_datestyle_time.Text = "1 - flexTime";
            this.mi_columnformat_datestyle_time.Click += new System.EventHandler(this.mi_column_datestyle_time_click);
            // 
            // mi_columnformat_datestyle_datetime
            // 
            this.mi_columnformat_datestyle_datetime.Name = "mi_columnformat_datestyle_datetime";
            this.mi_columnformat_datestyle_datetime.Size = new System.Drawing.Size(160, 22);
            this.mi_columnformat_datestyle_datetime.Text = "2 - flexDateTime";
            this.mi_columnformat_datestyle_datetime.Click += new System.EventHandler(this.mi_column_datestyle_datetime_click);
            // 
            // ToolStripSeparator26
            // 
            this.ToolStripSeparator26.Name = "ToolStripSeparator26";
            this.ToolStripSeparator26.Size = new System.Drawing.Size(173, 6);
            // 
            // mi_columnformat_lock
            // 
            this.mi_columnformat_lock.Name = "mi_columnformat_lock";
            this.mi_columnformat_lock.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_lock.Text = "Lock Column";
            this.mi_columnformat_lock.Click += new System.EventHandler(this.mi_columnformat_lock_click);
            // 
            // mi_columnformat_unlock
            // 
            this.mi_columnformat_unlock.Name = "mi_columnformat_unlock";
            this.mi_columnformat_unlock.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_unlock.Text = "Unlock Column";
            this.mi_columnformat_unlock.Click += new System.EventHandler(this.mi_columnformat_unlock_click);
            // 
            // ToolStripSeparator27
            // 
            this.ToolStripSeparator27.Name = "ToolStripSeparator27";
            this.ToolStripSeparator27.Size = new System.Drawing.Size(173, 6);
            // 
            // mi_columnformat_title
            // 
            this.mi_columnformat_title.Name = "mi_columnformat_title";
            this.mi_columnformat_title.Size = new System.Drawing.Size(176, 22);
            this.mi_columnformat_title.Text = "Column Title...";
            this.mi_columnformat_title.Click += new System.EventHandler(this.mi_columnformat_title_click);
            // 
            // ToolStripSeparator10
            // 
            this.ToolStripSeparator10.Name = "ToolStripSeparator10";
            this.ToolStripSeparator10.Size = new System.Drawing.Size(114, 6);
            // 
            // mi_cellformat
            // 
            this.mi_cellformat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_cellformat_setwraptext,
            this.mi_cellformat_cancelwrap,
            this.ToolStripSeparator28,
            this.mi_cellformat_lock,
            this.mi_cellformat_unlock,
            this.ToolStripSeparator29,
            this.mi_cellformat_iamge});
            this.mi_cellformat.Name = "mi_cellformat";
            this.mi_cellformat.Size = new System.Drawing.Size(117, 22);
            this.mi_cellformat.Text = "Cell";
            // 
            // mi_cellformat_setwraptext
            // 
            this.mi_cellformat_setwraptext.Name = "mi_cellformat_setwraptext";
            this.mi_cellformat_setwraptext.Size = new System.Drawing.Size(163, 22);
            this.mi_cellformat_setwraptext.Text = "Set WrapText";
            this.mi_cellformat_setwraptext.Click += new System.EventHandler(this.mi_cellformat_setwraptext_click);
            // 
            // mi_cellformat_cancelwrap
            // 
            this.mi_cellformat_cancelwrap.Name = "mi_cellformat_cancelwrap";
            this.mi_cellformat_cancelwrap.Size = new System.Drawing.Size(163, 22);
            this.mi_cellformat_cancelwrap.Text = "Cancel WrapText";
            this.mi_cellformat_cancelwrap.Click += new System.EventHandler(this.mi_cellformat_cancelwrap_click);
            // 
            // ToolStripSeparator28
            // 
            this.ToolStripSeparator28.Name = "ToolStripSeparator28";
            this.ToolStripSeparator28.Size = new System.Drawing.Size(160, 6);
            // 
            // mi_cellformat_lock
            // 
            this.mi_cellformat_lock.Name = "mi_cellformat_lock";
            this.mi_cellformat_lock.Size = new System.Drawing.Size(163, 22);
            this.mi_cellformat_lock.Text = "Lock Cell";
            this.mi_cellformat_lock.Click += new System.EventHandler(this.mi_cellformat_lock_click);
            // 
            // mi_cellformat_unlock
            // 
            this.mi_cellformat_unlock.Name = "mi_cellformat_unlock";
            this.mi_cellformat_unlock.Size = new System.Drawing.Size(163, 22);
            this.mi_cellformat_unlock.Text = "Unlock Cell";
            this.mi_cellformat_unlock.Click += new System.EventHandler(this.mi_cellformat_unlock_click);
            // 
            // ToolStripSeparator29
            // 
            this.ToolStripSeparator29.Name = "ToolStripSeparator29";
            this.ToolStripSeparator29.Size = new System.Drawing.Size(160, 6);
            // 
            // mi_cellformat_iamge
            // 
            this.mi_cellformat_iamge.Name = "mi_cellformat_iamge";
            this.mi_cellformat_iamge.Size = new System.Drawing.Size(163, 22);
            this.mi_cellformat_iamge.Text = "Set Cell Image...";
            this.mi_cellformat_iamge.Click += new System.EventHandler(this.mi_cellformat_iamge_click);
            // 
            // mi_help
            // 
            this.mi_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mi_help_manual,
            this.mi_help_updates,
            this.ToolStripSeparator24,
            this.mi_help_about});
            this.mi_help.Name = "mi_help";
            this.mi_help.Size = new System.Drawing.Size(44, 20);
            this.mi_help.Text = "&Help";
            // 
            // mi_help_manual
            // 
            this.mi_help_manual.Name = "mi_help_manual";
            this.mi_help_manual.Size = new System.Drawing.Size(171, 22);
            this.mi_help_manual.Text = "User Manual";
            this.mi_help_manual.Click += new System.EventHandler(this.mi_help_manual_click);
            // 
            // mi_help_updates
            // 
            this.mi_help_updates.Name = "mi_help_updates";
            this.mi_help_updates.Size = new System.Drawing.Size(171, 22);
            this.mi_help_updates.Text = "Check for Updates";
            this.mi_help_updates.Click += new System.EventHandler(this.mi_help_updates_click);
            // 
            // ToolStripSeparator24
            // 
            this.ToolStripSeparator24.Name = "ToolStripSeparator24";
            this.ToolStripSeparator24.Size = new System.Drawing.Size(168, 6);
            // 
            // mi_help_about
            // 
            this.mi_help_about.Name = "mi_help_about";
            this.mi_help_about.Size = new System.Drawing.Size(171, 22);
            this.mi_help_about.Text = "About ...";
            this.mi_help_about.Click += new System.EventHandler(this.mi_help_about_click);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_new,
            this.bt_open,
            this.bt_save,
            this.ToolStripSeparator12,
            this.bt_print,
            this.bt_printpreview,
            this.ToolStripSeparator13,
            this.bt_cut,
            this.bt_copy,
            this.bt_paste,
            this.ToolStripSeparator14,
            this.bt_merge,
            this.bt_unmerge,
            this.ToolStripSeparator15,
            this.bt_insertrow,
            this.bt_insertcolumn,
            this.bt_deleterow,
            this.bt_deletecolumn,
            this.ToolStripSeparator16,
            this.bt_image,
            this.bt_help});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(976, 25);
            this.ToolStrip1.TabIndex = 4;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // bt_new
            // 
            this.bt_new.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_new.Image = ((System.Drawing.Image)(resources.GetObject("bt_new.Image")));
            this.bt_new.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_new.Name = "bt_new";
            this.bt_new.Size = new System.Drawing.Size(23, 22);
            this.bt_new.Text = "ToolStripButton1";
            this.bt_new.ToolTipText = "New";
            this.bt_new.Click += new System.EventHandler(this.bt_new_click);
            // 
            // bt_open
            // 
            this.bt_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_open.Image = ((System.Drawing.Image)(resources.GetObject("bt_open.Image")));
            this.bt_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_open.Name = "bt_open";
            this.bt_open.Size = new System.Drawing.Size(23, 22);
            this.bt_open.Text = "ToolStripButton2";
            this.bt_open.ToolTipText = "Open";
            this.bt_open.Click += new System.EventHandler(this.bt_open_click);
            // 
            // bt_save
            // 
            this.bt_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_save.Image = ((System.Drawing.Image)(resources.GetObject("bt_save.Image")));
            this.bt_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_save.Name = "bt_save";
            this.bt_save.Size = new System.Drawing.Size(23, 22);
            this.bt_save.Text = "ToolStripButton3";
            this.bt_save.ToolTipText = "Save";
            this.bt_save.Click += new System.EventHandler(this.bt_save_click);
            // 
            // ToolStripSeparator12
            // 
            this.ToolStripSeparator12.Name = "ToolStripSeparator12";
            this.ToolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_print
            // 
            this.bt_print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_print.Image = ((System.Drawing.Image)(resources.GetObject("bt_print.Image")));
            this.bt_print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_print.Name = "bt_print";
            this.bt_print.Size = new System.Drawing.Size(23, 22);
            this.bt_print.Text = "ToolStripButton4";
            this.bt_print.ToolTipText = "Print";
            this.bt_print.Click += new System.EventHandler(this.bt_print_click);
            // 
            // bt_printpreview
            // 
            this.bt_printpreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_printpreview.Image = ((System.Drawing.Image)(resources.GetObject("bt_printpreview.Image")));
            this.bt_printpreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_printpreview.Name = "bt_printpreview";
            this.bt_printpreview.Size = new System.Drawing.Size(23, 22);
            this.bt_printpreview.Text = "ToolStripButton5";
            this.bt_printpreview.ToolTipText = "Print Preview";
            this.bt_printpreview.Click += new System.EventHandler(this.bt_printpreview_click);
            // 
            // ToolStripSeparator13
            // 
            this.ToolStripSeparator13.Name = "ToolStripSeparator13";
            this.ToolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_cut
            // 
            this.bt_cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_cut.Image = ((System.Drawing.Image)(resources.GetObject("bt_cut.Image")));
            this.bt_cut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_cut.Name = "bt_cut";
            this.bt_cut.Size = new System.Drawing.Size(23, 22);
            this.bt_cut.Text = "ToolStripButton6";
            this.bt_cut.ToolTipText = "Cut";
            this.bt_cut.Click += new System.EventHandler(this.bt_cut_click);
            // 
            // bt_copy
            // 
            this.bt_copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_copy.Image = ((System.Drawing.Image)(resources.GetObject("bt_copy.Image")));
            this.bt_copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_copy.Name = "bt_copy";
            this.bt_copy.Size = new System.Drawing.Size(23, 22);
            this.bt_copy.Text = "ToolStripButton7";
            this.bt_copy.ToolTipText = "Copy";
            this.bt_copy.Click += new System.EventHandler(this.bt_copy_click);
            // 
            // bt_paste
            // 
            this.bt_paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_paste.Image = ((System.Drawing.Image)(resources.GetObject("bt_paste.Image")));
            this.bt_paste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_paste.Name = "bt_paste";
            this.bt_paste.Size = new System.Drawing.Size(23, 22);
            this.bt_paste.Text = "ToolStripButton8";
            this.bt_paste.ToolTipText = "Paste";
            this.bt_paste.Click += new System.EventHandler(this.bt_paste_click);
            // 
            // ToolStripSeparator14
            // 
            this.ToolStripSeparator14.Name = "ToolStripSeparator14";
            this.ToolStripSeparator14.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_merge
            // 
            this.bt_merge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_merge.Image = ((System.Drawing.Image)(resources.GetObject("bt_merge.Image")));
            this.bt_merge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_merge.Name = "bt_merge";
            this.bt_merge.Size = new System.Drawing.Size(23, 22);
            this.bt_merge.Text = "ToolStripButton9";
            this.bt_merge.ToolTipText = "Merge";
            this.bt_merge.Click += new System.EventHandler(this.bt_merge_click);
            // 
            // bt_unmerge
            // 
            this.bt_unmerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_unmerge.Image = ((System.Drawing.Image)(resources.GetObject("bt_unmerge.Image")));
            this.bt_unmerge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_unmerge.Name = "bt_unmerge";
            this.bt_unmerge.Size = new System.Drawing.Size(23, 22);
            this.bt_unmerge.Text = "ToolStripButton10";
            this.bt_unmerge.ToolTipText = "Unmerge";
            this.bt_unmerge.Click += new System.EventHandler(this.bt_unmerge_click);
            // 
            // ToolStripSeparator15
            // 
            this.ToolStripSeparator15.Name = "ToolStripSeparator15";
            this.ToolStripSeparator15.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_insertrow
            // 
            this.bt_insertrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_insertrow.Image = ((System.Drawing.Image)(resources.GetObject("bt_insertrow.Image")));
            this.bt_insertrow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_insertrow.Name = "bt_insertrow";
            this.bt_insertrow.Size = new System.Drawing.Size(23, 22);
            this.bt_insertrow.Text = "ToolStripButton11";
            this.bt_insertrow.ToolTipText = "Insert Rows";
            this.bt_insertrow.Click += new System.EventHandler(this.bt_insertrow_click);
            // 
            // bt_insertcolumn
            // 
            this.bt_insertcolumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_insertcolumn.Image = ((System.Drawing.Image)(resources.GetObject("bt_insertcolumn.Image")));
            this.bt_insertcolumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_insertcolumn.Name = "bt_insertcolumn";
            this.bt_insertcolumn.Size = new System.Drawing.Size(23, 22);
            this.bt_insertcolumn.Text = "ToolStripButton12";
            this.bt_insertcolumn.ToolTipText = "Insert Columns";
            this.bt_insertcolumn.Click += new System.EventHandler(this.bt_insertcolumn_Click);
            // 
            // bt_deleterow
            // 
            this.bt_deleterow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_deleterow.Image = ((System.Drawing.Image)(resources.GetObject("bt_deleterow.Image")));
            this.bt_deleterow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_deleterow.Name = "bt_deleterow";
            this.bt_deleterow.Size = new System.Drawing.Size(23, 22);
            this.bt_deleterow.Text = "ToolStripButton13";
            this.bt_deleterow.ToolTipText = "Delete Rows";
            this.bt_deleterow.Click += new System.EventHandler(this.bt_deleterow_click);
            // 
            // bt_deletecolumn
            // 
            this.bt_deletecolumn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_deletecolumn.Image = ((System.Drawing.Image)(resources.GetObject("bt_deletecolumn.Image")));
            this.bt_deletecolumn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_deletecolumn.Name = "bt_deletecolumn";
            this.bt_deletecolumn.Size = new System.Drawing.Size(23, 22);
            this.bt_deletecolumn.Text = "ToolStripButton14";
            this.bt_deletecolumn.ToolTipText = "Delete Columns";
            this.bt_deletecolumn.Click += new System.EventHandler(this.bt_deletecolumn_click);
            // 
            // ToolStripSeparator16
            // 
            this.ToolStripSeparator16.Name = "ToolStripSeparator16";
            this.ToolStripSeparator16.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_image
            // 
            this.bt_image.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_image.Image = ((System.Drawing.Image)(resources.GetObject("bt_image.Image")));
            this.bt_image.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_image.Name = "bt_image";
            this.bt_image.Size = new System.Drawing.Size(23, 22);
            this.bt_image.Text = "ToolStripButton15";
            this.bt_image.ToolTipText = "Set Cell Image";
            this.bt_image.Click += new System.EventHandler(this.bt_image_Click);
            // 
            // bt_help
            // 
            this.bt_help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_help.Image = ((System.Drawing.Image)(resources.GetObject("bt_help.Image")));
            this.bt_help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_help.Name = "bt_help";
            this.bt_help.Size = new System.Drawing.Size(23, 22);
            this.bt_help.Text = "ToolStripButton16";
            this.bt_help.ToolTipText = "Help";
            this.bt_help.Click += new System.EventHandler(this.bt_help_click);
            // 
            // ToolStrip2
            // 
            this.ToolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cboFontName,
            this.cboFontSize,
            this.bt_fontbold,
            this.bt_fontitalic,
            this.bt_fontunderline,
            this.bt_FontStrikethrough,
            this.ToolStripSeparator17,
            this.ToolStripLabel1,
            this.bt_imagealign,
            this.ToolStripSeparator18,
            this.ToolStripLabel2,
            this.bt_textalign,
            this.ToolStripSeparator19,
            this.ToolStripLabel5,
            this.bt_cellcolor,
            this.ToolStripSeparator20,
            this.ToolStripLabel6,
            this.bt_textcolor,
            this.ToolStripSeparator21,
            this.ToolStripLabel7,
            this.bt_cellborder});
            this.ToolStrip2.Location = new System.Drawing.Point(0, 49);
            this.ToolStrip2.Name = "ToolStrip2";
            this.ToolStrip2.Size = new System.Drawing.Size(976, 25);
            this.ToolStrip2.TabIndex = 0;
            this.ToolStrip2.Text = "ToolStrip2";
            // 
            // cboFontName
            // 
            this.cboFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFontName.Name = "cboFontName";
            this.cboFontName.Size = new System.Drawing.Size(121, 25);
            this.cboFontName.ToolTipText = "Font Name";
            this.cboFontName.SelectedIndexChanged += new System.EventHandler(this.cboFontName_SelectedIndexChanged);
            // 
            // cboFontSize
            // 
            this.cboFontSize.Name = "cboFontSize";
            this.cboFontSize.Size = new System.Drawing.Size(80, 25);
            this.cboFontSize.ToolTipText = "Font Size";
            this.cboFontSize.SelectedIndexChanged += new System.EventHandler(this.cboFontSize_SelectedIndexChanged);
            this.cboFontSize.TextChanged += new System.EventHandler(this.cboFontSize_TextChanged);
            // 
            // bt_fontbold
            // 
            this.bt_fontbold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_fontbold.Image = ((System.Drawing.Image)(resources.GetObject("bt_fontbold.Image")));
            this.bt_fontbold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_fontbold.Name = "bt_fontbold";
            this.bt_fontbold.Size = new System.Drawing.Size(23, 22);
            this.bt_fontbold.Text = "ToolStripButton17";
            this.bt_fontbold.ToolTipText = "Bold";
            this.bt_fontbold.Click += new System.EventHandler(this.bt_fontbold_click);
            // 
            // bt_fontitalic
            // 
            this.bt_fontitalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_fontitalic.Image = ((System.Drawing.Image)(resources.GetObject("bt_fontitalic.Image")));
            this.bt_fontitalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_fontitalic.Name = "bt_fontitalic";
            this.bt_fontitalic.Size = new System.Drawing.Size(23, 22);
            this.bt_fontitalic.Text = "ToolStripButton18";
            this.bt_fontitalic.ToolTipText = "Italic";
            this.bt_fontitalic.Click += new System.EventHandler(this.bt_fontitalic_click);
            // 
            // bt_fontunderline
            // 
            this.bt_fontunderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_fontunderline.Image = ((System.Drawing.Image)(resources.GetObject("bt_fontunderline.Image")));
            this.bt_fontunderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_fontunderline.Name = "bt_fontunderline";
            this.bt_fontunderline.Size = new System.Drawing.Size(23, 22);
            this.bt_fontunderline.Text = "ToolStripButton19";
            this.bt_fontunderline.ToolTipText = "Underline";
            this.bt_fontunderline.Click += new System.EventHandler(this.bt_fontunderline_click);
            // 
            // bt_FontStrikethrough
            // 
            this.bt_FontStrikethrough.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_FontStrikethrough.Image = ((System.Drawing.Image)(resources.GetObject("bt_FontStrikethrough.Image")));
            this.bt_FontStrikethrough.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_FontStrikethrough.Name = "bt_FontStrikethrough";
            this.bt_FontStrikethrough.Size = new System.Drawing.Size(23, 22);
            this.bt_FontStrikethrough.Text = "ToolStripButton20";
            this.bt_FontStrikethrough.ToolTipText = "Strikeout";
            this.bt_FontStrikethrough.Click += new System.EventHandler(this.bt_fontstrikethrough_click);
            // 
            // ToolStripSeparator17
            // 
            this.ToolStripSeparator17.Name = "ToolStripSeparator17";
            this.ToolStripSeparator17.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripLabel1
            // 
            this.ToolStripLabel1.Name = "ToolStripLabel1";
            this.ToolStripLabel1.Size = new System.Drawing.Size(68, 22);
            this.ToolStripLabel1.Text = "ImageAlign";
            // 
            // bt_imagealign
            // 
            this.bt_imagealign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_imagealign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_imagealign_lefttop,
            this.bt_imagealign_leftcenter,
            this.bt_imagealign_leftbottom,
            this.bt_imagealign_centertop,
            this.bt_imagealign_centercenter,
            this.bt_imagealign_centerbottom,
            this.bt_imagealign_righttop,
            this.bt_imagealign_rightcenter,
            this.bt_imagealign_rightbottom,
            this.bt_imagealign_stretch});
            this.bt_imagealign.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign.Image")));
            this.bt_imagealign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_imagealign.Name = "bt_imagealign";
            this.bt_imagealign.Size = new System.Drawing.Size(32, 22);
            this.bt_imagealign.Text = "ToolStripSplitButton1";
            this.bt_imagealign.ToolTipText = "Picture Alignment";
            this.bt_imagealign.ButtonClick += new System.EventHandler(this.bt_imagealign_ButtonClick);
            // 
            // bt_imagealign_lefttop
            // 
            this.bt_imagealign_lefttop.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_lefttop.Image")));
            this.bt_imagealign_lefttop.Name = "bt_imagealign_lefttop";
            this.bt_imagealign_lefttop.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_lefttop.Text = "0 - Left Top";
            this.bt_imagealign_lefttop.Click += new System.EventHandler(this.bt_imagealign_lefttop_click);
            // 
            // bt_imagealign_leftcenter
            // 
            this.bt_imagealign_leftcenter.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_leftcenter.Image")));
            this.bt_imagealign_leftcenter.Name = "bt_imagealign_leftcenter";
            this.bt_imagealign_leftcenter.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_leftcenter.Text = "1 - Left Center";
            this.bt_imagealign_leftcenter.Click += new System.EventHandler(this.bt_imagealign_leftcenter_click);
            // 
            // bt_imagealign_leftbottom
            // 
            this.bt_imagealign_leftbottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_leftbottom.Image")));
            this.bt_imagealign_leftbottom.Name = "bt_imagealign_leftbottom";
            this.bt_imagealign_leftbottom.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_leftbottom.Text = "2 - Left Bottom";
            this.bt_imagealign_leftbottom.Click += new System.EventHandler(this.bt_imagealign_leftbottom_click);
            // 
            // bt_imagealign_centertop
            // 
            this.bt_imagealign_centertop.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_centertop.Image")));
            this.bt_imagealign_centertop.Name = "bt_imagealign_centertop";
            this.bt_imagealign_centertop.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_centertop.Text = "3 - Center Top";
            this.bt_imagealign_centertop.Click += new System.EventHandler(this.bt_imagealign_centertop_click);
            // 
            // bt_imagealign_centercenter
            // 
            this.bt_imagealign_centercenter.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_centercenter.Image")));
            this.bt_imagealign_centercenter.Name = "bt_imagealign_centercenter";
            this.bt_imagealign_centercenter.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_centercenter.Text = "4 - Center Center";
            this.bt_imagealign_centercenter.Click += new System.EventHandler(this.bt_imagealign_centercenter_click);
            // 
            // bt_imagealign_centerbottom
            // 
            this.bt_imagealign_centerbottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_centerbottom.Image")));
            this.bt_imagealign_centerbottom.Name = "bt_imagealign_centerbottom";
            this.bt_imagealign_centerbottom.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_centerbottom.Text = "5 - Center Bottom";
            this.bt_imagealign_centerbottom.Click += new System.EventHandler(this.bt_imagealign_centerbottom_click);
            // 
            // bt_imagealign_righttop
            // 
            this.bt_imagealign_righttop.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_righttop.Image")));
            this.bt_imagealign_righttop.Name = "bt_imagealign_righttop";
            this.bt_imagealign_righttop.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_righttop.Text = "6 - Right Top";
            this.bt_imagealign_righttop.Click += new System.EventHandler(this.bt_imagealign_righttop_click);
            // 
            // bt_imagealign_rightcenter
            // 
            this.bt_imagealign_rightcenter.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_rightcenter.Image")));
            this.bt_imagealign_rightcenter.Name = "bt_imagealign_rightcenter";
            this.bt_imagealign_rightcenter.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_rightcenter.Text = "7 - Right Center";
            this.bt_imagealign_rightcenter.Click += new System.EventHandler(this.bt_imagealign_rightcenter_click);
            // 
            // bt_imagealign_rightbottom
            // 
            this.bt_imagealign_rightbottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_rightbottom.Image")));
            this.bt_imagealign_rightbottom.Name = "bt_imagealign_rightbottom";
            this.bt_imagealign_rightbottom.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_rightbottom.Text = "8 - Right Bottom";
            this.bt_imagealign_rightbottom.Click += new System.EventHandler(this.bt_imagealign_rightbottom_click);
            // 
            // bt_imagealign_stretch
            // 
            this.bt_imagealign_stretch.Image = ((System.Drawing.Image)(resources.GetObject("bt_imagealign_stretch.Image")));
            this.bt_imagealign_stretch.Name = "bt_imagealign_stretch";
            this.bt_imagealign_stretch.Size = new System.Drawing.Size(169, 22);
            this.bt_imagealign_stretch.Text = "9 - Stretch";
            this.bt_imagealign_stretch.Click += new System.EventHandler(this.bt_imagealign_stretch_click);
            // 
            // ToolStripSeparator18
            // 
            this.ToolStripSeparator18.Name = "ToolStripSeparator18";
            this.ToolStripSeparator18.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripLabel2
            // 
            this.ToolStripLabel2.Name = "ToolStripLabel2";
            this.ToolStripLabel2.Size = new System.Drawing.Size(57, 22);
            this.ToolStripLabel2.Text = "TextAlign";
            // 
            // bt_textalign
            // 
            this.bt_textalign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_textalign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_textalign_lefttop,
            this.bt_textalign_leftcenter,
            this.bt_textalign_leftbottom,
            this.bt_textalign_centertop,
            this.bt_textalign_centercenter,
            this.bt_textalign_centerbottom,
            this.bt_textalign_righttop,
            this.bt_textalign_rightcenter,
            this.bt_textalign_rightbottom});
            this.bt_textalign.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign.Image")));
            this.bt_textalign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_textalign.Name = "bt_textalign";
            this.bt_textalign.Size = new System.Drawing.Size(32, 22);
            this.bt_textalign.Text = "ToolStripSplitButton1";
            this.bt_textalign.ToolTipText = "Text Alignment";
            this.bt_textalign.ButtonClick += new System.EventHandler(this.bt_textalign_ButtonClick);
            // 
            // bt_textalign_lefttop
            // 
            this.bt_textalign_lefttop.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_lefttop.Image")));
            this.bt_textalign_lefttop.Name = "bt_textalign_lefttop";
            this.bt_textalign_lefttop.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_lefttop.Text = "0 - Left Top";
            this.bt_textalign_lefttop.Click += new System.EventHandler(this.bt_textalign_lefttop_click);
            // 
            // bt_textalign_leftcenter
            // 
            this.bt_textalign_leftcenter.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_leftcenter.Image")));
            this.bt_textalign_leftcenter.Name = "bt_textalign_leftcenter";
            this.bt_textalign_leftcenter.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_leftcenter.Text = "1 - Left Center";
            this.bt_textalign_leftcenter.Click += new System.EventHandler(this.bt_textalign_leftcenter_click);
            // 
            // bt_textalign_leftbottom
            // 
            this.bt_textalign_leftbottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_leftbottom.Image")));
            this.bt_textalign_leftbottom.Name = "bt_textalign_leftbottom";
            this.bt_textalign_leftbottom.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_leftbottom.Text = "2 - Left Bottom";
            this.bt_textalign_leftbottom.Click += new System.EventHandler(this.bt_textalign_leftbottom_click);
            // 
            // bt_textalign_centertop
            // 
            this.bt_textalign_centertop.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_centertop.Image")));
            this.bt_textalign_centertop.Name = "bt_textalign_centertop";
            this.bt_textalign_centertop.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_centertop.Text = "3 - Center Top";
            this.bt_textalign_centertop.Click += new System.EventHandler(this.bt_textalign_centertop_click);
            // 
            // bt_textalign_centercenter
            // 
            this.bt_textalign_centercenter.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_centercenter.Image")));
            this.bt_textalign_centercenter.Name = "bt_textalign_centercenter";
            this.bt_textalign_centercenter.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_centercenter.Text = "4 - Center Center";
            this.bt_textalign_centercenter.Click += new System.EventHandler(this.bt_textalign_centercenter_click);
            // 
            // bt_textalign_centerbottom
            // 
            this.bt_textalign_centerbottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_centerbottom.Image")));
            this.bt_textalign_centerbottom.Name = "bt_textalign_centerbottom";
            this.bt_textalign_centerbottom.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_centerbottom.Text = "5 - Center Bottom";
            this.bt_textalign_centerbottom.Click += new System.EventHandler(this.bt_textalign_centerbottom_click);
            // 
            // bt_textalign_righttop
            // 
            this.bt_textalign_righttop.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_righttop.Image")));
            this.bt_textalign_righttop.Name = "bt_textalign_righttop";
            this.bt_textalign_righttop.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_righttop.Text = "6 - Right Top";
            this.bt_textalign_righttop.Click += new System.EventHandler(this.bt_textalign_righttop_click);
            // 
            // bt_textalign_rightcenter
            // 
            this.bt_textalign_rightcenter.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_rightcenter.Image")));
            this.bt_textalign_rightcenter.Name = "bt_textalign_rightcenter";
            this.bt_textalign_rightcenter.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_rightcenter.Text = "7 - Right Center";
            this.bt_textalign_rightcenter.Click += new System.EventHandler(this.bt_textalign_rightcenter_click);
            // 
            // bt_textalign_rightbottom
            // 
            this.bt_textalign_rightbottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_textalign_rightbottom.Image")));
            this.bt_textalign_rightbottom.Name = "bt_textalign_rightbottom";
            this.bt_textalign_rightbottom.Size = new System.Drawing.Size(169, 22);
            this.bt_textalign_rightbottom.Text = "8 - Right Bottom";
            this.bt_textalign_rightbottom.Click += new System.EventHandler(this.bt_textalign_rightbottom_click);
            // 
            // ToolStripSeparator19
            // 
            this.ToolStripSeparator19.Name = "ToolStripSeparator19";
            this.ToolStripSeparator19.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripLabel5
            // 
            this.ToolStripLabel5.Name = "ToolStripLabel5";
            this.ToolStripLabel5.Size = new System.Drawing.Size(59, 22);
            this.ToolStripLabel5.Text = "Cell Color";
            // 
            // bt_cellcolor
            // 
            this.bt_cellcolor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_cellcolor.Name = "bt_cellcolor";
            this.bt_cellcolor.Size = new System.Drawing.Size(31, 22);
            this.bt_cellcolor.Text = "[]";
            this.bt_cellcolor.ButtonClick += new System.EventHandler(this.bt_cellcolor_ButtonClick);
            this.bt_cellcolor.DropDownOpening += new System.EventHandler(this.bt_cellcolor_DropDownOpening);
            // 
            // ToolStripSeparator20
            // 
            this.ToolStripSeparator20.Name = "ToolStripSeparator20";
            this.ToolStripSeparator20.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripLabel6
            // 
            this.ToolStripLabel6.Name = "ToolStripLabel6";
            this.ToolStripLabel6.Size = new System.Drawing.Size(61, 22);
            this.ToolStripLabel6.Text = "Text Color";
            // 
            // bt_textcolor
            // 
            this.bt_textcolor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_textcolor.Name = "bt_textcolor";
            this.bt_textcolor.Size = new System.Drawing.Size(31, 22);
            this.bt_textcolor.Text = "[]";
            this.bt_textcolor.ButtonClick += new System.EventHandler(this.bt_textcolor_ButtonClick);
            this.bt_textcolor.DropDownOpening += new System.EventHandler(this.bt_textcolor_DropDownOpening);
            // 
            // ToolStripSeparator21
            // 
            this.ToolStripSeparator21.Name = "ToolStripSeparator21";
            this.ToolStripSeparator21.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripLabel7
            // 
            this.ToolStripLabel7.Name = "ToolStripLabel7";
            this.ToolStripLabel7.Size = new System.Drawing.Size(65, 22);
            this.ToolStripLabel7.Text = "Cell Border";
            // 
            // bt_cellborder
            // 
            this.bt_cellborder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt_cellborder_none,
            this.bt_cellborder_around,
            this.bt_cellborder_left,
            this.bt_cellborder_top,
            this.bt_cellborder_right,
            this.bt_cellborder_bottom,
            this.bt_cellborder_diagonalup,
            this.bt_cellborder_diagonaldown,
            this.bt_cellborder_inside,
            this.bt_cellborder_insidevertical,
            this.bt_cellborder_insidehorizontal,
            this.ToolStripSeparator22,
            this.bt_cellborder_thin,
            this.bt_cellborder_thick,
            this.bt_cellborder_dot,
            this.ToolStripSeparator23,
            this.bt_cellborder_color});
            this.bt_cellborder.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder.Image")));
            this.bt_cellborder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_cellborder.Name = "bt_cellborder";
            this.bt_cellborder.Size = new System.Drawing.Size(32, 22);
            this.bt_cellborder.ButtonClick += new System.EventHandler(this.bt_cellborder_ButtonClick);
            // 
            // bt_cellborder_none
            // 
            this.bt_cellborder_none.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_none.Image")));
            this.bt_cellborder_none.Name = "bt_cellborder_none";
            this.bt_cellborder_none.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_none.Text = "None";
            this.bt_cellborder_none.Click += new System.EventHandler(this.bt_cellborder_none_click);
            // 
            // bt_cellborder_around
            // 
            this.bt_cellborder_around.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_around.Image")));
            this.bt_cellborder_around.Name = "bt_cellborder_around";
            this.bt_cellborder_around.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_around.Text = "flexEdgeAround";
            this.bt_cellborder_around.Click += new System.EventHandler(this.bt_cellborder_around_click);
            // 
            // bt_cellborder_left
            // 
            this.bt_cellborder_left.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_left.Image")));
            this.bt_cellborder_left.Name = "bt_cellborder_left";
            this.bt_cellborder_left.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_left.Text = "flexEdgeLeft";
            this.bt_cellborder_left.Click += new System.EventHandler(this.bt_cellborder_left_click);
            // 
            // bt_cellborder_top
            // 
            this.bt_cellborder_top.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_top.Image")));
            this.bt_cellborder_top.Name = "bt_cellborder_top";
            this.bt_cellborder_top.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_top.Text = "flexEdgeTop";
            this.bt_cellborder_top.Click += new System.EventHandler(this.bt_cellborder_top_click);
            // 
            // bt_cellborder_right
            // 
            this.bt_cellborder_right.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_right.Image")));
            this.bt_cellborder_right.Name = "bt_cellborder_right";
            this.bt_cellborder_right.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_right.Text = "flexEdgeRight";
            this.bt_cellborder_right.Click += new System.EventHandler(this.bt_cellborder_right_click);
            // 
            // bt_cellborder_bottom
            // 
            this.bt_cellborder_bottom.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_bottom.Image")));
            this.bt_cellborder_bottom.Name = "bt_cellborder_bottom";
            this.bt_cellborder_bottom.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_bottom.Text = "flexEdgeBottom";
            this.bt_cellborder_bottom.Click += new System.EventHandler(this.bt_cellborder_bottom_click);
            // 
            // bt_cellborder_diagonalup
            // 
            this.bt_cellborder_diagonalup.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_diagonalup.Image")));
            this.bt_cellborder_diagonalup.Name = "bt_cellborder_diagonalup";
            this.bt_cellborder_diagonalup.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_diagonalup.Text = "flexDiagonalUp";
            this.bt_cellborder_diagonalup.Click += new System.EventHandler(this.bt_cellborder_diagonalup_click);
            // 
            // bt_cellborder_diagonaldown
            // 
            this.bt_cellborder_diagonaldown.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_diagonaldown.Image")));
            this.bt_cellborder_diagonaldown.Name = "bt_cellborder_diagonaldown";
            this.bt_cellborder_diagonaldown.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_diagonaldown.Text = "flexDiagonalDown";
            this.bt_cellborder_diagonaldown.Click += new System.EventHandler(this.bt_cellborder_diagonaldown_click);
            // 
            // bt_cellborder_inside
            // 
            this.bt_cellborder_inside.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_inside.Image")));
            this.bt_cellborder_inside.Name = "bt_cellborder_inside";
            this.bt_cellborder_inside.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_inside.Text = "flexInside";
            this.bt_cellborder_inside.Click += new System.EventHandler(this.bt_cellborder_inside_click);
            // 
            // bt_cellborder_insidevertical
            // 
            this.bt_cellborder_insidevertical.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_insidevertical.Image")));
            this.bt_cellborder_insidevertical.Name = "bt_cellborder_insidevertical";
            this.bt_cellborder_insidevertical.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_insidevertical.Text = "flexInsideVertical";
            this.bt_cellborder_insidevertical.Click += new System.EventHandler(this.bt_cellborder_insidevetical_click);
            // 
            // bt_cellborder_insidehorizontal
            // 
            this.bt_cellborder_insidehorizontal.Image = ((System.Drawing.Image)(resources.GetObject("bt_cellborder_insidehorizontal.Image")));
            this.bt_cellborder_insidehorizontal.Name = "bt_cellborder_insidehorizontal";
            this.bt_cellborder_insidehorizontal.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_insidehorizontal.Text = "flexInsideHorizontal";
            this.bt_cellborder_insidehorizontal.Click += new System.EventHandler(this.bt_cellborder_insidehorizontal_click);
            // 
            // ToolStripSeparator22
            // 
            this.ToolStripSeparator22.Name = "ToolStripSeparator22";
            this.ToolStripSeparator22.Size = new System.Drawing.Size(175, 6);
            // 
            // bt_cellborder_thin
            // 
            this.bt_cellborder_thin.Checked = true;
            this.bt_cellborder_thin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bt_cellborder_thin.Name = "bt_cellborder_thin";
            this.bt_cellborder_thin.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_thin.Text = "flexLineStyleThin";
            this.bt_cellborder_thin.Click += new System.EventHandler(this.bt_cellborder_thin_click);
            // 
            // bt_cellborder_thick
            // 
            this.bt_cellborder_thick.Name = "bt_cellborder_thick";
            this.bt_cellborder_thick.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_thick.Text = "flexLineStyleThick";
            this.bt_cellborder_thick.Click += new System.EventHandler(this.bt_cellborder_thick_click);
            // 
            // bt_cellborder_dot
            // 
            this.bt_cellborder_dot.Name = "bt_cellborder_dot";
            this.bt_cellborder_dot.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_dot.Text = "flexLineStyleDot";
            this.bt_cellborder_dot.Click += new System.EventHandler(this.bt_cellborder_dot_click);
            // 
            // ToolStripSeparator23
            // 
            this.ToolStripSeparator23.Name = "ToolStripSeparator23";
            this.ToolStripSeparator23.Size = new System.Drawing.Size(175, 6);
            // 
            // bt_cellborder_color
            // 
            this.bt_cellborder_color.Name = "bt_cellborder_color";
            this.bt_cellborder_color.Size = new System.Drawing.Size(178, 22);
            this.bt_cellborder_color.Text = "Cell Border Color";
            this.bt_cellborder_color.Click += new System.EventHandler(this.bt_cellborder_color_click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripStatusLabel1});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 759);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(976, 22);
            this.StatusStrip1.TabIndex = 5;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(961, 17);
            this.ToolStripStatusLabel1.Spring = true;
            this.ToolStripStatusLabel1.Text = "Ready";
            this.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabProperty
            // 
            this.tabProperty.Controls.Add(this.TabPage1);
            this.tabProperty.Controls.Add(this.TabPage2);
            this.tabProperty.Controls.Add(this.TabPage3);
            this.tabProperty.Location = new System.Drawing.Point(635, 83);
            this.tabProperty.Name = "tabProperty";
            this.tabProperty.SelectedIndex = 0;
            this.tabProperty.Size = new System.Drawing.Size(329, 647);
            this.tabProperty.TabIndex = 6;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.cboApplySelectionToImage);
            this.TabPage1.Controls.Add(this.cboShowContextMenu);
            this.TabPage1.Controls.Add(this.cboShowComboButton);
            this.TabPage1.Controls.Add(this.cboPictureOver);
            this.TabPage1.Controls.Add(this.cboScrollBarStyle);
            this.TabPage1.Controls.Add(this.cboScrollBars);
            this.TabPage1.Controls.Add(this.cboFocusRect);
            this.TabPage1.Controls.Add(this.cboHighLight);
            this.TabPage1.Controls.Add(this.cboSelectionMode);
            this.TabPage1.Controls.Add(this.cboExtendLastCol);
            this.TabPage1.Controls.Add(this.cboEnterKeyBehavior);
            this.TabPage1.Controls.Add(this.cboEllipsis);
            this.TabPage1.Controls.Add(this.cboEditable);
            this.TabPage1.Controls.Add(this.cboButtonLocked);
            this.TabPage1.Controls.Add(this.cboDateFormat);
            this.TabPage1.Controls.Add(this.cboGridLines);
            this.TabPage1.Controls.Add(this.cboBorderStyle);
            this.TabPage1.Controls.Add(this.cboAllowUserSort);
            this.TabPage1.Controls.Add(this.cboAllowUserResizing);
            this.TabPage1.Controls.Add(this.cboAllowUserReorder);
            this.TabPage1.Controls.Add(this.cboAllowSelection);
            this.TabPage1.Controls.Add(this.cboAllowBigSelection);
            this.TabPage1.Controls.Add(this.Label22);
            this.TabPage1.Controls.Add(this.Label21);
            this.TabPage1.Controls.Add(this.Label20);
            this.TabPage1.Controls.Add(this.Label19);
            this.TabPage1.Controls.Add(this.Label18);
            this.TabPage1.Controls.Add(this.Label17);
            this.TabPage1.Controls.Add(this.Label16);
            this.TabPage1.Controls.Add(this.Label15);
            this.TabPage1.Controls.Add(this.Label14);
            this.TabPage1.Controls.Add(this.Label13);
            this.TabPage1.Controls.Add(this.Label12);
            this.TabPage1.Controls.Add(this.Label11);
            this.TabPage1.Controls.Add(this.Label10);
            this.TabPage1.Controls.Add(this.Label9);
            this.TabPage1.Controls.Add(this.Label8);
            this.TabPage1.Controls.Add(this.Label7);
            this.TabPage1.Controls.Add(this.Label6);
            this.TabPage1.Controls.Add(this.Label5);
            this.TabPage1.Controls.Add(this.Label4);
            this.TabPage1.Controls.Add(this.Label3);
            this.TabPage1.Controls.Add(this.Label2);
            this.TabPage1.Controls.Add(this.Label1);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(321, 621);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Page1";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // cboApplySelectionToImage
            // 
            this.cboApplySelectionToImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboApplySelectionToImage.FormattingEnabled = true;
            this.cboApplySelectionToImage.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboApplySelectionToImage.Location = new System.Drawing.Point(142, 582);
            this.cboApplySelectionToImage.Name = "cboApplySelectionToImage";
            this.cboApplySelectionToImage.Size = new System.Drawing.Size(168, 21);
            this.cboApplySelectionToImage.TabIndex = 43;
            this.cboApplySelectionToImage.SelectedIndexChanged += new System.EventHandler(this.cboApplySelectionToImage_SelectedIndexChanged);
            // 
            // cboShowContextMenu
            // 
            this.cboShowContextMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShowContextMenu.FormattingEnabled = true;
            this.cboShowContextMenu.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboShowContextMenu.Location = new System.Drawing.Point(142, 555);
            this.cboShowContextMenu.Name = "cboShowContextMenu";
            this.cboShowContextMenu.Size = new System.Drawing.Size(168, 21);
            this.cboShowContextMenu.TabIndex = 42;
            this.cboShowContextMenu.SelectedIndexChanged += new System.EventHandler(this.cboShowContextMenu_SelectedIndexChanged);
            // 
            // cboShowComboButton
            // 
            this.cboShowComboButton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShowComboButton.FormattingEnabled = true;
            this.cboShowComboButton.Items.AddRange(new object[] {
            "0-Focus",
            "1-Editing",
            "2-MouseMove"});
            this.cboShowComboButton.Location = new System.Drawing.Point(142, 528);
            this.cboShowComboButton.Name = "cboShowComboButton";
            this.cboShowComboButton.Size = new System.Drawing.Size(168, 21);
            this.cboShowComboButton.TabIndex = 41;
            this.cboShowComboButton.SelectedIndexChanged += new System.EventHandler(this.cboShowComboButton_SelectedIndexChanged);
            // 
            // cboPictureOver
            // 
            this.cboPictureOver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPictureOver.FormattingEnabled = true;
            this.cboPictureOver.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboPictureOver.Location = new System.Drawing.Point(142, 501);
            this.cboPictureOver.Name = "cboPictureOver";
            this.cboPictureOver.Size = new System.Drawing.Size(168, 21);
            this.cboPictureOver.TabIndex = 40;
            this.cboPictureOver.SelectedIndexChanged += new System.EventHandler(this.cboPictureOver_SelectedIndexChanged);
            // 
            // cboScrollBarStyle
            // 
            this.cboScrollBarStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScrollBarStyle.FormattingEnabled = true;
            this.cboScrollBarStyle.Items.AddRange(new object[] {
            "0-Classic",
            "1-Flat",
            "2-Themed"});
            this.cboScrollBarStyle.Location = new System.Drawing.Point(142, 473);
            this.cboScrollBarStyle.Name = "cboScrollBarStyle";
            this.cboScrollBarStyle.Size = new System.Drawing.Size(168, 21);
            this.cboScrollBarStyle.TabIndex = 39;
            this.cboScrollBarStyle.SelectedIndexChanged += new System.EventHandler(this.cboScrollBarStyle_SelectedIndexChanged);
            // 
            // cboScrollBars
            // 
            this.cboScrollBars.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScrollBars.FormattingEnabled = true;
            this.cboScrollBars.Items.AddRange(new object[] {
            "0-None",
            "1-Horizontal",
            "2-Vertical",
            "3-Both"});
            this.cboScrollBars.Location = new System.Drawing.Point(142, 446);
            this.cboScrollBars.Name = "cboScrollBars";
            this.cboScrollBars.Size = new System.Drawing.Size(168, 21);
            this.cboScrollBars.TabIndex = 38;
            this.cboScrollBars.SelectedIndexChanged += new System.EventHandler(this.cboScrollBars_SelectedIndexChanged);
            // 
            // cboFocusRect
            // 
            this.cboFocusRect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFocusRect.FormattingEnabled = true;
            this.cboFocusRect.Items.AddRange(new object[] {
            "0-None",
            "1-ByCell",
            "2-ByRow"});
            this.cboFocusRect.Location = new System.Drawing.Point(142, 419);
            this.cboFocusRect.Name = "cboFocusRect";
            this.cboFocusRect.Size = new System.Drawing.Size(168, 21);
            this.cboFocusRect.TabIndex = 37;
            this.cboFocusRect.SelectedIndexChanged += new System.EventHandler(this.cboFocusRect_SelectedIndexChanged);
            // 
            // cboHighLight
            // 
            this.cboHighLight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHighLight.FormattingEnabled = true;
            this.cboHighLight.Items.AddRange(new object[] {
            "0-Never",
            "1-Always"});
            this.cboHighLight.Location = new System.Drawing.Point(142, 392);
            this.cboHighLight.Name = "cboHighLight";
            this.cboHighLight.Size = new System.Drawing.Size(168, 21);
            this.cboHighLight.TabIndex = 36;
            this.cboHighLight.SelectedIndexChanged += new System.EventHandler(this.cboHighLight_SelectedIndexChanged);
            // 
            // cboSelectionMode
            // 
            this.cboSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectionMode.FormattingEnabled = true;
            this.cboSelectionMode.Items.AddRange(new object[] {
            "0-Free",
            "1-ByRow",
            "2-ByColumn",
            "3-ListBox"});
            this.cboSelectionMode.Location = new System.Drawing.Point(142, 365);
            this.cboSelectionMode.Name = "cboSelectionMode";
            this.cboSelectionMode.Size = new System.Drawing.Size(168, 21);
            this.cboSelectionMode.TabIndex = 35;
            this.cboSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cboSelectionMode_SelectedIndexChanged);
            // 
            // cboExtendLastCol
            // 
            this.cboExtendLastCol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExtendLastCol.FormattingEnabled = true;
            this.cboExtendLastCol.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboExtendLastCol.Location = new System.Drawing.Point(142, 338);
            this.cboExtendLastCol.Name = "cboExtendLastCol";
            this.cboExtendLastCol.Size = new System.Drawing.Size(168, 21);
            this.cboExtendLastCol.TabIndex = 34;
            this.cboExtendLastCol.SelectedIndexChanged += new System.EventHandler(this.CboExtendLastCol_SelectedIndexChanged);
            // 
            // cboEnterKeyBehavior
            // 
            this.cboEnterKeyBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnterKeyBehavior.FormattingEnabled = true;
            this.cboEnterKeyBehavior.Items.AddRange(new object[] {
            "0-NextCol",
            "1-NextRow",
            "2-EnterKeyDefault"});
            this.cboEnterKeyBehavior.Location = new System.Drawing.Point(142, 311);
            this.cboEnterKeyBehavior.Name = "cboEnterKeyBehavior";
            this.cboEnterKeyBehavior.Size = new System.Drawing.Size(168, 21);
            this.cboEnterKeyBehavior.TabIndex = 33;
            this.cboEnterKeyBehavior.SelectedIndexChanged += new System.EventHandler(this.CboEnterKeyBehavior_SelectedIndexChanged);
            // 
            // cboEllipsis
            // 
            this.cboEllipsis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEllipsis.FormattingEnabled = true;
            this.cboEllipsis.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboEllipsis.Location = new System.Drawing.Point(142, 284);
            this.cboEllipsis.Name = "cboEllipsis";
            this.cboEllipsis.Size = new System.Drawing.Size(168, 21);
            this.cboEllipsis.TabIndex = 32;
            this.cboEllipsis.SelectedIndexChanged += new System.EventHandler(this.CboEllipsis_SelectedIndexChanged);
            // 
            // cboEditable
            // 
            this.cboEditable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEditable.FormattingEnabled = true;
            this.cboEditable.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboEditable.Location = new System.Drawing.Point(142, 257);
            this.cboEditable.Name = "cboEditable";
            this.cboEditable.Size = new System.Drawing.Size(168, 21);
            this.cboEditable.TabIndex = 31;
            this.cboEditable.SelectedIndexChanged += new System.EventHandler(this.CboEditable_SelectedIndexChanged);
            // 
            // cboButtonLocked
            // 
            this.cboButtonLocked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboButtonLocked.FormattingEnabled = true;
            this.cboButtonLocked.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboButtonLocked.Location = new System.Drawing.Point(142, 230);
            this.cboButtonLocked.Name = "cboButtonLocked";
            this.cboButtonLocked.Size = new System.Drawing.Size(168, 21);
            this.cboButtonLocked.TabIndex = 30;
            this.cboButtonLocked.SelectedIndexChanged += new System.EventHandler(this.CboButtonLocked_SelectedIndexChanged);
            // 
            // cboDateFormat
            // 
            this.cboDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDateFormat.FormattingEnabled = true;
            this.cboDateFormat.Items.AddRange(new object[] {
            "0-YMD",
            "1-MDY",
            "2-DMY"});
            this.cboDateFormat.Location = new System.Drawing.Point(142, 203);
            this.cboDateFormat.Name = "cboDateFormat";
            this.cboDateFormat.Size = new System.Drawing.Size(168, 21);
            this.cboDateFormat.TabIndex = 29;
            this.cboDateFormat.SelectedIndexChanged += new System.EventHandler(this.CboDateFormat_SelectedIndexChanged);
            // 
            // cboGridLines
            // 
            this.cboGridLines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGridLines.FormattingEnabled = true;
            this.cboGridLines.Items.AddRange(new object[] {
            "0-None",
            "1-Horizontal",
            "2-Vertical",
            "3-Both"});
            this.cboGridLines.Location = new System.Drawing.Point(142, 176);
            this.cboGridLines.Name = "cboGridLines";
            this.cboGridLines.Size = new System.Drawing.Size(168, 21);
            this.cboGridLines.TabIndex = 28;
            this.cboGridLines.SelectedIndexChanged += new System.EventHandler(this.cboGridLines_SelectedIndexChanged);
            // 
            // cboBorderStyle
            // 
            this.cboBorderStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBorderStyle.FormattingEnabled = true;
            this.cboBorderStyle.Items.AddRange(new object[] {
            "0-None",
            "1-FixedSingle",
            "2-Fixed3D"});
            this.cboBorderStyle.Location = new System.Drawing.Point(142, 148);
            this.cboBorderStyle.Name = "cboBorderStyle";
            this.cboBorderStyle.Size = new System.Drawing.Size(168, 21);
            this.cboBorderStyle.TabIndex = 27;
            this.cboBorderStyle.SelectedIndexChanged += new System.EventHandler(this.CboBorderStyle_SelectedIndexChanged);
            // 
            // cboAllowUserSort
            // 
            this.cboAllowUserSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllowUserSort.FormattingEnabled = true;
            this.cboAllowUserSort.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboAllowUserSort.Location = new System.Drawing.Point(142, 121);
            this.cboAllowUserSort.Name = "cboAllowUserSort";
            this.cboAllowUserSort.Size = new System.Drawing.Size(168, 21);
            this.cboAllowUserSort.TabIndex = 26;
            this.cboAllowUserSort.SelectedIndexChanged += new System.EventHandler(this.CboAllowUserSort_SelectedIndexChanged);
            // 
            // cboAllowUserResizing
            // 
            this.cboAllowUserResizing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllowUserResizing.FormattingEnabled = true;
            this.cboAllowUserResizing.Items.AddRange(new object[] {
            "0-None",
            "1-Columns",
            "2-Rows",
            "3-Both",
            "4-BothUniform"});
            this.cboAllowUserResizing.Location = new System.Drawing.Point(142, 94);
            this.cboAllowUserResizing.Name = "cboAllowUserResizing";
            this.cboAllowUserResizing.Size = new System.Drawing.Size(168, 21);
            this.cboAllowUserResizing.TabIndex = 25;
            this.cboAllowUserResizing.SelectedIndexChanged += new System.EventHandler(this.CboAllowUserResizing_SelectedIndexChanged);
            // 
            // cboAllowUserReorder
            // 
            this.cboAllowUserReorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllowUserReorder.FormattingEnabled = true;
            this.cboAllowUserReorder.Items.AddRange(new object[] {
            "0-None",
            "1-Rows",
            "2-Columns",
            "3-Both"});
            this.cboAllowUserReorder.Location = new System.Drawing.Point(142, 67);
            this.cboAllowUserReorder.Name = "cboAllowUserReorder";
            this.cboAllowUserReorder.Size = new System.Drawing.Size(168, 21);
            this.cboAllowUserReorder.TabIndex = 24;
            this.cboAllowUserReorder.SelectedIndexChanged += new System.EventHandler(this.CboAllowUserReorder_SelectedIndexChanged);
            // 
            // cboAllowSelection
            // 
            this.cboAllowSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllowSelection.FormattingEnabled = true;
            this.cboAllowSelection.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboAllowSelection.Location = new System.Drawing.Point(142, 40);
            this.cboAllowSelection.Name = "cboAllowSelection";
            this.cboAllowSelection.Size = new System.Drawing.Size(168, 21);
            this.cboAllowSelection.TabIndex = 23;
            this.cboAllowSelection.SelectedIndexChanged += new System.EventHandler(this.CboAllowSelection_SelectedIndexChanged);
            // 
            // cboAllowBigSelection
            // 
            this.cboAllowBigSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllowBigSelection.FormattingEnabled = true;
            this.cboAllowBigSelection.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboAllowBigSelection.Location = new System.Drawing.Point(142, 13);
            this.cboAllowBigSelection.Name = "cboAllowBigSelection";
            this.cboAllowBigSelection.Size = new System.Drawing.Size(168, 21);
            this.cboAllowBigSelection.TabIndex = 22;
            this.cboAllowBigSelection.SelectedIndexChanged += new System.EventHandler(this.CboAllowBigSelection_SelectedIndexChanged);
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.Location = new System.Drawing.Point(11, 586);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(119, 13);
            this.Label22.TabIndex = 21;
            this.Label22.Text = "ApplySelectionToImage";
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(11, 559);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(97, 13);
            this.Label21.TabIndex = 20;
            this.Label21.Text = "ShowContextMenu";
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Location = new System.Drawing.Point(11, 532);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(98, 13);
            this.Label20.TabIndex = 19;
            this.Label20.Text = "ShowComboButton";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(11, 505);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(63, 13);
            this.Label19.TabIndex = 18;
            this.Label19.Text = "PictureOver";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(11, 478);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(72, 13);
            this.Label18.TabIndex = 17;
            this.Label18.Text = "ScrollBarStyle";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(11, 451);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(54, 13);
            this.Label17.TabIndex = 16;
            this.Label17.Text = "ScrollBars";
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(11, 424);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(59, 13);
            this.Label16.TabIndex = 15;
            this.Label16.Text = "FocusRect";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(11, 397);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(52, 13);
            this.Label15.TabIndex = 14;
            this.Label15.Text = "HighLight";
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(11, 369);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(78, 13);
            this.Label14.TabIndex = 13;
            this.Label14.Text = "SelectionMode";
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(11, 342);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(75, 13);
            this.Label13.TabIndex = 12;
            this.Label13.Text = "ExtendLastCol";
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(11, 315);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(92, 13);
            this.Label12.TabIndex = 11;
            this.Label12.Text = "EnterKeyBehavior";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(11, 288);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(38, 13);
            this.Label11.TabIndex = 10;
            this.Label11.Text = "Ellipsis";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(11, 261);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(45, 13);
            this.Label10.TabIndex = 9;
            this.Label10.Text = "Editable";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(11, 234);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(74, 13);
            this.Label9.TabIndex = 8;
            this.Label9.Text = "ButtonLocked";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(11, 207);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(62, 13);
            this.Label8.TabIndex = 7;
            this.Label8.Text = "DateFormat";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(11, 180);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(51, 13);
            this.Label7.TabIndex = 6;
            this.Label7.Text = "GridLines";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(11, 153);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(61, 13);
            this.Label6.TabIndex = 5;
            this.Label6.Text = "BorderStyle";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(11, 126);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(73, 13);
            this.Label5.TabIndex = 4;
            this.Label5.Text = "AllowUserSort";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(11, 99);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(94, 13);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "AllowUserResizing";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(11, 72);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(92, 13);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "AllowUserReorder";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(11, 44);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(76, 13);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "AllowSelection";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(11, 17);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(91, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "AllowBigSelection";
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.cboSheetBorder);
            this.TabPage2.Controls.Add(this.picSheetBorderColor);
            this.TabPage2.Controls.Add(this.picGridColor);
            this.TabPage2.Controls.Add(this.picForeColorSel);
            this.TabPage2.Controls.Add(this.picForeColorFrozen);
            this.TabPage2.Controls.Add(this.picForeColorFixed);
            this.TabPage2.Controls.Add(this.picForeColor);
            this.TabPage2.Controls.Add(this.picBackColorSel);
            this.TabPage2.Controls.Add(this.picBackColorFrozen);
            this.TabPage2.Controls.Add(this.picBackColorBkg);
            this.TabPage2.Controls.Add(this.picBackColorAlternate);
            this.TabPage2.Controls.Add(this.picBackColor);
            this.TabPage2.Controls.Add(this.btnFont);
            this.TabPage2.Controls.Add(this.txtDefaultRowHeight);
            this.TabPage2.Controls.Add(this.txtDefaultColWidth);
            this.TabPage2.Controls.Add(this.txtFrozenCols);
            this.TabPage2.Controls.Add(this.txtFrozenRows);
            this.TabPage2.Controls.Add(this.txtFixedCols);
            this.TabPage2.Controls.Add(this.txtFixedRows);
            this.TabPage2.Controls.Add(this.txtColCount);
            this.TabPage2.Controls.Add(this.cboUseBackColorAlternate);
            this.TabPage2.Controls.Add(this.txtRowCount);
            this.TabPage2.Controls.Add(this.Label23);
            this.TabPage2.Controls.Add(this.Label24);
            this.TabPage2.Controls.Add(this.Label25);
            this.TabPage2.Controls.Add(this.Label26);
            this.TabPage2.Controls.Add(this.Label27);
            this.TabPage2.Controls.Add(this.Label28);
            this.TabPage2.Controls.Add(this.Label29);
            this.TabPage2.Controls.Add(this.Label30);
            this.TabPage2.Controls.Add(this.Label31);
            this.TabPage2.Controls.Add(this.Label32);
            this.TabPage2.Controls.Add(this.Label33);
            this.TabPage2.Controls.Add(this.Label34);
            this.TabPage2.Controls.Add(this.Label35);
            this.TabPage2.Controls.Add(this.Label36);
            this.TabPage2.Controls.Add(this.Label37);
            this.TabPage2.Controls.Add(this.Label38);
            this.TabPage2.Controls.Add(this.Label39);
            this.TabPage2.Controls.Add(this.Label40);
            this.TabPage2.Controls.Add(this.Label41);
            this.TabPage2.Controls.Add(this.Label42);
            this.TabPage2.Controls.Add(this.Label43);
            this.TabPage2.Controls.Add(this.Label44);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(321, 621);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Page2";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // cboSheetBorder
            // 
            this.cboSheetBorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSheetBorder.FormattingEnabled = true;
            this.cboSheetBorder.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboSheetBorder.Location = new System.Drawing.Point(142, 555);
            this.cboSheetBorder.Name = "cboSheetBorder";
            this.cboSheetBorder.Size = new System.Drawing.Size(168, 21);
            this.cboSheetBorder.TabIndex = 65;
            this.cboSheetBorder.SelectedIndexChanged += new System.EventHandler(this.cboSheetBorder_SelectedIndexChanged);
            // 
            // picSheetBorderColor
            // 
            this.picSheetBorderColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picSheetBorderColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSheetBorderColor.Location = new System.Drawing.Point(142, 582);
            this.picSheetBorderColor.Name = "picSheetBorderColor";
            this.picSheetBorderColor.Size = new System.Drawing.Size(34, 23);
            this.picSheetBorderColor.TabIndex = 64;
            this.picSheetBorderColor.TabStop = false;
            this.picSheetBorderColor.Click += new System.EventHandler(this.picSheetBorderColor_Click);
            // 
            // picGridColor
            // 
            this.picGridColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picGridColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGridColor.Location = new System.Drawing.Point(142, 528);
            this.picGridColor.Name = "picGridColor";
            this.picGridColor.Size = new System.Drawing.Size(34, 23);
            this.picGridColor.TabIndex = 63;
            this.picGridColor.TabStop = false;
            this.picGridColor.Click += new System.EventHandler(this.picGridColor_Click);
            // 
            // picForeColorSel
            // 
            this.picForeColorSel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picForeColorSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picForeColorSel.Location = new System.Drawing.Point(142, 501);
            this.picForeColorSel.Name = "picForeColorSel";
            this.picForeColorSel.Size = new System.Drawing.Size(34, 23);
            this.picForeColorSel.TabIndex = 62;
            this.picForeColorSel.TabStop = false;
            this.picForeColorSel.Click += new System.EventHandler(this.picForeColorSel_Click);
            // 
            // picForeColorFrozen
            // 
            this.picForeColorFrozen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picForeColorFrozen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picForeColorFrozen.Location = new System.Drawing.Point(142, 473);
            this.picForeColorFrozen.Name = "picForeColorFrozen";
            this.picForeColorFrozen.Size = new System.Drawing.Size(34, 23);
            this.picForeColorFrozen.TabIndex = 61;
            this.picForeColorFrozen.TabStop = false;
            this.picForeColorFrozen.Click += new System.EventHandler(this.picForeColorFrozen_Click);
            // 
            // picForeColorFixed
            // 
            this.picForeColorFixed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picForeColorFixed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picForeColorFixed.Location = new System.Drawing.Point(142, 446);
            this.picForeColorFixed.Name = "picForeColorFixed";
            this.picForeColorFixed.Size = new System.Drawing.Size(34, 23);
            this.picForeColorFixed.TabIndex = 60;
            this.picForeColorFixed.TabStop = false;
            this.picForeColorFixed.Click += new System.EventHandler(this.picForeColorFixed_Click);
            // 
            // picForeColor
            // 
            this.picForeColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picForeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picForeColor.Location = new System.Drawing.Point(142, 419);
            this.picForeColor.Name = "picForeColor";
            this.picForeColor.Size = new System.Drawing.Size(34, 23);
            this.picForeColor.TabIndex = 59;
            this.picForeColor.TabStop = false;
            this.picForeColor.Click += new System.EventHandler(this.picForeColor_Click);
            // 
            // picBackColorSel
            // 
            this.picBackColorSel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picBackColorSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBackColorSel.Location = new System.Drawing.Point(142, 365);
            this.picBackColorSel.Name = "picBackColorSel";
            this.picBackColorSel.Size = new System.Drawing.Size(34, 23);
            this.picBackColorSel.TabIndex = 58;
            this.picBackColorSel.TabStop = false;
            this.picBackColorSel.Click += new System.EventHandler(this.picBackColorSel_Click);
            // 
            // picBackColorFrozen
            // 
            this.picBackColorFrozen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picBackColorFrozen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBackColorFrozen.Location = new System.Drawing.Point(142, 338);
            this.picBackColorFrozen.Name = "picBackColorFrozen";
            this.picBackColorFrozen.Size = new System.Drawing.Size(34, 23);
            this.picBackColorFrozen.TabIndex = 57;
            this.picBackColorFrozen.TabStop = false;
            this.picBackColorFrozen.Click += new System.EventHandler(this.picBackColorFrozen_Click);
            // 
            // picBackColorBkg
            // 
            this.picBackColorBkg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picBackColorBkg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBackColorBkg.Location = new System.Drawing.Point(142, 311);
            this.picBackColorBkg.Name = "picBackColorBkg";
            this.picBackColorBkg.Size = new System.Drawing.Size(34, 23);
            this.picBackColorBkg.TabIndex = 56;
            this.picBackColorBkg.TabStop = false;
            this.picBackColorBkg.Click += new System.EventHandler(this.picBackColorBkg_Click);
            // 
            // picBackColorAlternate
            // 
            this.picBackColorAlternate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picBackColorAlternate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBackColorAlternate.Location = new System.Drawing.Point(142, 284);
            this.picBackColorAlternate.Name = "picBackColorAlternate";
            this.picBackColorAlternate.Size = new System.Drawing.Size(34, 23);
            this.picBackColorAlternate.TabIndex = 55;
            this.picBackColorAlternate.TabStop = false;
            this.picBackColorAlternate.Click += new System.EventHandler(this.picBackColorAlternate_Click);
            // 
            // picBackColor
            // 
            this.picBackColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBackColor.Location = new System.Drawing.Point(142, 257);
            this.picBackColor.Name = "picBackColor";
            this.picBackColor.Size = new System.Drawing.Size(34, 23);
            this.picBackColor.TabIndex = 54;
            this.picBackColor.TabStop = false;
            this.picBackColor.Click += new System.EventHandler(this.picBackColor_Click);
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(142, 230);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(34, 23);
            this.btnFont.TabIndex = 53;
            this.btnFont.Text = "...";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // txtDefaultRowHeight
            // 
            this.txtDefaultRowHeight.Location = new System.Drawing.Point(142, 203);
            this.txtDefaultRowHeight.Name = "txtDefaultRowHeight";
            this.txtDefaultRowHeight.Size = new System.Drawing.Size(168, 20);
            this.txtDefaultRowHeight.TabIndex = 52;
            this.txtDefaultRowHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDefaultRowHeight_KeyPress);
            // 
            // txtDefaultColWidth
            // 
            this.txtDefaultColWidth.Location = new System.Drawing.Point(142, 176);
            this.txtDefaultColWidth.Name = "txtDefaultColWidth";
            this.txtDefaultColWidth.Size = new System.Drawing.Size(168, 20);
            this.txtDefaultColWidth.TabIndex = 51;
            this.txtDefaultColWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDefaultColWidth_KeyPress);
            // 
            // txtFrozenCols
            // 
            this.txtFrozenCols.Location = new System.Drawing.Point(142, 148);
            this.txtFrozenCols.Name = "txtFrozenCols";
            this.txtFrozenCols.Size = new System.Drawing.Size(168, 20);
            this.txtFrozenCols.TabIndex = 50;
            this.txtFrozenCols.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrozenCols_KeyPress);
            // 
            // txtFrozenRows
            // 
            this.txtFrozenRows.Location = new System.Drawing.Point(142, 121);
            this.txtFrozenRows.Name = "txtFrozenRows";
            this.txtFrozenRows.Size = new System.Drawing.Size(168, 20);
            this.txtFrozenRows.TabIndex = 49;
            this.txtFrozenRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrozenRows_KeyPress);
            // 
            // txtFixedCols
            // 
            this.txtFixedCols.Location = new System.Drawing.Point(142, 94);
            this.txtFixedCols.Name = "txtFixedCols";
            this.txtFixedCols.Size = new System.Drawing.Size(168, 20);
            this.txtFixedCols.TabIndex = 48;
            this.txtFixedCols.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFixedCols_KeyPress);
            // 
            // txtFixedRows
            // 
            this.txtFixedRows.Location = new System.Drawing.Point(142, 67);
            this.txtFixedRows.Name = "txtFixedRows";
            this.txtFixedRows.Size = new System.Drawing.Size(168, 20);
            this.txtFixedRows.TabIndex = 47;
            this.txtFixedRows.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFixedRows_KeyPress);
            // 
            // txtColCount
            // 
            this.txtColCount.Location = new System.Drawing.Point(142, 40);
            this.txtColCount.Name = "txtColCount";
            this.txtColCount.Size = new System.Drawing.Size(168, 20);
            this.txtColCount.TabIndex = 46;
            this.txtColCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtColCount_KeyPress);
            // 
            // cboUseBackColorAlternate
            // 
            this.cboUseBackColorAlternate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUseBackColorAlternate.FormattingEnabled = true;
            this.cboUseBackColorAlternate.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboUseBackColorAlternate.Location = new System.Drawing.Point(142, 393);
            this.cboUseBackColorAlternate.Name = "cboUseBackColorAlternate";
            this.cboUseBackColorAlternate.Size = new System.Drawing.Size(168, 21);
            this.cboUseBackColorAlternate.TabIndex = 45;
            this.cboUseBackColorAlternate.SelectedIndexChanged += new System.EventHandler(this.cboUseBackColorAlternate_SelectedIndexChanged);
            // 
            // txtRowCount
            // 
            this.txtRowCount.Location = new System.Drawing.Point(142, 13);
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.Size = new System.Drawing.Size(168, 20);
            this.txtRowCount.TabIndex = 44;
            this.txtRowCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtRowCount_KeyPress);
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.Location = new System.Drawing.Point(11, 586);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(90, 13);
            this.Label23.TabIndex = 43;
            this.Label23.Text = "SheetBorderColor";
            // 
            // Label24
            // 
            this.Label24.AutoSize = true;
            this.Label24.Location = new System.Drawing.Point(11, 559);
            this.Label24.Name = "Label24";
            this.Label24.Size = new System.Drawing.Size(66, 13);
            this.Label24.TabIndex = 42;
            this.Label24.Text = "SheetBorder";
            // 
            // Label25
            // 
            this.Label25.AutoSize = true;
            this.Label25.Location = new System.Drawing.Point(11, 532);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(50, 13);
            this.Label25.TabIndex = 41;
            this.Label25.Text = "GridColor";
            // 
            // Label26
            // 
            this.Label26.AutoSize = true;
            this.Label26.Location = new System.Drawing.Point(11, 505);
            this.Label26.Name = "Label26";
            this.Label26.Size = new System.Drawing.Size(67, 13);
            this.Label26.TabIndex = 40;
            this.Label26.Text = "ForeColorSel";
            // 
            // Label27
            // 
            this.Label27.AutoSize = true;
            this.Label27.Location = new System.Drawing.Point(11, 478);
            this.Label27.Name = "Label27";
            this.Label27.Size = new System.Drawing.Size(84, 13);
            this.Label27.TabIndex = 39;
            this.Label27.Text = "ForeColorFrozen";
            // 
            // Label28
            // 
            this.Label28.AutoSize = true;
            this.Label28.Location = new System.Drawing.Point(11, 451);
            this.Label28.Name = "Label28";
            this.Label28.Size = new System.Drawing.Size(77, 13);
            this.Label28.TabIndex = 38;
            this.Label28.Text = "ForeColorFixed";
            // 
            // Label29
            // 
            this.Label29.AutoSize = true;
            this.Label29.Location = new System.Drawing.Point(11, 424);
            this.Label29.Name = "Label29";
            this.Label29.Size = new System.Drawing.Size(52, 13);
            this.Label29.TabIndex = 37;
            this.Label29.Text = "ForeColor";
            // 
            // Label30
            // 
            this.Label30.AutoSize = true;
            this.Label30.Location = new System.Drawing.Point(11, 397);
            this.Label30.Name = "Label30";
            this.Label30.Size = new System.Drawing.Size(117, 13);
            this.Label30.TabIndex = 36;
            this.Label30.Text = "UseBackColorAlternate";
            // 
            // Label31
            // 
            this.Label31.AutoSize = true;
            this.Label31.Location = new System.Drawing.Point(11, 369);
            this.Label31.Name = "Label31";
            this.Label31.Size = new System.Drawing.Size(71, 13);
            this.Label31.TabIndex = 35;
            this.Label31.Text = "BackColorSel";
            // 
            // Label32
            // 
            this.Label32.AutoSize = true;
            this.Label32.Location = new System.Drawing.Point(11, 342);
            this.Label32.Name = "Label32";
            this.Label32.Size = new System.Drawing.Size(88, 13);
            this.Label32.TabIndex = 34;
            this.Label32.Text = "BackColorFrozen";
            // 
            // Label33
            // 
            this.Label33.AutoSize = true;
            this.Label33.Location = new System.Drawing.Point(11, 315);
            this.Label33.Name = "Label33";
            this.Label33.Size = new System.Drawing.Size(75, 13);
            this.Label33.TabIndex = 33;
            this.Label33.Text = "BackColorBkg";
            // 
            // Label34
            // 
            this.Label34.AutoSize = true;
            this.Label34.Location = new System.Drawing.Point(11, 288);
            this.Label34.Name = "Label34";
            this.Label34.Size = new System.Drawing.Size(98, 13);
            this.Label34.TabIndex = 32;
            this.Label34.Text = "BackColorAlternate";
            // 
            // Label35
            // 
            this.Label35.AutoSize = true;
            this.Label35.Location = new System.Drawing.Point(11, 261);
            this.Label35.Name = "Label35";
            this.Label35.Size = new System.Drawing.Size(56, 13);
            this.Label35.TabIndex = 31;
            this.Label35.Text = "BackColor";
            // 
            // Label36
            // 
            this.Label36.AutoSize = true;
            this.Label36.Location = new System.Drawing.Point(11, 234);
            this.Label36.Name = "Label36";
            this.Label36.Size = new System.Drawing.Size(28, 13);
            this.Label36.TabIndex = 30;
            this.Label36.Text = "Font";
            // 
            // Label37
            // 
            this.Label37.AutoSize = true;
            this.Label37.Location = new System.Drawing.Point(11, 207);
            this.Label37.Name = "Label37";
            this.Label37.Size = new System.Drawing.Size(94, 13);
            this.Label37.TabIndex = 29;
            this.Label37.Text = "DefaultRowHeight";
            // 
            // Label38
            // 
            this.Label38.AutoSize = true;
            this.Label38.Location = new System.Drawing.Point(11, 180);
            this.Label38.Name = "Label38";
            this.Label38.Size = new System.Drawing.Size(84, 13);
            this.Label38.TabIndex = 28;
            this.Label38.Text = "DefaultColWidth";
            // 
            // Label39
            // 
            this.Label39.AutoSize = true;
            this.Label39.Location = new System.Drawing.Point(11, 153);
            this.Label39.Name = "Label39";
            this.Label39.Size = new System.Drawing.Size(59, 13);
            this.Label39.TabIndex = 27;
            this.Label39.Text = "FrozenCols";
            // 
            // Label40
            // 
            this.Label40.AutoSize = true;
            this.Label40.Location = new System.Drawing.Point(11, 126);
            this.Label40.Name = "Label40";
            this.Label40.Size = new System.Drawing.Size(66, 13);
            this.Label40.TabIndex = 26;
            this.Label40.Text = "FrozenRows";
            // 
            // Label41
            // 
            this.Label41.AutoSize = true;
            this.Label41.Location = new System.Drawing.Point(11, 99);
            this.Label41.Name = "Label41";
            this.Label41.Size = new System.Drawing.Size(52, 13);
            this.Label41.TabIndex = 25;
            this.Label41.Text = "FixedCols";
            // 
            // Label42
            // 
            this.Label42.AutoSize = true;
            this.Label42.Location = new System.Drawing.Point(11, 72);
            this.Label42.Name = "Label42";
            this.Label42.Size = new System.Drawing.Size(59, 13);
            this.Label42.TabIndex = 24;
            this.Label42.Text = "FixedRows";
            // 
            // Label43
            // 
            this.Label43.AutoSize = true;
            this.Label43.Location = new System.Drawing.Point(11, 44);
            this.Label43.Name = "Label43";
            this.Label43.Size = new System.Drawing.Size(50, 13);
            this.Label43.TabIndex = 23;
            this.Label43.Text = "ColCount";
            // 
            // Label44
            // 
            this.Label44.AutoSize = true;
            this.Label44.Location = new System.Drawing.Point(11, 17);
            this.Label44.Name = "Label44";
            this.Label44.Size = new System.Drawing.Size(57, 13);
            this.Label44.TabIndex = 22;
            this.Label44.Text = "RowCount";
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.cboDragDropMode);
            this.TabPage3.Controls.Add(this.Label54);
            this.TabPage3.Controls.Add(this.cboAllowDragDrop);
            this.TabPage3.Controls.Add(this.Label55);
            this.TabPage3.Controls.Add(this.cboTabKeyBehavior);
            this.TabPage3.Controls.Add(this.Label53);
            this.TabPage3.Controls.Add(this.cboAutoClipboard);
            this.TabPage3.Controls.Add(this.Label52);
            this.TabPage3.Controls.Add(this.cboHighlightHeaders);
            this.TabPage3.Controls.Add(this.Label51);
            this.TabPage3.Controls.Add(this.cboShowHeaderAutoText);
            this.TabPage3.Controls.Add(this.Label50);
            this.TabPage3.Controls.Add(this.cboShowRowHeaderImage);
            this.TabPage3.Controls.Add(this.Label49);
            this.TabPage3.Controls.Add(this.picThemeCustomColorTo);
            this.TabPage3.Controls.Add(this.picThemeCustomColorFrom);
            this.TabPage3.Controls.Add(this.cboThemeColor);
            this.TabPage3.Controls.Add(this.cboThemeStyle);
            this.TabPage3.Controls.Add(this.Label45);
            this.TabPage3.Controls.Add(this.Label46);
            this.TabPage3.Controls.Add(this.Label47);
            this.TabPage3.Controls.Add(this.Label48);
            this.TabPage3.Location = new System.Drawing.Point(4, 22);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(321, 621);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Page3";
            this.TabPage3.UseVisualStyleBackColor = true;
            // 
            // cboTabKeyBehavior
            // 
            this.cboTabKeyBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTabKeyBehavior.FormattingEnabled = true;
            this.cboTabKeyBehavior.Items.AddRange(new object[] {
            "0-NextCol",
            "1-NextRow",
            "2-StandardTab"});
            this.cboTabKeyBehavior.Location = new System.Drawing.Point(142, 238);
            this.cboTabKeyBehavior.Name = "cboTabKeyBehavior";
            this.cboTabKeyBehavior.Size = new System.Drawing.Size(168, 21);
            this.cboTabKeyBehavior.TabIndex = 68;
            this.cboTabKeyBehavior.SelectedIndexChanged += new System.EventHandler(this.cboTabKeyBehavior_SelectedIndexChanged);
            // 
            // Label53
            // 
            this.Label53.AutoSize = true;
            this.Label53.Location = new System.Drawing.Point(11, 243);
            this.Label53.Name = "Label53";
            this.Label53.Size = new System.Drawing.Size(86, 13);
            this.Label53.TabIndex = 67;
            this.Label53.Text = "TabKeyBehavior";
            // 
            // cboAutoClipboard
            // 
            this.cboAutoClipboard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAutoClipboard.FormattingEnabled = true;
            this.cboAutoClipboard.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboAutoClipboard.Location = new System.Drawing.Point(142, 208);
            this.cboAutoClipboard.Name = "cboAutoClipboard";
            this.cboAutoClipboard.Size = new System.Drawing.Size(168, 21);
            this.cboAutoClipboard.TabIndex = 65;
            this.cboAutoClipboard.SelectedIndexChanged += new System.EventHandler(this.cboAutoClipboard_SelectedIndexChanged);
            // 
            // Label52
            // 
            this.Label52.AutoSize = true;
            this.Label52.Location = new System.Drawing.Point(11, 212);
            this.Label52.Name = "Label52";
            this.Label52.Size = new System.Drawing.Size(73, 13);
            this.Label52.TabIndex = 64;
            this.Label52.Text = "AutoClipboard";
            // 
            // cboHighlightHeaders
            // 
            this.cboHighlightHeaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHighlightHeaders.FormattingEnabled = true;
            this.cboHighlightHeaders.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboHighlightHeaders.Location = new System.Drawing.Point(142, 179);
            this.cboHighlightHeaders.Name = "cboHighlightHeaders";
            this.cboHighlightHeaders.Size = new System.Drawing.Size(168, 21);
            this.cboHighlightHeaders.TabIndex = 63;
            this.cboHighlightHeaders.SelectedIndexChanged += new System.EventHandler(this.cboHighlightHeaders_SelectedIndexChanged);
            // 
            // Label51
            // 
            this.Label51.AutoSize = true;
            this.Label51.Location = new System.Drawing.Point(11, 182);
            this.Label51.Name = "Label51";
            this.Label51.Size = new System.Drawing.Size(88, 13);
            this.Label51.TabIndex = 62;
            this.Label51.Text = "HighlightHeaders";
            // 
            // cboShowHeaderAutoText
            // 
            this.cboShowHeaderAutoText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShowHeaderAutoText.FormattingEnabled = true;
            this.cboShowHeaderAutoText.Items.AddRange(new object[] {
            "0-None",
            "1-RowHeader",
            "2-ColHeader",
            "3-Both"});
            this.cboShowHeaderAutoText.Location = new System.Drawing.Point(142, 150);
            this.cboShowHeaderAutoText.Name = "cboShowHeaderAutoText";
            this.cboShowHeaderAutoText.Size = new System.Drawing.Size(168, 21);
            this.cboShowHeaderAutoText.TabIndex = 61;
            this.cboShowHeaderAutoText.SelectedIndexChanged += new System.EventHandler(this.cboShowHeaderAutoText_SelectedIndexChanged);
            // 
            // Label50
            // 
            this.Label50.AutoSize = true;
            this.Label50.Location = new System.Drawing.Point(11, 154);
            this.Label50.Name = "Label50";
            this.Label50.Size = new System.Drawing.Size(112, 13);
            this.Label50.TabIndex = 60;
            this.Label50.Text = "ShowHeaderAutoText";
            // 
            // cboShowRowHeaderImage
            // 
            this.cboShowRowHeaderImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboShowRowHeaderImage.FormattingEnabled = true;
            this.cboShowRowHeaderImage.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboShowRowHeaderImage.Location = new System.Drawing.Point(142, 121);
            this.cboShowRowHeaderImage.Name = "cboShowRowHeaderImage";
            this.cboShowRowHeaderImage.Size = new System.Drawing.Size(168, 21);
            this.cboShowRowHeaderImage.TabIndex = 59;
            this.cboShowRowHeaderImage.SelectedIndexChanged += new System.EventHandler(this.cboShowRowHeaderImage_SelectedIndexChanged);
            // 
            // Label49
            // 
            this.Label49.AutoSize = true;
            this.Label49.Location = new System.Drawing.Point(11, 126);
            this.Label49.Name = "Label49";
            this.Label49.Size = new System.Drawing.Size(120, 13);
            this.Label49.TabIndex = 58;
            this.Label49.Text = "ShowRowHeaderImage";
            // 
            // picThemeCustomColorTo
            // 
            this.picThemeCustomColorTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picThemeCustomColorTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picThemeCustomColorTo.Location = new System.Drawing.Point(142, 94);
            this.picThemeCustomColorTo.Name = "picThemeCustomColorTo";
            this.picThemeCustomColorTo.Size = new System.Drawing.Size(34, 23);
            this.picThemeCustomColorTo.TabIndex = 57;
            this.picThemeCustomColorTo.TabStop = false;
            this.picThemeCustomColorTo.Click += new System.EventHandler(this.picThemeCustomColorTo_Click);
            // 
            // picThemeCustomColorFrom
            // 
            this.picThemeCustomColorFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.picThemeCustomColorFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picThemeCustomColorFrom.Location = new System.Drawing.Point(142, 67);
            this.picThemeCustomColorFrom.Name = "picThemeCustomColorFrom";
            this.picThemeCustomColorFrom.Size = new System.Drawing.Size(34, 23);
            this.picThemeCustomColorFrom.TabIndex = 56;
            this.picThemeCustomColorFrom.TabStop = false;
            this.picThemeCustomColorFrom.Click += new System.EventHandler(this.picThemeCustomColorFrom_Click);
            // 
            // cboThemeColor
            // 
            this.cboThemeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThemeColor.FormattingEnabled = true;
            this.cboThemeColor.Items.AddRange(new object[] {
            "0-Blue",
            "1-Silver",
            "2-Olive",
            "3-Visual2005",
            "4-CustomColor"});
            this.cboThemeColor.Location = new System.Drawing.Point(142, 40);
            this.cboThemeColor.Name = "cboThemeColor";
            this.cboThemeColor.Size = new System.Drawing.Size(168, 21);
            this.cboThemeColor.TabIndex = 31;
            this.cboThemeColor.SelectedIndexChanged += new System.EventHandler(this.cboThemeColor_SelectedIndexChanged);
            // 
            // cboThemeStyle
            // 
            this.cboThemeStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThemeStyle.FormattingEnabled = true;
            this.cboThemeStyle.Items.AddRange(new object[] {
            "0-Flat",
            "1-Light3D",
            "2-Windows",
            "3-Office"});
            this.cboThemeStyle.Location = new System.Drawing.Point(142, 13);
            this.cboThemeStyle.Name = "cboThemeStyle";
            this.cboThemeStyle.Size = new System.Drawing.Size(168, 21);
            this.cboThemeStyle.TabIndex = 30;
            this.cboThemeStyle.SelectedIndexChanged += new System.EventHandler(this.cboThemeStyle_SelectedIndexChanged);
            // 
            // Label45
            // 
            this.Label45.AutoSize = true;
            this.Label45.Location = new System.Drawing.Point(11, 99);
            this.Label45.Name = "Label45";
            this.Label45.Size = new System.Drawing.Size(112, 13);
            this.Label45.TabIndex = 29;
            this.Label45.Text = "ThemeCustomColorTo";
            // 
            // Label46
            // 
            this.Label46.AutoSize = true;
            this.Label46.Location = new System.Drawing.Point(11, 72);
            this.Label46.Name = "Label46";
            this.Label46.Size = new System.Drawing.Size(122, 13);
            this.Label46.TabIndex = 28;
            this.Label46.Text = "ThemeCustomColorFrom";
            // 
            // Label47
            // 
            this.Label47.AutoSize = true;
            this.Label47.Location = new System.Drawing.Point(11, 44);
            this.Label47.Name = "Label47";
            this.Label47.Size = new System.Drawing.Size(64, 13);
            this.Label47.TabIndex = 27;
            this.Label47.Text = "ThemeColor";
            // 
            // Label48
            // 
            this.Label48.AutoSize = true;
            this.Label48.Location = new System.Drawing.Point(11, 17);
            this.Label48.Name = "Label48";
            this.Label48.Size = new System.Drawing.Size(63, 13);
            this.Label48.TabIndex = 26;
            this.Label48.Text = "ThemeStyle";
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "s1.ico");
            this.ImageList1.Images.SetKeyName(1, "s2.ico");
            this.ImageList1.Images.SetKeyName(2, "s3.ico");
            this.ImageList1.Images.SetKeyName(3, "s4.ico");
            this.ImageList1.Images.SetKeyName(4, "s5.ico");
            this.ImageList1.Images.SetKeyName(5, "s6.ico");
            this.ImageList1.Images.SetKeyName(6, "s7.ico");
            this.ImageList1.Images.SetKeyName(7, "s8.ico");
            // 
            // GrdView1
            // 
            this.GrdView1.AutoRedraw = true;
            this.GrdView1.CurrentColIndex = 1;
            this.GrdView1.CurrentRowIndex = 1;
            this.GrdView1.EditSelectedText = null;
            this.GrdView1.EditSelectionLength = 0;
            this.GrdView1.EditSelectionStart = 0;
            this.GrdView1.EditText = null;
            this.GrdView1.FormatProvider = new System.Globalization.CultureInfo("zh-CN");
            this.GrdView1.LanguageConfig = null;
            this.GrdView1.LeftCol = 1;
            this.GrdView1.LicenseKey = null;
            this.GrdView1.Location = new System.Drawing.Point(12, 90);
            this.GrdView1.Name = "GrdView1";
            this.GrdView1.Size = new System.Drawing.Size(585, 593);
            this.GrdView1.TabIndex = 7;
            this.GrdView1.ThemeCustomColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.GrdView1.ThemeCustomColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(204)))), ((int)(((byte)(221)))));
            this.GrdView1.TopRow = 1;
            this.GrdView1.ErrorInfo += new BaiqiSoft.GridControl.ErrorInfoEventHandler(this.GrdView1_ErrorInfo);
            // 
            // cboDragDropMode
            // 
            this.cboDragDropMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDragDropMode.FormattingEnabled = true;
            this.cboDragDropMode.Items.AddRange(new object[] {
            "0-Content",
            "1-FormatAndContent"});
            this.cboDragDropMode.Location = new System.Drawing.Point(142, 299);
            this.cboDragDropMode.Name = "cboDragDropMode";
            this.cboDragDropMode.Size = new System.Drawing.Size(168, 21);
            this.cboDragDropMode.TabIndex = 76;
            this.cboDragDropMode.SelectedIndexChanged += new System.EventHandler(this.cboDragDropMode_SelectedIndexChanged);
            // 
            // Label54
            // 
            this.Label54.AutoSize = true;
            this.Label54.Location = new System.Drawing.Point(11, 304);
            this.Label54.Name = "Label54";
            this.Label54.Size = new System.Drawing.Size(80, 13);
            this.Label54.TabIndex = 75;
            this.Label54.Text = "DragDropMode";
            // 
            // cboAllowDragDrop
            // 
            this.cboAllowDragDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAllowDragDrop.FormattingEnabled = true;
            this.cboAllowDragDrop.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboAllowDragDrop.Location = new System.Drawing.Point(142, 269);
            this.cboAllowDragDrop.Name = "cboAllowDragDrop";
            this.cboAllowDragDrop.Size = new System.Drawing.Size(168, 21);
            this.cboAllowDragDrop.TabIndex = 74;
            this.cboAllowDragDrop.SelectedIndexChanged += new System.EventHandler(this.cboAllowDragDrop_SelectedIndexChanged);
            // 
            // Label55
            // 
            this.Label55.AutoSize = true;
            this.Label55.Location = new System.Drawing.Point(11, 273);
            this.Label55.Name = "Label55";
            this.Label55.Size = new System.Drawing.Size(78, 13);
            this.Label55.TabIndex = 73;
            this.Label55.Text = "AllowDragDrop";
            // 
            // frmDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 781);
            this.Controls.Add(this.GrdView1);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.tabProperty);
            this.Controls.Add(this.ToolStrip2);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.MenuStrip1);
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "frmDemo";
            this.Text = "MstGrid Designer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ToolStrip2.ResumeLayout(false);
            this.ToolStrip2.PerformLayout();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.tabProperty.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSheetBorderColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGridColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColorSel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColorFrozen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColorFixed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picForeColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorSel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorFrozen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorBkg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColorAlternate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColor)).EndInit();
            this.TabPage3.ResumeLayout(false);
            this.TabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThemeCustomColorTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThemeCustomColorFrom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    internal System.Windows.Forms.MenuStrip MenuStrip1;
    internal System.Windows.Forms.ToolStripMenuItem mi_file;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_new;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_open;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_save;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_saveas;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_pagesetup;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_printpreview;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_print;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
    internal System.Windows.Forms.ToolStripMenuItem mi_file_exit;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_cut;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_copy;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_paste;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_hiderow;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_hidecolumn;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator8;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_unhiderow;
    internal System.Windows.Forms.ToolStripMenuItem mi_edit_unhidecolumn;
    internal System.Windows.Forms.ToolStripMenuItem mi_format;
    internal System.Windows.Forms.ToolStripMenuItem mi_format_column;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator10;
    internal System.Windows.Forms.ToolStripMenuItem mi_help;
    internal System.Windows.Forms.ToolStripMenuItem mi_help_manual;
    internal System.Windows.Forms.ToolStripMenuItem mi_help_about;
    internal System.Windows.Forms.ToolStrip ToolStrip1;
    internal System.Windows.Forms.ToolStrip ToolStrip2;
    internal System.Windows.Forms.ToolStripButton bt_new;
    internal System.Windows.Forms.ToolStripButton bt_open;
    internal System.Windows.Forms.ToolStripButton bt_save;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator12;
    internal System.Windows.Forms.ToolStripButton bt_print;
    internal System.Windows.Forms.ToolStripButton bt_printpreview;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator13;
    internal System.Windows.Forms.ToolStripButton bt_cut;
    internal System.Windows.Forms.ToolStripButton bt_copy;
    internal System.Windows.Forms.ToolStripButton bt_paste;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator14;
    internal System.Windows.Forms.ToolStripButton bt_merge;
    internal System.Windows.Forms.ToolStripButton bt_unmerge;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator15;
    internal System.Windows.Forms.ToolStripButton bt_insertrow;
    internal System.Windows.Forms.ToolStripButton bt_insertcolumn;
    internal System.Windows.Forms.ToolStripButton bt_deleterow;
    internal System.Windows.Forms.ToolStripButton bt_deletecolumn;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator16;
    internal System.Windows.Forms.ToolStripButton bt_image;
    internal System.Windows.Forms.ToolStripButton bt_help;
    internal System.Windows.Forms.ToolStripComboBox cboFontName;
    internal System.Windows.Forms.ToolStripComboBox cboFontSize;
    internal System.Windows.Forms.ToolStripButton bt_fontbold;
    internal System.Windows.Forms.ToolStripButton bt_fontitalic;
    internal System.Windows.Forms.ToolStripButton bt_fontunderline;
    internal System.Windows.Forms.ToolStripButton bt_FontStrikethrough;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator17;
    internal System.Windows.Forms.ToolStripLabel ToolStripLabel1;
    internal System.Windows.Forms.ToolStripSplitButton bt_imagealign;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_lefttop;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_leftcenter;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_leftbottom;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator18;
    internal System.Windows.Forms.ToolStripLabel ToolStripLabel2;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator19;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_centertop;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_centercenter;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_centerbottom;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_righttop;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_rightcenter;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_rightbottom;
    internal System.Windows.Forms.ToolStripSplitButton bt_textalign;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_lefttop;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_leftcenter;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_leftbottom;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_centertop;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_centercenter;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_centerbottom;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_righttop;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_rightcenter;
    internal System.Windows.Forms.ToolStripMenuItem bt_textalign_rightbottom;
    internal System.Windows.Forms.StatusStrip StatusStrip1;
    internal System.Windows.Forms.TabControl tabProperty;
    internal System.Windows.Forms.TabPage TabPage1;
    internal System.Windows.Forms.TabPage TabPage2;
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.Label Label5;
    internal System.Windows.Forms.Label Label4;
    internal System.Windows.Forms.Label Label3;
    internal System.Windows.Forms.Label Label2;
    internal System.Windows.Forms.Label Label6;
    internal System.Windows.Forms.Label Label22;
    internal System.Windows.Forms.Label Label21;
    internal System.Windows.Forms.Label Label20;
    internal System.Windows.Forms.Label Label19;
    internal System.Windows.Forms.Label Label18;
    internal System.Windows.Forms.Label Label17;
    internal System.Windows.Forms.Label Label16;
    internal System.Windows.Forms.Label Label15;
    internal System.Windows.Forms.Label Label14;
    internal System.Windows.Forms.Label Label13;
    internal System.Windows.Forms.Label Label12;
    internal System.Windows.Forms.Label Label11;
    internal System.Windows.Forms.Label Label10;
    internal System.Windows.Forms.Label Label9;
    internal System.Windows.Forms.Label Label8;
    internal System.Windows.Forms.Label Label7;
    internal System.Windows.Forms.Label Label23;
    internal System.Windows.Forms.Label Label24;
    internal System.Windows.Forms.Label Label25;
    internal System.Windows.Forms.Label Label26;
    internal System.Windows.Forms.Label Label27;
    internal System.Windows.Forms.Label Label28;
    internal System.Windows.Forms.Label Label29;
    internal System.Windows.Forms.Label Label30;
    internal System.Windows.Forms.Label Label31;
    internal System.Windows.Forms.Label Label32;
    internal System.Windows.Forms.Label Label33;
    internal System.Windows.Forms.Label Label34;
    internal System.Windows.Forms.Label Label35;
    internal System.Windows.Forms.Label Label36;
    internal System.Windows.Forms.Label Label37;
    internal System.Windows.Forms.Label Label38;
    internal System.Windows.Forms.Label Label39;
    internal System.Windows.Forms.Label Label40;
    internal System.Windows.Forms.Label Label41;
    internal System.Windows.Forms.Label Label42;
    internal System.Windows.Forms.Label Label43;
    internal System.Windows.Forms.Label Label44;
    internal System.Windows.Forms.TabPage TabPage3;
    internal System.Windows.Forms.ComboBox cboApplySelectionToImage;
    internal System.Windows.Forms.ComboBox cboShowContextMenu;
    internal System.Windows.Forms.ComboBox cboShowComboButton;
    internal System.Windows.Forms.ComboBox cboPictureOver;
    internal System.Windows.Forms.ComboBox cboScrollBarStyle;
    internal System.Windows.Forms.ComboBox cboScrollBars;
    internal System.Windows.Forms.ComboBox cboFocusRect;
    internal System.Windows.Forms.ComboBox cboHighLight;
    internal System.Windows.Forms.ComboBox cboSelectionMode;
    internal System.Windows.Forms.ComboBox cboExtendLastCol;
    internal System.Windows.Forms.ComboBox cboEnterKeyBehavior;
    internal System.Windows.Forms.ComboBox cboEllipsis;
    internal System.Windows.Forms.ComboBox cboEditable;
    internal System.Windows.Forms.ComboBox cboButtonLocked;
    internal System.Windows.Forms.ComboBox cboDateFormat;
    internal System.Windows.Forms.ComboBox cboGridLines;
    internal System.Windows.Forms.ComboBox cboBorderStyle;
    internal System.Windows.Forms.ComboBox cboAllowUserSort;
    internal System.Windows.Forms.ComboBox cboAllowUserResizing;
    internal System.Windows.Forms.ComboBox cboAllowUserReorder;
    internal System.Windows.Forms.ComboBox cboAllowSelection;
    internal System.Windows.Forms.ComboBox cboAllowBigSelection;
    internal System.Windows.Forms.TextBox txtDefaultRowHeight;
    internal System.Windows.Forms.TextBox txtDefaultColWidth;
    internal System.Windows.Forms.TextBox txtFrozenCols;
    internal System.Windows.Forms.TextBox txtFrozenRows;
    internal System.Windows.Forms.TextBox txtFixedCols;
    internal System.Windows.Forms.TextBox txtFixedRows;
    internal System.Windows.Forms.TextBox txtColCount;
    internal System.Windows.Forms.ComboBox cboUseBackColorAlternate;
    internal System.Windows.Forms.TextBox txtRowCount;
    internal System.Windows.Forms.Button btnFont;
    internal System.Windows.Forms.PictureBox picBackColor;
    internal System.Windows.Forms.ComboBox cboSheetBorder;
    internal System.Windows.Forms.PictureBox picSheetBorderColor;
    internal System.Windows.Forms.PictureBox picGridColor;
    internal System.Windows.Forms.PictureBox picForeColorSel;
    internal System.Windows.Forms.PictureBox picForeColorFrozen;
    internal System.Windows.Forms.PictureBox picForeColorFixed;
    internal System.Windows.Forms.PictureBox picForeColor;
    internal System.Windows.Forms.PictureBox picBackColorSel;
    internal System.Windows.Forms.PictureBox picBackColorFrozen;
    internal System.Windows.Forms.PictureBox picBackColorBkg;
    internal System.Windows.Forms.PictureBox picBackColorAlternate;
    internal System.Windows.Forms.ComboBox cboThemeColor;
    internal System.Windows.Forms.ComboBox cboThemeStyle;
    internal System.Windows.Forms.Label Label45;
    internal System.Windows.Forms.Label Label46;
    internal System.Windows.Forms.Label Label47;
    internal System.Windows.Forms.Label Label48;
    internal System.Windows.Forms.PictureBox picThemeCustomColorTo;
    internal System.Windows.Forms.PictureBox picThemeCustomColorFrom;
    internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
    internal System.Windows.Forms.ColorDialog ColorDialog1;
    internal System.Windows.Forms.FontDialog FontDialog1;
    internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
    internal System.Windows.Forms.SaveFileDialog SaveFileDialog1;
    internal System.Windows.Forms.ToolStripMenuItem bt_imagealign_stretch;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator20;
    internal System.Windows.Forms.ToolStripSplitButton bt_cellcolor;
    internal System.Windows.Forms.ToolStripSplitButton bt_textcolor;
    internal System.Windows.Forms.ToolStripLabel ToolStripLabel5;
    internal System.Windows.Forms.ToolStripLabel ToolStripLabel6;
    internal System.Windows.Forms.ImageList ImageList1;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator21;
    internal System.Windows.Forms.ToolStripLabel ToolStripLabel7;
    internal System.Windows.Forms.ToolStripSplitButton bt_cellborder;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_around;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_none;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_left;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_top;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_right;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_bottom;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_diagonalup;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_diagonaldown;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_inside;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_insidevertical;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_insidehorizontal;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator22;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_thin;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_thick;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_dot;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator23;
    internal System.Windows.Forms.ToolStripMenuItem bt_cellborder_color;
    internal System.Windows.Forms.ComboBox cboShowRowHeaderImage;
    internal System.Windows.Forms.Label Label49;
    internal System.Windows.Forms.ToolStripMenuItem mi_help_updates;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator24;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator25;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype_textbox;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype_combobox;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype_checkbox;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype_calendar;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype_button;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_celltype_hypelink;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_any;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_numeric;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_positivenumeric;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_integers;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_positiveintegers;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_letter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_letternumeric;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_upper;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_uppernumeric;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_lower;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_lowernumeric;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_editmask_chqno;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_lefttop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_leftcenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_leftbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_centertop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_centercenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_centerbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_righttop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_rightcenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_textalign_rightbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_lefttop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_leftcenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_leftbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_centertop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_centercenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_centerbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_righttop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_rightcenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_titlealign_rightbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_lefttop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_leftcenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_leftbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_centertop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_centercenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_centerbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_righttop;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_rightcenter;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_rightbottom;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_picalign_streth;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_sort;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_sort_bystring;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_sort_bystringnocase;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_sort_byboolean;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_sort_bydate;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_sort_bynumeric;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_datestyle;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_datestyle_date;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_datestyle_time;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_datestyle_datetime;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator26;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_lock;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_unlock;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator27;
    internal System.Windows.Forms.ToolStripMenuItem mi_columnformat_title;
    internal System.Windows.Forms.ToolStripMenuItem mi_cellformat;
    internal System.Windows.Forms.ToolStripMenuItem mi_cellformat_setwraptext;
    internal System.Windows.Forms.ToolStripMenuItem mi_cellformat_cancelwrap;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator28;
    internal System.Windows.Forms.ToolStripMenuItem mi_cellformat_lock;
    internal System.Windows.Forms.ToolStripMenuItem mi_cellformat_unlock;
    internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator29;
    internal System.Windows.Forms.ToolStripMenuItem mi_cellformat_iamge;
    internal System.Windows.Forms.ComboBox cboShowHeaderAutoText;
    internal System.Windows.Forms.Label Label50;
    internal System.Windows.Forms.ComboBox cboHighlightHeaders;
    internal System.Windows.Forms.Label Label51;
    internal System.Windows.Forms.Label Label53;
    internal System.Windows.Forms.ComboBox cboAutoClipboard;
    internal System.Windows.Forms.Label Label52;
    internal System.Windows.Forms.ComboBox cboTabKeyBehavior;
    internal MstGrid  GrdView1;
    private System.ComponentModel.IContainer components;
    internal System.Windows.Forms.ComboBox cboDragDropMode;
    internal System.Windows.Forms.Label Label54;
    internal System.Windows.Forms.ComboBox cboAllowDragDrop;
    internal System.Windows.Forms.Label Label55;
}

