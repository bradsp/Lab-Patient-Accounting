using System;
using System.Data;
using System.Windows.Forms;
// programmer added
using MCL;
using RFClassLibrary;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Collections;

namespace LabBilling.ReportByInsuranceCompany
{
    public partial class frmReport : Form
    {
        CAcc m_rAcc = null;
        DataTable m_dtDataView = null;
        //int nColumnFilter = -1;
        ArrayList arrayFinCodes;
        // The PrintDocument to be used for printing.
        PrintDocument MyPrintDocument;
        // The class that will do the printing process.
        ReportGenerator m_rgIns;
        ToolStripControlHost m_dpThru;
        ToolStripControlHost m_dpFrom;
        string m_strDBServer;
        string m_strDBase;
        ERR m_ERR;
        string m_strWhere;

        public frmReport(string[] args)
        {
            // wdk 20090403 in some applications we are using 
            //string[] strCline = Environment.GetCommandLineArgs(); to get the command line arguments 
            // instead of passing them from the calling application. When 2 arguments are passed to the
            // application this causes them to be arg[1] and args[2] with args[0] being the application 
            // executable name with path.
            // Billing applications that will be called from the menu need to use the constructor with
            // string[] args where args[0] is the server and args[1] is the database 
            InitializeComponent();
            m_ERR = new ERR(new string[] {"/LIVE", args[0],args[1]});
            m_ERR.m_Logfile.WriteLogFile(string.Format("ARGS Server: {0}, DataBase:{1}", args[0], args[1]));
            //Clipboard.SetText(m_ERR.propLogFileName);
            if (args.Length < 2)
            {
                MessageBox.Show(string.Format("There were an incorrect number of arguments passed to this application." +
                            "\r\nSee error log {0}." +
                            "\r\nCan not continue.", m_ERR.propLogFileName), "INITILIZATION ERROR");
                m_ERR.m_Logfile.WriteLogFile(string.Format("There were an incorrect number of arguments passed to this application." +
                            "\r\nSee error log {0}." +
                            "\r\nCan not continue.", m_ERR.propLogFileName));

                //Environment.Exit(13);
                return;
            }
            m_strDBServer = args[0].Remove(0,1);
            m_strDBase = args[1].Remove(0,1);
            // create recordsets
            m_rAcc = new CAcc(m_strDBServer, m_strDBase, ref m_ERR);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateDateTimes();
      //      CreateDataGridView();

            // load the fin_code combo box
            arrayFinCodes = new ArrayList();
            R_fin rFin = new R_fin(m_strDBServer, m_strDBase, ref m_ERR);
            m_strWhere = "fin_code is not null and deleted = 0";
            int nRecs = rFin.GetRecords(m_strWhere);
            for (int i = 0; i < nRecs; i++)
            {                
                arrayFinCodes.Add(new ListObject(rFin.m_strFinCode, rFin.m_strResParty));
                cbFinCode.Items.Add(arrayFinCodes);
                rFin.GetNext();
            }
            cbFinCode.ComboBox.DataSource = arrayFinCodes;         
            cbFinCode.ComboBox.DisplayMember = ListObject.ShortName.ToString();
            cbFinCode.ComboBox.ValueMember = ListObject.ShortName.ToString();
           

            // size the datagrid view
          //  dgReportInsurance.Top += 30;
         //   frmReport_Resize(sender, e);
            // create our reportgenerator and assign event handlers
            // Create a MyPrintDocument and set its PrintPage Handler event
            //MyPrintDocument = new PrintDocument();
            //MyPrintDocument.DefaultPageSettings.Landscape = true;
            //m_rgIns = new ReportGenerator(dgReportInsurance, MyPrintDocument, "Insurance Report by Plan Name", m_strDBase);
            //m_rgIns.m_dgvpReport.propFooterText = m_strWhere;
            //this.MyPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(m_rgIns.MyPrintDocument_PrintPage);
            //this.dgReportInsurance.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(m_rgIns.LaunchAcc_EventHandler);

            tscbInsABC.SelectedIndex = 0;
        }

        /// <summary>
        /// This creates the header for the data grid view each of the columns can be called using the 
        /// </summary>
//        private void CreateDataGridView()
//        {
//            // create the datagridview for the insurance report
//            dgReportInsurance.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
//            dgReportInsurance.RowHeadersVisible = true;
//            dgReportInsurance.ColumnCount = 13;
//            dgReportInsurance.Columns[0].Name = "Ins Code";
//            dgReportInsurance.Columns[1].Name = "Plan Name";
//            dgReportInsurance.Columns[2].Name = "Account";
//            dgReportInsurance.Columns[3].Name = "Patient Name";
//            dgReportInsurance.Columns[4].Name = "Policy Number"; // wdk 20110829 added which require the changing ot the numbers below
//            dgReportInsurance.Columns[5].Name = "Service Date";
//            dgReportInsurance.Columns[5].ValueType = typeof(DateTime);
//            dgReportInsurance.Columns[6].Name = "ABC";
//            dgReportInsurance.Columns[7].Name = "Fin";
//            // format the balance column for alignment and color
//            dgReportInsurance.Columns[8].Name = "Balance";
//            dgReportInsurance.Columns[8].DefaultCellStyle.Format = "c";
//            dgReportInsurance.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            DataGridViewColumn columnCur = dgReportInsurance.Columns[8];
//            DataGridViewCell cell = new DataGridViewTextBoxCell();
//            cell.Style.BackColor = Color.Wheat;
//            columnCur.CellTemplate = cell;
//            // end of formating the balance column

//            // wdk 20090113 ensure that all date columns include the word "DATE" for sorting purposes.
//            // wdk 20120521 removed as unnecessary
////            dgReportInsurance.Columns[9].Name = "Batch Date";
//  //          dgReportInsurance.Columns[9].ValueType = typeof(DateTime);
//    //        dgReportInsurance.Columns[9].DefaultCellStyle.Format = "d";

//            //DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
//            //{
//            //    column.HeaderText = "DATA MAILER";
//            //    column.Name = "DATA MAILER";
//            //    column.AutoSizeMode =
//            //        DataGridViewAutoSizeColumnMode.DisplayedCells;
//            //    column.FlatStyle = FlatStyle.Standard;
//            //    column.ThreeState = true;
//            //    column.CellTemplate = new DataGridViewCheckBoxCell();
//            //    column.CellTemplate.Style.BackColor = Color.Beige;
//            //    column.CellTemplate.ValueType = typeof(bool);
//            //}
//            //dgReportInsurance.Columns.Insert(8, column);

//            dgReportInsurance.Columns[9].Name = "Data Mailer";
//            dgReportInsurance.Columns[10].Name = "UB Date";
//            dgReportInsurance.Columns[10].ValueType = typeof(DateTime);
//            // wdk 20120521 removed as unnecessary
//            //dgReportInsurance.Columns[12].Name = "EBill Batch Date";
//            //dgReportInsurance.Columns[12].ValueType = typeof(DateTime);
//            dgReportInsurance.Columns[11].Name = "H1500 Date";
//            dgReportInsurance.Columns[11].ValueType = typeof(DateTime);
//            // wdk 20120521 removed as unnecessary
//            //dgReportInsurance.Columns[14].Name = "Ebill Batch 1500 Date";
//            //dgReportInsurance.Columns[14].ValueType = typeof(DateTime);
//            //dgReportInsurance.Columns[15].Name = "Claimsnet 1500 Date";
//            //dgReportInsurance.Columns[15].ValueType = typeof(DateTime);

//            dgReportInsurance.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dgReportInsurance_RowHeaderMouseDoubleClick);

//        }

        void dgReportInsurance_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //throw new NotImplementedException();
            LaunchAcc la = new LaunchAcc(m_strDBase);
            
            la.LaunchAccount(dgvReportInsurance["account", e.RowIndex].Value.ToString());
            
        }

        private void CreateDateTimes()
        {
            RFClassLibrary.RFCObject.CreateDateTimes(ref m_dpFrom, ref m_dpThru, "", "");
            tsMain.Items.Insert(7, m_dpFrom);
            tsMain.Items.Insert(9, m_dpThru);
            m_dpFrom.Text = DateTime.Now.Subtract(new TimeSpan(45, 0, 0, 0)).ToString("d");
            m_dpThru.Text = DateTime.Now.Subtract(new TimeSpan(15, 0, 0, 0)).ToString("d");
            
        }

        private void tsbtnCreateReport_Click(object sender, EventArgs e)
        {
            

            bool bWrite = m_ERR.m_Logfile.WriteLogFile("Entering Report_Click");
            //dgReportInsurance.Rows.Clear();
            // wdk 20120521 removed "","","", as unnecessary the three dates were removed
            //string[] rowInsert2 = { "WORKING", "", "", "", ""};
            //dgReportInsurance.Rows.Add(rowInsert2);

            
            //dgReportInsurance.Refresh();
            //dgReportInsurance.Rows.Clear();

            bWrite = m_ERR.m_Logfile.WriteLogFile("Grid Cleared");

            string strIns = "('A','B','C')";
            if (tscbInsABC.Text.Length == 1)
            {
                strIns = string.Format("('{0}')", tscbInsABC.Text);
            }
            // rgc/wdk 20120214 removed the filter element for "not (fin_code in ('client','W','X','Y','Z')" as the fin_code in already eliminates them.
            if (cbFinCode.Text != "E")
            {
                m_strWhere = string.Format("fin_code = '{0}' and trans_date between '{1}' and '{2}' and ins_a_b_c in {3} and mailer = 'N'", cbFinCode.Text, m_dpFrom.Text, m_dpThru, strIns);
            }
            else
            {
                m_strWhere = string.Format("fin_code = '{0}' and trans_date between '{1}' and '{2}' and ins_a_b_c in {3}", cbFinCode.Text, m_dpFrom.Text, m_dpThru, strIns);
            }

            /////////////////////////////////////////////////////////////////////////////////
            SqlConnection sqlConn = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                    + "Integrated Security = 'SSPI'", m_strDBServer, m_strDBase));
            using (SqlConnection connection = new SqlConnection(sqlConn.ConnectionString))
            {
                 SqlDataAdapter sda = new SqlDataAdapter();
                 SqlCommand cdmStartDate =
                     new SqlCommand(string.Format(@"Select *,cast (0 as money) as [Balance] from vw_prg_report_by_plan_name " +
                         " where {0}", m_strWhere), connection);
                 sda.SelectCommand = cdmStartDate;
                 m_dtDataView = new DataTable();
                 sda.Fill(m_dtDataView);
                 dgvReportInsurance.DataSource = m_dtDataView;
            }
            tspbLoading.Maximum = m_dtDataView.Rows.Count;
            foreach (DataRow dr in m_dtDataView.Rows)
            {
                
                string strBal = null;
                m_rAcc.GetBalance(dr["account"].ToString(), out strBal);
                strBal = double.Parse(double.Parse(strBal).ToString("F2")).ToString();
                dr["Balance"] =
                    decimal.Parse(strBal).ToString("F2");
                tspbLoading.PerformStep();    
            }
            tsslRecords.Text = string.Format("Loaded {0} accounts", tspbLoading.Maximum);
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            int nRows = dgvReportInsurance.Rows.Count;
            if (nRows < 1)
            {
                MessageBox.Show("Must have data in the grid to print.");
                return;
            }
            // Create the print document and set its properties.
            MyPrintDocument = new PrintDocument();
            MyPrintDocument.DefaultPageSettings.Landscape = true;
            // create the insurance report and set its properties.
            m_rgIns = new ReportGenerator(dgvReportInsurance, MyPrintDocument, "Insurance Report by Plan Name", m_strDBase);
            m_rgIns.m_dgvpReport.propFooterText = m_strWhere;
            this.dgvReportInsurance.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(m_rgIns.LaunchAcc_EventHandler);
            // now that we have a report generator PrintPage event handler created assign it to the print document we created earlier.
            this.MyPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(m_rgIns.MyPrintDocument_PrintPage);

            if (m_rgIns.SetupThePrinting(this.MyPrintDocument, this.dgvReportInsurance))
            {
                MyPrintDocument.Print();
            }       
        }



        private void cbFinCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFinCode.ToolTipText = ((ListObject)((ToolStripComboBox)sender).SelectedItem).Description;
        }

        //private void frmReport_Resize(object sender, EventArgs e)
        //{
        //    // size the datagrid view
        //    dgReportInsurance.Left = 30;
        //    dgReportInsurance.Width = this.Size.Width - 60;
        //    dgReportInsurance.Height = this.Size.Height -75;
        //    dgReportInsurance.Invalidate();
        //}

    

        //private void tsbAbout_Click(object sender, EventArgs e)
        //{
        //    AboutBox1 ab = new AboutBox1();
        //    ab.labelVersion.Text += string.Format(" Accessing Database {0} on Server {1}", m_strDBase, m_strDBServer);
        //    ab.Show();
        //}

  
        // wdk 20090120 Show the hidden columns if the user wants.
        private void frmReport_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {

                dgvReportInsurance.SortedColumn.Visible = false;
                    
                
            }
            if (e.KeyCode == Keys.F12)
            {
                foreach (DataGridViewColumn dgvc in dgvReportInsurance.Columns)
                {
                    if (dgvc.Visible == false)
                    {
                        dgvc.Visible = true;
                    }
                }
            }
            
        }

       // wdk 20090120 special sort to allow dates and balances to be sorted correctly. Also handles 
        // Balances with ERR in the field.
        private void dgReportInsurance_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Try to sort based on the cells in the current column.
            if (e.Column.Name == "Balance")
            {
                string strVal1;
                strVal1 = string.Format("{0}", e.CellValue1.ToString() == "ERR" ? "0" : e.CellValue1);
                string strVal2
                    = string.Format("{0}", e.CellValue2.ToString() == "ERR" ? "0" : e.CellValue2);
                e.SortResult = System.Decimal.Compare(
                    decimal.Parse(strVal1, System.Globalization.NumberStyles.Currency),
                    decimal.Parse(strVal2, System.Globalization.NumberStyles.Currency));
           
                e.Handled = true;
           
                return;
            }
            if (e.Column.Name.ToUpper().Contains("DATE"))
            {
                DateTime dt1;
                DateTime dt2;
                DateTime.TryParse(e.CellValue1.ToString(), out dt1);
                DateTime.TryParse(e.CellValue2.ToString(), out dt2);
                e.SortResult = System.DateTime.Compare(dt1, dt2);
                e.Handled = true;
           
                return;
            }
            try
            {
                e.SortResult = System.String.Compare(
                      e.CellValue1.ToString(), e.CellValue2.ToString());
            }
            catch (NullReferenceException)
            {
                // nothing to sort keep on trucking
            }

            e.Handled = true;
         
        }

         
        /// <summary>
        /// used to sort the account table only!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAccounts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string strColText = dgvReportInsurance.Columns[e.ColumnIndex].HeaderText;
            //nFilterColumn = e.ColumnIndex;
            System.Windows.Forms.SortOrder SO = dgvReportInsurance.SortOrder == System.Windows.Forms.SortOrder.None ? System.Windows.Forms.SortOrder.Ascending : dgvReportInsurance.SortOrder;

            BindingSource bbs = new BindingSource(m_dtDataView, "ACCOUNT");// m_dsAccount.Tables["ACC"], "ACCOUNT");

            BindingSource bs = new BindingSource(dgvReportInsurance.DataSource,
                dgvReportInsurance.Columns[e.ColumnIndex].Name);
            bs.DataMember = dgvReportInsurance.DataMember;
            string strFilter = bs.Filter;

            if (e.Button == MouseButtons.Right)
            {

                string strColName = dgvReportInsurance.Columns[e.ColumnIndex].Name;
                FormResponse f = new FormResponse();
                foreach (DataGridViewRow dr in dgvReportInsurance.Rows)
                {
                    string strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (strColText == "pat_name")
                    {
                        MessageBox.Show("Cannot filter on the Pat_name");
                        return;
                      //  strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString().Split(new char[] { ',' })[0].ToString();
                    }

                    if (!f.clbFilter.Items.Contains(strText))
                    {
                        f.clbFilter.Items.Add(strText);
                    }
                }
                string strResponse = null;
                string strFilterHelper = null;
                if (f.ShowDialog() == DialogResult.Yes)
                {
                    foreach (string str in f.clbFilter.CheckedItems)
                    {
                        strResponse += string.Format("'{0}',", str);
                        strFilterHelper += string.Format(" pat_name like '{0}%' or ", str);
                    }
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        //   int nli = strResponse.LastIndexOf(',');
                        //   int nlen = strResponse.Length;
                        strResponse = strResponse.Remove(strResponse.LastIndexOf(','));
                        strFilterHelper = strFilterHelper.Remove(strFilterHelper.LastIndexOf("or"));
                    }

                }

                //BindingSource 
                bs = new BindingSource(dgvReportInsurance.DataSource,
                    strColText);
                bs.DataMember = dgvReportInsurance.DataMember;


                if (string.IsNullOrEmpty(strResponse))
                {
                    bs.RemoveFilter();

                }
                else
                {

                    bs.Filter = string.Format("[{0}] in ({1})  ",
                        strColText, strResponse);
                }
                bs.Sort = string.Format("{0} {1},pat_name ASC", dgvReportInsurance.Columns[e.ColumnIndex].Name,
                    SO == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC");
                dgvReportInsurance.DataSource = bs;
            }
            else
            {
                string strSort = string.Format("{0} {1}, pat_name ASC", dgvReportInsurance.Columns[e.ColumnIndex].Name
                    , SO == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC"
                    );


                bs.Sort = strSort;
                dgvReportInsurance.DataSource = bs;
            }
            //tsslRecords.Text = string.Format("{0} Selected records.", dgvAccounts.Rows.Count - 1);
        }

   




    } // don't type below this line
}