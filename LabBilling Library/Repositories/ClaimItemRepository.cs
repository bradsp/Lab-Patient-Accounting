using System.Collections.Generic;
using PetaPoco;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess;

public class ClaimItemRepository : RepositoryCoreBase<ClaimItem>
{
    public ClaimItemRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) {  }

    public IList<ClaimItem> Fetch(Sql command)
    {
        return Context.Fetch<ClaimItem>(command);
    }

}
