using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class ProgressForm : BaseForm
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();

        public Action Worker { get; set; }
        public ProgressForm(Action worker)
        {
            InitializeComponent();
            if (worker == null)
                throw new ArgumentNullException();
            Worker = worker;

        }

        public MainForm _parentForm;
        public ProgressForm(MainForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;

            if(parentForm != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(parentForm.Location.X + parentForm.Width / 2 - this.Width / 2,
                    parentForm.Location.Y + parentForm.Height / 2 - this.Height / 2);

            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(Worker != null)
                Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            progressBar1.Maximum = _parentForm.progressReportModel.TotalRecords;
            progressBar1.Value = _parentForm.progressReportModel.RecordsProcessed >= 0 ? _parentForm.progressReportModel.RecordsProcessed : 0;

            StatusMessageLabel.Text = $"Processing {_parentForm.progressReportModel.RecordsProcessed} of {_parentForm.progressReportModel.TotalRecords}";

            if(_parentForm.progressReportModel.RecordsProcessed == _parentForm.progressReportModel.TotalRecords)
            {
                this.Close();
            }
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            if (_parentForm != null)
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                t.Interval = 1000;
                t.Tick += new EventHandler(OnTimedEvent);
                t.Start();
            }
        }
    }
}
