using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

public class DictionaryService
{
    private readonly IAppEnvironment appEnvironment;
    private const string clientFinCode = "CLIENT";

    public DictionaryService(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
    }

    public Cdm SaveCdm(Cdm cdm)
    {
        using UnitOfWorkMain uow = new(appEnvironment, true);

        var updatedCdm = uow.CdmRepository.Save(cdm);

        uow.Commit();
        return updatedCdm;
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

    public Cdm UpdateCdm(Cdm cdm)
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

    public InsCompany SaveInsCompany(InsCompany insCompany)
    {
        using UnitOfWorkMain uow = new(appEnvironment, true);

        var insc = GetInsCompany(insCompany.InsuranceCode);

        if (insc == null)
        {
            insc = uow.InsCompanyRepository.Add(insCompany);
        }
        else
        {
            insc = uow.InsCompanyRepository.Update(insCompany);
        }

        uow.Commit();
        return insc;

    }

    public async Task<List<Client>> GetAllClientsAsync(bool includeDeleted = false) => await Task.Run(() => GetAllClients(includeDeleted));

    public List<Client> GetAllClients(bool includeDeleted = false)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.ClientRepository.GetAll(includeDeleted);

    }

    public Client SaveClient(Client client)
    {
        using UnitOfWorkMain uow = new(appEnvironment, true);

        bool success;

        if (client.Id > 0) // existing record
        {
            uow.ClientDiscountRepository.SaveAll(client.Discounts);
            client = uow.ClientRepository.Update(client);
        }
        else
        {
            try
            {
                client = uow.ClientRepository.Add(client);
                if (client.Discounts != null)
                    uow.ClientDiscountRepository.SaveAll(client.Discounts);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException($"Error saving client record {client.ClientMnem}");
            }
        }

        uow.Commit();
        return client;
    }

    public Client GetClient(string clientMnem)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.ClientRepository.GetClient(clientMnem);
    }

    public object AddClient(Client client)
    {
        using UnitOfWorkMain uow = new(appEnvironment, true);

        var retval = uow.ClientRepository.Add(client);

        var account = uow.AccountRepository.GetByAccount(client.ClientMnem);

        if (account == null)
        {
            AddClientAccount(client.ClientMnem);
        }

        uow.Commit();
        return retval;
    }

    public object AddClientAccount(string clientMnem)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        var client = GetClient(clientMnem);

        var account = uow.AccountRepository.GetByAccount(client.ClientMnem);
        object retval = null;
        if (account == null)
        {
            //add account
            account = new Account
            {
                AccountNo = client.ClientMnem,
                PatFullName = client.Name,
                FinCode = clientFinCode,
                TransactionDate = DateTime.Today,
                Status = AccountStatus.New,
                ClientMnem = client.ClientMnem,
                MeditechAccount = client.ClientMnem
            };

            retval = uow.AccountRepository.Add(account);
        }

        return retval;

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

    public Fin GetFinCode(string finCode)
    {
        using UnitOfWorkMain uow = new(appEnvironment);
        return uow.FinRepository.GetFin(finCode);
    }


    #region Mappings

    public IEnumerable<Mapping> GetMappingsBySendingValue(string returnValueType, string sendingValue)        
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.MappingRepository.GetMappingsBySendingValue(returnValueType, sendingValue);
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

    #endregion Mappings

    public IList<WriteOffCode> GetWriteOffCodes()
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.WriteOffCodeRepository.GetAll();
    }


    #region Provider (Phy)
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

    public Phy SaveProvider(Phy phy)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        var retval = uow.PhyRepository.Save(phy);
        uow.Commit();

        return retval;
    }
    #endregion Provider (Phy)


    public RevenueCode GetRevenueCode(string code)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.RevenueCodeRepository.GetByCode(code);
    }

    public ClientType GetClientType(int type)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.ClientTypeRepository.GetByType(type);
    }

    public IList<ClientDiscount> GetClientDiscounts(string clientMnem, bool includeDeleted = false)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.ClientDiscountRepository.GetByClient(clientMnem, includeDeleted);
    }

    #region DictDx

    public DictDx GetDiagnosis(string code, DateTime transactionDate)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.DictDxRepository.GetByCode(code, transactionDate);
    }

    public DictDx GetDiagnosis(string code, string amaYear)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.DictDxRepository.GetByCode(code, amaYear);
    }

    public IEnumerable<DictDx> GetDiagnosisCodes(string searchText, DateTime transactionDate)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.DictDxRepository.Search(searchText, transactionDate);
    }

    #endregion DictDx

    #region AuditReports

    public IList<AuditReport> GetAuditReports()
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        return uow.AuditReportRepository.GetAll();
    }

    #endregion

}
