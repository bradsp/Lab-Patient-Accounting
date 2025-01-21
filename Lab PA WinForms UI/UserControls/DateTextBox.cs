using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace LabBilling.UserControls
{
    public partial class DateTextBox : TextBox
    {
        public DateTextBox()
        {
            InitializeComponent();
            dateValue = DateTime.MinValue;
        }

        // member variable used to keep datetime value
        private DateTime? dateValue;

        // default OnPaint
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Calling the base class OnPaint
            base.OnPaint(pe);
        }

        /// <summary>
        /// Keypress handler used to restrict user input
        /// to numbers and control characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows only numbers, dashes, slashes, or date computation (T, T-1, T+1, etc);
            // allows only numbers, decimals and control characters
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '/' && e.KeyChar != '-' && e.KeyChar != 'T' && e.KeyChar != 't')
            {
                e.Handled = true;
            }

            if (e.KeyChar == 'T' && this.Text.Contains("T"))
            {
                e.Handled = true;
            }

            if (e.KeyChar == 't' && this.Text.Contains("t"))
            {
                e.Handled = true;
            }

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
                if (dTmp.IsExpression(this.Text))
                {
                    dTmp = dTmp.ParseExpression(this.Text);
                }
                else
                {
                    // format the value as datetime
                    dTmp = dTmp.ValidateDate(this.Text);
                }
                if (!string.IsNullOrEmpty(this.Text))
                    this.Text = dTmp.ToShortDateString();

                if (dTmp == DateTime.MinValue)
                    dateValue = null;
                else
                    dateValue = dTmp;
            }
            catch { }
        }

        /// <summary>
        /// Revert back to the display of numbers only
        /// whenever the user clicks in the box for
        /// editing purposes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTextBox_Click(object sender, EventArgs e)
        {
            //this.Text = dateValue.ToString();

            //if (this.Text == "0")
              //  this.Clear();

            this.SelectionStart = this.Text.Length;
        }

        /// <summary>
        /// Update the date value each time the value is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime dTmp = DateTime.MinValue;
                if(dTmp.IsExpression(this.Text))
                {
                    dTmp = dTmp.ParseExpression(this.Text);
                }
                else
                {
                    // format the value as datetime
                    dTmp = dTmp.ValidateDate(this.Text);
                }
                if(!string.IsNullOrEmpty(this.Text))
                    this.Text = dTmp.ToShortDateString();

                if (dTmp == DateTime.MinValue)
                    dateValue = null;
                else
                    dateValue = dTmp;
            }
            catch { }
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
}
