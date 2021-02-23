using LabBilling.DataAccess;
using LabBilling.Logging;
using LabBilling.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class SystemParametersForm : Form
    {
        public SystemParametersForm()
        {
            InitializeComponent();
        }

        private readonly SystemParametersRepository paramsdb = new SystemParametersRepository(Helper.ConnVal);

        private void SystemParametersForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            List<SystemParameters> results = new List<SystemParameters>();

            //results = paramsdb.GetAll();

            SystemParmDGV.DataSource = paramsdb.GetAll();
            SystemParmDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }
    }
}
