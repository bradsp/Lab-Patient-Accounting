using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using RFClassLibrary;

namespace LabBilling.Forms
{
    public partial class PatientCollectionsRunWizard : Form
    {
        private PatientBilling patientBilling = new PatientBilling(Program.AppEnvironment);
        private BadDebtRepository badDebtRepository = new BadDebtRepository(Program.AppEnvironment);
        private SystemParametersRepository parametersRepository = new SystemParametersRepository(Program.AppEnvironment);
        private bool errorEncountered = false;
        private DateTime thruDate;
        private string batchNo;

        public PatientCollectionsRunWizard()
        {
            InitializeComponent();
        }

        private DialogResult AskError(string errorText)
        {
            var result = MessageBox.Show(errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return result;
        }

        private async void sendToCollectionsStartButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            try
            {
                sendToCollectionsProgressBar.Style = ProgressBarStyle.Continuous;
                sendToCollectionsProgressBar.Value = 0;
                sendToCollectionsProgressBar.Maximum = 100;
                patientBilling.ProgressIncrementedEvent += PatientBilling_ProgressIncrementedEvent;

                await patientBilling.SendToCollections();

                //patientBilling.ProgressIncementedEvent -= PatientBilling_ProgressIncementedEvent;

                compileStmtsStartButton.Enabled = true;

            }
            catch(ApplicationException apex)
            {
                Log.Instance.Error(apex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void PatientBilling_ProgressIncrementedEvent(object sender, ProgressEventArgs e)
        {
            if (sendToCollectionsProgressBar.InvokeRequired)
            {
                sendToCollectionsProgressBar.Invoke(new Action(() =>
                {
                    sendToCollectionsProgressBar.Value = e.PercentComplete;
                    sendToCollectionsTextbox.Text = e.Status;
                }));
            }
            else
            {
                sendToCollectionsProgressBar.Value = e.PercentComplete;
                sendToCollectionsTextbox.Text = e.Status;
            }
        }

        private void createStmtFileStartButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            try
            {
                createStmtFileProgressBar.Style = ProgressBarStyle.Marquee;
                createStmtFileTextBox.Text = "Creating statement file.";
                patientBilling.CreateStatementFile(DateTimeHelper.GetLastDayOfPrevMonth());

                createStmtFileProgressBar.Style = ProgressBarStyle.Continuous;
                createStmtFileProgressBar.Value = 100;
                createStmtFileProgressBar.Maximum = 100;
                createStmtFileTextBox.Text = "File created.";
            }
            catch (ApplicationException apex)
            {
                Log.Instance.Error(apex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void compileStmtsStartButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            try
            {
                compileStatementsProgressBar.Style = ProgressBarStyle.Marquee;
                patientBilling.CompileStatements(DateTimeHelper.GetLastDayOfPrevMonth());
                compileStatementsProgressBar.Style = ProgressBarStyle.Continuous;
                compileStatementsProgressBar.Value = 100;
                compileStatementsProgressBar.Maximum = 100;

                createStmtFileStartButton.Enabled = true;
            }
            catch(ArgumentException argex)
            {
                Log.Instance.Error(argex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch(ApplicationException apex)
            {
                Log.Instance.Error(apex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }

        private void PatientCollectionsRunWizard_Load(object sender, EventArgs e)
        {
            var records = badDebtRepository.GetNotSentRecords();

            sendToCollectionsTextbox.Text = $"{records.Count()} records to send to collections.";

            thruDate = DateTimeHelper.GetLastDayOfPrevMonth();

            batchNo = $"{thruDate.Year}{thruDate.Month:00}";

            batchNoLabel.Text = batchNo;
            throughDateLabel.Text = thruDate.ToShortDateString();

            //check of patient bills have already been run for this month
            if(patientBilling.BatchPreviouslyRun(batchNo))
            {
                bannerLabel.Text = $"Batch {batchNo} has already been run.";
                bannerLabel.ForeColor = Color.Red;
                sendToCollectionsStartButton.Enabled = false;
            }
            else
            {
                bannerLabel.Visible = false;
                sendToCollectionsStartButton.Enabled = true;
            }

            compileStmtsStartButton.Enabled = true;
            createStmtFileStartButton.Enabled = true;
           
        }

        private void PatientCollectionsRunWizard_HelpRequested(object sender, HelpEventArgs hlpevent)
        {

        }

        private void PatientCollectionsRunWizard_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            //string url = parametersRepository.GetByKey("documentation_site_url");
            string url = Program.AppEnvironment.ApplicationParameters.DocumentationSiteUrl;
            //string topicPath = parametersRepository.GetByKey("patient_statements_url");
            string topicPath = Program.AppEnvironment.ApplicationParameters.PatientStatementsUrl;

            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(topicPath))
            {
                System.Diagnostics.Process.Start($"{url}/{topicPath}");
            }
            else
            {
                MessageBox.Show("Documentation parameters not set. Cannot launch documentation.");
            }
        }
    }
}
