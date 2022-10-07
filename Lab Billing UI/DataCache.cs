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

namespace LabBilling
{
    public sealed class DataCache
    {
        IAppCache cache = new CachingService();
        private static FinRepository _finRepository;
        private static InsCompanyRepository _insCompanyRepository;
        private static PhyRepository _phyRepository;

        private DataCache() 
        {
            _finRepository = new FinRepository(Helper.ConnVal);
            _insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
            _phyRepository = new PhyRepository(Helper.ConnVal);
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
    }

}
