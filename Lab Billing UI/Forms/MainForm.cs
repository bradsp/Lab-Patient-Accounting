using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Forms;
using LabBilling.Logging;
using LabBilling.Legacy;
using System;
using System.Windows.Forms;
using LabBilling.ReportByInsuranceCompany;
using System.Linq;
using Opulos.Core.UI;
using LabBilling.Core.BusinessLogic;
using System.Threading;
using Application = System.Windows.Forms.Application;
using NLog.Config;
using NLog.Targets;
using NLog;
using System.Runtime.InteropServices;
using System.Collections.Generic;


/*
 * Tabbed MDI logic 
 * https://www.codeproject.com/Articles/17640/Tabbed-MDI-Child-Forms
 * 
 * 
 * 
 */


namespace LabBilling
{
    public partial class MainForm : BaseForm
    {

        private Accordion accordion = null;
        private readonly UserProfileRepository userProfile = null;
        private readonly AccountRepository accountRepository = null;
        private readonly SystemParametersRepository systemParametersRepository = null;
        private ProgressBar claimProgress;
        private Label claimProgressStatusLabel;
        private CancellationTokenSource cancellationToken;
        private TableLayoutPanel tlpRecentAccounts;
        private List<UserProfile> recentAccounts;
        private List<Account> recentAccountsByAccount;

        private const int WM_SETREDRAW = 11;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        public ProgressReportModel progressReportModel = new ProgressReportModel()
        {
            RecordsProcessed = -1
        };

        public MainForm()
        {
            InitializeComponent();

            ConfigureLogging();

            MainFormMenu.BackColor = Program.AppEnvironment.MenuBackgroundColor;
            MainFormMenu.ForeColor = Program.AppEnvironment.MenuTextColor;

            panel1.BackColor = Program.AppEnvironment.WindowBackgroundColor;
            mdiTabControl.Parent.BackColor = Program.AppEnvironment.WindowBackgroundColor;


            userProfile = new UserProfileRepository(Program.AppEnvironment);
            accountRepository = new AccountRepository(Program.AppEnvironment);
            systemParametersRepository = new SystemParametersRepository(Program.AppEnvironment);
            accordion = new Accordion();

            Program.AppEnvironment.ApplicationParameters = systemParametersRepository.LoadParameters();

            recentAccounts = userProfile.GetRecentAccount(Program.LoggedInUser.UserName).ToList();
            recentAccountsByAccount = new();
            foreach (UserProfile up in recentAccounts)
            {
                recentAccountsByAccount.Add(accountRepository.GetByAccount(up.ParameterData, true));
            }

        }

        private static void ConfigureLogging()
        {
            #region Configure NLog

            var configuration = new NLog.Config.LoggingConfiguration();

            LogLevel minLevel = NLog.LogLevel.Trace;

            var fileTarget = new FileTarget("logfile") { FileName = "c:\\temp\\lab-billing-log.txt" };
            //var consoleTarget = new NLog.Targets.ConsoleTarget("logconsole");
            string logTable = Program.AppEnvironment.ApplicationParameters.DatabaseEnvironment != "Production" ? "LogsTest" : "Logs";
            var dbTarget = new DatabaseTarget("database")
            {
                ConnectionString = Program.AppEnvironment.LogConnectionString,
                CommandText = $"INSERT INTO {logTable} (CreatedOn,Message,Level,Exception,StackTrace,Logger,HostName,Username,CallingSite,CallingSiteLineNumber,AppVersion,DatabaseName,DatabaseServer) " +
                    "VALUES (@datetime,@msg,@level,@exception,@trace,@logger,@hostname,@user,@callsite,@lineno,@version,@dbname,@dbserver)",
            };

            GlobalDiagnosticsContext.Set("dbname", Program.AppEnvironment.DatabaseName);
            GlobalDiagnosticsContext.Set("dbserver", Program.AppEnvironment.ServerName);

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@datetime", new NLog.Layouts.SimpleLayout("${date}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@msg", new NLog.Layouts.SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@trace", new NLog.Layouts.SimpleLayout("${stacktrace}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@hostname", new NLog.Layouts.SimpleLayout("${hostname}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@user", new NLog.Layouts.SimpleLayout("${environment-user}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@callsite", new NLog.Layouts.SimpleLayout("${callsite}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@lineno", new NLog.Layouts.SimpleLayout("${callsite-linenumber}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@version", new NLog.Layouts.SimpleLayout("${assembly-version}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@dbname", new NLog.Layouts.SimpleLayout("${gdc:item=dbname}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@dbserver", new NLog.Layouts.SimpleLayout("${gdc:item=dbserver}")));

            var dbRule = new LoggingRule("*", minLevel, dbTarget);

            configuration.AddRule(dbRule);

            LogManager.Configuration = configuration;

            #endregion

        }

        private void NewForm(Form childForm)
        {
            if(MdiChildren.Length >= Program.AppEnvironment.ApplicationParameters.TabsOpenLimit)
            {
                AskCloseTabForm askCloseTab = new(MdiChildren.Select(x => x.Text).ToList());
                if(askCloseTab.ShowDialog() == DialogResult.OK)
                {
                    var result = askCloseTab.SelectedForm;
                    try
                    {
                        MdiChildren.Where(x => x.Text == result).First().Close();
                    }
                    catch(Exception ex)
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
            childForm.Show();
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

            UserSecurity frm = new UserSecurity
            {
                MdiParent = this,
                WindowState = FormWindowState.Normal,
                AutoScroll = true
            };
            frm.Show();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            PersonSearchForm frm = new PersonSearchForm();
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
                    NewForm(accFrm);
                }
            }
        }

        private void AccFrm_AccountOpenedEvent(object sender, string e)
            => UpdateRecentAccounts(e);

        public void UpdateRecentAccounts(string newAccount)
        {
            var ar = accountRepository.GetByAccount(newAccount, true);
            if (ar != null)
            {
                tlpRecentAccounts.Controls.RemoveAt(0);
                LinkLabel a1 = new LinkLabel { Text = ar.PatFullName, Tag = newAccount };
                a1.LinkClicked += new LinkLabelLinkClickedEventHandler(RecentLabelClicked);
                tlpRecentAccounts.Controls.Add(a1);
                a1.Dock = DockStyle.Fill;
                accordion.Refresh();
                accordion.AutoScroll = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            Log.Instance.Info($"Launching MainForm");

            #region user authentication

            if (Program.LoggedInUser == null)
            {
                Log.Instance.Fatal("There is not a valid user object.");
                MessageBox.Show(this, "Application error with user object. Aborting.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
            #endregion

            LoadSideMenu();

            NewForm(new DashboardForm());

            //enable menu items based on permissions
            systemAdministrationToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;
            UpdateMenuAccess();
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
        }

        private void LoadSideMenu()
        {
            #region load accordion menu

            //recent accounts section

            tlpRecentAccounts = new TableLayoutPanel { Dock = DockStyle.Fill };
            tlpRecentAccounts.RowCount = recentAccounts.Count;
            tlpRecentAccounts.ColumnCount = 1;

            toolStripDatabaseLabel.Text = Program.AppEnvironment.DatabaseName;
            toolStripUsernameLabel.Text = Program.LoggedInUser.FullName;
            this.Text += " " + Program.AppEnvironment.DatabaseName;
            if (!string.IsNullOrEmpty(Program.LoggedInUser.ImpersonatingUser))
            {
                this.Text += $"  *** IMPERSONATING {Program.LoggedInUser.ImpersonatingUser} ***";
            }

            if (!Program.AppEnvironment.ApplicationParameters.AllowEditing)
                this.Text += " | READ ONLY MODE";
            if (!Program.AppEnvironment.ApplicationParameters.AllowChargeEntry)
                this.Text += " | Charge entry disabled";
            if (!Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry)
                this.Text += " | Pmt/Adj entry disabled";

            foreach (var acc in recentAccountsByAccount)
            {
                if (acc != null)
                {
                    LinkLabel a1 = new() { Text = acc.PatFullName, Tag = acc.AccountNo };
                    a1.LinkClicked += new LinkLabelLinkClickedEventHandler(RecentLabelClicked);
                    tlpRecentAccounts.Controls.Add(a1);
                    a1.Dock = DockStyle.Fill;
                }
            }

            panel1.Controls.Add(accordion);
            tlpRecentAccounts.AutoSize = true;
            accordion.Add(tlpRecentAccounts, "Recent Accounts", "Last Opened Accounts", 1, true);

            //Billing Menu Section
            TableLayoutPanel tlpBilling = new TableLayoutPanel { Dock = DockStyle.Fill };
            tlpBilling.ColumnCount = 1;
            tlpBilling.RowCount = 3;

            Button b1 = new() { Text = "Worklist", Name = "btnWorkList" };
            b1.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            b1.ForeColor = Program.AppEnvironment.ButtonTextColor;
            b1.Click += new EventHandler(worklistToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b1, 0, 0);
            b1.Dock = DockStyle.Fill;

            Button b2 = new() { Text = "Account", Name = "btnAccount" };
            b2.Click += new EventHandler(accountToolStripMenuItem_Click);
            b2.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            b2.ForeColor = Program.AppEnvironment.ButtonTextColor;
            tlpBilling.Controls.Add(b2, 0, 2);
            b2.Dock = DockStyle.Fill;

            Button b4 = new() { Text = "Account Charge Entry", Name = "btnAccountChargeEntry" };
            b4.Click += new EventHandler(accountChargeEntryToolStripMenuItem_Click);
            b4.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            b4.ForeColor = Program.AppEnvironment.ButtonTextColor;
            tlpBilling.Controls.Add(b4, 0, 3);
            b4.Dock = DockStyle.Fill;

            Button b5 = new() { Text = "Batch Remittance", Name = "btnBatchRemittance" };
            b5.Click += new EventHandler(batchRemittanceToolStripMenuItem_Click);
            b5.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            b5.ForeColor = Program.AppEnvironment.ButtonTextColor;
            tlpBilling.Controls.Add(b5, 0, 4);
            b5.Dock = DockStyle.Fill;

            Button b6 = new() { Text = "Claims Batch Management", Name = "ClaimBatchManagementButton" };
            b6.Click += new EventHandler(claimBatchManagementToolStripMenuItem_Click);
            b6.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            b6.ForeColor = Program.AppEnvironment.ButtonTextColor;
            tlpBilling.Controls.Add(b6, 0, 5);
            b6.Dock = DockStyle.Fill;

            accordion.Add(tlpBilling, "Billing", "Billing Functions", 1, true);

            //Reports Section
            TableLayoutPanel tlpReports = new TableLayoutPanel { Dock = DockStyle.Fill };
            tlpReports.ColumnCount = 1;
            tlpReports.RowCount = 1;

            Button r1 = new() { Text = "Monthly Reports", Name = "btnMonthlyReports" };
            r1.Click += new EventHandler(monthlyReportsToolStripMenuItem_Click);
            r1.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            r1.ForeColor = Program.AppEnvironment.ButtonTextColor;
            tlpReports.Controls.Add(r1, 0, 0);
            r1.Dock = DockStyle.Fill;

            Button r2 = new() { Text = "Reporting Portal", Name = "btnReportingPortal" };
            r2.Click += new EventHandler(reportingPortalToolStripMenuItem_Click);
            r2.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
            r2.ForeColor = Program.AppEnvironment.ButtonTextColor;
            tlpReports.Controls.Add(r2, 0, 0);
            r2.Dock = DockStyle.Fill;

            accordion.Add(tlpReports, "Reports", "Report Functions", 1, true);

            accordion.FillWidth = true;
            accordion.FillHeight = false;
            accordion.PerformLayout();
            accordion.PerformLayout();
            #endregion

            if (Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry)
            {
                batchRemittanceToolStripMenuItem.Visible = Program.LoggedInUser.CanAddPayments;
                remittancePostingToolStripMenuItem.Visible = Program.LoggedInUser.CanAddPayments;
                posting835RemitToolStripMenuItem.Visible = Program.LoggedInUser.CanAddPayments;
                b5.Visible = Program.LoggedInUser.CanAddPayments;
            }
            else
            {
                batchRemittanceToolStripMenuItem.Visible = false;
                remittancePostingToolStripMenuItem.Visible = false;
                posting835RemitToolStripMenuItem.Visible = false;
                b5.Visible = false;
            }

            if (Program.AppEnvironment.ApplicationParameters.AllowChargeEntry)
            {
                accountChargeEntryToolStripMenuItem.Visible = Program.LoggedInUser.CanSubmitCharges;
                batchChargeEntryToolStripMenuItem.Visible = Program.LoggedInUser.CanSubmitCharges;
                b4.Visible = Program.LoggedInUser.CanSubmitCharges;
            }
            else
            {
                accountChargeEntryToolStripMenuItem.Visible = false;
                batchChargeEntryToolStripMenuItem.Visible = false;
                b4.Visible = false;
            }

            if (!Program.AppEnvironment.ApplicationParameters.AllowEditing)
            {
                MessageBox.Show(this, "System is in read-only mode.", "Read Only Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void UpdateMenuAccess()
        {
            //during testing only - remove once batch charge entry is in production
            batchChargeEntryToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;

            batchRemittanceToolStripMenuItem.Visible = Program.LoggedInUser.CanAddPayments;
            badDebtMaintenanceToolStripMenuItem.Visible = Program.LoggedInUser.CanModifyBadDebt;
            posting835RemitToolStripMenuItem.Visible = Program.LoggedInUser.CanAddPayments;
            accountChargeEntryToolStripMenuItem.Visible = Program.LoggedInUser.CanSubmitCharges;

            //administrator only menu items
            systemAdministrationToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;

            if (Program.LoggedInUser.Access == "VIEW")
            {
                duplicateAccountsToolStripMenuItem.Visible = false;
                clientBillsNewToolStripMenuItem.Visible = false;
                batchChargeEntryToolStripMenuItem.Visible = false;
                accountChargeEntryToolStripMenuItem.Visible = false;
            }

        }

        private void RecentLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var linkLabel = (LinkLabel)sender;

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
                AccountForm accFrm = new AccountForm(account);
                accFrm.AccountOpenedEvent += AccFrm_AccountOpenedEvent;
                NewForm(accFrm);
                Cursor.Current = Cursors.Default;
            }
            ResumeLayout();
        }

        private void systemParametersToolStripMenuItem_Click(object sender, EventArgs e)
            => new SystemParametersForm().ShowDialog();

        private void monthlyReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MonthlyReportsForm frm = new(Program.AppEnvironment.GetArgs());
            frm.AccountLaunched += OnAccountLaunched;
            NewForm(frm);
        }

        private void worklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (Application.OpenForms.OfType<WorkListForm>().Count() > 0)
            {
                WorkListForm workListForm = Application.OpenForms.OfType<WorkListForm>().First();
                workListForm.Focus();
            }
            else
            {
                WorkListForm worklistForm = new WorkListForm(Helper.ConnVal);
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
                System.Diagnostics.Process.Start(url);
            }
            else
            {
                MessageBox.Show("Reporting Portal System Parameter not set or not valid. Please contact your administrator", "Application Error");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            AboutBox about = new AboutBox();
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
                NewForm(new PatientCollectionsForm());
            }
        }


        private void duplicateAccountsToolStripMenuItem_Click(object sender, EventArgs e)
            => NewForm(new DuplicateAccountsForm(Helper.GetArgs()));

        private void reportByInsuranceCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsuranceReportForm frm = new InsuranceReportForm(Program.AppEnvironment.GetArgs());
            frm.AccountLaunched += OnAccountLaunched;
            NewForm(frm);
        }

        private void posting835RemitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Posting835 frm = new Posting835(Program.AppEnvironment.GetArgs());
            frm.AccountLaunched += OnAccountLaunched;
            NewForm(frm);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Properties.Settings.Default.Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var formsList = Application.OpenForms.OfType<ClientMaintenanceForm>();

            if (formsList.Count() > 0)
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

            if (formsList.Count() > 0)
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
                    TabPage tp = new TabPage(this.ActiveMdiChild.Text);
                    tp.Tag = this.ActiveMdiChild;
                    tp.Parent = mdiTabControl;
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
            => ((sender as Form).Tag as TabPage).Dispose();

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
                ClientInvoiceForm form = new ClientInvoiceForm();
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

        private void claimValidationRulesToolStripMenuItem_Click(object sender, EventArgs e)
            => NewForm(new ClaimRuleEditorForm());

        private bool CheckForDuplicate(Form newForm)
        {
            bool bValue = false;
            foreach (Form fm in this.MdiChildren)
            {
                if (fm.GetType() == newForm.GetType())
                {
                    fm.Activate();
                    fm.WindowState = FormWindowState.Maximized;
                    bValue = true;
                }
            }
            return bValue;
        }

        private void insurancePlansToolStripMenuItem_Click(object sender, EventArgs e)
            => NewForm(new HealthPlanMaintenanceForm());

        private void remittancePostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remittance835 remittance835 = new Remittance835(Program.AppEnvironment);
            string file = @"\\wthmclbill\shared\Billing\TEST\Posting835Remit\MCL_NC_MCR_1093705428_835_11119267.RMT";

            remittance835.Load835(file);

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
            System.Diagnostics.Process.Start(url);
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
                SendMessage(this.Handle, WM_SETREDRAW, false, 0);
                (mdiTabControl.SelectedTab.Tag as Form).Select();
                SendMessage(this.Handle, WM_SETREDRAW, true, 0);
                this.Refresh();
            }
        }
    }
}
