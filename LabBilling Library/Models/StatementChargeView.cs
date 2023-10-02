using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("StatementChargeView")]
    public class StatementChargeView : IBaseEntity
    {

        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }


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
