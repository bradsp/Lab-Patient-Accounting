using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess
{
    public class MessagesInboundRepository : RepositoryBase<MessagesInbound>
    {
        public MessagesInboundRepository(string connection) : base(connection)
        {

        }

        public MessagesInboundRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override MessagesInbound GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<MessagesInbound> GetByMessageType(string type, DateTime fromDate, DateTime throughDate)
        {
            Log.Instance.Trace("Entering");

            var record = dbConnection.Fetch<MessagesInbound>("where msgType like @0 and msgDate between @1 and @2 order by msgDate DESC",
                type + "%", fromDate.ToString("yyyy-MM-dd HH:mm:ss"), throughDate.ToString("yyyy-MM-dd HH:mm:ss"));

            return (record);

        }

        /// <summary>
        /// Reprocess a DFT HL7 message
        /// </summary>
        /// <param name="msgID"></param>
        /// <returns></returns>
        public int ReprocessDFTMessage(int msgID)
        {
            return dbConnection.ExecuteNonQueryProc("usp_cerner_chrg_reprocess", new { @msgID = msgID });
        }
    }
}
