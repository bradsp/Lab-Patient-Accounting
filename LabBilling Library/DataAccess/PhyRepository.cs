using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;

namespace LabBilling.Core.DataAccess
{
    public class PhyRepository : RepositoryBase<Phy>
    {
        public PhyRepository(string connection) : base("phy", connection)
        {
                
        }

        public override Phy GetById(int id)
        {
            Phy phy = new Phy();
            Pth pth = new Pth();

            phy = dbConnection.SingleOrDefault<Phy>(id);
            pth = dbConnection.SingleOrDefault<Pth>(phy.pc_code);

            phy.pth = pth;

            return phy;
        }

    }
}
