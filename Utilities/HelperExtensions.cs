﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Utilities;

/// <summary>
/// 
/// </summary>
public static class HelperExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DataTable ConvertToDataTable<T>(List<T> data)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
            else
                table.Columns.Add(prop.Name, prop.PropertyType);
        }

        object[] values = new object[props.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DataTable ConvertToDataTable<T>(IEnumerable<T> data)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
            else
                table.Columns.Add(prop.Name, prop.PropertyType);
        }

        object[] values = new object[props.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }

    /// <summary>
    /// Generates an integer percentage value using two integers. Helpful for sending percentage complete to a progress bar.
    /// </summary>
    /// <param name="numerator"></param>
    /// <param name="denominator"></param>
    public static int ComputePercentage(int numerator, int denominator)
    {
        return (int)((double)numerator / (double)denominator * 100);
    }

    /// <summary>
    /// Makes a copy of an object. Object must be serializable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T Clone<T>(this T obj)
    {
        var serialized = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(serialized);
    }
    /// <summary>
    /// Converts a DataRow to an instance of a generic class.
    /// </summary>
    /// <typeparam name="T">The type of the class.</typeparam>
    /// <param name="row">The DataRow to convert.</param>
    /// <returns>An instance of the class populated with the DataRow values.</returns>
    public static T ConvertDataRowToObject<T>(this DataRow row) where T : new()
    {
        T obj = new T();
        foreach (var prop in typeof(T).GetProperties())
        {
            if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
            {
                prop.SetValue(obj, row[prop.Name]);
            }
        }
        return obj;
    }

}
