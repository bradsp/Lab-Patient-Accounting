using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Company Category
    [Category(_companyCategory)]
    [Description("")]
    public System.String BillingContact { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String BillingEntityCounty { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String BillingEmail { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String BillingEntityCity { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String BillingEntityFax { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityFedTaxId { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityState { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityStreet { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Address { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2City { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2CityStateZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Contact { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Fax { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Name { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Phone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2State { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Zip { get; set; }
    [Category(_companyCategory), Description("20120112 changed from pob per carol for claimsnet")]
    public System.String CompanyAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String CompanyCity { get; set; }
    [Category(_companyCategory)]
    [Description("20120112 zip changed for claimsnet per carol")]
    public System.String CompanyCityStateZip { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String CompanyContact { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String CompanyFax { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String CompanyName { get; set; }
    [Category(_companyCategory)]
    [Description("20090102 wdk should this ever need to go into an electornic 837 file check the format to see if the non numberic data is allowed")]
    public System.String CompanyPhone { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String CompanyState { get; set; }
    [Category(_companyCategory)]
    [Description("20110112 changed zip and added plus 4|20090102 wdk For Electronic billing this cannot contain a dash or blank per the 837 manual in Loop 2010AA N4-03")]
    public System.String CompanyZip { get; set; }
    [Category(_companyCategory)]
    [Description("09/26/2008 wdk facility JMCGH address change request and approved by Carol")]
    public System.String FacilityAddress { get; set; }
    [Category(_companyCategory)]
    [Description("")]
    public System.String FacilityCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FacilityName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FacilityState { get; set; }
    [Category(_companyCategory), Description("20120112 changed to 5010 format added the plus 4|20090102 wdk For Electronic billing this cannot contain a dash or blank per the 837 manual in Loop 20")]
    public System.String FacilityZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FederalTaxId { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyState { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyZipCode { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceLogoImagePath { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String NPINumber { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String PrimaryCliaNo { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToCountry { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToOrganizationName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToState { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToStreet { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToStreet2 { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String WTHNPI { get; set; }
    #endregion Company Category

}
