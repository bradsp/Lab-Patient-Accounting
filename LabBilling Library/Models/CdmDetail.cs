using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("cpt4")]
    public class CdmFeeSchedule1
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cdm")]
        public string ChargeItemId { get; set; }

        [Column("link")]
        public int Link { get; set; }

        [Column("code_flag")]
        public string CodeFlag { get; set; }

        [Column("cpt4")]
        public string Cpt4 { get; set; }

        [Column("descript")]
        public string Description { get; set; }

        [Column("mprice")]
        public double MClassPrice { get; set; }

        [Column("cprice")]
        public double CClassPrice { get; set; }

        [Column("zprice")]
        public double ZClassPrice { get; set; }

        [Column("rev_code")]
        public string RevenueCode { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("modi")]
        public string Modifier { get; set; }

        [Column("billcode")]
        public string BillCode { get; set; }

        [Column("mod_date")]
        public DateTime? mod_date { get; set; }

        [Column("mod_user")]
        public string mod_user { get; set; }

        [Column("mod_prg")]
        public string mod_prg { get; set; }

        [Column("mod_host")]
        public string mod_host { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

    }

    [TableName("cpt4_2")]
    public class CdmFeeSchedule2
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cdm")]
        public string ChargeItemId { get; set; }

        [Column("link")]
        public int Link { get; set; }

        [Column("code_flag")]
        public string CodeFlag { get; set; }

        [Column("cpt4")]
        public string Cpt4 { get; set; }

        [Column("descript")]
        public string Description { get; set; }

        [Column("mprice")]
        public double MClassPrice { get; set; }

        [Column("cprice")]
        public double CClassPrice { get; set; }

        [Column("zprice")]
        public double ZClassPrice { get; set; }

        [Column("rev_code")]
        public string RevenueCode { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("modi")]
        public string Modifier { get; set; }

        [Column("billcode")]
        public string BillCode { get; set; }

        [Column("mod_date")]
        public DateTime? mod_date { get; set; }

        [Column("mod_user")]
        public string mod_user { get; set; }

        [Column("mod_prg")]
        public string mod_prg { get; set; }

        [Column("mod_host")]
        public string mod_host { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

    }

    [TableName("cpt4_3")]
    public class CdmFeeSchedule3
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cdm")]
        public string ChargeItemId { get; set; }

        [Column("link")]
        public int Link { get; set; }

        [Column("code_flag")]
        public string CodeFlag { get; set; }

        [Column("cpt4")]
        public string Cpt4 { get; set; }

        [Column("descript")]
        public string Description { get; set; }

        [Column("mprice")]
        public double MClassPrice { get; set; }

        [Column("cprice")]
        public double CClassPrice { get; set; }

        [Column("zprice")]
        public double ZClassPrice { get; set; }

        [Column("rev_code")]
        public string RevenueCode { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("modi")]
        public string Modifier { get; set; }

        [Column("billcode")]
        public string BillCode { get; set; }

        [Column("mod_date")]
        public DateTime? mod_date { get; set; }

        [Column("mod_user")]
        public string mod_user { get; set; }

        [Column("mod_prg")]
        public string mod_prg { get; set; }

        [Column("mod_host")]
        public string mod_host { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

    }

    [TableName("cpt4_4")]
    public class CdmFeeSchedule4
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cdm")]
        public string ChargeItemId { get; set; }

        [Column("link")]
        public int Link { get; set; }

        [Column("code_flag")]
        public string CodeFlag { get; set; }

        [Column("cpt4")]
        public string Cpt4 { get; set; }

        [Column("descript")]
        public string Description { get; set; }

        [Column("mprice")]
        public double MClassPrice { get; set; }

        [Column("cprice")]
        public double CClassPrice { get; set; }

        [Column("zprice")]
        public double ZClassPrice { get; set; }

        [Column("rev_code")]
        public string RevenueCode { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("modi")]
        public string Modifier { get; set; }

        [Column("billcode")]
        public string BillCode { get; set; }

        [Column("mod_date")]
        public DateTime? mod_date { get; set; }

        [Column("mod_user")]
        public string mod_user { get; set; }

        [Column("mod_prg")]
        public string mod_prg { get; set; }

        [Column("mod_host")]
        public string mod_host { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

    }

    [TableName("cpt4_5")]
    public class CdmFeeSchedule5
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cdm")]
        public string ChargeItemId { get; set; }

        [Column("link")]
        public int Link { get; set; }

        [Column("code_flag")]
        public string CodeFlag { get; set; }

        [Column("cpt4")]
        public string Cpt4 { get; set; }

        [Column("descript")]
        public string Description { get; set; }

        [Column("mprice")]
        public double MClassPrice { get; set; }

        [Column("cprice")]
        public double CClassPrice { get; set; }

        [Column("zprice")]
        public double ZClassPrice { get; set; }

        [Column("rev_code")]
        public string RevenueCode { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("modi")]
        public string Modifier { get; set; }

        [Column("billcode")]
        public string BillCode { get; set; }

        [Column("mod_date")]
        public DateTime? mod_date { get; set; }

        [Column("mod_user")]
        public string mod_user { get; set; }

        [Column("mod_prg")]
        public string mod_prg { get; set; }

        [Column("mod_host")]
        public string mod_host { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

    }
}
