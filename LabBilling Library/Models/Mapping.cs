using PetaPoco;
using System;

namespace LabBilling.Core.Models
{
    [TableName("mapping")]
    [PrimaryKey("uid",AutoIncrement = true)]
    public class Mapping :IBaseEntity
    {
        public string return_value	{ get; set; }
        public string return_value_type { get; set; }
        public string sending_system { get; set; }
        public string sending_value { get; set; }
        public int uid  { get; set; }

        public DateTime? mod_date { get; set; }
        public string mod_prg	{ get; set; }
        public string mod_user	{ get; set; }
        public string mod_host	{ get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}
