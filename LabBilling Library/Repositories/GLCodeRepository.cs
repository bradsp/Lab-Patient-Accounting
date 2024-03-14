using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public sealed class GLCodeRepository : RepositoryBase<GLCode>
    {
        public GLCodeRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }
    }
}
