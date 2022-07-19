using System;
using PetaPoco;


namespace LabBilling.Core.Models
{
    [TableName("vw_cbill_select")]
    public class UnbilledAccounts
    {

        public string cl_mnem { get; set; }
        public string account { get; set; }
        public DateTime trans_date { get; set; }
        public string pat_name { get; set; }
        public string fin_code { get; set; }
        public double UnbilledAmount { get; set; }

    }

}
