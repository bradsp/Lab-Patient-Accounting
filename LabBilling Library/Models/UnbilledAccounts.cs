using System;
using PetaPoco;


namespace LabBilling.Core.Models
{
    [TableName("vw_cbill_select")]
    public sealed class UnbilledAccounts
    {

        [Column("cl_mnem")]
        public string ClientMnem { get; set; }

        [Column("account")]
        public string Account { get; set; }

        [Column("trans_date")]
        public DateTime TransactionDate { get; set; }

        [Column("pat_name")]
        public string PatientName { get; set; }

        [Column("fin_code")]
        public string FinancialClass { get; set; }

        [Column("UnbilledAmount")]
        public double UnbilledAmount { get; set; }

    }

}
