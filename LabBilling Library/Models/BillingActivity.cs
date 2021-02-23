using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Models
{
    [TableName("vw_billing_activity")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class BillingActivity : IBaseEntity
    {
        public string BillType { get; set; }
        public string account { get; set; }
        public double batch { get; set; }
        public string fin_code { get; set; }
        public bool printed { get; set; }
        public double ebill_batch { get; set; }
        public string ebill_status { get; set; }
        public DateTime? run_date { get; set; }
        public string run_user { get; set; }
        public DateTime? trans_date { get; set; }
        public string claimsnet_payer_id { get; set; }
        public string text { get; set; }
        public Guid rowguid { get; set; }

        [Ignore]
        public DateTime? mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
    }
}
