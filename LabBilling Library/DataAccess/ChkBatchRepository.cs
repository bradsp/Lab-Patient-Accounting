using LabBilling.Core.Models;
using System;
using static LabBilling.Core.BusinessLogic.HL7Processor;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using LabBilling.Logging;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChkBatchRepository : RepositoryBase<ChkBatch>
    {
        private ChkBatchDetailRepository _chkBatchDetailRepository;

        public ChkBatchRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            _chkBatchDetailRepository = new ChkBatchDetailRepository(appEnvironment);
        }

        public ChkBatch GetById(int id)
        {
            Log.Instance.Trace("Entering");

            var batch = dbConnection.SingleOrDefault<ChkBatch>(id);

            batch.ChkBatchDetails = _chkBatchDetailRepository.GetByBatch(batch.BatchNo);

            if(batch.ChkBatchDetails == null)
                batch.ChkBatchDetails = new List<ChkBatchDetail>();

            return batch;
        }

        public int UpdatePostedDate(int id, DateTime postedDate)
        {
            Log.Instance.Trace("Entering");

            return dbConnection.Update<ChkBatch>($"set {GetRealColumn(nameof(ChkBatch.PostedDate))} = @0 where {GetRealColumn(nameof(ChkBatch.BatchNo))} = @1",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = postedDate },
                new SqlParameter() { SqlDbType = SqlDbType.Int, Value = id });

        }

        public List<ChkBatch> GetOpenBatches()
        {
            Log.Instance.Trace("Entering");
            return dbConnection.Fetch<ChkBatch>($"where {GetRealColumn(nameof(ChkBatch.PostedDate))} is null");
        }

        public override bool Delete(ChkBatch table)
        {
            _chkBatchDetailRepository.DeleteBatch(table.BatchNo);

            return base.Delete(table);
        }

        public override bool Update(ChkBatch table)
        {
            var rv = base.Update(table);

            //save the chk details
            table.ChkBatchDetails.ForEach(x => x.Batch = table.BatchNo);

            table.ChkBatchDetails.ForEach(x => _chkBatchDetailRepository.Save(x));

            return rv;
        }

        public override object Add(ChkBatch table)
        {
            var rv = base.Add(table);

            //save the chk details
            table.ChkBatchDetails.ForEach(x => x.Batch = table.BatchNo);

            table.ChkBatchDetails.ForEach(x => _chkBatchDetailRepository.Save(x));

            return rv;
        }
    }

}
