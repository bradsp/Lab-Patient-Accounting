using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBilling.Forms
{
    public partial class HealthPlanMaintenanceForm : Form
    {
        InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        

        public HealthPlanMaintenanceForm()
        {
            InitializeComponent();
        }

        private void HealthPlanMaintenanceForm_Load(object sender, EventArgs e)
        {
            var insCompanies = insCompanyRepository.GetAll();

            
        }
    }
}
