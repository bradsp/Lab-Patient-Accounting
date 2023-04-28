using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public sealed class GLCodeRepository : RepositoryBase<GLCode>
    {
        public GLCodeRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }
    }
}
