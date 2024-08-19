using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.Repositories;
public class EobRepository : RepositoryBase<Eob>
{
    public EobRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }


}
