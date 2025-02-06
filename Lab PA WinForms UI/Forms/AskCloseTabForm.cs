using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;


namespace LabBilling.Forms;

public partial class AskCloseTabForm : Form
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public List<string> OpenTabs { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string SelectedForm { get; set; }

    public AskCloseTabForm(List<string> openTabs)
    {
        InitializeComponent();
        OpenTabs = openTabs;
    }

    private void OKbutton_Click(object sender, EventArgs e)
    {
        if (OpenTabsList.SelectedItems.Count > 0)
        {
            SelectedForm = OpenTabsList.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
        else
        {
            MessageBox.Show("Please select a tab to close.", "Select Tab", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
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
