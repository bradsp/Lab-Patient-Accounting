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
    public class Cdm : IBaseEntity
    {
        public bool deleted { get; set; }
        public string cdm { get; set; }
        public string descript { get; set; }
        public string mtype { get; set; }
        public double m_pa_amt { get; set; }
        public string ctype { get; set; }
        public double c_pa_amt { get; set; }
        public string ztype { get; set; }
        public double z_pa_amt { get; set; }
        public int orderable { get; set; }
        public int cbill_detail { get; set; }
        public string comments { get; set; }
        public string mnem { get; set; }
        public double cost { get; set; }
        public string ref_lab_id { get; set; }
        public string ref_lab_bill_code { get; set; }
        public double ref_lab_payment { get; set; }
        public DateTime? mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

        [Ignore]
        public List<CdmFeeSchedule1> cdmFeeSchedule1 { get; set; }
        [Ignore]
        public List<CdmFeeSchedule2> cdmFeeSchedule2 { get; set; }
        [Ignore]
        public List<CdmFeeSchedule3> cdmFeeSchedule3 { get; set; }
        [Ignore]
        public List<CdmFeeSchedule4> cdmFeeSchedule4 { get; set; }
        [Ignore]
        public List<CdmFeeSchedule5> cdmFeeSchedule5 { get; set; }


    }
}
