using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Logging;
using LabBilling.Core.Models;

namespace LabBilling
{
    public partial class UserSecurity : Form
    {
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        readonly EmpRepository empRepository = new EmpRepository(Program.AppEnvironment);
        private bool IsNewRecord = false;
        List<Emp> searchResults = new List<Emp>();

        public UserSecurity()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();

            //empRepository = EmpRepository.Instance;
        }

        private void SetPermissions()
        {
            if(!Program.LoggedInUser.IsAdministrator)
            {
                AddUserButton.Visible = false;
                SaveButton.Visible = false;
                UserName.ReadOnly = true;
                FullName.ReadOnly = true;
                Password.Visible = true;
                ResetPassword.Visible = false;
                AccessLevelCombo.Enabled = false;
                CanEditBadDebt.Enabled = false;
                CanAddAccountAdjustments.Enabled = false;
                CanAddCharges.Enabled = false;
                CanAddPayments.Enabled = false;
                CanChangeAccountFinCode.Enabled = false;
                CanEditBadDebt.Enabled = false;
                CanEditDictionaries.Enabled = false;
                CanSubmitBilling.Enabled = false;
                IsAdministrator.Enabled = false;
                canImpersonateUserCheckBox.Enabled = false;
                Reserved5.Enabled = false;
                Reserved6.Enabled = false;

            }
        }

        private void UserSecurity_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ShowInactive.Checked = false;
            Password.ReadOnly = true;
            RefreshDGV();

        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //empty edit fields
            Clear();
            UserName.ReadOnly = false;
            Password.ReadOnly = false;
        }

        private Emp ReadEditedData()
        {
            Log.Instance.Trace($"Entering");
            Emp editedEmp = new Emp
            {
                UserName = UserName.Text,
                FullName = FullName.Text,
                Access = AccessLevelCombo.Text,
                //editedEmp.Password = Helper.Encrypt(Password.Text.Trim());
                CanAddAdjustments = CanAddAccountAdjustments.Checked,
                CanSubmitCharges = CanAddCharges.Checked,
                CanAddPayments = CanAddPayments.Checked,
                CanModifyAccountFincode = CanChangeAccountFinCode.Checked,
                CanModifyBadDebt = CanEditBadDebt.Checked,
                CanEditDictionary = CanEditDictionaries.Checked,
                CanSubmitBilling = CanSubmitBilling.Checked,
                IsAdministrator = IsAdministrator.Checked,
                CanImpersonate = canImpersonateUserCheckBox.Checked
            };

            return editedEmp;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Emp editedEmp = ReadEditedData();

            if (!IsNewRecord)
            { 
                if(empRepository.Update(editedEmp) == true)
                {
                    MessageBox.Show("Record successfully updated.");
                    Clear();
                    //refresh DGV to pick up updates
                    RefreshDGV();
                }
            }
            else
            {
                //verify this username does not already exist
                bool userExists = false;
                foreach(DataGridViewRow row in UserListDGV.Rows)
                {
                    if(row.Cells[nameof(Emp.UserName)].Value.ToString().ToLower().Equals(editedEmp.UserName.ToLower()))
                    {
                        UserListDGV.FirstDisplayedScrollingRowIndex = row.Index;
                        userExists = true;
                        break;
                    }
                }

                if(userExists)
                {
                    //this user already exists
                    if(MessageBox.Show("This username already exists. Do you want to update this user with the new information?","User Exists",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (empRepository.Update(editedEmp) == true)
                        {
                            MessageBox.Show("Record successfully updated.");
                            Clear();
                            UserName.ReadOnly = true;
                            //refresh DGV to pick up updates
                            RefreshDGV();
                        }
                        else
                        {
                            Log.Instance.Error($"Attempt to update an existing user was not successful. Username {editedEmp}");
                        }
                    }
                    else
                    {
                        Clear();
                        RefreshDGV();
                        UserName.ReadOnly = true;
                    }
                }
                else
                {
                    //encrypt new password for saving
                    editedEmp.Password = Helper.Encrypt(Password.Text.Trim());
                    // add user record
                    if (empRepository.Add(editedEmp) != null)
                    {
                        MessageBox.Show("Record successfully inserted.");
                        Clear();
                        RefreshDGV();
                        UserName.ReadOnly = true;
                    }
                    else
                    {
                        Log.Instance.Error($"Attempt to add a user was not successful. Username {editedEmp}");
                    }
                }
            }
        }

        private void Clear()
        {
            Log.Instance.Trace($"Entering");
            UserName.Text = "";
            FullName.Text = "";
            AccessLevelCombo.SelectedIndex= -1;
            Password.Text = "";
            CanAddAccountAdjustments.Checked = false;
            CanAddCharges.Checked = false;
            CanAddPayments.Checked = false;
            CanChangeAccountFinCode.Checked = false;
            CanEditBadDebt.Checked = false;
            CanEditDictionaries.Checked = false;
            CanSubmitBilling.Checked = false;
            IsAdministrator.Checked = false;
            canImpersonateUserCheckBox.Checked = false;
            ModDateTime.Text = "";
            ModUser.Text = "";
            ModProgram.Text = "";
            IsNewRecord = true;
        }

        private void RefreshDGV()
        {
            Log.Instance.Trace($"Entering");
            searchResults = empRepository.GetAll().ToList();

            DataTable dt = Helper.ConvertToDataTable(searchResults);

            UserListDGV.DataSource = dt;
            UserListDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //filter out inactives
            if(ShowInactive.Checked == false)
            {
                dt.DefaultView.RowFilter = "Access <> 'NONE'";
            }
        }

        private void UserListDGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            try
            {
                IsNewRecord = false;
                UserName.ReadOnly = true;
                UserName.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.UserName)].Value.ToString();
                FullName.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.FullName)].Value.ToString();
                AccessLevelCombo.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.Access)].Value.ToString();
                //Password.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.Password)].Value == null ? "" : UserListDGV.SelectedRows[0].Cells[nameof(Emp.Password)].Value.ToString();
                CanAddAccountAdjustments.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanAddAdjustments)].Value);
                CanAddCharges.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanSubmitCharges)].Value);
                CanAddPayments.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanAddAdjustments)].Value);
                CanChangeAccountFinCode.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanModifyAccountFincode)].Value);
                CanEditBadDebt.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanModifyBadDebt)].Value);
                CanEditDictionaries.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanEditDictionary)].Value);
                CanSubmitBilling.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanSubmitBilling)].Value);
                IsAdministrator.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.IsAdministrator)].Value);
                canImpersonateUserCheckBox.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(Emp.CanImpersonate)].Value);

                ModDateTime.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.LastModifiedDate)].Value.ToString();
                ModUser.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.LastModifiedBy)].Value?.ToString();
                ModProgram.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.LastModifiedWith)].Value?.ToString();

            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, "Exception in UserSecurity.UserListDGV_RowHeaderMouseClick");
                MessageBox.Show("Exception error has been logged loading the UserSecurity dialog. Please notify your administrator.");
                this.Close();
            }

        }

        private void ShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            RefreshDGV();
        }

        private void ResetPassword_Click(object sender, EventArgs e)
        {
            // ask for new password
            InputBoxResult prompt = InputBox.ShowPassword("Enter new password:", "New Password");

            if(prompt.ReturnCode == DialogResult.OK)
            {
                Emp emp = ReadEditedData();

                emp.Password = Helper.Encrypt(prompt.Text.Trim());

                if (empRepository.Update(emp) == true)
                {
                    MessageBox.Show("Password updated.");
                    Clear();
                    //refresh DGV to pick up updates
                    RefreshDGV();
                }
            }
        }
    }
}
