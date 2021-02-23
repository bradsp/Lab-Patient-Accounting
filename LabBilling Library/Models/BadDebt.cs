using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Models
{
    [TableName("bad_debt")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class BadDebt : IBaseEntity
    {
        public Guid rowguid { get; set; }
        public bool deleted { get; set; }
        public string debtor_last_name { get; set; }
        public string debtor_first_name { get; set; }
        public string st_addr_1 { get; set; }
        public string st_addr_2 { get; set; }
        public string city { get; set; }
        public string state_zip { get; set; }
        public string spouse { get; set; }
        public string phone { get; set; }
        public string soc_security { get; set; }
        public string license_number { get; set; }
        public string employment { get; set; }
        public string remarks { get; set; }
        public string account_no { get; set; }
        public string patient_name { get; set; }
        public string remarks2 { get; set; }
        public string misc { get; set; }
        public DateTime? service_date { get; set; }
        public DateTime? payment_date { get; set; }
        public double balance { get; set; }
        public DateTime? date_entered { get; set; }
        public DateTime? date_sent { get; set; }
        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }

        [Ignore]
        public string State { get; set; }
        [Ignore]
        public string Zip { get; set; }

        [Ignore]
        public Pat Pat { get; set; } = new Pat();
    }
}
