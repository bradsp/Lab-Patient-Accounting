using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBilling.Forms
{
    public partial class InterfaceMapping : BaseForm
    {
        public InterfaceMapping()
        {
            InitializeComponent();
        }

        private readonly MappingRepository mappingRepository = new MappingRepository(Program.AppEnvironment);

        private void InterfaceMapping_Load(object sender, EventArgs e)
        {

            CodeSet.DataSource = mappingRepository.GetReturnTypeList();
            SendingSystem.DataSource = mappingRepository.GetSendingSystemList();

        }

        private void LoadData()
        {
            Cursor.Current = Cursors.WaitCursor;


            MappingDGV.DataSource = mappingRepository.GetMappings(CodeSet.Text, SendingSystem.Text);
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
