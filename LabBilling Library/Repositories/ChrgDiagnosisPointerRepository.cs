using System;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class ChrgDiagnosisPointerRepository : RepositoryBase<ChrgDiagnosisPointer>
    {
        public ChrgDiagnosisPointerRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base (appEnvironment, context)
        {

        }

        public ChrgDiagnosisPointer GetById(int id)
        {
            return Context.SingleOrDefault<ChrgDiagnosisPointer>((object)id);
        }

        public ChrgDiagnosisPointer GetById(double id)
        {
            return Context.SingleOrDefault<ChrgDiagnosisPointer>((object)id);
        }

        public override ChrgDiagnosisPointer Save(ChrgDiagnosisPointer record)
        {
            if(record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            if(record.ChrgDetailUri <= 0)
            {
                throw new ApplicationException($"Charge detail uri is not a valid ID.");
            }
            var existingRecord = GetById(record.ChrgDetailUri);

            if(existingRecord != null)
            {
                return Update(record);
            }
            else
            {
                return Add(record);
            }
        }

    }

}
