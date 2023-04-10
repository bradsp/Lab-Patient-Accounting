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
    public class Announcement : IBaseEntity
    {
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        [Column("message_text")]
        public string MessageText { get; set; }
        [Column("mod_date")]
        public DateTime mod_date { get; set; }
        [Column("mod_user")]
        public string mod_user { get; set; }
        [Column("mod_prg")]
        public string mod_prg { get; set; }
        [Column("mod_host")]
        public string mod_host { get; set; }
        [Column("id")]
        public int Id { get; set; }

        [Ignore]
        public Guid rowguid { get; set; }
    }
}
