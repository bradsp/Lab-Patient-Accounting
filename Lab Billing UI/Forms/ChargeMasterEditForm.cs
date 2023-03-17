using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using System.Web.UI.WebControls;
using System.Windows.Forms.DataVisualization.Charting;

namespace LabBilling.Forms
{
    public partial class ChargeMasterEditForm : Form
    {

        private bool addMode = false;
        private CdmRepository cdmRepository;

        public ChargeMasterEditForm()
        {
            InitializeComponent();

            cdmRepository = new CdmRepository(Helper.ConnVal);
        }

        public string SelectedCdm { get; set; }
        public Cdm cdm;
        private BindingSource feeSched1BindingSource = new BindingSource();
        private BindingSource feeSched2BindingSource = new BindingSource();
        private BindingSource feeSched3BindingSource = new BindingSource();
        private BindingSource feeSched4BindingSource = new BindingSource();
        private BindingSource feeSched5BindingSource = new BindingSource();

        private void ChargeMasterEditForm_Load(object sender, EventArgs e)
        {
            if (SelectedCdm != null)
            {
                cdm = cdmRepository.GetCdm(SelectedCdm);
                if(cdm == null)
                {
                    if(MessageBox.Show($"CDM {SelectedCdm} is not found. Do you want to add?", "Not Found", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    {
                        DialogResult = DialogResult.Cancel;
                        return;
                    }
                    addMode = true;
                }
            }
            else
            {
                cdm = new Cdm
                {
                    ChargeId = SelectedCdm,
                    CdmFeeSchedule1 = new List<ICdmDetail>(),
                    CdmFeeSchedule2 = new List<ICdmDetail>(),
                    CdmFeeSchedule3 = new List<ICdmDetail>(),
                    CdmFeeSchedule4 = new List<ICdmDetail>(),
                    CdmFeeSchedule5 = new List<ICdmDetail>()
                };

                addMode = true;
            }

            LoadData();
            //saveButton.Enabled = false; // save function not yet implemented           
        }

        private void LoadData()
        {
            chargeIdTextBox.Text = cdm.ChargeId;
            descriptionTextBox.Text = cdm.Description;
            notesTextBox.Text = cdm.Comments;

            costTextBox.Text = cdm.Cost.ToString();
            orderableCheckBox.Checked = cdm.IsOrderable != 0;
            cbillDetailCheckBox.Checked = cdm.CBillDetail != 0;
            deletedCheckBox.Checked = cdm.IsDeleted;

            lisOrderCodeTextBox.Text = cdm.Mnem;
            refLabBillCodeTextBox.Text = cdm.RefLabBillCode;
            refLabIdTextBox.Text = cdm.RefLabId;
            refLabPayment.Text = cdm.RefLabPayment.ToString();

            patChargeType.SelectedItem = cdm.MClassType;
            clientChargeType.SelectedItem = cdm.CClassType;
            zChargeType.SelectedItem = cdm.ZClassType;
            mProfTextBox.Text = cdm.MClassPaAmount.ToString();
            cProfTextBox.Text = cdm.CClassPaAmount.ToString();
            zProfTextBox.Text = cdm.ZClassPaAmount.ToString();

            feeSched1BindingSource.DataSource = cdm.CdmFeeSchedule1.ToDataTable();
            feeSched2BindingSource.DataSource = cdm.CdmFeeSchedule2.ToDataTable();
            feeSched3BindingSource.DataSource = cdm.CdmFeeSchedule3.ToDataTable();
            feeSched4BindingSource.DataSource = cdm.CdmFeeSchedule4.ToDataTable();
            feeSched5BindingSource.DataSource = cdm.CdmFeeSchedule5.ToDataTable();

            feeSched1Grid.DataSource = feeSched1BindingSource;
            feeSched2Grid.DataSource = feeSched2BindingSource;
            feeSched3Grid.DataSource = feeSched3BindingSource;
            feeSched4Grid.DataSource = feeSched4BindingSource;
            feeSched5Grid.DataSource = feeSched5BindingSource;

            List<DataGridViewComboBoxColumn> comboboxColumns = new List<DataGridViewComboBoxColumn>();
            for (int i = 0; i < 5; i++)
            {
                DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
                comboboxColumn.Items.Add("NORM");
                comboboxColumn.Items.Add("TC");
                comboboxColumn.Items.Add("PC");
                comboboxColumn.Items.Add("N/A");
                comboboxColumn.DataPropertyName = nameof(CdmDetail.Type);
                comboboxColumn.Name = nameof(CdmDetail.Type);
                comboboxColumns.Add(comboboxColumn);
            }

            feeSched1Grid.Columns.Remove(nameof(CdmDetail.Type));
            feeSched1Grid.Columns.Insert(1, comboboxColumns[0]);
            feeSched2Grid.Columns.Remove(nameof(CdmDetail.Type));
            feeSched2Grid.Columns.Insert(1, comboboxColumns[1]);
            feeSched3Grid.Columns.Remove(nameof(CdmDetail.Type));
            feeSched3Grid.Columns.Insert(1, comboboxColumns[2]);
            feeSched4Grid.Columns.Remove(nameof(CdmDetail.Type));
            feeSched4Grid.Columns.Insert(1, comboboxColumns[3]);
            feeSched5Grid.Columns.Remove(nameof(CdmDetail.Type));
            feeSched5Grid.Columns.Insert(1, comboboxColumns[4]);

            FormatGrid(feeSched1Grid);
            FormatGrid(feeSched2Grid);
            FormatGrid(feeSched3Grid);
            FormatGrid(feeSched4Grid);
            FormatGrid(feeSched5Grid);

        }

        private void FormatGrid(DataGridView dgv)
        {
            dgv.SetColumnsVisibility(false);
            int colOrder = 1;
            dgv.Columns[nameof(CdmDetail.Link)].Visible = true;
            dgv.Columns[nameof(CdmDetail.Link)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.CodeFlag)].Visible = true;
            dgv.Columns[nameof(CdmDetail.CodeFlag)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.Cpt4)].Visible = true;
            dgv.Columns[nameof(CdmDetail.Cpt4)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.Description)].Visible = true;
            dgv.Columns[nameof(CdmDetail.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns[nameof(CdmDetail.Description)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.RevenueCode)].Visible = true;
            dgv.Columns[nameof(CdmDetail.RevenueCode)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.Modifier)].Visible = true;
            dgv.Columns[nameof(CdmDetail.Modifier)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.BillCode)].Visible = true;
            dgv.Columns[nameof(CdmDetail.BillCode)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.Type)].Visible = true;
            dgv.Columns[nameof(CdmDetail.Type)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.MClassPrice)].Visible = true;
            dgv.Columns[nameof(CdmDetail.MClassPrice)].DefaultCellStyle.Format = "N2";
            dgv.Columns[nameof(CdmDetail.MClassPrice)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.CClassPrice)].Visible = true;
            dgv.Columns[nameof(CdmDetail.CClassPrice)].DefaultCellStyle.Format = "N2";
            dgv.Columns[nameof(CdmDetail.CClassPrice)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.ZClassPrice)].Visible = true;
            dgv.Columns[nameof(CdmDetail.ZClassPrice)].DefaultCellStyle.Format = "N2";
            dgv.Columns[nameof(CdmDetail.ZClassPrice)].DisplayIndex = colOrder++;
            dgv.Columns[nameof(CdmDetail.Cost)].Visible = true;
            dgv.Columns[nameof(CdmDetail.Cost)].DisplayIndex = colOrder++;
            dgv.AutoResizeColumns();
        }

        private void cptTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cptTabControl.SelectedTab.Name == feeSched1tab.Name)
                FormatGrid(feeSched1Grid);
            else if (cptTabControl.SelectedTab.Name == feeSched2tab.Name)
                FormatGrid(feeSched2Grid);
            else if (cptTabControl.SelectedTab.Name == feeSched3tab.Name)
                FormatGrid(feeSched3Grid);
            else if (cptTabControl.SelectedTab.Name == feeSched4tab.Name)
                FormatGrid(feeSched4Grid);
            else if (cptTabControl.SelectedTab.Name == feeSched5tab.Name)
                FormatGrid(feeSched5Grid);

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            return;

            cdm.ChargeId = chargeIdTextBox.Text;
            cdm.CBillDetail = cbillDetailCheckBox.Checked ? 1 : 0;
            cdm.CClassPaAmount = Convert.ToDouble(cProfTextBox.Text);
            cdm.CClassType = clientChargeType.Text;
            cdm.Comments = notesTextBox.Text;
            cdm.Cost = Convert.ToDouble(costTextBox.Text);
            cdm.Description = descriptionTextBox.Text;
            cdm.IsDeleted = deletedCheckBox.Checked;
            cdm.IsOrderable = orderableCheckBox.Checked ? 1 : 0;
            cdm.MClassPaAmount = Convert.ToDouble(mProfTextBox.Text);
            cdm.MClassType = patChargeType.Text;
            cdm.ZClassPaAmount = Convert.ToDouble(zProfTextBox.Text);
            cdm.ZClassType = zChargeType.Text;
            cdm.RefLabBillCode = refLabBillCodeTextBox.Text;
            cdm.RefLabId = refLabIdTextBox.Text;
            cdm.RefLabPayment = Convert.ToDouble(refLabPayment.Text);
            cdm.Mnem = lisOrderCodeTextBox.Text;


        }

        private void chargeIdTextBox_Leave(object sender, EventArgs e)
        {
            if (addMode)
            {
                var record = cdmRepository.GetCdm(chargeIdTextBox.Text);
                if (record != null)
                {
                    if (MessageBox.Show($"Cdm {chargeIdTextBox.Text} exists. Load CDM for editing?", "CDM Exists",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SelectedCdm = record.ChargeId;
                        cdm = record;
                        LoadData();
                        addMode = false;
                    }
                    else
                    {
                        DialogResult = DialogResult.Cancel;
                        return;
                    }
                }
            }
        }
    }
}
