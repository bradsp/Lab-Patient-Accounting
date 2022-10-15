using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;

namespace LabBilling.Forms
{
    public partial class HealthPlanMaintenanceForm : Form
    {
        InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        

        public HealthPlanMaintenanceForm()
        {
            InitializeComponent();
        }

        private void HealthPlanMaintenanceForm_Load(object sender, EventArgs e)
        {
            includeDeletedCheckBox.ForeColor = Color.Black;
            LoadHealthPlanGrid();
        }

        private void LoadHealthPlanGrid()
        {
            healthPlanGrid.DataSource = null;
            healthPlanGrid.Rows.Clear();
            healthPlanGrid.Columns.Clear();

            bool excludeDeleted = !includeDeletedCheckBox.Checked;
            var insCompanies = insCompanyRepository.GetAll(excludeDeleted).OrderBy(x => x.PlanName).ToList();

            healthPlanGrid.DataSource = insCompanies;
            healthPlanGrid.Columns[nameof(InsCompany.mod_date)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.mod_host)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.mod_prg)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.mod_user)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.ClaimFilingIndicatorCode)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.PayorCode)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.rowguid)].Visible = false;

            healthPlanGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            healthPlanGrid.AllowUserToAddRows = false;
            healthPlanGrid.AllowUserToDeleteRows = false;
            healthPlanGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            healthPlanGrid.ForeColor = Color.Black;
            healthPlanGrid.Columns[nameof(InsCompany.InsuranceCode)].Frozen = true;
            healthPlanGrid.Columns[nameof(InsCompany.PlanName)].Frozen = true;

            //set permissions

        }

        private void includeDeletedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadHealthPlanGrid();
        }

        private void healthPlanGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToBoolean(healthPlanGrid.Rows[e.RowIndex].Cells[nameof(InsCompany.IsDeleted)].Value) == true)
            {
                healthPlanGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        private void healthPlanGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = healthPlanGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                string selectedInsCode = healthPlanGrid.SelectedRows[0].Cells[nameof(InsCompany.InsuranceCode)].Value.ToString();

                HealtPlanMaintenanceEditForm form = new HealtPlanMaintenanceEditForm(selectedInsCode);
                form.ShowDialog();

                return;
            }

        }
    }
}
