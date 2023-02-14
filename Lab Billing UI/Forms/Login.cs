using LabBilling.Core.Models;
using System;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using System.Runtime.InteropServices;
using System.Configuration;
using LabBilling.Logging;
using MetroFramework.Forms;
using MetroFramework.Controls;
using LabBilling.Forms;

namespace LabBilling
{
    public partial class Login : MetroForm
    {
        public Login(bool test = false)
        {
            Log.Instance.Trace($"Entering");
            testEnvironment = test;
            InitializeComponent();
        }

        public bool testEnvironment = false;
        public bool IsLoggedIn { get; set; }
        public Emp LoggedInUser { get; set; }
        private EmpRepository db;
        private string systemUser;
        private string systemDomain;
        private bool skipImpersonateComboSelectionChange = false;

        //declare active directory logon method
        [DllImport("advapi32.dll")]
        public static extern Boolean LogonUser(string name, string domain, string pass, int logType, int logpv, ref IntPtr pht);

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if ((username.Text == string.Empty || password.Text == string.Empty) && !IntegratedAuthentication.Checked)
            {
                statustext.Text = "Please enter a username and password.";
                return;
            }

            DialogResult = DialogResult.OK;

            //bool loginSuccess = false;

/*            IntPtr th = IntPtr.Zero;
            if(IntegratedAuthentication.Checked)
            {
                username.Text = systemUser;
                loginSuccess = true;
            }
            else
            {
                loginSuccess = LogonUser(username.Text, "WTHMC", password.Text, 2, 0, ref th);
            }

            if (loginSuccess)
            {
                GetUserProfile();
                if (!IsLoggedIn)
                {
                    Log.Instance.Info(string.Format("Username {0} is not authorized for billing system access.", systemUser));
                    statustext.Text = string.Format("Username {0} is not authorized for billing system access.", systemUser);
                    return;
                }
                DialogResult = DialogResult.OK;
            }
            else
            {
                // if active directory login failed, revert to local login
                loginSuccess = db.LoginCheck(username.Text, Helper.Encrypt(password.Text.Trim()));
                statustext.Text = "Active Directory login failed. Using local login.";
                Log.Instance.Error(statustext);
                if (loginSuccess)
                {
                    IsLoggedIn = true;
                    LoggedInUser = db.GetByUsername(username.Text);
                    Program.LoggedInUser = LoggedInUser;
                    Program.LoggedInUser.Password = "";
                    Log.Instance.Info(string.Format("Login Success - {0}", LoggedInUser.UserName));
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    IsLoggedIn = false;
                    statustext.Text = "Invalid username or password, or access not granted. Please try again.";
                    Log.Instance.Error(statustext);
                    return;
                }
            } */
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            IsLoggedIn = false;
            this.DialogResult = DialogResult.Cancel;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string[] paramsLogin = domainUser.Split('\\');

            username.Text = systemUser = paramsLogin[1].ToString();
            domain.Text = systemDomain = paramsLogin[0].ToString();

            if (testEnvironment)
            {
                Program.Database = Properties.Settings.Default.TestDbName;
                Program.Server = Properties.Settings.Default.TestDbServer;
                Program.LogDatabase = Properties.Settings.Default.LogDbName;
            }
            else
            {
                Program.Database = Properties.Settings.Default.DbName;
                Program.Server = Properties.Settings.Default.DbServer;
                Program.LogDatabase = Properties.Settings.Default.LogDbName;
            }

            IntegratedAuthentication.Checked = true;
            IntegratedAuthentication_CheckedChanged(sender, e);

            db = new EmpRepository(Helper.ConnVal);

            //check username now to see if it is valid and has impersonate permissions
            GetUserProfile();
            if(IsLoggedIn)
            {
                if(LoggedInUser.CanImpersonate)
                {
                    impersonateUserLabel.Visible = true;
                    impersonateUserComboBox.Visible = true;
                    
                    //load impersonateUserComboBox
                    var emps = db.GetActiveUsers();

                    skipImpersonateComboSelectionChange = true;
                    impersonateUserComboBox.DataSource = emps;
                    impersonateUserComboBox.DisplayMember = nameof(Emp.FullName);
                    impersonateUserComboBox.ValueMember = nameof(Emp.UserName);

                    impersonateUserComboBox.SelectedValue = LoggedInUser.UserName;

                    skipImpersonateComboSelectionChange = false;
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private bool GetUserProfile()
        {

            IsLoggedIn = true;
            LoggedInUser = db.GetByUsername(username.Text);
            Program.LoggedInUser = LoggedInUser;
            Program.LoggedInUser.Password = "";
            if (LoggedInUser == null)
            {
                IsLoggedIn = false;
            }

            return IsLoggedIn;
        }

        private bool LocalLogin()
        {
            // if active directory login failed, revert to local login
            bool loginSuccess = db.LoginCheck(username.Text, Helper.Encrypt(password.Text.Trim()));
            statustext.Text = "Active Directory login failed. Using local login.";
            Log.Instance.Error(statustext);
            if (loginSuccess)
            {
                IsLoggedIn = true;
                LoggedInUser = db.GetByUsername(username.Text);
                Program.LoggedInUser = LoggedInUser;
                Program.LoggedInUser.Password = "";
                Log.Instance.Info(string.Format("Login Success - {0}", LoggedInUser.UserName));
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                IsLoggedIn = false;
                statustext.Text = "Invalid username or password, or access not granted. Please try again.";
                Log.Instance.Error(statustext);
                return false;
            }

            return true;

        }

        private void IntegratedAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if(IntegratedAuthentication.Checked)
            {
                username.Enabled = false;
                password.Enabled = false;
                domain.Enabled = false;
                username.Text = systemUser;
            }
            else
            {
                username.Enabled = true;
                password.Enabled = true;
                domain.Enabled = true;
            }
        }

        private void impersonateUserComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if(!skipImpersonateComboSelectionChange)
            {
                string impersonatedUsername = impersonateUserComboBox.SelectedValue.ToString();
                if(impersonatedUsername != LoggedInUser.UserName)
                {
                    //get impersonated user profile
                    var impersonatedUser = db.GetByUsername(impersonatedUsername);
                    if(impersonatedUser != null)
                    {
                        //copy impersonated user permissions to loggedinuser
                        LoggedInUser.CanEditDictionary = impersonatedUser.CanEditDictionary;
                        LoggedInUser.CanAddAdjustments = impersonatedUser.CanAddAdjustments;
                        LoggedInUser.CanAddPayments = impersonatedUser.CanAddPayments;
                        LoggedInUser.CanModifyAccountFincode = impersonatedUser.CanModifyAccountFincode;
                        LoggedInUser.CanModifyBadDebt = impersonatedUser.CanModifyBadDebt;
                        LoggedInUser.CanSubmitBilling = impersonatedUser.CanSubmitBilling;
                        LoggedInUser.CanSubmitCharges = impersonatedUser.CanSubmitCharges;
                        if(LoggedInUser.IsAdministrator)
                            LoggedInUser.IsAdministrator = impersonatedUser.IsAdministrator;
                        LoggedInUser.Access = impersonatedUser.Access;
                        LoggedInUser.ImpersonatingUser = impersonatedUser.UserName;
                    }
                }
            }
        }

        private void setupImage_Click(object sender, EventArgs e)
        {
            //DatabaseSettingsForm dbForm = new DatabaseSettingsForm();

            //dbForm.ShowDialog();
        }

    }
}
