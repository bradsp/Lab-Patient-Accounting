using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace LabBilling.Core.Models;

public sealed partial class ApplicationParameters
{
    private const string _collectionsCategory = "Collections";
    private const string _accountingCategory = "Accounting";
    private const string _billingCategory = "Billing";
    private const string _chargingCategory = "Charging";
    private const string _companyCategory = "Company";
    private const string _documentationSiteCategory = "DocumentationSite";
    private const string _environmentCategory = "Environment";
    private const string _invoicingCategory = "Invoicing";
    private const string _pathologyGroupCategory = "PathologyGroup";
    private const string _operationsCategory = "Operations";
    private const string _systemCategory = "System";
    private const string _otherCategory = "Other";
    private const string _viewerSlidesCategory = "ViewerSlides";

    public static ApplicationParameters Load(string xml)
    {
        XmlSerializer xmlSerializer = new(typeof(ApplicationParameters));
        using StringReader xmlReader = new(xml);

        var appParm = (ApplicationParameters)xmlSerializer.Deserialize(xmlReader);
        return appParm;
    }

    public static string GetDescription(string propertyName)
    {
        var prop = typeof(ApplicationParameters).GetProperty(propertyName);
        var descriptionInfo = prop.GetCustomAttribute<DescriptionAttribute>();
        var description = descriptionInfo.Description;

        return description;
    }

    public static string GetCategory(string propertyName)
    {
        var prop = typeof(ApplicationParameters).GetProperty(propertyName);
        var attributeInfo = prop.GetCustomAttribute<CategoryAttribute>();
        var attributeValue = attributeInfo.Category;

        return attributeValue;
    }

    public static object GetDefaultValue(string propertyName)
    {
        var prop = typeof(ApplicationParameters).GetProperty(propertyName);
        var attributeInfo = prop.GetCustomAttribute<DefaultValueAttribute>();
        var attributeValue = attributeInfo?.Value ?? "";

        return attributeValue;
    }

    public string GetProductionEnvironment()
    {
        string env = this.DatabaseEnvironment;
        if (env == "Production")
            return "P";
        else
            return "T";
    }

    public class LogLevelTypeConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> list = new()
            {
                "",
                "Trace",
                "Debug",
                "Info",
                "Warn",
                "Error",
                "Fatal",
            };
            return new StandardValuesCollection(list);
        }
    }

    public class LogLocationTypeConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> list = new()
            {
                "",
                "Database",
                "FilePath"
            };
            return new StandardValuesCollection(list);
        }
    }
}
