using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class ClientTypeRepository : RepositoryBase<ClientType>
    {
        public ClientTypeRepository(string connectionString) : base(connectionString)
        {
        }

        public ClientTypeRepository(Database database) : base(database) 
        { 
        }

        public ClientType GetByType(int type)
        {
            return dbConnection.SingleOrDefault<ClientType>(type);
        }

    }
}
