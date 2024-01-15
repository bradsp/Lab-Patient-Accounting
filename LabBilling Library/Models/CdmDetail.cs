using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("cpt4")]
    public sealed class CdmFeeSchedule1 : CdmDetail, ICdmDetail
    {

    }

    [TableName("cpt4_2")]
    public sealed class CdmFeeSchedule2 : CdmDetail, ICdmDetail
    {

    }

    [TableName("cpt4_3")]
    public sealed class CdmFeeSchedule3 : CdmDetail, ICdmDetail
    {

    }

    [TableName("cpt4_4")]
    public sealed class CdmFeeSchedule4 : CdmDetail, ICdmDetail
    {

    }

    [TableName("cpt4_5")]
    public sealed class CdmFeeSchedule5 : CdmDetail, ICdmDetail
    {

    }

    [TableName("dictionary.cdm_detail")]
    [PrimaryKey("rowguid", AutoIncrement = false)]
    public class CdmDetail : ICdmDetail,IBaseEntity
    {
        [Column("rowguid")]
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("fee_sched")]
        public string FeeSchedule { get; set; }

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
        public DateTime UpdatedDate { get; set; }

        [Column("mod_user")]
        public string UpdatedUser { get; set; }

        [Column("mod_prg")]
        public string UpdatedApp { get; set; }

        [Column("mod_host")]
        public string UpdatedHost { get; set; }

        [Column("cost")]
        public decimal Cost { get; set; }

    }

    public interface ICdmDetail
    {
        Guid rowguid { get; set; }

        bool IsDeleted { get; set; }

        string FeeSchedule { get; set; }

        string ChargeItemId { get; set; }

        int Link { get; set; }

        string CodeFlag { get; set; }

        string Cpt4 { get; set; }

        string Description { get; set; }

        double MClassPrice { get; set; }

        double CClassPrice { get; set; }

        double ZClassPrice { get; set; }

        string RevenueCode { get; set; }

        string Type { get; set; }

        string Modifier { get; set; }

        string BillCode { get; set; }

        DateTime UpdatedDate { get; set; }

        string UpdatedUser { get; set; }

        string UpdatedApp { get; set; }

        string UpdatedHost { get; set; }

        decimal Cost { get; set; }
    }
        
}
