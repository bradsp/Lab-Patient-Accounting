using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;
public interface IDictionaryService
{
    Cdm AddCdm(Cdm cdm);
    object AddClient(Client client);
    object AddClientAccount(string clientMnem);
    bool DeleteAuditReport(int id);
    List<Announcement> GetActiveAnnouncements();
    List<Cdm> GetAllCdms(bool includeDeleted = false);
    List<Client> GetAllClients(bool includeDeleted = false);
    Task<List<Client>> GetAllClientsAsync(bool includeDeleted = false);
    IList<AuditReport> GetAuditReports();
    Cdm GetCdm(string cdm, bool includeDeleted = false);
    IList<Cdm> GetCdmByCpt(string cpt);
    Client GetClient(string clientMnem);
    IList<ClientDiscount> GetClientDiscounts(string clientMnem, bool includeDeleted = false);
    ClientType GetClientType(int type);
    DictDx GetDiagnosis(string code, DateTime transactionDate);
    DictDx GetDiagnosis(string code, string amaYear);
    IEnumerable<DictDx> GetDiagnosisCodes(string searchText, DateTime transactionDate);
    IList<Fin> GetFinancialCodes(bool includeDeleted = false);
    Fin GetFinCode(string finCode);
    List<GLCode> GetGLCodes();
    List<InsCompany> GetInsCompanies(bool excludeDeleted = true);
    InsCompany GetInsCompany(string code);
    IList<Mapping> GetMappings(string returnType, string sendingSystem);
    IEnumerable<Mapping> GetMappingsBySendingValue(string returnValueType, string sendingValue);
    IList<string> GetMappingsReturnTypeList();
    IList<string> GetMappingsSendingSystemList();
    Phy GetProvider(string npi);
    RevenueCode GetRevenueCode(string code);
    IList<WriteOffCode> GetWriteOffCodes();
    AuditReport SaveAuditReport(AuditReport report);
    Cdm SaveCdm(Cdm cdm);
    Client SaveClient(Client client);
    InsCompany SaveInsCompany(InsCompany insCompany);
    Phy SaveProvider(Phy phy);
    List<Phy> SearchProviderByName(string lastName, string firstName);
    Cdm UpdateCdm(Cdm cdm);
}