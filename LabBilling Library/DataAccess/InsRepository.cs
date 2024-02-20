using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Utilities;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using PetaPoco;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess;

public sealed class InsRepository : RepositoryBase<Ins>
{
    private readonly DictionaryService dictionaryService;
    public InsRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
        dictionaryService = new(appEnvironment);
    }

    public List<Ins> GetByAccount(string account)
    {
        Log.Instance.Debug($"Entering - account {account}");
        var sql = Sql.Builder
            .Where($"{GetRealColumn(nameof(Ins.Account))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
            .OrderBy($"{GetRealColumn(nameof(Ins.Coverage))}");

        var records = Context.Fetch<Ins>(sql);

        foreach(Ins ins in records)
        {
            if (!string.IsNullOrEmpty(ins.HolderFullName))
            {
                if (string.IsNullOrEmpty(ins.HolderLastName) || string.IsNullOrEmpty(ins.HolderFirstName))
                {
                    if (!StringExtensions.ParseName(ins.HolderFullName.ToString(),
                        out string lname, out string fname, out string mname, out string suffix))
                    {
                        //error parsing name
                        Log.Instance.Info($"Insurance holder name could not be parsed. {ins.HolderFullName}");
                    }

                    ins.HolderLastName = lname;
                    ins.HolderFirstName = fname;
                    ins.HolderMiddleName = mname;
                }
                else
                {
                    ins.HolderFullName = $"{ins.HolderLastName},{ins.HolderFirstName} {ins.HolderMiddleName}";
                }
            }
            else
            {
                ins.HolderFullName = string.Empty;
            }

            if (ins.InsCode != null)
            {
                ins.InsCompany = dictionaryService.GetInsCompany(ins.InsCode);
            }
            ins.InsCompany ??= new InsCompany();

            StringExtensions.ParseCityStZip(ins.HolderCityStZip, out string strCity, out string strState, out string strZip);
            ins.HolderCity = strCity;
            ins.HolderState = strState;
            ins.HolderZip = strZip;

            StringExtensions.ParseCityStZip(ins.PlanCityState, out strCity, out strState, out strZip);
            ins.PlanCity = strCity;
            ins.PlanState = strState;
            ins.PlanZip = strZip;

        }

        return records;
    }
    
    public Ins GetByAccount(string account, InsCoverage coverage)
    {
        Log.Instance.Trace($"Entering - account {account} coverage {coverage.ToString()}");

        if(!coverage.IsValid)
        {
            throw new ArgumentNullException(nameof(coverage));
        }

        var sql = Sql.Builder
            .Where($"{GetRealColumn(nameof(Ins.Account))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account })
            .Where($"{GetRealColumn(nameof(Ins.Coverage))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = coverage.ToString() });

        var record = Context.SingleOrDefault<Ins>(sql);
        if (record != null)
        {
            if (record.InsCode != null)
            {
                record.InsCompany = dictionaryService.GetInsCompany(record.InsCode);
            }
            record.InsCompany ??= new InsCompany();

            StringExtensions.ParseCityStZip(record.HolderCityStZip, out string strCity, out string strState, out string strZip);
            record.HolderCity = strCity;
            record.HolderState = strState;
            record.HolderZip = strZip;

            StringExtensions.ParseCityStZip(record.PlanCityState, out strCity, out strState, out strZip);
            record.PlanCity = strCity;
            record.PlanState = strState;
            record.PlanZip = strZip;

        }
        return record;
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
        var insurances = GetByAccount(table.Account);

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

        if(cols.Contains(nameof(Ins.HolderState)) || 
            cols.Contains(nameof(Ins.HolderCity)) || 
            cols.Contains(nameof(Ins.HolderZip)))
        {
            cols.Add(nameof(Ins.HolderCityStZip));
            cols.Remove(nameof(Ins.HolderState));
            cols.Remove(nameof(Ins.HolderCity));
            cols.Remove(nameof(Ins.HolderZip));
        }
        if(cols.Contains(nameof(Ins.HolderLastName)) || 
            cols.Contains(nameof(Ins.HolderFirstName)) || 
            cols.Contains(nameof(Ins.HolderMiddleName)))
        {
            cols.Add(nameof(Ins.HolderFullName));
        }
        return base.Update(table, cols);
    }
}
