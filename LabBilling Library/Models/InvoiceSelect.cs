using System;
using PetaPoco;


namespace LabBilling.Core.Models
{
    [TableName("vw_cbill_select")]
    public sealed class InvoiceSelect : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("cl_mnem")]
        public string ClientMnem { get; set; }
        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }
        [Column("pat_name")]
        public string PatientName { get; set; }
        [Column("fin_code")]
        public string FinCode { get; set; }

        [Ignore]
        public DateTime mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }

}
