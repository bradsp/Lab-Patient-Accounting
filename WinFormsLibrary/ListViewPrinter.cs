/* Class developed by David Kelly for printing the ListView.
 * 
 * Created 20091207
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;


namespace Utilities
{
    class ListViewPrinter: PrintDocument
    {
     
        private string m_strHeader;
        /// <summary>
        /// Property for setting or getting the header text.
        /// </summary>
        public string propHeader
        {
            get { return m_strHeader; }
            set { m_strHeader = value; }
        }

        private string m_strFooter;
        /// <summary>
        /// Property for setting or getting the footer text.
        /// </summary>
        public string propFooter
        {
            get { return m_strFooter; }
            set { m_strFooter = value; }
        }

    //    private int m_npgNumber;

        private int m_npgWidth;
        private int m_npgHeight;
        private int m_nLMargin = 0;
        private int m_nRMargin = 0;
        private int m_nTMargin = 0;
        private int m_nBMargin = 0;

        private float m_fYPosition =0.00f;

        private float m_fRowHeight = 0.00f;
        private float m_fRowHeadHeight = 0.00f;
        /// <summary>
        /// List of float values for the column widths.
        /// </summary>
        private List<float> m_lfColWidth = new List<float>();

        private float m_nfListViewWidth = 0.00f;

        private List<Point[]> m_ptColumns = new List<Point[]>();
        private List<float> m_nColPointsWidth = new List<float>();
        private int m_nColPoint= 0;

        PrintDocument m_pd = new PrintDocument();
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="lvToPrint">List View to Print</param>
        /// <param name="pdDoc">Print document to use</param>
        /// <param name="strTitle">Add a title if desired</param>
        public ListViewPrinter(ListView lvToPrint, PrintDocument pdDoc, string strTitle)
        {
            if (lvToPrint == null)
            {        
                 return;		
            }
            int nMargin = m_nLMargin + m_nRMargin + m_nTMargin + m_nBMargin+m_nColPoint;
            float fWid = m_fYPosition + m_fRowHeadHeight + m_fRowHeight+ m_nfListViewWidth;

            if (pdDoc != null)
            {
                m_pd = pdDoc;
            }
            if (!pdDoc.DefaultPageSettings.Landscape)
            {
                m_npgWidth =
                    m_pd.DefaultPageSettings.PaperSize.Width;
                m_npgHeight =
                    m_pd.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                m_npgHeight =
                    m_pd.DefaultPageSettings.PaperSize.Width;
                m_npgWidth =
                    m_pd.DefaultPageSettings.PaperSize.Height;
            }
            
            m_nLMargin = 25;
            m_nTMargin = 30;
            m_nRMargin = 35;
            m_nBMargin = 35;
        }

           private void SetSizes(Graphics g)
           {
               //if (m_npgNumber == 0)
               //{
               //    SizeF sTemp = new SizeF();
               //    Font fTemp;
               //    float fTempWidth;

               //    m_nfListViewWidth = 0;
               //    for (int i = 0; i < m_lvToBePrinted.Columns.Count; i++)
               //    {

               //    }
               //}
           }

        

    } // don't go below this line
}
