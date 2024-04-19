namespace WinFormsLibrary;

public static class DataGridViewExtensions
{

    public static void SetVisibilityOrder(this DataGridViewColumn dgvColumn, bool visible, int displayIndex)
    {
        dgvColumn.Visible = visible;
        dgvColumn.DisplayIndex = displayIndex;
    }

}
