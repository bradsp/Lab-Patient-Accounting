using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.Repositories;
public class RandomDrugScreenPersonRepository : RepositoryBase<RandomDrugScreenPerson>
{
    public RandomDrugScreenPersonRepository(IAppEnvironment appEnvironment, IDatabase context) : base(appEnvironment, context)
    {
    }

}
