using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class ClientMaintenanceForm : Form
    {
        public ClientMaintenanceForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
        }

        private readonly ClientRepository clientRepository = new ClientRepository(Helper.ConnVal);
        private readonly GLCodeRepository gLCodeRepository = new GLCodeRepository(Helper.ConnVal);

        private List<Client> _clientList = null;

        private void Clients_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            _clientList = DataCache.Instance.GetClients(); // db.GetAll().ToList();
            dgvClients.DataSource = _clientList;
            Log.Instance.Trace($"Exiting");
        }

        private void dgvClients_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string clientMnem = dgvClients[nameof(Client.ClientMnem), e.RowIndex].Value.ToString();

            ClientMaintenanceEditForm editForm = new ClientMaintenanceEditForm();
            editForm.SelectedClient = clientMnem;

            if(editForm.ShowDialog() == DialogResult.OK)
            {
                Client client = editForm.client;
                var record = _clientList.Find(c => c.ClientMnem == client.ClientMnem);
                record = client;
                try
                {
                    clientRepository.Update(client);
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex);
                    MessageBox.Show("Error updating client. Changes not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }

        }

        private void includeDeletedCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
