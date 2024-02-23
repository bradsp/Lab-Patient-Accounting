using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Library;

public partial class WaitForm : Utilities.BaseForm
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

    public WaitForm() : base(Program.AppEnvironment)
    {
        InitializeComponent();
    }

    public WaitForm(Action worker) : base(Program.AppEnvironment)
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
