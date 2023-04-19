using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    public sealed class ProgressReportModel
    {
        public int PercentageComplete { get; set; } = 0;
        public int RecordsProcessed { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;
        public string StatusMessage { get; set; } = string.Empty;
    }
}
