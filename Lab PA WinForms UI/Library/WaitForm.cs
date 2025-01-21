using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Library;

public partial class WaitForm : Form
{
    public Action Worker { get; set; }
    public ProgressBarStyle ProgressBarStyle
    {
        get
        {
            return progressBar1.Style;
        }
        set
        {
            progressBar1.Style = value;
        }
    }

    public WaitForm()
    {
        InitializeComponent();
    }

    public WaitForm(Action worker)
    {
        InitializeComponent();
        Worker = worker;
    }

    protected override void OnLoad(EventArgs e)
    {

        base.OnLoad(e);
        if (Worker != null)
            Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void UpdateProgress(int progress, string statusText = "Processing ... ")
    {
        progressBar1.Value = progress;
        statusLabel.Text = statusText;
    }
    
}
