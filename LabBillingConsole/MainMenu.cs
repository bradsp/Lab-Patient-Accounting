using LabBilling.Core.DataAccess;
using Spectre.Console;
using System;
using System.Text;

namespace LabBillingConsole;

public class MainMenu
{
    private AppEnvironment _appEnvironment;
    private Utilities _utilities;
    private Testing _testing;

    public MainMenu(string servername, string databasename)
    {
        if (string.IsNullOrEmpty(servername))
            throw new ArgumentNullException(nameof(servername));
        if (string.IsNullOrEmpty(databasename))
            throw new ArgumentNullException(nameof(databasename));

        _appEnvironment = new AppEnvironment
        {
            ServerName = servername,
            DatabaseName = databasename,
            LogDatabaseName = "Log"
        };
    }

    public bool MainMenuPanel()
    {
        _appEnvironment.UserName = Environment.UserName;
        _appEnvironment.IntegratedAuthentication = true;

        _testing = new Testing(_appEnvironment);
        _utilities = new Utilities(_appEnvironment);

        bool showMenu = true;

        while (showMenu)
        {
            Console.Clear();

            StringBuilder menuText = new();
            menuText.AppendLine($"Database: {_appEnvironment.DatabaseName}\n\n");
            menuText.AppendLine("Choose an option:");
            menuText.AppendLine("1) Utility Menu");
            menuText.AppendLine("2) Testing Menu");
            menuText.AppendLine("X) Exit");

            var panel = new Panel(menuText.ToString())
            {
                Header = new PanelHeader("Lab Patient Accounting Main Menu"),
                Border = BoxBorder.Square,
                Expand = true,
                Padding = new Padding(2, 2, 2, 2)
            };

            AnsiConsole.Write(panel);

            var selected = AnsiConsole.Ask<string>("Select an option: ");

            showMenu = ExecuteMenuSelection(selected);
        }

        return false;
    }

    private bool ExecuteMenuSelection(string selection)
    {
        switch (selection)
        {
            case "1":
                Console.Clear();
                _utilities.LaunchMenu();
                return true;
            case "2":
                Console.Clear();
                _testing.LaunchMenu();
                return true;
            case "X":
                return false;
            case "x":
                return false;
            default:
                return true;
        }
    }
}
