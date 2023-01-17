using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class MessagesInboundRepository : RepositoryBase<MessageInbound>
    {
        public MessagesInboundRepository(string connection) : base(connection)
        {

        }

        public MessagesInboundRepository(PetaPoco.Database db) : base(db)
        {

        }

        public List<MessageInbound> GetUnprocessedMessages()
        {
            Log.Instance.Trace($"Entering");

            var command = PetaPoco.Sql.Builder;

            command.Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = 'N'");
            command.OrderBy($"{GetRealColumn(nameof(MessageInbound.MessageDate))}");

            var records = dbConnection.Fetch<MessageInbound>(command);

            return records;
        }

        public MessageInbound GetById(int id)
        {
            Log.Instance.Trace($"Entering {id}");

            return dbConnection.SingleOrDefault<MessageInbound>(id);

        }

        public List<MessageInbound> GetByDateRange(DateTime fromDate, DateTime throughDate)
        {
            Log.Instance.Trace("Entering");

            var cmd = PetaPoco.Sql.Builder;

            cmd.Where($"msgDate between @0 and @1",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate.ToString("yyyy-MM-dd HH:mm:ss") },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate.ToString("yyyy-MM-dd HH:mm:ss") });

            var record = dbConnection.Fetch<MessageInbound>(cmd);

            return (record);
        }

        public List<MessageInbound> GetByMessageType(string type, DateTime fromDate, DateTime throughDate)
        {
            Log.Instance.Trace("Entering");

            var record = dbConnection.Fetch<MessageInbound>("where msgType like @0 and msgDate between @1 and @2 order by msgDate DESC",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = type + "%" }, 
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate.ToString("yyyy-MM-dd HH:mm:ss") }, 
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate.ToString("yyyy-MM-dd HH:mm:ss") });

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
