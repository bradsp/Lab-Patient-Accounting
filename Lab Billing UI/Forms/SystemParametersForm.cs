using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Forms
{
    public partial class SystemParametersForm : Form
    {
        public SystemParametersForm()
        {
            InitializeComponent();
        }

        private readonly SystemParametersRepository paramsdb = new SystemParametersRepository(Helper.ConnVal);

        private void SystemParametersForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //List<SystemParameters> results = new List<SystemParameters>();
            
            propertyGrid.SelectedObject = BuildDynamicClass();
        }

        protected object BuildDynamicClass()
        {
            Log.Instance.Trace($"Entering");
            // Define the dynamic assembly, module and type
            AssemblyName assemblyName = new AssemblyName("SystemParametersAssembly");
            AssemblyBuilder assemblyBuilder =
                Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("SystemParametersModule");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("ParameterType", TypeAttributes.Public);

            DataTable dt = Helper.ConvertToDataTable(paramsdb.GetAll());

            // Create dynamic properties corresponding to query results
            foreach (DataRow row in dt.Rows)
            {
                string name = row["key_name"]?.ToString();
                string category = row["category"]?.ToString();
                string description = row["description"]?.ToString();
                Type dataType = Type.GetType(row["dataType"]?.ToString());

                if (dataType != null)
                {
                    this.BuildProperty(typeBuilder, name, category, description, dataType);
                }
                else
                {
                    Log.Instance.Error($"System parameter {name} has an invalid type.");
                    MessageBox.Show($"System parameter {name} has an invalid type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Create and instantiate the dynamic type
            Type type = typeBuilder.CreateType();
            Object parameterType = Activator.CreateInstance(type, new object[] { });

            // Set each property's default value
            foreach (DataRow row in dt.Rows)
            {
                string name = row["key_name"].ToString();
                Type dataType = Type.GetType(row["dataType"].ToString());
                object value = row["value"];
                if (dataType == typeof(bool))
                {
                    bool boolValue;
                    if (bool.TryParse(value.ToString(), out boolValue))
                        value = boolValue;
                    else
                    {
                        int intValue;
                        if (int.TryParse(value.ToString(), out intValue))
                            value = System.Convert.ChangeType(intValue, dataType);
                    }

                }
                else
                {
                    value = (Convert.IsDBNull(value)) ? null : Convert.ChangeType(value, dataType);
                }
                type.InvokeMember(name,
                                    BindingFlags.SetProperty,
                                    null,
                                    parameterType,
                                    new object[] { value });
            }

            return parameterType;
        }

        protected void BuildProperty(TypeBuilder typeBuilder,
                                        string name,
                                        string category,
                                        string description,
                                        Type fieldType)
        {
            // Generate the private field/public property name pair 
            // (field begins w/LC, property begins w/UC)
            char[] chars = name.ToCharArray();

            chars[0] = char.ToLower(chars[0]);
            //string fieldName = new string(chars);
            string fieldName = "_" + name;

            //chars[0] = char.ToUpper(chars[0]);
            string propertyName = name;  new string(chars);

            // Create the private field
            FieldBuilder fieldBuilder = typeBuilder.DefineField(fieldName,
                                                                    fieldType,
                                                                    FieldAttributes.Private);

            // Create the corresponding public property
            PropertyBuilder propertyBuilder =
                typeBuilder.DefineProperty(propertyName,
                                            System.Reflection.PropertyAttributes.HasDefault,
                                            fieldType,
                                            null);

            // Define the required set of property attributes
            MethodAttributes propertyAttributes = MethodAttributes.Public |
                                                    MethodAttributes.SpecialName |
                                                    MethodAttributes.HideBySig;

            // Build the getter
            MethodBuilder getter = typeBuilder.DefineMethod("get_" + propertyName,
                                                                propertyAttributes,
                                                                fieldType,
                                                                Type.EmptyTypes);
            ILGenerator getterIlGen = getter.GetILGenerator();
            getterIlGen.Emit(OpCodes.Ldarg_0);
            getterIlGen.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIlGen.Emit(OpCodes.Ret);

            // Build the setter
            MethodBuilder setter = typeBuilder.DefineMethod("set_" + propertyName,
                                                                propertyAttributes,
                                                                null,
                                                                new Type[] { fieldType });
            ILGenerator setterIlGen = setter.GetILGenerator();
            setterIlGen.Emit(OpCodes.Ldarg_0);
            setterIlGen.Emit(OpCodes.Ldarg_1);
            setterIlGen.Emit(OpCodes.Stfld, fieldBuilder);
            setterIlGen.Emit(OpCodes.Ret);

            // Bind the getter and setter
            propertyBuilder.SetGetMethod(getter);
            propertyBuilder.SetSetMethod(setter);

            // Set the Category and Description attributes
            propertyBuilder.SetCustomAttribute(
                new CustomAttributeBuilder(
                    typeof(CategoryAttribute).GetConstructor(
                        new Type[] { typeof(string) }), new object[] { category }));
            propertyBuilder.SetCustomAttribute(
                new CustomAttributeBuilder(
                    typeof(DescriptionAttribute).GetConstructor(
                        new Type[] { typeof(string) }), new object[] { description }));
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Update System Parameter when a value changes

            MessageBox.Show(string.Format("{0} changed from {1} to {2}",
                e.ChangedItem.Label,
                e.OldValue, 
                e.ChangedItem.Value));

            SystemParameters systemParameters = new SystemParameters();
            systemParameters.key_name = e.ChangedItem.Label;
            systemParameters.value = e.ChangedItem.Value.ToString();

            try
            {
                paramsdb.Update(systemParameters, new[] { "key_name", "value" });
            }
            catch(Exception ex)
            {
                Log.Instance.Error("Error updating system parameter.", ex);
                MessageBox.Show("Error during update. Parameter was not updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
