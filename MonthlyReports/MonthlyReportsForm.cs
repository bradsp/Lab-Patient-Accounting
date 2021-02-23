using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
// wdk 20090330 programmer added
using System.IO;
using System.Data.SqlClient; // for SqlException trapping
using System.Drawing.Printing; // for printing the gridview
using RFClassLibrary;


namespace MonthlyReports
{
    public partial class frmReports : Form
    {
       
        SqlDataReader m_sqlReader = null;
        DataTable m_dtRecords = null;
        ERR m_Err = null;
        // wdk 20100916 added
        Dictionary<string, string> m_dicCode;
        // 
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        public StreamWriter errLog;
        PrintDocument ViewerPrintDocument;         // The PrintDocument to be used for printing.
        ReportGenerator m_rgReport;         // The class that will do the printing process.
        private string m_strConn;
        private string m_strReportTitle;
        private string m_strQuery;
        private DataSet m_dsSource;
        private string m_strServer;
        private string m_strDatabase;

        public frmReports(string[] args)
        {
            
            InitializeComponent();
           // MessageBox.Show("This application only runs on LIVE data it cannot be changed to TEST data");
            if (args.Length != 2)
            {
                MessageBox.Show("Incorrect number of arguments passed. \r\nCan not continue.");
                Environment.Exit(13);
            }
            m_strServer = args[0];
            m_strDatabase = args[1];
            if (args[0].StartsWith("/"))
            {
                m_strServer = args[0].Remove(0, 1);
            }
            if (args[1].StartsWith("/"))
            {
                m_strDatabase = args[1].Remove(0, 1);
            }
            if (!m_strServer.Contains("MCLBILL"))
            {
                MessageBox.Show(string.Format("These reports were not written for server [{0}]. \r\nCan not continue.", m_strServer));
                Environment.Exit(13);
            }
            string[] argErr = new string[3];
            argErr[0] = "/LIVE";
            argErr[1] = string.Format("/{0}",m_strServer);
            argErr[2] = "/MCLLIVE";

            m_Err = new ERR(argErr);

            CreateConnectionString();
            m_dgvReport.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(LaunchAcc.LaunchAcc_EventHandler);
            //this.Text += " PRODUCTION ENVIRONMENT LIVE";
        }

        private void CreateConnectionString()
        {
            //int n = 0;
            //int y = 9;
            //int z = y / n;
            m_strConn = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=SSPI", m_strServer, m_strDatabase);
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            CreateDateTimes();
            CreatePrintDocument();
            tssbTableReports_ButtonClick(null, null);
            m_dgvReport.ContextMenu = GetContextMenu();
            using (SqlConnection conn = new SqlConnection(m_strConn))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(
                    "select cli_mnem,cli_nme from client where deleted = 0 order by cli_mnem", conn);
                DataTable dt = new DataTable();
                //adapter.FillError += new FillErrorEventHandler(adapter_FillError);
                //adapter.Disposed += new EventHandler(adapter_Disposed);
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    tscbClients.Items.Add(string.Format("{0} - {1}",
                        dr["cli_mnem"], dr["cli_nme"]));
                }
            }
            ((ComboBox)tscbClients.Control).DisplayMember = "cli_mnem";
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            m_Err.m_Logfile.WriteLogFile(string.Format("APPLICATION EXCEPTION -- [{0}]", e.Exception.Message));
        }

        private void CreatePrintDocument()
        {
          ViewerPrintDocument = new PrintDocument(); // create a new print document each time so the .PrintPage handler only gets handled once.
        }

        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dpFrom.Text = DateTime.Now.Subtract(new TimeSpan((DateTime.Now.Day-1), 0, 0, 0)).ToString("d");
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            m_dpFrom.Control.Width = 95;
            m_dpFrom.Control.Refresh();
            m_dpFrom.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFrom = new ToolStripLabel("From: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

            m_dpThru = new ToolStripControlHost(new DateTimePicker());
            m_dpThru.Text = DateTime.Now.AddDays((DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)-DateTime.Now.Day)).ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            m_dpThru.Control.Width = 95;
            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
           // tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }

        private void tsmi80299_Click(object sender, EventArgs e)
        {
            /* wdk 20090330 original query from VIEWER SQL
            SELECT     TOP (100) PERCENT dbo.acc.account, dbo.acc.pat_name, dbo.amt.cpt4, dbo.acc.trans_date, dbo.chrg.qty
            FROM         dbo.chrg INNER JOIN
                      dbo.amt ON dbo.chrg.chrg_num = dbo.amt.chrg_num INNER JOIN
                      dbo.acc ON dbo.chrg.account = dbo.acc.account
            WHERE     (dbo.amt.cpt4 LIKE 'j%' OR
                     dbo.amt.cpt4 = '80299') AND (dbo.acc.trans_date BETWEEN CONVERT(DATETIME, '2009-02-01 00:00:00', 102) AND CONVERT(DATETIME, '2009-02-28 23:59:00', 102))
            ORDER BY dbo.acc.pat_name
             */
            m_strReportTitle = string.Format("CPT code beginning with J or 80299 as the cpt code and the date of service is between {0} and {1}",m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("SELECT     TOP (100) PERCENT acc.fin_code, acc.account, acc.pat_name, amt.cpt4, acc.trans_date, chrg.qty "+
                                       "FROM       chrg INNER JOIN "+
                                       " amt ON chrg.chrg_num = amt.chrg_num INNER JOIN "+
                                       " acc ON chrg.account = acc.account "+
                                       " WHERE     (amt.cpt4 LIKE 'j%' OR amt.cpt4 = '80299') "+
                                       " AND (acc.trans_date BETWEEN CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102)) "+
                                       "ORDER BY acc.pat_name", m_dpFrom.Text, m_dpThru.Text);
            LoadGrid();
        }

        private void LoadGrid()
        {
            tspbCount.Value = 0;
            tspbCount.Step = 1;
            tspbCount.Increment(10);
            tspbCount.ToolTipText = "Working";

        //    m_dsSource = new DataSet("PrintDataSet");
            m_dtRecords = new DataTable("PrintDataTable");
           
            m_Err.m_Logfile.WriteLogFile("After setting grids Datasource");

            SelectRows(m_strQuery);
            
            try
            {
            //    m_Err.m_Logfile.WriteLogFile(string.Format("DataSource Table = [{0}] ", m_dsSource.Tables[0].TableName));
                m_Err.m_Logfile.WriteLogFile(
                    string.Format("DataTable = [{0}]", m_dtRecords.TableName));
                if (m_dtRecords == null)
                {
                    m_dgvReport.DataSource = null;
                    m_Err.m_Logfile.WriteLogFile("m_dsSource.Tables.Count == 0 so return.");
                    return;
                }
            }
            catch (Exception ex)
            {
                m_Err.m_Logfile.WriteLogFile(string.Format("m_dsSource.Tables[0].TableName threw an exception. [{0}]", ex.Message));
            }
          
            m_Err.m_Logfile.WriteLogFile("Before setting grids Datasource");
            BindingSource bs = new BindingSource();
            
            try
            {
                //m_dgvReport.DataSource = m_dtRecords;
                bs.DataSource = m_dtRecords;
                m_dgvReport.DataSource = bs;
            }
            catch (Exception)
            {
                m_dgvReport.DataSource = m_dtRecords;
            }
            
            if (m_dgvReport.Rows.Count > 0)
            {
                SetRowNumbers();
            }
            m_Err.m_Logfile.WriteLogFile("After SetRowNumbers()");

            tsslRecords.Text = string.Format("{0} rows loaded.", m_dgvReport.Rows.Count);
            try
            {
                MessageBox.Show(string.Format("{0} rows loaded.", m_dgvReport.Rows.Count), "LOAD FINISHED");
            }
            catch (Exception exmb)
            {
                m_Err.m_Logfile.WriteLogFile(exmb.Message);
            }
            
        }

        private void LoadButton(ToolStripMenuItem toolStripMenuItem)
        {
            tspbCount.Value = 0;
            tspbCount.Step = 1;
            tspbCount.Increment(10);
            tspbCount.ToolTipText = "Working";

            //    m_dsSource = new DataSet("PrintDataSet");
            m_dtRecords = new DataTable("PrintDataTable");

            m_Err.m_Logfile.WriteLogFile("After setting grids Datasource");

            SelectRows(m_strQuery);

            try
            {
                //    m_Err.m_Logfile.WriteLogFile(string.Format("DataSource Table = [{0}] ", m_dsSource.Tables[0].TableName));
                m_Err.m_Logfile.WriteLogFile(
                    string.Format("DataTable = [{0}]", m_dtRecords.TableName));
                if (m_dtRecords == null)
                {
                    m_dgvReport.DataSource = null;
                    m_Err.m_Logfile.WriteLogFile("m_dsSource.Tables.Count == 0 so return.");
                    return;
                }
            }
            catch (Exception ex)
            {
                m_Err.m_Logfile.WriteLogFile(string.Format("m_dsSource.Tables[0].TableName threw an exception. [{0}]", ex.Message));
            }

            m_Err.m_Logfile.WriteLogFile("Before setting grids Datasource");
            BindingSource bs = new BindingSource();
            bs.DataSource = m_dtRecords;

            m_dgvReport.DataSource = bs;

            if (m_dgvReport.Rows.Count > 0)
            {
                SetRowNumbers();
            }
            foreach (DataGridViewRow dr in m_dgvReport.Rows)
            {
                toolStripMenuItem.DropDownItems.Add(dr.ToString());

            }
            m_Err.m_Logfile.WriteLogFile("After SetRowNumbers()");

            tsslRecords.Text = string.Format("{0} rows loaded.", m_dgvReport.Rows.Count);
            try
            {
                MessageBox.Show(string.Format("{0} rows loaded.", m_dgvReport.Rows.Count), "LOAD FINISHED");
            }
            catch (Exception exmb)
            {
                m_Err.m_Logfile.WriteLogFile(exmb.Message);
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
            m_Err.m_Logfile.WriteLogFile(
                string.Format("m_dgvReport has [{0}] rows and Progress bar max value is  [{1}]",
                 m_dgvReport.Rows.Count, tspbCount.Maximum));
            tsslRecords.Text = string.Format("{0} rows loaded.", m_dgvReport.Rows.Count);
          }

        private void SelectRows(string m_strQuery)
        {
            m_Err.m_Logfile.WriteLogFile("Entered SelectRows()");
            using (SqlConnection connection =
                new SqlConnection(m_strConn))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = new SqlCommand(
                    m_strQuery, connection);
                    
                    
                    int nRec = adapter.Fill(m_dtRecords);

                    m_dsSource = new DataSet();    
                    adapter.Fill(m_dsSource);

                
                }
                catch (SqlException se)
                {
                    //MessageBox.Show(se.Message + "\r\n\nCannot process request", "SQL EXCEPTION");
                    m_Err.m_Logfile.WriteLogFile("se.EXCEPTION in SELECTROWS()");
                    m_Err.m_Logfile.WriteLogFile(se.Message);
                    if (string.IsNullOrEmpty(niMsg.BalloonTipText) || niMsg.BalloonTipTitle != "SQLEXCEPTION")
                    {
                        niMsg.BalloonTipText = "Server is running slow.";
                        niMsg.BalloonTipTitle = "SQLEXCEPTION";
                        niMsg.Visible = true;
                        niMsg.ShowBalloonTip(30);
                        if (m_strQuery.Contains("TOP("))
                        {
                            decimal d = decimal.Parse(m_strQuery.Substring(m_strQuery.IndexOf("TOP(")));
                            d = d / 2;
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
                    m_Err.m_Logfile.WriteLogFile("ioe.EXCEPTION in SELECTROWS()");
                    m_Err.m_Logfile.WriteLogFile(ioe.Message);
                    niMsg.BalloonTipText = ioe.Message;
                    niMsg.BalloonTipTitle = "InvalidOperationException";
                    niMsg.Visible = true;
                    niMsg.ShowBalloonTip(30);
                }
                catch (Exception ex)
                {
                    m_Err.m_Logfile.WriteLogFile("ex.EXCEPTION in SELECTROWS()");
                    m_Err.m_Logfile.WriteLogFile(ex.Message);
                    niMsg.BalloonTipText = ex.Message;
                    niMsg.BalloonTipTitle = "GENERAL EXCEPTION";
                    niMsg.Visible = true;
                    niMsg.ShowBalloonTip(30);
                }
            }
            m_Err.m_Logfile.WriteLogFile("Leaving SelectRows()");
        }

        void adapter_Disposed(object sender, EventArgs e)
        {
            m_Err.m_Logfile.WriteLogFile("adapter disposed");
        }

        void adapter_FillError(object sender, FillErrorEventArgs e)
        {
            m_Err.m_Logfile.WriteLogFile("INSIDE adapter_FillError");
            m_Err.m_Logfile.WriteLogFile(sender.GetType().ToString());
            m_Err.m_Logfile.WriteLogFile(e.Errors.Message);
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
            ViewerPrintDocument.DefaultPageSettings.Landscape = bLandscape;
            m_rgReport = new ReportGenerator(m_dgvReport, ViewerPrintDocument, m_strReportTitle, m_strDatabase);

            ViewerPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
            ViewerPrintDocument.DocumentName = m_strReportTitle;

            ViewerPrintDocument.Print();
            ViewerPrintDocument.PrintPage -= new System.Drawing.Printing.PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
        }

        private void tsmi59Modis_Click(object sender, EventArgs e)
        {
            
            m_strReportTitle = string.Format("Accounts with combination of GC (87591) CHL (87491) and Group B (87149) and the date of service is between {0} and {1}",m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("with cte (account, cdm, cpt4, [count], qty) "+
                    "as  "+
                    "(  "+
	                "    select  top(1000000)ISNULL(vw_chrgdetail.account, 'GRAND TOTAL') as [ACCOUNT], "+
			        "            ISNULL(cdm, NULL) as [CDM], "+
			        "            ISNULL(cpt4, NULL) as [CPT4], "+
			        "            count(cpt4) as [Count of Cpt4], "+
			        "            sum(qty)as [Total for CPT4] "+
	                "     from vw_chrgdetail "+
	                "    inner join acc on acc.account = vw_chrgdetail.account and acc.status not in ('paid_out','closed') and acc.trans_date >= '{0}' "+
	                "    right outer  join client on  acc.cl_mnem = client.cli_mnem and client.type not in (0,6)  "+
                    "    where cdm not in ('CBILL','') and cpt4 not in ('NONE','') and cdm is not null and cpt4 is not null " + //and credited = 0
			        "            and modi is null "+
	                "    group by vw_chrgdetail.account, cdm, cpt4 with rollup "+
	                "    having count(cpt4) > 1 "+
	                "    order by vw_chrgdetail.account, cdm, cpt4 "+
                    ") "+
                    "select cte.*, acc.fin_code "+
                    "from cte  "+
                    "inner join acc on acc.account = cte.account and acc.trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                    "where cpt4 is not null   "+
                    "and cpt4 not in ('80101','83896','86256','86003')  "+
                    "and qty > 1 "+
                    "order by fin_code, trans_date, account", m_dpFrom, m_dpThru);
                LoadGrid();
        }

        private void modifiersMultipleCpt4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            m_strReportTitle = string.Format("Accounts with Multiple CPT4 combinations of GC (87591) CHL (87491) and Group B (87149) and the date of service is between {0} and {1}",m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("with MultiCpt4(account, cpt4_87149, modi_87149, "+  
				"        cpt4_87591, modi4_87591,  "+
				"	    cpt4_87491, modi4_87491,  "+
		        "        service_date, fin_code) as "+
                "( "+
	            "    select distinct a.account, a.cpt4, a.modi, b.cpt4, b.modi, c.cpt4, c.modi, convert(varchar(10),acc.trans_date,101), acc.fin_code from vw_chrgdetail a "+
	            "    inner join vw_chrgdetail b on a.account = b.account and b.cpt4 = '87591' "+
	            "    inner join vw_chrgdetail c on c.account = b.account and c.cpt4 = '87491' "+	
	            "    inner join acc on acc.account = a.account and acc.status not in ('paid_out','closed') and acc.trans_date >= '{0}' "+
	            "    right outer  join client on  acc.cl_mnem = client.cli_mnem and client.type not in (0,6)  "+
	            "    where a.cdm not in ('CBILL','') and a.cpt4 not in ('NONE','') and a.cdm is not null and a.cpt4 is not null"+
	            "    and  a.cpt4 = '87149' "+
                ") "+
                "select * from MultiCpt4  "+
                "where coalesce (modi4_87591, modi4_87491,modi_87149) is null and service_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                "order by fin_code, service_date, account", m_dpFrom, m_dpThru); 
                LoadGrid(); 
        }
        private int nFilterColumn;
        private void m_dgvReport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                nFilterColumn = e.ColumnIndex;
                m_dgvReport.ContextMenu.Show(m_dgvReport, MousePosition);
            }
            SetRowNumbers();        
        }

        private ContextMenu GetContextMenu()
        {
            ContextMenu cm = new ContextMenu();

            //Can create STATIC custom menu items if exists here...          
            MenuItem m1, m2, m3, m4, mHeader;
            mHeader = new MenuItem("REPORTS MENU");
            
            m1 = new MenuItem("Visualize Changes");
            m1.Click += new EventHandler(m1_Click);
            m2 = new MenuItem("Filter Data");
            m2.Click += new EventHandler(m2_Click);
            m3 = new MenuItem("Hide Column");
            m3.Click += new EventHandler(m3_Click);
            m4 = new MenuItem("Show Hidden Columns");
            m4.Click += new EventHandler(m4_Click);
            
           
            //Can add functionality for the custom menu items here...
            cm.MenuItems.Add(mHeader);
            cm.MenuItems.Add(m1);
            cm.MenuItems.Add(m2);
            cm.MenuItems.Add(m3);
            cm.MenuItems.Add(m4);

            
            
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
                    DataGridViewCellStyle dgvcs = new DataGridViewCellStyle();
                    dgvcs.BackColor = Color.PeachPuff;

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
            FormResponse f = new FormResponse();
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
                    strResponse += string.Format("'{0}',", str);
                }
                if (!string.IsNullOrEmpty(strResponse))
                {
                    int nli = strResponse.LastIndexOf(',');
                    int nlen = strResponse.Length;

                    strResponse = strResponse.Remove(strResponse.LastIndexOf(','));
                }
                
            }

            string strColText = m_dgvReport.Columns[nFilterColumn].HeaderText;
            BindingSource bs = new BindingSource(m_dgvReport.DataSource,
                strColText);
            bs.DataMember = m_dgvReport.DataMember;
            if (string.IsNullOrEmpty(strResponse))
            {
                bs.RemoveFilter();

            }
            else
            {

                bs.Filter = string.Format("{0} in ({1})", strColText, strResponse);
            }
            m_dgvReport.DataSource = bs;

        }

        private void mailerErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_strReportTitle = string.Format("Accounts with Mailer errors and the date of service is between {0} and {1}",m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("with cte "+
                                        "as "+
                                        "( "+
                                        "select acc.account from acc  "+
                                        "inner join pat on acc.account = pat.account  "+
                                        "where acc.fin_code <> 'E' and pat.mailer in  ('1') and acc.trans_date >= '{0}' and original_fincode <> 'E' "+
                                        "and acc.status not in ('closed','paid_out') "+
                                        ") "+
                                        "select * from acc " +
                                        "inner join cte on cte.account = acc.account", m_dpFrom, m_dpThru);
            LoadGrid();
        }

        private void accountsWithNoPatientRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date from acc
left outer join pat on pat.account = acc.account
where pat.account is null and acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed')
and trans_date between '03/01/2009 00:00' and '03/31/2009 23:59'
order by acc.account*/
            m_strReportTitle = string.Format("Accounts with no Patient Records and the date of service is between {0} and {1}",m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date from acc " +
                                        "left outer join pat on pat.account = acc.account " +
                                        "where pat.account is null and acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed') and acc.fin_code not in ('X','Y') " +
                                        "and trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                                        "order by acc.account", m_dpFrom, m_dpThru);
            LoadGrid();
            
        }

        private void accountsWithNoInsuranceRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * select top(10)ins.account,acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date from acc
left outer join ins on ins.account = acc.account
where ins.account is null and acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed')
and trans_date between '02/01/2009 00:00' and '02/28/2009 23:59'
order by acc.account
             * */
            m_strReportTitle = string.Format("Accounts with no Insurance Records and the date of service is between {0} and {1}", m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, acc.trans_date from acc " +
                                        "left outer join ins on ins.account = acc.account " +
                                        "where ins.account is null and acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed') and acc.fin_code not in ('X','Y') " +
                                        "and trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                                        "order by acc.account", m_dpFrom, m_dpThru);
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
            m_strReportTitle = string.Format("Accounts where Insurance and account fin codes do not match and the date of service is between {0} and {1}", m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            // set up the query from the argument
            m_strQuery = string.Format("select acc.account, acc.pat_name, acc.cl_mnem, acc.fin_code, ins.fin_code, acc.trans_date from acc " +
                                        "inner join ins on ins.account = acc.account and ins.ins_a_b_c = 'A' and ins.fin_code <> acc.fin_code " +
                                        "where acc.fin_code <> 'CLIENT' and acc.status not in ('paid_out','closed') " +
                                        "and trans_date between CONVERT(DATETIME, '{0}', 102) AND CONVERT(DATETIME, '{1}', 102) " +
                                        "order by acc.account", m_dpFrom, m_dpThru);
            LoadGrid();
        }

        private void aBNsReportedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * 
with cte (account, cli_mnem, coll_date, pat_name, rowguid)
as
(
select s.account,  w.cli_mnem, w.coll_date, w.pat_lname+', '+w.pat_fname, w.rowguid from wreq w
inner join FORD.dbo.services_status s on s.wreq_rowguid = w.rowguid
where s.account is not null
)
select c.account, c.cli_mnem, c.coll_date, c.pat_name, w.test_mnem from cte c
inner join worders w on c.rowguid = w.wreq_rowguid and w.abn = 1
and c.coll_date between '01/01/2009 00:00' and '03/31/2009 23:59'
order by c.account

             * */
            m_strReportTitle = string.Format("Accounts with ABN's and the date of service is between {0} and {1}", m_dpFrom.Text, m_dpThru.Text);
            tsslReportTitle.Text = m_strReportTitle;
            m_strQuery = string.Format("with cte (account, cli_mnem, coll_date, pat_name, rowguid) "+
                                        "   as "+
                                        "   ( "+
                                        "   select s.account,  w.cli_mnem, w.coll_date, w.pat_lname+', '+w.pat_fname, w.rowguid from wreq w "+
                                        "   inner join FORD.dbo.services_status s on s.wreq_rowguid = w.rowguid "+
                                        "   where s.account is not null "+
                                        "   ) "+
                                        "   select c.account, c.cli_mnem, c.coll_date, c.pat_name, w.test_mnem from cte c "+
                                        "   inner join worders w on c.rowguid = w.wreq_rowguid and w.abn = 1 "+
                                        "   and c.coll_date between convert(datetime,'{0} 00:00',102) and convert(datetime,'{1} 23:59', 102) "+
                                        "   order by c.account", m_dpFrom, m_dpThru);
            LoadGrid();
        }

        private void tsddbtnMCLOEReports_DropDownOpening(object sender, EventArgs e)
        {
            m_strServer = "MCLOE";
            m_strDatabase = "GOMCLLIVE";
            CreateConnectionString();
        }

        private void tsddbtnBillingReports_DropDownOpening(object sender, EventArgs e)
        {
            m_strServer = "WTHMCLBILL";
            m_strDatabase = "MCLLIVE";
            CreateConnectionString();
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
            m_strReportTitle = string.Format("Accounts containing {0} and {1} and the date of service is between {2} and {3}", strCpt4s[0], strCpt4s[1], m_dpFrom.Text, m_dpThru.Text);            tsslReportTitle.Text = m_strReportTitle;
            m_strQuery = string.Format("select  chrg.account "+ 
                                        "from chrg "+
                                        "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') "+
                                        "inner join amt on chrg.chrg_num = amt.chrg_num "+
                                        "where cdm <> 'cbill' and chrg.credited = 0 " +
                                        "and service_date between '{0} 00:00' and '{1} 23:59' and amt.cpt4 in ('{2}') "+
                                        "intersect "+
                                        "select  chrg.account "+
                                        "from chrg "+
                                        "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') "+
                                        "inner join amt on chrg.chrg_num = amt.chrg_num "+
                                        "where cdm <> 'cbill' and chrg.credited = 0  " +
                                        "and service_date between '{3} 00:00' and '{4} 23:59' and amt.cpt4 in ('{5}')", m_dpFrom, m_dpThru, strCpt4s[1], m_dpFrom, m_dpThru, strCpt4s[0]);
                LoadGrid();
        }

        private void tsmi_C3_C4_Click(object sender, EventArgs e)
        {
            string strTag = ((ToolStripMenuItem)sender).Tag.ToString();
            string[] strCpt4s = strTag.Split(new char[] { '|' });
            m_strReportTitle = string.Format("Accounts containing C3 and C4 and the date of service is between {0} and {1}", m_dpFrom.Text, m_dpThru.Text); 
            tsslReportTitle.Text = m_strReportTitle;
            m_strQuery = string.Format("select  chrg.account " +
                                        "from chrg " +
                                        "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
                                        "inner join amt on chrg.chrg_num = amt.chrg_num " +
                                        "where cdm <> 'cbill' and chrg.credited = 0 and chrg.cdm = '5606106' "+
                                        "and service_date between '{0} 00:00' and '{1} 23:59' and amt.cpt4 in ('{2}') " +
                                        "intersect " +
                                        "select  chrg.account " +
                                        "from chrg " +
                                        "inner join acc on acc.account = chrg.account and acc.status not in ('closed','paid_out') " +
                                        "inner join amt on chrg.chrg_num = amt.chrg_num " +
                                        "where cdm <> 'cbill' and chrg.credited = 0 and chrg.cdm = '5602376' " +
                                        "and service_date between '{3} 00:00' and '{4} 23:59' and amt.cpt4 in ('{5}')", m_dpFrom, m_dpThru, strCpt4s[1], m_dpFrom, m_dpThru, strCpt4s[0]);
            LoadGrid();
        }

        private void tscbCPT4s_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (tscbCPT4s.Text.Length == 5)
                {
                    tscbCPT4s.Items.Add(tscbCPT4s.Text);
                    tscbCPT4s.Text = string.Empty;
                }
            }
        }

        private void tsbtnComboCpt4_Click(object sender, EventArgs e)
        {
            m_strServer = "WTHMCLBILL";
            m_strDatabase = "MCLLIVE";
            CreateConnectionString();
            string strFirstCpt4 = string.Empty;
            string strRemainingCpt4s = string.Empty;
            if (tscbCPT4s.Items.Count < 2)
            {
                MessageBox.Show("Not enought Cpt4's to search with.");
                return;
            }
            strFirstCpt4 = tscbCPT4s.Items[0].ToString();
            for (int i = 1; i < tscbCPT4s.Items.Count; i++)
            {
                strRemainingCpt4s += string.Format("{0}{1}{2}",
                                        i == 1 ? "" : ",'",
                                            tscbCPT4s.Items[i].ToString(),
                                            i == (tscbCPT4s.Items.Count-1) ? "" : "'");
                        ;
            }

            m_strReportTitle = string.Format("Accounts containing {0} and {1} and the date of service is between {2} and {3}", strFirstCpt4, strRemainingCpt4s, m_dpFrom.Text, m_dpThru.Text); tsslReportTitle.Text = m_strReportTitle;
            m_strQuery = string.Format("select  chrg.account " +
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
                                        "and service_date between '{3} 00:00' and '{4} 23:59' and amt.cpt4 in ('{5}')", m_dpFrom, m_dpThru, strFirstCpt4, m_dpFrom, m_dpThru, strRemainingCpt4s);
            LoadGrid();
            tscbCPT4s.Items.Clear();
        }

      
       

        private void tstbCDM_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string strTextBoxName = ((ToolStripTextBox)sender).Name;
                string strCDM1 = tstbCDM1.Text;
                string strCDM2 = tstbCDM2.Text;
                if (strTextBoxName.ToUpper() == "TSTBCDM1")
                {
                    if (!string.IsNullOrEmpty(strCDM1))
                    {
                        if (strCDM1.Length == 7)
                        {
                            ((ToolStripTextBox)sender).ReadOnly = true;
                            tstbCDM2.Focus();
                        }
                        else
                        {
                            MessageBox.Show("First CDM must contain 7 characters");
                            ((ToolStripTextBox)sender).Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("First CDM must contain 7 characters");
                        ((ToolStripTextBox)sender).Focus();
                    }
                }
                if (strTextBoxName.ToUpper() == "TSTBCDM2")
                {
                    if (!string.IsNullOrEmpty(strCDM2))
                    {
                        if (strCDM2.Length == 7)
                        {

                            if (strCDM1.ToUpper() == strCDM2.ToUpper())
                            {
                                MessageBox.Show("Both CDM's are the same. Cannot process report.");
                                ((ToolStripTextBox)sender).Focus();
                                return;
                            }
                            ((ToolStripTextBox)sender).ReadOnly = true;    
                            //GetReport();
                        }
                        else
                        {
                            MessageBox.Show("Second CDM must contain 7 characters");
                            ((ToolStripTextBox)sender).Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Second CDM must contain 7 characters");
                        ((ToolStripTextBox)sender).Focus();
                    }
                }
             
            }
        }

        private void tssbCDM_DropDownOpening(object sender, EventArgs e)
        {
            tstbCDM1.ReadOnly = false;
            tstbCDM2.ReadOnly = false;
        }


        private void GetReport()
        {
            string strFirstCDM = tstbCDM1.Text;
            string strSecondCDM = tstbCDM2.Text;
            if (string.IsNullOrEmpty(strSecondCDM))
            {
                m_strReportTitle = string.Format("Accounts containing {0} with date of service between {1} and {2}", strFirstCDM, m_dpFrom.Text, m_dpThru.Text); 
                 m_strQuery = 
                string.Format("with cte (account, cdm)"+
                    "as"+
                    "("+
                    "	select account, cdm from chrg "+
	                "   where service_date between '{0} 00:00' and '{1} 23:59' "+
                    "	and credited = 0 "+
                    ")" +
                    "select distinct cte.account, cte.cdm as [FIRST CDM], convert(varchar(10),service_date,101) as [Service Date]"+
                    "from cte "+
                    "join chrg on chrg.account = cte.account "+
                    "where service_date between '{0} 00:00' and '{1} 23:59' "+
	                " and credited = 0", m_dpFrom, m_dpThru, strFirstCDM);

 
            }
            else
            {
                m_strReportTitle = string.Format("Accounts containing {0} and {1} with date of service between {2} and {3}", strFirstCDM, strSecondCDM, m_dpFrom.Text, m_dpThru.Text); 
                 m_strQuery = 
                string.Format("with cte (account, cdm)"+
                    "as"+
                    "("+
                    "	select account, cdm from chrg "+
	                "   where service_date between '{0} 00:00' and '{1} 23:59' "+
                    "	and cdm = '{2}' and credited = 0 "+
                    ")" +
                    "select distinct cte.account, cte.cdm as [FIRST CDM], chrg.cdm as [SECOND CDM], convert(varchar(10),service_date,101) as [Service Date]"+
                    "from cte "+
                    "join chrg on chrg.account = cte.account "+
                    "where service_date between '{0} 00:00' and '{1} 23:59' "+
	                "and chrg.cdm = '{3}' and credited = 0", m_dpFrom, m_dpThru, strFirstCDM, strSecondCDM);

 
            }
            tsslReportTitle.Text = m_strReportTitle;
            LoadGrid();
   
        }

        private void tssbCDM_ButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tstbCDM1.Text) )//|| string.IsNullOrEmpty(tstbCDM2))
            {
                MessageBox.Show("You must put a CDM in the first text box");
                return;
            }
            GetReport();
        }

        private void tssbTableReports_ButtonClick(object sender, EventArgs e)
        {
            tssbTableReports.DropDownItems.Clear();
            m_dicCode = new Dictionary<string, string>();
            string strQuery = "select * from dbo.Monthly_Reports order by button, report_title";
           
            SqlDataAdapter sda = new SqlDataAdapter(strQuery, m_strConn);//strConnection);
            DataSet ds = new DataSet("Monthly_Reports");
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
                    tsiBtn = (ToolStripMenuItem)tssbTableReports.DropDownItems.Add(strButton);
                }
               ToolStripMenuItem tsi = (ToolStripMenuItem)tsiBtn.DropDownItems.Add(dr["mi_name"].ToString());
               tsi.Tag = dr["child_button"].ToString();
               //if (tsi.Tag.ToString() == bool.TrueString)
               //{
               //    ToolStripMenuItem tsiReport = (ToolStripMenuItem)tsi.DropDownItems.Add("REPORT");
               //    tsiReport.CheckOnClick = true;
               //    tsiReport.Click += new EventHandler(tsiReport_Click);
               //    tsi.DropDownItems.Add(tsiReport);
               //}
               m_dicCode.Add(dr["mi_name"].ToString(), dr["sql_code"].ToString());              
               tsi.CheckOnClick = true;
               tsi.Click += new EventHandler(tsi_Click);              
               tsiBtn.DropDownItems.Add(tsi);
            }
        }

        void tsiReport_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void tsi_Click(object sender, EventArgs e)
        {
            string strButton = ((ToolStripMenuItem)sender).Text;
            string strCode = null;
            if (!m_dicCode.TryGetValue(strButton, out strCode))
            {
                MessageBox.Show("Not valid");
            }
            
            m_strReportTitle = strButton;// ((ToolStripMenuItem)e.ClickedItem).DropDownItems[0].Text;
            m_strQuery = strCode.Replace("{0}", m_dpFrom.Text).Replace("{1}", m_dpThru.Text);

            m_Err.m_Logfile.WriteLogFile(string.Format("Date from = [{0}], Date thru = [{1}]", m_dpFrom, m_dpThru));

            if (m_strQuery.Contains(" cl_mnem in ({2})") && !string.IsNullOrEmpty(tscbClients.SelectedItem.ToString()))
            {
                string[] strParts = tscbClients.SelectedItem.ToString().Split(new char[] { '-' });
                m_strQuery = m_strQuery.Replace("{2}", string.Format("'{0}'", strParts[0].Trim()));
                //  return;
            }
            else
                if (m_strQuery.Contains("{2}"))
                {

                    FormDataCollection f = new FormDataCollection();
                    if (f.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show("Cannot process this query with out data");
                        tssbTableReports.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
                        return;
                    }
                    m_Err.m_Logfile.WriteLogFile(string.Format("Dialog Result = [{0}]", f.DialogResult.ToString()));
                    if (string.IsNullOrEmpty(f.tbData.Text))
                    {
                        MessageBox.Show("Cannot process this query with out data");
                        tssbTableReports.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
                        return;
                    }

                    string strCdm = null;
                    string[] strCDMs = f.tbData.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string str in strCDMs)
                    {
                        Application.DoEvents();
                        strCdm += string.Format("'{0}',", str);
                    }
                    strCdm = strCdm.Remove(strCdm.LastIndexOf(','));
                    m_strQuery = m_strQuery.Replace("{2}", strCdm);

                }
            m_Err.m_Logfile.WriteLogFile(string.Format("QUERY FILTER = [{0}]", m_strQuery));
            tssbTableReports.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
            Application.DoEvents();
            
           
            try
            {
                //if (bool.TrueString == ((ToolStripMenuItem)sender).Tag.ToString())
                //{
                //    LoadButton((ToolStripMenuItem)sender);
                //}
                //else
                //{
                    LoadGrid();
                //}
            }
            catch (Exception ex)
            {

                m_Err.m_Logfile.WriteLogFile("Error in LoadGrid");
                m_Err.m_Logfile.WriteLogFile(ex.Message);
                MessageBox.Show(ex.Message, "ERROR");
            }
            tscbClients.SelectedIndex = -1;
            m_Err.m_Logfile.WriteLogFile("Leaving tsi_Click.");
            tsslDisplayReportTitle.Text = string.Format(" | {0}", strButton);

        }
        
        void tssbTableReports_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
          //  ((ToolStripMenuItem)e.ClickedItem).Checked = true;

            string strButton = e.ClickedItem.ToString();
           // string strCode = null;
           // if (!m_dicCode.TryGetValue(strButton, out strCode))
           // {
                MessageBox.Show(
                    string.Format("Select a Report from menu [{0}].",strButton));
                return;
            //}

            //m_strReportTitle = ((ToolStripMenuItem)e.ClickedItem).DropDownItems[0].Text;
            //m_strQuery = strCode.Replace("{0}", m_dpFrom.Text).Replace("{1}", m_dpThru.Text);
            
            //m_Err.m_Logfile.WriteLogFile(string.Format("Date from = [{0}], Date thru = [{1}]", m_dpFrom, m_dpThru));

            //if (m_strQuery.Contains(" cl_mnem in ({2})") && !string.IsNullOrEmpty(tscbClients.SelectedItem.ToString()))
            //{
            //    string[] strParts = tscbClients.SelectedItem.ToString().Split(new char[] { '-' });
            //    m_strQuery = m_strQuery.Replace("{2}", string.Format("'{0}'",strParts[0].Trim()));
            //  //  return;
            //}
            //else
            //if (m_strQuery.Contains("{2}"))
            //{

            //    FormDataCollection f = new FormDataCollection();
            //    if (f.ShowDialog() != DialogResult.OK)
            //    {
            //        MessageBox.Show("Cannot process this query with out data");
            //        tssbTableReports.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
            //        return;
            //    }
            //    m_Err.m_Logfile.WriteLogFile(string.Format("Dialog Result = [{0}]", f.DialogResult.ToString()));
            //    if (string.IsNullOrEmpty(f.tbData.Text))
            //    {
            //        MessageBox.Show("Cannot process this query with out data");
            //        tssbTableReports.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
            //        return;
            //    }

            //    string strCdm = null;
            //    string[] strCDMs = f.tbData.Text.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                
            //    foreach (string str in strCDMs)
            //    {
            //        Application.DoEvents();
            //        strCdm += string.Format("'{0}',", str); 
            //    }
            //    strCdm = strCdm.Remove(strCdm.LastIndexOf(','));
            //    m_strQuery = m_strQuery.Replace("{2}", strCdm);

            //}
            //m_Err.m_Logfile.WriteLogFile(string.Format("QUERY FILTER = [{0}]", m_strQuery));
            //tssbTableReports.DropDown.Close(ToolStripDropDownCloseReason.CloseCalled);
            //try
            //{
            //    LoadGrid();
            //}
            //catch (Exception ex)
            //{
                
            //    m_Err.m_Logfile.WriteLogFile("Error in LoadGrid");
            //    m_Err.m_Logfile.WriteLogFile(ex.Message);
            //    MessageBox.Show(ex.Message, "ERROR");
            //}
            //tscbClients.SelectedIndex = -1;
            //m_Err.m_Logfile.WriteLogFile("Leaving tssbTableReports.");
            
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
                    tssbExcel.ShowDropDown();
                    tsmiExcelDirectory.ShowDropDown();
                    tsmicbExcelDirectory.Focus();
                    return;
                }
                if (tsmiExcelFile.Text.Length == 0)
                {
                    tssbExcel.ShowDropDown();
                    tsmiExcelFile.ShowDropDown();
                    tstbExcelFileName.Focus();

                }
                RFClassLibrary.GridviewToExcel.export2Excel ex = new RFClassLibrary.GridviewToExcel.export2Excel();
                ex.ExportToExcel(m_dgvReport, string.Format(@"{0}{1}{2}.xlsx", tsmicbExcelDirectory.Text, tsmicbExcelDirectory.Text.EndsWith(@"\") ? "" : @"\", tstbExcelFileName.Text), tstbExcelWorkSheetName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString(), "Error creating Excel");
                m_Err.m_Logfile.WriteLogFile(string.Format("QUERY FILTER = [{0}]", m_strQuery));
                m_Err.m_Logfile.WriteLogFile("Error in Create Excel");
                m_Err.m_Logfile.WriteLogFile(ex.Message);
            }
        }

        private void m_dgvReport_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //foreach (DataGridViewRow dr in ((DataGridView)sender).Rows)
            //{
            //    Application.DoEvents();
            //    foreach (DataGridViewCell dc in dr.Cells)
            //    {
            //        dc.Value = dc.FormattedValue.ToString().Replace('\'', '`');
            //    }
            //}
            Application.DoEvents();
        }

        private void frmReports_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Err.m_Logfile.WriteLogFile(sender.GetType().ToString());
            m_Err.m_Logfile.WriteLogFile(string.Format("CloseReason = {0}",e.CloseReason.ToString()));
            Application.DoEvents();
        }

        private void m_dgvReport_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                Form f = new Form();
                f.Size = new Size(800, 200);
                f.Text = string.Format("SELECTED CELL CONTENTS - {0} - Column {1}", 
                    m_dgvReport.Rows[e.RowIndex].Cells["account"].Value.ToString(),
                    m_dgvReport.Columns[e.ColumnIndex].Name);
                TextBox tb = new TextBox();
                tb.Multiline = true;
                tb.Dock = DockStyle.Fill;
                tb.ReadOnly = true;
                tb.Text = m_dgvReport[e.ColumnIndex, e.RowIndex].Value.ToString();
                tb.SelectionLength = 0;
                f.Controls.Add(tb);
                f.Show();
            }
            Application.DoEvents();
        }

        private void m_dgvReport_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            int x;
            x = 9;
            Application.DoEvents();
        }

        private void frmReports_Enter(object sender, EventArgs e)
        {
            Application.DoEvents();
        }





       
  
       
      

       
    }
}

