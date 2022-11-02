using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace LabBilling
{
    public static class Helper
    {
        public static string ConnVal
        {
            get
            {
                SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder();

                myBuilder.InitialCatalog = Program.Database;
                myBuilder.DataSource = Program.Server;
                myBuilder.IntegratedSecurity = true;
                myBuilder.ConnectTimeout = 30;

                return myBuilder.ConnectionString;
            }
        }

        public static string LogConnVal
        {
            get
            {
                SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder();

                myBuilder.InitialCatalog = Program.LogDatabase;
                myBuilder.DataSource = Program.Server;
                myBuilder.IntegratedSecurity = true;
                myBuilder.ConnectTimeout = 30;

                return myBuilder.ConnectionString;
            }
        }

        public static string[] GetArgs()
        {
            //ConnectionString connString = Helper.ConnVal;
            
            string[] args = new string[2];

            args[0] = Program.Server;
            args[1] = Program.Database;

            return args;
        }

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

        public static void SetControlsAccess(Control.ControlCollection controls, bool allowAccess)
        {
            foreach (Control c in controls)
            {
                if (c is TextBox)
                    (c as TextBox).ReadOnly = !allowAccess;
                if (c is MetroTextBox)
                    (c as MetroTextBox).ReadOnly = !allowAccess;
                if (c is CheckBox)
                    ((CheckBox)c).Enabled = allowAccess;
                if (c is MetroCheckBox)
                    ((MetroCheckBox)c).Enabled = allowAccess;
                if (c is ComboBox)
                    ((ComboBox)c).Enabled = allowAccess;
                if (c is MetroComboBox)
                    ((MetroComboBox)c).Enabled = allowAccess;
                if (c is MaskedTextBox)
                    ((MaskedTextBox)c).ReadOnly = !allowAccess;
                if (c is Button)
                    ((Button)c).Enabled = allowAccess;
                if (c is MetroButton)
                    ((MetroButton)c).Enabled = allowAccess;
                if (c is DataGridView)
                    ((DataGridView)c).ReadOnly = !allowAccess;
                if (c is MetroGrid)
                    ((MetroGrid)c).ReadOnly = !allowAccess;
            }
        }

        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }

        /// <summary>
        /// Returns a new DataRow from an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DataRow ToDataRow<T>(this T item) where T : class
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                //PropertyType type;
                if (prop.PropertyType.Name.Contains("Nullable"))
                    tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                else
                    tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            DataRow dr = tb.NewRow();

            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                dr[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value;
            }

            return dr;
        }

        /// <summary>
        /// Returns a DataRow from an object. Use this to pass an existing datarow to be updated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DataRow ToDataRow<T>(this T item, DataRow row) where T:class
        {
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                row[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value;
            }

            return row;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items) where T : class
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                //PropertyType type;
                if (prop.PropertyType.Name.Contains("Nullable"))
                    tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                else
                    tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null) ?? DBNull.Value;
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static void DoubleBuffered(this Control control, bool enabled)
        {
            var prop = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(control, enabled, null);
        }

    }
}
