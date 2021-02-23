using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Views
{
    interface IPhysicianMaintenanceView
    {
        string Upin { get; set; }
        string UP_upin { get; set; }
        string TNH_num { get; set; }
        string BillingNPI { get; set; }
        string pc_code { get; set; }
        string Client { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string MidInit { get; set; }
        string Group { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string ZipCode { get; set; }
        string phone { get; set; }
        int LabelCount { get; set; }
        double uri { get; set; }
        string AliasMnem { get; set; }
        string Credentials { get; set; }
        string OVcode { get; set; }
        string DocNo { get; set; }
        string Pathologist { get; set; }

        event EventHandler NewProvider;
        event EventHandler ProviderSelected;
        event EventHandler SaveProvider;        

    }

    class PhysicianMaintenanceView
    {
    }
}
