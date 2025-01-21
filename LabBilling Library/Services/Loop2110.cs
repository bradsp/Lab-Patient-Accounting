using System.Collections.Generic;

namespace LabBilling.Core.Services
{
    public class Loop2110
    {
        public string ProcedureCode { get; set; }
        public string LineItemChargeAmount { get; set; }
        public string MonetaryAmount { get; set; }
        public string RevenueCode { get; set; }
        public string PaidAmount { get; set; }
        public string AllowedAmount { get; set; }

        public List<Loop2110Adj> Adjustments { get; set; } = new List<Loop2110Adj>();
    }




}
