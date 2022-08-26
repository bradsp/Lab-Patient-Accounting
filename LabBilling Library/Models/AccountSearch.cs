using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("AccountSearchView")]
    [PrimaryKey("account", AutoIncrement = false)]
    public class AccountSearch : IBaseEntity
    {
        [Column("account")]
        [ResultColumn]
        public string Account { get; set; }
        [Column("pat_name")]
        [ResultColumn]
        public string Name { get; set; }
        [Column("ssn")]
        [ResultColumn]
        public string SSN { get; set; }
        [Column("dob_yyyy")]
        [ResultColumn]
        public DateTime? DateOfBirth { get; set; }
        [Column("sex")]
        [ResultColumn]
        public string Sex { get; set; }
        [Column("mri")]
        [ResultColumn]
        public string MRN { get; set; }
        [Column("trans_date")]
        [ResultColumn]
        public DateTime? ServiceDate { get; set; }
        [Column("fin_code")]
        [ResultColumn]
        public string FinCode { get; set; }
        [Column("status")]
        [ResultColumn]
        public string Status { get; set; }

        [Ignore]
        public DateTime mod_date { get; set; }
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
