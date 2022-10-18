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
    public partial class InsuranceLookup : UserControl
    {
        public delegate void SelectedValueChangedEventHandler(object source, EventArgs args);
        public event SelectedValueChangedEventHandler SelectedValueChanged;
        private Color _backgroundColor = Color.White;
        private System.Windows.Forms.DataGridView resultsDataGrid;

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

        private void InitializeResultsControl()
        {
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();

            this.resultsDataGrid.AllowUserToAddRows = false;
            this.resultsDataGrid.AllowUserToDeleteRows = false;
            this.resultsDataGrid.AllowUserToResizeColumns = false;
            this.resultsDataGrid.AllowUserToResizeRows = false;
            this.resultsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.resultsDataGrid.Location = new System.Drawing.Point(4, 22);
            this.resultsDataGrid.MultiSelect = false;
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsDataGrid.Size = new System.Drawing.Size(276, 9);
            this.resultsDataGrid.TabIndex = 1;
            this.resultsDataGrid.Visible = false;
            this.resultsDataGrid.SelectionChanged += new System.EventHandler(this.resultsDataGrid_SelectionChanged);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();

            this.Controls.Add(this.resultsDataGrid);
        }

        public int CharacterLookupCountMin { get; set; } = 3;
        public int ResultBoxHeight { get; set; } = 150;
        public List<InsCompany> Datasource { get; set; }
        public string SelectedValue 
        { 
            get
            {
                return _selectedValue;
            }
            set
            {
                skipSelectionChanged = true;
                if (!string.IsNullOrEmpty(value))
                {
                    _selectedValue = value;

                    var display = Datasource.Where(x => x.InsuranceCode == _selectedValue).First();

                    _displayValue = display.PlanName;

                    if (!string.IsNullOrEmpty(_displayValue))
                        UpdateSearchText();
                }
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

        public InsuranceLookup()
        {
            InitializeComponent();
            InitializeResultsControl();
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

                    var inscQuery =
                        from insc in Datasource
                        where insc.PlanName.StartsWith(searchTextBox.Text.ToUpper())
                        select insc;

                    if (inscQuery.Count() > 0)
                    {
                        skipSelectionChanged = true;
                        resultsDataGrid.DataSource = inscQuery.ToList();
                        resultsDataGrid.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
                        resultsDataGrid.Columns[nameof(InsCompany.PlanName)].Visible = true;
                        resultsDataGrid.Columns[nameof(InsCompany.InsuranceCode)].Visible = true;
                        resultsDataGrid.Columns[nameof(InsCompany.PlanName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                    _selectedValue = row.Cells[nameof(InsCompany.InsuranceCode)].Value.ToString();
                    _displayValue = row.Cells[nameof(InsCompany.PlanName)].Value.ToString();
                    UpdateSearchText();
                    searchTextBox.SelectionStart = searchTextBox.Text.Length;
                    searchTextBox.SelectionLength = 0;
                    OnSelectedValueChanged();
                }
            }
        }

        private void InsuranceLookup_Load(object sender, EventArgs e)
        {
            
        }
    }
}
