using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("data_billing_history")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class BillingActivity : IBaseEntity
    {
        public Guid rowguid { get; set; }
        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("account")]
        public string AccountNo { get; set; }

        [Column("ins_abc")]
        public string Coverage { get; set; }

        [Column("pat_name")]
        public string PatientName { get; set; }

        [Column("fin_code")]
        public string FinCode { get; set; }

        [Column("ins_code")]
        public string InsCode { get; set; }

        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }

        [Column("run_date")]
        public DateTime RunDate { get; set; }

        [Column("printed")]
        public bool IsPrinted { get; set; }

        [Column("run_user")]
        public string Runuser { get; set; }

        [Column("batch")]
        public double Batch { get; set; }

        [Column("ebill_status")]
        public string EBillStatus { get; set; }

        [Column("ebill_batch")]
        public double EBillBatch { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("ins_complete")]
        public DateTime IsInsComplete { get; set; }
        
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }

    }
}
