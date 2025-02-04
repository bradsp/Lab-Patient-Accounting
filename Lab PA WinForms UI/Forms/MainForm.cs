using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Forms;
using LabBilling.Legacy;
using LabBilling.Logging;
using LabBilling.Properties;
using LabBilling.ReportByInsuranceCompany;
using MenuBar;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using Utilities;
using Application = System.Windows.Forms.Application;
using Button = System.Windows.Forms.Button;
using Image = System.Drawing.Image;
using Label = System.Windows.Forms.Label;
using ProgressBar = System.Windows.Forms.ProgressBar;


/*
 * Tabbed MDI logic 
 * https://www.codeproject.com/Articles/17640/Tabbed-MDI-Child-Forms
 */


namespace LabBilling;

public partial class MainForm : Form
{
    private TableLayoutPanel _menuTable;
    private readonly ProgressBar _claimProgress;
    private readonly Label _claimProgressStatusLabel;
    private readonly CancellationTokenSource _cancellationToken;
    private List<UserProfile> _recentAccounts;
    private List<Account> _recentAccountsByAccount;
    private int _recentAccountsStartRow;

    private readonly AccountService _accountService;
    private readonly SystemService _systemService;

    public ProgressReportModel progressReportModel = new()
    {
        RecordsProcessed = -1
    };
    private readonly Bitmap _closeImage;

    public MainForm()
    {
        InitializeComponent();

        ConfigureLogging();

        _accountService = new(Program.AppEnvironment);
        _systemService = new(Program.AppEnvironment);

        MainFormMenu.BackColor = Program.AppEnvironment.MenuBackgroundColor;
        MainFormMenu.ForeColor = Program.AppEnvironment.MenuTextColor;

        panel1.BackColor = Program.AppEnvironment.WindowBackgroundColor;

        Program.AppEnvironment.ApplicationParameters = _systemService.LoadSystemParameters();
        this.Text += $" - {Program.AppEnvironment.DatabaseName}";
        ImageList il = new();
        il.Images.Add((Image)Resources.hiclipart_com_id_dbhyp);
        mdiTabControl.ImageList = il;
    }

    private static void ConfigureLogging()
    {
        #region Configure NLog
        LogLevel minLevel = NLog.LogLevel.Warn;

        var configuration = new NLog.Config.LoggingConfiguration();
        switch (Program.AppEnvironment.ApplicationParameters.LogLevel)
        {
            case "Trace":
                minLevel = LogLevel.Trace;
                break;
            case "Debug":
                minLevel = LogLevel.Debug;
                break;
            case "Info":
                minLevel = LogLevel.Info;
                break;
            case "Warn":
                minLevel = LogLevel.Warn;
                break;
            case "Error":
                minLevel = LogLevel.Error;
                break;
            case "Fatal":
                minLevel = LogLevel.Fatal;
                break;
            default:
                break;
        }

        GlobalDiagnosticsContext.Set("dbname", Program.AppEnvironment.DatabaseName);
        GlobalDiagnosticsContext.Set("dbserver", Program.AppEnvironment.ServerName);
        FileTarget fileTarget;
        if (string.IsNullOrEmpty(Program.AppEnvironment.ApplicationParameters.LogFilePath))
        {
            throw new ArgumentNullException(nameof(ApplicationParameters.LogFilePath));
        }
        fileTarget = new FileTarget("logfile")
        {
            FileName = $"{Program.AppEnvironment.ApplicationParameters.LogFilePath}\\{OS.GetMachineName()}_{OS.GetUserName()}_{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}.log",
            Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}|${exception}|${stacktrace}|${hostname}|${environment-user}|${callsite}|${callsite-linenumber}|${assembly-version}|${gdc:item=dbname|${gdc:item=dbserver}"
        };
        //var consoleTarget = new NLog.Targets.ConsoleTarget("logconsole");
        string logProcedure = Program.AppEnvironment.ApplicationParameters.DatabaseEnvironment != "Production" ? "NLog_AddEntry_t" : "NLog_AddEntry_p";
        var dbTarget = new DatabaseTarget("database")
        {
            ConnectionString = Program.AppEnvironment.LogConnectionString,
            CommandType = System.Data.CommandType.StoredProcedure,
            CommandText = $"[dbo].[{logProcedure}]"
        };

        dbTarget.Parameters.Add(new DatabaseParameterInfo("@createdon", new NLog.Layouts.SimpleLayout("${date}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", new NLog.Layouts.SimpleLayout("${message}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@stacktrace", new NLog.Layouts.SimpleLayout("${stacktrace}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@hostname", new NLog.Layouts.SimpleLayout("${hostname}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@username", new NLog.Layouts.SimpleLayout("${environment-user}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@callingsite", new NLog.Layouts.SimpleLayout("${callsite}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@callingsitelinenumber", new NLog.Layouts.SimpleLayout("${callsite-linenumber}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@appversion", new NLog.Layouts.SimpleLayout("${assembly-version}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@databasename", new NLog.Layouts.SimpleLayout("${gdc:item=dbname}")));
        dbTarget.Parameters.Add(new DatabaseParameterInfo("@databaseserver", new NLog.Layouts.SimpleLayout("${gdc:item=dbserver}")));

        switch (Program.AppEnvironment.ApplicationParameters.LogLocation)
        {
            case "Database":
                configuration.AddRule(new LoggingRule("*", minLevel, dbTarget));
                break;
            case "FilePath":
                if (fileTarget == null)
                    break;
                configuration.AddRule(new LoggingRule("*", minLevel, fileTarget));
                break;

        }

        configuration.AddRule(new LoggingRule("*", LogLevel.Debug, fileTarget));

        LogManager.Configuration = configuration;

        Log.Instance.Warn($"Log configuration -  MinimumLevel: {minLevel}, LogLocation: {Program.AppEnvironment.ApplicationParameters.LogLocation}");

        #endregion

    }

    private void NewForm(Form childForm)
    {
        if (MdiChildren.Length >= Program.AppEnvironment.ApplicationParameters.TabsOpenLimit)
        {
            AskCloseTabForm askCloseTab = new(MdiChildren.Select(x => x.Text).ToList());
            if (askCloseTab.ShowDialog() == DialogResult.OK)
            {
                var result = askCloseTab.SelectedForm;
                try
                {
                    MdiChildren.Where(x => x.Text == result).First().Close();
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex, "Error closing tab.");
                    MessageBox.Show("Error closing tab. Contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        childForm.MdiParent = this;
        childForm.TextChanged += ChildForm_TextChanged;
        childForm.FormClosed += ChildForm_FormClosed;
        childForm.Show();
    }

    private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (sender is Form frm)
        {
            int i = mdiTabControl.TabPages.IndexOfKey(frm.Text);

            if (i >= 0)
            {
                mdiTabControl.TabPages.Remove(mdiTabControl.TabPages[i]);
            }

            if (mdiTabControl.TabPages.ContainsKey("Work List"))
            {
                int idx = mdiTabControl.TabPages.IndexOfKey("Work List");
                mdiTabControl.SelectedIndex = idx;
            }
        }
    }


    private void ChildForm_TextChanged(object sender, EventArgs e)
    {
        Form form = (Form)sender;
        if (form != null)
        {
            if (form.Tag != null)
            {
                TabPage tp = (TabPage)form.Tag;
                tp.Text = form.Text;
            }
        }
    }

    private void userSecurityToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        if (Program.LoggedInUser.IsAdministrator)
        {
            UserSecurity frm = new()
            {
                MdiParent = this,
                WindowState = FormWindowState.Normal,
                AutoScroll = true
            };
            frm.Show();
        }
        else
        {
            MessageBox.Show("You do not have permission to access this feature.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }

    private void accountToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        PersonSearchForm frm = new();
        if (frm.ShowDialog() == DialogResult.OK)
        {

            var formsList = Application.OpenForms.OfType<AccountForm>();
            bool formFound = false;
            foreach (var form in formsList)
            {
                if (form.SelectedAccount == frm.SelectedAccount)
                {
                    //form is already open, activate this one
                    form.Focus();
                    formFound = true;
                    break;
                }
            }

            if (!formFound)
            {
                AccountForm accFrm = new AccountForm(frm.SelectedAccount);
                accFrm.AccountOpenedEvent += AccFrm_AccountOpenedEvent;
                accFrm.AccountUpdatedEvent += AccFrm_AccountUpdatedEvent;
                NewForm(accFrm);
            }
        }
    }

    private void AccFrm_AccountUpdatedEvent(object sender, Account e)
    {
        //if there is a worklist form opened - call method to update the account in the worklist
        if (Application.OpenForms.OfType<WorkListForm>().Any())
        {
            WorkListForm workListForm = Application.OpenForms.OfType<WorkListForm>().First();
            workListForm.UpdateAccount(e);
        }
    }

    private void AccFrm_AccountOpenedEvent(object sender, string e)
        => UpdateRecentAccounts(e);


    private void LoadRecentAccounts()
    {
        //remove any existing recent accounts
        recentAccountsToolStripMenuItem.DropDownItems.Clear();

        // Add recent accounts to toolstripmenu
        foreach (var acc in _recentAccountsByAccount)
        {
            if (acc != null)
            {
                ToolStripMenuItem accountLink = new()
                {
                    Name = $"recentAccountToolStripMenuItem_{acc.AccountNo}",
                    Text = acc.PatFullName,
                    Tag = acc.AccountNo,
                };
                accountLink.Click += RecentLabelClicked;
                recentAccountsToolStripMenuItem.DropDownItems.Add(accountLink);
            }

        }
    }

    public void UpdateRecentAccounts(string newAccount)
    {
        var ar = _accountService.GetAccountMinimal(newAccount);
        if (ar != null)
        {
            // Update the recent accounts list
            _recentAccountsByAccount.Insert(0, ar);

            // Limit the number of recent accounts to 5
            if (_recentAccountsByAccount.Count > 5)
            {
                _recentAccountsByAccount.RemoveAt(5);
            }

            LoadRecentAccounts();
        }

    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        Log.Instance.Info($"Launching MainForm");

        #region user authentication

        if (Program.LoggedInUser == null)
        {
            Log.Instance.Fatal("There is not a valid user object.");
            MessageBox.Show(this, "Application error with user object. Aborting.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Application.Exit();
        }
        #endregion

        NewForm(new DashboardForm());

        var results = await _systemService.GetRecentAccountsAsync(Program.LoggedInUser.UserName);
        _recentAccounts = results.ToList();
        _recentAccountsByAccount = new();

        foreach (UserProfile up in _recentAccounts)
        {
            _recentAccountsByAccount.Add(_accountService.GetAccountMinimal(up.ParameterData));
        }

        mdiTabControl.TabClosing += mdiTabControl_TabClosing;

        //enable menu items based on permissions
        systemAdministrationToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;
        LoadSideMenu();
        LoadRecentAccounts();
        toolStripDatabaseLabel.Text = Program.AppEnvironment.DatabaseName;
        toolStripUsernameLabel.Text = Program.LoggedInUser.UserName;
        this.WindowState = FormWindowState.Maximized;
        this.Focus();
    }

    private void mdiTabControl_TabClosing(object sender, TabControlCancelEventArgs e)
    {
        Form frm = e.TabPage.Tag as Form;
        frm.Close();

        if (mdiTabControl.TabPages.ContainsKey("Work List"))
        {
            int idx = mdiTabControl.TabPages.IndexOfKey("Work List");
            mdiTabControl.SelectedIndex = idx;
        }
    }

    private void LoadSideMenu()
    {
        // Update menu access
        UpdateMenuAccess();

        // Clear panel1
        panel1.Controls.Clear();

        // Create a TableLayoutPanel to hold the menu
        _menuTable = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            AutoScroll = true,
            AutoSize = false, // Set to false to prevent expansion
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            Margin = new Padding(0),
            Padding = new Padding(0),
            Location = new Point(0, 0)
        };

        panel1.Controls.Add(_menuTable);

        int currentRow = 0;
        // Define the button height
        int buttonHeight = 50;
        int labelHeight = 30;
        // Clear any existing RowStyles
        _menuTable.RowStyles.Clear();
        _menuTable.ColumnStyles.Clear();

        // Add ColumnStyle
        _menuTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // ************************  Add Account Management Section ***************************
        var accountLabel = CreateMenuLabel("Account Management");
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, labelHeight)); // Set label height
        _menuTable.Controls.Add(accountLabel, 0, currentRow++);


        var btnWorkList = CreateMenuButton("Worklist", "WorklistButton", worklistToolStripMenuItem_Click);
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
        _menuTable.Controls.Add(btnWorkList, 0, currentRow++);

        var btnAccount = CreateMenuButton("Account", "AccountButton", accountToolStripMenuItem_Click);
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
        _menuTable.Controls.Add(btnAccount, 0, currentRow++);

        if (Program.AppEnvironment.ApplicationParameters.AllowChargeEntry && Program.LoggedInUser.CanSubmitCharges)
        {
            var btnAccountChargeEntry = CreateMenuButton("Account Charge Entry", "AccountChargeEntryButton", accountChargeEntryToolStripMenuItem_Click);
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
            _menuTable.Controls.Add(btnAccountChargeEntry, 0, currentRow++);
        }

        // Add spacing between sections
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
        currentRow++;

        // ******************** Add Billing section ********************
        var billingLabel = CreateMenuLabel("Billing");
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, labelHeight)); // Set label height
        _menuTable.Controls.Add(billingLabel, 0, currentRow++);


        if (Program.LoggedInUser.CanSubmitBilling || Program.LoggedInUser.IsAdministrator)
        {
            var btnClaimBatchManagement = CreateMenuButton("Claims Batch Management", "ClaimBatchManagementButton", claimBatchManagementToolStripMenuItem_Click);
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
            _menuTable.Controls.Add(btnClaimBatchManagement, 0, currentRow++);
        }

        if (Program.AppEnvironment.ApplicationParameters.ClientBillFilter.Split('|').Contains(Program.LoggedInUser.UserName) || Program.LoggedInUser.IsAdministrator)
        {
            var btnClientBills = CreateMenuButton("Client Bills", "ClientBillsButton", clientBillsNewToolStripMenuItem_Click);
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
            _menuTable.Controls.Add(btnClientBills, 0, currentRow++);
        }

        if (Program.LoggedInUser.CanModifyBadDebt || Program.LoggedInUser.IsAdministrator)
        {
            var btnBadDebtMaintenance = CreateMenuButton("Patient Collections", "PatientCollectionsButton", badDebtMaintenanceToolStripMenuItem_Click);
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
            _menuTable.Controls.Add(btnBadDebtMaintenance, 0, currentRow++);
        }

        // Add spacing between sections
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
        currentRow++;

        if ((Program.LoggedInUser.CanAddPayments && Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry) || Program.LoggedInUser.IsAdministrator)
        {
            // ******************** Add Payment Posting section ********************
            var paymentPostingLabel = CreateMenuLabel("Payment/Adjustment Posting");
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, labelHeight)); // Set label height
            _menuTable.Controls.Add(paymentPostingLabel, 0, currentRow++);


            var btnBatchRemittance = CreateMenuButton("Batch Remittance", "BathRemittanceButton", batchRemittanceToolStripMenuItem_Click);
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
            _menuTable.Controls.Add(btnBatchRemittance, 0, currentRow++);

            var btnRemittancePosting = CreateMenuButton("Remittance Posting", "RemittancePostingButton", remittancePostingToolStripMenuItem_Click);
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
            _menuTable.Controls.Add(btnRemittancePosting, 0, currentRow++);

            // Add spacing between sections
            _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
            currentRow++;
        }

        // ********************** Add Reports section **********************
        var reportsLabel = CreateMenuLabel("Reports");
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, labelHeight)); // Set label height
        _menuTable.Controls.Add(reportsLabel, 0, currentRow++);

        // Add Reports buttons
        var btnMonthlyReports = CreateMenuButton("Monthly Reports", "MonthlyReportsButton", monthlyReportsToolStripMenuItem_Click);
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
        _menuTable.Controls.Add(btnMonthlyReports, 0, currentRow++);

        var btnReportingPortal = CreateMenuButton("Reporting Portal", "ReportingPortalButton", reportingPortalToolStripMenuItem_Click);
        _menuTable.RowStyles.Add(new RowStyle(SizeType.Absolute, buttonHeight)); // Set row height
        _menuTable.Controls.Add(btnReportingPortal, 0, currentRow++);

        _menuTable.RowCount = currentRow;

        // Optional: Set the TableLayoutPanel to not grow beyond its content size
        _menuTable.MaximumSize = new Size(panel1.Width, _menuTable.PreferredSize.Height);
    }

    private static Button CreateMenuButton(string text, string name, EventHandler eventHandler)
    {
        var button = new MenuButton
        {
            Text = text,
            Name = name,
            BackColor = Program.AppEnvironment.ButtonBackgroundColor,
            ForeColor = Program.AppEnvironment.ButtonTextColor,
            FlatStyle = FlatStyle.Flat,
            Dock = DockStyle.Fill,
            Margin = new Padding(5, 0, 5, 0),
            TextAlign = ContentAlignment.MiddleCenter,
            BorderRadius = 10,
            BorderSize = 0,
            BorderColor = Color.Transparent,
            AutoSize = false
        };
        button.Click += eventHandler;
        return button;
    }

    private Label CreateMenuLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = false,
            Font = new Font("Arial", 12, FontStyle.Bold),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(5),
            Margin = new Padding(0),
            AutoEllipsis = false,
            MaximumSize = new Size(panel1.Width - 10, 0), // Adjust width as needed
        };
    }

    private void UpdateMenuAccess()
    {
        bool viewOnly = Program.LoggedInUser.Access == "VIEW";
        // Update visibility of menu items based on permissions

        // Debugging output to verify conditions
        Debug.WriteLine($"AllowPaymentAdjustmentEntry: {Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry}");
        Debug.WriteLine($"CanAddPayments: {Program.LoggedInUser.CanAddPayments}");
        Debug.WriteLine($"viewOnly: {viewOnly}");

        batchRemittanceToolStripMenuItem.Visible = (Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry && Program.LoggedInUser.CanAddPayments) && !viewOnly;
        remittancePostingToolStripMenuItem.Visible = (Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry && Program.LoggedInUser.CanAddPayments) && !viewOnly;

        accountChargeEntryToolStripMenuItem.Visible = (Program.AppEnvironment.ApplicationParameters.AllowChargeEntry && Program.LoggedInUser.CanSubmitCharges) && !viewOnly;

        //during testing only - remove once batch charge entry is in production
        batchChargeEntryToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;
        badDebtMaintenanceToolStripMenuItem.Visible = Program.LoggedInUser.CanModifyBadDebt && !viewOnly;

        //administrator only menu items
        systemAdministrationToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;

        duplicateAccountsToolStripMenuItem.Visible = !viewOnly;
        clientBillsNewToolStripMenuItem.Visible = !viewOnly;

        // Debugging output to verify visibility settings
        Debug.WriteLine($"batchRemittanceToolStripMenuItem.Visible: {batchRemittanceToolStripMenuItem.Visible}");
        Debug.WriteLine($"remittancePostingToolStripMenuItem.Visible: {remittancePostingToolStripMenuItem.Visible}");
        Debug.WriteLine($"accountChargeEntryToolStripMenuItem.Visible: {accountChargeEntryToolStripMenuItem.Visible}");
        Debug.WriteLine($"batchChargeEntryToolStripMenuItem.Visible: {batchChargeEntryToolStripMenuItem.Visible}");
        Debug.WriteLine($"badDebtMaintenanceToolStripMenuItem.Visible: {badDebtMaintenanceToolStripMenuItem.Visible}");
        Debug.WriteLine($"systemAdministrationToolStripMenuItem.Visible: {systemAdministrationToolStripMenuItem.Visible}");
        Debug.WriteLine($"duplicateAccountsToolStripMenuItem.Visible: {duplicateAccountsToolStripMenuItem.Visible}");
        Debug.WriteLine($"clientBillsNewToolStripMenuItem.Visible: {clientBillsNewToolStripMenuItem.Visible}");

    }

    private void RecentLabelClicked(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        var linkLabel = (ToolStripMenuItem)sender;

        LaunchAccount(linkLabel.Tag.ToString());
    }

    private void LaunchAccount(string account)
    {
        Log.Instance.Trace("Entering");
        SuspendLayout();
        bool IsAlreadyOpen = false;
        if (Application.OpenForms.OfType<AccountForm>().Any())
        {
            foreach (AccountForm frm in Application.OpenForms.OfType<AccountForm>())
            {
                if (frm.SelectedAccount == account)
                {
                    IsAlreadyOpen = true;
                    frm.Focus();
                }
            }
        }

        if (!IsAlreadyOpen)
        {
            Cursor.Current = Cursors.WaitCursor;
            AccountForm accFrm = new(account);
            accFrm.AccountOpenedEvent += AccFrm_AccountOpenedEvent;
            accFrm.AccountUpdatedEvent += AccFrm_AccountUpdatedEvent;
            accFrm.FormClosed += (s, args) =>
            {
                accFrm.AccountOpenedEvent -= AccFrm_AccountOpenedEvent;
                accFrm.AccountUpdatedEvent -= AccFrm_AccountUpdatedEvent;
            };
            NewForm(accFrm);
            Cursor.Current = Cursors.Default;
        }
        ResumeLayout();
    }

    private void systemParametersToolStripMenuItem_Click(object sender, EventArgs e)
        => new SystemParametersForm().ShowDialog();

    private void monthlyReportsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        AuditReportsForm frm = new(Program.AppEnvironment.GetArgs());
        frm.AccountLaunched += OnAccountLaunched;
        NewForm(frm);
    }

    private void worklistToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        if (Application.OpenForms.OfType<WorkListForm>().Any())
        {
            WorkListForm workListForm = Application.OpenForms.OfType<WorkListForm>().First();
            workListForm.Focus();
        }
        else
        {
            WorkListForm worklistForm = new();
            worklistForm.AccountLaunched += OnAccountLaunched;
            NewForm(worklistForm);
        }
    }

    private void OnAccountLaunched(object sender, string e) => LaunchAccount(e);

    private void reportingPortalToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        string url = Program.AppEnvironment.ApplicationParameters.ReportingPortalUrl;
        //ReportingPortalForm frm = new ReportingPortalForm(url);
        if (url != "")
        {
            OS.OpenBrowser(url);
        }
        else
        {
            MessageBox.Show("Reporting Portal System Parameter not set or not valid. Please contact your administrator", "Application Error");
        }
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        AboutBox about = new();
        about.ShowDialog();
    }

    private void badDebtMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        var formsList = Application.OpenForms.OfType<PatientCollectionsForm>();

        if (formsList.Any())
        {
            formsList.First().Focus();
        }
        else
        {
            PatientCollectionsForm frm = new();
            frm.AccountLaunched += OnAccountLaunched;
            NewForm(frm);
        }
    }


    private void duplicateAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new DuplicateAccountsForm(Helper.GetArgs()));

    private void reportByInsuranceCompanyToolStripMenuItem_Click(object sender, EventArgs e)
    {
        InsuranceReportForm frm = new(Program.AppEnvironment.GetArgs());
        frm.AccountLaunched += OnAccountLaunched;
        NewForm(frm);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Log.Instance.Trace($"Entering");
        //clear all locks opened by this user & host
        try
        {
            _accountService.ClearAccountLocks(Program.AppEnvironment.User, OS.GetMachineName());
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal("Error removing account locks.", ex);
        }
        try
        {
            Properties.Settings.Default.Save();
        }
        catch (Exception exc)
        {
            Log.Instance.Fatal("Exception during close.", exc);
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        var formsList = Application.OpenForms.OfType<ClientMaintenanceForm>();

        if (formsList.Any())
        {
            formsList.First().Focus();
        }
        else
        {
            NewForm(new ClientMaintenanceForm());
        }
    }

    private void accountChargeEntryToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        var formsList = Application.OpenForms.OfType<AccountChargeEntry>();

        if (formsList.Any())
        {
            formsList.First().Focus();
        }
        else
        {
            NewForm(new AccountChargeEntry());
        }

    }

    private void batchRemittanceToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new BatchRemittance());

    private void physiciansToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new PhysicianMaintenanceForm());

    private void interfaceMappingToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new InterfaceMapping());

    private void MainForm_MdiChildActivate(object sender, EventArgs e)
    {
        if (this.ActiveMdiChild == null)
            mdiTabControl.Visible = false;
        else
        {
            this.ActiveMdiChild.WindowState = FormWindowState.Maximized;
            if (this.ActiveMdiChild.Tag == null)
            {
                //Add a tabPage to the tabControl with child form caption
                TabPage tp = new(this.ActiveMdiChild.Text)
                {
                    Tag = this.ActiveMdiChild,
                    Name = this.ActiveMdiChild.Text,
                };

                tp.Padding = new Padding(3);

                mdiTabControl.TabPages.Add(tp);

                mdiTabControl.SelectedTab = tp;

                this.ActiveMdiChild.Tag = tp;
                this.ActiveMdiChild.FormClosed += new FormClosedEventHandler(ActiveMdiChild_FormClosed);
            }
            else
            {
                if (this.ActiveMdiChild.Tag is TabPage tp)
                    mdiTabControl.SelectedTab = tp;
            }

            if (!mdiTabControl.Visible) mdiTabControl.Visible = true;
        }
    }

    private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (sender is Form form && form.Tag is TabPage tabPage)
        {
            tabPage.Dispose();
        }
    }

    private void clientBillsNewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace("Entering");

        var formsList = Application.OpenForms.OfType<ClientInvoiceForm>();

        if (formsList.Any())
        {
            formsList.First().Focus();
        }
        else
        {
            ClientInvoiceForm form = new();
            form.AccountLaunched += OnAccountLaunched;
            NewForm(form);
        }
    }

    private void interfaceMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new InterfaceMonitor());

    private void pathologistsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void insurancePlansToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new HealthPlanMaintenanceForm());

    private void remittancePostingToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ProcessRemittanceForm frm = new();
        frm.RemittanceFileSelected += OnRemittanceSelected;
        NewForm(frm);
    }

    private void OnRemittanceSelected(object sender, int e)
    {
        PostRemittanceForm frm = new(e);
        frm.AccountLaunched += OnAccountLaunched;
        frm.FormClosed += PostRemittance_FormClosed;
        NewForm(frm);
    }

    private void PostRemittance_FormClosed(object sender, FormClosedEventArgs e)
    {
        //when the PostRemittanceForm closes, maked the ProcessRemittancesForm visible if it exists
        if (Application.OpenForms.OfType<ProcessRemittanceForm>().Any())
        {
            Application.OpenForms.OfType<ProcessRemittanceForm>().First().Activate();
        }
    }

    private void chargeMasterToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace("Entering");

        var formsList = Application.OpenForms.OfType<ChargeMasterMaintenance>();

        if (formsList.Any())
        {
            formsList.First().Focus();
        }
        else
        {
            NewForm(new ChargeMasterMaintenance());
        }
    }

    private void systemLogViewerToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new LogViewerForm());

    private void claimBatchManagementToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ClaimsManagementForm form = new();
        form.AccountLaunched += OnAccountLaunched;
        NewForm(form);
    }

    private void batchChargeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        => NewForm(new BatchChargeEntryForm());

    private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Form activForm;
        activForm = Form.ActiveForm.ActiveMdiChild;
        if (activForm == null)
            return;

        string url = Program.AppEnvironment.ApplicationParameters.DocumentationSiteUrl;
        string topicPath = null;
        switch (activForm.Name)
        {
            case nameof(WorkListForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.WorklistUrl;
                break;
            case nameof(AccountForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.AccountManagementUrl;
                break;
            case nameof(ChargeMasterMaintenance):
            case nameof(ChargeMasterEditForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.ChargeMasterMaintenanceUrl;
                break;
            case nameof(HealthPlanMaintenanceEditForm):
            case nameof(HealthPlanMaintenanceForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.InsurancePlanMaintenanceUrl;
                break;
            case nameof(PhysicianMaintenanceForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.PhysicianMaintenanceUrl;
                break;
            case nameof(ClientMaintenanceForm):
            case nameof(ClientMaintenanceEditForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.ClientMaintenanceUrl;
                break;
            case nameof(BatchRemittance):
                topicPath = Program.AppEnvironment.ApplicationParameters.BatchRemittanceUrl;
                break;
            case nameof(ClaimsManagementForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.ClaimsManagementUrl;
                break;
            case nameof(AccountChargeEntry):
                topicPath = Program.AppEnvironment.ApplicationParameters.AccountChargeEntryUrl;
                break;
            case nameof(ClientInvoiceForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.ClientInvoicingUrl;
                break;
            case nameof(PatientCollectionsForm):
            case nameof(PatientCollectionsEditForm):
                topicPath = Program.AppEnvironment.ApplicationParameters.PatientCollectionsUrl;
                break;
            default:
                break;
        }
        if (!string.IsNullOrWhiteSpace(topicPath))
            url += "/" + topicPath;
        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.F1)
            documentationToolStripMenuItem_Click(sender, e);
    }

    private void latestUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string url = Program.AppEnvironment.ApplicationParameters.DocumentationSiteUrl;
        string topicPath = null;
        topicPath = Program.AppEnvironment.ApplicationParameters.LatestUpdatesUrl;
        if (!string.IsNullOrWhiteSpace(topicPath))
            url += "/" + topicPath;
        System.Diagnostics.Process.Start(url);
    }

    private void mdiTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((mdiTabControl.SelectedTab != null) &&
            (mdiTabControl.SelectedTab.Tag != null))
        {
            Form frm = mdiTabControl.SelectedTab.Tag as Form;
            frm.Activate();
        }
    }

    private void auditReportsToolStripMenuItem_Click(object sender, EventArgs e) => NewForm(new AuditReportMaintenanceForm());

    private void viewLocksToolStripMenuItem_Click(object sender, EventArgs e)
    {
        AccountLocksForm frm = new();

        frm.ShowDialog();
    }
}
