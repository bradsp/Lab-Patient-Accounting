using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;
using MicroRuleEngine;

namespace LabBilling.Forms
{
    public partial class PhysicianMaintenanceForm : Form
    { 
        public PhysicianMaintenanceForm()
        {
            InitializeComponent();
        }

        private readonly PhyRepository phydb = new PhyRepository(Program.AppEnvironment);
        private List<Phy> physicians = new List<Phy>();
        private BindingList<Phy> bindingList = new BindingList<Phy>();
        private BindingSource bindingSource = new BindingSource();

        private void PhysicianMaintenanceForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
        }

        private void LoadProviderGrid()
        {
            PhysicianDGV.Columns[nameof(Phy.rowguid)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.reserved)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.mod_user)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.mod_date)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.mod_prg)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.mod_host)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.Upin)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.Ub92Upin)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.PathologistCode)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.LastName)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.FirstName)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.MiddleInitial)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.uri)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.Pathologist)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.PathologistCode)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.LabelPrintCount)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.uri)].Visible = false;
            PhysicianDGV.Columns[nameof(Phy.OvCode)].Visible = false;

            int z = 0;
            PhysicianDGV.Columns[nameof(Phy.NpiId)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.BillingNpi)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.FullName)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.Credentials)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.ProviderGroup)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.Address1)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.Address2)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.City)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.State)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.ZipCode)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.Phone)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.DoctorNumber)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.ClientMnem)].DisplayIndex = z++;
            PhysicianDGV.Columns[nameof(Phy.LISMnem)].DisplayIndex = z++;

            PhysicianDGV.AutoResizeColumns();

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //add new physician
            PhysicianMaintenanceEditForm editForm = new PhysicianMaintenanceEditForm();
            editForm.PhyModel = new Phy();
            if(editForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var existing = phydb.GetByNPI(editForm.PhyModel.NpiId);
                    if (existing != null)
                    {
                        if(MessageBox.Show("Provider already exists. Save anyway?", "Existing Provider", MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            phydb.Save(editForm.PhyModel);
                        }
                        else
                        {
                            return;
                        }
                    }

                    phydb.Save(editForm.PhyModel);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            bindingList.Clear();
            PhysicianDGV.DataSource = null;
            PhysicianDGV.Refresh();

            if(string.IsNullOrEmpty(searchText.Text))
            {
                MessageBox.Show("Please enter a search term");
                return;
            }

            physicians = phydb.GetByName(searchText.Text, "").ToList();
            bindingList.AddRange(physicians);
            PhysicianDGV.DataSource = bindingList;
            LoadProviderGrid();
        }

        private void PhysicianDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PhysicianMaintenanceEditForm editForm = new PhysicianMaintenanceEditForm();
            var phy = phydb.GetByNPI(PhysicianDGV.Rows[e.RowIndex].Cells[nameof(Phy.NpiId)].Value.ToString());

            editForm.PhyModel = phy;

            if(editForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    phydb.Save(editForm.PhyModel);
                    buttonSearch_Click(sender, e);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
