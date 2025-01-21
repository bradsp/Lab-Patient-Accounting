using System;
using System.Windows.Forms;

namespace LabBilling.UserControls
{
    public class WizardPages : System.Windows.Forms.TabControl
    {
        //protected override void WndProc(ref Message m)
        //{
        //    //hide tabs by trapping the TCM_ADJUSTREC message
        //    if (m.Msg == 0x1328 && !DesignMode)
        //        m.Result = (IntPtr)1;
        //    else
        //        base.WndProc(ref m);
        //}

        //protected override void OnKeyDown(KeyEventArgs ke)
        //{
        //    // Block Ctrl+Tab and Ctrl+Shift+Tab hotkeys
        //    if (ke.Control && ke.KeyCode == Keys.Tab)
        //        return;
        //    base.OnKeyDown(ke);
        //}
    }
}
