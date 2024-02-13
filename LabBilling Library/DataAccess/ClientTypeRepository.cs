using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public sealed class ClientTypeRepository : RepositoryBase<ClientType>
    {
        public ClientTypeRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
        }

        public ClientType GetByType(int type)
        {
            return Context.SingleOrDefault<ClientType>(type);
        }

    }
}
