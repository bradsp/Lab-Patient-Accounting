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
        public string PatientName { get; set; }
        public string SocSecNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Client { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Comment { get; set; }
        public string CDM { get; set; }
        public int Qty { get; set; }
        public float Amount { get; set; }

    }
}
