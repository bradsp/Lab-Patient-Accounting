Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Globalization
Imports System.Reflection


<Designer(GetType(MTGCComboBox.MyTypeDesigner))> _
    Public Class MTGCComboBox
    Inherits System.Windows.Forms.ComboBox

#Region " **************** Public ReDeclararion ****************"
    Public Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hWnd As Integer, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Public Declare Function GetWindow Lib "user32" (ByVal hwnd As Integer, ByVal wCmd As Integer) As Integer
#End Region

#Region " **************** Enumerations and class variables ****************"
    Dim PressedKey As Boolean = False

    Dim wcol, wcol1, wcol2, wcol3, wcol4 As String  'Column widths
    Dim currentColor As Color                       'Current border color

    Dim WithEvents myTimer As New Timer
    Dim arrowWidth As Integer = 12
    Dim bUsingLargeFont As Boolean = False
    Dim customBorderColor As Color = Color.Empty
    Dim enabledBackColor As Color
    Dim enabledForeColor As Color
    Dim borderColor As Color = SystemColors.Highlight
    Dim dropDownArrowAreaNormalColor As Color = SystemColors.Control
    Dim dropDownArrowAreaHotColor As Color = Color.Black
    Dim dropDownArrowAreaPressedColor As Color = Color.Black
    Dim Highlighted As Boolean = True
    Dim Indice(4) As Integer
    Dim MouseOver As Boolean = False
    Private m_RaiseSelectedIndexChanged As Boolean = True

    'constants for CharacterCasing
    Const CBS_UPPERCASE As Integer = &H2000
    Const CBS_LOWERCASE As Integer = &H4000

    'Border Type
    Enum TipiBordi
        [Fixed3D]
        [FlatXP]
    End Enum

    'Loading Type
    Enum CaricamentoCombo
        [ComboBoxItem]
        [DataTable]
    End Enum

    'Our DropDownStyle to manage the DropDownList with Autocomplete
    Enum CustomDropDownStyle
        [DropDown]
        [DropDownList]
    End Enum

    'Property Declaration
    Private WithEvents mComboBox As System.Windows.Forms.ComboBox
    Dim m_ColumnNum As Integer = 1
    Dim m_ColumnWidth As String = Me.Width
    Dim m_ArrowColor As Color = Color.Black
    Dim m_ArrowBoxColor As Color = SystemColors.Control
    Dim m_DisabledArrowBoxColor As Color = SystemColors.Control
    Dim m_DisabledArrowColor As Color = Color.LightGray
    Dim m_NormalBorderColor As Color = Color.Black
    Dim m_DisabledBackColor As Color = SystemColors.Control
    Dim m_DisabledForeColor As Color = SystemColors.GrayText
    Dim m_DisabledBorderColor As Color = SystemColors.InactiveBorder
    Dim m_DropDownForeColor As Color = Color.Black
    Dim m_DropDownBackColor As Color = Color.FromArgb(193, 210, 238)
    Dim m_DropDownArrowBackColor As Color = Color.FromArgb(136, 169, 223)
    Dim m_GridLineVertical As Boolean = False
    Dim m_GridLineHorizontal As Boolean = False
    Dim m_GridLineColor As Color = Color.LightGray
    Dim m_CharacterCasing As CharacterCasing = CharacterCasing.Normal
    Dim m_BorderStyle As TipiBordi = TipiBordi.FlatXP 'TipiBordi.Fixed3D
    Dim m_HighlightBorderColor As Color = Color.Blue
    Dim m_HighlightBorderOnMouseEvents As Boolean = True
    Dim m_DataTable As DataTable
    Dim m_SourceDataString As String()
    Dim m_LoadingType As CaricamentoCombo = CaricamentoCombo.ComboBoxItem
    Dim m_ManagingFastMouseMoving As Boolean = True
    Dim m_ManagingFastMouseMovingInterval As Integer = 30
    Dim m_DropDownStyle As CustomDropDownStyle = CustomDropDownStyle.DropDown
    Dim m_SelectedItem As MTGCComboBoxItem
#End Region

#Region " **************** Custom Events ****************"
    Public Shadows Event DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs)
#End Region

#Region " **************** Constructor and Dispose ****************"
    'Constructor
    Public Sub New()
        MyBase.New()
        mComboBox = Me
        AddHandler myTimer.Tick, AddressOf TimerEventProcessor
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If myTimer.Enabled Then myTimer.Stop()
        If disposing Then MyBase.Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region " **************** Designer Custom ****************"
    Friend Class MyTypeDesigner
        Inherits System.Windows.Forms.Design.ControlDesigner

        Private verbi As DesignerVerbCollection

        Private Sub OnVerbFlatXP(ByVal sender As Object, ByVal e As EventArgs)
            For Each verbo As DesignerVerb In Verbs
                verbo.Checked = False
            Next
            CType(sender, DesignerVerb).Checked = True
            Dim tipoBordo As PropertyDescriptor
            tipoBordo = TypeDescriptor.GetProperties(Control)("BorderStyle")
            tipoBordo.SetValue(Control, TipiBordi.FlatXP)
        End Sub

        Private Sub OnVerbFixed3D(ByVal sender As Object, ByVal e As EventArgs)
            For Each verbo As DesignerVerb In Verbs
                verbo.Checked = False
            Next
            CType(sender, DesignerVerb).Checked = True
            Dim tipoBordo As PropertyDescriptor
            tipoBordo = TypeDescriptor.GetProperties(Control)("BorderStyle")
            tipoBordo.SetValue(Control, TipiBordi.Fixed3D)
        End Sub

        Private Sub OnVerbInformazioniSu(ByVal sender As Object, ByVal e As EventArgs)
            Dim frmAbout As New frmAboutComboBox

            frmAbout.ShowDialog()
        End Sub

        Public Overloads Overrides ReadOnly Property Verbs() As DesignerVerbCollection
            Get
                If verbi Is Nothing Then
                    verbi = New DesignerVerbCollection
                    verbi.Add(New DesignerVerb("FlatXP", New EventHandler(AddressOf OnVerbFlatXP)))
                    verbi.Add(New DesignerVerb("Fixed3D", New EventHandler(AddressOf OnVerbFixed3D)))
                    verbi.Add(New DesignerVerb("About MTGCComboBox", New EventHandler(AddressOf OnVerbInformazioniSu)))

                    Dim tipoBordo As PropertyDescriptor
                    tipoBordo = TypeDescriptor.GetProperties(Control)("BorderStyle")
                    Dim mtipoBordo As TipiBordi

                    mtipoBordo = CType(tipoBordo.GetValue(Me.Component), TipiBordi)
                    If mtipoBordo = TipiBordi.FlatXP Then
                        verbi(0).Checked = True
                    ElseIf mtipoBordo = TipiBordi.Fixed3D Then
                        verbi(1).Checked = True
                    End If

                End If
                Return verbi
            End Get
        End Property

        Protected Overrides Sub PreFilterProperties(ByVal properties As IDictionary)
            MyBase.PreFilterProperties(properties)

            Dim attributoVero() As System.Attribute = {New ReadOnlyAttribute(True)}
            Dim attributoFalso() As System.Attribute = {New ReadOnlyAttribute(False)}

            Dim HighlightBorderOnMouseEvents As PropertyDescriptor
            HighlightBorderOnMouseEvents = properties("HighlightBorderOnMouseEvents")
            Dim HighlightBorderColor As PropertyDescriptor
            HighlightBorderColor = properties("HighlightBorderColor")
            Dim NormalBorderColor As PropertyDescriptor
            NormalBorderColor = properties("NormalBorderColor")
            Dim DropDownArrowBackColor As PropertyDescriptor
            DropDownArrowBackColor = properties("DropDownArrowBackColor")
            Dim tipoBordo As PropertyDescriptor
            tipoBordo = properties("BorderStyle")
            Dim mtipoBordo As TipiBordi

            mtipoBordo = CType(tipoBordo.GetValue(Me.Component), TipiBordi)
            Select Case mtipoBordo
                Case TipiBordi.FlatXP
                    properties("HighlightBorderOnMouseEvents") = TypeDescriptor.CreateProperty(HighlightBorderOnMouseEvents.ComponentType, HighlightBorderOnMouseEvents, attributoFalso)
                    properties("HighlightBorderColor") = TypeDescriptor.CreateProperty(HighlightBorderColor.ComponentType, HighlightBorderColor, attributoFalso)
                    properties("NormalBorderColor") = TypeDescriptor.CreateProperty(NormalBorderColor.ComponentType, NormalBorderColor, attributoFalso)
                    properties("DropDownArrowBackColor") = TypeDescriptor.CreateProperty(DropDownArrowBackColor.ComponentType, DropDownArrowBackColor, attributoFalso)
                Case TipiBordi.Fixed3D
                    properties("HighlightBorderOnMouseEvents") = TypeDescriptor.CreateProperty(HighlightBorderOnMouseEvents.ComponentType, HighlightBorderOnMouseEvents, attributoVero)
                    properties("HighlightBorderColor") = TypeDescriptor.CreateProperty(HighlightBorderColor.ComponentType, HighlightBorderColor, attributoVero)
                    properties("NormalBorderColor") = TypeDescriptor.CreateProperty(NormalBorderColor.ComponentType, NormalBorderColor, attributoVero)
                    properties("DropDownArrowBackColor") = TypeDescriptor.CreateProperty(DropDownArrowBackColor.ComponentType, DropDownArrowBackColor, attributoVero)
            End Select

            Dim sourceDataTable As PropertyDescriptor
            sourceDataTable = properties("SourceDataTable")
            Dim sourceDataString As PropertyDescriptor
            sourceDataString = properties("SourceDataString")
            Dim tipoCaricamento As PropertyDescriptor
            tipoCaricamento = properties("LoadingType")
            Dim mtipoCaricamento As CaricamentoCombo

            mtipoCaricamento = CType(tipoCaricamento.GetValue(Me.Component), CaricamentoCombo)
            Select Case mtipoCaricamento
                Case CaricamentoCombo.ComboBoxItem
                    properties("SourceDataTable") = TypeDescriptor.CreateProperty(sourceDataTable.ComponentType, sourceDataTable, attributoVero)
                    properties("SourceDataString") = TypeDescriptor.CreateProperty(sourceDataString.ComponentType, sourceDataString, attributoVero)
                Case CaricamentoCombo.DataTable
                    properties("SourceDataTable") = TypeDescriptor.CreateProperty(sourceDataTable.ComponentType, sourceDataTable, attributoFalso)
                    properties("SourceDataString") = TypeDescriptor.CreateProperty(sourceDataString.ComponentType, sourceDataString, attributoFalso)
            End Select

            Dim ManagingFastMouseMovingInterval As PropertyDescriptor
            ManagingFastMouseMovingInterval = properties("ManagingFastMouseMovingInterval")
            Dim mManagingFastMouseMoving As Boolean
            Dim ManagingFastMouseMoving As PropertyDescriptor
            ManagingFastMouseMoving = properties("ManagingFastMouseMoving")

            mManagingFastMouseMoving = CType(ManagingFastMouseMoving.GetValue(Me.Component), Boolean)
            If mManagingFastMouseMoving Then
                properties("ManagingFastMouseMovingInterval") = TypeDescriptor.CreateProperty(ManagingFastMouseMovingInterval.ComponentType, ManagingFastMouseMovingInterval, attributoFalso)
            Else
                properties("ManagingFastMouseMovingInterval") = TypeDescriptor.CreateProperty(ManagingFastMouseMovingInterval.ComponentType, ManagingFastMouseMovingInterval, attributoVero)
            End If
        End Sub

        'Starting values when you drop the control on a form
        Public Overrides Sub OnSetComponentDefaults()
            CType(Me.Component, MTGCComboBox).Text = ""
            CType(Me.Component, MTGCComboBox).BorderStyle = TipiBordi.FlatXP
        End Sub
    End Class
#End Region

#Region " **************** Properties **************** "
    <Description("When DropDownList, you can only select items in the combo")>
    Public Shadows Property DropDownStyle() As CustomDropDownStyle
        Get
            Return m_DropDownStyle
        End Get
        Set(ByVal Value As CustomDropDownStyle)
            m_DropDownStyle = Value
            TypeDescriptor.Refresh(Me)
        End Set
    End Property

    <Description("Set this property to 'True' if you want to manage the fast mouse moving over the combo while Highlighted")>
    Public Property ManagingFastMouseMoving() As Boolean
        Get
            Return m_ManagingFastMouseMoving
        End Get
        Set(ByVal Value As Boolean)
            m_ManagingFastMouseMoving = Value
            TypeDescriptor.Refresh(Me)
        End Set
    End Property

    <Description("Timer interval used in Fast Mouve Moving managament (in ms)")>
    Public Property ManagingFastMouseMovingInterval() As Integer
        Get
            Return m_ManagingFastMouseMovingInterval
        End Get
        Set(ByVal Value As Integer)
            m_ManagingFastMouseMovingInterval = Value
        End Set
    End Property

    <Description("Arrow Color")>
    Public Property ArrowColor() As Color
        Get
            Return m_ArrowColor
        End Get
        Set(ByVal Value As Color)
            m_ArrowColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Arrow Box Color")>
    Public Property ArrowBoxColor() As Color
        Get
            Return m_ArrowBoxColor
        End Get
        Set(ByVal Value As Color)
            m_ArrowBoxColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Arrow Box Color when Disabled")>
    Public Property DisabledArrowBoxColor() As Color
        Get
            Return m_DisabledArrowBoxColor
        End Get
        Set(ByVal Value As Color)
            m_DisabledArrowBoxColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Arrow Color when disabled")>
    Public Property DisabledArrowColor() As Color
        Get
            Return m_DisabledArrowColor
        End Get
        Set(ByVal Value As Color)
            m_DisabledArrowColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Border Color when the Combobox is not Highlighted")>
    Public Property NormalBorderColor() As Color
        Get
            Return m_NormalBorderColor
        End Get
        Set(ByVal Value As Color)
            m_NormalBorderColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Background color when disabled")>
    Public Property DisabledBackColor() As Color
        Get
            Return m_DisabledBackColor
        End Get
        Set(ByVal Value As Color)
            m_DisabledBackColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Forecolor when disabled")>
    Public Property DisabledForeColor() As Color
        Get
            Return m_DisabledForeColor
        End Get
        Set(ByVal Value As Color)
            m_DisabledForeColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Border Color when disabled")>
    Public Property DisabledBorderColor() As Color
        Get
            Return m_DisabledBorderColor
        End Get
        Set(ByVal Value As Color)
            m_DisabledBorderColor = Value
            Me.Invalidate()
        End Set
    End Property

    <Description("Text Color of the item selected in the DropDownList")>
    Public Property DropDownForeColor() As Color
        Get
            Return m_DropDownForeColor
        End Get
        Set(ByVal Value As Color)
            m_DropDownForeColor = Value
        End Set
    End Property

    <Description("Back Color of the item selected in the DropDownList")>
    Public Property DropDownBackColor() As Color
        Get
            Return m_DropDownBackColor
        End Get
        Set(ByVal Value As Color)
            m_DropDownBackColor = Value
        End Set
    End Property

    <Description("Background Color of the Arrow when the Dropdownlist is open")>
    Public Property DropDownArrowBackColor() As Color
        Get
            Return m_DropDownArrowBackColor
        End Get
        Set(ByVal Value As Color)
            m_DropDownArrowBackColor = Value
        End Set
    End Property

    <Description("Columns number (max 4)")>
    Public Property ColumnNum() As Integer
        Get
            Return m_ColumnNum
        End Get
        Set(ByVal Value As Integer)
            If Value > 4 Then
                m_ColumnNum = 4
            ElseIf Value < 1 Then
                m_ColumnNum = 1
            Else
                m_ColumnNum = Value
            End If
        End Set
    End Property

    <Description("Size of columns in pixel, splitted by ;"), RefreshProperties(RefreshProperties.All)>
    Public Property ColumnWidth() As String
        Get
            Return m_ColumnWidth
        End Get
        Set(ByVal Value As String)
            m_ColumnWidth = Value
            Select Case Me.ColumnNum
                Case 1
                    wcol1 = m_ColumnWidth
                    If Me.DropDownWidth < CInt(wcol1) + 20 Then
                        Me.DropDownWidth = CInt(wcol1) + 20 '+20 to take care of vertical scrollbar
                    End If
                Case 2
                    wcol = m_ColumnWidth
                    wcol1 = Microsoft.VisualBasic.Left(wcol, InStr(wcol, ";") - 1)
                    wcol2 = Microsoft.VisualBasic.Right(wcol, Len(wcol) - Len(wcol1) - 1)
                    If Me.DropDownWidth < CInt(wcol1) + CInt(wcol2) + 20 Then
                        Me.DropDownWidth = CInt(wcol1) + CInt(wcol2) + 20 '+20 to take care of vertical scrollbar
                    End If
                Case 3
                    wcol = m_ColumnWidth
                    wcol1 = Microsoft.VisualBasic.Left(wcol, InStr(wcol, ";") - 1)
                    wcol = Microsoft.VisualBasic.Right(wcol, Len(wcol) - Len(wcol1) - 1)
                    wcol2 = Microsoft.VisualBasic.Left(wcol, InStr(wcol, ";") - 1)
                    wcol3 = Microsoft.VisualBasic.Right(wcol, Len(wcol) - Len(wcol2) - 1)
                    If Me.DropDownWidth < CInt(wcol1) + CInt(wcol2) + CInt(wcol3) + 20 Then
                        Me.DropDownWidth = CInt(wcol1) + CInt(wcol2) + CInt(wcol3) + 20 '+20 to take care of vertical scrollbar
                    End If
                Case 4
                    wcol = m_ColumnWidth
                    wcol1 = Microsoft.VisualBasic.Left(wcol, InStr(wcol, ";") - 1)
                    wcol = Microsoft.VisualBasic.Right(wcol, Len(wcol) - Len(wcol1) - 1)
                    wcol2 = Microsoft.VisualBasic.Left(wcol, InStr(wcol, ";") - 1)
                    wcol = Microsoft.VisualBasic.Right(wcol, Len(wcol) - Len(wcol2) - 1)
                    wcol3 = Microsoft.VisualBasic.Left(wcol, InStr(wcol, ";") - 1)
                    wcol4 = Microsoft.VisualBasic.Right(wcol, Len(wcol) - Len(wcol3) - 1)
                    If Me.DropDownWidth < CInt(wcol1) + CInt(wcol2) + CInt(wcol3) + CInt(wcol4) + 20 Then
                        Me.DropDownWidth = CInt(wcol1) + CInt(wcol2) + CInt(wcol3) + CInt(wcol4) + 20 '+20 to take care of vertical scrollbar
                    End If
            End Select
        End Set
    End Property

    <Description("Set to true if you want the vertical line to divide every column in the Dropdownlist")>
    Public Property GridLineVertical() As Boolean
        Get
            Return m_GridLineVertical
        End Get
        Set(ByVal Value As Boolean)
            m_GridLineVertical = Value
        End Set
    End Property

    <Description("Set to true if you want the horizontal line to divide every column in the Dropdownlist")>
    Public Property GridLineHorizontal() As Boolean
        Get
            Return m_GridLineHorizontal
        End Get
        Set(ByVal Value As Boolean)
            m_GridLineHorizontal = Value
        End Set
    End Property

    <Description("Color of the gridlines in the Dropdownlist")>
    Public Property GridLineColor() As Color
        Get
            Return m_GridLineColor
        End Get
        Set(ByVal Value As Color)
            m_GridLineColor = Value
        End Set
    End Property

    <Description("Combobox text style: Normal, Upper, Lower")>
    Public Property CharacterCasing() As CharacterCasing
        Get
            Return m_CharacterCasing
        End Get
        Set(ByVal Value As CharacterCasing)
            If m_CharacterCasing <> Value Then
                m_CharacterCasing = Value
                RecreateHandle()
            End If
        End Set
    End Property
    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            If m_CharacterCasing = CharacterCasing.Lower Then
                cp.Style = cp.Style Or CBS_LOWERCASE
            ElseIf m_CharacterCasing = CharacterCasing.Upper Then
                cp.Style = cp.Style Or CBS_UPPERCASE
            End If
            Return cp
        End Get
    End Property

    <Description("Style of the Combobox Border")>
    Public Property BorderStyle() As TipiBordi
        Get
            Return m_BorderStyle
        End Get
        Set(ByVal Value As TipiBordi)
            m_BorderStyle = Value
            If Value = TipiBordi.FlatXP Then
                Me.HighlightBorderColor = Color.Blue
                Me.HighlightBorderOnMouseEvents = True
                Me.DropDownBackColor = Color.FromArgb(193, 210, 238)
                Me.DropDownArrowBackColor = Color.FromArgb(136, 169, 223)
                Me.DropDownForeColor = Color.Black
            End If
            Me.Invalidate()
            TypeDescriptor.Refresh(Me)
        End Set
    End Property

    <Description("How to load the combobox: through the ComboboxItem or a DataTable")>
    Public Property LoadingType() As CaricamentoCombo
        Get
            Return m_LoadingType
        End Get
        Set(ByVal Value As CaricamentoCombo)
            m_LoadingType = Value
            TypeDescriptor.Refresh(Me)
        End Set
    End Property

    <Description("Color of the Border when the combo is focused or the mouse is over")>
    Public Property HighlightBorderColor() As Color
        Get
            Return m_HighlightBorderColor
        End Get
        Set(ByVal Value As Color)
            m_HighlightBorderColor = Value
        End Set
    End Property

    <Description("Set to true if you want to highlight the combobox on GotFocus and MouseEnter")>
    Public Property HighlightBorderOnMouseEvents() As Boolean
        Get
            Return m_HighlightBorderOnMouseEvents
        End Get
        Set(ByVal Value As Boolean)
            m_HighlightBorderOnMouseEvents = Value
            TypeDescriptor.Refresh(Me)
        End Set
    End Property

    <Description("ColumnNames of the Datatable passed through SourceDataTable Property to show in the Dropdownlist")>
    Public Property SourceDataString() As String()
        Get
            Return m_SourceDataString
        End Get
        Set(ByVal Value As String())
            m_SourceDataString = Value

            Dim j As Integer = 0
            If Not m_DataTable Is Nothing Then
                For Each Column_Name As String In m_SourceDataString
                    If m_DataTable.Columns.Contains(Column_Name) Then
                        Dim i As Integer = 0
                        For Each Colonna As DataColumn In m_DataTable.Columns
                            If UCase(Colonna.ColumnName) = UCase(Column_Name) Then
                                Indice(j) = i
                            End If
                            i += 1
                        Next
                        j += 1
                    End If
                Next
            End If
        End Set
    End Property

    <Description("DataTable used as source in the Combobox")>
    Public Property SourceDataTable() As DataTable
        Get
            Return m_DataTable
        End Get
        Set(ByVal Value As DataTable)
            m_DataTable = Value
            If Not Value Is Nothing Then
                Dim j As Integer = 0
                If Not (m_SourceDataString Is Nothing) AndAlso (m_SourceDataString.Length > 0) Then
                    For Each Column_Name As String In m_SourceDataString
                        If m_DataTable.Columns.Contains(Column_Name) Then
                            Dim i As Integer = 0
                            For Each Colonna As DataColumn In m_DataTable.Columns
                                If UCase(Colonna.ColumnName) = UCase(Column_Name) Then
                                    Indice(j) = i
                                End If
                                i += 1
                            Next
                            j += 1
                        End If
                    Next
                Else
                    'the SourceDataString Property hasn't been set ---> columns are taken in the order they are in datatable
                    Indice(0) = 0
                    Indice(1) = 1
                    Indice(2) = 2
                    Indice(3) = 3
                End If
            End If

            For Each dr As DataRow In Value.Rows
                Select Case Me.ColumnNum
                    Case 1
                        Me.Items.Add(New MTGCComboBoxItem(Assegna(dr(Indice(0)))))
                    Case 2
                        Me.Items.Add(New MTGCComboBoxItem(Assegna(dr(Indice(0))), Assegna(dr(Indice(1)))))
                    Case 3
                        Me.Items.Add(New MTGCComboBoxItem(Assegna(dr(Indice(0))), Assegna(dr(Indice(1))), Assegna(dr(Indice(2)))))
                    Case 4
                        Me.Items.Add(New MTGCComboBoxItem(Assegna(dr(Indice(0))), Assegna(dr(Indice(1))), Assegna(dr(Indice(2))), Assegna(dr(Indice(3)))))
                End Select
            Next
        End Set
    End Property

    <Description("This is the Selected Item in the combobox")>
    Public Shadows Property SelectedItem() As MTGCComboBoxItem
        Get
            Return m_SelectedItem
        End Get
        Set(ByVal Value As MTGCComboBoxItem)
            m_SelectedItem = Value
        End Set
    End Property

    <Description("This is the Selected Value in the combobox")>
    Public Shadows Property SelectedValue() As String
        Get
            If Me.SelectedItem Is Nothing Then
                Return Nothing
            Else
                Return Me.SelectedItem.Col1
            End If
        End Get
        Set(ByVal value As String)
            Dim i As Integer = 0
            For Each cbo As MTGCComboBoxItem In Me.Items 'MTGCCombobox.Items
                If cbo.Col1.ToString.ToLower = value.ToString.ToLower OrElse
                      cbo.Col2.ToString.ToLower = value.ToString.ToLower OrElse
                      cbo.Col3.ToString.ToLower = value.ToString.ToLower OrElse
                      cbo.Col4.ToString.ToLower = value.ToString.ToLower Then
                    Me.SelectedIndex = i
                    Exit For
                End If
                i += 1
            Next
        End Set
    End Property

#End Region

#Region " **************** General Methods and Overrides ****************"
    'Select the item
    Public Function ItemSelect(ByVal colNumber As Integer, ByVal value As String, Optional ByVal CaseSensitive As Boolean = False, Optional ByVal RaiseSelectedIndexChanged As Boolean = False) As Boolean
        Dim comboValue As Object

        If colNumber = 0 Then
            'It means that i want to set selectedindex to -1
            m_RaiseSelectedIndexChanged = RaiseSelectedIndexChanged
            Me.SelectedIndex = -1
            m_RaiseSelectedIndexChanged = True
            Return True
        End If

        If colNumber <= m_ColumnNum Then
            For i As Integer = 0 To Me.Items.Count - 1
                Select Case colNumber
                    Case 1
                        comboValue = CType(Me.Items(i), MTGCComboBoxItem).Col1
                    Case 2
                        comboValue = CType(Me.Items(i), MTGCComboBoxItem).Col2
                    Case 3
                        comboValue = CType(Me.Items(i), MTGCComboBoxItem).Col3
                    Case 4
                        comboValue = CType(Me.Items(i), MTGCComboBoxItem).Col4
                End Select
                If Not CaseSensitive Then
                    comboValue = UCase(comboValue)
                    value = UCase(value)
                End If
                If comboValue = value Then
                    m_RaiseSelectedIndexChanged = RaiseSelectedIndexChanged
                    Me.SelectedIndex = i
                    m_RaiseSelectedIndexChanged = True
                    Return True
                End If
            Next

            Return False
        Else
            Return False
        End If
    End Function

    'This function is used to take care of DBNull in the DataTable
    Private Function Assegna(ByVal Value As Object) As String
        If IsDBNull(Value) Then
            Assegna = ""
        Else
            Assegna = CStr(Value)
        End If
    End Function

    'Event fired every time the Timer ticks
    Private Sub TimerEventProcessor(ByVal myObject As Object, ByVal myEventArgs As EventArgs)
        If Me.BorderStyle = TipiBordi.FlatXP AndAlso Me.DesignMode = False Then
            If Me.Focused Then Exit Sub
            Dim mouseIsOver As Boolean
            Dim mousePosition As Point = Control.MousePosition
            Try
                mousePosition = PointToClient(mousePosition)
            Catch ex As Exception
                'Debug.WriteLine("Error")
                Exit Sub
            End Try
            mouseIsOver = ClientRectangle.Contains(mousePosition)
            If currentColor.Equals(m_HighlightBorderColor) Then
                'Combo active
                If Not mouseIsOver Then
                    Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
                    DrawBorder(g, m_NormalBorderColor)
                    DrawNormalArrow(g, True)
                    currentColor = m_NormalBorderColor
                    g.Dispose()
                    MouseOver = False
                End If
            ElseIf currentColor.Equals(m_NormalBorderColor) Then
                'Combo disactive
                If mouseIsOver AndAlso MouseOver Then
                    Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
                    If Me.HighlightBorderOnMouseEvents = True Then
                        If Not Highlighted Then
                            currentColor = Me.HighlightBorderColor
                            DrawBorder(g, currentColor)
                            DrawHighlightedArrow(g, False)
                        End If
                    End If
                    g.Dispose()
                End If
            End If
        End If
    End Sub

    'Calculate the location of the Arrow Box
    Private Sub ArrowBoxPosition(ByRef left As Integer, ByRef top As Integer, ByRef width As Integer, ByRef height As Integer)
        Dim rc As Rectangle = ClientRectangle
        width = arrowWidth
        left = rc.Right - width - 2
        top = rc.Top + 2
        height = rc.Height - 4
    End Sub

    'Draw the Flat Arrow Box when not highlighted
    Private Sub DrawNormalArrow(ByRef g As Graphics, ByVal disable As Boolean)
        If Me.BorderStyle = TipiBordi.FlatXP Then
            Dim left, top, arrowWidth, height As Integer
            ArrowBoxPosition(left, top, arrowWidth, height)

            Dim stripeColorBrush As Brush
            If Me.Enabled Then
                stripeColorBrush = New SolidBrush(m_ArrowBoxColor)
            Else
                stripeColorBrush = New SolidBrush(m_DisabledArrowBoxColor)
            End If
            Dim Larghezza As Integer = SystemInformation.VerticalScrollBarWidth - arrowWidth
            If (Me.Enabled) Then
                Dim b As Brush = New SolidBrush(m_ArrowBoxColor)
                g.FillRectangle(b, New Rectangle(left - Larghezza, top - 2, SystemInformation.VerticalScrollBarWidth, height + 4))
            End If

            If Me.Enabled Then
                Dim p As Pen = New Pen(m_NormalBorderColor)
                g.DrawLine(p, New Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Top), New Point(ClientRectangle.Right, ClientRectangle.Top))
                g.DrawLine(p, New Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Bottom - 1), New Point(ClientRectangle.Right, ClientRectangle.Bottom - 1))

                If Not disable Then
                    DrawHighlightedArrow(g, True)
                    g.FillRectangle(stripeColorBrush, left, top - 1, arrowWidth + 1, height + 2)
                Else
                    g.FillRectangle(stripeColorBrush, left - 4, top - 1, arrowWidth + 5, height + 2)
                End If

                DrawArrow(g, False)
            Else
                Dim p As Pen = New Pen(DisabledBorderColor)
                g.DrawLine(p, New Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Top), New Point(ClientRectangle.Right, ClientRectangle.Top))
                g.DrawLine(p, New Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Bottom - 1), New Point(ClientRectangle.Right, ClientRectangle.Bottom - 1))

                ' Now draw the unselected background
                g.FillRectangle(stripeColorBrush, left - 5, top - 1, arrowWidth + 6, height + 2)

                DrawArrow(g, True)
            End If
            Highlighted = False
        End If
    End Sub

    'Draw the Flat Arrow Box when highlighted
    Private Sub DrawHighlightedArrow(ByRef g As Graphics, ByVal Delete As Boolean)
        If Me.BorderStyle = TipiBordi.FlatXP Then
            Dim left, top, arrowWidth, height As Integer
            ArrowBoxPosition(left, top, arrowWidth, height)

            If (Me.Enabled) Then
                Dim comboTextWidth As Integer = SystemInformation.VerticalScrollBarWidth - arrowWidth
                If (comboTextWidth < 0) Then comboTextWidth = 1
                Dim b As Brush = New SolidBrush(HighlightBorderColor)
            End If

            If Not Delete Then
                If (DroppedDown) Then
                    Dim cbg As Graphics = CreateGraphics()
                    Dim pressedColorBrush As Brush = New SolidBrush(m_DropDownArrowBackColor)
                    Dim Larghezza As Integer = SystemInformation.VerticalScrollBarWidth - arrowWidth
                    cbg.FillRectangle(pressedColorBrush, New Rectangle((left - Larghezza), top - 1, SystemInformation.VerticalScrollBarWidth + 1, height + 2))
                    Dim p As Pen = New Pen(HighlightBorderColor)
                    cbg.DrawRectangle(p, (left - Larghezza) - 1, top - 2, SystemInformation.VerticalScrollBarWidth + 2, height + 4)
                    DrawArrow(cbg, False)
                    cbg.Dispose()
                    Exit Sub
                Else
                    If Enabled Then
                        Dim b As Brush = New SolidBrush(m_DropDownBackColor)
                        Dim Larghezza As Integer = SystemInformation.VerticalScrollBarWidth - arrowWidth
                        g.FillRectangle(b, New Rectangle((left - Larghezza), top - 1, SystemInformation.VerticalScrollBarWidth + 1, height + 2))

                        Dim pencolor As Color = customBorderColor
                        If (pencolor.Equals(Color.Empty)) Then
                            pencolor = BackColor
                        End If
                    End If
                End If
            Else
                Dim b As Brush = New SolidBrush(BackColor)
                g.FillRectangle(b, left - 1, top - 1, arrowWidth + 2, height + 2)
            End If
            If Me.Enabled Then DrawArrow(g, False)
            Highlighted = True
        End If
    End Sub

    Private Sub DrawArrow(ByVal g As Graphics, ByVal Disable As Boolean)
        If Me.BorderStyle = TipiBordi.FlatXP Then
            Dim left, top, arrowWidth, height As Integer
            ArrowBoxPosition(left, top, arrowWidth, height)

            Dim extra As Integer = 1
            If (bUsingLargeFont) Then extra = 2

            'triangle vertex of the arrow
            Dim pts(2) As Point
            pts(0) = New Point(left + arrowWidth / 2 - 2 - extra - 2, top + height / 2 - 1)
            pts(1) = New Point(left + arrowWidth / 2 + 3 + extra - 1, top + height / 2 - 1)
            pts(2) = New Point(left + arrowWidth / 2 - 1, (top + height / 2 - 1) + 3 + extra)

            'draw the arrow as a polygon
            If (Disable) Then
                Dim b As Brush = New SolidBrush(m_DisabledArrowColor)
                g.FillPolygon(b, pts)
            Else
                Dim b As Brush = New SolidBrush(m_ArrowColor)
                g.FillPolygon(b, pts)
            End If
        End If
    End Sub

    Private Sub DrawBorder(ByVal g As Graphics, ByVal DrawColor As Color)
        If Me.BorderStyle = TipiBordi.FlatXP Then
            g.DrawRectangle(New Pen(Me.BackColor, 1), ClientRectangle.Left + 1, ClientRectangle.Top + 1, ClientRectangle.Width - 1, ClientRectangle.Height - 3)

            'Draw the Border
            If Me.Enabled = False Then 'combo disabilitato
                DrawColor = DisabledBorderColor
            End If

            Dim pen As Pen = New Pen(DrawColor, 1)
            'Border Rectangle
            g.DrawRectangle(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - 1, ClientRectangle.Height - 1)
            'Button Rectangle
            g.DrawRectangle(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - 3, ClientRectangle.Height - 1)
        End If
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If Me.BorderStyle = TipiBordi.FlatXP Then
            Select Case m.Msg
                Case &HF, &H133
                    'WM_PAINT

                    'We have to find if the Mouse is Over the combo
                    Dim mouseIsOver As Boolean
                    Dim mousePosition As Point = Control.MousePosition
                    mousePosition = PointToClient(mousePosition)
                    mouseIsOver = ClientRectangle.Contains(mousePosition)

                    If Me.HighlightBorderOnMouseEvents AndAlso (mouseIsOver OrElse Me.Focused) Then
                        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)

                        DrawBorder(g, Me.HighlightBorderColor)
                        DrawHighlightedArrow(g, False)
                    Else
                        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)

                        DrawBorder(g, m_NormalBorderColor)
                        DrawNormalArrow(g, True)
                    End If
                Case &H2A3
                    'WM_MOUSELEAVE
                    If Me.Focused Then Exit Sub
                    If currentColor.Equals(m_HighlightBorderColor) Then
                        Dim mouseIsOver As Boolean
                        Dim mousePosition As Point = Control.MousePosition
                        mousePosition = PointToClient(mousePosition)
                        mouseIsOver = ClientRectangle.Contains(mousePosition)

                        If Not mouseIsOver Then
                            Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
                            DrawBorder(g, m_NormalBorderColor)
                            DrawNormalArrow(g, True)
                            g.Dispose()
                        End If
                    End If
                Case &H200
                    'WM_MOUSEMOVE
                    If Me.HighlightBorderOnMouseEvents = True AndAlso Not Highlighted Then
                        currentColor = Me.HighlightBorderColor
                        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
                        DrawBorder(g, currentColor)
                        DrawHighlightedArrow(g, False)
                        g.Dispose()
                    End If
                Case &H46
                    'WM_WINDOWPOSCHANGING 
                    If Me.BorderStyle = TipiBordi.FlatXP Then
                        'Repaint the arrow when pressed
                        If Me.HighlightBorderOnMouseEvents Then
                            Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
                            Dim pressedColorBrush As Brush = New SolidBrush(m_DropDownBackColor)
                            Dim Larghezza As Integer = SystemInformation.VerticalScrollBarWidth - arrowWidth
                            g.FillRectangle(pressedColorBrush, New Rectangle((Left - Larghezza), Top - 1, SystemInformation.VerticalScrollBarWidth + 1, Height + 2))
                            Dim p As Pen = New Pen(HighlightBorderColor)
                            g.DrawRectangle(p, (Left - Larghezza) - 1, Top - 2, SystemInformation.VerticalScrollBarWidth + 2, Height + 4)
                            DrawArrow(g, False)
                            g.Dispose()
                            Me.Invalidate()
                        End If
                    End If
                Case Else
                    Exit Select

            End Select
        End If
    End Sub

    'Custom painting of the DropDownList
    Private Sub mComboBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles mComboBox.DrawItem
        Dim g As Graphics = e.Graphics
        Dim r As Rectangle = e.Bounds
        Dim b As SolidBrush
        Dim i As Integer

        If e.Index >= 0 AndAlso e.Index < Me.Items.Count Then
            Dim rd As Rectangle = r
            rd.Width = rd.Left + 100
            b = New SolidBrush(sender.ForeColor)
            g.FillRectangle(b, rd)
            Dim sf As New StringFormat
            sf.Alignment = StringAlignment.Near
            '******************* WINDOWS 98 **********************
            If e.State = DrawItemState.Selected Then
                'item selected
                e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), r)
                Select Case Me.ColumnNum
                    Case 1
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                            If Me.m_GridLineHorizontal Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                            End If
                        End If
                    Case 2
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 3
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If wcol3 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) + CInt(wcol2) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col3.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(2))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 4
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If wcol3 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) + CInt(wcol2) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col3.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(2))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            End If
                        End If
                        If wcol4 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) - CInt(wcol3) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col4.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(3))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                End Select
                If Me.BorderStyle = TipiBordi.FlatXP Then
                    'Use the border color to highlight the selected item 
                    If Me.GridLineHorizontal Then
                        e.Graphics.DrawRectangle(New Pen(Me.HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 2)
                    Else
                        e.Graphics.DrawRectangle(New Pen(Me.HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 1)
                    End If
                End If
                e.DrawFocusRectangle()

                '******************* WINDOWS 2000/XP **********************
            ElseIf (e.State = (DrawItemState.NoAccelerator Or DrawItemState.Selected)) Or (e.State = (DrawItemState.Selected Or DrawItemState.NoAccelerator Or DrawItemState.NoFocusRect)) Then
                'item selected
                e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), r)
                Select Case Me.ColumnNum
                    Case 1
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                            If Me.m_GridLineHorizontal Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                            End If
                        End If
                    Case 2
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 3
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If wcol3 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) + CInt(wcol2) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(2))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col3.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 4
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If wcol3 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) + CInt(wcol2) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(2))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col3.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            End If
                        End If
                        If wcol4 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(DropDownBackColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) - CInt(wcol3) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(3))).ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col4.ToString, Me.Font, New SolidBrush(DropDownForeColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                End Select
                If Me.BorderStyle = TipiBordi.FlatXP Then
                    'Use the border color to highlight the selected item 
                    If Me.GridLineHorizontal Then
                        e.Graphics.DrawRectangle(New Pen(Me.HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 2)
                    Else
                        e.Graphics.DrawRectangle(New Pen(Me.HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 1)
                    End If
                End If
                e.DrawFocusRectangle()
            Else
                'items NOT selected
                e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), r)
                If BorderStyle = TipiBordi.FlatXP Then
                    If Me.GridLineHorizontal Then
                        e.Graphics.DrawRectangle(New Pen(Me.BackColor, 1), r.X, r.Y, r.Width, r.Height - 1)
                    Else
                        e.Graphics.DrawRectangle(New Pen(Me.BackColor, 1), r.X, r.Y, r.Width, r.Height)
                    End If
                End If
                Select Case Me.ColumnNum
                    Case 1
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 2
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 3
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If wcol3 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), rd.X + CInt(wcol1) + CInt(wcol2) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(2))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col3.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                    Case 4
                        If wcol1 > 0 Then
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(0))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col1.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X, rd.Y, sf)
                            End If
                        End If
                        If wcol2 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) - 2, rd.Y, rd.X + CInt(wcol1) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), rd.X + CInt(wcol1) - 1, rd.Y, r.Width - CInt(wcol1) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(1))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col2.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1), rd.Y, sf)
                            End If
                        End If
                        If wcol3 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), rd.X + CInt(wcol1) + CInt(wcol2) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(2))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col3.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1) + CInt(wcol2), rd.Y, sf)
                            End If
                        End If
                        If wcol4 > 0 Then
                            If Me.m_GridLineVertical Then
                                e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 2, rd.Y, rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 2, rd.Y + 15)
                            End If
                            e.Graphics.FillRectangle(New SolidBrush(sender.BackColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3) - 1, rd.Y, r.Width - CInt(wcol1) - CInt(wcol2) - CInt(wcol3) + 1, r.Height)
                            If Me.LoadingType = CaricamentoCombo.DataTable Then
                                e.Graphics.DrawString(Assegna(m_DataTable.Rows(e.Index)(Indice(3))).ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3), rd.Y, sf)
                            ElseIf Me.LoadingType = CaricamentoCombo.ComboBoxItem Then
                                e.Graphics.DrawString(Me.Items.Item(e.Index).Col4.ToString, Me.Font, New SolidBrush(sender.ForeColor), rd.X + CInt(wcol1) + CInt(wcol2) + CInt(wcol3), rd.Y, sf)
                            End If
                        End If
                        If Me.m_GridLineHorizontal Then
                            e.Graphics.DrawLine(New Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + Me.DropDownWidth, rd.Y + rd.Height - 1)
                        End If
                End Select
                e.DrawFocusRectangle()
            End If
        End If
    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'AUTOCOMPLETE: we have to know when a key has been really pressed

        Debug.WriteLine("OnKeyPress " & Me.Text)
        If Me.DropDownStyle = CustomDropDownStyle.DropDown Then
            PressedKey = True
            If Asc(e.KeyChar) = 8 Then
                If Me.SelectedText = Me.Text Then
                    Me.SelectedIndex = -1
                End If
                If Asc(e.KeyChar) = 13 Then e.Handled = True 'This is used to suppress the "Beep" when Enter key is pressed
            End If
        Else
            'ReadOnly AutoComplete Management
            Dim sTypedText As String
            Dim iFoundIndex As Integer
            Dim currentText As String
            Dim Start, selLength As Integer

            If Asc(e.KeyChar) = 8 Then
                If Me.SelectedText = Me.Text Then
                    PressedKey = True
                    Me.SelectedIndex = -1
                    Exit Sub
                End If
            End If
            If Me.SelectionLength > 0 Then
                Start = Me.SelectionStart
                selLength = Me.SelectionLength

                'This is equivalent to Me.Text, but sometimes using Me.Text it doesn't work
                currentText = Me.AccessibilityObject.Value
                Dim posizione As Long
                Dim testingString As String
                posizione = InStr(Me.Text, "&")
                If posizione > 0 Then
                    'The "&" character is contained in Me.Text
                    testingString = Microsoft.VisualBasic.Left(Me.Text, posizione - 1) & Microsoft.VisualBasic.Right(Me.Text, Len(Me.Text) - posizione)
                Else
                    testingString = Me.Text
                End If
                If UCase(testingString) = UCase(Me.AccessibilityObject.Value) Then
                    currentText = Me.Text
                End If

                currentText = currentText.Remove(Start, selLength)
                currentText = currentText.Insert(Start, e.KeyChar)
                sTypedText = currentText
            Else
                Start = Me.SelectionStart
                sTypedText = Me.Text.Insert(Start, e.KeyChar)
            End If
            iFoundIndex = Me.FindString(sTypedText)
            If (iFoundIndex >= 0) Then
                PressedKey = True
            Else
                e.Handled = True
            End If
        End If

        MyBase.OnKeyPress(e)
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        Debug.WriteLine("OnKeyDown " & Me.Text)
        If Me.DropDownStyle = CustomDropDownStyle.DropDownList AndAlso e.KeyCode = Keys.Delete Then
            If Me.Text <> Me.SelectedText Then
                e.Handled = True
            Else
                Me.SelectedIndex = -1
            End If
        End If
        If Me.DropDownStyle = CustomDropDownStyle.DropDown AndAlso e.KeyCode = Keys.Delete Then
            If Me.Text = Me.SelectedText Then
                Me.SelectedIndex = -1
            End If
        End If

        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnKeyUp(ByVal e As System.Windows.Forms.KeyEventArgs)
        'AUTOCOMPLETING

        'WARNING: With VB.Net 2003 there is a strange behaviour. This event is raised not just when any key is pressed
        'but also when the Me.Text property changes. Particularly, it happens when you write in a fast way (for example
        'you press 2 keys and the event is raised 3 times). To manage this we have added a boolean variable PressedKey that
        'is set to true in the OnKeyPress Event

        Dim sTypedText As String
        Dim iFoundIndex As Integer
        Dim oFoundItem As Object
        Dim sFoundText As String
        Dim sAppendText As String

        Debug.WriteLine("OnKeyUp " & Me.Text)
        If PressedKey Then
            'Ignoring alphanumeric chars
            Select Case e.KeyCode
                Case Keys.Left, Keys.Right, Keys.Up, Keys.Delete, Keys.Down, Keys.End, Keys.Home
                    Return
            End Select

            'Get the Typed Text and Find it in the list
            sTypedText = Me.Text
            If e.KeyCode <> Keys.Back Then
                iFoundIndex = Me.FindString(sTypedText)
            Else
                iFoundIndex = Me.FindStringExact(sTypedText)
            End If

            'If we found the Typed Text in the list then Autocomplete
            If iFoundIndex >= 0 AndAlso Me.Text <> "" Then

                'Get the Item from the list (Return Type depends if Datasource was bound 
                ' or List Created)
                oFoundItem = Me.Items(iFoundIndex)

                'Use the ListControl.GetItemText to resolve the Name in case the Combo 
                ' was Data bound
                sFoundText = Me.GetItemText(oFoundItem)

                'Append then found text to the typed text to preserve case
                sAppendText = sFoundText.Substring(sTypedText.Length)
                Me.Text = sTypedText & sAppendText

                'Select the Appended Text
                Me.SelectionStart = sTypedText.Length
                Me.SelectionLength = sAppendText.Length

                If e.KeyCode = Keys.Enter Then
                    iFoundIndex = Me.FindStringExact(Me.Text)
                    Me.SelectedIndex = iFoundIndex
                    SendKeys.Send(vbTab)
                    e.Handled = True
                End If
            Else
                'Forcing SelectedItem to Nothing if we can't autocomplete
                Me.SelectedIndex = -1
                Me.SelectedItem = Nothing
            End If

        End If
        PressedKey = False
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        'Selecting the item which text is showed in the text area of the ComboBox
        Dim iFoundIndex As Integer
        'The Me.AccessibilityObject.Value is used instead of Me.Text to manage
        'the event when you write in the combobox text and the DropDownList
        'is open. In this case, if you click outside the combo, Me.Text mantains 
        'the old value and not the current one
        Dim currentText As String
        currentText = Me.AccessibilityObject.Value
        Dim testingString As String
        Dim posizione As Long
        posizione = InStr(Me.Text, "&")
        If posizione > 0 Then
            'The "&" character is contained in Me.Text
            testingString = Microsoft.VisualBasic.Left(Me.Text, posizione - 1) & Microsoft.VisualBasic.Right(Me.Text, Len(Me.Text) - posizione)
        Else
            testingString = Me.Text
        End If
        If UCase(testingString) = UCase(Me.AccessibilityObject.Value) Then
            currentText = Me.Text
        End If

        iFoundIndex = Me.FindStringExact(currentText)
        Me.SelectedIndex = iFoundIndex
        If iFoundIndex = -1 Then
            Me.SelectedItem = Nothing
        End If
        MyBase.OnLeave(e)
    End Sub

    'Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
    '    If Not Me.SelectedItem Is Nothing Then
    '        Debug.WriteLine("OnTextChanged " & Me.Text & " " & Me.SelectedItem.Col1)
    '    Else
    '        Debug.WriteLine("OnTextChanged " & Me.Text & " Nessun item selezionato")
    '    End If
    '    MyBase.OnTextChanged(e)
    'End Sub

    Protected Overrides Sub OnCreateControl()
        Me.DisplayMember = "Text"
        Me.DrawMode = DrawMode.OwnerDrawFixed
        currentColor = m_NormalBorderColor
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        MyBase.OnHandleCreated(e)
        If Me.ManagingFastMouseMoving Then
            myTimer.Interval = m_ManagingFastMouseMovingInterval
            myTimer.Start()
        End If
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        Const WM_SETFONT As Integer = &H30
        Try
            WndProc(Message.Create(Handle, WM_SETFONT, Font.ToHfont, IntPtr.Zero))
        Catch ex As Win32Exception
            ' This error is raised usually because the OnFontChanged is called when
            ' the parent form has not been created yet
            Debug.WriteLine("Error creating window handle")
        End Try
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As System.EventArgs)
        If Me.Enabled = True Then
            Const WM_SETFONT As Integer = &H30
            WndProc(Message.Create(Handle, WM_SETFONT, Font.ToHfont, IntPtr.Zero))
        End If

        Me.SelectionLength = 0

        MyBase.OnEnabledChanged(e)
        Dim m As Message
        m.Msg = &HF
        If Me.Enabled Then
            currentColor = Me.NormalBorderColor
        Else
            currentColor = SystemColors.InactiveBorder
        End If
        'Generate a PAINT event
        WndProc(m)

        'Set BackColor
        If Not MyBase.Enabled Then
            'Set back color
            enabledBackColor = MyBase.BackColor
            enabledForeColor = MyBase.ForeColor
            MyBase.BackColor = Me.DisabledBackColor
            MyBase.ForeColor = Me.DisabledForeColor
            'Get the controls editbox
            Dim hwnd As Int32 = GetWindow(MyBase.Handle.ToInt32, &H5)  'GW_CHILD = &H5
            'Enable it so it adopts the controls BackColor and displays text in normal(ForeColor)
            SendMessage(hwnd, &HA, 1, 0)  'WM_ENABLE = &H100
        Else
            'Set back color to original
            MyBase.BackColor = enabledBackColor
            MyBase.ForeColor = enabledForeColor
        End If
        'Generate a PAINT event
        WndProc(m)
    End Sub

#End Region

#Region " **************** Mouse and focus Overrides ****************"
    'Override mouse and focus events to draw
    'proper borders. Basically, set the color and Invalidate(),
    'In general, Invalidate causes a control to redraw itself.
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        If Me.HighlightBorderOnMouseEvents = True AndAlso Not Highlighted Then
            currentColor = Me.HighlightBorderColor
            Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
            DrawBorder(g, currentColor)
            DrawHighlightedArrow(g, False)
            g.Dispose()
        End If
        MouseOver = True
    End Sub

    Protected Overrides Sub OnMouseHover(ByVal e As System.EventArgs)
        MyBase.OnMouseHover(e)
        MouseOver = True
    End Sub

    Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
        If Me.SelectedIndex <> -1 Then
            m_SelectedItem = Me.Items(Me.SelectedIndex)
        Else
            m_SelectedItem = Nothing
        End If
        If m_RaiseSelectedIndexChanged Then
            MyBase.OnSelectedIndexChanged(e)
        End If
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        If Me.Focused Then Exit Sub
        If Me.HighlightBorderOnMouseEvents = True AndAlso Highlighted Then
            Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
            DrawBorder(g, m_NormalBorderColor)
            DrawNormalArrow(g, True)
            g.Dispose()
        End If
        MouseOver = False
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        MouseOver = True
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        MyBase.OnLostFocus(e)
        If Me.HighlightBorderOnMouseEvents = True AndAlso Highlighted Then
            currentColor = Me.NormalBorderColor
            Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
            DrawBorder(g, m_NormalBorderColor)
            DrawNormalArrow(g, True)
            g.Dispose()
        End If
        If Not Me.BindedControl Is Nothing AndAlso Len(Trim(Me.BindedControl.Control)) > 0 AndAlso UCase(Me.BindedControl.Control) <> UCase(Me.Name) AndAlso Me.BindedControl.Clear = True Then
            Dim contenit As Contenitore
            contenit.Indice = -1
            FindControl(Me.BindedControl.Control, MyBase.FindForm, contenit)
            If contenit.Indice <> -1 Then contenit.cnt.Controls(contenit.Indice).Text = ""
        End If
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        If Not Me.BindedControl Is Nothing AndAlso Len(Trim(Me.BindedControl.Control)) > 0 AndAlso UCase(Me.BindedControl.Control) <> UCase(Me.Name) Then
            Dim contenit As Contenitore
            contenit.Indice = -1
            FindControl(Me.BindedControl.Control, MyBase.FindForm, contenit)
            If contenit.Indice <> -1 Then contenit.cnt.Controls(contenit.Indice).Text = Me.BindedControl.Text
        End If

        MyBase.OnGotFocus(e)
        If Me.HighlightBorderOnMouseEvents = True AndAlso Not Highlighted Then
            currentColor = Me.HighlightBorderColor
            Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
            DrawBorder(g, currentColor)
            DrawHighlightedArrow(g, False)
            g.Dispose()
        End If
    End Sub
#End Region

#Region "***************** Type Converter for ControlBinding ***************"
    Public Class ControlList : Inherits System.ComponentModel.StringConverter
        '''
        Public Overloads Overrides Function GetStandardValues(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.ComponentModel.TypeConverter.StandardValuesCollection
            Return New StandardValuesCollection(NomeControlli)
        End Function
        '''
        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function
        '''
        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function
        ''''

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

    Dim _BindedControl As New ControlloAssociato("", "", False)
    Dim _Controlli As String
    Dim NumeroControlli As Integer = 0
    Shared formContenitore As Form
    Shared NomeControlli() As String

    Structure Contenitore
        Dim Indice As Integer
        Dim cnt As System.Windows.Forms.Control
    End Structure

    Private Function FindControl(ByVal controlName As String, ByVal cnt As System.Windows.Forms.Control, ByRef cont As Contenitore)
        If Not cnt Is Nothing Then
            For i As Integer = 0 To cnt.Controls.Count - 1
                If ControlloContenitore(cnt.Controls(i)) Then
                    FindControl(controlName, cnt.Controls(i), cont)
                ElseIf UCase(cnt.Controls(i).Name) = UCase(controlName) Then
                    cont.Indice = i
                    cont.cnt = cnt
                End If
            Next
        End If
    End Function

    Private Function ControlloContenitore(ByVal Controllo As System.Windows.Forms.Control) As Boolean
        ControlloContenitore = False

        If TypeOf Controllo Is TabControl Then
            ControlloContenitore = True
        End If
        If TypeOf Controllo Is GroupBox Then
            ControlloContenitore = True
        End If
        If TypeOf Controllo Is Panel Then
            ControlloContenitore = True
        End If
    End Function

    Private Sub SalvaValoriContenitore(ByVal cntContenitore As System.Windows.Forms.Control)
        Dim i As Integer

        If Not cntContenitore Is Nothing Then
            For i = 0 To cntContenitore.Controls.Count - 1
                If ControlloContenitore(cntContenitore.Controls(i)) Then
                    SalvaValoriContenitore(cntContenitore.Controls(i))
                Else
                    ReDim Preserve NomeControlli(NumeroControlli)
                    NomeControlli(NumeroControlli) = CType(cntContenitore.Controls(i), System.Windows.Forms.Control).Name
                    NumeroControlli += 1
                End If
            Next
        End If
    End Sub

    Public Property BindedControl() As ControlloAssociato
        Get
            formContenitore = MyBase.FindForm
            Return _BindedControl
        End Get
        Set(ByVal Value As ControlloAssociato)
            _BindedControl = Value
        End Set
    End Property

    <SerializableAttribute(), TypeConverter(GetType(BindedControlConverter))> _
    Public Class ControlloAssociato
        Private _cnt As String
        Private _text As String
        Private _clear As Boolean
        Private NumeroControlli As Integer = 0

        Public Sub New(ByVal cnt As String, ByVal testo As String, ByVal clear As Boolean)
            _cnt = cnt
            _text = testo
            _clear = clear
        End Sub

        Private Function ControlloContenitore(ByVal Controllo As System.Windows.Forms.Control) As Boolean
            ControlloContenitore = False

            If TypeOf Controllo Is TabControl Then
                ControlloContenitore = True
            End If
            If TypeOf Controllo Is GroupBox Then
                ControlloContenitore = True
            End If
            If TypeOf Controllo Is Panel Then
                ControlloContenitore = True
            End If
        End Function

        Private Sub SalvaValoriContenitore(ByVal cntContenitore As System.Windows.Forms.Control)
            Dim i As Integer

            If Not cntContenitore Is Nothing Then
                For i = 0 To cntContenitore.Controls.Count - 1
                    If ControlloContenitore(cntContenitore.Controls(i)) Then
                        SalvaValoriContenitore(cntContenitore.Controls(i))
                    Else
                        ReDim Preserve NomeControlli(NumeroControlli)
                        NomeControlli(NumeroControlli) = CType(cntContenitore.Controls(i), System.Windows.Forms.Control).Name
                        NumeroControlli += 1
                    End If
                Next
            End If
        End Sub

        <TypeConverter(GetType(ControlList))> _
        Public Property Control() As String
            Get
                NumeroControlli = 0
                SalvaValoriContenitore(formContenitore)
                If Not NomeControlli Is Nothing Then NomeControlli.Sort(NomeControlli)
                Return _cnt
            End Get
            Set(ByVal Value As String)
                _cnt = Value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal Value As String)
                _text = Value
            End Set
        End Property

        Public Property Clear() As Boolean
            Get
                Return _clear
            End Get
            Set(ByVal Value As Boolean)
                _clear = Value
            End Set
        End Property
    End Class

    Friend Class BindedControlConverter
        Inherits ExpandableObjectConverter

        Public Overloads Overrides Function CanConvertFrom( _
                        ByVal context As ITypeDescriptorContext, _
                        ByVal sourceType As Type) As Boolean

            If (sourceType.Equals(GetType(String))) Then
                Return True
            Else
                Return MyBase.CanConvertFrom(context, sourceType)
            End If
        End Function

        Public Overloads Overrides Function CanConvertTo( _
            ByVal context As ITypeDescriptorContext, _
            ByVal destinationType As Type) As Boolean

            If (destinationType.Equals(GetType(String))) Then
                Return True
            Else
                Return MyBase.CanConvertTo(context, destinationType)
            End If
        End Function

        Public Overloads Overrides Function ConvertFrom( _
         ByVal context As ITypeDescriptorContext, _
         ByVal cultureInfo As CultureInfo, _
         ByVal value As Object) As Object

            If (TypeOf value Is String) Then
                Dim propertyList() As String
                propertyList = CType(value, String).Split(New Char() {";"})

                Try
                    Return New ControlloAssociato(Convert.ToString(Trim(propertyList(1))), Convert.ToString(Trim(propertyList(2))), Boolean.Parse(propertyList(0)))
                Catch
                    Throw New InvalidCastException("Argomenti non validi " & value)
                End Try
            Else
                Return MyBase.ConvertFrom(context, cultureInfo, value)
            End If
        End Function

        Public Overloads Overrides Function ConvertTo( _
         ByVal context As ITypeDescriptorContext, _
         ByVal cultureInfo As CultureInfo, _
         ByVal value As Object, ByVal destinationType _
                                         As Type) As Object

            If (TypeOf value Is ControlloAssociato) Then
                If (destinationType.Equals(GetType(String))) Then
                    Dim cnt As ControlloAssociato = CType(value, ControlloAssociato)

                    Return cnt.Clear.ToString & "; " & cnt.Control & "; " & cnt.Text
                End If
            Else
                Return MyBase.ConvertTo(context, cultureInfo, _
                                        value, destinationType)
            End If
        End Function

        Public Overloads Overrides Function GetProperties(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal value As Object, ByVal attributes() As System.Attribute) As System.ComponentModel.PropertyDescriptorCollection
            Return TypeDescriptor.GetProperties(GetType(ControlloAssociato))
        End Function

        Public Overloads Overrides Function GetPropertiesSupported(ByVal context As System.ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function
    End Class
#End Region

End Class

#Region " ----- This class is used to create and manage the items of the combobox (with max 4 columns) -----"

Public Class MTGCComboBoxItem
    'Since all we need is a "Text" property for this example we can
    'Subclass by inheriting any object desired.
    'For this example, we'll use the ListViewItem
    Inherits ListViewItem
    Implements IComparable

    'each of the below public declarations will be "visible" to the outside
    'You may add as many of these declarations using whatever types you desire
    Public Col1 As String
    Public Col2 As String
    Public Col3 As String
    Public Col4 As String

    'every value of MyInfo you want to store, get's added to the NEW declaration 
    Sub New(ByVal C1 As String, Optional ByVal C2 As String = "", Optional ByVal C3 As String = "", Optional ByVal C4 As String = "")
        MyBase.New()
        'transfer all incoming parameters to your local storage
        Col1 = C1
        Col2 = C2
        Col3 = C3
        Col4 = C4
        'and finally, pass back the Text property
        Me.Text = C1
    End Sub

    'Function used to sort the items on first element Col1
    Private Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        'every not nothing object is greater than nothing
        If obj Is Nothing Then Return 1

        'this is used to take care of late binding
        Dim other As MTGCComboBoxItem = CType(obj, MTGCComboBoxItem)

        'comparing strings
        Return StrComp(Col1, other.Col1, CompareMethod.Text)
    End Function

End Class

#End Region

