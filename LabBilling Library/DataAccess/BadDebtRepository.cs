using LabBilling.Core.Models;
using Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LabBilling.Core.DataAccess
{
    public sealed class BadDebtRepository : RepositoryBase<BadDebt>
    {
        public BadDebtRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public BadDebt GetRecord(string rowguid)
        {
            BadDebt badDebt = new BadDebt();

            _ = Guid.TryParse(rowguid, out Guid gRowGuid);

            badDebt = dbConnection.SingleOrDefault<BadDebt>($"where {GetRealColumn(nameof(BadDebt.rowguid))} = @0", new SqlParameter() { SqlDbType = SqlDbType.UniqueIdentifier, Value = gRowGuid });

            if (!string.IsNullOrWhiteSpace(badDebt.StateZip) && badDebt.StateZip.Length >= 7)
            {
                badDebt.State = badDebt.StateZip.Substring(0, 2);
                badDebt.Zip = badDebt.StateZip.Substring(3);
            }

            return badDebt;
        }

        public override bool Update(BadDebt table)
        {
            table.StateZip = string.Format("{0} {1}", table.State, table.Zip);

            return base.Update(table);
        }

        public override bool Update(BadDebt table, IEnumerable<string> columns)
        {
            table.StateZip = string.Format("{0} {1}", table.State, table.Zip);

            return base.Update(table, columns);
        }

        public IEnumerable<BadDebt> GetNotSentRecords()
        {
            List<BadDebt> records = new List<BadDebt>();

            var sql = PetaPoco.Sql.Builder;
            sql.Where($"{GetRealColumn(nameof(BadDebt.DateSent))} is null");

            records = dbConnection.Fetch<BadDebt>(sql);

            foreach (var record in records)
            {
                if (!string.IsNullOrWhiteSpace(record.StateZip) && record.StateZip.Length >= 7)
                {
                    record.State = record.StateZip.Substring(0, 2);
                    record.Zip = record.StateZip.Substring(3);
                }
            }

            return records;
        }

        public IEnumerable<BadDebt> GetSentByDate(DateTime date)
        {
            List<BadDebt> records = new List<BadDebt>();

            var sql = PetaPoco.Sql.Builder;
            sql.Where($"{GetRealColumn(nameof(BadDebt.DateSent))} between @0 and @1",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = date.BeginningOfTheDay()},
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = date.EndOfTheDay()});

            records = dbConnection.Fetch<BadDebt>(sql);

            foreach (var record in records)
            {
                if (!string.IsNullOrWhiteSpace(record.StateZip) && record.StateZip.Length >= 7)
                {
                    record.State = record.StateZip.Substring(0, 2);
                    record.Zip = record.StateZip.Substring(3);
                }
            }

            return records;
        }
    }
}
