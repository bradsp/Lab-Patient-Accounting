<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmColumn
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
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOK = New System.Windows.Forms.Button
        Me.txtColumnFormat = New System.Windows.Forms.TextBox
        Me.txtColumnTitle = New System.Windows.Forms.TextBox
        Me.txtDecimalLength = New System.Windows.Forms.TextBox
        Me.txtMaxLength = New System.Windows.Forms.TextBox
        Me._Label1_11 = New System.Windows.Forms.Label
        Me._Label1_10 = New System.Windows.Forms.Label
        Me._Label1_8 = New System.Windows.Forms.Label
        Me._Label1_7 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(242, 183)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(82, 28)
        Me.cmdCancel.TabIndex = 54
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(154, 183)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(82, 28)
        Me.cmdOK.TabIndex = 53
        Me.cmdOK.Text = "Ok"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'txtColumnFormat
        '
        Me.txtColumnFormat.AcceptsReturn = True
        Me.txtColumnFormat.BackColor = System.Drawing.SystemColors.Window
        Me.txtColumnFormat.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtColumnFormat.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtColumnFormat.Location = New System.Drawing.Point(133, 53)
        Me.txtColumnFormat.MaxLength = 0
        Me.txtColumnFormat.Name = "txtColumnFormat"
        Me.txtColumnFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtColumnFormat.Size = New System.Drawing.Size(191, 21)
        Me.txtColumnFormat.TabIndex = 51
        '
        'txtColumnTitle
        '
        Me.txtColumnTitle.AcceptsReturn = True
        Me.txtColumnTitle.BackColor = System.Drawing.SystemColors.Window
        Me.txtColumnTitle.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtColumnTitle.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtColumnTitle.Location = New System.Drawing.Point(133, 17)
        Me.txtColumnTitle.MaxLength = 0
        Me.txtColumnTitle.Name = "txtColumnTitle"
        Me.txtColumnTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtColumnTitle.Size = New System.Drawing.Size(191, 21)
        Me.txtColumnTitle.TabIndex = 50
        '
        'txtDecimalLength
        '
        Me.txtDecimalLength.AcceptsReturn = True
        Me.txtDecimalLength.BackColor = System.Drawing.SystemColors.Window
        Me.txtDecimalLength.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDecimalLength.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDecimalLength.Location = New System.Drawing.Point(133, 125)
        Me.txtDecimalLength.MaxLength = 0
        Me.txtDecimalLength.Name = "txtDecimalLength"
        Me.txtDecimalLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDecimalLength.Size = New System.Drawing.Size(76, 21)
        Me.txtDecimalLength.TabIndex = 49
        '
        'txtMaxLength
        '
        Me.txtMaxLength.AcceptsReturn = True
        Me.txtMaxLength.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxLength.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxLength.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxLength.Location = New System.Drawing.Point(133, 89)
        Me.txtMaxLength.MaxLength = 0
        Me.txtMaxLength.Name = "txtMaxLength"
        Me.txtMaxLength.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxLength.Size = New System.Drawing.Size(76, 21)
        Me.txtMaxLength.TabIndex = 48
        '
        '_Label1_11
        '
        Me._Label1_11.AutoSize = True
        Me._Label1_11.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_11.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_11.Font = New System.Drawing.Font("宋体", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me._Label1_11.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_11.Location = New System.Drawing.Point(17, 57)
        Me._Label1_11.Name = "_Label1_11"
        Me._Label1_11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_11.Size = New System.Drawing.Size(105, 13)
        Me._Label1_11.TabIndex = 39
        Me._Label1_11.Text = "Column Format:"
        '
        '_Label1_10
        '
        Me._Label1_10.AutoSize = True
        Me._Label1_10.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_10.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_10.Font = New System.Drawing.Font("宋体", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me._Label1_10.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_10.Location = New System.Drawing.Point(17, 21)
        Me._Label1_10.Name = "_Label1_10"
        Me._Label1_10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_10.Size = New System.Drawing.Size(98, 13)
        Me._Label1_10.TabIndex = 38
        Me._Label1_10.Text = "Column Title:"
        '
        '_Label1_8
        '
        Me._Label1_8.AutoSize = True
        Me._Label1_8.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_8.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_8.Font = New System.Drawing.Font("宋体", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me._Label1_8.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_8.Location = New System.Drawing.Point(17, 129)
        Me._Label1_8.Name = "_Label1_8"
        Me._Label1_8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_8.Size = New System.Drawing.Size(112, 13)
        Me._Label1_8.TabIndex = 36
        Me._Label1_8.Text = "Decimal Length:"
        '
        '_Label1_7
        '
        Me._Label1_7.AutoSize = True
        Me._Label1_7.BackColor = System.Drawing.SystemColors.Control
        Me._Label1_7.Cursor = System.Windows.Forms.Cursors.Default
        Me._Label1_7.Font = New System.Drawing.Font("宋体", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me._Label1_7.ForeColor = System.Drawing.SystemColors.ControlText
        Me._Label1_7.Location = New System.Drawing.Point(17, 93)
        Me._Label1_7.Name = "_Label1_7"
        Me._Label1_7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._Label1_7.Size = New System.Drawing.Size(84, 13)
        Me._Label1_7.TabIndex = 35
        Me._Label1_7.Text = "Max Length:"
        '
        'frmColumn
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(343, 228)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.txtColumnFormat)
        Me.Controls.Add(Me.txtColumnTitle)
        Me.Controls.Add(Me.txtDecimalLength)
        Me.Controls.Add(Me.txtMaxLength)
        Me.Controls.Add(Me._Label1_11)
        Me.Controls.Add(Me._Label1_10)
        Me.Controls.Add(Me._Label1_8)
        Me.Controls.Add(Me._Label1_7)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmColumn"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Column Title"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmdCancel As System.Windows.Forms.Button
    Public WithEvents cmdOK As System.Windows.Forms.Button
    Public WithEvents txtColumnFormat As System.Windows.Forms.TextBox
    Public WithEvents txtColumnTitle As System.Windows.Forms.TextBox
    Public WithEvents txtDecimalLength As System.Windows.Forms.TextBox
    Public WithEvents txtMaxLength As System.Windows.Forms.TextBox
    Public WithEvents _Label1_11 As System.Windows.Forms.Label
    Public WithEvents _Label1_10 As System.Windows.Forms.Label
    Public WithEvents _Label1_8 As System.Windows.Forms.Label
    Public WithEvents _Label1_7 As System.Windows.Forms.Label
End Class
