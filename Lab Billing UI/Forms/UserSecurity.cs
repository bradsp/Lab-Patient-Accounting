using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Logging;
using LabBilling.Core.Models;
using LabBilling.Forms;
using LabBilling.Core.Services;

namespace LabBilling
{
    public partial class UserSecurity : BaseForm
    {
        private bool IsNewRecord = false;
        List<UserAccount> searchResults = new();

        private readonly SystemService systemService = new(Program.AppEnvironment);

        public UserSecurity()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
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

        private UserAccount ReadEditedData()
        {
            Log.Instance.Trace($"Entering");
            UserAccount editedEmp = new()
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
            UserAccount editedEmp = ReadEditedData();

            if (!IsNewRecord)
            {
                try
                {
                    editedEmp = systemService.UpdateUser(editedEmp);
                    MessageBox.Show("Record successfully updated.");
                    Clear();
                    //refresh DGV to pick up updates
                    RefreshDGV();
                }
                catch(ApplicationException apex) 
                { 
                    MessageBox.Show(apex.Message);
                    Log.Instance.Error(apex.Message, apex);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Instance.Error(ex.Message, ex);
                }
            }
            else
            {
                //verify this username does not already exist
                bool userExists = false;
                foreach(DataGridViewRow row in UserListDGV.Rows)
                {
                    if(row.Cells[nameof(UserAccount.UserName)].Value.ToString().ToLower().Equals(editedEmp.UserName.ToLower()))
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
                        try
                        {
                            editedEmp = systemService.UpdateUser(editedEmp);
                            MessageBox.Show("Record successfully updated.");
                            Clear();
                            UserName.ReadOnly = true;
                            //refresh DGV to pick up updates
                            RefreshDGV();
                        }
                        catch (ApplicationException apex)
                        {
                            MessageBox.Show(apex.Message);
                            Log.Instance.Error($"Attempt to update an existing user was not successful. Username {editedEmp}", apex);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            Log.Instance.Error($"Attempt to update an existing user was not successful. Username {editedEmp}", ex);
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
                    try
                    {
                        //encrypt new password for saving
                        editedEmp.Password = Helper.Encrypt(Password.Text.Trim());
                        // add user record
                        editedEmp = systemService.AddUser(editedEmp);
                        MessageBox.Show("Record successfully inserted.");
                        Clear();
                        RefreshDGV();
                        UserName.ReadOnly = true;
                    }
                    catch (ApplicationException apex)
                    {
                        MessageBox.Show(apex.Message);
                        Log.Instance.Error($"Attempt to update an existing user was not successful. Username {editedEmp}", apex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Log.Instance.Error($"Attempt to update an existing user was not successful. Username {editedEmp}", ex);
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
            searchResults = systemService.GetUsers().ToList();

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
                UserName.Text = UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.UserName)].Value.ToString();
                FullName.Text = UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.FullName)].Value.ToString();
                AccessLevelCombo.Text = UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.Access)].Value.ToString();
                //Password.Text = UserListDGV.SelectedRows[0].Cells[nameof(Emp.Password)].Value == null ? "" : UserListDGV.SelectedRows[0].Cells[nameof(Emp.Password)].Value.ToString();
                CanAddAccountAdjustments.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanAddAdjustments)].Value);
                CanAddCharges.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanSubmitCharges)].Value);
                CanAddPayments.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanAddAdjustments)].Value);
                CanChangeAccountFinCode.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanModifyAccountFincode)].Value);
                CanEditBadDebt.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanModifyBadDebt)].Value);
                CanEditDictionaries.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanEditDictionary)].Value);
                CanSubmitBilling.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanSubmitBilling)].Value);
                IsAdministrator.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.IsAdministrator)].Value);
                canImpersonateUserCheckBox.Checked = Convert.ToBoolean(UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.CanImpersonate)].Value);

                ModDateTime.Text = UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.LastModifiedDate)].Value.ToString();
                ModUser.Text = UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.LastModifiedBy)].Value?.ToString();
                ModProgram.Text = UserListDGV.SelectedRows[0].Cells[nameof(UserAccount.LastModifiedWith)].Value?.ToString();

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
                UserAccount user = ReadEditedData();

                user.Password = Helper.Encrypt(prompt.Text.Trim());

                try
                {
                    user = systemService.UpdateUser(user);
                    MessageBox.Show("Password updated.");
                    Clear();
                    //refresh DGV to pick up updates
                    RefreshDGV();
                }
                catch (ApplicationException apex)
                {
                    MessageBox.Show(apex.Message);
                    Log.Instance.Error($"Attempt to reset password was not successful. Username {user}", apex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Log.Instance.Error($"Attempt to reset password was not successful. Username {user}", ex);
                }

            }
        }
    }
}
