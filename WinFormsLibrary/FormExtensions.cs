using System.Reflection;

namespace WinFormsLibrary;

public static class FormExtensions
{
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        Type dgvType = dgv.GetType();
        PropertyInfo? pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        pi?.SetValue(dgv, setting, null);
    }

    public static List<T> GetAllControls<T>(this Control container) where T : Control
    {
        List<T> controls = new();
        if (container.Controls.Count > 0)
        {
            controls.AddRange(container.Controls.OfType<T>());
            foreach (Control c in container.Controls)
            {
                controls.AddRange(c.GetAllControls<T>());
            }
        }

        return controls;
    }
}
