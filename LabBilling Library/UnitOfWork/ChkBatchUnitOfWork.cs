using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Identity.Client;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.UnitOfWork
{
    public class ChkBatchUnitOfWork : UnitOfWorkMain
    {
        public ChkBatchUnitOfWork(Database context) : base(context) { }

        public ChkBatchUnitOfWork(IAppEnvironment appEnvironment, bool useTransaction = false) : base(appEnvironment, useTransaction) { }


        public bool AddBatch(List<Chk> chks)
        {
            Log.Instance.Trace("Entering");

            try
            {
                // Some transactional DB work
                foreach (Chk chk in chks)
                {
                    ChkRepository.Add(chk);
                }
            }
            catch (Exception e)
            {
                Log.Instance.Fatal(e, $"Exception adding chk record");
                throw new ApplicationException("Exception encountered posting batch. Records have been rolled back.", e);
            }

            return true;

        }

    }
}
