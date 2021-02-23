using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LabBilling
{
    public static class Helper
    {
        private static string _environment = "";

        public static string ConnVal
        {

            get
            {
                if (_environment == null || _environment == "") //default to MCLTEST if value has not been set
                    return ConfigurationManager.ConnectionStrings["MCLTEST"].ConnectionString;
                else
                    return ConfigurationManager.ConnectionStrings[_environment].ConnectionString;
            }
                
            set 
            { 
                _environment = value;
                args[1] = "/" + _environment;
            }

            //if testing
            //return ConfigurationManager.ConnectionStrings["MCLTEST"].ConnectionString;

            //if production
            //return ConfigurationManager.ConnectionStrings["MCLLIVE"].ConnectionString;
        }

        public static string Environment
        {
            get { return _environment; }
            set 
            { 
                _environment = value;
                args[1] = "/" + _environment;
            }
        }

        public static string[] args = new string[]
        {
            "/WTHMCLBILL",
            Environment
        };

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        //public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        //{
        //    List<IDataRecord> list = data.Cast<IDataRecord>().ToList();

        //    PropertyDescriptorCollection props = null;
        //    DataTable table = new DataTable();
        //    if (list != null && list.Count > 0)
        //    {
        //        props = TypeDescriptor.GetProperties(list[0]);
        //        for (int i = 0; i < props.Count; i++)
        //        {
        //            PropertyDescriptor prop = props[i];
        //            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //        }
        //    }
        //    if (props != null)
        //    {
        //        object[] values = new object[props.Count];
        //        foreach (T item in data)
        //        {
        //            for (int i = 0; i < values.Length; i++)
        //            {
        //                values[i] = props[i].GetValue(item) ?? DBNull.Value;
        //            }
        //            table.Rows.Add(values);
        //        }
        //    }
        //    return table;
        //}

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
    }
}
