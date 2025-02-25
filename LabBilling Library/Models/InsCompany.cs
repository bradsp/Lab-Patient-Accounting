﻿using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.Models;

[TableName("insc")]
[PrimaryKey("code", AutoIncrement = false)]
public sealed class InsCompany : IBaseEntity
{

    [Column("code")]
    public string InsuranceCode { get; set; }

    [Column("name")]
    public string PlanName { get; set; }

    [Column("addr1")]
    public string Address1 { get; set; }

    [Column("addr2")]
    public string Address2 { get; set; }

    [Column("citystzip")]
    public string CityStateZip { get; set; }
    [Column("city")]
    public string City { get; set; }
    [Column("state")]
    public string State { get; set; }
    [Column("zipcode")]
    public string Zip { get; set; }
    [Column("provider_no_qualifier")]
    public string ProviderNoQualifer { get; set; }

    [Column("provider_no")]
    public string ProviderNo { get; set; }

    [Column("payer_no")]
    public string PayerNo { get; set; }

    [Column("claimsnet_payer_id")]
    public string ClaimsNetPayerId { get; set; }

    [Column("bill_form")]
    public string BillForm { get; set; }

    [Column("fin_code")]
    public string FinancialCode { get; set; }

    [Column("comment")]
    public string Comment { get; set; }

    [Column("is_mc_hmo")]
    public bool IsMedicareHmo { get; set; }

    [Column("allow_outpatient_billing")]
    public bool AllowOutpatientBilling { get; set; }

    [Column("payor_code")]
    public string PayorCode { get; set; }

    [Column("nthrive_payer_no")]
    public string NThrivePayerNo { get; set; }

    [Column("is_generic")]
    public bool IsGenericPayor { get; set; }

    [Ignore]
    public string FinancialClass
    {
        get
        {
            return this.FinancialCode;
        }
    }

    [Column("bill_as_jmcgh")]
    public bool BillAsJmcgh { get; set; }

    [Column("claim_filing_indicator_code")]
    public string ClaimFilingIndicatorCode { get; set; }

    [Ignore]
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
    public List<Mapping> Mappings { get; set; } = new List<Mapping>();

}
