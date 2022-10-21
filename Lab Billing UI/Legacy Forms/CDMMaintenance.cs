using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace LabBilling.Legacy
{
    public partial class frmCDM : Form
    {
        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        // ERR m_Err = null;
        //ToolStripControlHost m_dpFrom;
        //ToolStripControlHost m_dpThru;
        ToolStripControlHost m_cboxFS2; // CheckBox
        ToolStripControlHost m_cboxFS3; // CheckBox
                                        //DateTime m_dtFrom;
                                        //DateTime m_dtThru;
                                        //    private PrintDocument m_ViewerPrintDocument;
                                        //    private ReportGenerator m_rgReport;
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }
        private SqlConnection m_sqlConn = null;


        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            //m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            //m_dpFrom.Text = DateTime.Now.ToString("d");
            //((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            //((DateTimePicker)m_dpFrom.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            //((DateTimePicker)m_dpFrom.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            //((DateTimePicker)m_dpFrom.Control).Name = "FROM";
            //m_dpFrom.Control.Width = 95;
            //m_dpFrom.Control.Refresh();
            //m_dpFrom.Invalidate();
            //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            //ToolStripLabel tslFrom = new ToolStripLabel("From: ");
            //tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            //tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

            //m_dpThru = new ToolStripControlHost(new DateTimePicker());
            //m_dpThru.Text = DateTime.Now.AddDays(5).ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
            //((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            //((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            //((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            //((DateTimePicker)m_dpThru.Control).Name = "THRU";
            //m_dpThru.Control.Width = 95;
            //m_dpThru.Control.Refresh();
            //m_dpThru.Invalidate();

            //ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            //tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            //tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
            ////   tsMain.BackColor = Color.Lavender;

            // check box
            ToolStripLabel tslFS2 = new ToolStripLabel("Fee Schedule 2");
            m_cboxFS2 = new ToolStripControlHost(new CheckBox());
            tsMain.Items.Insert(tsMain.Items.Count, tslFS2);
            tsMain.Items.Insert(tsMain.Items.Count, m_cboxFS2);

            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFS3 = new ToolStripLabel("Fee Schedule 3");
            m_cboxFS3 = new ToolStripControlHost(new CheckBox());
            tsMain.Items.Insert(tsMain.Items.Count, tslFS3);
            tsMain.Items.Insert(tsMain.Items.Count, m_cboxFS3);

            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }
        //void frmAcc_CloseUp(object sender, EventArgs e)
        //{
        //    //  Requery();
        //}

        //void frmAcc_ValueChanged(object sender, EventArgs e)
        // {
        //     if (((DateTimePicker)sender).Name == "FROM")
        //     {
        //         m_dtFrom = ((DateTimePicker)sender).Value;
        //     }
        //     else
        //     {
        //         m_dtThru = ((DateTimePicker)sender).Value;
        //     }

        // }


        public frmCDM(string[] args)
        {
            InitializeComponent();
            if (args.GetUpperBound(0) < 1)
            {
                MessageBox.Show("Not enough arguments to start this application");
                Environment.Exit(13);
            }
            m_strServer = args[0];
            m_strDatabase = args[1];
            m_strProductionEnvironment = m_strDatabase.Contains("LIVE") ? "LIVE" : "TEST";
            string[] strArgs = new string[3];
            strArgs[0] = string.Format("/{0}", m_strProductionEnvironment);
            strArgs[1] = args[0];
            strArgs[2] = args[1];
            // m_Err = new ERR(strArgs);

            //this.Text += string.Format(" - Production Environment {0}", m_strProductionEnvironment);
            CreateDateTimes();
        }

        private void frmCDM_Load(object sender, EventArgs e)
        {

            // m_ViewerPrintDocument = new PrintDocument();
            //m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            //m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            //m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);

            m_sqlConn = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                      + "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));
            // use this to load any combo box on the toolstip
            SqlDataAdapter sda = new SqlDataAdapter(); // local to this function only
            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                Application.DoEvents();
                //     tsslNote.Text = "Loading Start Date";
                // get the operating date range for queries.

                SqlCommand cdmSelect =
                   new SqlCommand(
                       string.Format("select cdm from dbo.cdm where deleted = 0 and orderable = 1 order by cdm"
                       )
                       , connection);
                sda.SelectCommand = cdmSelect;
                Application.DoEvents();
                DataTable dtValue = new DataTable();
                Application.DoEvents();
                sda.Fill(dtValue);
                //m_dtStartSSI = DateTime.Parse(dtStart.Rows[0]["value"].ToString()); //OR
                tscbCDM.ComboBox.DataSource = dtValue;
                tscbCDM.ComboBox.DisplayMember = "CDM";
            }
        }

        private void tsbLoadCDM_Click(object sender, EventArgs e)
        {
            dgvFS2.Rows.Clear();
            dgvFS3.Rows.Clear();
            SqlDataAdapter sda = new SqlDataAdapter(); // local to this function only
            SqlDataAdapter sdaFS2 = new SqlDataAdapter();
            SqlDataAdapter sdaFS3 = new SqlDataAdapter();
            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                Application.DoEvents();
                //     tsslNote.Text = "Loading Start Date";
                // get the operating date range for queries.

                SqlCommand cdmSelectCDM =
                   new SqlCommand(
                       string.Format("SELECT dbo.cpt4.cdm ,dbo.cpt4.link ,dbo.cpt4.code_flag " +
                       ",dbo.cpt4.cpt4 ,dbo.cpt4.descript ,dbo.cpt4.mprice ,dbo.cpt4.cprice " +
                       ",dbo.cpt4.zprice ,dbo.cpt4.rev_code ,dbo.cpt4.type ,dbo.cpt4.modi " +
                       ",dbo.cpt4.billcode " +
                       "FROM cpt4 WHERE cdm = '{0}'	AND deleted = 0"
                       , tscbCDM.Text)
                       , connection);
                sda.SelectCommand = cdmSelectCDM;
                Application.DoEvents();
                DataTable dtValue = new DataTable();
                Application.DoEvents();
                sda.Fill(dtValue);

                dgvCDM.DataSource = dtValue;
                dgvCDM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dgvCDM.AutoGenerateColumns = true;

                CreateDataGrids();
                // fee schedule 2

                SqlCommand cdmSelectFS2 =
                   new SqlCommand(
                       string.Format("SELECT dbo.cpt4_2.cdm ,dbo.cpt4_2.link ,dbo.cpt4_2.code_flag " +
                       ",dbo.cpt4_2.cpt4 ,dbo.cpt4_2.descript ,dbo.cpt4_2.mprice ,dbo.cpt4_2.cprice " +
                       ",dbo.cpt4_2.zprice ,dbo.cpt4_2.rev_code ,dbo.cpt4_2.type ,dbo.cpt4_2.modi " +
                       ",dbo.cpt4_2.billcode " +
                       "FROM cpt4_2 WHERE cdm = '{0}'	AND deleted = 0"
                       , tscbCDM.Text)
                       , connection);
                sdaFS2.SelectCommand = cdmSelectFS2;
                Application.DoEvents();
                DataTable dtValueFS2 = new DataTable();
                Application.DoEvents();
                sdaFS2.Fill(dtValueFS2);

                foreach (DataRow dr in dtValueFS2.Rows)
                {
                    dgvFS2.Rows.Add(new object[] { dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6], dr[7], dr[8], dr[9], dr[10], dr[11] });
                }

                dgvFS2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                foreach (DataGridViewRow drMain in dgvCDM.Rows)
                {
                    DataGridViewRow drFS2;
                    try
                    {
                        drFS2 = dgvFS2.Rows[drMain.Index];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        int nRows = dtValueFS2.Rows.Count;
                        dtValueFS2.Rows.Add(new object[] { "Missing" });

                        dtValueFS2.Rows[dtValueFS2.Rows.Count - 1].SetColumnError(0, "row not present in FS2");
                        //strText = ((DataRow)((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row)["status"].ToString().ToUpper();
                        drFS2 = new DataGridViewRow();
                        drFS2.SetValues(dtValueFS2.Rows[dtValueFS2.Rows.Count - 1].ItemArray);
                        //drFS2.Cells.AddRange(dtValueFS2.Rows[drMain.Index].ItemArray);
                        //int nRow = dgvFS2.Rows.Count;
                        //dgvFS2.Rows[nRow].ErrorText = "row not present in FS2";
                        continue;
                    }

                    for (int i = 0; i < drMain.Cells.Count; i++)
                    {
                        //if (string.IsNullOrEmpty(drFS2.HeaderCell.FormattedValue.ToString() ))
                        //{
                        //    continue;
                        //}
                        if (i == 5) // mprice
                        {
                            continue;
                        }
                        if (drFS2.Cells[i].FormattedValue.ToString() != drMain.Cells[i].FormattedValue.ToString())
                        {
                            drFS2.ErrorText = "different";
                            drFS2.Cells[i].ErrorText = "this one";
                        }
                    }
                    foreach (DataGridViewRow dr in dgvFS2.Rows)
                    {
                        if (dr.IsNewRow)
                        {
                            continue;
                        }
                        if (dr.Index > dgvCDM.Rows.Count - 1)
                        {
                            dr.ErrorText = "Excess Row will be deleted if updated.";
                        }
                    }

                    // fee schedule 3
                    SqlCommand cdmSelectFS3 =
                      new SqlCommand(
                          string.Format("SELECT dbo.cpt4_3.cdm ,dbo.cpt4_3.link ,dbo.cpt4_3.code_flag " +
                          ",dbo.cpt4_3.cpt4 ,dbo.cpt4_3.descript ,dbo.cpt4_3.mprice ,dbo.cpt4_3.cprice " +
                          ",dbo.cpt4_3.zprice ,dbo.cpt4_3.rev_code ,dbo.cpt4_3.type ,dbo.cpt4_3.modi " +
                          ",dbo.cpt4_3.billcode " +
                          "FROM cpt4_3 WHERE cdm = '{0}'	AND deleted = 0"
                          , tscbCDM.Text)
                          , connection);
                    sdaFS3.SelectCommand = cdmSelectFS3;
                    Application.DoEvents();
                    DataTable dtValueFS3 = new DataTable();
                    Application.DoEvents();
                    sdaFS3.Fill(dtValueFS3);
                }


            }

        }

        private void CreateDataGrids()
        {
            if (dgvFS2.Columns.Count > 0)
            {
                return;
            }
            foreach (DataGridViewColumn dc in dgvCDM.Columns)
            {
                dgvFS2.Columns.Add(dc.Name, dc.HeaderText);
                dgvFS3.Columns.Add(dc.Name, dc.HeaderText);
            }
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            // copy selected items to current tabls fee schedule
            DataGridView dgv = new DataGridView();


            if (((CheckBox)m_cboxFS2.Control).Checked)
            {
                dgvFS2.EndEdit();
                dgv = ((DataGridView)tcMain.SelectedTab.Controls[
                 tcMain.SelectedTab.Controls.GetChildIndex(dgvFS2)]);
                // update fs2
                ((CheckBox)m_cboxFS2.Control).Checked = false;
                using (SqlConnection conn = new SqlConnection(m_sqlConn.ConnectionString))
                {
                    SqlCommand cmdDelete = new SqlCommand(
                        string.Format("delete from cpt4_2 where cdm = '{0}'", tscbCDM.Text), conn);
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.DeleteCommand = cmdDelete;
                    sda.DeleteCommand.Connection.Open();
                    try
                    {
                        sda.DeleteCommand.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        sda.DeleteCommand.Connection.Close();
                    }
                    foreach (DataGridViewRow dr in dgvFS2.Rows)
                    {
                        if (dr.IsNewRow)
                        {
                            continue;
                        }
                        if (dr.ErrorText.Contains("Excess"))
                        {
                            continue;
                        }
                        SqlCommand cmdInsert = new SqlCommand(
                            "INSERT INTO dbo.cpt4_2 " +
                            "(	cdm ,link ,	cpt4 ,descript ,	mprice , " +
                            "	cprice ,zprice ,rev_code ,	type ,	modi , " +
                            "	billcode ,	mod_date ,	mod_user ,	mod_prg , mod_host , " +
                            "   code_flag	) " +
                            "VALUES	( " +
                            "@1,  " + //-- cdm - varchar(7)
                            "@2 , " + //-- link - int 
                            "@3 , " + //-- cpt4 - varchar(5)
                            "@4 , " + //-- descript - varchar(50)
                            "@5 , " + //-- mprice - money 
                            "@6 ,  " + //-- cprice - money 
                            "@7 ,  " + //-- zprice - money
                            "@8 ,  " + //-- rev_code - varchar(4)
                            "@9 ,  " + //-- type - varchar(4)
                            "@10 ,  " + //-- modi - varchar(2)
                            "@11 ,  " + //-- billcode - varchar(7)
                            "@12 ,  " + //-- mod_date - datetime
                            "@13 ,  " + //-- mod_user - varchar(50)
                            "@14 ,  " + //-- mod_prg - varchar(50)
                            "@15 ,  " + //-- mod_host - varchar(50)
                            "@16)  ", conn); //-- code_flag - varchar(50)

                        cmdInsert.Parameters.AddWithValue("@1", dr.Cells["cdm"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@2", dr.Cells["link"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@3", dr.Cells["cpt4"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@4", dr.Cells["descript"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@5", dr.Cells["mprice"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@6", dr.Cells["cprice"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@7", dr.Cells["zprice"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@8", dr.Cells["rev_code"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@9", dr.Cells["type"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@10", dr.Cells["modi"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@11", dr.Cells["billcode"].Value.ToString());
                        cmdInsert.Parameters.AddWithValue("@12", DateTime.Now.ToShortDateString());
                        cmdInsert.Parameters.AddWithValue("@13", string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName));
                        cmdInsert.Parameters.AddWithValue("@14", propAppName);
                        cmdInsert.Parameters.AddWithValue("@15", Environment.MachineName);
                        cmdInsert.Parameters.AddWithValue("@16", dr.Cells["code_flag"].Value.ToString());

                        sda = new SqlDataAdapter();
                        sda.InsertCommand = cmdInsert;
                        sda.InsertCommand.Connection.Open();
                        try
                        {
                            sda.InsertCommand.ExecuteNonQuery();
                        }
                        catch (SqlException se)
                        {
                            MessageBox.Show(se.Message);
                        }
                        finally
                        {
                            sda.InsertCommand.Connection.Close();
                        }
                    }
                }
            }
            if (((CheckBox)m_cboxFS3.Control).Checked)
            {
                dgv = ((DataGridView)tcMain.SelectedTab.Controls[
                 tcMain.SelectedTab.Controls.GetChildIndex(dgvFS3)]);
                //update fs3
                ((CheckBox)m_cboxFS3.Control).Checked = false;
            }

            tsbLoadCDM_Click(null, null);


        }


        private void dgvCDM_DragLeave(object sender, EventArgs e)
        {
            WarningException ellipse = ((WarningException)sender);
            if (ellipse != null)
            {
                //  ellipse.Fill = _previousFill;
            }
        }

        private void dgvCDM_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo info = dgvCDM.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    DataGridViewSelectedRowCollection view = dgvCDM.SelectedRows;//.Rows[info.RowIndex].DataBoundItem;
                                                                                 // DataRowView view = (DataRowView)dgvCDM.Rows[info.RowIndex].DataBoundItem;
                    if (view != null)
                    {
                        dgvCDM.DoDragDrop(view, DragDropEffects.Copy);
                    }
                }
            }


        }

        private void dgv_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dgv_DragDrop(object sender, DragEventArgs e)
        {
            DataGridView dgv = ((DataGridView)sender);
            // if (e.Data.GetDataPresent(typeof(DataRowView)))
            if (e.Data.GetDataPresent(typeof(DataGridViewSelectedRowCollection)))
            {
                int nCount = ((DataGridViewSelectedRowCollection)e.Data.GetData(typeof(DataGridViewSelectedRowCollection))).Count;
                DataGridViewSelectedRowCollection drc =
                    ((DataGridViewSelectedRowCollection)e.Data.GetData(typeof(DataGridViewSelectedRowCollection)));
                DataGridViewRow[] arDgv = new DataGridViewRow[3];
                drc.CopyTo(arDgv, 0);
                foreach (DataGridViewRow dr in arDgv)
                {

                    dgv.Rows.Add(dr);
                }

                //IEnumerator en = drc.GetEnumerator();
                //en.Reset();
                //int nFound = -1;
                //while (en.MoveNext())
                //{
                //    dgv.Rows.Add( ((DataGridViewRow)en.Current));
                //}

                //for (int i = 0; i < drc.Count; i++)
                //{
                //    dgv.Rows.Add(   ((DataGridViewRow)drc[i]));
                //}

                //dgvFS2.Rows.Add(((DataRowView)e.Data.GetData(typeof(DataRowView))).Row.ItemArray);
            }
        }
    }
}
