using System.Collections.Generic;

namespace LabBilling.Core.Services
{
    public class Loop2000
    {
        public string TotalClaimCount { get; set; }
        public string TotalClaimChargeAmount { get; set; }
        public string TotalHCPCSReportedChargeAmount { get; set; }
        public string TotalHCPCSPayableAmount { get; set; }

        public List<Loop2100> Loop2100s { get; set; } = new List<Loop2100>();
    }

}
