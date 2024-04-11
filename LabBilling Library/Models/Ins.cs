using PetaPoco;
using System;

namespace LabBilling.Core.Models;

[TableName("ins")]
[PrimaryKey("rowguid", AutoIncrement = false)]
public sealed class Ins : IBaseEntity
{
    [Column("account")]
    public string Account { get; set; }
    [Column("ins_a_b_c")]
    public string Coverage { get; set; }


    [Column("holder_lname")]
    public string HolderLastName { get; set; }
    [Column("holder_fname")]
    public string HolderFirstName { get; set; }
    [Column("holder_mname")]
    public string HolderMiddleName { get; set; }
    [Column("holder_nme")]
    public string HolderFullName { get; set; }
    [Column("holder_addr")]
    public string HolderStreetAddress { get; set; }

    [Column("holder_city_st_zip")]
    public string HolderCityStZip { get; set; }
    [Ignore]
    public string HolderAddress
    {
        get
        {
            if (string.IsNullOrEmpty(HolderStreetAddress) && string.IsNullOrEmpty(HolderCity) && string.IsNullOrEmpty(HolderState))
            {
                return "";
            }
            else
                return ($"{HolderStreetAddress}, {HolderCity}, {HolderState} {HolderZip}");
        }
    }

    [Column("holder_city")]
    public string HolderCity { get; set; }
    [Column("holder_state")]
    public string HolderState { get; set; }
    [Column("holder_zip")]
    public string HolderZip { get; set; }

    [Column("holder_sex")]
    public string HolderSex { get; set; }
    [Column("holder_dob")]
    public DateTime? HolderBirthDate { get; set; }

    [Column("ins_code")]
    public string InsCode { get; set; }
    [Column("plan_nme")]
    public string PlanName { get; set; }
    [Column("plan_addr1")]
    public string PlanStreetAddress1 { get; set; }
    [Column("plan_addr2")]
    public string PlanStreetAddress2 { get; set; }
    [Column("p_city_st")]
    public string PlanCityState { get; set; }
    [Ignore]
    public string PlanCity { get; set; }
    [Ignore]
    public string PlanState { get; set; }
    [Ignore]
    public string PlanZip { get; set; }
    [Ignore]
    public string PlanAddress
    {
        get
        {
            return ($"{PlanStreetAddress1} {PlanStreetAddress2}, {PlanCityState}");
        }
    }
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
    [Column("relation")]
    public string Relation { get; set; }

    [Column("plan_effective_date")]
    public DateTime? PlanEffectiveDate { get; set; }
    [Column("plan_expiration_date")]
    public DateTime? PlanExpirationDate { get; set; }

    public Guid rowguid { get; set; }

    [Column("deleted")]
    public bool IsDeleted { get; set; }

    [Column("mod_date")]
    [ResultColumn]
    public DateTime UpdatedDate { get; set; }
    [Column("mod_user")]
    [ResultColumn]
    public string UpdatedUser { get; set; }
    [Column("mod_prg")]
    [ResultColumn]
    public string UpdatedApp { get; set; }
    [Column("mod_host")]
    [ResultColumn]
    public string UpdatedHost { get; set; }


    [Ignore]
    public InsCompany InsCompany { get; set; }

}
