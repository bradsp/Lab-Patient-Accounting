using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RFClassLibrary
{
    /// <summary>
    /// Local form for responses
    /// </summary>
    public partial class FormResponse : Form
    {
        /// <summary>
        /// returns a filter element if yes is selected to close the form.
        /// </summary>
        public FormResponse()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = 
                DialogResult.Yes;
            
            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = 
                DialogResult.No;
            Close();
        }

        private void tsbSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbFilter.Items.Count; i++ )
            {
                clbFilter.SetItemChecked(i, !clbFilter.GetItemChecked(i));
            }
        }

    }
}
