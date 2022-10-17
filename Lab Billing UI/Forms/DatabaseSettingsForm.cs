using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;

namespace LabBilling.Forms
{
    public partial class DatabaseSettingsForm : Form
    {
        public DatabaseSettingsForm()
        {
            InitializeComponent();
        }

        private void DatabaseSettingsForm_Load(object sender, EventArgs e)
        {
            serverComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            databaseComboBox.DropDownStyle = ComboBoxStyle.DropDown;

            serverComboBox.Text = Properties.Settings.Default.DbServer;
            databaseComboBox.Text = Properties.Settings.Default.DbName;

        }

        private void serverComboBox_DropDown(object sender, EventArgs e)
        {
            if (serverComboBox.Items.Count <= 0)
            {
                DataTable dt = SmoApplication.EnumAvailableSqlServers(true);
                serverComboBox.ValueMember = "Name";
                serverComboBox.DataSource = dt;
            }
        }

        private void databaseComboBox_DropDown(object sender, EventArgs e)
        {
            string serverName;
            if (serverComboBox.SelectedItem != null)
                serverName = serverComboBox.SelectedItem.ToString();
            else
                serverName = serverComboBox.Text;

            if (!string.IsNullOrEmpty(serverName))
            {
                Server server = new Server(serverName);

                try
                {
                    foreach (Database database in server.Databases)
                    {
                        databaseComboBox.Items.Add(database);
                    }
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
