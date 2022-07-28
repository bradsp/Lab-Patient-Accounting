using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.ComponentModel;
using System.IO;
//using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



namespace MCL
{
    /// <summary>
    /// Class to create a client invoice
    /// </summary>
    [Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
    public class CInvoice
    {
        
        # region Defines
        //string m_strPDF = null;
        //Stream m_streamText;
        static int CurrentRow = 0;
        SizeF stringSize;
        /// <summary>
        /// BindingList object for CInvoice class
        /// </summary>
        public BindingList<object[]> m_ListToPrint = null;
        private object m_objToPrint;
        /// <summary>
        /// The object to be printed.
        /// </summary>
        public object propObjToPrint
        {
            get { return m_objToPrint; }
            set { m_objToPrint = value; }
        }
        
        private string[] m_ClientAddress = new string[10];
        /// <summary>
        /// This is the clients address from client
        /// </summary>
        public string[] propClientAddress
        {
            get { return m_ClientAddress; }
            set { m_ClientAddress = value; }
        }

        float CurrentY = (float)3;
        float CurrentX = (float)25;
        private Bitmap m_bLogo;
        Rectangle rectLogo = new Rectangle(new Point(25, 3), new Size(283, 48));
        /// <summary>
        /// The print handler for the CInvoice Class
        /// </summary>
        public PrintDocument m_PrintDocInvoice;
        private string m_strInvoice;
        /// <summary>
        /// Invoice we are trying to print.
        /// </summary>
        public string propInvoice
        {
            get { return m_strInvoice; }
        }

        private int m_nPage = 0;

        private Font fontInvLarge;
        /// <summary>
        /// Gets the font used for large text
        ///    = new Font(new FontFamily("Arial"),24,FontStyle.Bold, GraphicsUnit.Pixel);
        /// </summary>
        public Font propInvLarge
        {
            get { return fontInvLarge; }
        }

        private Font fontInvRegular;
        /// <summary>
        /// Gets the font used for black text text
        ///  = new Font(new FontFamily("Arial"),12,FontStyle.Regular, GraphicsUnit.Pixel);
        /// </summary>
        public Font propInvRegular
        {
            get { return fontInvRegular; }
        }

        private Font fontInvReverse;
        /// <summary>
        /// Gets the white text for header boxes
        ///  = new Font(new FontFamily("Arial"),12,FontStyle.Bold,GraphicsUnit.Display);
        /// </summary>
        public Font propInvReverse
        {
            get { return fontInvReverse; }
        }
        
        Rectangle rectHeader = new Rectangle(025, 0005, 760, 0331);
        /// <summary>
        /// Rectangle object containing the header of the invoice.
        /// </summary>
        public Rectangle propRectHeader
        { get { return rectHeader; } }
        Rectangle rectBalances = new Rectangle(025, 0337, 760, 0055);
        Rectangle rectBody = new Rectangle(025, 0395, 760, 0630);
        Rectangle rectFooter = new Rectangle(025, 1022, 760, 0025);

        Rectangle rectInvoice = new Rectangle(035, 0190, 660, 0015);
        Rectangle rectAddress = new Rectangle(085, 0215, 400, 0100);

        Rectangle rectBalPryorBalLabel = new Rectangle(025, 0337, 108, 0035);
        Rectangle rectBalPryorBalAmt = new Rectangle(025, 0373, 108, 0019);
        Rectangle rectBalPaymentLabel = new Rectangle(133, 0337, 108, 0035);
        Rectangle rectBalPaymentAmt = new Rectangle(133, 0373, 108, 0019);
        Rectangle rectBalBalForwardLabel = new Rectangle(241, 0337, 108, 0035);
        Rectangle rectBalBalForwardAmt = new Rectangle(241, 0373, 108, 0019);
        Rectangle rectBalCurrentChargesLabel = new Rectangle(349, 0337, 108, 0035);
        Rectangle rectBalCurrentChargesAmt = new Rectangle(349, 0373, 108, 0019);
        Rectangle rectBalCurrentAmtDueLabel = new Rectangle(457, 0337, 108, 0035);
        Rectangle rectBalCurrentAmtDueAmt = new Rectangle(457, 0373, 108, 0019);
        Rectangle rectBalNetAmtDueLabel = new Rectangle(565, 0337, 108, 0035);
        Rectangle rectBalNetAmtDueAmt = new Rectangle(565, 0373, 108, 0019);
        Rectangle rectBalDateDueLabel = new Rectangle(673, 0337, 112, 0035);
        Rectangle rectBalDateDueDate = new Rectangle(673, 0373, 112, 0019);


        //string m_strBalPryorBalAmt = null;
        //string m_strBalPaymentAmt = null;
        //string m_strBalBalForwardAmt = null;
        //string m_strBalCurrentChargesAmt = null;
        //string m_strBalCurrentAmtDueAmt = null;
        //string m_strBalNetAmtDueAmt = null;
        //string m_strBalDateDueDate = null;

        Rectangle[] rectBalancesArray;

        Rectangle rectDetailTDateLabel = new Rectangle(025, 0393, 055, 18);
        Rectangle rectDetailTDateVal = new Rectangle(025, 0411, 055, 610); // 50 lines at 610 per page print if more than 49 charges print 50 else print "** End of List **"
        Rectangle rectDetailAccLabel = new Rectangle(080, 0393, 070, 18);
        Rectangle rectDetailAccVal = new Rectangle(080, 0411, 070, 610);
        Rectangle rectDetailPatNameLabel = new Rectangle(150, 0393, 170, 18);
        Rectangle rectDetailPatNameVal = new Rectangle(150, 0411, 170, 610);
        Rectangle rectDetailChargeCodeLabel = new Rectangle(320, 393, 100, 18);
        Rectangle rectDetailChargeCodeVal = new Rectangle(320, 411, 100, 610);
        Rectangle rectDetailQtyLabel = new Rectangle(420, 393, 40, 18);
        Rectangle rectDetailQtyVal = new Rectangle(420, 411, 40, 610);
        Rectangle rectDetailChargeDescLabel = new Rectangle(460, 393, 250, 18);
        Rectangle rectDetailChargeDescVal = new Rectangle(460, 411, 250, 610);
        Rectangle rectDetailAmtLabel = new Rectangle(710, 393, 75, 18);
        Rectangle rectDetailAmtVal = new Rectangle(710, 411, 75, 610);

        Rectangle[] rectDetailsArray;
        Rectangle[] rectDetailsArrayHeader;
        #endregion Defines


        /// <summary>
        /// Creates the invoice
        /// </summary>
        /// <param name="strInvoice">Invoice Number cannot empty</param>
        public CInvoice(string strInvoice)
        {
            m_strInvoice = strInvoice;
            if (string.IsNullOrEmpty(m_strInvoice))
            {
                throw new Exception("Cannot print an invoice without an invoice number!");
            }
            CreateInvoiceFonts();
            CreateInvoiceRectangles();
            m_PrintDocInvoice = new PrintDocument();
            m_PrintDocInvoice.BeginPrint += new PrintEventHandler(m_PrintDocInvoice_BeginPrint);
            m_PrintDocInvoice.PrintPage += new PrintPageEventHandler(m_PrintDocInvoice_PrintPage);
            m_ListToPrint = new BindingList<object[]>();
            //IFormatter formatter = new BinaryFormatter();
            //char[] charBuffer = new char[(int)m_PrintDocInvoice.d
            //m_streamText = new MemoryStream(
           
        }

        void m_PrintDocInvoice_BeginPrint(object sender, PrintEventArgs e)
        {
            CurrentY = (float)3;
            CurrentX = (float)25;
            m_nPage = 1;
        }

 
        void m_PrintDocInvoice_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.HasMorePages = CreateForm(e.Graphics);
            
        }

        private bool CreateForm(Graphics graphics)
        {
            try
            {             
                DrawHeader(graphics);
                bool bContinue = DrawDetails(graphics);
                if (!bContinue)
                {
                    m_nPage = 0;
                    DrawFooter(graphics);
                    CurrentRow = 0;
                }
                return bContinue;
            }
            catch (Exception ex)
            {
                CurrentRow = 0;
                return false;
            }

            //return bContinue;
        }

        private void DrawFooter(Graphics graphics)
        {
                graphics.DrawString("** End of Items List **", fontInvRegular, Brushes.DarkSlateBlue,
                    new PointF(rectDetailChargeDescVal.Left, CurrentY));
            
            //    CurrentY += stringSize.Height;
            
            
            #region footer
            graphics.DrawString("NOTE MINUS SIGN (-) INDICATES CREDIT"
            , fontInvRegular
            , Brushes.Black
            , rectFooter);
            #endregion footer
        }

        private bool DrawDetails(Graphics graphics)
        {
           // int nRow = 0;

            while (CurrentRow < m_ListToPrint.Count)
            //for (int i = 0; i < m_ListToPrint.Count; i++)
            {
                if (CurrentRow <= 52)
                {
                    CurrentX = rectBody.Left + 1;
                    string strTemp = DateTime.Parse(m_ListToPrint[CurrentRow][0].ToString()).ToShortDateString();
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY), StringFormat.GenericTypographic);

                    strTemp = m_ListToPrint[CurrentRow][1].ToString();
                    CurrentX = rectDetailAccVal.Left + 1;
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY), StringFormat.GenericTypographic);

                    strTemp = m_ListToPrint[CurrentRow][2].ToString();
                    CurrentX = rectDetailPatNameVal.Left + 1;
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY), StringFormat.GenericTypographic);

                    strTemp = m_ListToPrint[CurrentRow][3].ToString();
                    CurrentX = rectDetailChargeCodeVal.Left + 1;
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY), StringFormat.GenericTypographic);

                    strTemp = m_ListToPrint[CurrentRow][4].ToString();
                    CurrentX = rectDetailQtyVal.Left + 1;
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY), StringFormat.GenericTypographic);

                    strTemp = m_ListToPrint[CurrentRow][5].ToString();
                    CurrentX = rectDetailChargeDescVal.Left + 1;
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY), StringFormat.GenericTypographic);

                    StringFormat sf = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                    strTemp = double.Parse(m_ListToPrint[CurrentRow][6].ToString()).ToString("F2");
                    CurrentX = rectDetailAmtVal.Right - 2;
                    graphics.DrawString(strTemp, fontInvRegular, Brushes.DarkTurquoise,
                        new PointF(CurrentX, CurrentY)
                        ,  sf);

                    CurrentY += stringSize.Height;
                    
                }
                else
                {
                    m_nPage++;
                    if (m_nPage > 3)
                    {
                        return false;
                    }
                    return true;
                }

                CurrentRow++;
            }
            return false;
            
        }

        private void DrawHeader(Graphics graphics)
        {
            // should have billing on the machine so this should work every time.
            try
            {
                m_bLogo = new Bitmap(@"C:\Program Files\Medical Center Laboratory\MCL Billing\mcllogo.bmp");
            }
            catch (Exception)
            {
                m_bLogo = new Bitmap(rectLogo.Width,rectLogo.Height);
            }
            graphics.DrawImage(m_bLogo, rectLogo);
            // write INVOICE in Header
            if (m_nPage > 1)
            {
                graphics.DrawString(string.Format("INVOICE {0} continued. Page {1}", m_strInvoice,m_nPage) , fontInvLarge, Brushes.Black, new PointF((float)400, (float)25));
               // graphics.DrawString(string.Format("INVOICE: {0} Page: {1}", m_strInvoice.PadRight(150), m_nPage),
               //fontInvRegular, Brushes.Black, rectInvoice);
                return;
            }
            else
            {
                graphics.DrawString("INVOICE", fontInvLarge, Brushes.Black, new PointF((float)500, (float)25));
            }
            CurrentY = rectLogo.Height;
            // add our address
            stringSize = new SizeF();
            stringSize = graphics.MeasureString("Medical Center Laboratory", fontInvRegular);
            graphics.DrawString("Medical Center Laboratory", fontInvRegular, Brushes.Black
                , new PointF(CurrentX + 75, CurrentY), StringFormat.GenericTypographic);
            CurrentY += stringSize.Height;
            graphics.DrawString("PO BOX 3099", fontInvRegular, Brushes.Black
                            , new PointF(CurrentX + 75, CurrentY), StringFormat.GenericTypographic);
            CurrentY += stringSize.Height;
            graphics.DrawString("Jackson, Tennessee 38303", fontInvRegular, Brushes.Black
                , new PointF(CurrentX + 75, CurrentY), StringFormat.GenericTypographic);
            CurrentY += stringSize.Height;
            graphics.DrawString("731.927.7300 / 800.642.1703", fontInvRegular, Brushes.Black
                , new PointF(CurrentX + 75, CurrentY), StringFormat.GenericTypographic);
            CurrentY += stringSize.Height;
            // invoice and page number
            graphics.DrawString(string.Format("INVOICE: {0} Page: {1}", m_strInvoice.PadRight(150), m_nPage),
               fontInvRegular, Brushes.Black, rectInvoice);
            // clients address
            CurrentY = (float)rectAddress.Top;
            foreach (string strLine in m_ClientAddress)
            {
                graphics.DrawString(strLine, fontInvRegular, Brushes.Black
                    , new PointF(CurrentX + 90, CurrentY), StringFormat.GenericTypographic);
                CurrentY += stringSize.Height;
            }
            // draw the box around the client address
            graphics.DrawRectangle(Pens.Black, rectAddress);

            #region BalanceArea            // draw the balance rectangles
 
            rectBalancesArray = new Rectangle[] 
            { 
                rectBalPryorBalLabel, rectBalPryorBalAmt,
                rectBalPaymentLabel, rectBalPaymentAmt, 
                rectBalBalForwardLabel, rectBalBalForwardAmt,
                rectBalCurrentChargesLabel, rectBalCurrentChargesAmt, 
                rectBalCurrentAmtDueLabel, rectBalCurrentAmtDueAmt, 
                rectBalNetAmtDueLabel, rectBalNetAmtDueAmt,
                rectBalDateDueLabel, rectBalDateDueDate
            };
            //graphics.DrawRectangles(Pens.Black, rectBalancesArray); 
            Rectangle[] rectWBorder = new Rectangle[] 
             { 
                rectBalPryorBalLabel,
                rectBalPaymentLabel, 
                rectBalBalForwardLabel,
                rectBalCurrentChargesLabel,
                rectBalCurrentAmtDueLabel,
                rectBalNetAmtDueLabel, 
                rectBalDateDueLabel
             };
            graphics.FillRectangles(Brushes.Black, rectWBorder);
            //graphics.FillRectangles(Brushes.Black, new Rectangle[] 
            // { 
            //    rectBalPryorBalLabel,// rectBalPryorBalAmt,
            //    rectBalPaymentLabel, //rectBalPaymentAmt, 
            //    rectBalBalForwardLabel,// rectBalBalForwardAmt,
            //    rectBalCurrentChargesLabel,// rectBalCurrentChargesAmt, 
            //    rectBalCurrentAmtDueLabel,// rectBalCurrentAmtDueAmt, 
            //    rectBalNetAmtDueLabel, //rectBalNetAmtDueAmt,
            //    rectBalDateDueLabel//, rectBalDateDueDate    
            // });
            graphics.DrawRectangles(Pens.White, rectWBorder);
            graphics.DrawRectangles(Pens.Black, new Rectangle[] 
             { 
                rectBalPryorBalAmt,
                rectBalPaymentAmt, 
                rectBalBalForwardAmt,
                rectBalCurrentChargesAmt, 
                rectBalCurrentAmtDueAmt, 
                rectBalNetAmtDueAmt,
                rectBalDateDueDate    
             });

            graphics.DrawRectangle(Pens.Black, rectBalances);

            StringFormat flags = new StringFormat();
            flags.Alignment = StringAlignment.Center;
            flags.FormatFlags = StringFormatFlags.FitBlackBox;
            

            graphics.DrawString("PRIOR BALANCE"
                , fontInvReverse
                , Brushes.White
                , rectBalPryorBalLabel
                , flags);


            graphics.DrawString("PAYMENTS REC'D"
                , fontInvReverse
                , Brushes.White
                , rectBalPaymentLabel
                , flags);


            graphics.DrawString("BALANCE FORWARD"
                , fontInvReverse
                , Brushes.White
                , rectBalBalForwardLabel
                , flags);

            graphics.DrawString("CURRENT CHARGES"
                , fontInvReverse
                , Brushes.White
                , rectBalCurrentChargesLabel
                , flags);

            graphics.DrawString("CURRENT AMOUNT DUE"
                , fontInvReverse
                , Brushes.White
                , rectBalCurrentAmtDueLabel
                , flags);

            graphics.DrawString("NET AMOUNT DUE"
                , fontInvReverse
                , Brushes.White
                , rectBalNetAmtDueLabel
                , flags);

            graphics.DrawString("DUE DATE"
                , fontInvReverse
                , Brushes.White
                , rectBalDateDueLabel
                , flags);


            graphics.DrawString("Yesterday"
                , fontInvReverse
                , Brushes.Black
                , rectBalDateDueDate
                , flags);
#endregion BalanceArea            // end of balance area

            #region details
            rectDetailsArrayHeader = new Rectangle[] 
            { 
                rectDetailTDateLabel,
                rectDetailAccLabel,
                rectDetailPatNameLabel,
                rectDetailChargeCodeLabel,
                rectDetailQtyLabel,
                rectDetailChargeDescLabel,
                rectDetailAmtLabel
            };
            rectDetailsArray = new Rectangle[] 
            { 
                rectDetailTDateLabel, rectDetailTDateVal, 
                rectDetailAccLabel, rectDetailAccVal,
                rectDetailPatNameLabel ,rectDetailPatNameVal, 
                rectDetailChargeCodeLabel,rectDetailChargeCodeVal,
                rectDetailQtyLabel,rectDetailQtyVal,
                rectDetailChargeDescLabel,rectDetailChargeDescVal,
                rectDetailAmtLabel, rectDetailAmtVal
            };
            graphics.DrawRectangles(Pens.Black, rectDetailsArray);
            graphics.FillRectangles(Brushes.Black, new Rectangle[] 
            {
                rectDetailTDateLabel,// rectDetailTDateVal, 
                rectDetailAccLabel, //rectDetailAccVal,
                rectDetailPatNameLabel ,//rectDetailPatNameVal, 
                rectDetailChargeCodeLabel,//rectDetailChargeCodeVal,
                rectDetailQtyLabel,//rectDetailQtyVal,
                rectDetailChargeDescLabel,//rectDetailChargeDescVal,
                rectDetailAmtLabel//, rectDetailAmtVal
            });
            graphics.DrawRectangles(Pens.White, rectDetailsArrayHeader);
            graphics.DrawString("DATE"
                , fontInvReverse
                , Brushes.White
                , rectDetailTDateLabel
                , flags);

            graphics.DrawString("ACCOUNT"
                , fontInvReverse
                , Brushes.White
                , rectDetailAccLabel
                , flags);

            graphics.DrawString("PATIENT NAME"
                , fontInvReverse
                , Brushes.White
                , rectDetailPatNameLabel
                , flags);
            graphics.DrawString("CHARGE CODE"
                , fontInvReverse
                , Brushes.White
                , rectDetailChargeCodeLabel
                , flags);
            graphics.DrawString("QTY"
                , fontInvReverse
                , Brushes.White
                , rectDetailQtyLabel
                , flags);
            graphics.DrawString("CHARGE DESCRIPTION"
                , fontInvReverse
                , Brushes.White
                , rectDetailChargeDescLabel
                , flags);
            graphics.DrawString("AMOUNT"
                , fontInvReverse
                , Brushes.White
                , rectDetailAmtLabel
                , flags);

            #endregion details
            #region Details Info

            #endregion details
            #region Details Info

            #endregion details

            #region Details Info
            stringSize = graphics.MeasureString("THIS", fontInvRegular,
                rectBody.Size, StringFormat.GenericTypographic,
                out int nCharsPerLine, out int nLinesPerPage);
            CurrentY = rectBody.Top + rectDetailTDateLabel.Height;
            #endregion Details Info
     
        }

 

        private void CreateInvoiceRectangles()
        {
           rectBalancesArray = new Rectangle[] 
            { 
                rectBalPryorBalLabel, rectBalPryorBalAmt,
                rectBalPaymentLabel, rectBalPaymentAmt, 
                rectBalBalForwardLabel, rectBalBalForwardAmt,
                rectBalCurrentChargesLabel, rectBalCurrentChargesAmt, 
                rectBalCurrentAmtDueLabel, rectBalCurrentAmtDueAmt, 
                rectBalNetAmtDueLabel, rectBalNetAmtDueAmt,
                rectBalDateDueLabel, rectBalDateDueDate
            };
    
            rectDetailsArray = new Rectangle[] 
            { 
                rectDetailTDateLabel, rectDetailTDateVal, 
                rectDetailAccLabel, rectDetailAccVal,
                rectDetailPatNameLabel ,rectDetailPatNameVal, 
                rectDetailChargeCodeLabel,rectDetailChargeCodeVal,
                rectDetailQtyLabel,rectDetailQtyVal,
                rectDetailChargeDescLabel,rectDetailChargeDescVal,
                rectDetailAmtLabel, rectDetailAmtVal
            };
            
            rectDetailsArrayHeader = new Rectangle[] 
            { 
                rectDetailTDateLabel,
                rectDetailAccLabel,
                rectDetailPatNameLabel,
                rectDetailChargeCodeLabel,
                rectDetailQtyLabel,
                rectDetailChargeDescLabel,
                rectDetailAmtLabel
            };
        }

        private void CreateInvoiceFonts()
        {
            fontInvLarge = new Font(new FontFamily("Arial"), 24, FontStyle.Bold, GraphicsUnit.Pixel);
            fontInvRegular = new Font(new FontFamily("Arial"), 10, FontStyle.Regular, GraphicsUnit.Pixel);
            fontInvReverse = new Font(new FontFamily("Arial"), 10, FontStyle.Bold, GraphicsUnit.Pixel);
        }



        
    }
}
