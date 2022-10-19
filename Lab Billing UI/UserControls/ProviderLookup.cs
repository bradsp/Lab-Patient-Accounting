using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using LabBilling.Core.Models;

namespace LabBilling.Library
{
    public partial class ProviderLookup : UserControl
    {
        public delegate void SelectedValueChangedEventHandler(object source, EventArgs args);
        public event SelectedValueChangedEventHandler SelectedValueChanged;
        private Color _backgroundColor = Color.White;
        public override Color BackColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                searchTextBox.BackColor = value;
            }
        }

        public int CharacterLookupCountMin { get; set; } = 3;
        public int ResultBoxHeight { get; set; } = 150;
        public List<Phy> Datasource { get; set; }
        public string SelectedValue 
        { 
            get
            {
                return _selectedValue;
            }
            set
            {
                skipSelectionChanged = true;
                _selectedValue = value;
                if(!string.IsNullOrEmpty(_displayValue))
                    UpdateSearchText();
                skipSelectionChanged = false;
            }
        }
        public string DisplayValue
        {
            get
            {
                return _displayValue;
            }
            
            set
            {
                skipSelectionChanged = true;
                _displayValue = value;
                if(!string.IsNullOrEmpty(_selectedValue))
                    UpdateSearchText();
                skipSelectionChanged = false;
            }
        }

        private const int _timerInterval = 650;
        private bool skipSelectionChanged = false;
        private System.Windows.Forms.Timer _timer;
        private string _selectedValue = null;
        private string _displayValue = null;

        private void UpdateSearchText()
        {
            searchTextBox.Text = $"{_displayValue} ({_selectedValue})";
        }

        public ProviderLookup()
        {
            InitializeComponent();
            resultsDataGrid.Visible = false;
            resultsDataGrid.RowHeadersVisible = false;

            _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(searchTextBox_OnKeyUpDone);
        }

        private void searchTextBox_OnKeyUpDone(object sender, EventArgs args)
        {
            _timer.Stop();

            if (Datasource != null)
            {
                //load box with selectable entries once there are the minumum number of letters in the box
                if (searchTextBox.Text.Length >= CharacterLookupCountMin)
                {
                    if (!resultsDataGrid.Visible)
                    {
                        //get screen height remaining
                        var heightRemaining = Parent.Height - this.Location.Y;
                        if (heightRemaining < ResultBoxHeight)
                            resultsDataGrid.Height = heightRemaining;
                        else
                            resultsDataGrid.Height = ResultBoxHeight;

                        resultsDataGrid.Visible = true;

                    }

                    var providerQuery =
                        from prov in Datasource
                        where prov.FullName.StartsWith(searchTextBox.Text.ToUpper())
                        select prov;

                    if (providerQuery.Count() > 0)
                    {
                        skipSelectionChanged = true;
                        resultsDataGrid.DataSource = providerQuery.ToList();
                        resultsDataGrid.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
                        resultsDataGrid.Columns[nameof(Phy.FullName)].Visible = true;
                        resultsDataGrid.Columns[nameof(Phy.NpiId)].Visible = true;
                        resultsDataGrid.Columns[nameof(Phy.FullName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        resultsDataGrid.AutoResizeColumns();
                        resultsDataGrid.ClearSelection();
                        skipSelectionChanged = false;
                    }
                }
            }
        }

        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        protected virtual void OnSelectedValueChanged()
        {
            if(SelectedValue != null)
                SelectedValueChanged(this, null);
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            resultsDataGrid.Visible = false;
        }

        private void resultsDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (!skipSelectionChanged)
            {
                DataGridViewRow row = resultsDataGrid.SelectedRows[0];
                if (row != null)
                {
                    _selectedValue = row.Cells[nameof(Phy.NpiId)].Value.ToString();
                    _displayValue = row.Cells[nameof(Phy.FullName)].Value.ToString();
                    UpdateSearchText();
                    searchTextBox.SelectionStart = searchTextBox.Text.Length;
                    searchTextBox.SelectionLength = 0;
                    OnSelectedValueChanged();
                }
            }
        }
    }
}
