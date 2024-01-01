using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChrgDetailRepository : RepositoryBase<ChrgDetail>
    {
        public ChrgDetailRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public override object Add(ChrgDetail table)
        {
            Log.Instance.Trace("Entering");
            try
            {
                ChrgDiagnosisPointerRepository chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(_appEnvironment);

                var value = base.Add(table);

                if (table.DiagnosisPointer != null)
                {
                    table.DiagnosisPointer.ChrgDetailUri = Convert.ToDouble(value);
                    chrgDiagnosisPointerRepository.Save(table.DiagnosisPointer);
                }

                return value;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex, "Exception encountered in ChrgDetail.Add");
                throw new ApplicationException("Exception encountered in ChrgDetail.Add", ex);
            }
        }

        public IEnumerable<ChrgDetail> GetByCharge(int chrg_num)
        {
            Log.Instance.Trace("Entering");

            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(_appEnvironment);
            ChrgDiagnosisPointerRepository chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(_appEnvironment);
            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{this.GetRealColumn(nameof(ChrgDetail.ChrgNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = chrg_num });

            var results = dbConnection.Fetch<ChrgDetail>(sql);

            foreach(var result in results)
            {
                result.RevenueCodeDetail = revenueCodeRepository.GetByCode(result.RevenueCode);
                result.DiagnosisPointer = chrgDiagnosisPointerRepository.GetById(result.uri);
            }

            return results;
        }

        public int AddModifier(int uri, string modifier)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{this.GetRealColumn(nameof(ChrgDetail.uri))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal,Value = uri });

            var result = dbConnection.SingleOrDefault<ChrgDetail>(sql);

            if(result != null)
            {
                result.Modifier = modifier;

                return dbConnection.Update(result);
            }

            return 0;
        }

        public int RemoveModifier(int uri)
        {
            return AddModifier(uri, string.Empty);
        }

    }

    public class ChrgDiagnosisPointerRepository : RepositoryBase<ChrgDiagnosisPointer>
    {
        public ChrgDiagnosisPointerRepository(IAppEnvironment appEnvironment) : base (appEnvironment)
        {

        }

        public ChrgDiagnosisPointer GetById(int id)
        {
            return dbConnection.SingleOrDefault<ChrgDiagnosisPointer>(id);
        }

        public ChrgDiagnosisPointer GetById(double id)
        {
            return dbConnection.SingleOrDefault<ChrgDiagnosisPointer>(id);
        }

        public override bool Save(ChrgDiagnosisPointer record)
        {
            if(record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            if(record.ChrgDetailUri <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ChrgDiagnosisPointer.ChrgDetailUri));
            }
            var existingRecord = GetById(record.ChrgDetailUri);

            if(existingRecord != null)
            {
                return Update(record);
            }
            else
            {
                Add(record);
                return true;
            }
        }

    }

}
