using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Core
{
    public partial class LabeledTextBox : UserControl
    {
        public LabeledTextBox()
        {
            InitializeComponent();
        }

        public string Label 
        { 
            get { return label1.Text; }
            set { label1.Text = value; } 
        }

    }
}
