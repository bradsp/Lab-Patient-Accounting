using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsLibrary
{
    public static class DataGridViewExtensions
    {

        public static void SetVisibilityOrder(this DataGridViewColumn dgvColumn, bool visible, int displayIndex)
        {
            dgvColumn.Visible = visible;
            dgvColumn.DisplayIndex = displayIndex;
        }

    }
}
