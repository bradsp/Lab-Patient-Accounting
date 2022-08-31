using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetaPoco;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using System.Windows.Forms.DataVisualization.Charting;

namespace LabBilling.Forms
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadChart();
        }            


        private void LoadChart()
        {
            chart1.Series.Clear();
            chart1.Series.Add("A/R Balance");
            ReportingRepository reportingRepository = new ReportingRepository(Helper.ConnVal);

            var data = reportingRepository.GetARByFinCode();
            chart1.DataSource = data;
            chart1.Series["A/R Balance"].XValueMember = "Financial Class";
            chart1.Series["A/R Balance"].YValueMembers = "Balance";

            //chart1.Series["A/R Balance"].XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;

            chart1.Titles.Add("A/R Balance by Financial Class");
            chart1.Dock = DockStyle.Fill;
            dashboardLayoutPanel.Controls.Add(chart1);


        }
    }
}
