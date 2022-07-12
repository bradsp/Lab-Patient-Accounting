using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace LabBilling.MasterGridView
{
    public class MasterGridView : DataGridView
    {
        internal List<int> lstCurrentRows = new List<int>();
        internal int rowDefaultHeight = 22;
        internal int rowExpandedHeight = 150;
        internal int rowDefaultDivider = 0;
        internal int rowExpandedDivider = 150 - 22;
        internal int rowDividerMargin = 3;
        internal bool doCollapseRow;


        ImageList rowHeaderIconList = new ImageList();

        internal DetailTabControl detailTabControl = new DetailTabControl
        {
            Height = 278 /*rowExpandedDivider*/ - 5/*rowDividerMargin*/ * 2,
            Visible = false
        };

        private System.ComponentModel.IContainer components;

        public enum rowHeaderIcons
        {
            expand = 1,
            collapse = 0
        }

        public MasterGridView()
        {
            InitializeComponent();
            
            rowHeaderIconList.Images.Add(MasterGridViewResource.expanded);
            rowHeaderIconList.Images.Add(MasterGridViewResource.collapsed);
            rowHeaderIconList.TransparentColor = System.Drawing.Color.Transparent;
            rowHeaderIconList.Images.SetKeyName(1, "expanded.gif");
            rowHeaderIconList.Images.SetKeyName(0, "collapsed.gif");

            Scroll += MasterControl_Scroll;

            // draw the symbol "+"
            RowPostPaint += MasterControl_RowPostPaint;

            // By clicking on the symbol "+" from row header
            RowHeaderMouseClick += MasterControl_RowHeaderMouseClick;

            // Show the description of the Property in a tooltip when the mouse hovers over it
            CellMouseEnter += MasterGridView_CellMouseEnter;

            // Clear cell when ESC is pressed
            CellEndEdit += MasterGridView_CellEndEdit;

            // Delete records or open details
            KeyDown += MasterGridView_KeyDown;

            cModule.applyGridTheme(this);
        }

        void MasterGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            MasterGridView grid = (MasterGridView)sender;

            // Clear the row error in case the user presses ESC.   
            grid.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        void MasterGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // refresh cells that have not been committed
            if (this.IsCurrentCellDirty)
            {
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        void MasterGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.Equals(Keys.Delete))
            {
                BindingSource bs = (BindingSource)this.DataSource;

                if (bs.Current != null)
                {
                    //TODO pending validation that it is not being used elsewhere
                    if (MessageBox.Show("Are you sure want to delete " + bs.Current.ToString(), "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        bs.RemoveCurrent();
                    }
                }
            }
            else if (e.Control && e.KeyCode.Equals(Keys.Enter))
            {
                // open detail
                OpenDetail(this.CurrentRow.Index);
            }
        }

        void comboOpt2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                ComboBox combo = (ComboBox)this.EditingControl;
                combo.SelectedIndex = -1;
            }
        }

        void comboOpt1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                ComboBox combo = (ComboBox)this.EditingControl;
                combo.SelectedIndex = -1;

            }
        }


        ToolTip tt = new ToolTip();
        void MasterGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            //// Show tooltip of Property Description in Header's Column
            //if (e.RowIndex == -1 && e.ColumnIndex != -1 && this.DataSource != null)
            //{
            //    var tipo = typeof(T);
            //    var property = tipo.GetProperty(this.Columns[e.ColumnIndex].DataPropertyName);
            //    var description = TypeMethods.GetDescriptionFromPropertyInfo(property);

            //    if (string.IsNullOrWhiteSpace(description))
            //    {
            //        tt.Hide(this);
            //    }
            //    else
            //    {
            //        tt.SetToolTip(this, description);
            //    }
            //}
            ////else
            ////{
            ////    tt.Hide(this);
            ////}
        }



        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterGridView));


            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
        }

        ///// <summary>
        ///// Indicates whether the object contains generic lists
        ///// Note: Keep in mind that not all generic lists become child grids.
        ///// </summary>
        ///// <returns></returns>
        //private bool HasDetailList()
        //{
        //    bool hasDetailList = false;
        //    foreach (FieldInfo field in typeof(T).GetFields())
        //    {
        //        if (field.FieldType.IsGenericType 
        //            && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)
        //            )
        //        {
        //            hasDetailList |= true;
        //        }
        //    }

        //    return hasDetailList;
        //}
        /// <summary>
        /// It is necessary to set the child grid at the same hierarchy level as the Mastergrid
        /// </summary>
        public void SetChild()
        {
            if (this.Parent == null)
            {
                throw new Exception("The control should be in a container.");
            }
            this.Parent.Controls.Add(detailTabControl);
            detailTabControl.BringToFront();
        }


        private void MasterControl_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rect = new Rectangle(e.RowBounds.X + ((rowDefaultHeight - 16) / 2), e.RowBounds.Y + ((rowDefaultHeight - 16) / 2), 16, 16);
            
            //if (HasDetailList())
            {
                if (doCollapseRow)
                {
                    if (lstCurrentRows.Contains(e.RowIndex))
                    {
                        this.Rows[e.RowIndex].DividerHeight = this.Rows[e.RowIndex].Height - rowDefaultHeight;

                        e.Graphics.DrawImage(rowHeaderIconList.Images[(int)rowHeaderIcons.collapse], rect);

                        detailTabControl.Location = new Point(e.RowBounds.Left + this.RowHeadersWidth, e.RowBounds.Top + rowDefaultHeight + 5);
                        detailTabControl.Width = e.RowBounds.Right - this.RowHeadersWidth;
                        detailTabControl.Height = this.Rows[e.RowIndex].DividerHeight - 10;
                        detailTabControl.Visible = true;
                    }
                    else
                    {
                        detailTabControl.Visible = false;
                        e.Graphics.DrawImage(rowHeaderIconList.Images[(int)rowHeaderIcons.expand], rect);
                    }
                    doCollapseRow = false;
                }
                else
                {
                    if (lstCurrentRows.Contains(e.RowIndex))
                    {
                        this.Rows[e.RowIndex].DividerHeight = this.Rows[e.RowIndex].Height - rowDefaultHeight;
                        e.Graphics.DrawImage(rowHeaderIconList.Images[(int)rowHeaderIcons.collapse], rect);
                        detailTabControl.Location = new Point(e.RowBounds.Left + this.RowHeadersWidth, e.RowBounds.Top + rowDefaultHeight + 5);
                        detailTabControl.Width = e.RowBounds.Right - this.RowHeadersWidth;
                        detailTabControl.Height = this.Rows[e.RowIndex].DividerHeight - 10;
                        detailTabControl.Visible = true;
                    }
                    else
                    {
                        e.Graphics.DrawImage(rowHeaderIconList.Images[(int)rowHeaderIcons.expand], rect);
                    }
                }
            }

            cModule.rowPostPaint_HeaderCount(sender, e);
        }

        private void MasterControl_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.Rows[e.RowIndex].DataBoundItem == null)
            {
                return;
            }
            Rectangle rect = new Rectangle((rowDefaultHeight - 16) / 2, (rowDefaultHeight - 16) / 2, 16, 16);
            if (rect.Contains(e.Location))
            {
                // if you click on the plus symbol, the detail opens
                //TODO: possibly count number of rows in child and determine detail height

                OpenDetail(e.RowIndex);
            }
            else
            {
                doCollapseRow = false;
            }
        }

        /// <summary>
        /// Open/Close the detail subgrid
        /// </summary>
        /// <param name="rowIndex">Index of the record to edit</param>
        private void OpenDetail(int rowIndex)
        {
            if (lstCurrentRows.Contains(rowIndex))
            {
                lstCurrentRows.Clear();
                this.Rows[rowIndex].Height = rowDefaultHeight;
                this.Rows[rowIndex].DividerHeight = rowDefaultDivider;
            }
            else
            {
                if (lstCurrentRows.Count != 0)
                {
                    int eRow = lstCurrentRows[0];
                    lstCurrentRows.Clear();
                    this.Rows[eRow].Height = rowDefaultHeight;
                    this.Rows[eRow].DividerHeight = rowDefaultDivider;
                    this.ClearSelection();
                    doCollapseRow = true;
                    this.Rows[eRow].Selected = true;
                }

                lstCurrentRows.Add(rowIndex);

                Type parentType = TypeMethods.HeuristicallyDetermineType((IList)this.DataSource);
                object parentObject = this.Rows[rowIndex].DataBoundItem;

                // Detail
                detailTabControl.TabPages.Clear();

                detailTabControl.openDetailEvent += detailTabControl_OpenDetail;

                if (parentObject != null)
                {
                    foreach (FieldInfo childField in parentType.GetFields())
                    {
                        if (childField.FieldType.IsGenericType
                            && childField.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                            && childField.FieldType.GetGenericTypeDefinition() != typeof(List<double>))
                        {
                            IList listOfDetail = (IList)childField.GetValue(parentObject);

                            string name = TypeMethods.GetDescriptionFromFieldInfo(childField);

                            detailTabControl.AddChildgrid(listOfDetail, name);
                        }
                    }
                }

                // expand row
                if (detailTabControl.HasChildren)
                {
                    this.Rows[rowIndex].Height = rowExpandedHeight;
                    this.Rows[rowIndex].DividerHeight = rowExpandedDivider;
                }
                else
                {
                    detailTabControl.Visible = false;
                }
            }
            this.ClearSelection();
            doCollapseRow = true;
            this.Rows[rowIndex].Selected = true;
        }

        void detailTabControl_OpenDetail()
        {
            // The method that opens/closes the detail grid is called
            OpenDetail(this.CurrentRow.Index);
        }

        private void MasterControl_Scroll(object sender, ScrollEventArgs e)
        {
            if (!(lstCurrentRows.Count == 0))
            {
                doCollapseRow = true;
                this.ClearSelection();
                this.Rows[lstCurrentRows[0]].Selected = true;
            }
        }
    }
}
