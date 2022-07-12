using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.MasterGridView
{
    static class cModule
    {
        static DataGridViewCellStyle dateCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleRight
        };

        static DataGridViewCellStyle amountCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleRight
        };

        /// <summary>
        /// grid header
        /// </summary>
        static DataGridViewCellStyle columnHeaderCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(Convert.ToInt32(Convert.ToByte(79)), Convert.ToInt32(Convert.ToByte(129)), Convert.ToInt32(Convert.ToByte(189))),
            Font = new Font("Segoe UI", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0)),
            ForeColor = SystemColors.ControlLightLight,
            SelectionBackColor = SystemColors.Highlight,
            SelectionForeColor = SystemColors.HighlightText,
            WrapMode = DataGridViewTriState.True


        };

        /// <summary>
        /// Default cell style
        /// </summary>
        static DataGridViewCellStyle defaultCellStyle = new DataGridViewCellStyle
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.SystemColors.ControlLightLight,
            Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0)),
            ForeColor = System.Drawing.SystemColors.ControlText,
            SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(155)), Convert.ToInt32(Convert.ToByte(187)), Convert.ToInt32(Convert.ToByte(89))),
            SelectionForeColor = System.Drawing.SystemColors.HighlightText,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False
        };

        /// <summary>
        /// row header style
        /// </summary>
        static DataGridViewCellStyle rowHeaderCellStyle = new DataGridViewCellStyle
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight,
            BackColor = System.Drawing.Color.Lavender,
            Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0)),
            ForeColor = System.Drawing.SystemColors.WindowText,
            SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(155)), Convert.ToInt32(Convert.ToByte(187)), Convert.ToInt32(Convert.ToByte(89))),
            SelectionForeColor = System.Drawing.SystemColors.HighlightText,
            WrapMode = System.Windows.Forms.DataGridViewTriState.True
        };


        public static void applyGridTheme(DataGridView dataGridView)
        {
            dataGridView.AllowUserToAddRows = true;
            dataGridView.AllowUserToDeleteRows = true;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.MultiSelect = true;
            dataGridView.ReadOnly = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView.ShowCellToolTips = false;


            dataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // Format columnheader
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.ColumnHeadersDefaultCellStyle = columnHeaderCellStyle;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dataGridView.ColumnHeadersHeight = 32;

            // Format rowheader
            dataGridView.RowHeadersVisible = true;
            dataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            dataGridView.RowHeadersDefaultCellStyle = rowHeaderCellStyle;
            dataGridView.TopLeftHeaderCell.Value = "Nro ";
            dataGridView.TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView.DefaultCellStyle = defaultCellStyle;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.GridColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridView.Font = columnHeaderCellStyle.Font;
        }

        /// <summary>
        /// Set the style of the columns based on their type
        /// Important: Invoke after filling the columns
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void setGridColumnStyleAfterBinding(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.ValueType == null)
                {
                    //cCol.DefaultCellStyle = dateCellStyle;
                }
                else if (column.ValueType == typeof(DateTime))
                {
                    column.DefaultCellStyle = dateCellStyle;
                }
                else if (column.ValueType == typeof(decimal) || column.ValueType == typeof(double) || column.ValueType == typeof(int))
                {
                    column.DefaultCellStyle = amountCellStyle;
                }
            }
            //dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        /// <summary>
        /// Put a counter in the rowheader (left column)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void rowPostPaint_HeaderCount(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            string rowIdx = (e.RowIndex + 1).ToString();
            dynamic centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            centerFormat.LineAlignment = StringAlignment.Center;
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dataGridView.RowHeadersWidth, e.RowBounds.Height  /* - sender.rows(e.RowIndex).DividerHeight*/  );
            e.Graphics.DrawString(rowIdx, dataGridView.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

    }
}
