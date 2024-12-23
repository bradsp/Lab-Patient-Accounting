using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Services;
using LabBilling.Core.Models;
using WinFormsLibrary;

namespace LabBilling.Forms;
public partial class PostRemittanceForm : Form
{
    private readonly Remittance835Service remittanceService = new(Program.AppEnvironment);
    private readonly BindingList<RemittanceFile> remittanceBindingList = new();
    private readonly BindingSource remittanceBindingSource = new();

    public PostRemittanceForm()
    {
        InitializeComponent();

        chargeAmountLabel = new Label();
        chargeAmountLabel.Text = "Charge Amount";
        chargeAmountTextBox = new TextBox();
        chargeAmountTextBox.Text = "0.00";
        paymentAmountLabel = new Label();
        paymentAmountLabel.Text = "Payment Amount";
        paymentAmountTextBox = new TextBox();
        paymentAmountTextBox.Text = "0.00";
        contractualAmountLabel = new Label();
        contractualAmountLabel.Text = "Contractual Amount";
        contractualAmountTextBox = new TextBox();
        contractualAmountTextBox.Text = "0.00";
        checkDateLabel = new Label();
        checkDateLabel.Text = "Check Date";
        checkDateTextBox = new TextBox();
        checkDateTextBox.Text = "";
        deniedAmountLabel = new Label();
        deniedAmountLabel.Text = "Denied Amount";
        deniedAmountTextbox = new TextBox();
        deniedAmountTextbox.Text = "0.00";
        checkNumberLabel = new Label();
        checkNumberLabel.Text = "Check Number";
        checkNumberTextBox = new TextBox();
        checkNumberTextBox.Text = "";
    }

    private readonly Label chargeAmountLabel;
    private readonly TextBox chargeAmountTextBox;
    private readonly Label paymentAmountLabel;
    private readonly TextBox paymentAmountTextBox;
    private readonly Label contractualAmountLabel;
    private readonly TextBox contractualAmountTextBox;
    private readonly Label checkDateLabel;
    private readonly TextBox checkDateTextBox;
    private readonly Label deniedAmountLabel;
    private readonly TextBox deniedAmountTextbox;
    private readonly Label checkNumberLabel;
    private readonly TextBox checkNumberTextBox;

    private void PostRemittanceForm_Load(object sender, EventArgs e)
    {
        tableLayoutPanel1.Controls.Add(chargeAmountLabel, 0, 0);
        tableLayoutPanel1.Controls.Add(chargeAmountTextBox, 1, 0);
        tableLayoutPanel1.Controls.Add(paymentAmountLabel, 2, 0);
        tableLayoutPanel1.Controls.Add(paymentAmountTextBox, 3, 0);
        tableLayoutPanel1.Controls.Add(contractualAmountLabel, 0, 1);
        tableLayoutPanel1.Controls.Add(contractualAmountTextBox, 1, 1);
        tableLayoutPanel1.Controls.Add(checkDateLabel, 2, 1);
        tableLayoutPanel1.Controls.Add(checkDateTextBox, 3, 1);
        tableLayoutPanel1.Controls.Add(deniedAmountLabel, 0, 2);
        tableLayoutPanel1.Controls.Add(deniedAmountTextbox, 1, 2);
        tableLayoutPanel1.Controls.Add(checkNumberLabel, 2, 2);
        tableLayoutPanel1.Controls.Add(checkNumberTextBox, 3, 2);


        remittanceBindingList.AddRange(remittanceService.GetAllRemittances());
        remittancesDataGridView.DataSource = remittanceBindingList;

        remittancesDataGridView.SetColumnsVisibility(false);

        remittancesDataGridView.Columns[nameof(RemittanceFile.RemittanceId)].SetVisibilityOrder(true, 0);
        remittancesDataGridView.Columns[nameof(RemittanceFile.FileName)].SetVisibilityOrder(true, 1);
        remittancesDataGridView.Columns[nameof(RemittanceFile.ProcessedDate)].SetVisibilityOrder(true, 2);

    }

    private void LoadRemittance(RemittanceFile remittance)
    {
        chargeAmountTextBox.Text = remittance.Claims.Sum(c => c.ClaimChargeAmount).ToString("C");
        paymentAmountTextBox.Text = remittance.Claims.Sum(c => c.PaidAmount).ToString("C");
        contractualAmountTextBox.Text = "0.00";
        checkDateTextBox.Text = remittance.ProcessedDate.ToString("MM/dd/yyyy");
        deniedAmountTextbox.Text = "";
        checkNumberTextBox.Text = "";
    }


    private void remittancesDataGridView_SelectionChanged(object sender, EventArgs e)
    {
        if (remittancesDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = remittancesDataGridView.SelectedRows[0];
            var selectedRemittance = (RemittanceFile)selectedRow.DataBoundItem;
            // You can now use selectedRemittance for further processing

            var remittance = remittanceService.GetRemittance(selectedRemittance.RemittanceId);
            LoadRemittance(remittance);
        }
    }
}
