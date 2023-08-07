using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public List<ChrgDetail> GetByAccount(string accountNo)
        {
            Log.Instance.Trace("Entering");

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));

            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(AppEnvironment);

            var sql = Sql.Builder
                .From($"{_tableName}")
                .Where($"{this.GetRealColumn(nameof(ChrgDetail.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

            var results = dbConnection.Fetch<ChrgDetail>(sql);

            foreach (var result in results)
            {
                result.RevenueCodeDetail = revenueCodeRepository.GetByCode(result.RevenueCode);
            }

            return results;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="showCredited"></param>
        /// <param name="includeInvoiced"></param>
        /// <param name="asOfDate"></param>
        /// <param name="excludeCBill"></param>
        /// <returns></returns>
        public List<ChrgDetail> GetByAccount(string account, bool showCredited = true, bool includeInvoiced = true, DateTime? asOfDate = null, bool excludeCBill = true)
        {
            Log.Instance.Trace("Entering");

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(AppEnvironment);

            var sql = Sql.Builder
                .From($"{_tableName}")
                .Where($"{_tableName}.{this.GetRealColumn(nameof(ChrgDetail.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
                .Where($"{_tableName}.{GetRealColumn(nameof(ChrgDetail.IsCredited))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = showCredited })
                .Where($"{_tableName}.{GetRealColumn(nameof(ChrgDetail.ServiceDate))} <= @0", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = asOfDate })
                .Where($"{_tableName}.{GetRealColumn(nameof(ChrgDetail.BillingCode))} <> @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "CBILL" });

            if(!includeInvoiced)
            {
                sql.Where($"{GetRealColumn(nameof(ChrgDetail.Invoice))} = '' or {GetRealColumn(nameof(ChrgDetail.Invoice))} is null");
            }

            var results = dbConnection.Fetch<ChrgDetail>(sql);

            foreach (var result in results)
            {
                result.RevenueCodeDetail = revenueCodeRepository.GetByCode(result.RevenueCode);
            }

            return results;
        }


        /// <summary>
        /// Add a list of charge details.
        /// </summary>
        /// <param name="chrgDetails"></param>
        /// <returns></returns>
        public List<ChrgDetail> Add(IList<ChrgDetail> chrgDetails)
        {
            Log.Instance.Trace("Entering");
            List<ChrgDetail> addedCharges = new List<ChrgDetail>();

            foreach(var cd in chrgDetails)
            {
                var value = Add(cd);
                cd.ChrgDetailId = Convert.ToInt32(value);
                addedCharges.Add(cd);
            }

            return addedCharges;
        }

        public override object Add(ChrgDetail table)
        {
            try
            {
                var value = base.Add(table);
                table.ChrgDetailId = Convert.ToInt32(value);
                return table;
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

            if (chrg_num <= 0)
                throw new ArgumentOutOfRangeException(nameof(chrg_num));

            RevenueCodeRepository revenueCodeRepository = new RevenueCodeRepository(AppEnvironment);
            ChrgDiagnosisPointerRepository chrgDiagnosisPointerRepository = new ChrgDiagnosisPointerRepository(AppEnvironment);
            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{_tableName}.{this.GetRealColumn(nameof(ChrgDetail.ChrgNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = chrg_num });

            var results = dbConnection.Fetch<ChrgDetail>(sql);

            foreach(var result in results)
            {
                result.RevenueCodeDetail = revenueCodeRepository.GetByCode(result.RevenueCode);
            }

            return results;
        }

        public int AddModifier(int uri, string modifier)
        {
            Log.Instance.Trace("Entering");

            if (uri <= 0)
                throw new ArgumentOutOfRangeException(nameof(uri));

            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{_tableName}.{this.GetRealColumn(nameof(ChrgDetail.ChrgDetailId))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal,Value = uri });

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
            Log.Instance.Trace("Entering");
            if (uri <= 0)
                throw new ArgumentOutOfRangeException(nameof(uri));

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
