using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Windows.Forms;
using System.Reflection;
using LabBilling.Core.Services;

namespace LabBilling.Forms;

public partial class SystemParametersForm : Form
{
    private readonly SystemService systemService = new(Program.AppEnvironment);

    public SystemParametersForm() 
    {
        InitializeComponent();
    }

    private void SystemParametersForm_Load(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        propertyGrid.SelectedObject = Program.AppEnvironment.ApplicationParameters;
    }

    private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
        // Update System Parameter when a value changes
        SysParameter systemParameter = new SysParameter();
        systemParameter.KeyName = e.ChangedItem.Label;
        systemParameter.Value = e.ChangedItem.Value.ToString();

        PropertyInfo[] properties = typeof(ApplicationParameters).GetProperties();

        Type propertyType = typeof(ApplicationParameters);

        var pInfo = propertyType.GetProperty(systemParameter.KeyName);

        if(pInfo != null)
        {
            pInfo.SetValue(Program.AppEnvironment.ApplicationParameters, e.ChangedItem.Value);

            try
            {
                systemService.SaveSystemParameter(systemParameter);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error updating system parameter.", ex);
                MessageBox.Show("Error during update. Parameter was not updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
