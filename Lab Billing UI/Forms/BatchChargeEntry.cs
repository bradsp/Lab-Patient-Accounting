using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBilling.Forms
{
    public partial class BatchChargeEntry : Form
    {
        public List<BatchCharge> Charges { get; set; }
        private readonly ClientRepository dbClient = new ClientRepository(Helper.ConnVal);
        private readonly CdmRepository dbCdm = new CdmRepository(Helper.ConnVal);

        public BatchChargeEntry()
        {
            InitializeComponent();
        }

        private void dgvBatchEntry_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BatchChargeEntry_Load(object sender, EventArgs e)
        {
            List<Client> clients = dbClient.GetAll().ToList();
            Client.DataSource = clients;
            Client.ValueMember = "cli_mnem";
            Client.DisplayMember = "cli_nme";

            dgvBatchEntry.DataSource = Charges;

        }

        private void dgvBatchEntry_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvBatchEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvBatchEntry.Columns[e.ColumnIndex].Name == "CDM")
            {

                Cdm cdm = new Cdm();

                //look up cdm number and get amount
                cdm = dbCdm.GetCdm(dgvBatchEntry[e.ColumnIndex, e.RowIndex].Value.ToString());

                MessageBox.Show(string.Format("CDM Description: {0}",cdm.descript));

                //if cdm is not valid, show an error


            }
        }

        private void SaveCharges_Click(object sender, EventArgs e)
        {
            //loop through rows to write charges

        }

        private void CDM_Leave(object sender, EventArgs e)
        {
            //look up cdm number and get amount
            Cdm cdm = dbCdm.GetCdm(CDM.Text);

            if (cdm == null)
            {
                MessageBox.Show(string.Format("CDM {0} not found", CDM.Text));
                CDM.BackColor = Color.Red;
            }
            else
            {
                MessageBox.Show(string.Format("CDM Description: {0}", cdm.descript));
                CDM.BackColor = Color.White;
            }

            //if cdm is not valid, show an error

        }

        private void AddChargeToGrid_Click(object sender, EventArgs e)
        {

        }
    }
}
