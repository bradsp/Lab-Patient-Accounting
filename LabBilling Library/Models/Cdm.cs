using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [PetaPoco.TableName("cdm")]
    [PetaPoco.PrimaryKey("cdm", AutoIncrement = false)]
    public sealed class Cdm : IBaseEntity
    {
        [Column("deleted")]
        public bool IsDeleted { get; set; }
        [Column("cdm")]
        public string ChargeId { get; set; }
        [Column("descript")]
        public string Description { get; set; }
        [Column("mtype")]
        public string MClassType { get; set; }
        [Column("m_pa_amt")]
        public double MClassPaAmount { get; set; }
        [Column("ctype")]
        public string CClassType { get; set; }
        [Column("c_pa_amt")]
        public double CClassPaAmount { get; set; }
        [Column("ztype")]
        public string ZClassType { get; set; }
        [Column("z_pa_amt")]
        public double ZClassPaAmount { get; set; }
        [Column("orderable")]
        public int IsOrderable { get; set; }
        [Column("cbill_detail")]
        public int CBillDetail { get; set; }
        [Column("comments")]
        public string Comments { get; set; }
        [Column("mnem")]
        public string Mnem { get; set; }
        [Column("cost")]
        public double Cost { get; set; }
        [Column("ref_lab_id")]
        public string RefLabId { get; set; }
        [Column("ref_lab_bill_code")]
        public string RefLabBillCode { get; set; }
        [Column("ref_lab_payment")]
        public double RefLabPayment { get; set; }
        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [Column("mod_user")]
        public string mod_user { get; set; }
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [Column("mod_host")]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

        [Ignore]
        public List<CdmDetail> CdmDetails { get; set; }

        [Ignore]
        public List<CdmDetail> CdmFeeSchedule1 { get; set; }
        [Ignore]
        public List<CdmDetail> CdmFeeSchedule2 { get; set; }
        [Ignore]
        public List<CdmDetail> CdmFeeSchedule3 { get; set; }
        [Ignore]
        public List<CdmDetail> CdmFeeSchedule4 { get; set; }
        [Ignore]
        public List<CdmDetail> CdmFeeSchedule5 { get; set; }

        public override string ToString()
        {
            string value = $"{ChargeId}|{Description}|{Mnem}";

            return value;
        }

    }
}
