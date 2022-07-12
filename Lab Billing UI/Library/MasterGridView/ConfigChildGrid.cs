using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.MasterGridView
{
    public static class ConfigChildGrid
    {
        public static void configColumns(DataGridView grid, Type type)
        {
            DataGridViewColumnCollection columns = grid.Columns;
            columns.Add(DataGridColumnFactory.IntegerColumnStyle("chrg_num", "Chrg ID"));
            columns.Add(DataGridColumnFactory.TextColumnStyle("revcode", "Revenue Code"));
            columns.Add(DataGridColumnFactory.TextColumnStyle("cpt4", "CPT Code"));
            columns.Add(DataGridColumnFactory.TextColumnStyle("modi", "Modifier"));
            columns.Add(DataGridColumnFactory.TextColumnStyle("type", "Type"));
            columns.Add(DataGridColumnFactory.DecimalColumnStyle("amount", "Amount"));
            columns.Add(DataGridColumnFactory.TextColumnStyle("diagnosis_code_ptr", "DX Code Ptr"));
        }
    }
}
