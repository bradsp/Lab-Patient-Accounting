using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// Programmer added
using RFClassLibrary;
using MCL;
using System.Data.SqlClient;
using System.Drawing.Printing;  // billing

          

namespace LabBilling.Legacy
{
    public partial class frmGlobalBilling : Form
    {
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }
        R_ins m_rIns = null;
        R_pat m_rPat = null;
        DataSet m_dsJPG = null;
        R_chrg m_rChrgCurrent = null;
        R_chrg m_rChrgNew = null;
        R_chrg m_rChrgJPG = null;
        R_acc m_rAccOrig = null;
        R_acc m_rAccJPG = null;
        R_cli_dis m_rCliDisJPG = null;

        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;
        int m_nFilterColumn = 1;
        
        DateTime m_dtReportsThru;
        DateTime m_dtReportsFrom;

        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        ERR m_Err = null;
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        ToolStripControlHost m_cboxInclude; // CheckBox

        public frmGlobalBilling(string[] args)
        {
            InitializeComponent();
            if (args.GetUpperBound(0) < 1)
            {
                MessageBox.Show("Not enough arguments to start this application", propAppName);
                Environment.Exit(13);
            }
            m_strServer = args[0].Remove(0, 1);
            m_strDatabase = args[1].Remove(0, 1);
            m_strProductionEnvironment = m_strDatabase.Contains("LIVE") ? "LIVE" : "TEST";
            string[] strArgs = new string[3];
            strArgs[0] = string.Format("/{0}", m_strProductionEnvironment);
            strArgs[1] = args[0];
            strArgs[2] = args[1];
            m_Err = new ERR(strArgs);

            m_rChrgCurrent = new R_chrg(m_strServer, m_strDatabase, ref m_Err);
            m_rChrgNew = new R_chrg(m_strServer, m_strDatabase, ref m_Err);
            m_rChrgJPG = new R_chrg(m_strServer, m_strDatabase, ref m_Err);
            m_rCliDisJPG = new R_cli_dis(m_strServer, m_strDatabase, ref m_Err);
            m_rAccOrig = new R_acc(m_strServer, m_strDatabase, ref m_Err);
            m_rAccJPG = new R_acc(m_strServer, m_strDatabase, ref m_Err);
            m_rIns = new R_ins(m_strServer, m_strDatabase, ref m_Err);
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);

            //this.Text += string.Format(" Production environment {0}", m_strProductionEnvironment);
           

        }
        
        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dtReportsFrom = new DateTime(2012,1,20);
            m_dpFrom.Text = m_dtReportsFrom.ToShortDateString();
            ((DateTimePicker)m_dpFrom.Control).MinDate = new DateTime(2012, 1, 20);
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpFrom.Control).TextChanged += new EventHandler(frmMonthEnd_FromTextChanged);
            m_dpFrom.Control.Width = 95;
            m_dpFrom.Control.Refresh();
            m_dpFrom.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFrom = new ToolStripLabel("From: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

            m_dpThru = new ToolStripControlHost(new DateTimePicker());
           // m_dpThru.Text = DateTime.Now.AddDays(5).ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
            int nDays = -4;
            int nDIM = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            int nTDay = DateTime.Today.Day;
            // wdk 20130401 if there are less than 4 days in the month shorten the lead time.
            if (nDIM - nTDay < 4)
            {
                nDays = nTDay - nDIM;
            }
            // wdk 20130401 if we have not posted (usually on the 5th) ensure the last 4 days of the month 
            // charges continue to be put on the JPG's bill.
            if (nTDay <= 4)
            {
                nDays = -1 * nTDay;
            }

            m_dtReportsThru = DateTime.Today.AddDays(nDays);
                //new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1).AddMonths(1).AddDays(-1);
            m_dpThru.Text = m_dtReportsThru.ToShortDateString();
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpThru.Control).TextChanged += new EventHandler(frmMonthEnd_ThruTextChanged);
            m_dpThru.Control.Width = 95;
            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
            //   tsMain.BackColor = Color.Lavender;

            // wdk 20100322 added check box
            ToolStripLabel tslInclude = new ToolStripLabel("Include \"J's\" in Filter");
            m_cboxInclude = new ToolStripControlHost(new CheckBox());
            m_cboxInclude.Enabled = false;
            tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
            tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

            //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }

        void frmMonthEnd_ThruTextChanged(object sender, EventArgs e)
        {
            m_dtReportsThru = ((DateTimePicker)sender).Value;
        }
        void frmMonthEnd_FromTextChanged(object sender, EventArgs e)
        {
            m_dtReportsFrom = ((DateTimePicker)sender).Value;
        }
        
        private void frmGlobalBilling_Load(object sender, EventArgs e)
        {
            CreateDateTimes();
            m_ViewerPrintDocument = new PrintDocument();
         //   m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            m_rgReport.m_dgvpReport.propFooterText = "Carol Plumlee";
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);

        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {

            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X + 50, Cursor.Position.Y + 50);
            Cursor.Current = Cursors.WaitCursor;
           // Cursor.Clip = new Rectangle(this.Location, this.Size);
           
            tsbBill.Enabled = !((CheckBox)m_cboxInclude.Control).Checked;
            m_dsJPG = new DataSet();
            using (SqlConnection conn =
            new SqlConnection(
                string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                m_strServer, m_strDatabase)))
            {
                // TODO: once the HC global billing issue is resolved modify the below stored procedure
                // to add an inner join on the acc and add cl_mnem <> 'HC' to the filter
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand =
                    new SqlCommand(
                        string.Format("exec usp_prg_global_billing @startDate = '{0} 00:00',@endDate = '{1} 23:59:59'",
                        m_dtReportsFrom.ToString("d"), m_dtReportsThru.ToString("d")), conn);

                
                sda.SelectCommand.CommandTimeout += sda.SelectCommand.CommandTimeout * 2;
                
                sda.Fill(m_dsJPG);
                if (!((CheckBox)m_cboxInclude.Control).Checked)
                {
                    DataRow[] drJs = m_dsJPG.Tables[0].Select("account like 'J%'");
                    tspbRecords.Minimum = 0;
                    tspbRecords.Maximum = dgvRecords.Rows.Count;
                    foreach (DataRow dr in drJs)
                    {
                        Application.DoEvents();
                        tspbRecords.PerformStep();
                        m_dsJPG.Tables[0].Rows.Remove(dr);
                    }
                }
                dgvRecords.DataSource = m_dsJPG.Tables[0];
                Application.DoEvents();
                tsslRecords.Text = string.Format("{0} Records ", dgvRecords.Rows.Count);
            }
            tspbRecords.Minimum = 0;
            tspbRecords.Maximum = dgvRecords.Rows.Count;
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                Application.DoEvents();
                tspbRecords.PerformStep();
                if (dr.Cells["fin_code"].Value.ToString() == "Y")
                {
                    ((DataRowView)dr.DataBoundItem).Row.RowError = "CHECK BEFORE BILLING";
                }
                if (DateTime.Parse(dr.Cells["DOS"].Value.ToString()).AddDays(90) < DateTime.Today)
                {
                    ((DataRowView)dr.DataBoundItem).Row.RowError = "DO NOT PROCESS -- TOO OLD";
                }

            }
            this.Refresh();
            MessageBox.Show("Load Completed",propAppName);
        }

        /// <summary>
        /// used to sort the account table only!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRecords_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_nFilterColumn = e.ColumnIndex;
            if (e.Button == MouseButtons.Right)
            {
                //  works but has limited use.
                FormResponse f = new FormResponse();
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    string strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString();
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
                string strColText = dgvRecords.Columns[m_nFilterColumn].HeaderText;
                BindingSource bs = new BindingSource(dgvRecords.DataSource,
                    strColText);
                bs.DataMember = dgvRecords.DataMember;
                if (string.IsNullOrEmpty(strResponse))
                {
                     bs.RemoveFilter();
                }
                else
                {
                    bs.Filter = string.Format("{0} in ({1})",  strColText, strResponse);
                }
                dgvRecords.DataSource = bs;
            }
            tsslRecords.Text = string.Format("{0} Records", dgvRecords.Rows.Count);
        }


        private void tsbPrint_Click(object sender, EventArgs e)
        {
            int nRows = dgvRecords.Rows.Count;
            if (nRows == 0)
            {
                MessageBox.Show("No records ready to print.", propAppName);
                return;
            }
            m_ViewerPrintDocument.Print();
        }

        private void tsbBill_Click(object sender, EventArgs e)
        {
            tsbBill.Enabled = false; // prevent second reversal
          /*
           * 1. copy the charge record to a temp holder
           * 2. credit the charge record
           * 3. write the reverse charge record
           * 4. add new charge to JPG account with temp info to get new price
           */
            tspbRecords.Minimum = 0;
            tspbRecords.Maximum = dgvRecords.Rows.Count;
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                
                if (!string.IsNullOrEmpty(dr.ErrorText))
                {
                    //continue;
                }
                tspbRecords.PerformStep();
                Application.DoEvents();
                string strAcc = dr.Cells["account"].Value.ToString().ToUpper();
                string strAccReplace = strAcc.Replace("C", "J");
                if (strAccReplace.StartsWith("D"))
                {
                    strAccReplace.Replace("D", "J");
                }
                string strWhereOrigAcc = string.Format("account = '{0}'",strAcc);
                string strWhereAcc = string.Format("account = '{0}'", strAcc);
                int nRec = m_rAccOrig.GetActiveRecords(strWhereAcc);
                if (nRec == 1)
                {
                    string strMsg = "";
                    if (m_rAccOrig.m_strStatus.ToUpper() != "NEW")
                    {
                        if (m_rAccOrig.UpdateField("status", "NEW", strWhereOrigAcc, out strMsg) == -1)
                        {
                            m_Err.m_Logfile.WriteLogFile(strMsg);
                        }
                    }
                }
                m_rAccJPG.GetActiveRecords(strWhereAcc);
                    
                string strWhereChrg = string.Format("account = '{0}' and chrg_num = '{1}'",
                    strAcc, dr.Cells["chrg_num"].Value.ToString());
                
                // try a transaction
               

                nRec = m_rChrgCurrent.GetRecords(strWhereChrg);
                m_rChrgNew.GetRecords(strWhereChrg);
                
                if (nRec == 1)
                {
                    m_rChrgCurrent.propCredited = "1";
                    int nRecUpdate = -1;
                    
                        nRecUpdate = m_rChrgCurrent.Update();
                
                        if (nRecUpdate != 1)
                        {
                            m_Err.m_Logfile.WriteLogFile(strWhereChrg);
                            m_Err.m_Logfile.WriteLogFile(
                                string.Format("Connection Timeout: {0}",m_rChrgCurrent.propDBConnection.ConnectionTimeout)); // wdk 20130701 added for debugging
                            MessageBox.Show(m_rChrgCurrent.propErrMsg, propAppName);
                        }
                    
                    m_rChrgNew.m_rsAmt.propChrg_num = "";

                    m_rChrgNew.propAccount = strAcc;
                    m_rChrgNew.propCredited = "1";
                    m_rChrgNew.propComment = string.Format("moved to [{0}]",strAccReplace);
                    int nQty = int.Parse(m_rChrgNew.propQty);
                    m_rChrgNew.propQty = string.Format("{0}",int.Parse(m_rChrgNew.propQty) * -1);
                    m_rChrgNew.m_strRowguid = Guid.NewGuid().ToString();
                    strWhereChrg = string.Format("rowguid = '{0}'", m_rChrgNew.m_strRowguid);
                    int nRecChrgAdded = m_rChrgNew.AddRecord(strWhereChrg);
                    if (nRecChrgAdded == 1)
                    {
                        m_rChrgCurrent.m_rsAmt.propChrg_num = m_rChrgNew.propChrg_num;
                        
                        m_rChrgCurrent.m_rsAmt.AddRecord(
                            string.Format("chrg_num = '{0}'",m_rChrgNew.propChrg_num));

                    }
                    //then add jpg charge record
                    WriteJpgCharge(m_rChrgCurrent.propCdm);
                }

                // copy the pat record
                m_rPat.ClearMemberVariables();
                int nPatRec = m_rPat.GetActiveRecords(strWhereOrigAcc);
                m_rPat.propAccount = strAcc.Replace('C', 'J').Replace('D', 'J');
                m_rPat.m_strRowguid = Guid.NewGuid().ToString();
                m_rPat.m_strModDate = DateTime.Now.ToString();
                m_rPat.m_strModUser = OS.GetUserName();
                m_rPat.m_strModPrg = string.Format("{0} {1}",Application.ProductName , Application.ProductVersion);
                m_rPat.m_strModHost = OS.GetMachineName();
                m_rPat.AddRecord();
                // copy the ins record
                m_rIns.ClearMemberVariables();
                int nInsRec = m_rIns.GetActiveRecords(strWhereOrigAcc);
                m_rIns.propAccount = strAcc.Replace('C', 'J').Replace('D', 'J');
                m_rIns.m_strRowguid = Guid.NewGuid().ToString();
                m_rIns.m_strModDate = DateTime.Now.ToString();
                m_rIns.m_strModUser = OS.GetUserName();
                m_rIns.m_strModPrg = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
                m_rIns.m_strModHost = OS.GetMachineName();
                nInsRec = m_rIns.AddRecord(string.Format("rowguid = '{0}'", m_rIns.m_strRowguid));


                    
            }                    
            
            m_dsJPG.Clear();
            tsbLoad_Click(null, null);
            tsbPrint_Click(null, null);
            MessageBox.Show("POSTING COMPLETE \r\r\nDeliver print out to Billing.",propAppName);

                
            
        }

        private void WriteJpgCharge(string strCdm)
        {
            m_rCliDisJPG.ClearMemberVariables();
            int nRec = m_rCliDisJPG.GetActiveRecords(
                string.Format("cli_mnem = 'JPG' and start_cdm = '{0}'", strCdm));
            if (nRec == 1)
            {
                if (string.IsNullOrEmpty(m_rCliDisJPG.propPrice))
                {
                    string strErr = string.Format("account {0} with cdm {1} has no price in client discount table. Sending Email to LIS.", m_rAccJPG.m_strAccount.ToUpper(), strCdm);
                    MessageBox.Show(string.Format("{0}\r\r\n{1}","POSTING COMPLETE",strErr ) ,propAppName);
                    m_Err.m_Email.Send(string.Format("{0}.GlobalBilling@WTH.ORG", m_strProductionEnvironment), "carol.sellars@wth.org;jan.smith@wth.org;david.kelly@wth.org", "cdm missing price", strErr);
                    Environment.Exit(13);
                }
            }
            string strAccount = m_rAccJPG.m_strAccount.ToUpper().Replace("C","J");
            if (strAccount.StartsWith("D"))
            {
                strAccount = strAccount.Replace("D", "J");
            }
            if (m_rAccJPG.GetActiveRecords(string.Format("account = '{0}'", strAccount)) < 1)
            {
                // write the account first
                m_rAccJPG.ClearMemberVariables();
                m_rAccJPG.m_strAccount = strAccount;
                m_rAccJPG.m_strPatName = m_rAccOrig.m_strPatName;
                m_rAccJPG.m_strCliMnem = "JPG";
                m_rAccJPG.m_strFinCode = "Y";
                m_rAccJPG.propTransDate = m_rAccOrig.propTransDate;
                m_rAccJPG.m_strStatus = m_rAccOrig.m_strStatus;
                m_rAccJPG.m_strSsn = m_rAccOrig.m_strSsn;
                m_rAccJPG.m_strMeditechAccount = strAccount;
                m_rAccJPG.m_strOriginalFincode = m_rAccOrig.m_strOriginalFincode;
                m_rAccJPG.m_strMri = m_rAccOrig.m_strMri;

                int nRecAcc = m_rAccJPG.AddRecord();
                if (nRecAcc != 1)
                {
                    MessageBox.Show(string.Format("Account [{0}] not added", strAccount),propAppName);
                }
            }
            else
            {
                // if the accounts status is not NEW make it NEW to prevent aging history from skipping the account
                if (m_rAccJPG.m_strStatus != "NEW")
                {
                   // MessageBox.Show(string.Format("account = '{0}'", strAccount) + " acc updated to 'NEW' status",propAppName);
                    string strMsg = "";
                    m_rAccJPG.UpdateField("status", "NEW", string.Format("account = '{0}'", strAccount), out strMsg);
                    if (!string.IsNullOrEmpty(strMsg))
                    {
                        m_Err.m_Logfile.WriteLogFile(strMsg);
                    }
                }
            }
            // write the charges
            m_rChrgJPG.ClearMemberVariables();
            m_rChrgJPG.m_strRowguid = Guid.NewGuid().ToString();
            m_rChrgJPG.propAccount = strAccount; 
            m_rChrgJPG.propStatus = "NEW";
            m_rChrgJPG.propService_date = m_rChrgCurrent.propService_date;
            m_rChrgJPG.propCdm = strCdm;
            m_rChrgJPG.propQty = m_rChrgCurrent.propQty;
            m_rChrgJPG.propRetail = m_rChrgCurrent.propRetail;
            m_rChrgJPG.propInp_price = m_rChrgCurrent.propInp_price;
            m_rChrgJPG.propComment = string.Format("moved from account [{0}]", m_rChrgCurrent.propAccount);
            m_rChrgJPG.propNet_amt = m_rCliDisJPG.propPrice;
                //(int.Parse(m_rChrgCurrent.propQty)*double.Parse(m_rCliDisJPG.propPrice)).ToString("F2");
            m_rChrgJPG.propFin_type = "C";
            m_rChrgJPG.propFinCode = "Y";
            string strWhere = string.Format("rowguid = '{0}'", m_rChrgJPG.m_strRowguid);

            int nRecChrg = m_rChrgJPG.AddRecord(strWhere);
            if (nRecChrg == 1)
            {
                m_rChrgJPG.m_rsAmt.propChrg_num = m_rChrgJPG.propChrg_num;
                m_rChrgJPG.m_rsAmt.propCpt4 = m_rChrgCurrent.m_rsAmt.propCpt4;
                m_rChrgJPG.m_rsAmt.propAmount = m_rCliDisJPG.propPrice;
                m_rChrgJPG.m_rsAmt.propType = "TC";
                m_rChrgJPG.m_rsAmt.propModi = "TC";
                m_rChrgJPG.m_rsAmt.propRevcode = "300";

                int nRecAmt = m_rChrgJPG.m_rsAmt.AddRecord(
                    string.Format("chrg_num = '{0}'", m_rChrgJPG.propChrg_num));
                if (nRecAmt != 1)
                {
                    string strErr = string.Format("Error in moving charge [0] to JPG account [1]", m_rChrgJPG.propChrg_num, m_rChrgJPG.propAccount);
                    MessageBox.Show(strErr, propAppName);
                }

            }
            InsertIntoMergedTable(m_rAccOrig.m_strAccount.ToUpper(), strAccount);
            
        }

        private void InsertIntoMergedTable(string strOrigAcc, string strDupAcc)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            DataTable dtMergedAcc = new DataTable();
            sda.SelectCommand = new SqlCommand(
                string.Format("insert into acc_merges (account, dup_acc, mod_prg) " +
                "values ('{0}','{1}','{2}')", strOrigAcc, strDupAcc, propAppName), new SqlConnection(
                string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                m_strServer, m_strDatabase)));

            sda.SelectCommand.Connection.Open();
            try
            {
                int nX = sda.SelectCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                // this application has multiple accounts listed so do nothing once the inital item is loaded
            }
            finally
            {
                sda.SelectCommand.Connection.Close();
            }
            


        }

        private void dgvRecords_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                LaunchAcc la = new LaunchAcc(m_strDatabase);
                string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                if (strAcc.StartsWith("C"))
                {
                    la.LaunchAccount(strAcc);
                    la.LaunchAccount(strAcc.Replace('C', 'J'));
                }
                else
                {
                    la.LaunchAccount(strAcc);
                    la.LaunchAccount(strAcc.Replace('J', 'C'));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Exception occured trying to open the account.", propAppName);
            }
        }

    } // don't go here
}
