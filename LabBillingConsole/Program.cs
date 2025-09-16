using Spectre.Console;
using System.Text;

namespace LabBillingConsole;

internal class Program
{

    static void Main(string[] args)
    {
        bool showMenu = true;

        while (showMenu)
        {
            showMenu = MainMenuPanel();
        }
    }

    private static bool MainMenuPanel()
    {
        string serverName;
        string databaseName;

        StringBuilder dbSelect = new();
        dbSelect.AppendLine("Select Database:\n\n");
        dbSelect.AppendLine("1) Production Database");
        dbSelect.AppendLine("2) Test Database");
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

        // Get server and database names from environment variables or use defaults
        string defaultServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "${DB_SERVER}";
        string prodDbName = Environment.GetEnvironmentVariable("PROD_DB_NAME") ?? "${PROD_DB_NAME}";
        string testDbName = Environment.GetEnvironmentVariable("TEST_DB_NAME") ?? "${TEST_DB_NAME}";
        
        serverName = defaultServer;

        switch (menuSelect)
        {
            case 0:
                return false;
            case 1:
                databaseName = prodDbName;
                serverName = defaultServer;
                break;
            case 2:
                databaseName = testDbName;
                serverName = defaultServer;
                break;
            default:
                return true;
                //break;
        }

        MainMenu mainMenu = new(serverName, databaseName);

        return mainMenu.MainMenuPanel();

    }

}
