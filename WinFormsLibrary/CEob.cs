using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using RFClassLibrary;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;

namespace MCL
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
    public class CEob : RFCObject
    {
        private R_system m_rSystem;
        private string m_strEobAgency;
        private string m_strEobAddress;
        private string m_strEobCSZ;
        private string m_strEobPhone;
        private int m_nDetailRecCount = -1; // wdk 20090605 made member level

        // comittable transaction
        private CommittableTransaction m_Transaction;
        /// <summary>
        /// 
        /// </summary>
        public CommittableTransaction propCommittableTrans
        {
            get { return m_Transaction; }
            set { m_Transaction = value; }
        }

        PrintDocument m_printDoc;
        // for printing
        private FontStyle fontStyle;
        private int fontSize;
        /// <summary>
        /// 
        /// </summary>
        public int propFontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                m_printFont = new Font("Courier New", fontSize, propFontStyle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FontStyle propFontStyle
        {
            get { return fontStyle; }
            set
            {
                fontStyle = value;
                m_printFont = new Font("Courier New", propFontSize, fontStyle);
            }
        }

        Font m_printFont;
        float m_leftMargin;
        float m_topMargin;

        StringFormat m_strLineFmt;
        //        public R_eob_detail m_eobDetails;
        // private string m_strServer;
        // private string m_strDatabase;
        //   List<string> m_strEOB;
        /// <summary>
        /// 
        /// </summary>
        public float m_linePos = 0;
 
        private ERR m_Err;
        /// <summary>
        /// This classes instance of the recordset for the eob table
        /// </summary>
        public R_eob m_Reob;
        /// <summary>
        /// 
        /// </summary>
        public R_eob_detail m_ReobDetail;
        
        private string m_strServer;
        /// <summary>
        /// 
        /// </summary>
        public string propServer
        {
            get { return m_strServer; }
        }
        private string m_strDatabase;
        /// <summary>
        /// 
        /// </summary>
        public string propDatabase
        {
            get { return m_strDatabase; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDatabase"></param>
        /// <param name="errLog"></param>
        public CEob(string strServer, string strDatabase, ref ERR errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDatabase;
            m_Err = errLog;
            m_Reob = new R_eob(m_strServer, m_strDatabase, ref errLog);
            m_ReobDetail = new R_eob_detail(m_strServer, m_strDatabase, ref errLog);
            // rgc/wdk 20120124 added system call for eob_address
            m_rSystem = new R_system(m_strServer, m_strDatabase, ref errLog);
            string[] strTemp = m_rSystem.GetValue("eob_address").Split(new char[] {'|'});
            m_strEobAgency = strTemp[0];
            m_strEobAddress = strTemp[1];
            m_strEobCSZ = strTemp[2];
            m_strEobPhone = strTemp[3];
            // create and initialize the print document
            propFontSize = 11;
            propFontStyle = FontStyle.Regular;
            m_printFont = new Font("Courier New", propFontSize, propFontStyle); // 05/13/2008 wdk this is a fixed pitch font.

            m_leftMargin = 25;
            m_topMargin = 75;
            m_strLineFmt = new StringFormat();
            m_printDoc = new PrintDocument();
            
            
            m_printDoc.DocumentName = "EOB";
            m_printDoc.DefaultPageSettings.Margins = new Margins(25, 50, 75, 75);
            m_printDoc.PrintPage += new PrintPageEventHandler(m_printDoc_PrintPage);

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
           if (m_Transaction == null)
           {
               try
               {
                   TransactionOptions to = new TransactionOptions();
                   to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted; // 05/21/2008 rgc/wdk so other processes can read the committed transactions without locking up the table.
                   m_Transaction = new CommittableTransaction(to);
                   m_Reob.propDBConnection.EnlistTransaction(m_Transaction);
                   m_Err.m_Logfile.WriteLogFile("CEOB::CreateCommittableTransaction() CREATED");
               }
               catch (Exception ex)
               {
                   m_Err.m_Logfile.WriteLogFile("CEOB::CreateCommittableTransaction()");
                   m_Err.m_Logfile.WriteLogFile(string.Format("Exception thrown while trying to create transaction: {0}\r\n ", ex.Message));
                   nRetVal = 0;
               }
           }
           else
           {
               m_Err.m_Logfile.WriteLogFile("CEOB::CreateCommittableTransaction() ALREADY EXISTS");
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
            if (m_Transaction == null)
            {
                CreateCommittableTransaction();
            }

            SqlCommand eobCommand = new SqlCommand();

            if (m_Reob.propDBConnection.State.Equals(ConnectionState.Closed))
            {
                m_Reob.propDBConnection.Open();

            }
            if (m_Transaction?.TransactionInformation.Status != TransactionStatus.Active)
            {
                m_Reob.propDBConnection.EnlistTransaction(m_Transaction);
            }
            eobCommand.Connection = m_Reob.propDBConnection;

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
                                         m_Reob.propTable.ToString(),
                                         m_Reob.m_strRowguid, m_Reob.m_strAccount, m_Reob.m_strSubscriberID, m_Reob.m_strSubscriberName, m_Reob.m_strDateOfService, m_Reob.m_strICN, //5
                                         m_Reob.m_strPatStat, m_Reob.m_strClaimStatus, m_Reob.m_strClaimType, m_Reob.m_strChargesReported, m_Reob.m_strChargesNoncovered, //10
                                         m_Reob.m_strChargesDenied, m_Reob.m_strPatLibCoinsurance, m_Reob.m_strPatLibNoncovered, m_Reob.m_strPayDataPatRefund, m_Reob.m_strPayDataReimbRate, //15  
                                         m_Reob.m_strPayDataMSPPrimPay,
                                         m_Reob.m_strPayDataHcpcsAmt, m_Reob.m_strPayDataContAdjAmt, m_Reob.m_strPayDataPerDiemRate, m_Reob.m_strPayDataNetReimbAmt, m_Reob.m_strClaimForwardedTo, //20
                                         m_Reob.m_strClaimForwardedId, m_Reob.m_strEftFile, m_Reob.m_strEftNumber, m_Reob.m_strEftDate, //24
                                         m_Reob.m_strEobPrintDate.Length == 0 ? "null" : string.Format("'{0}'", m_Reob.m_strEobPrintDate), m_Reob.m_strModDate, m_Reob.m_strModPrg, m_Reob.m_strModUser, m_Reob.m_strModHost, m_Reob.m_strBillCycleDate, m_Reob.m_strCheckNo);

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
            m_nDetailRecCount = m_ReobDetail.GetRecords(string.Format("rowguid = '{0}' order by ServiceCode, UID", m_Reob.m_strRowguid));


            m_printDoc.PrintController = new System.Drawing.Printing.StandardPrintController();
            
            m_printDoc.Print();
            UpdatePrinted();

        }


        private void m_printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // set the format for tab stops           
            float[] tabStops = { 100.0f, 250.0f, 200.0f };
            m_strLineFmt.SetTabStops(200.0f, tabStops);

            // float linesPerPage = 0;
            m_linePos = 0; // initialize the pages line position
            // int count = 6;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;

            // Calculate the number of lines per page.
            // linesPerPage = e.MarginBounds.Height / m_printFont.GetHeight(e.Graphics);         

            // print the header 
            m_linePos = PrintPageHeader(e.Graphics);

            // Print each line of the detail.
            tabStops = new float[] { 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f, 25.0f };
            m_strLineFmt.SetTabStops(25.0f, tabStops);
            // print detail header line
            string strDetail = string.Format("Service Code Rev  Units APC# Allowed  Stat Wght    Date    Charges    Paid Reason  Adj Amt",
                                                 Environment.NewLine);
            propFontStyle = FontStyle.Bold; // set the detail header bold
            propFontSize = 10;
            e.Graphics.DrawString(strDetail, m_printFont, Brushes.Black,
                   leftMargin, m_linePos, m_strLineFmt);
            m_linePos += m_printFont.GetHeight(e.Graphics);
            propFontStyle = FontStyle.Regular; // turn off head bold
            
            
            if (m_nDetailRecCount < 1)
            {
                propFontStyle = FontStyle.Bold | FontStyle.Italic;
                propFontSize = 20;
                m_linePos += m_printFont.GetHeight(e.Graphics);
                e.Graphics.DrawString("No Detail records found for this EOB", m_printFont, Brushes.Black,
                      leftMargin, m_linePos, m_strLineFmt);

                return;
            }
            while (m_ReobDetail.propErrMsg != "EOF")
            {
                // PR 2's are Patient Responsibility and 2 means the adjustment code is split between two lines this is the second so only the amount are to be printed.
                if (m_ReobDetail.m_strReasonType == "PR" && m_ReobDetail.m_strReasonCode == "2")
                {
                    string strBlank = "";
                    strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                                   m_ReobDetail.m_strServiceCode.PadRight(13), //strBlank.PadRight(13),//wdk 20131114 for visual acuity
                                       strBlank.PadRight(5),//m_eobDetails.m_strRevCode.PadRight(5),
                                           strBlank.PadLeft(4),//m_eobDetails.m_strUnits.PadLeft(4),
                                               strBlank.PadLeft(5),//m_eobDetails.m_strApcNr.PadLeft(5),
                                                   strBlank.PadLeft(9),//m_eobDetails.m_strAllowedAmt.PadLeft(9),
                                                       strBlank.PadRight(5),//m_eobDetails.m_strStat.PadRight(5),
                                                           strBlank.PadLeft(6),//m_eobDetails.m_strWght.PadLeft(6),
                                                               strBlank.PadRight(10),//m_eobDetails.m_strDateOfService.PadRight(10),
                                                                   strBlank.PadLeft(8),//m_eobDetails.m_strChargeAmt.PadLeft(8),
                                                                       strBlank.PadLeft(8),//m_eobDetails.m_strPaidAmt.PadLeft(8),
                                                                           ((string)m_ReobDetail.m_strReasonType + "/" + m_ReobDetail.m_strReasonCode).PadRight(6),
                                                                               m_ReobDetail.m_strOtherAdjAmt.Trim().PadLeft(8));// wdk 20131114 changed from 10

                }
                else
                {
                    strDetail = string.Format("{0}{1}{2} {3}{4}{5}{6}{7}{8}{9} {10}{11}",
                    m_ReobDetail.m_strServiceCode.PadRight(13),
                        m_ReobDetail.m_strRevCode.PadRight(5),
                            m_ReobDetail.m_strUnits.PadLeft(4),
                                m_ReobDetail.m_strApcNr.PadLeft(5),
                                    m_ReobDetail.m_strAllowedAmt.PadLeft(9),
                                        m_ReobDetail.m_strStat.PadRight(5),
                                            m_ReobDetail.m_strWght.PadLeft(6),
                                                m_ReobDetail.m_strDateOfService.PadRight(10),
                                                    m_ReobDetail.m_strChargeAmt.PadLeft(8),
                                                        m_ReobDetail.m_strPaidAmt.PadLeft(8),
                                                            ((string)m_ReobDetail.m_strReasonType + "/" + m_ReobDetail.m_strReasonCode).PadRight(6),
                                                            m_ReobDetail.m_strContractualAdjAmt == "0.00" ? m_ReobDetail.m_strOtherAdjAmt.PadLeft(8) : m_ReobDetail.m_strContractualAdjAmt.PadLeft(8));//,
                }

                
                e.Graphics.DrawString(strDetail, m_printFont, Brushes.Black,
                    leftMargin, m_linePos, m_strLineFmt);
                m_linePos += m_printFont.GetHeight(e.Graphics);

                e.HasMorePages = m_ReobDetail.GetNext(); // if more detail lines e.HasMorePages will be true.

                if (m_linePos >= 900.0f && e.HasMorePages)
                {
                    strDetail = "\t\tContinued...";
                    e.Graphics.DrawString(strDetail, m_printFont, Brushes.Black,
                            leftMargin, m_linePos, m_strLineFmt);
                    m_linePos += m_printFont.GetHeight(e.Graphics);
                    break;
                }
                if (!e.HasMorePages) // print footer
                {
                    e.Graphics.DrawString(string.Format("Total Service Contractual Adj...          {0}", m_Reob.m_strPayDataContAdjAmt.PadLeft(10)), m_printFont, Brushes.Black,
                            leftMargin, m_linePos, m_strLineFmt);
                    m_linePos += m_printFont.GetHeight(e.Graphics);
                    e.Graphics.DrawString(string.Format("Total Service Denied Amt...               {0}", m_Reob.m_strChargesDenied.PadLeft(10)), m_printFont, Brushes.Black,
                    leftMargin, m_linePos, m_strLineFmt);
                    m_linePos += m_printFont.GetHeight(e.Graphics);
                    e.Graphics.DrawString(string.Format("Total Service NonCovered Amt...           {0}", m_Reob.m_strChargesNoncovered.PadLeft(10)), m_printFont, Brushes.Black,
                    leftMargin, m_linePos, m_strLineFmt);
                    m_linePos += m_printFont.GetHeight(e.Graphics);
                    m_linePos += m_printFont.GetHeight(e.Graphics); // insert blank line
                    if (m_Reob.m_strClaimForwardedTo.Length > 0)
                    {
                        e.Graphics.DrawString(string.Format("Claim forwarded to {0} id # - {1}", m_Reob.m_strClaimForwardedTo, m_Reob.m_strClaimForwardedId), m_printFont, Brushes.Black,
                        leftMargin, m_linePos, m_strLineFmt);
                        m_linePos += m_printFont.GetHeight(e.Graphics);
                    }
                    m_linePos += m_printFont.GetHeight(e.Graphics); // insert blank line
                    e.Graphics.DrawString(string.Format("Printed {0}", DateTime.Now.ToLongDateString()), m_printFont, Brushes.Black,
                            leftMargin, m_linePos, m_strLineFmt);
                    m_linePos += m_printFont.GetHeight(e.Graphics);
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
            propFontSize = 11;
            List<string> strEOB = new List<string>();
            if (m_Reob.m_strEftFile.IndexOf('_') < 0)
            {
                strEOB.Add(string.Format("MEDICARE NATIONAL STANADARD INTERMEDIARY REMITTANCE ADVICE{0}", Environment.NewLine));
                strEOB.Add(string.Format("Explanation of Medicare Benefits (EOMB){0}", Environment.NewLine));
                strEOB.Add(string.Format("Jackson-Madison Cty General\tFile Date: {0}\t{1}", 
                    m_Reob.m_strEftDate, m_strEobAgency));
                strEOB.Add(string.Format("620 Skyline Drive\tBill cycle: {0}\t{1}", 
                    m_Reob.m_strBillCycleDate, m_strEobAddress));
                strEOB.Add(string.Format("Jackson TN 38301-3923\tFile #: {0}\t{1}", 
                    m_Reob.m_strEftNumber, m_strEobCSZ));
                strEOB.Add(string.Format("Provider No - 440002\t\t{0}",m_strEobPhone)); // todo need to store and retrieve the provider no from the file.
            }
            else
            {
                strEOB.Add(string.Format("            TLC INTERMEDIARY REMITTANCE ADVICE{0}", Environment.NewLine));
                strEOB.Add(string.Format("    Explanation of Benefits (EOB){0}", Environment.NewLine));
                strEOB.Add(string.Format("Jackson-Madison Cty General\tFile Date: {0}\tTLC FAMILY CARE HEALTHPLAN", m_Reob.m_strEftDate));
                strEOB.Add(string.Format("708 W. Forest Ave.\tBill cycle: {0}\tPO BOX 49", m_Reob.m_strBillCycleDate));
                strEOB.Add(string.Format("Jackson TN 38301\tFile #: {0}\tMemphis TN 38101", m_Reob.m_strEftNumber));
                strEOB.Add(string.Format("Provider No - 440002\t\t800/473-6523")); // todo need to store and retrieve the provider no from the file.
            }

            strEOB.Add("====================================================================================");
            m_linePos = 0; // position from top of page


            // set the tab stops for the File Header
            float[] tabStops = { 100.0f, 250.0f, 175.0f };
            //StringFormat strFmt = new StringFormat();
            m_strLineFmt.SetTabStops(200.0f, tabStops);
            string strToPrint;
            m_linePos = m_topMargin;
            for (int i = 0; i < strEOB.Count; i++)
            {
                // SizeF sF = e.MeasureString(strEOB[i].ToString(), m_printFont);
                m_leftMargin = 25;
                propFontStyle = FontStyle.Regular;
                if (i == 0)
                {
                    m_leftMargin = 100;
                    propFontStyle = FontStyle.Bold;
                }
                if (i == 1)
                {
                    m_leftMargin = 200;
                    propFontStyle = FontStyle.Bold;
                }

                strToPrint = strEOB[i];
                e.DrawString(strToPrint, m_printFont, Brushes.Black,
                        m_leftMargin, m_linePos, m_strLineFmt);

                m_linePos += m_printFont.GetHeight(e);
            }
            // print the Subscribers section
            m_linePos = PrintSubscriberSection(e);
            return (m_linePos);
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
            List<string> strEOB = new List<string>();

            strEOB.Add(string.Format("Name: {0}\tService From {1} thru {2}{3}",
                                    m_Reob.m_strSubscriberName,
                                                        m_Reob.m_strDateOfService,
                                                                    m_Reob.m_strDateOfService,
                                                                            Environment.NewLine));
            strEOB.Add(string.Format("HIC:  {0}\tICN: {1}\tPat.Stat:{2}{3}",
                                                m_Reob.m_strSubscriberID,
                                                    m_Reob.m_strICN,
                                                        m_Reob.m_strPatStat,
                                                            Environment.NewLine));
            strEOB.Add(string.Format("PCN:  {0}\tMRN: {1}\tClaim St: {2} Type: {3}{4}",
                                            m_Reob.m_strAccount,
                                                "" /*MRN is blank at this time*/,
                                                        m_Reob.m_strClaimStatus,
                                                            m_Reob.m_strClaimType,
                                                                Environment.NewLine));
            strEOB.Add("====================================================================================");
            // add the subscribers data charges, patient libility and payment data.

            strEOB.Add(string.Format("CHARGES\tPATIENT LIABILITY\tPAYMENT DATA{0}", Environment.NewLine));
            strEOB.Add(string.Format("Reported: {0}\tTotal Deduct: {1}\tReimb Rate: {2}{3}",
                                                 m_Reob.m_strChargesReported.PadLeft(10),
                                                       ((string)"0.00").PadLeft(8),
                                                            m_Reob.m_strPayDataReimbRate.PadLeft(11),
                                                                     Environment.NewLine));
            strEOB.Add(string.Format("NCovd: {0}\tCoinsurance: {1}\tMSP Prim Pay(): {2}{3}",
                                                 m_Reob.m_strChargesNoncovered.PadLeft(13),
                                                       m_Reob.m_strPatLibCoinsurance.PadLeft(9),
                                                            m_Reob.m_strPayDataMSPPrimPay.PadLeft(7),
                                                                     Environment.NewLine));
            strEOB.Add(string.Format("Denied: {0}\tBlood Deduct: {1}\tProf Comp: {2}{3}",
                                                 m_Reob.m_strChargesDenied.PadLeft(12),
                                                       ((string)"0.00").PadLeft(8),
                                                            ((string)"0.00").PadLeft(12),
                                                                     Environment.NewLine));
            strEOB.Add(string.Format("\tMSP Liab Met: {0}\tESRD Amt: {1}{2}",
                                                 ((string)"0.00").PadLeft(8),
                                                            ((string)"0.00").PadLeft(13),
                                                                     Environment.NewLine));
            strEOB.Add(string.Format("\tNCovd Charges: {0}\tHCPCS Amt: {1}{2}",
                                                 m_Reob.m_strPatLibNoncovered.PadLeft(7),
                                                            m_Reob.m_strPayDataHcpcsAmt.PadLeft(12),
                                                                     Environment.NewLine));
            strEOB.Add(string.Format("\t\tG/R Amt: {0}{1}",
                                                 ((string)"0.00").PadLeft(14),
                                                     Environment.NewLine));
            strEOB.Add(string.Format("\t\tCont Adj Amt: {0}{1}",
                                                 m_Reob.m_strPayDataContAdjAmt.PadLeft(9),
                                                     Environment.NewLine));
            strEOB.Add(string.Format("\t\tInterest: {0}{1}",
                                                 ((string)"0.00").PadLeft(13),
                                                     Environment.NewLine));
            strEOB.Add(string.Format("\t\tPat. Refund: {0}{1}",
                                                 m_Reob.m_strPayDataPatRefund.PadLeft(10),
                                                     Environment.NewLine));
            strEOB.Add(string.Format("\t\tPer Diem Amt: {0}{1}",
                                                 m_Reob.m_strPayDataPerDiemRate.PadLeft(9),
                                                     Environment.NewLine));
            strEOB.Add(string.Format("\t\tNet Reim Amt: {0}{1}",
                                                 m_Reob.m_strPayDataNetReimbAmt.PadLeft(9),
                                                     Environment.NewLine));
            strEOB.Add("====================================================================================");
            m_strLineFmt.SetTabStops(200.0f, tabStops);
            string strToPrint;

            for (int i = 0; i < strEOB.Count; i++)
            {
                strToPrint = strEOB[i];
                propFontStyle = FontStyle.Regular;
                if (strEOB[i].StartsWith("CHARGES"))
                {
                    propFontStyle = FontStyle.Bold;
                }

                e.DrawString(strToPrint, m_printFont, Brushes.Black,
                        m_leftMargin, m_linePos, m_strLineFmt);
                m_linePos += m_printFont.GetHeight(e);
            }
            return (m_linePos);
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
            m_Reob.m_strEobPrintDate = DateTime.Now.ToString("G");
            strWhere = string.Format("rowguid  = '{0}' ",
                                        m_Reob.m_DataSet.Tables[m_Reob.propTable.ToString()]?.Rows[m_Reob.m_CurrentRecordIndex]["rowguid"].ToString());
            m_Reob.m_strEobPrintDate = DateTime.Now.ToString();
            strSQL = string.Format("UPDATE {0} SET eob_print_date = '{1}' where {2}",
                                         m_Reob.propTable.ToString(),
                                            m_Reob.m_strEobPrintDate,
                                         strWhere);

            return m_Reob.SQLExec(strSQL, out m_strErrMsg);
        }



    } // don't go below this line
}
