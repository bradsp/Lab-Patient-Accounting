﻿using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Forms;
using LabBilling.Logging;
using LabBilling.Legacy;
using System;
using System.Windows.Forms;
using LabBilling.ReportByInsuranceCompany;
using System.Linq;
using Opulos.Core.UI;
using System.Text;
using System.Collections.Generic;
using LabBilling.Core.BusinessLogic;
using MetroFramework.Forms;
using MetroFramework.Controls;
using System.Drawing;

namespace LabBilling
{
    public partial class MainForm : MetroForm
    {

        private Accordion accordion = new Accordion();
        private readonly UserProfileRepository userProfile = new UserProfileRepository(Helper.ConnVal);
        private readonly AccountRepository accountRepository = new AccountRepository(Helper.ConnVal);
        private readonly SystemParametersRepository systemParametersRepository = new SystemParametersRepository(Helper.ConnVal);

        public MainForm()
        {
            InitializeComponent();
        }

        private void userSecurityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            UserSecurity frm = new UserSecurity();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            PersonSearchForm frm = new PersonSearchForm();
            if(frm.ShowDialog() == DialogResult.OK)
            {
                AccountForm accFrm = new AccountForm(frm.SelectedAccount);
                accFrm.MdiParent = this;
                accFrm.WindowState = FormWindowState.Normal;
                accFrm.AutoScroll = true;
                accFrm.Show();
            }
            
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            

            #region user authentication

            if(Program.LoggedInUser == null)
            {
                Log.Instance.Fatal("There is not a valid user object.");
                MessageBox.Show("Application error with user object. Aborting.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }

            //enable menu items based on permissions
            systemAdministrationToolStripMenuItem.Enabled = Program.LoggedInUser.IsAdministrator;

            if (Convert.ToBoolean(systemParametersRepository.GetByKey("allow_chk_entry")))
            {
                paymentProcessingToolStripMenuItem.Enabled = Program.LoggedInUser.CanAddPayments;
                batchRemittanceToolStripMenuItem.Enabled = Program.LoggedInUser.CanAddPayments;
            }
            else
            {
                paymentProcessingToolStripMenuItem.Enabled = false;
                batchRemittanceToolStripMenuItem.Enabled = false;
            }
            if (Convert.ToBoolean(systemParametersRepository.GetByKey("allow_chrg_entry")))
            {
                batchChargeEntryToolStripMenuItem.Enabled = Program.LoggedInUser.CanSubmitCharges;
            }
            else
            {
                batchChargeEntryToolStripMenuItem.Enabled = false;
            }
            if (Convert.ToBoolean(systemParametersRepository.GetByKey("allow_edit")))
            {
                sSISubmissionToolStripMenuItem.Enabled = Program.LoggedInUser.CanSubmitBilling;
            }
            else
            {
                sSISubmissionToolStripMenuItem.Enabled = false;
                MessageBox.Show("System is in read-only mode.");
            }
            #endregion

            #region load accordion menu

            //recent accounts section
            var recentAccounts = userProfile.GetRecentAccount(Program.LoggedInUser.UserName).ToList();

            TableLayoutPanel tlpRecentAccounts = new TableLayoutPanel { Dock = DockStyle.Fill };
            tlpRecentAccounts.RowCount = recentAccounts.Count;
            tlpRecentAccounts.ColumnCount = 1;

            this.Text += " " + Helper.Environment;
            
            if (!Convert.ToBoolean(systemParametersRepository.GetByKey("allow_edit")))
                this.Text += " | READ ONLY MODE";
            if (!Convert.ToBoolean(systemParametersRepository.GetByKey("allow_chrg_entry")))
                this.Text += " | Charge entry disabled";
            if (!Convert.ToBoolean(systemParametersRepository.GetByKey("allow_chk_entry")))
                this.Text += " | Pmt/Adj entry disabled";
            
            foreach (UserProfile up in recentAccounts)
            {
                var ar = accountRepository.GetByAccount(up.ParameterData,true);
                if(ar != null)
                {
                    LinkLabel a1 = new LinkLabel { Text = ar.PatFullName, Tag = up.ParameterData };
                    a1.LinkClicked += new LinkLabelLinkClickedEventHandler(RecentLabelClicked);
                    tlpRecentAccounts.Controls.Add(a1);
                    a1.Dock = DockStyle.Fill;
                }
            }

            panel1.Controls.Add(accordion);
            accordion.Add(tlpRecentAccounts, "Recent Accounts", "Last Opened Accounts", 1, true);

            //Billing Menu Section
            TableLayoutPanel tlpBilling = new TableLayoutPanel { Dock = DockStyle.Fill };
            tlpBilling.ColumnCount = 1;
            tlpBilling.RowCount = 3;

            MetroButton b1 = new MetroButton { Text = "Workqueue", Name = "btnWorkQueue" };
            b1.Click += new EventHandler(workqueuesToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b1, 0, 0);
            b1.Dock = DockStyle.Fill;

            MetroButton b2 = new MetroButton { Text = "Account", Name = "btnAccount" };
            b2.Click += new EventHandler(accountToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b2, 0, 1);
            b2.Dock = DockStyle.Fill;

            //Button b3 = new Button { Text = "Demographics", Name = "btnDemographics" };
            //b3.Click += new EventHandler(PatientDemographics_Click);
            //tlpBilling.Controls.Add(b3, 0, 2);
            //b3.Dock = DockStyle.Fill;

            MetroButton b4 = new MetroButton { Text = "Batch Charge Entry", Name = "btnBatchChargeEntry" };
            b4.Click += new EventHandler(batchChargeEntryToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b4, 0, 3);
            b4.Dock = DockStyle.Fill;

            MetroButton b5 = new MetroButton { Text = "Batch Remittance", Name = "btnBatchRemittance" };
            b5.Click += new EventHandler(batchRemittanceToolStripMenuItem_Click);
            tlpBilling.Controls.Add(b5, 0, 4);
            b5.Dock = DockStyle.Fill;

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

        }

        private void RecentLabelClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var linkLabel = (LinkLabel)sender;

            bool IsAlreadyOpen = false;
            if (Application.OpenForms.OfType<AccountForm>().Count() > 0)
            {
                foreach (AccountForm frm in Application.OpenForms.OfType<AccountForm>())
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
                AccountForm frm = new AccountForm(linkLabel.Tag.ToString());
                frm.MdiParent = this;
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

            frmReports frm = new frmReports(Helper.args);
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

        private void workqueuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            WorkqueueForm frm = new WorkqueueForm(Helper.ConnVal);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();

        }

        private void reportingPortalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            string url = systemParametersRepository.GetByKey("report_portal_url");
            ReportingPortalForm frm = new ReportingPortalForm(url);

            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
            return;

            //SystemParametersRepository da = new SystemParametersRepository();
            /*
            string url = systemParametersRepository.GetByKey("report_portal_url");
            if(url != "")
            {
                System.Diagnostics.Process.Start(url);
            }
            else
            {
                MessageBox.Show("Reporting Portal System Parameter not set or not valid. Please contact your administrator","Application Error");
            }
            */
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

            frmBadDebt frm = new frmBadDebt(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void questCorrectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmCorrection frm = new frmCorrection(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void questProcessingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmQuest frm = new frmQuest(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void globalBillingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmGlobalBilling frm = new frmGlobalBilling(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void chargeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            frmCDM frm = new frmCDM(Helper.args);
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

        private void clientBillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmViewer frm = new frmViewer(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void duplicateAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            FrmDupAcc frm = new FrmDupAcc(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void sSISortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmSSISort frm = new frmSSISort(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void sSISubmissionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmSSI frm = new frmSSI(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void reportByInsuranceCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            frmReport frm = new frmReport(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Show();
        }

        private void posting835RemitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            Legacy.Posting835 frm = new Legacy.Posting835(Helper.args);
            frm.MdiParent = this;
            frm.AutoScroll = true;
            frm.WindowState = FormWindowState.Normal;

            frm.Show();
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            //if this form is closing, close all other open forms
            Application.Exit();
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

            ClientsForm frm = new ClientsForm();

            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

        private void batchChargeEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            BatchChargeEntry frm = new BatchChargeEntry();

            frm.MdiParent = this;
            frm.Show();
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

            PhysicianMaintenanceForm frm = new PhysicianMaintenanceForm();

            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();
        }

        private void interfaceMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            InterfaceMapping frm = new InterfaceMapping();
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();

        }

        private void Dashboard_MdiChildActivate(object sender, EventArgs e)
        {
            if(this.MdiChildren.Count() > Convert.ToInt32(systemParametersRepository.GetByKey("tabs_open_limit")))
            {

                List<string> openForms = new List<string>();

                foreach (Form child in this.MdiChildren)
                {
                    openForms.Add(child.Text);
                }

                AskCloseTabForm frm = new AskCloseTabForm(openForms);

                if(frm.ShowDialog(this) == DialogResult.OK)
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
            ClientInvoiceForm frm = new ClientInvoiceForm();

            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Normal;
            frm.AutoScroll = true;
            frm.Show();

        }

        private void interfaceMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            InterfaceMonitor frm = new InterfaceMonitor();

            frm.ShowDialog();
            
        }

        private void pathologistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void professionalBillingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClaimGenerator claims = new ClaimGenerator(Helper.ConnVal);
            int claims_processed = claims.CompileProfessionalBilling();

            if (claims_processed < 0)
            {
                MessageBox.Show("Error processing claims. No file generated.", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show($"File generated. {claims_processed} claims generated.");
            }

        }

        private void claimValidationRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            ClaimRuleEditorForm frm = new ClaimRuleEditorForm();

            frm.ShowDialog();
        }
    }
}