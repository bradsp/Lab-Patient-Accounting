using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("ins")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class Ins : IBaseEntity
    {
        [Column("account")]
        public string Account { get; set; }
        [Column("ins_a_b_c")]
        public string Coverage { get; set; }
        [Column("holder_nme")]
        public string HolderName { get; set; }
        [Column("holder_addr")]
        public string HolderAddress { get; set; }
        [Column("holder_city_st_zip")]
        public string HolderCityStZip { get; set; }
        [Column("holder_sex")]
        public string HolderSex { get; set; }
        [Column("holder_dob")]
        public DateTime? HolderBirthDate { get; set; }
        [Column("plan_nme")]
        public string PlanName { get; set; }
        [Column("plan_addr1")]
        public string PlanAddress1 { get; set; }
        [Column("plan_addr2")]
        public string PlanAddress2 { get; set; }
        [Column("p_city_st")]
        public string PlanCityState { get; set; }
        [Column("policy_num")]
        public string PolicyNumber { get; set; }
        [Column("cert_ssn")]
        public string CertSSN { get; set; }
        [Column("grp_nme")]
        public string GroupName { get; set; }
        [Column("grp_num")]
        public string GroupNumber { get; set; }
        [Column("employer")]
        public string Employer { get; set; }
        [Column("e_city_st")]
        public string EmployerCityState { get; set; }
        [Column("fin_code")]
        public string FinCode { get; set; }
        [Column("ins_code")]
        public string InsCode { get; set; }
        [Column("relation")]
        public string Relation { get; set; }
        [Column("holder_lname")]
        public string HolderLastName { get; set; }
        [Column("holder_fname")]
        public string HolderFirstName { get; set; }
        [Column("holder_mname")]
        public string HolderMiddleName { get; set; }
        [Column("plan_effective_date")]
        public DateTime? PlanEffectiveDate { get; set; }
        [Column("plan_expiration_date")]
        public DateTime? PlanExpirationDate { get; set; }
        
        public Guid rowguid { get; set; }
        public bool deleted { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }

        [Ignore]
        public InsCompany InsCompany { get; set; }

    }
}
