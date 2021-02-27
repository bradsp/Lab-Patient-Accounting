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
using LabBilling.Views;

namespace LabBilling.Forms
{
    public partial class PhysicianMaintenanceForm : Form, IPhysicianMaintenanceView
    {
        public PhysicianMaintenanceForm()
        {
            InitializeComponent();
        }

        public string Upin { get; set; }
        public string UP_upin { get; set; }
        public string TNH_num { get; set; }
        public string BillingNPI { get; set; }
        public string pc_code { get; set; }
        public string Client { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MidInit { get; set; }
        public string Group { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string phone { get; set; }
        public int LabelCount { get; set; }
        public double uri { get; set; }
        public string AliasMnem { get; set; }
        public string Credentials { get; set; }
        public string OVcode { get; set; }
        public string DocNo { get; set; }
        public string Pathologist { get; set; }

        public event EventHandler NewProvider;
        public event EventHandler ProviderSelected;
        public event EventHandler SaveProvider;

        private readonly PhyRepository phydb = new PhyRepository(Helper.ConnVal);

        private void PhysicianMaintenanceForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");

            List<Phy> physicians = new List<Phy>();

            //physicians = phydb.GetAll();

            PhysicianDGV.DataSource = phydb.GetAll();

            PhysicianDGV.Columns["rowguid"].Visible = false;
            PhysicianDGV.Columns["reserved"].Visible = false;
            PhysicianDGV.Columns["mod_user"].Visible = false;
            PhysicianDGV.Columns["mod_date"].Visible = false;
            PhysicianDGV.Columns["mod_prg"].Visible = false;
            PhysicianDGV.Columns["mod_host"].Visible = false;

            PhysicianDGV.AutoResizeColumns();
        }
    }
}
