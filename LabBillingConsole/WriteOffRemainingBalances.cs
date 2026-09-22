using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using PetaPoco;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LabBillingConsole;

/// <summary>
/// Utility to write off remaining balances on accounts.
/// Used for end-of-period cleanup to write off small remaining balances.
/// </summary>
public sealed class WriteOffRemainingBalances : MenuBase
{
    private readonly IUnitOfWork _uow;
    private readonly AccountService _accountService;
    private readonly DictionaryService _dictionaryService;

    // Default write-off code for small balance write-offs
    private const string DefaultWriteOffCode = "SB";
    private const string DefaultWriteOffComment = "Small balance write-off - End of period cleanup";

    public WriteOffRemainingBalances(IAppEnvironment appEnvironment) : base(appEnvironment)
    {
        _uow = new UnitOfWorkMain(appEnvironment);
        _accountService = new AccountService(appEnvironment);
        _dictionaryService = new DictionaryService(appEnvironment);
    }

    public override bool LaunchMenu()
    {
        // This class is intended to be invoked directly, not through a menu
        return false;
    }

    public override bool ExecuteMenuSelection(string selection)
    {
        return false;
    }

    /// <summary>
    /// Executes the write-off process.
    /// </summary>
    /// <param name="executeImmediately">If true, skips dry run and executes immediately. Default is false (dry run mode).</param>
    /// <param name="writeOffCode">The write-off code to use. If not specified, uses default small balance code.</param>
    /// <returns>True if the operation completed successfully.</returns>
    public bool Execute(bool executeImmediately = false, string writeOffCode = null)
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("Write-Off Balances").Color(Color.Blue));
        Console.WriteLine();

        // Validate write-off code exists
        writeOffCode ??= DefaultWriteOffCode;
        var writeOffCodes = _dictionaryService.GetWriteOffCodes(_uow);
        var selectedWriteOffCode = writeOffCodes.FirstOrDefault(w => w.Code.Equals(writeOffCode, StringComparison.OrdinalIgnoreCase));

        if (selectedWriteOffCode == null)
        {
            AnsiConsole.MarkupLine($"[red]Error: Write-off code '{writeOffCode}' not found in the system.[/]");
            AnsiConsole.MarkupLine("[yellow]Available write-off codes:[/]");

            var table = new Table();
            table.AddColumn("Code");
            table.AddColumn("Description");

            foreach (var code in writeOffCodes.OrderBy(c => c.Code))
            {
                table.AddRow(code.Code, code.Description ?? "");
            }

            AnsiConsole.Write(table);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return false;
        }

        AnsiConsole.MarkupLine($"[green]Using write-off code: {selectedWriteOffCode.Code} - {selectedWriteOffCode.Description}[/]");
        Console.WriteLine();

        // Get accounts with balance > 0
        var accountsWithBalance = GetAccountsWithPositiveBalance();

        if (accountsWithBalance.Count == 0)
        {
            AnsiConsole.MarkupLine("[green]No accounts found with a balance greater than zero.[/]");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return true;
        }

        // Display preview (dry run)
        DisplayAccountsPreview(accountsWithBalance);

        // If execute immediately flag is set, skip confirmation
        if (!executeImmediately)
        {
            Console.WriteLine();
            AnsiConsole.MarkupLine("[yellow]DRY RUN MODE - No changes have been made.[/]");
            Console.WriteLine();

            if (!AnsiConsole.Confirm($"Do you want to proceed with writing off {accountsWithBalance.Count} accounts for a total of {accountsWithBalance.Sum(a => a.Balance):C}?"))
            {
                AnsiConsole.MarkupLine("[yellow]Operation cancelled by user.[/]");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return false;
            }
        }

        // Execute write-offs
        var result = ExecuteWriteOffs(accountsWithBalance, selectedWriteOffCode);

        // Display summary
        DisplaySummary(result);

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();

        return result.Errors.Count == 0;
    }

    /// <summary>
    /// Queries all accounts with a balance greater than zero.
    /// </summary>
    private List<AccountBalanceInfo> GetAccountsWithPositiveBalance()
    {
        var results = new List<AccountBalanceInfo>();

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Querying accounts with positive balance...", ctx =>
            {
                // Query to get accounts with calculated balance > 0
                // Balance = Sum(charges) - Sum(payments + contractual + write-offs)
                var sql = Sql.Builder
                    .Select(new[]
                    {
                        "acc.account",
                        "acc.pat_name",
                        "acc.cl_mnem",
                        "acc.fin_code",
                        "acc.trans_date",
                        "acc.status",
                        @"COALESCE((
                            SELECT SUM(c.qty * c.retail)
                            FROM chrg c
                            WHERE c.account = acc.account
                            AND c.credited = 0
                            AND ((acc.fin_code = 'CLIENT' AND c.status <> 'CBILL')
                                 OR (acc.fin_code <> 'CLIENT' AND c.status NOT IN ('CBILL', 'CAP', 'N/A')))
                        ), 0) -
                        COALESCE((
                            SELECT SUM(ch.amt_paid + ch.contractual + ch.write_off)
                            FROM chk ch
                            WHERE ch.account = acc.account
                        ), 0) as balance"
                    })
                    .From("acc")
                    .Where("acc.deleted = 0");

                // Execute the query
                var rawResults = _uow.Context.Fetch<dynamic>(sql);

                // Filter to only accounts with balance > 0
                foreach (var row in rawResults)
                {
                    double balance = Convert.ToDouble(row.balance);
                    if (balance > 0.005) // Small threshold to avoid floating point issues
                    {
                        results.Add(new AccountBalanceInfo
                        {
                            AccountNo = row.account,
                            PatientName = row.pat_name ?? "",
                            ClientMnem = row.cl_mnem ?? "",
                            FinCode = row.fin_code ?? "",
                            TransactionDate = row.trans_date,
                            Status = row.status ?? "",
                            Balance = Math.Round(balance, 2)
                        });
                    }
                }
            });

        return results.OrderBy(a => a.AccountNo).ToList();
    }

    /// <summary>
    /// Displays a preview of accounts that will be written off.
    /// </summary>
    private void DisplayAccountsPreview(List<AccountBalanceInfo> accounts)
    {
        AnsiConsole.MarkupLine($"[bold]Found {accounts.Count} accounts with balance > $0.00[/]");
        Console.WriteLine();

        // Create a table for display
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("Account");
        table.AddColumn("Patient Name");
        table.AddColumn("Client");
        table.AddColumn("Fin Code");
        table.AddColumn("Service Date");
        table.AddColumn("Status");
        table.AddColumn(new TableColumn("Balance").RightAligned());

        // Limit display to first 100 accounts if there are many
        int displayLimit = Math.Min(accounts.Count, 100);

        foreach (var account in accounts.Take(displayLimit))
        {
            table.AddRow(
                account.AccountNo,
                TruncateString(account.PatientName, 25),
                account.ClientMnem,
                account.FinCode,
                account.TransactionDate.ToString("MM/dd/yyyy"),
                account.Status,
                account.Balance.ToString("C")
            );
        }

        AnsiConsole.Write(table);

        if (accounts.Count > displayLimit)
        {
            AnsiConsole.MarkupLine($"[dim]... and {accounts.Count - displayLimit} more accounts[/]");
        }

        Console.WriteLine();

        // Summary statistics
        var summaryTable = new Table();
        summaryTable.Border(TableBorder.Heavy);
        summaryTable.AddColumn("Metric");
        summaryTable.AddColumn(new TableColumn("Value").RightAligned());

        summaryTable.AddRow("[bold]Total Accounts[/]", accounts.Count.ToString("N0"));
        summaryTable.AddRow("[bold]Total Balance[/]", accounts.Sum(a => a.Balance).ToString("C"));
        summaryTable.AddRow("[bold]Average Balance[/]", accounts.Average(a => a.Balance).ToString("C"));
        summaryTable.AddRow("[bold]Minimum Balance[/]", accounts.Min(a => a.Balance).ToString("C"));
        summaryTable.AddRow("[bold]Maximum Balance[/]", accounts.Max(a => a.Balance).ToString("C"));

        AnsiConsole.Write(summaryTable);
    }

    /// <summary>
    /// Executes write-off transactions for all accounts.
    /// </summary>
    private WriteOffResult ExecuteWriteOffs(List<AccountBalanceInfo> accounts, WriteOffCode writeOffCode)
    {
        var result = new WriteOffResult();

        AnsiConsole.Progress()
            .Columns(new ProgressColumn[]
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new RemainingTimeColumn(),
                new SpinnerColumn()
            })
            .Start(ctx =>
            {
                var task = ctx.AddTask("[green]Processing write-offs[/]", maxValue: accounts.Count);

                foreach (var accountInfo in accounts)
                {
                    try
                    {
                        // Create write-off transaction
                        var chk = new Chk
                        {
                            AccountNo = accountInfo.AccountNo,
                            ChkDate = DateTime.Today,
                            DateReceived = DateTime.Today,
                            CheckNo = $"WO-{DateTime.Now:yyyyMMdd}",
                            PaidAmount = 0,
                            WriteOffAmount = accountInfo.Balance,
                            ContractualAmount = 0,
                            Status = "NEW",
                            Source = "WRITEOFF",
                            FinCode = accountInfo.FinCode,
                            WriteOffDate = DateTime.Today,
                            Comment = DefaultWriteOffComment,
                            WriteOffCode = writeOffCode.Code,
                            PostingDate = DateTime.Today,
                            IsCollectionPmt = false
                        };

                        // Add the payment/write-off using AccountService
                        var savedChk = _accountService.AddPayment(chk, _uow);

                        if (savedChk != null && savedChk.PaymentNo > 0)
                        {
                            result.ProcessedAccounts.Add(accountInfo);
                            result.TotalWrittenOff += accountInfo.Balance;

                            Log.Instance.Info($"Write-off created for account {accountInfo.AccountNo}: ${accountInfo.Balance:F2} (PaymentNo: {savedChk.PaymentNo})");
                        }
                        else
                        {
                            result.Errors.Add(new WriteOffError
                            {
                                AccountNo = accountInfo.AccountNo,
                                ErrorMessage = "Failed to create write-off transaction - no payment number returned"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add(new WriteOffError
                        {
                            AccountNo = accountInfo.AccountNo,
                            ErrorMessage = ex.Message
                        });

                        Log.Instance.Error($"Error writing off account {accountInfo.AccountNo}: {ex.Message}", ex);
                    }

                    task.Increment(1);
                }
            });

        return result;
    }

    /// <summary>
    /// Displays a summary of the write-off operation.
    /// </summary>
    private void DisplaySummary(WriteOffResult result)
    {
        Console.WriteLine();
        AnsiConsole.Write(new Rule("[bold blue]Write-Off Summary[/]").RuleStyle("blue"));
        Console.WriteLine();

        var summaryTable = new Table();
        summaryTable.Border(TableBorder.Heavy);
        summaryTable.AddColumn("Metric");
        summaryTable.AddColumn(new TableColumn("Value").RightAligned());

        summaryTable.AddRow("[green]Accounts Successfully Processed[/]", result.ProcessedAccounts.Count.ToString("N0"));
        summaryTable.AddRow("[green]Total Amount Written Off[/]", result.TotalWrittenOff.ToString("C"));

        if (result.Errors.Count > 0)
        {
            summaryTable.AddRow("[red]Accounts with Errors[/]", result.Errors.Count.ToString("N0"));
        }

        AnsiConsole.Write(summaryTable);

        // Display errors if any
        if (result.Errors.Count > 0)
        {
            Console.WriteLine();
            AnsiConsole.MarkupLine("[red bold]Errors encountered:[/]");

            var errorTable = new Table();
            errorTable.Border(TableBorder.Rounded);
            errorTable.AddColumn("Account");
            errorTable.AddColumn("Error");

            foreach (var error in result.Errors.Take(50)) // Limit to first 50 errors
            {
                errorTable.AddRow(error.AccountNo, TruncateString(error.ErrorMessage, 80));
            }

            AnsiConsole.Write(errorTable);

            if (result.Errors.Count > 50)
            {
                AnsiConsole.MarkupLine($"[dim]... and {result.Errors.Count - 50} more errors[/]");
            }
        }
    }

    private static string TruncateString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return "";
        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 3) + "...";
    }

    #region Helper Classes

    private sealed class AccountBalanceInfo
    {
        public string AccountNo { get; set; }
        public string PatientName { get; set; }
        public string ClientMnem { get; set; }
        public string FinCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public double Balance { get; set; }
    }

    private sealed class WriteOffResult
    {
        public List<AccountBalanceInfo> ProcessedAccounts { get; } = new List<AccountBalanceInfo>();
        public List<WriteOffError> Errors { get; } = new List<WriteOffError>();
        public double TotalWrittenOff { get; set; }
    }

    private sealed class WriteOffError
    {
        public string AccountNo { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion
}
