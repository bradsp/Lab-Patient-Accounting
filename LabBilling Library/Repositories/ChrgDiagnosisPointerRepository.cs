using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess;

public class ChrgDiagnosisPointerRepository : RepositoryBase<ChrgDiagnosisPointer>
{
    public ChrgDiagnosisPointerRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public ChrgDiagnosisPointer GetById(int id)
    {
        return Context.SingleOrDefault<ChrgDiagnosisPointer>((object)id);
    }

    public ChrgDiagnosisPointer GetById(double id)
    {
        return Context.SingleOrDefault<ChrgDiagnosisPointer>((object)id);
    }

    public List<ChrgDiagnosisPointer> GetByAccount(string accountNo)
    {
        var cmd = Sql.Builder
            .Where($"{GetRealColumn(nameof(ChrgDiagnosisPointer.AccountNo))} = @0",
            new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = accountNo });

        return Context.Fetch<ChrgDiagnosisPointer>(cmd);
    }

    public override ChrgDiagnosisPointer Save(ChrgDiagnosisPointer record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (record.Id > 0)
        {
            return Update(record);
        }
        else
        {
            return Add(record);
        }
    }

}
