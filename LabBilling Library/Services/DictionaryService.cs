using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using static FastExpressionCompiler.ExpressionCompiler;

namespace LabBilling.Core.Services
{
    public class DictionaryService
    {
        private IAppEnvironment appEnvironment;
        public DictionaryService(IAppEnvironment appEnvironment) 
        { 
            this.appEnvironment = appEnvironment;
        }

        public bool SaveCdm(Cdm cdm)
        {
            using UnitOfWorkMain uow = new(appEnvironment, true);

            var retval = uow.CdmRepository.Save(cdm);

            uow.Commit();
            return retval;
        }

        public Cdm GetCdm(string cdm, bool includeDeleted = false)
        {
            using UnitOfWorkMain uow = new(appEnvironment);
            
            var record = uow.CdmRepository.GetCdm(cdm, includeDeleted);
            if (record != null)
            {
                record.CdmFeeSchedule1 = uow.CdmDetailRepository.GetByCdm(cdm, "1");
                record.CdmFeeSchedule2 = uow.CdmDetailRepository.GetByCdm(cdm, "2");
                record.CdmFeeSchedule3 = uow.CdmDetailRepository.GetByCdm(cdm, "3");
                record.CdmFeeSchedule4 = uow.CdmDetailRepository.GetByCdm(cdm, "4");
                record.CdmFeeSchedule5 = uow.CdmDetailRepository.GetByCdm(cdm, "5");
            }

            return record;
        }

        public IList<Cdm> GetCdmByCpt(string cpt)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var cdmDetails = uow.CdmDetailRepository.GetByCpt(cpt);           
            List<string> distinctCdms = cdmDetails.Select(c => c.ChargeItemId).Distinct().ToList();
            
            var results = uow.CdmRepository.GetCdm(distinctCdms);
            results.ForEach(c => c.CdmFeeSchedule1 = uow.CdmDetailRepository.GetByCdm(c.ChargeId, "1"));
            results.ForEach(c => c.CdmFeeSchedule2 = uow.CdmDetailRepository.GetByCdm(c.ChargeId, "2"));
            results.ForEach(c => c.CdmFeeSchedule3 = uow.CdmDetailRepository.GetByCdm(c.ChargeId, "3"));
            results.ForEach(c => c.CdmFeeSchedule4 = uow.CdmDetailRepository.GetByCdm(c.ChargeId, "4"));
            results.ForEach(c => c.CdmFeeSchedule5 = uow.CdmDetailRepository.GetByCdm(c.ChargeId, "5"));

            return results;
        }

        public List<Cdm> GetAllCdms(bool includeDeleted = false)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.CdmRepository.GetAll(includeDeleted);
        }

        public bool UpdateCdm(Cdm cdm)
        {
            using UnitOfWorkMain uow = new(appEnvironment, true);
            //update all fee schedules as well

            cdm.CdmFeeSchedule1.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule2.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule3.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule4.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule5.ForEach(cd => uow.CdmDetailRepository.Save(cd));

            var retval = uow.CdmRepository.Update(cdm);

            uow.Commit();
            return retval;
        }

        public object AddCdm(Cdm cdm)
        {
            using UnitOfWorkMain uow = new(appEnvironment, true);
            //update all fee schedules as well

            cdm.CdmFeeSchedule1.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule2.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule3.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule4.ForEach(cd => uow.CdmDetailRepository.Save(cd));
            cdm.CdmFeeSchedule5.ForEach(cd => uow.CdmDetailRepository.Save(cd));

            var retval = uow.CdmRepository.Add(cdm);

            uow.Commit();
            return retval;
        }

        public InsCompany GetInsCompany(string code)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.InsCompanyRepository.GetByCode(code);
        }

        public List<InsCompany> GetInsCompanies(bool excludeDeleted = true)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.InsCompanyRepository.GetAll(excludeDeleted);
        }

        public bool SaveInsCompany(InsCompany insCompany)
        {
            using UnitOfWorkMain uow = new(appEnvironment, true);

            var insc = GetInsCompany(insCompany.InsuranceCode);
            bool retval;

            if (insc == null)
            {
                uow.InsCompanyRepository.Add(insCompany);
            }
            else
            {
                retval = uow.InsCompanyRepository.Update(insCompany);
            }

            uow.Commit();
            return true;

        }

        public List<Client> GetAllClients(bool includeDeleted = false)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.ClientRepository.GetAll(includeDeleted);

        }

        public bool SaveClient(Client client)
        {
            using UnitOfWorkMain uow = new(appEnvironment, true);

            var retval = uow.ClientRepository.Save(client);
            uow.Commit();

            return retval;
        }

        public Client GetClient(string clientMnem)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.ClientRepository.GetClient(clientMnem);
        }

        public List<GLCode> GetGLCodes()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.GLCodeRepository.GetAll();

        }

        public List<Announcement> GetActiveAnnouncements()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.AnnouncementRepository.GetActive();
        }

        public IList<Fin> GetFinancialCodes(bool includeDeleted = false)
        {
            using UnitOfWorkMain uow = new(appEnvironment);
            if (includeDeleted)
                return uow.FinRepository.GetAll();
            else
                return uow.FinRepository.GetActive();
        }

        public IList<string> GetMappingsReturnTypeList()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.MappingRepository.GetReturnTypeList();
        }

        public IList<string> GetMappingsSendingSystemList()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.MappingRepository.GetSendingSystemList();
        }

        public IList<Mapping> GetMappings(string returnType, string sendingSystem)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.MappingRepository.GetMappings(returnType, sendingSystem).ToList();

        }

        public IList<WriteOffCode> GetWriteOffCodes()
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.WriteOffCodeRepository.GetAll();
        }

        public Phy GetProvider(string npi)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.PhyRepository.GetByNPI(npi);
        }

        public List<Phy> SearchProviderByName(string lastName, string firstName)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.PhyRepository.GetByName(lastName, firstName);
        }

        public bool SaveProvider(Phy phy)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            var retval = uow.PhyRepository.Save(phy);
            uow.Commit();

            return retval;
        }

        public RevenueCode GetRevenueCode(string code)
        {
            using UnitOfWorkMain uow = new(appEnvironment);

            return uow.RevenueCodeRepository.GetByCode(code);
        }

    }
}
