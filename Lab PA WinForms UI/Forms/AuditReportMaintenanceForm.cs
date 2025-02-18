using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Data;
using System.Text;
using LabBilling.Library;
using System.Security.AccessControl;

namespace LabBilling.Forms;
public partial class AuditReportMaintenanceForm : Form
{
    public AuditReportMaintenanceForm()
    {
        InitializeComponent();
        menuSelectionComboBox.SelectedIndexChanged += MenuSelectionComboBox_SelectedIndexChanged;
    }
    private bool _formLoaded = false;
    private DictionaryService _dictionaryService;
    private BindingSource _bindingSource;
    private DataTable _auditReports = new();

    // Cached database schema
    private HashSet<string> _databaseTables;
    private HashSet<string> _databaseColumns;

    private void AuditReportMaintenanceForm_Load(object sender, EventArgs e)
    {
        _dictionaryService = new(Program.AppEnvironment);
        //load grid
        try
        {
            // Cache the database schema
            _databaseTables = LoadDatabaseTables();
            _databaseColumns = LoadDatabaseColumns();

            menuSelectionComboBox.Items.AddRange([.. _dictionaryService.GetAuditReportMenus()]);
            _bindingSource = new();
            _auditReports = _dictionaryService.GetAuditReports().ToDataTable();
            _auditReports.PrimaryKey = [_auditReports.Columns[nameof(AuditReport.Id)]];
            if (_auditReports.Rows.Count > 0)
            {
                _bindingSource.DataSource = _auditReports;

                reportListDataGrid.DataSource = _bindingSource;
            }
            else
            {
                MessageBox.Show("No records found.");
                Close();
            }
            reportListDataGrid.SetColumnsVisibility(false);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.ReportName), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.ReportTitle), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.Comments), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.Button), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.IsChildButton), true);
            reportListDataGrid.Columns[nameof(AuditReport.ReportName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            reportListDataGrid.AutoResizeColumns();

            _formLoaded = true;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
        }

    }

    private void MenuSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (menuSelectionComboBox.SelectedItem != null)
        {
            string selectedButton = menuSelectionComboBox.SelectedItem.ToString();
            _bindingSource.Filter = $"{nameof(AuditReport.Button)} = '{selectedButton}'";
        }
        else
        {
            _bindingSource.RemoveFilter();
        }
    }

    private void reportListDataGrid_SelectionChanged(object sender, EventArgs e)
    {
        if (!_formLoaded)
            return;

        // If the form is not fully loaded, return
        if (!IsHandleCreated)
            return;

        if (reportListDataGrid.SelectedRows.Count <= 0)
            return;

        DataGridViewRow row = reportListDataGrid.SelectedRows[0];
        var rawSql = row.Cells[nameof(AuditReport.ReportCode)].Value.ToString();

        // Parse the raw SQL using Microsoft.SqlServer.TransactSql.ScriptDom
        var parser = new TSql120Parser(false);
        TSqlFragment fragment;
        IList<ParseError> errors;

        using (TextReader reader = new StringReader(rawSql))
        {
            fragment = parser.Parse(reader, out errors);
        }

        if (errors != null && errors.Count > 0)
        {
            // If parsing errors occur, display the raw SQL
            reportCodeTextbox.Text = rawSql;
        }
        else
        {
            // Configure formatting options
            var generatorOptions = new SqlScriptGeneratorOptions
            {
                KeywordCasing = KeywordCasing.Uppercase,
                IndentationSize = 4,
                IncludeSemicolons = true,
                AlignClauseBodies = false,
                AsKeywordOnOwnLine = false,
                NewLineBeforeFromClause = true,
                NewLineBeforeWhereClause = true,
                NewLineBeforeGroupByClause = true,
                NewLineBeforeHavingClause = true,
                NewLineBeforeOrderByClause = true,
                NewLineBeforeJoinClause = true
            };

            var scriptGenerator = new Sql120ScriptGenerator(generatorOptions);
            string formattedSql;
            scriptGenerator.GenerateScript(fragment, out formattedSql);

            reportCodeTextbox.Text = formattedSql;
        }

        reportNameTextbox.Text = row.Cells[nameof(AuditReport.ReportName)].Value.ToString();
        reportTitleTextbox.Text = row.Cells[nameof(AuditReport.ReportTitle)].Value.ToString();
        buttonTextBox.Text = row.Cells[nameof(AuditReport.Button)].Value.ToString();
        commentsTextbox.Text = row.Cells[nameof(AuditReport.Comments)].Value.ToString();
        isChildButtonCheckBox.Checked = Convert.ToBoolean(row.Cells[nameof(AuditReport.IsChildButton)].Value);
        idLabel.Text = row.Cells[nameof(AuditReport.Id)].Value.ToString();

        // Validate the formatted SQL code
        string validationErrors = ValidateSqlCode(reportCodeTextbox.Text);
        if (!string.IsNullOrEmpty(validationErrors))
        {
            validationErrorsTextBox.Text = $"\n\nValidation issues:\n{validationErrors}";
        }
    }


    private void saveButton_Click(object sender, EventArgs e)
    {
        // Retrieve the SQL code from the text box
        string sqlCode = reportCodeTextbox.Text;

        // Validate the SQL code
        string validationError = ValidateSqlCode(sqlCode);
        validationErrorsTextBox.Text = validationError;

        if (!string.IsNullOrEmpty(validationError))
        {
            MessageBox.Show($"SQL validation failed:\n{validationError}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        AuditReport auditReport = new()
        {
            ReportName = reportNameTextbox.Text,
            ReportTitle = reportTitleTextbox.Text,
            ReportCode = reportCodeTextbox.Text,
            Comments = commentsTextbox.Text,
            Button = buttonTextBox.Text,
            IsChildButton = isChildButtonCheckBox.Checked,
            Id = Convert.ToInt32(idLabel.Text)
        };

        _dictionaryService.SaveAuditReport(auditReport);
        bool add = false;
        DataRow row = _auditReports.Rows.Find(Convert.ToInt32(auditReport.Id));
        if (row == null)
        {
            //add a row
            row = _auditReports.NewRow();
            row[nameof(AuditReport.Id)] = auditReport.Id;
            add = true;
        }
        row[nameof(AuditReport.ReportName)] = auditReport.ReportName;
        row[nameof(AuditReport.ReportTitle)] = auditReport.ReportTitle;
        row[nameof(AuditReport.ReportCode)] = auditReport.ReportCode;
        row[nameof(AuditReport.Comments)] = auditReport.Comments;
        row[nameof(AuditReport.Button)] = auditReport.Button;
        row[nameof(AuditReport.IsChildButton)] = auditReport.IsChildButton;
        if (add)
            _auditReports.Rows.Add(row);

        //if save is successful, update the grid
        reportListDataGrid.Refresh();
        MessageBox.Show("Save successful", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {

    }

    private void deleteToolStripButton_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show($"Delete {reportTitleTextbox.Text}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            if (_dictionaryService.DeleteAuditReport(Convert.ToInt32(idLabel.Text)))
            {
                DataRow row = _auditReports.Rows.Find(Convert.ToInt32(idLabel.Text));
                row.Delete();
            }
        }
    }


    private string ValidateSqlCode(string sqlCode)
    {
        StringBuilder validationErrors = new();

        // Parse the SQL code
        TSql150Parser parser = new TSql150Parser(false);
        IList<ParseError> errors;
        TSqlFragment fragment;

        using (TextReader reader = new StringReader(sqlCode))
        {
            fragment = parser.Parse(reader, out errors);
        }

        if (errors != null && errors.Count > 0)
        {
            // Collect all error messages
            foreach (var error in errors)
            {
                validationErrors.AppendLine(error.Message);
            }
        }

        // Extract table and column names only if parsing was successful
        if (validationErrors.Length == 0)
        {
            var visitor = new SchemaObjectVisitor();
            fragment.Accept(visitor);

            // Validate table and column names
            string schemaValidationError = ValidateSchemaObjects(visitor.TableNames, visitor.ColumnNames, visitor.CommonTableExpressions);
            if (!string.IsNullOrEmpty(schemaValidationError))
            {
                validationErrors.AppendLine(schemaValidationError);
            }

            // Optional: Execute a dry run to catch runtime errors
            string executionError = ExecuteDryRun(sqlCode);
            if (!string.IsNullOrEmpty(executionError))
            {
                validationErrors.AppendLine(executionError);
            }
        }

        return validationErrors.ToString();


    }

    private string ValidateSchemaObjects(HashSet<string> tableNames, HashSet<string> columnNames, HashSet<string> cteNames)
    {
        // Use cached schema data
        var validTables = _databaseTables;
        var validColumns = _databaseColumns;

        // Combine valid tables with CTE names
        var allValidTables = new HashSet<string>(validTables, StringComparer.OrdinalIgnoreCase);
        allValidTables.UnionWith(cteNames);

        // Filter out placeholders in brackets from table and column names
        tableNames.RemoveWhere(name => IsPlaceholder(name));
        columnNames.RemoveWhere(name => IsPlaceholder(name));

        // Check for invalid tables
        var invalidTables = tableNames.Except(allValidTables).ToList();
        if (invalidTables.Any())
        {
            return $"Invalid table(s): {string.Join(", ", invalidTables)}";
        }

        // Check for invalid columns
        var invalidColumns = columnNames.Except(validColumns).ToList();
        if (invalidColumns.Any())
        {
            return $"Invalid column(s): {string.Join(", ", invalidColumns)}";
        }

        return null;
    }

    private bool IsPlaceholder(string name)
    {
        return name.StartsWith("{") && name.EndsWith("}");
    }

    private string ExecuteDryRun(string sqlCode)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(Program.AppEnvironment.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SET FMTONLY ON; {sqlCode}; SET FMTONLY OFF;", conn);
                cmd.ExecuteNonQuery();
            }
        }
        catch (SqlException ex)
        {
            return $"SQL execution error: {ex.Message}";
        }
        return null;
    }

    // Method to load and cache database table names
    private HashSet<string> LoadDatabaseTables()
    {
        HashSet<string> tableNames = new(StringComparer.OrdinalIgnoreCase);
        using (SqlConnection conn = new(Program.AppEnvironment.ConnectionString))
        {
            conn.Open();
            SqlCommand cmd = new("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableNames.Add(reader.GetString(0));
                }
            }
        }
        return tableNames;
    }

    // Method to load and cache database column names
    private HashSet<string> LoadDatabaseColumns()
    {
        HashSet<string> columnNames = new(StringComparer.OrdinalIgnoreCase);
        using (SqlConnection conn = new(Program.AppEnvironment.ConnectionString))
        {
            conn.Open();
            SqlCommand cmd = new("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS", conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnNames.Add(reader.GetString(0));
                }
            }
        }
        return columnNames;
    }

    private void fixSqlCodeButton_Click(object sender, EventArgs e)
    {
        try
        {
            var (modifiedSql, parameters) = SqlParameterConverter.ConvertToSqlParameters(reportCodeTextbox.Text);

            // Display results in a modal dialog box. Show old code and new code side by side
            using var dialog = new Form();
            dialog.Text = "SQL Code Comparison";
            dialog.Size = new Size(800, 600);

            var oldCodeTextbox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Left,
                Width = dialog.ClientSize.Width / 2,
                Text = reportCodeTextbox.Text
            };

            var newCodeTextbox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Right,
                Width = dialog.ClientSize.Width / 2,
                Text = modifiedSql
            };

            dialog.Controls.Add(oldCodeTextbox);
            dialog.Controls.Add(newCodeTextbox);

            // Add resize event handler to adjust text boxes
            dialog.Resize += (s, args) =>
            {
                oldCodeTextbox.Width = dialog.ClientSize.Width / 2;
                newCodeTextbox.Width = dialog.ClientSize.Width / 2;
            };

            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error converting SQL code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
}


class SchemaObjectVisitor : TSqlFragmentVisitor
{
    public HashSet<string> TableNames { get; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> ColumnNames { get; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> CommonTableExpressions { get; } = new(StringComparer.OrdinalIgnoreCase);

    public override void Visit(CommonTableExpression node)
    {
        string cteName = node.ExpressionName.Value;
        if (!string.IsNullOrEmpty(cteName))
        {
            CommonTableExpressions.Add(cteName);
        }
        base.Visit(node);
    }

    public override void Visit(NamedTableReference node)
    {
        string tableName = node.SchemaObject.BaseIdentifier.Value;

        if (!string.IsNullOrEmpty(tableName))
        {
            TableNames.Add(tableName);
        }
        base.Visit(node);
    }

    public override void Visit(ColumnReferenceExpression node)
    {
        string columnName = node.MultiPartIdentifier.Identifiers.Last().Value;

        if (!string.IsNullOrEmpty(columnName))
        {
            ColumnNames.Add(columnName);
        }

        base.Visit(node);
    }
}
