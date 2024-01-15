using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace LabBilling.Core.DataAccess
{
    public sealed class AccountNoteRepository : RepositoryBase<AccountNote>
    {
        public AccountNoteRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public List<AccountNote> GetByAccount(string account)
        {
            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(AccountNote.Account))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
                .OrderBy($"{GetRealColumn(nameof(AccountNote.UpdatedDate))} DESC");

            var records = dbConnection.Fetch<AccountNote>(sql);

            return records;
        }
    }
}
