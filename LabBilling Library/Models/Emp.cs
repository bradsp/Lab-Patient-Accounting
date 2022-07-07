using System;
using PetaPoco;
using ColumnAttribute = PetaPoco.ColumnAttribute;

namespace LabBilling.Core.Models
{
    [PetaPoco.TableName("emp")]
    [PetaPoco.PrimaryKey("name", AutoIncrement=false)]
    [PetaPoco.ExplicitColumns]
    public class Emp : IBaseEntity
    {
        [PetaPoco.Column("name")]
        public string UserName { get; set; }
        [PetaPoco.Column("full_name")]
        public string FullName { get; set; }
        [PetaPoco.Column("access")]
        public string Access { get; set; }
        [PetaPoco.Column("access_edit_dictionary")]
        public bool CanEditDictionary { get; set; }
        [PetaPoco.Column("access_bad_debt")]
        public bool CanModifyBadDebt { get; set; }
        [PetaPoco.Column("access_billing")]
        public bool CanSubmitBilling { get; set; }
        [PetaPoco.Column("access_fin_code")]
        public bool CanModifyAccountFincode { get; set; }
        [PetaPoco.Column("add_chrg")]
        public bool CanSubmitCharges { get; set; }
        [PetaPoco.Column("add_chk")]
        public bool CanAddAdjustments { get; set; }
        [PetaPoco.Column("add_chk_amt")]
        public bool CanAddPayments { get; set; }
        [PetaPoco.Column("reserve4")]
        public bool IsAdministrator { get; set; }
        [PetaPoco.Column("reserve5")]
        public bool reserve5 { get; set; }
        [PetaPoco.Column("reserve6")]
        public bool reserve6 { get; set; }
        [PetaPoco.ResultColumn("mod_user")]
        public string LastModifiedBy { get; set; }
        [PetaPoco.ResultColumn("mod_prg")]
        public string LastModifiedWith { get; set; }
        [PetaPoco.ResultColumn("mod_date")]
        public DateTime LastModifiedDate { get; set; }
        [PetaPoco.Column("password")]
        public string Password { get; set; }
        //[Column(Name = "mainmenu")]
        //public string MainMenu { get; set; }

        [Ignore]
        public DateTime mod_date { get { return LastModifiedDate; } set { LastModifiedDate = value; } }
        [Ignore]
        public string mod_user { get { return LastModifiedBy;  } set { LastModifiedBy = value; } }
        [Ignore]
        public string mod_prg {  get { return LastModifiedWith; } set { LastModifiedWith = value; } }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
