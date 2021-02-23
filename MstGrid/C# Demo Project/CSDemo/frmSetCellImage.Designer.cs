using System.Drawing;
using System.Windows.Forms;
using System;
using BaiqiSoft.GridControl;

public partial class frmSetCellImage : System.Windows.Forms.Form
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
		this.cmdSet = new System.Windows.Forms.Button();
		base.Load += new System.EventHandler(frmSetCellImage_Load);
		this.cmdSet.Click += new System.EventHandler(cmdSet_Click);
		this.cmdClose = new System.Windows.Forms.Button();
		this.cmdClose.Click += new System.EventHandler(cmdClose_Click);
		this.cmdAdd = new System.Windows.Forms.Button();
		this.cmdAdd.Click += new System.EventHandler(cmdAdd_Click);
		this.cmdRemove = new System.Windows.Forms.Button();
		this.cmdRemove.Click += new System.EventHandler(cmdRemove_Click);
		this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		this.GrdView1 = new MstGrid ();
		this.SuspendLayout();
		//
		//cmdSet
		//
		this.cmdSet.Location = new System.Drawing.Point(249, 12);
		this.cmdSet.Name = "cmdSet";
		this.cmdSet.Size = new System.Drawing.Size(70, 25);
		this.cmdSet.TabIndex = 1;
		this.cmdSet.Text = "Set";
		this.cmdSet.UseVisualStyleBackColor = true;
		//
		//cmdClose
		//
		this.cmdClose.Location = new System.Drawing.Point(249, 48);
		this.cmdClose.Name = "cmdClose";
		this.cmdClose.Size = new System.Drawing.Size(70, 25);
		this.cmdClose.TabIndex = 2;
		this.cmdClose.Text = "Close";
		this.cmdClose.UseVisualStyleBackColor = true;
		//
		//cmdAdd
		//
		this.cmdAdd.Location = new System.Drawing.Point(249, 195);
		this.cmdAdd.Name = "cmdAdd";
		this.cmdAdd.Size = new System.Drawing.Size(70, 25);
		this.cmdAdd.TabIndex = 3;
		this.cmdAdd.Text = "Add";
		this.cmdAdd.UseVisualStyleBackColor = true;
		//
		//cmdRemove
		//
		this.cmdRemove.Location = new System.Drawing.Point(249, 228);
		this.cmdRemove.Name = "cmdRemove";
		this.cmdRemove.Size = new System.Drawing.Size(70, 25);
		this.cmdRemove.TabIndex = 4;
		this.cmdRemove.Text = "Remove";
		this.cmdRemove.UseVisualStyleBackColor = true;
		//
		//OpenFileDialog1
		//
		this.OpenFileDialog1.FileName = "OpenFileDialog1";
		//
		//GrdView1
		//
		this.GrdView1.AllowUserToAddRows = false;
		this.GrdView1.AllowUserToDeleteRows = false;
		this.GrdView1.AutoRedraw = true;
		this.GrdView1.CurrentColIndex = 1;
		this.GrdView1.CurrentRowIndex = 1;
		this.GrdView1.EditSelectedText = null;
		this.GrdView1.EditSelectionLength = 0;
		this.GrdView1.EditSelectionStart = 0;
		this.GrdView1.EditText = null;
		this.GrdView1.FormatProvider = new System.Globalization.CultureInfo("zh-CN");
		this.GrdView1.LanguageConfig = null;
		this.GrdView1.LeftCol = 1;
		this.GrdView1.LicenseKey = null;
		this.GrdView1.Location = new System.Drawing.Point(12, 12);
		this.GrdView1.Name = "GrdView1";
		this.GrdView1.Size = new System.Drawing.Size(225, 199);
		this.GrdView1.TabIndex = 5;
		this.GrdView1.ThemeCustomColorFrom = System.Drawing.Color.FromArgb(System.Convert.ToInt32((byte) (244)), System.Convert.ToInt32((byte) (249)), System.Convert.ToInt32((byte) (251)));
		this.GrdView1.ThemeCustomColorTo = System.Drawing.Color.FromArgb(System.Convert.ToInt32((byte) (191)), System.Convert.ToInt32((byte) (204)), System.Convert.ToInt32((byte) (221)));
		this.GrdView1.TopRow = 1;
		//
		//frmSetCellImage
		//
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		this.ClientSize = new System.Drawing.Size(335, 268);
		this.Controls.Add(this.GrdView1);
		this.Controls.Add(this.cmdRemove);
		this.Controls.Add(this.cmdAdd);
		this.Controls.Add(this.cmdClose);
		this.Controls.Add(this.cmdSet);
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "frmSetCellImage";
		this.ShowInTaskbar = false;
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Set Cell Image";
		this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.Button cmdSet;
	internal System.Windows.Forms.Button cmdClose;
	internal System.Windows.Forms.Button cmdAdd;
	internal System.Windows.Forms.Button cmdRemove;
	internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
	internal MstGrid  GrdView1;
}

