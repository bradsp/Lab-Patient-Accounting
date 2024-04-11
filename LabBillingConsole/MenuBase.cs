using LabBilling.Core.DataAccess;

namespace LabBillingConsole;

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
