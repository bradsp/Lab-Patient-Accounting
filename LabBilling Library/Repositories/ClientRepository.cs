using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace LabBilling.Core.DataAccess;

public sealed class ClientRepository : RepositoryBase<Client>
{

    public ClientRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    public async Task<List<Client>> GetAllAsync(bool includeInactive = false) => await Task.Run(() => GetAllAsync(includeInactive));

    public List<Client> GetAll(bool includeInactive = false)
    {
        Log.Instance.Trace("Entering");

        string clientTypeTableName = ClientTypeRepository.GetTableInfo(typeof(ClientType)).TableName;

        PetaPoco.Sql sql = Sql.Builder
            .Select("*")
            .From(_tableName)
            .LeftJoin(clientTypeTableName)
            .On($"{clientTypeTableName}.{GetRealColumn(typeof(ClientType), nameof(ClientType.Type))} = {_tableName}.{GetRealColumn(nameof(Client.Type))}");

        if (!includeInactive)
            sql.Where($"{GetRealColumn(nameof(Client.IsDeleted))} = @0", false);

        var queryResult = Context.Fetch<Client, ClientType, Client>((c, ct) => { c.ClientType = ct; return c; }, sql);

        Log.Instance.Debug(Context.LastSQL);

        return queryResult;
    }

    public Client GetClient(string clientMnem)
    {
        Log.Instance.Debug($"Entering - {clientMnem}");

        if (clientMnem == null)
        {
            throw new ArgumentNullException(nameof(clientMnem));
        }

        var record = Context.SingleOrDefault<Client>($"where {GetRealColumn(nameof(Client.ClientMnem))} = @0",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
        Log.Instance.Debug(Context.LastSQL);

        return record;
    }

    public override Client Add(Client model)
    {
        if (string.IsNullOrEmpty(model.BillProfCharges))
            model.BillProfCharges = "NO";

        if (string.IsNullOrEmpty(model.BillMethod))
            model.BillMethod = "PER ACCOUNT";

        return base.Add(model);
    }

    /// <summary>
    /// Compute the client balance. Uses the vw_chrg_bal_cbill and vw_chk_bal_cbill
    /// which is essentially the same as the vw_chrg_bal and vw_chk_bal
    /// except it excludes charges/checks that have not been on a cbill
    /// (by using the invoice field).
    /// </summary>
    /// <param name="clientMnem"></param>
    /// <returns></returns>
    public double Balance(string clientMnem)
    {
        Log.Instance.Debug($"Entering");

        if (clientMnem == null)
            throw new ArgumentNullException(nameof(clientMnem));

        var balanceReturn = Context.ExecuteScalar<double>("select dbo.GetAccBalance(@0)",
           new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });

        return balanceReturn;

    }

    public double Balance(string clientMnem, DateTime asOfDate)
    {
        if (clientMnem == null)
            throw new ArgumentNullException(nameof(clientMnem));
        if (asOfDate > DateTime.Now)
            throw new ArgumentOutOfRangeException(nameof(asOfDate));

        var balance = Context.ExecuteScalar<double>("select dbo.GetAccBalByDate(@0, @1)",
            new SqlParameter() { ParameterName = "@account", SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem },
            new SqlParameter() { ParameterName = "@effDate", SqlDbType = System.Data.SqlDbType.DateTime, Value = asOfDate });

        return balance;
    }

    public async Task<List<UnbilledClient>> GetUnbilledClientsAsync(DateTime thruDate, IProgress<int> progress) => await Task.Run(() => GetUnbilledClients(thruDate, progress));

    public event EventHandler<ProgressEventArgs> ProgressChanged;

    public List<UnbilledClient> GetUnbilledClients(DateTime thruDate, IProgress<int> progress)
    {
        AccountService accountService = new(AppEnvironment);

        List<UnbilledClient> unbilledClients = new();

        var unbilledAccounts = GetUnbilledAccounts(thruDate, progress);

        var clients = GetAll(false);

        int total = clients.Count;
        int processed = 0;

        clients.AsParallel().ForAll(client =>
        {
            UnbilledClient unbilledClient = new()
            {
                ClientMnem = client.ClientMnem,
                ClientName = client.Name,
                ClientType = client.ClientType.Description,
                PriorBalance = accountService.GetBalance(client.ClientMnem),
                UnbilledAccounts = unbilledAccounts.Where(x => x.ClientMnem == client.ClientMnem).ToList()
            };

            if (unbilledClient.PriorBalance != 0 || unbilledClient.UnbilledAccounts.Sum(x => x.UnbilledAmount) != 0)
                unbilledClients.Add(unbilledClient);

            progress?.Report(HelperExtensions.ComputePercentage(++processed, total));
        });

        return unbilledClients;
    }

    private List<UnbilledAccounts> GetUnbilledAccounts(DateTime thruDate, IProgress<int> progress)
    {
        string chrgTableName = GetTableInfo(typeof(Chrg)).TableName;
        string accTableName = GetTableInfo(typeof(Account)).TableName;

        var cmd = Sql.Builder
            .Select(new[]
            {
                chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo)),
                chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.ClientMnem)),
                accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.TransactionDate)),
                accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.PatFullName)),
                chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.FinancialType)),
                $"dbo.GetAccClientBalance({chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo))}, {chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.ClientMnem))}) as {GetRealColumn(typeof(UnbilledAccounts), nameof(UnbilledAccounts.UnbilledAmount))}"
            })
            .From(chrgTableName)
            .InnerJoin(accTableName)
            .On($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo))} = {accTableName}.{GetRealColumn(typeof(Account), nameof(Account.AccountNo))}")
            .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Status))} not in ('CBILL','N/A')")
            .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} is null or {chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.Invoice))} = ''")
            .Where($"{chrgTableName}.{GetRealColumn(typeof(Chrg), nameof(Chrg.FinancialType))} =  'C'")
            .Where($"{accTableName}.{GetRealColumn(typeof(Account), nameof(Account.Status))} not like '%HOLD%'")
            .Where($"{accTableName}.{GetRealColumn(typeof(Account), nameof(Account.TransactionDate))} <= @0 ",
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate })
            .GroupBy(new[]
            {
                chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.AccountNo)),
                chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.ClientMnem)),
                accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.TransactionDate)),
                accTableName + "." + GetRealColumn(typeof(Account), nameof(Account.PatFullName)),
                chrgTableName + "." + GetRealColumn(typeof(Chrg), nameof(Chrg.FinancialType))
            });

        var results = Context.Fetch<UnbilledAccounts>(cmd);

        return results.ToList(); // Where(x => x.UnbilledAmount != 0.00).ToList();
    }
}
