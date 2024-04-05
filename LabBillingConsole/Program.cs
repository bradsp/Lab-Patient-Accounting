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
        dbSelect.AppendLine("1) LabBillingProd");
        dbSelect.AppendLine("2) LabBillingTest (WTMCLBILL)");
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

        serverName = "WTHMCLBILL";

        switch (menuSelect)
        {
            case 0:
                return false;
            case 1:
                databaseName = "LabBillingProd";
                serverName = "WTHMCLBILL";
                break;
            case 2:
                databaseName = "LabBillingTest";
                serverName = "WTHMCLBILL";
                break;
            default:
                return true;
                //break;
        }

        MainMenu mainMenu = new(serverName, databaseName);

        return mainMenu.MainMenuPanel();

    }

}
