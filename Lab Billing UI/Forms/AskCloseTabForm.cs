using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace LabBilling.Forms
{
    public partial class AskCloseTabForm : MetroForm
    {
        public List<string> OpenTabs { get; set; }
        public string SelectedForm { get; set; }

        public AskCloseTabForm()
        {
            InitializeComponent();
        }

        public AskCloseTabForm(List<string> openTabs) : this()
        {
            OpenTabs = openTabs;
            
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            SelectedForm = OpenTabsList.SelectedItem.ToString();

            this.DialogResult = DialogResult.OK;

            return;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            return;
        }

        private void AskCloseTabForm_Load(object sender, EventArgs e)
        {
            OpenTabsList.Items.AddRange(OpenTabs.ToArray());

        }
    }
}
