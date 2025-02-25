﻿using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Utilities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess;

/// <summary>
/// 
/// </summary>
public sealed class PatRepository : RepositoryBase<Pat>
{

    public PatRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public bool RecordExists(string accountNo)
    {
        var pat = Context.SingleOrDefault<Pat>($"where account = @0",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

        if (pat == null)
            return false;
        else
            return true;
    }

    public Pat GetPatByAccount(Account account)
    {
        Log.Instance.Trace($"Entering - account {account}");

        var record = Context.SingleOrDefault<Pat>("where account = @0",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account.AccountNo });

        if (record == null)
        {
            //if there ios not a pat record, create one. All accounts must have a pat record

            record = new Pat
            {
                AccountNo = account.AccountNo
            };

            Add(record);

            record = Context.SingleOrDefault<Pat>("where account = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account.AccountNo });

            if (record == null)
                return null;
        }



        if (!StringExtensions.ParseName(record.GuarantorFullName, out string strGuarLastName, out string strGuarFirstName, out string strGuarMidName, out string strGuarSuffix))
        {
            if (!string.IsNullOrEmpty(this.Errors))
                this.Errors += Environment.NewLine;

            this.Errors += $"Guarantor name could not be parsed. {record.GuarantorFullName} {record.AccountNo}";
        }
        else
        {
            record.GuarantorLastName = strGuarLastName;
            record.GuarantorFirstName = strGuarFirstName;
            record.GuarantorMiddleName = strGuarMidName;
            record.GuarantorNameSuffix = strGuarSuffix;
        }

        if (!StringExtensions.ParseCityStZip(record.CityStateZip, out string strCity, out string strState, out string strZip))
        {
            this.Errors += $"Patient CityStZip could not be parsed. {record.CityStateZip} - {record.AccountNo}";
        }
        else
        {
            record.City = strCity;
            record.State = strState;
            record.ZipCode = strZip;
        }

        if(string.IsNullOrEmpty(record.MaritalStatus))
        {
            record.MaritalStatus = "U";
        }



        return record;
    }

    public Pat SaveDiagnoses(Pat pat)
    {
        Log.Instance.Trace($"Entering - account {pat.AccountNo}");
        // this function will validate and save the dx from the model in both the pat record and in the patdx table

        //first - the updated diagnoses is in the PatDiag object. We need to update the individual fields in the Pat object from this
        // clear the individual fields
        pat.Dx1 = "";
        pat.Dx2 = "";
        pat.Dx3 = "";
        pat.Dx4 = "";
        pat.Dx5 = "";
        pat.Dx6 = "";
        pat.Dx7 = "";
        pat.Dx8 = "";
        pat.Dx9 = "";

        foreach (PatDiag dx in pat.Diagnoses)
        {
            //check the individual dx code and update
            switch (dx.No)
            {
                case 1:
                    pat.Dx1 = dx.Code;
                    break;
                case 2:
                    pat.Dx2 = dx.Code;
                    break;
                case 3:
                    pat.Dx3 = dx.Code;
                    break;
                case 4:
                    pat.Dx4 = dx.Code;
                    break;
                case 5:
                    pat.Dx5 = dx.Code;
                    break;
                case 6:
                    pat.Dx6 = dx.Code;
                    break;
                case 7:
                    pat.Dx7 = dx.Code;
                    break;
                case 8:
                    pat.Dx8 = dx.Code;
                    break;
                case 9:
                    pat.Dx9 = dx.Code;
                    break;
                default:
                    break;
            }
        }

        return Update(pat);

    }

    public override Pat Save(Pat model)
    {
        try
        {
            if(RecordExists(model.AccountNo))
                return this.Update(model);
            else
                return this.Add(model);
        }
        catch(Exception ex)
        {
            throw new ApplicationException("Error in PatRepository.Save", ex);
        }
    }

    public override Pat Add(Pat table)
    {
        table.GuarantorFullName =
            String.Format("{0},{1} {2} {3}",
            table.GuarantorLastName,
            table.GuarantorFirstName,
            table.GuarantorMiddleName,
            table.GuarantorNameSuffix);
        table.GuarantorFullName = table.GuarantorFullName.Trim();

        table.GuarantorCityState = $"{table.GuarantorCity}, {table.GuarantorState} {table.GuarantorZipCode}";

        table.CityStateZip = $"{table.City}, {table.State} {table.ZipCode}";

        if (string.IsNullOrWhiteSpace(table.StatementFlag))
            table.StatementFlag = "N";

        return base.Add(table);
    }

    public override Pat Update(Pat table)
    {
        Log.Instance.Trace($"Entering - account {table.AccountNo}");

        table.GuarantorFullName =
            String.Format("{0},{1} {2} {3}",
            table.GuarantorLastName,
            table.GuarantorFirstName,
            table.GuarantorMiddleName,
            table.GuarantorNameSuffix);
        table.GuarantorFullName = table.GuarantorFullName.Trim();

        if (string.IsNullOrWhiteSpace(table.StatementFlag))
            table.StatementFlag = "N";

        return base.Update(table);
    }

    public override Pat Update(Pat table, IEnumerable<string> columns)
    {
        Log.Instance.Trace($"Entering - account {table.AccountNo}");
        table.GuarantorFullName =
            String.Format("{0},{1} {2} {3}",
            table.GuarantorLastName,
            table.GuarantorFirstName,
            table.GuarantorMiddleName,
            table.GuarantorNameSuffix);
        table.GuarantorFullName = table.GuarantorFullName.Trim();

        if (string.IsNullOrWhiteSpace(table.StatementFlag))
            table.StatementFlag = "N";

        return base.Update(table, columns);
    }

    public Pat SetStatementFlag(Pat model, string flag)
    {
        string[] validFlags = { "N", "Y", "1", "2", "3", "4" };
        if (!validFlags.Contains(flag))
            throw new ApplicationException("Invalid statement flag");

        model.StatementFlag = flag;

        return Update(model, new[] { nameof(Pat.StatementFlag) });
    }

    public int SetStatementFlag(string account, string flag)
    {
        string[] validFlags = { "N", "Y", "1", "2", "3", "4" };

        if(!validFlags.Contains(flag))
            throw new ApplicationException("Invalid statement flag");

        var sql = PetaPoco.Sql.Builder;

        sql.Set($"{GetRealColumn(nameof(Pat.StatementFlag))}=@0",
            new SqlParameter() { DbType = DbType.String, Value = flag });
        sql.Where($"{GetRealColumn(nameof(Pat.AccountNo))}=@0",
            new SqlParameter() { DbType = DbType.String, Value = account });
        try
        {
            var result = Context.Update<Pat>(sql);
            return result;
        }
        catch(Exception ex)
        {
            throw new ApplicationException($"Exception updating statement flag on {account}", ex);
        }

    }
}
