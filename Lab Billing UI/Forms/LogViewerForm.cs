using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;

namespace LabBilling.Forms
{
    public partial class LogViewerForm : Form
    {
        public LogViewerForm()
        {
            InitializeComponent();
        }

        LogRepository logRepository = new LogRepository(Helper.LogConnVal);
        BindingSource bindingSource = new BindingSource();

        private void LogViewerForm_Load(object sender, EventArgs e)
        {
            DateTime fromDate = DateTime.Now.AddDays(-1);
            DateTime thruDate = DateTime.Now;

            bindingSource.DataSource = Helper.ConvertToDataTable(logRepository.GetDateRange(fromDate, thruDate));

            logViewDataGrid.DataSource = bindingSource.DataSource;

        }
    }
}
