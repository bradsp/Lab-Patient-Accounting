using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.UserControls;


/// <summary>
/// Represents a DataGridView control that supports master-detail row expansion and collapse.
/// </summary>
public class MasterDetailDataGridView : DataGridView
{
    private List<DataGridViewColumn> detailColumns = new();
    private BindingSource masterBindingSource = new();
    private BindingSource detailBindingSource = new();
    // Mapping from master column name to detail data source
    private string masterKeyColumn = string.Empty;
    private string detailKeyColumn = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="MasterDetailDataGridView"/> class.
    /// Sets up the event handler for cell clicks to manage master-detail row expansion and collapse.
    /// </summary>
    public MasterDetailDataGridView()
    {
        this.CellClick += MasterDetailDataGridView_CellClick;
    }

    /// <summary>
    /// Gets or sets the data source for the master DataGridView.
    /// </summary>
    /// <value>
    /// The data source for the master DataGridView.
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object MasterDataSource
    {
        get => masterBindingSource.DataSource;
        set
        {
            masterBindingSource.DataSource = value;
            this.DataSource = masterBindingSource;
        }
    }

    /// <summary>
    /// Gets or sets the data source for the detail DataGridView.
    /// </summary>
    /// <value>
    /// The data source for the detail DataGridView.
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object DetailDataSource
    {
        get => detailBindingSource.DataSource;
        set => detailBindingSource.DataSource = value;
    }

    /// <summary>
    /// Adds an array of columns to the main data.
    /// </summary>
    /// <param name="columns">The columns to add.</param>
    public void AddMainColumn(params DataGridViewColumn[] columns)
    {
        this.Columns.AddRange(columns);
    }

    /// <summary>
    /// Adds a single column to the main data.
    /// </summary>
    /// <param name="column">The column to add.</param>
    public void AddMainColumn(DataGridViewColumn column)
    {
        this.Columns.Add(column);
    }

    /// <summary>
    /// Adds an array of columns to the detail data.
    /// </summary>
    /// <param name="columns">The columns to add.</param>
    public void AddDetailColumn(params DataGridViewColumn[] columns)
    {
        detailColumns.AddRange(columns);
    }

    /// <summary>
    /// Adds a single column to the detail data.
    /// </summary>
    /// <param name="column">The column to add.</param>
    public void AddDetailColumn(DataGridViewColumn column)
    {
        detailColumns.Add(column);
    }

    /// <summary>
    /// Adds a mapping between a master column and a detail column.
    /// </summary>
    /// <param name="masterColumn">The name of the master column.</param>
    /// <param name="detailColumn">The name of the detail column.</param>
    public void AddColumnMapping(string masterColumn, string detailColumn)
    {
        masterKeyColumn = masterColumn;
        detailKeyColumn = detailColumn;
    }

    /// <summary>
    /// Handles the CellClick event of the DataGridView to manage row expansion and collapse.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
    private void MasterDetailDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return; // Ignore header clicks

        var clickedRow = this.Rows[e.RowIndex];
        bool isExpanded = (bool)(clickedRow.Tag ?? false); // Check if the row is expanded

        if (isExpanded)
        {
            // Collapse the row
            this.Rows.RemoveAt(e.RowIndex + 1);
            clickedRow.Tag = false; // Update the tag
        }
        else
        {
            // Expand the row
            var detailData = GetDetailData(clickedRow.DataBoundItem); // Retrieve detail data based on master row
            var detailsRow = new DataGridViewRow();
            detailsRow.CreateCells(this);

            // Fill detail cells
            for (int i = 0; i < detailColumns.Count; i++)
            {
                detailsRow.Cells[i + this.Columns.Count - detailColumns.Count].Value = "Detail Info " + (i + 1); // Customize as needed
            }

            // Insert the detail row
            this.Rows.Insert(e.RowIndex + 1, detailsRow);
            clickedRow.Tag = true; // Update the tag
        }
    }

    /// <summary>
    /// Finalizes the detail columns in the control.
    /// </summary>
    public void FinalizeDetailColumns()
    {
        foreach (var column in detailColumns)
        {
            this.Columns.Add(column);
        }
    }

    /// <summary>
    /// Retrieves detail data based on the master row.
    /// </summary>
    /// <param name="masterRowData">The master row data.</param>
    /// <returns>An array of detail data objects.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when column mappings or detail columns are not set.
    /// </exception>
    private object[] GetDetailData(object masterRowData)
    {
        //check masterKeyColumn and detailKeyColumn and throw exception if not set or is not a valid column in their respective data sources
        if (string.IsNullOrEmpty(masterKeyColumn) || string.IsNullOrEmpty(detailKeyColumn))
        {
            throw new InvalidOperationException("Master key column and detail key column must be set.");
        }

        //check to see if masterKeyColumn is a valid column in master columns
        if (!this.Columns.Contains(masterKeyColumn))
        {
            throw new InvalidOperationException("Master key column must be a valid column in the master data source.");
        }

        //check to see if detailKeyColumn is a valid column in detail columns
        if (!detailColumns.Any(c => c.Name == detailKeyColumn))
        {
            throw new InvalidOperationException("Detail key column must be a valid column in the detail data source.");
        }

        // Example: Retrieve detail data based on the master row
        var masterType = masterRowData.GetType();
        var detailData = new List<object>();

        // retrieve detail data based on the master row masterKeyColumn
        var masterKeyValue = masterType.GetProperty(masterKeyColumn)?.GetValue(masterRowData);
        if (masterKeyValue == null)
        {
            throw new InvalidOperationException("Master key value cannot be null.");
        }

        var detailDataSource = detailBindingSource.DataSource as IEnumerable<object>;
        if (detailDataSource == null)
        {
            throw new InvalidOperationException("Detail data source must be an IEnumerable<object>.");
        }

        foreach (var detailRow in detailDataSource)
        {
            var detailType = detailRow.GetType();
            var detailKeyValue = detailType.GetProperty(detailKeyColumn)?.GetValue(detailRow);
            if (detailKeyValue != null && detailKeyValue.Equals(masterKeyValue))
            {
                detailData.Add(detailRow);
            }
        }

        return detailData.ToArray();
    }
}
