using System;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using LabBilling.Core.UnitOfWork;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess
{
    public sealed class SystemParametersRepository : RepositoryBase<SysParameter>
    {

        public SystemParametersRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }
        

        public string GetProductionEnvironment()
        {
            string env = AppEnvironment.Environment;
            if (env == "Production")
                return "P";
            else
                return "T";
        }

        [Obsolete]
        public string GetByKey(string key)
        {
            Log.Instance.Trace($"Entering");

            SysParameter record;

            record = Context.SingleOrDefault<SysParameter>($"where {GetRealColumn(nameof(SysParameter.key_name))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = key });
  
            if(string.IsNullOrEmpty(record.Value))
                throw new InvalidParameterValueException("Parameter not defined", key);
  
            return record.Value;
        }

        public string GetByKeyName(string keyName)
        {
            Log.Instance.Trace($"Entering");

            SysParameter record;

            record = Context.SingleOrDefault<SysParameter>($"where {GetRealColumn(nameof(SysParameter.KeyName))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = keyName });

            if (record == null)
                return "";

            if (string.IsNullOrEmpty(record.Value))
                throw new InvalidParameterValueException("Parameter not defined", keyName);

            return record.Value;
        }

        public SysParameter GetParameter(string keyName)
        {
            Log.Instance.Trace($"Entering");

            SysParameter record;

            record = Context.SingleOrDefault<SysParameter>($"where {GetRealColumn(nameof(SysParameter.KeyName))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = keyName });

            return record;
        }

        public ApplicationParameters LoadParameters()
        {
            Log.Instance.Trace("Entering");
            var parameters = new ApplicationParameters();

            PropertyInfo[] properties = typeof(ApplicationParameters).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    var value = GetParameter(property.Name);
                    if(value == null)
                    {
                        var category = ApplicationParameters.GetCategory(property.Name);
                        var description = ApplicationParameters.GetDescription(property.Name);
                        var defaultValue = ApplicationParameters.GetDefaultValue(property.Name) ?? "";

                        SaveParameter(property.Name, defaultValue, category, description, property.PropertyType.Name);
                    }

                    object v = null;
                    //need to do data type conversion

                    if (property.PropertyType == typeof(string))
                        v = value?.Value ?? null;
                    else if(property.PropertyType == typeof(double))
                        v = Convert.ToDouble(value?.Value.ToString());
                    else if (property.PropertyType == typeof(int))
                        v = Convert.ToInt32(value?.Value.ToString());
                    else if (property.PropertyType == typeof(Int16))
                        v = Convert.ToInt16(value?.Value.ToString());
                    else if (property.PropertyType == typeof(Int32))
                        v = Convert.ToInt32(value?.Value.ToString());
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime temp = DateTime.MinValue;
                        bool v1 = DateTime.TryParse(value?.Value.ToString(), out temp);
                        v = temp;
                    }
                    else if (property.PropertyType == typeof(bool))
                        v = Convert.ToBoolean(value?.Value);
                    else v = value?.Value;



                    property.SetValue(parameters, v);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error loading parameters.", ex);
                }            
            }

            return parameters;
        }

        public void SaveParameter(string keyName, object value, string category, string description, string dataType)
        {
            Log.Instance.Trace("Entering");
            try
            {
                SysParameter parm = GetParameter(keyName);

                if (parm == null)
                {
                    parm = new SysParameter
                    {
                        KeyName = keyName,
                        Value = value.ToString(),
                        Category = category,
                        Description = description,
                        key_name = keyName,
                        DataType = dataType
                    };
                    Add(parm);
                }
                else
                {
                    parm.KeyName = keyName;
                    parm.Value = value.ToString();
                    parm.Category = category;
                    parm.Description = description;
                    parm.DataType = dataType;
                    Update(parm);

                }
            }
            catch (Exception)
            {

            }

        }

        public void SaveParameters(ApplicationParameters parameters)
        {
            Log.Instance.Trace("Entering");
            PropertyInfo[] properties = typeof(ApplicationParameters).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var sysParm = new SysParameter();

                property.GetValue(sysParm);
            }
        }
    }
}
