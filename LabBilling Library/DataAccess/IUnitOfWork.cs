using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Database db { get; }
    }
}
