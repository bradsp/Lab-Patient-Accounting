using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Models
{
    [TableName("announcements")]
    [PrimaryKey("id", AutoIncrement = true)]
    public sealed class Announcement : IBaseEntity
    {
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        [Column("message_text")]
        public string MessageText { get; set; }
        [Column("mod_date")]
        public DateTime UpdatedDate { get; set; }
        [Column("mod_user")]
        public string UpdatedUser { get; set; }
        [Column("mod_prg")]
        public string UpdatedApp { get; set; }
        [Column("mod_host")]
        public string UpdatedHost { get; set; }
        [Column("id")]
        public int Id { get; set; }
        [Column("is_rtf")]
        public bool IsRtf { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
