using System;
using System.Windows.Forms;

namespace LabBilling.Legacy;

public partial class FormDataCollection : Form
{
    DataGridView gridView = new();
    ToolStripControlHost comboBox = new(new ComboBox());
    ToolStripDropDown dropDown = new();
    public FormDataCollection()
    {
        InitializeComponent();
    }

    private void FormDataCollection_Load(object sender, EventArgs e)
    {
        tsMain.Items.Insert(tsMain.Items.Count, comboBox);
        gridView.BorderStyle = BorderStyle.None;
        gridView.Columns.Add("1", "1");
        gridView.Columns.Add("2", "2");
        gridView.Columns.Add("3", "3");
        #region
        //comment
        string str = this.ToString();
        #endregion




        // dropDown.Items.Add((ToolStripItem)gridView);
        // ((ComboBox)comboBox.Control).dropc.DropDown = dropDown;

    }
}
