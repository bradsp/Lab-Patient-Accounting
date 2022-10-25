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
using MCL;

namespace LabBilling.Forms
{
    public partial class HealthPlanMaintenanceForm : Form
    {
        private InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        private DataTable _insCompanyTable = null;
        private BindingSource insCompanySource = new BindingSource();
        

        public HealthPlanMaintenanceForm()
        {
            InitializeComponent();
        }

        private void HealthPlanMaintenanceForm_Load(object sender, EventArgs e)
        {
            includeDeletedCheckBox.ForeColor = Color.Black;

            _insCompanyTable = insCompanyRepository.GetAll(false).ToDataTable();

            _insCompanyTable.PrimaryKey = new DataColumn[] { _insCompanyTable.Columns[nameof(InsCompany.InsuranceCode)] };

            insCompanySource.DataSource = _insCompanyTable;

            healthPlanGrid.DataSource = insCompanySource;

            _insCompanyTable.DefaultView.RowFilter = $"{nameof(InsCompany.IsDeleted)} = false";

            //set permissions
            AddPlanButton.Visible = Program.LoggedInUser.CanEditDictionary;

            LoadHealthPlanGrid();
        }

        private void LoadHealthPlanGrid()
        {
            //healthPlanGrid.DataSource = null;
           // healthPlanGrid.Rows.Clear();
            //healthPlanGrid.Columns.Clear();

            //bool excludeDeleted = !includeDeletedCheckBox.Checked;
            //var insCompanies = insCompanyRepository.GetAll(excludeDeleted).OrderBy(x => x.PlanName).ToList();

            //_insCompanyTable = insCompanies.ToDataTable();

            //healthPlanGrid.DataSource = insCompanies;
            healthPlanGrid.Columns[nameof(InsCompany.mod_date)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.mod_host)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.mod_prg)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.mod_user)].Visible = false;
            //healthPlanGrid.Columns[nameof(InsCompany.ClaimFilingIndicatorCode)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.PayorCode)].Visible = false;
            healthPlanGrid.Columns[nameof(InsCompany.rowguid)].Visible = false;

            healthPlanGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            healthPlanGrid.AllowUserToAddRows = false;
            healthPlanGrid.AllowUserToDeleteRows = false;
            healthPlanGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            healthPlanGrid.ForeColor = Color.Black;
            healthPlanGrid.Columns[nameof(InsCompany.InsuranceCode)].Frozen = true;
            healthPlanGrid.Columns[nameof(InsCompany.PlanName)].Frozen = true;
        }

        private void includeDeletedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //LoadHealthPlanGrid();
            if(includeDeletedCheckBox.Checked)
                _insCompanyTable.DefaultView.RowFilter = String.Empty;
            else
                _insCompanyTable.DefaultView.RowFilter = $"{nameof(InsCompany.IsDeleted)} = false";

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

                HealthPlanMaintenanceEditForm form = new HealthPlanMaintenanceEditForm(selectedInsCode);
                if(form.ShowDialog() == DialogResult.OK)
                {
                    var record = _insCompanyTable.Rows.Find(form.insCompany.InsuranceCode);
                    if (record == null)
                    {
                        record = _insCompanyTable.NewRow();
                    }
                    record = form.insCompany.ToDataRow(record);
                }
                //LoadHealthPlanGrid();
            }
        }

        private void AddPlanButton_Click(object sender, EventArgs e)
        {
            HealthPlanMaintenanceEditForm form = new HealthPlanMaintenanceEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var record = _insCompanyTable.Rows.Find(form.insCompany.InsuranceCode);

                if (record == null)
                {
                    record = _insCompanyTable.NewRow();
                }
                record = form.insCompany.ToDataRow(record);
            }
            //LoadHealthPlanGrid();
        }
    }
}
