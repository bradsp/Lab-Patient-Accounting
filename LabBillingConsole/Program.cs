using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core;
using System.IO;
using PetaPoco;
using RFClassLibrary;
using System.Data.Common;
using PetaPoco.Providers;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;
using System.Net.Http.Headers;
using Spectre.Console;

namespace LabBillingConsole
{
    internal class Program
    {

        static void Main(string[] args)
        {
            bool showMenu = true;

            while(showMenu)
            {
                showMenu = MainMenuPanel();
            }
        }

        private static bool MainMenuPanel()
        {
            string serverName;
            string databaseName;

            StringBuilder dbSelect = new StringBuilder();
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
                    if (db == 0 || db == 1 || db == 2)
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
                    break;
                case 2:
                    databaseName = "LabBillingTest";
                    break;
                default:
                    return true;
                    //break;
            }

            MainMenu mainMenu = new MainMenu(serverName, databaseName);

            return mainMenu.MainMenuPanel();

        }

    }


}
