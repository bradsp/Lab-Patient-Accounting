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
    }

    private void PostRemittanceForm_Load(object sender, EventArgs e)
    {


        remittanceBindingList.AddRange(remittanceService.GetAllRemittances());
        remittancesDataGridView.DataSource = remittanceBindingList;

        remittancesDataGridView.SetColumnsVisibility(false);

        remittancesDataGridView.Columns[nameof(RemittanceFile.RemittanceId)].SetVisibilityOrder(true, 0);
        remittancesDataGridView.Columns[nameof(RemittanceFile.FileName)].SetVisibilityOrder(true, 1);
        remittancesDataGridView.Columns[nameof(RemittanceFile.ProcessedDate)].SetVisibilityOrder(true, 2);

    }

    private void LoadRemittanceGrid()
    {

    }

    private void remittancesDataGridView_SelectionChanged(object sender, EventArgs e)
    {

    }
}
