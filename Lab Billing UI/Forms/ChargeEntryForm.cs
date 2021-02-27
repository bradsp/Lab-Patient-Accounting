using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System;
using System.Windows.Forms;
using LabBilling.Logging;
using System.Linq;

namespace LabBilling.Forms
{
    public partial class ChargeEntryForm : Form
    {
        private AccountSummary _currentAccount = new AccountSummary();
        private readonly CdmRepository cdmRepository = new CdmRepository(Helper.ConnVal);
        private readonly ChrgRepository dbChrg = new ChrgRepository(Helper.ConnVal);

        public ChargeEntryForm(AccountSummary currentAccount)
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

            MTGCComboBoxItem[] items = new MTGCComboBoxItem[chrgItems.Count];
            int i = 0;

            foreach (Cdm cdm in chrgItems)
            {
                items[i] = new MTGCComboBoxItem(cdm.descript, cdm.cdm);
                i++;
            }

            cbChargeItem.ColumnNum = 2;
            cbChargeItem.GridLineHorizontal = true;
            cbChargeItem.GridLineVertical = true;
            cbChargeItem.ColumnWidth = "200;75";
            //cbInsCode.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown;
            cbChargeItem.SelectedIndex = -1;
            cbChargeItem.Items.Clear();
            cbChargeItem.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            cbChargeItem.Items.AddRange(items);
            #endregion

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Chrg newChrg = new Chrg();

            newChrg.account = _currentAccount.account;
            newChrg.service_date = _currentAccount.trans_date;
            newChrg.cdm = cbChargeItem.SelectedValue;
            newChrg.qty = Convert.ToInt32(nQty.Value);
            newChrg.comment = tbComment.Text;

            dbChrg.AddCharge(newChrg);

            DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            DialogResult = DialogResult.Cancel;
            return;
        }
    }
}
