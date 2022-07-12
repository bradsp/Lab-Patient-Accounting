using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class AccountNoteRepository : RepositoryBase<AccountNote>
    {
        public AccountNoteRepository(string connection) : base("notes", connection)
        {

        }

        public AccountNoteRepository(string connection, PetaPoco.Database db) : base("notes", connection, db)
        {

        }

        public override AccountNote GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<AccountNote> GetByAccount(string account)
        {
            var sql = PetaPoco.Sql.Builder
                .Select("*")
                .From(_tableName)
                .Where("account = @0", account)
                .OrderBy("mod_date DESC");

            var records = dbConnection.Fetch<AccountNote>(sql);

            return records;
        }
    }
}
