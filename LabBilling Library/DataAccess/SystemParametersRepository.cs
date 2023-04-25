using System;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;
using NPOI.HSSF.Record;
using System.Reflection;
using System.Windows.Input;

namespace LabBilling.Core.DataAccess
{
    public sealed class SystemParametersRepository : RepositoryBase<SysParameter>
    {
        public SystemParametersRepository(string connection) : base(connection)
        {
                
        }

        public SystemParametersRepository(PetaPoco.Database db) : base(db)
        {

        }

        public string GetProductionEnvironment()
        {
            string env = GetByKey("dbenvironment");
            if (env == "Production")
                return "P";
            else
                return "T";
        }

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

        public void LoadParameters(ApplicationParameters parameters)
        {
            PropertyInfo[] properties = typeof(ApplicationParameters).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    var value = GetByKeyName(property.Name);
                    property.SetValue(parameters, value);
                }
                catch(Exception ex)
                {
                    //property.SetValue(parameters, new object());
                }            
            }
        }

        public void SaveParameter(string keyName, object value, string category, string description)
        {
            try
            {
                SysParameter parm = GetParameter(keyName);
                if (parm == null)
                {
                    parm = new SysParameter();

                    parm.KeyName = keyName;
                    parm.Value = value.ToString();
                    parm.Category = category;
                    parm.Description = description;
                    parm.key_name = keyName;
                    Add(parm);
                }
                else
                {
                    parm.KeyName = keyName;
                    parm.Value = value.ToString();
                    parm.Category = category;
                    parm.Description = description;
                    Update(parm);

                }
            }
            catch(Exception e)
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
