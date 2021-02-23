Imports BaiqiSoft.GridControl


Public Class frmSetCellImage

    Private Sub frmSetCellImage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        GrdView1.AutoRedraw = False

        GrdView1.Left = 12
        GrdView1.Top = 12
        GrdView1.Width = 220
        GrdView1.Height = 238

        GrdView1.RowCount = 1
        GrdView1.ColCount = 2
        GrdView1.FixedCols = 0
        GrdView1.AllowUserResizing = ResizeMode.None
        GrdView1.Editable = False
        GrdView1.ScrollBars = ScrollBars.Vertical
        GrdView1.SelectionMode = SelectionMode.ByRow
        GrdView1.FocusRect = FocusRectMode.ByRow
        GrdView1.AllowSelection = False
        GrdView1.HighlightHeaders = False

        GrdView1.Cell(0, 0).Text = "Index"
        GrdView1.Cell(0, 1).Text = "Image"
        GrdView1.Column(0).FixedAlignment = TextAlignment.CenterCenter
        GrdView1.Column(0).Alignment = TextAlignment.CenterCenter
        GrdView1.Column(1).FixedAlignment = TextAlignment.CenterCenter
        GrdView1.Column(1).PictureAlignment = PictureAlignment.CenterCenter

        GrdView1.ImageList = frmDemo.ImageList1
        ShowImage()

        GrdView1.AutoRedraw = True

    End Sub


    Private Sub ShowImage()

        Dim i As Integer

        GrdView1.RowCount = 1
        For i = 0 To frmDemo.ImageList1.Images.Count - 1
            GrdView1.AddItem(i)
            GrdView1.Cell(i + 1, 1).ImageIndex = i
        Next i

    End Sub


    Private Sub cmdSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSet.Click

        If frmDemo.GrdView1.CurrentRowIndex > -1 And frmDemo.GrdView1.CurrentColIndex > -1 Then
            frmDemo.GrdView1.ActiveCell.ImageIndex = GrdView1.CurrentRowIndex - 1
        End If

        Me.Close()

    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click

        Me.Close()

    End Sub


    Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click

        If GrdView1.CurrentRowIndex > 0 And GrdView1.CurrentRowIndex <= frmDemo.ImageList1.Images.Count Then
            GrdView1.AutoRedraw = False
            frmDemo.ImageList1.Images.RemoveAt(GrdView1.CurrentRowIndex - 1)
            ShowImage()
            GrdView1.AutoRedraw = True
        End If

    End Sub


    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "Icon (*.ico)|*.ico|Cursor (*.cur)|*.cur|Bitmap (*.bmp)|*.bmp|JPEG File (*.jpg)|*.jpg|GIF File (*.gif)|*.gif|All Files (*.*)|*.*"
        OpenFileDialog1.ShowDialog()
        If Len(OpenFileDialog1.FileName) > 0 Then
            GrdView1.AutoRedraw = False
            frmDemo.ImageList1.Images.Add(Image.FromFile(OpenFileDialog1.FileName))
            ShowImage()
            GrdView1.CurrentRowIndex = GrdView1.RowCount - 1
            GrdView1.Cell(GrdView1.CurrentRowIndex, 0).EnsureVisible()
            GrdView1.AutoRedraw = True
        End If

    End Sub


End Class