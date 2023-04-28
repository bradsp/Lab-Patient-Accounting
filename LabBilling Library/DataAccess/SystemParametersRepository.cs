using System;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;
using NPOI.HSSF.Record;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace LabBilling.Core.DataAccess
{
    public sealed class SystemParametersRepository : RepositoryBase<SysParameter>
    {
        //public SystemParametersRepository(string connection) : base(connection)
        //{
                
        //}

        //public SystemParametersRepository(PetaPoco.Database db) : base(db)
        //{

        //}

        public SystemParametersRepository(IAppEnvironment appEnvironment) : base(appEnvironment) { }
        

        public string GetProductionEnvironment()
        {
            //string env = GetByKey("dbenvironment");
            string env = _appEnvironment.Environment;
            if (env == "Production")
                return "P";
            else
                return "T";
        }

        [Obsolete]
        public string GetByKey(string key)
        {
            Log.Instance.Debug($"Entering");

            SysParameter record;

            record = dbConnection.SingleOrDefault<SysParameter>($"where {GetRealColumn(nameof(SysParameter.key_name))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = key });
  
            if(string.IsNullOrEmpty(record.Value))
                throw new InvalidParameterValueException("Parameter not defined", key);
  
            return record.Value;
        }

        public string GetByKeyName(string keyName)
        {
            Log.Instance.Debug($"Entering");

            SysParameter record;

            record = dbConnection.SingleOrDefault<SysParameter>($"where {GetRealColumn(nameof(SysParameter.KeyName))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = keyName });

            if (record == null)
                return "";

            if (string.IsNullOrEmpty(record.Value))
                throw new InvalidParameterValueException("Parameter not defined", keyName);

            return record.Value;
        }

        public SysParameter GetParameter(string keyName)
        {
            Log.Instance.Debug($"Entering");

            SysParameter record;

            record = dbConnection.SingleOrDefault<SysParameter>($"where {GetRealColumn(nameof(SysParameter.KeyName))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = keyName });

            return record;
        }

        public ApplicationParameters LoadParameters()
        {
            var parameters = new ApplicationParameters();

            PropertyInfo[] properties = typeof(ApplicationParameters).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    var value = GetParameter(property.Name);
                    if(value == null)
                    {
                        //add it
                        //var categoryInfo = property.GetCustomAttribute<CategoryAttribute>();
                        //var descriptionInfo = property.GetCustomAttribute<DescriptionAttribute>();
                        //var defaultInfo = property.GetCustomAttribute<DefaultValueAttribute>();

                        //var category = categoryInfo.Category;
                        //var description = descriptionInfo.Description;
                        //var defaultValue = defaultInfo.Value;

                        var category = parameters.GetCategory(nameof(property.Name));
                        var description = parameters.GetDescription(nameof(property.Name));
                        var defaultValue = parameters.GetDefaultValue(nameof(property.Name));

                        SaveParameter(property.Name, defaultValue, category, description, property.PropertyType.Name);
                    }

                    object v = null;
                    //need to do data type conversion


                    if (property.PropertyType == typeof(string))
                        v = value.Value ?? null;
                    else if(property.PropertyType == typeof(double))
                        v = Convert.ToDouble(value.Value.ToString());
                    else if (property.PropertyType == typeof(int))
                        v = Convert.ToInt32(value.Value.ToString());
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime temp = DateTime.MinValue;
                        DateTime.TryParse(value.Value.ToString(), out temp);
                        v = temp;
                    }
                    else if (property.PropertyType == typeof(bool))
                        v = Convert.ToBoolean(value.Value);
                    else v = value.Value;



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
            PropertyInfo[] properties = typeof(ApplicationParameters).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var sysParm = new SysParameter();

                property.GetValue(sysParm);
            }
        }
    }
}
