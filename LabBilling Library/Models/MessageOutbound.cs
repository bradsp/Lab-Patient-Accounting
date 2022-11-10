using System;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("infce.messages_outbound")]
    [PrimaryKey("systemMsgId", AutoIncrement = true)]
    public class MessageOutbound : IBaseEntity
    {
        public string account_cerner { get; set; } // varchar(50), null
        public decimal? sourceMsgId { get; set; } // numeric(18,0), null
        public string sourceInfce { get; set; } // varchar(50), null
        public string msgType { get; set; } // varchar(20), null
        public DateTime? msgDate { get; set; } // datetime, null
        public string msgContent { get; set; } // varchar(max), null
        public string processFlag { get; set; } // varchar(5), null
        public string processStatusMsg { get; set; } // varchar(250), null
        public decimal systemMsgId { get; set; } // numeric(18,0), not null
        public bool? dx_processed { get; set; } // bit, null
        public DateTime mod_date { get; set; } // datetime, null
        public string order_pat_id { get; set; } // varchar(50), null
        public string order_visit_id { get; set; } // varchar(50), null
        public DateTime? DOS { get; set; } // datetime, null
        public string dx_processed_method { get; set; } // varchar(50), null
        [Ignore]
        public string mod_user { get; set; }
        [Ignore]
        public string mod_prg { get; set; }
        [Ignore]
        public string mod_host { get; set; }
        [Ignore]
        public Guid rowguid { get; set; }
    }
}

