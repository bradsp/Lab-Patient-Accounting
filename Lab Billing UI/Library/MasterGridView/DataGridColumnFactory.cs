using CSUST.Data;
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace LabBilling.MasterGridView
{
    public partial class DataGridColumnFactory
    {
        public static DataGridViewColumn TextColumnStyle(string propertyname, string headerText, bool pReadonly = false, bool pVisible = true)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.DataPropertyName = propertyname;
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            column.HeaderText = headerText;
            column.Name = propertyname;
            column.ReadOnly = pReadonly;
            if (pReadonly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.Visible = pVisible;
            column.Width = 100;
            return column;
        }

        public static DataGridViewColumn CheckBoxColumnStyle(string pDataPropertyName, string pHeaderText, bool pReadOnly, bool pVisible = true)
        {
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column.DataPropertyName = pDataPropertyName;
            column.HeaderText = pHeaderText;
            column.FalseValue = false;
            column.ReadOnly = pReadOnly;
            if (pReadOnly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.TrueValue = true;
            column.Visible = pVisible;
            return column;
        }

        public static DataGridViewTextBoxColumn DateTimeColumnStyle(string pDataPropertyName, string pHeaderText, bool pReadOnly, bool pVisible = true)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.Format = "dd/MM/yyyy";
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DataPropertyName = pDataPropertyName;
            column.HeaderText = pHeaderText;
            column.Name = pDataPropertyName;
            column.ReadOnly = pReadOnly;
            if (pReadOnly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.Visible = pVisible;
            column.Width = 85;
            return column;
        }

        public static DataGridViewTextBoxColumn TimeColumnStyle(string pDataPropertyName, string pHeaderText, bool pReadOnly, bool pVisible)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.Format = "dd/MM/yyyy HH:mm:ss";
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DataPropertyName = pDataPropertyName;
            column.HeaderText = pHeaderText;
            column.Name = pDataPropertyName;
            column.ReadOnly = pReadOnly;
            if (pReadOnly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.Visible = pVisible;
            column.Width = 85;
            return column;
        }

        public static DataGridViewTextBoxColumn DecimalColumnStyle(string pDataPropertyName, string pHeaderText, int decimaldigits = 4, bool pReadOnly = false, bool pVisible = true)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberDecimalDigits = decimaldigits;
            nfi.NumberGroupSeparator = "";


            TNumEditDataGridViewColumn column = new TNumEditDataGridViewColumn();
            column.AllowNegative = false;
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.Format = "N";
            column.CellTemplate.Style.FormatProvider = nfi;
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DataPropertyName = pDataPropertyName;
            column.DecimalLength = decimaldigits;
            column.HeaderText = pHeaderText;
            column.Name = pDataPropertyName;
            column.ReadOnly = pReadOnly;
            if (pReadOnly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.Visible = pVisible;
            return column;
        }

        public static DataGridViewTextBoxColumn CurrencyColumnStyle(string pDataPropertyName, string pHeaderText, bool pReadOnly, int decimaldigits = 3, bool pVisible = true)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            nfi.CurrencyDecimalDigits = decimaldigits;
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencyGroupSeparator = "";

            TNumEditDataGridViewColumn column = new TNumEditDataGridViewColumn();
            column.AllowNegative = false;
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.Format = "N";
            column.CellTemplate.Style.FormatProvider = nfi;
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DataPropertyName = pDataPropertyName;
            column.DecimalLength = decimaldigits;
            column.HeaderText = pHeaderText;
            column.Name = pDataPropertyName;
            column.ReadOnly = pReadOnly;
            if (pReadOnly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.Visible = pVisible;
            return column;
        }

        public static DataGridViewTextBoxColumn IntegerColumnStyle(string pDataPropertyName, string pHeaderText, bool pReadOnly = false, bool pVisible = true)
        {
            NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            nfi.NumberDecimalDigits = 0;
            nfi.NumberGroupSeparator = "";


            TNumEditDataGridViewColumn column = new TNumEditDataGridViewColumn();
            column.AllowNegative = false;
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.Format = "N";
            column.CellTemplate.Style.FormatProvider = nfi;
            column.CellTemplate.Style.NullValue = string.Empty;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderText = pHeaderText;
            column.DataPropertyName = pDataPropertyName;
            column.Name = pDataPropertyName;
            column.ReadOnly = pReadOnly;
            if (pReadOnly)
            {
                column.DefaultCellStyle.ForeColor = Color.Gray;
            }
            column.Visible = pVisible;
            return column;
        }

        public static DataGridViewComboBoxColumn ComboboxColumnStyle(string pDataPropertyName, string pHeaderText, int pWidth, ArrayList datasource, string DisplayMember, string ValueMember, bool pVisible = true)
        {
            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.CellTemplate.Style.NullValue = null;
            column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DataSource = datasource;
            column.DataPropertyName = pDataPropertyName;
            column.DisplayMember = DisplayMember;
            column.HeaderText = pHeaderText;
            column.Name = pDataPropertyName;
            column.ReadOnly = false;
            column.ValueMember = ValueMember;
            column.Visible = pVisible;
            column.Width = pWidth;
            return column;
        }
    }
}


