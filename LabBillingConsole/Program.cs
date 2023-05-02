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
        //public const string connectionString = "Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;";

        public static AppEnvironment appEnvironment;
        public static Utilities utilities;
        public static Testing testing;

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
            appEnvironment = new AppEnvironment
            {
                DatabaseName = "LabBillingProd",
                ServerName = "WTHMCLBILL",
                LogDatabaseName = "Log"
            };

            testing = new Testing(appEnvironment);
            utilities = new Utilities(appEnvironment);

            Console.Clear();

            StringBuilder menuText = new StringBuilder();
            menuText.AppendLine($"Connection String: {appEnvironment.ConnectionString}\n\n");
            menuText.AppendLine("Choose an option:");
            menuText.AppendLine("1) Utility Menu");
            menuText.AppendLine("2) Testing Menu");
            menuText.AppendLine("X) Exit");

            var panel = new Panel(menuText.ToString());
            panel.Header = new PanelHeader("Lab Patient Accounting Main Menu");
            panel.Border = BoxBorder.Square;
            panel.Padding = new Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);

            var selected = AnsiConsole.Ask<string>("Select an option: ");

            return ExecuteMenuSelection(selected);
        }

        private static bool ExecuteMenuSelection(string selection)
        {
            switch (selection)
            {
                case "1":
                    Console.Clear();
                    utilities.LaunchMenu();
                    return true;
                case "2":
                    Console.Clear();
                    testing.LaunchMenu();
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


}
