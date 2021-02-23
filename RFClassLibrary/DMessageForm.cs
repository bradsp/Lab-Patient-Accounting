using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

namespace RFClassLibrary
{
    /// <summary>
    /// Create printable MessageBox Class
    /// </summary>
    public partial class DMessageForm : Form
    {
         PrintDocument pd = new PrintDocument();
         private StringReader stringToPrint;

            
        /// <summary>
        /// Special MessageBox to print error messages from.
        /// </summary>
        public DMessageForm()
        {
            InitializeComponent();

        }

        /// <summary>
        /// prints the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPrintMessage_Click(object sender, EventArgs e)
        {
            stringToPrint = new StringReader(rtbError.Text);
            pd.Print();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Font printFont;
            printFont = new Font("Arial", 10);

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
               ((line = stringToPrint.ReadLine()) != null))
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

        private void DMessageForm_Load(object sender, EventArgs e)
        {
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        }



    }
}
