using PetaPoco;
using System;
using System.Linq;
using System.Reflection;

namespace LabBilling.Core.Models;
public class PetaPocoModel<TPoco> where TPoco : class
{
    private TableInfo _tableInfo;
    public static string TableName
    {
        get
        {
            return GetTableInfo(typeof(TPoco)).TableName;
        }
    }
    public PetaPocoModel()
    {
        _tableInfo = GetTableInfo(typeof(TPoco));
    }

    public static TableInfo GetTableInfo(Type t)
    {
        TableInfo tableInfo = new();
        object[] customAttributes = t.GetCustomAttributes(typeof(TableNameAttribute), true);
        tableInfo.TableName = (customAttributes.Length == 0) ? t.Name : (customAttributes[0] as TableNameAttribute).Value;
        customAttributes = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
        tableInfo.PrimaryKey = (customAttributes.Length == 0) ? "ID" : (customAttributes[0] as PrimaryKeyAttribute).Value;
        tableInfo.SequenceName = (customAttributes.Length == 0) ? null : (customAttributes[0] as PrimaryKeyAttribute).SequenceName;
        tableInfo.AutoIncrement = customAttributes.Length != 0 && (customAttributes[0] as PrimaryKeyAttribute).AutoIncrement;
        return tableInfo;
    }

    private static string ColumnName(Type poco, string propertyName)
    {
        PropertyInfo pi = poco.GetProperty(propertyName);
        return ColumnName(poco, pi);
    }

    public static string ColumnName(PropertyInfo propertyInfo)
    {
        return ColumnName(typeof(TPoco), propertyInfo);
    }

    /// <summary>
    /// Returns the database column name for the POCO property.
    /// </summary>
    /// <param name="propertyName">For safety, pass propertyName using nameof(PocoClass.Property)</param>
    /// <returns>String with database column name mapped to the property.</returns>
    public static string ColumnName(string propertyName)
    {
        return ColumnName(typeof(TPoco), propertyName);
    }

    private static string ColumnName(Type t, PropertyInfo pi)
    {

        if (t == null)
            throw new ArgumentNullException(nameof(t));
        if (pi == null)
            throw new ArgumentNullException(nameof(pi));

        //this returns an array of the applied attributes (will be 0-length if no attributes are applied
        var attributes = pi.GetCustomAttributes(typeof(ColumnAttribute), false).ToArray();

        foreach (ColumnAttribute attribute in attributes.Cast<ColumnAttribute>())
        {
            if (attribute.GetType() != typeof(ResultColumnAttribute))
            {
                return attribute.Name;
            }
        }

        return "";
    }

}
