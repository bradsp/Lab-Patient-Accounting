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

        [Column("batch")]
        public double Batch { get; set; }

        [Column("run_date")]
        public DateTime RunDate { get; set; }

        [Column("run_user")]
        public string RunUser { get; set; }

        [Column("x12_text")]
        public string X12Text { get; set; }

        [Column("claim_count")]
        public int ClaimCount { get; set; }

        [Column("TotalBilled")]
        public double TotalBilled { get; set; }

        [Column("BatchType")]
        public string BatchType { get; set; }

        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        public Guid rowguid { get; set; }

        [Ignore]
        public List<BillingActivity> BillingActivities { get; set; } = new List<BillingActivity>();
    }
}
