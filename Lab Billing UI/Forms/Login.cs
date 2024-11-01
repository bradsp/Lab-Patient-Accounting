﻿using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using System.Runtime.InteropServices;

namespace LabBilling;

public partial class Login : Form
{
    private SystemService _systemService;

    public Login(bool test = false)
    {
        Log.Instance.Trace($"Entering");
        testEnvironment = test;
        InitializeComponent();
    }

    public bool testEnvironment = false;
    public bool IsLoggedIn { get; set; }
    public UserAccount LoggedInUser { get; set; }
    private string _systemUser;
    private string _systemDomain;
    private bool _skipImpersonateComboSelectionChange = false;

    //declare active directory logon method
    [DllImport("advapi32.dll")]
    public static extern bool LogonUser(string name, string domain, string pass, int logType, int logpv, ref IntPtr pht);

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

        bool loginSuccess = _systemService.LoginCheck(username.Text, Helper.Encrypt(password.Text.Trim()));

        if (loginSuccess)
        {
            IsLoggedIn = true;
            LoggedInUser = _systemService.GetUser(username.Text);
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
            Program.AppEnvironment.LogDatabaseName = Properties.Settings.Default.TestLogDbName;
            Program.AppEnvironment.IntegratedAuthentication = Properties.Settings.Default.TestIntegratedSecurity;
        }
        else
        {
            Program.AppEnvironment.DatabaseName = Properties.Settings.Default.ProdDbName;
            Program.AppEnvironment.ServerName = Properties.Settings.Default.ProdDbServer;
            Program.AppEnvironment.LogDatabaseName = Properties.Settings.Default.ProdLogDbName;
            Program.AppEnvironment.IntegratedAuthentication = Properties.Settings.Default.ProdIntegratedSecurity;
        }

        _systemService = new(Program.AppEnvironment);
        if (Program.AppEnvironment.IntegratedAuthentication)
        {

            string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string[] paramsLogin = domainUser.Split('\\');

            username.Text = _systemUser = paramsLogin[1].ToString();
            domain.Text = _systemDomain = paramsLogin[0].ToString();

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

                    //load impersonateUserComboBox
                    var emps = _systemService.GetActiveUsers();

                    _skipImpersonateComboSelectionChange = true;
                    impersonateUserComboBox.DataSource = emps;
                    impersonateUserComboBox.DisplayMember = nameof(UserAccount.FullName);
                    impersonateUserComboBox.ValueMember = nameof(UserAccount.UserName);

                    impersonateUserComboBox.SelectedValue = LoggedInUser.UserName;

                    _skipImpersonateComboSelectionChange = false;
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
        LoggedInUser = _systemService.GetUser(username.Text);
        Program.LoggedInUser = LoggedInUser;
        Program.LoggedInUser.Password = "";
        Program.AppEnvironment.User = LoggedInUser.UserName;
        Program.AppEnvironment.UserAccount = LoggedInUser;
        if (LoggedInUser == null)
        {
            IsLoggedIn = false;
        }

        return IsLoggedIn;
    }

    private void IntegratedAuthentication_CheckedChanged(object sender, EventArgs e)
    {
        if (IntegratedAuthentication.Checked)
        {
            username.Enabled = false;
            password.Enabled = false;
            domain.Enabled = false;
            username.Text = _systemUser;
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
        if (!_skipImpersonateComboSelectionChange)
        {
            string impersonatedUsername = impersonateUserComboBox.SelectedValue.ToString();
            if (impersonatedUsername != LoggedInUser.UserName)
            {
                //get impersonated user profile
                var impersonatedUser = _systemService.GetUser(impersonatedUsername);
                if (impersonatedUser != null)
                {
                    //copy impersonated user permissions to loggedinuser
                    LoggedInUser.CanEditDictionary = impersonatedUser.CanEditDictionary;
                    LoggedInUser.CanAddAdjustments = impersonatedUser.CanAddAdjustments;
                    LoggedInUser.CanAddPayments = impersonatedUser.CanAddPayments;
                    LoggedInUser.CanModifyAccountFincode = impersonatedUser.CanModifyAccountFincode;
                    LoggedInUser.CanModifyBadDebt = impersonatedUser.CanModifyBadDebt;
                    LoggedInUser.CanSubmitBilling = impersonatedUser.CanSubmitBilling;
                    LoggedInUser.CanSubmitCharges = impersonatedUser.CanSubmitCharges;
                    if (LoggedInUser.IsAdministrator)
                        LoggedInUser.IsAdministrator = impersonatedUser.IsAdministrator;
                    LoggedInUser.Access = impersonatedUser.Access;
                    LoggedInUser.ImpersonatingUser = impersonatedUser.UserName;
                }
            }
        }
    }

    private void setupImage_Click(object sender, EventArgs e)
    {
    }

}
