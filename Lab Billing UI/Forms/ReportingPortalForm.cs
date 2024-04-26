using LabBilling.Logging;

namespace LabBilling.Forms;

public partial class ReportingPortalForm : Krypton.Toolkit.KryptonForm
{
    private string url;

    public ReportingPortalForm(string url)
    {
        this.url = url;
        InitializeComponent();
    }

    private void ReportingPortalForm_Load(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        try
        {
            kryptonWebBrowser1.Navigate(new Uri(url));
        }
        catch (System.UriFormatException ex)
        {
            Log.Instance.Error("report_portal_url is not formatted correctly.", ex);
            MessageBox.Show("Address is not formatted correctly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ReportingPortalForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        kryptonWebBrowser1.Dispose();
    }

}
