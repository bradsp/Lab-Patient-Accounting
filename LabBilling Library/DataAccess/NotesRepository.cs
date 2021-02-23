using LabBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.DataAccess
{
    public class NotesRepository : RepositoryBase<Notes>
    {
        public NotesRepository(string connection) : base("notes", connection)
        {

        }

        public override Notes GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Notes> GetByAccount(string account)
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
