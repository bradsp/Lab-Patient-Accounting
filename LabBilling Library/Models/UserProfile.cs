using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("UserProfile")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class UserProfile : IBaseEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Parameter { get; set; }
        public string ParameterData { get; set; }   
        
        [ResultColumn]
        public DateTime? ModDate { get; set; }
        [ResultColumn]
        public string ModUser { get; set; }
        [ResultColumn]
        public string ModPrg { get; set; }
        [ResultColumn]
        public string ModHost { get; set; }

        [Ignore]
        public DateTime? mod_date { get { return ModDate; } set { ModDate = value; } }
        [Ignore]
        public string mod_user { get { return ModUser; } set { ModUser = value; } }
        [Ignore]
        public string mod_prg { get { return ModPrg; } set { ModPrg = value; } }
        [Ignore]
        public string mod_host { get { return ModHost; } set { ModHost = value; } }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
