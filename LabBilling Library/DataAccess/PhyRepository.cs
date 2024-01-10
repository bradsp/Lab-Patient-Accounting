using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using NPOI.XWPF.UserModel;

namespace LabBilling.Core.DataAccess
{
    public sealed class PhyRepository : RepositoryBase<Phy>
    {
        public PhyRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
                
        }

        public Phy GetByNPI(string npi)
        {
            Log.Instance.Trace($"Entering - npi {npi}");
            Phy phy = new();
            Pth pth = new();

            if (!string.IsNullOrEmpty(npi))
                phy = dbConnection.SingleOrDefault<Phy>($"where {GetRealColumn(nameof(Phy.NpiId))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = npi });
            if (phy != null)
            {
                if (!string.IsNullOrEmpty(phy.PathologistCode))
                {
                    pth = dbConnection.SingleOrDefault<Pth>(Convert.ToInt32(phy.PathologistCode));
                    phy.Pathologist = pth;
                }
            }

            return phy;
        }

        public List<Phy> GetByName(string lastName, string firstName)
        {
            Log.Instance.Trace($"Entering - name {lastName} {firstName}");
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

            Log.Instance.Trace(dbConnection.LastSQL);
            return queryResult;
        }

        public Phy GetById(double id)
        {
            Log.Instance.Trace($"Entering - id {id}");
            Phy phy = new Phy();
            Pth pth = new Pth();

            phy = dbConnection.SingleOrDefault<Phy>(id);
            if(phy == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(phy.PathologistCode))
            {
                pth = dbConnection.SingleOrDefault<Pth>(phy.PathologistCode);
                phy.Pathologist = pth;
            }
            
            return phy;
        }

        public override bool Save(Phy table)
        {
            var existing = GetById(table.uri);

            if(existing != null)
            {
                return this.Update(table);
            }
            else
            {
                if (this.Add(table) != null)
                    return true;
                else
                    return false;
            }
        }

    }
}
