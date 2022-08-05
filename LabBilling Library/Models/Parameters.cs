using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace LabBilling.Core.Models
{
    [Serializable]
    public class Parameters 
    {
        //[Category("Billing")]
        //[Description("")]
        //[DisplayName("")]
        //[DefaultValue("")]

        #region Properties

        [Category("Billing")]
        [Description("new form to be used")]
        [DefaultValue("")]
        public DateTime ProfessionalClaimStartDate { get; set; }

        [Category("Billing")]
        [Description("Location to place 837p claim files.")]
        public string ProfessionalClaimFileLocation { get; set; }

        [Category("System")]
        [Description("")]
        [DefaultValue(true)]
        public bool AllowPaymentAdjustmentEntry { get; set; }

        [Category("System")]
        [Description("")]
        [DefaultValue(true)]
        public bool AllowChargeEntry { get; set; }

        [Category("System")]
        [Description("")]
        [DefaultValue(true)]
        public bool AllowEditing { get; set; }

        [Category("System")]
        [Description("SET SELECT/MOVE OF ACCOUNTS")]
        public string AllowSelMove { get; set; }

        [Category("System")]
        [Description("")]
        public string AccessMedPlusProvNo { get; set; }

        [Category("System")]
        [Description("")]
        public string ArchiveDB { get; set; }

        [Category("Billing")]
        [Description("Sets the date to run bad debt")]
        public DateTime CollectionsRun { get; set; }

        [Category("Billing")]
        [Description("")]
        public string BlueCrossProvNo { get; set; }

        [Category("Billing")]
        [Description("")]
        public string BlueCrossReceiverNo { get; set; }

        [Category("Billing")]
        [Description("")]
        public string BHGroupNo { get; set; }

        [Category("Billing")]
        [Description("")]
        public string BHProviderNo { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingContact { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEmail { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityCity { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityCounty { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityFax { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityFedTaxId { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityName { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityPhone { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityState { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityStreet { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingEntityZip { get; set; }

        [Category("Company")]
        [Description("")]
        public string BillingPhone { get; set; }

        [Category("Billing")]
        [Description("")]
        public string BundleInsurance { get; set; }

        [Category("Billing")]
        [Description("Users authorized to run client invoices.")]
        public string AuthorizedToRunClientBills { get; set; }

        [Category("Billing")]
        [Description("SET CBILL FILTER")]
        public string ClientBillFilter { get; set; }

        [Category("Billing")]
        [Description("")]
        public string ChampusGroupNo { get; set; }

        [Category("Other")]
        [Description("")]
        public string ChargeTotalApp { get; set; }

        [Category("Billing")]
        [Description("")]
        public string CNetReceiverId { get; set; }

        [Category("Billing")]
        [Description("CODE STATS")]
        public string CodeStats { get; set; }

        [Category("Interfaces")]
        [Description("Location for collections file")]
        public string CollectionsFileLocation { get; set; }

        [Category("Interfaces")]
        [Description("Password for Collections SFTP file upload")]
        public string CollectionsSftpPassword { get; set; }

        [Category("Interfaces")]
        [Description("Server name or IP for Collections SFTP file upload")]
        public string CollectionsSftpServer { get; set; }

        [Category("Interfaces")]
        [Description("Username for Collections SFTP file upload")]
        public string CollectionsSftpUsername { get; set; }

        [Category("Company")]
        [Description("20120112 changed from pob per carol for claimsnet")]
        public string CompanyAddress { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyCity { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyCityStateZip { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyContact { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyFax { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyName { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyPhone { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyState { get; set; }

        [Category("Company")]
        [Description("")]
        public string CompanyZip { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2Address { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2City { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2CityStateZip { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2Contact { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2Fax { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2Name { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2Phone { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2State { get; set; }

        [Category("Company")]
        [Description("")]
        public string Company2Zip { get; set; }

        [Category("Billing")]
        [Description("")]
        public DateTime ElectronicBillDate { get; set; }

        [Category("Billing")]
        [Description("")]
        public string DBMailProfile { get; set; }

        [Category("Operations")]
        [Description("use to move the day back in an hourly query")]
        public string DemoDays { get; set; }

        [Category("Environment")]
        [Description("")]
        public string Default1500Printer { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultMedicareBillPath { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultClientRequisitionPrinter { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultCytologyRequisitionPrinter { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultDetailBillPrinter { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultFilePath { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultNursingHomePrinter { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultPathologyReqPrinter { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultSpoolFileDrive { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultSpoolFilePath { get; set; }

        [Category("Environment")]
        [Description("")]
        public string DefaultUBPrinter { get; set; }

        [Category("Billing")]
        [Description("use this to turn off or on the remaining validations that are hard coded in the application")]
        public bool Diagnosis { get; set; }

        [Category("Billing")]
        [Description("Enable insurance diagnosis pointers")]
        public string DiagnosisCodePointerSelect { get; set; }

        [Category("Billing")]
        [Description("")]
        public string EobAddress { get; set; }

        [Category("Company")]
        [Description("")]
        public string FacilityAddress { get; set; }

        [Category("Company")]
        [Description("")]
        public string FacilityCity { get; set; }

        [Category("Company")]
        [Description("")]
        public string FacilityName { get; set; }

        [Category("Company")]
        [Description("")]
        public string FacilityState { get; set; }

        [Category("Company")]
        [Description("")]
        public string FacilityZip { get; set; }

        [Category("Company")]
        [Description("")]
        public string FederalTaxId { get; set; }

        [Category("Charging")]
        [Description("Enable Fee Schedules")]
        public string FeeSchedules { get; set; }

        [Category("Operations")]
        [Description("")]
        [DefaultValue(120)]
        public int FileMaintenanceAdtMessages { get; set; }

        [Category("Operations")]
        [Description("")]
        [DefaultValue(120)]
        public int FileMaintenanceMessagesInbound { get; set; }

        [Category("Charging")]
        [Description("")]
        public string GeneralHealthPanelInsurances { get; set; }

        [Category("Charging")]
        [Description("")]
        public string GeneralHealthPanelTests { get; set; }

        [Category("System")]
        [Description("")]
        [DefaultValue(10)]
        public string ICDVersion { get; set; }

        [Category("Billing")]
        [Description("date files last imported")]
        public DateTime RemitPostingDate { get; set; }

        [Category("Billing")]
        [Description("Location of remittance files for import")]
        public string RemitImportDirectory { get; set; }

        [Category("Billing")]
        [Description("Location of local remittance files")]
        public string RemitProcessingDirectory { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceCompanyAddress { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceCompanyCity { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceCompanyName { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceCompanyPhone { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceCompanyState { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceCompanyZipCode { get; set; }

        [Category("Company")]
        [Description("")]
        public string InvoiceLogoImagePath { get; set; }

        [Category("System")]
        [Description("Laboratory Director's name for reports.")]
        public string LabDirector { get; set; }

        [Category("Billing")]
        [Description("")]
        public string MedicareReceiverId { get; set; }

        [Category("Billing")]
        [Description("")]
        public string MedicareBProviderNo { get; set; }

        [Category("Billing")]
        [Description("Added to facilitate CAHABA medicare billing")]
        public string MedicareElectronicReceiverId { get; set; }

        [Category("Billing")]
        [Description("")]
        public string MedicareProviderNo { get; set; }

        [Category("Billing")]
        [Description("Added to facilitate CAHABA medicare billing")]
        public string MedicareSubmitterId { get; set; }

        [Category("Billing")]
        [Description("")]
        public string MedicaidProviderId { get; set; }

        [Category("Billing")]
        [Description("SET NURSING HOME BILL THRU DATE")]
        public string NursingHomeBillThruDate { get; set; }

        [Category("Company")]
        [Description("")]
        public string NPINumber { get; set; }

        [Category("Charging")]
        [Description("")]
        public string OBPanelInsurances { get; set; }

        [Category("Charging")]
        [Description("")]
        public string OBPanelTests { get; set; }

        [Category("Billing")]
        [Description("Added for use if outpatient becomes valid.")]
        public DateTime OutpatientBillStart { get; set; }

        [Category("Company")]
        [Description("")]
        public string PrimaryCliaNo { get; set; }

        [Category("System")]
        [Description("")]
        public bool ProcessPCCharges { get; set; }

        [Category("Charging")]
        [Description("SET QUEST EMAIL RECEIPENTS")]
        public string QuestBillingEmailRecipients { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToAddress { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToCity { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToCountry { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToOrganizationName { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToState { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToStreet { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToStreet2 { get; set; }

        [Category("Company")]
        [Description("")]
        public string RemitToZip { get; set; }

        [Category("System")]
        [Description("")]
        public string ReportingPortalUrl { get; set; }

        [Category("Operations")]
        [Description("")]
        public string RollupCpt4s { get; set; }

        [Category("Billing")]
        [Description("Discontinue the swapping of insurance's and allow secondary billing via applications")]
        public DateTime SecondaryBilling { get; set; }

        [Category("ViewerSlides")]
        [Description("")]
        public string IHCStainsQuery { get; set; }

        [Category("ViewerSlides")]
        [Description("")]
        public string SlidesQuery { get; set; }

        [Category("ViewerSlides")]
        [Description("")]
        public string SpecialStainsQuery { get; set; }

        [Category("ViewerSlides")]
        [Description("used to set access to the special grid in the application")]
        public string SpecialClientsQuery { get; set; }

        [Category("ViewerSlides")]
        [Description("New slide application billing changes")]
        public DateTime SlidesStartDate { get; set; }

        [Category("Billing")]
        [Description("Amount for small balance writeoff. Statements will not be generated for balance less than this amount.")]
        public Double SmallBalanceAmount { get; set; }

        [Category("Billing")]
        [Description("Added for use in ViewerAcc")]
        public DateTime SSIBillThruDate { get; set; }

        [Category("Billing")]
        [Description("Used by ViewerAcc to seperate the new claim methodolgy from the old method.")]
        public DateTime SSIStartDate { get; set; }

        [Category("Billing")]
        [Description("")]
        public string StatementsFileLocation { get; set; }

        [Category("Billing")]
        [Description("")]
        public string StatementsSftpPassword { get; set; }

        [Category("Billing")]
        [Description("")]
        public string StatementsSftpServer { get; set; }

        [Category("Billing")]
        [Description("")]
        public string StatementsSftpUsername { get; set; }

        [Category("System")]
        [Description("")]
        public string SystemVersion { get; set; }

        [Category("System")]
        [Description("Number of tabs that can be open at one time.")]
        [DefaultValue(4)]
        public int TabsOpenLimit { get; set; }

        [Category("Billing")]
        [Description("")]
        public string TLCProviderId { get; set; }

        [Category("Billing")]
        [Description("")]
        public string UHCProviderId { get; set; }

        [Category("Billing")]
        [Description("Set to 1 when we start using for stored procedures to skip making new accounts")]
        [DefaultValue(1)]
        public int UseBillMethod { get; set; }

        [Category("Company")]
        [Description("")]
        public string SystemNPI { get; set; }

        #endregion

        public List<SysParameter> ExtractSystemParameters()
        {
            List<SysParameter> parameters = new List<SysParameter>();

            PropertyInfo[] properties = typeof(Parameters).GetProperties();
            foreach(var property in properties)
            {
                parameters.Add(new SysParameter()
                {
                    KeyName = property.Name,
                    Value = property.GetValue(this).ToString(),
                    Description = property.GetCustomAttribute(typeof(DescriptionAttribute)).ToString(),
                    Category = property.GetCustomAttribute(typeof(CategoryAttribute)).ToString()
                });
            }

            return parameters;
        }

        public SysParameter ExtractSystemParameter(string keyName, object value)
        {
            SysParameter parameter = new SysParameter();

            parameter.KeyName = keyName;
            parameter.Value = value.ToString();

            return parameter;
        }

        public void LoadSystemParameters(List<SysParameter> parameters)
        {

            foreach(var parameter in parameters)
            {
                var prop = typeof(Parameters).GetProperty(parameter.KeyName);
                object value;
                if (prop == null)
                    continue;
                if (prop.PropertyType == typeof(DateTime))
                {
                    DateTime temp;
                    DateTime.TryParse(parameter.Value, out temp);
                    value = temp;
                }
                else if (prop.PropertyType == typeof(int))
                {
                    value = Convert.ToInt16(parameter.Value);
                }
                else if (prop.PropertyType == typeof(double))
                {
                    value = Convert.ToDouble(parameter.Value);
                }
                else if(prop.PropertyType == typeof(bool))
                {
                    if (parameter.Value == "True")
                        value = true;
                    else
                        value = false;
                }
                else
                {
                    value = parameter.Value;
                }

                prop.SetValue(this, value, null);
            }

        }
    }


}
