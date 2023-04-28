using LabBilling.Core.Models;
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
        public ClientTypeRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
        }

        public ClientType GetByType(int type)
        {
            return dbConnection.SingleOrDefault<ClientType>(type);
        }

    }
}
