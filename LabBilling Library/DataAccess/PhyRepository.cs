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

        public PhyRepository(string connection, PetaPoco.Database db) : base("phy", connection, db)
        {

        }

        public Phy GetByNPI(string npi)
        {
            Phy phy = new Phy();
            Pth pth = new Pth();

            if (!string.IsNullOrEmpty(npi))
                phy = dbConnection.SingleOrDefault<Phy>("where tnh_num = @0", npi);

            if (!string.IsNullOrEmpty(phy.pc_code))
            {
                pth = dbConnection.SingleOrDefault<Pth>(phy.pc_code);
                phy.pth = pth;
            }

            return phy;
        }

        public override Phy GetById(int id)
        {
            Phy phy = new Phy();
            Pth pth = new Pth();

            phy = dbConnection.SingleOrDefault<Phy>(id);
            if (phy.pc_code != null)
            {
                pth = dbConnection.SingleOrDefault<Pth>(phy.pc_code);
                phy.pth = pth;
            }
            
            return phy;
        }

    }
}
