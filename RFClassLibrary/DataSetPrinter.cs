using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace RFClassLibrary
{
    /// <summary>
    /// Class designed to print from a DataSet
    /// </summary>
    public class DataSetPrinter
    {
        private string m_strFooterText;
        /// <summary>
        /// Property for setting/getting m_strFooterText
        /// </summary>
        public string propFooterText
        {
            get { return m_strFooterText; }
            set { m_strFooterText = value; }
        }
        /// <summary>
        /// wdk 06/11/2008 version of the DataSetPrinter in use.
        /// </summary>
        private string m_strVersion = "1.0";

        /// <summary>
        /// wdk 06/11/2008 This property is read only and returns the current version of 
        /// RFClassLibrary's DataSetPriter.
        /// </summary>
        public string prop_strVersion
        {
            get { return m_strVersion; }
            //set { m_strVersion = value; } // wdk 06/11/2008 can only set this above
        }

        // The DataSet to be printed
        private DataSet TheDataSet;
        /// <summary>
        ///  The PrintDocument to be used for printing
        /// </summary>
        public PrintDocument ThePrintDocument;

        /// <summary>
        /// Which table in the DataSet to print
        /// </summary>
        private string m_strTheDataTable;

        /// Determine if the report will be printed in the Top-Center of the page
        private bool IsCenterOnPage;
        /// Determine if the page contain title text
        private bool IsWithTitle;
        /// The title text to be printed in each page (if IsWithTitle is set to true)
        private string TheTitleText;
        
        /// The font to be used with the title text (if IsWithTitle is set to true)
        private Font TheTitleFont;
        /// <summary>
        /// The font to be used for the printing.
        /// </summary>
        private Font ThePrintFont;
        /// The color to be used with the title text (if IsWithTitle is set to true)
        private Color TheTitleColor;
        /// Determine if paging is used
        private bool IsWithPaging;
        /// A static parameter that keep track on which Row (in the DataSet) to be printed
        static int CurrentRow;

        private int PageNumber;
        private int PageWidth;
        private int PageHeight;
        private int LeftMargin;
        private int TopMargin;
        private int RightMargin;
        private int BottomMargin;

        // A parameter that keep track on the y coordinate of the page,
        // so the next object to be printed will start from this y coordinate
        private float CurrentY;

        private float RowHeaderHeight;
        private List<float> RowsHeight;
        private List<float> ColumnsWidth;
        private float TheDataSetWidth;

        // Maintain a generic list to hold start/stop points for the column printing
        // This will be used for wrapping in situations where the DataSet will not
        // fit on a single page
        private List<int[]> mColumnPoints;
        private List<float> mColumnPointsWidth;
        private int mColumnPoint;
        private ERR m_ERR;

        /// <summary>
        /// Creates the data set printer and intializes some of the necessary variables
        /// The footer text is set using the class's propFooterText
        /// </summary>
        /// <param name="aDataSet">The DataSet that contains the table we need to print</param>
        /// <param name="strDataTable">The name of the table in the dataset to print</param>
        /// <param name="aPrintDocument">The print document</param>
        /// <param name="aTitleText">The title of the print document</param>
        /// <param name="errLog">Reference to the applications error log</param>
        public DataSetPrinter(DataSet aDataSet, string strDataTable, 
                PrintDocument aPrintDocument, string aTitleText, ref ERR errLog)
        {
            m_ERR = errLog;// assign applications error log for use in this class
            TheDataSet = aDataSet;
            m_strTheDataTable = strDataTable;
            ThePrintDocument = aPrintDocument;
            IsCenterOnPage = true;// CenterOnPage; 06/12/2008 wdk always center.

            IsWithTitle = true; //  WithTitle; 06/12/2008 wdk always with title.
            TheTitleText = aTitleText;

            // 06/11/2008 wdk always use these values
            TheTitleFont = new Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point); //aTitleFont;
            ThePrintFont = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);
            TheTitleColor = Color.Black; //aTitleColor;
            // end of 06/11/2008 wdk
            IsWithPaging = true; // WithPaging; 06/11/2008 wdk always page.

            PageNumber = 0;

            RowsHeight = new List<float>();
            ColumnsWidth = new List<float>();

            mColumnPoints = new List<int[]>();
            mColumnPointsWidth = new List<float>();

            // Set the page margins
            LeftMargin = 25; // Should not change this 06/12/2008 wdk
            TopMargin = 30; // Should not change this 06/12/2008 wdk
            RightMargin = 35; // Don't change this  06/12/2008 wdk
            BottomMargin = 50; // Don't Change this 06/12/2008 wdk 


            // Calculating the PageWidth and the PageHeight set landscape in the application by
            //    MyPrintDocument.DefaultPageSettings.Landscape = true;
            if (!ThePrintDocument.DefaultPageSettings.Landscape)
            {
                // 06/1/2008 wdk our printers cannot print the full 850 the below line returns, so the margins must total more than or equal to 50.
                PageWidth =  
                  (ThePrintDocument.DefaultPageSettings.PaperSize.Width);// - (LeftMargin+RightMargin));
                PageHeight =
                  (ThePrintDocument.DefaultPageSettings.PaperSize.Height);// - (TopMargin+BottomMargin));
            }
            else
            {
                PageHeight =
                  (ThePrintDocument.DefaultPageSettings.PaperSize.Width);// - (TopMargin+BottomMargin));
                PageWidth =
                  (ThePrintDocument.DefaultPageSettings.PaperSize.Height);// - (LeftMargin+RightMargin));
            }
            
            // First, the current row to be printed is the first row in the DataSet
            CurrentRow = 0;

        }

        // This function calculates the height of each row (including the header row),
        // the width of each column (according to the longest text in all its cells including
        // the header cell), and the whole DataSet
        private void Calculate(Graphics g)
        {
            if (PageNumber == 0) // Just calculate once
            {
                // Get the column sizes
                SizeF tmpSize = new SizeF();
                float tmpWidth;
                int mStartPoint = -1;
                int mEndPoint = -1;
                
                TheDataSetWidth = 0;
                for (int nColumnCurrent = 0; nColumnCurrent < TheDataSet.Tables[m_strTheDataTable].Columns.Count; nColumnCurrent++)
                {
                    // if the column is not visible just continue don't waste the time on checking the width.
                    bool bVisible = true;
                    try
                    {
                        bVisible = TheDataSet.Tables[m_strTheDataTable].Columns[nColumnCurrent].ExtendedProperties["VISIBLE"].Equals(true);
                    }
                    catch (Exception)
                    {
                        // print it anyway
                    }
                    if (!bVisible)
                    {
                        ColumnsWidth.Add(0.0f);
                        continue;
                    }
                    else
                    {
                        if (mStartPoint == -1)
                        {
                            mStartPoint = nColumnCurrent; // start with this column
                        }
                        if (nColumnCurrent < TheDataSet.Tables[m_strTheDataTable].Columns.Count)
                        {
                            mEndPoint = nColumnCurrent;
                        }                            
                    }
                    // measure the column's name width using the print font as the basis for our column width.
                    tmpSize = g.MeasureString(TheDataSet.Tables[m_strTheDataTable].Columns[nColumnCurrent].ColumnName,
                                ThePrintFont);
                    tmpWidth = tmpSize.Width; // save the width into our temporary width variable
                    RowHeaderHeight = tmpSize.Height;
                    
                    // Check each Row in the DataSet for this column and see what is the widest column value.
                    for (int nRowCurrent = 0; nRowCurrent < TheDataSet.Tables[m_strTheDataTable].Rows.Count; nRowCurrent++)
                    {
                        tmpSize = g.MeasureString("Anything", ThePrintFont); // string to test against
                        RowsHeight.Add(tmpSize.Height); //should remain constaint
                        // our current rows current columns data to measure
                        tmpSize =  g.MeasureString(
                           TheDataSet.Tables[m_strTheDataTable].Rows[nRowCurrent][nColumnCurrent].ToString(),
                                ThePrintFont);
                        if (tmpSize.Width > tmpWidth)
                        {
                            tmpWidth = tmpSize.Width;
                        }
                    }
                    TheDataSetWidth += tmpWidth;
                    ColumnsWidth.Add(tmpWidth);
                }
                #region hide removed 06/12/2008 wdk set above so the dataset doesn't have to be scrolled through its columns 3 times just once
                // Define the start/stop column points based on the page width and
                // the DataSet Width.  We will use this to determine
                // the columns which are drawn on each page and how wrapping will be handled
                // By default, the wrapping will occur such that the maximum number of
                // columns for a page will be determine
               
                //for (mStartPoint = 0; mStartPoint < TheDataSet.Tables[m_strTheDataTable].Columns.Count; mStartPoint++)
                //{
                //    bool bVisible = true;
                //    try
                //    {
                //        bVisible = TheDataSet.Tables[m_strTheDataTable].Columns[mStartPoint].ExtendedProperties["VISIBLE"].Equals(true);
                //    }
                //    catch
                //    {                        // don't care set default 
                //    }
                //    if (bVisible)
                //    {
                //        break;
                //    }

                //}
 
                //for (mEndPoint = TheDataSet.Tables[m_strTheDataTable].Columns.Count; mEndPoint > 0; mEndPoint--)
                //{
                //    bool bVisible = true;
                //    try
                //    {
                //        bVisible = TheDataSet.Tables[m_strTheDataTable].Columns[mEndPoint].ExtendedProperties["VISIBLE"].Equals(true);
                //    }
                //    catch
                //    {                        // don't care set default 
                //    }
                //    if (!bVisible)  //if (TheDataSet.Tables[m_strTheDataTable].Columns[k].ExtendedProperties["VISIBLE"].Equals(true))
                //    {
                //        break;
                //    }
                //}
                #endregion hide 
               
                float mTempWidth = TheDataSetWidth;
                float mTempPrintArea = (float)PageWidth - (float)LeftMargin -
                    (float)RightMargin;

                // We only care about handling where the total dataset width is bigger than the print area
                if (TheDataSetWidth > mTempPrintArea)
                {
                    mTempWidth = 0.0F;
                    for ( int k = 0; k < TheDataSet.Tables[m_strTheDataTable].Columns.Count; k++)
                    {
                        bool bVisible = true;
                        try
                        {
                            bVisible = TheDataSet.Tables[m_strTheDataTable].Columns[k].ExtendedProperties["VISIBLE"].Equals(true);
                        }
                        catch (Exception )//ex)
                        {// don't care set default                    
                        }
                        //finally
                        //{   // for debug purporses
                        //    throw new Exception("DataSetPrinter Exception in Calculate(Graphics g)");
                        //}
                        if (bVisible)
                        {
                            mTempWidth += ColumnsWidth[k];
                            // If the width is greater than the page with, define a new column print range
                            if (mTempWidth > mTempPrintArea)
                            {
                                mTempWidth -= ColumnsWidth[k];
                                mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
                                mColumnPointsWidth.Add(mTempWidth);
                                mStartPoint = k;
                                mTempWidth = ColumnsWidth[k];
                            }
                        }
                        // Our end point is actually
                        // one index above the current index
                        mEndPoint = k + 1;
                    }
                }
                // Add the last set of columns
                mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
                mColumnPointsWidth.Add(mTempWidth);
                mColumnPoint = 0;
            }
        }

        // The funtion that print the title, page number, and the header row
        private void DrawHeader(Graphics g)
        {
            CurrentY = (float)TopMargin;

            // Printing the page number (if isWithPaging is set to true)
            if (IsWithPaging)
            {
                PageNumber++;
                string PageString = "Page " + PageNumber.ToString();
                string strApp = "C# App: " + OS.GetAppName();

                StringFormat PageStringFormat = new StringFormat();
                PageStringFormat.Trimming = StringTrimming.Word;
                PageStringFormat.FormatFlags = StringFormatFlags.NoWrap |
                    StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                PageStringFormat.Alignment = StringAlignment.Far;

                //
                StringFormat AppStringFormat = new StringFormat();
                AppStringFormat.Trimming = StringTrimming.Word;
                AppStringFormat.FormatFlags = StringFormatFlags.NoWrap |
                    StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                AppStringFormat.Alignment = StringAlignment.Near;


                Font PageStringFont = new Font("Tahoma", 8, FontStyle.Regular,
                    GraphicsUnit.Point);
                //
                Font AppStringFont = new Font("Tahoma", 8, FontStyle.Regular,
                    GraphicsUnit.Point);

                RectangleF PageStringRectangle =
                   new RectangleF((float)LeftMargin, CurrentY,
                   (float)PageWidth - (float)RightMargin - (float)LeftMargin,
                   g.MeasureString(PageString, PageStringFont).Height);

                //
                g.DrawString(strApp, AppStringFont,
                   new SolidBrush(Color.Black),
                   PageStringRectangle, AppStringFormat);

                g.DrawString(PageString, PageStringFont,
                   new SolidBrush(Color.Black),
                   PageStringRectangle, PageStringFormat);

                CurrentY += g.MeasureString(PageString,
                                     PageStringFont).Height;
            }

            // Printing the title (if IsWithTitle is set to true)
            if (IsWithTitle)
            {
                StringFormat TitleFormat = new StringFormat();
                TitleFormat.Trimming = StringTrimming.Word;
                TitleFormat.FormatFlags = StringFormatFlags.NoWrap |
                    StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                if (IsCenterOnPage)
                    TitleFormat.Alignment = StringAlignment.Center;
                else
                    TitleFormat.Alignment = StringAlignment.Near;

                RectangleF TitleRectangle =
                    new RectangleF((float)LeftMargin, CurrentY,
                    (float)PageWidth - (float)RightMargin - (float)LeftMargin,
                    g.MeasureString(TheTitleText, TheTitleFont).Height);

                g.DrawString(TheTitleText, TheTitleFont,
                    new SolidBrush(TheTitleColor),
                    TitleRectangle, TitleFormat);

                CurrentY += g.MeasureString(TheTitleText, TheTitleFont).Height;
            }

            // Calculating the starting x coordinate
            // that the printing process will start from
            float CurrentX = (float)LeftMargin;
            if (IsCenterOnPage)
                CurrentX += (((float)PageWidth - (float)RightMargin -
                  (float)LeftMargin) - mColumnPointsWidth[mColumnPoint]) / 2.0F;

            // Setting the HeaderFore style
            Color HeaderForeColor = Color.White;
                  //TheDataGridView.ColumnHeadersDefaultCellStyle.ForeColor;
            //if (HeaderForeColor.IsEmpty)
            //    // If there is no special HeaderFore style,
            //    // then use the default DataGridView style
            //    HeaderForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
            SolidBrush HeaderForeBrush = new SolidBrush(HeaderForeColor);

            // Setting the HeaderBack style
            Color HeaderBackColor = Color.Black;
            SolidBrush HeaderBackBrush = new SolidBrush(HeaderBackColor);

            // Setting the LinePen that will
            // be used to draw lines and rectangles
            Pen TheLinePen = new Pen(Color.Black, 1);//TheDataGridView.GridColor, 1);

            // Setting the HeaderFont style
            Font HeaderFont = ThePrintFont;// TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
           
            // Calculating and drawing the HeaderBounds        
            RectangleF HeaderBounds = new RectangleF(CurrentX, CurrentY,
                mColumnPointsWidth[mColumnPoint], RowHeaderHeight);
            g.FillRectangle(HeaderBackBrush, HeaderBounds);

            // Setting the format that will be
            // used to print each cell of the header row
            StringFormat CellFormat = new StringFormat();
            CellFormat.Trimming = StringTrimming.Word;
            CellFormat.FormatFlags = StringFormatFlags.NoWrap |
               StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            // Printing each column of the header row
            RectangleF CellBounds;
            float ColumnWidth;
            
            for (int i = (int)mColumnPoints[mColumnPoint].GetValue(0);
                i <= (int)mColumnPoints[mColumnPoint].GetValue(1); i++) //06/17/2008 wdk/rgc add the equal
            {
                /* if the = is used above we get an error on the second page the column title will print but the 
                 * data does not. without the = above the column title will not print but the data does.
                 * This is not an issue if only one page is being printed. Should be ever be able to 
                 * switch between landscape and portrait the clouds will part and the sun will shine.
                 */
                
                // If the column is not visible then ignore this iteration
                bool bVisible = true;
                try
                {
                    bVisible = TheDataSet.Tables[m_strTheDataTable].Columns[i].ExtendedProperties["VISIBLE"].Equals(true);
                }
                catch
                {                        // don't care set default 
                }
                if (!bVisible) //if (!TheDataSet.Tables[m_strTheDataTable].Columns[i].ExtendedProperties["VISIBLE"].Equals(true))
                {
                    continue;
                }
               
                ColumnWidth = ColumnsWidth[i];

                // Check the CurrentCell from the dataset columns extented properties
                // alignment and apply it to the CellFormat
                CellFormat.Alignment = StringAlignment.Near; // Right Alignment by default.
                try
                {

                    if (TheDataSet.Tables[m_strTheDataTable].Columns[i].ExtendedProperties["ALIGNMENT"].ToString().Contains("Right"))
                    {
                        CellFormat.Alignment = StringAlignment.Far;
                    }
                    if (TheDataSet.Tables[m_strTheDataTable].Columns[i].ExtendedProperties["ALIGNMENT"].ToString().Contains("Center"))
                    {
                        CellFormat.Alignment = StringAlignment.Center;
                    }
                }
                catch (Exception)
                {       // don't care use StringAlignment.Near
                }
                CellBounds = new RectangleF(CurrentX, CurrentY,
                             ColumnWidth, RowHeaderHeight);

                // Printing the Column text
                g.DrawString(TheDataSet.Tables[m_strTheDataTable].Columns[i].ColumnName,
                             HeaderFont, HeaderForeBrush,
                   CellBounds, CellFormat);

                // Drawing the cell bounds
                g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth,  RowHeaderHeight);

                CurrentX += ColumnWidth;
            }
            CurrentY += RowHeaderHeight;
        }

        // The function that print a bunch of rows that fit in one page
        // When it returns true, meaning that
        // there are more rows still not printed,
        // so another PagePrint action is required
        // When it returns false, meaning that all rows are printed
        // (the CureentRow parameter reaches
        // the last row of the DataGridView control)
        // and no further PagePrint action is required
        private bool DrawRows(Graphics g)
        {
            // Setting the LinePen that will be used to draw lines and rectangles
            // (derived from the GridColor property of the DataGridView control)
            Pen TheLinePen = new Pen(Color.Gray,1);//TheDataGridView.GridColor, 1);

            // The style paramters that will be used to print each cell
            Font RowFont;
            Color RowForeColor;
            Color RowBackColor;
            SolidBrush RowForeBrush;
            SolidBrush RowBackBrush;
            SolidBrush RowAlternatingBackBrush;

            // Setting the format that will be used to print each cell
            StringFormat CellFormat = new StringFormat();
            CellFormat.Trimming = StringTrimming.Word;
            CellFormat.FormatFlags = StringFormatFlags.NoWrap |
                                     StringFormatFlags.LineLimit;

            // Printing each visible cell
            RectangleF RowBounds;
            float CurrentX;
            float ColumnWidth;
            while (CurrentRow < TheDataSet.Tables[m_strTheDataTable].Rows.Count)
            {
                // Print the cells of the CurrentRow only if that row is visible
                if (true)//TheDataSet.Tables[m_strTheDataTable].Rows[CurrentRow])
                {
                    // Setting the row font style
                    RowFont = ThePrintFont;// TheDataSet.Tables[m_strTheDatatable].Rows[CurrentRow].DefaultCellStyle.Font;
                    // If the there is no special font style of the CurrentRow,
                    // then use the default one associated with the DataGridView control
                    //if (RowFont == null)
                    //    RowFont = TheDataGridView.DefaultCellStyle.Font;

                    // Setting the RowFore style
                    RowForeColor = Color.Black;
                    //  TheDataS.Rows[CurrentRow].DefaultCellStyle.ForeColor;
                    //// If the there is no special RowFore style of the CurrentRow,
                    //// then use the default one associated with the DataGridView control
                    //if (RowForeColor.IsEmpty)
                    //    RowForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
                    RowForeBrush = new SolidBrush(RowForeColor);

                    // Setting the RowBack (for even rows) and the RowAlternatingBack
                    // (for odd rows) styles
                    RowBackColor = Color.White;
                    RowBackBrush = new SolidBrush(RowBackColor);
                    RowAlternatingBackBrush = new SolidBrush(Color.LightGray);

                    // Calculating the starting x coordinate
                    // that the printing process will
                    // start from
                    CurrentX = (float)LeftMargin;
                    if (IsCenterOnPage)
                        CurrentX += (((float)PageWidth - (float)RightMargin -
                            (float)LeftMargin) -
                            mColumnPointsWidth[mColumnPoint]) / 2.0F;

                    // Calculating the entire CurrentRow bounds                
                    RowBounds = new RectangleF(CurrentX, CurrentY,
                        mColumnPointsWidth[mColumnPoint], RowsHeight[CurrentRow]);

                    // Filling the back of the CurrentRow
                    if (CurrentRow % 2 == 0)
                        g.FillRectangle(RowBackBrush, RowBounds);
                    else
                        g.FillRectangle(RowAlternatingBackBrush, RowBounds);

                    // Printing each visible column of the CurrentRow                
                    for (int CurrentCell = (int)mColumnPoints[mColumnPoint].GetValue(0);
                            CurrentCell <= (int)mColumnPoints[mColumnPoint].GetValue(1); // 06/17/2008 wdk/rgc add the equals
                                CurrentCell++)
                    {
                        // If the cell is belong to invisible
                        // column, then ignore this iteration
                        bool bVisible = true;
                        try
                        {
                            bVisible = TheDataSet.Tables[m_strTheDataTable].Columns[CurrentCell].ExtendedProperties["VISIBLE"].Equals(true);
                        }
                        catch
                        {                        // don't care set default\
                        }
                        if (!bVisible) //if (TheDataSet.Tables[m_strTheDataTable].Columns[CurrentCell].ExtendedProperties["VISIBLE"].Equals(false))
                        {
                            continue;
                        }

                            // Check the CurrentCell alignment
                            // and apply it to the CellFormat
                            CellFormat.Alignment = StringAlignment.Near; // Left alignment
                            try
                            {
                                if (TheDataSet.Tables[m_strTheDataTable].Columns[CurrentCell].ExtendedProperties["ALIGNMENT"].ToString().Contains("Right"))
                                {
                                    CellFormat.Alignment = StringAlignment.Far;
                                }
                                if (TheDataSet.Tables[m_strTheDataTable].Columns[CurrentCell].ExtendedProperties["ALIGNMENT"].ToString().Contains("Center"))
                                {
                                    CellFormat.Alignment = StringAlignment.Center;
                                }
                            }
                            catch (Exception)
                            {
                                // don't care use StringAlignment.Near
                            }


                            ColumnWidth = ColumnsWidth[CurrentCell];
                            RectangleF CellBounds = new RectangleF(CurrentX, CurrentY,
                                ColumnWidth, RowsHeight[CurrentRow]);

                            // Printing the cell text
                            string strToPrint = TheDataSet.Tables[m_strTheDataTable].Rows[CurrentRow][CurrentCell].ToString();
                            try
                            {
                                if (TheDataSet.Tables[m_strTheDataTable].Columns[CurrentCell].ExtendedProperties["CURRENCY"].Equals(true))
                                {                                    
                                    double dRetVal = 0.0f;
                                    if (double.TryParse(strToPrint, out dRetVal))
                                    {
                                        strToPrint = string.Format("{0:C}", dRetVal);
                                    }

                                }
                            }
                            catch (Exception)
                            { // keep on trucking
                            }

                            g.DrawString(strToPrint, RowFont, RowForeBrush, CellBounds, CellFormat);

                            // Drawing the cell bounds
                            // Draw the cell border only
                            // if the CellBorderStyle is not None
                            //if (TheDataGridView.CellBorderStyle !=
                            //            DataGridViewCellBorderStyle.None)
                            g.DrawRectangle(TheLinePen, CurrentX, CurrentY,
                                      ColumnWidth, RowsHeight[CurrentRow]);

                            CurrentX += ColumnWidth;
                        }
                        CurrentY += RowsHeight[CurrentRow];

                        // Checking if the CurrentY is exceeds the page boundries
                        // If so then exit the function and returning true meaning another
                        // PagePrint action is required
                        if ((int)CurrentY > (PageHeight - TopMargin - BottomMargin))
                        {
                            CurrentRow++;
                            return true;
                        }
                    }
                    CurrentRow++;
                }

                CurrentRow = 0;
                // Continue to print the next group of columns
                mColumnPoint++;

                if (mColumnPoint == mColumnPoints.Count)
                // Which means all columns are printed
                {
                    mColumnPoint = 0;
                    return false;
                }
                else
                {
                    return true;
                }
            
        }

        /// <summary>
        /// 07/24/2007 wdk
        /// The funtion that prints the Footer
        /// </summary>
        /// <param name="g"></param>
        private void DrawFooter(Graphics g)
        {
            float FooterY = ((float)PageHeight - (float)BottomMargin - 10);

            string PageString = string.Format("Run Date: {0} for {1}", DateTime.Now.ToString("d"), m_strFooterText);

            StringFormat PageStringFormat = new StringFormat();
            PageStringFormat.Trimming = StringTrimming.None;//.Word;
            PageStringFormat.FormatFlags = StringFormatFlags.NoWrap |
                StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            PageStringFormat.Alignment = StringAlignment.Far;

            Font PageStringFont = new Font("Tahoma", 8, FontStyle.Regular,
                GraphicsUnit.Point);

            RectangleF PageStringRectangle =
               new RectangleF((float)LeftMargin, FooterY,
               (float)PageWidth - (float)RightMargin - (float)LeftMargin,
               g.MeasureString(PageString, PageStringFont).Height);

            SizeF sfPageStringWidth = g.MeasureString(PageString, PageStringFont);
            float fPageWidth = PageWidth - RightMargin - LeftMargin;
            if (sfPageStringWidth.Width <= fPageWidth)
            {
                g.DrawString(PageString, PageStringFont,
                   new SolidBrush(Color.Black),
                   PageStringRectangle, PageStringFormat);
            }
            else
            {
                FooterY -= g.MeasureString(PageString, PageStringFont).Height;
                // reset the rectangle after FooterY is changed.
                PageStringRectangle =
               new RectangleF((float)LeftMargin, FooterY,
               (float)PageWidth - (float)RightMargin - (float)LeftMargin,
               g.MeasureString(PageString, PageStringFont).Height);

                int nPageMid = (PageString.Length) / 2;
                string hPageString = PageString.Substring(nPageMid);
                PageString = PageString.Substring(0, nPageMid);
                g.DrawString(PageString, PageStringFont,
                  new SolidBrush(Color.Black),
                  PageStringRectangle, PageStringFormat);

                FooterY += g.MeasureString(PageString, PageStringFont).Height + 3; // advance the line
                // reset the rectangle after FooterY is changed.
                PageStringRectangle =
               new RectangleF((float)LeftMargin, FooterY,
               (float)PageWidth - (float)RightMargin - (float)LeftMargin,
               g.MeasureString(PageString, PageStringFont).Height);

                g.DrawString(hPageString, PageStringFont,
                  new SolidBrush(Color.Black),
                  PageStringRectangle, PageStringFormat);
            }
        }

        /// <summary>
        /// 05/31/2007 wdk
        ///The method that calls all other functions 
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool DrawDataSet(Graphics g)
        {
            bool bContinue = false;
            try
            {
                Calculate(g);
                DrawHeader(g);
            }
            catch (Exception)
            {
            }
            try
            {
                bContinue = DrawRows(g);
                if (!bContinue)
                {
                    PageNumber = 0;
                }
                DrawFooter(g);
               
            }
            catch (Exception )
            {
                //MessageBox.Show("Operation failed: " + ex.Message.ToString(),
                //    Application.ProductName + " - Error", MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);
                //return false;
            }
            return bContinue;
        }

        /// <summary>
        ///  The PrintPage action for the PrintDocument control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MyPrintDocument_PrintPage(object sender,
            System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = DrawDataSet(e.Graphics);

            if (more == true)
            {
                e.HasMorePages = true;
            }
        }


    } // don't go below this line
}
