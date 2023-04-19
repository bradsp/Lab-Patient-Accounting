using LabBilling.Core.Models;
using System;
using static LabBilling.Core.BusinessLogic.HL7Processor;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChkBatchRepository : RepositoryBase<ChkBatch>
    {
        public ChkBatchRepository(string connection) : base(connection)
        {

        }

        public ChkBatchRepository(PetaPoco.Database db) : base(db)
        {

        }

        public ChkBatch GetById(int id)
        {
            return dbConnection.SingleOrDefault<ChkBatch>(id);
        }

        public int UpdatePostedDate(int id, DateTime postedDate)
        {

            return dbConnection.Update<ChkBatch>($"set {GetRealColumn(nameof(ChkBatch.PostedDate))} = @0 where {GetRealColumn(nameof(ChkBatch.BatchNo))} = @1",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = postedDate },
                new SqlParameter() { SqlDbType = SqlDbType.Int, Value = id });

        }

    }
}
