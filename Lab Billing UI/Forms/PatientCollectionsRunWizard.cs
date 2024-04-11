using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System.ComponentModel;
using System.IO;
using Utilities;

namespace LabBilling.Forms;

public partial class PatientCollectionsRunWizard : Form
{
    private PatientBillingService _patientBillingService = new(Program.AppEnvironment);
    private DateTime _thruDate;
    private string _batchNo;
    private bool _errorEncountered = false;

    public PatientCollectionsRunWizard()
    {
        InitializeComponent();
    }

    private static DialogResult AskError(string errorText)
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
            sendToCollectionsProgressBar.Minimum = 0;
            sendToCollectionsProgressBar.Maximum = 100;
            _patientBillingService.ProgressIncrementedEvent += PatientBilling_ProgressIncrementedEvent;

            string filename = await _patientBillingService.SendToCollectionsAsync();

            //send collections file
            SFTP.UploadSftp(filename, Program.AppEnvironment.ApplicationParameters.CollectionsSftpUploadPath + '/' + Path.GetFileName(filename),
                Program.AppEnvironment.ApplicationParameters.CollectionsSftpServer, 22,
                Program.AppEnvironment.ApplicationParameters.CollectionsSftpUsername,
                Program.AppEnvironment.ApplicationParameters.CollectionsSftpPassword);

            sendToCollectionsTextbox.Text += $"{filename} uploaded to {Program.AppEnvironment.ApplicationParameters.CollectionsSftpServer}\n";
            Log.Instance.Debug($"{filename} uploaded to {Program.AppEnvironment.ApplicationParameters.CollectionsSftpServer}");

            compileStmtsStartButton.Enabled = true;
            nextButton.Enabled = true;
        }
        catch (ApplicationException apex)
        {
            Log.Instance.Error(apex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
            DialogResult = DialogResult.Cancel;
            return;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
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
                sendToCollectionsTextbox.AppendText(Environment.NewLine + e.Status);
            }));
        }
        else
        {
            sendToCollectionsProgressBar.Value = e.PercentComplete;
            sendToCollectionsTextbox.AppendText(Environment.NewLine + e.Status);
        }
    }

    private void createStmtFileStartButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace("Entering");
        try
        {
            createStmtFileProgressBar.Style = ProgressBarStyle.Marquee;
            createStmtFileTextBox.Text = "Creating statement file.";
            var filename = _patientBillingService.CreateStatementFile(DateTimeHelper.GetLastDayOfPrevMonth());

            createStmtFileProgressBar.Style = ProgressBarStyle.Continuous;
            createStmtFileProgressBar.Value = 100;
            createStmtFileProgressBar.Maximum = 100;
            createStmtFileTextBox.Text = "File created." + Environment.NewLine;

            //send statement file
            SFTP.UploadSftp(filename, Program.AppEnvironment.ApplicationParameters.StatementsSftpUploadPath + '/' + Path.GetFileName(filename),
                Program.AppEnvironment.ApplicationParameters.StatementsSftpServer, 22,
                Program.AppEnvironment.ApplicationParameters.StatementsSftpUsername,
                Program.AppEnvironment.ApplicationParameters.StatementsSftpPassword);

            createStmtFileTextBox.Text += $"{filename} uploaded to {Program.AppEnvironment.ApplicationParameters.StatementsSftpServer}";
            Log.Instance.Debug($"{filename} uploaded to {Program.AppEnvironment.ApplicationParameters.StatementsSftpServer}");
        }
        catch (ApplicationException apex)
        {
            Log.Instance.Error(apex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
            DialogResult = DialogResult.Cancel;
            return;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
            DialogResult = DialogResult.Cancel;
            return;
        }
    }

    private async void compileStmtsStartButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace("Entering");
        try
        {
            compileStatementsProgressBar.Style = ProgressBarStyle.Continuous;
            compileStatementsProgressBar.Value = 0;

            //await patientBilling.CompileStatementsAsync(DateTimeHelper.GetLastDayOfPrevMonth());
            await _patientBillingService.CompileStatementsNewAsync(DateTimeHelper.GetLastDayOfPrevMonth());

            compileStatementsProgressBar.Style = ProgressBarStyle.Continuous;
            compileStatementsProgressBar.Value = 100;
            compileStatementsProgressBar.Maximum = 100;

            nextButton.Enabled = true;
            createStmtFileStartButton.Enabled = true;
        }
        catch (ArgumentException argex)
        {
            Log.Instance.Error(argex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
            DialogResult = DialogResult.Cancel;
            return;
        }
        catch (ApplicationException apex)
        {
            Log.Instance.Error(apex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
            DialogResult = DialogResult.Cancel;
            return;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
            _errorEncountered = true;
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
        using UnitOfWorkMain unitOfWork = new(Program.AppEnvironment);
        var records = unitOfWork.BadDebtRepository.GetNotSentRecords();

        sendToCollectionsTextbox.Text = $"{records.Count()} records to send to collections.";


        _thruDate = DateTimeHelper.GetLastDayOfPrevMonth();

        _batchNo = $"{_thruDate.Year}{_thruDate.Month:00}";

        batchNoLabel.Text = _batchNo;
        throughDateLabel.Text = _thruDate.ToShortDateString();

        //check of patient bills have already been run for this month
        if (_patientBillingService.BatchPreviouslyRun(_batchNo))
        {
            bannerLabel.Text = $"Batch {_batchNo} has already been run.";
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
        nextButton.Enabled = true;

        if (!records.Any())
        {
            skipSendCollectionsCheckBox.Checked = true;
        }
    }

    private void PatientCollectionsRunWizard_HelpRequested(object sender, HelpEventArgs hlpevent)
    {

    }

    private void PatientCollectionsRunWizard_HelpButtonClicked(object sender, CancelEventArgs e)
    {
        string url = Program.AppEnvironment.ApplicationParameters.DocumentationSiteUrl;
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

    private void nextButton_Click(object sender, EventArgs e)
    {
        tabControl1.SelectedIndex =
        (tabControl1.SelectedIndex + 1) % tabControl1.TabCount;

    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tabControl1.TabPages[tabControl1.SelectedIndex].Name == sendCollectionsTabPage.Name)
        {
            sendToCollectionsStartButton.Enabled = !skipSendCollectionsCheckBox.Checked;
            nextButton.Enabled = skipSendCollectionsCheckBox.Checked;
        }
        else if (tabControl1.SelectedIndex == 0)
        {
            nextButton.Enabled = true;
        }
        else
        {
            nextButton.Enabled = false;
        }
    }

    private void skipSendCollectionsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        sendToCollectionsStartButton.Enabled = !skipSendCollectionsCheckBox.Checked;
        nextButton.Enabled = skipSendCollectionsCheckBox.Checked;
    }
}
