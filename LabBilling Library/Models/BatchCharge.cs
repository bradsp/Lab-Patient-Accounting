using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    public class BatchCharge
    {
        public string AccountNo { get; set; }
        public string CDM { get; set; }
        public string ChargeDescription { get; set; }
        public int Qty { get; set; }
        public float Amount { get; set; }
        public string Comment { get; set; }
    }
}
