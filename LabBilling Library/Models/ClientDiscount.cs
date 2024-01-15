using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dictionary.cli_dis")]
    [PrimaryKey("uri", AutoIncrement = true)]
    public sealed class ClientDiscount : IBaseEntity
    {
        public Guid rowguid { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("cli_mnem")]
        public string ClientMnem { get; set; }

        [Column("start_cdm")]
        public string Cdm { get; set; }

        [Ignore]
        public string CdmDescription { get; set; }

        [Column("end_cdm")]
        public string EndCdmRange { get; set; }

        [Column("percent_ds")]
        public double PercentDiscount { get; set; }

        [Column("price")]
        public double Price { get; set; }

        [Column("mod_date")]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        public string UpdatedHost { get; set; }

        public double uri { get; set; }
    }
}
