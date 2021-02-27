using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("acc")]
    [PrimaryKey("account", AutoIncrement = false)]
    public class AccountSearch : IBaseEntity
    {
        [Column("account")]
        public string Account { get; set; }
        [Column("pat_name")]
        public string Name { get; set; }
        public string SSN { get; set; }
        [Column("dob_yyyy")]
        public DateTime? DateOfBirth { get; set; }
        [Column("sex")]
        public string Sex { get; set; }
        [Column("mri")]
        public string MRN { get; set; }
        [Column("trans_date")]
        public DateTime? ServiceDate { get; set; }

        [Ignore]
        public DateTime? mod_date { get; set; }
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
