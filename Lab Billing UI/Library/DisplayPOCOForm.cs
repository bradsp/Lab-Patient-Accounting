using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Core
{
    public sealed class DisplayPOCOForm<C> where C : class
    {
        private C _data;

        public string Title { get; set; }

        public DisplayPOCOForm(C data)
        {
            _data = data;
        }

        public void Show()
        {
            ToolTip toolTip = new ToolTip();

            Type type = _data.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Form frm = new()
            {
                Text = Title
            };
            TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill };
            //tlp.RowCount = columns / 2;
            tlp.ColumnCount = 4;
            int tlpRow = 0;
            int tlpCol = 0;

            foreach (PropertyInfo property in properties)
            {
                Console.WriteLine("Name: " + property.Name + ", Value: " + property.GetValue(_data, null));
                Label lbl = new Label { Text = property.Name };
                lbl.Font = new Font(lbl.Font.Name, lbl.Font.Size, FontStyle.Bold);
                toolTip.SetToolTip(lbl, property.Name);

                Label txt = new Label { Text = property.GetValue(_data, null)?.ToString() };
                txt.Dock = DockStyle.Fill;
                toolTip.SetToolTip(txt, txt.Text);

                tlp.Controls.Add(lbl, tlpCol, tlpRow);
                tlp.Controls.Add(txt, tlpCol + 1, tlpRow);
                
                if (tlpCol == 0)
                {
                    tlpCol = 2;
                }
                else
                {
                    tlpCol = 0;
                    tlpRow++;
                }

            }

            frm.Controls.Add(tlp);
            frm.Size = new Size(600, 700);
            tlp.Dock = DockStyle.Fill;
            tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlp.AutoSize = true;
            frm.ShowDialog();
        }
    }
}
