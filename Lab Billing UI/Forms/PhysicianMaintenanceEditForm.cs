using LabBilling.Core.Models;
using MetroFramework.Forms;
using MicroRuleEngine;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class PhysicianMaintenanceEditForm : MetroForm
    {
        public PhysicianMaintenanceEditForm()
        {
            InitializeComponent();

            this.SuspendLayout();

            this.Text = "Physican Maintenance";

            //IsDeleted
            IsDeleted.Text = "Deleted";
            IsDeleted.Checked = false;
            IsDeleted.CheckState = CheckState.Unchecked;
            //LastNameLabel
            LastNameLabel.Text = "Last Name";
            LastNameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //FirstNameLabel
            FirstNameLabel.Text = "First Name";
            FirstNameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //MiddleNameLabel
            MiddleNameLabel.Text = "Middle Name";
            MiddleNameLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //Client
            Client.Text = "Client";
            Client.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //ProviderGroupLabel
            ProviderGroupLabel.Text = "Provider Group";
            ProviderGroupLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //AddressLabel
            AddressLabel.Text = "Address";
            AddressLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //CityLabel
            CityLabel.Text = "City";
            CityLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //StateLabel
            StateLabel.Text = "State";
            StateLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //ZipLabel
            ZipLabel.Text = "Zip";
            ZipLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //PhoneLabel
            PhoneLabel.Text = "Phone";
            PhoneLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //LISMenmLabel
            LISMnemLabel.Text = "LIS Mnem";
            LISMnemLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //CredentialsLabel
            CredentialsLabel.Text = "Credentials";
            CredentialsLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //DoctorNoLabel
            DoctorNoLabel.Text = "Doctor No";
            DoctorNoLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //NPILabel
            NPILabel.Text = "NPI";
            NPILabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            //BillingNPILabel
            BillingNPILabel.Text = "Billing NPI";
            BillingNPILabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            //LastNameTextBox
            LastNameTextBox.Dock = DockStyle.Fill;
            LastNameTextBox.MaxLength = 30;
            //FirstNameTextBox
            FirstNameTextBox.Dock = DockStyle.Fill;
            FirstNameTextBox.MaxLength = 30;
            //MiddleNameTextBox
            MiddleNameTextBox.Dock = DockStyle.Fill;
            MiddleNameTextBox.MaxLength = 30;
            //ClientTextBox
            ClientTextBox.Dock = DockStyle.Fill;
            ClientTextBox.MaxLength = 15;
            //ProviderGroupTextBox
            ProviderGroupTextBox.Dock = DockStyle.Fill;
            ProviderGroupTextBox.MaxLength = 50;
            //Address1TextBox
            Address1TextBox.Dock = DockStyle.Fill;
            Address1TextBox.MaxLength = 50;
            //Address2TextBox
            Address2TextBox.Dock = DockStyle.Fill;
            Address2TextBox.MaxLength = 50;
            //CityTextBox
            CityTextBox.Dock = DockStyle.Fill;
            CityTextBox.MaxLength = 30;
            //StateTextBox
            StateTextBox.Dock = DockStyle.Fill;
            StateTextBox.MaxLength = 2;
            //ZipTextBox
            ZipTextBox.Dock = DockStyle.Fill;
            ZipTextBox.MaxLength = 10;
            //PhoneTextBox
            PhoneTextBox.Dock = DockStyle.Fill;
            PhoneTextBox.MaxLength = 40;
            //LISMenmTextBox
            LISMenmTextBox.Dock = DockStyle.Fill;
            LISMenmTextBox.MaxLength = 15;
            //CredentialsTextBox
            CredentialsTextBox.Dock = DockStyle.Fill;
            CredentialsTextBox.MaxLength = 50;
            //DoctorNoTextBox
            DoctorNoTextBox.Dock = DockStyle.Fill;
            DoctorNoTextBox.MaxLength = 5;
            //NPITextBox
            NPITextBox.Dock = DockStyle.Fill;
            NPITextBox.MaxLength = 15;
            //BillingNPITextBox
            BillingNPITextBox.Dock = DockStyle.Fill;


            layoutPanel.ColumnCount = 4;
            layoutPanel.RowCount = 13;
            layoutPanel.Dock = DockStyle.Fill;
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            int col = 0;
            int row = 1;


            layoutPanel.Controls.Add(LastNameLabel, column: col, row: row);
            layoutPanel.Controls.Add(LastNameTextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(FirstNameLabel, column: col, row: row);
            layoutPanel.Controls.Add(FirstNameTextBox, column: col+1, row: row++);
            
            layoutPanel.Controls.Add(MiddleNameLabel, column: col, row: row);
            layoutPanel.Controls.Add(MiddleNameTextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(CredentialsLabel, column: col, row: row);
            layoutPanel.Controls.Add(CredentialsTextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(ProviderGroupLabel, column: col, row: row);
            layoutPanel.Controls.Add(ProviderGroupTextBox, column: col + 1, row: row++);
            
            layoutPanel.Controls.Add(AddressLabel, column: col, row: row);
            layoutPanel.Controls.Add(Address1TextBox, column: col+1, row: row++);
            layoutPanel.Controls.Add(Address2TextBox, column: col+1, row: row++);
            
            layoutPanel.Controls.Add(CityLabel, column: col, row: row);
            layoutPanel.Controls.Add(CityTextBox, column: col + 1, row: row++);
            
            layoutPanel.Controls.Add(StateLabel, column: col, row: row);
            layoutPanel.Controls.Add(StateTextBox, column: col + 1, row: row++);
            
            layoutPanel.Controls.Add(ZipLabel, column: col, row: row);
            layoutPanel.Controls.Add(ZipTextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(PhoneLabel, column: col, row: row);
            layoutPanel.Controls.Add(PhoneTextBox, column: col + 1, row: row++);
           
            layoutPanel.Controls.Add(IsDeleted, column: col + 1, row: row++);


            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            
            col = 2;
            row = 1;

            layoutPanel.Controls.Add(NPILabel, column: col, row: row);
            layoutPanel.Controls.Add(NPITextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(BillingNPILabel, column: col, row: row);
            layoutPanel.Controls.Add(BillingNPITextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(DoctorNoLabel, column: col, row: row);
            layoutPanel.Controls.Add(DoctorNoTextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(LISMnemLabel, column: col, row: row);
            layoutPanel.Controls.Add(LISMenmTextBox, column: col + 1, row: row++);

            layoutPanel.Controls.Add(Client, column: col, row: row);
            layoutPanel.Controls.Add(ClientTextBox, column: col + 1, row: row++);

            this.Controls.Add(layoutPanel);

            this.ResumeLayout();

        }

        private readonly TableLayoutPanel layoutPanel = new TableLayoutPanel();
        private readonly CheckBox IsDeleted = new CheckBox();
        private readonly Label LastNameLabel = new Label();
        private readonly TextBox LastNameTextBox = new TextBox();
        private readonly Label FirstNameLabel = new Label();
        private readonly TextBox FirstNameTextBox = new TextBox();
        private readonly Label MiddleNameLabel = new Label();
        private readonly TextBox MiddleNameTextBox = new TextBox();
        private readonly Label Client = new Label();
        private readonly TextBox ClientTextBox = new TextBox();
        private readonly Label ProviderGroupLabel = new Label();
        private readonly TextBox ProviderGroupTextBox = new TextBox();
        private readonly Label AddressLabel = new Label();
        private readonly TextBox Address1TextBox = new TextBox();
        private readonly TextBox Address2TextBox = new TextBox();
        private readonly Label CityLabel = new Label();
        private readonly TextBox CityTextBox = new TextBox();
        private readonly Label StateLabel = new Label();
        private readonly TextBox StateTextBox = new TextBox();
        private readonly Label ZipLabel = new Label();
        private readonly TextBox ZipTextBox = new TextBox();
        private readonly Label PhoneLabel = new Label();
        private readonly TextBox PhoneTextBox = new TextBox();
        private readonly Label LISMnemLabel = new Label();
        private readonly TextBox LISMenmTextBox = new TextBox();
        private readonly Label CredentialsLabel = new Label();
        private readonly TextBox CredentialsTextBox = new TextBox();
        private readonly Label DoctorNoLabel = new Label();
        private readonly TextBox DoctorNoTextBox = new TextBox();
        private readonly Label NPILabel = new Label();
        private readonly TextBox NPITextBox = new TextBox();
        private readonly Label BillingNPILabel = new Label();
        private readonly TextBox BillingNPITextBox = new TextBox();

        public Phy PhyModel { get; set; } = new Phy();
        private readonly BindingSource bindingSource = new BindingSource();

        private void PhysicianMaintenanceEditForm_Load(object sender, EventArgs e)
        {
            bindingSource.DataSource = PhyModel;

            LastNameTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.LastName));
            FirstNameTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.FirstName));
            MiddleNameTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.MiddleInitial));
            ClientTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.ClientMnem));
            ProviderGroupTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.ProviderGroup));
            Address1TextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.Address1));
            Address2TextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.Address2));
            CityTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.City));
            StateTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.State));
            ZipTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.ZipCode));
            PhoneTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.Phone));
            LISMenmTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.LISMnem));
            CredentialsTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.Credentials));
            DoctorNoTextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.DoctorNumber));
            IsDeleted.DataBindings.Add("Checked", bindingSource, nameof(Phy.IsDeleted), false, DataSourceUpdateMode.OnPropertyChanged);
            NPITextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.NpiId));
            BillingNPITextBox.DataBindings.Add("Text", bindingSource, nameof(Phy.BillingNpi));
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.ActiveControl = LastNameLabel;
            DialogResult = DialogResult.OK;
            return;
        }
    }
}
