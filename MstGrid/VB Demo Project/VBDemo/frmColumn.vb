
Imports System.Drawing

Friend Class frmColumn


    Private Sub frmColumn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Get property value
        If frmDemo.GrdView1.FixedRows > 0 Then
            txtColumnTitle.Text = frmDemo.GrdView1.Cell(0, frmDemo.GrdView1.CurrentColIndex).Text
            txtColumnTitle.Enabled = True
            txtColumnTitle.BackColor = ColorTranslator.FromOle(&HFFFFFF)
        Else
            txtColumnTitle.Text = ""
            txtColumnTitle.Enabled = False
            txtColumnTitle.BackColor = ColorTranslator.FromOle(&H8000000F)
        End If
        txtColumnFormat.Text = frmDemo.GrdView1.Column(frmDemo.GrdView1.CurrentColIndex).FormatString
        txtMaxLength.Text = CStr(frmDemo.GrdView1.Column(frmDemo.GrdView1.CurrentColIndex).MaxLength)
        txtDecimalLength.Text = CStr(frmDemo.GrdView1.Column(frmDemo.GrdView1.CurrentColIndex).DecimalLength)

    End Sub


    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        frmDemo.GrdView1.AutoRedraw = False
        If txtColumnTitle.Enabled Then
            frmDemo.GrdView1.Cell(0, frmDemo.GrdView1.CurrentColIndex).Text = txtColumnTitle.Text
        End If
        frmDemo.GrdView1.Column(frmDemo.GrdView1.CurrentColIndex).FormatString = txtColumnFormat.Text
        frmDemo.GrdView1.Column(frmDemo.GrdView1.CurrentColIndex).MaxLength = Val(txtMaxLength.Text)
        frmDemo.GrdView1.Column(frmDemo.GrdView1.CurrentColIndex).DecimalLength = Val(txtDecimalLength.Text)
        frmDemo.GrdView1.AutoRedraw = True
        Me.Close()

    End Sub


    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

        Me.Close()

    End Sub


End Class