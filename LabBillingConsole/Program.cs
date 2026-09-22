using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using Spectre.Console;
using System;
using System.Linq;
using System.Text;

namespace LabBillingConsole;

internal class Program
{
    static void Main(string[] args)
    {
        // Check for command-line arguments for automated operations
        if (args.Length > 0)
        {
            if (HandleCommandLineArgs(args))
            {
                return;
            }
        }

        // Default interactive menu mode
        bool showMenu = true;

        while (showMenu)
        {
            showMenu = MainMenuPanel();
        }
    }

    /// <summary>
    /// Handles command-line arguments for automated/scripted operations.
    /// </summary>
    /// <param name="args">Command-line arguments</param>
    /// <returns>True if a command was handled, false to continue to interactive menu</returns>
    private static bool HandleCommandLineArgs(string[] args)
    {
        // Parse arguments
        string command = args.FirstOrDefault()?.ToLowerInvariant();
        bool execute = args.Contains("--execute", StringComparer.OrdinalIgnoreCase);
        string database = GetArgValue(args, "--database") ?? GetArgValue(args, "-d");
        string server = GetArgValue(args, "--server") ?? GetArgValue(args, "-s") ?? "WTH014";
        string writeOffCode = GetArgValue(args, "--code") ?? GetArgValue(args, "-c");

        // Show help
        if (command == "--help" || command == "-h" || command == "help")
        {
            ShowHelp();
            return true;
        }

        // Handle write-off command
        if (command == "writeoff" || command == "write-off")
        {
            if (string.IsNullOrEmpty(database))
            {
                AnsiConsole.MarkupLine("[red]Error: --database (-d) is required for command-line execution.[/]");
                AnsiConsole.MarkupLine("[yellow]Usage: LabBillingConsole writeoff --database LabBillingTest [--execute] [--code SB][/]");
                return true;
            }

            return ExecuteWriteOffCommand(server, database, execute, writeOffCode);
        }

        // Unknown command
        if (command != null && command.StartsWith("-"))
        {
            // Looks like an argument, not a command - fall through to interactive
            return false;
        }

        if (!string.IsNullOrEmpty(command))
        {
            AnsiConsole.MarkupLine($"[red]Unknown command: {command}[/]");
            ShowHelp();
            return true;
        }

        return false;
    }

    private static string GetArgValue(string[] args, string argName)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i].Equals(argName, StringComparison.OrdinalIgnoreCase))
            {
                return args[i + 1];
            }
        }
        return null;
    }

    private static void ShowHelp()
    {
        Console.WriteLine();
        AnsiConsole.Write(new FigletText("Lab Billing Console").Color(Color.Blue));
        Console.WriteLine();

        AnsiConsole.MarkupLine("[bold]Usage:[/]");
        AnsiConsole.MarkupLine("  LabBillingConsole [command] [options]");
        Console.WriteLine();

        AnsiConsole.MarkupLine("[bold]Commands:[/]");
        AnsiConsole.MarkupLine("  writeoff    Write off remaining balances on all accounts with balance > 0");
        AnsiConsole.MarkupLine("  help        Show this help message");
        Console.WriteLine();

        AnsiConsole.MarkupLine("[bold]Write-off Options:[/]");
        AnsiConsole.MarkupLine("  --database, -d <name>    Database name (required for command-line mode)");
        AnsiConsole.MarkupLine("  --server, -s <name>      Server name (default: WTH014)");
        AnsiConsole.MarkupLine("  --execute                Execute immediately without confirmation prompt");
        AnsiConsole.MarkupLine("  --code, -c <code>        Write-off code to use (default: SB)");
        Console.WriteLine();

        AnsiConsole.MarkupLine("[bold]Examples:[/]");
        AnsiConsole.MarkupLine("  [dim]# Dry run - preview accounts that would be written off[/]");
        AnsiConsole.MarkupLine("  LabBillingConsole writeoff --database LabBillingTest");
        Console.WriteLine();
        AnsiConsole.MarkupLine("  [dim]# Execute write-offs immediately (for scripted use)[/]");
        AnsiConsole.MarkupLine("  LabBillingConsole writeoff --database LabBillingTest --execute");
        Console.WriteLine();
        AnsiConsole.MarkupLine("  [dim]# Use a specific write-off code[/]");
        AnsiConsole.MarkupLine("  LabBillingConsole writeoff --database LabBillingProd --code SB --execute");
        Console.WriteLine();
    }

    private static bool ExecuteWriteOffCommand(string server, string database, bool execute, string writeOffCode)
    {
        try
        {
            AnsiConsole.MarkupLine($"[bold]Connecting to {server}/{database}...[/]");

            var appEnvironment = new AppEnvironment
            {
                ServerName = server,
                DatabaseName = database,
                LogDatabaseName = "Log",
                UserName = Environment.UserName,
                IntegratedAuthentication = true
            };

            var writeOffUtil = new WriteOffRemainingBalances(appEnvironment);
            return writeOffUtil.Execute(executeImmediately: execute, writeOffCode: writeOffCode);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            Log.Instance.Error("Error executing write-off command", ex);
            return false;
        }
    }

    private static bool MainMenuPanel()
    {
        string serverName;
        string databaseName;

        StringBuilder dbSelect = new();
        dbSelect.AppendLine("Select Database:\n\n");
        dbSelect.AppendLine("1) LabBillingProd");
        dbSelect.AppendLine("2) LabBillingTest");
        dbSelect.AppendLine("0) Exit");

        var panel1 = new Panel(dbSelect.ToString());
        panel1.Header = new PanelHeader("Lab Patient Accounting");
        panel1.Border = BoxBorder.Square;
        panel1.Expand = true;
        panel1.Padding = new Padding(2, 2, 2, 2);

        AnsiConsole.Write(panel1);

        var menuSelect = AnsiConsole.Prompt(new TextPrompt<int>("Select database: ")
            .PromptStyle("green")
            .Validate(db =>
            {
                if (db == 0 || db == 1 || db == 2 || db == 3)
                    return ValidationResult.Success();
                else
                    return ValidationResult.Error("Invalid selection.");
            }));

        serverName = "WTH014";

        switch (menuSelect)
        {
            case 0:
                return false;
            case 1:
                databaseName = "LabBillingProd";
                serverName = "WTH014";
                break;
            case 2:
                databaseName = "LabBillingTest";
                serverName = "WTH014";
                break;
            default:
                return true;
                //break;
        }

        MainMenu mainMenu = new(serverName, databaseName);

        return mainMenu.MainMenuPanel();

    }

}
