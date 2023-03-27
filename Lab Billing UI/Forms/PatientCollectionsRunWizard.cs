using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core;
using LabBilling.Core.BusinessLogic;
using LabBilling.Logging;
using RFClassLibrary;

namespace LabBilling.Forms
{
    public partial class PatientCollectionsRunWizard : Form
    {
        private PatientBilling patientBilling = new PatientBilling(Helper.ConnVal);
        private bool errorEncountered = false;

        public PatientCollectionsRunWizard()
        {
            InitializeComponent();
        }

        private DialogResult AskError(string errorText)
        {
            var result = MessageBox.Show(errorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return result;
        }

        private void sendToCollectionsStartButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            try
            {
                patientBilling.SendToCollections();


            }
            catch(ApplicationException apex)
            {
                Log.Instance.Error(apex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void createStmtFileStartButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            try
            {
                patientBilling.CreateStatementFile();
            }
            catch (ApplicationException apex)
            {
                Log.Instance.Error(apex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void compileStmtsStartButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            try
            {
                patientBilling.CompileStatements(DateTimeHelper.GetLastDayOfPrevMonth());
            }
            catch(ArgumentException argex)
            {
                Log.Instance.Error(argex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch(ApplicationException apex)
            {
                Log.Instance.Error(apex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                AskError("Error occurred in SendToCollections. Patient Billing will be aborted. Notify your administrator.");
                errorEncountered = true;
                DialogResult = DialogResult.Cancel;
                return;
            }
        }
    }
}
