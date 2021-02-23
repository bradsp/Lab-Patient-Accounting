using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// Programmer added
using System.Drawing.Printing;
using System.Data.SqlClient;
using RFClassLibrary;
using MCL;
using System.IO; 
          

namespace LabBilling.Legacy
{
    public partial class frmSSISort : Form
    {
         private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }
       
        string m_strFilter = null;
        int nFilterColumn = -1;
        SqlConnection m_sqlConn = null;
        DataTable m_dtTreeView = null;
        DataTable m_dtDataView = null;
        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        ERR m_Err = null;
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        ToolStripControlHost m_cboxInclude; // CheckBox
        //ToolStripControlHost m_cboxIncludeBT; // CheckBox
        DateTime m_dtFrom;
        DateTime m_dtThru;
        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;
        
        /// <summary>
        /// comment missing
        /// </summary>
        /// <param name="args"></param>
        public frmSSISort(string[] args)
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
            m_Err = new ERR(strArgs);

            //this.Text += string.Format(" - {0}", m_strProductionEnvironment);
          
        }

        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dpFrom.Text = DateTime.Now.ToString("d");
            m_dtFrom = DateTime.Now;
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpFrom.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpFrom.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpFrom.Control).Name = "FROM";
            m_dpFrom.Control.Width = 95;
            m_dpFrom.Control.Refresh();
            m_dpFrom.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFrom = new ToolStripLabel("From: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

            m_dpThru = new ToolStripControlHost(new DateTimePicker());
            m_dpThru.Text = DateTime.Now.AddDays(5).ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
            m_dtThru = DateTime.Now.AddDays(5);
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpThru.Control).Name = "THRU";
            m_dpThru.Control.Width = 95;
            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
            //   tsMain.BackColor = Color.Lavender;

            // check box
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslInclude = new ToolStripLabel("Include \"Paid Out\" records?");
            m_cboxInclude = new ToolStripControlHost(new CheckBox());
            ((CheckBox)m_cboxInclude.Control).Click += new EventHandler(frmSSISort_Click);
            ((CheckBox)m_cboxInclude.Control).Checked = true;
            frmSSISort_Click(((CheckBox)m_cboxInclude.Control), null);
            tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
            tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);


            //// check box
            //ToolStripLabel tslIncludeBT = new ToolStripLabel("BILLING TYPE");
            //m_cboxIncludeBT = new ToolStripControlHost(new CheckBox());
            //tsMain.Items.Insert(tsMain.Items.Count, tslIncludeBT);
            //tsMain.Items.Insert(tsMain.Items.Count, m_cboxIncludeBT);


            //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }

        void frmSSISort_Click(object sender, EventArgs e)
        {
            m_strFilter =
                string.Format(((CheckBox)sender).Checked ? "" : " and status <> 'PAID_OUT'");
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

        }

          
        private void frmSSISort_Load(object sender, EventArgs e)
        {
            m_ViewerPrintDocument = new PrintDocument();
            //m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
            CreateDateTimes();
           m_sqlConn = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                    + "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));

            //string strSelect = string.Format("select distinct {0} "+
            //    " from pat where {0} is not null
           using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
           {
               SqlDataAdapter sda = new SqlDataAdapter();
               SqlCommand cdmStartDate =
                   new SqlCommand("select value from system where key_name = 'ssi_start_date'", connection);
               sda.SelectCommand = cdmStartDate;
               DataTable dtDate = new DataTable();
               sda.Fill(dtDate);
               m_dtFrom = DateTime.Parse(dtDate.Rows[0]["value"].ToString());
               m_dpFrom.Text = m_dtFrom.ToShortDateString();

               cdmStartDate =
                   new SqlCommand("select value from system where key_name = 'ssi_bill_thru_date'", connection);
               sda.SelectCommand = cdmStartDate;
               dtDate = new DataTable();
               sda.Fill(dtDate);

               m_dtThru = DateTime.Parse(dtDate.Rows[0]["value"].ToString());
               m_dpThru.Text = m_dtThru.ToShortDateString();
           }

          
        }

      

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            tvRecords.Nodes.Clear();
            LoadTreeView();
        }

        private void LoadTreeView()
        {
           using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
           {
               // batch numbers for viewing
               SqlDataAdapter sda = new SqlDataAdapter();
               SqlCommand cdmStartDate = 
                   new SqlCommand(string.Format("with cte as ( "+
                       " select distinct acc.status, trans_date, ssi_batch "+//, isnull(ub_date, h1500_date) as [date sent]  
                       " ,convert(datetime,convert(varchar(10),coalesce(ub_date,h1500_date),101)) as [batch_date] "+
                       " from pat "+
                       " inner join acc on acc.account = pat.account "+
                       //" where ssi_batch is not null "+
                       " and  acc.status in ('UB','UBOP','1500','SSIUB','SSIUBOP','SSI1500'))"+
                       " select status, ssi_batch, batch_date from cte "+
                       " where trans_date between '{0} 00:00' and '{1} 23:59:59' "+
                       " order by status, ssi_batch desc",m_dtFrom.ToShortDateString(), m_dtThru.ToShortDateString())
                       , connection);
               sda.SelectCommand = cdmStartDate;
               m_dtTreeView = new DataTable();
               sda.Fill(m_dtTreeView);

               TreeNode tnStatus = new TreeNode();
               TreeNode tnBatch = new TreeNode();
               TreeNode tnUB = new TreeNode("UB");
               tnUB.Name = "UB";           
               TreeNode tnUBOP = new TreeNode("UBOP");
               tnUBOP.Name = "UBOP";
               TreeNode tn1500 = new TreeNode("1500");
               tn1500.Name = "1500";
               TreeNode tnSSIUB = new TreeNode("SSIUB");
               tnSSIUB.Name = "SSIUB";
               TreeNode tnSSIUBOP = new TreeNode("SSIUBOP");
               tnSSIUBOP.Name = "SSIUBOP";
               TreeNode tnSSI1500 = new TreeNode("SSI1500");
               tnSSI1500.Name = "SSI1500";

               tvRecords.Nodes.Add(tnUB);
               tvRecords.Nodes.Add(tnUBOP);
               tvRecords.Nodes.Add(tn1500);
               tvRecords.Nodes.Add(tnSSIUB);
               tvRecords.Nodes.Add(tnSSIUBOP);
               tvRecords.Nodes.Add(tnSSI1500);
               
               foreach (DataRow dr in m_dtTreeView.Rows)
               {
                   string strBatch = dr["ssi_batch"].ToString();
                   if (string.IsNullOrEmpty(strBatch))
                   {
                       strBatch = "NOT SENT";
                   }
                   string strDate = dr["batch_date"].ToString();
                   
                  
                   // UB's not sent to SSI yet
                   if (tnUB.Text == dr["status"].ToString())
                   {
                       if (!tnUB.Nodes.ContainsKey(strBatch))
                       {
                           tnBatch = tnUB.Nodes.Add(strBatch);
                           tnBatch.Name = strBatch;
                           if (!string.IsNullOrEmpty(strDate))
                           {
                               tnBatch.Tag = strDate;
                           }
                           continue;
                       }
                   }

                   // UBOP's not sent to SSI
                   if (tnUBOP.Text == dr["status"].ToString())
                   {
                       if (!tnUBOP.Nodes.ContainsKey(strBatch))
                       {
                           tnBatch = tnUBOP.Nodes.Add(strBatch);
                           tnBatch.Name = strBatch;
                           if (!string.IsNullOrEmpty(strDate))
                           {
                               tnBatch.Tag = strDate;
                           }
                           continue;
                       }
                   }
                   // 1500 not sent to SSI
                   if (tn1500.Text == dr["status"].ToString())
                   {
                       if (!tn1500.Nodes.ContainsKey(strBatch))
                       {
                           tnBatch = tn1500.Nodes.Add(strBatch);
                           tnBatch.Name = strBatch;
                           if (!string.IsNullOrEmpty(strDate))
                           {
                               tnBatch.Tag = strDate;
                           }
                           continue;
                       }
                   }
                   // UB's that have been sent to SSI
                   if (tnSSIUB.Text == dr["status"].ToString())
                   {
                       if (!tnSSIUB.Nodes.ContainsKey(strBatch))
                       {
                            tnBatch = tnSSIUB.Nodes.Add(strBatch);
                            tnBatch.Name = strBatch;
                            if (!string.IsNullOrEmpty(strDate))
                            {
                                tnBatch.Tag = strDate;
                            }
                            continue;
                       }
                   }

                   if (tnSSIUBOP.Text == dr["status"].ToString())
                   {
                       if (!tnSSIUBOP.Nodes.ContainsKey(strBatch))
                       {
                           tnBatch = tnSSIUBOP.Nodes.Add(strBatch);
                           tnBatch.Name = strBatch;
                           if (!string.IsNullOrEmpty(strDate))
                           {
                               tnBatch.Tag = strDate;
                           }
                           continue;
                       }
                   }

                   if (tnSSI1500.Text == dr["status"].ToString())
                   {
                       if (!tnSSI1500.Nodes.ContainsKey(strBatch))
                       {
                           tnBatch = tnSSI1500.Nodes.Add(strBatch);
                           tnBatch.Name = strBatch;
                           if (!string.IsNullOrEmpty(strDate))
                           {
                               tnBatch.Tag = strDate;
                           }
                           continue;
                       }
                   }
                  
                   
               }
               
           }
           
           tvRecords.Sort();
           
        }


        private void tvRecords_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                string strDate = e.Node.Tag.ToString();
                string strBatch = e.Node.Text;
                e.Node.Text = strDate;
                e.Node.Tag = strBatch;
            }
            string strFilter = string.Format("ssi_batch = '{0}'", e.Node.Text);
            if (!string.IsNullOrEmpty(m_strFilter))
            {
                strFilter += m_strFilter;// " and status = 'PAID_OUT'";
            }
            
            if (e.Node.Level == 1)
            {
                LoadDataGrid(strFilter);
            }
            if (e.Node.Level == 0)
            {
                strFilter = string.Format("status = '{0}'", e.Node.Text);
                LoadDataGrid(strFilter);
            }
        }

        private void LoadDataGrid(string strFilter)
        {
          //  dgvRecords.Rows.Clear();
          //  dgvRecords.Columns.Clear();
          //  dgvRecords = new DataGridView();

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                // batch numbers for viewing
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cdmStartDate =
                    new SqlCommand(
                        string.Format("with cte as ( "+
                        " select acc.status,acc.fin_code,acc.pat_name, trans_date "+
                        " , pat.account, ssi_batch, isnull(ub_date,h1500_date) as [ssi_date] "+
                        " , ins_code ,mailer"+
                        " from pat "+
                        " inner join acc on acc.account = pat.account "+
                        " inner join ins on ins.account = pat.account and ins_a_b_c = 'A'"+
                        " where {2} and pat.mailer = 'N' )"+
                        " select status,ssi_batch, cte.account, fin_code, pat_name, trans_date, ssi_date, ins_code, mailer, "+
                        " case when status_type = 'EDI'"+
                        " then convert(varchar(10),status_date,101)"+
                        " 	end as [EDI Date]"+
                        " , case when status_type = 'PAY'"+
                        " 	then convert(varchar(10),status_date,101)"+
                        " 	end as [PAYOR Date]"+
                        " , case when status_type = 'INS'"+
                        " 	then convert(varchar(10),status_date,101)"+
                        " 	end as [INS Date]"+
                        " , status_on_claim"+
                        " from cte "+
                        "left outer join data_electronic_status deStat on deStat.account = cte.account "+
                        " where trans_date between '{0} 00:00' and '{1} 23:59:59' "+
                        " order by cte.account", 
                            m_dtFrom.ToShortDateString(), m_dtThru.ToShortDateString(), strFilter), connection);
                sda.SelectCommand = cdmStartDate;
                m_dtDataView = new DataTable();
                sda.Fill(m_dtDataView);

                dgvRecords.DataSource = m_dtDataView;
                dgvRecords.Invalidate();

                
                tsslRecords.Text = string.Format("{0} Selected records.", dgvRecords.Rows.Count );
            }
        }


        private void dgvAccount_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                LaunchAcc la = new LaunchAcc(m_strDatabase);
                string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                la.LaunchAccount(strAcc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Exception occured trying to open the account. \r\n {0}", ex.Message));
            }

        }

        /// <summary>
        /// used to sort the account table only!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRecords_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string strColText = dgvRecords.Columns[e.ColumnIndex].HeaderText;
            nFilterColumn = e.ColumnIndex;
            System.Windows.Forms.SortOrder SO = dgvRecords.SortOrder == System.Windows.Forms.SortOrder.None ? System.Windows.Forms.SortOrder.Ascending : dgvRecords.SortOrder;
            
            BindingSource bbs = new BindingSource(m_dtDataView, "ACCOUNT");// m_dsAccount.Tables["ACC"], "ACCOUNT");

            BindingSource bs = new BindingSource(dgvRecords.DataSource,
                dgvRecords.Columns[e.ColumnIndex].Name);
            bs.DataMember = dgvRecords.DataMember;
            string strFilter = bs.Filter;

            if (e.Button == MouseButtons.Right)
            {
                
                string strColName = dgvRecords.Columns[e.ColumnIndex].Name;
                FormResponse f = new FormResponse();
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    string strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (strColText == "pat_name")
                    {
                        strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString().Split(new char[] { ',' })[0][0].ToString();
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
                bs = new BindingSource(dgvRecords.DataSource,
                    strColText);
                bs.DataMember = dgvRecords.DataMember;
                

                if (string.IsNullOrEmpty(strResponse))
                {
                     bs.RemoveFilter();
                   
                }
                else
                {

                    bs.Filter = string.Format("[{0}] in ({1})  ",
                        strColText, strResponse//, tscbFinCodes.SelectedItem.ToString()
                        //, ((CheckBox)m_cboxInclude.Control).Checked ? "OUTPATIENT" : "REF LAB"
                        );
                }
                bs.Sort = string.Format("{0} {1},pat_name ASC", dgvRecords.Columns[e.ColumnIndex].Name,
                    SO == System.Windows.Forms.SortOrder.Ascending ? "ASC":"DESC");
                dgvRecords.DataSource = bs;
            }
            else
            {
                string strSort = string.Format("{0} {1}, pat_name ASC", dgvRecords.Columns[e.ColumnIndex].Name
                    , SO == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC"
                    );
                
                
                bs.Sort = strSort;
                dgvRecords.DataSource = bs;
            }
            tsslRecords.Text = string.Format("{0} Selected records.", dgvRecords.Rows.Count);
        }

        private void tvRecords_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in tvRecords.Nodes)
            {
                foreach (TreeNode tn2 in tn.Nodes)
                {
                    tn2.BackColor = Color.White;
                }
                tn.BackColor = Color.White;
            }

        

        }

        private void tvRecords_MouseClick(object sender, MouseEventArgs e)
        {
            ((TreeView)sender).HitTest(e.Location).Node.BackColor = Color.LightBlue;
        }

        private void tvRecords_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            try
            {
                e.Node.ToolTipText = e.Node.Tag.ToString();
            }
            catch (NullReferenceException)
            {
                // keep on trucking level 0 has no tag
            }
             
            
        }

        /// <summary>
        /// Prints the applications current view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrintView_Click(object sender, EventArgs e)
        {
            Bitmap[] bmps = RFClassLibrary.dkPrint.Capture(dkPrint.CaptureType.Form);
            try
            {
                bmps[0].Save(string.Format(@"C:\Temp\{0}.bmp", Application.ProductName));
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                RFClassLibrary.dkPrint.propStreamToPrint =
                  new StreamReader(string.Format(@"C:\Temp\{0}.bmp", Application.ProductName));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                RFClassLibrary.dkPrint.propStreamToPrint.Close();
            }
            PrintDocument printDoc = new PrintDocument();

            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.PrintPage += new PrintPageEventHandler
                (RFClassLibrary.dkPrint.PrintGraphic_PrintPage);

            printDoc.Print();

            printDoc.PrintPage -= new PrintPageEventHandler
                (RFClassLibrary.dkPrint.PrintGraphic_PrintPage);

            RFClassLibrary.dkPrint.propStreamToPrint.Close();

        }







        private void tsbPrintGrid_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(string.Format("Grid has {0} records! Continue?", dgvRecords.Rows.Count),
                "PRINT GRID", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            m_ViewerPrintDocument.Print();

        }

        private void tsbClearBatch_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will reset all records in the datagrid from SSI status.\r\nThis will add a note to the account that you cleared the record.\r\r\nContinue?",propAppName,MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                string strType = dr.Cells["status"].Value.ToString();
                string strAcc = dr.Cells["account"].Value.ToString();

                tsslAcc.Text = string.Format("Working on account {0}", strAcc);
                Application.DoEvents();
                
                using (SqlConnection conn = new SqlConnection(m_sqlConn.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    SqlCommand cmdPatUpdate = new SqlCommand(
                        string.Format("update pat " +
                        "set ssi_batch = NULL, " +
                        "{0} = NULL " +
                        //",mod_prg = '{1}', mod_date = '{2}', mod_user = '{3}', mod_host = '{4}' " +
                        "where account = '{1}'", strType.Contains("1500") ? "h1500_date" : "ub_date" 
                        //,propAppName, DateTime.Now, Environment.UserName, Environment.MachineName
                        ,strAcc)
                        , conn);
                    SqlCommand cmdAccUpdate = new SqlCommand(
                        string.Format("update acc " +
                        "set status = '{0}' " +
                        //",mod_prg = '{1}', mod_date = '{2}', mod_user = '{3}', mod_host = '{4}' " +
                        "where account = '{1}' ", strType.Replace("SSI", "")
                        //, propAppName, DateTime.Now, Environment.UserName, Environment.MachineName
                        , strAcc)
                        , conn);
                    SqlCommand cmdNoteInsert = new SqlCommand(
                        string.Format("Insert into notes " +
                        "(account, mod_date, mod_user, mod_prg, mod_host, comment, rowguid) " +
                        "values "+
                        "('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ",
                        strAcc, DateTime.Now, Environment.UserName,propAppName, Environment.MachineName
                        ,string.Format("{0} Status removed from account",strType),Guid.NewGuid().ToString())
                        , conn);
                    sda.InsertCommand = cmdNoteInsert;
                    sda.UpdateCommand = cmdAccUpdate;
                    sda.UpdateCommand.Connection.Open();
                    int nUpdate = -1;
                    try
                    {
                        nUpdate = sda.UpdateCommand.ExecuteNonQuery();
                        sda.UpdateCommand = cmdPatUpdate;
                        nUpdate += sda.UpdateCommand.ExecuteNonQuery();
                        nUpdate += sda.InsertCommand.ExecuteNonQuery();
                        
                    }
                    catch (SqlException se)
                    {
                        string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
                        strText += "\r\n\r\nModule: tsbClearBatch_Click()";
                        dr.ErrorText = strText;
                       // MessageBox.Show(strText);
                       // return; // if we can't update the Acc record don't update the pat record until this is resolved.
                    }
                    finally
                    {
                        sda.UpdateCommand.Connection.Close();
                    }
                    if (nUpdate == -1)
                    {
                        return;
                    }

                }
            }
            MessageBox.Show("BATCH CLEARED", propAppName);
            tsbClearBatch.Checked = false;
        }


        

         
 

        

        

 

        

        


    } // don't go below here
}
