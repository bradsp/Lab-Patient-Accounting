using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("Address")]
    [PrimaryKey("AddressId", AutoIncrement = true)]
    public sealed class Address : IBaseEntity
    {
        public int AddressId { get; set; }
        public AddressType AddressType { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }


        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }

    }

    public enum AddressType
    {
        Home = 0,
        Work = 1,
        Billing = 2,
        Other = 3
    }
}
