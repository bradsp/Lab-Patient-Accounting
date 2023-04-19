using System;

namespace LabBilling.Core.Models
{
    public sealed class ClientStatementDetailModel
    {
        public string Account { get; set; }
        public string Invoice { get; set; }
        public string Reference { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
    }
}
