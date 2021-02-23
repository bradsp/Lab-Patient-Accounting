using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MonthlyReports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmReports(args));
            }
            catch (Exception ex)
            {
                TextBox tb = new TextBox();
                tb.Dock = DockStyle.Fill;
                tb.Text = ex.Message;
                tb.ReadOnly = true;
                Form f = new Form();
                f.WindowState = FormWindowState.Maximized;
                f.Text = "EXCEPTION HANDLER. DO NOT CLOSE!!! -- Show this to David or Bradley";
                f.Controls.Add(tb);
                f.ShowDialog();
            }
        }
    }
}
