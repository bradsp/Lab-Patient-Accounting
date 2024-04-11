using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
// programmer added



namespace Utilities
{
    /// <summary>
    /// 01/10/08 rgc/wdk
    /// Derived PrintPreviewDialog that adds print dialog functionality to the preview.
    /// 02/08/2008 wdk 
    /// Modified to handle a null reference in the Load() that is setting the 
    ///     the forms title text and append the file name from the BaseStream of the StreamReader via the FileStream.
    ///     If null just don't do it.
    ///     Also added a property for Topmost (initially false) to the base, so it is not a pain to use.
    /// 
    /// Calling example 1
    /// ViewerPrint() is derived from this class. ViewerPrint() is provided to be used with the MCL billing applications.
    ///     In the Viewers Load() the document is set up like the following.
    ///     
    ///     base.m_streamToPrint = new StreamReader(strFileName); //Required and strFileName is complete with path\file.ext
    ///     base.PrintPreviewControl.ForeColor = Color.GhostWhite; // Optional 
    ///     base.DocumentName = strFileName.Substring(strFileName.LastIndexOf(@"\"),
    ///            (strFileName.LastIndexOf(@".")-strFileName.LastIndexOf(@"\")));  // optional
    ///     base.propAboutDlg = this.m_AboutDlg; // optional. This is the About box of the application. In ViewerPrints() case its AboutBoxViewerPrint() etc.
    ///      
    /// end of 02/08/2008 changes
    /// 
    /// Calling example 2
    ///     RFCPrintPreview ppd = new RFCPrintPreview(); // Create an Instance of the class.
    ///     StreamReader m_streamToPrint = new StreamReader(@"C:\Temp\file.txt");
    ///     ppd.m_streamToPrint = m_streamToPrint;       // set the stream to print. 
    ///     
    ///     or using a file open dialog.
    ///     
    ///     OpenFileDialog ofd = new OpenFileDialog();
    ///     if (ofd.ShowDialog() == DialogResult.OK)
    ///     {
    ///         ppd.m_streamToPrint = new StreamReader(ofd.FileName);
    ///     }
    ///     else
    ///     {
    ///         return; // no stream set so adios
    ///     }
    ///     ppd.DocumentName = "YourDocumentNameHere";
    ///     ppd.ShowDialog();
    /// 
    /// </summary>
    public partial class RFCPrintPreview : PrintPreviewDialog
    {
        class FormEventArgs : EventArgs
        {
           public FormEventArgs(int nPage, bool bInitial)
           {
               this.nPage = nPage;
               this.bInitial = bInitial;
           }

            private int nPage = 1;
            public int propPage
            {
                get { return nPage ; }
                set { nPage = value; }
            }
 
            private bool bInitial;
            public bool propInitial
            {
                get { return bInitial; }
                set { bInitial = value; }
            }
	
        }
        private int m_nPage = 0;
        //private int[] m_nPageSearchFound;
        //public int[] propPageSearchFound
        //{
        //    get { return m_nPageSearchFound; }
        //}
        private string m_strToSearchFor = null;
        /// <summary>
        /// Allows searching for a value in a particular file if found a 
        /// header is printed and the page is started new.
        /// </summary>
        public string propStrToSearchFor
        {
            get { return m_strToSearchFor; }
            set { m_strToSearchFor = value; }
        }
        ToolStripStatusLabel m_tsslNotify; // notification area.
        private bool m_bLandscape = true;
        /// <summary>
        /// Give the programmer the choice to set landscape or not.
        /// </summary>
        public bool propLandscape
        {
            get { return m_bLandscape; }
            set { m_bLandscape = value; }
        }
        /// <summary>
        /// Both m_strToTruncate and m_strToPrint are used for printing
        /// </summary>
        private string m_strToTruncate;
        private string m_strToPrint;

        /// <summary>
        /// Used to set the m_strToPrint 
        /// as of 09/30/2008 wdk converts a FileStream, MemoryStream, StreamReader or a string to set
        /// m_strToPrint
        /// 
        /// </summary>
        public object propStreamToPrint
        {
            set
            {
                switch (value.GetType().ToString().ToUpper())
                {
                    case "SYSTEM.IO.FILESTREAM":
                        {
                            FileStream fs = (FileStream)value;
                            StreamReader streamToPrint = new StreamReader(fs);
                            streamToPrint.BaseStream.Position = 0;
                            byte[] byteBuffer = new byte[(int)streamToPrint.BaseStream.Length + 1];
                            char[] charBuffer = new char[(int)streamToPrint.BaseStream.Length + 1];
                            streamToPrint.Read(charBuffer, 0, (int)streamToPrint.BaseStream.Length);
                            byteBuffer = Encoding.ASCII.GetBytes(charBuffer);
                            m_strToPrint = new string(charBuffer);
                            m_strToTruncate = m_strToPrint;
                            break;
                        }
                    case "SYSTEM.IO.STRINGREADER":
                        {
                            m_strToPrint = ((StringReader)value).ReadToEnd();
                            m_strToTruncate = m_strToPrint;
                            break;
                        }
                    case "SYSTEM.STRING":
                        {
                            m_strToPrint = (string)value;
                            m_strToTruncate = m_strToPrint;
                            break;
                        }
                    case "SYSTEM.IO.MEMORYSTREAM":
                        {
                            MemoryStream memoryStream = (MemoryStream)value;
                            StreamReader streamReader = new StreamReader(memoryStream);
                            streamReader.BaseStream.Position = 0;
                            //byte[] byteBuffer = new byte[(int)memoryStream.Length + 1];
                            char[] charBuffer = new char[(int)memoryStream.Length + 1];
                            streamReader.Read(charBuffer, 0, (int)streamReader.BaseStream.Length);
                            //byteBuffer = Encoding.ASCII.GetBytes(charBuffer);
                            m_strToPrint = new string(charBuffer);
                            m_strToTruncate = m_strToPrint;
                            break;
                        }
                    case "SYSTEM.IO.STREAMREADER":
                        {
                            StreamReader streamToPrint = (StreamReader)value;
                            streamToPrint.BaseStream.Position = 0;
                           // byte[] byteBuffer = new byte[(int)streamToPrint.BaseStream.Length + 1];
                            char[] charBuffer = new char[(int)streamToPrint.BaseStream.Length + 1];
                            streamToPrint.Read(charBuffer, 0, (int)streamToPrint.BaseStream.Length);
                           // byteBuffer = Encoding.ASCII.GetBytes(charBuffer);
                           // m_memoryStream = new MemoryStream(byteBuffer);
                            m_strToPrint = new string(charBuffer);
                            m_strToTruncate = m_strToPrint;
                            break;
                        }
                    default:
                        {
                            StreamReader streamToPrint = (StreamReader)value;
                            streamToPrint.BaseStream.Position = 0;
                           // byte[] byteBuffer = new byte[(int)streamToPrint.BaseStream.Length + 1];
                            char[] charBuffer = new char[(int)streamToPrint.BaseStream.Length + 1];
                            streamToPrint.Read(charBuffer, 0, (int)streamToPrint.BaseStream.Length);
                            //byteBuffer = Encoding.ASCII.GetBytes(charBuffer);
                            //m_memoryStream = new MemoryStream(byteBuffer);
                            m_strToPrint = new string(charBuffer);
                            m_strToTruncate = m_strToPrint;
                            break;
                        }
                }
            }
        }
        
        /// <summary>
        /// RFCPrintPreview's print dialog.
        /// </summary>
        public PrintDialog m_dlgPrint;

        /// <summary>
        /// Document name for printing ONLY. Mirrors the base classes naming convention.
        /// </summary>
        public string m_DocumentName;

        private bool m_bTopMost = false;

        /// <summary>
        /// Displays the dialog on top when set true.
        /// </summary>
        public bool propTopMost
        {
            get { return m_bTopMost; }
            set { m_bTopMost = value; }
        }
        private Form m_dlgAbout = null;

        /// <summary>
        /// Sets the about box for the printer dialog for the application that calls the Print Dialog.
        /// If no About box is passed then the button to display the About box is not created, and placed on
        /// the tool strip.
        /// 
        /// 02/08/2008 wdk
        /// </summary>
        public Form propAboutDlg
        {
            get { return m_dlgAbout; }
            set { m_dlgAbout = value; }
        }

        FormEventArgs m_FormEventArgs;

	    /// <summary>
	    /// Initial constructor that Initializes components.
	    /// </summary>
        public RFCPrintPreview()
        {
            InitializeComponent();
            m_FormEventArgs = new FormEventArgs(1,true);
        }

        private void RFCPrintPreview_Load(object sender, EventArgs e)
        {
            Bitmap image1 = null;
            try
            {
                image1 = new Bitmap(@"C:\bmps\boom2.bmp", true);
            }
            catch (Exception)
            {
               // don't display
            }
            base.Document = new PrintDocument();
      //      if (m_FormEventArgs.propInitial.Equals(true))
           // {
                m_FormEventArgs.propInitial = false;
                // create the print button to place on the toolstip.
                ToolStripButton tsbtnPrint = new ToolStripButton("&PRINT", null, tsmiPrint_Click);
                tsbtnPrint.BackColor = Color.Gold;
                tsbtnPrint.ToolTipText = "Use this print button to dislay the Print Control Dialog.";

                ToolStripButton tsbtnAbout = null;
                if (m_dlgAbout != null)
                {
                    // create the about button to place on the toolstip.
                    tsbtnAbout = new ToolStripButton("&ABOUT", null, tsmiAbout_Click);
                    tsbtnAbout.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    tsbtnAbout.BackColor = Color.BlanchedAlmond;
                    tsbtnAbout.ToolTipText = "Displays information about the Print Control Dialog.";
                }
                ToolStripButton tsbtnLandscape = new ToolStripButton("&LANDSCAPE", null, tsmiLandscape_Click);
                tsbtnLandscape.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                tsbtnLandscape.BackColor = Color.Cornsilk;
                tsbtnLandscape.ToolTipText = "Toggle document from Portrate to Landscape and back";
                foreach (Control ctl in base.Controls)
                {
                    m_tsslNotify = new ToolStripStatusLabel();
                    if (ctl.GetType() == typeof(StatusStrip) || ctl.GetType() == typeof (StatusStrip))
                    {
                        ((StatusStrip)ctl).Items.Add(new ToolStripSeparator());
                        ((StatusStrip)ctl).Items.Add(m_tsslNotify);
                    }
                    else
                    {
                        StatusStrip nStatus = new StatusStrip();
                        nStatus.Items.Add(m_tsslNotify);
                        base.Controls.Add(nStatus);
                    }

                    if (ctl.GetType() == typeof(ToolStrip))
                    {
                        if (ctl.Text.ToUpper() == "TOOLSTRIP1")
                        {
                            ((ToolStrip)ctl).Items[0].ToolTipText = "Use this print button to print without displaying the Print Control Dialog.";
                            //((ToolStrip)ctl).Items[0].Visible = false;
                            ((ToolStrip)ctl).Items.Add(new ToolStripSeparator());
                            ((ToolStrip)ctl).Items.Add(tsbtnPrint);
                            ((ToolStrip)ctl).Items.Add(new ToolStripSeparator());
                          //  ((ToolStrip)ctl).Items.Add(tsbtnLandscape); // 09/30/2008 wdk can't flip it yet so disable
                          //  ((ToolStrip)ctl).Items.Add(new ToolStripSeparator());
                            if (tsbtnAbout != null)
                            {
                                ((ToolStrip)ctl).Items.Add(tsbtnAbout);
                                ((ToolStrip)ctl).Items.Add(new ToolStripSeparator());
                            }

                            break;
                        }
                    }
                }
               
                base.Document.DocumentName = m_DocumentName;
                base.Document.DefaultPageSettings.Landscape = m_bLandscape;
               // base.Document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPageStream);
                base.Document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
                if (m_DocumentName != null)
                {
                    ((Form)this).Text += string.Format(" -- File: {0}", m_DocumentName.Length > 0 ? m_DocumentName : "");
                }
            ((Form)this).TopMost = m_bTopMost;
            
           
            
        }

        /// <summary>
        /// Click handler for toggle between portrait and landscape 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiLandscape_Click(object sender, EventArgs e)
        {
            m_bLandscape = ((ToolStripItem)sender).Text.Equals("&LANDSCAPE");
            if (m_bLandscape)
            {
                ((ToolStripItem)sender).Text = "&PORTRAIT";
            }
            else
            {
                ((ToolStripItem)sender).Text = "&LANDSCAPE";
            }
            base.Document.DefaultPageSettings.Landscape = m_bLandscape;
            
        }


       private void tsmiPrint_Click(object sender, EventArgs e)
        {
            m_dlgPrint = new PrintDialog();
            m_dlgPrint.Document = base.Document;
           
            if (m_dlgPrint.ShowDialog() == DialogResult.OK)
            {
                //base.Document.PrinterSettings.PrinterName = m_dlgPrint.PrinterSettings.PrinterName;
                base.Document.PrinterSettings = m_dlgPrint.PrinterSettings;
                base.Document.Print();
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            m_dlgAbout.ShowDialog();
        }

        /// <summary>
        /// Printing Guidelines
        /// Margins	    |           Landscape	            |       Portrait
	    ///             | Lines Per Page  |	Characters      |   Lines Per Page	    |Characters 
        ///             |                 | per line        |                       |per line
        /// 1 inch      |	    33	      |      97	        |           46          |	70
        /// ½ inch	    |       38        | 	107	        |           52          |	80
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>    
        private void pd_PrintPageStream(object sender, PrintPageEventArgs ev)
        {
            m_nPage++;
            
            int charactersOnLine = ev.MarginBounds.Left == 100 ? m_bLandscape ? 97 : 107 : m_bLandscape ? 70 : 80;
            int linesPerPage = ev.MarginBounds.Left == 100 ? m_bLandscape ? 33 : 38 : m_bLandscape ? 46 : 52;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            Font printFont = new Font("Arial", 14);
          
            string[] strSplit = m_strToTruncate.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in strSplit)
            {
                Application.DoEvents(); // wdk 20130124 testing
                string strLine = line;
                if (m_strToSearchFor != null)
                {
                    if (strLine.Contains(m_strToSearchFor))
                    {
                        if (count == 0)
                        {
                            PrintSearchHeader(ev, leftMargin, topMargin);
                            count = 2;
                        }
                        else
                        {
                            ev.HasMorePages = true;
                            return;
                        }
                    }
                }
                
                if (count < linesPerPage)
                {
                    yPos = topMargin + (count *
                       printFont.GetHeight(ev.Graphics));
                   // int nWidth = 0;
                    float tabMargin = leftMargin;
                    while (strLine.Length > charactersOnLine)
                    {
                        string strLinePart = strLine.Substring(0, charactersOnLine-(int)(tabMargin-leftMargin));
                       ev.Graphics.DrawString(strLinePart, printFont, Brushes.Black,
                       tabMargin, yPos, new StringFormat());
                       strLine = strLine.Substring(charactersOnLine-(int)(tabMargin-leftMargin));
                        count++;
                       yPos = topMargin + (count *
                       printFont.GetHeight(ev.Graphics));
                       if (tabMargin == leftMargin)
                       {
                           tabMargin += 5.0f;
                       }
                    }
                    ev.Graphics.DrawString(strLine, printFont, Brushes.Black,
                       leftMargin, yPos, new StringFormat());
                    count++;
                    #region footer
                    //// print footer
                    //if (count == (linesPerPage - 2))
                    //{
                    //    line = string.Format("");
                    //    yPos = topMargin + (count *
                    //   printFont.GetHeight(ev.Graphics));
                    //    ev.Graphics.DrawString(line, printFont, Brushes.Black,
                    //       leftMargin, yPos, new StringFormat());
                    //    count++;

                    //    line = string.Format("Page {0}", m_nPage++);
                    //    yPos = topMargin + (count *
                    //   printFont.GetHeight(ev.Graphics));
                    //    ev.Graphics.DrawString(line, printFont, Brushes.Black,
                    //       leftMargin, yPos, new StringFormat());
                    //    count++;
                    //}
                    #endregion footer
                    m_strToTruncate = m_strToTruncate.Substring(line.Length);
                    ev.HasMorePages = true;
                }
                else
                {

                    return;               
                }
            }

            ev.HasMorePages = false;
            m_strToTruncate = m_strToPrint; // restore the string so we can print
        }


       // static int nBreak = 0;
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font printFont = new Font("Arial", 14);
          
            int charactersOnPage = 0;
            int linesPerPage = 0;
            if (!m_strToTruncate.Contains("*** SEARCH VALUE FOUND ***") && !string.IsNullOrEmpty(m_strToSearchFor))
            {
                if (m_strToTruncate.Contains(m_strToSearchFor))
                {
                    m_strToTruncate = m_strToTruncate.Replace(m_strToSearchFor, string.Format("\r\n *** SEARCH VALUE FOUND *** \r\n {0}", m_strToSearchFor));
                }
            }
            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            
            e.Graphics.MeasureString(m_strToTruncate, printFont,//this.Font,
                e.MarginBounds.Size, new StringFormat(StringFormat.GenericTypographic),
                out charactersOnPage, out linesPerPage);
            
            // Draws the string within the bounds of the page
            e.Graphics.DrawString(m_strToTruncate, 
                    printFont,//this.Font, 
                        Brushes.Black,
                            e.MarginBounds, 
                                new StringFormat(StringFormat.GenericTypographic));

            // Remove the portion of the string that has been printed.
            m_strToTruncate = m_strToTruncate.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (m_strToTruncate.Length > 0);
            if (!e.HasMorePages)
            {
                m_strToTruncate = m_strToPrint; // restore the string for printing not just showing.
            }
            //if (nBreak < 3)
            //{
            //    nBreak++;
            //}
            //else
            //{
            //    e.HasMorePages = false;
            //    m_strToTruncate = m_strToPrint; // restore the string so we can print
            //}
        }

        

        private void PrintSearchHeader(PrintPageEventArgs ev, float leftMargin, float topMargin)
        {
            //if (m_nPageSearchFound == null)
            //{
            //    m_nPageSearchFound = new int[5] {0,0,0,0,0};
            //}
            //for (int i= 0; i < m_nPageSearchFound.GetLength(0); i++)
            //{
            //    if (!m_nPageSearchFound.GetValue(i).Equals(0))
            //    {
            //        continue;
            //    }
            //    m_nPageSearchFound.SetValue(m_nPage,i);
            //}
            m_tsslNotify.Text += m_nPage.ToString() + ", ";
            string strHeader = "* * * * * * * * SEARCH VALUE FOUND * * * * * * * *";
            Font printFont = new Font("Arial", 12);
            ev.Graphics.DrawString(strHeader, printFont, Brushes.Red,
                      leftMargin, topMargin, new StringFormat());
            //e.Graphics.MeasureString(m_strToTruncate, this.Font,
            //    e.MarginBounds.Size, new StringFormat(StringFormat.GenericTypographic),
            //    out charactersOnPage, out linesPerPage);
        }


    } // don't go below this line
}