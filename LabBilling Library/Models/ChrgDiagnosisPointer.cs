using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("chrg_dx_pointer")]
    [PrimaryKey("chrg_detail_uri", AutoIncrement = false)]
    public class ChrgDiagnosisPointer : IBaseEntity
    {

        [Column("chrg_detail_uri")]
        public double ChrgDetailUri { get; set; }

        [Column("diag_ptr")]
        public string DiagnosisPointer { get; set; }

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

    }
}
