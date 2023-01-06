using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class FinRepository : RepositoryBase<Fin>
    {
        public FinRepository(string connection) : base(connection)
        {

        }

        public FinRepository(PetaPoco.Database db) : base(db)
        {

        }

        public List<Fin> GetActive()
        {
            return dbConnection.Query<Fin>($"where {GetRealColumn(nameof(Fin.IsDeleted))} = 0").ToList();
        }

        public override Fin GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Fin GetFin(string finCode)
        {
            return dbConnection.SingleOrDefault<Fin>($"where {GetRealColumn(nameof(Fin.FinCode))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = finCode });
        }
    }
}
