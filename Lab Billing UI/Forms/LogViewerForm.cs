using LabBilling.Core.DataAccess;
using System;
using System.Windows.Forms;

namespace LabBilling.Forms;

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
