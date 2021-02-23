using System.Windows.Forms;
using System;
using BaiqiSoft.GridControl;
using System.Drawing;


public partial class frmSetCellImage
{
    public frmSetCellImage()
    {
        InitializeComponent();

        //Added to support default instance behavour in C#
        if (defaultInstance == null)
            defaultInstance = this;
    }

    #region Default Instance

    private static frmSetCellImage defaultInstance;

    public static frmSetCellImage Default
    {
        get
        {
            if (defaultInstance == null)
            {
                defaultInstance = new frmSetCellImage();
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

    private void frmSetCellImage_Load(object sender, EventArgs e)
    {
        GrdView1.AutoRedraw = false;

        GrdView1.Left = 12;
        GrdView1.Top = 12;
        GrdView1.Width = 220;
        GrdView1.Height = 238;

        GrdView1.RowCount = 1;
        GrdView1.ColCount = 2;
        GrdView1.FixedCols = 0;
        GrdView1.AllowUserResizing = ResizeMode.None;
        GrdView1.Editable = false;
        GrdView1.ScrollBars = BaiqiSoft.GridControl.ScrollBars.Vertical;
        GrdView1.SelectionMode = BaiqiSoft.GridControl.SelectionMode.ByRow;
        GrdView1.FocusRect = FocusRectMode.ByRow;
        GrdView1.AllowSelection = false;
        GrdView1.HighlightHeaders = false;

        GrdView1.Cell(0, 0).Text = "Index";
        GrdView1.Cell(0, 1).Text = "Image";
        GrdView1.Column(0).FixedAlignment = TextAlignment.CenterCenter;
        GrdView1.Column(0).Alignment = TextAlignment.CenterCenter;
        GrdView1.Column(1).FixedAlignment = TextAlignment.CenterCenter;
        GrdView1.Column(1).PictureAlignment = PictureAlignment.CenterCenter;

        GrdView1.ImageList = frmDemo.Default.ImageList1;
        ShowImage();

        GrdView1.AutoRedraw = true;
    }


    private void ShowImage()
    {
        GrdView1.RowCount = 1;
        for (int i = 0; i < frmDemo.Default.ImageList1.Images.Count; i++)
        {
            GrdView1.AddItem(i.ToString(), -1);
            GrdView1.Cell(i + 1, 1).ImageIndex = i;
        }
    }


    private void cmdSet_Click(object sender, EventArgs e)
    {
        if (frmDemo.Default.GrdView1.CurrentRowIndex > -1 && frmDemo.Default.GrdView1.CurrentColIndex > -1)
        {
            frmDemo.Default.GrdView1.ActiveCell.ImageIndex = GrdView1.CurrentRowIndex - 1;
        }
        this.Close();
    }


    private void cmdClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }


    private void cmdRemove_Click(object sender, EventArgs e)
    {
        if (GrdView1.CurrentRowIndex > 0 && GrdView1.CurrentRowIndex <= frmDemo.Default.ImageList1.Images.Count)
        {
            GrdView1.AutoRedraw = false;
            frmDemo.Default.ImageList1.Images.RemoveAt(GrdView1.CurrentRowIndex - 1);
            ShowImage();
            GrdView1.AutoRedraw = true;
        }
    }


    private void cmdAdd_Click(object sender, EventArgs e)
    {
        OpenFileDialog1.FileName = "";
        OpenFileDialog1.Filter = "Icon (*.ico)|*.ico|Cursor (*.cur)|*.cur|Bitmap (*.bmp)|*.bmp|JPEG File (*.jpg)|*.jpg|GIF File (*.gif)|*.gif|All Files (*.*)|*.*";
        OpenFileDialog1.ShowDialog();
        if (OpenFileDialog1.FileName.Length > 0)
        {
            GrdView1.AutoRedraw = false;
            frmDemo.Default.ImageList1.Images.Add(Image.FromFile(OpenFileDialog1.FileName));
            ShowImage();
            GrdView1.CurrentRowIndex = GrdView1.RowCount - 1;
            GrdView1.Cell(GrdView1.CurrentRowIndex, 0).EnsureVisible();
            GrdView1.AutoRedraw = true;
        }
    }
}
