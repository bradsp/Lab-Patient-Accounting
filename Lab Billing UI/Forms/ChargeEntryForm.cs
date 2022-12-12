using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core;
using System;
using System.Windows.Forms;
using LabBilling.Logging;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace LabBilling.Forms
{
    public partial class ChargeEntryForm : Form
    {
        private Account _currentAccount = new Account();
        private readonly CdmRepository cdmRepository = new CdmRepository(Helper.ConnVal);
        private readonly AccountRepository accountRepository = new AccountRepository(Helper.ConnVal);
        //private DataTable cdmSortedByCdm;
        //private DataTable cdmSortedByDesc;
        private Timer _timer;
        private const int _timerInterval = 650;

        public ChargeEntryForm(Account currentAccount)
        {
            Log.Instance.Trace($"Entering");
            _currentAccount = currentAccount;
            InitializeComponent();
            _timer = new Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(cdmTextBox_KeyUpDone);
        }

        private void ChargeEntryForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tbBannerAccount.Text = _currentAccount.AccountNo;
            tbBannerName.Text = _currentAccount.PatFullName;
            tbBannerMRN.Text = _currentAccount.MRN;
            tbDateOfService.Text = _currentAccount.TransactionDate.ToShortDateString();

            //BuildCargeItemCombo();
        }

        //private void BuildCargeItemCombo()
        //{
        //    var chrgItems = cdmRepository.GetAll().ToList();

        //    DataTable cdmDataTable = new DataTable(typeof(Cdm).Name);
        //    cdmDataTable.Columns.Add("cdm");
        //    cdmDataTable.Columns.Add("descript");
        //    var values = new object[2];
        //    //add a null row value
        //    values[0] = "";
        //    values[1] = "<select a charge item>";
        //    cdmDataTable.Rows.Add(values);
        //    foreach (Cdm cdm in chrgItems)
        //    {
        //        values[0] = cdm.ChargeId;
        //        values[1] = cdm.Description;
        //        cdmDataTable.Rows.Add(values);
        //    }
        //    cdmDataTable.DefaultView.Sort = "cdm asc";

        //    cdmSortedByCdm = cdmDataTable.DefaultView.ToTable();

        //    cdmDataTable.DefaultView.Sort = "descript asc";
        //    cdmSortedByDesc = cdmDataTable.DefaultView.ToTable();

        //    cbChargeItem.Items.Clear();
        //    cbChargeItem.DataSource = cdmSortedByCdm;

        //    cbChargeItem.DisplayMember = "cdm";
        //    cbChargeItem.ValueMember = "cdm";
        //}

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            try
            {
                string cdm = "";

                cdm = cdmTextBox.Text;

                if(string.IsNullOrEmpty(cdm))
                {
                    MessageBox.Show("Please select a charge item.", "Incomplete request", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                accountRepository.AddCharge(_currentAccount.AccountNo,
                    cdm,
                    Convert.ToInt32(nQty.Value),
                    _currentAccount.TransactionDate,
                    tbComment.Text,
                    ReferenceNumber.Text);
            }
            catch(CdmNotFoundException)
            {
                // this should not happen
                MessageBox.Show("CDM number is not valid. Charge entry failed.");
            }
            catch(AccountNotFoundException)
            {
                MessageBox.Show("Account number is not valid.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\nCharge not written.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Cancel action in ChargeEntryForm");
            DialogResult = DialogResult.Cancel;
            return;
        }

        private void SearchByCheckChanged (object sender, EventArgs e)
        {
            //if (cbChargeItem != null)
            //{
            //    if (SearchByCdm.Checked)
            //    {
            //        cbChargeItem.DataSource = cdmSortedByCdm;
            //        cbChargeItem.DisplayMember = "cdm";
            //        cbChargeItem.ValueMember = "cdm";
            //    }
            //    else if (SearchByDescription.Checked)
            //    {
            //        cbChargeItem.DataSource = cdmSortedByDesc;
            //        cbChargeItem.DisplayMember = "descript";
            //        cbChargeItem.ValueMember = "cdm";
            //    }
            //}
        }

        private void cdmTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void cdmTextBox_KeyUpDone(object sender, EventArgs e)
        {
            _timer.Stop();
            var cdmLookup = new CdmLookupForm();
            cdmLookup.InitialSearchText = cdmTextBox.Text;
            cdmLookup.Datasource = cdmRepository.GetAll(false);

            if(cdmLookup.ShowDialog() == DialogResult.OK)
            {
                cdmTextBox.Text = cdmLookup.SelectedValue;
            }
        }
    }
}
