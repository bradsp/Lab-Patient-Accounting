using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
// programmer added
using MCL; // for eob recordset
using RFClassLibrary;
using System.Collections;
using System.IO;
using System.Drawing.Printing;
using Microsoft.Data.SqlClient; // for listobject

namespace LabBilling.Legacy
{
    /// <summary>
    /// Class for PrintEOB
    /// </summary>
    public partial class PrintEOBForm : Form
    {
        SqlConnection m_sqlConnection;
        Dictionary<string, string> m_dicReason = new Dictionary<string, string>();
        private Font printFontBold;
        private Font printFontRegular;
        ArrayList m_alPrint = new ArrayList();
        private CEob m_eob;
        private ERR m_ERR;
        private string m_strDatabase = "";
        private string m_strServer = "";
        private string m_strAccount = "";
        /// <summary>
        /// PrintEOB Constructor.
        /// </summary>
        /// <param name="args"></param>
        public PrintEOBForm(string[] args)
        {
            InitializeComponent();

            if (args.Length != 3)
            {
                MessageBox.Show("Incorrect number of arguments. Needs server, database and account.");
                return;
            }

            m_strServer = args[0];
            m_strDatabase = args[1];

            m_strAccount = args[2];

            m_sqlConnection = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                + "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] strErrLogArray = new string[] { string.Format("{0}", m_strServer.IndexOf("LIVE") > -1 ? "LIVE" : "TEST"), m_strServer, m_strDatabase };
            m_ERR = new ERR(strErrLogArray);

            // create the eob recordset
            m_eob = new CEob(m_strServer, m_strDatabase, ref m_ERR);

            LoadDataGrid();// set the forms title text with the account number and text.
            this.Text += string.Format(" Account {0} - {1}", m_eob.m_Reob.m_strAccount, m_eob.m_Reob.m_strSubscriberName);

            tsbPrint.Enabled = false;

            LoadClaimsAdjustmentCodes();

        }

        private void LoadClaimsAdjustmentCodes()
        {
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlCommand cmdSelect = new SqlCommand("select * from dict_claim_adjustment_codes", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmdSelect);
                DataTable dtInfo = new DataTable();
                int nRec = sda.Fill(dtInfo);
                //tscbClaimAdjCodes.ComboBox.DataSource = dtInfo;
                //tscbClaimAdjCodes.ComboBox.DisplayMember = "code";
                //tscbClaimAdjCodes.ComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
                //tscbClaimAdjCodes.ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //tscbClaimAdjCodes.ComboBox.ValueMember = "description";

                //((RichTextBox)m_txtMsg.Control).DataBindings.Add("text", dtInfo, "description");
                foreach (DataRow dr in dtInfo.Rows)
                {
                    m_dicReason.Add(dr["code"].ToString(), dr["description"].ToString());
                }

            }
        }

        /// <summary>
        /// Mods needed return a dataset from the CEOB class that has done this then use the dataset to display
        /// </summary>
        private void LoadDataGrid()
        {
            // get the records in the table
            int nRecCount = m_eob.m_Reob.GetActiveRecords(string.Format("account = '{0}'", m_strAccount));
            if (nRecCount < 1)
            {
                if (m_strAccount[1].Equals('A')) //09/25/2008 wdk/rgc modified to account for second character to be "A" or not to be "A"
                {
                    nRecCount = m_eob.m_Reob.GetActiveRecords(string.Format("account = '{0}'", m_strAccount.Replace("A", "")));
                    if (nRecCount < 1)
                    {

                        MessageBox.Show("This account is not listed in the EOB tables.");
                        Environment.Exit(13);
                    }
                }
            }


            for (int i = 0; i < nRecCount; i++)
            {
                dgvSelection.Rows.Add(m_eob.m_Reob.m_strClaimStatus, m_eob.m_Reob.m_strEftDate, m_eob.m_Reob.m_strEftNumber, m_eob.m_Reob.m_strEftFile);
                m_eob.m_Reob.GetNext();
            }
            dgvSelection.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvSelection.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }


        private void CreateEOBDetailGridHeader()
        {
            throw new Exception("The method or operation is not implemented.");
        }


        /// <summary>
        /// Print handler for the only button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            m_eob.PrintEOB();
            tsbPrint.Enabled = false;
        }

        private void dgvSelection_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (((DataGridView)sender).Rows[e.RowIndex].IsNewRow)
            {
                return;
            }
            string strSelect = string.Format("account = '{0}' AND claim_status = '{1}' and eft_number = '{2}' ",
                               m_strAccount,
                                   dgvSelection.Rows[e.RowIndex].Cells[0].FormattedValue.ToString(),
                                        dgvSelection.Rows[e.RowIndex].Cells[2].FormattedValue.ToString());
            if (m_eob.m_Reob.GetActiveRecords(strSelect) != -1)
            {
                tsbPrint.Enabled = true;
            }
            else
            {
                MessageBox.Show(m_eob.propErrMsg);
            }

        }

        PrintDocument m_pd = new PrintDocument();
        private void tsbViewEOB_Click(object sender, EventArgs e)
        {
            printFontBold = new Font("Courier New", 10, FontStyle.Bold);
            printFontRegular = new Font("Courier New", 10, FontStyle.Regular);

            CreatePrintDocument();

            m_pd.DefaultPageSettings.Landscape = true;
            m_pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = m_pd;
            if (ppd.ShowDialog() == DialogResult.OK)
            {
                m_pd.Print();
            }
            m_pd.PrintPage -= new PrintPageEventHandler(pd_PrintPage);

        }

        private void CreatePrintDocument()
        {
            string strDetail;
            m_alPrint.Add(string.Format("Account: {0}\r\n", m_eob.m_Reob.m_strAccount));
            m_alPrint.Add(string.Format("Service Code Rev  Units APC# Allowed  Stat Wght    Date    Charges    Paid Reason  Adj Amt",
                                                 Environment.NewLine));
            int nRec = m_eob.m_ReobDetail.GetRecords(string.Format("rowguid = '{0}' order by ServiceCode, UID", m_eob.m_Reob.m_strRowguid));
            ArrayList alReasons = new ArrayList();
            while (nRec > 0)
            //while (m_eob.m_ReobDetail.propErrMsg != "EOF")
            {
                nRec--;
                // PR 2's are Patient Responsibility and 2 means the adjustment code is split between two lines this is the second so only the amount are to be printed.
                if (m_eob.m_ReobDetail.m_strReasonType == "PR" && m_eob.m_ReobDetail.m_strReasonCode == "2")
                {
                    string strBlank = "";
                    strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                                   m_eob.m_ReobDetail.m_strServiceCode.PadRight(13), //strBlank.PadRight(13),//wdk 20131114 for visual acuity
                                       strBlank.PadRight(5),//m_eobDetails.m_strRevCode.PadRight(5),
                                           strBlank.PadLeft(4),//m_eobDetails.m_strUnits.PadLeft(4),
                                               strBlank.PadLeft(5),//m_eobDetails.m_strApcNr.PadLeft(5),
                                                   strBlank.PadLeft(9),//m_eobDetails.m_strAllowedAmt.PadLeft(9),
                                                       strBlank.PadRight(5),//m_eobDetails.m_strStat.PadRight(5),
                                                           strBlank.PadLeft(6),//m_eobDetails.m_strWght.PadLeft(6),
                                                               strBlank.PadRight(10),//m_eobDetails.m_strDateOfService.PadRight(10),
                                                                   strBlank.PadLeft(8),//m_eobDetails.m_strChargeAmt.PadLeft(8),
                                                                       strBlank.PadLeft(8),//m_eobDetails.m_strPaidAmt.PadLeft(8),
                                                                           ((string)m_eob.m_ReobDetail.m_strReasonType + "/" + m_eob.m_ReobDetail.m_strReasonCode).PadRight(6),
                                                                               m_eob.m_ReobDetail.m_strOtherAdjAmt.Trim().PadLeft(8));// wdk 20131114 changed from 10
                    m_alPrint.Add(strDetail);

                }
                else
                {
                    strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                    m_eob.m_ReobDetail.m_strServiceCode.PadRight(13),
                        m_eob.m_ReobDetail.m_strRevCode.PadRight(5),
                            m_eob.m_ReobDetail.m_strUnits.PadLeft(4),
                                m_eob.m_ReobDetail.m_strApcNr.PadLeft(5),
                                    m_eob.m_ReobDetail.m_strAllowedAmt.PadLeft(9),
                                        m_eob.m_ReobDetail.m_strStat.PadRight(5),
                                            m_eob.m_ReobDetail.m_strWght.PadLeft(6),
                                                m_eob.m_ReobDetail.m_strDateOfService.PadRight(10),
                                                    m_eob.m_ReobDetail.m_strChargeAmt.PadLeft(8),
                                                        m_eob.m_ReobDetail.m_strPaidAmt.PadLeft(8),
                                                            ((string)m_eob.m_ReobDetail.m_strReasonType + "/" + m_eob.m_ReobDetail.m_strReasonCode).PadRight(6),
                                                            m_eob.m_ReobDetail.m_strContractualAdjAmt == "0.00" ? m_eob.m_ReobDetail.m_strOtherAdjAmt.PadLeft(8) : m_eob.m_ReobDetail.m_strContractualAdjAmt.PadLeft(8));//,
                    m_alPrint.Add(strDetail);
                    if (!alReasons.Contains(m_eob.m_ReobDetail.m_strReasonCode))
                    {
                        alReasons.Add(m_eob.m_ReobDetail.m_strReasonCode);
                    }
                }
                m_eob.m_ReobDetail.GetNext();
            }
            m_alPrint.Add("\t\tEND...");
            if (alReasons.Count > 0)
            {
                m_alPrint.Add("");
                m_alPrint.Add("REASONS:");
                foreach (string strReason in alReasons)
                {
                    string strText;
                    m_dicReason.TryGetValue(strReason, out strText);
                    if (strText.Length <= 70)
                    {
                        m_alPrint.Add(string.Format("{0}:\t{1}", strReason, strText));
                    }
                    else
                    {
                        ArrayList alSplit = new ArrayList();
                        int nLenOrig = strText.Length;
                        while (strText.Length > 0)
                        {
                            int nLen = strText.Length;
                            int nPos = strText.IndexOf(" ",
                                strText.Length > 80 ? 80 : strText.Length);
                            if (nPos <= nLen)
                            {
                                if (nPos != -1)
                                {
                                    nLen = nPos;
                                }
                            }
                            string strInsert = strText.Substring(0, nLen);
                            m_alPrint.Add(string.Format("{0}{1}",
                                nLenOrig == strText.Length ? string.Format("{0}:\t", strReason.Trim()) : "\t\t"
                                , strText.Substring(0, nLen)));
                            strText = strText.Replace(strInsert, "");

                        }
                    }
                }
            }


        }


        void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {

            // set the format for tab stops           
            float[] tabStops = new float[] { 30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f, 30.0f };
            StringFormat strLineFmt = new StringFormat();
            strLineFmt.FormatFlags = StringFormatFlags.NoClip;
            strLineFmt.SetTabStops(30.0f, tabStops);

            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFontRegular.GetHeight(ev.Graphics);

            try
            {
                while (count < linesPerPage &&
                    ((line = m_alPrint[count].ToString()) != null))
                {
                    yPos = topMargin + (count *
                       printFontRegular.GetHeight(ev.Graphics));

                    if (count < 2)
                    {
                        ev.Graphics.DrawString(line, printFontBold, Brushes.Black,
                            leftMargin, yPos, strLineFmt);

                    }
                    else
                    {
                        ev.Graphics.DrawString(line, printFontRegular, Brushes.Black,
                            leftMargin, yPos, strLineFmt);
                    }
                    count++;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                line = null;
                ev.HasMorePages = false;
            }
            // if(line != null)
            if (string.IsNullOrEmpty(line))
                ev.HasMorePages = false;
            else
                ev.HasMorePages = true;


        }


    }
}