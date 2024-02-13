using System;
using System.Windows.Forms;
using LabBilling.Core.Services;

namespace LabBilling.Forms
{
    public partial class InterfaceMapping : BaseForm
    {
        private readonly DictionaryService dictionaryService = new(Program.AppEnvironment);
        private readonly HL7ProcessorService hL7ProcessorService = new(Program.AppEnvironment);

        public InterfaceMapping()
        {
            InitializeComponent();
        }

        private void InterfaceMapping_Load(object sender, EventArgs e)
        {
            CodeSet.DataSource = dictionaryService.GetMappingsReturnTypeList();
            SendingSystem.DataSource = dictionaryService.GetMappingsSendingSystemList();
        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;


            MappingDGV.DataSource = dictionaryService.GetMappings(CodeSet.Text, SendingSystem.Text);
            MappingDGV.Columns["mod_date"].Visible = false;
            MappingDGV.Columns["mod_host"].Visible = false;
            MappingDGV.Columns["mod_prg"].Visible = false;
            MappingDGV.Columns["mod_user"].Visible = false;
            MappingDGV.Columns["rowguid"].Visible = false;
            MappingDGV.AutoResizeColumns();

            Cursor.Current = Cursors.Default;
        }


        private void LoadGrid_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
