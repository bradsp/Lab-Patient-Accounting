using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("data_billing_batch")]
    [PrimaryKey("batch", AutoIncrement = false)]
    public class BillingBatch : IBaseEntity
    {
        public double batch { get; set; }
        public DateTime run_date { get; set; }
        public string run_user { get; set; }
        public string x12_text { get; set; }
        public int claim_count { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        public Guid rowguid { get; set; }
    }
}
