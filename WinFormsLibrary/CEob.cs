using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;
using System.Transactions;
using Utilities;
using MCL;

namespace WinFormsLibrary;

/// <summary>
/// 
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class CEob : RFCObject
{
    private readonly R_system _rSystem;
    private readonly string _strEobAgency;
    private readonly string _strEobAddress;
    private readonly string _strEobCSZ;
    private readonly string _strEobPhone;
    private int _nDetailRecCount = -1; // wdk 20090605 made member level

    // comittable transaction

    /// <summary>
    /// 
    /// </summary>
    public CommittableTransaction? PropCommittableTrans { get; set; }

    PrintDocument _printDoc;
    // for printing
    private FontStyle _fontStyle;
    private int _fontSize;

    /// <summary>
    /// 
    /// </summary>
    public int PropFontSize
    {
        get { return _fontSize; }
        set
        {
            _fontSize = value;
            _printFont = new Font("Courier New", _fontSize, PropFontStyle);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public FontStyle PropFontStyle
    {
        get { return _fontStyle; }
        set
        {
            _fontStyle = value;
            _printFont = new Font("Courier New", PropFontSize, _fontStyle);
        }
    }

    private Font _printFont;
    private float _leftMargin;
    private readonly float _topMargin;

    StringFormat _strLineFmt;
    //        public R_eob_detail m_eobDetails;
    // private string m_strServer;
    // private string m_strDatabase;
    //   List<string> m_strEOB;
    /// <summary>
    /// 
    /// </summary>
    public float LinePos = 0;

    private readonly ERR _err;
    /// <summary>
    /// This classes instance of the recordset for the eob table
    /// </summary>
    public R_eob Reob;
    /// <summary>
    /// 
    /// </summary>
    public R_eob_detail ReobDetail;

    private readonly string _strServer;
    /// <summary>
    /// 
    /// </summary>
    public string PropServer
    {
        get { return _strServer; }
    }
    private readonly string _strDatabase;
    /// <summary>
    /// 
    /// </summary>
    public string PropDatabase
    {
        get { return _strDatabase; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strServer"></param>
    /// <param name="strDatabase"></param>
    /// <param name="errLog"></param>
    public CEob(string strServer, string strDatabase, ref ERR errLog)
    {
        _strServer = strServer;
        _strDatabase = strDatabase;
        _err = errLog;
        Reob = new R_eob(_strServer, _strDatabase, ref errLog);
        ReobDetail = new R_eob_detail(_strServer, _strDatabase, ref errLog);
        // rgc/wdk 20120124 added system call for eob_address
        _rSystem = new R_system(_strServer, _strDatabase, ref errLog);
        string[] strTemp = _rSystem.GetValue("eob_address").Split(new char[] { '|' });
        _strEobAgency = strTemp[0];
        _strEobAddress = strTemp[1];
        _strEobCSZ = strTemp[2];
        _strEobPhone = strTemp[3];
        // create and initialize the print document
        PropFontSize = 11;
        PropFontStyle = FontStyle.Regular;
        _printFont = new Font("Courier New", PropFontSize, PropFontStyle); // 05/13/2008 wdk this is a fixed pitch font.

        _leftMargin = 25;
        _topMargin = 75;
        _strLineFmt = new StringFormat();
        _printDoc = new PrintDocument();


        _printDoc.DocumentName = "EOB";
        _printDoc.DefaultPageSettings.Margins = new Margins(25, 50, 75, 75);
        _printDoc.PrintPage += new PrintPageEventHandler(m_printDoc_PrintPage);

    }

    /// <summary>
    /// The error handler in the application will have it's m_strError set (see the ERR class)
    /// 
    /// </summary>
    /// <returns>1 for Created
    ///          0 for not created -- error thrown see m_strError of ERR class
    ///          -1 for Already exists</returns>
    public int CreateCommittableTransaction()
    {
        int nRetVal = 1;
        if (PropCommittableTrans == null)
        {
            try
            {
                TransactionOptions to = new()
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted // 05/21/2008 rgc/wdk so other processes can read the committed transactions without locking up the table.
                };
                PropCommittableTrans = new CommittableTransaction(to);
                Reob.propDBConnection.EnlistTransaction(PropCommittableTrans);
                _err.m_Logfile.WriteLogFile("CEOB::CreateCommittableTransaction() CREATED");
            }
            catch (Exception ex)
            {
                _err.m_Logfile.WriteLogFile("CEOB::CreateCommittableTransaction()");
                _err.m_Logfile.WriteLogFile(string.Format("Exception thrown while trying to create transaction: {0}\r\n ", ex.Message));
                nRetVal = 0;
            }
        }
        else
        {
            _err.m_Logfile.WriteLogFile("CEOB::CreateCommittableTransaction() ALREADY EXISTS");
            nRetVal = -1;
        }
        return nRetVal;
    }


    /// <summary>
    /// Add a NEW record to the table using a committable transaction. 
    /// If any record fails no records are added 
    /// Load this class's member variables and call this function to
    /// add a new record.
    /// 
    /// 
    /// 05/19/2008 wdk
    /// Modified to use a comittable transaction.
    /// </summary>
    /// <returns>number of records updated 
    /// Or 
    /// -1 = Error m_strErrMsg has detailis of error</returns>
    public int AddComittableRecord()
    {
        if (PropCommittableTrans == null)
        {
            CreateCommittableTransaction();
        }

        SqlCommand eobCommand = new();

        if (Reob.propDBConnection.State.Equals(ConnectionState.Closed))
        {
            Reob.propDBConnection.Open();

        }
        if (PropCommittableTrans?.TransactionInformation.Status != TransactionStatus.Active)
        {
            Reob.propDBConnection.EnlistTransaction(PropCommittableTrans);
        }
        eobCommand.Connection = Reob.propDBConnection;

        //throw new System.NotImplementedException();
        int iRetVal = -1;
        string strSQL;

        //INSERT INTO MyTable (PriKey, Description)
        //   VALUES (123, 'A description of part 123.')
        //                                table fields   

        /* minimum fields that must be inserted
            account, date_of_service, eft_file, eft_number, eft_date, eob_detail_rowguid
        */
        strSQL = string.Format("INSERT INTO {0}" +
            "(rowguid, account, subscriberID, subscriberName, date_of_service, ICN," +
                "PatStat, claim_status, claim_type, charges_reported,  charges_noncovered," +
                    "charges_denied, pat_lib_coinsurance, pat_lib_noncovered, pay_data_pat_refund, pay_data_reimb_rate," +
                    "pay_data_msp_prim_pay," +
                        "pay_data_hcpcs_amt,  pay_data_cont_adj_amt, pay_data_per_diem_rate, pay_data_net_reimb_amt, claim_forwarded_to, " +
                            "claim_forwarded_id, eft_file,  eft_number, eft_date, " +
                                "eob_print_date, mod_date, mod_prg, mod_user, mod_host, bill_cycle_date, check_no) " +
                                        "VALUES ('{1}','{2}','{3}','{4}','{5}', " +
                                                " '{6}','{7}','{8}','{9}','{10}', " +
                                                    " '{11}','{12}','{13}','{14}','{15}', " +
                                                        " '{16}','{17}','{18}','{19}','{20}'," +
                                                            " '{21}','{22}','{23}','{24}','{25}'," +
                                                                " '{26}',{27},'{28}','{29}', '{30}', '{31}', '{32}', '{33}')",
                                     Reob.propTable.ToString(),
                                     Reob.m_strRowguid, Reob.m_strAccount, Reob.m_strSubscriberID, Reob.m_strSubscriberName, Reob.m_strDateOfService, Reob.m_strICN, //5
                                     Reob.m_strPatStat, Reob.m_strClaimStatus, Reob.m_strClaimType, Reob.m_strChargesReported, Reob.m_strChargesNoncovered, //10
                                     Reob.m_strChargesDenied, Reob.m_strPatLibCoinsurance, Reob.m_strPatLibNoncovered, Reob.m_strPayDataPatRefund, Reob.m_strPayDataReimbRate, //15  
                                     Reob.m_strPayDataMSPPrimPay,
                                     Reob.m_strPayDataHcpcsAmt, Reob.m_strPayDataContAdjAmt, Reob.m_strPayDataPerDiemRate, Reob.m_strPayDataNetReimbAmt, Reob.m_strClaimForwardedTo, //20
                                     Reob.m_strClaimForwardedId, Reob.m_strEftFile, Reob.m_strEftNumber, Reob.m_strEftDate, //24
                                     Reob.m_strEobPrintDate.Length == 0 ? "null" : string.Format("'{0}'", Reob.m_strEobPrintDate), Reob.m_strModDate, Reob.m_strModPrg, Reob.m_strModUser, Reob.m_strModHost, Reob.m_strBillCycleDate, Reob.m_strCheckNo);

        eobCommand.CommandText = strSQL;
        iRetVal = eobCommand.ExecuteNonQuery();

        return iRetVal;
    }


    /// <summary>
    /// Calls the documents print.
    /// </summary>
    public void PrintEOB()
    {
        // wdk 20090605 moved from the printpage() handler so that GetRecords() would not reset the 
        // recordset to the first record each time it looped through.
        _nDetailRecCount = ReobDetail.GetRecords(string.Format("rowguid = '{0}' order by ServiceCode, UID", Reob.m_strRowguid));


        _printDoc.PrintController = new System.Drawing.Printing.StandardPrintController();

        _printDoc.Print();
        UpdatePrinted();

    }


    private void m_printDoc_PrintPage(object sender, PrintPageEventArgs e)
    {
        // set the format for tab stops           
        float[] tabStops = { 100.0f, 250.0f, 200.0f };
        _strLineFmt.SetTabStops(200.0f, tabStops);

        // float linesPerPage = 0;
        LinePos = 0; // initialize the pages line position
        // int count = 6;
        float leftMargin = e.MarginBounds.Left;
        float topMargin = e.MarginBounds.Top;

        // Calculate the number of lines per page.
        // linesPerPage = e.MarginBounds.Height / m_printFont.GetHeight(e.Graphics);         

        // print the header 
        LinePos = PrintPageHeader(e.Graphics);

        // Print each line of the detail.
        tabStops = new float[] { 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f };
        _strLineFmt.SetTabStops(25.0f, tabStops);
        // print detail header line
        string strDetail = string.Format("Service Code Rev  Units APC# Allowed  Stat Wght    Date    Charges    Paid Reason  Adj Amt",
                                             Environment.NewLine);
        PropFontStyle = FontStyle.Bold; // set the detail header bold
        PropFontSize = 10;
        e.Graphics.DrawString(strDetail, _printFont, Brushes.Black,
               leftMargin, LinePos, _strLineFmt);
        LinePos += _printFont.GetHeight(e.Graphics);
        PropFontStyle = FontStyle.Regular; // turn off head bold


        if (_nDetailRecCount < 1)
        {
            PropFontStyle = FontStyle.Bold | FontStyle.Italic;
            PropFontSize = 20;
            LinePos += _printFont.GetHeight(e.Graphics);
            e.Graphics.DrawString("No Detail records found for this EOB", _printFont, Brushes.Black,
                  leftMargin, LinePos, _strLineFmt);

            return;
        }
        while (ReobDetail.propErrMsg != "EOF")
        {
            // PR 2's are Patient Responsibility and 2 means the adjustment code is split between two lines this is the second so only the amount are to be printed.
            if (ReobDetail.m_strReasonType == "PR" && ReobDetail.m_strReasonCode == "2")
            {
                string strBlank = "";
                strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                               ReobDetail.m_strServiceCode.PadRight(13), //strBlank.PadRight(13),//wdk 20131114 for visual acuity
                                   strBlank.PadRight(5),//m_eobDetails.m_strRevCode.PadRight(5),
                                       strBlank.PadLeft(4),//m_eobDetails.m_strUnits.PadLeft(4),
                                           strBlank.PadLeft(5),//m_eobDetails.m_strApcNr.PadLeft(5),
                                               strBlank.PadLeft(9),//m_eobDetails.m_strAllowedAmt.PadLeft(9),
                                                   strBlank.PadRight(5),//m_eobDetails.m_strStat.PadRight(5),
                                                       strBlank.PadLeft(6),//m_eobDetails.m_strWght.PadLeft(6),
                                                           strBlank.PadRight(10),//m_eobDetails.m_strDateOfService.PadRight(10),
                                                               strBlank.PadLeft(8),//m_eobDetails.m_strChargeAmt.PadLeft(8),
                                                                   strBlank.PadLeft(8),//m_eobDetails.m_strPaidAmt.PadLeft(8),
                                                                       ((string)ReobDetail.m_strReasonType + "/" + ReobDetail.m_strReasonCode).PadRight(6),
                                                                           ReobDetail.m_strOtherAdjAmt.Trim().PadLeft(8));// wdk 20131114 changed from 10

            }
            else
            {
                strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                ReobDetail.m_strServiceCode.PadRight(13),
                    ReobDetail.m_strRevCode.PadRight(5),
                        ReobDetail.m_strUnits.PadLeft(4),
                            ReobDetail.m_strApcNr.PadLeft(5),
                                ReobDetail.m_strAllowedAmt.PadLeft(9),
                                    ReobDetail.m_strStat.PadRight(5),
                                        ReobDetail.m_strWght.PadLeft(6),
                                            ReobDetail.m_strDateOfService.PadRight(10),
                                                ReobDetail.m_strChargeAmt.PadLeft(8),
                                                    ReobDetail.m_strPaidAmt.PadLeft(8),
                                                        ((string)ReobDetail.m_strReasonType + "/" + ReobDetail.m_strReasonCode).PadRight(6),
                                                        ReobDetail.m_strContractualAdjAmt == "0.00" ? ReobDetail.m_strOtherAdjAmt.PadLeft(8) : ReobDetail.m_strContractualAdjAmt.PadLeft(8));//,
            }


            e.Graphics.DrawString(strDetail, _printFont, Brushes.Black,
                leftMargin, LinePos, _strLineFmt);
            LinePos += _printFont.GetHeight(e.Graphics);

            e.HasMorePages = ReobDetail.GetNext(); // if more detail lines e.HasMorePages will be true.

            if (LinePos >= 900.0f && e.HasMorePages)
            {
                strDetail = "\t\tContinued...";
                e.Graphics.DrawString(strDetail, _printFont, Brushes.Black,
                        leftMargin, LinePos, _strLineFmt);
                LinePos += _printFont.GetHeight(e.Graphics);
                break;
            }
            if (!e.HasMorePages) // print footer
            {
                e.Graphics.DrawString(string.Format("Total Service Contractual Adj...          {0}", Reob.m_strPayDataContAdjAmt.PadLeft(10)), _printFont, Brushes.Black,
                        leftMargin, LinePos, _strLineFmt);
                LinePos += _printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString(string.Format("Total Service Denied Amt...               {0}", Reob.m_strChargesDenied.PadLeft(10)), _printFont, Brushes.Black,
                leftMargin, LinePos, _strLineFmt);
                LinePos += _printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString(string.Format("Total Service NonCovered Amt...           {0}", Reob.m_strChargesNoncovered.PadLeft(10)), _printFont, Brushes.Black,
                leftMargin, LinePos, _strLineFmt);
                LinePos += _printFont.GetHeight(e.Graphics);
                LinePos += _printFont.GetHeight(e.Graphics); // insert blank line
                if (Reob.m_strClaimForwardedTo.Length > 0)
                {
                    e.Graphics.DrawString(string.Format("Claim forwarded to {0} id # - {1}", Reob.m_strClaimForwardedTo, Reob.m_strClaimForwardedId), _printFont, Brushes.Black,
                    leftMargin, LinePos, _strLineFmt);
                    LinePos += _printFont.GetHeight(e.Graphics);
                }
                LinePos += _printFont.GetHeight(e.Graphics); // insert blank line
                e.Graphics.DrawString(string.Format("Printed {0}", DateTime.Now.ToLongDateString()), _printFont, Brushes.Black,
                        leftMargin, LinePos, _strLineFmt);
                LinePos += _printFont.GetHeight(e.Graphics);
            }

        }


        //  If more lines exist, print another page.
        // if (line != null)
        //    e.HasMorePages = true;
        //else
        //    e.HasMorePages = false;

    }

    /// <summary>
    /// Prints the page header with subscriber section.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private float PrintPageHeader(Graphics e)
    {

        // this is a test of the emergency.
        // todo need to switch on payor for header ie. TLC
        PropFontSize = 11;
        List<string> strEOB = new List<string>();
        if (Reob.m_strEftFile.IndexOf('_') < 0)
        {
            strEOB.Add(string.Format("MEDICARE NATIONAL STANADARD INTERMEDIARY REMITTANCE ADVICE{0}", Environment.NewLine));
            strEOB.Add(string.Format("Explanation of Medicare Benefits (EOMB){0}", Environment.NewLine));
            strEOB.Add(string.Format("Jackson-Madison Cty General\tFile Date: {0}\t{1}",
                Reob.m_strEftDate, _strEobAgency));
            strEOB.Add(string.Format("620 Skyline Drive\tBill cycle: {0}\t{1}",
                Reob.m_strBillCycleDate, _strEobAddress));
            strEOB.Add(string.Format("Jackson TN 38301-3923\tFile #: {0}\t{1}",
                Reob.m_strEftNumber, _strEobCSZ));
            strEOB.Add(string.Format("Provider No - 440002\t\t{0}", _strEobPhone)); // todo need to store and retrieve the provider no from the file.
        }
        else
        {
            strEOB.Add(string.Format("            TLC INTERMEDIARY REMITTANCE ADVICE{0}", Environment.NewLine));
            strEOB.Add(string.Format("    Explanation of Benefits (EOB){0}", Environment.NewLine));
            strEOB.Add(string.Format("Jackson-Madison Cty General\tFile Date: {0}\tTLC FAMILY CARE HEALTHPLAN", Reob.m_strEftDate));
            strEOB.Add(string.Format("708 W. Forest Ave.\tBill cycle: {0}\tPO BOX 49", Reob.m_strBillCycleDate));
            strEOB.Add(string.Format("Jackson TN 38301\tFile #: {0}\tMemphis TN 38101", Reob.m_strEftNumber));
            strEOB.Add(string.Format("Provider No - 440002\t\t800/473-6523")); // todo need to store and retrieve the provider no from the file.
        }

        strEOB.Add("====================================================================================");
        LinePos = 0; // position from top of page


        // set the tab stops for the File Header
        float[] tabStops = { 100.0f, 250.0f, 175.0f };
        //StringFormat strFmt = new StringFormat();
        _strLineFmt.SetTabStops(200.0f, tabStops);
        string strToPrint;
        LinePos = _topMargin;
        for (int i = 0; i < strEOB.Count; i++)
        {
            // SizeF sF = e.MeasureString(strEOB[i].ToString(), m_printFont);
            _leftMargin = 25;
            PropFontStyle = FontStyle.Regular;
            if (i == 0)
            {
                _leftMargin = 100;
                PropFontStyle = FontStyle.Bold;
            }
            if (i == 1)
            {
                _leftMargin = 200;
                PropFontStyle = FontStyle.Bold;
            }

            strToPrint = strEOB[i];
            e.DrawString(strToPrint, _printFont, Brushes.Black,
                    _leftMargin, LinePos, _strLineFmt);

            LinePos += _printFont.GetHeight(e);
        }
        // print the Subscribers section
        LinePos = PrintSubscriberSection(e);
        return (LinePos);
    }

    /// <summary>
    /// Prints the subscribers section
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private float PrintSubscriberSection(Graphics e)
    {
        // set the tab stops for the File Header
        float[] tabStops = { 75.0f, 250.0f, 200.0f };
        List<string> strEOB = new();

        strEOB.Add(string.Format("Name: {0}\tService From {1} thru {2}{3}",
                                Reob.m_strSubscriberName,
                                                    Reob.m_strDateOfService,
                                                                Reob.m_strDateOfService,
                                                                        Environment.NewLine));
        strEOB.Add(string.Format("HIC:  {0}\tICN: {1}\tPat.Stat:{2}{3}",
                                            Reob.m_strSubscriberID,
                                                Reob.m_strICN,
                                                    Reob.m_strPatStat,
                                                        Environment.NewLine));
        strEOB.Add(string.Format("PCN:  {0}\tMRN: {1}\tClaim St: {2} Type: {3}{4}",
                                        Reob.m_strAccount,
                                            "" /*MRN is blank at this time*/,
                                                    Reob.m_strClaimStatus,
                                                        Reob.m_strClaimType,
                                                            Environment.NewLine));
        strEOB.Add("====================================================================================");
        // add the subscribers data charges, patient libility and payment data.

        strEOB.Add(string.Format("CHARGES\tPATIENT LIABILITY\tPAYMENT DATA{0}", Environment.NewLine));
        strEOB.Add(string.Format("Reported: {0}\tTotal Deduct: {1}\tReimb Rate: {2}{3}",
                                             Reob.m_strChargesReported.PadLeft(10),
                                                   ((string)"0.00").PadLeft(8),
                                                        Reob.m_strPayDataReimbRate.PadLeft(11),
                                                                 Environment.NewLine));
        strEOB.Add(string.Format("NCovd: {0}\tCoinsurance: {1}\tMSP Prim Pay(): {2}{3}",
                                             Reob.m_strChargesNoncovered.PadLeft(13),
                                                   Reob.m_strPatLibCoinsurance.PadLeft(9),
                                                        Reob.m_strPayDataMSPPrimPay.PadLeft(7),
                                                                 Environment.NewLine));
        strEOB.Add(string.Format("Denied: {0}\tBlood Deduct: {1}\tProf Comp: {2}{3}",
                                             Reob.m_strChargesDenied.PadLeft(12),
                                                   ((string)"0.00").PadLeft(8),
                                                        ((string)"0.00").PadLeft(12),
                                                                 Environment.NewLine));
        strEOB.Add(string.Format("\tMSP Liab Met: {0}\tESRD Amt: {1}{2}",
                                             ((string)"0.00").PadLeft(8),
                                                        ((string)"0.00").PadLeft(13),
                                                                 Environment.NewLine));
        strEOB.Add(string.Format("\tNCovd Charges: {0}\tHCPCS Amt: {1}{2}",
                                             Reob.m_strPatLibNoncovered.PadLeft(7),
                                                        Reob.m_strPayDataHcpcsAmt.PadLeft(12),
                                                                 Environment.NewLine));
        strEOB.Add(string.Format("\t\tG/R Amt: {0}{1}",
                                             ((string)"0.00").PadLeft(14),
                                                 Environment.NewLine));
        strEOB.Add(string.Format("\t\tCont Adj Amt: {0}{1}",
                                             Reob.m_strPayDataContAdjAmt.PadLeft(9),
                                                 Environment.NewLine));
        strEOB.Add(string.Format("\t\tInterest: {0}{1}",
                                             ((string)"0.00").PadLeft(13),
                                                 Environment.NewLine));
        strEOB.Add(string.Format("\t\tPat. Refund: {0}{1}",
                                             Reob.m_strPayDataPatRefund.PadLeft(10),
                                                 Environment.NewLine));
        strEOB.Add(string.Format("\t\tPer Diem Amt: {0}{1}",
                                             Reob.m_strPayDataPerDiemRate.PadLeft(9),
                                                 Environment.NewLine));
        strEOB.Add(string.Format("\t\tNet Reim Amt: {0}{1}",
                                             Reob.m_strPayDataNetReimbAmt.PadLeft(9),
                                                 Environment.NewLine));
        strEOB.Add("====================================================================================");
        _strLineFmt.SetTabStops(200.0f, tabStops);
        string strToPrint;

        for (int i = 0; i < strEOB.Count; i++)
        {
            strToPrint = strEOB[i];
            PropFontStyle = FontStyle.Regular;
            if (strEOB[i].StartsWith("CHARGES"))
            {
                PropFontStyle = FontStyle.Bold;
            }

            e.DrawString(strToPrint, _printFont, Brushes.Black,
                    _leftMargin, LinePos, _strLineFmt);
            LinePos += _printFont.GetHeight(e);
        }
        return (LinePos);
    }

    /// <summary>
    /// Updates the eob_print_date in the table.
    /// </summary>
    /// <returns></returns>
    private int UpdatePrinted()
    {
        string strSQL;
        string strWhere;
        /*
        strSQL = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3}",
            m_strTable, //* a property field in this class 
            strField,
            strNewValue,
            strFilter);
        */

        /*
         * Set the where clause for the KEY for this table from the DataSet values
         */
        Reob.m_strEobPrintDate = DateTime.Now.ToString("G");
        strWhere = string.Format("rowguid  = '{0}' ",
                                    Reob.m_DataSet.Tables[Reob.propTable.ToString()]?.Rows[Reob.m_CurrentRecordIndex]["rowguid"].ToString());
        Reob.m_strEobPrintDate = DateTime.Now.ToString();
        strSQL = string.Format("UPDATE {0} SET eob_print_date = '{1}' where {2}",
                                     Reob.propTable.ToString(),
                                        Reob.m_strEobPrintDate,
                                     strWhere);

        return Reob.SQLExec(strSQL, out m_strErrMsg);
    }



} // don't go below this line
