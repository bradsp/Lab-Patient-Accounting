using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
//
using RFClassLibrary;
using System.Diagnostics;


namespace LabBilling.Legacy
{
    public partial class frmCorrection : Form
    {

        DataSet m_ds360 = null;
        HtmlDocument m_htmDoc = null;
        WebBrowser m_wbPrint = null;
        Dictionary<string, string> m_dicPatient;
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }
        string m_strEmailAddress = null;
        string m_strEmail = null;
        //       static bool mailSent = false;
        SqlDataAdapter m_sdaAcc = null;
        DataTable m_dtAcc = null;
        SqlConnection m_sqlConnection = null;
        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        //  ERR m_Err = null;
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        // ToolStripControlHost m_cboxInclude; // CheckBox
        DateTime m_dtFrom;
        DateTime m_dtThru;
        //     private PrintDocument m_ViewerPrintDocument;
        //     private ReportGenerator m_rgReport;


        private void CreateDateTimes()
        {
            //ToolStrip tsDates = new ToolStrip();

            //int nSert = tsDates.Items.Count;
            //// create the datetime controls for the From and Thru dates
            //m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            //m_dpFrom.Text = "10/01/2012";
            //m_dtFrom = new DateTime(2012, 10, 1);
            //((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            //((DateTimePicker)m_dpFrom.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            //((DateTimePicker)m_dpFrom.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            //((DateTimePicker)m_dpFrom.Control).Name = "FROM";
            //m_dpFrom.Control.Width = 95;
            //m_dpFrom.Control.Refresh();
            //m_dpFrom.Invalidate();
            //tsDates.Items.Insert(tsDates.Items.Count, new ToolStripSeparator());
            //ToolStripLabel tslFrom = new ToolStripLabel("From: ");
            //tsDates.Items.Insert(tsDates.Items.Count, tslFrom);
            //tsDates.Items.Insert(tsDates.Items.Count, m_dpFrom);

            //m_dpThru = new ToolStripControlHost(new DateTimePicker());
            //m_dpThru.Text = DateTime.Now.AddDays(5).ToString();
            //m_dtThru = DateTime.Now.AddDays(5);
            //((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            //((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            //((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            //((DateTimePicker)m_dpThru.Control).Name = "THRU";
            //m_dpThru.Control.Width = 95;
            //m_dpThru.Control.Refresh();
            //m_dpThru.Invalidate();

            //ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            //tsDates.Items.Insert(tsDates.Items.Count, tslThru);
            //tsDates.Items.Insert(tsDates.Items.Count, m_dpThru);

            //// check box
            ////ToolStripLabel tslInclude = new ToolStripLabel("Include in Filter");
            ////m_cboxInclude = new ToolStripControlHost(new CheckBox());
            ////tsDates.Items.Insert(tsDates.Items.Count, tslInclude);
            ////tsDates.Items.Insert(tsDates.Items.Count, m_cboxInclude);

            //tsDates.Items.Insert(tsDates.Items.Count, new ToolStripSeparator());
            //tsDates.Refresh();
            //this.Controls.Add(tsDates);

            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dpFrom.Text = "10/01/2012";

            m_dtFrom = new DateTime(2012, 10, 1);
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpFrom.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpFrom.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpFrom.Control).Name = "FROM";
            ((DateTimePicker)m_dpFrom.Control).MinDate = m_dtFrom;
            m_dtFrom = DateTime.Today.AddDays(-60);
            m_dpFrom.Text = m_dtFrom.ToShortDateString();
            m_dpFrom.Control.Width = 105;
            m_dpFrom.Control.Refresh();
            m_dpFrom.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFrom = new ToolStripLabel("FROM: ");
            tslFrom.Width = 100;
            tslFrom.Height = 20;
            tslFrom.TextAlign = ContentAlignment.MiddleCenter;
            tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

            m_dpThru = new ToolStripControlHost(new DateTimePicker());
            m_dpThru.Text = DateTime.Now.AddDays(5).ToString();
            m_dtThru = DateTime.Now.AddDays(5);
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpThru.Control).Name = "THRU";
            m_dpThru.Control.Width = 105;


            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("THRU: ");
            tslThru.Width = 100;
            tslThru.Height = 20;
            tslThru.Alignment = ToolStripItemAlignment.Right;
            tslThru.TextAlign = ContentAlignment.BottomCenter;
            tslThru.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);

            // check box
            //ToolStripLabel tslInclude = new ToolStripLabel("Include in Filter");
            //m_cboxInclude = new ToolStripControlHost(new CheckBox());
            //tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
            //tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }
        void frmAcc_CloseUp(object sender, EventArgs e)
        {
            //  Requery();
        }

        void frmAcc_ValueChanged(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).Name == "FROM")
            {
                m_dtFrom = ((DateTimePicker)sender).Value;
            }
            else
            {
                m_dtThru = ((DateTimePicker)sender).Value;
            }
            // Query();

        }


        private void dgvAccount_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //LaunchAcc la = new LaunchAcc(m_strDatabase);
                string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                //la.LaunchAccount(strAcc);

                new LabBilling.Forms.AccountForm(strAcc, this.ParentForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Exception occured trying to open the account. \r\n {0}", ex.Message));
            }

        }

        private bool DgvBillingUpdate(object sender, DataGridViewCellMouseEventArgs e)
        {
            throw new Exception("should not be here after 09/09/2014. Call David 17379.");
            //return false;
            //bool bRetVal = false;

            //bool bEntered = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["deleted"].Value);

            //if (!bEntered)
            //{
            //    return bRetVal;
            //}
            //string strReqNo = ((DataGridView)sender).Rows[e.RowIndex].Cells["req_no"].Value.ToString();

            //SqlCommand cmdUpdate = new SqlCommand(
            //string.Format("update data_quest_billing " +
            //    "set deleted = 1, " +
            //    "mod_prg = '{0}', mod_date = '{1}', mod_user = '{2}', mod_host = '{3}' " +
            //    " where req_no = '{4}'", 
            //        string.Format("{0} {1}", Application.ProductName, Application.ProductVersion),
            //            DateTime.Today, 
            //                Environment.UserName, 
            //                    Environment.MachineName,
            //                        strReqNo)
            //    , m_sqlConnection);
            ////SqlDataAdapter sda = new SqlDataAdapter(cmdSelect);
            ////int nSelRec = sda.Fill(m_dtUser);
            //SqlDataAdapter sda = new SqlDataAdapter();
            //sda.UpdateCommand = cmdUpdate;
            //sda.UpdateCommand.Connection.Open();

            //int nUpdates = -1;
            //try
            //{
            //    nUpdates = sda.UpdateCommand.ExecuteNonQuery();
            //}
            //catch (SqlException se)
            //{
            //    string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
            //    strText += "\r\n\r\nModule: dgvBillingUpdate()";
            //    MessageBox.Show(strText);

            //}
            //finally
            //{
            //    sda.UpdateCommand.Connection.Close();
            //}

            //((DataGridView)sender).Rows[e.RowIndex].ErrorText = "SUCCESS";
            //((DataGridView)sender).Invalidate();
            //this.Invalidate();
            //return (nUpdates > 0);
        }




        public frmCorrection(string[] args)
        {
            InitializeComponent();

            if (args.GetUpperBound(0) < 1)
            {
                MessageBox.Show("Not enough arguments to start this application");
                Environment.Exit(13);
            }
            m_strServer = args[0].Remove(0, 1);
            m_strDatabase = args[1].Remove(0, 1);
            m_strProductionEnvironment = m_strDatabase.Contains("LIVE") ? "LIVE" : "TEST";
            string[] strArgs = new string[3];
            strArgs[0] = string.Format("/{0}", m_strProductionEnvironment);
            strArgs[1] = args[0];
            strArgs[2] = args[1];
            //     m_Err = new ERR(strArgs);

            this.Text += string.Format(" - Production Environment {0}", m_strProductionEnvironment);

            //    m_ViewerPrintDocument = new PrintDocument();
            //m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            //    string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            //    m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            //   m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
            m_sqlConnection =
               new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                   + "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));

            if (m_strProductionEnvironment.Contains("TEST"))
            {
                this.BackColor = Color.IndianRed;
            }

        }

        private void frmCorrection_Load(object sender, EventArgs e)
        {
            //this.Cursor = new Cursor(Cursor.Current.Handle);
            //Cursor.Position = new Point(Cursor.Position.X + 50, Cursor.Position.Y + 50);
            //Cursor.Current = Cursors.AppStarting;

            CreateDateTimes();
            LoadEmail();

            tiReload = new System.Windows.Forms.Timer();
            tiReload.Interval = 120000;
            tiReload.Tick += new EventHandler(tiReload_Tick);

            dgv360.TopLeftHeaderCell.Value = "DATA_QUEST_360";
            //    dgvBilling.TopLeftHeaderCell.Value = "DATA_QUEST_BILLING";
            dgvChrg.TopLeftHeaderCell.Value = "CHRG";

            // Cursor.Current = Cursors.Arrow;

        }

        private void LoadEmail()
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            DataTable dtEmail = new DataTable();
            SqlCommand cdmSel = new SqlCommand("select value from system where key_name = 'Quest_Billing'", m_sqlConnection);
            sda.SelectCommand = cdmSel;
            int nRec = sda.Fill(dtEmail);
            if (nRec != 1)
            {
                return;
            }
            m_strEmail = dtEmail.Rows[0]["value"].ToString();
            //new ArrayList(dtEmail.Rows[0]["value"].ToString().Split(new char[] {'|'}));
            //     throw new NotImplementedException();
        }

        void tiReload_Tick(object sender, EventArgs e)
        {
            tsbReload_Click(null, null);
        }

        private void Query()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X + 50, Cursor.Position.Y + 50);
            Cursor.Current = Cursors.AppStarting;
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                string.Format("select account, pat_name, status, trans_date from acc  " +
                " where trans_date between '{0} 00:00' and '{1} 23:59:59' " +
                " and not account in (select cli_mnem from client where type between 3 and 9 ) " +
                " and (fin_code in ('D') or cl_mnem in ('QUESTR','QUESTREF')) " +
                " order by pat_name "
                , m_dtFrom.ToShortDateString(), m_dtThru.ToShortDateString()), m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";

            tsslRecords.Text = string.Format(" {0} Accounts ", m_dtAcc.Rows.Count);

        }

        private void tscbAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            int nSelRec = ((ToolStripComboBox)sender).SelectedIndex;
            if (nSelRec == -1)
            {
                return;
            }
            string strAcc = null;
            try
            {
                strAcc = ((DataRowView)((ToolStripComboBox)sender).SelectedItem).Row[0].ToString();
            }
            catch (NullReferenceException)
            {
                return;
            }
            if (string.IsNullOrEmpty(strAcc))
            {
                return;
            }
            string strName = m_dtAcc.Rows[nSelRec]["pat_name"].ToString();
            string strTransDate = m_dtAcc.Rows[nSelRec]["trans_date"].ToString();

            string strReqNo = null;
            try
            {
                strReqNo = m_dtAcc.Rows[nSelRec]["req_no"].ToString();
            }
            catch (ArgumentException)
            {
            }
            int nParse = 0;
            foreach (char c in strAcc)
            {
                if (Char.IsDigit(c))
                {
                    break;
                }
                nParse++;
            }
            Application.DoEvents();
            tvMain.Nodes.Clear();

            DataRow[] drAccts = m_dtAcc.Select(
                string.Format("account like '%{0}'", strAcc.Substring(nParse)));


            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter adp = new SqlDataAdapter();
                DataTable dt360 = new DataTable();
                dgv360.DataSource = null;
                dgv360.Rows.Clear();

                adp.SelectCommand = new SqlCommand(
                    string.Format("select " +
                    "uid as [req_no], deleted , patient, patid , html_doc , account , date_of_service , " +
                    "entered AS [Demo Entered], charges_entered AS [Charges Entered], entry_date ," +
                    "emailed, transmission_date , " +
                    " bill_type , mod_date ,  mod_user ,    mod_prg ,     mod_host, rowguid, uid" +
                    " from data_quest_360 where account like '%{0}'" +
                    " and transmission_date is not null",
                    strAcc.Substring(nParse)), conn);
                adp.Fill(dt360);
                dgv360.DataSource = dt360;

                if (dt360.Rows.Count == 0)
                {
                    ClearGrids();

                    adp.SelectCommand = new SqlCommand(
                      string.Format("SELECT distinct dbo.data_quest_billing.req_no , " +
                      "dbo.data_quest_billing.deleted , " +
                      "dbo.data_quest_billing.Patient , " +
                      "dbo.data_quest_360.patid, " +
                      "dbo.data_quest_360.html_doc, " +
                      "dbo.data_quest_billing.account , " +
                      "dbo.data_quest_billing.collection_date AS [date_of_service], " +
                      "1 AS [Demo Entered], " +
                      "1 AS [Charges Entered], " +
                      "dbo.data_quest_billing.date_entered AS [entry_date], " +
                      "NULL AS [emailed], " +
                      "NULL AS [transmission_date]	, " +
                      "'Q' AS [bill_type], " +
                      "dbo.data_quest_billing.mod_date , " +
                      "dbo.data_quest_billing.mod_user , " +
                      "dbo.data_quest_billing.mod_prg , " +
                      "dbo.data_quest_billing.mod_host , " +
                      "dbo.data_quest_360.rowguid , " +
                      "dbo.data_quest_billing.uid " +
                      "FROM dbo.data_quest_billing  " +
                      "inner join dbo.data_quest_360 ON dbo.data_quest_360.account = dbo.data_quest_billing.account " +
                      "WHERE dbo.data_quest_billing.account LIKE '%{0}'", strAcc.Substring(nParse)), conn);
                    adp.Fill(dt360);
                    dgv360.DataSource = dt360;

                }


                DataTable dtChrg = new DataTable();
                adp.SelectCommand = new SqlCommand(
                    string.Format("SELECT  account,   chrg_num, service_date, cdm, qty,net_amt, comment, invoice, " +
                    "mod_date, mod_user, mod_prg,  mod_host, post_date, fin_code " +
                    "FROM         chrg " +
                    "where credited = 0 " +
                    "and account like '%{0}' ", strAcc.Substring(nParse)), conn);
                adp.Fill(dtChrg);
                dgvChrg.DataSource = dtChrg;

            }


        }

        private void tsbCorrection_Click(object sender, EventArgs e)
        {
            dgv360.EndEdit();
            DataGridViewRow dr = new DataGridViewRow();
            try
            {
                dr = dgv360.SelectedRows[0];
            }
            catch (Exception)
            {
                MessageBox.Show("You must select a row.");
                return;
            }

            string strReqNo = dgv360.SelectedRows[0].Cells["req_no"].Value.ToString();
            if (string.IsNullOrEmpty(strReqNo))
            {
                MessageBox.Show("You must select a requisition to report cancelled.");
                return;
            }
            else
            {
                if (MessageBox.Show(string.Format("Select Yes to cancel requisition number {0}", strReqNo),
                    "CANCEL REQ", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }


            string strLine = null;
            StringBuilder sbMessage = new StringBuilder();

            if (((bool)dr.Cells["deleted"].Value))
            {
                MessageBox.Show("Deleted requisitions can not be cancelled.", "CANCEL REQ");
                return;
            }

            sbMessage.Append(Environment.NewLine);
            sbMessage.Append("Canceled in Care360.\r\n");
            sbMessage.Append(Environment.NewLine);

            if (m_strProductionEnvironment == "LIVE")
            {
                strLine = string.Format("REQUISITION NUMBER: {0}\r\n",
                    dr.Cells["req_no"].Value.ToString().PadRight(18));
                sbMessage.Append(strLine);
                strLine = string.Format("PATIENT NAME: {0}\r\n",
                        dr.Cells["Patient"].Value.ToString().PadRight(20));
                sbMessage.Append(strLine);
                strLine = string.Format("DATE OF SERVICE: {0}\r\n",
                            DateTime.Parse(dr.Cells["date_of_service"].Value.ToString()).ToShortDateString().PadRight(15));
                sbMessage.Append(strLine);
                strLine = string.Format("REASON: {0}\r\n",
                                    dr.Cells["patid"].Value.ToString());

                sbMessage.Append(strLine);

                strReqNo = dr.Cells["req_no"].Value.ToString();


                strLine = string.Format("Cancellation of req no {0}", strReqNo);
            }
            else
            {
                //strReqNo = dr.Cells["req_no"].Value.ToString();
                strLine = string.Format("Testing of email Cancellation of req no {0}", strReqNo);
                m_strEmail = "david.kelly@wth.org";
            }

            using (SqlConnection conn = new SqlConnection(
                string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                m_strServer, m_strDatabase)))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = new SqlCommand(
                    string.Format("exec msdb.dbo.sp_send_dbmail @recipients = N'{2}', " +
                    " @body='{0}', @subject= N'{1}', @profile_name = N'WTHMCLBILL'", sbMessage.ToString(), strLine, m_strEmail), conn);
                sda.SelectCommand.Connection.Open();
                try
                {
                    sda.SelectCommand.ExecuteNonQuery();
                }
                finally
                {
                    sda.SelectCommand.Connection.Close();
                }
            }
            strReqNo = dr.Cells["req_no"].Value.ToString();

        }

        private void SendEmail(object state)
        {
            string strReqNo = null;
            string strLine = null;
            DataGridViewRow dr = dgvBilling.SelectedRows[0];
            if (((bool)dr.Cells["deleted"].Value))
            {
                return;
            }
            if (strReqNo == dr.Cells["req_no"].Value.ToString())
            {
                return;
            }

            SmtpClient client = new SmtpClient("WTHEXCH.WTH.ORG");
            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);



            {

                MailAddress from = new MailAddress("david.kelly@wth.org", "david", System.Text.Encoding.UTF8);
                MailAddress to =
                    new MailAddress(m_strEmailAddress);
                MailAddress cc = new MailAddress("vqc@Correction.lab");

                MailMessage message = new MailMessage(from, to);
                message.CC.Add(cc);
                // Include some non-ASCII characters in body and subject.
                //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                message.Body += Environment.NewLine;// +someArrows;

                message.Body += "Please Respond to David if you recieve this email. This is a one time element to verify that emails are being sent to Quest.";
                message.Body += Environment.NewLine;

                message.Body += "Canceled in Care360.";
                message.Body += Environment.NewLine;

                strLine = string.Format("REQUISITION NUMBER: {0}\r\n",
                    dr.Cells["req_no"].Value.ToString().PadRight(18));
                message.Body += strLine;
                strLine = string.Format("PATIENT NAME: {0}\r\n",
                        dr.Cells["Patient"].Value.ToString().PadRight(20));
                message.Body += strLine;
                strLine = string.Format("DATE OF SERVICE: {0}\r\n",
                            DateTime.Parse(dr.Cells["collection_date"].Value.ToString()).ToShortDateString().PadRight(15));
                message.Body += strLine;
                strLine = string.Format("REASON: {0}\r\n",
                                    dr.Cells["status"].Value.ToString());

                message.Body += strLine;

                strReqNo = dr.Cells["req_no"].Value.ToString();


                message.BodyEncoding = System.Text.Encoding.UTF8;

                message.Subject = string.Format("Cancellation of req no {0}", strReqNo);
                //"m_dtAcc.Rows[tscbAcc.SelectedIndex]";//["account"].ToString()";

                message.SubjectEncoding = System.Text.Encoding.UTF8;
                // string userState = "Send";


                try
                {
                    //    em.Send(from.ToString(), to.ToString(), message.Subject, message.Body);
                    client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //client.SendAsync(message, userState);
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    string strErr = ex.GetType().ToString();
                    strErr = ex.Message;
                }

            }

            //message.Dispose();
        }

        void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {

            string token = (string)e.UserState;
            if (e.Cancelled)
            {
            }
            if (e.Error != null)
            {
            }
            else
            {
            }
            //   mailSent = true;

        }



        private void dgv360_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["deleted"].Value))
            {
                SqlCommand cmdUpdate = new SqlCommand(
                string.Format("update data_quest_billing " +
                    "set deleted = 1, " +
                    "mod_prg = '{0}', mod_date = '{1}', mod_user = '{2}', mod_host = '{3}' " +
                    " where req_no = '{4}'", string.Format("{0} {1}", Application.ProductName, Application.ProductVersion),
                    DateTime.Today, Environment.UserName, Environment.MachineName,
                    ((DataGridView)sender).Rows[e.RowIndex].Cells["req_no"].Value.ToString())
                    , m_sqlConnection);
                //SqlDataAdapter sda = new SqlDataAdapter(cmdSelect);
                //int nSelRec = sda.Fill(m_dtUser);
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.UpdateCommand = cmdUpdate;
                sda.UpdateCommand.Connection.Open();

                int nUpdates = -1;
                try
                {
                    nUpdates = sda.UpdateCommand.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.GetType().ToString());
                    strText += "\r\ndgvAccount_RowHeaderMouseDoubleClick()";
                    MessageBox.Show(strText);

                }
                finally
                {
                    sda.UpdateCommand.Connection.Close();
                }

                ((DataGridView)sender).Rows[e.RowIndex].ErrorText = "SUCCESS";
                ((DataGridView)sender).Invalidate();
                this.Invalidate();
                return; // (nUpdates != 1);

            }
        }

        private void dgv360_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.ColumnIndex == -1) // row header
            {
                return;
            }
            if (dgv360.Columns[e.ColumnIndex].HeaderText.ToUpper() != "HTML_DOC")
            {
                return;
            }
            Form f = new Form();
            f.Text = dgv360.Rows[e.RowIndex].Cells["account"].Value.ToString();
            f.WindowState = FormWindowState.Maximized;
            WebBrowser wb = new WebBrowser();
            wb.Dock = DockStyle.Fill;
            wb.DocumentText = dgv360.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            f.Controls.Add(wb);
            f.Show();
        }

        private void tsbReload_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            Query();
        }

        private void TsbDups_Click(object sender, EventArgs e)
        {
            return;
            //ClearDataGridViews();
            //m_sdaAcc = new SqlDataAdapter();
            //m_dtAcc = new DataTable();
            //SqlCommand cmdSelct = new SqlCommand(
            //    string.Format("with cte "+
            //    "as "+
            //    "( 	select replace(account,'C','Q') as [account], patient as [pat_name], quest_code, req_no "+
            //    ",convert(varchar(10),collection_date,101) as [trans_date] "+
            //    ",'QBill' as [status] "+
            //    "from data_quest_billing "+
            //    "where deleted = 0 ) "+
            //    "select account, pat_name, quest_code, trans_date,count(req_no) as [Number of Reqs], status "+
            //    "from cte "+
            //    "group by account, pat_name, trans_date, quest_code, status "+
            //    "having count(req_no) > 1 "+
            //    "order by account, count(req_no) " 
            //    , m_dtFrom.ToShortDateString(), m_dtThru.ToShortDateString()), m_sqlConnection);

            //m_sdaAcc.SelectCommand = cmdSelct;
            //int nRec = m_sdaAcc.Fill(m_dtAcc);
            //tscbAcc.ComboBox.DataSource = m_dtAcc;
            //tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            //tscbName.ComboBox.DataSource = m_dtAcc;
            //tscbName.ComboBox.DisplayMember = "pat_name";
            //DataTableReader reader = m_dtAcc.CreateDataReader();

            //tsslRecords.Text = string.Format(" {0} Duplicate Accounts ", AccountCount(reader));

            //DataTable dtQuestBilling = new DataTable();
            //cmdSelct = new SqlCommand(
            //    "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            //m_sdaAcc.SelectCommand = cmdSelct;
            //nRec = m_sdaAcc.Fill(dtQuestBilling);
            //tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            //tscbReqNo.ComboBox.DisplayMember = "req_no";
            //tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            //tscbDQBAccount.ComboBox.DisplayMember = "account";

        }

        private void ClearDataGridViews()
        {
            try
            {
                m_dtAcc.Rows.Clear();
                dgv360.DataSource = null;
                dgv360.Rows.Clear();

                //dgvBilling.DataSource = null;
                //dgvBilling.Rows.Clear();

                dgvChrg.DataSource = null;
                dgvChrg.Rows.Clear();
            }
            catch (NullReferenceException)
            {
            }

        }

        private void dgv360_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //return;
            dgv360.EndEdit();
            //    bool bpre360_error = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["pre360_error"].Value);
            //    bool bbill_code_error = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["bill_code_error"].Value);
            bool bentered = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["Demo Entered"].Value); //demo entered
            bool bChrgEntered = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["Charges Entered"].Value); // charges
            string strRowguid = ((DataGridView)sender).Rows[e.RowIndex].Cells["rowguid"].Value.ToString();
            bool bDeleted = ((bool)((DataGridView)sender).Rows[e.RowIndex].Cells["deleted"].Value);

            SqlCommand cmdUpdate = new SqlCommand(
            string.Format("update data_quest_360 " +
                "set entered = '{0}', " +
                "deleted = '{1}', " +
                //"pre360_error = 0, bill_code_error = 0, " +
                "mod_prg = '{2}', mod_date = '{3}', mod_user = '{4}', mod_host = '{5}' " +
                ",charges_entered = '{6}' " +
                " where rowguid = '{7}'",
                bentered,
                bDeleted,
                propAppName,
                //string.Format("{0} {1}", Application.ProductName, Application.ProductVersion),
                DateTime.Today, Environment.UserName, Environment.MachineName, bChrgEntered,
                strRowguid
                  //bpre360_error, bbill_code_error,
                  )
                , m_sqlConnection);
            return;

            //// fix this 
            //SqlDataAdapter sda = new SqlDataAdapter();
            //sda.UpdateCommand = cmdUpdate;
            //sda.UpdateCommand.Connection.Open();

            //int nUpdates = -1;
            //try
            //{
            //    nUpdates = sda.UpdateCommand.ExecuteNonQuery();
            //}
            //catch (SqlException se)
            //{
            //    string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
            //    strText += "\r\n\r\nModule: dgv360_RowHeaderMouseDoubleClick()";
            //    MessageBox.Show(strText);

            //}
            //finally
            //{
            //    sda.UpdateCommand.Connection.Close();
            //}

            //((DataGridView)sender).Rows[e.RowIndex].ErrorText = "SUCCESS";
            //((DataGridView)sender).Invalidate();
            //this.Invalidate();


        }

        private void dgvBilling_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {


            }

        }

        void mi_Click(object sender, EventArgs e)
        {
            //   dgvBillingUpdate(object sender, DataGridViewCellMouseEventArgs e);
        }

        private void tscbReqNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tscbReqNo_DropDownClosed(object sender, EventArgs e)
        {
            if (tscbDQBAccount.ComboBox.SelectedIndex == -1)
            {
                return;
            }

            string strAccount = ((DataRowView)tscbDQBAccount.Items[tscbDQBAccount.SelectedIndex]).Row.ItemArray[1].ToString();
            try
            {
                tscbAcc.SelectedIndex = tscbAcc.FindStringExact(strAccount);
            }
            catch (Exception)
            {
            }
        }

        private void tsmiBillCodeErrors_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                "SELECT  dq3.account, pat_name, bill_code_error,  date_of_service as [trans_date],'QError' as [status] " +
                "FROM data_quest_360 dq3 " +
                "inner join acc on acc.account = dq3.account " +
                "WHERE     (bill_type = 'Q') and (entered = 1) AND (charges_entered = 0) and bill_code_error = 1 " +
                "order BY datepart(month, date_of_service), date_of_service, bill_code_error, dq3.account  "
                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";
            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Bill Code Error Accounts ", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";
        }

        private void tsmi360NotEntered_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                ";with cte " +
                "as " +
                "( " +
                "select  " +
                "case when chrg.account = 'QUESTR' " +
                "	then substring(chrg.comment " +// -- expression
                ",charindex('[',chrg.comment,0)+1 " +// -- start
                ",charindex(']',chrg.comment,0)-charindex('[',chrg.comment,0)) " +
                "else chrg.account " +
                "end as [account] " +
                ", cdm,  " +
                "case when chrg.account = 'QUESTR'  " +
                "then sum(qty*net_amt) over (partition by chrg.account, cdm, comment) " +
                "	else sum(qty*net_amt) over (partition by chrg.account, cdm) " +
                "end  as [charge]  " +
                ", comment as [notes] " +
                ", service_date as [DOS] " +
                ", pat_name  " +
                "from chrg  " +
                "inner join acc on acc.account = chrg.account " +
                "where coalesce(invoice,'') = ''  " +
                "and cdm <> 'CBILL' " +
                "and chrg.account in (select account from acc where cl_mnem = 'QUESTR') " +
                ")  " +
                ", cteQB " +
                "as " +
                "( " +
                "	SELECT  distinct req_no, REPLACE(account,'c','q') AS [ACCOUNT] " +
                "		, Patient " +
                "		, collection_date " +
                "		, DOB " +
                "		, data.quest_code " +
                "		, data.quest_desc " +
                "		, dict.cdm " +
                "		, date_entered " +
                "	FROM data_quest_billing data " +
                "	inner join dict_quest_reference_lab_tests dict on dict.quest_code = data.quest_code and data.deleted = 0 " +
                ") " +
                ", cteQBCredit " +
                "as " +
                "( " +
                "	SELECT  distinct req_no, REPLACE(account,'c','q') AS [ACCOUNT] " +
                "		, Patient " +
                "		, collection_date " +
                "		, DOB " +
                "		, data.quest_code " +
                "		, data.quest_desc " +
                "		, dict.cdm " +
                "		, date_entered " +
                "	FROM data_quest_billing data " +
                "	inner join dict_quest_reference_lab_tests dict on dict.quest_code = data.quest_code and data.deleted = 1 " +
                ") " +
                "select  " +
                "cte.account " +
                //"replace(cte.account,']','CR') as [account] "+
                ", coalesce(cteQB.cdm, cteQBCredit.cdm,cte.cdm) as [CDM] " +
                ", coalesce(cteQB.patient, cteQBCredit.patient,cte.pat_name) as [pat_name] " +
                ", coalesce(cteQB.req_no, cteQBCredit.req_no) as [req_no] " +
                ", convert(varchar(10),coalesce(cteQB.collection_date, cteQBCredit.collection_date,cte.dos),101) as [trans_date] " +
                ", coalesce(cteQB.DOB, cteQBCredit.DOB) as [Date of Birth] " +
                ", coalesce(cteQB.quest_code, cteQBCredit.quest_code) as [quest_code] " +
                ", coalesce(cteQB.quest_desc, cteQBCredit.quest_desc) as [quest_desc] " +
                ",cte.charge " +
                ",'Not Entered' as [status] " +
                "from cte  " +
                "left outer join cteQB on cteQB.account = cte.account and cteQB.cdm = cte.cdm " +
                "left outer join cteQBCredit on cteQBCredit.account+']' = cte.account and cteQBCredit.cdm = cte.cdm " +
                "where [charge] > 0  " +
                "and coalesce(cteQB.cdm, cteQBCredit.cdm)  is null " +
                "order by cte.dos,cte.account "

                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            m_sdaAcc.SelectCommand.CommandTimeout = m_sdaAcc.SelectCommand.CommandTimeout * 2;
            int nRec = -1;
            try
            {
                nRec = m_sdaAcc.Fill(m_dtAcc);
            }
            catch (SqlException se)
            {
                if (se.Message.Contains("Timeout expired."))
                {
                    m_sdaAcc.SelectCommand.CommandTimeout = m_sdaAcc.SelectCommand.CommandTimeout * 2;
                    try
                    {
                        nRec = m_sdaAcc.Fill(m_dtAcc);
                    }
                    catch (SqlException se2)
                    {
                        MessageBox.Show(se2.Message, propAppName);
                        return;
                    }
                }
            }
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";
            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Accounts not entered into Care360", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";
        }

        private void bUNDLINGNEEDEDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                "select chrg.account,acc.pat_name ,trans_date,cdm, count(cdm) as [CDM Count], sum(qty) as [Qty Sum] " +
                ",'BUNDLE' as [status] " +
                "from chrg " +
                "inner join acc on acc.account = chrg.account " +
                "where chrg.account in (select account from acc where fin_code = 'd' " +
                //"and trans_date > '03/01/2013 00:00' "+
                "and status = 'new') " +
                "and (not cdm between '5520000' and '552ZZZZ') " +
                "and credited = 0 " +
                "and coalesce(invoice,'') = '' " +
                "group by chrg.account, acc.pat_name, trans_date, cdm " +
                "having sum(qty) > 1 and count(cdm) > 1 "

                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";

            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Accounts needing bundling", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";
        }

        private void tsmiClientDiscount_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                "select acc.account, acc.pat_name, chrg.cdm, trans_date,  quest_code,quest_description as [description] " +
                ", cli_dis.price as [Price in Client Discount Table] " +
                ",'NEED PRICE' as [status] " +
                "from dict_quest_reference_lab_tests dt " +
                "left outer join cli_dis on cli_dis.start_cdm = dt.cdm " +
                "inner join chrg on chrg.cdm = dt.cdm " +
                "inner join acc on acc.account = chrg.account " +
                "where cli_dis.start_cdm is null and dt.deleted = 0 " +
                "and acc.status = 'QUEST'"

                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";
            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Accounts needing entered into client discount table.", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";

            /*; with cteChrg
as
(
	select chrg.account, chrg.cdm , descript as [description]
	from chrg	
	inner join acc on acc.account = chrg.account
	inner join cdm on cdm.cdm = chrg.cdm
	where acc.fin_code = 'Y' and trans_date > '10/01/2012 00:00' and acc.status = 'QUEST'
	and chrg.cdm <> 'CBILL'
)

select account, cteChrg.cdm, [description]
from cteChrg
left outer join cli_dis on cli_dis.start_cdm = cteChrg.cdm
where cli_dis.start_cdm is null 
order by cteChrg.cdm
             */

        }

        private void tsmiImportBillingErrors_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelect = new SqlCommand(
                "; with cte360Error " +
                "as " +
                "( " +
                "select account, req_no,patient as [pat_name], ssn , collection_date as [trans_date], quest_desc as [description], " +
                " status " +
                "from data_quest_billing	" +
                "where status like 'ERROR:%'" +
                ") " +
                "select account, req_no, pat_name, trans_date, [description], [status] " +
                "from cte360Error "

                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelect;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            if (nRec > 0)
            {
                // handle any name suffixs
                string strPatName = m_dtAcc.Rows[0]["pat_name"].ToString();
                strPatName = strPatName.Insert(strPatName.LastIndexOf(","), "%");

                string strReqNo = m_dtAcc.Rows[0]["req_no"].ToString();
                cmdSelect = new SqlCommand(
                    string.Format("select account, trans_date, pat_name from acc " +
                    " where pat_name like '{0}%' and trans_date = '{1}' and cl_mnem = 'QUESTR'",
                    strPatName, m_dtAcc.Rows[0]["trans_date"])
                    , m_sqlConnection);
                m_sdaAcc.SelectCommand = cmdSelect;
                m_dtAcc.Rows.Clear();
                nRec = m_sdaAcc.Fill(m_dtAcc);

                //  m_dtAcc.Columns.Add("req_no", typeof(string));
                if (nRec > 0)
                {
                    m_dtAcc.Rows[0]["req_no"] = strReqNo;
                }
            }
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";

            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Accounts with Import Billing Errors.", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelect = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelect;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";

        }

        private void tsmiCancelNotify_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                "; with cte360Cancel " +
                "as " +
                "( " +
                "select account, patient as [pat_name], ssn , collection_date as [trans_date], quest_desc as [description], " +
                " status " +
                "from data_quest_billing	" +
                "where comment = 'Canceled in Care360' and deleted = 0" +
                ") " +
                "select account, pat_name, trans_date, [description], [status] " +
                "from cte360Cancel "

                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";
            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Canceled Accounts needing Notification.", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";

        }

        private void tRANSDATEERRORSToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                ";with cteTransDateErrors " +
                "as " +
                "( select " +
                "	 dqb.account, dqb.patient as [pat_name], dqb.ssn, dqb.collection_date as [collection date] " +
                "	, dqb.quest_desc as [description], dqb.status, acc.trans_date " +
                "	from data_quest_billing dqb " +
                "	inner join acc on acc.account = dqb.account " +
                "	where dqb.deleted = 0 and dqb.collection_date <> acc.trans_date " +
                " and acc.trans_date >= '05/01/2013 00:00:00' " +
                ") " +
                "select * from cteTransDateErrors " +
                "order by trans_date"
                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";
            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Accounts with Trans_date errors.", AccountCount(reader));
            //m_dtAcc.Rows.Count);

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";

        }

        private int AccountCount(DataTableReader reader)
        {
            Dictionary<string, int> dicCount = new Dictionary<string, int>();
            //using (DataTableReader reader = m_dtAcc.CreateDataReader())
            using (reader)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!dicCount.Keys.Contains(reader["Account"].ToString()))
                        {
                            dicCount.Add(reader["Account"].ToString(), 1);
                        }
                    }
                }


            }
            return dicCount.Count();
        }

        private void tsbDirections_Click(object sender, EventArgs e)
        {
            string strFile = @"C:\Program Files\Medical Center Laboratory\MCL Billing\QuestCorrectionProcess.pdf";
            //@"C:\source\MCLVS2008\ViewerQuestCorrection\ViewerQuestCorrection\ViewerQuestCorrection\bin\Debug\Quest Correction Process.pdf";

            Process prop = new Process();
            prop.StartInfo.FileName = strFile;
            prop.StartInfo.Arguments = @"C:\\Program Files\\Adobe\\Acrobat 5.0\\Reader\\AcroRd32.exe";// 1;
            try
            {
                prop.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, propAppName);
            }
        }

        private void cBILLFIXESREQUIREDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDataGridViews();
            m_sdaAcc = new SqlDataAdapter();
            m_dtAcc = new DataTable();
            SqlCommand cmdSelct = new SqlCommand(
                string.Format("execute usp_quest_cbill @startDate = '{0}', @endDate = '{1}'", m_dpFrom
                    , m_dpThru)
                , m_sqlConnection);

            m_sdaAcc.SelectCommand = cmdSelct;
            int nRec = m_sdaAcc.Fill(m_dtAcc);
            tscbAcc.ComboBox.DataSource = m_dtAcc;
            tscbAcc.ComboBox.DisplayMember = "ACCOUNT";
            tscbName.ComboBox.DataSource = m_dtAcc;
            tscbName.ComboBox.DisplayMember = "pat_name";
            DataTableReader reader = m_dtAcc.CreateDataReader();

            tsslRecords.Text = string.Format(" {0} Accounts not entered into Care360.", AccountCount(reader));

            DataTable dtQuestBilling = new DataTable();
            cmdSelct = new SqlCommand(
                "select req_no, account from data_quest_billing order by req_no", m_sqlConnection);
            m_sdaAcc.SelectCommand = cmdSelct;
            nRec = m_sdaAcc.Fill(dtQuestBilling);
            tscbReqNo.ComboBox.DataSource = dtQuestBilling;
            tscbReqNo.ComboBox.DisplayMember = "req_no";
            tscbDQBAccount.ComboBox.DataSource = dtQuestBilling;
            tscbDQBAccount.ComboBox.DisplayMember = "account";


        }

        private void tsmiInvoice_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string strInvoice = tscbInvoices.Text;
                Form f = new Form();
                f.Text = string.Format("Invoice {0}", strInvoice);
                ToolStrip tsfMain = new ToolStrip();

                tsfMain.Dock = DockStyle.Bottom;
                //tsfMain.GripMargin.All = 2;
                tsfMain.TabIndex = 0;
                tsfMain.GripStyle = ToolStripGripStyle.Visible;

                tsfMain.Location = new System.Drawing.Point(0, 0);

                ToolStripButton tsb = new ToolStripButton();
                tsb.Text = "CLOSE";
                tsb.Click += new EventHandler(this.tsb_Click);
                tsfMain.Items.Add(tsb);

                tsfMain.Items.Add(new ToolStripSeparator());

                ToolStripButton tsbExcel = new ToolStripButton();
                tsbExcel.Text = "EXCEL";
                tsbExcel.Click += new EventHandler(this.tsbExcel_Click);
                tsfMain.Items.Add(tsbExcel);

                f.Controls.Add(tsfMain);

                DataGridView dgv = new DataGridView();
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                dgv.TabIndex = 1;
                dgv.Dock = DockStyle.Fill;

                f.Controls.Add(dgv);
                ClearDataGridViews();
                m_sdaAcc = new SqlDataAdapter();
                DataTable dtInvoice = new DataTable();
                SqlCommand cmdSelct = new SqlCommand(
                    string.Format("execute usp_quest_cbill_by_invoice @invoice = '{0}'", strInvoice)
                    , m_sqlConnection);

                m_sdaAcc.SelectCommand = cmdSelct;
                int nRec = m_sdaAcc.Fill(dtInvoice);
                dgv.DataSource = dtInvoice;
                dgv.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgvAccount_RowHeaderMouseDoubleClick);
                f.Show();


            }
        }

        private void tsbExcel_Click(object sender, EventArgs e)
        {
            string strFile = ((Control)
                                        ((ToolStrip)
                                            ((ToolStripButton)sender)
                                            .GetCurrentParent())
                                         .Parent).Text.Replace(" ", "_");
            DataGridView dgv = ((DataGridView)
                                    ((Control)
                                        ((ToolStrip)
                                            ((ToolStripButton)sender)
                                            .GetCurrentParent())
                                         .Parent)
                                     .Controls[1]);
            DataTable dt = new DataTable("INVOICE", @"http:/www.w3.org/2001/XMLSchema;http:/www.w3.org/2001/XMLSchema-datatypes"); //
            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                if (dc.ValueType == typeof(DateTime))
                {

                    dt.Columns.Add(dc.Name, dc.ValueType);
                    dt.Columns[dc.Index].DateTimeMode = DataSetDateTime.Unspecified;


                }
                else
                {
                    dt.Columns.Add(dc.Name, dc.ValueType);
                }

            }
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                // m_drCur = (dr.DataBoundItem as DataRowView).Row; either this one or the next one work.
                //m_drCur = ((DataRow)((DataRowView)dr.DataBoundItem).Row);
                if (dgvr.IsNewRow)
                {
                    continue;
                }
                dgvr.Cells["collection_date"].Value =
                    dgvr.Cells["collection_date"].Value.ToString().Split(new string[] { " " }, StringSplitOptions.None)[0];

                DataRow dr = ((DataRow)((DataRowView)dgvr.DataBoundItem).Row);
                dt.ImportRow(dr);
            }

            dt.WriteXml(string.Format(@"C:\temp\{0}.xls", strFile));
            dt.WriteXml(string.Format(@"C:\temp\{0}.xml", strFile));
            dt.WriteXml(string.Format(@"C:\temp\{0}.xlxs", strFile));




        }
        private void tsb_Click(object sender, EventArgs e)
        {
            this.Close();
            /*http://aspalliance.com/762_Binding_the_ReportViewer_Control_to_Multiple_Generic_Collections
                Control ctr = ((ToolStrip)((ToolStripButton)sender).GetCurrentParent()).Parent;
                DataTable dtFromGrid = new DataTable();
                dtFromGrid = ((DataTable)((DataGridView)ctr.Controls[1]).DataSource);

                BindingSource bs = new BindingSource();
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "fly";
                rds.Value = bs;
                ReportViewer rv = new ReportViewer();
                //Report rpt = new Report();
                //rpt.DisplayName = "Report1.rdlc";
                rv.LocalReport.ReportEmbeddedResource = "this.Report1.rdlc";
                bs.DataSource = dtFromGrid;
                rv.Dock = DockStyle.Fill;
              //  ((DataGridView)ctr.Controls[1]).Hide();
                rv.Show();

                */



            //Microsoft.Reporting.WinForms.ReportDataSource rds = new ReportDataSource();
            //rds.Name = "fly";
            //rds.Value = new BindingSource(dsFromGrid, "account");
            //ReportViewer rv = new ReportViewer();
            //rv.LocalReport.ReportEmbeddedResource = new Report();

            ////rpt


            ////ctr.Controls.Add(rv);
            //rv.Show();
            //ctr.Refresh();


        }

        //private void tsmiInvoice_Enter(object sender, EventArgs e)
        //{

        //    ((ToolStripTextBox)sender).Clear();
        //}
        /*
                private void tsmiLoad360FixInfo_Click(object sender, EventArgs e)
                {
                    return;
                    string strAcc = null;
                    Form f = new Form();
                    f.Text = "ENTER SELECTION";

                    Button bYes = new Button();
                    bYes.Text = "YES";
                    bYes.Location = new Point((f.Size.Width / 2) - (bYes.Width / 2), (f.Size.Height - 30) / 2);
                    bYes.DialogResult = DialogResult.Yes;
                    bYes.TabIndex = 1;

                    Button bNo = new Button();
                    bNo.Location = new Point((f.Size.Width / 2) - (bNo.Width / 2), (f.Size.Height + 30) / 2);
                    bNo.Text = "NO";
                    bNo.DialogResult = DialogResult.No;
                    bNo.TabIndex = 2;

                    TextBox t = new TextBox();
                    t.Dock = DockStyle.Fill;
                    t.TabIndex = 0;

                    f.Controls.Add(t);
                    f.Controls.Add(bYes);
                    f.Controls.Add(bNo);
                    t.Select();
                    DataGridView dgvRecords = new DataGridView();


                    if (f.ShowDialog() == DialogResult.Yes)
                    {
                        strAcc = t.Text;
                    }
                    if (string.IsNullOrEmpty(strAcc))
                    {
                        return;
                    }
                    f.Controls.Add(dgvRecords);
                    DataSet dsQUE = new DataSet();
                    using (SqlConnection conn =
                    new SqlConnection(
                        string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                        m_strServer, m_strDatabase)))
                    {

                        SqlDataAdapter sda = new SqlDataAdapter();
                        string strQuery =
                             string.Format("with cteAcc as ( " +
                             "select acc.account, datediff(year,dob_yyyy,trans_date) as [Age] " +
                             "from acc " +
                             "inner join pat on pat.account = acc.account " +
                             "where fin_code in ('D','Y') and trans_date between '{0} 00:00' and '{1} 23:59:59' " +
                             ") " +
                             ", cteChrg as ( " +
                             "select chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age] " +
                             ", convert(datetime,convert(varchar(10),service_date,101)) as [DOS] " +
                             "from chrg " +
                             "inner join cteAcc on cteAcc.account = chrg.account " +
                             "inner join amt on amt.chrg_num = chrg.chrg_num " +
                             "where credited = 0 " +
                             "and cdm <> 'CBILL' " +
                             ") " +
                             "select distinct cteChrg.account, cteChrg.chrg_num, cteChrg.cdm, cteChrg.cpt4 " +
                             ", cteChrg.qty, cteChrg.dos " +
                             ", [Age] " +
                             ",case when dd.cpt is null " +
                             "then 'GAP' " +
                             " else case when (age > 11 and age_appropriate = 1) " +
                             " then 'GAP' else " +
                             "'EXCLUSION' " +
                             " end " +
                             "end as [Bill Type] " +
                             ", coalesce(dt.quest_code,dt2.quest_code) as [Quest Code] " +
                             ", coalesce(dt.quest_description,dt2.quest_description) as [Quest Description] " +
                             "from cteChrg " +
                             "left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0 " +
                             "left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                             "left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                             "where account = '{2}' " +
                             "order by account, cdm, cteChrg.cpt4 ",
                             m_dtFrom.ToString("d"), m_dtThru.ToString("d"), strAcc);
                        sda.SelectCommand =
                        new SqlCommand(strQuery, conn);

                        DateTime dtWait = DateTime.Now;
                        try
                        {
                            int nRec = sda.Fill(dsQUE);
                        }
                        catch (SqlException se)
                        {
                            int n = 0;
                            while (DateTime.Now < dtWait.AddSeconds(10))
                            {
                                n += n++; // just hum a bit
                            }

                            MessageBox.Show(se.Message, "SQL EXCEPTION DURING LOAD");
                        }


                        dgvRecords.DataSource = dsQUE.Tables[0];
                    }

                    dgvRecords.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    //LoadStatusBar();
                    foreach (DataGridViewRow dr in dgvRecords.Rows)
                    {
                        if (dr.Cells["cdm"].Value.ToString().StartsWith("552"))
                        {
                            if (dr.Cells["cdm"].Value.ToString().Equals("5527418") ||
                                dr.Cells["cdm"].Value.ToString().Equals("5527419"))
                            {
                                continue;
                            }
                            dr.ErrorText = "CHANTILLY Charge";
                        }
                        Application.DoEvents();
                    }
                    MessageBox.Show("Load Completed");
                    dgvRecords.Invalidate();
                    this.Invalidate();
                }
        */
        //private void tsmiCreate360Info_Click(object sender, EventArgs e)
        //{
        //    DataGridViewSelectedRowCollection drs = dgvChrg.SelectedRows;
        //    // create the HTML

        //    DataSet m_ds360 = new DataSet();
        //    m_ds360.Tables.Add("ACC");
        //    m_ds360.Tables.Add("PAT");
        //    m_ds360.Tables.Add("INS");
        //    m_ds360.Tables.Add("CHRG_CLINICAL");
        //    m_ds360.Tables.Add("CHRG_CYTO_HISTO"); // wdk 20121218 modification


        //    string strAccOld = null;
        //    string sbChrgNums = "(";
        //    foreach (DataGridViewRow drAcc in drs)
        //    {
        //        if (drs[0].Cells["account"].Value.ToString().ToUpper().StartsWith("C") ||
        //            drs[0].Cells["account"].Value.ToString().ToUpper().StartsWith("QR"))
        //    {
        //        continue;
        //    }

        //    string strAcc = drAcc.Cells["account"].Value.ToString();
        //    string strSelectAcc = string.Format("account = '{0}'", strAcc);
        //    DataRow[] drAccs = dsQUE.Tables[0].Select(strSelectAcc);

        //    foreach (DataRow drChrg in drAccs)
        //    {
        //        sbChrgNums += string.Format("'{0}',", drChrg["chrg_num"].ToString());
        //    }
        //    sbChrgNums = sbChrgNums.Remove(sbChrgNums.LastIndexOf(','), 1);
        //    sbChrgNums += ")";

        //    if (strAcc == strAccOld)
        //    {
        //        continue;
        //    }
        //    strAccOld = strAcc;

        //    m_ds360.Tables["CHRG_CLINICAL"].Rows.Clear();
        //    m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Clear();
        //    m_ds360.Tables["INS"].Rows.Clear();
        //    m_ds360.Tables["PAT"].Rows.Clear();
        //    m_ds360.Tables["ACC"].Rows.Clear();
        //        /*
        //            // using the current row fill the tables
        //            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
        //            {
        //                SqlDataAdapter sda = new SqlDataAdapter();
        //                DataTable dtAcc = new DataTable();
        //                #region ACCOUNT
        //                SqlCommand cmdAccSelect = new SqlCommand(
        //                    string.Format("select  " +
        //                    "	account, " +
        //                    " substring(pat_name, 0, charindex(',',pat_name,0)) as [lastname], " +
        //                    "	substring(pat_name, " +
        //                    "	charindex(',',pat_name,0)+1 " +
        //                    " , case when charindex(' ',pat_name,charindex(',',pat_name,0)-1) = 0 " +
        //                    "then len(pat_name) - charindex(',',pat_name,0) " +
        //                    "else  " +
        //                    " charindex(' ',pat_name,charindex(',',pat_name,0)+1) - " +
        //                    " charindex(',',pat_name,0) " +
        //                    "	end) as [firstname] " +
        //                    "	,	substring(pat_name,  " +
        //                    "	charindex(' ',pat_name, charindex(',',pat_name,0)+1)+1 " +
        //                    " 	, case when charindex(' ',pat_name,charindex(',',pat_name,0)+1) = 0 " +
        //                    " then 0 " +
        //                    " else  charindex(' ',pat_name,charindex(',',pat_name,0)+1)			" +
        //                    " end)	as [midname]" +
        //                    ", cl_mnem " + //, fin_code
        //                    "	, convert(varchar(10),trans_date,101) as [trans_date] " +
        //                    " ,  stuff(stuff(ssn,4,0,'-'),7,0,'-') as [ssn] " +
        //                    " from acc " +
        //                    " where acc.account = '{0}' ", strAcc)
        //                    , conn);
        //                sda.SelectCommand = cmdAccSelect;
        //                int nAccRec = sda.Fill(m_ds360.Tables["ACC"]);

        //                m_ds360.Tables["ACC"].PrimaryKey =
        //                    new DataColumn[] { m_ds360.Tables["ACC"].Columns["ACCOUNT"] };

        //                #endregion ACCOUNT
        //                #region PAT
        //                SqlCommand cmdPatSelect = new SqlCommand(
        //                   string.Format("select " +
        //                   " pat.account " +
        //                   " ,trans_date " +
        //                   ", pat_addr1, pat_addr2, city_st_zip " +
        //                   ", convert(varchar(10),dob_yyyy,101) as [DOB], sex, relation " +
        //                   ", guarantor, guar_addr, g_city_st" +
        //                   ", guar_phone, pat_marital  " +
        //                   ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
        //                   ", phy.last_name +','+ phy.first_name +' '+ isnull(phy.mid_init,'') +' ('+phy_id +')' as [phy_id] " +
        //                   "  from pat " +
        //                   "inner join acc on acc.account = pat.account " +
        //                   "left outer join phy on phy.tnh_num = pat.phy_id " +
        //                   " where pat.account = '{0}'", strAcc)
        //                   , conn);

        //                sda.SelectCommand = cmdPatSelect;
        //                int nPatRec = sda.Fill(m_ds360.Tables["PAT"]);

        //                //if (!m_ds360.Relations.Contains("AccPat"))
        //                //{
        //                //    DataRelation drAccPat = m_ds360.Relations.Add(
        //                //        "AccPat", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
        //                //        m_ds360.Tables["PAT"].Columns["ACCOUNT"]);
        //                //}

        //                #endregion PAT
        //                #region INS

        //                SqlCommand cmdInsSelect = new SqlCommand(
        //                   string.Format("select " +
        //                   "	ins.account, ins_a_b_c, holder_nme, holder_dob, holder_sex, holder_addr, holder_city_st_zip, plan_nme, plan_addr1, plan_addr2, " +
        //                   "    p_city_st, policy_num, cert_ssn, grp_nme, grp_num, employer, e_city_st " +
        //                   "	, ins_code, relation " +
        //                   " from ins " +
        //                   " where account = '{0}' and deleted = 0 ", strAcc)
        //                   , conn);

        //                sda.SelectCommand = cmdInsSelect;
        //                int nInsRec = sda.Fill(m_ds360.Tables["INS"]);

        //                //if (!m_ds360.Relations.Contains("AccIns"))
        //                //{
        //                //    DataRelation drAccPat = m_ds360.Relations.Add(
        //                //        "AccIns", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
        //                //        m_ds360.Tables["INS"].Columns["ACCOUNT"]);
        //                //}

        //                #endregion INS


        //                #region account charges
        //                string strSelectChrg =
        //                    string.Format("select account, qty, chrg.cdm, " +
        //                    " coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
        //                    "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
        //                    "from chrg " +
        //                    "left outer join dict_quest_reference_lab_tests dt " +
        //                    "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
        //                    "left outer join dict_quest_reference_lab_tests dt2 " +
        //                    "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
        //                    "where account = '{0}' " +
        //                    "and (not chrg.cdm between '5920000' and '594ZZZZ') " +
        //                    "and chrg_num in {1} "
        //                    , strAcc, sbChrgNums);

        //                sda.SelectCommand =

        //                    new SqlCommand(strSelectChrg, conn);

        //                DateTime dtWait = DateTime.Now;
        //                try
        //                {
        //                    int nChrgRec = sda.Fill(m_ds360.Tables["CHRG_CLINICAL"]);
        //                }
        //                catch (SqlException se)
        //                {
        //                    m_Err.m_Logfile.WriteLogFile(se.Message);
        //                    int n = 0;
        //                    while (DateTime.Now < dtWait.AddSeconds(10))
        //                    {
        //                        n += n++; // just hum a bit
        //                    }
        //                    //    if (int.Parse(((ToolStripButton)sender).Tag.ToString()) == 2)
        //                    //{
        //                    //    tsbLoad_Click(sender, e);
        //                    //}
        //                    MessageBox.Show(se.Message, "SQL EXCEPTION DURING LOAD");
        //                }
        //                //m_ds360.Tables["CHRG_CLINICAL"].PrimaryKey = null;
        //                //if (strAcc.StartsWith("QR"))
        //                //{
        //                //    // there are no quest codes for reference lab testing.
        //                //    m_ds360.Tables["CHRG_CLINICAL"].PrimaryKey =
        //                //    new DataColumn[] { m_ds360.Tables["CHRG_CLINICAL"].Columns["ACCOUNT"],
        //                //                         m_ds360.Tables["CHRG_CLINICAL"].Columns["CHRG_NUM"],
        //                //                            m_ds360.Tables["CHRG_CLINICAL"].Columns["CDM"]//,
        //                //                                //m_ds360.Tables["CHRG_CLINICAL"].Columns["CPT4"]
        //                //                                    };
        //                //}
        //                //else
        //                //{
        //                //    m_ds360.Tables["CHRG_CLINICAL"].PrimaryKey =
        //                //      new DataColumn[] { m_ds360.Tables["CHRG_CLINICAL"].Columns["ACCOUNT"],
        //                //                         m_ds360.Tables["CHRG_CLINICAL"].Columns["CHRG_NUM"],
        //                //                            m_ds360.Tables["CHRG_CLINICAL"].Columns["CDM"],
        //                //                                //m_ds360.Tables["CHRG_CLINICAL"].Columns["CPT4"],
        //                //                                    m_ds360.Tables["CHRG_CLINICAL"].Columns["QUEST CODE"]};
        //                //}

        //                //if (!m_ds360.Relations.Contains("AccChrg"))
        //                //{
        //                //    DataRelation drAccPat = m_ds360.Relations.Add(
        //                //        "AccChrg", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
        //                //        m_ds360.Tables["CHRG_CLINICAL"].Columns["ACCOUNT"]);
        //                //}

        //                #endregion account charges

        //                #region charges cyto_hysto
        //                sda.SelectCommand =
        //                    new SqlCommand(
        //                    string.Format("select distinct account, qty, chrg.cdm, " +
        //                    "coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
        //                    "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
        //                    "from chrg " +
        //                    "left outer join dict_quest_reference_lab_tests dt " +
        //                    "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
        //                    "left outer join dict_quest_reference_lab_tests dt2 " +
        //                    "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
        //                    "where account = '{0}' " +
        //                    "and (chrg.cdm between '5920000' and '594ZZZZ') " +
        //                    "and chrg_num in {1} ", strAcc, sbChrgNums),
        //                conn);

        //                dtWait = DateTime.Now;
        //                try
        //                {
        //                    int nChrgCytoHystoRec = sda.Fill(m_ds360.Tables["CHRG_CYTO_HISTO"]);
        //                }
        //                catch (SqlException se)
        //                {
        //                    m_Err.m_Logfile.WriteLogFile(se.Message);
        //                    int n = 0;
        //                    while (DateTime.Now < dtWait.AddSeconds(10))
        //                    {
        //                        n += n++; // just hum a bit
        //                    }

        //                    MessageBox.Show(se.Message, "SQL EXCEPTION DURING LOAD");
        //                }
        //                m_ds360.Tables["CHRG_CYTO_HISTO"].PrimaryKey = null;



        //                #endregion charges cyto_hysto


        //            }
        //            if (m_ds360.Tables["CHRG_CLINICAL"].Rows.Count > 0)
        //            {
        //                CreateHTML(false);
        //            }
        //            if (m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Count > 0)
        //            {
        //                CreateHTML(true);
        //            }
        //        */
        //        }
        //        MessageBox.Show("HTML Created");




        //        /////////////

        //}

        #region createhtml
        private void CreateHTML(bool bCytoHisto)
        {
            m_dicPatient = new Dictionary<string, string>();
            string strPatID = "";

            foreach (DataRow dr in m_ds360.Tables["ACC"].Rows)
            {

                try
                {
                    strPatID = string.Format("{0}{1}{2}{3}{4}{5}{8}|{6}|{7}",
                                            dr["LASTNAME"].ToString().Trim(), //0
                                            dr["FIRSTNAME"].ToString().Trim(), //1
                                            dr["MIDNAME"].ToString().Trim(), //2
                                            string.Format("{0,8:yyyyMMdd}", //3
                                                DateTime.Parse(m_ds360.Tables["PAT"].Rows[0]["DOB"].ToString())),
                                            m_ds360.Tables["PAT"].Rows[0]["SEX"].ToString().Trim(), //4
                                            dr["SSN"].ToString().Trim(), //5
                                            dr["ACCOUNT"], //6
                                            dr["trans_date"], //7
                                            string.Format("[CH{0}]", bCytoHisto) //8
                                            );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("{0}\r\n\r{1}", ex.GetType().ToString(), ex.Message), dr["ACCOUNT"].ToString());
                    return;
                }

                m_wbPrint = new WebBrowser();
                m_wbPrint.AllowNavigation = false;
                m_wbPrint.Navigate(new Uri("about:blank"));

                m_htmDoc = m_wbPrint.Document;
                m_htmDoc.OpenNew(false);
                m_htmDoc.Write("<HTML>");
                m_htmDoc.Write("<BODY MARGIN-LEFT: .5IN MARGIN-RIGHT: .5IN  MARGIN-TOP: = .25 MARGIN-BOTTOM = .5>");
                StringBuilder sb = new StringBuilder();
                m_htmDoc.Title = "360 Billing";
                sb.AppendLine("<IMG SRC= \"file://C:/Program Files/Medical Center Laboratory/MCL Billing/mcllogo.bmp\">");

                sb.AppendLine("<BR>");
                //if (m_strBillType.StartsWith("Q"))
                //if (dr["ACCOUNT"].ToString().StartsWith("QR") || m_strBillType == "QR")
                //{
                //    sb.AppendFormat("<HR><P ALIGN=CENTER><B>BILLING TYPE: {0}</B></P><HR>", "CHANTILLY");
                //}
                //else
                //{
                sb.AppendFormat("<HR><P ALIGN=CENTER><B>BILLING TYPE: {0}</B></P><HR>", "ATLANTA");
                //}
                #region TablePatient
                // table with a table
                sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #0000FF><TD COLSPAN = 3><B>PATIENT DATA</B></TD></TR>");

                sb.AppendLine("<TR>");

                // PATIENT TABLE
                sb.AppendLine("<TD WIDTH =300>");
                sb.AppendLine("<TABLE WIDTH = 300 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>PATIENT</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 100><B>Last Name</B></td>");

                sb.AppendFormat("<td width = 200>{0}</td>", dr["LASTNAME"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>First Name</B></td>");

                sb.AppendFormat("<td>{0}</td>", dr["FIRSTNAME"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Middle Init</B></td>");

                sb.AppendFormat("<td>{0}</td>", dr["MIDNAME"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>BIRTH DATE</B></TD>");

                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["DOB"]);//drPat[0]["DOB"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>GENDER</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["sex"]);//drPat[0]["sex"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>SSN</B></TD>");
                sb.AppendFormat("<td>{0}</td>", dr["ssn"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                //// end of PATIENT table
                //// add table here
                sb.AppendLine("<TD WIDTH =300>");
                sb.AppendLine("<TABLE WIDTH = 300 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>ADMINISTRATIVE DETAILS</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD WIDTH = 100><B>Primary Provider</B></TD>");
                sb.AppendFormat("<td WIDTH = 200>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["phy_id"]);//drPat[0]["phy_id"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>LAB REF ID</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["account"]);//drPat[0]["account"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Health ID</B></TD>");
                /* wdk 20121204 removed and leave blank
                DataRow dr360 = 
                    m_ds360.Tables["360"].Rows.Find(new object[] {
                        strPatID.Substring(0,strPatID.IndexOf('|')), 
                        drPat[0]["account"],
                        m_strBillType });
                sb.AppendFormat("<td>{0}</td>", dr360 == null ? "":dr360["patid"]);
                 */
                sb.AppendFormat("<td>{0}</td>", "");
                sb.AppendLine("</TR>");


                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>DOS</B></TD>");
                sb.AppendFormat("<TD>{0}</TD>", //DateTime.Parse(drPat[0]["trans_date"].ToString()).ToShortDateString());
                    DateTime.Parse(m_ds360.Tables["PAT"].Rows[0]["trans_date"].ToString()).ToShortDateString());//
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>RELATION</B></TD>");
                sb.AppendFormat("<TD>{0}</TD>", ConvertRelation(m_ds360.Tables["PAT"].Rows[0]["relation"]));//drPat[0]["relation"]));

                // line fillers
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD></TD>");
                sb.AppendLine("</TR>");

                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                // end of new table

                //// add third table
                sb.AppendLine("<TD WIDTH =350>");
                sb.AppendLine("<TABLE WIDTH = 350 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>CONTACT INFO</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 20>");
                sb.AppendLine("<TD WIDTH = 90><B>Address Line 1</B></TD>");
                //sb.AppendLine("<TD WIDTH = 200>one</TD>");
                sb.AppendFormat("<td width = 260>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr1"]);//drPat[0]["pat_addr1"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Address Line 2</B></TD>");
                //sb.AppendLine("<TD>two</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr2"]);//drPat[0]["pat_addr2"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                string[] strCSZ = ParseCSZ(m_ds360.Tables["PAT"].Rows[0]["city_st_zip"]);//drPat[0]["city_st_zip"]);
                sb.AppendLine("<TD><B>City</B></TD>");
                //sb.AppendLine("<TD>c</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[0]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>STATE/Province/Region</B></TD>");
                //sb.AppendLine("<TD>s</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[1]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Zip/Postal Code</B></TD>");
                //sb.AppendLine("<TD>z</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[2]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Home Phone</B></TD>");
                //sb.AppendLine("<TD>h</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["guar_phone"]);//drPat[0]["guar_phone"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of third table

                sb.AppendLine("</TR>");
                #endregion tablePat

                sb.AppendLine("<BR>");
                sb.AppendLine("<hr/>");

                #region TableGuarantor
                /* Not used by Care360 Atlanta
               
                // table with a table
                
                sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #00FF00><TD COLSPAN = 3><B>GUARANTOR DATA</B></TD></TR>");

                sb.AppendLine("<TR>");

                 // Guarantor TABLE
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>GUARANTOR</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 200><B>Last Name</B></td>");
                string[] strGuar = ParseName(m_ds360.Tables["PAT"].Rows[0]["guarantor"]);//drPat[0]["guarantor"]);
                sb.AppendFormat("<td width = 275>{0}</td>", strGuar[0]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>First Name</B></td>");
                sb.AppendFormat("<td>{0}</td>", strGuar[1]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Middle Init</B></td>");
                sb.AppendFormat("<td>{0}</td>", strGuar[2]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>BIRTH DATE</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["DOB"]);//drPat[0]["DOB"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>GENDER</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["sex"]);//drPat[0]["sex"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>SSN</B></TD>");
                sb.AppendFormat("<td>{0}</td>", dr["ssn"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                //// end of Guarantor table
                
                //// add guarantor contact info table
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>CONTACT INFO</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD WIDTH = 125><B>Address Line 1</B></TD>");
                sb.AppendFormat("<td width = 350>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr1"]);//drPat[0]["pat_addr1"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Address Line 2</B></TD>");
                //sb.AppendLine("<TD>two</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr2"]);//drPat[0]["pat_addr2"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                strCSZ = ParseCSZ(m_ds360.Tables["PAT"].Rows[0]["city_st_zip"]);//drPat[0]["city_st_zip"]);
                sb.AppendLine("<TD><B>City</B></TD>");
                //sb.AppendLine("<TD>c</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[0]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>STATE/Province/Region</B></TD>");
                //sb.AppendLine("<TD>s</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[1]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Zip/Postal Code</B></TD>");
                //sb.AppendLine("<TD>z</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[2]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Home Phone</B></TD>");
                //sb.AppendLine("<TD>h</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["guar_phone"]);//drPat[0]["guar_phone"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of third table
                 */
                #endregion TableGuarantor

                #region INS
                // add a new row with the insurance tables.


                #region TableIns
                sb.AppendLine("<TR>");

                sb.AppendLine("<hr/>");
                // table with a table                
                sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #FF0000><TD COLSPAN = 3><B>INSURANCE DATA</B></TD></TR>");

                sb.AppendLine("<TR>");

                // PRIMARY INSURANCE TABLE
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>PRIMARY INSURANCE</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 116><B>Insurance Name</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>", m_ds360.Tables["INS"].Rows[0]["plan_nme"]);//drIns[0]["plan_nme"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Insurance ID</B></td>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["INS"].Rows[0]["policy_num"]);//drIns[0]["policy_num"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Group Number</B></td>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["INS"].Rows[0]["grp_num"]);//drIns[0]["grp_num"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Insurance Number</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["INS"].Rows[0]["policy_num"]);//drIns[0]["policy_num"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of PRIMARY INSURANCE table



                //// add table here
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>INSURANCE ADDRESS</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 116><B>Address Line 1</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>", "1 CAMERON HILL CIRCLE");
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Address Line 2</B></td>");
                sb.AppendFormat("<td>{0}</td>", "SUITE 0002");
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>CITY STATE and ZIP</B></td>");
                sb.AppendFormat("<td>{0}</td>", "CHATTANOOGA, TN 37402-0002");
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B></B></TD>");
                sb.AppendFormat("<td>{0}</td>", "");
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                // end of insurance address table

                #region additional insurance
                /*
                // Secondary Insurance Table 
                string strSecInsPlanName = "";
                string strSecInsPolicyNum = "";
                string strSecInsGroupNum = "";
                string strSecInsInsNum = "";
                string strTerInsPlanName = "";
                string strTerInsPolicyNum = "";
                string strTerInsGroupNum = "";
                string strTerInsInsNum = "";
                try
                {
                    strSecInsPlanName = drIns[1]["plan_nme"].ToString();
                    strSecInsPolicyNum = drIns[1]["policy_num"].ToString();
                    strSecInsGroupNum = drIns[1]["grp_num"].ToString();
                    strSecInsInsNum = drIns[1]["policy_num"].ToString(); // change this when we know what it is

                    strTerInsPlanName = drIns[2]["plan_nme"].ToString();
                    strTerInsPolicyNum = drIns[2]["policy_num"].ToString();
                    strTerInsGroupNum = drIns[2]["grp_num"].ToString();
                    strTerInsInsNum = drIns[2]["policy_num"].ToString(); // change this when we know what it is
                }
                catch (IndexOutOfRangeException)
                {
                    // no second or third insurance so no problem
                }

                //// add table here
                sb.AppendLine("<TD WIDTH =316>");
                sb.AppendLine("<TABLE WIDTH = 300 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>SECONDARY INSURANCE</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 116><B>Insurance Name</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>",strSecInsPlanName);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Insurance ID</B></td>");
                sb.AppendFormat("<td>{0}</td>", strSecInsPolicyNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Group Number</B></td>");
                sb.AppendFormat("<td>{0}</td>", strSecInsGroupNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Insurance Number</B></TD>");
                sb.AppendFormat("<td>{0}</td>", strSecInsInsNum);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                // end of secondary insurance  table

                //// add tertiary insurance table
                sb.AppendLine("<TD WIDTH =320>");
                sb.AppendLine("<TABLE WIDTH = 320 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>TERTIARY INSURANCE</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 120><B>Insurance Name</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>", strTerInsPlanName);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Insurance ID</B></td>");
                sb.AppendFormat("<td>{0}</td>", strTerInsPolicyNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Group Number</B></td>");
                sb.AppendFormat("<td>{0}</td>", strTerInsGroupNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Insurance Number</B></TD>");
                sb.AppendFormat("<td>{0}</td>", strTerInsInsNum);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of third insurance table

                #endregion additional insurance 
                sb.AppendLine("</TR>");
                #endregion INS
                #endregion tableIns
                sb.AppendLine("</TR>");
                // end of new row with the insurance tables.

                sb.AppendLine("</TR>");
                #endregion tableGuarantor

                //..................                sb.AppendLine("<BR>");
                */
                #endregion INS

                #endregion Insurance

                #endregion INS

                if (!bCytoHisto)
                {
                    #region TableOrder
                    // table with a table
                    sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #0000FF><TD COLSPAN = 3><B>CLINICAL ORDER DATA</B></TD></TR>");

                    sb.AppendLine("<TR>");

                    // Diagnosis TABLE
                    sb.AppendLine("<TD WIDTH =475>");
                    sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>DIAGNOSIS</B></TD></TR>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td width = 100><B>Diagnosis 1</B></td>");
                    //sb.AppendLine("<td width = 200>ltest</td>");
                    sb.AppendFormat("<td width = 200>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_1"]);//drPat[0]["icd9_1"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 2</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_2"]);//drPat[0]["icd9_2"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 3</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_3"]);//drPat[0]["icd9_3"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 4</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_4"]);//drPat[0]["icd9_4"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 5</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_5"]);//drPat[0]["icd9_5"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 6</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_6"]);//drPat[0]["icd9_6"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 7</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_7"]);//drPat[0]["icd9_7"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 8</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_8"]);//drPat[0]["icd9_8"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 9</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_9"]);//drPat[0]["icd9_9"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("</TABLE>");

                    sb.AppendLine("</TD>");
                    //// end of Diagnosis table

                    //// add Order table here only for Atlanta 
                    //if (m_strBillType == "Q")
                    //{

                    sb.AppendLine("<TD WIDTH =475>");
                    sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 4><B>ORDERED TESTS</B></TD></TR>");
                    sb.AppendLine("<TD WIDTH = 100><B>QUEST CODE</B></TD>");
                    sb.AppendLine("<TD WIDTH = 100><B>QUEST DESCRIPTION</B></TD>");
                    sb.AppendLine("<TD WIDTH = 100><B>QTY</B></TD>");
                    sb.AppendLine("<TD WIDTH = 100><B>CDM</B></TD>");

                    StringBuilder strLine = new StringBuilder();
                    StringBuilder strLastLine = new StringBuilder();
                    // while the number of tests ordered
                    foreach (DataRow drOrder in m_ds360.Tables["CHRG_CLINICAL"].Rows) //drChrgClinical)
                    {
                        strLine = new StringBuilder();
                        strLine.AppendLine("<TR HEIGHT = 25>");
                        strLine.AppendFormat("<td><b>{0}</b></td>", drOrder["quest code"]);
                        strLine.AppendFormat("<td>{0}</td>", drOrder["Quest Description"]);
                        strLine.AppendFormat("<td>{0}</td>", drOrder["qty"]);
                        strLine.AppendFormat("<td>{0}</td>", drOrder["cdm"]);
                        strLine.AppendLine("</TR>");
                        if (strLine.ToString().Trim() != strLastLine.ToString().Trim())
                        {
                            sb.Append(strLine);
                            strLastLine = strLine;
                        }
                    }
                    sb.AppendLine("</TABLE>");
                    sb.AppendLine("</TD>");
                    // end of new table
                    //}
                    sb.AppendLine("</TR>");
                    sb.AppendLine("</TABLE>");
                    #endregion tablePat

                    sb.AppendLine("<BR>");
                }
                if (bCytoHisto)
                {
                    #region CytoHisto

                    // table with a table
                    sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #0000FF><TD COLSPAN = 3><B>CYTO/HISTO ORDER DATA</B></TD></TR>");

                    sb.AppendLine("<TR>");

                    // Diagnosis TABLE
                    sb.AppendLine("<TD WIDTH =475>");
                    sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>DIAGNOSIS</B></TD></TR>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td width = 100><B>Diagnosis 1</B></td>");
                    //sb.AppendLine("<td width = 200>ltest</td>");
                    sb.AppendFormat("<td width = 200>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_1"]);//drPat[0]["icd9_1"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 2</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_2"]);//drPat[0]["icd9_2"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 3</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_3"]);//drPat[0]["icd9_3"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 4</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_4"]);//drPat[0]["icd9_4"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 5</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_5"]);//drPat[0]["icd9_5"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 6</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_6"]);//drPat[0]["icd9_6"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 7</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_7"]);//drPat[0]["icd9_7"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 8</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_8"]);//drPat[0]["icd9_8"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 9</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_9"]);//drPat[0]["icd9_9"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("</TABLE>");


                    sb.AppendLine("</TD>");
                    //// end of Diagnosis table

                    //if (m_strBillType == "Q")
                    //{
                    //// add Order table here
                    sb.AppendLine("<TD WIDTH =475>");
                    sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 4><B>ORDERED TESTS</B></TD></TR>");
                    sb.AppendLine("<TD WIDTH = 100><B>QUEST CODE</B></TD>");
                    sb.AppendLine("<TD WIDTH = 100><B>QUEST DESCRIPTION</B></TD>");
                    sb.AppendLine("<TD WIDTH = 100><B>QTY</B></TD>");
                    sb.AppendLine("<TD WIDTH = 100><B>CDM</B></TD>");


                    foreach (DataRow drOrderCH in m_ds360.Tables["CHRG_CYTO_HISTO"].Rows)//drChrgCytoHisto)
                    {
                        sb.AppendLine("<TR HEIGHT = 25>");
                        sb.AppendFormat("<td><b>{0}</b></td>", drOrderCH["quest code"]);
                        sb.AppendFormat("<td>{0}</td>", drOrderCH["Quest Description"]);
                        sb.AppendFormat("<td>{0}</td>", drOrderCH["qty"]);
                        sb.AppendFormat("<td>{0}</td>", drOrderCH["cdm"]);
                        sb.AppendLine("</TR>");
                    }
                    sb.AppendLine("</TABLE>");
                    sb.AppendLine("</TD>");
                    // end of new table
                    //}

                    sb.AppendLine("</TR>");
                    sb.AppendLine("</TABLE>");


                    sb.AppendLine("<BR>");
                    #endregion CytoHisto
                }
                //..................
                // end of document
                sb.Append("</BODY>\r\n</HTML>");
                m_htmDoc.Write(sb.ToString());

                string strDocText = m_wbPrint.DocumentText;

                // tsslAmount.Text = string.Format("Working on account {0}", dr["account"]);
                this.Invalidate();

                m_dicPatient.Add(strPatID, strDocText);
                /*
                try
                {
                    Clipboard.SetText(strDocText, TextDataFormat.Text);
                }
                catch (Exception ex)
                {
                    // why
                    MessageBox.Show(ex.Message);
                }
                 */
                //  break;
            }
            //     throw new Exception("remove this error and reinstate the insert below");
            InsertIntoData360();

        }
        #endregion


        private string[] ParseCSZ(object p)
        {
            string[] retVal = new string[3];
            if (string.IsNullOrEmpty(p.ToString()))
            {
                return retVal;
            }
            string strCity = p.ToString().ToUpper().Split(new char[] { ',' })[0].Trim();
            string strSTZip = p.ToString().ToUpper().Split(new char[] { ',' })[1].Trim();
            string strState = strSTZip.Split(new char[] { ' ' })[0].Trim();
            string strZip = strSTZip.Split(new char[] { ' ' })[1].Trim();

            retVal[0] = strCity;
            retVal[1] = strState;
            retVal[2] = strZip;

            return retVal;

        }

        private void InsertIntoData360()
        {
            // put them in the table
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                foreach (KeyValuePair<string, string> kvp in m_dicPatient)
                {
                    string strId = kvp.Key.ToString().Split(new char[] { '|' })[0];
                    string strAcc = kvp.Key.ToString().Split(new char[] { '|' })[1];
                    string strDOS = kvp.Key.ToString().Split(new char[] { '|' })[2];
                    SqlCommand cmdInsert = new SqlCommand(string.Format(
                        "INSERT INTO data_quest_360 " +
                        "(patid, html_doc, account, date_of_service, pre360_error, bill_code_error, " +
                        " bill_type, " +
                        " mod_date, mod_user, mod_prg, mod_host) " +
                        "VALUES " +
                        "    ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                        strId,
                            kvp.Value.ToString().Replace("'", "''"),
                            strAcc,
                            strDOS,
                            0, 0,
                            "Q",
                        DateTime.Now, Environment.UserName, Application.ProductName + Application.ProductVersion,
                            Environment.MachineName), conn);

                    cmdInsert.Connection.Open();
                    try
                    {
                        int nRecEx = cmdInsert.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // m_Err.m_Logfile.WriteLogFile(se.Message);
                    }
                    catch (Exception ex)
                    {
                        Type tT = ex.GetType();
                        //  m_Err.m_Logfile.WriteLogFile(ex.Message);
                    }
                    finally
                    {
                        cmdInsert.Connection.Close();
                    }


                }
            }
        }


        private string ConvertRelation(object p)
        {
            string strRetVal = "OTHER";
            int nP = 9;
            if (!int.TryParse(p.ToString(), out nP))
            {
                return strRetVal;
            }
            switch (nP)
            {
                case 1:
                    {
                        strRetVal = "SELF";
                        break;
                    }
                case 2:
                    {
                        strRetVal = "SPOUSE";
                        break;
                    }
                case 3:
                    {
                        strRetVal = "CHILD";
                        break;
                    }
                default:
                    {
                        strRetVal = "OTHER";
                        break;
                    }
            }
            return strRetVal;
        }

        private void tsbFixHtml_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection drcCharges = dgvChrg.SelectedRows;
            if (drcCharges.Count < 1)
            {
                MessageBox.Show("No select charges in the Charge DataGrid.", propAppName);
                return;
            }

            string strSelect = null;
            for (int i = 0; i < drcCharges.Count; i++)
            {
                strSelect += string.Format("{0}{1}{2}{3}",
                            i == 0 ? "(" : "",
                            drcCharges[i].Cells["chrg_num"].FormattedValue.ToString(),
                            i < (drcCharges.Count - 1) ? "," : "",
                            i == (drcCharges.Count - 1) ? ")" : "");
            }
            m_ds360 = new DataSet();
            m_ds360.Tables.Add("ACC");
            m_ds360.Tables.Add("PAT");
            m_ds360.Tables.Add("INS");
            m_ds360.Tables.Add("CHRG_CLINICAL");
            m_ds360.Tables.Add("CHRG_CYTO_HISTO");

            string strAcc = drcCharges[0].Cells["account"].Value.ToString();



            m_ds360.Tables["CHRG_CLINICAL"].Rows.Clear();
            m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Clear();
            m_ds360.Tables["INS"].Rows.Clear();
            m_ds360.Tables["PAT"].Rows.Clear();
            m_ds360.Tables["ACC"].Rows.Clear();

            // using the current row fill the tables
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable dtAcc = new DataTable();
                #region ACCOUNT
                SqlCommand cmdAccSelect = new SqlCommand(
                    string.Format("select  " +
                    "	account, " +
                    " substring(pat_name, 0, charindex(',',pat_name,0)) as [lastname], " +
                    "	substring(pat_name, " +
                    "	charindex(',',pat_name,0)+1 " +
                    " , case when charindex(' ',pat_name,charindex(',',pat_name,0)-1) = 0 " +
                    "then len(pat_name) - charindex(',',pat_name,0) " +
                    "else  " +
                    " charindex(' ',pat_name,charindex(',',pat_name,0)+1) - " +
                    " charindex(',',pat_name,0) " +
                    "	end) as [firstname] " +
                    "	,	substring(pat_name,  " +
                    "	charindex(' ',pat_name, charindex(',',pat_name,0)+1)+1 " +
                    " 	, case when charindex(' ',pat_name,charindex(',',pat_name,0)+1) = 0 " +
                    " then 0 " +
                    " else  charindex(' ',pat_name,charindex(',',pat_name,0)+1)			" +
                    " end)	as [midname]" +
                    ", cl_mnem " + //, fin_code
                    "	, convert(varchar(10),trans_date,101) as [trans_date] " +
                    " ,  stuff(stuff(ssn,4,0,'-'),7,0,'-') as [ssn] " +
                    " from acc " +
                    " where acc.account = '{0}' ", strAcc)
                    , conn);
                sda.SelectCommand = cmdAccSelect;
                int nAccRec = sda.Fill(m_ds360.Tables["ACC"]);

                m_ds360.Tables["ACC"].PrimaryKey =
                    new DataColumn[] { m_ds360.Tables["ACC"].Columns["ACCOUNT"] };

                #endregion ACCOUNT
                #region PAT
                SqlCommand cmdPatSelect = new SqlCommand(
                   string.Format("select " +
                   " pat.account " +
                   " ,trans_date " +
                   ", pat_addr1, pat_addr2, city_st_zip " +
                   ", convert(varchar(10),dob_yyyy,101) as [DOB], sex, relation " +
                   ", guarantor, guar_addr, g_city_st" +
                   ", guar_phone, pat_marital  " +
                   ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
                   ", phy.last_name +','+ phy.first_name +' '+ isnull(phy.mid_init,'') +' ('+phy_id +')' as [phy_id] " +
                   "  from pat " +
                   "inner join acc on acc.account = pat.account " +
                   "left outer join phy on phy.tnh_num = pat.phy_id " +
                   " where pat.account = '{0}'", strAcc)
                   , conn);

                sda.SelectCommand = cmdPatSelect;
                int nPatRec = sda.Fill(m_ds360.Tables["PAT"]);

                //if (!m_ds360.Relations.Contains("AccPat"))
                //{
                //    DataRelation drAccPat = m_ds360.Relations.Add(
                //        "AccPat", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                //        m_ds360.Tables["PAT"].Columns["ACCOUNT"]);
                //}

                #endregion PAT
                #region INS

                SqlCommand cmdInsSelect = new SqlCommand(
                   string.Format("select " +
                   "	ins.account, ins_a_b_c, holder_nme, holder_dob, holder_sex, holder_addr, holder_city_st_zip, plan_nme, plan_addr1, plan_addr2, " +
                   "    p_city_st, policy_num, cert_ssn, grp_nme, grp_num, employer, e_city_st " +
                   "	, ins_code, relation " +
                   " from ins " +
                   " where account = '{0}' and deleted = 0 ", strAcc)
                   , conn);

                sda.SelectCommand = cmdInsSelect;
                int nInsRec = sda.Fill(m_ds360.Tables["INS"]);

                //if (!m_ds360.Relations.Contains("AccIns"))
                //{
                //    DataRelation drAccPat = m_ds360.Relations.Add(
                //        "AccIns", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                //        m_ds360.Tables["INS"].Columns["ACCOUNT"]);
                //}

                #endregion INS


                #region account charges
                string strSelectChrg =
                    string.Format("select account, qty, chrg.cdm, " +
                    " coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
                    "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
                    "from chrg " +
                    "left outer join dict_quest_reference_lab_tests dt " +
                    "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                    "left outer join dict_quest_reference_lab_tests dt2 " +
                    "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                    "where account = '{0}' " +
                    " and (chrg_num in {1}) " +
                    "and ((not chrg.cdm between '5920000' and '594ZZZZ') and chrg.cdm <> 'CBILL' " +
                    "and chrg.cdm <> '5686046') " + // wdk 20130327 added to move HPV to CytoHisto for Quest
                    "order by convert(numeric(18),coalesce(dt.quest_code,dt2.quest_code)) " // wdk 20130327 added to aid tracking in care360
                    , strAcc, strSelect);

                sda.SelectCommand =
           new SqlCommand(strSelectChrg, conn);

                DateTime dtWait = DateTime.Now;
                try
                {
                    int nChrgRec = sda.Fill(m_ds360.Tables["CHRG_CLINICAL"]);
                }
                catch (SqlException se)
                {
                    // m_Err.m_Logfile.WriteLogFile(se.Message);

                    MessageBox.Show(se.Message, "SQL EXCEPTION DURING LOAD");
                }

                #endregion account charges

                #region charges cyto_hysto
                sda.SelectCommand =
                    new SqlCommand(
                    string.Format("select account, qty, chrg.cdm, " +
                    "coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
                    "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
                    "from chrg " +
                    "left outer join dict_quest_reference_lab_tests dt " +
                    "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                    "left outer join dict_quest_reference_lab_tests dt2 " +
                    "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                    "where account = '{0}' " +
                    "and (chrg_num in {1}) " +
                    "and ((chrg.cdm between '5920000' and '594ZZZZ') " +
                    "or (chrg.cdm in ('5686046') and (chrg_num in {1}) )) " + //wdk 20130327 added to move hpv to cyto\histo sheet.
                    "order by convert(numeric(18),coalesce(dt.quest_code,dt2.quest_code))" // wdk 20130327 added to aid tracking in Care360 
                    , strAcc, strSelect),
                conn);

                dtWait = DateTime.Now;
                try
                {
                    int nChrgCytoHystoRec = sda.Fill(m_ds360.Tables["CHRG_CYTO_HISTO"]);
                }
                catch (SqlException se)
                {
                    // m_Err.m_Logfile.WriteLogFile(se.Message);
                    //int n = 0;
                    //while (DateTime.Now < dtWait.AddSeconds(10))
                    //{
                    //    n += n++; // just hum a bit
                    //}

                    MessageBox.Show(se.Message, "SQL EXCEPTION DURING LOAD");
                }
                m_ds360.Tables["CHRG_CYTO_HISTO"].PrimaryKey = null;

                #endregion charges cyto_hysto
            }

            if (m_ds360.Tables["CHRG_CLINICAL"].Rows.Count > 0)
            {
                CreateHTML(false);
            }
            if (m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Count > 0)
            {
                CreateHTML(true); // for cyto/histo 
            }

            MessageBox.Show("HTML Created");
        }

        private void dgvChrg_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvChrg.CancelEdit();
            if (dgvChrg.Rows[e.RowIndex].Cells["cdm"].FormattedValue.ToString() == "CBILL")
            {
                dgvChrg.Rows[e.RowIndex].Selected = false;
            }
        }



        private void tssbFilter_MouseHover(object sender, EventArgs e)
        {
            return;
            // SystemInformation.MouseHoverTime = 5;
            //Form f = new Form();
            //f.Text = ((ToolStripItem)sender).Text;
            //int nDex = int.Parse(((ToolStripItem)sender).Tag.ToString());
            //string strText = null;
            //switch (nDex)
            //{
            //    case 1:
            //        {
            //            strText = "1";
            //            break;
            //        }
            //    case 2:
            //        {
            //            strText = "2";
            //            break;
            //        }
            //}
            //Button btnOk = new Button();
            //btnOk.Text = strText;
            //btnOk.Dock = DockStyle.Left;
            //btnOk.DialogResult = DialogResult.OK;
            //Button btnClose = new Button();
            //btnClose.Text = "CLOSE";
            //btnClose.Dock = DockStyle.Right;
            //btnClose.DialogResult = DialogResult.Cancel;
            //f.Controls.AddRange(new Control[] { btnOk, btnClose });
            //f.Show();
        }

        private void tsmiReqNo_Click(object sender, EventArgs e)
        {
            dgvBilling.EndEdit();
            bool bDeleted = ((bool)(dgvBilling.SelectedRows[0].Cells["deleted"].Value));


            string strReqno = dgvBilling.SelectedRows[0].Cells["req_no"].Value.ToString();
            string strStatus = dgvBilling.SelectedRows[0].Cells["status"].Value.ToString();
            string strAcc = dgvBilling.SelectedRows[0].Cells["account"].Value.ToString();
            string strUID = dgvBilling.SelectedRows[0].Cells["UID"].Value.ToString();

            SqlCommand cmdUpdate = new SqlCommand(
            string.Format("update data_quest_billing " +
                "set deleted = '{0}', " +
                " status = '{1}', " +
                " account = '{2}', " +
                "mod_prg = '{3}', mod_date = '{4}', mod_user = '{5}', mod_host = '{6}' " +
                " where req_no = '{7}'",
                bDeleted, strStatus, strAcc, string.Format("{0} {1}", Application.ProductName, Application.ProductVersion),
                DateTime.Today, Environment.UserName, Environment.MachineName,
                strReqno)
                , m_sqlConnection);

            SqlDataAdapter sda = new SqlDataAdapter();
            sda.UpdateCommand = cmdUpdate;
            sda.UpdateCommand.Connection.Open();

            int nUpdates = -1;
            try
            {
                nUpdates = sda.UpdateCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
                strText += "\r\n\r\nModule: dgv360_RowHeaderMouseDoubleClick()";
                MessageBox.Show(strText, propAppName);

            }
            finally
            {
                sda.UpdateCommand.Connection.Close();
            }

            dgvBilling.SelectedRows[0].ErrorText = "SUCCESS";
            dgvBilling.Invalidate();
            this.Invalidate();


        }

        private void tsmiUID_Click(object sender, EventArgs e)
        {
            dgvBilling.EndEdit();
            bool bDeleted = ((bool)(dgvBilling.SelectedRows[0].Cells["deleted"].Value));


            string strReqno = dgvBilling.SelectedRows[0].Cells["req_no"].Value.ToString();
            string strStatus = dgvBilling.SelectedRows[0].Cells["status"].Value.ToString();
            string strAcc = dgvBilling.SelectedRows[0].Cells["account"].Value.ToString();
            string strUID = dgvBilling.SelectedRows[0].Cells["UID"].Value.ToString();

            SqlCommand cmdUpdate = new SqlCommand(
            string.Format("update data_quest_billing " +
                "set deleted = '{0}', " +
                " status = '{1}', " +
                " account = '{2}', " +
                "mod_prg = '{3}', mod_date = '{4}', mod_user = '{5}', mod_host = '{6}' " +
                " where uid = '{7}'",
                bDeleted, strStatus, strAcc, string.Format("{0} {1}", Application.ProductName, Application.ProductVersion),
                DateTime.Today, Environment.UserName, Environment.MachineName,
                strUID)
                , m_sqlConnection);

            SqlDataAdapter sda = new SqlDataAdapter();
            sda.UpdateCommand = cmdUpdate;
            sda.UpdateCommand.Connection.Open();

            int nUpdates = -1;
            try
            {
                nUpdates = sda.UpdateCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
                strText += "\r\n\r\nModule: dgv360_RowHeaderMouseDoubleClick()";
                MessageBox.Show(strText, propAppName);

            }
            finally
            {
                sda.UpdateCommand.Connection.Close();
            }

            dgvBilling.SelectedRows[0].ErrorText = "SUCCESS";
            dgvBilling.Invalidate();
            this.Invalidate();


        }

        private void dgvBilling_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvBilling.EndEdit();
        }

        private void tscbAcc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ClearGrids();
            }
            else
            {
                return;
            }
            tscbAcc_SelectedIndexChanged(tscbAcc, null);

        }

        private void ClearGrids()
        {
            Application.DoEvents();
            dgv360.DataSource = null;
            dgvBilling.DataSource = null;
            dgvChrg.DataSource = null;
            Application.DoEvents();
        }

        private void tsmiCbillInvoice_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand =
                    new SqlCommand("SELECT invoice, CONVERT(VARCHAR(10),thru_date,101) AS [thru_date] " +
                    " FROM dbo.cbill_hist WHERE cl_mnem = 'QUESTR'	 ORDER BY thru_date DESC  ", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    tscbInvoices.ComboBox.Items.Add(dr["invoice"]);
                }


            }
        }





























    } // don't go below this line
}
