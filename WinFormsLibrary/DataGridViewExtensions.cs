using System.Reflection;

namespace WinFormsLibrary;

public static class DataGridViewExtensions
{

    public static void SetVisibilityOrder(this DataGridViewColumn dgvColumn, bool visible, int displayIndex)
    {
        dgvColumn.Visible = visible;
        dgvColumn.DisplayIndex = displayIndex;
    }

    //public static void DoubleBuffered(this DataGridView dgv, bool setting)
    //{
    //    var dgvType = dgv.GetType();
    //    var pi = dgvType.GetProperty("DoubleBuffered",
    //          BindingFlags.Instance | BindingFlags.NonPublic);
    //    pi.SetValue(dgv, setting, null);
    //}

}
