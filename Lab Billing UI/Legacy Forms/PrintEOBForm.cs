// programmer added
using Microsoft.Data.SqlClient; // for listobject
using System.Collections;
using System.Data;
using System.Drawing.Printing;
using Utilities;
using WinFormsLibrary;

namespace LabBilling.Legacy;

/// <summary>
/// Class for PrintEOB
/// </summary>
public partial class PrintEOBForm : Form
{
    private SqlConnection _sqlConnection;
    private Dictionary<string, string> _dicReason = new();
    private Font _printFontBold;
    private Font _printFontRegular;
    private readonly ArrayList _alPrint = new();
    private CEob _eob;
    private ERR _eRR;
    private readonly string _strDatabase = "";
    private readonly string _strServer = "";
    private readonly string _strAccount = "";
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

        _strServer = args[0];
        _strDatabase = args[1];

        _strAccount = args[2];

        _sqlConnection = new SqlConnection($"Data Source={_strServer}; Initial Catalog = {_strDatabase}; Integrated Security = 'SSPI'");
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        string[] strErrLogArray = new string[] { _strServer.IndexOf("LIVE") > -1 ? "LIVE" : "TEST", _strServer, _strDatabase };
        _eRR = new ERR(strErrLogArray);

        // create the eob recordset
        _eob = new CEob(_strServer, _strDatabase, ref _eRR);

        LoadDataGrid();// set the forms title text with the account number and text.
        this.Text += string.Format(" Account {0} - {1}", _eob.Reob.m_strAccount, _eob.Reob.m_strSubscriberName);

        tsbPrint.Enabled = false;

        LoadClaimsAdjustmentCodes();

    }

    private void LoadClaimsAdjustmentCodes()
    {
        using (SqlConnection conn = new SqlConnection(_sqlConnection.ConnectionString))
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
                _dicReason.Add(dr["code"].ToString(), dr["description"].ToString());
            }
        }
    }

    /// <summary>
    /// Mods needed return a dataset from the CEOB class that has done this then use the dataset to display
    /// </summary>
    private void LoadDataGrid()
    {
        // get the records in the table
        int nRecCount = _eob.Reob.GetActiveRecords(string.Format("account = '{0}'", _strAccount));
        if (nRecCount < 1)
        {
            if (_strAccount[1].Equals('A')) //09/25/2008 wdk/rgc modified to account for second character to be "A" or not to be "A"
            {
                nRecCount = _eob.Reob.GetActiveRecords(string.Format("account = '{0}'", _strAccount.Replace("A", "")));
                if (nRecCount < 1)
                {
                    MessageBox.Show("This account is not listed in the EOB tables.");
                    Environment.Exit(13);
                }
            }
        }


        for (int i = 0; i < nRecCount; i++)
        {
            dgvSelection.Rows.Add(_eob.Reob.m_strClaimStatus, _eob.Reob.m_strEftDate, _eob.Reob.m_strEftNumber, _eob.Reob.m_strEftFile);
            _eob.Reob.GetNext();
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
        _eob.PrintEOB();
        tsbPrint.Enabled = false;
    }

    private void dgvSelection_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (((DataGridView)sender).Rows[e.RowIndex].IsNewRow)
        {
            return;
        }
        string strSelect = string.Format("account = '{0}' AND claim_status = '{1}' and eft_number = '{2}' ",
                           _strAccount,
                               dgvSelection.Rows[e.RowIndex].Cells[0].FormattedValue.ToString(),
                                    dgvSelection.Rows[e.RowIndex].Cells[2].FormattedValue.ToString());
        if (_eob.Reob.GetActiveRecords(strSelect) != -1)
        {
            tsbPrint.Enabled = true;
        }
        else
        {
            MessageBox.Show(_eob.propErrMsg);
        }

    }

    private readonly PrintDocument _pd = new();
    private void tsbViewEOB_Click(object sender, EventArgs e)
    {
        _printFontBold = new Font("Courier New", 10, FontStyle.Bold);
        _printFontRegular = new Font("Courier New", 10, FontStyle.Regular);

        CreatePrintDocument();

        _pd.DefaultPageSettings.Landscape = true;
        _pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        PrintPreviewDialog ppd = new PrintPreviewDialog();
        ppd.Document = _pd;
        if (ppd.ShowDialog() == DialogResult.OK)
        {
            _pd.Print();
        }
        _pd.PrintPage -= new PrintPageEventHandler(pd_PrintPage);

    }

    private void CreatePrintDocument()
    {
        string strDetail;
        _alPrint.Add(string.Format("Account: {0}\r\n", _eob.Reob.m_strAccount));
        _alPrint.Add(string.Format("Service Code Rev  Units APC# Allowed  Stat Wght    Date    Charges    Paid Reason  Adj Amt",
                                             Environment.NewLine));
        int nRec = _eob.ReobDetail.GetRecords(string.Format("rowguid = '{0}' order by ServiceCode, UID", _eob.Reob.m_strRowguid));
        ArrayList alReasons = new();
        while (nRec > 0)
        //while (m_eob.m_ReobDetail.propErrMsg != "EOF")
        {
            nRec--;
            // PR 2's are Patient Responsibility and 2 means the adjustment code is split between two lines this is the second so only the amount are to be printed.
            if (_eob.ReobDetail.m_strReasonType == "PR" && _eob.ReobDetail.m_strReasonCode == "2")
            {
                string strBlank = "";
                strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                               _eob.ReobDetail.m_strServiceCode.PadRight(13), //strBlank.PadRight(13),//wdk 20131114 for visual acuity
                                   strBlank.PadRight(5),//m_eobDetails.m_strRevCode.PadRight(5),
                                       strBlank.PadLeft(4),//m_eobDetails.m_strUnits.PadLeft(4),
                                           strBlank.PadLeft(5),//m_eobDetails.m_strApcNr.PadLeft(5),
                                               strBlank.PadLeft(9),//m_eobDetails.m_strAllowedAmt.PadLeft(9),
                                                   strBlank.PadRight(5),//m_eobDetails.m_strStat.PadRight(5),
                                                       strBlank.PadLeft(6),//m_eobDetails.m_strWght.PadLeft(6),
                                                           strBlank.PadRight(10),//m_eobDetails.m_strDateOfService.PadRight(10),
                                                               strBlank.PadLeft(8),//m_eobDetails.m_strChargeAmt.PadLeft(8),
                                                                   strBlank.PadLeft(8),//m_eobDetails.m_strPaidAmt.PadLeft(8),
                                                                       ((string)_eob.ReobDetail.m_strReasonType + "/" + _eob.ReobDetail.m_strReasonCode).PadRight(6),
                                                                           _eob.ReobDetail.m_strOtherAdjAmt.Trim().PadLeft(8));// wdk 20131114 changed from 10
                _alPrint.Add(strDetail);

            }
            else
            {
                strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                _eob.ReobDetail.m_strServiceCode.PadRight(13),
                    _eob.ReobDetail.m_strRevCode.PadRight(5),
                        _eob.ReobDetail.m_strUnits.PadLeft(4),
                            _eob.ReobDetail.m_strApcNr.PadLeft(5),
                                _eob.ReobDetail.m_strAllowedAmt.PadLeft(9),
                                    _eob.ReobDetail.m_strStat.PadRight(5),
                                        _eob.ReobDetail.m_strWght.PadLeft(6),
                                            _eob.ReobDetail.m_strDateOfService.PadRight(10),
                                                _eob.ReobDetail.m_strChargeAmt.PadLeft(8),
                                                    _eob.ReobDetail.m_strPaidAmt.PadLeft(8),
                                                        ((string)_eob.ReobDetail.m_strReasonType + "/" + _eob.ReobDetail.m_strReasonCode).PadRight(6),
                                                        _eob.ReobDetail.m_strContractualAdjAmt == "0.00" ? _eob.ReobDetail.m_strOtherAdjAmt.PadLeft(8) : _eob.ReobDetail.m_strContractualAdjAmt.PadLeft(8));//,
                _alPrint.Add(strDetail);
                if (!alReasons.Contains(_eob.ReobDetail.m_strReasonCode))
                {
                    alReasons.Add(_eob.ReobDetail.m_strReasonCode);
                }
            }
            _eob.ReobDetail.GetNext();
        }
        _alPrint.Add("\t\tEND...");
        if (alReasons.Count > 0)
        {
            _alPrint.Add("");
            _alPrint.Add("REASONS:");
            foreach (string strReason in alReasons)
            {
                string strText;
                _dicReason.TryGetValue(strReason, out strText);
                if (strText.Length <= 70)
                {
                    _alPrint.Add(string.Format("{0}:\t{1}", strReason, strText));
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
                        string strInsert = strText[..nLen];
                        _alPrint.Add(string.Format("{0}{1}",
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
        StringFormat strLineFmt = new()
        {
            FormatFlags = StringFormatFlags.NoClip
        };
        strLineFmt.SetTabStops(30.0f, tabStops);

        float linesPerPage = 0;
        float yPos = 0;
        int count = 0;
        float leftMargin = ev.MarginBounds.Left;
        float topMargin = ev.MarginBounds.Top;
        string line = null;

        // Calculate the number of lines per page.
        linesPerPage = ev.MarginBounds.Height /
           _printFontRegular.GetHeight(ev.Graphics);

        try
        {
            while (count < linesPerPage &&
                ((line = _alPrint[count].ToString()) != null))
            {
                yPos = topMargin + (count *
                   _printFontRegular.GetHeight(ev.Graphics));

                if (count < 2)
                {
                    ev.Graphics.DrawString(line, _printFontBold, Brushes.Black,
                        leftMargin, yPos, strLineFmt);

                }
                else
                {
                    ev.Graphics.DrawString(line, _printFontRegular, Brushes.Black,
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