using LabBilling.Core.Models;
using System;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using System.Runtime.InteropServices;
using System.Configuration;
using LabBilling.Logging;

namespace LabBilling
{
    public partial class Login : Form
    {
        public Login()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
        }

        public bool IsLoggedIn { get; set; }
        public Emp LoggedInUser { get; set; }
        private EmpRepository db;
        private string systemUser;
        private string systemDomain;

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

            Helper.ConnVal = (string)Environment.SelectedItem;

            db = new EmpRepository(Helper.ConnVal);

            bool loginSuccess = false;

            IntPtr th = IntPtr.Zero;
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
                IsLoggedIn = true;
                LoggedInUser = db.GetByUsername(username.Text);
                if(LoggedInUser == null)
                {
                    IsLoggedIn = false;
                    Log.Instance.Info(string.Format("Username {0} is not authorized for billing system access.", systemUser));
                    statustext.Text = string.Format("Username {0} is not authorized for billing system access.", systemUser);
                    return;
                }
                this.DialogResult = DialogResult.OK;
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
            string domainUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string[] paramsLogin = domainUser.Split('\\');

            username.Text = systemUser = paramsLogin[1].ToString();
            domain.Text = systemDomain = paramsLogin[0].ToString();

            //load the available database environments from appconfig
            foreach (ConnectionStringSettings connString in ConfigurationManager.ConnectionStrings)
            {
                Environment.Items.Add(connString.Name);
            }

            Environment.SelectedItem = "MCLTEST";
            IntegratedAuthentication.Checked = true;
            IntegratedAuthentication_CheckedChanged(sender, e);
        }

        private void IntegratedAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if(IntegratedAuthentication.Checked)
            {
                username.Enabled = false;
                password.Enabled = false;
                username.Text = systemUser;
            }
            else
            {
                username.Enabled = true;
                password.Enabled = true;
            }
        }
    }
}
