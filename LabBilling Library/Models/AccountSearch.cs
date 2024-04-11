using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("AccountSearchView")]
    [PrimaryKey("account", AutoIncrement = false)]
    public sealed class AccountSearch : IBaseEntity
    {
        [Column("account")]
        [ResultColumn]
        public string Account { get; set; }
        [Column("pat_name")]
        [ResultColumn]
        public string Name { get; set; }
        [Column("first_name")]
        [ResultColumn]
        public string FirstName { get; set; }
        [Column("last_name")]
        [ResultColumn]
        public string LastName { get; set; }
        [Column("middle_name")]
        [ResultColumn]
        public string MiddleName { get; set; }

        [Column("ssn")]
        [ResultColumn]
        public string SSN { get; set; }
        [Column("birthdate")]
        [ResultColumn]
        public DateTime? DateOfBirth { get; set; }
        [Column("sex")]
        [ResultColumn]
        public string Sex { get; set; }
        [Column("mri")]
        [ResultColumn]
        public string MRN { get; set; }
        [Column("HNE_NUMBER")]
        [ResultColumn]
        public string EMPINumber { get; set; }
        [Column("trans_date")]
        [ResultColumn]
        public DateTime? ServiceDate { get; set; }
        [Column("fin_code")]
        [ResultColumn]
        public string FinCode { get; set; }
        [Column("fin_type")]
        [ResultColumn]
        public string FinType { get; set; }
        [Column("ins_code")]
        [ResultColumn]
        public string PrimaryInsCode { get; set; }
        [Column("plan_nme")]
        [ResultColumn]
        public string PrimaryInsPlanName { get; set; }
        [Column("status")]
        [ResultColumn]
        public string Status { get; set; }
        [Column("cl_mnem")]
        [ResultColumn]
        public string ClientMnem { get; set; }
        [Column("last_validation_date")]
        [ResultColumn]
        public DateTime LastValidationDate { get; set; }

        [Column("validation_text")]        
        [ResultColumn]
        public string ValidationStatus { get; set; }

        [Column("Balance")]
        [ResultColumn]
        public double Balance { get; set; }
        [Column("ClientBalance")]
        [ResultColumn]
        public double ClientBalance { get; set; }
        [Column("ThirdPartyBalance")]
        [ResultColumn]
        public double ThirdPartyBalance { get; set; }

        [Column("TotalCharges")]
        [ResultColumn]
        public double TotalCharges { get; set; }

        [Column("TotalPayments")]
        [ResultColumn]
        public double TotalPayments { get; set; }
        
        [Ignore]
        public DateTime UpdatedDate { get; set; }
        [Ignore]
        public string UpdatedUser { get; set; }
        [Ignore]
        public string UpdatedApp { get; set; }
        [Ignore]
        public string UpdatedHost { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
