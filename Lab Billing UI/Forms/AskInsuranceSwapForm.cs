using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LabBilling.Core.Models;

namespace LabBilling.Forms;

public partial class AskInsuranceSwapForm : Utilities.BaseForm
{
    private Account _account;
    private DataTable dt;
    private BindingSource bindingSource;
    public string swap1;
    public string swap2;

    public AskInsuranceSwapForm(ref Account account) : base(Program.AppEnvironment)
    {
        _account = account;

        InitializeComponent();
    }

    private void AskInsuranceSwapForm_Load(object sender, EventArgs e)
    {
        dt = new DataTable();
        dt.Columns.Add("Select", typeof(bool));
        dt.Columns.Add("Coverage", typeof(string));
        dt.Columns.Add("Plan Name", typeof(string));

        foreach(Ins ins in _account.Insurances)
        {
            DataRow row = dt.NewRow();
            row["Coverage"] = ins.Coverage;
            row["Plan Name"] = ins.PlanName;
            dt.Rows.Add(row);
        }

        bindingSource = new BindingSource();
        bindingSource.DataSource = dt;

        insSwapDataGrid.DataSource = bindingSource;

        insSwapDataGrid.Columns["Plan Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        insSwapDataGrid.AutoResizeColumns();

    }

    private void okButton_Click(object sender, EventArgs e)
    {
        int checkedRows = 0;
        //get the checked rows

        List<string> swap = new List<string>();

        for (int i = 0; i < insSwapDataGrid.RowCount; i++)
        {
            DataGridViewCheckBoxCell cbCell = insSwapDataGrid.Rows[i].Cells["Select"] as DataGridViewCheckBoxCell;
            bool checkedValue = string.IsNullOrEmpty(cbCell.Value.ToString()) ? false : true;

            if (checkedValue)
            {
                checkedRows++;
                if(checkedRows > 2)
                {
                    MessageBox.Show("Cannot select more than two insurances to swap. Remove one of the checkboxes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                swap.Add(insSwapDataGrid.Rows[i].Cells["Coverage"].Value.ToString());    
            }
        }

        if(swap.Count != 2)
        {
            MessageBox.Show("Must have two insurances selected for swap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        swap1 = swap[0];
        swap2 = swap[1];
        DialogResult = DialogResult.OK;
        return;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        return;
    }

    private void insSwapDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {

    }
}
