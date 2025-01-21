using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using WinFormsLibrary;
using LabBilling.Core.Services;

namespace LabBilling.Forms;

public partial class ClientMaintenanceForm : Form
{
    private readonly System.Windows.Forms.Timer _timer;
    private readonly int _timerDelay = 650;

    private readonly DictionaryService _dictionaryService;
    private readonly BindingSource _clientSource = new();
    private List<Client> _clientList = null;
    private DataTable _clientTable = null;
    //private IEnumerable<Client> clientQuery = null;

    public ClientMaintenanceForm() 
    {
        Log.Instance.Trace($"Entering");
        InitializeComponent();
        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerDelay };
        _timer.Tick += new EventHandler(filterTextBox_KeyUpDone);
        _dictionaryService = new(Program.AppEnvironment);
    }

    private void Clients_Load(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");

        _clientList = DataCache.Instance.GetClientsIncludeInactive();

        _clientTable = _clientList.ToDataTable();
        _clientTable.PrimaryKey = new DataColumn[] { _clientTable.Columns[nameof(Client.ClientMnem)] };

        _clientSource.DataSource = _clientTable;
        dgvClients.DataSource = _clientSource;
        
        _clientTable.DefaultView.RowFilter = $"{nameof(Client.IsDeleted)} = false";
        
        dgvClients.SetColumnsVisibility(false);
        int i = 0;
        dgvClients.Columns[nameof(Client.IsDeleted)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.ClientMnem)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.Name)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.StreetAddress1)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.StreetAddress2)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.City)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.State)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.ZipCode)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.FacilityNo)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.Type)].SetVisibilityOrder(true, i++);
        dgvClients.Columns[nameof(Client.BillMethod)].SetVisibilityOrder(true, i++);

        dgvClients.Columns[nameof(Client.Name)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dgvClients.AutoResizeColumns();

        Log.Instance.Trace($"Exiting");
    }

    private void dgvClients_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        string clientMnem = dgvClients[nameof(Client.ClientMnem), e.RowIndex].Value.ToString();

        ClientMaintenanceEditForm editForm = new()
        {
            SelectedClient = clientMnem
        };

        if (editForm.ShowDialog() == DialogResult.OK)
        {
            Client client = editForm.client;
            var record = _clientTable.Rows.Find(client.ClientMnem);
            try
            {
                _dictionaryService.SaveClient(client);

                record = client.ToDataRow(record);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                MessageBox.Show("Error updating client. Changes not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DataCache.Instance.ClearClientCache();
        }

    }

    private void includeDeletedCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (includeDeletedCheckBox.Checked)
        {
            _clientTable.DefaultView.RowFilter = String.Empty;
        }
        else
        {
            _clientTable.DefaultView.RowFilter = $"{nameof(Client.IsDeleted)} = false";
        }
    }

    private void newClientButton_Click(object sender, EventArgs e)
    {
        ClientMaintenanceEditForm editForm = new ClientMaintenanceEditForm();
        if (editForm.ShowDialog() == DialogResult.OK)
        {
            Client client = editForm.client;
            var record = _clientTable.Rows.Find(client.ClientMnem);
            if (record != null)
            {
                record = client.ToDataRow(record);
            }
            else
            {
                record = _clientTable.NewRow();
                _clientTable.Rows.Add(client.ToDataRow(record));
            }
            try
            {
                client = _dictionaryService.SaveClient(client);
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"Error adding client {client.ClientMnem} - {client.Name}. Changes not saved.", ex);
                MessageBox.Show("Error adding client. Changes not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DataCache.Instance.ClearClientCache();
        }
    }

    private void filterTextBox_KeyUpDone(object sender, EventArgs e)
    {

        if (includeDeletedCheckBox.Checked)
        {
            _clientTable.DefaultView.RowFilter = 
                $"{nameof(Client.Name)} LIKE '%{filterTextBox.Text}%' or {nameof(Client.ClientMnem)} = '{filterTextBox.Text.ToUpper()}'";

        }
        else
        {
            _clientTable.DefaultView.RowFilter =
                $"({nameof(Client.Name)} LIKE '%{filterTextBox.Text}%' or {nameof(Client.ClientMnem)} = '{filterTextBox.Text.ToUpper()}') and {nameof(Client.IsDeleted)} = false";

        }

    }

    private void filterTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        _timer.Stop();
        _timer.Start();
    }

    private void dgvClients_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
    {
        if ((bool)dgvClients[nameof(Client.IsDeleted), e.RowIndex].Value == true)
        {
            dgvClients.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
        }
    }
}
