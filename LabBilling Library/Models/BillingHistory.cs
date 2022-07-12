using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("data_billing_history")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class BillingHistory : IBaseEntity
    {

        public Guid rowguid { get; set; }
        public bool deleted { get; set; }
        public string account { get; set; }
        public string ins_abc { get; set; }
        public string pat_name { get; set; }
        public string fin_code { get; set; }
        public string ins_code { get; set; }
        public DateTime? trans_date { get; set; }
        public DateTime? run_date { get; set; }
        public bool printed { get; set; }
        public string run_user { get; set; }
        public double batch { get; set; }
        public string ebill_status { get; set; }
        public double ebill_batch { get; set; }
        public string text { get; set; }
        public DateTime? ins_complete { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }

    }
}
