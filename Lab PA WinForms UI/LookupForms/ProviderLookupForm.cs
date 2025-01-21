using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsLibrary;

namespace LabBilling.LookupForms
{
    public partial class ProviderLookupForm : Form
    {
        public int CharacterLookupCountMin { get; set; } = 3;
        public List<Phy> Datasource { get; set; }
        public string InitialSearchText { get; set; }
        public string SelectedValue { get; set; }
        public Phy SelectedPhy { get; set; }

        private const int _timerInterval = 650;
        private bool skipSelectionChanged = false;
        private System.Windows.Forms.Timer _timer;
        public ProviderLookupForm()
        {
            InitializeComponent();
            _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(searchTextBox_OnKeyUpDone);
        }

        private void LookupForm_Load(object sender, EventArgs e)
        {
            searchTextBox.Focus();
        }

        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void searchTextBox_OnKeyUpDone(object sender, EventArgs args)
        {
            _timer.Stop();

            if (Datasource != null)
            {
                //load box with selectable entries once there are the minumum number of letters in the box
                if (searchTextBox.Text.Length >= CharacterLookupCountMin)
                {
                    var phyQuery =
                        from phy in Datasource
                        where phy.LastName.ToUpper().Contains(searchTextBox.Text.ToUpper()) || phy.FirstName.ToUpper().Equals(searchTextBox.Text.ToUpper())
                        orderby phy.FullName
                        select phy;

                    if (phyQuery.Any())
                    {
                        skipSelectionChanged = true;
                        resultsDataGrid.DataSource = phyQuery.ToList();
                        resultsDataGrid.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
                        int z = 1;
                        resultsDataGrid.Columns[nameof(Phy.FullName)].SetVisibilityOrder(true, z++);
                        resultsDataGrid.Columns[nameof(Phy.NpiId)].SetVisibilityOrder(true, z++);
                        resultsDataGrid.Columns[nameof(Phy.Address1)].SetVisibilityOrder(true, z++);
                        resultsDataGrid.Columns[nameof(Phy.City)].SetVisibilityOrder(true, z++);
                        resultsDataGrid.Columns[nameof(Phy.State)].SetVisibilityOrder(true, z++);
                        resultsDataGrid.Columns[nameof(Phy.ZipCode)].SetVisibilityOrder(true, z++);
                        resultsDataGrid.Columns[nameof(Phy.FullName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        resultsDataGrid.AutoResizeColumns();
                        resultsDataGrid.ClearSelection();
                        skipSelectionChanged = false;
                    }
                }
            }
        }

        private void resultsDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (!skipSelectionChanged)
            {
                if (resultsDataGrid.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = resultsDataGrid.SelectedRows[0];
                    if (row != null)
                    {
                        SelectedPhy = (Phy)row.DataBoundItem;
                    }
                }
            }
        }

        private void LookupForm_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(InitialSearchText))
            {
                searchTextBox.Text = InitialSearchText;
                searchTextBox_OnKeyUpDone(sender, e);
                searchTextBox.Focus();
            }
        }

        private void resultsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectButton_Click(sender, e);
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
