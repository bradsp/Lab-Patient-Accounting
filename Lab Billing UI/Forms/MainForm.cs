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
using System.Collections.Generic;
using LabBilling.Core.BusinessLogic;
using MetroFramework.Forms;
using MetroFramework.Controls;
using MetroFramework;
using System.Threading.Tasks;
using System.Threading;

using Application = System.Windows.Forms.Application;
using NLog.Config;
using NLog.Targets;
using RFClassLibrary;
using NLog;

namespace LabBilling
{
    public partial class MainForm : MetroForm
    {

        private Accordion accordion = null;
        private readonly UserProfileRepository userProfile = null;
        private readonly AccountRepository accountRepository = null;
        private readonly SystemParametersRepository systemParametersRepository = null;
        private ProgressBar claimProgress;
        private Label claimProgressStatusLabel;
        private CancellationTokenSource cancellationToken;
        private TableLayoutPanel tlpRecentAccounts;

        public ProgressReportModel progressReportModel = new ProgressReportModel()
        {
            RecordsProcessed = -1
        };

        public MainForm()
        {
            InitializeComponent();

            #region Configure NLog

            var configuration = new NLog.Config.LoggingConfiguration();

            var fileTarget = new NLog.Targets.FileTarget("logfile") { FileName = "c:\\temp\\lab-billing-log.txt" };
            var consoleTarget = new NLog.Targets.ConsoleTarget("logconsole");
            var dbTarget = new NLog.Targets.DatabaseTarget("database")
            {
                ConnectionString = Program.AppEnvironment.LogConnectionString,
                CommandText = @"INSERT INTO Logs(CreatedOn,Message,Level,Exception,StackTrace,Logger,HostName,Username,CallingSite,CallingSiteLineNumber,AppVersion,DatabaseName,DatabaseServer) VALUES (@datetime,@msg,@level,@exception,@trace,@logger,@hostname,@user,@callsite,@lineno,@version,@dbname,@dbserver)",
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

            var dbRule = new LoggingRule("*", NLog.LogLevel.Trace, dbTarget);

            configuration.AddRule(dbRule);

            NLog.LogManager.Configuration = configuration;

            #endregion


            userProfile = new UserProfileRepository(Program.AppEnvironment);
            accountRepository = new AccountRepository(Program.AppEnvironment);
            systemParametersRepository = new SystemParametersRepository(Program.AppEnvironment);
            accordion = new Accordion();
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

                var formsList = System.Windows.Forms.Application.OpenForms.OfType<AccountForm>();
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
                    accFrm.MdiParent = this;
                    accFrm.WindowState = FormWindowState.Normal;
                    accFrm.AutoScroll = true;
                    accFrm.Show();
                    //accFrm.AccountOpenedEvent -= AccFrm_AccountOpenedEvent;
                }
            }
        }

        private void AccFrm_AccountOpenedEvent(object sender, string e)
        {
            var ar = accountRepository.GetByAccount(e, true);
            if (ar != null)
            {
                LinkLabel a1 = new LinkLabel { Text = ar.PatFullName, Tag = e };
                a1.LinkClicked += new LinkLabelLinkClickedEventHandler(RecentLabelClicked);
                tlpRecentAccounts.Controls.Add(a1);
                a1.Dock = DockStyle.Fill;
                accordion.Refresh();
                accordion.AutoScroll = true;
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            //set shadowtype to none to resolve access after dispose error
            this.ShadowType = MetroFormShadowType.None;

            #region user authentication

            if (Program.LoggedInUser == null)
            {
                Log.Instance.Fatal("There is not a valid user object.");
                MetroMessageBox.Show(this, "Application error with user object. Aborting.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                System.Windows.Forms.Application.Exit();
            }
            #endregion

            #region load accordion menu

            Program.AppEnvironment.ApplicationParameters = systemParametersRepository.LoadParameters();

            //recent accounts section
            var recentAccounts = userProfile.GetRecentAccount(Program.LoggedInUser.UserName).ToList();

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

            foreach (UserProfile up in recentAccounts)
            {
                var ar = await accountRepository.GetByAccountAsync(up.ParameterData, true);
                if (ar != null)
                {
                    LinkLabel a1 = new LinkLabel { Text = ar.PatFullName, Tag = up.ParameterData };
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

            MetroButton b1 = new MetroButton { Text = "Worklist", Name = "btnWorkList" };
            b1.Click += new EventHandler(worklistToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b1, 0, 0);
            b1.Dock = DockStyle.Fill;
            /*
            MetroButton b3 = new MetroButton { Text = "Workqueue", Name = "btnWorkQueue" };
            b3.Click += new EventHandler(workqueuesToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b3, 0, 1);
            b3.Dock = DockStyle.Fill;
            */
            MetroButton b2 = new MetroButton { Text = "Account", Name = "btnAccount" };
            b2.Click += new EventHandler(accountToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b2, 0, 2);
            b2.Dock = DockStyle.Fill;

            //Button b3 = new Button { Text = "Demographics", Name = "btnDemographics" };
            //b3.Click += new EventHandler(PatientDemographics_Click);
            //tlpBilling.Controls.Add(b3, 0, 2);
            //b3.Dock = DockStyle.Fill;

            MetroButton b4 = new MetroButton { Text = "Account Charge Entry", Name = "btnAccountChargeEntry" };
            b4.Click += new EventHandler(accountChargeEntryToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b4, 0, 3);
            b4.Dock = DockStyle.Fill;

            MetroButton b5 = new MetroButton { Text = "Batch Remittance", Name = "btnBatchRemittance" };
            b5.Click += new EventHandler(batchRemittanceToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b5, 0, 4);
            b5.Dock = DockStyle.Fill;

            MetroButton b6 = new MetroButton { Text = "Claims Batch Management", Name = "ClaimBatchManagementButton" };
            b6.Click += new EventHandler(claimBatchManagementToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b6, 0, 5);
            b6.Dock = DockStyle.Fill;

            accordion.Add(tlpBilling, "Billing", "Billing Functions", 1, true);

            //Reports Section
            TableLayoutPanel tlpReports = new TableLayoutPanel { Dock = DockStyle.Fill };
            tlpReports.ColumnCount = 1;
            tlpReports.RowCount = 1;

            MetroButton r1 = new MetroButton { Text = "Monthly Reports", Name = "btnMonthlyReports" };
            r1.Click += new EventHandler(monthlyReportsToolStripMenuItem_Click);
            tlpReports.Controls.Add(r1, 0, 0);
            r1.Dock = DockStyle.Fill;

            MetroButton r2 = new MetroButton { Text = "Reporting Portal", Name = "btnReportingPortal" };
            r2.Click += new EventHandler(reportingPortalToolStripMenuItem_Click);
            tlpReports.Controls.Add(r2, 0, 0);
            r2.Dock = DockStyle.Fill;

            accordion.Add(tlpReports, "Reports", "Report Functions", 1, true);

            accordion.FillWidth = true;
            accordion.FillHeight = false;
            accordion.PerformLayout();
            accordion.PerformLayout();
            #endregion

            DashboardForm frm = new DashboardForm();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();

            //enable menu items based on permissions
            systemAdministrationToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;

            if(Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry)
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

            if(!Program.AppEnvironment.ApplicationParameters.AllowEditing)
            {                
                MetroMessageBox.Show(this, "System is in read-only mode.", "Read Only Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            UpdateMenuAccess();

        }

        private void UpdateMenuAccess()
        {
            //during testing only - remove once batch charge entry is in production
            batchChargeEntryToolStripMenuItem.Visible = Program.LoggedInUser.IsAdministrator;

            generateClaimsToolStripMenuItem.Visible = false; // Program.LoggedInUser.CanSubmitBilling;
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

            bool IsAlreadyOpen = false;
            if (Application.OpenForms.OfType<AccountForm>().Count() > 0)
            {
                foreach (AccountForm frm in System.Windows.Forms.Application.OpenForms.OfType<AccountForm>())
                {
                    if (frm.SelectedAccount == linkLabel.Tag.ToString())
                    {
                        IsAlreadyOpen = true;
                        frm.Focus();
                    }
                }
            }

            if (!IsAlreadyOpen)
            {
                AccountForm frm = new AccountForm(linkLabel.Tag.ToString())
                {
                    MdiParent = this
                };
                Cursor.Current = Cursors.WaitCursor;
                frm.Show();
                Cursor.Current = Cursors.Default;
            }

        }

        private void systemParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            SystemParametersForm frm = new SystemParametersForm();
            frm.ShowDialog();
        }

        private void monthlyReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmReports frm = new frmReports(Helper.GetArgs());
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

        private void worklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (System.Windows.Forms.Application.OpenForms.OfType<WorkListForm>().Count() > 0)
            {
                WorkListForm workListForm = System.Windows.Forms.Application.OpenForms.OfType<WorkListForm>().First();
                workListForm.Focus();
            }
            else
            {
                WorkListForm worklistForm = new WorkListForm(Helper.ConnVal);
                worklistForm.MdiParent = this;
                worklistForm.AutoScroll = true;
                worklistForm.WindowState = FormWindowState.Normal;
                worklistForm.Show();
            }
        }

        private void reportingPortalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            //string url = systemParametersRepository.GetByKey("report_portal_url");
            string url = Program.AppEnvironment.ApplicationParameters.ReportingPortalUrl;
            ReportingPortalForm frm = new ReportingPortalForm(url);

            if(url != "")
            {
                System.Diagnostics.Process.Start(url);
            }
            else
            {
                MessageBox.Show("Reporting Portal System Parameter not set or not valid. Please contact your administrator","Application Error");
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

            if (formsList.Count() > 0)
            {
                formsList.First().Focus();
            }
            else
            {

                PatientCollectionsForm frm = new PatientCollectionsForm
                {
                    MdiParent = this,
                    AutoScroll = true,
                    WindowState = FormWindowState.Normal
                };
                frm.Show();
            }
        }


        private void duplicateAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            FrmDupAcc frm = new FrmDupAcc(Helper.GetArgs());
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void reportByInsuranceCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmReport frm = new frmReport(Helper.GetArgs()) { MdiParent = this, AutoScroll = true, WindowState = FormWindowState.Normal };
            frm.Show();
        }

        private void posting835RemitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            Legacy.Posting835 frm = new Legacy.Posting835(Helper.GetArgs())
            {
                MdiParent = this,
                AutoScroll = true,
                WindowState = FormWindowState.Normal
            };

            frm.Show();
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Properties.Settings.Default.Save();
            //if this form is closing, close all other open forms
            System.Windows.Forms.Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            this.Close();
            //Application.Exit();
        }

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
                ClientMaintenanceForm frm = new ClientMaintenanceForm
                {
                    MdiParent = this,
                    WindowState = FormWindowState.Normal,
                    AutoScroll = true
                };
                frm.Show();
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
                AccountChargeEntry frm = new AccountChargeEntry
                {
                    MdiParent = this
                };
                frm.Show();
            }

        }

        private void batchRemittanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            BatchRemittance frm = new BatchRemittance();
            frm.MdiParent = this;
            frm.Show();
        }

        private void physiciansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            PhysicianMaintenanceForm frm = new PhysicianMaintenanceForm
            {
                MdiParent = this,
                WindowState = FormWindowState.Normal,
                AutoScroll = true
            };
            frm.Show();
        }

        private void interfaceMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            InterfaceMapping frm = new InterfaceMapping { MdiParent = this, WindowState = FormWindowState.Normal, AutoScroll = true };
            frm.Show();

        }

        private void Dashboard_MdiChildActivate(object sender, EventArgs e)
        {
            //if (this.MdiChildren.Count() > Convert.ToInt32(systemParametersRepository.GetByKey("tabs_open_limit")))
            if(this.MdiChildren.Count() > Program.AppEnvironment.ApplicationParameters.TabsOpenLimit)
            {

                List<string> openForms = new List<string>();

                foreach (Form child in this.MdiChildren)
                {
                    openForms.Add(child.Text);
                }

                AskCloseTabForm frm = new AskCloseTabForm(openForms);

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    string selectedForm = frm.SelectedForm;

                    this.MdiChildren.First(x => x.Text == selectedForm).Close();
                }

                //MessageBox.Show(sb.ToString());
            }
        }

        private void clientBillsNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            var formsList = Application.OpenForms.OfType<ClientInvoiceForm>();

            if (formsList.Count() > 0)
            {
                formsList.First().Focus();
            }
            else
            {
                ClientInvoiceForm frm = new ClientInvoiceForm { MdiParent = this, WindowState = FormWindowState.Normal, AutoScroll = true };
                frm.Show();
            }
        }

        private void interfaceMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            InterfaceMonitor frm = new InterfaceMonitor { MdiParent = this, WindowState = FormWindowState.Normal, AutoScroll = true };
            frm.Show();

        }

        private void pathologistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void claimValidationRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            ClaimRuleEditorForm frm = new ClaimRuleEditorForm();

            frm.ShowDialog();
        }

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

        private void professionalToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _ = RunBillingBatch(BillingType.Professional);
        }

        private enum BillingType
        {
            Institutional,
            Professional
        }

        private async Task RunBillingBatch(BillingType billingType)
        {
            cancellationToken = new CancellationTokenSource();

            TableLayoutPanel tlpClaimBatch = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1
            };

            Label claimProcessTitleLabel = new Label();
            switch (billingType)
            {
                case BillingType.Institutional:
                    claimProcessTitleLabel.Text = "Institutional Claims";
                    break;
                case BillingType.Professional:
                    claimProcessTitleLabel.Text = "Professional Claims";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(billingType));
            }
            tlpClaimBatch.Controls.Add(claimProcessTitleLabel, 0, 0);
            claimProcessTitleLabel.Dock = DockStyle.Fill;

            claimProgress = new ProgressBar();
            claimProgress.Style = ProgressBarStyle.Continuous;
            claimProgress.Minimum = 0;
            tlpClaimBatch.Controls.Add(claimProgress, 0, 1);
            claimProgress.Dock = DockStyle.Fill;

            claimProgressStatusLabel = new Label();
            tlpClaimBatch.Controls.Add(claimProgressStatusLabel, 0, 2);
            claimProgressStatusLabel.Text = "Processing...";
            claimProgressStatusLabel.Dock = DockStyle.Fill;

            MetroButton cancelButton = new MetroButton { Text = "Cancel", Name = "cancelButton" };
            cancelButton.Click += new EventHandler(cancelButton_Click);
            tlpClaimBatch.Controls.Add(cancelButton, 0, 3);
            cancelButton.Dock = DockStyle.Fill;

            accordion.Add(tlpClaimBatch, "Claim Batch", "Claim Batch", 1, true);
            accordion.PerformLayout();

            ClaimGenerator claims = new ClaimGenerator(Program.AppEnvironment);
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;
            try
            {
                int claimsProcessed = 0;
                if (billingType == BillingType.Institutional)
                {
                    claimsProcessed = await Task.Run(() =>
                    {
                        return claims.CompileBillingBatch(Core.ClaimType.Institutional, progress, cancellationToken.Token);
                    });
                }
                else if (billingType == BillingType.Professional)
                {
                    claimsProcessed = await Task.Run(() =>
                    {
                        return claims.CompileBillingBatch(Core.ClaimType.Professional, progress, cancellationToken.Token);
                    });
                }

                if (claimsProcessed < 0)
                {
                    MetroMessageBox.Show(this, "Error processing claims. No file generated.", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MetroMessageBox.Show(this, $"File generated. {claimsProcessed} claims generated.");
                }
            }
            catch (TaskCanceledException tce)
            {
                Log.Instance.Error(tce, $"{Enum.GetName(typeof(BillingType), billingType)} Claim Batch cancelled by user", tce);
                MetroMessageBox.Show(this, "Claim batch cancelled by user. No file was generated and batch has been rolled back.");
                claimProgressStatusLabel.Text = "Job aborted.";
                claimProgress.Value = claimProgress.Maximum;
                claimProgress.SetState(2);
                cancelButton.Enabled = false;
            }
        }

        private void institutionalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = RunBillingBatch(BillingType.Institutional);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will cancel the claim batch and rollback any changes. Are you sure?", "Cancel Batch?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                cancellationToken.Cancel();
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            claimProgress.Maximum = e.TotalRecords;
            claimProgress.Value = e.RecordsProcessed;
            claimProgressStatusLabel.Text = $"Processing {e.RecordsProcessed} of {e.TotalRecords}";
        }

        private void insurancePlansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            HealthPlanMaintenanceForm frm = new HealthPlanMaintenanceForm { MdiParent = this, WindowState = FormWindowState.Normal, AutoScroll = true };
            frm.Show();
        }

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

            if (formsList.Count() > 0)
            {
                formsList.First().Focus();
            }
            else
            {
                ChargeMasterMaintenance frm = new ChargeMasterMaintenance { MdiParent = this, WindowState = FormWindowState.Normal, AutoScroll = true };
                frm.Show();
            }
        }

        private void systemLogViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            LogViewerForm frm = new LogViewerForm { MdiParent = this, WindowState = FormWindowState.Normal, AutoScroll = true };
            frm.Show();
        }

        private void claimBatchManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            ClaimsManagementForm frm = new ClaimsManagementForm();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();

        }

        private void batchChargeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            BatchChargeEntryForm frm = new BatchChargeEntryForm();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

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
            if(e.KeyData == Keys.F1)
            {
                documentationToolStripMenuItem_Click(sender, e);
            }
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
    }
}
