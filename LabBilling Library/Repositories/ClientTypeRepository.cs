using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess;

public sealed class ClientTypeRepository : RepositoryBase<ClientType>
{
    public ClientTypeRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {
    }

    public ClientType GetByType(int type)
    {
        return Context.SingleOrDefault<ClientType>((object)type);
    }

}
