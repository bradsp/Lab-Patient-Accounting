using LabBilling.Forms;
using System;
using System.Threading.Tasks;

namespace LabBilling.Library
{
    public partial class WaitForm : BaseForm
    {
        public Action Worker { get; set; }

        public WaitForm(Action worker)
        {
            InitializeComponent();
            Worker = worker ?? throw new ArgumentNullException();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
