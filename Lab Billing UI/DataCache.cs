using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LazyCache;
using LazyCache.Providers;
using Microsoft.Extensions.Caching.Memory;
using PetaPoco;

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

        private DataCache() 
        {
            _finRepository = new FinRepository(Helper.ConnVal);
            _insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
            _phyRepository = new PhyRepository(Helper.ConnVal);
            _clientRepository = new ClientRepository(Helper.ConnVal);
            _cdmRepository = new CdmRepository(Helper.ConnVal);
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

        Func<IEnumerable<Fin>> finGetter = () => _finRepository.GetAll();
        
        public List<Fin> GetFins()
        {
            var fins = cache.GetOrAdd("fin", finGetter);

            return fins.ToList();
        }

        Func<IEnumerable<InsCompany>> inscGetter = () => _insCompanyRepository.GetAll();

        public List<InsCompany> GetInsCompanies()
        {
            var inscs = cache.GetOrAdd("insc", inscGetter);

            return inscs.ToList();
        }

        Func<IEnumerable<Phy>> phyGetter = () => _phyRepository.GetActive();

        public List<Phy> GetProviders()
        {
            var phys = cache.GetOrAdd("phy", phyGetter);

            return phys.ToList();
        }

        Func<IEnumerable<Client>> clientGetter = () => _clientRepository.GetAll(false);

        public List<Client> GetClients()
        {
            var clients = cache.GetOrAdd("client", clientGetter);

            return clients.ToList();
        }

        Func<IEnumerable<Cdm>> cdmGetter = () => _cdmRepository.GetAll(false);

        public List<Cdm> GetCdms()
        {
            var cdms = cache.GetOrAdd("cdm", cdmGetter);

            return cdms.ToList();
        }
    }

}
