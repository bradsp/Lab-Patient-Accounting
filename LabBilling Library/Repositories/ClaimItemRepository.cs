using LabBilling.Core.Models;
using PetaPoco;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess;

public class ClaimItemRepository : RepositoryCoreBase<ClaimItem>
{
    public ClaimItemRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }

    public IList<ClaimItem> Fetch(Sql command)
    {
        return Context.Fetch<ClaimItem>(command);
    }

}
