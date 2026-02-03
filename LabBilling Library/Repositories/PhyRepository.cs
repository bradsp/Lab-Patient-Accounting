using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.Models;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class PhyRepository : RepositoryBase<Phy>
    {
        public PhyRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
                
        }

        public Phy GetByNPI(string npi)
        {
            Log.Instance.Trace($"Entering - npi {npi}");
            Phy phy = new();
            Pth pth = new();

            if (!string.IsNullOrEmpty(npi))
                phy = Context.SingleOrDefault<Phy>($"where {GetRealColumn(nameof(Phy.NpiId))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = npi });
            if (phy != null)
            {
                if (!string.IsNullOrEmpty(phy.PathologistCode))
                {
                    pth = Context.SingleOrDefault<Pth>((Object)Convert.ToInt32(phy.PathologistCode));
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
                    .From(_tableName);
                WhereLike(command, this.GetRealColumn(typeof(Phy), nameof(Phy.LastName)), lastName);
                WhereLike(command, this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName)), firstName);
                command.OrderBy($"{this.GetRealColumn(typeof(Phy), nameof(Phy.LastName))}, {this.GetRealColumn(typeof(Phy), nameof(Phy.FirstName))}");

                phy = Context.Fetch<Phy>(command);

                return phy;
            }

            return null;
        }

        public List<Phy> GetActive()
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(Phy.IsDeleted))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false });

            var queryResult = Context.Fetch<Phy>(sql);

            Log.Instance.Trace(Context.LastSQL);
            return queryResult;
        }

        public Phy GetById(double id)
        {
            Log.Instance.Trace($"Entering - id {id}");
            Phy phy = new();
            Pth pth = new();

            phy = Context.SingleOrDefault<Phy>((object)id);
            if(phy == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(phy.PathologistCode))
            {
                pth = Context.SingleOrDefault<Pth>((object)phy.PathologistCode);
                phy.Pathologist = pth;
            }
            
            return phy;
        }

        public override Phy Save(Phy table)
        {
            var existing = GetById(table.uri);

            if(existing != null)
            {
                return this.Update(table);
            }
            else
            {
                return this.Add(table);
            }
        }

    }
}
