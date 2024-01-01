using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RFClassLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FilterListBox : ListBox
    {
        /// <summary>
        /// 
        /// </summary>
        public FilterListBox()
        {
            Visible = false;
            IntegralHeight = true;
            BorderStyle = BorderStyle.FixedSingle;
            TabStop = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override bool ProcessKeyMessage(ref Message m)
        {
            return ProcessKeyEventArgs(ref m);
        }

    }
}
