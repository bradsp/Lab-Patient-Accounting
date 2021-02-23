using System.Drawing;
using System.Windows.Forms;
using System;



partial class frmColumn
{
    public frmColumn()
    {
        InitializeComponent();

        //Added to support default instance behavour in C#
        if (defaultInstance == null)
            defaultInstance = this;
    }

    #region Default Instance

    private static frmColumn defaultInstance;

    public static frmColumn Default
    {
        get
        {
            if (defaultInstance == null)
            {
                defaultInstance = new frmColumn();
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


    private void frmColumn_Load(object sender, EventArgs e)
    {
        //Get property values
        if (frmDemo.Default.GrdView1.FixedRows > 0)
        {
            txtColumnTitle.Text =Convert.ToString ( frmDemo.Default.GrdView1.Cell(0, frmDemo.Default.GrdView1.CurrentColIndex).Text) ;
            txtColumnTitle.Enabled = true;
            txtColumnTitle.BackColor = ColorTranslator.FromOle(0xFFFFFF);
        }
        else
        {
            txtColumnTitle.Text = "";
            txtColumnTitle.Enabled = false;
            txtColumnTitle.BackColor = ColorTranslator.FromOle(0x800000);
        }
        txtColumnFormat.Text = frmDemo.Default.GrdView1.Column(frmDemo.Default.GrdView1.CurrentColIndex).FormatString;
        txtMaxLength.Text = frmDemo.Default.GrdView1.Column(frmDemo.Default.GrdView1.CurrentColIndex).MaxLength.ToString();
        txtDecimalLength.Text = frmDemo.Default.GrdView1.Column(frmDemo.Default.GrdView1.CurrentColIndex).DecimalLength.ToString();
    }


    private void cmdOK_Click(object sender, EventArgs e)
    {
        frmDemo.Default.GrdView1.AutoRedraw = false;
        if (txtColumnTitle.Enabled)
        {
            frmDemo.Default.GrdView1.Cell(0, frmDemo.Default.GrdView1.CurrentColIndex).Text = txtColumnTitle.Text;
        }
        frmDemo.Default.GrdView1.Column(frmDemo.Default.GrdView1.CurrentColIndex).FormatString = txtColumnFormat.Text;
        frmDemo.Default.GrdView1.Column(frmDemo.Default.GrdView1.CurrentColIndex).MaxLength = int.Parse(txtMaxLength.Text);
        frmDemo.Default.GrdView1.Column(frmDemo.Default.GrdView1.CurrentColIndex).DecimalLength = short.Parse(txtDecimalLength.Text);
        frmDemo.Default.GrdView1.AutoRedraw = true;
        this.Close();
    }


    private void cmdCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
