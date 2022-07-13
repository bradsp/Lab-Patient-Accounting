using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// programmer added
using System.IO;
using System.Drawing.Printing;
using MetroFramework.Forms;

namespace LabBilling.Forms
{

    /// <summary>
    /// A form to display the errors on an existing record.
    /// </summary>
    public partial class ErrorsForm : MetroForm
    {
        System.Drawing.Printing.PrintDocument pd = null;
        /// <summary>
        /// Form Initiaziation
        /// </summary>
        public ErrorsForm()
        {
            InitializeComponent();
        }

        private void frmErrors_Load(object sender, EventArgs e)
        {
            pd = new System.Drawing.Printing.PrintDocument();
            pd.DocumentName = this.Text;
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);

        }
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            try
            {
                pd.Print();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            int nCharPerLine = 0;
            int nLinesPerPage = 0;
          
            System.Drawing.Font printFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 9.0f);
            //StreamReader sr = new StreamReader(rtbErrors.Text);
          //  MessageBox.Show(rtbErrors.Text);
            string line = rtbErrors.Text;// sr.ReadToEnd();

            // Print each line of the file.
      
            ev.Graphics.MeasureString(line, printFont, ev.MarginBounds.Size, StringFormat.GenericTypographic,
               out nCharPerLine, out nLinesPerPage);
      
            ev.Graphics.DrawString(line, printFont, Brushes.Black, ev.MarginBounds,
               StringFormat.GenericTypographic);
      
            ev.HasMorePages = false;
          
        }

        private void frmErrors_FormClosing(object sender, FormClosingEventArgs e)
        {
           // MessageBox.Show(e.CloseReason.ToString());
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    } // don't go below
}
