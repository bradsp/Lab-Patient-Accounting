using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("pat")]
    [PrimaryKey("account",AutoIncrement = false)]
    public class Pat : IBaseEntity
    {
        [Column("deleted")]
        public bool IsDeleted { get; set; }
        [Column("account")]
        public string AccountNo { get; set; }
        [Column("ssn")]
        public string SocSecNo { get; set; }
        [Column("hne_epi_number")]
        public string EMPIId { get; set; }
        [Column("pat_full_name")]
        public string PatFullName { get; set; }
        [Ignore]
        public string PatFirstName { get; set; }
        [Ignore]
        public string PatMiddleName { get; set; }
        [Ignore]
        public string PatLastName { get; set; }
        [Ignore]
        public string PatNameSuffix { get; set; }

        [Column("dob_yyyy")]
        public DateTime? BirthDate { get; set; }
        [Column("sex")]
        public string Sex { get; set; }
        [Column("pat_marital")]
        public string MaritalStatus { get; set; }

        [Column("pat_addr1")]
        public string Address1 { get; set; }
        [Column("pat_addr2")]
        public string Address2 { get; set; }
        [Column("city_st_zip")]
        public string CityStateZip { get; set; }

        [Column("pat_city")]
        public string City { get; set; }
        [Column("pat_state")]
        public string State { get; set; }
        [Column("pat_zip")]
        public string ZipCode { get; set; }
        [Column("pat_race")]
        public string Race { get; set; }
        [Column("pat_phone")]
        public string PrimaryPhone { get; set; }
        [Column("pat_email")]
        public string EmailAddress { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("relation")]
        public string GuarRelationToPatient { get; set; }
        [Column("guarantor")]
        public string GuarantorFullName { get; set; }
        [Column("guar_addr")]
        public string GuarantorAddress { get; set; }
        [Column("g_city_st")]
        public string GuarantorCityState { get; set; }
        [Column("guar_city")]
        public string GuarantorCity { get; set; }
        [Column("guar_state")]
        public string GuarantorState { get; set; }
        [Column("guar_zip")]
        public string GuarantorZipCode { get; set; }
        [Column("guar_phone")]
        public string GuarantorPrimaryPhone { get; set; }
        [Ignore]
        public string GuarantorLastName { get; set; }
        [Ignore]
        public string GuarantorFirstName { get; set; }
        [Ignore]
        public string GuarantorMiddleName { get; set; }
        [Ignore]
        public string GuarantorNameSuffix { get; set; }

        [Column("icd9_1")]
        public string Dx1 { get; set; }
        [Column("icd9_2")]
        public string Dx2 { get; set; }
        [Column("icd9_3")]
        public string Dx3 { get; set; }
        [Column("icd9_4")]
        public string Dx4 { get; set; }
        [Column("icd9_5")]
        public string Dx5 { get; set; }
        [Column("icd9_6")]
        public string Dx6 { get; set; }
        [Column("icd9_7")]
        public string Dx7 { get; set; }
        [Column("icd9_8")]
        public string Dx8 { get; set; }
        [Column("icd9_9")]
        public string Dx9 { get; set; }
        [Column("icd_indicator")]
        public string ICDIndicator { get; set; }

        [Column("pc_code")]
        public string ProfessionalComponentCode { get; set; }
        [Column("phy_id")]
        public string ProviderId { get; set; }

        [Column("mailer")]
        public string StatementFlag { get; set; }
        [Column("first_dm")]
        public DateTime? FirstStatementDate { get; set; }
        [Column("last_dm")]
        public DateTime? LastStatementDate { get; set; }
        [Column("min_amt")]
        public double MinimumPaymentAmount { get; set; }

        [Column("dbill_date")]
        public DateTime? DetailBillDate { get; set; }
        [Column("ub_date")]
        public DateTime? InstitutionalClaimDate { get; set; }
        [Column("h1500_date")]
        public DateTime? ProfessionalClaimDate { get; set; }
        [Column("ssi_batch")]
        public string SSIBatch { get; set; }
        [Column("colltr_date")]
        public DateTime? CollectionLetterDate { get; set; }
        [Column("baddebt_date")]
        public DateTime? SentToCollectionsDate { get; set; }
        [Column("batch_date")]
        public DateTime? BatchDate { get; set; }
        [Column("bd_list_date")]
        public DateTime? BadDebtListDate { get; set; }
        [Column("ebill_batch_date")]
        public DateTime? EBillBatchDate { get; set; }
        [Column("ebill_batch_1500")]
        public DateTime? EbillBatch1500 { get; set; }
        [Column("e_ub_demand")]
        public bool EUBDemand { get; set; }
        [Column("e_ub_demand_date")]
        public DateTime? EUBDemandDate { get; set; }
        [Column("claimsnet_1500_batch_date")]
        public DateTime? Claimsnet1500BatchDate { get; set; }
        [Column("claimsnet_ub_batch_date")]
        public DateTime? ClaimsnetUbBatchDate { get; set; }

        [Column("phy_comment")]
        public string ProviderComment { get; set; }
        [Column("dx_update_prg")]
        public string DxUpdatePrg { get; set; }

        [ResultColumn]
        public DateTime mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]
        public string mod_host { get; set; }


        [Ignore]
        public string DOBSex => $"{ BirthDate.GetValueOrDefault().ToShortDateString() } - { Sex }";

        [Ignore]
        public string AddressLine => $"{ Address1 }, { Address2 }, { CityStateZip }";

        [Ignore]
        public string Dx1Desc { get; set; }
        [Ignore]
        public string Dx2Desc { get; set; }
        [Ignore]
        public string Dx3Desc { get; set; }
        [Ignore]
        public string Dx4Desc { get; set; }
        [Ignore]
        public string Dx5Desc { get; set; }
        [Ignore]
        public string Dx6Desc { get; set; }
        [Ignore]
        public string Dx7Desc { get; set; }
        [Ignore]
        public string Dx8Desc { get; set; }
        [Ignore]
        public string Dx9Desc { get; set; }

        [Ignore]
        public List<PatDiag> Diagnoses { get; set; } = new List<PatDiag>();
        [Ignore]
        public Phy Physician { get; set; } = new Phy();
        [Ignore]
        public Guid rowguid { get; set; }

    }

    public class PatDiag
    {
        public int No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
