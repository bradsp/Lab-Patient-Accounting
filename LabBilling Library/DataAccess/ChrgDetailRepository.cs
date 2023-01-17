using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class ChrgDetailRepository : RepositoryBase<ChrgDetail>
    {
        public ChrgDetailRepository(string connection) : base(connection)
        {

        }

        public ChrgDetailRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override object Add(ChrgDetail table)
        {
            ChrgDiagnosisPointerRepository chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(dbConnection);

            var value = base.Add(table);

            table.DiagnosisPointer.ChrgDetailUri = Convert.ToDouble(value);

            chrgDiagnosisPointerRepository.Save(table.DiagnosisPointer);

            return value;
        }

        public IEnumerable<ChrgDetail> GetByCharge(int chrg_num)
        {
            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(dbConnection);
            ChrgDiagnosisPointerRepository chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(dbConnection);
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
    }

    public class ChrgDiagnosisPointerRepository : RepositoryBase<ChrgDiagnosisPointer>
    {
        public ChrgDiagnosisPointerRepository(string connection) : base (connection)
        {

        }

        public ChrgDiagnosisPointerRepository(Database db) : base(db)
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
            if(record.ChrgDetailUri <= 0)
            {
                throw new ArgumentOutOfRangeException("ChrgDetailUri");
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
