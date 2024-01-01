using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LazyCache;

namespace LabBilling
{
    public sealed class DataCache
    {
        IAppCache cache = new CachingService();
        private static FinRepository _finRepository;
        private static InsCompanyRepository _insCompanyRepository;
        private static PhyRepository _phyRepository;
        private static ClientRepository _clientRepository;
        private static CdmRepository _cdmRepository;
        private static RevenueCodeRepository _revenueCodeRepository;

        private DataCache() 
        {
            _finRepository = new FinRepository(Program.AppEnvironment);
            _insCompanyRepository = new InsCompanyRepository(Program.AppEnvironment);
            _phyRepository = new PhyRepository(Program.AppEnvironment);
            _clientRepository = new ClientRepository(Program.AppEnvironment);
            _cdmRepository = new CdmRepository(Program.AppEnvironment);
            _revenueCodeRepository = new RevenueCodeRepository(Program.AppEnvironment);
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

        Func<IEnumerable<Fin>> finGetter = () => _finRepository.GetActive();
        
        public List<Fin> GetFins()
        {
            var fins = cache.GetOrAdd("fin", finGetter);

            return fins.ToList();
        }

        public void ClearFinCache()
        {
            cache.Remove("fin");
        }

        Func<IEnumerable<InsCompany>> inscGetter = () => _insCompanyRepository.GetAll();

        public List<InsCompany> GetInsCompanies()
        {
            var inscs = cache.GetOrAdd("insc", inscGetter);

            return inscs.ToList();
        }

        public void ClearInscCache()
        {
            cache.Remove("insc");
        }

        Func<IEnumerable<Phy>> phyGetter = () => _phyRepository.GetActive();

        public List<Phy> GetProviders()
        {
            var phys = cache.GetOrAdd("phy", phyGetter);

            return phys.ToList();
        }

        public void ClearProviderCache()
        {
            cache.Remove("phy");
        }

        Func<IEnumerable<Client>> clientGetter = () => _clientRepository.GetAll(false);

        public List<Client> GetClients()
        {
            var clients = cache.GetOrAdd("client", clientGetter);

            return clients.ToList();
        }

        Func<IEnumerable<Client>> clientAllGetter = () => _clientRepository.GetAll(true);

        public List<Client> GetClientsIncludeInactive()
        {
            var clients = cache.GetOrAdd("client", clientAllGetter);
            return clients.ToList();
        }

        public void ClearClientCache()
        {
            cache.Remove("client");
        }

        Func<IEnumerable<Cdm>> cdmGetter = () => _cdmRepository.GetAll(false);

        public List<Cdm> GetCdms()
        {
            var cdms = cache.GetOrAdd("cdm", cdmGetter);

            return cdms.ToList();
        }

        public void ClearCdmCache()
        {
            cache.Remove("cdm");
        }

        Func<IEnumerable<RevenueCode>> revenueCodeGetter = () => _revenueCodeRepository.GetAll();

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
