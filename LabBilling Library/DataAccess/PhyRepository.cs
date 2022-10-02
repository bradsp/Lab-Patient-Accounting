using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using NPOI.XWPF.UserModel;

namespace LabBilling.Core.DataAccess
{
    public class PhyRepository : RepositoryBase<Phy>
    {
        public PhyRepository(string connection) : base(connection)
        {
                
        }

        public PhyRepository(PetaPoco.Database db) : base(db)
        {

        }

        public Phy GetByNPI(string npi)
        {
            Phy phy = new Phy();
            Pth pth = new Pth();

            if (!string.IsNullOrEmpty(npi))
                phy = dbConnection.SingleOrDefault<Phy>("where tnh_num = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = npi });

            if (!string.IsNullOrEmpty(phy.PathologistCode))
            {
                pth = dbConnection.SingleOrDefault<Pth>(Convert.ToInt32(phy.PathologistCode));
                phy.Pathologist = pth;
            }

            return phy;
        }

        public List<Phy> GetByName(string lastName, string firstName)
        {
            List<Phy> phy = new List<Phy>();
            Pth pth = new Pth();

            if(!string.IsNullOrEmpty(lastName) || !string.IsNullOrEmpty(firstName))
            {
                var command = PetaPoco.Sql.Builder
                    .From(_tableName)
                    .Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.LastName))} like @0+'%'",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastName })
                    .Where($"{this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName))} like @0+'%'",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstName })
                    .OrderBy($"{this.GetRealColumn(typeof(Phy), nameof(Phy.LastName))}, {this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName))}");

                phy = dbConnection.Fetch<Phy>(command);

                return phy;
            }

            return null;
        }

        public List<Phy> GetActive()
        {
            Log.Instance.Trace("Entering");

            string sql = $"SELECT * FROM {_tableName} where deleted = 0";

            var queryResult = dbConnection.Fetch<Phy>(sql);

            Log.Instance.Trace("Exiting");
            return queryResult;
        }

        public override Phy GetById(int id)
        {
            Phy phy = new Phy();
            Pth pth = new Pth();

            phy = dbConnection.SingleOrDefault<Phy>(id);
            if (phy.PathologistCode != null)
            {
                pth = dbConnection.SingleOrDefault<Pth>(phy.PathologistCode);
                phy.Pathologist = pth;
            }
            
            return phy;
        }

    }
}
