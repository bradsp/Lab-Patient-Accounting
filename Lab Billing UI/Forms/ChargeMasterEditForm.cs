using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using System.Data;

namespace LabBilling.Forms;

public partial class ChargeMasterEditForm : Form
{

    private bool _addMode = false;
    private readonly DictionaryService _dictionaryService;

    public ChargeMasterEditForm()
    {
        InitializeComponent();

        _dictionaryService = new(Program.AppEnvironment);
    }

    public string SelectedCdm { get; set; }
    public Cdm cdm;
    private readonly BindingSource _feeSched1BindingSource = new BindingSource();
    private readonly BindingSource _feeSched2BindingSource = new BindingSource();
    private readonly BindingSource _feeSched3BindingSource = new BindingSource();
    private readonly BindingSource _feeSched4BindingSource = new BindingSource();
    private readonly BindingSource _feeSched5BindingSource = new BindingSource();

    private void ChargeMasterEditForm_Load(object sender, EventArgs e)
    {
        if (SelectedCdm != null)
        {
            cdm = _dictionaryService.GetCdm(SelectedCdm, true);
            if (cdm == null)
            {
                if (MessageBox.Show($"CDM {SelectedCdm} is not found. Do you want to add?", "Not Found",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                {
                    DialogResult = DialogResult.Cancel;
                    return;
                }
                _addMode = true;
            }
        }
        else
        {
            cdm = new Cdm
            {
                ChargeId = SelectedCdm
            };

            _addMode = true;
        }

        LoadData();
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
        variablePriceCheckBox.Checked = cdm.Variable;

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

        _feeSched1BindingSource.DataSource = cdm.CdmFeeSchedule1.ToDataTable();
        _feeSched2BindingSource.DataSource = cdm.CdmFeeSchedule2.ToDataTable();
        _feeSched3BindingSource.DataSource = cdm.CdmFeeSchedule3.ToDataTable();
        _feeSched4BindingSource.DataSource = cdm.CdmFeeSchedule4.ToDataTable();
        _feeSched5BindingSource.DataSource = cdm.CdmFeeSchedule5.ToDataTable();

        feeSched1Grid.DataSource = _feeSched1BindingSource;
        feeSched2Grid.DataSource = _feeSched2BindingSource;
        feeSched3Grid.DataSource = _feeSched3BindingSource;
        feeSched4Grid.DataSource = _feeSched4BindingSource;
        feeSched5Grid.DataSource = _feeSched5BindingSource;

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
        //dgv.Columns[nameof(CdmDetail.Link)].Visible = true;
        //dgv.Columns[nameof(CdmDetail.Link)].DisplayIndex = colOrder++;
        dgv.Columns[nameof(CdmDetail.CodeFlag)].Visible = true;
        dgv.Columns[nameof(CdmDetail.CodeFlag)].DisplayIndex = colOrder++;
        dgv.Columns[nameof(CdmDetail.Cpt4)].Visible = true;
        ((DataGridViewTextBoxColumn)dgv.Columns[nameof(CdmDetail.Cpt4)]).MaxInputLength = 5;
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

    private void RefreshGrid(DataGridView dgv, BindingSource bs, string feeSched)
    {
        if (dgv.Rows.Count == 1 && dgv.Rows[0].IsNewRow)
        {
            if (MessageBox.Show("Do you want to copy rows from fee schedule 1?", "Copy from Fee Schedule 1?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataRow row in ((DataTable)_feeSched1BindingSource.DataSource).Rows)
                {
                    ((DataTable)bs.DataSource).ImportRow(row);
                    foreach (DataRow r in ((DataTable)bs.DataSource).Rows)
                    {
                        r.BeginEdit();
                        r[nameof(CdmDetail.FeeSchedule)] = feeSched;
                        r[nameof(CdmDetail.rowguid)] = DBNull.Value;
                    }
                }
                dgv.Refresh();
            }
        }
        FormatGrid(dgv);
    }

    private void cptTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cptTabControl.SelectedTab.Name == feeSched1tab.Name)
            FormatGrid(feeSched1Grid);
        else if (cptTabControl.SelectedTab.Name == feeSched2tab.Name)
            RefreshGrid(feeSched2Grid, _feeSched2BindingSource, "2");
        else if (cptTabControl.SelectedTab.Name == feeSched3tab.Name)
            RefreshGrid(feeSched3Grid, _feeSched3BindingSource, "3");
        else if (cptTabControl.SelectedTab.Name == feeSched4tab.Name)
            RefreshGrid(feeSched4Grid, _feeSched4BindingSource, "4");
        else if (cptTabControl.SelectedTab.Name == feeSched5tab.Name)
            RefreshGrid(feeSched5Grid, _feeSched5BindingSource, "5");
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        cdm.ChargeId = chargeIdTextBox.Text;
        cdm.CBillDetail = cbillDetailCheckBox.Checked ? 1 : 0;
        cdm.CClassPaAmount = Convert.ToDouble(cProfTextBox.Text);
        cdm.CClassType = clientChargeType.Text;
        cdm.Comments = notesTextBox.Text;
        cdm.Cost = Convert.ToDouble(costTextBox.Text);
        cdm.Description = descriptionTextBox.Text;
        cdm.IsDeleted = deletedCheckBox.Checked;
        cdm.IsOrderable = orderableCheckBox.Checked ? 1 : 0;
        cdm.Variable = variablePriceCheckBox.Checked;
        cdm.MClassPaAmount = Convert.ToDouble(mProfTextBox.Text);
        cdm.MClassType = patChargeType.Text;
        cdm.ZClassPaAmount = Convert.ToDouble(zProfTextBox.Text);
        cdm.ZClassType = zChargeType.Text;
        cdm.RefLabBillCode = refLabBillCodeTextBox.Text;
        cdm.RefLabId = refLabIdTextBox.Text;
        cdm.RefLabPayment = Convert.ToDouble(refLabPayment.Text);
        cdm.Mnem = lisOrderCodeTextBox.Text;

        cdm.CdmDetails.Clear();
        ReadFeeSchedule(cdm.CdmDetails, feeSched1Grid, "1");
        ReadFeeSchedule(cdm.CdmDetails, feeSched2Grid, "2");
        ReadFeeSchedule(cdm.CdmDetails, feeSched3Grid, "3");
        ReadFeeSchedule(cdm.CdmDetails, feeSched4Grid, "4");
        ReadFeeSchedule(cdm.CdmDetails, feeSched5Grid, "5");

        try
        {
            cdm = _dictionaryService.SaveCdm(cdm);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error saving cdm.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Instance.Error(ex);
            DialogResult = DialogResult.Cancel;
            return;
        }

        return;
    }

    private void ReadFeeSchedule(List<CdmDetail> cdmDetails, DataGridView dgv, string feeSched)
    {
        int linkNo = 1;
        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (row.IsNewRow)
                continue;

            string rg = row.Cells[nameof(CdmDetail.rowguid)].Value.ToString();
            Guid lRowguid = Guid.Empty;
            if (!string.IsNullOrEmpty(rg))
                lRowguid = Guid.Parse(rg);

            cdmDetails.Add(new CdmDetail()
            {
                rowguid = lRowguid,
                FeeSchedule = feeSched,
                Cpt4 = row.Cells[nameof(CdmDetail.Cpt4)].Value.ToString(),
                ChargeItemId = cdm.ChargeId,
                Link = linkNo++,
                CodeFlag = row.Cells[nameof(CdmDetail.CodeFlag)].Value.ToString(),
                Description = row.Cells[nameof(CdmDetail.Description)].Value.ToString(),
                MClassPrice = Convert.ToDouble(row.Cells[nameof(CdmDetail.MClassPrice)].Value.ToString()),
                CClassPrice = Convert.ToDouble(row.Cells[nameof(CdmDetail.CClassPrice)].Value.ToString()),
                ZClassPrice = Convert.ToDouble(row.Cells[nameof(CdmDetail.ZClassPrice)].Value.ToString()),
                RevenueCode = row.Cells[nameof(CdmDetail.RevenueCode)].Value.ToString(),
                Type = row.Cells[nameof(CdmDetail.Type)].Value.ToString(),
                Modifier = row.Cells[nameof(CdmDetail.Modifier)].Value.ToString(),
                BillCode = row.Cells[nameof(CdmDetail.BillCode)].Value.ToString(),
                Cost = Convert.ToDecimal(row.Cells[nameof(CdmDetail.Cost)].Value.ToString())
            });
        }
    }

    private void chargeIdTextBox_Leave(object sender, EventArgs e)
    {
        if (_addMode)
        {
            var record = _dictionaryService.GetCdm(chargeIdTextBox.Text);
            if (record != null)
            {
                if (MessageBox.Show($"Cdm {chargeIdTextBox.Text} exists. Load CDM for editing?", "CDM Exists",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SelectedCdm = record.ChargeId;
                    cdm = record;
                    LoadData();
                    _addMode = false;
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
