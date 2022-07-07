using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class NotesRepository : RepositoryBase<Notes>
    {
        public NotesRepository(string connection) : base("notes", connection)
        {

        }

        public NotesRepository(string connection, PetaPoco.Database db) : base("notes", connection, db)
        {

        }

        public override Notes GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Notes> GetByAccount(string account)
        {
            var sql = PetaPoco.Sql.Builder
                .Select("*")
                .From(_tableName)
                .Where("account = @0", account)
                .OrderBy("mod_date DESC");

            var records = dbConnection.Fetch<Notes>(sql);

            return records;
        }
    }
}
