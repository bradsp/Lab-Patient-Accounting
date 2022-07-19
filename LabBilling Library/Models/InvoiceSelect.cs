using System;
using PetaPoco;


namespace LabBilling.Core.Models
{
    [TableName("vw_cbill_select")]
    public class InvoiceSelect : IBaseEntity
    {
        public string account { get; set; }
        public string cl_mnem { get; set; }
        public DateTime trans_date { get; set; }
        public string pat_name { get; set; }
        public string fin_code { get; set; }

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
