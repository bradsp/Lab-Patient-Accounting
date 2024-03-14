using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LabBilling.Core.UnitOfWork;
using PetaPoco;


namespace LabBilling.Core.DataAccess
{
    public sealed class AccountNoteRepository : RepositoryBase<AccountNote>
    {
        public AccountNoteRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
        {

        }

        public List<AccountNote> GetByAccount(string account)
        {
            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(AccountNote.Account))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
                .OrderBy($"{GetRealColumn(nameof(AccountNote.UpdatedDate))} DESC");

            var records = Context.Fetch<AccountNote>(sql);

            return records;
        }

        public AccountNote Add (string accountNo, string comment)
        {
            var note = new AccountNote
            {
                Account = accountNo,
                Comment = comment
            };

            return (AccountNote)Add(note);

        }
    }
}
