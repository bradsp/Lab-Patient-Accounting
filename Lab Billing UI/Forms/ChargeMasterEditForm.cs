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

namespace LabBilling.Forms
{
    public partial class ChargeMasterEditForm : Form
    {
        public ChargeMasterEditForm()
        {
            InitializeComponent();
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
            CdmRepository cdmRepository = new CdmRepository(Helper.ConnVal);
            cdm = cdmRepository.GetCdm(SelectedCdm);
            
            saveButton.Enabled = false; // save function not yet implemented
            
            LoadData();
        }

        private void LoadData()
        {
            chargeIdTextBox.Text = cdm.ChargeId;
            descriptionTextBox.Text = cdm.Description;
            notesTextBox.Text = cdm.Comments;

            costTextBox.Text = cdm.Cost.ToString();
            orderableCheckBox.Checked = cdm.IsOrderable == 0 ? false : true;
            cbillDetailCheckBox.Checked = cdm.CBillDetail == 0 ? false : true;
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
            MessageBox.Show("Save function not implemeted.");
        }
    }
}
