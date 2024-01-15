using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("fin")]
    [PrimaryKey("fin_code", AutoIncrement = false)]
    public sealed class Fin : IBaseEntity
    {
        [Column("fin_code")]
        public string FinCode { get; set; }

        [Column("res_party")]
        public string Description { get; set; }

        [Column("form_type")]
        public string ClaimType { get; set; }

        [Column("chrgsource")]
        public string ChargeSource { get; set; }

        [Column("type")]
        public string FinClass { get; set; }

        [Column("h1500")]
        public string ProfessionalFlag { get; set; }

        [Column("ub92")]
        public string InstitutionalFlag { get; set; }
        [Column("mod_date")]
        [ResultColumn]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        [ResultColumn]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        [ResultColumn]
        public string UpdatedApp { get; set; }


        [Column("deleted")]
        public bool IsDeleted { get; set; }
        [Ignore]
        public string UpdatedHost { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }

    }
}
