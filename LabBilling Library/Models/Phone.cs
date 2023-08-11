using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("phone")]
    [PrimaryKey("PhoneId", AutoIncrement = true)]
    public sealed class Phone : IBaseEntity
    {
        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType PhoneType { get; set; }


        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
    }

    public enum PhoneType
    {
        Home = 0,
        Mobile = 1,
        Work = 2,
        Other = 3
    }
}
