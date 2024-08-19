using LabBilling.Forms;
using MCL;
using System.Collections;
using System.Drawing.Printing;
using System.Reflection;
// programmer added
using Utilities;
using WinFormsLibrary;

namespace LabBilling.Legacy;

public partial class DuplicateAccountsForm : Form
{
    ERR m_ERR;
    R_fin m_rFin; // wdk 20120214 move all local recordset to member level and initialize once in load.
    CAcc m_cAcc; // wdk 20120214 move all local recordset to member level and initialize once in load.
    CAcc m_cAccDupe; // wdk 20120214 move all local recordset to member level and initialize once in load.
    ToolStripControlHost m_dpFrom;
    ToolStripControlHost m_dpThru;
    ToolStripControlHost m_cboxInclude;

    private string m_strServer;
    private string m_strDatabase;
    public DuplicateAccountsForm(string[] strArgs)
    {
        if (strArgs.GetUpperBound(0) == 1)
        {
            m_strServer = strArgs[0];
            m_strDatabase = strArgs[1];
        }
        else
        {
            MessageBox.Show("Not enought arguments to start the application.", "ARGUMENT ERROR");
            Environment.Exit(13);
        }
        m_ERR = new ERR(new string[] { m_strDatabase.Contains("TEST") ? "/TEST" : "/LIVE", strArgs[0], strArgs[1] });

        InitializeComponent();
        CreateDataGridView();
        CreateDateTimes();
        //this.Text += string.Format(" -- Production Environment {0}", m_strDatabase);
    }
    private void CreateDateTimes()
    {
        int nSert = tsMain.Items.Count;
        // create the datetime controls for the From and Thru dates
        m_dpFrom = new ToolStripControlHost(new DateTimePicker());
        m_dpFrom.Text = DateTime.Now.Subtract(new TimeSpan(45, 0, 0, 0)).ToString("d");
        ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
        m_dpFrom.Control.Width = 95;
        m_dpFrom.Control.Refresh();
        m_dpFrom.Invalidate();
        tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        ToolStripLabel tslFrom = new ToolStripLabel("From: ");
        tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
        tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

        m_dpThru = new ToolStripControlHost(new DateTimePicker());
        m_dpThru.Text = DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)).ToString("d");//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
        ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
        m_dpThru.Control.Width = 95;
        m_dpThru.Control.Refresh();
        m_dpThru.Invalidate();

        ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
        tsMain.Items.Insert(tsMain.Items.Count, tslThru);
        tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
        //   tsMain.BackColor = Color.Lavender;

        tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());

        // wdk 20100322 added check box
        ToolStripLabel tslInclude = new ToolStripLabel("By Policy Number");
        m_cboxInclude = new ToolStripControlHost(new CheckBox());
        tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
        tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

        tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());

        // Checked Combo Box for fin codes
        //ToolStripLabel tslInclude = new ToolStripLabel("Fin Codes");
        //m_cboxInclude = new ToolStripControlHost(new CheckBox());
        //tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
        //tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

        //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());


        tsMain.Refresh();
    }

    private void CreateDataGridView()
    {
        dgvAccounts.Columns.Add("ACCOUNT", "ACCOUNT");
        dgvAccounts.Columns.Add("NAME", "NAME");
        dgvAccounts.Columns["NAME"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dgvAccounts.Columns.Add("TRANS_DATE", "TRANSACTION DATE");
        dgvAccounts.Columns.Add("CLIENT", "CLIENT");
        dgvAccounts.Columns.Add("FIN_CODE", "FIN CODE");
        DataGridViewCellStyle styleDob = new DataGridViewCellStyle();
        styleDob.Format = "d";
        styleDob.DataSourceNullValue = "";
        styleDob.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvAccounts.Columns.Add("DOB", "DATE OF BIRTH");
        dgvAccounts.Columns["DOB"].DefaultCellStyle = styleDob;
        dgvAccounts.Columns.Add("SSN", "SSN");
        dgvAccounts.Columns.Add("STATUS", "STATUS");
        dgvAccounts.Columns.Add("BALANCE", "BALANCE");
        DataGridViewCellStyle style = new DataGridViewCellStyle();
        style.Format = "C";
        dgvAccounts.Columns["BALANCE"].DefaultCellStyle = style;
        dgvAccounts.Columns["BALANCE"].DefaultCellStyle.NullValue = "0.00";
        dgvAccounts.Columns["BALANCE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        dgvAccounts.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvAccounts_CellFormatting);
        dgvAccounts.RowsAdded += new DataGridViewRowsAddedEventHandler(dgvAccounts_RowsAdded);
    }

    void dgvAccounts_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
        ((DataGridView)sender).Rows[e.RowIndex].HeaderCell.Value = string.Format("{0}", (e.RowIndex + 1));


    }

    void dgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        try
        {
            if (e.CellStyle.Format == "d")
            {
                if (!string.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = string.Format("{0:d}", DateTime.Parse(e.Value.ToString()));
                }
            }
            if (e.CellStyle.Format == "C")
            {
                if (!string.IsNullOrEmpty(e.Value.ToString()))
                {
                    decimal dRes = 0.00m;
                    if (decimal.TryParse(e.Value.ToString(), out dRes))
                    {
                        e.Value = dRes.ToString("c");
                    }
                    else
                    {
                        e.Value = "0";
                    }
                }

            }
        }
        catch (NullReferenceException)
        {
            // nothing loaded keep on trucking.
        }
    }

    private void frmDupAcc_Load(object sender, EventArgs e)
    {
        m_rFin = new R_fin(m_strServer, m_strDatabase, ref m_ERR);
        m_rFin.ClearMemberVariables();
        m_rFin.GetActiveRecords("1=1");
        while (m_rFin.propErrMsg != "EOF")
        {
            tssbFincodes.Items.Add(m_rFin.m_strFinCode);
            m_rFin.GetNext();
        }
        m_rFin.Dispose();
        m_cAcc = new CAcc(m_strServer, m_strDatabase, ref m_ERR);

        m_cAccDupe = new CAcc(m_strServer, m_strDatabase, ref m_ERR);

    }

    private void dgvAccounts_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {

        //LaunchAcc la = new LaunchAcc(m_strDatabase);
        //la.LaunchAccount(dgvAccounts["ACCOUNT", e.RowIndex].FormattedValue.ToString());

        string strAccount = dgvAccounts["ACCOUNT", e.RowIndex].FormattedValue.ToString();
        AccountForm frm = new AccountForm(strAccount)
        {
            MdiParent = this.ParentForm
        };
        frm.Show();
    }

    private void tsbLoad_Click(object sender, EventArgs e)
    {
        if (((CheckBox)m_cboxInclude.Control).Checked)
        {
            LoadByPolicyNumber();
            return;
        }
        dgvAccounts.Rows.Clear();
        tsProgress.Value = 0;
        ArrayList alDupAcc = new ArrayList();

        string strFinCode = "NOT IN ('A','W','X','Y','Z','CLIENT')";
        if (tssbFincodes.SelectedItem != null)
        {
            if (!string.IsNullOrEmpty(tssbFincodes.SelectedItem.ToString()))
            {
                strFinCode = string.Format("= ('{0}')", tssbFincodes.SelectedItem.ToString());
            }
        }
        // rgc/wdk 20110630 added blank ssn to filter 
        string strFilter =
            string.Format("trans_date between '{0}' and '{1}' and status not in ('closed','paid_out') " +
            " and fin_code {2} ",
            ((DateTimePicker)m_dpFrom.Control).Value.ToString("d"),
                ((DateTimePicker)m_dpThru.Control).Value.ToString("d"),
                    strFinCode);
        int nRec = m_cAcc.m_Racc.GetActiveRecords(strFilter);
        tsProgress.Maximum = nRec;
        tsProgress.ToolTipText = string.Format("{0} Records selected.", nRec);
        for (int i = 0; i < nRec; i++)
        {
            Application.DoEvents();
            tsProgress.PerformStep();
            if (DateTime.Today <= new DateTime(2013, 04, 01, 16, 30, 0))
            {
                if (m_cAcc.m_Racc.m_strAccount != "C3767704")
                {
                    continue;
                }
            }
            if (m_cAccDupe.LoadAccount(m_cAcc.m_Racc.m_strAccount))
            {
                m_cAccDupe.HasDuplicateAccount(ref alDupAcc);
                //m_cAccDupe.m_rAccDup.Dispose();
            }
            m_cAcc.m_Racc.GetNext();
        }
        foreach (string str in alDupAcc)
        {
            Application.DoEvents();
            dgvAccounts.Rows.Add(str.Split(new char[] { '|' }));
        }
        if (dgvAccounts.Rows.Count == 0)
        {
            dgvAccounts.Rows.Add("no records for this date range");
        }

    }

    private void LoadByPolicyNumber()
    {
        throw new NotImplementedException();
    }

    DataGridViewPrinter dgv = null;
    private void tsbPrintGrid_Click(object sender, EventArgs e)
    {
        PrintDocument pd = new PrintDocument();
        pd.DefaultPageSettings.Margins = new Margins(25, 25, 25, 50);
        pd.DefaultPageSettings.Landscape = true;
        pd.DocumentName = "DUPLICATE ACCOUNTS";
        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        dgv = new DataGridViewPrinter(dgvAccounts, pd, "DUPLICATE ACCOUNTS");
        pd.Print();

        pd.PrintPage -= new PrintPageEventHandler(pd_PrintPage);

    }

    void pd_PrintPage(object sender, PrintPageEventArgs e)
    {
        e.HasMorePages = dgv.DrawDataGridView(e.Graphics);

    }

    private void tsbAbout_Click(object sender, EventArgs e)
    {
        RichTextBox tb = new RichTextBox();
        tb.Dock = DockStyle.Fill;
        tb.Font = new Font("BLACK ARIAL", 12.0f);
        tb.Multiline = true;
        tb.Text = string.Format("Version {0}\r\n\n When the FinCodes Box has no selected item the form will filter out " +
            "all fin codes 'A','W','X','Y','Z','client'. \r\n\nSelecting a fin code in the combo will " +
            "filter the selection on that code only. \r\n\n" +
            "The date range from the Date time pickers is included in the filter, along with filtering " +
            "out the closed and paid out accounts.",
            Assembly.GetExecutingAssembly().GetName().Version.ToString());

        Form f = new Form();
        f.Text = "ABOUT ViewerDupAcc";
        f.Controls.Add(tb);
        f.ShowDialog();

    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        if (Environment.UserName.ToUpper() != "WKELLY")
        {
            return;
        }
        R_acc_merges lam = new R_acc_merges(m_strServer, m_strDatabase, ref m_ERR);
        foreach (DataGridViewRow dr in dgvAccounts.Rows)
        {
            Application.DoEvents();
            lam.ClearMemberVariables();
            string strFilter = string.Format("dupacc = '{0}' or account = '{0}'", dr.Cells["account"].Value.ToString());
            int nRec = lam.GetRecords(strFilter);

        }

    }
}
