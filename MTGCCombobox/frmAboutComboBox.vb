Friend Class frmAboutComboBox
    Inherits System.Windows.Forms.Form

#Region " Codice generato da Progettazione Windows Form "

    Public Sub New()
        MyBase.New()

        'Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent()

        'Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent()

    End Sub

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    Friend WithEvents lblMTGC As System.Windows.Forms.LinkLabel
    Friend WithEvents pct As System.Windows.Forms.PictureBox
    Friend WithEvents lblVersione As System.Windows.Forms.Label
    Friend WithEvents lblFramework As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAboutComboBox))
        Me.pct = New System.Windows.Forms.PictureBox
        Me.lblMTGC = New System.Windows.Forms.LinkLabel
        Me.lblVersione = New System.Windows.Forms.Label
        Me.lblFramework = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pct
        '
        Me.pct.Image = CType(resources.GetObject("pct.Image"), System.Drawing.Image)
        Me.pct.Location = New System.Drawing.Point(56, 8)
        Me.pct.Name = "pct"
        Me.pct.Size = New System.Drawing.Size(185, 35)
        Me.pct.TabIndex = 0
        Me.pct.TabStop = False
        '
        'lblMTGC
        '
        Me.lblMTGC.AutoSize = True
        Me.lblMTGC.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.lblMTGC.Location = New System.Drawing.Point(80, 66)
        Me.lblMTGC.Name = "lblMTGC"
        Me.lblMTGC.Size = New System.Drawing.Size(126, 17)
        Me.lblMTGC.TabIndex = 1
        Me.lblMTGC.TabStop = True
        Me.lblMTGC.Text = "http://www.mtgc.net"
        '
        'lblVersione
        '
        Me.lblVersione.AutoSize = True
        Me.lblVersione.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersione.Location = New System.Drawing.Point(8, 56)
        Me.lblVersione.Name = "lblVersione"
        Me.lblVersione.Size = New System.Drawing.Size(215, 17)
        Me.lblVersione.TabIndex = 2
        Me.lblVersione.Text = "MTGCComboBox for .NET, Version 1.0.0.0"
        '
        'lblFramework
        '
        Me.lblFramework.AutoSize = True
        Me.lblFramework.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFramework.Location = New System.Drawing.Point(8, 72)
        Me.lblFramework.Name = "lblFramework"
        Me.lblFramework.Size = New System.Drawing.Size(140, 17)
        Me.lblFramework.TabIndex = 3
        Me.lblFramework.Text = "[.NET Framework, Version]"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(215, Byte), CType(222, Byte), CType(238, Byte))
        Me.Panel1.Controls.Add(Me.pct)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(304, 48)
        Me.Panel1.TabIndex = 4
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lblMTGC)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 88)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(280, 88)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(242, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "MT Global Consulting Srl - Via Roccasparvera, 4"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(189, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "12010 - S.Rocco Castagnaretta  (CN)"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(241, 17)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Tel. +39.0171.491274 - Fax. +39.0171.494516"
        '
        'frmAboutComboBox
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(235, Byte), CType(192, Byte), CType(44, Byte))
        Me.ClientSize = New System.Drawing.Size(298, 184)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.lblFramework)
        Me.Controls.Add(Me.lblVersione)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAboutComboBox"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About: MTGCComboBox"
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub lblMTGC_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblMTGC.LinkClicked
        Process.Start("http://www.mtgc.net")
    End Sub

    Private Sub frmAboutComboBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblVersione.Text = "MTGCComboBox for .NET, Version " & System.Reflection.Assembly.GetExecutingAssembly().GetName.Version.ToString
        lblFramework.Text = "[.NET Framework, Version & " & System.Reflection.Assembly.GetExecutingAssembly().ImageRuntimeVersion.ToString & "]"
    End Sub

End Class
