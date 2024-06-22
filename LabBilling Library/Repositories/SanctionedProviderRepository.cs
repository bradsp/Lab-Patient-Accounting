using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System.Data;
using System;
using LabBilling.Logging;
using System.Collections.Generic;

namespace LabBilling.Core.Repositories;
public class SanctionedProviderRepository : RepositoryBase<SanctionedProvider>, IRepositoryBase<SanctionedProvider>
{
    public SanctionedProviderRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public SanctionedProvider GetByNPI(string npi)
    {
        Log.Instance.Trace($"Entering - npi {npi}");
        SanctionedProvider phy = new();

        if (!string.IsNullOrEmpty(npi))
            phy = Context.SingleOrDefault<SanctionedProvider>($"where {GetRealColumn(nameof(SanctionedProvider.NPI))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = npi });

        return phy;
    }

    public List<SanctionedProvider> GetByName(string lastName, string firstName)
    {
        Log.Instance.Trace($"Entering - name {lastName} {firstName}");
        List<SanctionedProvider> phy;

        if (!string.IsNullOrEmpty(lastName) || !string.IsNullOrEmpty(firstName))
        {
            var command = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{this.GetRealColumn(nameof(SanctionedProvider.LastName))} like @0+'%'",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = lastName })
                .Where($"{this.GetRealColumn(nameof(SanctionedProvider.FirstName))} like @0+'%'",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = firstName })
                .OrderBy($"{this.GetRealColumn(nameof(SanctionedProvider.LastName))}, {this.GetRealColumn(nameof(SanctionedProvider.FirstName))}");

            phy = Context.Fetch<SanctionedProvider>(command);

            return phy;
        }

        return null;
    }

}
