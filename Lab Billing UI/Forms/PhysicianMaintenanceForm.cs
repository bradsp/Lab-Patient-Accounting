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
using LabBilling.Core;
using LabBilling.Logging;

namespace LabBilling.Forms
{
    public partial class PhysicianMaintenanceForm : Form
    { 
        public PhysicianMaintenanceForm()
        {
            InitializeComponent();
        }

        private readonly PhyRepository phydb = new PhyRepository(Program.AppEnvironment);
        private List<Phy> physicians = new List<Phy>();

        private void PhysicianMaintenanceForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            //PhysicianDGV.DataSource = phydb.GetAll();
        }

        private void LoadProviderGrid()
        {
            PhysicianDGV.Columns["rowguid"].Visible = false;
            PhysicianDGV.Columns["reserved"].Visible = false;
            PhysicianDGV.Columns["mod_user"].Visible = false;
            PhysicianDGV.Columns["mod_date"].Visible = false;
            PhysicianDGV.Columns["mod_prg"].Visible = false;
            PhysicianDGV.Columns["mod_host"].Visible = false;

            PhysicianDGV.AutoResizeColumns();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            PhysicianDGV.DataSource = phydb.GetByName(searchText.Text, "");
            LoadProviderGrid();
        }
    }
}
