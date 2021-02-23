Imports BaiqiSoft.GridControl

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetCellImage
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmdSet = New System.Windows.Forms.Button
        Me.cmdClose = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdRemove = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GrdView1 = New MstGrid
        Me.SuspendLayout()
        '
        'cmdSet
        '
        Me.cmdSet.Location = New System.Drawing.Point(249, 12)
        Me.cmdSet.Name = "cmdSet"
        Me.cmdSet.Size = New System.Drawing.Size(70, 25)
        Me.cmdSet.TabIndex = 1
        Me.cmdSet.Text = "Set"
        Me.cmdSet.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(249, 48)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(70, 25)
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdAdd
        '
        Me.cmdAdd.Location = New System.Drawing.Point(249, 195)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(70, 25)
        Me.cmdAdd.TabIndex = 3
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'cmdRemove
        '
        Me.cmdRemove.Location = New System.Drawing.Point(249, 228)
        Me.cmdRemove.Name = "cmdRemove"
        Me.cmdRemove.Size = New System.Drawing.Size(70, 25)
        Me.cmdRemove.TabIndex = 4
        Me.cmdRemove.Text = "Remove"
        Me.cmdRemove.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'GrdView1
        '
        Me.GrdView1.AllowUserToAddRows = False
        Me.GrdView1.AllowUserToDeleteRows = False
        Me.GrdView1.AutoRedraw = True
        Me.GrdView1.CurrentColIndex = 1
        Me.GrdView1.CurrentRowIndex = 1
        Me.GrdView1.EditSelectedText = Nothing
        Me.GrdView1.EditSelectionLength = 0
        Me.GrdView1.EditSelectionStart = 0
        Me.GrdView1.EditText = Nothing
        Me.GrdView1.FormatProvider = New System.Globalization.CultureInfo("zh-CN")
        Me.GrdView1.LanguageConfig = Nothing
        Me.GrdView1.LeftCol = 1
        Me.GrdView1.LicenseKey = Nothing
        Me.GrdView1.Location = New System.Drawing.Point(12, 12)
        Me.GrdView1.Name = "GrdView1"
        Me.GrdView1.Size = New System.Drawing.Size(225, 199)
        Me.GrdView1.TabIndex = 5
        Me.GrdView1.ThemeCustomColorFrom = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.GrdView1.ThemeCustomColorTo = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(204, Byte), Integer), CType(CType(221, Byte), Integer))
        Me.GrdView1.TopRow = 1
        '
        'frmSetCellImage
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(335, 268)
        Me.Controls.Add(Me.GrdView1)
        Me.Controls.Add(Me.cmdRemove)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSet)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSetCellImage"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set Cell Image"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdSet As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents cmdAdd As System.Windows.Forms.Button
    Friend WithEvents cmdRemove As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GrdView1 As MstGrid
End Class
