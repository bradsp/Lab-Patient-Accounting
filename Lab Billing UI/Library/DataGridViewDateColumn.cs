using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.XEvent;
using RFClassLibrary;

namespace LabBilling.Library
{
    public class DataGridViewDateCell : DataGridViewTextBoxCell
    {
        // member variable used to keep datetime value
        private DateTime? dateValue;

        public DataGridViewDateCell()
        {
            dateValue = DateTime.MinValue;
        }

        protected override void OnClick(DataGridViewCellEventArgs e)
        {

            base.OnClick(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e, int rowIndex)
        {
            if(this == null || this.Value == null)
            {
                return;
            }
            // allows only numbers, dashes, slashes, or date computation (T, T-1, T+1, etc);
            // allows only numbers, decimals and control characters
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '/' && e.KeyChar != '-' && e.KeyChar != 'T' && e.KeyChar != 't')
            {
                e.Handled = true;
            }

            if (e.KeyChar == 'T' && this.Value.ToString().Contains("T"))
            {
                e.Handled = true;
            }

            if (e.KeyChar == 't' && this.Value.ToString().Contains("t"))
            {
                e.Handled = true;
            }

            base.OnKeyPress(e, rowIndex);
        }

        
        /// <summary>
        /// Update display to show date as ShortDateString
        /// whenver it is validated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                DateTime dTmp = DateTime.MinValue;
                if (dTmp.IsExpression(this.Value.ToString()))
                {
                    dTmp = dTmp.ParseExpression(this.Value.ToString());
                }
                else
                {
                    // format the value as datetime
                    dTmp = dTmp.ValidateDate(this.Value.ToString());
                }
                if (!string.IsNullOrEmpty(this.Value.ToString()))
                    this.Value = dTmp.ToShortDateString();

                if (dTmp == DateTime.MinValue)
                    dateValue = null;
                else
                    dateValue = dTmp;
            }
            catch { }
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            try
            {
                if (this.Value != null)
                {
                    DateTime dTmp = DateTime.MinValue;
                    if (dTmp.IsExpression(this.Value.ToString()))
                    {
                        dTmp = dTmp.ParseExpression(this.Value.ToString());
                    }
                    else
                    {
                        // format the value as datetime
                        dTmp = dTmp.ValidateDate(this.Value.ToString());
                    }
                    if (!string.IsNullOrEmpty(this.Value.ToString()))
                        this.Value = dTmp.ToShortDateString();

                    if (dTmp == DateTime.MinValue)
                        dateValue = null;
                    else
                        dateValue = dTmp;
                }
            }
            catch { }

            base.OnLeave(rowIndex, throughMouseClick);
        }

        /// <summary>
        /// property to maintain value of control
        /// </summary>
        public DateTime? DateValue
        {
            get
            {
                return dateValue;
            }
            set
            {
                dateValue = value;
            }
        }

    }

    public class DataGridViewDateColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewDateColumn()
        {
            this.CellTemplate = new DataGridViewDateCell();
        }
    }
}
