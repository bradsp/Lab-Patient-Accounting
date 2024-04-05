using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

public class DictionaryService
{
    private readonly IAppEnvironment _appEnvironment;

    public DictionaryService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
    }

    public Cdm SaveCdm(Cdm cdm)
    {
        using UnitOfWorkMain uow = new(_appEnvironment, true);
        Cdm returnCdm;

        if (uow.CdmRepository.GetCdm(cdm.ChargeId) != null)
        {
            returnCdm = UpdateCdm(cdm);
        }
        else
        {
            returnCdm = AddCdm(cdm); 
        }

        uow.Commit();
        return returnCdm;
    }

    public Cdm GetCdm(string cdm, bool includeDeleted = false)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var record = uow.CdmRepository.GetCdm(cdm, includeDeleted);
        if (record != null)
        {
            record.CdmDetails = uow.CdmDetailRepository.GetByCdm(cdm);
        }

        return record;
    }

    public IList<Cdm> GetCdmByCpt(string cpt)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var cdmDetails = uow.CdmDetailRepository.GetByCpt(cpt);
        List<string> distinctCdms = cdmDetails.Select(c => c.ChargeItemId).Distinct().ToList();

        var results = uow.CdmRepository.GetCdm(distinctCdms);
        results.ForEach(c => c.CdmDetails = uow.CdmDetailRepository.GetByCdm(c.ChargeId));

        return results;
    }

    public List<Cdm> GetAllCdms(bool includeDeleted = false)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        return uow.CdmRepository.GetAll(includeDeleted);
    }

    public Cdm UpdateCdm(Cdm cdm)
    {
        using UnitOfWorkMain uow = new(_appEnvironment, true);
        //update all fee schedules as well
        cdm.CdmDetails.ForEach(cd => uow.CdmDetailRepository.Save(cd));

        var retval = uow.CdmRepository.Update(cdm);

        uow.Commit();
        return retval;
    }

    public Cdm AddCdm(Cdm cdm)
    {
        using UnitOfWorkMain uow = new(_appEnvironment, true);
        //update all fee schedules as well
        cdm.CdmDetails.ForEach(cd => uow.CdmDetailRepository.Save(cd));
        var retval = uow.CdmRepository.Add(cdm);

        uow.Commit();
        return retval;
    }

    public InsCompany GetInsCompany(string code)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.InsCompanyRepository.GetByCode(code);
    }

    public List<InsCompany> GetInsCompanies(bool excludeDeleted = true)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.InsCompanyRepository.GetAll(excludeDeleted);
    }

    public InsCompany SaveInsCompany(InsCompany insCompany)
    {
        using UnitOfWorkMain uow = new(_appEnvironment, true);

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
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.ClientRepository.GetAll(includeDeleted);

    }

    public Client SaveClient(Client client)
    {
        using UnitOfWorkMain uow = new(_appEnvironment, true);

        //bool success;

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
        using UnitOfWorkMain uow = new(_appEnvironment);

        var record = uow.ClientRepository.GetClient(clientMnem);
        if (record != null)
        {
            record.Discounts = GetClientDiscounts(clientMnem).ToList();
            record.Discounts?.ForEach(d => d.CdmDescription = GetCdm(d?.Cdm)?.Description);
            record.ClientType = GetClientType(record.Type);
            record.Mappings = GetMappingsBySendingValue("CLIENT", record.ClientMnem).ToList();
        }
        return record;
    }

    public object AddClient(Client client)
    {
        using UnitOfWorkMain uow = new(_appEnvironment, true);

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
        using UnitOfWorkMain uow = new(_appEnvironment);

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
                FinCode = _appEnvironment.ApplicationParameters.ClientAccountFinCode,
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
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.GLCodeRepository.GetAll();

    }

    public List<Announcement> GetActiveAnnouncements()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.AnnouncementRepository.GetActive();
    }

    public IList<Fin> GetFinancialCodes(bool includeDeleted = false)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        if (includeDeleted)
            return uow.FinRepository.GetAll();
        else
            return uow.FinRepository.GetActive();
    }

    public Fin GetFinCode(string finCode)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        return uow.FinRepository.GetFin(finCode);
    }


    #region Mappings

    public IEnumerable<Mapping> GetMappingsBySendingValue(string returnValueType, string sendingValue)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.MappingRepository.GetMappingsBySendingValue(returnValueType, sendingValue);
    }

    public IList<string> GetMappingsReturnTypeList()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.MappingRepository.GetReturnTypeList();
    }

    public IList<string> GetMappingsSendingSystemList()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.MappingRepository.GetSendingSystemList();
    }

    public IList<Mapping> GetMappings(string returnType, string sendingSystem)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.MappingRepository.GetMappings(returnType, sendingSystem).ToList();

    }

    #endregion Mappings

    public IList<WriteOffCode> GetWriteOffCodes()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.WriteOffCodeRepository.GetAll();
    }


    #region Provider (Phy)
    public Phy GetProvider(string npi)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.PhyRepository.GetByNPI(npi);
    }

    public List<Phy> SearchProviderByName(string lastName, string firstName)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.PhyRepository.GetByName(lastName, firstName);
    }

    public Phy SaveProvider(Phy phy)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var retval = uow.PhyRepository.Save(phy);
        uow.Commit();

        return retval;
    }
    #endregion Provider (Phy)


    public RevenueCode GetRevenueCode(string code)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.RevenueCodeRepository.GetByCode(code);
    }

    public ClientType GetClientType(int type)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.ClientTypeRepository.GetByType(type);
    }

    public IList<ClientDiscount> GetClientDiscounts(string clientMnem, bool includeDeleted = false)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.ClientDiscountRepository.GetByClient(clientMnem, includeDeleted);
    }

    #region DictDx

    public DictDx GetDiagnosis(string code, DateTime transactionDate)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.DictDxRepository.GetByCode(code, transactionDate);
    }

    public DictDx GetDiagnosis(string code, string amaYear)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.DictDxRepository.GetByCode(code, amaYear);
    }

    public IEnumerable<DictDx> GetDiagnosisCodes(string searchText, DateTime transactionDate)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.DictDxRepository.Search(searchText, transactionDate);
    }

    #endregion DictDx

    #region AuditReports

    public IList<AuditReport> GetAuditReports()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.AuditReportRepository.GetAll();
    }

    public AuditReport SaveAuditReport(AuditReport report)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        if (report.Id > 0)
        {
            return uow.AuditReportRepository.Update(report);
        }
        else
        {
            return uow.AuditReportRepository.Add(report);
        }
    }

    public bool DeleteAuditReport(int id)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var record = uow.AuditReportRepository.GetByKey(id);
        if (record != null)
        {
            uow.AuditReportRepository.Delete(record);
            return true;
        }
        return false;
    }

    #endregion

}
