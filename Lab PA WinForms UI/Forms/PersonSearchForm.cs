using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using System.ComponentModel;
using WinFormsLibrary;

namespace LabBilling.Forms;

public partial class PersonSearchForm : Form
{
    List<AccountSearch> _searchResults = new();
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SelectedAccount { get; set; }

    private readonly AccountService _accountService;
    private readonly DictionaryService _dictionaryService;

    public PersonSearchForm()
    {
        Log.Instance.Trace($"Entering");
        InitializeComponent();

        PersonAccountResults.BackgroundColor = Program.AppEnvironment.WindowBackgroundColor;
        _accountService = new(Program.AppEnvironment);
        _dictionaryService = new(Program.AppEnvironment);
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        //make sure there are some values before running search
        string dobText = "";

        if (dobSearchText.MaskFull)
            dobText = dobSearchText.Text;

        _searchResults = _accountService.SearchAccounts(txtLastName.Text, txtFirstName.Text, mrnSearchText.Text, ssnSearchText.Text, dobText,
            cbSexSearch.SelectedIndex < 0 ? "" : cbSexSearch.SelectedValue.ToString(), accountSearchText.Text).ToList();

        var searchBindingList = new BindingList<AccountSearch>(_searchResults);
        var source = new BindingSource(searchBindingList, null);

        PersonAccountResults.DataSource = source;
        PersonAccountResults.SetColumnsVisibility(false);

        int i = 0;
        PersonAccountResults.Columns[nameof(AccountSearch.Account)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.Name)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.DateOfBirth)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.Sex)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.ServiceDate)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.Balance)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.FinCode)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.ClientMnem)].SetVisibilityOrder(true, i++);
        PersonAccountResults.Columns[nameof(AccountSearch.Status)].SetVisibilityOrder(true, i++);

        PersonAccountResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        PersonAccountResults.Columns[nameof(AccountSearch.Name)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

    }

    private void PersonSearchForm_Load(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        cbSexSearch.DataSource = new BindingSource(Dictionaries.SexSource, null);
        cbSexSearch.ValueMember = "Key";
        cbSexSearch.DisplayMember = "Value";

        cbSexSearch.SelectedIndex = -1;
    }

    private void PersonAccountResults_DoubleClick(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        SelectButton_Click(sender, e);
    }

    private void SelectButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        int selectedRows = PersonAccountResults.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {
            SelectedAccount = PersonAccountResults.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            this.DialogResult = DialogResult.OK;
            return;
        }
        else
        {
            MessageBox.Show("No account selected.");
            this.DialogResult = DialogResult.Cancel;
            return;
        }
    }

    private void AddAccount_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        NewAccountForm naf = new();

        if (naf.ShowDialog() == DialogResult.OK)
        {
            SelectedAccount = naf.CreatedAccount;
            ClearButton_Click(sender, e);
            accountSearchText.Text = SelectedAccount;
            SearchButton_Click(sender, e);
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        accountSearchText.Text = string.Empty;
        mrnSearchText.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtFirstName.Text = string.Empty;
        ssnSearchText.Text = string.Empty;
        dobSearchText.Text = string.Empty;
        cbSexSearch.SelectedIndex = -1;
    }
}
