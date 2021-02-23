using System.Drawing;
using System.Windows.Forms;
using System;
using System.IO;
using System.Drawing.Text;
using BaiqiSoft.GridControl;



public partial class frmDemo
{
    public frmDemo()
    {
        InitializeComponent();

        //Added to support default instance behavour in C#
        if (defaultInstance == null)
            defaultInstance = this;
    }

    #region Default Instance

    private static frmDemo defaultInstance;

    public static frmDemo Default
    {
        get
        {
            if (defaultInstance == null)
            {
                defaultInstance = new frmDemo();
                defaultInstance.FormClosed += new FormClosedEventHandler(defaultInstance_FormClosed);
            }

            return defaultInstance;
        }
    }

    static void defaultInstance_FormClosed(object sender, FormClosedEventArgs e)
    {
        defaultInstance = null;
    }

    #endregion

    string m_FileName = "";
    LineStyle m_LineStyle = LineStyle.Thin;
    Color m_LineColor = Color.Black;

    private void CboAllowBigSelection_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AllowBigSelection = bool.Parse(cboAllowBigSelection.Text);
    }


    private void CboAllowSelection_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AllowSelection = bool.Parse(cboAllowSelection.Text);
    }


    private void CboAllowUserReorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AllowUserReorder = (ReorderMode)cboAllowUserReorder.SelectedIndex;
    }


    private void CboAllowUserResizing_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AllowUserResizing = (ResizeMode)cboAllowUserResizing.SelectedIndex;
    }


    private void CboAllowUserSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AllowUserSort = bool.Parse(cboAllowUserSort.Text);
    }


    private void CboBorderStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.BorderStyle = (GridBorderStyle)cboBorderStyle.SelectedIndex;
    }


    private void cboGridLines_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.GridLines = (GridLines)cboGridLines.SelectedIndex;
    }


    private void CboDateFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.DateFormat = (DateFormat)cboDateFormat.SelectedIndex;
    }


    private void CboButtonLocked_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ButtonLocked = bool.Parse(cboButtonLocked.Text);
    }


    private void CboEditable_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.Editable = bool.Parse(cboEditable.Text);
    }


    private void CboEllipsis_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.Ellipsis = bool.Parse(cboEllipsis.Text);
    }


    private void CboEnterKeyBehavior_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.EnterKeyBehavior = (EnterKeyBehavior)cboEnterKeyBehavior.SelectedIndex;
    }


    private void CboExtendLastCol_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ExtendLastCol = bool.Parse(cboExtendLastCol.Text);
    }


    private void cboSelectionMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.SelectionMode = (BaiqiSoft.GridControl.SelectionMode)cboSelectionMode.SelectedIndex;
    }


    private void cboHighLight_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.HighLight = (HighLightMode)cboHighLight.SelectedIndex;
    }


    private void cboFocusRect_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.FocusRect = (FocusRectMode)cboFocusRect.SelectedIndex;
    }


    private void cboScrollBars_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ScrollBars = (BaiqiSoft.GridControl.ScrollBars)cboScrollBars.SelectedIndex;
    }


    private void cboScrollBarStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ScrollBarStyle = (ScrollBarStyle)cboScrollBarStyle.SelectedIndex;
    }


    private void cboPictureOver_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.PicturesOver = bool.Parse(cboPictureOver.Text);
    }


    private void cboShowComboButton_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ShowComboButton = (ShowComboButton)cboShowComboButton.SelectedIndex;
    }


    private void cboShowContextMenu_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ShowContextMenu = bool.Parse(cboShowContextMenu.Text);
    }


    private void cboApplySelectionToImage_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ApplySelectionToImage = bool.Parse(cboApplySelectionToImage.Text);
    }


    private void TxtRowCount_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.RowCount = int.Parse(txtRowCount.Text);
            }
        }
    }


    private void TxtColCount_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.ColCount = int.Parse(txtColCount.Text);
            }
        }
    }


    private void txtFixedRows_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.FixedRows = int.Parse(txtFixedRows.Text);
            }
        }
    }


    private void txtFixedCols_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.FixedCols = int.Parse(txtFixedCols.Text);
            }
        }
    }


    private void txtFrozenRows_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.FrozenRows = int.Parse(txtFrozenRows.Text);
            }
        }
    }


    private void txtFrozenCols_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.FrozenCols = int.Parse(txtFrozenCols.Text);
            }
        }
    }


    private void TxtDefaultColWidth_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.DefaultColWidth = int.Parse(txtDefaultColWidth.Text);
            }
        }
    }


    private void TxtDefaultRowHeight_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar < '0' || e.KeyChar > '9')
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                e.Handled = true;
            }
            else
            {
                //Enter Key
                GrdView1.DefaultRowHeight = int.Parse(txtDefaultRowHeight.Text);
            }
        }
    }


    private void picBackColor_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picBackColor.BackColor;
        ColorDialog1.ShowDialog();
        picBackColor.BackColor = ColorDialog1.Color;
        GrdView1.BackColor = picBackColor.BackColor;
    }


    private void picBackColorAlternate_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picBackColorAlternate.BackColor;
        ColorDialog1.ShowDialog();
        picBackColorAlternate.BackColor = ColorDialog1.Color;
        GrdView1.BackColorAlternate = picBackColorAlternate.BackColor;
    }


    private void picBackColorBkg_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picBackColorBkg.BackColor;
        ColorDialog1.ShowDialog();
        picBackColorBkg.BackColor = ColorDialog1.Color;
        GrdView1.BackColorBkg = picBackColorBkg.BackColor;
    }


    private void picBackColorFrozen_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picBackColorFrozen.BackColor;
        ColorDialog1.ShowDialog();
        picBackColorFrozen.BackColor = ColorDialog1.Color;
        GrdView1.BackColorFrozen = picBackColorFrozen.BackColor;
    }


    private void picBackColorSel_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picBackColorSel.BackColor;
        ColorDialog1.ShowDialog();
        picBackColorSel.BackColor = ColorDialog1.Color;
        GrdView1.BackColorSel = picBackColorSel.BackColor;
    }


    private void cboUseBackColorAlternate_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.UseBackColorAlternate = bool.Parse(cboUseBackColorAlternate.Text);
    }


    private void picForeColor_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picForeColor.BackColor;
        ColorDialog1.ShowDialog();
        picForeColor.BackColor = ColorDialog1.Color;
        GrdView1.ForeColor = picForeColor.BackColor;
    }


    private void picForeColorFixed_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picForeColorFixed.BackColor;
        ColorDialog1.ShowDialog();
        picForeColorFixed.BackColor = ColorDialog1.Color;
        GrdView1.ForeColorFixed = picForeColorFixed.BackColor;
    }


    private void picForeColorFrozen_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picForeColorFrozen.BackColor;
        ColorDialog1.ShowDialog();
        picForeColorFrozen.BackColor = ColorDialog1.Color;
        GrdView1.ForeColorFrozen = picForeColorFrozen.BackColor;
    }


    private void picForeColorSel_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picForeColorSel.BackColor;
        ColorDialog1.ShowDialog();
        picForeColorSel.BackColor = ColorDialog1.Color;
        GrdView1.ForeColorSel = picForeColorSel.BackColor;
    }


    private void picGridColor_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picGridColor.BackColor;
        ColorDialog1.ShowDialog();
        picGridColor.BackColor = ColorDialog1.Color;
        GrdView1.GridColor = picGridColor.BackColor;
    }


    private void cboSheetBorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.SheetBorder = bool.Parse(cboSheetBorder.Text);
    }


    private void picSheetBorderColor_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picSheetBorderColor.BackColor;
        ColorDialog1.ShowDialog();
        picSheetBorderColor.BackColor = ColorDialog1.Color;
        GrdView1.SheetBorderColor = picSheetBorderColor.BackColor;
    }


    private void cboThemeStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ThemeStyle = (ThemeStyle)cboThemeStyle.SelectedIndex;
    }


    private void cboThemeColor_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ThemeColor = (ThemeColor)cboThemeColor.SelectedIndex;
        picThemeCustomColorFrom.BackColor = GrdView1.ThemeCustomColorFrom;
        picThemeCustomColorTo.BackColor = GrdView1.ThemeCustomColorTo;
    }


    private void picThemeCustomColorFrom_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picThemeCustomColorFrom.BackColor;
        ColorDialog1.ShowDialog();
        picThemeCustomColorFrom.BackColor = ColorDialog1.Color;
        GrdView1.ThemeCustomColorFrom = picThemeCustomColorFrom.BackColor;
    }


    private void picThemeCustomColorTo_Click(object sender, EventArgs e)
    {
        ColorDialog1.Color = picThemeCustomColorTo.BackColor;
        ColorDialog1.ShowDialog();
        picThemeCustomColorTo.BackColor = ColorDialog1.Color;
        GrdView1.ThemeCustomColorTo = picThemeCustomColorTo.BackColor;
    }


    private void btnFont_Click(object sender, System.EventArgs e)
    {
        FontDialog1.Font = GrdView1.Font;
        FontDialog1.ShowDialog();
        GrdView1.Font = FontDialog1.Font;
    }


    private void cboShowRowHeaderImage_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.ShowRowHeaderImage = bool.Parse(cboShowRowHeaderImage.Text);
    }


    private void cboHighlightHeaders_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.HighlightHeaders = bool.Parse(cboHighlightHeaders.Text);
    }


    private void ReadProperty()
    {
        //Page1
        cboAllowBigSelection.SelectedIndex = GrdView1.AllowBigSelection ? 0 : 1;
        cboAllowSelection.SelectedIndex = GrdView1.AllowSelection ? 0 : 1;
        cboAllowUserReorder.SelectedIndex = (int)GrdView1.AllowUserReorder;
        cboAllowUserResizing.SelectedIndex = (int)GrdView1.AllowUserResizing;
        cboAllowUserSort.SelectedIndex = GrdView1.AllowUserSort ? 0 : 1;
        cboBorderStyle.SelectedIndex = (int)GrdView1.BorderStyle;
        cboGridLines.SelectedIndex = (int)GrdView1.GridLines;
        cboDateFormat.SelectedIndex = (int)GrdView1.DateFormat;
        cboButtonLocked.SelectedIndex = GrdView1.ButtonLocked ? 0 : 1;
        cboEditable.SelectedIndex = GrdView1.Editable ? 0 : 1;
        cboEllipsis.SelectedIndex = GrdView1.Ellipsis ? 0 : 1;
        cboEnterKeyBehavior.SelectedIndex = (int)GrdView1.EnterKeyBehavior;
        cboExtendLastCol.SelectedIndex = GrdView1.ExtendLastCol ? 0 : 1;
        cboSelectionMode.SelectedIndex = (int)GrdView1.SelectionMode;
        cboHighLight.SelectedIndex = (int)GrdView1.HighLight;
        cboFocusRect.SelectedIndex = (int)GrdView1.FocusRect;
        cboScrollBars.SelectedIndex = (int)GrdView1.ScrollBars;
        cboScrollBarStyle.SelectedIndex = (int)GrdView1.ScrollBarStyle;
        cboPictureOver.SelectedIndex = GrdView1.PicturesOver ? 0 : 1;
        cboShowComboButton.SelectedIndex = (int)GrdView1.ShowComboButton;
        cboShowContextMenu.SelectedIndex = GrdView1.ShowContextMenu ? 0 : 1;
        cboApplySelectionToImage.SelectedIndex = GrdView1.ApplySelectionToImage ? 0 : 1;
        //Page2
        txtRowCount.Text = GrdView1.RowCount.ToString();
        txtColCount.Text = GrdView1.ColCount.ToString();
        txtFixedRows.Text = GrdView1.FixedRows.ToString();
        txtFixedCols.Text = GrdView1.FixedCols.ToString();
        txtFrozenRows.Text = GrdView1.FrozenRows.ToString();
        txtFrozenCols.Text = GrdView1.FrozenCols.ToString();
        txtDefaultColWidth.Text = GrdView1.DefaultColWidth.ToString();
        txtDefaultRowHeight.Text = GrdView1.DefaultRowHeight.ToString();
        picBackColor.BackColor = GrdView1.BackColor;
        picBackColorAlternate.BackColor = GrdView1.BackColorAlternate;
        picBackColorBkg.BackColor = GrdView1.BackColorBkg;
        picBackColorFrozen.BackColor = GrdView1.BackColorFrozen;
        picBackColorSel.BackColor = GrdView1.BackColorSel;
        cboUseBackColorAlternate.SelectedIndex = GrdView1.UseBackColorAlternate ? 0 : 1;
        picForeColor.BackColor = GrdView1.ForeColor;
        picForeColorFixed.BackColor = GrdView1.ForeColorFixed;
        picForeColorFrozen.BackColor = GrdView1.ForeColorFrozen;
        picForeColorSel.BackColor = GrdView1.ForeColorSel;
        picGridColor.BackColor = GrdView1.GridColor;
        cboSheetBorder.SelectedIndex = GrdView1.SheetBorder ? 0 : 1;
        picSheetBorderColor.BackColor = GrdView1.SheetBorderColor;
        //Page3
        cboThemeStyle.SelectedIndex = (int)GrdView1.ThemeStyle;
        cboThemeColor.SelectedIndex = (int)GrdView1.ThemeColor;
        picThemeCustomColorFrom.BackColor = GrdView1.ThemeCustomColorFrom;
        picThemeCustomColorTo.BackColor = GrdView1.ThemeCustomColorTo;
        cboShowRowHeaderImage.SelectedIndex = GrdView1.ShowRowHeaderImage ? 0 : 1;
        cboShowHeaderAutoText.SelectedIndex = (int)GrdView1.ShowHeaderAutoText;
        cboHighlightHeaders.SelectedIndex = GrdView1.HighlightHeaders ? 0 : 1;
        cboAutoClipboard.SelectedIndex = GrdView1.AutoClipboard ? 0 : 1;
        cboTabKeyBehavior.SelectedIndex = (int)GrdView1.TabKeyBehavior;
        cboAllowDragDrop.SelectedIndex = GrdView1.AllowDragDrop ? 0 : 1;
        cboDragDropMode.SelectedIndex = (int)GrdView1.DragDropMode;
    }


    private void Main_Load(object sender, System.EventArgs e)
    {
        bt_imagealign.Tag = 1;
        bt_cellborder.Tag = -1;
        bt_textalign.Tag = 1;
        bt_cellcolor.ForeColor = Color.White;
        bt_textcolor.ForeColor = Color.Black;
        //Font Name
        FontFamily[] fontFamilies;
        InstalledFontCollection installedFontCollection = new InstalledFontCollection();
        fontFamilies = installedFontCollection.Families;
        for (int i = 0; i < fontFamilies.Length; i++)
        {
            cboFontName.Items.Add(fontFamilies[i].Name);
        }
        cboFontName.Text = GrdView1.Font.Name;
        //FontSize
        for (int i = 5; i <= 72; i++)
        {
            cboFontSize.Items.Add(i.ToString());
        }
        cboFontSize.Text = GrdView1.Font.SizeInPoints.ToString();
        //Init Grid
        GrdView1.NewFile(51, 11);
        GrdView1.ImageList = ImageList1;
        GrdView1.AllowDragDrop = true;
        //Get Property Values
        ReadProperty();
    }


    private void Main_Resize(object sender, System.EventArgs e)
    {
        tabProperty.Left = this.ClientRectangle.Width - tabProperty.Width - 15;
        tabProperty.Top = this.ToolStrip2.Bottom + 3;

        GrdView1.Left = 0;
        GrdView1.Top = tabProperty.Top;
        if (tabProperty.Left - 12 > 0)
        {
            GrdView1.Width = tabProperty.Left - 12;
        }
        if (this.Height - this.MenuStrip1.Height - this.ToolStrip1.Height - this.ToolStrip2.Height - this.StatusStrip1.Height - 45 > 0)
        {
            GrdView1.Height = this.Height - this.MenuStrip1.Height - this.ToolStrip1.Height - this.ToolStrip2.Height - this.StatusStrip1.Height - 45;
            if (GrdView1.Height < tabProperty.Height)
            {
                GrdView1.Height = tabProperty.Height;
            }
        }
    }


    private void mi_file_new_click(object sender, EventArgs e)
    {
        m_FileName = "";
        GrdView1.NewFile(51, 11);
        ReadProperty();
    }


    private void mi_file_open_click(object sender, EventArgs e)
    {
        OpenFileDialog1.FileName = "";
        OpenFileDialog1.Filter = "XML File (*.xml)|*.xml";
        OpenFileDialog1.Title = "Open";
        OpenFileDialog1.ShowDialog();
        if (OpenFileDialog1.FileName.Length > 0)
        {
            m_FileName = OpenFileDialog1.FileName;
            GrdView1.OpenFile(m_FileName);
            ReadProperty();
        }
    }


    private void mi_file_save_click(object sender, EventArgs e)
    {
        if (m_FileName.Length > 0)
        {
            GrdView1.SaveFile(m_FileName);
        }
        else
        {
            SaveFileDialog1.Filter = "XML File (*.xml)|*.xml";
            SaveFileDialog1.Title = "Save";
            SaveFileDialog1.ShowDialog();
            if (SaveFileDialog1.FileName.Length > 0)
            {
                m_FileName = SaveFileDialog1.FileName;
                GrdView1.SaveFile(m_FileName);
            }
        }
    }


    private void mi_file_saveas_click(object sender, EventArgs e)
    {
        SaveFileDialog1.OverwritePrompt = true;
        SaveFileDialog1.Filter = "XML File (*.xml)|*.xml";
        SaveFileDialog1.Title = "Save As";
        SaveFileDialog1.ShowDialog();
        if (SaveFileDialog1.FileName.Length > 0)
        {
            m_FileName = SaveFileDialog1.FileName;
            GrdView1.SaveFile(m_FileName);
        }
    }


    private void mi_file_pagesetup_click(object sender, EventArgs e)
    {
        GrdView1.ShowPageSetupDialog();
    }


    private void mi_file_printpreview_click(object sender, EventArgs e)
    {
        GrdView1.PrintPreview(true, true, 0, 0, 0, 0);
    }


    private void mi_file_print_click(object sender, EventArgs e)
    {
        GrdView1.PrintOut(0, 0, 1, false);
    }


    private void mi_file_exit_click(object sender, EventArgs e)
    {
        frmColumn.Default.Close();
        frmSetCellImage.Default.Close();
        this.Close();
    }

    private void mi_edit_cut_click(object sender, EventArgs e)
    {
        GrdView1.Selection.Cut();
    }

    private void mi_edit_copy_click(object sender, EventArgs e)
    {
        GrdView1.Selection.Copy();
    }

    private void mi_edit_paste_click(object sender, EventArgs e)
    {
        GrdView1.Selection.Paste();
    }

    private void mi_edit_hiderow_click(object sender, EventArgs e)
    {
        GrdView1.AutoRedraw = false;
        for (int i = GrdView1.Selection.FirstRow; i <= GrdView1.Selection.LastRow; i++)
        {
            if (i >= 0)
            {
                GrdView1.Row(i).Hidden = true;
            }
        }
        GrdView1.AutoRedraw = true;
    }

    private void mi_edit_hidecolumn_click(object sender, EventArgs e)
    {
        GrdView1.AutoRedraw = false;
        for (int j = GrdView1.Selection.FirstCol; j <= GrdView1.Selection.LastCol; j++)
        {
            if (j >= 0)
            {
                GrdView1.Column(j).Hidden = true;
            }
        }
        GrdView1.AutoRedraw = true;
    }

    private void mi_edit_unhiderow_click(object sender, EventArgs e)
    {
        GrdView1.AutoRedraw = false;
        for (int i = GrdView1.Selection.FirstRow; i <= GrdView1.Selection.LastRow; i++)
        {
            if (i >= 0)
            {
                GrdView1.Row(i).Hidden = false;
            }
        }
        GrdView1.AutoRedraw = true;
    }

    private void mi_edit_unhidecolumn_click(object sender, EventArgs e)
    {
        GrdView1.AutoRedraw = false;
        for (int j = GrdView1.Selection.FirstCol; j <= GrdView1.Selection.LastCol; j++)
        {
            if (j >= 0)
            {
                GrdView1.Column(j).Hidden = false;
            }
        }
        GrdView1.AutoRedraw = true;
    }

    private void mi_help_manual_click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("http://www.baiqisoft.com/help/mstgridnet");
    }

    private void mi_help_about_click(object sender, EventArgs e)
    {
        GrdView1.AboutBox();
    }


    private void bt_new_click(object sender, EventArgs e)
    {
        mi_file_new.PerformClick();
    }

    private void bt_open_click(object sender, EventArgs e)
    {
        mi_file_open.PerformClick();
    }

    private void bt_save_click(object sender, EventArgs e)
    {
        mi_file_save.PerformClick();
    }

    private void bt_print_click(object sender, EventArgs e)
    {
        mi_file_print.PerformClick();
    }

    private void bt_printpreview_click(object sender, EventArgs e)
    {
        mi_file_printpreview.PerformClick();
    }

    private void bt_cut_click(object sender, EventArgs e)
    {
        mi_edit_cut.PerformClick();
    }

    private void bt_copy_click(object sender, EventArgs e)
    {
        mi_edit_copy.PerformClick();
    }

    private void bt_paste_click(object sender, EventArgs e)
    {
        mi_edit_paste.PerformClick();
    }

    private void bt_merge_click(object sender, EventArgs e)
    {
        GrdView1.Selection.Merge();
    }

    private void bt_unmerge_click(object sender, EventArgs e)
    {
        GrdView1.Selection.UnMerge();
    }

    private void bt_insertrow_click(object sender, EventArgs e)
    {
        GrdView1.Selection.InsertRows();
    }

    private void bt_insertcolumn_Click(object sender, EventArgs e)
    {
        GrdView1.Selection.InsertCols();
    }

    private void bt_deleterow_click(object sender, EventArgs e)
    {
        GrdView1.Selection.DeleteRows();
    }

    private void bt_deletecolumn_click(object sender, EventArgs e)
    {
        GrdView1.Selection.DeleteCols();
    }

    private void bt_image_Click(object sender, EventArgs e)
    {
        mi_cellformat_iamge.PerformClick();
    }

    private void bt_help_click(object sender, EventArgs e)
    {
        mi_help_manual.PerformClick();
    }

    private void bt_fontbold_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentRowIndex > -1 && GrdView1.CurrentColIndex > -1)
        {
            GrdView1.Selection.FontBold = !GrdView1.ActiveCell.FontBold;
        }
    }

    private void bt_fontitalic_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentRowIndex > -1 && GrdView1.CurrentColIndex > -1)
        {
            GrdView1.Selection.FontItalic = !GrdView1.ActiveCell.FontItalic;
        }
    }

    private void bt_fontunderline_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentRowIndex > -1 && GrdView1.CurrentColIndex > -1)
        {
            GrdView1.Selection.FontUnderline = !GrdView1.ActiveCell.FontUnderline;
        }
    }

    private void bt_fontstrikethrough_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentRowIndex > -1 && GrdView1.CurrentColIndex > -1)
        {
            GrdView1.Selection.FontStrikethrough = !GrdView1.ActiveCell.FontStrikethrough;
        }
    }

    private void bt_imagealign_ButtonClick(object sender, EventArgs e)
    {
        GrdView1.Selection.PictureAlignment = (PictureAlignment)bt_imagealign.Tag;
    }

    private void bt_imagealign_lefttop_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 0;
        bt_imagealign.Image = bt_imagealign_lefttop.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.LeftTop;
    }


    private void bt_imagealign_leftcenter_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 1;
        bt_imagealign.Image = bt_imagealign_leftcenter.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.LeftCenter;
    }

    private void bt_imagealign_leftbottom_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 2;
        bt_imagealign.Image = bt_imagealign_leftbottom.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.LeftBottom;
    }

    private void bt_imagealign_centertop_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 3;
        bt_imagealign.Image = bt_imagealign_centertop.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.CenterTop;
    }

    private void bt_imagealign_centercenter_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 4;
        bt_imagealign.Image = bt_imagealign_centercenter.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.CenterCenter;
    }

    private void bt_imagealign_centerbottom_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 5;
        bt_imagealign.Image = bt_imagealign_centerbottom.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.CenterBottom;
    }

    private void bt_imagealign_righttop_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 6;
        bt_imagealign.Image = bt_imagealign_righttop.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.RightTop;
    }

    private void bt_imagealign_rightcenter_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 7;
        bt_imagealign.Image = bt_imagealign_rightcenter.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.RightCenter;
    }

    private void bt_imagealign_rightbottom_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 8;
        bt_imagealign.Image = bt_imagealign_rightbottom.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.RightBottom;
    }

    private void bt_imagealign_stretch_click(object sender, EventArgs e)
    {
        bt_imagealign.Tag = 9;
        bt_imagealign.Image = bt_imagealign_stretch.Image;
        GrdView1.Selection.PictureAlignment = PictureAlignment.Stretch;
    }

    private void bt_textalign_ButtonClick(object sender, EventArgs e)
    {
        GrdView1.Selection.Alignment = (TextAlignment)bt_textalign.Tag;
    }

    private void bt_textalign_lefttop_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 0;
        bt_textalign.Image = bt_textalign_lefttop.Image;
        GrdView1.Selection.Alignment = TextAlignment.LeftTop;
    }

    private void bt_textalign_leftcenter_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 1;
        bt_textalign.Image = bt_textalign_leftcenter.Image;
        GrdView1.Selection.Alignment = TextAlignment.LeftCenter;
    }

    private void bt_textalign_leftbottom_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 2;
        bt_textalign.Image = bt_textalign_leftbottom.Image;
        GrdView1.Selection.Alignment = TextAlignment.LeftBottom;
    }

    private void bt_textalign_centertop_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 3;
        bt_textalign.Image = bt_textalign_centertop.Image;
        GrdView1.Selection.Alignment = TextAlignment.CenterTop;
    }

    private void bt_textalign_centercenter_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 4;
        bt_textalign.Image = bt_textalign_centercenter.Image;
        GrdView1.Selection.Alignment = TextAlignment.CenterCenter;
    }

    private void bt_textalign_centerbottom_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 5;
        bt_textalign.Image = bt_textalign_centerbottom.Image;
        GrdView1.Selection.Alignment = TextAlignment.CenterBottom;
    }

    private void bt_textalign_righttop_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 6;
        bt_textalign.Image = bt_textalign_righttop.Image;
        GrdView1.Selection.Alignment = TextAlignment.RightTop;
    }

    private void bt_textalign_rightcenter_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 7;
        bt_textalign.Image = bt_textalign_rightcenter.Image;
        GrdView1.Selection.Alignment = TextAlignment.RightCenter;
    }

    private void bt_textalign_rightbottom_click(object sender, EventArgs e)
    {
        bt_textalign.Tag = 8;
        bt_textalign.Image = bt_textalign_rightbottom.Image;
        GrdView1.Selection.Alignment = TextAlignment.RightBottom;
    }

    private void bt_cellcolor_ButtonClick(object sender, EventArgs e)
    {
        GrdView1.Selection.BackColor = bt_cellcolor.ForeColor;
    }

    private void bt_cellcolor_DropDownOpening(object sender, System.EventArgs e)
    {
        ColorDialog1.Color = bt_cellcolor.ForeColor;
        ColorDialog1.ShowDialog();
        bt_cellcolor.ForeColor = ColorDialog1.Color;
        GrdView1.Selection.BackColor = bt_cellcolor.ForeColor;
    }

    private void bt_textcolor_ButtonClick(object sender, EventArgs e)
    {
        GrdView1.Selection.ForeColor = bt_textcolor.ForeColor;
    }

    private void bt_textcolor_DropDownOpening(object sender, System.EventArgs e)
    {
        ColorDialog1.Color = bt_textcolor.ForeColor;
        ColorDialog1.ShowDialog();
        bt_textcolor.ForeColor = ColorDialog1.Color;
        GrdView1.Selection.ForeColor = bt_textcolor.ForeColor;
    }

    private void cboFontName_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        GrdView1.Selection.FontName = cboFontName.Text;
    }

    private void cboFontSize_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        GrdView1.Selection.FontSize = float.Parse(cboFontSize.Text);
    }

    private void cboFontSize_TextChanged(object sender, System.EventArgs e)
    {
        GrdView1.Selection.FontSize = float.Parse(cboFontSize.Text);
    }

    private void bt_cellborder_thin_click(object sender, EventArgs e)
    {
        m_LineStyle = LineStyle.Thin;
        bt_cellborder_thin.Checked = true;
        bt_cellborder_thick.Checked = false;
        bt_cellborder_dot.Checked = false;
    }

    private void bt_cellborder_thick_click(object sender, EventArgs e)
    {
        m_LineStyle = LineStyle.Thick;
        bt_cellborder_thin.Checked = false;
        bt_cellborder_thick.Checked = true;
        bt_cellborder_dot.Checked = false;
    }

    private void bt_cellborder_dot_click(object sender, EventArgs e)
    {
        m_LineStyle = LineStyle.Dot;
        bt_cellborder_thin.Checked = false;
        bt_cellborder_thick.Checked = false;
        bt_cellborder_dot.Checked = true;
    }

    private void bt_cellborder_color_click(object sender, EventArgs e)
    {
        ColorDialog1.Color = m_LineColor;
        ColorDialog1.ShowDialog();
        m_LineColor = ColorDialog1.Color;
    }

    private void bt_cellborder_ButtonClick(object sender, EventArgs e)
    {
        if ((int)bt_cellborder.Tag == -1)
        {
            GrdView1.AutoRedraw = false;
            GrdView1.Selection.Borders(CellBorders.Around).LineStyle = LineStyle.None;
            GrdView1.Selection.Borders(CellBorders.Inside).LineStyle = LineStyle.None;
            GrdView1.Selection.Borders(CellBorders.DiagonalUp).LineStyle = LineStyle.None;
            GrdView1.Selection.Borders(CellBorders.DiagonalDown).LineStyle = LineStyle.None;
            GrdView1.AutoRedraw = true;
        }
        else
        {
            GrdView1.AutoRedraw = false;
            GrdView1.Selection.Borders((CellBorders)bt_cellborder.Tag).LineStyle = m_LineStyle;
            GrdView1.Selection.Borders((CellBorders)bt_cellborder.Tag).Color = m_LineColor;
            GrdView1.AutoRedraw = true;
        }
    }

    private void bt_cellborder_none_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = -1;
        bt_cellborder.Image = bt_cellborder_none.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Around).LineStyle = LineStyle.None;
        GrdView1.Selection.Borders(CellBorders.Inside).LineStyle = LineStyle.None;
        GrdView1.Selection.Borders(CellBorders.DiagonalUp).LineStyle = LineStyle.None;
        GrdView1.Selection.Borders(CellBorders.DiagonalDown).LineStyle = LineStyle.None;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_around_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 0;
        bt_cellborder.Image = bt_cellborder_around.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Around).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.Around).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_left_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 1;
        bt_cellborder.Image = bt_cellborder_left.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Left).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.Left).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_top_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 2;
        bt_cellborder.Image = bt_cellborder_top.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Top).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.Top).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_right_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 3;
        bt_cellborder.Image = bt_cellborder_right.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Right).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.Right).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_bottom_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 4;
        bt_cellborder.Image = bt_cellborder_bottom.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Bottom).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.Bottom).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_diagonalup_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 5;
        bt_cellborder.Image = bt_cellborder_diagonalup.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.DiagonalUp).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.DiagonalUp).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_diagonaldown_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 6;
        bt_cellborder.Image = bt_cellborder_diagonaldown.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.DiagonalDown).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.DiagonalDown).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_inside_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 7;
        bt_cellborder.Image = bt_cellborder_inside.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.Inside).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.Inside).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_insidevetical_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 8;
        bt_cellborder.Image = bt_cellborder_insidevertical.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.InsideVertical).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.InsideVertical).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void bt_cellborder_insidehorizontal_click(object sender, EventArgs e)
    {
        bt_cellborder.Tag = 9;
        bt_cellborder.Image = bt_cellborder_insidehorizontal.Image;
        GrdView1.AutoRedraw = false;
        GrdView1.Selection.Borders(CellBorders.InsideHorizontal).LineStyle = m_LineStyle;
        GrdView1.Selection.Borders(CellBorders.InsideHorizontal).Color = m_LineColor;
        GrdView1.AutoRedraw = true;
    }

    private void GrdView1_ErrorInfo(object sender, ErrorInfoEventArgs e)
    {
        MessageBox.Show(e.LastErrorNumber.ToString() + "   " + e.LastErrorMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void mi_help_updates_click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("http://www.baiqisoft.com");
    }

    private void mi_cellformat_setwraptext_click(object sender, EventArgs e)
    {
        GrdView1.Selection.WrapText = true;
    }

    private void mi_cellformat_cancelwrap_click(object sender, EventArgs e)
    {
        GrdView1.Selection.WrapText = false;
    }

    private void mi_cellformat_lock_click(object sender, EventArgs e)
    {
        GrdView1.Selection.Locked = true;
    }

    private void mi_cellformat_unlock_click(object sender, EventArgs e)
    {
        GrdView1.Selection.Locked = false;
    }

    private void mi_cellformat_iamge_click(object sender, EventArgs e)
    {
        frmSetCellImage.Default.ShowDialog();
        GrdView1.Refresh();
    }

    private void mi_column_celltype_textbox_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.TextBox;
        }
    }

    private void mi_column_celltype_combobox_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.ComboBox;
        }
    }

    private void mi_column_celltype_checkbox_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.CheckBox;
        }
    }

    private void mi_column_celltype_calendar_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.Calendar;
        }
    }

    private void mi_column_celltype_button_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.Button;
        }
    }

    private void mi_column_celltype_hypelink_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).CellType = CellType.HyperLink;
        }
    }

    private void mi_column_editmask_any_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Any;
        }
    }

    private void mi_column_editmask_numeric_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Numeric;
        }
    }

    private void mi_column_editmask_positivenumeric_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.PositiveNumeric;
        }
    }

    private void mi_column_editmask_integers_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Integers;
        }
    }

    private void mi_column_editmask_positiveintegers_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.PositiveIntegers;
        }
    }

    private void mi_column_editmask_letter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Letter;
        }
    }

    private void mi_column_editmask_letternumeric_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.LetterNumeric;
        }
    }

    private void mi_column_editmask_upper_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Upper;
        }
    }

    private void mi_column_editmask_uppernumeric_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.UpperNumeric;
        }
    }

    private void mi_column_editmask_lower_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.Lower;
        }
    }

    private void mi_column_editmask_lowernumeric_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.LowerNumeric;
        }
    }

    private void mi_column_editmask_chqno_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).EditMask = EditMask.ChqNo;
        }
    }

    private void mi_column_textalign_lefttop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.LeftTop;
        }
    }

    private void mi_column_textalign_leftcenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.LeftCenter;
        }
    }

    private void mi_column_textalign_leftbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.LeftBottom;
        }
    }

    private void mi_column_textalign_centertop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.CenterTop;
        }
    }

    private void mi_column_textalign_centercenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.CenterCenter;
        }
    }

    private void mi_column_textalign_centerbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.CenterBottom;
        }
    }

    private void mi_column_textalign_righttop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.RightTop;
        }
    }

    private void mi_column_textalign_rightcenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.RightCenter;
        }
    }

    private void mi_column_textalign_rightbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Alignment = TextAlignment.RightBottom;
        }
    }

    private void mi_column_titlealign_lefttop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.LeftTop;
        }
    }

    private void mi_column_titlealign_leftcenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.LeftCenter;
        }
    }

    private void mi_column_titlealign_leftbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.LeftBottom;
        }
    }

    private void mi_column_titlealign_centertop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.CenterTop;
        }
    }

    private void mi_column_titlealign_centercenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.CenterCenter;
        }
    }

    private void mi_column_titlealign_centerbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.CenterBottom;
        }
    }

    private void mi_column_titlealign_righttop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.RightTop;
        }
    }

    private void mi_column_titlealign_rightcenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.RightCenter;
        }
    }

    private void mi_column_titlealign_rightbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).FixedAlignment = TextAlignment.RightBottom;
        }
    }

    private void mi_column_picalign_lefttop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.LeftTop;
        }
    }

    private void mi_column_picalign_leftcenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.LeftCenter;
        }
    }

    private void mi_column_picalign_leftbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.LeftBottom;
        }
    }

    private void mi_column_picalign_centertop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.CenterTop;
        }
    }

    private void mi_column_picalign_centercenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.CenterCenter;
        }
    }

    private void mi_column_picalign_centerbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.CenterBottom;
        }
    }

    private void mi_column_picalign_righttop_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.RightTop;
        }
    }

    private void mi_column_picalign_rightcenter_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.RightCenter;
        }
    }

    private void mi_column_picalign_rightbottom_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.RightBottom;
        }
    }

    private void mi_column_picalign_stretch_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).PictureAlignment = PictureAlignment.Stretch;
        }
    }

    private void mi_column_sort_bystring_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByString;
        }
    }

    private void mi_column_sort_bystringnocase_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByStringNoCase;
        }
    }

    private void mi_column_sort_byboolean_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByBoolean;
        }
    }

    private void mi_column_sort_bydate_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByDate;
        }
    }

    private void mi_column_sort_bynumeric_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).SortType = SortType.ByNumeric;
        }
    }

    private void mi_column_datestyle_date_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).DateStyle = DateStyle.flexDate;
        }
    }

    private void mi_column_datestyle_time_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).DateStyle = DateStyle.flexTime;
        }
    }

    private void mi_column_datestyle_datetime_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).DateStyle = DateStyle.flexDateTime;
        }
    }

    private void mi_columnformat_lock_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Locked = true;
        }
    }

    private void mi_columnformat_unlock_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentColIndex != -1)
        {
            GrdView1.Column(GrdView1.CurrentColIndex).Locked = false;
        }
    }

    private void mi_columnformat_title_click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentRowIndex != -1 && GrdView1.CurrentColIndex != -1)
        {
            frmColumn.Default.ShowDialog();
        }
    }

    private void cboShowHeaderAutoText_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        GrdView1.ShowHeaderAutoText = (HeaderAutoText)cboShowHeaderAutoText.SelectedIndex;
    }

    private void cboAutoClipboard_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AutoClipboard = bool.Parse(cboAutoClipboard.Text);
    }

    private void cboTabKeyBehavior_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.TabKeyBehavior = (TabKeyBehavior)cboTabKeyBehavior.SelectedIndex;
    }

    private void cboAllowDragDrop_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.AllowDragDrop = bool.Parse(cboAllowDragDrop.Text);
    }

    private void cboDragDropMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdView1.DragDropMode = (DragDropMode)cboDragDropMode.SelectedIndex;
    }
}
