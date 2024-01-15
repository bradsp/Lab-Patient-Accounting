using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("InvoiceChargeView")]
    public class InvoiceChargeView : IBaseEntity
    {
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }
        [Column("qty")]
        public int Quantity { get; set; }
        [Column("inp_amt")]
        public double HospAmount { get; set; }
        [Column("retail")]
        public double RetailAmount { get; set; }
        [Column("amount")]
        public double Amount { get; set; }
        [Column("cdm")]
        public string ChargeItemId { get; set; }
        [Column("descript")]
        public string ChargeDescription { get; set; }
        [Column("cptList")]
        public string CptList { get; set; }
        [Column("fin_code")]
        public string FinCode { get; set; }
        [Column("fin_type")]
        public string FinancialType { get; set; }
        [Column("cl_mnem")]
        public string ClientMnem { get; set; }

        [Ignore]
        public DateTime UpdatedDate { get; set; }
        [Ignore]
        public string UpdatedUser { get; set; }
        [Ignore]
        public string UpdatedApp { get; set; }
        [Ignore]
        public string UpdatedHost { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
