using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using System.Windows.Forms;

namespace Utilities
{
    /// <summary>
    /// wdk 07/23/2007
    /// </summary>
    public class ToolStripDateTimePicker : ToolStripControlHost 
    {
        /// <summary>
        /// wdk 07/23/2007
        /// </summary>
         public ToolStripDateTimePicker() : base(new DateTimePicker()) 
         { 
         }
    }
}
