using LabBilling.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBillingConsole
{
    public abstract class MenuBase
    {
        protected IAppEnvironment _appEnvironment;

        public MenuBase(IAppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public abstract bool LaunchMenu();

        public abstract bool ExecuteMenuSelection(string selection);

    }
}
