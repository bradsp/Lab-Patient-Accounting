using LabBilling.Core.Models;
using System;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using System.Runtime.InteropServices;
using LabBilling.Logging;
using MetroFramework.Forms;

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

            Program.AppEnvironment.UserName = username.Text;
            Program.AppEnvironment.Password = password.Text;

            if (!IntegratedAuthentication.Checked)
            {
                if (!ServerLogin())
                    return;
            }

            DialogResult = DialogResult.OK;
        }

        public bool ServerLogin()
        {
            db = new EmpRepository(Program.AppEnvironment);

            bool loginSuccess = db.LoginCheck(username.Text, Helper.Encrypt(password.Text.Trim()));

            if (loginSuccess)
            {
                IsLoggedIn = true;
                LoggedInUser = db.GetByUsername(username.Text);
                Program.LoggedInUser = LoggedInUser;
                Program.LoggedInUser.Password = "";
                Log.Instance.Info(string.Format("Login Success - {0}", LoggedInUser.UserName));
                return true;
            }
            else
            {
                IsLoggedIn = false;
                statustext.Text = "Invalid username or password, or access not granted. Please try again.";
                Log.Instance.Error(statustext);
                return false;
            }
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

            domain.Visible = false;
            label3.Visible = false;

            if (testEnvironment)
            {
                Program.AppEnvironment.DatabaseName = Properties.Settings.Default.TestDbName;
                Program.AppEnvironment.ServerName = Properties.Settings.Default.TestDbServer;
                Program.AppEnvironment.LogDatabaseName = Properties.Settings.Default.LogDbName;
            }
            else
            {
                Program.AppEnvironment.DatabaseName = Properties.Settings.Default.DbName;
                Program.AppEnvironment.ServerName = Properties.Settings.Default.DbServer;
                Program.AppEnvironment.LogDatabaseName = Properties.Settings.Default.LogDbName;
            }

            Program.AppEnvironment.IntegratedAuthentication = Properties.Settings.Default.IntegratedSecurity;

            if (Properties.Settings.Default.IntegratedSecurity)
            {
                db = new EmpRepository(Program.AppEnvironment);

                string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string[] paramsLogin = domainUser.Split('\\');

                username.Text = systemUser = paramsLogin[1].ToString();
                domain.Text = systemDomain = paramsLogin[0].ToString();

                IntegratedAuthentication.Checked = true;
                IntegratedAuthentication_CheckedChanged(sender, e);

                //check username now to see if it is valid and has impersonate permissions
                GetUserProfile();

                if (IsLoggedIn)
                {
                    if (LoggedInUser.CanImpersonate)
                    {
                        impersonateUserLabel.Visible = true;
                        impersonateUserComboBox.Visible = true; 
                        db = new EmpRepository(Program.AppEnvironment);

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
            else
            {
                IntegratedAuthentication.Checked = false;
                IntegratedAuthentication_CheckedChanged(sender, e);

                IntegratedAuthentication.Enabled = false;
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
