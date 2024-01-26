using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;
using WinFormsLibrary;

namespace LabBilling.Forms
{
    public partial class PhysicianMaintenanceForm : BaseForm
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
            PhysicianDGV.SetColumnsVisibility(false);

            int z = 0;
            PhysicianDGV.Columns[nameof(Phy.NpiId)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.BillingNpi)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.FullName)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.Credentials)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.ProviderGroup)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.Address1)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.Address2)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.City)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.State)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.ZipCode)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.Phone)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.DoctorNumber)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.ClientMnem)].SetVisibilityOrder(true, z++);
            PhysicianDGV.Columns[nameof(Phy.LISMnem)].SetVisibilityOrder(true, z++);

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
