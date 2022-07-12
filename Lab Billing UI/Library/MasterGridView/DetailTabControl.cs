using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.MasterGridView
{
    public class DetailTabControl : TabControl
    {
        public delegate void OpenDetailDelegate();
        public event OpenDetailDelegate openDetailEvent;

        public DetailTabControl()
        {

        }

        void grid_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            if (e.Control && e.KeyCode.Equals(Keys.Enter))
            {
                if (openDetailEvent != null)
                {
                    openDetailEvent();
                }
            }
            else if (e.Control && e.KeyCode.Equals(Keys.Delete))
            {
                BindingSource bs = (BindingSource)grid.DataSource;

                if (bs.Current != null)
                {
                    //TODO 
                    if (MessageBox.Show("Are you sure to delete " + bs.Current.ToString(), "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        bs.RemoveCurrent();
                    }
                }

            }
        }


        internal void AddChildgrid(IList listOfDetail, string name)
        {
            DataGridView grid = new DataGridView();
            // put a counter in the rowheader
            grid.RowPostPaint += cModule.rowPostPaint_HeaderCount;
            // Show the description of the Property in a tooltip when the mouse hovers over it
            grid.CellMouseEnter += newGrid_CellMouseEnter;
            grid.CellEndEdit += grid_CellEndEdit;
            
            grid.KeyDown += grid_KeyDown;

            Type type = TypeMethods.HeuristicallyDetermineType(listOfDetail);
            ConfigChildGrid.configColumns(grid, type);

            cModule.applyGridTheme(grid);

            // Add the date
            BindingSource bs = new BindingSource();
            bs.DataSource = listOfDetail;
            bs.AllowNew = true;
            grid.DataSource = bs;

            cModule.setGridColumnStyleAfterBinding(grid);

            // There are some collections of type double that we do not want to be displayed
            if (TypeMethods.HeuristicallyDetermineType(listOfDetail) != typeof(double))
            {
                TabPage tabpage = new TabPage { Text = name };
                
                tabpage.ToolTipText = TypeMethods.GetDescriptionFromType(type);
                tabpage.Controls.Add(grid);

                this.TabPages.Add(tabpage);
            }
        }

        void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;

            // Clear the row error in case the user presses ESC.   
            grid.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        ToolTip tt = new ToolTip();
        /// <summary>
        /// Show the description of the Property in a tooltip when the mouse hovers over it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView newGrid = (DataGridView)sender;
            BindingSource bs = (BindingSource)newGrid.DataSource;

            // Show the description of the Property in a tooltip when the mouse hovers over it
            if (e.RowIndex == -1 && e.ColumnIndex != -1 && newGrid.DataSource != null)
            {
                Type tipo = TypeMethods.HeuristicallyDetermineType((IEnumerable)bs.DataSource);
                var property = tipo.GetProperty(newGrid.Columns[e.ColumnIndex].DataPropertyName);
                var description = TypeMethods.GetDescriptionFromPropertyInfo(property);

                if (string.IsNullOrWhiteSpace(description))
                {
                    tt.Hide(newGrid);
                }
                else
                {
                    tt.SetToolTip(newGrid, description);
                }

            }
            //else
            //{
            //    tt.Hide(newGrid);
            //}
        }
    }
}
