using System;
using System.Collections.Generic;
using PetaPoco;

namespace LabBilling.Models
{
    [TableName("pat")]
    [PrimaryKey("account",AutoIncrement = false)]
    public class Pat : IBaseEntity
    {
        public Pat()
        {
            //Diagnoses = new List<PatDiag>();
        }

        public bool deleted { get; set; }
        public string account { get; set; }
        public string ssn { get; set; }
        public string pat_addr1 { get; set; }
        public string pat_addr2 { get; set; }
        public string city_st_zip { get; set; }
        public DateTime? dob_yyyy { get; set; }
        public string sex { get; set; }
        public string relation { get; set; }
        public string guarantor { get; set; }
        public string guar_addr { get; set; }
        public string g_city_st { get; set; }
        public string pat_marital { get; set; }
        public string icd9_1 { get; set; }
        public string icd9_2 { get; set; }
        public string icd9_3 { get; set; }
        public string icd9_4 { get; set; }
        public string icd9_5 { get; set; }
        public string icd9_6 { get; set; }
        public string icd9_7 { get; set; }
        public string icd9_8 { get; set; }
        public string icd9_9 { get; set; }
        public string icd_indicator { get; set; }
        public string pc_code { get; set; }
        public string mailer { get; set; }
        public DateTime? first_dm { get; set; }
        public DateTime? last_dm { get; set; }
        public double min_amt { get; set; }
        public string phy_id { get; set; }
        public DateTime? dbill_date { get; set; }
        public DateTime? ub_date { get; set; }
        public DateTime? h1500_date { get; set; }
        public string ssi_batch { get; set; }
        public DateTime? colltr_date { get; set; }
        public DateTime? baddebt_date { get; set; }
        public DateTime? batch_date { get; set; }
        public string guar_phone { get; set; }
        public DateTime? bd_list_date { get; set; }
        public DateTime? ebill_batch_date { get; set; }
        public DateTime? ebill_batch_1500 { get; set; }
        public bool e_ub_demand { get; set; }
        public DateTime? e_ub_demand_date { get; set; }
        public DateTime? claimsnet_1500_batch_date { get; set; }
        public DateTime? claimsnet_ub_batch_date { get; set; }
        public string mod_host { get; set; }
        public string hne_epi_number { get; set; }
        public string pat_full_name { get; set; }
        public string pat_city { get; set; }
        public string pat_state { get; set; }
        public string pat_zip { get; set; }
        public string guar_city { get; set; }
        public string guar_state { get; set; }
        public string guar_zip { get; set; }
        public string pat_race { get; set; }
        public string pat_phone { get; set; }
        public string phy_comment { get; set; }
        public string location { get; set; }
        public string pat_email { get; set; }
        public string dx_update_prg { get; set; }

        [ResultColumn]
        public DateTime? mod_date { get; set; }
        [ResultColumn]
        public string mod_user { get; set; }
        [ResultColumn]
        public string mod_prg { get; set; }
        [ResultColumn]

        [Ignore]
        public string DOBSex => $"{ dob_yyyy.GetValueOrDefault().ToShortDateString() } - { sex }";

        [Ignore]
        public string AddressLine => $"{ pat_addr1 }, { pat_addr2 }, { city_st_zip }";

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
        public Guid rowguid { get; set; }

    }

    public class PatDiag
    {
        public int No { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
