using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LazyCache;

namespace LabBilling
{
    public sealed class DataCache
    {
        IAppCache cache = new CachingService();
        private static readonly UnitOfWorkMain unitOfWork = new(Program.AppEnvironment);

        private DataCache() 
        {
            
        }

        private static DataCache instance = null;

        public static DataCache Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new DataCache();
                }
                return instance;
            }
        }

        readonly Func<IEnumerable<Fin>> finGetter = () => unitOfWork.FinRepository.GetActive();
        
        public List<Fin> GetFins()
        {
            var fins = cache.GetOrAdd("fin", finGetter);

            return fins.ToList();
        }

        public void ClearFinCache()
        {
            cache.Remove("fin");
        }

        readonly Func<IEnumerable<InsCompany>> inscGetter = () => unitOfWork.InsCompanyRepository.GetAll();

        public List<InsCompany> GetInsCompanies()
        {
            var inscs = cache.GetOrAdd("insc", inscGetter);

            return inscs.ToList();
        }

        public void ClearInscCache()
        {
            cache.Remove("insc");
        }

        readonly Func<IEnumerable<Phy>> phyGetter = () => unitOfWork.PhyRepository.GetActive();

        public List<Phy> GetProviders()
        {
            var phys = cache.GetOrAdd("phy", phyGetter);

            return phys.ToList();
        }

        public void ClearProviderCache()
        {
            cache.Remove("phy");
        }

        readonly Func<IEnumerable<Client>> clientGetter = () => unitOfWork.ClientRepository.GetAll(false);

        public List<Client> GetClients()
        {
            var clients = cache.GetOrAdd("client", clientGetter);

            return clients.ToList();
        }

        readonly Func<IEnumerable<Client>> clientAllGetter = () => unitOfWork.ClientRepository.GetAll(true);

        public List<Client> GetClientsIncludeInactive()
        {
            var clients = cache.GetOrAdd("client", clientAllGetter);
            return clients.ToList();
        }

        public void ClearClientCache()
        {
            cache.Remove("client");
        }

        readonly Func<IEnumerable<Cdm>> cdmGetter = () => unitOfWork.CdmRepository.GetAll(false);

        public List<Cdm> GetCdms()
        {
            var cdms = cache.GetOrAdd("cdm", cdmGetter);

            return cdms.ToList();
        }

        public void ClearCdmCache()
        {
            cache.Remove("cdm");
        }

        readonly Func<IEnumerable<RevenueCode>> revenueCodeGetter = () => unitOfWork.RevenueCodeRepository.GetAll();

        public List<RevenueCode> GetRevenueCodes()
        {
            var revenuecodes = cache.GetOrAdd("revcode", revenueCodeGetter);

            return revenuecodes.ToList();
        }

        public void ClearRevenueCodeCache()
        {
            cache.Remove("revcode");
        }
    }

}
