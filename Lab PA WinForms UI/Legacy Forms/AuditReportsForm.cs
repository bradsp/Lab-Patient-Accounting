using LabBilling.Logging;
using Microsoft.Data.SqlClient; // for SqlException trapping
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing; // for printing the gridview
// wdk 20090330 programmer added
using System.IO;
using System.Windows.Forms;
using WinFormsLibrary;
using Utilities;

namespace LabBilling.Legacy;

public partial class AuditReportsForm : Form
{

    private DataTable _dtRecords = null;
    private Dictionary<string, string> _dicCode;
    private ToolStripControlHost _dpFrom;
    private ToolStripControlHost _dpThru;
    private PrintDocument _viewerPrintDocument;         // The PrintDocument to be used for printing.
    private ReportGenerator _rgReport;         // The class that will do the printing process.
    private string _strReportTitle;
    private string _strQuery;
    private DataSet _dsSource;
    private readonly string _strServer;
    private readonly string _strDatabase;

    public event EventHandler<string> AccountLaunched;
    public StreamWriter errLog;

    public AuditReportsForm(string[] args)
    {

        InitializeComponent();
        // MessageBox.Show("This application only runs on LIVE data it cannot be changed to TEST data");
        if (args.Length != 2)
        {
            MessageBox.Show("Incorrect number of arguments passed. \r\nCan not continue.");
            Environment.Exit(13);
        }
        _strServer = args[0];
        _strDatabase = args[1];
        if (args[0].StartsWith("/"))
        {
            _strServer = args[0].Remove(0, 1);
        }
        if (args[1].StartsWith("/"))
        {
            _strDatabase = args[1].Remove(0, 1);
        }
        string environment = null;
        if (_strDatabase.ToUpper().Contains("TEST"))
            environment = "TEST";
        else if (_strDatabase.ToUpper().Contains("LIVE") || _strDatabase.ToUpper().Contains("PROD"))
            environment = "LIVE";

        string[] argErr = new string[3];
        argErr[0] = "/" + environment;
        argErr[1] = string.Format("/{0}", _strServer);
        argErr[2] = "/" + _strDatabase;

    }

    private void MonthlyReportsForm_Load(object sender, EventArgs e)
    {
        splitContainer1.Panel1Collapsed = true;
        Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        CreateDateTimes();
        CreatePrintDocument();
        tssbTableReports_ButtonClick(null, null);
        m_dgvReport.ContextMenuStrip = GetContextMenu();
        using (SqlConnection conn = new(Program.AppEnvironment.ConnectionString))
        {
            SqlDataAdapter adapter = new()
            {
                SelectCommand = new SqlCommand(
                "select cli_mnem,cli_nme from client where deleted = 0 order by cli_mnem", conn)
            };
            DataTable dt = new();
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                clientsToolStripComboBox.Items.Add(string.Format("{0} - {1}",
                    dr["cli_mnem"], dr["cli_nme"]));
            }
        }
        ((ComboBox)clientsToolStripComboBox.Control).DisplayMember = "cli_mnem";
    }

    void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        Log.Instance.Error($"APPLICATION EXCEPTION -- [{e.Exception.Message}]", e.Exception);
    }

    private void CreatePrintDocument()
    {
        _viewerPrintDocument = new PrintDocument(); // create a new print document each time so the .PrintPage handler only gets handled once.
    }

    private void CreateDateTimes()
    {
        int nSert = tsMain.Items.Count;
        // create the datetime controls for the From and Thru dates
        _dpFrom = new ToolStripControlHost(new DateTimePicker())
        {
            Text = DateTime.Now.Subtract(new TimeSpan((DateTime.Now.Day - 1), 0, 0, 0)).ToString("d")
        };
        ((DateTimePicker)_dpFrom.Control).Format = DateTimePickerFormat.Short;
        _dpFrom.Control.Width = 95;
        _dpFrom.Control.Refresh();
        _dpFrom.Invalidate();
        tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        ToolStripLabel tslFrom = new ToolStripLabel("From: ");
        tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
        tsMain.Items.Insert(tsMain.Items.Count, _dpFrom);

        _dpThru = new ToolStripControlHost(new DateTimePicker());
        _dpThru.Text = DateTime.Now.AddDays((DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day)).ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
        ((DateTimePicker)_dpThru.Control).Format = DateTimePickerFormat.Short;
        _dpThru.Control.Width = 95;
        _dpThru.Control.Refresh();
        _dpThru.Invalidate();

        ToolStripLabel tslThru = new("Thru: ");
        tsMain.Items.Insert(tsMain.Items.Count, tslThru);
        tsMain.Items.Insert(tsMain.Items.Count, _dpThru);
        // tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        tsMain.Refresh();
    }

    private void tsmi80299_Click(object sender, EventArgs e)
    {
        _strReportTitle = $"CPT code beginning with J or 80299 as the cpt code and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        currentReportTitle.Text = _strReportTitle;
        tsslReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = string.Format("SELECT TOP (100) PERCENT acc.fin_code, acc.account, acc.pat_name, amt.cpt4, acc.trans_date, chrg.qty " +
                                   "FROM chrg INNER JOIN " +
                                   " amt ON chrg.chrg_num = amt.chrg_num INNER JOIN " +
                                   " acc ON chrg.account = acc.account " +
                                   " WHERE  (amt.cpt4 LIKE 'j%' OR amt.cpt4 = '80299') " +
                                   " AND (acc.trans_date BETWEEN CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102)) " +
                                   "ORDER BY acc.pat_name", _dpFrom.Text, _dpThru.Text);
        LoadGrid();
    }

    private void LoadGrid()
    {
        tspbCount.Value = 0;
        tspbCount.Step = 1;
        tspbCount.Increment(10);
        tspbCount.ToolTipText = "Working";

        //    m_dsSource = new DataSet("PrintDataSet");
        _dtRecords = new DataTable("PrintDataTable");

        Log.Instance.Trace("After setting grids Datasource");

        SelectRows(_strQuery);

        try
        {
            Log.Instance.Debug($"DataTable = [{_dtRecords.TableName}]");
            if (_dtRecords == null)
            {
                m_dgvReport.DataSource = null;
                Log.Instance.Debug("m_dsSource.Tables.Count == 0 so return.");
                return;
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"m_dsSource.Tables[0].TableName threw an exception. [{ex.Message}]", ex);
        }

        Log.Instance.Trace("Before setting grids Datasource");
        BindingSource bs = new();

        try
        {
            //m_dgvReport.DataSource = m_dtRecords;
            bs.DataSource = _dtRecords;
            m_dgvReport.DataSource = bs;
        }
        catch (Exception)
        {
            m_dgvReport.DataSource = _dtRecords;
        }

        if (m_dgvReport.Rows.Count > 0)
        {
            SetRowNumbers();
        }
        Log.Instance.Trace("After SetRowNumbers()");

        tsslRecords.Text = string.Format("{0} rows loaded.", m_dgvReport.Rows.Count);
        try
        {
            MessageBox.Show(string.Format("{0} rows loaded.", m_dgvReport.Rows.Count), "LOAD FINISHED");
        }
        catch (Exception exmb)
        {
            Log.Instance.Error(exmb.Message, exmb);
        }

    }

    private void SetRowNumbers()
    {
        int rowNum = 1;
        foreach (DataGridViewRow row in m_dgvReport.Rows)
        {

            row.HeaderCell.Value = rowNum.ToString();
            rowNum++;
            tspbCount.Increment(1);
            Application.DoEvents();
        }
        Log.Instance.Debug($"m_dgvReport has [{m_dgvReport.Rows.Count}] rows and Progress bar max value is [{tspbCount.Maximum}]");
        tsslRecords.Text = $"{m_dgvReport.Rows.Count} rows loaded.";
    }

    private void SelectRows(string m_strQuery)
    {
        Log.Instance.Trace("Entered SelectRows()");
        using (SqlConnection connection = new(Program.AppEnvironment.ConnectionString))
        {
            try
            {
                SqlDataAdapter adapter = new();
                adapter.SelectCommand = new SqlCommand(m_strQuery, connection);

                int nRec = adapter.Fill(_dtRecords);

                _dsSource = new DataSet();
                adapter.Fill(_dsSource);
            }
            catch (SqlException se)
            {
                Log.Instance.Error("se.EXCEPTION in SELECTROWS()", se);
                if (string.IsNullOrEmpty(niMsg.BalloonTipText) || niMsg.BalloonTipTitle != "SQLEXCEPTION")
                {
                    niMsg.BalloonTipText = "Server is running slow.";
                    niMsg.BalloonTipTitle = "SQLEXCEPTION";
                    niMsg.Visible = true;
                    niMsg.ShowBalloonTip(30);
                    if (m_strQuery.Contains("TOP("))
                    {
                        decimal d = decimal.Parse(m_strQuery.Substring(m_strQuery.IndexOf("TOP(")));
                        d /= 2;
                    }
                    SelectRows(m_strQuery); // recursive call use the blank text as a get out of loop free variable.
                }
                else
                {
                    niMsg.BalloonTipText = "Retry query failed. Select a shorter date range and retry.";
                    niMsg.BalloonTipTitle = "REQUERY NEEDED";
                    niMsg.ShowBalloonTip(30);
                }
            }
            catch (InvalidOperationException ioe)
            {
                //MessageBox.Show(ioe.Message);
                Log.Instance.Error("ioe.EXCEPTION in SELECTROWS()", ioe);
                niMsg.BalloonTipText = ioe.Message;
                niMsg.BalloonTipTitle = "InvalidOperationException";
                niMsg.Visible = true;
                niMsg.ShowBalloonTip(30);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("ex.EXCEPTION in SELECTROWS()", ex);
                niMsg.BalloonTipText = ex.Message;
                niMsg.BalloonTipTitle = "GENERAL EXCEPTION";
                niMsg.Visible = true;
                niMsg.ShowBalloonTip(30);
            }
        }
        Log.Instance.Debug("Leaving SelectRows()");
    }

    void adapter_Disposed(object sender, EventArgs e)
    {
        Log.Instance.Trace("adapter disposed");
    }

    void adapter_FillError(object sender, FillErrorEventArgs e)
    {
        Log.Instance.Trace("INSIDE adapter_FillError");
        Log.Instance.Trace(sender.GetType().ToString());
        Log.Instance.Debug(e.Errors.Message);
    }

    private void tsbtnPrint_Click(object sender, EventArgs e)
    {
        int nRows = m_dgvReport.Rows.Count;
        if (nRows < 1)
        {
            MessageBox.Show("Must have data in the grid to print.");
            return;
        }
        bool bLandscape = false;
        if (MessageBox.Show("PRINT LANDSCAPE", "PRINTER PREFERENCES", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            bLandscape = true;
        }
        // create our reportgenerator and assign event handlers
        string strWhere = string.Empty;
        _viewerPrintDocument.DefaultPageSettings.Landscape = bLandscape;
        _rgReport = new ReportGenerator(m_dgvReport, _viewerPrintDocument, _strReportTitle, _strDatabase);

        _viewerPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(_rgReport.MyPrintDocument_PrintPage);
        _viewerPrintDocument.DocumentName = _strReportTitle;

        _viewerPrintDocument.Print();
        _viewerPrintDocument.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(_rgReport.MyPrintDocument_PrintPage);
    }

    private void tsmi59Modis_Click(object sender, EventArgs e)
    {

        _strReportTitle = $"Accounts with combination of GC (87591) CHL (87491) and Group B (87149) and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = string.Format("with cte (account, cdm, cpt4, [count], qty) " +
                "as  " +
                "(  " +
                "    select  top(1000000)ISNULL(vw_chrgdetail.account, 'GRAND TOTAL') as [ACCOUNT], " +
                "            ISNULL(cdm, NULL) as [CDM], " +
                "            ISNULL(cpt4, NULL) as [CPT4], " +
                "            count(cpt4) as [Count of Cpt4], " +
                "            sum(qty)as [Total for CPT4] " +
                "     from vw_chrgdetail " +
                "    inner join acc on acc.account = vw_chrgdetail.account and acc.status not in ('paid_out','closed') and acc.trans_date >= '{0}' " +
                "    right outer  join client on  acc.cl_mnem = client.cli_mnem and client.type not in (0,6)  " +
                "    where cdm not in ('CBILL','') and cpt4 not in ('NONE','') and cdm is not null and cpt4 is not null " + //and credited = 0
                "            and modi is null " +
                "    group by vw_chrgdetail.account, cdm, cpt4 with rollup " +
                "    having count(cpt4) > 1 " +
                "    order by vw_chrgdetail.account, cdm, cpt4 " +
                ") " +
                "select cte.*, acc.fin_code " +
                "from cte  " +
                "inner join acc on acc.account = cte.account and acc.trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                "where cpt4 is not null   " +
                "and cpt4 not in ('80101','83896','86256','86003')  " +
                "and qty > 1 " +
                "order by fin_code, trans_date, account", _dpFrom, _dpThru);
        LoadGrid();
    }

    private void modifiersMultipleCpt4ToolStripMenuItem_Click(object sender, EventArgs e)
    {

        _strReportTitle = $"Accounts with Multiple CPT4 combinations of GC (87591) CHL (87491) and Group B (87149) and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = "with MultiCpt4(account, cpt4_87149, modi_87149, " +
            "        cpt4_87591, modi4_87591,  " +
            "	    cpt4_87491, modi4_87491,  " +
            "        service_date, fin_code) as " +
            "( " +
            "    select distinct a.account, a.cpt4, a.modi, b.cpt4, b.modi, c.cpt4, c.modi, convert(varchar(10),acc.trans_date,101), acc.fin_code from vw_chrgdetail a " +
            "    inner join vw_chrgdetail b on a.account = b.account and b.cpt4 = '87591' " +
            "    inner join vw_chrgdetail c on c.account = b.account and c.cpt4 = '87491' " +
            $"    inner join acc on acc.account = a.account and acc.status not in ('paid_out','closed') and acc.trans_date >= '{_dpFrom}' " +
            "    right outer  join client on  acc.cl_mnem = client.cli_mnem and client.type not in (0,6)  " +
            "    where a.cdm not in ('CBILL','') and a.cpt4 not in ('NONE','') and a.cdm is not null and a.cpt4 is not null" +
            "    and  a.cpt4 = '87149' " +
            ") " +
            "select * from MultiCpt4  " +
            $"where coalesce (modi4_87591, modi4_87491,modi_87149) is null and service_date between CONVERT(DATETIME, '{_dpFrom}', 102) AND CONVERT(DATETIME, '{_dpThru}', 102) " +
            "order by fin_code, service_date, account";
        LoadGrid();
    }
    private int nFilterColumn;
    private void dgvReport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            nFilterColumn = e.ColumnIndex;
            m_dgvReport.ContextMenuStrip.Show(m_dgvReport, MousePosition);
        }
        SetRowNumbers();
    }

    private ContextMenuStrip GetContextMenu()
    {
        ContextMenuStrip cm = new ContextMenuStrip();

        //Can create STATIC custom menu items if exists here...          
        ToolStripMenuItem m1, m2, m3, m4, mHeader;
        mHeader = new ToolStripMenuItem("REPORTS MENU");
        m1 = new ToolStripMenuItem("Visualize Changes");
        m1.Click += new EventHandler(m1_Click);
        m2 = new ToolStripMenuItem("Filter Data");
        m2.Click += new EventHandler(m2_Click);
        m3 = new ToolStripMenuItem("Hide Column");
        m3.Click += new EventHandler(m3_Click);
        m4 = new ToolStripMenuItem("Show Hidden Columns");
        m4.Click += new EventHandler(m4_Click);

        //Can add functionality for the custom menu items here...
        cm.Items.Add(mHeader);
        cm.Items.Add(m1);
        cm.Items.Add(m2);
        cm.Items.Add(m3);
        cm.Items.Add(m4);

        return cm;
    }

    void m4_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewColumn dc in m_dgvReport.Columns)
        {
            dc.Visible = true;
            Application.DoEvents();
        }
    }

    void m3_Click(object sender, EventArgs e)
    {
        m_dgvReport.Columns[nFilterColumn].Visible = false;
    }

    void m1_Click(object sender, EventArgs e)
    {
        VisualizeChanges();
    }

    void m2_Click(object sender, EventArgs e)
    {
        FilterDataGrid();
        SetRowNumbers();
    }

    /// <summary>
    /// wdk 20100928 colors the differences from one row to the next
    /// </summary>
    private void VisualizeChanges()
    {
        DataGridViewRow drOld = null;
        DataGridView dgvHist = m_dgvReport;
        foreach (DataGridViewRow dr in dgvHist.Rows)
        {
            Application.DoEvents();
            if (dr.IsNewRow)
            {
                break;
            }
            drOld = dr;
            foreach (DataGridViewColumn dc in dgvHist.Columns)
            {
                // set the cells backcolor style for assignmment later.
                DataGridViewCellStyle dgvcsOld = dc.DefaultCellStyle;
                DataGridViewCellStyle dgvcs = new()
                {
                    BackColor = Color.PeachPuff
                };

                DataGridViewCell dgvc = dgvHist[dc.Index, dr.Index];
                try
                {
                    if (dgvHist[dc.Index, dr.Index].FormattedValue.ToString() != dgvHist[dc.Index, dr.Index + 1].FormattedValue.ToString())
                    {
                        if (dgvHist[dc.Index, dr.Index].FormattedValue.ToString() != dgvHist[dc.Index, dr.Index + 1].FormattedValue.ToString())
                            dgvc.Style = dgvcs;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // dr.index + 1 will over run the grid just continue becuase its the last in the grid.
                }
            }
        }
    }

    private void FilterDataGrid()
    {
        FormResponse f = new();
        foreach (DataGridViewRow dr in m_dgvReport.Rows)
        {
            Application.DoEvents();
            string strText = dr.Cells[nFilterColumn].FormattedValue.ToString();
            if (!f.clbFilter.Items.Contains(strText))
            {
                f.clbFilter.Items.Add(strText);
            }
        }
        string strResponse = null;

        if (f.ShowDialog() == DialogResult.Yes)
        {
            foreach (string str in f.clbFilter.CheckedItems)
            {
                strResponse += $"'{str}',";
            }
            if (!string.IsNullOrEmpty(strResponse))
            {
                int nli = strResponse.LastIndexOf(',');
                int nlen = strResponse.Length;

                strResponse = strResponse.Remove(strResponse.LastIndexOf(','));
            }
        }

        string strColText = m_dgvReport.Columns[nFilterColumn].HeaderText;
        BindingSource bs = new(m_dgvReport.DataSource, strColText);
        bs.DataMember = m_dgvReport.DataMember;
        if (string.IsNullOrEmpty(strResponse))
        {
            bs.RemoveFilter();
        }
        else
        {
            bs.Filter = $"{strColText} in ({strResponse})";
        }
        m_dgvReport.DataSource = bs;

    }

    private void mailerErrorsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _strReportTitle = $"Accounts with Mailer errors and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = string.Format("with cte " +
                                    "as " +
                                    "( " +
                                    "select acc.account from acc  " +
                                    "inner join pat on acc.account = pat.account  " +
                                    "where acc.fin_code <> 'E' and pat.mailer in  ('1') and acc.trans_date >= '{0}' and original_fincode <> 'E' " +
                                    "and acc.status not in ('closed','paid_out') " +
                                    ") " +
                                    "select * from acc " +
                                    "inner join cte on cte.account = acc.account", _dpFrom, _dpThru);
        LoadGrid();
    }

    private void accountsWithNoPatientRecordToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _strReportTitle = $"Accounts with no Patient Records and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = string.Format("select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date from acc " +
                                    "left outer join pat on pat.account = acc.account " +
                                    "where pat.account is null and acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed') and acc.fin_code not in ('X','Y') " +
                                    "and trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                                    "order by acc.account", _dpFrom, _dpThru);
        LoadGrid();

    }

    private void accountsWithNoInsuranceRecordToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _strReportTitle = $"Accounts with no Insurance Records and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = string.Format("select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date from acc " +
                                    "left outer join ins on ins.account = acc.account " +
                                    "where ins.account is null and acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed') and acc.fin_code not in ('X','Y') " +
                                    "and trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                                    "order by acc.account", _dpFrom, _dpThru);
        LoadGrid();
    }

    private void accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        /*
         * select top(10)ins.account,acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date, ins.fin_code from acc
        inner join ins on ins.account = acc.account and ins.ins_a_b_c = 'A' and ins.fin_code <> acc.fin_code
        where acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed')
        and trans_date between '01/01/2009 00:00' and '03/31/2009 23:59'
        order by acc.account
        */
        _strReportTitle = string.Format("Accounts where Insurance and account fin codes do not match and the date of service is between {0} and {1}", _dpFrom.Text, _dpThru.Text);
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        // set up the query from the argument
        _strQuery = string.Format("select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, ins.fin_code, acc.trans_date from acc " +
                                    "inner join ins on ins.account = acc.account and ins.ins_a_b_c = 'A' and ins.fin_code <> acc.fin_code " +
                                    "where acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed') " +
                                    "and trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                                    "order by acc.account", _dpFrom, _dpThru);
        LoadGrid();
    }

    private void aBNsReportedToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _strReportTitle = string.Format("Accounts with ABN's and the date of service is between {0} and {1}", _dpFrom.Text, _dpThru.Text);
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        _strQuery = string.Format("with cte (account, cli_mnem, coll_date, pat_name, rowguid) " +
                                    "   as " +
                                    "   ( " +
                                    "   select s.account,  w.cli_mnem, w.coll_date, w.pat_lname+', '+w.pat_fname, w.rowguid from wreq w " +
                                    "   inner join FORD.dbo.services_status s on s.wreq_rowguid = w.rowguid " +
                                    "   where s.account is not null " +
                                    "   ) " +
                                    "   select c.account, c.cli_mnem, c.coll_date, c.pat_name, w.test_mnem from cte c " +
                                    "   inner join worders w on c.rowguid = w.wreq_rowguid and w.abn = 1 " +
                                    "   and c.coll_date between convert(datetime,'{0} 00:00',102) and convert(datetime,'{1} 23:59', 102) " +
                                    "   order by c.account", _dpFrom, _dpThru);
        LoadGrid();
    }

    private void tsmi_Contains_Click(object sender, EventArgs e)
    {
        /*
        select  chrg.account --chrg.chrg_num,qty, chrg.cdm, amt.cpt4 , amt.modi, amt.modi2 
        from chrg
        inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out')
        inner join amt on chrg.chrg_num = amt.chrg_num
        where cdm <> 'cbill' and chrg.credited = 0 --and (amt.modi <> '59' or amt.modi2 <> '59')
        and service_date between '01/01/2009 00:00' and '03/31/2009 23:59' and amt.cpt4 in ('82575')

        intersect

        select  chrg.account --chrg.chrg_num,qty, chrg.cdm, amt.cpt4 , amt.modi, amt.modi2 
        from chrg
        inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out')
        inner join amt on chrg.chrg_num = amt.chrg_num
        where cdm <> 'cbill' and chrg.credited = 0 --and (amt.modi <> '59' or amt.modi2 <> '59')
        and service_date between '01/01/2009 00:00' and '03/31/2009 23:59' and amt.cpt4 in ('82565')
         * 
         * 
         * and (amt.modi <> '59' or amt.modi2 <> '59')
         * and (amt.modi <> '59' or amt.modi2 <> '59')
        */
        string strTag = ((ToolStripMenuItem)sender).Tag.ToString();
        string[] strCpt4s = strTag.Split(new char[] { '|' });
        _strReportTitle = $"Accounts containing {strCpt4s[0]} and {strCpt4s[1]} and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        _strQuery = string.Format("select  chrg.account " +
                                    "from chrg " +
                                    "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
                                    "inner join amt on chrg.chrg_num = amt.chrg_num " +
                                    "where cdm <> 'cbill' and chrg.credited = 0 " +
                                    "and service_date between '{0} 00:00' and '{1} 23:59' and amt.cpt4 in ('{2}') " +
                                    "intersect " +
                                    "select  chrg.account " +
                                    "from chrg " +
                                    "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
                                    "inner join amt on chrg.chrg_num = amt.chrg_num " +
                                    "where cdm <> 'cbill' and chrg.credited = 0  " +
                                    "and service_date between '{3} 00:00' and '{4} 23:59' and amt.cpt4 in ('{5}')", _dpFrom, _dpThru, strCpt4s[1], _dpFrom, _dpThru, strCpt4s[0]);
        LoadGrid();
    }

    private void tsmi_C3_C4_Click(object sender, EventArgs e)
    {
        string strTag = ((ToolStripMenuItem)sender).Tag.ToString();
        string[] strCpt4s = strTag.Split(new char[] { '|' });
        _strReportTitle = string.Format("Accounts containing C3 and C4 and the date of service is between {0} and {1}", _dpFrom.Text, _dpThru.Text);
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        _strQuery = string.Format("select  chrg.account " +
                                    "from chrg " +
                                    "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
                                    "inner join amt on chrg.chrg_num = amt.chrg_num " +
                                    "where cdm <> 'cbill' and chrg.credited = 0 and chrg.cdm = '5606106' " +
                                    "and service_date between '{0} 00:00' and '{1} 23:59' and amt.cpt4 in ('{2}') " +
                                    "intersect " +
                                    "select  chrg.account " +
                                    "from chrg " +
                                    "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
                                    "inner join amt on chrg.chrg_num = amt.chrg_num " +
                                    "where cdm <> 'cbill' and chrg.credited = 0 and chrg.cdm = '5602376' " +
                                    "and service_date between '{3} 00:00' and '{4} 23:59' and amt.cpt4 in ('{5}')", _dpFrom, _dpThru, strCpt4s[1], _dpFrom, _dpThru, strCpt4s[0]);
        LoadGrid();
    }

    private void accountsWithSpecificCpt_Click(object sender, EventArgs e)
    {
        splitContainer1.Panel1Collapsed = false;

        Label cptLabel = new();
        cptLabel.Text = "Type cpt code and press enter";
        cptLabel.Dock = DockStyle.Fill;
        ListBox cptListBox = new();
        cptListBox.Dock = DockStyle.Fill;
        Button cptRunReportButton = new();
        TableLayoutPanel tlp = new();
        TextBox cptEntryTextBox = new();
        cptEntryTextBox.Dock = DockStyle.Fill;

        cptEntryTextBox.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                cptListBox.Items.Add(cptEntryTextBox.Text);
                cptEntryTextBox.Text = string.Empty;
                cptEntryTextBox.Focus();
            }
        };
        cptRunReportButton.Text = "Run";
        cptRunReportButton.Click += (object sender, EventArgs e) =>
        {
            List<string> cpt4s = new();
            foreach (var item in cptListBox.Items)
            {
                cpt4s.Add(item.ToString());
            }
            splitContainer1.Panel1Collapsed = true;
            AccountsByCpt4(cpt4s);
            splitContainer1.Panel1.Controls.Remove(tlp);
        };

        tlp.Controls.Add(cptLabel, 0, 0);
        tlp.Controls.Add(cptEntryTextBox, 0, 1);
        tlp.Controls.Add(cptListBox, 0, 2);
        tlp.Controls.Add(cptRunReportButton, 0, 3);
        tlp.Dock = DockStyle.Fill;
        splitContainer1.Panel1.Controls.Add(tlp);

    }

    private void AccountsByCpt4(List<string> cpt4s)
    {

        if (cpt4s.Count < 2)
        {
            MessageBox.Show("Not enough Cpt4's to search with.");
            return;
        }
        string strFirstCpt4 = cpt4s[0].ToString();
        string strRemainingCpt4s = string.Empty;
        for (int i = 1; i < cpt4s.Count; i++)
        {
            strRemainingCpt4s += string.Format("{0}{1}{2}",
                                    i == 1 ? "" : ",'",
                                        cpt4s[i].ToString(),
                                        i == (cpt4s.Count - 1) ? "" : "'");
        }

        _strReportTitle = $"Accounts containing {strFirstCpt4} and {strRemainingCpt4s} and the date of service is between {_dpFrom.Text} and {_dpThru.Text}";
        tsslReportTitle.Text = _strReportTitle;
        currentReportTitle.Text = _strReportTitle;
        _strQuery = string.Format("select  chrg.account " +
            "from chrg " +
            "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
            "inner join chrg_details amt on chrg.chrg_num = amt.chrg_num " +
            "where cdm <> 'CBILL' and chrg.credited = 0 " +
            "and service_date between '{0} 00:00' and '{1} 23:59' and amt.cpt4 in ('{2}') " +
            "intersect " +
            "select  chrg.account " +
            "from chrg " +
            "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
            "inner join chrg_details amt on chrg.chrg_num = amt.chrg_num " +
            "where cdm <> 'CBILL' and chrg.credited = 0  " +
            "and service_date between '{3} 00:00' and '{4} 23:59' and amt.cpt4 in ('{5}')", _dpFrom, _dpThru, strFirstCpt4, _dpFrom, _dpThru, strRemainingCpt4s);
        LoadGrid();
    }

    private void GetReport(List<string> cdms)
    {
        if (cdms.Count < 2)
        {
            _strReportTitle = $"Accounts containing {cdms[0]} with date of service between {_dpFrom.Text} and {_dpThru.Text}";
            currentReportTitle.Text = _strReportTitle;
            _strQuery =
                $"with cte (account, cdm)" +
                "as" +
                "(" +
                "	select account, cdm from chrg " +
                $"  where service_date between '{_dpFrom} 00:00' and '{_dpThru} 23:59' " +
                $"	and cdm = {cdms[0]} and credited = 0 " +
                ")" +
                "select distinct cte.account, cte.cdm as [FIRST CDM], convert(varchar(10),service_date,101) as [Service Date]" +
                "from cte " +
                "join chrg on chrg.account = cte.account ";
        }
        else
        {
            string strFirstCDM = cdms[0];
            string strRemainingCdms = string.Empty;
            for (int i = 1; i < cdms.Count; i++)
            {
                strRemainingCdms += string.Format("{0}{1}{2}",
                                        i == 1 ? "" : ",'",
                                            cdms[i].ToString(),
                                            i == (cdms.Count - 1) ? "" : "'");
            }


            _strReportTitle = $"Accounts containing {strFirstCDM} and {strRemainingCdms} with date of service between {_dpFrom.Text} and {_dpThru.Text}";
            currentReportTitle.Text = _strReportTitle;
            tsslReportTitle.Text = _strReportTitle;
            _strQuery =
                @$"with cte (account, cdm)" +
                @$"as" +
                @$"(" +
                @$"	select account, cdm from chrg " +
                @$"   where service_date between '{_dpFrom} 00:00' and '{_dpThru} 23:59' " +
                @$"	and cdm = '{strFirstCDM}' and credited = 0 " +
                @$")" +
                @$"select distinct cte.account, cte.cdm as [FIRST CDM], chrg.cdm as [SECOND CDM], convert(varchar(10),service_date,101) as [Service Date]" +
                @$"from cte " +
                @$"join chrg on chrg.account = cte.account " +
                @$"where service_date between '{_dpFrom} 00:00' and '{_dpThru} 23:59' " +
                @$"and chrg.cdm in ('{strRemainingCdms}') and credited = 0";
        }
        tsslReportTitle.Text = _strReportTitle;
        LoadGrid();
    }

    private void tssbTableReports_ButtonClick(object sender, EventArgs e)
    {
        tableReportsToolStripItem.DropDownItems.Clear();
        _dicCode = new();
        string strQuery = "select * from dbo.Monthly_Reports order by button, report_title";

        SqlDataAdapter sda = new(strQuery, Program.AppEnvironment.ConnectionString);
        DataSet ds = new("Monthly_Reports");
        try
        {
            sda.Fill(ds, "Monthly_Reports");
        }
        catch (SqlException se)
        {
            MessageBox.Show(se.Message);
            return;
        }

        string strButton = "";
        ToolStripMenuItem tsiBtn = null;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Application.DoEvents();
            if (strButton != dr["button"].ToString())
            {
                strButton = dr["button"].ToString();
                tsiBtn = (ToolStripMenuItem)tableReportsToolStripItem.DropDownItems.Add(strButton);
            }
            ToolStripMenuItem tsi = (ToolStripMenuItem)tsiBtn.DropDownItems.Add(dr["mi_name"].ToString());
            tsi.Tag = dr["child_button"].ToString();

            _dicCode.Add(dr["mi_name"].ToString(), dr["sql_code"].ToString());
            tsi.Click += new EventHandler(tsi_Click);
            tsiBtn.DropDownItems.Add(tsi);
        }
    }

    void tsi_Click(object sender, EventArgs e)
    {
        string strButton = ((ToolStripMenuItem)sender).Text;
        string strCode = null;
        if (!_dicCode.TryGetValue(strButton, out strCode))
        {
            MessageBox.Show("Not valid");
        }
        // {0} = from date
        // {1} = thru date
        // {2} = cdm
        _strReportTitle = strButton;
        currentReportTitle.Text = _strReportTitle;
        tsslReportTitle.Text = _strReportTitle;
        _strQuery = strCode.Replace("{0}", _dpFrom.Text).Replace("{1}", _dpThru.Text);

        Log.Instance.Debug($"Date from = [{_dpFrom}], Date thru = [{_dpThru}]");

        if (_strQuery.Contains(" cl_mnem in ({2})") && !string.IsNullOrEmpty(clientsToolStripComboBox.SelectedItem.ToString()))
        {
            string[] strParts = clientsToolStripComboBox.SelectedItem.ToString().Split(['-']);
            _strQuery = _strQuery.Replace("{2}", $"'{strParts[0].Trim()}'");
        }
        else if (_strQuery.Contains("{2}"))
        {
            FormDataCollection f = new();
            if (f.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Cannot process this query with out data");
                tableReportsToolStripItem.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
                return;
            }
            Log.Instance.Debug($"Dialog Result = [{f.DialogResult}]");
            if (string.IsNullOrEmpty(f.tbData.Text))
            {
                MessageBox.Show("Cannot process this query with out data");
                tableReportsToolStripItem.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
                return;
            }

            string strCdm = null;
            string[] strCDMs = f.tbData.Text.Split([","], StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in strCDMs)
            {
                Application.DoEvents();
                strCdm += string.Format("'{0}',", str);
            }
            strCdm = strCdm.Remove(strCdm.LastIndexOf(','));
            _strQuery = _strQuery.Replace("{2}", strCdm);

        }
        Log.Instance.Debug(string.Format("QUERY FILTER = [{0}]", _strQuery));
        tableReportsToolStripItem.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
        Application.DoEvents();

        try
        {
            LoadGrid();
        }
        catch (Exception ex)
        {

            Log.Instance.Error("Error in LoadGrid", ex);
            MessageBox.Show(ex.Message, "ERROR");
        }
        clientsToolStripComboBox.SelectedIndex = -1;
        Log.Instance.Trace("Leaving tsi_Click.");
        tsslDisplayReportTitle.Text = string.Format(" | {0}", strButton);
    }

    void tssbTableReports_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
        string strButton = e.ClickedItem.ToString();

        MessageBox.Show($"Select a Report from menu [{strButton}].");
        return;
    }

    private void tssbExcel_ButtonClick(object sender, EventArgs e)
    {
        // wdk 20110920 try/catch added after watching Jan's machine kill the application when
        // no data is on the grid. This does not cause my machine to fail but hers won't load
        // the womans clinic report either.
        try
        {
            if (tsmicbExcelDirectory.Text.Length == 0)
            {
                excelToolStripButton.ShowDropDown();
                tsmiExcelDirectory.ShowDropDown();
                tsmicbExcelDirectory.Focus();
                return;
            }
            if (tsmiExcelFile.Text.Length == 0)
            {
                excelToolStripButton.ShowDropDown();
                tsmiExcelFile.ShowDropDown();
                tstbExcelFileName.Focus();

            }
            WinFormsLibrary.DataGridToExcel.Export(m_dgvReport, string.Format(@"{0}{1}{2}.xlsx", tsmicbExcelDirectory.Text, tsmicbExcelDirectory.Text.EndsWith(@"\") ? "" : @"\", tstbExcelFileName.Text), tstbExcelWorkSheetName.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.InnerException.ToString(), "Error creating Excel");
            Log.Instance.Error($"QUERY FILTER = [{_strQuery}]", ex);
        }
    }

    private void dgvReport_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
        Application.DoEvents();
    }

    private void frmReports_FormClosing(object sender, FormClosingEventArgs e)
    {
        Log.Instance.Debug(sender.GetType().ToString());
        Log.Instance.Debug($"CloseReason = {e.CloseReason}");
    }

    private void dgvReport_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex == -1)
        {
            return;
        }
        if (e.Button == MouseButtons.Right)
        {
            Form f = new()
            {
                Size = new Size(800, 200),
                Text = string.Format("SELECTED CELL CONTENTS - {0} - Column {1}",
                    m_dgvReport.Rows[e.RowIndex].Cells["account"].Value.ToString(),
                    m_dgvReport.Columns[e.ColumnIndex].Name)
            };
            TextBox tb = new()
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = m_dgvReport[e.ColumnIndex, e.RowIndex].Value.ToString(),
                SelectionLength = 0
            };
            f.Controls.Add(tb);
            f.Show();
        }
        Application.DoEvents();
    }

    private void m_dgvReport_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        Application.DoEvents();
    }

    private void frmReports_Enter(object sender, EventArgs e)
    {
        Application.DoEvents();
    }

    private void m_dgvReport_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {

        DataGridView dgvGrid = (DataGridView)sender;
        if (e.RowIndex < 0)
        {
            MessageBox.Show("You did not selected a current row.");
            return;
        }
        string strAcc;
        try
        {
            strAcc = dgvGrid["ACCOUNT", e.RowIndex].Value.ToString();
        }
        catch (ArgumentException ae)
        {
            MessageBox.Show(ae.Message);
            return;
        }
        if (strAcc.Length < 2)
        {
            MessageBox.Show("Account not long enough"); // don't expect this to happen.
            return;
        }
        if (strAcc[1] == '0' || strAcc[1] == 'A') // if the seconds character is 0 or A remove 
        {
            strAcc = strAcc.Remove(1, 1);
        }
        AccountLaunched?.Invoke(this, strAcc);
    }

    private void accountsWithSpecificCDMsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        splitContainer1.Panel1Collapsed = false;

        Label cdmLabel = new();
        cdmLabel.Text = "Type CDM and press enter";
        cdmLabel.Dock = DockStyle.Fill;
        ListBox cdmListBox = new();
        cdmListBox.Dock = DockStyle.Fill;
        Button cdmRunReportButton = new();
        TableLayoutPanel tlp = new();
        TextBox cdmEntryTextBox = new();
        cdmEntryTextBox.Dock = DockStyle.Fill;

        cdmEntryTextBox.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                cdmListBox.Items.Add(cdmEntryTextBox.Text);
                cdmEntryTextBox.Text = string.Empty;
                cdmEntryTextBox.Focus();
            }
        };
        cdmRunReportButton.Text = "Run";
        cdmRunReportButton.Click += (object sender, EventArgs e) =>
        {
            List<string> cdms = new();
            foreach (var item in cdmListBox.Items)
            {
                cdms.Add(item.ToString());
            }
            splitContainer1.Panel1Collapsed = true;
            GetReport(cdms);
            splitContainer1.Panel1.Controls.Remove(tlp);
        };

        tlp.Controls.Add(cdmLabel, 0, 0);
        tlp.Controls.Add(cdmEntryTextBox, 0, 1);
        tlp.Controls.Add(cdmListBox, 0, 2);
        tlp.Controls.Add(cdmRunReportButton, 0, 3);
        tlp.Dock = DockStyle.Fill;
        splitContainer1.Panel1.Controls.Add(tlp);
    }
}

