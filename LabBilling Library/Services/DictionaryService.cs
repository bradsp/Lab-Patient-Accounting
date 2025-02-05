using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Log = LabBilling.Logging.Log;

namespace LabBilling.Core.Services;

public class DictionaryService
{
    private readonly IAppEnvironment _appEnvironment;
    private readonly IUnitOfWork _uow;

    public DictionaryService(IAppEnvironment appEnvironment, IUnitOfWork uow)
    {
        this._appEnvironment = appEnvironment;
        _uow = uow;
    }

    public string GetCptAmaDescription(string cptCode)
    {
        var cpt = _uow.CptAmaRepository.GetCpt(cptCode);

        return cpt?.ShortDescription;
    }

    public Cdm SaveCdm(Cdm cdm)
    {
        _uow.StartTransaction();
        Cdm returnCdm;

        if (_uow.CdmRepository.GetCdm(cdm.ChargeId, true) != null)
        {
            returnCdm = UpdateCdm(cdm);
        }
        else
        {
            returnCdm = AddCdm(cdm); 
        }

        _uow.Commit();
        return returnCdm;
    }

    public Cdm GetCdm(string cdm, bool includeDeleted = false)
    {
        var record = _uow.CdmRepository.GetCdm(cdm, includeDeleted);
        if (record != null)
        {
            record.CdmDetails = _uow.CdmDetailRepository.GetByCdm(cdm);
        }

        return record;
    }

    public IList<Cdm> GetCdmByCpt(string cpt)
    {
        var cdmDetails = _uow.CdmDetailRepository.GetByCpt(cpt);
        List<string> distinctCdms = cdmDetails.Select(c => c.ChargeItemId).Distinct().ToList();

        var results = _uow.CdmRepository.GetCdm(distinctCdms);
        results.ForEach(c => c.CdmDetails = _uow.CdmDetailRepository.GetByCdm(c.ChargeId));

        return results;
    }

    public List<Cdm> GetAllCdms(bool includeDeleted = false) => _uow.CdmRepository.GetAll(includeDeleted);


    public Cdm UpdateCdm(Cdm cdm)
    {
        _uow.StartTransaction();
        //update all fee schedules as well
        _uow.CdmDetailRepository.Delete(cdm.ChargeId);

        cdm.CdmDetails.ForEach(cd => _uow.CdmDetailRepository.Save(cd));

        var retval = _uow.CdmRepository.Update(cdm);

        _uow.Commit();
        return retval;
    }

    public Cdm AddCdm(Cdm cdm)
    {
        _uow.StartTransaction();
        //update all fee schedules as well
        cdm.CdmDetails.ForEach(cd => _uow.CdmDetailRepository.Save(cd));
        var retval = _uow.CdmRepository.Add(cdm);

        _uow.Commit();
        return retval;
    }

    public InsCompany GetInsCompany(string code)
    {
        return _uow.InsCompanyRepository.GetByCode(code);
    }
    
    public InsCompany GetInsCompanyByCode(string code)
    {
        Logging.Log.Instance.Debug($"Entering");

        if (code == null)
        {
            Log.Instance.Error("Null value passed to InsCompanyRepository GetByCode.");
            return new InsCompany();
        }
        var record = _uow.InsCompanyRepository.GetByCode(code);

        if (record != null)
        {
            record.Mappings = GetMappingsBySendingValue("INS_CODE", record.InsuranceCode).ToList();
        }

        return record;
    }

    public List<InsCompany> GetInsCompanies(bool excludeDeleted = true)
    {
        return _uow.InsCompanyRepository.GetAll(excludeDeleted);
    }

    public InsCompany SaveInsCompany(InsCompany insCompany)
    {
        _uow.StartTransaction();

        var insc = GetInsCompany(insCompany.InsuranceCode);

        if (insc == null)
        {
            insc = _uow.InsCompanyRepository.Add(insCompany);
        }
        else
        {
            insc = _uow.InsCompanyRepository.Update(insCompany);
        }

        _uow.Commit();
        return insc;

    }

    public async Task<List<Client>> GetAllClientsAsync(bool includeDeleted = false) => await Task.Run(() => GetAllClients(includeDeleted));

    public List<Client> GetAllClients(bool includeDeleted = false)
    {
        return _uow.ClientRepository.GetAll(includeDeleted);
    }

    public Client SaveClient(Client client)
    {
        _uow.StartTransaction();

        //bool success;

        if (client.Id > 0) // existing record
        {
            _uow.ClientDiscountRepository.SaveAll(client.Discounts);
            client = _uow.ClientRepository.Update(client);
        }
        else
        {
            try
            {
                client = _uow.ClientRepository.Add(client);
                if (client.Discounts != null)
                    _uow.ClientDiscountRepository.SaveAll(client.Discounts);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException($"Error saving client record {client.ClientMnem}");
            }
        }

        _uow.Commit();
        return client;
    }

    public Client GetClient(string clientMnem)
    {
        var record = _uow.ClientRepository.GetClient(clientMnem);
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
        _uow.StartTransaction();

        var retval = _uow.ClientRepository.Add(client);

        var account = _uow.AccountRepository.GetByAccount(client.ClientMnem);

        if (account == null)
        {
            AddClientAccount(client.ClientMnem);
        }

        _uow.Commit();
        return retval;
    }

    public object AddClientAccount(string clientMnem)
    {
        var client = GetClient(clientMnem);

        var account = _uow.AccountRepository.GetByAccount(client.ClientMnem);
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

            retval = _uow.AccountRepository.Add(account);
        }

        return retval;

    }

    public List<GLCode> GetGLCodes() => _uow.GLCodeRepository.GetAll();


    public List<Announcement> GetActiveAnnouncements() => _uow.AnnouncementRepository.GetActive();

    public IList<Fin> GetFinancialCodes(bool includeDeleted = false)
    {
        if (includeDeleted)
            return _uow.FinRepository.GetAll();
        else
            return _uow.FinRepository.GetActive();
    }

    public Fin GetFinCode(string finCode) => _uow.FinRepository.GetFin(finCode);


    #region Mappings

    public IEnumerable<Mapping> GetMappingsBySendingValue(string returnValueType, string sendingValue) => _uow.MappingRepository.GetMappingsBySendingValue(returnValueType, sendingValue);

    public IList<string> GetMappingsReturnTypeList() => _uow.MappingRepository.GetReturnTypeList();

    public IList<string> GetMappingsSendingSystemList() => _uow.MappingRepository.GetSendingSystemList();

    public IList<Mapping> GetMappings(string returnType, string sendingSystem) => _uow.MappingRepository.GetMappings(returnType, sendingSystem).ToList();


    #endregion Mappings

    public IList<WriteOffCode> GetWriteOffCodes() => _uow.WriteOffCodeRepository.GetAll();
 

    #region Provider (Phy)
    public Phy GetProvider(string npi) => _uow.PhyRepository.GetByNPI(npi);
 
    public List<Phy> SearchProviderByName(string lastName, string firstName) => _uow.PhyRepository.GetByName(lastName, firstName);

    public Phy SaveProvider(Phy phy)
    {
        var retval = _uow.PhyRepository.Save(phy);
        _uow.Commit();

        return retval;
    }
    #endregion Provider (Phy)


    public RevenueCode GetRevenueCode(string code) => _uow.RevenueCodeRepository.GetByCode(code);

    public ClientType GetClientType(int type) => _uow.ClientTypeRepository.GetByType(type);

    public IList<ClientDiscount> GetClientDiscounts(string clientMnem, bool includeDeleted = false) => _uow.ClientDiscountRepository.GetByClient(clientMnem, includeDeleted);

    public SanctionedProvider GetSanctionedProvider(string npi) => _uow.SanctionedProviderRepository.GetByNPI(npi);


    #region DictDx

    public DictDx GetDiagnosis(string code, DateTime transactionDate) => _uow.DictDxRepository.GetByCode(code, transactionDate);

    public DictDx GetDiagnosis(string code, string amaYear) => _uow.DictDxRepository.GetByCode(code, amaYear);

    public IEnumerable<DictDx> GetDiagnosisCodes(string searchText, DateTime transactionDate) => _uow.DictDxRepository.Search(searchText, transactionDate);


    #endregion DictDx

    #region AuditReports

    public IList<AuditReport> GetAuditReports() => _uow.AuditReportRepository.GetAll();


    public AuditReport SaveAuditReport(AuditReport report)
    {
        if (report.Id > 0)
        {
            return _uow.AuditReportRepository.Update(report);
        }
        else
        {
            return _uow.AuditReportRepository.Add(report);
        }
    }

    public bool DeleteAuditReport(int id)
    {
        var record = _uow.AuditReportRepository.GetByKey(id);
        if (record != null)
        {
            _uow.AuditReportRepository.Delete(record);
            return true;
        }
        return false;
    }

    #endregion

}
