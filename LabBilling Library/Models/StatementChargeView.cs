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
