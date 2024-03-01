using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class MessagesInboundRepository : RepositoryBase<MessageInbound>
    {
        public MessagesInboundRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public List<MessageInbound> GetUnprocessedMessages()
        {
            Log.Instance.Trace($"Entering");
            
            var command = PetaPoco.Sql.Builder;
            command.Where($"{GetRealColumn(nameof(MessageInbound.ProcessFlag))} = 'N'");
            command.OrderBy($"{GetRealColumn(nameof(MessageInbound.MessageDate))}");

            var records = Context.Fetch<MessageInbound>(command);

            return records;
        }

        public MessageInbound GetById(int id)
        {
            Log.Instance.Trace($"Entering {id}");

            return Context.SingleOrDefault<MessageInbound>((object)id);

        }

        public List<MessageInbound> GetByDateRange(DateTime fromDate, DateTime throughDate)
        {
            Log.Instance.Trace("Entering");

            var cmd = PetaPoco.Sql.Builder;

            cmd.Where($"msgDate between @0 and @1",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate.ToString("yyyy-MM-dd HH:mm:ss") },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate.ToString("yyyy-MM-dd HH:mm:ss") });

            var record = Context.Fetch<MessageInbound>(cmd);

            return (record);
        }

        public List<MessageInbound> GetByMessageType(string type, DateTime fromDate, DateTime throughDate)
        {
            Log.Instance.Trace("Entering");

            var record = Context.Fetch<MessageInbound>("where msgType like @0 and msgDate between @1 and @2 order by msgDate DESC",
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
            return Context.ExecuteNonQueryProc("usp_cerner_chrg_reprocess", new { @msgID = msgID });
        }
    }
}
