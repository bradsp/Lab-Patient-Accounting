using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Log = LabBilling.Logging.Log;

namespace LabBilling.Core.Services;

public class DictionaryService
{
    private readonly IAppEnvironment _appEnvironment;

    public DictionaryService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
    }

    public string GetCptAmaDescription(string cptCode, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var cpt = uow.CptAmaRepository.GetCpt(cptCode);

        return cpt?.ShortDescription;
    }

    public Cdm SaveCdm(Cdm cdm, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        Cdm returnCdm;

        if (uow.CdmRepository.GetCdm(cdm.ChargeId, true) != null)
        {
            returnCdm = UpdateCdm(uow, cdm);
        }
        else
        {
            returnCdm = AddCdm(uow, cdm);
        }

        uow.Commit();
        return returnCdm;
    }

    public Cdm GetCdm(string cdm) => GetCdm(cdm, false, null);
    public Cdm GetCdm(string cdm, IUnitOfWork uow = null) => GetCdm(cdm, false, uow);
    public Cdm GetCdm(string cdm, bool includeDeleted, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var record = uow.GetRepository<CdmRepository>(true).GetCdm(cdm, includeDeleted);
        if (record != null)
        {
            record.CdmDetails = uow.GetRepository<CdmDetailRepository>(true).GetByCdm(cdm);
        }

        return record;
    }

    public IList<Cdm> GetCdmByCpt(string cpt, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var cdmDetails = uow.GetRepository<CdmDetailRepository>(true).GetByCpt(cpt);
        List<string> distinctCdms = cdmDetails.Select(c => c.ChargeItemId).Distinct().ToList();

        var results = uow.GetRepository<CdmRepository>(true).GetCdm(distinctCdms);
        results.ForEach(c => c.CdmDetails = uow.GetRepository<CdmDetailRepository>(true).GetByCdm(c.ChargeId));

        return results;
    }

    public List<Cdm> GetAllCdms(bool includeDeleted = false, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.CdmRepository.GetAll(includeDeleted);
    }


    public Cdm UpdateCdm(IUnitOfWork uow, Cdm cdm)
    {
        uow.StartTransaction();
        //update all fee schedules as well
        uow.CdmDetailRepository.Delete(cdm.ChargeId);

        cdm.CdmDetails.ForEach(cd => uow.CdmDetailRepository.Save(cd));

        var retval = uow.CdmRepository.Update(cdm);

        uow.Commit();
        return retval;
    }

    public Cdm AddCdm(IUnitOfWork uow, Cdm cdm)
    {
        uow.StartTransaction();
        //update all fee schedules as well
        cdm.CdmDetails.ForEach(cd => uow.CdmDetailRepository.Save(cd));
        var retval = uow.CdmRepository.Add(cdm);

        uow.Commit();
        return retval;
    }

    public InsCompany GetInsCompany(string code, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.InsCompanyRepository.GetByCode(code);
    }

    public InsCompany GetInsCompanyByCode(IUnitOfWork uow, string code)
    {
        Logging.Log.Instance.Debug($"Entering");

        if (code == null)
        {
            Log.Instance.Error("Null value passed to InsCompanyRepository GetByCode.");
            return new InsCompany();
        }
        var record = uow.InsCompanyRepository.GetByCode(code);

        if (record != null)
        {
            record.Mappings = GetMappingsBySendingValue(uow, "INS_CODE", record.InsuranceCode).ToList();
        }

        return record;
    }

    public List<InsCompany> GetInsCompanies(bool excludeDeleted = true, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.InsCompanyRepository.GetAll(excludeDeleted);
    }

    public InsCompany SaveInsCompany(InsCompany insCompany, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        var insc = GetInsCompany(insCompany.InsuranceCode, uow);

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

    public async Task<List<Client>> GetAllClientsAsync(IUnitOfWork uow, bool includeDeleted = false) => await Task.Run(() => GetAllClients(uow, includeDeleted));

    public List<Client> GetAllClients(IUnitOfWork uow, bool includeDeleted = false)
    {
        return uow.ClientRepository.GetAll(includeDeleted);
    }

    public Client SaveClient(Client client, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

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

    public Client GetClient(string clientMnem, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var record = uow.ClientRepository.GetClient(clientMnem);
        if (record != null)
        {
            record.Discounts = GetClientDiscounts(uow, clientMnem).ToList();
            record.Discounts?.ForEach(d => d.CdmDescription = GetCdm(d?.Cdm, uow)?.Description);
            record.ClientType = GetClientType(record.Type, uow);
            record.Mappings = GetMappingsBySendingValue(uow, "CLIENT", record.ClientMnem).ToList();
        }
        return record;
    }

    public object AddClient(IUnitOfWork uow, Client client)
    {
        uow.StartTransaction();

        var retval = uow.ClientRepository.Add(client);

        var account = uow.AccountRepository.GetByAccount(client.ClientMnem);

        if (account == null)
        {
            AddClientAccount(uow, client.ClientMnem);
        }

        uow.Commit();
        return retval;
    }

    public object AddClientAccount(IUnitOfWork uow, string clientMnem)
    {
        var client = GetClient(clientMnem, uow);

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

    public List<GLCode> GetGLCodes(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.GLCodeRepository.GetAll();
    }


    public List<Announcement> GetActiveAnnouncements(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.AnnouncementRepository.GetActive();
    }

    public IList<Fin> GetFinancialCodes() => GetFinancialCodes(false, null);
    public IList<Fin> GetFinancialCodes(bool includeDeleted = false, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        if (includeDeleted)
            return uow.FinRepository.GetAll();
        else
            return uow.FinRepository.GetActive();
    }

    public Fin GetFinCode(IUnitOfWork uow, string finCode) => uow.FinRepository.GetFin(finCode);


    #region Mappings

    public IEnumerable<Mapping> GetMappingsBySendingValue(IUnitOfWork uow, string returnValueType, string sendingValue) => uow.MappingRepository.GetMappingsBySendingValue(returnValueType, sendingValue);

    public IList<string> GetMappingsReturnTypeList(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.MappingRepository.GetReturnTypeList();
    }

    public IList<string> GetMappingsSendingSystemList(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.MappingRepository.GetSendingSystemList();
    }

    public IList<Mapping> GetMappings(string returnType, string sendingSystem, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.MappingRepository.GetMappings(returnType, sendingSystem).ToList();
    }


    #endregion Mappings

    public IList<WriteOffCode> GetWriteOffCodes(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.WriteOffCodeRepository.GetAll();
    }


    #region Provider (Phy)
    public Phy GetProvider(string npi, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.PhyRepository.GetByNPI(npi);
    }


    public List<Phy> SearchProviderByName(string lastName, string firstName, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.PhyRepository.GetByName(lastName, firstName);
    }

    public Phy SaveProvider(Phy phy, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var retval = uow.PhyRepository.Save(phy);
        uow.Commit();

        return retval;
    }
    #endregion Provider (Phy)


    public RevenueCode GetRevenueCode(IUnitOfWork uow, string code) => uow.RevenueCodeRepository.GetByCode(code);

    public ClientType GetClientType(int type, IUnitOfWork uow)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.ClientTypeRepository.GetByType(type);
    }

    public IList<ClientDiscount> GetClientDiscounts(IUnitOfWork uow, string clientMnem, bool includeDeleted = false) => uow.ClientDiscountRepository.GetByClient(clientMnem, includeDeleted);

    public SanctionedProvider GetSanctionedProvider(IUnitOfWork uow, string npi) => uow.SanctionedProviderRepository.GetByNPI(npi);


    #region DictDx

    public DictDx GetDiagnosis(string code, DateTime transactionDate, IUnitOfWork uow = null)
    {
        if (uow == null)
            uow = new UnitOfWorkMain(_appEnvironment);

        return uow.DictDxRepository.GetByCode(code, transactionDate);
    }

    public DictDx GetDiagnosis(IUnitOfWork uow, string code, string amaYear) => uow.DictDxRepository.GetByCode(code, amaYear);

    public IEnumerable<DictDx> GetDiagnosisCodes(string searchText, DateTime transactionDate, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.DictDxRepository.Search(searchText, transactionDate);
    }


    #endregion DictDx

    #region AuditReports

    public IList<AuditReport> GetAuditReports(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.AuditReportRepository.GetAll();
    }

    public AuditReport SaveAuditReport(AuditReport report, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        if (report.Id > 0)
        {
            return uow.AuditReportRepository.Update(report);
        }
        else
        {
            return uow.AuditReportRepository.Add(report);
        }
    }

    public bool DeleteAuditReport(int id, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
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
