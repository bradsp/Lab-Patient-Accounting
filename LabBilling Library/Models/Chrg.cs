using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.Models;

[TableName("chrg")]
[PrimaryKey("chrg_num", AutoIncrement = true)]
public sealed class Chrg : IBaseEntity
{
    [Column("credited")]
    public bool IsCredited { get; set; }
    [Column("chrg_num")]
    public int ChrgId { get; set; }
    [Column("account")]
    public string AccountNo { get; set; }
    [Column("status")]
    public string Status { get; set; }
    [Column("service_date")]
    public DateTime? ServiceDate { get; set; }
    [Column("hist_date")]
    public DateTime? HistoryDate { get; set; }
    [Column("cdm")]
    public string CDMCode { get; set; }
    [Column("qty")]
    public int Quantity { get; set; }
    [Column("net_amt")]
    public double NetAmount { get; set; }
    [Column("comment")]
    public string Comment { get; set; }
    [Column("invoice")]
    public string Invoice { get; set; }
    [Column("fin_type")]
    public string FinancialType { get; set; }
    [Column("mt_req_no")]
    public string LISReqNo { get; set; }
    [Column("post_date")]
    public DateTime? PostingDate { get; set; }
    [Column("cl_mnem")]
    public string ClientMnem { get; set; }
    [Column("fin_code")]
    public string FinCode { get; set; }
    [Column("performing_site")]
    public string PerformingSite { get; set; }
    [Column("bill_method")]
    public string BillMethod { get; set; }
    [Column("order_site")]
    public string OrderingSite { get; set; }
    [Column("location")]
    public string Location { get; set; }
    [Column("responsiblephy")]
    public string ResponsibleProvider { get; set; }
    [Column("mt_mnem")]
    public string OrderMnem { get; set; }
    [Column("facility")]
    public string Facility { get; set; }
    [Column("referencereq")]
    public string ReferenceReq { get; set; }
    [Column("retail")]
    public double RetailAmount { get; set; }
    [Column("inp_price")]
    public double HospAmount { get; set; }
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

    public Guid rowguid { get; set; }

    [Ignore]
    public string CptList
    {
        get
        {
            if (ChrgDetails != null && ChrgDetails.Count > 0)
            {
                string cptList = string.Empty;
                ChrgDetails.ForEach(x => cptList += x.Cpt4 + ", ");

                cptList = cptList.Remove(cptList.Length - 1);

                return cptList;
            }

            return string.Empty;
        }
    }

    [Ignore]
    public string CdmDescription
    {
        get
        {
            if (this.Cdm != null)
                return this.Cdm.Description;

            return string.Empty;
        }
    }

    [Ignore]
    public List<ChrgDetail> ChrgDetails { get; set; } = new List<ChrgDetail>();

    [Ignore]
    public Cdm Cdm { get; set; } = new Cdm();

}
