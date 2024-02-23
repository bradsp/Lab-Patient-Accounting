using System;
using System.Windows.Forms;
using LabBilling.Logging;

namespace LabBilling.Forms;

public partial class ReportingPortalForm : Utilities.BaseForm
{
    private string url;

    public ReportingPortalForm(string url) : base(Program.AppEnvironment)
    {
        this.url = url;
        InitializeComponent();
    }

    private void ReportingPortalForm_Load(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        //SystemParametersRepository systemParametersRepository = new SystemParametersRepository(this.connection);

        //string url = systemParametersRepository.GetByKey("report_portal_url");

        try
        {
            webBrowser1.Navigate(new Uri(url));
        }
        catch(System.UriFormatException ex)
        {
            Log.Instance.Error("report_portal_url is not formatted correctly.", ex);
            MessageBox.Show("Address is not formatted correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ReportingPortalForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        webBrowser1.Dispose();
    }

    private void homeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show("I clicked home!");
    }
}
