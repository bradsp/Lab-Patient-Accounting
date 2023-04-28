using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatDxRepository : RepositoryBase<PatDx>
    {
        public PatDxRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

    }
}
