﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class MutuallyExclusiveEditRepository : RepositoryBase<MutuallyExclusiveEdit>
    {
        public MutuallyExclusiveEditRepository(string connection) : base(connection)
        {

        }

        public MutuallyExclusiveEditRepository(PetaPoco.Database db) : base(db)
        {

        }
        public override MutuallyExclusiveEdit GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MutuallyExclusiveEdit GetEdit(string cpt1, string cpt2)
        {
            string cpt1RealName = this.GetRealColumn(typeof(MutuallyExclusiveEdit), nameof(MutuallyExclusiveEdit.Cpt1));
            string cpt2RealName = this.GetRealColumn(typeof(MutuallyExclusiveEdit), nameof(MutuallyExclusiveEdit.Cpt1));
            return dbConnection.SingleOrDefault<MutuallyExclusiveEdit>($"where {cpt1RealName} = @0 and {cpt2RealName} = @1", cpt1, cpt2);
        }
    }
}