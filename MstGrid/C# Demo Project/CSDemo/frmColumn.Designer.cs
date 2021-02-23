using System.Drawing;
using System.Windows.Forms;
using System;

public partial class frmColumn : System.Windows.Forms.Form
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
	{
		this.cmdCancel = new System.Windows.Forms.Button();
		base.Load += new System.EventHandler(frmColumn_Load);
		this.cmdCancel.Click += new System.EventHandler(cmdCancel_Click);
		this.cmdOK = new System.Windows.Forms.Button();
		this.cmdOK.Click += new System.EventHandler(cmdOK_Click);
		this.txtColumnFormat = new System.Windows.Forms.TextBox();
		this.txtColumnTitle = new System.Windows.Forms.TextBox();
		this.txtDecimalLength = new System.Windows.Forms.TextBox();
		this.txtMaxLength = new System.Windows.Forms.TextBox();
		this._Label1_11 = new System.Windows.Forms.Label();
		this._Label1_10 = new System.Windows.Forms.Label();
		this._Label1_8 = new System.Windows.Forms.Label();
		this._Label1_7 = new System.Windows.Forms.Label();
		this.SuspendLayout();
		//
		//cmdCancel
		//
		this.cmdCancel.BackColor = System.Drawing.SystemColors.Control;
		this.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default;
		this.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText;
		this.cmdCancel.Location = new System.Drawing.Point(242, 183);
		this.cmdCancel.Name = "cmdCancel";
		this.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.cmdCancel.Size = new System.Drawing.Size(82, 28);
		this.cmdCancel.TabIndex = 54;
		this.cmdCancel.Text = "Cancel";
		this.cmdCancel.UseVisualStyleBackColor = false;
		//
		//cmdOK
		//
		this.cmdOK.BackColor = System.Drawing.SystemColors.Control;
		this.cmdOK.Cursor = System.Windows.Forms.Cursors.Default;
		this.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText;
		this.cmdOK.Location = new System.Drawing.Point(154, 183);
		this.cmdOK.Name = "cmdOK";
		this.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.cmdOK.Size = new System.Drawing.Size(82, 28);
		this.cmdOK.TabIndex = 53;
		this.cmdOK.Text = "Ok";
		this.cmdOK.UseVisualStyleBackColor = false;
		//
		//txtColumnFormat
		//
		this.txtColumnFormat.AcceptsReturn = true;
		this.txtColumnFormat.BackColor = System.Drawing.SystemColors.Window;
		this.txtColumnFormat.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.txtColumnFormat.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtColumnFormat.Location = new System.Drawing.Point(133, 53);
		this.txtColumnFormat.MaxLength = 0;
		this.txtColumnFormat.Name = "txtColumnFormat";
		this.txtColumnFormat.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.txtColumnFormat.Size = new System.Drawing.Size(191, 21);
		this.txtColumnFormat.TabIndex = 51;
		//
		//txtColumnTitle
		//
		this.txtColumnTitle.AcceptsReturn = true;
		this.txtColumnTitle.BackColor = System.Drawing.SystemColors.Window;
		this.txtColumnTitle.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.txtColumnTitle.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtColumnTitle.Location = new System.Drawing.Point(133, 17);
		this.txtColumnTitle.MaxLength = 0;
		this.txtColumnTitle.Name = "txtColumnTitle";
		this.txtColumnTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.txtColumnTitle.Size = new System.Drawing.Size(191, 21);
		this.txtColumnTitle.TabIndex = 50;
		//
		//txtDecimalLength
		//
		this.txtDecimalLength.AcceptsReturn = true;
		this.txtDecimalLength.BackColor = System.Drawing.SystemColors.Window;
		this.txtDecimalLength.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.txtDecimalLength.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtDecimalLength.Location = new System.Drawing.Point(133, 125);
		this.txtDecimalLength.MaxLength = 0;
		this.txtDecimalLength.Name = "txtDecimalLength";
		this.txtDecimalLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.txtDecimalLength.Size = new System.Drawing.Size(76, 21);
		this.txtDecimalLength.TabIndex = 49;
		//
		//txtMaxLength
		//
		this.txtMaxLength.AcceptsReturn = true;
		this.txtMaxLength.BackColor = System.Drawing.SystemColors.Window;
		this.txtMaxLength.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.txtMaxLength.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtMaxLength.Location = new System.Drawing.Point(133, 89);
		this.txtMaxLength.MaxLength = 0;
		this.txtMaxLength.Name = "txtMaxLength";
		this.txtMaxLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.txtMaxLength.Size = new System.Drawing.Size(76, 21);
		this.txtMaxLength.TabIndex = 48;
		//
		//_Label1_11
		//
		this._Label1_11.AutoSize = true;
		this._Label1_11.BackColor = System.Drawing.SystemColors.Control;
		this._Label1_11.Cursor = System.Windows.Forms.Cursors.Default;
		this._Label1_11.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (134)));
		this._Label1_11.ForeColor = System.Drawing.SystemColors.ControlText;
		this._Label1_11.Location = new System.Drawing.Point(17, 57);
		this._Label1_11.Name = "_Label1_11";
		this._Label1_11.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this._Label1_11.Size = new System.Drawing.Size(105, 13);
		this._Label1_11.TabIndex = 39;
		this._Label1_11.Text = "Column Format:";
		//
		//_Label1_10
		//
		this._Label1_10.AutoSize = true;
		this._Label1_10.BackColor = System.Drawing.SystemColors.Control;
		this._Label1_10.Cursor = System.Windows.Forms.Cursors.Default;
		this._Label1_10.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (134)));
		this._Label1_10.ForeColor = System.Drawing.SystemColors.ControlText;
		this._Label1_10.Location = new System.Drawing.Point(17, 21);
		this._Label1_10.Name = "_Label1_10";
		this._Label1_10.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this._Label1_10.Size = new System.Drawing.Size(98, 13);
		this._Label1_10.TabIndex = 38;
		this._Label1_10.Text = "Column Title:";
		//
		//_Label1_8
		//
		this._Label1_8.AutoSize = true;
		this._Label1_8.BackColor = System.Drawing.SystemColors.Control;
		this._Label1_8.Cursor = System.Windows.Forms.Cursors.Default;
		this._Label1_8.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (134)));
		this._Label1_8.ForeColor = System.Drawing.SystemColors.ControlText;
		this._Label1_8.Location = new System.Drawing.Point(17, 129);
		this._Label1_8.Name = "_Label1_8";
		this._Label1_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this._Label1_8.Size = new System.Drawing.Size(112, 13);
		this._Label1_8.TabIndex = 36;
		this._Label1_8.Text = "Decimal Length:";
		//
		//_Label1_7
		//
		this._Label1_7.AutoSize = true;
		this._Label1_7.BackColor = System.Drawing.SystemColors.Control;
		this._Label1_7.Cursor = System.Windows.Forms.Cursors.Default;
		this._Label1_7.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (134)));
		this._Label1_7.ForeColor = System.Drawing.SystemColors.ControlText;
		this._Label1_7.Location = new System.Drawing.Point(17, 93);
		this._Label1_7.Name = "_Label1_7";
		this._Label1_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this._Label1_7.Size = new System.Drawing.Size(84, 13);
		this._Label1_7.TabIndex = 35;
		this._Label1_7.Text = "Max Length:";
		//
		//frmColumn
		//
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.ClientSize = new System.Drawing.Size(343, 228);
		this.Controls.Add(this.cmdCancel);
		this.Controls.Add(this.cmdOK);
		this.Controls.Add(this.txtColumnFormat);
		this.Controls.Add(this.txtColumnTitle);
		this.Controls.Add(this.txtDecimalLength);
		this.Controls.Add(this.txtMaxLength);
		this.Controls.Add(this._Label1_11);
		this.Controls.Add(this._Label1_10);
		this.Controls.Add(this._Label1_8);
		this.Controls.Add(this._Label1_7);
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "frmColumn";
		this.ShowInTaskbar = false;
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Column Title";
		this.ResumeLayout(false);
		this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.Button cmdCancel;
	public System.Windows.Forms.Button cmdOK;
	public System.Windows.Forms.TextBox txtColumnFormat;
	public System.Windows.Forms.TextBox txtColumnTitle;
	public System.Windows.Forms.TextBox txtDecimalLength;
	public System.Windows.Forms.TextBox txtMaxLength;
	public System.Windows.Forms.Label _Label1_11;
	public System.Windows.Forms.Label _Label1_10;
	public System.Windows.Forms.Label _Label1_8;
	public System.Windows.Forms.Label _Label1_7;
}

