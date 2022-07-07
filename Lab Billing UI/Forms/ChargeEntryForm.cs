using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core;
using System;
using System.Windows.Forms;
using LabBilling.Logging;
using System.Linq;

namespace LabBilling.Forms
{
    public partial class ChargeEntryForm : Form
    {
        private Account _currentAccount = new Account();
        private readonly CdmRepository cdmRepository = new CdmRepository(Helper.ConnVal);
        private readonly ChrgRepository dbChrg = new ChrgRepository(Helper.ConnVal);
        MTGCComboBoxItem[] cdmItemsByNo;
        MTGCComboBoxItem[] cdmItemsByDesc;

        public ChargeEntryForm(Account currentAccount)
        {
            Log.Instance.Trace($"Entering");
            _currentAccount = currentAccount;
            InitializeComponent();
        }

        private void ChargeEntryForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tbBannerAccount.Text = _currentAccount.account;
            tbBannerName.Text = _currentAccount.pat_name;
            tbBannerMRN.Text = _currentAccount.mri;
            tbDateOfService.Text = _currentAccount.trans_date.Value.ToShortDateString();

            #region Setup Charge Item Combobox
            var chrgItems = cdmRepository.GetAll().ToList();

            cdmItemsByNo = new MTGCComboBoxItem[chrgItems.Count];
            cdmItemsByDesc = new MTGCComboBoxItem[chrgItems.Count];
            int i = 0;

            foreach (Cdm cdm in chrgItems)
            {
                cdmItemsByDesc[i] = new MTGCComboBoxItem(cdm.descript, cdm.cdm);
                cdmItemsByNo[i] = new MTGCComboBoxItem(cdm.cdm, cdm.descript);
                i++;
            }

            Array.Sort(cdmItemsByNo);
            Array.Sort(cdmItemsByDesc);
            
            cbChargeItem.ColumnNum = 2;
            cbChargeItem.GridLineHorizontal = true;
            cbChargeItem.GridLineVertical = true;
            cbChargeItem.ColumnWidth = "75; 200";
            cbChargeItem.SelectedIndex = -1;
            cbChargeItem.Items.Clear();
            cbChargeItem.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            cbChargeItem.Items.AddRange(cdmItemsByNo);
            #endregion

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            ChargeProcessing cp = new ChargeProcessing(Helper.ConnVal);

            try
            {
                string cdm = "";

                if (SearchByCdm.Checked)
                    cdm = cbChargeItem.SelectedItem.Col1.ToString();
                if (SearchByDescription.Checked)
                    cdm = cbChargeItem.SelectedItem.Col1.ToString();

                cp.AddCharge(_currentAccount.account,
                    cdm,
                    Convert.ToInt32(nQty.Value),
                    _currentAccount.trans_date ?? DateTime.Today,
                    tbComment.Text,
                    ReferenceNumber.Text);
            }
            catch(CdmNotFoundException)
            {
                // this should not happen
                MessageBox.Show("CDM number is not valid.");
            }
            catch(AccountNotFoundException)
            {
                MessageBox.Show("Account number is not valid.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            DialogResult = DialogResult.Cancel;
            return;
        }

        private void SearchByCheckChanged (object sender, EventArgs e)
        {
            if(SearchByCdm.Checked)
            {
                cbChargeItem.ColumnWidth = "75; 200";
                cbChargeItem.SelectedIndex = -1;
                cbChargeItem.Items.Clear();
                cbChargeItem.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
                cbChargeItem.Items.AddRange(cdmItemsByNo);
            }
            else if(SearchByDescription.Checked)
            {
                cbChargeItem.ColumnWidth = "200; 75";
                cbChargeItem.SelectedIndex = -1;
                cbChargeItem.Items.Clear();
                cbChargeItem.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
                cbChargeItem.Items.AddRange(cdmItemsByDesc);
            }

        }
    }
}
