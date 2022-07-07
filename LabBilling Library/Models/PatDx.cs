using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [PetaPoco.TableName("patdx")]
    [PrimaryKey("uid", AutoIncrement = true)]
    public class PatDx : IBaseEntity
    {
        public bool deleted { get; set; }
        public string account { get; set; }
        public int dx_number { get; set; }
        public string diagnosis { get; set; }
        public string version { get; set; }
        public string code_qualifier { get; set; }
        public bool is_error { get; set; }
        public string import_file { get; set; }
        public DateTime mod_date { get; set; }
        public string mod_prg { get; set; }
        public string mod_user { get; set; }
        public string mod_host { get; set; }
        public DateTime? posted_date { get; set; }
        public int uid { get; set; }

        [Ignore]
        public string DxDescription { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
