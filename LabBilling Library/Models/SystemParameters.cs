using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("system")]
    [PrimaryKey("key_name",AutoIncrement = false)]
    public class SystemParameters : IBaseEntity
    {
        public string key_name { get; set; }
        public string value { get; set; }
        public string programs { get; set; }
        public string comment { get; set; }
        public string update_prg { get; set; }
        public string button { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string dataType { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }

    }
}
