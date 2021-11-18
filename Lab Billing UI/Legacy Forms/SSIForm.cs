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
using System.IO;
using System.Diagnostics;
// programmer added
using RFClassLibrary;
using MCL;
using System.Drawing.Printing;
using System.Configuration;
// Move these two lines to the header space
using System.Reflection;
using System.Drawing.Imaging;


namespace LabBilling.Legacy
{
    public partial class frmSSI : Form
    {
        #region Declarations
        public Bitmap m_memoryImage;
        StreamReader m_streamToPrint;
        StringReader m_stringToPrint = null;
        bool m_bHas1500Error = false;
        StringBuilder m_sbUBHeader = new StringBuilder();
        StringBuilder m_sbUB = new StringBuilder();
        StringBuilder m_sb1500Header = new StringBuilder();
        StringBuilder m_sb1500 = new StringBuilder();

        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }

        string m_strType = null;
        ArrayList m_alNameSuffix = new ArrayList() { "JR", "SR", "I", "II", "III", "IV", "V", "VI", "VII" };
        private string m_strTransSetControlNumber;
        private Dictionary<string, string> m_dicClaimFilingIndicatorCode;
        private Dictionary<string, string> m_dicRefs;
        private DataTable m_dtInsuranceInfo;
        private DataTable m_dtCpt4Desc;
        private ArrayList m_alCpt4DescriptionRequired;
        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;
        R_acc m_rAccUpdate = null;
        R_pat m_rPat = null;
        R_number m_rNumber = null;
        ERR m_Err = null;
        private string m_strFilter = null;
        DataSet m_dsBilling = null;
        int m_nHLCounter = 1;
        int m_nHLParent = 0;
        int m_nST = 1;
        int m_nSTSegments = 2; // includes the ST and SE without counting
        string m_strSubmitterId = null;
        int m_nFunctionalGroups = 0;
        string m_strBHT = null;
        string m_strST = null;
        string m_strSE = null;
        string m_strGS = null;
        string m_strGE = null;
        string m_strISA = null;
        string m_strIEA = null;
        ArrayList m_alFile = null;
        SqlDataAdapter m_daAcc = null;
        SqlDataAdapter m_daPat = null;
        SqlDataAdapter m_daIns = null;
        SqlDataAdapter m_daChrg = null;
        SqlDataAdapter m_daAmt = null;
        SqlConnection m_sqlConnection = null;
        string m_strInterchageControlNumber = null;
        string m_strProductionEnvironment;
        string m_strServer = null;
        string m_strDatabase = null;
        const string m_837i_version = "005010X223A2";
        const string m_837p_version = "004010X098A1";
        #endregion

        public string PropProductionEnvironment
        {
            get { return m_strProductionEnvironment; }
            set
            {
                // TODO: Reinstate before releasing the application for billing
                if (value == "LIVE")
                {
                    m_strProductionEnvironment = "P";
                }
                else
                {
                    m_strProductionEnvironment = "T";
                }
            }
        }

        public frmSSI(string[] args)
        {
            InitializeComponent();
            if (args.GetUpperBound(0) < 1)
            {
                MessageBox.Show("Not enough arguments to start the program");
                Environment.Exit(13);
            }
            m_strServer = args[0].Remove(0, 1);
            m_strDatabase = args[1].Remove(0, 1);

            m_sqlConnection =
                new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                    + "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));

            PropProductionEnvironment = m_strDatabase.Contains("LIVE") ? "LIVE" : "TEST";
            m_strSubmitterId = GetSystemParameter("fed_tax_id") ?? "626010402";
            m_alFile = new ArrayList();
            string[] strArgs = new string[3];
            strArgs[0] = m_strDatabase.Contains("LIVE") ? "/LIVE" : "/TEST";
            strArgs[1] = args[0];
            strArgs[2] = args[1];

            m_Err = new ERR(strArgs);
            m_rAccUpdate = new R_acc(m_strServer, m_strDatabase, ref m_Err);
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);
            m_rNumber = new R_number(m_strServer, m_strDatabase, ref m_Err);

            //this.Text += string.Format(" Production Environment - {0}", m_strDatabase);

        }

        private string GetSystemParameter(string keyName)
        {
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                string retVal = null;
                string sql = "SELECT value from [system] WHERE key_name = @key";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@key", SqlDbType.VarChar);
                cmd.Parameters["@key"].Value = keyName;
                try
                {
                    conn.Open();
                    retVal = (string)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    retVal = null;
                    MessageBox.Show(string.Format("Error retrieving parameter {0} - {1}", keyName, ex.Message));
                }

                return retVal;
            }
        }

        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            ToolStripControlHost m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dpFrom.Text = DateTime.Now.ToLongDateString();
            ((DateTimePicker)m_dpFrom.Control).MaxDate = DateTime.Now;
            ((DateTimePicker)m_dpFrom.Control).MinDate = DateTime.Now;
            m_dpFrom.ToolTipText = "This date cannot be changed.";
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Long;
            ((DateTimePicker)m_dpFrom.Control).Name = "Date";
            m_dpFrom.Control.Width = 195;
            m_dpFrom.Control.Refresh();
            m_dpFrom.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFrom = new ToolStripLabel("Date: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);
            #region Removed Code
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

            //// check box
            //ToolStripLabel tslInclude = new ToolStripLabel("Include in Filter");
            //m_cboxInclude = new ToolStripControlHost(new CheckBox());
            //tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
            //tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

            //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            #endregion
            tsMain.Refresh();
        }

        void frmAcc_CloseUp(object sender, EventArgs e)
        {
            //  Requery();
        }

        #region PrintButton

        ToolStripSplitButton tsmiPrint = new ToolStripSplitButton();
        ToolStripMenuItem tsmiPrintView = new ToolStripMenuItem();
        ToolStripMenuItem tsmiPrintGrid = new ToolStripMenuItem();


        private void CreatePrintButtons()
        {
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            int nSert = tsMain.Items.Count;
            // create the main button
            // 
            // tsmiPrint
            // 
            this.tsmiPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPrintView,
            this.tsmiPrintGrid});
            //this.tsmiPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPrint.Image")));
            this.tsmiPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiPrint.Name = "tsmiPrint";
            this.tsmiPrint.Size = new System.Drawing.Size(69, 22);
            this.tsmiPrint.Text = "PRINT";
            tsMain.Items.Insert(tsMain.Items.Count, tsmiPrint);
            // 
            // tsmiPrintView
            // 
            this.tsmiPrintView.CheckOnClick = true;
            this.tsmiPrintView.Name = "tsmiPrintView";
            this.tsmiPrintView.Size = new System.Drawing.Size(144, 22);
            this.tsmiPrintView.Text = "PRINT VIEW";
            this.tsmiPrintView.Click += new System.EventHandler(this.tsmiPrintView_Click);
            // 
            // tsmiPrintGrid
            // 
            this.tsmiPrintGrid.Name = "tsmiPrintGrid";
            this.tsmiPrintGrid.Size = new System.Drawing.Size(144, 22);
            this.tsmiPrintGrid.Text = "PRINT GRID";
            this.tsmiPrintGrid.Click += new System.EventHandler(this.tsmiPrintGrid_Click);

            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }

        private void tsmiPrintView_Click(object sender, EventArgs e)
        {
            #region Capture
            Application.DoEvents();
            Screen[] screens = Screen.AllScreens;
            Rectangle rc = Application.OpenForms[0].Bounds;

            Bitmap image = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            m_memoryImage = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            try
            {
                using (Graphics memoryGraphics = Graphics.FromImage(image))
                {
                    memoryGraphics.CopyFromScreen(rc.X, rc.Y,
                    0, 0, rc.Size, CopyPixelOperation.SourceCopy);

                    m_memoryImage = image;
                }
            }
            catch (Exception ex)
            {
                string strErr = string.Format("METHOD:\t\t {0}.\r\nEXCEPTION:\t {1}\r\nERROR TYPE:\t {2}.",
                    MethodBase.GetCurrentMethod().Name, ex.Message, ex.GetType());
                CreateMessageForm(strErr);
                //MessageBox.Show(
                //string.Format("METHOD:\t\t {0}.\r\nEXCEPTION:\t {1}\r\nERROR TYPE:\t {2}.",
                //    MethodBase.GetCurrentMethod().Name, ex.Message, ex.GetType()), propAppName);
            }
            #endregion Capture
            string strPath = string.Format(@"C:\Temp\{0}_{1}.bmp", propAppName, DateTime.Now.ToFileTime());//"YYYYmmDDHHMMss"));

            try
            {
                m_memoryImage.Save(strPath);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                m_streamToPrint = new StreamReader(strPath);

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, propAppName);
                m_stringToPrint.Close();
            }
            PrintDocument printDoc = new PrintDocument();

            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.PrintPage += new PrintPageEventHandler(PrintGraphic_PrintPage);

            printDoc.Print();

            printDoc.PrintPage -= new PrintPageEventHandler
                (PrintGraphic_PrintPage);

            m_streamToPrint.Close();

        }

        private void tsmiPrintGrid_Click(object sender, EventArgs e)
        {
            // m_ViewerPrintDocument = new PrintDocument();
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);

            m_ViewerPrintDocument.DocumentName = this.Text;
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            int nRows = dgvRecords.Rows.Count;
            if (nRows == 0)
            {
                MessageBox.Show("No records ready to print.", propAppName);
                return;
            }
            m_ViewerPrintDocument.Print();
        }

        /// <summary>
        /// This function prints graphic objects. To print text call PrintText_PrintPage()
        /// Can be used instead of having to create a PrintPage event Handler in the application.
        /// /// <code> <example>
        ///     PrintDocument printDoc = new PrintDocument();
        ///     printDoc.PrintPage += new PrintPageEventHandler(RFClassLibrary.dkPrint.PrintGraphic_PrintPage);
        /// </example></code>
        /// 09/21/2007 wdk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PrintGraphic_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (m_memoryImage != null)
            {
                Bitmap page;
                if (m_memoryImage.Height < e.PageBounds.Height && m_memoryImage.Width < e.PageBounds.Width)
                {
                    page = m_memoryImage;
                }
                else
                {
                    page = new Bitmap(m_memoryImage, e.PageBounds.Width, e.PageBounds.Height);
                }
                e.Graphics.DrawImage(m_memoryImage, 0, 0);
            }
        }

        /// <summary>
        /// This function prints text objects. To print graphics call PrintGraphic_PrintPage
        /// Can be used instead of having to create a PrintPage event Handler in the application.
        /// 
        /// <code> <example>
        ///     PrintDocument printDoc = new PrintDocument();
        ///     printDoc.PrintPage += new PrintPageEventHandler(RFClassLibrary.dkPrint.PrintText_PrintPage);
        /// </example></code>
        /// 
        /// 09/21/2007 wdk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        public void PrintText_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Font printFont = new Font("Arial", 10);
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = m_stringToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        #endregion PrintButton

        private void Load_frmSSI(object sender, EventArgs e)
        {

            CreatePrintButtons();
            m_dsBilling = new DataSet();
            m_dsBilling.Tables.Add("ACC");
            m_dsBilling.Tables.Add("PAT");
            m_dsBilling.Tables.Add("INS");
            m_dsBilling.Tables.Add("CHRG");
            m_dsBilling.Tables.Add("AMT");

            //m_sqlConnection =
            //    new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
            //        + "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));

            m_daAcc = new SqlDataAdapter();
            m_daPat = new SqlDataAdapter();
            m_daIns = new SqlDataAdapter();
            m_daChrg = new SqlDataAdapter();
            m_daAmt = new SqlDataAdapter();

            // printing of the rtb
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            // grid printing
            m_ViewerPrintDocument = new PrintDocument();
            //m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, "SSI", m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);

            CreateDirectories();
            CreateInsuranceDataTable();
            CreateDateTimes();
            ProcessBNPs();
        }

        private void CreateDirectories()
        {
            string path = @"C:\temp\SSI";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = @"C:\temp\UB";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = @"C:\temp\UBOP";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = @"C:\temp\1500";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        /// <summary>
        /// No longer used - this function does nothing.
        /// </summary>
        private void ProcessBNPs()
        {
            return;
            //using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            //{
            //    SqlCommand cmd = new SqlCommand();
            //    SqlDataReader reader;
            //    cmd.CommandText = "usp_bnp_update";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Connection = conn;
            //    try
            //    {
            //        conn.Open();
            //        reader = cmd.ExecuteReader();
            //    }
            //    catch (SqlException se)
            //    {
            //        if (se.Message.Contains("Could not find stored procedure 'usp_bnp_update'."))
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            throw (se); // rethrow so we can figure out the problem.

            //        }
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //    conn.Close();
            //}
        }


        /// <summary>
        /// Contains insurances company codes with valid qualifier ID's 
        /// if the insurance is not in the list use the "OTHER" to select the NPI qualifier for MCL.
        /// </summary>
        private void CreateInsuranceDataTable()
        {
            m_dicRefs = new Dictionary<string, string>();
            SqlDataAdapter sda = new SqlDataAdapter();
            m_dtInsuranceInfo = new DataTable("INSURANCE");
            m_dtCpt4Desc = new DataTable();
            DataTable dtWarnings = new DataTable();
            m_alCpt4DescriptionRequired = new ArrayList();

            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlCommand cmdSelect = new SqlCommand(
                    " SELECT    code, name, addr1, addr2, citystzip, provider_no_qualifier, provider_no, payer_no, claimsnet_payer_id,  " +
                    " fin_code, comment " +
                    " FROM         insc " +
                    " WHERE     (deleted = 0) ", conn);
                sda.SelectCommand = cmdSelect;
                sda.Fill(m_dtInsuranceInfo);

                cmdSelect = new SqlCommand(
                    " select cdm.cdm,cdm.descript as [Line Desc], " +
                    " cpt4, cpt4.descript as [cpt4 desc] " +
                    " from cdm " +
                    " inner join cpt4 on cpt4.cdm = cdm.cdm " +
                    " where cdm.deleted = 0 and cpt4.deleted = 0 " +
                    " order by link", conn);
                sda.SelectCommand = cmdSelect;
                sda.Fill(m_dtCpt4Desc);

                cmdSelect = new SqlCommand(
                    "select cpt4 from dict_cpt4_warnings where deleted = 0", conn);
                sda.SelectCommand = cmdSelect;

                sda.Fill(dtWarnings);
            }
            foreach (DataRow dr in m_dtInsuranceInfo.Rows)
            {
                m_dicRefs.Add(dr["code"].ToString().Trim(),
                    string.Format("REF*{0}*{1}~",
                        dr["provider_no_qualifier"].ToString().Trim(),
                        dr["provider_no"].ToString().Trim()));
            }

            foreach (DataRow dr in dtWarnings.Rows)
            {
                m_alCpt4DescriptionRequired.Add(dr["cpt4"].ToString());
            }

            //// 2320 LOOP SBR09 SEGMENT (SSI edit source pay code indicator?)
            m_dicClaimFilingIndicatorCode = new Dictionary<string, string>();
            // //m_dicClaimFilingIndicatorCode.Add("11", "Other Non-Federal Programs");
            ////m_dicClaimFilingIndicatorCode.Add("12", "Preferred Provider Organization (PPO)");
            ////m_dicClaimFilingIndicatorCode.Add("13", "Point of Service (POS)");
            ////m_dicClaimFilingIndicatorCode.Add("14", "Exclusive Provider Organization (EPO)");
            ////m_dicClaimFilingIndicatorCode.Add("15", "Indemnity Insurance");
            ////m_dicClaimFilingIndicatorCode.Add("16", "Health Maintenance Organization (HMO) Medicare Risk");
            ////m_dicClaimFilingIndicatorCode.Add("17", "Dental Maintenance Organization");
            ////m_dicClaimFilingIndicatorCode.Add("AM", "Automobile Medical");
            ///* rgc/wdk 20111116 837 manual indicates the below is correct.
            //// However the SSI edit requres that BlueCross be sent as Commercial so we 
            //// changed the code to the be 
            m_dicClaimFilingIndicatorCode.Add("1B", "BL"); //, "Blue Cross/Blue Shield");
                                                           //

            //// rgc/wdk 20111116 SSI EDIT work around
            //m_dicClaimFilingIndicatorCode.Add("1B", "CI"); //, "Blue Cross/Blue Shield"); 

            m_dicClaimFilingIndicatorCode.Add("1H", "CH"); //, "Champus");
            ////m_dicClaimFilingIndicatorCode.Add("CI", "Commercial Insurance Co.");
            ////m_dicClaimFilingIndicatorCode.Add("DS", "Disability");
            ////m_dicClaimFilingIndicatorCode.Add("FI", "Federal Employees Program");
            m_dicClaimFilingIndicatorCode.Add("HM", "HM");
            ////m_dicClaimFilingIndicatorCode.Add("LM", "Liability Medical");
            ////m_dicClaimFilingIndicatorCode.Add("1C", "MA"); //, "Medicare Part A");
            ////m_dicClaimFilingIndicatorCode.Add("MB", "Medicare Part B");
            m_dicClaimFilingIndicatorCode.Add("1D", "MC"); //, "Medicaid");
                                                           ////m_dicClaimFilingIndicatorCode.Add("OF", "Other Federal Program"); //Use code OF when submitting Medicare Part D claims.
                                                           ////m_dicClaimFilingIndicatorCode.Add("TV", "Title V");
                                                           ////m_dicClaimFilingIndicatorCode.Add("VA", "Veterans Affairs Plan");
                                                           ////m_dicClaimFilingIndicatorCode.Add("WC", "Workers’ Compensation Health Claim";
                                                           ////m_dicClaimFilingIndicatorCode.Add("ZZ", "Mutually Defined"); // use code zz when type of insurance is not known

        }

        private void LoadGrid(string strType)
        {
            m_strType = strType;
            m_dsBilling.Tables["AMT"].Rows.Clear();
            m_dsBilling.Tables["CHRG"].Rows.Clear();
            m_dsBilling.Tables["PAT"].Rows.Clear();
            m_dsBilling.Tables["INS"].Rows.Clear();
            m_dsBilling.Tables["ACC"].Rows.Clear();
            rtbDoc.Text = "";
            string strFilterAcc;
            using (SqlConnection connection = new SqlConnection(m_sqlConnection.ConnectionString))
            {

                // if you don't have any valid ub's set the date to today to bypass the selection filter
                SqlCommand cmdSelectAcc = new SqlCommand();
                string strWhere = "";
                if (m_strFilter.Contains("MEDICARE"))
                {
                    //strWhere = string.Format("where status = '{0}' and (plan_nme in ('MEDICARE','CIGNA'))", strType);
                    strWhere = string.Format("where status = '{0}' and (ins_code in ('MC','CIGNA'))", strType);
                }
                if (m_strFilter.Contains("OTHER UB"))
                {
                    //strWhere = string.Format("where status = '{0}' and (NOT plan_nme in ('MEDICARE','CIGNA'))", strType);
                    strWhere = string.Format("where status = '{0}' and (NOT ins_code in ('MC','CIGNA'))", strType);
                }
                if (m_strFilter.Contains("OUTPATIENT"))
                {
                    strWhere = string.Format("where status = '{0}'", strType);
                }
                if (m_strFilter.Contains("CHAMPUS"))
                {
                    // strWhere = "where status = '1500' and plan_nme like 'CHAMPUS%'";
                    strWhere = "where status = '1500' and ins_code like 'CHAMPUS%'";
                }
                if (m_strFilter.Contains("1500"))
                {
                    //strWhere = string.Format("where status = '{0}' and plan_nme not in ('CHAMPUS')", strType);
                    strWhere = string.Format("where status = '{0}' and ins_code not in ('CHAMPUS')", strType);
                }

                cmdSelectAcc =
                   new SqlCommand(
                       string.Format(
                       " select  status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme " +
                       " from acc " +
                       " inner join ins on ins.account = acc.account and ins_a_b_c = 'a' " +
                       " {0} ", strWhere)
                       , connection);

                m_daAcc.SelectCommand = cmdSelectAcc;
                int nRec = m_daAcc.Fill(m_dsBilling.Tables["ACC"]);

                if (m_dsBilling.Tables["ACC"].Rows.Count == 0)
                {
                    m_Err.m_Logfile.WriteLogFile(
                        string.Format("No account records ready for {0}'s", strType));
                    MessageBox.Show(string.Format("No account records ready for {0}'s", strType));
                    return;
                }

                strFilterAcc = "(";
                for (int i = 0; i < m_dsBilling.Tables["ACC"].Rows.Count; i++)
                {
                    strFilterAcc += string.Format("'{0}',", m_dsBilling.Tables["ACC"].Rows[i]["ACCOUNT"].ToString());
                }
                strFilterAcc = strFilterAcc.Remove(strFilterAcc.Length - 1);
                strFilterAcc += ")";
                //if (DateTime.Now < new DateTime(2012, 02, 21))
                //{
                //    strFilterAcc
                //        = "('C3499221')";
                //          //  = "('C3473712','C3458575')";
                //         // = "('c3456940','C3419888')";
                //}
                SqlCommand cmdSelectPat =
                    new SqlCommand(
                        string.Format(
                        "SELECT  pat.account, acc.pat_name,  pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation " +
                            ", guarantor, guar_addr, g_city_st, pat_marital " +
                            ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
                            ", guar_phone, acc.trans_date,  phy_id, phy.last_name, phy.first_name, phy.mid_init" +
                            ", icd_indicator " + // wdk 20150601
                            " FROM     pat " +
                            " inner join acc on acc.account = pat.account  " +
                            " left outer join phy on phy.tnh_num = pat.phy_id and phy.deleted = 0 " +
                            " where pat.account in {0}", strFilterAcc),
                             connection);
                m_daPat.SelectCommand = cmdSelectPat;
                nRec = m_daPat.Fill(m_dsBilling.Tables["PAT"]);

                SqlCommand cmdSelectIns =
                    new SqlCommand(
                        string.Format("SELECT ins.account, ins_a_b_c, holder_nme, " +
                        " case when pat.relation = '01' " +
                        " then isnull(holder_dob, pat.dob_yyyy) " +
                        " else holder_dob end as [holder_dob], " +
                        " insc.[name] as [plan_nme], insc.addr1 as [plan_addr1], insc.citystzip as [plan_csz], " +
                        " policy_num, cert_ssn, grp_nme, grp_num, " +
                        " case when pat.relation = '01' then coalesce(holder_sex,pat.sex)" +
                        " else holder_sex end as [holder_sex], ins_code, " +
                        " case when nullif(ins.relation,'') is not null " +
                        " then ins.relation else pat.relation end as [relation] " +
                        " , isnull(guar_addr,pat_addr1) as [Addr], isnull(g_city_st,city_st_zip) as [City_st_zip] " +
                        " , insc.provider_no_qualifier, insc.provider_no, insc.payer_no, acc.trans_date, insc.addr1 as [insc addr1], insc.addr2 as [insc addr2], " +
                        " insc.citystzip as [insc csz] " +
                        " FROM  ins " +
                        " inner join acc on acc.account = ins.account " +
                        " INNER JOIN PAT on pat.account = ins.account" +
                        //" INNER JOIN INSC on insc.name = ins.plan_nme  or (insc.code = ins.ins_code and insc.citystzip is not null)" + // wdk 20120930 fix for C3653897 which has  "A" insurance records???!!!!!
                        " INNER JOIN INSC on insc.deleted = 0 and (insc.name = ins.plan_nme  or insc.code = ins.ins_code)" + // wdk 20120930 fix for C3653897 which has  "A" insurance records???!!!!!
                        " where  ins.ins_a_b_c = 'A' and ins.deleted = 0  AND insc.deleted = 0 " + // wdk 20120803 removed to try secondary billing [ins.ins_a_b_c = 'A' and]
                        " and ins.account in {0} " +
                        " and isnull(ins.relation,pat.relation ) is not null " +
                        " AND CASE WHEN insc.code = ins.ins_code THEN 'yes' ELSE 'no' END  = 'yes'" +
                        " order by ins.account, ins_a_b_c", strFilterAcc), connection);
                m_daIns.SelectCommand = cmdSelectIns;
                nRec = m_daIns.Fill(m_dsBilling.Tables["INS"]);


                SqlCommand cmdSelectChrg =
                    new SqlCommand(
                        string.Format("SELECT  chrg.account, chrg_num, cdm, qty, net_amt, trans_date " +
                        " FROM chrg " +
                        " inner join acc on acc.account = chrg.account  " +
                        " where chrg.account in {0}" +
                        " and chrg.credited = 0" // wdk 20130322 added to removed quest billed charges from the file.
                        , strFilterAcc)
                        , connection);
                m_daChrg.SelectCommand = cmdSelectChrg;
                nRec = m_daChrg.Fill(m_dsBilling.Tables["CHRG"]);

                string strFilterChrg = "(";
                foreach (DataRow dr in m_dsBilling.Tables["CHRG"].Rows)
                {
                    strFilterChrg += string.Format("'{0}',", dr["chrg_num"].ToString());
                }
                strFilterChrg = strFilterChrg.Remove(strFilterChrg.LastIndexOf(','));
                strFilterChrg += ")";


                SqlCommand cmdSelectAmt = null;
                cmdSelectAmt = new SqlCommand(string.Format(
                    " with cte as ( " +
                    " select chrg.account, amt.chrg_num, chrg.qty, chrg.cdm, " +
                     "cpt4, type, amount " +
                     ", case when modi <> '' " +
                " then modi " +
                " else " +
                " case when lmrp is null " +
                " then null else " +
                " case when lmrp = 0 " +
                " then 'GA' else 'GZ' end " +
                " end end as modi, " +
                     " revcode, modi2, diagnosis_code_ptr, acc.trans_date" +
                     " from amt " +
                     " inner join chrg on chrg.chrg_num = amt.chrg_num " +
                     " inner join acc on acc.account = chrg.account " +
                     " left outer join abn on abn.account = chrg.account and abn.cdm = chrg.cdm " +
                     "  WHERE     (chrg.credited = 0) " + // wdk 20140806 AND (fin_type = 'M') " +
                     " and amt.chrg_num in {0})" +
                " select account,		sum(qty) as [qty],	cpt4,	type,	" +
                " sum(qty*amount) as [amount],	modi,	revcode,	modi2,	diagnosis_code_ptr,	trans_date " +
                " from cte " +
                " group by account, cpt4,type, modi,revcode,	modi2,	diagnosis_code_ptr,	trans_date " +
                " having sum(qty*amount) > 0 and sum(qty) > 0 " +
                " order by cpt4"
                     , strFilterChrg)
                     , connection);
                m_daAmt.SelectCommand = cmdSelectAmt;
                nRec = m_daAmt.Fill(m_dsBilling.Tables["AMT"]);

            }
            m_dsBilling.Tables["ACC"].PrimaryKey =
                new DataColumn[] { m_dsBilling.Tables["ACC"].Columns["ACCOUNT"] };

            try
            {
                m_dsBilling.Tables["PAT"].PrimaryKey =
                    new DataColumn[] { m_dsBilling.Tables["PAT"].Columns["ACCOUNT"] };
            }
            catch (DataException de)
            {
                m_Err.m_Logfile.WriteLogFile(de.GetType().ToString());
                m_Err.m_Logfile.WriteLogFile(de.Message);
                MessageBox.Show(string.Format("Call LIS. Error written to log file. Cannot continue.\r\nAccount {0}"
                    , de.Data.Values.ToString())
                     , string.Format("{0} - PAT PRIMARY KEY - {1}", propAppName, de.GetType().ToString()));
                Environment.Exit(13);
            }
            catch (ArgumentException ae)
            {
                //dgvRecords.DataSource = m_dsBilling.Tables["PAT"];
                m_Err.m_Logfile.WriteLogFile(ae.GetType().ToString());
                m_Err.m_Logfile.WriteLogFile(ae.Message);
                MessageBox.Show(string.Format("Call LIS. Error written to log file. Cannot continue.\r\nAccount {0}"
                    , ae.ParamName)
                     , string.Format("{0} - PAT PRIMARY KEY - {1}", propAppName, ae.GetType().ToString()));
                Environment.Exit(13);
            }
            catch (Exception ex)
            {
                m_Err.m_Logfile.WriteLogFile(ex.GetType().ToString());
                m_Err.m_Logfile.WriteLogFile(ex.Message);
                MessageBox.Show(string.Format("Call LIS. Error written to log file. Cannot continue.")
                     , string.Format("{0} - PAT PRIMARY KEY - {1}", propAppName, ex.GetType().ToString()));
                Environment.Exit(13);
            }
            m_dsBilling.Tables["INS"].PrimaryKey =
                new DataColumn[] { m_dsBilling.Tables["INS"].Columns["ACCOUNT"],
                                    m_dsBilling.Tables["INS"].Columns["INS_A_B_C"] };

            m_dsBilling.Tables["CHRG"].PrimaryKey =
                new DataColumn[] { m_dsBilling.Tables["CHRG"].Columns["ACCOUNT"],
                                    m_dsBilling.Tables["CHRG"].Columns["CHRG_NUM"] };


            if (!m_dsBilling.Relations.Contains("AccPat"))
            {
                DataRelation darelAccPat = m_dsBilling.Relations.Add(
                    "AccPat", m_dsBilling.Tables["ACC"].Columns["ACCOUNT"],
                    m_dsBilling.Tables["PAT"].Columns["ACCOUNT"]);
            }

            if (!m_dsBilling.Relations.Contains("AccIns"))
            {
                DataRelation darelAccIns = m_dsBilling.Relations.Add(
                    "AccIns", m_dsBilling.Tables["ACC"].Columns["ACCOUNT"],
                    m_dsBilling.Tables["INS"].Columns["ACCOUNT"]);
            }
            if (!m_dsBilling.Relations.Contains("AccChrg"))
            {
                DataRelation darelAccChrg = m_dsBilling.Relations.Add(
                    "AccChrg", m_dsBilling.Tables["ACC"].Columns["ACCOUNT"],
                    m_dsBilling.Tables["CHRG"].Columns["ACCOUNT"]);
            }

            if (!m_dsBilling.Relations.Contains("AccAmt"))
            {
                DataRelation darelAccAmt = m_dsBilling.Relations.Add(
                    "AccAmt", m_dsBilling.Tables["ACC"].Columns["ACCOUNT"],
                    m_dsBilling.Tables["AMT"].Columns["ACCOUNT"]);
            }

            dgvRecords.DataSource = m_dsBilling.Tables["ACC"];// dsRecords.Tables[0];

            tsslRecCount.Text = dgvRecords.Rows.Count.ToString();
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                dr.HeaderCell.ToolTipText = dr.HeaderCell.RowIndex.ToString();
                using (SqlConnection connection = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    SqlCommand cmdSelect = new SqlCommand(
                        string.Format("select chrg.account, cpt4 " +
                        "from chrg " +
                        "inner join amt on amt.chrg_num = chrg.chrg_num " +
                        "inner join dict_quest_exclusions_final_draft AS dd on dd.cpt = amt.cpt4 " +
                        "where chrg.account = '{0}' " +
                        "and credited = 0 and coalesce(invoice,'') = ''  and chrg.cdm <> 'CBILL'", dr.Cells["account"].Value.ToString())
                        , connection);
                    sda.SelectCommand = cmdSelect;
                    int nRec = sda.Fill(dt);
                    if (nRec == -1)
                    {
                        dr.ErrorText = "CPT not billable";
                    }
                }

            }
            dgvRecords.Invalidate();
            //PerformQuestBilling(strFilterAcc); 
            if (DateTime.Now > new DateTime(2015, 09, 08, 14, 00, 00))
            {

                DataRow[] drAr = m_dsBilling.Tables["ACC"].Select("pat_name like 'ZZTEST%'");
                if (drAr.Count() > 0)
                {
                    btnStart.Enabled = false;
                    MessageBox.Show("You must remove the patient ZZTEST from the billing file.");
                    dgvRecords.Sort(dgvRecords.Columns["pat_name"], ListSortDirection.Descending);
                    Application.DoEvents();
                }
            }

            if (strType == "1500")
            {
                foreach (DataRow drSelectedAcc in m_dsBilling.Tables["ACC"].Rows)
                {
                    Application.DoEvents();
                    DataRow[] drAmt = drSelectedAcc.GetChildRows("AccAmt");
                    for (int i = 0; i <= drAmt.GetUpperBound(0); i++)
                    {
                        string strCodePtr = drAmt[i]["diagnosis_code_ptr"].ToString();
                        if (string.IsNullOrEmpty(strCodePtr))
                        {
                            //  m_bHas1500Error = true;
                            //   MessageBox.Show(string.Format("Account {0} has a null code pointer. THIS FILE WILL NOT BE MOVED TO THE UPLOAD DIRECTORY.", drAmt[i][0]), propAppName);
                            drSelectedAcc.RowError = "Null Diagnosis Code Pointer";
                        }
                    }
                }
            }
            Application.DoEvents();

        }

        private void PerformQuestBilling(string strFilterAcc)
        {
            using (SqlConnection connection = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlCommand cmdSelectIns =
            new SqlCommand(
                string.Format("SELECT distinct ins.account, ins_a_b_c, holder_nme, " +
                " case when pat.relation = '01' " +
                " then isnull(holder_dob, pat.dob_yyyy) " +
                " else holder_dob " +
                " end as [holder_dob], " +
                " [name] as [plan_nme], insc.addr1 as [plan_addr1], insc.citystzip as [plan_csz], " +
                " policy_num, cert_ssn, grp_nme, grp_num, " +
                " case when pat.relation = '01' then " +
                " case when holder_sex = '' or holder_sex is null" +
                " then pat.sex " +
                " else holder_sex " +
                " end else holder_sex " +
                " end as [holder_sex], ins_code, " +
                " case when ins.relation is not null and ins.relation <> '' " +
                " then ins.relation " +
                " else pat.relation end as [relation] " +
                " , isnull(guar_addr,pat_addr1) as [Addr], isnull(g_city_st,city_st_zip) as [City_st_zip] " +
                " , insc.provider_no_qualifier, insc.provider_no, insc.payer_no, acc.trans_date, insc.addr1 as [insc addr1], insc.addr2 as [insc addr2], " +
                " insc.citystzip as [insc csz] " +
                " FROM  ins " +
                " inner join acc on acc.account = ins.account " +
                " INNER JOIN PAT on pat.account = ins.account" +
                //" INNER JOIN INSC on insc.name = ins.plan_nme or insc.code = ins.ins_code " +
                " INNER JOIN INSC on insc.name = ins.plan_nme  or (insc.code = ins.ins_code and insc.citystzip is not null)" + // wdk 20120930 fix for C3653897 which has  "A" insurance records???!!!!!
                " inner join chrg on chrg.account = ins.account " +
                "		and chrg.cdm in (select cdm from dict_outreach_supplies where deleted = 0 and performed_by = 'QUEST') " +
                " where  ins.ins_a_b_c in ('B','C','D') and ins.deleted = 0  AND insc.deleted = 0 " + // wdk 20120803 removed to try secondary billing [ins.ins_a_b_c = 'A' and]
                " and ins.ins_code = 'TNBC' " +
                " and ins.account in {0} " +
                //  " and isnull(ins.relation,pat.relation ) is not null " +
                " order by ins.account, ins_a_b_c", strFilterAcc), connection);
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmdSelectIns;
                DataTable dtInsB = new DataTable();
                int nRec = sda.Fill(dtInsB);

                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    DataRow[] dra = dtInsB.Select(string.Format("account = '{0}'",
                        dr.Cells["account"].Value.ToString()));
                    if (dra.GetUpperBound(0) >= 0)
                    {
                        //         MessageBox.Show(dr.Cells["account"].Value.ToString());
                    }
                }
            }
        }


        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            //  fix me
            ///  double strNum = m_rNumber.GetNumber("ssi_batch");
            // / m_strInterchageControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}",
            //  /    DateTime.Now.Year, strNum)));

            btnStart.Enabled = false;
            string strBillingType;
            if (m_dsBilling.Tables["ACC"].Rows.Count < 1)
            {
                m_Err.m_Logfile.WriteLogFile("No records to process.");
                MessageBox.Show("Must have records to process.");
                return;
            }
            m_dsBilling.AcceptChanges();//.Tables["ACC"].AcceptChanges();
            strBillingType = m_dsBilling.Tables["ACC"].Rows[0]["status"].ToString().ToUpper();

            if (strBillingType == "UBOP")
            {
                _CreateOutPatient();
            }
            if (strBillingType == "UB")
            {
                _CreateUB(); //837i
            }
            if (strBillingType == "1500")
            {
                _Create1500(); //837p
            }
            this.TopMost = true;

            this.Select();
            MessageBox.Show(string.Format("{0} file created.", strBillingType));
            tsbPrintView_Click(null, null);

            DataGridView dgvPrinta = new DataGridView();
            foreach (DataGridViewColumn dc in dgvRecords.Columns)
            {
                dgvPrinta.Columns.Add(dc.Name, dc.Name);
            }

            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                if (string.IsNullOrEmpty(dr.ErrorText))
                {
                    continue;
                }
                int nSert = dgvPrinta.Rows.Add(new object[]
                    {
                        dr.Cells[0].Value.ToString(),
                        dr.Cells[1].Value.ToString(),
                        dr.Cells[2].Value.ToString(),
                        dr.Cells[3].Value.ToString(),
                        dr.Cells[4].Value.ToString(),
                        dr.Cells[5].Value.ToString(),
                        dr.Cells[6].Value.ToString(),
                        dr.Cells[7].Value.ToString()
                    }
                );
                dgvPrinta.Rows[nSert].ErrorText = dr.ErrorText;
            }
            if (dgvPrinta.Rows.Count > 1)
            {
                tsbPrintGrid_Click(dgvPrinta, null);
            }

            this.TopMost = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _CreateOutPatient()
        {
            tsslBatch.Text = "";
            rtbDoc.Clear();
            m_alFile.Clear();
            m_sbUBHeader = new StringBuilder();
            CreateOP_Envelope();
            CreateOP_Header();
            CreateOP_SubmitterName();
            CreateOP_ReceiverName();
            CreateOP_ProviderHL();
            CreateOP_BillingProviderName();
            #region Removed Code
            //try
            //{
            //    DataRow[] drIns = dr.GetChildRows("AccIns");
            //    if (drIns[0]["PLAN_NME"].ToString() == "BLUE CROSS")
            //    {
            //        CreateBlueCrossRef();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // handled below 
            //}
            #endregion
            tsbpCreate.Value = 0;
            tsbpCreate.Maximum = m_dsBilling.Tables["ACC"].Rows.Count;
            // process each account
            foreach (DataRow dr in m_dsBilling.Tables["ACC"].Rows)
            {
                m_sbUB = new StringBuilder();
                //  m_sbUB = m_sbUBHeader;
                m_sbUB.Insert(0, m_sbUBHeader);
                dgvRecords.Refresh();
                Application.DoEvents();
                tsbpCreate.PerformStep();
                DataRow[] drPat = dr.GetChildRows("AccPat");
                DataRow[] drIns = dr.GetChildRows("AccIns");
                DataRow[] drChrg = dr.GetChildRows("AccChrg");
                DataRow[] drAmt = dr.GetChildRows("AccAmt");

                if (drPat.GetUpperBound(0) == -1 ||
                        drIns.GetUpperBound(0) == -1 ||
                        drChrg.GetUpperBound(0) == -1 ||
                        drAmt.GetUpperBound(0) == -1)
                {
                    string strError = string.Format("Account {0} has error\r\n{1}{2}{3}{4}  ",
                        dr["Account"].ToString(),
                        drPat.GetUpperBound(0) == -1 ? "No Patient record\r\n" : "",
                        drIns.GetUpperBound(0) == -1 ? "No Insurance record\r\n" : "",
                        drChrg.GetUpperBound(0) == -1 ? "No Charge record\r\n" : "",
                        drAmt.GetUpperBound(0) == -1 ? "No Amount Record\r\n" : "");
                    dr.RowError = strError;
                    m_Err.m_Logfile.WriteLogFile(strError);
                    Application.DoEvents();
                    continue;
                }
                while (dr != null)
                {
                    // need to clear the rows here for the m_sbUB.AppendFormat("{0}\r\n", for previous person
                    CreateOP_SubscriberHL();

                    // SUBSCRIBER HIERARCHICAL LEVEL LOOP ID 2000B
                    // wdk 20120120 per discussion with carol only send primary insurance for UB's
                    CreateOP_SubscriberInfo(drIns[0]);
                    if (drIns[0]["relation"].ToString() != "01")
                    {
                        CreateOP_PatInfo(drPat[0], drIns[0]["policy_num"].ToString());
                    }
                    #region Removed Code
                    //for (int i = 0; i <= drIns.GetUpperBound(0); i++) // wdk 20120120 per discussion with carol
                    //{
                    //    CreateUB_5010_SubscriberInfo(drIns[i]);
                    //    if (drIns[i]["relation"].ToString() != "01")
                    //    {
                    //        CreateUB_5010_PatInfo(drPat[0], drIns[i]["policy_num"].ToString());
                    //    }
                    //}
                    #endregion
                    // end of 20120120 changes
                    try
                    {
                        CreateOP_ClaimHeader(drChrg[0]);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(string.Format("Account {0} has no charge record", dr["account"].ToString()));
                        return;
                    }

                    CreateOP_DiagnosisInfo(drPat[0]);

                    CreateOP_ClaimInfo(drPat[0]);

                    CreateOP_ServiceLine(drAmt);
                    break;
                }
                if (string.IsNullOrEmpty(dr.RowError))
                {
                    if (DateTime.Now < new DateTime(2013, 08, 01, 14, 00, 00))
                    {
                        return;
                    }
                    m_rAccUpdate.GetRecords(string.Format("Account = '{0}'", dr["ACCOUNT"]));
                    m_rAccUpdate.m_strStatus = "SSIUBOP";
                    m_rAccUpdate.m_strPatName = RFCObject.staticSqlClean(m_rAccUpdate.m_strPatName);
                    int nRecUpdate = m_rAccUpdate.Update();
                    if (nRecUpdate > 0)
                    {
                        m_rPat.ClearMemberVariables();
                        string strWhere = string.Format("account = '{0}'", m_rAccUpdate.m_strAccount);
                        string strErr;
                        int nPatRec = m_rPat.GetActiveRecords(strWhere);

                        if (nPatRec == 1)
                        {
                            nPatRec = m_rPat.UpdateField("ub_date", DateTime.Today.ToShortDateString(), strWhere, out strErr);
                            if (nPatRec != 1)
                            {
                                m_Err.m_Logfile.WriteLogFile(strErr);
                            }
                            nPatRec = m_rPat.UpdateField("ssi_batch", m_strInterchageControlNumber, strWhere, out strErr);
                            if (nPatRec != 1)
                            {
                                m_Err.m_Logfile.WriteLogFile(strErr);
                            }
                        }

                    }
                }
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlCommand cmdInsert = new SqlCommand(
                        string.Format("insert into data_billing_history " +
                        "(account, ins_abc, pat_name, fin_code, ins_code, " +
                        "trans_date, run_date, batch, ebill_status, ebill_batch, " +
                        "text, mod_date, mod_user, mod_prg, mod_host) " +
                        "VALUES " +
                        " ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', " +
                        "'{6}', '{7}', '{8}', '{9}', '{10}', " +
                        "'{11}',suser_sname(),'{12}',host_name() ) ",
                        dr["account"].ToString()
                            , drIns[0]["ins_a_b_c"].ToString()
                                , dr["pat_name"].ToString()
                                    , dr["fin_code"].ToString()
                                        , drIns[0]["ins_code"].ToString()
                        , dr["trans_date"].ToString()
                            , DateTime.Today,
                                m_strInterchageControlNumber,
                                    m_strFilter
                                        , m_strInterchageControlNumber
                        , m_sbUB.ToString()
                            , DateTime.Now
                                , propAppName
                        )
                        , conn);
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.InsertCommand = cmdInsert;
                    sda.InsertCommand.Connection.Open();
                    try
                    {
                        sda.InsertCommand.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // todo needs handler to print error added carol to the permissions on this table to prevent
                    }
                    finally
                    {
                        sda.InsertCommand.Connection.Close();
                    }

                }
            }


            CreateOP_SE();

            CreateOP_GE();

            CreateOP_IEA();

            string strFile = string.Format(@"C:\temp\UBOP\UBOP_{0}.x12", DateTime.Now.ToString("yyyyMMddHHmm"));
            StreamWriter sw = new StreamWriter(strFile);
            sw.AutoFlush = true;
            foreach (string str in m_alFile)
            {
                sw.Write(str);
            }
            sw.Close();

            if (m_strDatabase.Contains("LIVE"))
            {
                string strFile2 = string.Format(@"\\wth251\ssiapp\ssi\00852\billing\uploadh\mcl\UBOP_{0}.in", DateTime.Now.ToString("yyyyMMddHHmm"));
                StreamWriter sw2 = new StreamWriter(strFile2);
                sw2.AutoFlush = true;

                foreach (string str in m_alFile)
                {
                    sw2.Write(str);
                }

                sw2.Close();
            }

            // wdk 20130829
            m_nHLCounter = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateBlueCrossRef()
        {

            string strRefIB = "REF*1B*1000427~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefIB;
            m_alFile.Add(strRefIB);
            if (m_strFilter == "1500")
            {
                m_sb1500Header.AppendFormat("{0}\r\n", strRefIB);
            }
            else
            {
                m_sbUBHeader.AppendFormat("{0}\r\n", strRefIB);
            }
            m_nSTSegments++;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateOP_IEA()
        {
            m_strIEA = string.Format("IEA*{0}*{1}~",
             m_nFunctionalGroups,
                 m_strInterchageControlNumber);

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strIEA;
            m_alFile.Add(m_strIEA);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateOP_GE()
        {
            m_strGE = string.Format("GE*{0}*{1}~",
                  m_nFunctionalGroups,
                      m_strInterchageControlNumber);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strGE;
            m_alFile.Add(m_strGE);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateOP_SE()
        {
            m_strSE = string.Format("SE*837*{0}*{1}*CH~",
             m_nSTSegments,
                 DateTime.Now.ToString("yyyyMMdd"),
                     DateTime.Now.ToString("HHmm").ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strSE;
            m_alFile.Add(m_strSE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drAmt"></param>
        private void CreateOP_ServiceLine(DataRow[] drAmt)
        {
            int nLX = 1;

            for (int i = 0; i <= drAmt.GetUpperBound(0); i++)
            {
                string strDesc = null;
                if (m_alCpt4DescriptionRequired.Contains(drAmt[i]["cpt4"]))
                {
                    DataRow[] drWarning = m_dtCpt4Desc.Select(string.Format(" cpt4 = '{0}'", drAmt[i]["cpt4"]));
                    if (drWarning.GetUpperBound(0) > -1)
                    {
                        strDesc = drWarning[0]["line desc"].ToString();
                    }
                    //if (string.IsNullOrEmpty(strDesc))
                    //{
                    //    strDesc = "SSI has a warning on this CPT4";
                    //}
                }
                ///////////////
                string strLX = string.Format("LX*{0}~", nLX++);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strLX;
                m_alFile.Add(strLX);
                m_sbUB.AppendFormat("{0}\r\n", strLX);
                m_nSTSegments++;
                // rgc/wdk 20120403 removed one of the sub field indicators in SV2-02 for ssi import
                string strSRV = string.Format("SV2*{0}*HC:{1}:{2}:{3}:::{4}*{5}*UN*{6}**{7}~",
                    drAmt[i]["revcode"].ToString(),
                    drAmt[i]["cpt4"],
                    drAmt[i]["modi"].ToString().Trim(),
                    drAmt[i]["modi2"].ToString().Trim(),
                    //string.IsNullOrEmpty(drAmt[i]["modi"].ToString().Trim()) ? "" : string.Format(":{0}", drAmt[i]["modi"]),
                    //string.IsNullOrEmpty(drAmt[i]["modi2"].ToString().Trim()) ? "" : string.Format(":{0}", drAmt[i]["modi2"]),
                    strDesc,
                    double.Parse(drAmt[i]["amount"].ToString()).ToString("F2"),
                    drAmt[i]["qty"],
                    drAmt[i]["modi"].ToString().Trim() == "GZ" ? double.Parse(drAmt[i]["amount"].ToString()).ToString("F2") : ""

                   );

                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strSRV;
                m_alFile.Add(strSRV);
                m_sbUB.AppendFormat("{0}\r\n", strSRV);
                m_nSTSegments++;

                // dRetVal += double.Parse(drAmt[i]["amount"].ToString());

                DateTime dtTransDate = DateTime.Parse(drAmt[i]["trans_date"].ToString());
                string strTransDate = dtTransDate.ToString("yyyyMMdd");
                string strCLMDate = string.Format("DTP*472*D8*{0}~",
                  strTransDate);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strCLMDate;
                m_alFile.Add(strCLMDate);
                m_sbUB.AppendFormat("{0}\r\n", strCLMDate);
                m_nSTSegments++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        private void CreateOP_ClaimInfo(DataRow dataRow)
        {
            // rgc/wdk 20110825 changed from Referring Provider to  Attending Provider. 
            // documentation says use Referring if the two are not the same. Referring is DN not 71
            string strClaimAttendingProviderNM1 = string.Format("NM1*71*1*{0}*{1}*{2}***XX*{3}~",
                    dataRow["last_name"].ToString(),
                    dataRow["first_name"].ToString(),
                    dataRow["mid_init"].ToString(),
                    dataRow["phy_id"].ToString());

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimAttendingProviderNM1;
            m_alFile.Add(strClaimAttendingProviderNM1);
            m_sbUB.AppendFormat("{0}\r\n", strClaimAttendingProviderNM1);
            m_nSTSegments++;
            #region Removed Code
            //string strClaimAttendingProviderNM1 = @"NM1*LI*2*MEDICAL CENTER LABORATORY*****XX*1720160708~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimAttendingProviderNM1;
            //m_alFile.Add(strClaimAttendingProviderNM1);
            //m_sbUB.AppendFormat("{0}\r\n",strClaimRenderingProviderRef);
            //m_nSTSegments++;

            //string strClaimRenderingProviderN3 = "N3*620 SKYLINE DRIVE~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderN3;
            //m_alFile.Add(strClaimRenderingProviderN3);
            //m_sbUB.AppendFormat("{0}\r\n",strClaimRenderingProviderRef);
            //m_nSTSegments++;

            //string strClaimRenderingProviderN4 = "N4*JACKSON*TN*383010000~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderN4;
            //m_alFile.Add(strClaimRenderingProviderN4);
            //m_sbUB.AppendFormat("{0}\r\n",strClaimRenderingProviderRef);
            //m_nSTSegments++;

            //string strClaimRenderingProviderRef = "1720160708~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderRef;
            //m_alFile.Add(strClaimRenderingProviderRef);
            //m_sbUB.AppendFormat("{0}\r\n",strClaimRenderingProviderRef);
            //m_nSTSegments++;
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drPAT"></param>
        private void CreateOP_DiagnosisInfo(DataRow drPAT)
        {
            string strIndicator = "BK";
            string strIndicator2 = "BF";
            if (drPAT["icd_indicator"].ToString() == "I10")
            {
                strIndicator = "ABK";
                strIndicator2 = "ABF";
            }

            // Primary Diagnosis this visit
            // HI:BK = icd9
            // HI:ABK = icd10;
            string strHI = string.Format("HI*{1}:{0}~",//{1}{2}{3}~",
               drPAT["icd9_1"].ToString().Replace(".", ""), strIndicator//,
                                                                        //string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()) ? "" : string.Format("*BF:{0}", drPAT["icd9_2"].ToString().Replace(".", "")),
                                                                        //string.IsNullOrEmpty(drPAT["icd9_3"].ToString().Trim()) ? "" : string.Format("*BF:{0}", drPAT["icd9_3"].ToString().Replace(".", "")),
                                                                        //string.IsNullOrEmpty(drPAT["icd9_4"].ToString().Trim()) ? "" : string.Format("*BF:{0}", drPAT["icd9_4"].ToString().Replace(".", ""))
               );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHI;
            m_alFile.Add(strHI);
            m_sbUB.AppendFormat("{0}\r\n", strHI);
            m_nSTSegments++;

            // must have a second diagnois to include this segment
            if (!string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()))
            {
                // HI:BF = icd9
                // HI:ABF = icd10;
                string strHISecondary = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}~",
                    string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()) ? "" : string.Format("HI*{1}:{0}", drPAT["icd9_2"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_3"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_3"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_4"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_4"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_5"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_5"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_6"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_6"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_7"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_7"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_8"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_8"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_9"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_9"].ToString().Replace(".", ""), strIndicator2)
                   );
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strHISecondary;
                m_alFile.Add(strHISecondary);
                m_sbUB.AppendFormat("{0}\r\n", strHISecondary);
                m_nSTSegments++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        private void CreateOP_ClaimHeader(DataRow dr)
        {
            string strAccount = dr["account"].ToString();
            DataTable dttotal = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = new SqlCommand(string.Format("select sum(qty*net_amt) as [Total Chrg] " +
                " from chrg " +
                " where account = '{0}'", strAccount), m_sqlConnection);
            sda.Fill(dttotal);
            // string strCLM = string.Format("CLM*{0}*{1:F2}***14:A:1*Y*A*Y*A*P~", rgc/wdk 20110824 removed the [:F2] from the amount 
            // from x223.pdf         CLM*756048Q*89.95***13:A:1*Y*C*Y*Y~
            string strCLM = string.Format("CLM*{0}*{1}***14:A:1**C*Y*Y~",
                  strAccount,
                  double.Parse(dttotal.Rows[0]["Total Chrg"].ToString()).ToString("F2"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strCLM;
            m_alFile.Add(strCLM);
            m_sbUB.AppendFormat("{0}\r\n", strCLM);
            m_nSTSegments++;


            sda.SelectCommand = new SqlCommand(string.Format("select trans_date" +
                " from acc " +
                " where account = '{0}'", strAccount), m_sqlConnection);
            DataTable dtTransDate = new DataTable();
            sda.Fill(dtTransDate);
            DateTime dtTDate = DateTime.Parse(dtTransDate.Rows[0]["trans_date"].ToString());
            string strDTP = string.Format("DTP*434*RD8*{0}-{0}~", dtTDate.ToString("yyyyMMdd"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strDTP;
            m_alFile.Add(strDTP);
            m_sbUB.AppendFormat("{0}\r\n", strDTP);
            m_nSTSegments++;


            string strCL1 = "CL1*9*1*01~"; // rgc/wdk 20120509 added the admission type code of 9
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strCL1;
            m_alFile.Add(strCL1);
            m_sbUB.AppendFormat("{0}\r\n", strCL1);
            m_nSTSegments++;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drPAT"></param>
        /// <param name="p"></param>
        private void CreateOP_PatInfo(DataRow drPAT, string p)
        {
            string strRelation = drPAT["relation"].ToString();
            if (strRelation == "03")
            {
                strRelation = "19";
            }
            if (strRelation == "02")
            {
                strRelation = "01";
            }
            if (strRelation == "09")
            {
                strRelation = "G8";
            }

            string strPatHL = string.Format("HL*{0}*{1}*{2}*0~",
                m_nHLCounter++, m_nHLParent,
                strRelation);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatHL;
            m_alFile.Add(strPatHL);
            m_sbUB.AppendFormat("{0}\r\n", strPatHL);
            m_nSTSegments++;

            string strPat = string.Format("PAT*{0}*~",
                strRelation);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPat;
            m_alFile.Add(strPat);
            m_sbUB.AppendFormat("{0}\r\n", strPat);
            m_nSTSegments++;

            string strName = drPAT["pat_name"].ToString();
            string strLastName = string.Empty;
            string strFirstName = string.Empty;
            string strMiddleName = string.Empty;
            string strNameSuffix = string.Empty;
            if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(","))
            {
                //strLastName = strName.Substring(0, strName.IndexOf(",")).Trim();
                //strName = strName.Replace(strLastName, "").Trim();
                //strName = strName.Replace(",", "").Trim();

                // rgc/wdk 20120510 modified to handle names like "JAMES,JAMES"
                strLastName = strName.Substring(0, strName.IndexOf(","));
                string strReplace = string.Format("{0},", strLastName).Trim();
                strName = strName.Replace(strReplace, "").Trim();
                //strName = strName.Replace(",", "").Trim();
                if (strLastName.Contains(" "))
                {
                    string[] strNameSplit = strLastName.Split(new string[] { " " }, StringSplitOptions.None);
                    strLastName = strNameSplit[0].Trim();
                    strNameSuffix = strNameSplit[1].Trim();
                }
                strFirstName = strName;
                if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(" "))
                {
                    strFirstName = strName.Substring(0, strName.IndexOf(" ")).Trim();
                    strMiddleName = strName.Replace(strFirstName, "").Trim();
                }
            }

            string strPatName = string.Format("NM1*QC*1*{0}*{1}*{2}**{3}~",//*MI*{4}~",
               strLastName, strFirstName, strMiddleName, strNameSuffix);
            //, strPolicyIdentifier);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatName;
            m_alFile.Add(strPatName);
            m_sbUB.AppendFormat("{0}\r\n", strPatName);
            m_nSTSegments++;

            string strPatN3 = string.Format("N3*{0}~",
                drPAT["pat_addr1"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatN3;
            m_alFile.Add(strPatN3);
            m_sbUB.AppendFormat("{0}\r\n", strPatN3);
            m_nSTSegments++;

            string[] strNoFail = new string[] { "", "", "", "" };
            string[] strCSZ = drPAT["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            strCSZ.CopyTo(strNoFail, 0);

            string strPatN4 = string.Format("N4*{0}*{1}*{2}~",
                strNoFail[0], strNoFail[1], strNoFail[2]);
            if (!string.IsNullOrEmpty(strNoFail[3]))
            {
                strPatN4 = string.Format("N4*{0}*{1}*{2}~",
                string.Format("{0} {1}", strNoFail[0], strNoFail[1]).Trim(), strNoFail[2], strNoFail[3].Replace("-", ""));
            }
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatN4;
            m_alFile.Add(strPatN4);
            m_sbUB.AppendFormat("{0}\r\n", strPatN4);
            m_nSTSegments++;

            // DateTime dtTransDate = DateTime.Parse(drPAT["trans_date"].ToString());
            DateTime dtPatDob = DateTime.Parse(drPAT["dob_yyyy"].ToString());
            string strPatDMG = string.Format("DMG*D8*{0}*{1}~",
                dtPatDob.ToString("yyyyMMdd"),
                //dtTransDate.ToString("yyyyMMdd"), 
                drPAT["sex"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatDMG;
            m_alFile.Add(strPatDMG);
            m_sbUB.AppendFormat("{0}\r\n", strPatDMG);
            m_nSTSegments++;

            // LOOP 2010CA situational. shouldn't be needed because the secondary info is the same as the primary
            // wdk 20100316 Have no policy number in the pat table throw error.
            //string strREFSec = string.Format("REF*1G*{0}~",
            //    drPAT["policy_num"].ToString());
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strREFSec;
            //m_alFile.Add(strREFSec);

        }

        private void CreateOP_SubscriberInfo(DataRow drINS)
        {
            string strRespSeqNo = "P";
            if (drINS["ins_a_b_c"].ToString() == "B")
            {
                strRespSeqNo = "S";
            }
            if (drINS["ins_a_b_c"].ToString() == "C")
            {
                strRespSeqNo = "T";
            }
            string strRelation = drINS["relation"].ToString().Trim() == "01" ? "18" : "";

            /*Just FYI we are getting a new UB edit this morning:  I have contacted SSI and this is a new 5010 edit.  Julie is going in the bill format and taking out our group name in FL 61
            Email from Debbie Thornhill 02/17/2011
                INSURANCE GROUP NUMBER (PRI) AND INSURANCE GROUP NAME (PRI) CANNOT BOTH BE PRESENT.
                - 2000B SBR03 AND 04 -  **** HIPAA ANSI GENERIC EDIT ****   [VERSION 02/15/2012]
                SOURCE = WPC 837 5010 IMPLEMENTATION GUIDE
            */
            string strGrpNum = drINS["grp_num"].ToString();
            if (!string.IsNullOrEmpty(strGrpNum))
            {
                if (strGrpNum.StartsWith("\0"))
                {
                    strGrpNum = "";
                }
            }
            string strTempGrpNum = strGrpNum;
            // rgc/wdk 20120509 added to covert bluecare name to hospitals table entry
            string strPlanName = drINS["plan_nme"].ToString().ToUpper();
            // wdk 20150831 use ins_code instead of plan name
            string strInsCode = drINS["ins_code"].ToString().ToUpper();

            if (strPlanName == "BLUECARE/TNCARE SEL" || strInsCode == "TNBC")
            {
                strPlanName = "BLUECARE";
            }
            if (strPlanName == "CHAMPUS/TRICARE" || strInsCode == "CHAMPUS")
            {
                strPlanName = "CHAMPUS TRICARE";
            }
            if (strPlanName == "UHC COMMUNITY PLAN" || strInsCode == "UHC")
            {
                strTempGrpNum = "";
            }
            // wdk 20121004 not valid for 131 bill type
            // wdk 20121010 primary payor name must be in SBR or will fail ssi validation and go to skipped claim
            //if (strPlanName == "MEDICARE") // added wdk 20121003 new medicare edit
            //{
            //    strPlanName = "";
            //}

            string strSBR = string.Format("SBR*{0}*{1}*{2}*{3}~",
                strRespSeqNo,
                strRelation,
                strTempGrpNum,//drINS["policy_num"].ToString(),
                              // per 5010 this is not the policy number, that is submitted in the NM109 below.
                              // wdk 20121003 changed dr["plan_name"] to use the adjusted plan name from above
                string.IsNullOrEmpty(strTempGrpNum) ? (strInsCode == "MC" || strPlanName == "MEDICARE") ? "" : strPlanName : "");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strSBR;
            m_alFile.Add(strSBR);
            m_sbUB.AppendFormat("{0}\r\n", strSBR);
            m_nSTSegments++;

            // 2010BA Subscriber Name
            string strName = drINS["holder_nme"].ToString();
            string strLastName = string.Empty;
            string strFirstName = string.Empty;
            string strMiddleName = string.Empty;
            string strNameSuffix = string.Empty;
            if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(","))
            {
                strLastName = strName.Substring(0, strName.IndexOf(","));
                strName = strName.Replace(strLastName, "").Trim();
                strName = strName.Replace(",", "").Trim();
                if (strLastName.Contains(" "))
                {
                    string[] strNameSplit = strLastName.Split(new string[] { " " }, StringSplitOptions.None);
                    strLastName = strNameSplit[0].Trim();
                    strNameSuffix = strNameSplit[1].Trim();
                }
                strFirstName = strName;
                if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(" "))
                {
                    strFirstName = strName.Substring(0, strName.IndexOf(" ")).Trim();
                    strMiddleName = strName.Replace(strFirstName, "").Trim();
                }
            }

            string strPolicyNum = drINS["policy_num"].ToString();
            if (strPlanName == "UHC COMMUNITY PLAN")
            {
                if (!strPolicyNum.StartsWith("JD"))
                {
                    if (strGrpNum.StartsWith("JD"))
                    {
                        strPolicyNum = strGrpNum;
                    }
                }
            }

            string strNM1 = string.Format("NM1*IL*1*{0}*{1}*{2}**{3}*MI*{4}~",
                strLastName, strFirstName, strMiddleName, strNameSuffix,
                strPolicyNum);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sbUB.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;


            string strN3 = string.Format("N3*{0}~",
                drINS["addr"]);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sbUB.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string[] strNoFail = new string[] { "", "", "", "" };
            string strCity = drINS["city_st_zip"].ToString().Split(new char[] { ',' })[0].Trim();
            string strStZip = drINS["city_st_zip"].ToString().Split(new char[] { ',' })[1].Trim();
            strNoFail[0] = strCity;
            string[] strSZ = strStZip.Split(new string[] { " " }, StringSplitOptions.None);
            strNoFail[1] = strSZ[0];
            strNoFail[2] = strSZ[1];
            //string[] strCSZ = drINS["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            //strCSZ.CopyTo(strNoFail, 0);

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                 strNoFail[0], strNoFail[1].Trim(), strNoFail[2].Replace("-", "").Trim()); ;
            if (!string.IsNullOrEmpty(strNoFail[3]))
            {
                strN4 = string.Format("N4*{0}*{1}*{2}~",
                    string.Format("{0} {1}", strNoFail[0], strNoFail[1]).Trim(), strNoFail[2], strNoFail[3].Replace("-", "").Trim());
            }
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sbUB.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;


            string strSubDemoInfo = string.Format("DMG*D8*{0}*{1}~",
                 RFCObject.ConvertDateTimeToHL7Date(drINS["HOLDER_DOB"].ToString()), drINS["HOLDER_SEX"]);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strSubDemoInfo;
            m_alFile.Add(strSubDemoInfo);
            m_sbUB.AppendFormat("{0}\r\n", strSubDemoInfo);
            m_nSTSegments++;

            // LOOP 2010BB

            // SECURE PLUS does not have a contract with the Hospital will have to bill another way!!!!
            //if (strPlanName.ToUpper() == "SECURE PLUS")
            //{
            //    strPlanName = "";
            //}

            string strPayer = drINS["payer_no"].ToString();
            Dictionary<string, string> dicOP = new Dictionary<string, string>();
            dicOP.Add("MC", GetSystemParameter("mcare_prov_id") ?? "440002");
            dicOP.Add("BC", "1000427");
            dicOP.Add("TNBC", "1000427");
            dicOP.Add("UHC", GetSystemParameter("fed_tax_id") ?? "626010402");
            if (dicOP.ContainsKey(drINS["INS_CODE"].ToString()))
            {
                if (!dicOP.TryGetValue(drINS["INS_CODE"].ToString(), out strPayer))
                {
                    strPayer = drINS["payer_no"].ToString();
                }
            }
            // wdk 20121009 changed to blank for medicare
            // wdk 20121011 not the correct place to remove the problem on the UB form
            //if (strPlanName.ToUpper() == "MEDICARE")
            //{
            //    strPlanName = "";
            //}
            string strPayerNM1 = string.Format("NM1*PR*2*{0}*****PI*{1}~",
               strPlanName,
               strPayer);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayerNM1;
            m_alFile.Add(strPayerNM1);
            m_sbUB.AppendFormat("{0}\r\n", strPayerNM1);
            m_nSTSegments++;

            if (!string.IsNullOrEmpty(drINS["plan_addr1"].ToString()))
            {
                string strPayorN3 = string.Format("N3*{0}~",
               drINS["plan_addr1"]);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strPayorN3;
                m_alFile.Add(strPayorN3);
                m_sbUB.AppendFormat("{0}\r\n", strPayorN3);
                m_nSTSegments++;

                string[] strPayorNoFail = new string[] { "", "", "", "" };
                string strCityTest = drINS["plan_csz"].ToString().Substring(0, drINS["plan_csz"].ToString().IndexOf(",")).Trim();
                string strStZipTest =
                    drINS["plan_csz"].ToString().Substring(
                        drINS["plan_csz"].ToString().IndexOf(",") + 1,
                        drINS["plan_csz"].ToString().Length - drINS["plan_csz"].ToString().IndexOf(",") - 1).Trim();
                string[] strPayorCity
                    = drINS["plan_csz"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string[] strPayorStZip
                    = strStZipTest.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                // = drINS["plan_csz"].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                strPayorCity.CopyTo(strPayorNoFail, 0);
                strPayorStZip.CopyTo(strPayorNoFail, 1);

                string strPayorN4 = string.Format("N4*{0}*{1}*{2}~",
                     strPayorNoFail[0], strPayorNoFail[1].Trim(), strPayorNoFail[2].Replace("-", "").Trim()); ;
                if (!string.IsNullOrEmpty(strPayorNoFail[3]))
                {
                    strPayorN4 = string.Format("N4*{0}*{1}*{2}~",
                        string.Format("{0} {1}", strPayorNoFail[0], strPayorNoFail[1]).Trim(), strPayorNoFail[2], strPayorNoFail[3].Replace("-", "").Trim());
                }
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strPayorN4;
                m_alFile.Add(strPayorN4);
                m_sbUB.AppendFormat("{0}\r\n", strPayorN4);
                m_nSTSegments++;

            }
            #region Removed Code
            /* DO NOT ADD THIS BACK every SSI claim must have it deleted to be valid!!!!!
             * wdk 20120914
            // wdk 20120907 new try with 4010 codes as 5010 does not need this for BlueCross
            string strInsCode = m_dsBilling.Tables["INS"].Rows[0]["plan_nme"].ToString().ToUpper().Trim();
            if (strInsCode == "BLUE CROSS")
            {
                string strRef2U  = "REF*2U*1000427~";
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strRef2U;
                m_alFile.Add(strRef2U);
                m_nSTSegments++;
            }
             */
            /// wdk 20120514 removed altogether as it creates additional errors in SSI
            //// rgc/wdk 20120510 attempt to put payer info on SSI bluecross
            //// LOOP 2010BB Pay-to secondary identification - number necessary for blue cross rgc/wdk 20120509
            //if (drINS["INS_CODE"].ToString().ToUpper() == "BC")
            //{
            //     string strRefNF = "REF*NF*1000427~"; 
            //     rtbDoc.Text += Environment.NewLine;
            //     rtbDoc.Text += strRefNF;
            //     m_alFile.Add(strRefNF);
            //     m_nSTSegments++;
            ////    string strRefG2 = "REF*G2*1000427~";
            ////    rtbDoc.Text += Environment.NewLine;
            ////    rtbDoc.Text += strRefG2;
            ////    m_alFile.Add(strRefG2);
            ////    m_nSTSegments++;
            ////   // string strRef2U = "REF*2U*1000427~"; wdk 20120514 did not work on 20120510 run
            ////   // rtbDoc.Text += Environment.NewLine;
            ////   // rtbDoc.Text += strRef2U;
            ////   // m_alFile.Add(strRef2U);
            ////   // m_nSTSegments++;
            //}
            #endregion
        }

        private void CreateOP_SubscriberHL()
        {
            m_nHLParent = m_nHLCounter;

            //string strHL = string.Format("HL*{0}*1*22*1~", m_nHLCounter++); // rgc/wdk 20110824 changed *1~  to *0~ as there should be no child HL's for this subscriber
            string strHL = string.Format("HL*{0}*1*22*0~", m_nHLCounter++);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHL;
            m_alFile.Add(strHL);
            m_sbUB.AppendFormat("{0}\r\n", strHL);
            m_nSTSegments++;
        }

        private void CreateOP_BillingProviderName()
        {
            // rgc/wdk 20120119 reinstated to provide UB's with taxomity code
            //string strPRV = string.Format("PRV*BI*PXC*291U00000X~"); // our taxomity code
            string strPRV = string.Format("PRV*BI*PXC*282N00000X~"); // hospitals taxomity code
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPRV;
            m_alFile.Add(strPRV);
            m_sbUBHeader.AppendFormat("{0}\r\n", strPRV);
            m_nSTSegments++;

            string strNM1 = string.Format("NM1*85*2*{0}*****XX*{1}~",
                GetSystemParameter("company2_name") ?? "Jackson Madison CTY GNRL",
                GetSystemParameter("wth_npi") ?? "1093705428"); // hospitals NPI
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sbUBHeader.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;

            string strN3 = string.Format("N3*{0}~",
                GetSystemParameter("company_address") ?? "620 SKYLINE DRIVE"); // should be company2_address?
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sbUBHeader.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                GetSystemParameter("company_city") ?? "JACKSON",
                GetSystemParameter("company_state") ?? "TN",
                GetSystemParameter("company_zip") ?? "383013923"); // 5010 requires 9 digits no space or punc.
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sbUBHeader.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;


            string strRefEI = string.Format("REF*EI*{0}~",
                    GetSystemParameter("fed_tax_id") ?? "626010402");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefEI;
            m_alFile.Add(strRefEI);
            m_sbUBHeader.AppendFormat("{0}\r\n", strRefEI);
            m_nSTSegments++;

            CreateBlueCrossRef();
        }

        private void CreateOP_ProviderHL()
        {
            string strHL = string.Format("HL*{0}**20*1~", m_nHLCounter++);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHL;
            m_alFile.Add(strHL);
            m_sbUBHeader.AppendFormat("{0}\r\n", strHL);
            m_nSTSegments++;
        }

        private void CreateOP_ReceiverName()
        {
            // TODO get info from SSI for this segment
            string strRec = string.Format("NM1*40*2*{0}*****46*{1}~",
                        "SSI", "SSI Code");
            //string strInsCode = m_dsBilling.Tables["INS"].Rows[0]["plan_nme"].ToString().ToUpper().Trim();
            //if (strInsCode == "BLUE CROSS")
            //{
            //    strRec = "NM1*40*2*BC*****46*1000427~";
            //}

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRec;
            m_alFile.Add(strRec);
            m_sbUBHeader.AppendFormat("{0}\r\n", strRec);
            m_nSTSegments++;
        }

        private void CreateOP_SubmitterName()
        {
            // create the NM1 Submitter Name
            string strNm1 = string.Format("NM1*41*2*{0}*****46*{1}~",
                GetSystemParameter("company2_name") ?? "JACKSON MADISON CTY GNRL",
                GetSystemParameter("fed_tax_id") ?? "626010402");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNm1;
            m_alFile.Add(strNm1);
            m_sbUBHeader.AppendFormat("{0}\r\n", strNm1);
            m_nSTSegments++;
            // create the per 
            string strPer = string.Format("PER*IC*{0}*TE*{1}*EM*{2}~",
                GetSystemParameter("billing_contact") ?? "CAROL PLUMLEE",
                GetSystemParameter("billing_phone") ?? "7315417320",
                GetSystemParameter("billing_email") ?? "carol.plumlee@wth.org");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPer;
            m_alFile.Add(strPer);
            m_sbUBHeader.AppendFormat("{0}\r\n", strPer);
            m_nSTSegments++;
        }

        private void CreateOP_Header()
        {
            m_strST = string.Format("ST*837*{0}*~",
                    string.Format("{0:D6}", m_nST++));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strST;
            m_alFile.Add(m_strST);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strST);

            m_strBHT = string.Format("BHT*0019*00*{0}*{1}*{2}*CH~",
                   m_strInterchageControlNumber.PadRight(6),
                    DateTime.Now.ToString("yyyyMMdd"),
                        DateTime.Now.ToString("HHmm"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strBHT;
            m_alFile.Add(m_strBHT);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strBHT);
            m_nSTSegments++;
        }

        private void CreateOP_Envelope()
        {
            string strNum = m_rNumber.GetNumber("ssi_batch");
            m_strInterchageControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}",
                DateTime.Now.Year, strNum)));

            tsslBatch.Text = string.Format("Batch: {0}", m_strInterchageControlNumber);

            m_strISA = string.Format("ISA*00*          *00*          *ZZ*{0}*ZZ*{1}*" +
                "{2}*{3}*U*00501*{4}*1*{5}*:~",
                    m_strSubmitterId.PadLeft(15),
                    string.Format("ZMIXED").PadLeft(15), // 8
                        DateTime.Now.ToString("yyMMdd").ToString(), // 9
                            DateTime.Now.ToString("HHmm").ToString(), // 10
                                m_strInterchageControlNumber,
                                    PropProductionEnvironment);
            rtbDoc.Text = m_strISA;
            m_alFile.Add(m_strISA);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strISA);

            m_strGS = string.Format("GS*HC*{0}*ZMIXED*{1}*{2}*{3}*X*005010X223~",
                m_strSubmitterId.PadRight(10),
                    DateTime.Now.ToString("yyyyMMdd"),
                        DateTime.Now.ToString("HHmm"),
                            m_strInterchageControlNumber);

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strGS;
            m_alFile.Add(m_strGS);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strGS);

        }

        private void _CreateUB()
        {
            tsslBatch.Text = "";
            rtbDoc.Clear();
            m_alFile.Clear();

            m_sbUBHeader = new StringBuilder();
            CreateUB_Envelope();
            CreateUB_Header();
            CreateUB_SubmitterName();
            CreateUB_ReceiverName();
            CreateUB_ProviderHL();
            if (m_dsBilling.Tables["INS"].Rows[0].ItemArray[4].ToString().ToUpper() == "MEDICARE" ||
                m_dsBilling.Tables["INS"].Rows[0].ItemArray[12].ToString().ToUpper() == "MC" || // wdk 20150831 use inscode
                m_dsBilling.Tables["INS"].Rows[0].ItemArray[4].ToString().ToUpper() == "CIGNA" ||
                m_dsBilling.Tables["INS"].Rows[0].ItemArray[12].ToString().ToUpper() == "CIGNA"
                )
            {
                CreateUB_BillingProviderNameAlt();
            }
            else
            {
                CreateUB_BillingProviderName();
            }
            tsbpCreate.Value = 0;
            tsbpCreate.Maximum = m_dsBilling.Tables["ACC"].Rows.Count;
            // process each account
            foreach (DataRow dr in m_dsBilling.Tables["ACC"].Rows)
            {
                m_sbUB = new StringBuilder();
                //m_sbUB = m_sbUBHeader;
                m_sbUB.Insert(0, m_sbUBHeader);
                Application.DoEvents();
                tsbpCreate.PerformStep();
                DataRow[] drPat = dr.GetChildRows("AccPat");
                DataRow[] drIns = dr.GetChildRows("AccIns");
                DataRow[] drChrg = dr.GetChildRows("AccChrg");
                DataRow[] drAmt = dr.GetChildRows("AccAmt");

                if (drPat.GetUpperBound(0) == -1 ||
                       drIns.GetUpperBound(0) == -1 ||
                       drChrg.GetUpperBound(0) == -1 ||
                       drAmt.GetUpperBound(0) == -1)
                {
                    string strError = string.Format("Account {0} has error\r\n{1}{2}{3}{4}  ",
                        dr["Account"].ToString(),
                        drPat.GetUpperBound(0) == -1 ? "No Patient record\r\n" : "",
                        drIns.GetUpperBound(0) == -1 ? "No Insurance record\r\n" : "",
                        drChrg.GetUpperBound(0) == -1 ? "No Charge record\r\n" : "",
                        drAmt.GetUpperBound(0) == -1 ? "No Amount Record\r\n" : "");
                    dr.RowError = strError;
                    m_Err.m_Logfile.WriteLogFile(strError);
                    Application.DoEvents();

                    continue;
                }
                while (dr != null)
                {
                    CreateUB_SubscriberHL();
                    // SUBSCRIBER HIERARCHICAL LEVEL LOOP ID 2000B
                    // wdk 20120120 per discussion with carol only send primary insurance for UB's
                    CreateUB_SubscriberInfo(drIns[0]);
                    if (drIns[0]["relation"].ToString() != "01")
                    {
                        CreateUB_PatInfo(drPat[0], drIns[0]["policy_num"].ToString());
                    }
                    //for (int i = 0; i <= drIns.GetUpperBound(0); i++) // wdk 20120120 per discussion with carol
                    //{
                    //    CreateUB_5010_SubscriberInfo(drIns[i]);
                    //    if (drIns[i]["relation"].ToString() != "01")
                    //    {
                    //        CreateUB_5010_PatInfo(drPat[0], drIns[i]["policy_num"].ToString());
                    //    }

                    //}
                    // end of 20120120 changes
                    CreateUB_ClaimHeader(drChrg[0]);
                    CreateUB_DiagnosisInfo(drPat[0]);
                    CreateUB_ClaimInfo(drPat[0]);

                    CreateUB_ServiceLine(drAmt);
                    break;
                }
                if (string.IsNullOrEmpty(dr.RowError))
                {

                    string strMsg = null;
                    m_rAccUpdate.GetRecords(string.Format("Account = '{0}'", dr["ACCOUNT"]));
                    //m_rAccUpdate.m_strStatus = "SSIUB";
                    int nRecUpdate = //m_rAccUpdate.Update();
                    m_rAccUpdate.UpdateField("status", "SSIUB", string.Format("Account = '{0}'", dr["ACCOUNT"]), out strMsg);
                    if (nRecUpdate > 0)
                    {
                        m_rPat.ClearMemberVariables();
                        string strWhere = string.Format("account = '{0}'", m_rAccUpdate.m_strAccount);
                        string strErr;
                        int nPatRec = m_rPat.GetActiveRecords(strWhere);

                        if (nPatRec == 1)
                        {
                            nPatRec = m_rPat.UpdateField("ub_date", DateTime.Today.ToShortDateString(), strWhere, out strErr);
                            if (nPatRec != 1)
                            {
                                m_Err.m_Logfile.WriteLogFile(strErr);
                            }
                            nPatRec = m_rPat.UpdateField("ssi_batch", m_strInterchageControlNumber, strWhere, out strErr);
                            if (nPatRec != 1)
                            {
                                m_Err.m_Logfile.WriteLogFile(strErr);
                            }
                        }
                    }
                }
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlCommand cmdInsert = new SqlCommand(
                        string.Format("insert into data_billing_history " +
                        "(account, ins_abc, pat_name, fin_code, ins_code, " +
                        "trans_date, run_date, batch, ebill_status, ebill_batch, " +
                        "text, mod_date, mod_user, mod_prg, mod_host) " +
                        "VALUES " +
                        " ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', " +
                        "'{6}', '{7}', '{8}', '{9}', '{10}', " +
                        "'{11}',suser_sname(),'{12}',host_name() ) ",
                        dr["account"].ToString()
                            , drIns[0]["ins_a_b_c"].ToString()
                                , dr["pat_name"].ToString()
                                    , dr["fin_code"].ToString()
                                        , drIns[0]["ins_code"].ToString()
                        , dr["trans_date"].ToString()
                            , DateTime.Today,
                                m_strInterchageControlNumber,
                                    m_strFilter
                                        , m_strInterchageControlNumber
                        , m_sbUB.ToString()
                            , DateTime.Now
                                , propAppName
                        )
                        , conn);
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.InsertCommand = cmdInsert;
                    sda.InsertCommand.Connection.Open();
                    try
                    {
                        sda.InsertCommand.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        sda.InsertCommand.Connection.Close();
                    }

                }

            }

            CreateUB_SE();
            CreateUB_GE();
            CreateUB_IEA();

            if (DateTime.Now < new DateTime(2013, 02, 24, 16, 0, 0))
            {
                return;
            }
            string strDateTime = DateTime.Now.ToString("yyyyMMddHHmm");
            string strFile = string.Format(@"C:\temp\UB\UB_{0}.x12", strDateTime);
            m_Err.m_Logfile.WriteLogFile(string.Format("\r\n{0}\r\n", strFile)); //wdk 20120702 added to track each file.
            StreamWriter sw = new StreamWriter(strFile);
            sw.AutoFlush = true;
            foreach (string str in m_alFile)
            {
                sw.Write(str);

            }
            sw.Close();

            if (m_strDatabase.Contains("LIVE"))
            {

                string strFile2 = string.Format(@"\\wth251\ssiapp\ssi\00852\billing\uploadh\mcl\UB_{0}.in", strDateTime);// DateTime.Now.ToString("yyyyMMddHHmm"));
                StreamWriter sw2 = new StreamWriter(strFile2);
                sw2.AutoFlush = true;

                foreach (string str in m_alFile)
                {

                    sw2.Write(str);
                }

                sw2.Close();
            }
            // wdk 20130829
            m_nHLCounter = 1;
        }


        private void CreateUB_IEA()
        {
            m_strIEA = string.Format("IEA*{0}*{1}~",
              m_nFunctionalGroups,
                  m_strInterchageControlNumber);

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strIEA;
            m_alFile.Add(m_strIEA);
        }

        private void CreateUB_GE()
        {
            m_strGE = string.Format("GE*{0}*{1}~",
                m_nFunctionalGroups,
                    m_strInterchageControlNumber);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strGE;
            m_alFile.Add(m_strGE);
        }

        private void CreateUB_SE()
        {
            m_strSE = string.Format("SE*837*{0}*{1}*CH~",
             m_nSTSegments,
                 DateTime.Now.ToString("yyyyMMdd"),
                     DateTime.Now.ToString("HHmm").ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strSE;
            m_alFile.Add(m_strSE);
        }

        private void CreateUB_ServiceLine(DataRow[] drAmt)
        {
            int nLX = 1;

            for (int i = 0; i <= drAmt.GetUpperBound(0); i++)
            {
                string strDesc = null;
                if (m_alCpt4DescriptionRequired.Contains(drAmt[i]["cpt4"]))
                {
                    DataRow[] drWarning = m_dtCpt4Desc.Select(string.Format(" cpt4 = '{0}'", drAmt[i]["cpt4"]));
                    if (drWarning.GetUpperBound(0) > -1)
                    {
                        strDesc = drWarning[0]["line desc"].ToString();
                    }
                }
                ///////////////
                string strLX = string.Format("LX*{0}~", nLX++);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strLX;
                m_alFile.Add(strLX);
                m_sbUB.AppendFormat("{0}\r\n", strLX);
                m_nSTSegments++;
                // rgc/wdk 20120403 removed one of the sub field indicators in SV2-02 for ssi import
                string strSRV = string.Format("SV2*{0}*HC:{1}:{2}:{3}:::{4}*{5}*UN*{6}**{7}~",
                    drAmt[i]["revcode"].ToString(),
                    drAmt[i]["cpt4"],
                    drAmt[i]["modi"].ToString().Trim(),
                    drAmt[i]["modi2"].ToString().Trim(),
                    //string.IsNullOrEmpty(drAmt[i]["modi"].ToString().Trim()) ? "" : string.Format(":{0}", drAmt[i]["modi"]),
                    //string.IsNullOrEmpty(drAmt[i]["modi2"].ToString().Trim()) ? "" : string.Format(":{0}", drAmt[i]["modi2"]),
                    strDesc,
                    double.Parse(drAmt[i]["amount"].ToString()).ToString("F2"),
                    drAmt[i]["qty"],
                    drAmt[i]["modi"].ToString().Trim() == "GZ" ? double.Parse(drAmt[i]["amount"].ToString()).ToString("F2") : ""

                   );

                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strSRV;
                m_alFile.Add(strSRV);
                m_sbUB.AppendFormat("{0}\r\n", strSRV);
                m_nSTSegments++;

                // dRetVal += double.Parse(drAmt[i]["amount"].ToString());

                DateTime dtTransDate = DateTime.Parse(drAmt[i]["trans_date"].ToString());
                string strTransDate = dtTransDate.ToString("yyyyMMdd");
                string strCLMDate = string.Format("DTP*472*D8*{0}~",
                  strTransDate);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strCLMDate;
                m_alFile.Add(strCLMDate);
                m_sbUB.AppendFormat("{0}\r\n", strCLMDate);
                m_nSTSegments++;
            }
        }

        private void CreateUB_ClaimInfo(DataRow dataRow)
        {
            // rgc/wdk 20110825 changed from Referring Provider to  Attending Provider. 
            // documentation says use Referring if the two are not the same. Referring is DN not 71
            string strClaimAttendingProviderNM1 = string.Format("NM1*71*1*{0}*{1}*{2}***XX*{3}~",
                    dataRow["last_name"].ToString(),
                    dataRow["first_name"].ToString(),
                    dataRow["mid_init"].ToString(),
                    dataRow["phy_id"].ToString());

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimAttendingProviderNM1;
            m_alFile.Add(strClaimAttendingProviderNM1);
            m_sbUB.AppendFormat("{0}\r\n", strClaimAttendingProviderNM1);
            m_nSTSegments++;
            #region Removed Code
            //string strClaimAttendingProviderNM1 = @"NM1*LI*2*MEDICAL CENTER LABORATORY*****XX*1720160708~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimAttendingProviderNM1;
            //m_alFile.Add(strClaimAttendingProviderNM1);
            //m_alUB.Add(strClaimAttendingProviderNM1);
            //m_nSTSegments++;

            //string strClaimRenderingProviderN3 = "N3*620 SKYLINE DRIVE~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderN3;
            //m_alFile.Add(strClaimRenderingProviderN3);
            //m_alUB.Add(strClaimRenderingProviderN3);
            //m_nSTSegments++;

            //string strClaimRenderingProviderN4 = "N4*JACKSON*TN*383010000~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderN4;
            //m_alFile.Add(strClaimRenderingProviderN4);
            //m_alUB.Add(strClaimRenderingProviderN4);
            //m_nSTSegments++;

            //string strClaimRenderingProviderRef = "1720160708~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderRef;
            //m_alFile.Add(strClaimRenderingProviderRef);
            //m_alUB.Add(strClaimRenderingProviderRef);
            //m_nSTSegments++;
            #endregion

        }
        // 2300 Claim Imformation Principal Diagnosis
        private void CreateUB_DiagnosisInfo(DataRow drPAT)
        {
            string strIndicator = "BK";
            string strIndicator2 = "BF";
            if (drPAT["icd_indicator"].ToString() == "I10")
            {
                strIndicator = "ABK";
                strIndicator2 = "ABF";
            }


            // Primary Diagnosis this visit
            // HI:BK = icd9
            // HI:ABK = icd10;
            string strHI = string.Format("HI*{0}:{1}~",//{1}{2}{3}~",
               strIndicator,
               drPAT["icd9_1"].ToString().Replace(".", "")//,
                                                          //string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()) ? "" : string.Format("*BF:{0}", drPAT["icd9_2"].ToString().Replace(".", "")),
                                                          //string.IsNullOrEmpty(drPAT["icd9_3"].ToString().Trim()) ? "" : string.Format("*BF:{0}", drPAT["icd9_3"].ToString().Replace(".", "")),
                                                          //string.IsNullOrEmpty(drPAT["icd9_4"].ToString().Trim()) ? "" : string.Format("*BF:{0}", drPAT["icd9_4"].ToString().Replace(".", ""))
               );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHI;
            m_alFile.Add(strHI);
            m_sbUB.AppendFormat("{0}\r\n", strHI);
            m_nSTSegments++;

            // must have a second diagnois to include this segment
            if (!string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()))
            {
                // HI:BF = icd9
                // HI:ABF = icd10;
                string strHISecondary = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}~",
                    string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()) ? "" : string.Format("HI*{1}:{0}", drPAT["icd9_2"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_3"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_3"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_4"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_4"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_5"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_5"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_6"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_6"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_7"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_7"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_8"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_8"].ToString().Replace(".", ""), strIndicator2),
                    string.IsNullOrEmpty(drPAT["icd9_9"].ToString().Trim()) ? "" : string.Format("*HI*{1}:{0}", drPAT["icd9_9"].ToString().Replace(".", ""), strIndicator2)
                   );
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strHISecondary;
                m_alFile.Add(strHISecondary);
                m_sbUB.AppendFormat("{0}\r\n", strHISecondary);
                m_nSTSegments++;
            }
        }
        // 2300 Claim Information LOOP
        private void CreateUB_ClaimHeader(DataRow dr)
        {
            string strAccount = dr["account"].ToString();
            DataTable dttotal = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = new SqlCommand(string.Format("select sum(qty*net_amt) as [Total Chrg] " +
                " from chrg " +
                " where account = '{0}'", strAccount), m_sqlConnection);
            sda.Fill(dttotal);
            // string strCLM = string.Format("CLM*{0}*{1:F2}***14:A:1*Y*A*Y*A*P~", rgc/wdk 20110824 removed the [:F2] from the amount 

            string strCLM = string.Format("CLM*{0}*{1}***14:A:1**B*Y*Y~",
                  strAccount,
                  double.Parse(dttotal.Rows[0]["Total Chrg"].ToString()).ToString("F2"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strCLM;
            m_alFile.Add(strCLM);
            m_sbUB.AppendFormat("{0}\r\n", strCLM);
            m_nSTSegments++;

            // rgc/wdk added cause dummy missed it the first time around.
            sda.SelectCommand = new SqlCommand(string.Format("select trans_date" +
                " from acc " +
                " where account = '{0}'", strAccount), m_sqlConnection);
            DataTable dtTransDate = new DataTable();
            sda.Fill(dtTransDate);
            DateTime dtTDate = DateTime.Parse(dtTransDate.Rows[0]["trans_date"].ToString());
            string strDTP = string.Format("DTP*434*RD8*{0}-{0}~", dtTDate.ToString("yyyyMMdd"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strDTP;
            m_alFile.Add(strDTP);
            m_sbUB.AppendFormat("{0}\r\n", strDTP);
            m_nSTSegments++;


            string strCL1 = "CL1*9*1*01~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strCL1;
            m_alFile.Add(strCL1);
            m_sbUB.AppendFormat("{0}\r\n", strCL1);
            m_nSTSegments++;


        }

        private void CreateUB_PatInfo(DataRow drPAT, string p)
        {
            string strRelation = drPAT["relation"].ToString();
            if (strRelation == "03")
            {
                strRelation = "19";
            }
            if (strRelation == "02")
            {
                strRelation = "01";
            }
            if (strRelation == "09")
            {
                strRelation = "G8";
            }

            string strPatHL = string.Format("HL*{0}*{1}*{2}*0~",
                m_nHLCounter++, m_nHLParent,
                strRelation);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatHL;
            m_alFile.Add(strPatHL);
            m_sbUB.AppendFormat("{0}\r\n", strPatHL);
            m_nSTSegments++;

            string strPat = string.Format("PAT*{0}*~",
                strRelation);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPat;
            m_alFile.Add(strPat);
            m_sbUB.AppendFormat("{0}\r\n", strPat);
            m_nSTSegments++;

            string strName = drPAT["pat_name"].ToString();
            string strLastName = string.Empty;
            string strFirstName = string.Empty;
            string strMiddleName = string.Empty;
            string strNameSuffix = string.Empty;
            if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(","))
            {
                //strLastName = strName.Substring(0, strName.IndexOf(",")).Trim();
                //strName = strName.Replace(strLastName, "").Trim();
                //strName = strName.Replace(",", "").Trim();
                // rgc/wdk 20120510 modified to handle names like "JAMES,JAMES"
                strLastName = strName.Substring(0, strName.IndexOf(","));
                string strReplace = string.Format("{0},", strLastName).Trim();
                strName = strName.Replace(strReplace, "").Trim();
                //strName = strName.Replace(",", "").Trim();
                if (strLastName.Contains(" "))
                {
                    string[] strNameSplit = strLastName.Split(new string[] { " " }, StringSplitOptions.None);
                    strLastName = strNameSplit[0].Trim();
                    strNameSuffix = strNameSplit[1].Trim();
                }
                strFirstName = strName;
                if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(" "))
                {
                    strFirstName = strName.Substring(0, strName.IndexOf(" ")).Trim();
                    strMiddleName = strName.Replace(strFirstName, "").Trim();
                }
            }

            string strPatName = string.Format("NM1*QC*1*{0}*{1}*{2}**{3}~",//*MI*{4}~",
               strLastName, strFirstName, strMiddleName, strNameSuffix);
            //, strPolicyIdentifier);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatName;
            m_alFile.Add(strPatName);
            m_sbUB.AppendFormat("{0}\r\n", strPatName);
            m_nSTSegments++;


            string strPatN3 = string.Format("N3*{0}~",
                drPAT["pat_addr1"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatN3;
            m_alFile.Add(strPatN3);
            m_sbUB.AppendFormat("{0}\r\n", strPatN3);
            m_nSTSegments++;

            string[] strNoFail = new string[] { "", "", "", "" };
            string[] strCSZ = drPAT["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            strCSZ.CopyTo(strNoFail, 0);

            string strPatN4 = string.Format("N4*{0}*{1}*{2}~",
                strNoFail[0], strNoFail[1], strNoFail[2]);
            if (!string.IsNullOrEmpty(strNoFail[3]))
            {
                strPatN4 = string.Format("N4*{0}*{1}*{2}~",
                string.Format("{0} {1}", strNoFail[0], strNoFail[1]).Trim(), strNoFail[2], strNoFail[3]);
            }
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatN4;
            m_alFile.Add(strPatN4);
            m_sbUB.AppendFormat("{0}\r\n", strPatN4);
            m_nSTSegments++;

            // DateTime dtTransDate = DateTime.Parse(drPAT["trans_date"].ToString());
            DateTime dtPatDob = DateTime.Parse(drPAT["dob_yyyy"].ToString());
            string strPatDMG = string.Format("DMG*D8*{0}*{1}~",
                dtPatDob.ToString("yyyyMMdd"),
                //dtTransDate.ToString("yyyyMMdd"), 
                drPAT["sex"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatDMG;
            m_alFile.Add(strPatDMG);
            m_sbUB.AppendFormat("{0}\r\n", strPatDMG);
            m_nSTSegments++;

            // LOOP 2010CA situational. shouldn't be needed because the secondary info is the same as the primary
            // wdk 20100316 Have no policy number in the pat table throw error.
            //string strREFSec = string.Format("REF*1G*{0}~",
            //    drPAT["policy_num"].ToString());
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strREFSec;
            //m_alFile.Add(strREFSec);


        }
        // 2000B Subscriber Hierarchical level
        private void CreateUB_SubscriberInfo(DataRow drINS)
        {
            string strRespSeqNo = "P";
            if (drINS["ins_a_b_c"].ToString() == "B")
            {
                strRespSeqNo = "S";
            }
            if (drINS["ins_a_b_c"].ToString() == "C")
            {
                strRespSeqNo = "T";
            }
            string strRelation = drINS["relation"].ToString().Trim() == "01" ? "18" : "";

            /*Just FYI we are getting a new UB edit this morning:  I have contacted SSI and this is a new 5010 edit.  Julie is going in the bill format and taking out our group name in FL 61
            Email from Debbie Thornhill 02/17/2011
                INSURANCE GROUP NUMBER (PRI) AND INSURANCE GROUP NAME (PRI) CANNOT BOTH BE PRESENT.
                - 2000B SBR03 AND 04 -  **** HIPAA ANSI GENERIC EDIT ****   [VERSION 02/15/2012]
                SOURCE = WPC 837 5010 IMPLEMENTATION GUIDE
            */
            string strGrpNum = drINS["grp_num"].ToString();
            if (!string.IsNullOrEmpty(strGrpNum))
            {
                if (strGrpNum.StartsWith("\0"))
                {
                    strGrpNum = "";
                }
            }
            string strPlanName = drINS["plan_nme"].ToString().ToUpper();
            string strInsCode = drINS["ins_code"].ToString().ToUpper(); // wdk 20150831 
            // wdk 20121010 will fail ssi validation and go to skipped claims.
            //if (strPlanName.ToUpper() == "MEDICARE")
            //{
            //    strPlanName = "";
            //}
            string strSBR = string.Format("SBR*{0}*{1}*{2}*{3}~",
                strRespSeqNo,
                strRelation,
                strGrpNum,//drINS["policy_num"].ToString(), // per 5010 this is not the policy number, that is submitted in the NM109 below.
                string.IsNullOrEmpty(strGrpNum) ?
                (strInsCode == "MC" || strPlanName == "MEDICARE") ? "" : strPlanName : "");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strSBR;
            m_alFile.Add(strSBR);
            m_sbUB.AppendFormat("{0}\r\n", strSBR);
            m_nSTSegments++;

            // 2010BA Subscriber Name
            string strName = drINS["holder_nme"].ToString();
            string strLastName = string.Empty;
            string strFirstName = string.Empty;
            string strMiddleName = string.Empty;
            string strNameSuffix = string.Empty;
            if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(","))
            {
                strLastName = strName.Substring(0, strName.IndexOf(","));
                strName = strName.Replace(strLastName, "").Trim();
                strName = strName.Replace(",", "").Trim();
                if (strLastName.Contains(" "))
                {
                    string[] strNameSplit = strLastName.Split(new string[] { " " }, StringSplitOptions.None);
                    strLastName = strNameSplit[0].Trim();
                    strNameSuffix = strNameSplit[1].Trim();
                }
                strFirstName = strName;
                if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(" "))
                {
                    strFirstName = strName.Substring(0, strName.IndexOf(" ")).Trim();
                    strMiddleName = strName.Replace(strFirstName, "").Trim();
                }
            }
            string strNM1 = string.Format("NM1*IL*1*{0}*{1}*{2}**{3}*MI*{4}~",
                strLastName, strFirstName, strMiddleName, strNameSuffix,
                drINS["policy_num"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sbUB.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;


            string strN3 = string.Format("N3*{0}~",
                drINS["addr"]);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sbUB.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string[] strNoFail = new string[] { "", "", "", "" };
            // wdk 20120719 fails with towns like "PORT SAINT LUCIE"
            //string[] strCSZ = drINS["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            //strCSZ.CopyTo(strNoFail, 0);
            string[] strAddrIns = drINS["city_st_zip"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] strCSZ = strAddrIns[1].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            strAddrIns.CopyTo(strNoFail, 0);
            strCSZ.CopyTo(strNoFail, 1);

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                 strNoFail[0], strNoFail[1].Trim(), strNoFail[2].Replace("-", "").Trim()); ;
            if (!string.IsNullOrEmpty(strNoFail[3]))
            {
                strN4 = string.Format("N4*{0}*{1}*{2}~",
                    string.Format("{0} {1}", strNoFail[0], strNoFail[1]).Trim(), strNoFail[2], strNoFail[3].Replace("-", "").Trim());
            }
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sbUB.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;

            string strSubDemoInfo = string.Format("DMG*D8*{0}*{1}~",
                 RFCObject.ConvertDateTimeToHL7Date(drINS["HOLDER_DOB"].ToString()), drINS["HOLDER_SEX"]);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strSubDemoInfo;
            m_alFile.Add(strSubDemoInfo);
            m_sbUB.AppendFormat("{0}\r\n", strSubDemoInfo);
            m_nSTSegments++;

            // LOOP 2010BB
            // wdk 20121010 for PR*2 clear if medicare because of new edit in CMS
            // wdk 20121011 not the correct place to remove the value from the form.
            //if (strPlanName.ToUpper() == "MEDICARE")
            //{
            //    strPlanName = "";
            //}
            string strPayerNM1 = string.Format("NM1*PR*2*{0}*****PI*{1}~",
               strPlanName,// drINS["plan_nme"], wdk 20121009 changed to blank for medicare
               drINS["payer_no"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayerNM1;
            m_alFile.Add(strPayerNM1);
            m_sbUB.AppendFormat("{0}\r\n", strPayerNM1);
            m_nSTSegments++;

            //todo: may need to add payer address here??? Yes for paper claims 20120328 
            if (!string.IsNullOrEmpty(drINS["plan_addr1"].ToString()))
            {
                string strPayorN3 = string.Format("N3*{0}~",
                     drINS["plan_addr1"]);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strPayorN3;
                m_alFile.Add(strPayorN3);
                m_sbUB.AppendFormat("{0}\r\n", strPayorN3);
                m_nSTSegments++;

                string[] strPayorNoFail = new string[] { "", "", "", "" };
                string[] strPayorCSZ = drINS["plan_csz"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                strPayorCSZ.CopyTo(strPayorNoFail, 0);

                string strPayorN4 = string.Format("N4*{0}*{1}*{2}~",
                     strPayorNoFail[0], strPayorNoFail[1].Trim(), strPayorNoFail[2].Replace("-", "").Trim()); ;
                if (!string.IsNullOrEmpty(strPayorNoFail[3]))
                {
                    strPayorN4 = string.Format("N4*{0}*{1}*{2}~",
                        string.Format("{0} {1}", strPayorNoFail[0], strPayorNoFail[1]).Trim(), strPayorNoFail[2], strPayorNoFail[3].Replace("-", "").Trim());
                }
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strPayorN4;
                m_alFile.Add(strPayorN4);
                m_sbUB.AppendFormat("{0}\r\n", strPayorN4);
                m_nSTSegments++;
            }

        }

        // 2000B loop Subscriber Hierarchical Level
        private void CreateUB_SubscriberHL()
        {
            m_nHLParent = m_nHLCounter;

            //string strHL = string.Format("HL*{0}*1*22*1~", m_nHLCounter++); // rgc/wdk 20110824 changed *1~  to *0~ as there should be no child HL's for this subscriber
            string strHL = string.Format("HL*{0}*1*22*0~", m_nHLCounter++);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHL;
            m_alFile.Add(strHL);
            m_sbUB.AppendFormat("{0}\r\n", strHL);
            m_nSTSegments++;
        }

        private void CreateUB_BillingProviderName()
        {
            // rgc/wdk 20120119 reinstated to provide UB's with taxomity code
            string strPRV = string.Format("PRV*BI*PXC*291U00000X~");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPRV;
            m_alFile.Add(strPRV);
            m_sbUBHeader.AppendFormat("{0}\r\n", strPRV);
            m_nSTSegments++;

            string strNM1 = string.Format("NM1*85*2*{0}*****XX*{1}~",
                GetSystemParameter("billing_entity_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("npi_number") ?? "1720160708");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sbUBHeader.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;

            //string strN3 = "N3*PO BOX 3099~"; // rgc/wdk 20110824 after 5010 meeting 
            string strN3 = string.Format("N3*{0}~",
                GetSystemParameter("company_address") ?? "620 SKYLINE DRIVE");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sbUBHeader.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string strN4 = string.Format("N4*JACKSON*TN*383013923~",
                GetSystemParameter("company_city") ?? "JACKSON",
                GetSystemParameter("company_state") ?? "TN",
                GetSystemParameter("company_zip") ?? "383013923"); // 5010 requires 9 digits no space or punc.
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sbUBHeader.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;

            //string strRefIB = "REF*1B*003091277~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strRefIB;
            //m_alFile.Add(strRefIB);
            //m_alUBHeader.Add(strRefIB);
            //m_nSTSegments++;

            string strRefEI = string.Format("REF*EI*{0}~",
                GetSystemParameter("fed_tax_id") ?? "626010402");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefEI;
            m_alFile.Add(strRefEI);
            m_sbUBHeader.AppendFormat("{0}\r\n", strRefEI);
            m_nSTSegments++;

            CreateBlueCrossRef();
        }


        // 2010AA loop Alternate for MEDICARE and CIGNA
        private void CreateUB_BillingProviderNameAlt()
        {
            // rgc/wdk 20120119 reinstated to provide UB's with taxomity code
            string strPRV = string.Format("PRV*BI*PXC*291U00000X~");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPRV;
            m_alFile.Add(strPRV);
            m_sbUBHeader.AppendFormat("{0}\r\n", strPRV);
            m_nSTSegments++;

            string strNM1 = string.Format("NM1*85*2*{0}*****XX*{1}~",
                GetSystemParameter("company2_name") ?? "Jackson Madison CTY GNRL", // company2_name
                GetSystemParameter("wth_npi") ?? "1093705428" // wth_npi
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sbUBHeader.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;

            string strN3 = string.Format("N3*{1}~",
                GetSystemParameter("company_address") ?? "620 SKYLINE DRIVE");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sbUBHeader.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                GetSystemParameter("company_city") ?? "JACKSON",
                GetSystemParameter("company_state") ?? "TN",
                GetSystemParameter("company_zip") ?? "383013923"
                ); // 5010 requires 9 digits no space or punc.
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sbUBHeader.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;

            //string strRefIB = "REF*1B*003091277~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strRefIB;
            //m_alFile.Add(strRefIB);
            //m_alUBHeader.Add(strRefIB);
            //m_nSTSegments++;

            string strRefEI = string.Format("REF*EI*{0}~",
                GetSystemParameter("fed_tax_id") ?? "626010402" //fed_tax_id 
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefEI;
            m_alFile.Add(strRefEI);
            m_sbUBHeader.AppendFormat("{0}\r\n", strRefEI);
            m_nSTSegments++;

            CreateBlueCrossRef();
        }

        private void CreateUB_ProviderHL()
        {
            string strHL = string.Format("HL*{0}**20*1~", m_nHLCounter++);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHL;
            m_alFile.Add(strHL);
            //m_alFile.Add(strHL);
            m_sbUBHeader.AppendFormat("{0}\r\n", strHL);
            m_nSTSegments++;
        }

        private void CreateUB_ReceiverName()
        {
            // TODO get info from SSI for this segment
            string strRec = string.Format("NM1*40*2*{0}*****46*{1}~",
                        "SSI", "SSI Code");
            //string strInsCode = m_dsBilling.Tables["INS"].Rows[0]["plan_nme"].ToString().ToUpper().Trim();
            //if (strInsCode == "BLUE CROSS")
            //{
            //    strRec = "NM1*40*2*BC*****46*1000427~";
            //}

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRec;
            m_alFile.Add(strRec);
            m_sbUBHeader.AppendFormat("{0}\r\n", strRec);
            m_nSTSegments++;
        }

        private void CreateUB_SubmitterName()
        {
            // create the NM1 Submitter Name
            string strNm1 = string.Format("NM1*41*2*{0}*****46*{1}~",
                GetSystemParameter("company_name") ?? "MEDICAL CENTER LABORATORY", //company_name
                GetSystemParameter("fed_tax_id") ?? "626010402" //fed_tax_id
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNm1;
            m_alFile.Add(strNm1);
            m_sbUBHeader.AppendFormat("{0}\r\n", strNm1);
            m_nSTSegments++;
            // create the per 
            string strPer = string.Format("PER*IC*{0}*TE*{1}*EM*{2}~",
                GetSystemParameter("billing_contact") ?? "CAROL PLUMLEE",
                GetSystemParameter("billing_phone") ?? "7315417320",
                GetSystemParameter("billing_email") ?? "carol.plumlee@wth.org"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPer;
            m_alFile.Add(strPer);
            m_sbUBHeader.AppendFormat("{0}\r\n", strPer);
            m_nSTSegments++;

        }

        private void CreateUB_Header()
        {
            m_strST = string.Format("ST*837*{0}*{1}~",
                string.Format("{0:D6}", m_nST++),
                m_837i_version);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strST;
            m_alFile.Add(m_strST);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strST);


            m_strBHT = string.Format("BHT*0019*00*{0}*{1}*{2}*CH~",
                   m_strInterchageControlNumber.PadRight(6),
                    DateTime.Now.ToString("yyyyMMdd"),
                        DateTime.Now.ToString("HHmm"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strBHT;
            m_alFile.Add(m_strBHT);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strBHT);
            m_nSTSegments++;

            // not used in 5010
            //string strBHTRef = string.Format("REF*87*004010X098A1***~");
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strBHTRef;
            //m_alFile.Add(strBHTRef);
            //m_alUBHeader.Add(strBHTRef);
            //m_nSTSegments++;
        }

        private void CreateUB_Envelope()
        {
            string strNum = m_rNumber.GetNumber("ssi_batch");
            m_strInterchageControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}",
                DateTime.Now.Year, strNum)));

            tsslBatch.Text = string.Format("Batch: {0}", m_strInterchageControlNumber);

            m_strISA = string.Format("ISA*00*          *00*          *ZZ*{0}*ZZ*{1}*" +
                "{2}*{3}*U*00501*{4}*1*{5}*:~",
                    m_strSubmitterId.PadLeft(15),
                    string.Format("ZMIXED").PadLeft(15), // 8
                        DateTime.Now.ToString("yyMMdd").ToString(), // 9
                            DateTime.Now.ToString("HHmm").ToString(), // 10
                                m_strInterchageControlNumber,
                                    PropProductionEnvironment);
            rtbDoc.Text = m_strISA;
            m_alFile.Add(m_strISA);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strISA);

            m_strGS = string.Format("GS*HC*{0}*ZMIXED*{1}*{2}*{3}*X*{4}~",
                m_strSubmitterId.PadRight(10),
                    DateTime.Now.ToString("yyyyMMdd"),
                        DateTime.Now.ToString("HHmm"),
                            m_strInterchageControlNumber,
                            m_837i_version);

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strGS;
            m_alFile.Add(m_strGS);
            m_sbUBHeader.AppendFormat("{0}\r\n", m_strGS);
        }

        private void _Create1500()
        {
            tsslBatch.Text = "";
            rtbDoc.Clear();
            m_alFile.Clear();
            m_sb1500Header = new StringBuilder();
            // ENVELOPE
            Create1500_Envelope();

            // HEADER
            Create1500_ST_Header();

            // SUBMITTER LOOP ID 1000A
            Create1500_SubmitterName();
            // RECEIVER LOOP ID 1000B
            Create1500_ReceiverName();
            // BILLING/PAY-TO LOOP ID 2000A
            Create1500_BillingProviderHL();

            // BILLING PROVIDER NAME LOOP ID 2010AA
            // wdk 20130911 testing to see if this fixes the new requirement from the provider edit in ssi
            //if (dgvRecords["plan_nme", 0].Value.ToString().ToUpper().Contains("CHAMPUS"))
            //{
            //    Create1500_BillingProviderNameChampus();
            //}
            //else
            {
                Create1500_BillingProviderName();
            }
            // PAY TO PROVIDER NAME LOOP ID 2010AB
            //not required when the billing and pay-to are the same. pay to is the po box so not the same
            Create1500_PayToProviderName();

            tsbpCreate.Value = 0;
            tsbpCreate.Maximum = m_dsBilling.Tables["ACC"].Rows.Count;
            // process each account
            foreach (DataRow drSelectedAcc in m_dsBilling.Tables["ACC"].Rows)
            {
                Application.DoEvents();
                if (!string.IsNullOrEmpty(drSelectedAcc.RowError))
                {
                    continue;
                }
                m_sb1500 = new StringBuilder();
                // m_sb1500 = m_sb1500Header;
                m_sb1500.Insert(0, m_sb1500Header);

                tsbpCreate.PerformStep();
                DataRow[] drPat = drSelectedAcc.GetChildRows("AccPat");
                DataRow[] drIns = drSelectedAcc.GetChildRows("AccIns");
                DataRow[] drChrg = drSelectedAcc.GetChildRows("AccChrg");
                DataRow[] drAmt = drSelectedAcc.GetChildRows("AccAmt");
                if (drPat.GetUpperBound(0) == -1 ||
                       drIns.GetUpperBound(0) == -1 ||
                       drChrg.GetUpperBound(0) == -1 ||
                       drAmt.GetUpperBound(0) == -1
                       )
                {
                    string strError = string.Format("Account {0} has error\r\n{1}{2}{3}{4}  ",
                        drSelectedAcc["Account"].ToString(),
                        drPat.GetUpperBound(0) == -1 ? "No Patient record\r\n" : "",
                        drIns.GetUpperBound(0) == -1 ? "No Insurance record\r\n" : "",
                        drChrg.GetUpperBound(0) == -1 ? "No Charge record\r\n" : "",
                        drAmt.GetUpperBound(0) == -1 ? "No Amount Record\r\n" : "");
                    drSelectedAcc.RowError = strError;
                    m_Err.m_Logfile.WriteLogFile(strError);
                    Application.DoEvents();
                    continue;
                }

                #region Removed Code
                //if (drPat.GetUpperBound(0) == -1 ||
                //        drIns.GetUpperBound(0) == -1 ||
                //            drChrg.GetUpperBound(0) == -1 ||
                //                drAmt.GetUpperBound(0) == -1)
                //{
                //    continue;
                //}
                #endregion

                while (drSelectedAcc != null)
                {
                    if (drIns[0]["plan_nme"].ToString() == "BLUE CROSS")
                    {
                        CreateBlueCrossRef();
                    }

                    #region Removed Code
                    //if (DateTime.Today <= DateTime.Parse("08/03/2012 23:59"))
                    //{
                    //    Create1500_SubscriberHL(drPat[0]["relation"].ToString());

                    //    // SUBSCRIBER HIERARCHICAL LEVEL LOOP ID 2000B

                    //    Create1500_SubscriberInfo(drIns[0]);
                    //    Create1500_SubscriberName(drIns[0]);
                    //    Create1500_5010_PayerName(drIns[0]);

                    //    // if the patient and the subscriber are not the same add the pat info after the SBR before the subscribers address.
                    //    if (drPat[0]["relation"].ToString() != "01")
                    //    {
                    //        Create1500_PatInfo(drPat[0], drIns[0]["policy_num"].ToString());
                    //    }
                    //    Create1500_ClaimHeader(drChrg[0]);
                    //    Create1500_DiagnosisInfo(drPat[0]);
                    //    Create1500_5010_ReferringProviderInfo(drPat[0]);// referring and rendering go here
                    //    // already have the primary insurance start with the second.
                    //    for (int i = 1; i <= drIns.GetUpperBound(0); i++)
                    //    {
                    //        Create1500_SubscriberInfo(drIns[i]);
                    //        Create1500_SubscriberName(drIns[i]);
                    //        Create1500_5010_PayerName(drIns[i]);

                    //        // if the patient and the subscriber are not the same add the pat info after the SBR before the subscribers address.
                    //        if (drPat[0]["relation"].ToString() != "01")
                    //        {
                    //            Create1500_PatInfo(drPat[0], drIns[i]["policy_num"].ToString());
                    //        }
                    //    }

                    //}
                    //else
                    #endregion

                    {
                        Create1500_SubscriberHL(drPat[0]["relation"].ToString());
                        // primary only
                        //for (int i = 0; i <= drIns.GetUpperBound(0); i++)
                        {
                            Create1500_SubscriberInfo(drIns[0]);
                            Create1500_SubscriberName(drIns[0]);
                            Create1500_5010_PayerName(drIns[0]);

                            // if the patient and the subscriber are not the same add the pat info after the SBR before the 
                            // subscribers address.

                            if (drPat[0]["relation"].ToString() != "01")
                            {
                                Create1500_PatInfo(drPat[0], drIns[0]["policy_num"].ToString());
                            }
                        }

                        Create1500_ClaimHeader(drChrg[0]);
                        //Create1500_4010_ClaimRefs(drIns); may not be required for SSI
                        Create1500_DiagnosisInfo(drPat[0]);
                        // referring and rendering go here
                        Create1500_5010_ReferringProviderInfo(drPat[0]);
                    }
                    Create1500_ServiceLine(drAmt);

                    break;
                }
                if (string.IsNullOrEmpty(drSelectedAcc.RowError))
                {
                    m_rAccUpdate.GetRecords(string.Format("Account = '{0}'", drSelectedAcc["ACCOUNT"]));

                    string strAccUpdateWhere = string.Format("Account = '{0}'", drSelectedAcc["ACCOUNT"]);
                    string strMsg = "";
                    int nRecUpdate = m_rAccUpdate.UpdateField("status", "SSI1500", strAccUpdateWhere, out strMsg);

                    string strWhere = string.Format("account = '{0}'", m_rAccUpdate.m_strAccount);
                    string strErr;

                    int nPatRec = m_rPat.GetActiveRecords(strWhere);

                    if (nPatRec == 1)
                    {
                        nPatRec = m_rPat.UpdateField("h1500_date", DateTime.Today.ToShortDateString(), strWhere, out strErr);
                        if (nPatRec != 1)
                        {
                            m_Err.m_Logfile.WriteLogFile(strErr);
                        }
                        nPatRec = m_rPat.UpdateField("ssi_batch", m_strInterchageControlNumber, strWhere, out strErr);
                        if (nPatRec != 1)
                        {
                            m_Err.m_Logfile.WriteLogFile(strErr);
                        }
                    }
                }
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlCommand cmdInsert = new SqlCommand(
                        string.Format("insert into data_billing_history " +
                        "(account, ins_abc, pat_name, fin_code, ins_code, " +
                        "trans_date, run_date, batch, ebill_status, ebill_batch, " +
                        "text, mod_date, mod_user, mod_prg, mod_host) " +
                        "VALUES " +
                        " ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', " +
                        "'{6}', '{7}', '{8}', '{9}', '{10}', " +
                        "'{11}',suser_sname(),'{12}',host_name() ) ",
                        drSelectedAcc["account"].ToString()
                            , drIns[0]["ins_a_b_c"].ToString()
                                , drSelectedAcc["pat_name"].ToString()
                                    , drSelectedAcc["fin_code"].ToString()
                                        , drIns[0]["ins_code"].ToString()
                        , drSelectedAcc["trans_date"].ToString()
                            , DateTime.Today,
                                m_strInterchageControlNumber,
                                    m_strFilter
                                        , m_strInterchageControlNumber
                        , m_sb1500.ToString()
                            , DateTime.Now
                                , propAppName
                        )
                        , conn);
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.InsertCommand = cmdInsert;
                    sda.InsertCommand.Connection.Open();
                    try
                    {
                        sda.InsertCommand.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        sda.InsertCommand.Connection.Close();
                    }

                }
            }

            Create1500_5010_SE();
            Create1500_5010_GE();
            Create1500_IEA();

            if (DateTime.Now < new DateTime(2013, 08, 16, 14, 0, 0))
            {
                return;
            }

            string strFile = string.Format(@"C:\temp\1500\1500_{0}.x12", DateTime.Now.ToString("yyyyMMddHHmm"));
            StreamWriter sw = new StreamWriter(strFile);
            sw.AutoFlush = true;

            foreach (string str in m_alFile)
            {
                sw.Write(str);
            }
            sw.Close();

            // wdk 20130829
            m_nHLCounter = 1;
            if (m_bHas1500Error) // don't write to the upload directory.
            {
                return;
            }
            try
            {
                if (m_strDatabase.Contains("LIVE"))
                {
                    string strFile2 = string.Format(@"\\wth251\ssiapp\SSI\00852\Billing\Uploadp\MCL\input\1500_{0}.IN", DateTime.Now.ToString("yyyyMMddHHmm"));
                    StreamWriter sw2 = new StreamWriter(strFile2);
                    sw2.AutoFlush = true;

                    foreach (string str in m_alFile)
                    {

                        sw2.Write(str);
                    }

                    sw2.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}\r\n\n{1}", ex.GetType().ToString(), ex.Message), propAppName);
            }
        }

        /// <summary>
        /// 2420A Rendering Provider Speciality Info.
        /// </summary>
        private void Create1500_5010_RenderingProviderPRV()
        {
            string strPRV = string.Format("PRV*PE*PXC*291U00000X~");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPRV;
            m_alFile.Add(strPRV);
            m_nSTSegments++;

        }

        /// <summary>
        /// 2010BB Payer Name
        /// </summary>
        /// <param name="dataRow"></param>
        private void Create1500_5010_PayerName(DataRow drINS)
        {
            string strPayerNM1 = string.Format("NM1*PR*2*{0}*****PI*{1}~",
               drINS["plan_nme"].ToString().Trim(),
              // drINS["provider_no"].ToString().Trim()
              drINS["payer_no"].ToString().Trim());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayerNM1;
            m_alFile.Add(strPayerNM1);
            m_sb1500.AppendFormat("{0}\r\n", strPayerNM1);
            m_nSTSegments++;

            string strPayerN3 = string.Format("N3*{0}*{1}~",
               drINS["insc addr1"].ToString().Trim(),
               drINS["insc addr2"].ToString().Trim());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayerN3;
            m_alFile.Add(strPayerN3);
            m_sb1500.AppendFormat("{0}\r\n", strPayerN3);
            m_nSTSegments++;

            string striCSZ = drINS["insc csz"].ToString().Trim();
            string strarCity = null;
            string strarSt = null;
            string strarZip = null;
            if (striCSZ.Contains(','))
            {
                strarCity = striCSZ.Substring(0, striCSZ.IndexOf(',')).Trim();
                striCSZ = striCSZ.Replace(strarCity, "").Replace(",", "").Trim();
                if (striCSZ.Contains(" "))
                {
                    strarSt = striCSZ.Split(new string[] { " " }, StringSplitOptions.None)[0].Trim();
                    strarZip = striCSZ.Split(new string[] { " " }, StringSplitOptions.None)[1].Trim();
                    if (strarZip.Contains("-"))
                    {
                        strarZip = strarZip.Replace("-", "");
                    }
                    if (strarZip.Contains(" "))
                    {
                        strarZip = strarZip.Replace(" ", "");
                    }
                }

            }

            string strPayerN4 = string.Format("N4*{0}*{1}*{2}~",
               strarCity,
               strarSt,
               strarZip);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayerN4;
            m_alFile.Add(strPayerN4);
            m_sb1500.AppendFormat("{0}\r\n", strPayerN4);
            m_nSTSegments++;
        }

        /// <summary>
        /// 2010BA Subscriber NAME
        /// </summary>
        /// <param name="dataRow">Current Insurance row</param>
        private void Create1500_SubscriberName(DataRow drINS)
        {
            // rgc/wdk 20120321 get the insurance from the insurance table if it is 
            // BLUECARE/TNCARE SEL then the name can only be 20 Alphabetic characters. No hypens, apostrophes or spaces
            string strInsCode = "";
            try
            {
                strInsCode = m_dsBilling.Tables["INS"].Rows[0]["plan_nme"].ToString().ToUpper().Trim();
            }
            catch (Exception)
            {
            }
            string strName = drINS["holder_nme"].ToString();
            string strLastName = string.Empty;
            string strFirstName = string.Empty;
            string strMiddleName = string.Empty;
            string strNameSuffix = string.Empty;
            if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(","))
            {
                // rgc/wdk 20120510 modified to handle names like "JAMES,JAMES"
                strLastName = strName.Substring(0, strName.IndexOf(","));
                string strReplace = string.Format("{0},", strLastName).Trim();
                strName = strName.Replace(strReplace, "").Trim();
                //strName = strName.Replace(",", "").Trim();
                if (strLastName.Contains(" "))
                {
                    string[] strNameSplit = strLastName.Split(new string[] { " " }, StringSplitOptions.None);
                    strLastName = strNameSplit[0].Trim();
                    strNameSuffix = strNameSplit[1].Trim();
                    if (!m_alNameSuffix.Contains(strNameSuffix))
                    {
                        strLastName += strNameSuffix;
                        strNameSuffix = "";
                    }

                }
                // rgc/wdk 20120321 added to handle 
                if (strInsCode == "BLUECARE/TNCARE SEL")
                {
                    strLastName = strLastName.Replace(" ", "").Replace("'", "").Replace("-", "");
                    strLastName = strLastName.Substring(0, strLastName.Length > 20 ? 20 : strLastName.Length);
                }

                strFirstName = strName;
                if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(" "))
                {
                    strFirstName = strName.Substring(0, strName.IndexOf(" ")).Trim();
                    strMiddleName = strName.Replace(strFirstName, "").Trim();
                }
            }
            string strNM1 = string.Format("NM1*IL*1*{0}*{1}*{2}**{3}*MI*{4}~",
                strLastName, strFirstName, strMiddleName, strNameSuffix,
                drINS["policy_num"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sb1500.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;

            string strAddr = drINS["addr"].ToString().Replace(".", " ");
            string strN3 = string.Format("N3*{0}~",
                strAddr);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sb1500.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string[] strNoFail = new string[] { "", "", "", "" };
            // wdk 20120615 below crashes when "PORT SAINT LUCIE" is the city
            //string[] strCSZ = drINS["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            //strCSZ.CopyTo(strNoFail, 0);
            // try this instead

            string[] strAddrIns = drINS["city_st_zip"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] strCSZ = new string[] { "", "", "", "" };
            try
            {
                strCSZ = strAddrIns[1].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch
            {
                MessageBox.Show(string.Format("Account [{0}] has an invalid address. The batch will need to be cleared, the address corrected and the file recreated.", drINS["account"].ToString()));
            }
            //string[] strCSZ = strAddrIns[1].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            strAddrIns.CopyTo(strNoFail, 0);
            strCSZ.CopyTo(strNoFail, 1);

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                 strNoFail[0], strNoFail[1], strNoFail[2]); ;
            if (!string.IsNullOrEmpty(strNoFail[3]))
            {
                strN4 = string.Format("N4*{0}*{1}*{2}~",
                    string.Format("{0} {1}", strNoFail[0], strNoFail[1]).Trim(), strNoFail[2], strNoFail[3]);
            }
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sb1500.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;

            string strDOB = "";
            DateTime dtDOB = DateTime.MaxValue;
            if (DateTime.TryParse(drINS["holder_dob"].ToString(), out dtDOB))
            {
                strDOB = dtDOB.ToString("yyyyMMdd");
            }
            string strPatDMG = string.Format("DMG*D8*{0}*{1}~",
                strDOB,
                drINS["holder_sex"]);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatDMG;
            m_alFile.Add(strPatDMG);
            m_sb1500.AppendFormat("{0}\r\n", strPatDMG);
            m_nSTSegments++;


        }

        /// <summary>
        /// 2310D Loop Service Facility Location
        /// </summary>
        private void Create1500_ServiceFacilityInfo()
        {
            // wdk 20120123 added to try to get billing provider npi not the same as rendering/performing
            string strClaimRenderingProviderNM1 = string.Format("NM1*LI*2*{0}*****XX*{1}~",
                GetSystemParameter("company_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("npi_number") ?? "1720160708"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimRenderingProviderNM1;
            m_alFile.Add(strClaimRenderingProviderNM1);
            m_nSTSegments++;

            string strPayToProviderN3 = string.Format("N3*{0}~",
                GetSystemParameter("company_address") ?? "620 SKYLINE DRIVE"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderN3;
            m_alFile.Add(strPayToProviderN3);
            m_nSTSegments++;


            string strPayToProviderN4 = string.Format("N4*{0}*{1}*{2}~",
                GetSystemParameter("company_city") ?? "JACKSON",
                GetSystemParameter("company_state") ?? "TN",
                GetSystemParameter("company_zip") ?? "383013923"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderN4;
            m_alFile.Add(strPayToProviderN4);
            m_nSTSegments++;


            string strClaimServiceProviderRef = string.Format("REF*TJ*{0}~",
                GetSystemParameter("fed_tax_id") ?? "626010402");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimServiceProviderRef;
            m_alFile.Add(strClaimServiceProviderRef);
            m_nSTSegments++;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drIns"></param>
        private void Create1500_5010_ClaimRefs(DataRow[] drIns)
        {
            foreach (DataRow dr in drIns)
            {
                string strCLMRef = null;
                if (m_dicRefs.ContainsKey(dr["ins_code"].ToString()))
                {
                    strCLMRef = m_dicRefs[dr["ins_code"].ToString()];
                    if (strCLMRef.StartsWith("REF**"))
                    {
                        strCLMRef = null;
                    }
                }
                // don't add blank refs to the file.
                if (!string.IsNullOrEmpty(strCLMRef))
                {
                    rtbDoc.Text += Environment.NewLine;
                    rtbDoc.Text += strCLMRef;
                    m_alFile.Add(strCLMRef);
                    m_nSTSegments++;
                }
            }
        }


        /// <summary>
        /// rgc/wdk 20110728 added 
        /// 2010AB Loop Situational when the pay-to is not the billing in our case it is the same except
        /// addresses are different
        /// </summary>
        private void Create1500_PayToProviderName()
        {
            // we have mostly rewritten this to 5010. Some parts are failing in 4010 differences are shown
            // for easy conversion to 5010
            // wdk 20120123 for 5010 use this
            //string strPayToProviderName = "NM1*87*2*MEDICAL CENTER LABORATORY~";
            // wdk 20120123 for 4010 use this
            string strPayToProviderName = string.Format("NM1*87*2*{0}*****XX*{1}~",
                GetSystemParameter("company_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("npi_number") ?? "1720160708"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderName;
            m_alFile.Add(strPayToProviderName);
            m_sb1500Header.AppendFormat("{0}\r\n", strPayToProviderName);
            m_nSTSegments++;

            string strPayToProviderN3 = string.Format("N3*{0}~",
                GetSystemParameter("remit_to_address") ?? "PO BOX 3099"
                ); //wdk 20130905
            //string strPayToProviderN3 = "N3*620 SKYLINE DRIVE~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderN3;
            m_alFile.Add(strPayToProviderN3);
            m_sb1500Header.AppendFormat("{0}\r\n", strPayToProviderN3);
            m_nSTSegments++;


            string strPayToProviderN4 = string.Format("N4*{0}*{1}*{2}~",
                GetSystemParameter("remit_to_city") ?? "JACKSON",
                GetSystemParameter("remit_to_state") ?? "TN",
                GetSystemParameter("remit_to_zip") ?? "383033099" 
                ); // wdk 20130905
            //string strPayToProviderN4 = "N4*JACKSON*TN*383013923~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderN4;
            m_alFile.Add(strPayToProviderN4);
            m_sb1500Header.AppendFormat("{0}\r\n", strPayToProviderN4);
            m_nSTSegments++;

            //string strClaimRenderingProviderRef = "REF*1B*003091277~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimRenderingProviderRef;
            //m_alFile.Add(strClaimRenderingProviderRef);
            //m_sb1500Header.AppendFormat("{0}\r\n",strClaimRenderingPoviderRef);
            //m_nSTSegments++;

            string strClaimRenderingProviderRef = string.Format("REF*EI*{0}~",
                GetSystemParameter("fed_tax_id") ?? "626010402"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimRenderingProviderRef;
            m_alFile.Add(strClaimRenderingProviderRef);
            m_sb1500Header.AppendFormat("{0}\r\n", strClaimRenderingProviderRef);
            m_nSTSegments++;

        }


        /// <summary>
        ///  REFERRING PROVIDER LOOP 2310A
        /// </summary>
        /// <param name="dataRow">Insurance Record</param>
        private void Create1500_5010_ReferringProviderInfo(DataRow dataRow)
        {
            string strRefProviderLastName = dataRow["last_name"].ToString();
            if (string.IsNullOrEmpty(strRefProviderLastName))
            {
                m_Err.m_Logfile.WriteLogFile(string.Format("referring provider physician last name is blank for account {0}", dataRow["account"].ToString()));
            }
            int nDex = -1;
            string[] strArr = new string[] { "-", "*" };
            foreach (string str in strArr)
            {
                strRefProviderLastName = strRefProviderLastName.Replace(str, " ");
            }
            if ((nDex = strRefProviderLastName.IndexOfAny(new char[] { '-' })) > -1)
            {
                strRefProviderLastName.Replace("-", " ");
            }

            // use the insurance row not the pat row            
            string strClaimReferringProviderNM1 = string.Format("NM1*DN*1*{0}*{1}*{2}***XX*{3}~",
                    strRefProviderLastName,
                    dataRow["first_name"].ToString(),
                    dataRow["mid_init"].ToString(),
                    dataRow["phy_id"].ToString());

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimReferringProviderNM1;
            m_alFile.Add(strClaimReferringProviderNM1);
            m_sb1500.AppendFormat("{0}\r\n", strClaimReferringProviderNM1);
            m_nSTSegments++;
        }

        /// <summary>
        /// RENDERING PROVIDER LOOP 2310B
        /// called twice in 4010 for SSI hospital has an edit called
        ///         xx      **CARRIER EDIT** [VERSION 04/27/2011]
        ///          SOURCE =  PAYOR TESTING
        ///          PERFORMING DOCTOR LAST NAME                              edit      177
        ///          PERFORMING DOCTOR LAST NAME CANNOT BE BLANK.
        ///     that can be satisified if passed in 2420E loops (837 standards say pass only once)
        /// </summary>
        private void Create1500_RenderingProviderInfo(int nLoop)
        {
            /* wdk 20120123 this loop provides the following error telling us the rendering provider cannot
             * be the billing provider NPI cannot be the same as this npi going to send the LI instead of 82
                                                                            edit #
          BILLING PROVIDER NPI          1720160708                          1343
     xx      IF THE RENDERING PROVIDER NPI EXIST (ID TYPE IDENTIFIER = 82
             (RENDERING/PERFORMING ) AND ID QUALIFIER = XX AND NPI)(CLM), THE
             NPI CANNOT BE THE SAME AS THE BILLING PROVIDER NPI.
             - LOOP 2310B NM108 -  *** HIPAA ANSI GENERIC EDIT *** [VERSION
             07/08/2011]
             SOURCE = WPC 837 5010 PROFESSIONAL IMPLEMENTATION GUIDE
             * */
            //string strClaimRenderingProviderNM1 = @"NM1*82*2*MEDICAL CENTER LABORATORY******~";
            // removed for testing 20120123
            string strClaimRenderingProviderNM1 = @"NM1*82*1*CENTER LABORATORY*MEDICAL****XX*1720160708~";


            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimRenderingProviderNM1;
            m_alFile.Add(strClaimRenderingProviderNM1);
            m_nSTSegments++;

            // end of 20120123 testing

            /* should not be needed
            string strPayToProviderN3 = "N3*620 SKYLINE DRIVE~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderN3;
            m_alFile.Add(strPayToProviderN3);
            m_nSTSegments++;


            string strPayToProviderN4 = "N4*JACKSON*TN*383013923~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPayToProviderN4;
            m_alFile.Add(strPayToProviderN4);
            m_nSTSegments++;
             */

            //// may need to reinstate this
            //string strClaimServiceProviderRef = "REF*EI*626010402~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strClaimServiceProviderRef;
            //m_alFile.Add(strClaimServiceProviderRef);
            //m_nSTSegments++;


        }


        /// <summary>
        /// SERVICE LINE SERVICE PROVIDER LOOP 2420C
        /// if this loop is to be used you must add the service facility address to the method.
        /// </summary>
        /// <param name="dr"></param>
        private void Create1500_ClaimServiceProvider(DataRow dr)
        {

            string str = dr[0].ToString();
            str = dr[1].ToString();

            string strClaimServiceProviderNM1 = string.Format("NM1*77*2*{0}*****XX*{1}~",
                GetSystemParameter("billing_entity_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("npi_number") ?? "1720160708"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strClaimServiceProviderNM1;
            m_alFile.Add(strClaimServiceProviderNM1);
            m_nSTSegments++;

        }

        ArrayList m_alDiagnosis = new ArrayList();
        /// <summary>
        /// Loading all nine we capture but only need the first four.
        /// </summary>
        /// <param name="drPAT"></param>
        /// <returns></returns>
        private int LoadDiagnosisArray(DataRow drPAT)
        {
            m_alDiagnosis = new ArrayList();
            //drPAT["icd9_1"].ToString().Replace(".", "")
            try
            {
                if (!string.IsNullOrEmpty(drPAT["icd9_1"].ToString().Replace(".", "")))
                {
                    m_alDiagnosis.Add(drPAT["icd9_1"].ToString().Replace(".", ""));
                }

                if (!m_alDiagnosis.Contains(drPAT["icd9_2"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_2"].ToString().Replace(".", ""));
                    }
                }


                if (!m_alDiagnosis.Contains(drPAT["icd9_3"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_3"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_3"].ToString().Replace(".", ""));
                    }
                }


                if (!m_alDiagnosis.Contains(drPAT["icd9_4"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_4"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_4"].ToString().Replace(".", ""));
                    }
                }

                if (!m_alDiagnosis.Contains(drPAT["icd9_5"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_5"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_5"].ToString().Replace(".", ""));
                    }
                }


                if (!m_alDiagnosis.Contains(drPAT["icd9_6"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_6"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_6"].ToString().Replace(".", ""));
                    }
                }


                if (!m_alDiagnosis.Contains(drPAT["icd9_7"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_7"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_7"].ToString().Replace(".", ""));
                    }
                }


                if (!m_alDiagnosis.Contains(drPAT["icd9_8"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_8"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_8"].ToString().Replace(".", ""));
                    }
                }

                if (!m_alDiagnosis.Contains(drPAT["icd9_9"].ToString().Replace(".", "")))
                {
                    if (!string.IsNullOrEmpty(drPAT["icd9_9"].ToString().Replace(".", "")))
                    {
                        m_alDiagnosis.Add(drPAT["icd9_9"].ToString().Replace(".", ""));
                    }
                }


            }
            catch (IndexOutOfRangeException)
            {
                // 
            }

            if (m_alDiagnosis.Count != 0)
            {
                for (int i = (m_alDiagnosis.Count - 1); i >= 0; i--)
                {
                    if (string.IsNullOrEmpty(m_alDiagnosis[i].ToString()))
                    {
                        m_alDiagnosis.RemoveAt(i);
                    }
                }
            }


            return m_alDiagnosis.Count;
        }

        /// <summary>
        /// 2300 Loop Health Care Diagnosis Code
        /// </summary>
        /// <param name="drPAT"></param>
        private void Create1500_DiagnosisInfo(DataRow drPAT)
        {
            string strIndicator = "BK";
            string strIndicator2 = "BF";
            if (drPAT["icd_indicator"].ToString() == "I10")
            {
                strIndicator = "ABK";
                strIndicator2 = "ABF";
            }

            int n = LoadDiagnosisArray(drPAT);
            string strHI = string.Format("HI*{12}:{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}~",
               drPAT["icd9_1"].ToString().Replace(".", ""),
               string.IsNullOrEmpty(drPAT["icd9_2"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_2"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_3"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_3"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_4"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_4"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_5"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_5"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_6"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_6"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_7"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_7"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_8"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_8"].ToString().Replace(".", ""), strIndicator2),
               string.IsNullOrEmpty(drPAT["icd9_9"].ToString().Trim()) ? "" : string.Format("*{1}:{0}", drPAT["icd9_9"].ToString().Replace(".", ""), strIndicator2),
               "", "", "", strIndicator //once converted to using the patDx Table add the additional icd's here

               );

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHI;
            m_alFile.Add(strHI);
            m_sb1500.AppendFormat("{0}\r\n", strHI);
            m_nSTSegments++;
        }

        /// <summary>
        /// 2000C Patient Hierachchial level and
        /// 2010CA Patient Name Loops
        /// SITUATIONAL required when the Patient is not the subscriber.
        /// </summary>
        /// <param name="drPAT"></param>
        /// <param name="strPolicyIdentifier"></param>
        private void Create1500_PatInfo(DataRow drPAT, string strPolicyIdentifier)
        {
            // rgc/wdk 20120321 get the insurance from the insurance table if it is 
            // BLUECARE/TNCARE SEL then the name can only be 20 Alphabetic characters. No hypens, apostrophes or spaces
            string strInsCode = "";
            try
            {
                strInsCode = m_dsBilling.Tables["INS"].Rows[0]["plan_nme"].ToString().ToUpper().Trim();
            }
            catch (Exception)
            {
            }
            string strPatHL = string.Format("HL*{0}*{1}*23*0~",
                m_nHLCounter++, m_nHLParent);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatHL;
            m_alFile.Add(strPatHL);
            m_sb1500.AppendFormat("{0}\r\n", strPatHL);
            m_nSTSegments++;

            string strRelation = "19"; // child

            // other
            if (drPAT["relation"].ToString().Trim() == "09" ||
                drPAT["relation"].ToString().Trim() == "04") // rgc/wdk 20120118 added to handle MCLOE sending "04" to billing via PostWeb
            {
                strRelation = "G8";
            }
            // spouce
            if (drPAT["relation"].ToString().Trim() == "02")
            {
                strRelation = "01";
            }


            string strPat = string.Format("PAT*{0}~",
                strRelation);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPat;
            m_alFile.Add(strPat);
            m_sb1500.AppendFormat("{0}\r\n", strPat);
            m_nSTSegments++;

            string strName = drPAT["pat_name"].ToString();
            string strLastName = string.Empty;
            string strFirstName = string.Empty;
            string strMiddleName = string.Empty;
            string strNameSuffix = string.Empty;
            if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(","))
            {
                strLastName = strName.Substring(0, strName.IndexOf(","));
                strName = strName.Replace(strLastName, "").Trim();
                strName = strName.Replace(",", "").Trim();
                if (strLastName.Contains(" "))
                {
                    string[] strNameSplit = strLastName.Split(new string[] { " " }, StringSplitOptions.None);
                    strLastName = strNameSplit[0].Trim();
                    strNameSuffix = strNameSplit[1].Trim();
                    if (!m_alNameSuffix.Contains(strNameSuffix))
                    {
                        strLastName += strNameSuffix;
                        strNameSuffix = "";
                    }
                }
                // rgc/wdk 20120321 added to handle 
                if (strInsCode == "BLUECARE/TNCARE SEL")
                {
                    strLastName = strLastName.Replace(" ", "").Replace("'", "").Replace("-", "");
                    strLastName = strLastName.Substring(0, strLastName.Length > 20 ? 20 : strLastName.Length);
                }

                strFirstName = strName;
                if (!string.IsNullOrEmpty(strName.Trim()) && strName.Contains(" "))
                {
                    strFirstName = strName.Substring(0, strName.IndexOf(" ")).Trim();
                    strMiddleName = strName.Replace(strFirstName, "").Trim();
                }
            }

            // 2010CA Loop Patient Name
            string strPatName = string.Format("NM1*QC*1*{0}*{1}*{2}**{3}~",
               strLastName, strFirstName, strMiddleName, strNameSuffix);//,
                                                                        //strPolicyIdentifier);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatName;
            m_alFile.Add(strPatName);
            m_sb1500.AppendFormat("{0}\r\n", strPatName);
            m_nSTSegments++;


            string strPatN3 = string.Format("N3*{0}~",
                drPAT["pat_addr1"].ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatN3;
            m_alFile.Add(strPatN3);
            m_sb1500.AppendFormat("{0}\r\n", strPatN3);
            m_nSTSegments++;

            // wdk 20120615 below crashes when "PORT SAINT LUCIE" is the city
            //string[] strCSZ = drINS["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            //strCSZ.CopyTo(strNoFail, 0);
            // try this instead
            string[] strNoFail = { "", "", "", "" };
            string[] strAddrPat = drPAT["city_st_zip"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] strCSZ = strAddrPat[1].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            strAddrPat.CopyTo(strNoFail, 0);
            strCSZ.CopyTo(strNoFail, 1);
            //string[] strNoFail = new string[] { "", "", "", "" };
            //string[] strCSZ = drPAT["city_st_zip"].ToString().Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            //strCSZ.CopyTo(strNoFail, 0);

            string strPatN4 = string.Format("N4*{0}*{1}*{2}~",
                strNoFail[0], strNoFail[1], strNoFail[2]);
            if (!string.IsNullOrEmpty(strNoFail[3]))
            {
                strPatN4 = string.Format("N4*{0}*{1}*{2}~",
                string.Format("{0} {1}", strNoFail[0], strNoFail[1]).Trim(), strNoFail[2],
                strNoFail[3].LastIndexOf('-') == strNoFail[3].Length ? strNoFail[3].Replace("-", "") : strNoFail[3].Trim()
                );
            }
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatN4;
            m_alFile.Add(strPatN4);
            m_sb1500.AppendFormat("{0}\r\n", strPatN4);
            m_nSTSegments++;

            DateTime dtDOB = DateTime.Parse(drPAT["dob_yyyy"].ToString());
            string strPatDMG = string.Format("DMG*D8*{0}*{1}~",
                dtDOB.ToString("yyyyMMdd"),
                drPAT["sex"]);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPatDMG;
            m_alFile.Add(strPatDMG);
            m_sb1500.AppendFormat("{0}\r\n", strPatDMG);
            m_nSTSegments++;

            // LOOP 2010CA situational. shouldn't be needed because the secondary info is the same as the primary
            // wdk 20100316 Have no policy number in the pat table throw error.
            //string strREFSec = string.Format("REF*1G*{0}~",
            //    drPAT["policy_num"].ToString());
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strREFSec;
            //m_alFile.Add(strREFSec);
            //m_sb1500.AppendFormat("{0}\r\n",strREFSec);


        }

        /// <summary>
        /// 2400 Loop Id Service Line
        /// </summary>
        /// <param name="drAmt"></param>
        private void Create1500_ServiceLine(DataRow[] drAmt)
        {
            int nLX = 1;
            for (int i = 0; i <= drAmt.GetUpperBound(0); i++)
            {
                string strLX = string.Format("LX*{0}~", nLX++);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strLX;
                m_alFile.Add(strLX);
                m_sb1500.AppendFormat("{0}\r\n", strLX);
                m_nSTSegments++;
                string strCodePtr = drAmt[i]["diagnosis_code_ptr"].ToString();
                if (string.IsNullOrEmpty(strCodePtr))
                {
                    m_bHas1500Error = true;
                    MessageBox.Show(string.Format("Account {0} has a null code pointer. THIS FILE WILL NOT BE MOVED TO THE UPLOAD DIRECTORY.", drAmt[i][0]), propAppName);
                }
                /*                string strCodePtr = "A:";
                                if (!string.IsNullOrEmpty(drAmt[i]["diagnosis_code_ptr"].ToString()))
                                {
                                    string[] strAr = drAmt[i]["diagnosis_code_ptr"].ToString().Split(new char[] { ':' });
                                    foreach (string str in strAr)
                                    {
                                           strCodePtr +=  string.Format("{0}:", Convert.ToChar(int.Parse(str)+64));

                                    }
                                    if (strCodePtr.EndsWith(":"))
                                    {
                                        strCodePtr.Remove(strCodePtr.LastIndexOf(":"));
                                    }

                                }
                  */
                // rgc/wdk 20111117 removed the 81 from the service line so the reffering provider loop in 2300 will work
                //string strSRV = string.Format("SV1*HC:{0}{1}{2}*{3}*UN*{4}*81**{5}~",
                // SSI edit for SV105 requires that the "81" below be submitted for 1500 although pt 335 of the 837P manual says it does not have to be provided
                //  provided to circumvent the SSI edit.
                string strSRV = string.Format("SV1*HC:{0}{1}{2}*{3}*UN*{4}*81**{5}~",
                    drAmt[i]["cpt4"],
                    string.IsNullOrEmpty(drAmt[i]["modi"].ToString().Trim()) ? "" : string.Format(":{0}", drAmt[i]["modi"]),
                    string.IsNullOrEmpty(drAmt[i]["modi2"].ToString().Trim()) ? "" : string.Format(":{0}", drAmt[i]["modi2"]),
                    double.Parse(drAmt[i]["amount"].ToString()).ToString("F2"),
                    drAmt[i]["qty"],
                    strCodePtr);

                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strSRV;
                m_alFile.Add(strSRV);
                m_sb1500.AppendFormat("{0}\r\n", strSRV);
                m_nSTSegments++;


                DateTime dtTransDate = DateTime.Parse(drAmt[i]["trans_date"].ToString());
                string strTransDate = dtTransDate.ToString("yyyyMMdd");
                string strCLMDate = string.Format("DTP*472*D8*{0}~",
                  strTransDate);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strCLMDate;
                m_alFile.Add(strCLMDate);
                m_sb1500.AppendFormat("{0}\r\n", strCLMDate);
                m_nSTSegments++;

                // CLIA 
                string strCLIA = @"REF*X4*44D0315453~";
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strCLIA;
                m_alFile.Add(strCLIA);
                m_sb1500.AppendFormat("{0}\r\n", strCLIA);
                m_nSTSegments++;

            }

            //   return dRetVal;

        }

        /// <summary>
        /// LOOP ID 2300 CLAIM INFO
        /// </summary>
        /// <param name="dr"></param>
        private void Create1500_ClaimHeader(DataRow drChrg)
        {
            string strAccount = drChrg["account"].ToString();
            DataTable dttotal = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = new SqlCommand(string.Format("select sum(qty*net_amt) as [Total Chrg] " +
                " from chrg " +
                " where account = '{0}'", strAccount), m_sqlConnection);
            sda.Fill(dttotal);
            // wdk 20120214 change back to 81
            // rgc/wdk 20111117 changed from 81 - independent Lab to 11 - Office for SSI
            //string strCLM = string.Format("CLM*{0}*{1}***81::1*Y*A*Y*Y*B~",
            string strCLM = string.Format("CLM*{0}*{1}***81:B:*Y*A*Y*Y*P~",
                  strAccount,
                  double.Parse(dttotal.Rows[0]["Total Chrg"].ToString()).ToString("F2"));
            //double.Parse(dr["net_amt"].ToString()).ToString("F2"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strCLM;
            m_alFile.Add(strCLM);
            m_sb1500.AppendFormat("{0}\r\n", strCLM);
            m_nSTSegments++;
        }
        /// <summary>
        /// LOOP ID 2000BA Subscriber and patient are the same.
        /// </summary>
        /// <param name="dr"></param>
        private void Create1500_SubscriberInfo(DataRow drINS)
        {
            string strRespSeqNo = "P";
            if (drINS["ins_a_b_c"].ToString() == "B")
            {
                strRespSeqNo = "S";
            }
            if (drINS["ins_a_b_c"].ToString() == "C")
            {
                strRespSeqNo = "T";
            }

            // wdk 20111031 added strlaimFilingIndocatorCode
            string strClaimFilingIndicatorCode;

            if (!m_dicClaimFilingIndicatorCode.TryGetValue(drINS["provider_no_qualifier"].ToString().Trim(), out strClaimFilingIndicatorCode))
            {
                strClaimFilingIndicatorCode = "CI";
            }

            // wdk 20120123 changed to us relations codes from 5010 although not on error list
            // the change for the plan_name that is necessary may cause this to be an issue
            string strRelation = drINS["relation"].ToString() == "01" ? "18" : "";

            string strGrpNum = drINS["grp_num"].ToString();
            if (!string.IsNullOrEmpty(strGrpNum))
            {
                if (strGrpNum.StartsWith("\0"))
                {
                    strGrpNum = "";
                }
            }
            /*
             * Just FYI we are getting a new UB edit this morning:  I have contacted SSI and this is a new 5010 edit.  Julie is going in the bill format and taking out our group name in FL 61
            Email from Debbie Thornhill 02/17/2011
                INSURANCE GROUP NUMBER (PRI) AND INSURANCE GROUP NAME (PRI) CANNOT BOTH BE PRESENT.
                - 2000B SBR03 AND 04 -  **** HIPAA ANSI GENERIC EDIT ****   [VERSION 02/15/2012]
                SOURCE = WPC 837 5010 IMPLEMENTATION GUIDE

             * */

            string strSBR = string.Format("SBR*{0}*" + // claim type Primary etc
                                                "{1}*" + // Pat relation
                                                "{2}*" + // Subscriber Group or policy number
                                                "{3}*" + //
                                                "****{4}~",
                strRespSeqNo,
                strRelation,
                strGrpNum,//string.IsNullOrEmpty(drINS["grp_num"].ToString())? "": drINS["grp_num"].ToString(),
                          //drINS["grp_nme"].ToString(),
                string.IsNullOrEmpty(strGrpNum) ? drINS["grp_nme"].ToString() : "", // wdk 20120123 ssi edit if primary group number exists then primary group name must be blank 
                strClaimFilingIndicatorCode);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strSBR;
            m_alFile.Add(strSBR);
            m_sb1500.AppendFormat("{0}\r\n", strSBR);
            m_nSTSegments++;

            if (!string.IsNullOrEmpty(drINS["plan_addr1"].ToString()))
            {
                string strPayorN3 = string.Format("N3*{0}~",
               drINS["plan_addr1"]);
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strPayorN3;
                m_alFile.Add(strPayorN3);
                m_sb1500.AppendFormat("{0}\r\n", strPayorN3);
                m_nSTSegments++;

                // make a place for the address
                string[] strPayorNoFail = new string[] { "", "", "", "" };
                // get the city by splitting on the comma only this makes
                // the zero element the city and the one element the rest of the address
                string[] strAddrIns = drINS["plan_csz"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                // split the one element into parts using the space
                string[] strPayorCSZ = strAddrIns[1].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                // copy the city to the NoFail array's first element
                strAddrIns.CopyTo(strPayorNoFail, 0);
                // copy the rest of the address to the second and subsequent elements
                strPayorCSZ.CopyTo(strPayorNoFail, 1);

                string strPayorN4 = string.Format("N4*{0}*{1}*{2}~",
                     strPayorNoFail[0], strPayorNoFail[1].Trim(), strPayorNoFail[2].Replace("-", "").Trim()); ;
                if (!string.IsNullOrEmpty(strPayorNoFail[3]))
                {
                    strPayorN4 = string.Format("N4*{0}*{1}*{2}~",
                        string.Format("{0} {1}", strPayorNoFail[0], strPayorNoFail[1]).Trim(), strPayorNoFail[2], strPayorNoFail[3].Replace("-", "").Trim());
                }
                rtbDoc.Text += Environment.NewLine;
                rtbDoc.Text += strPayorN4;
                m_alFile.Add(strPayorN4);
                m_sb1500.AppendFormat("{0}\r\n", strPayorN4);
                m_nSTSegments++;

            }
        }


        private void Create1500_ST_Header()
        {
            m_strTransSetControlNumber = string.Format("{0:D6}", m_nST);
            m_strST = string.Format("ST*837*{0}~", m_strTransSetControlNumber);
            m_nST++;
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strST;
            m_alFile.Add(m_strST);
            m_sb1500Header.AppendFormat("{0}\r\n", m_strST);
            //m_nSTSegments++; ST and SE included in starting count

            m_strBHT = string.Format("BHT*0019*00*{0}*{1}*{2}*CH~",
                   m_strInterchageControlNumber.PadRight(6),
                    DateTime.Now.ToString("yyyyMMdd"),
                        DateTime.Now.ToString("HHmm"));
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strBHT;
            m_alFile.Add(m_strBHT);
            m_sb1500Header.AppendFormat("{0}\r\n", m_strBHT);
            m_nSTSegments++;

            // rgc/wdk 20120117 removed per crosswalk
            //string strBHTRef = string.Format("REF*87*005010X222***~");
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strBHTRef;
            //m_alFile.Add(strBHTRef);
            //m_sb1500Header.AppendFormat("{0}\r\n",strBHTRef);
            //m_nSTSegments++;


        }

        private void Create1500_Envelope()
        {
            string strNum = m_rNumber.GetNumber("ssi_batch");
            m_strInterchageControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}",
                         DateTime.Now.Year,
                                 strNum)));

            tsslBatch.Text = string.Format("Batch: {0}", m_strInterchageControlNumber);

            m_strISA = string.Format("ISA*00*          *00*          *ZZ*{0}*ZZ*{1}*" +
                "{2}*{3}*U*00401*{4}*1*{5}*:~",
                m_strSubmitterId.PadLeft(15),
                string.Format("ZMIXED").PadLeft(15), // 8
                DateTime.Now.ToString("yyMMdd").ToString(), // 9
                DateTime.Now.ToString("HHmm").ToString(), // 10
                m_strInterchageControlNumber,
                PropProductionEnvironment);
            rtbDoc.Text = m_strISA;
            m_alFile.Add(m_strISA);
            m_sb1500Header.AppendFormat("{0}\r\n", m_strISA);

            m_strGS = string.Format("GS*HC*{0}*ZMIXED*{1}*{2}*{3}*X*004010X098A1~",
                m_strSubmitterId.PadRight(10),
                DateTime.Now.ToString("yyyyMMdd"),
                DateTime.Now.ToString("HHmm"),
                m_strInterchageControlNumber);

            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strGS;
            m_alFile.Add(m_strGS);
            m_sb1500Header.AppendFormat("{0}\r\n", m_strGS);
        }

        /// <summary>
        /// LOOP ID 2000B
        /// </summary>
        private void Create1500_SubscriberHL(string strRelation)
        {
            m_nHLParent = m_nHLCounter;
            // see page 115 of 837 P x222.pdf
            string strHL =
                string.Format("HL*{0}*1*22*{1}~",
                    m_nHLCounter++, strRelation == "01" ? 0 : 1);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHL;
            m_alFile.Add(strHL);
            m_sb1500.AppendFormat("{0}\r\n", strHL);
            m_nSTSegments++;
        }
        /// <summary>
        /// LOOP ID 2010AA -- BILLING PROVIDER NAME for all other 1500's
        /// </summary>
        private void Create1500_BillingProviderName()
        {
            string strPRV = string.Format("PRV*BI*PXC*291U00000X~");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPRV;
            m_alFile.Add(strPRV);
            m_sb1500Header.AppendFormat("{0}\r\n", strPRV);
            m_nSTSegments++;

            string strNM1 = string.Format("NM1*85*2*{0}*****XX*{1}~",
                GetSystemParameter("billing_entity_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("npi_number") ?? "1720160708"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_sb1500Header.AppendFormat("{0}\r\n", strNM1);
            m_nSTSegments++;

            string strN3 = string.Format("N3*{0}~",
                GetSystemParameter("billing_entity_street") ?? "620 SKYLINE DRIVE"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_sb1500Header.AppendFormat("{0}\r\n", strN3);
            m_nSTSegments++;

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                GetSystemParameter("billing_entity_city") ?? "JACKSON",
                GetSystemParameter("billing_entity_state") ?? "TN",
                GetSystemParameter("billing_entity_zip") ?? "383013923"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_sb1500Header.AppendFormat("{0}\r\n", strN4);
            m_nSTSegments++;

            string strRefEI = string.Format("REF*EI*{0}~",
                GetSystemParameter("fed_tax_id") ?? "626010402"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefEI;
            m_alFile.Add(strRefEI);
            m_sb1500Header.AppendFormat("{0}\r\n", strRefEI);
            m_nSTSegments++;

            /* wdk 20130806 added for bluecross*/
            /* wdk 20130808 only necessary for UBOP's
            string strRefIB = "REF*1B*003091277~";
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefIB;
            m_alFile.Add(strRefIB);
            m_sb1500Header.AppendFormat("{0}\r\n",strRefIB);
            m_nSTSegments++;
             * */
        }

        /// <summary>
        /// LOOP ID 2010AA -- BILLING PROVIDER NAME for Champus
        /// </summary>
        private void Create1500_BillingProviderNameChampus()
        {
            string strPRV = string.Format("PRV*BI*PXC*291U00000X~");
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPRV;
            m_alFile.Add(strPRV);
            m_nSTSegments++;

            string strNM1 = string.Format("NM1*85*2*{0}*****XX*{1}~",
                GetSystemParameter("billing_entity_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("npi_number") ?? "1720160708"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNM1;
            m_alFile.Add(strNM1);
            m_nSTSegments++;

            string strN3 = string.Format("N3*{0}~",
                GetSystemParameter("billing_entity_street") ?? "620 SKYLINE DRIVE"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN3;
            m_alFile.Add(strN3);
            m_nSTSegments++;

            string strN4 = string.Format("N4*{0}*{1}*{2}~",
                GetSystemParameter("billing_entity_city") ?? "JACKSON",
                GetSystemParameter("billing_entity_state") ?? "TN",
                GetSystemParameter("billing_entity_zip") ?? "383013923"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strN4;
            m_alFile.Add(strN4);
            m_nSTSegments++;

            //string strRefIB = "REF*1B*003091277~";
            //rtbDoc.Text += Environment.NewLine;
            //rtbDoc.Text += strRefIB;
            //m_alFile.Add(strRefIB);
            //m_nSTSegments++;

            string strRefEI = string.Format("REF*EI*{0}*031~",
                GetSystemParameter("fed_tax_id") ?? "626010402"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRefEI;
            m_alFile.Add(strRefEI);
            m_nSTSegments++;
        }

        /// <summary>
        /// LOOP ID 2000A
        /// </summary>
        private void Create1500_BillingProviderHL()
        {
            string strHL = string.Format("HL*{0}**20*1~",
                m_nHLCounter++);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strHL;
            m_alFile.Add(strHL);
            m_sb1500Header.AppendFormat("{0}\r\n", strHL);
            m_nSTSegments++;
        }
        /// <summary>
        /// LOOP ID 1000B
        /// </summary>
        private void Create1500_ReceiverName()
        {
            // TODO get info from SSI for this segment
            string strRec = string.Format("NM1*40*2*{0}*****46*{1}~",
                    "SSI", "SSI Code");
            //string strInsCode = m_dsBilling.Tables["INS"].Rows[0]["plan_nme"].ToString().ToUpper().Trim();
            //if (strInsCode == "BLUE CROSS")
            //{
            //        strRec = "NM1*40*2*BC*****46*1000427~";
            //}


            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strRec;
            m_alFile.Add(strRec);
            m_sb1500Header.AppendFormat("{0}\r\n", strRec);
            m_nSTSegments++;
        }

        /// <summary>
        /// LOOP ID 1000A
        /// </summary>
        private void Create1500_SubmitterName()
        {
            // create the NM1 Submitter Name
            string strNm1 = string.Format("NM1*41*2*{0}*****46*{1}~",
                GetSystemParameter("billing_entity_name") ?? "MEDICAL CENTER LABORATORY",
                GetSystemParameter("fed_tax_id") ?? "626010402"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strNm1;
            m_alFile.Add(strNm1);
            m_sb1500Header.AppendFormat("{0}\r\n", strNm1);
            m_nSTSegments++;

            // create the per 
            string strPer = string.Format("PER*IC*{0}*TE*{1}*EM*{2}~",
                GetSystemParameter("billing_contact") ?? "CAROL PLUMLEE",
                GetSystemParameter("billing_phone") ?? "7315417320",
                GetSystemParameter("billing_email") ?? "carol.plumlee@wth.org"
                );
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += strPer;
            m_alFile.Add(strPer);
            m_sb1500Header.AppendFormat("{0}\r\n", strPer);
            m_nSTSegments++;
        }

        /// <summary>
        /// LOOP ID Envelope -- TRANSACTION SET TRAILER
        /// </summary>
        private void Create1500_5010_SE()
        {
            m_strSE = string.Format("SE*{0}*{1}~",
             m_nSTSegments, m_strTransSetControlNumber);
            //m_strSE = string.Format("SE*837*{0}*{1}*CH~",
            //    m_nSTSegments,
            //        DateTime.Now.ToString("yyyyMMdd"),
            //            DateTime.Now.ToString("HHmm").ToString());
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strSE;
            m_alFile.Add(m_strSE);

        }

        /// <summary>
        /// LOOP ID Envelope -- FUNCTIONAL GROUP TRAILER
        /// </summary>
        private void Create1500_5010_GE()
        {
            m_strGE = string.Format("GE*{0}*{1}~",
                m_nFunctionalGroups,
                m_strInterchageControlNumber);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strGE;
            m_alFile.Add(m_strGE);

        }

        /// <summary>
        /// LOOP ID Envelope -- INTERCHANGE CONTROL TRAILER
        /// </summary>
        private void Create1500_IEA()
        {
            m_strIEA = string.Format("IEA*{0}*{1}~",
                m_nFunctionalGroups,
                m_strInterchageControlNumber);
            rtbDoc.Text += Environment.NewLine;
            rtbDoc.Text += m_strIEA;
            m_alFile.Add(m_strIEA);

        }


        private void tsmiPrintText_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();

            pd.Document = printDocument1;
            m_stringToPrint = new StringReader(rtbDoc.Text);
            m_nPage = 1;
            printDocument1.Print();

        }
        int m_nPage = 0;
        void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {

            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = 25;// ev.MarginBounds.Left;
            float topMargin = 50;// ev.MarginBounds.Top;
            string line = null;

            Font printFont = rtbDoc.Font;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = m_stringToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());

                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
            {
                ev.HasMorePages = true;

            }
            else
            {
                ev.HasMorePages = false;
            }
            yPos = ev.MarginBounds.Bottom + (2 *
                  printFont.GetHeight(ev.Graphics));
            ev.Graphics.DrawString(string.Format("Page {0}", m_nPage++), printFont, Brushes.Black,
                leftMargin, yPos, new StringFormat());

        }

        private void dgvRecords_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = @"C:\Program Files\Medical Center Laboratory\MCL Billing\acc.exe";

            p.StartInfo.Arguments = string.Format("{0}{1} /{2}",
                m_strDatabase[0] == '/' ? "" : @"/",
                m_strDatabase,
                ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString());
            p.Start();
        }

        private void rtbDoc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tsslRecCount.Text = string.Format("{0} Lines", rtbDoc.Lines.GetUpperBound(0) + 1);
        }

        private void LoadGrid_UB_Click(object sender, EventArgs e)
        {
            //   btnStart.Enabled = true;
            ////   m_strFilter = "";
            //   LoadGrid("UB");


        }


        static int nPage = 0;
        int nLen = 0;
        private void pAGEToolStripMenuItem_Click(object sender, EventArgs e)
        {

            rtbDoc.Text = "";
            int nLines = 56;
            string strLine = null;
            for (int i = nPage; i < m_alFile.Count; i++)
            {
                strLine = string.Format("{0}\r\n", m_alFile[i].ToString());
                nLen += strLine.Length;

                rtbDoc.Text += strLine;

                if (i == m_alFile.Count)
                {
                    MessageBox.Show("You have reached the end of the file.");
                    return;
                }
                if (i == (nPage + nLines))
                {
                    nPage += ++nLines;
                    break;
                }
                if (nLen > Int32.MaxValue)
                {
                    nPage = ++i;
                    nLen = 0;
                    MessageBox.Show("Rich text box max exceeded. Print and continue with page.");
                    return;
                }


            }


        }

        private void tsbPrintGrid_Click(object sender, EventArgs e)
        {
            m_ViewerPrintDocument = new PrintDocument();
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, "SSI", m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
            int nRows = dgvRecords.Rows.Count;
            if (nRows == 0)
            {
                MessageBox.Show("No records ready to print.");
                return;
            }
            m_ViewerPrintDocument.DocumentName = m_strFilter;

            m_rgReport.m_dgvpReport.propTitle = string.Format("{0} in batch {1}",
                m_strFilter, m_strInterchageControlNumber);
            m_rgReport.m_dgvpReport.propFooterText = string.Format("[{0} Records", nRows);
            try
            {
                m_ViewerPrintDocument.Print();
            }
            catch (IndexOutOfRangeException)
            {
                // keep on trucking.
            }
            m_ViewerPrintDocument.PrintPage -= new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);

        }

        /// <summary>
        /// this gets all UB's that are Medicare/Cigna
        /// Medicare/Cigna are billed with Jackson General address and info the others
        /// are billed with Medical Center Lab info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void medicareCIGNAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            m_strFilter = "MEDICARE";
            LoadGrid("UB");

        }

        /// <summary>
        /// this gets all UB's that are not Medicare/Cigna
        /// Medicare/Cigna are billed with Jackson General address and info these
        /// are billed with Medical Center Lab info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allOthersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            m_strFilter = "OTHER UB";
            LoadGrid("UB");

        }

        private void outPatientBillingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            m_strFilter = "OUTPATIENT";
            LoadGrid("UBOP");
        }

        private void tsbPrintView_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Normal;

            Bitmap[] bmps = RFClassLibrary.dkPrint.Capture(dkPrint.CaptureType.Form);
            try
            {
                bmps[0].Save(@"C:\Temp\ViewerX12.bmp");
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                RFClassLibrary.dkPrint.propStreamToPrint = new StreamReader(@"C:\Temp\ViewerX12.bmp");
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
            //   this.WindowState = FormWindowState.Maximized;
        }


        private void dgvRecords_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            tsslRecCount.Text = string.Format(" {0} Record{1}.", dgvRecords.Rows.Count,
                dgvRecords.Rows.Count > 1 ? "s" : "");
        }

        private void tsmiChampus_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            m_strFilter = "CHAMPUS";
            LoadGrid("CHAMPUS");
        }

        private void tsmiAllOther1500_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            m_strFilter = "1500";
            LoadGrid("1500");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            string strBatch = tsslBatch.Text;
            string strIns = "";
            //string strAcc = null;

            // allow users to enter a batch if one is not on form
            Form f = new Form();
            f.Text = "ENTER SELECTION";

            Button bYes = new Button();
            bYes.Text = "YES";
            bYes.Location = new Point((f.Size.Width / 2) - (bYes.Width / 2), (f.Size.Height - 30) / 2);
            bYes.DialogResult = DialogResult.Yes;
            bYes.TabIndex = 3;

            Button bNo = new Button();
            bNo.Location = new Point((f.Size.Width / 2) - (bNo.Width / 2), (f.Size.Height + 30) / 2);
            bNo.Text = "NO";
            bNo.DialogResult = DialogResult.No;
            bNo.TabIndex = 4;

            TextBox tBatch = new TextBox();
            tBatch.Dock = DockStyle.Fill;
            tBatch.TabIndex = 0;
            tBatch.Text = string.Format("Batch: {0}", strBatch);
            tBatch.CharacterCasing = CharacterCasing.Upper;
            tBatch.Tag = "BATCH: ";
            tBatch.SelectAll();

            TextBox tType = new TextBox();
            //  tType.Dock = DockStyle.Left;
            tType.Location = new Point(0, 25);
            tType.Width = f.Width;
            tType.TabIndex = 1;
            tType.Text = "TYPE: ";
            tType.Tag = "TYPE: ";
            tType.CharacterCasing = CharacterCasing.Upper;
            tType.SelectAll();

            TextBox tIns = new TextBox();
            tIns.Location = new Point(0, 50);
            tIns.Width = f.Width;
            tIns.TabIndex = 2;
            tIns.Text = "INS: ";
            tIns.Tag = "INS: ";
            tIns.CharacterCasing = CharacterCasing.Upper;


            f.Controls.Add(tBatch);
            f.Controls.Add(tType);
            f.Controls.Add(bYes);
            f.Controls.Add(bNo);
            f.Controls.Add(tIns);
            tBatch.Select();


            if (f.ShowDialog() == DialogResult.Yes)
            {
                strBatch = tBatch.Text.Replace(tBatch.Tag.ToString(), "");
                m_strType = tType.Text.Replace(tType.Tag.ToString(), "");
                strIns = tIns.Text.Replace(tIns.Tag.ToString(), "");
            }
            else
            {
                return;
            }
            if (string.IsNullOrEmpty(m_strType) ||
                m_strType == "Type:")
            {
                m_strType = "";
                MessageBox.Show("Must enter a type.");
                return;
            }
            if (string.IsNullOrEmpty(strBatch) ||
                strBatch == "Batch:")
            {
                m_strType = "";
                MessageBox.Show("No batch selected for undo.");
                return;
            }

            //}
            if (strBatch.Contains("Batch: "))
            {
                strBatch = strBatch.Remove(0, 7).Trim();
            }

            if (MessageBox.Show(string.Format("Are you sure you want to clear batch {0}?", strBatch), "WARNING", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            SqlCommand cmdSelect = new SqlCommand(
                string.Format("select pat.account, 'Batch [{0}] cleared' as comment from Pat " +
                " inner join acc on acc.account = pat.account " +
                " where ssi_batch = '{0}'{1}", strBatch
                , string.IsNullOrEmpty(strIns) ? "" : string.Format(" and fin_code = '{0}'", strIns)
                ), m_sqlConnection);
            SqlDataAdapter sda = new SqlDataAdapter();


            SqlCommand cmdUpdate = new SqlCommand(
            string.Format("update acc " +
                "set status = replace(status, 'SSI', ''), " +
                "mod_prg = '{0}', mod_date = '{1}', mod_user = '{2}', mod_host = '{3}' " +
                " where acc.account in ( select acc.account from acc " +
                " inner join pat on pat.account = acc.account " +
                "where ssi_batch = '{4}'{5})", string.Format("{0} {1}", Application.ProductName, Application.ProductVersion),
                DateTime.Today, Environment.UserName, Environment.MachineName, strBatch
                , string.IsNullOrEmpty(strIns) ? "" : string.Format(" and acc.fin_code = '{0}'", strIns)
                )
                , m_sqlConnection);

            sda.UpdateCommand = cmdUpdate;
            sda.UpdateCommand.CommandTimeout = 120;
            sda.UpdateCommand.Connection.Open();

            SqlTransaction transAccUpdate = sda.UpdateCommand.Connection.BeginTransaction("AccUpdate");
            sda.UpdateCommand.Transaction = transAccUpdate;


            int nUpdates = -1;
            try
            {
                nUpdates = sda.UpdateCommand.ExecuteNonQuery();
                transAccUpdate.Commit();
            }
            catch (SqlException se)
            {
                transAccUpdate.Rollback();
                string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
                strText += "\r\n\r\nModule: btnClear_Click()";
                MessageBox.Show(strText, propAppName);
                return; // if we can't update the Acc record don't update the pat record until this is resolved.
            }
            finally
            {
                sda.UpdateCommand.Connection.Close();
            }


            if (m_strType == "1500")
            {
                cmdUpdate = new SqlCommand(
                    string.Format("update pat " +
                    "set ssi_batch = NULL, " +
                    "h1500_date = NULL " +
                    "from pat inner join acc on acc.account = pat.account " +
                    " where ssi_batch = '{0}'{1}", strBatch, string.IsNullOrEmpty(strIns) ? "" : string.Format(" and acc.fin_code = '{0}'", strIns)
                    )
                , m_sqlConnection);

                sda = new SqlDataAdapter();
                sda.UpdateCommand = cmdUpdate;
                sda.UpdateCommand.CommandTimeout = 120;
                sda.UpdateCommand.Connection.Open();

                nUpdates = -1;
                try
                {
                    nUpdates = sda.UpdateCommand.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
                    strText += "\r\n\r\nModule: btnClear_Click() PatUpdate";
                    MessageBox.Show(strText);
                }
                finally
                {
                    sda.UpdateCommand.Connection.Close();
                }
            }

            if (m_strType == "UB" ||
                    m_strType == "UBOP")
            {

                cmdUpdate = new SqlCommand(
                    string.Format("update pat " +
                    "set ssi_batch = NULL, " +
                    "ub_date = NULL " +
                    "from pat inner join acc on acc.account = pat.account " +
                    " where ssi_batch = '{0}'{1}", strBatch, string.IsNullOrEmpty(strIns) ? "" : string.Format(" and acc.fin_code = '{0}'", strIns)
                    )
                , m_sqlConnection);

                sda = new SqlDataAdapter();
                sda.UpdateCommand = cmdUpdate;
                sda.UpdateCommand.CommandTimeout = 120;
                sda.UpdateCommand.Connection.Open();

                nUpdates = -1;
                try
                {
                    nUpdates = sda.UpdateCommand.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    string strText = string.Format("Application: {0}\r\n\r\nError Type: {1}\r\n\r\nError:{2}", Application.ProductName, se.GetType().ToString(), se.Message);
                    strText += "\r\n\r\nModule: btnClear_Click() PatUpdate";
                    MessageBox.Show(strText);
                }
                finally
                {
                    sda.UpdateCommand.Connection.Close();
                }
            }

            MessageBox.Show("Batch Cleared");

        }

        private void tsbFileLoad_Click(object sender, EventArgs e)
        {
            rtbDoc.Text = "";
            foreach (ToolStripItem tsi in tsMain.Items)
            {
                tsi.Enabled = false;
            }
            tcMain.SelectedTab = tpDoc;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Temp";
            ofd.Filter = "Claim Files|*.x12";
            ofd.Multiselect = false;

            StreamReader streamReader;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                streamReader = new StreamReader(ofd.OpenFile());
                rtbDoc.Text = streamReader.ReadToEnd().Replace("~", string.Format("~{0}", Environment.NewLine));
            }

            foreach (ToolStripItem tsi in tsMain.Items)
            {
                tsi.Enabled = true;
            }
        }

        private void rtbDoc_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                rtbDoc.ZoomFactor += 1;
            }
            if (e.Button == MouseButtons.Right)
            {
                rtbDoc.Find("clm*");
            }
        }

        int nLast = 0;
        string strSearch = "";
        private void rtbDoc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Alt)
            {
                if (e.Shift)
                {
                    if (rtbDoc.ZoomFactor + (-.5f) <= 0.015)
                    {
                        return;
                    }
                    rtbDoc.ZoomFactor -= .5f;
                }
                else
                {
                    if (rtbDoc.ZoomFactor + (.5f) >= 64.0)
                    {
                        return;
                    }
                    rtbDoc.ZoomFactor += .5f;
                }

            }
            if (e.Shift)
            {
                //string strSearch = "";
                Form f = new Form();
                f.Text = "SEARCH SELECTION";

                Button bYes = new Button();
                bYes.Text = "YES";
                bYes.Location = new Point((f.Size.Width / 2) - (bYes.Width / 2), (f.Size.Height - 30) / 2);
                bYes.DialogResult = DialogResult.Yes;
                bYes.TabIndex = 3;

                Button bNo = new Button();
                bNo.Location = new Point((f.Size.Width / 2) - (bNo.Width / 2), (f.Size.Height + 30) / 2);
                bNo.Text = "NO";
                bNo.DialogResult = DialogResult.No;
                bNo.TabIndex = 4;

                TextBox tSearch = new TextBox();
                tSearch.Dock = DockStyle.Fill;
                tSearch.TabIndex = 0;
                tSearch.Text = string.Format("SEARCH FOR: {0}", strSearch);
                tSearch.CharacterCasing = CharacterCasing.Upper;
                tSearch.Tag = "SEARCH FOR: ";
                tSearch.SelectAll();

                f.Controls.Add(tSearch);
                f.Controls.Add(bYes);
                f.Controls.Add(bNo);
                tSearch.Select();


                if (f.ShowDialog() == DialogResult.Yes)
                {
                    strSearch = tSearch.Text.Replace(tSearch.Tag.ToString(), "");
                    //m_strType = tType.Text.Replace(tType.Tag.ToString(), "");
                    //strIns = tIns.Text.Replace(tIns.Tag.ToString(), "");
                }
                else
                {
                    return;
                }
                //if (string.IsNullOrEmpty(m_strType) ||
                //    m_strType == "Type:")
                //{
                //    m_strType = "";
                //    MessageBox.Show("Must enter a type.");
                //    return;
                //}
                if (string.IsNullOrEmpty(strSearch) ||
                    strSearch == "SEARCH FOR:")
                {
                    //m_strType = "";
                    MessageBox.Show("Search criteria is empty.");
                    return;
                }

                //}
                if (strSearch.Contains("SEARCH FOR: "))
                {
                    strSearch = strSearch.Remove(0, 12).Trim();
                }
                try
                {
                    nLast = rtbDoc.Find(strSearch, nLast, RichTextBoxFinds.None) + strSearch.Length;
                    rtbDoc.SelectionBackColor = Color.Red;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("[{0}] was not found.\r\nError: {1}", strSearch, ex.Message), "SEARCH ERROR");

                }

            }
            if (e.Control)
            {
                try
                {
                    nLast = rtbDoc.Find(strSearch, nLast, RichTextBoxFinds.None) + strSearch.Length;
                    rtbDoc.SelectionBackColor = Color.Red;
                }
                catch (Exception)
                {
                    // keep on trucking
                }
            }
        }

        private void frmSSI_Shown(object sender, EventArgs e)
        {
            #region Removed Code
            /*
            //----------
            //string strBatch;
            //string strIns;
            //Form f;
            //TextBox tBatch;
            //TextBox tType;
            //TextBox tIns;
            string strErr = string.Format("error {0} {1} {2}", 10, 20, 30);
            //Form fMsg = 
            CreateMessageForm(strErr);
                //out strBatch, out strIns, out f, out tBatch, out tType, out tIns);

           // fMsg.Show();
            

            //------------
            string strDirPath = @"C:\fred:\2500";
            try
            {
                if (!Directory.Exists(strDirPath))
                {
                    Directory.CreateDirectory(strDirPath);
                }
            }
            catch (Exception ex)
            {
                // does nothing if the directory already exists
                string strErr1 =
               string.Format("METHOD:\t\t {0}.\r\nEXCEPTION:\t {1}\r\nERROR TYPE:\t {2}.",
                   MethodBase.GetCurrentMethod().Name, "ex.Message", "ex.GetType()");
                CreateMessageForm(strErr1);

            }
            finally
            {
               string strErr2 =
               string.Format("METHOD:\t\t {0}.\r\nEXCEPTION:\t {1}\r\nERROR TYPE:\t {2}.",
                   MethodBase.GetCurrentMethod().Name, "ex.Message", "ex.GetType()");
                CreateMessageForm(strErr2);
               
            }

            //StreamWriter sw = new StreamWriter(strDirectory);
            //sw.AutoFlush = true;
            //string strFile = string.Format(@"{0}\1500_{1}.x12", strDirectory,
            //               DateTime.Now.ToString("yyyyMMddHHmm"));
            //try
            //{

            //    sw = new StreamWriter(strFile);
            //    sw.Write("this is a test");

            //}
            //catch (DirectoryNotFoundException dnfe)
            //{

            //    sw.Write("what just happened");
            //    sw = new StreamWriter(strFile);

            //}
            //finally
            //{
            //    sw.Close();
            //}
            //Environment.Exit(13);

            */
            #endregion

        }

        private void CreateMessageForm(string strErr)
        //out string strBatch, out string strIns, out Form f, out TextBox tBatch, out TextBox tType, out TextBox tIns)
        {
            // create a printable MessageBox
            //string strValue1 = null;
            //string strValue2 = null;
            //string strValue3 = null;
            // strBatch = "null";
            //    strIns = "null";
            Form f = new Form();
            f.Text = string.Format("{0} ERROR", propAppName);
            f.StartPosition = FormStartPosition.CenterScreen;

            Button bYes = new Button();
            bYes.Text = "YES";
            bYes.Location = new Point((f.Size.Width / 2 - 30) - (bYes.Width / 2), (f.Size.Height) / 2);
            bYes.DialogResult = DialogResult.Yes;
            bYes.TabIndex = 3;

            Button bNo = new Button();
            bNo.Location = new Point((f.Size.Width / 2 + 30) - (bNo.Width / 2), (f.Size.Height) / 2);
            bNo.Text = "NO";
            bNo.DialogResult = DialogResult.No;
            bNo.TabIndex = 4;

            TextBox tMsg = new TextBox();
            tMsg.Dock = DockStyle.Fill;
            tMsg.TabIndex = 0;
            tMsg.ReadOnly = true;
            tMsg.Text = strErr;// string.Format("Batch: {0}", strValue1);
            tMsg.CharacterCasing = CharacterCasing.Upper;
            tMsg.Tag = "ERROR";
            //tMsg.SelectAll();


            f.Controls.Add(tMsg);
            f.Controls.Add(bYes);
            f.Controls.Add(bNo);

            // return f;
            f.Show();
            tsmiPrintView_Click(null, null);
        }

    }

}
