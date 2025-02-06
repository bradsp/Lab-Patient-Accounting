using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Utilities;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using PetaPoco;

namespace LabBilling.Core.DataAccess;

public sealed class InsRepository : RepositoryBase<Ins>
{

    public InsRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public List<Ins> GetInsByAccount(string account)
    {
        Log.Instance.Debug($"Entering - account {account}");
        var sql = Sql.Builder
            .Where($"{GetRealColumn(nameof(Ins.Account))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
            .OrderBy($"{GetRealColumn(nameof(Ins.Coverage))}");

        var records = Context.Fetch<Ins>(sql);

        return records;
    }
    

    public override Ins Add(Ins model)
    {
        model.HolderCityStZip = $"{model.HolderCity}, {model.HolderState} {model.HolderZip}";
        model.HolderFullName = $"{model.HolderLastName},{model.HolderFirstName} {model.HolderMiddleName}".TrimEnd();
        return base.Add(model);
    }

    public override bool Delete(Ins table)
    {
        var result = base.Delete(table);

        //deleting an insurance may result in other insurances being reassigned ordering
        var insurances = GetInsByAccount(table.Account);

        int iteration = 1;
        foreach(var ins in insurances)
        {
            switch(iteration)
            {
                case 1:
                    if(ins.Coverage != "A")
                    {
                        ins.Coverage = "A";
                        Update(ins);
                    }
                    break;
                case 2:
                    if(ins.Coverage != "B")
                    {
                        ins.Coverage = "B";
                        Update(ins);
                    }
                    break;
                case 3:
                    if(ins.Coverage != "C")
                    {
                        ins.Coverage = "C";
                        Update(ins);
                    }
                    break;
                default:
                    break;
            }
            iteration++;
        }

        return result;
    }

    public override Ins Save(Ins table)
    {
        if (table.Coverage == null)
            throw new ApplicationException($"Coverage value on Ins record is not valid. Account {table.Account} Plan {table.PlanName}");

        try
        {
            if (table.rowguid == Guid.Empty)
                return this.Add(table);
            else
                return this.Update(table);
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Exception in InsRepository.Save", ex);         
        }
    }

    public override Ins Update(Ins table, IEnumerable<string> columns)
    {
        Log.Instance.Trace($"Entering - account {table.Account}");
        List<string> cols = columns.ToList();

        if(cols.Contains(nameof(Ins.HolderLastName)) || 
            cols.Contains(nameof(Ins.HolderFirstName)) || 
            cols.Contains(nameof(Ins.HolderMiddleName)))
        {
            cols.Add(nameof(Ins.HolderFullName));
        }
        return base.Update(table, cols);
    }

    /// <summary>
    /// Deletes all insurance records for an account.
    /// </summary>
    /// <param name="account"></param>
    /// <returns>Number of rows deleted.</returns>
    public int DeleteAccountInsurances(string account)
    {
        Log.Instance.Trace($"Entering - account {account}");

        string sql = $"where {GetRealColumn(nameof(Ins.Account))} = @0";

        return Context.Delete<Ins>(sql, new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
    }
}
