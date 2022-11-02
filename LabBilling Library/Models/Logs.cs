using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    /// <summary>
    /// This table is in the NLog database
    /// </summary>
    [TableName("Logs")]
    public class Logs
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Exception { get; set; }
        public string Logger { get; set; }
        public string HostName { get; set; }
        public string Username { get; set; }
        public string CallingSite { get; set; }
        public string CallingSiteLineNumber { get; set; }
        public string AppVersion { get; set; }
    }
}
