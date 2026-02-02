using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LabBilling.Core.Repositories;

public class RandomDrugScreenPersonRepository : RepositoryBase<RandomDrugScreenPerson>
{
    public RandomDrugScreenPersonRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
    {
    }

    /// <summary>
    /// Gets candidates by client mnemonic
    /// </summary>
    public async Task<List<RandomDrugScreenPerson>> GetByClientAsync(string clientMnem, bool includeDeleted = false)
    {
        var sql = Sql.Builder
            .Select("*")
            .From(_tableName)
            .Where("cli_mnem = @0", clientMnem)
            .Where("(@0 = 1 OR deleted = 0)",
                new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = includeDeleted });

        var result = await Context.FetchAsync<RandomDrugScreenPerson>(sql);
        return result.ToList();
    }

    /// <summary>
    /// Gets candidates by client and shift
    /// </summary>
    public async Task<List<RandomDrugScreenPerson>> GetByClientAndShiftAsync(string clientMnem, string shift, bool includeDeleted = false)
    {
        var sql = Sql.Builder
            .Select("*")
            .From(_tableName)
            .Where("cli_mnem = @0", clientMnem)
            .Where("shift = @0", shift)
            .Where("(@0 = 1 OR deleted = 0)",
                new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = includeDeleted });

        var result = await Context.FetchAsync<RandomDrugScreenPerson>(sql);
        return result.ToList();
    }

    /// <summary>
    /// Gets distinct client mnemonics
    /// </summary>
    public async Task<List<string>> GetDistinctClientsAsync()
    {
 var sql = Sql.Builder
      .Select("DISTINCT cli_mnem")
 .From(_tableName)
     .Where("deleted = 0")
        .OrderBy("cli_mnem");

        var result = await Context.FetchAsync<string>(sql);
        return result.ToList();
    }

    /// <summary>
    /// Gets distinct shifts, optionally filtered by client
    /// </summary>
    public async Task<List<string>> GetDistinctShiftsAsync(string clientMnem = null)
    {
        var sql = Sql.Builder
            .Select("DISTINCT shift")
            .From(_tableName)
            .Where("deleted = 0")
            .Where("shift IS NOT NULL")
            .Where("shift <> ''")
            .Where("(@0 IS NULL OR cli_mnem = @0)",
                string.IsNullOrEmpty(clientMnem)
                    ? (object)DBNull.Value
                    : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem })
            .OrderBy("shift");

        var result = await Context.FetchAsync<string>(sql);
        return result.ToList();
    }

    /// <summary>
    /// Gets count of candidates matching criteria
    /// </summary>
    public async Task<int> GetCandidateCountAsync(string clientMnem, string shift = null, bool includeDeleted = false)
    {
        var sql = Sql.Builder
            .Select("COUNT(*)")
            .From(_tableName)
            .Where("cli_mnem = @0", clientMnem)
            .Where("(@0 IS NULL OR shift = @0)",
                string.IsNullOrEmpty(shift)
                    ? (object)DBNull.Value
                    : new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = shift })
            .Where("(@0 = 1 OR deleted = 0)",
                new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = includeDeleted });

        var result = await Context.ExecuteScalarAsync<int>(sql);
        return result;
    }

    /// <summary>
    /// Soft deletes all candidates for a client
    /// </summary>
    public async Task<int> SoftDeleteByClientAsync(string clientMnem)
    {
  var sql = Sql.Builder
  .Append("UPDATE " + _tableName)
.Append("SET deleted = 1,")
            .Append("mod_date = GETDATE(),")
.Append("mod_user = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName })
   .Append("mod_prg = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Utilities.OS.GetAppName() })
      .Append("mod_host = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName })
 .Where("cli_mnem = @0", clientMnem);

        var result = await Context.ExecuteAsync(sql);
        return result;
    }

    /// <summary>
    /// Marks candidates as deleted if their names are not in the provided list
    /// </summary>
    public async Task<int> MarkMissingAsDeletedAsync(string clientMnem, List<string> existingNames)
    {
        if (existingNames == null || existingNames.Count == 0)
        {
     return await SoftDeleteByClientAsync(clientMnem);
        }

        var sql = Sql.Builder
    .Append("UPDATE " + _tableName)
       .Append("SET deleted = 1,")
 .Append("mod_date = GETDATE(),")
      .Append("mod_user = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName })
          .Append("mod_prg = @0,", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Utilities.OS.GetAppName() })
    .Append("mod_host = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName })
       .Where("cli_mnem = @0", clientMnem)
      .Where("name NOT IN (@0)", existingNames);

        var result = await Context.ExecuteAsync(sql);
    return result;
    }

    /// <summary>
    /// Gets a candidate by key async
    /// </summary>
    public async Task<RandomDrugScreenPerson> GetByKeyAsync(object key)
    {
    return await Task.Run(() => GetByKey(key));
  }
}
