/*
 * Created 05/25/2007 by David Kelly
 * Purpose: Allow both Billing 6.5 (VC++) and the new Billing Menu (VS.NET C#) the ability to pass
 *  1. Connection to the database
 *  2. Table being viewed
 *  3. Filter preferably containing a rowguid.
 * by sending args in the constructor. If no args are passed a message is displaced and this app closes.
 * Args need to be in the form.
 *  
 *  arg[0] -- Datasource Server // MCL03
 *  arg[1] -- Database catalog //MCLTEST
 *  arg[2] -- Audit Table //audit_acc
 *  arg[3] -- table's field to filter on (should be named ~ xxx_rowguid where xxx is the original table name.
 *  arg[4] -- field value to look for in the history //master table rowguid 333-4443kkjf2-kdkjfkj2-kdjflkdj
 * 
 * Allow filtering in this view for a date range subset of the records
 */

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
// programmer added
using Microsoft.Data.SqlClient; // for sql connection
using System.Drawing.Printing; // for printing the gridview
using RFClassLibrary;

namespace LabBilling.Legacy
{
    /// <summary>
    /// 05/31/2007 wdk main form
    /// </summary>
    public partial class frmHistory : Form
    {
        
          // The DataGridView Control which will be printed.
        //DataGridView dgvHist;
        // The PrintDocument to be used for printing.
        PrintDocument AuditPrintDocument;
        // The class that will do the printing process.
        private DataGridViewPrinter AuditDataGridViewPrinter;
        
        //AboutBoxViewerHistory m_AboutDlg;
        ToolStripControlHost m_dateTimePickerFrom;
        ToolStripControlHost m_dateTimePickerThru;
        ToolStripControlHost m_chkboxUseDateTimes;
        private string m_strConn;
        private string m_strTable;
        private string m_strQueryAudit;
 //       private string m_strQueryCurrent;
        private string m_strWhere;
        private string m_strFields = "*";
        private DataSet m_dsSource;
        private BindingSource m_bsDataSource;

        /// <summary>
        /// 05/30/2007 class to create our new history viewer
        /// </summary>
        /// <param name="args">Args passed from at creation see documentation
        ///*  arg[0] -- Datasource Server // MCL03
        ///*  arg[1] -- Database catalog //MCLTEST
        ///*  arg[2] -- Audit Table //audit_acc
        ///*  arg[3] -- table's field to filter on (should be named ~ xxx_rowguid where xxx is the original table name.
        ///*  arg[4] -- field value to look for in the history //master table rowguid 333-4443kkjf2-kdkjfkj2-kdjflkdj
        ///</param>
        public frmHistory(string[] args)
        {            
            //InitializeComponent();
            InitializeDefaults(args);          
        }

        private void InitializeDefaults(string[] args)
        {
            //m_AboutDlg = new AboutBoxViewerHistory();
            if (args.Length < 6)
            {
                foreach (String s in args)
                {
                    //m_AboutDlg.textBoxDescription.Text += s+"\r\n";
                }
                MessageBox.Show("There were an incorrect number of arguments passed to this application." +
                            "\r\nCan not continue.", "INITILIZATION ERROR");
                //m_AboutDlg.ShowDialog();
                //Environment.Exit(13);
            }
            //try // this sets the window title 
            //{
                this.Text += args[5].Remove(0, 1);
            // }
            //catch (IndexOutOfRangeException)
            //{
            //    // don't do anything a blank was passed for the window title just keep on trucking.
            //    // 06/25/2007 wdk provides backwards comptability with the ACC program until it can be updated.
            //}


            // set connection properties
            m_strConn = 
                string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=SSPI;"+
                "Connection Timeout = 60", args[0].Remove(0, 1), args[1].Remove(0, 1)); // wdk 20130530 added connection timeout to overcome 15 second limit.
            m_strTable = args[2].Remove(0, 1);
            // set up the where statment for the query for SelectRows()
            m_strWhere = string.Format(" where {0} = '{1}'", args[3].Remove(0, 1), args[4].Remove(0, 1));
           // m_strWhere = string.Format(" where chrg_num = '18864360'");
            // set up the query for SelectRows()
            m_strQueryAudit = string.Format("select * from {0}{1} order by mod_date desc", m_strTable, m_strWhere);
            // create a new DataSet
            m_dsSource = new DataSet("AuditDataSet");
            // Set up the data source.
            m_bsDataSource = new BindingSource();
            
           
            string[] strArgs = { "Database Server", "Database", "Audit Table" };//, "Audit Rowguid" };
            string strText = "";
            int i = 0;
            foreach (string str in args)
            {
                cmsCommandLineArgs.Items.Add(str);
               
                if (i > 2)
                {
                    break;
                }
                strText += string.Format("{0}:\t {1}\r\n", strArgs[i++], str.Replace("\\", ""));
                
            }
            //m_AboutDlg.textBoxDescription.Text = strText;    
        }

      

        /// <summary>
        ///  wdk 05/25/2007 returns the data set matching the query using the connection string passed.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="connectionString"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private static DataSet SelectRows(DataSet dataset,
            string connectionString, string queryString)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(
                    queryString, connection);
               // adapter.SelectCommand.CommandTimeout = connection.ConnectionTimeout * 2;
                try
                {
                    adapter.Fill(dataset);
                }
                catch (SqlException se)
                {
                    MessageBox.Show(se.Message + "\r\n\nCannot process request", "SQL EXCEPTION");
                }
                return dataset;
            }
        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
           
            //dgvHist.Location = new Point(10, 30);
            // initially sets the grids size. Resets on forms resize
            //dgvHist.Height = this.Size.Height - 75;
            //dgvHist.Width = this.Size.Width - 30;
            //dgvHist.Invalidate();
           
            Application.DoEvents();
           
            LoadGrid(); 
            int nIndex = -1;
            nIndex = cmsDropDownFields.Items.Add(new ToolStripMenuItem("*"));
            ((ToolStripMenuItem)cmsDropDownFields.Items[nIndex]).Checked = true;
            foreach (DataColumn dc in m_dsSource.Tables[0].Columns)
            {
                nIndex = cmsDropDownFields.Items.Add(new ToolStripMenuItem( dc.ColumnName));
                ((ToolStripMenuItem)cmsDropDownFields.Items[nIndex]).Checked = true;
                
            }      
        }

        private void LoadGrid()
        {           
            dgvHist.Columns.Clear();
            dgvHist.Invalidate();
            Application.DoEvents();
           
            dgvHist.AutoGenerateColumns = true;
      
            m_dsSource = new DataSet("AuditDataSet");

            // 01/24/2008 rgc/wdk removed due to new methodology of the audit trigger. The current record
            // was the first and last using this older method.
            /////////////current record
            //// add the current record to the datagridview
            //// trim the audit_ from the table name.
            //int nTableStart = m_strQueryAudit.IndexOf("audit_", 0);
            //m_strQueryCurrent = m_strQueryAudit.Remove(nTableStart, 6);
            //// trim the XXX_ (table name from the rowguid)
            //int nRowguidStart = m_strQueryCurrent.IndexOf("_", nTableStart);
            //int nFirstSpaceStart = m_strQueryCurrent.LastIndexOf(" ", nRowguidStart);
            //m_strQueryCurrent = m_strQueryCurrent.Remove((nFirstSpaceStart + 1), (nRowguidStart - nFirstSpaceStart));

            //// set the dataset from the returned dataset
            //m_dsSource = SelectRows(m_dsSource, m_strConn, m_strQueryCurrent);
            //// set the binding source from the returned table
            //m_bsDataSource.DataSource = m_dsSource.Tables[0];    // always 0 [Zero] becuase our current dataset only uses one table
            //// set the gridviews data source
            //dgvHist.DataSource = m_bsDataSource;
            

            /////////// audit records
            // set the dataset from the returned dataset
            m_dsSource = SelectRows(m_dsSource, m_strConn, m_strQueryAudit);
            if (m_dsSource.Tables.Count == 0)
            {
                Environment.Exit(0);
            }
            // set the binding source from the returned table
            m_bsDataSource.DataSource = m_dsSource.Tables[0];    // always 0 [Zero] becuase our current dataset only uses one table
           
            // set the gridviews data source
            dgvHist.DataSource = m_bsDataSource;
            dgvHist.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            
            int rowNum = 1;
            foreach (DataGridViewRow row in dgvHist.Rows)
            {
                row.HeaderCell.Value = rowNum.ToString();
                rowNum++;
            }
            foreach (DataGridViewColumn col in dgvHist.Columns)
            {
                if (col.HeaderText.Contains("rowguid") || col.HeaderText.Contains("uid") || col.HeaderText.Contains("req_num"))
                {
                    col.Visible = false;
                }
            }
            try
            {
                dgvHist.Columns["mod_date"].DisplayIndex = 1;
                dgvHist.Columns["mod_user"].DisplayIndex = 2;
                dgvHist.Columns["mod_prg"].DisplayIndex = 3;
                dgvHist.Columns["mod_host"].DisplayIndex = 4;
            }
            catch
            {
                // don't care some tables don't have these fields and some don't have all of these fields.
            }

            dgvHist.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            VisualizeChanges();
            dgvHist.Invalidate();    
        }


        /// <summary>
        /// 05/25/2007 David this fuction loads controls with data then moves the 
        /// controls onto the tool strip (tsMain) then subscribes the Events necessary
        /// for the added controls
        /// </summary>
        private void DesignControls()
        {
            #region combolistboxes // not used at this time
            //// create a new host passing the checked list box control
            //ToolStripControlHost tschMain = new ToolStripControlHost(new CheckedListBox());
            ////tschMain.Font = new Font("Arial", 7.0F, FontStyle.Italic);
            //   tsMain.Items.Add(tschMain);
            #endregion combolistboxes 
            
            #region checkboxes // checkboxes

            // create a new toolstripcontrolhost, passing in a control.
            m_chkboxUseDateTimes = new ToolStripControlHost(new CheckBox());
            m_chkboxUseDateTimes.Text = "Use Date Times";
            // Cast the Control property back to the original type to set a 
            // type-specific properties.
            ((CheckBox)m_chkboxUseDateTimes.Control).Checked = false;
            ((CheckBox)m_chkboxUseDateTimes.Control).BackColor = tsMain.BackColor;
            // add the control to the tool strip
            tsMain.Items.Add(m_chkboxUseDateTimes);
            // subscribe the controls events.
            ((CheckBox)m_chkboxUseDateTimes.Control).CheckedChanged
                   += new System.EventHandler(CheckBoxValueChanged); // CheckBoxValueChanged() is defined below
            #endregion checkboxes


           #region datetime pickers // date time pickers

            // Create a new ToolStripControlHost, passing in a control.
            m_dateTimePickerFrom = new ToolStripControlHost(new DateTimePicker());
            m_dateTimePickerThru = new ToolStripControlHost(new DateTimePicker());

            // Set the font on the ToolStripControlHost, this will affect the hosted control.
            //      m_dateTimePickerFrom.Font = new Font("Arial", 7.0F, FontStyle.Italic);

            // Set the Width property, this will also affect the hosted control.
            //m_dateTimePickerFrom.Width = 50;
            m_dateTimePickerFrom.DisplayStyle = ToolStripItemDisplayStyle.Text;
            m_dateTimePickerThru.DisplayStyle = ToolStripItemDisplayStyle.Text;

            m_dateTimePickerThru.Name = "DateTimePickerThru";
            m_dateTimePickerFrom.Name = "DateTimePickerFrom";

            // Setting the Text property requires a string that converts to a 
            // DateTime type since that is what the hosted control requires.
            m_dateTimePickerFrom.Text = DateTime.Today.Subtract(new TimeSpan(7, 0, 0, 0)).ToShortDateString();
            m_dateTimePickerThru.Text = DateTime.Today.ToShortDateString();

            // Cast the Control property back to the original type to set a 
            // type-specific property.
            ((DateTimePicker)m_dateTimePickerFrom.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dateTimePickerThru.Control).Format = DateTimePickerFormat.Short;

            // Add the control host to the ToolStrip.
            ToolStripLabel tslF = new ToolStripLabel();
            tslF.Name = "Date From";
            tslF.Text = "From Date:";
            tsMain.Items.Add(tslF);
            tsMain.Items.Add(m_dateTimePickerFrom);
            // Add the control host to the ToolStrip.
            ToolStripLabel tslT = new ToolStripLabel();
            tslT.Name = "Date Thru";
            tslT.Text = "Thru Date:";
            tsMain.Items.Add(tslT);
            tsMain.Items.Add(m_dateTimePickerThru);
            

            // subscribe the controls events.
            ((DateTimePicker)m_dateTimePickerFrom.Control).ValueChanged
                   += new System.EventHandler(DateTimeValueChanged); // DateTimeValueChanged() is defined below

            ((DateTimePicker)m_dateTimePickerThru.Control).ValueChanged
                       += new System.EventHandler(DateTimeValueChanged); // DateTimeValueChanged() is defined below

            #endregion datetime pickers

            // call the function to set the default behavior.
            CheckBoxValueChanged((object)((CheckBox)m_chkboxUseDateTimes.Control), null);
        }

        private void CheckBoxValueChanged(object sender, EventArgs e)
        {
            foreach (ToolStripItem tsi in tsMain.Items)
            {
                if (tsi.Name.IndexOf("Date") >= 0)
                {
                    tsi.Visible = ((CheckBox)sender).Checked;
                }
                if (tsi.Text.IndexOf(":") > 0)
                {
                    tsi.Visible = ((CheckBox)sender).Checked;
                }
                
            }

            if (((CheckBox)m_chkboxUseDateTimes.Control).Checked)
            {
                m_strWhere += string.Format(" and mod_date between '{0} 00:00' and '{1} 23:59'", ((DateTimePicker)m_dateTimePickerFrom.Control).Text, ((DateTimePicker)m_dateTimePickerThru.Control).Text);
                m_strQueryAudit = string.Format("select {0} from {1}{2}", m_strFields, m_strTable, m_strWhere);
            }
            else
            {
                if (m_strWhere.IndexOf(" and mod_date") >-1)
                {
                    m_strWhere = m_strWhere.Substring(0, m_strWhere.IndexOf(" and mod_date"));
                    m_strQueryAudit = string.Format("select {0} from {1}{2}", m_strFields, m_strTable, m_strWhere);

                }
            }

        }

        /// <summary>
        /// 05/25/2007 wdk Provides a handler for the ToolStripControlHost date times for the from and thru
        /// </summary>
        /// <param name="sender">((DateTimePicker)sender).Value Represents the newly selected date time</param>
        /// <param name="e">This should be null at all times</param>
        private void DateTimeValueChanged(object sender, EventArgs e)
        {
            //int x;
            //x = 9;
        }

        private void frmHistory_Resize(object sender, EventArgs e)
        {
            // when the form resizes resize the gridview
            dgvHist.Height = this.Size.Height - 75;
            dgvHist.Width = this.Size.Width - 30;
            dgvHist.Invalidate();
        }

        private void cmsDropDownFields_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((ToolStripMenuItem)e.ClickedItem).Checked = !((ToolStripMenuItem)e.ClickedItem).Checked;
            // 05/25/2007 turning off the autoclose so the user can select all the fields necessary for the filter
            // enabling the autoclose is handled from the menus mouse leave event.
            cmsDropDownFields.AutoClose = false;
            if (e.ClickedItem.ToString().IndexOf("*") > -1)
            {
                bool bCheckAll = ((ToolStripMenuItem)e.ClickedItem).Checked;
                if (!bCheckAll)
                {
                    MessageBox.Show("You have unselectd ALL items. At least one item must be selected.");
                }
                foreach (ToolStripMenuItem tsmi in cmsDropDownFields.Items)
                {
                    tsmi.Checked = bCheckAll;
                }
            }
        }

 
        /// <summary>
        /// 05/25/2007 wdk 
        /// Clicking outside the dropdown menu will cause this to fire which then closes the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmsDropDownFields_MouseLeave(object sender, EventArgs e)
        {
            cmsDropDownFields.AutoClose = true;
        }

        private void cmsDropDownFields_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {           
            dgvHist.Columns.Clear();
            dgvHist.Invalidate();
            dgvHist.AutoGenerateColumns = true;
            m_strFields = "";
            foreach (ToolStripMenuItem tsmi in cmsDropDownFields.Items)
            {
                if (tsmi.Checked)
                {
                    if (tsmi.Text == "*")
                    {
                        continue;
                    }
                    m_strFields += string.Format("{0}, ", tsmi.Text);                    
                }
            }
            // if the fields list is empty display a message an return because you can't load the grid.
            if (m_strFields.Length <= 0)
            {
                MessageBox.Show("You have not selected any fields for display in the grid.");
                return;
            }
            // remove the space and comma from the end of the string 
            m_strFields = m_strFields.TrimEnd(null);
            m_strFields = m_strFields.TrimEnd(',');
            m_strQueryAudit = string.Format("select {0} from {1}{2} order by mod_date desc", m_strFields, m_strTable, m_strWhere)  ;
            LoadGrid();

        }

        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            LoadGrid();
         }


        /// <summary>
        /// 05/30/2007 wdk colors the differences from one row to the next
        /// </summary>
        private void VisualizeChanges()
        {
            DataGridViewRow drOld = null;
            foreach (DataGridViewRow dr in dgvHist.Rows)
            {
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
                        if (dgvHist[dc.Index, dr.Index].Value.ToString() != dgvHist[dc.Index, dr.Index + 1].Value.ToString())
                        {
                            if (dgvHist[dc.Index, dr.Index].Value.ToString() != dgvHist[dc.Index, dr.Index + 1].Value.ToString())
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

        private void tsbtnPrint_Click(object sender, EventArgs e)
        {
            // Create a AuditPrintDocument and set its PrintPage Handler event
            AuditPrintDocument = new PrintDocument();
            this.AuditPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.MyPrintDocument_PrintPage);
        }
 
        /// <summary>
        ///  05/31/2007 wdk
        /// The PrintPage action for the PrintDocument control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPrintDocument_PrintPage(object sender,
            System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = AuditDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

        /// <summary>
        /// 05/31/2007 wdk
        ///     Click event handler for the print preview menu item on the Print grid toolstrip button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPrintPreview_Click(object sender, EventArgs e)
        {
            if (SetupThePrinting(false))
            {
                PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                MyPrintPreviewDialog.Document = AuditPrintDocument;
                MyPrintPreviewDialog.ShowDialog();
            }
        }

        private void tsmiPrint_Click(object sender, EventArgs e)
        {
            if (SetupThePrinting(true))
            {
                AuditPrintDocument.Print();
            }
        }

        /// <summary>
        /// 05/31/2007 wdk  The printing setup function
        /// </summary>
        /// <returns></returns>
        private bool SetupThePrinting(bool bShowPrintDlg)
        {
        PrintDialog AuditPrintDialog = new PrintDialog();
        AuditPrintDialog.AllowCurrentPage = false;
        AuditPrintDialog.AllowPrintToFile = false;
        AuditPrintDialog.PrintToFile = false;
        AuditPrintDialog.AllowSelection = false;
        AuditPrintDialog.AllowSomePages = false;
        AuditPrintDialog.ShowHelp = false;
        AuditPrintDialog.ShowNetwork = false;
        AuditPrintDialog.UseEXDialog = true;
            

        if (bShowPrintDlg)
        {
            if (AuditPrintDialog.ShowDialog() != DialogResult.OK)
                return false;
        }
        AuditPrintDocument.DocumentName = m_strTable.ToUpper()+" History";
        AuditPrintDocument.PrinterSettings = 
                            AuditPrintDialog.PrinterSettings;
        AuditPrintDocument.DefaultPageSettings =
        AuditPrintDialog.PrinterSettings.DefaultPageSettings;
        AuditPrintDocument.DefaultPageSettings.Margins = 
                         new Margins(40, 40, 40, 40);
        AuditDataGridViewPrinter = new DataGridViewPrinter(dgvHist,
        AuditPrintDocument, "true, true");//, m_strTable.ToString(), new Font("Tahoma", 18,
        //FontStyle.Bold, GraphicsUnit.Point), Color.Black, true, "");
        AuditPrintDocument.DefaultPageSettings.Landscape = true;

        return true;
    }

        private void dgvHist_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    dgvHist.Columns[e.ColumnIndex].Visible = false;
                }
                catch
                {
                    MessageBox.Show("Cannot hide this column.");
                }
            }
            VisualizeChanges();
            dgvHist.Invalidate();
            Application.DoEvents();
        }

       

        private void dgvHist_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    dgvHist.Rows[e.RowIndex].Visible = false;
                }
                catch
                {
                    MessageBox.Show("Cannot hide this row.");
                }
            }
            VisualizeChanges();
            dgvHist.Invalidate();
            Application.DoEvents();

        }

        private void frmHistory_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            //{
            //    MessageBox.Show("Pressed " + Keys.Control);
            //    e.Handled = true;

            //}
            
        }

 

  

        

       




    }
}