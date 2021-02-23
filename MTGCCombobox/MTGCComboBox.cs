// -------------------------------------------------------------------
// *****************************
// *   MTGCComboBox for .NET   *
// ***************************** 
// 
// Copyright © 2004, MT Global Consulting srl. All rights reserved

// Version: 1.0.0.0
// Developed by: Claudio Di Flumeri, Massimiliano Silvestro
// Web Site: http://www.mtgc.net
// e-mail: claudio@mtgc.net
// 
// You may include the source code, modified source code, assembly
// within your own projects for either personal or commercial use
// 
// 
// Disclaimer: 
// This code is provided as is and without warranty, written or implied.
// -------------------------------------------------------------------

using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

[Designer(typeof(MyTypeDesigner))]
public class MTGCComboBox : ComboBox
{
    private bool PressedKey = false;

    private string wcol, wcol1, wcol2, wcol3, wcol4;  // Column widths
    private Color currentColor;                       // Current border color

    private Timer _myTimer;

    private Timer myTimer
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _myTimer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_myTimer != null)
            {
            }

            _myTimer = value;
            if (_myTimer != null)
            {
            }
        }
    }

    private int arrowWidth = 12;
    private bool bUsingLargeFont = false;
    private Color arrowColor = Color.Black;
    private Color arrowDisableColor = Color.LightGray;
    private Color customBorderColor = Color.Empty;
    private Color borderColor = SystemColors.Highlight;
    private Color dropDownArrowAreaNormalColor = SystemColors.Control;
    private Color dropDownArrowAreaHotColor = Color.Black;
    private Color dropDownArrowAreaPressedColor = Color.Black;
    private bool Highlighted = true;
    private int[] Indice = new int[5];
    private bool MouseOver = false;

    // constants for CharacterCasing
    private const int CBS_UPPERCASE = 0x2000;
    private const int CBS_LOWERCASE = 0x4000;

    // Border Type
    enum TipiBordi
    {
        Fixed3D,
        FlatXP
    }

    // Loading Type
    enum CaricamentoCombo
    {
        ComboBoxItem,
        DataTable
    }

    // Our DropDownStyle to manage the DropDownList with Autocomplete
    enum CustomDropDownStyle
    {
        DropDown,
        DropDownList
    }

    // Property Declaration
    private ComboBox _mComboBox;

    private ComboBox mComboBox
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _mComboBox;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_mComboBox != null)
            {
                _mComboBox.DrawItem -= mComboBox_DrawItem;
            }

            _mComboBox = value;
            if (_mComboBox != null)
            {
                _mComboBox.DrawItem += mComboBox_DrawItem;
            }
        }
    }

    private int m_ColumnNum = 1;
    private string m_ColumnWidth = Conversions.ToString(Width);
    private Color m_NormalBorderColor = Color.Black;
    private Color m_DropDownForeColor = Color.Black;
    private Color m_DropDownBackColor = Color.FromArgb(193, 210, 238);
    private Color m_DropDownArrowBackColor = Color.FromArgb(136, 169, 223);
    private bool m_GridLineVertical = false;
    private bool m_GridLineHorizontal = false;
    private Color m_GridLineColor = Color.LightGray;
    private CharacterCasing m_CharacterCasing = CharacterCasing.Normal;
    private TipiBordi m_BorderStyle = TipiBordi.FlatXP; // TipiBordi.Fixed3D
    private Color m_HighlightBorderColor = Color.Blue;
    private bool m_HighlightBorderOnMouseEvents = true;
    private DataTable m_DataTable;
    private string[] m_SourceDataString;
    private CaricamentoCombo m_LoadingType = CaricamentoCombo.ComboBoxItem;
    private bool m_ManagingFastMouseMoving = true;
    private int m_ManagingFastMouseMovingInterval = 30;
    private CustomDropDownStyle m_DropDownStyle = CustomDropDownStyle.DropDown;

    public new event DrawItemEventHandler DrawItem;

    public new delegate void DrawItemEventHandler(object sender, DrawItemEventArgs e);

    // Constructor
    public MTGCComboBox() : base()
    {
        mComboBox = this;
        myTimer.Tick += TimerEventProcessor;
    }

    protected new override void Dispose(bool disposing)
    {
        if (myTimer.Enabled)
            myTimer.Stop();
        if (disposing)
            base.Dispose(true);
        GC.SuppressFinalize(this);
    }

    internal class MyTypeDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        private DesignerVerbCollection verbi;

        private void OnVerbFlatXP(object sender, EventArgs e)
        {
            foreach (DesignerVerb verbo in Verbs)
                verbo.Checked = false;
            ((DesignerVerb)sender).Checked = true;
            PropertyDescriptor tipoBordo;
            tipoBordo = TypeDescriptor.GetProperties(Control)["BorderStyle"];
            tipoBordo.SetValue(Control, TipiBordi.FlatXP);
        }

        private void OnVerbFixed3D(object sender, EventArgs e)
        {
            foreach (DesignerVerb verbo in Verbs)
                verbo.Checked = false;
            ((DesignerVerb)sender).Checked = true;
            PropertyDescriptor tipoBordo;
            tipoBordo = TypeDescriptor.GetProperties(Control)["BorderStyle"];
            tipoBordo.SetValue(Control, TipiBordi.Fixed3D);
        }

        private void OnVerbInformazioniSu(object sender, EventArgs e)
        {
            var frmAbout = new frmAboutComboBox();

            frmAbout.ShowDialog();
        }

        public new override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbi == null)
                {
                    verbi = new DesignerVerbCollection();
                    verbi.Add(new DesignerVerb("FlatXP", new EventHandler(OnVerbFlatXP)));
                    verbi.Add(new DesignerVerb("Fixed3D", new EventHandler(OnVerbFixed3D)));
                    verbi.Add(new DesignerVerb("About MTGCComboBox", new EventHandler(OnVerbInformazioniSu)));

                    PropertyDescriptor tipoBordo;
                    tipoBordo = TypeDescriptor.GetProperties(Control)["BorderStyle"];
                    TipiBordi mtipoBordo;

                    mtipoBordo = (TipiBordi)tipoBordo.GetValue(Component);
                    if ((int)mtipoBordo == (int)TipiBordi.FlatXP)
                        verbi[0].Checked = true;
                    else if (mtipoBordo == (int)TipiBordi.Fixed3D)
                        verbi[1].Checked = true;
                }
                return verbi;
            }
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            var attributoVero = new[] { new ReadOnlyAttribute(true) };
            var attributoFalso = new[] { new ReadOnlyAttribute(false) };

            PropertyDescriptor HighlightBorderOnMouseEvents;
            HighlightBorderOnMouseEvents = properties["HighlightBorderOnMouseEvents"];
            PropertyDescriptor HighlightBorderColor;
            HighlightBorderColor = properties["HighlightBorderColor"];
            PropertyDescriptor NormalBorderColor;
            NormalBorderColor = properties["NormalBorderColor"];
            PropertyDescriptor DropDownArrowBackColor;
            DropDownArrowBackColor = properties["DropDownArrowBackColor"];
            PropertyDescriptor tipoBordo;
            tipoBordo = properties["BorderStyle"];
            TipiBordi mtipoBordo;

            mtipoBordo = (TipiBordi)tipoBordo.GetValue(Component);
            switch (mtipoBordo)
            {
                case TipiBordi.FlatXP:
                    {
                        properties["HighlightBorderOnMouseEvents"] = TypeDescriptor.CreateProperty(HighlightBorderOnMouseEvents.ComponentType, HighlightBorderOnMouseEvents, attributoFalso);
                        properties["HighlightBorderColor"] = TypeDescriptor.CreateProperty(HighlightBorderColor.ComponentType, HighlightBorderColor, attributoFalso);
                        properties["NormalBorderColor"] = TypeDescriptor.CreateProperty(NormalBorderColor.ComponentType, NormalBorderColor, attributoFalso);
                        properties["DropDownArrowBackColor"] = TypeDescriptor.CreateProperty(DropDownArrowBackColor.ComponentType, DropDownArrowBackColor, attributoFalso);
                        break;
                    }

                case TipiBordi.Fixed3D:
                    {
                        properties["HighlightBorderOnMouseEvents"] = TypeDescriptor.CreateProperty(HighlightBorderOnMouseEvents.ComponentType, HighlightBorderOnMouseEvents, attributoVero);
                        properties["HighlightBorderColor"] = TypeDescriptor.CreateProperty(HighlightBorderColor.ComponentType, HighlightBorderColor, attributoVero);
                        properties["NormalBorderColor"] = TypeDescriptor.CreateProperty(NormalBorderColor.ComponentType, NormalBorderColor, attributoVero);
                        properties["DropDownArrowBackColor"] = TypeDescriptor.CreateProperty(DropDownArrowBackColor.ComponentType, DropDownArrowBackColor, attributoVero);
                        break;
                    }
            }

            PropertyDescriptor sourceDataTable;
            sourceDataTable = properties["SourceDataTable"];
            PropertyDescriptor sourceDataString;
            sourceDataString = properties["SourceDataString"];
            PropertyDescriptor tipoCaricamento;
            tipoCaricamento = properties["LoadingType"];
            CaricamentoCombo mtipoCaricamento;

            mtipoCaricamento = (CaricamentoCombo)tipoCaricamento.GetValue(Component);
            switch (mtipoCaricamento)
            {
                case CaricamentoCombo.ComboBoxItem:
                    {
                        properties["SourceDataTable"] = TypeDescriptor.CreateProperty(sourceDataTable.ComponentType, sourceDataTable, attributoVero);
                        properties["SourceDataString"] = TypeDescriptor.CreateProperty(sourceDataString.ComponentType, sourceDataString, attributoVero);
                        break;
                    }

                case CaricamentoCombo.DataTable:
                    {
                        properties["SourceDataTable"] = TypeDescriptor.CreateProperty(sourceDataTable.ComponentType, sourceDataTable, attributoFalso);
                        properties["SourceDataString"] = TypeDescriptor.CreateProperty(sourceDataString.ComponentType, sourceDataString, attributoFalso);
                        break;
                    }
            }

            PropertyDescriptor ManagingFastMouseMovingInterval;
            ManagingFastMouseMovingInterval = properties["ManagingFastMouseMovingInterval"];
            bool mManagingFastMouseMoving;
            PropertyDescriptor ManagingFastMouseMoving;
            ManagingFastMouseMoving = properties["ManagingFastMouseMoving"];

            mManagingFastMouseMoving = Conversions.ToBoolean(ManagingFastMouseMoving.GetValue(Component));
            if (mManagingFastMouseMoving)
                properties["ManagingFastMouseMovingInterval"] = TypeDescriptor.CreateProperty(ManagingFastMouseMovingInterval.ComponentType, ManagingFastMouseMovingInterval, attributoFalso);
            else
                properties["ManagingFastMouseMovingInterval"] = TypeDescriptor.CreateProperty(ManagingFastMouseMovingInterval.ComponentType, ManagingFastMouseMovingInterval, attributoVero);
        }

        // Starting values when you drop the control on a form
        public override void OnSetComponentDefaults()
        {
            ((MTGCComboBox)Component).Text = "";
            ((MTGCComboBox)Component).BorderStyle = TipiBordi.FlatXP;
        }
    }

    [Description("When DropDownList, you can only select items in the combo")]
    public new CustomDropDownStyle DropDownStyle
    {
        get
        {
            return m_DropDownStyle;
        }
        set
        {
            m_DropDownStyle = value;
            TypeDescriptor.Refresh(this);
        }
    }

    [Description("Set this property to 'True' if you want to manage the fast mouse moving over the combo while Highlighted")]
    public bool ManagingFastMouseMoving
    {
        get
        {
            return m_ManagingFastMouseMoving;
        }
        set
        {
            m_ManagingFastMouseMoving = value;
            TypeDescriptor.Refresh(this);
        }
    }

    [Description("Timer interval used in Fast Mouve Moving managament (in ms)")]
    public int ManagingFastMouseMovingInterval
    {
        get
        {
            return m_ManagingFastMouseMovingInterval;
        }
        set
        {
            m_ManagingFastMouseMovingInterval = value;
        }
    }


    [Description("Border Color when the Combobox is not Highlighted")]
    public Color NormalBorderColor
    {
        get
        {
            return m_NormalBorderColor;
        }
        set
        {
            m_NormalBorderColor = value;
            Invalidate();
        }
    }

    [Description("Text Color of the item selected in the DropDownList")]
    public Color DropDownForeColor
    {
        get
        {
            return m_DropDownForeColor;
        }
        set
        {
            m_DropDownForeColor = value;
        }
    }

    [Description("Back Color of the item selected in the DropDownList")]
    public Color DropDownBackColor
    {
        get
        {
            return m_DropDownBackColor;
        }
        set
        {
            m_DropDownBackColor = value;
        }
    }

    [Description("Background Color of the Arrow when the Dropdownlist is open")]
    public Color DropDownArrowBackColor
    {
        get
        {
            return m_DropDownArrowBackColor;
        }
        set
        {
            m_DropDownArrowBackColor = value;
        }
    }

    [Description("Columns number (max 4)")]
    public int ColumnNum
    {
        get
        {
            return m_ColumnNum;
        }
        set
        {
            if (value > 4)
                m_ColumnNum = 4;
            else if (value < 1)
                m_ColumnNum = 1;
            else
                m_ColumnNum = value;
        }
    }

    [Description("Size of columns in pixel, splitted by ;")]
    [RefreshProperties(RefreshProperties.All)]
    public string ColumnWidth
    {
        get
        {
            return m_ColumnWidth;
        }
        set
        {
            m_ColumnWidth = value;
            switch (ColumnNum)
            {
                case 1:
                    {
                        wcol1 = m_ColumnWidth;
                        if (DropDownWidth < Conversions.ToInteger(wcol1) + 20)
                            DropDownWidth = Conversions.ToInteger(wcol1) + 20;// +20 to take care of vertical scrollbar
                        break;
                    }

                case 2:
                    {
                        wcol = m_ColumnWidth;
                        wcol1 = Microsoft.VisualBasic.Left(wcol, Strings.InStr(wcol, ";") - 1);
                        wcol2 = Microsoft.VisualBasic.Right(wcol, Strings.Len(wcol) - Strings.Len(wcol1) - 1);
                        if (DropDownWidth < Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + 20)
                            DropDownWidth = Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + 20;// +20 to take care of vertical scrollbar
                        break;
                    }

                case 3:
                    {
                        wcol = m_ColumnWidth;
                        wcol1 = Microsoft.VisualBasic.Left(wcol, Strings.InStr(wcol, ";") - 1);
                        wcol = Microsoft.VisualBasic.Right(wcol, Strings.Len(wcol) - Strings.Len(wcol1) - 1);
                        wcol2 = Microsoft.VisualBasic.Left(wcol, Strings.InStr(wcol, ";") - 1);
                        wcol3 = Microsoft.VisualBasic.Right(wcol, Strings.Len(wcol) - Strings.Len(wcol2) - 1);
                        if (DropDownWidth < Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) + 20)
                            DropDownWidth = Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) + 20;// +20 to take care of vertical scrollbar
                        break;
                    }

                case 4:
                    {
                        wcol = m_ColumnWidth;
                        wcol1 = Microsoft.VisualBasic.Left(wcol, Strings.InStr(wcol, ";") - 1);
                        wcol = Microsoft.VisualBasic.Right(wcol, Strings.Len(wcol) - Strings.Len(wcol1) - 1);
                        wcol2 = Microsoft.VisualBasic.Left(wcol, Strings.InStr(wcol, ";") - 1);
                        wcol = Microsoft.VisualBasic.Right(wcol, Strings.Len(wcol) - Strings.Len(wcol2) - 1);
                        wcol3 = Microsoft.VisualBasic.Left(wcol, Strings.InStr(wcol, ";") - 1);
                        wcol4 = Microsoft.VisualBasic.Right(wcol, Strings.Len(wcol) - Strings.Len(wcol3) - 1);
                        if (DropDownWidth < Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) + Conversions.ToInteger(wcol4) + 20)
                            DropDownWidth = Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) + Conversions.ToInteger(wcol4) + 20;// +20 to take care of vertical scrollbar
                        break;
                    }
            }
        }
    }

    [Description("Set to true if you want the vertical line to divide every column in the Dropdownlist")]
    public bool GridLineVertical
    {
        get
        {
            return m_GridLineVertical;
        }
        set
        {
            m_GridLineVertical = value;
        }
    }

    [Description("Set to true if you want the horizontal line to divide every column in the Dropdownlist")]
    public bool GridLineHorizontal
    {
        get
        {
            return m_GridLineHorizontal;
        }
        set
        {
            m_GridLineHorizontal = value;
        }
    }

    [Description("Color of the gridlines in the Dropdownlist")]
    public Color GridLineColor
    {
        get
        {
            return m_GridLineColor;
        }
        set
        {
            m_GridLineColor = value;
        }
    }

    [Description("Combobox text style: Normal, Upper, Lower")]
    public CharacterCasing CharacterCasing
    {
        get
        {
            return m_CharacterCasing;
        }
        set
        {
            if ((int)m_CharacterCasing != (int)value)
            {
                m_CharacterCasing = value;
                RecreateHandle();
            }
        }
    }
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            if ((int)m_CharacterCasing == (int)CharacterCasing.Lower)
                cp.Style = cp.Style | CBS_LOWERCASE;
            else if ((int)m_CharacterCasing == (int)CharacterCasing.Upper)
                cp.Style = cp.Style | CBS_UPPERCASE;
            return cp;
        }
    }

    [Description("Style of the Combobox Border")]
    public TipiBordi BorderStyle
    {
        get
        {
            return m_BorderStyle;
        }
        set
        {
            m_BorderStyle = value;
            if ((int)value == (int)TipiBordi.FlatXP)
            {
                HighlightBorderColor = Color.Blue;
                HighlightBorderOnMouseEvents = true;
                DropDownBackColor = Color.FromArgb(193, 210, 238);
                DropDownArrowBackColor = Color.FromArgb(136, 169, 223);
                DropDownForeColor = Color.Black;
            }
            Invalidate();
            TypeDescriptor.Refresh(this);
        }
    }

    [Description("How to load the combobox: through the ComboboxItem or a DataTable")]
    public CaricamentoCombo LoadingType
    {
        get
        {
            return m_LoadingType;
        }
        set
        {
            m_LoadingType = value;
            TypeDescriptor.Refresh(this);
        }
    }

    [Description("Color of the Border when the combo is focused or the mouse is over")]
    public Color HighlightBorderColor
    {
        get
        {
            return m_HighlightBorderColor;
        }
        set
        {
            m_HighlightBorderColor = value;
        }
    }

    [Description("Set to true if you want to highlight the combobox on GotFocus and MouseEnter")]
    public bool HighlightBorderOnMouseEvents
    {
        get
        {
            return m_HighlightBorderOnMouseEvents;
        }
        set
        {
            m_HighlightBorderOnMouseEvents = value;
            TypeDescriptor.Refresh(this);
        }
    }

    [Description("ColumnNames of the Datatable passed through SourceDataTable Property to show in the Dropdownlist")]
    public string[] SourceDataString
    {
        get
        {
            return m_SourceDataString;
        }
        set
        {
            m_SourceDataString = value;

            int j = 0;
            if (!(m_DataTable == null))
            {
                foreach (string Column_Name in m_SourceDataString)
                {
                    if (m_DataTable.Columns.Contains(Column_Name))
                    {
                        int i = 0;
                        foreach (DataColumn Colonna in m_DataTable.Columns)
                        {
                            if ((Strings.UCase(Colonna.ColumnName) ?? "") == (Strings.UCase(Column_Name) ?? ""))
                                Indice[j] = i;
                            i += 1;
                        }
                        j += 1;
                    }
                }
            }
        }
    }

    [Description("DataTable used as source in the Combobox")]
    public DataTable SourceDataTable
    {
        get
        {
            return m_DataTable;
        }
        set
        {
            m_DataTable = value;
            if (!(value == null))
            {
                int j = 0;
                if (!(m_SourceDataString == null) && m_SourceDataString.Length > 0)
                {
                    foreach (string Column_Name in m_SourceDataString)
                    {
                        if (m_DataTable.Columns.Contains(Column_Name))
                        {
                            int i = 0;
                            foreach (DataColumn Colonna in m_DataTable.Columns)
                            {
                                if ((Strings.UCase(Colonna.ColumnName) ?? "") == (Strings.UCase(Column_Name) ?? ""))
                                    Indice[j] = i;
                                i += 1;
                            }
                            j += 1;
                        }
                    }
                }
                else
                {
                    // the SourceDataString Property hasn't been set ---> columns are taken in the order they are in datatable
                    Indice[0] = 0;
                    Indice[1] = 1;
                    Indice[2] = 2;
                    Indice[3] = 3;
                }
            }

            foreach (DataRow dr in value.Rows)
            {
                switch (ColumnNum)
                {
                    case 1:
                        {
                            Items.Add(new MTGCComboBoxItem(Assegna(dr[Indice[0]])));
                            break;
                        }

                    case 2:
                        {
                            Items.Add(new MTGCComboBoxItem(Assegna(dr[Indice[0]]), Assegna(dr[Indice[1]])));
                            break;
                        }

                    case 3:
                        {
                            Items.Add(new MTGCComboBoxItem(Assegna(dr[Indice[0]]), Assegna(dr[Indice[1]]), Assegna(dr[Indice[2]])));
                            break;
                        }

                    case 4:
                        {
                            Items.Add(new MTGCComboBoxItem(Assegna(dr[Indice[0]]), Assegna(dr[Indice[1]]), Assegna(dr[Indice[2]]), Assegna(dr[Indice[3]])));
                            break;
                        }
                }
            }
        }
    }

    // This function is used to take care of DBNull in the DataTable
    private string Assegna(object Value)
    {
        string AssegnaRet = default(string);
        if (Information.IsDBNull(Value))
            AssegnaRet = "";
        else
            AssegnaRet = Conversions.ToString(Value);
        return AssegnaRet;
    }

    // Event fired every time the Timer ticks
    private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
    {
        if ((int)BorderStyle == (int)TipiBordi.FlatXP && DesignMode == false)
        {
            if (Focused)
                return;
            bool mouseIsOver;
            var mousePosition = MousePosition;
            try
            {
                mousePosition = PointToClient(mousePosition);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine("Error")
                return;
            }
            mouseIsOver = ClientRectangle.Contains(mousePosition);
            if (currentColor.Equals(m_HighlightBorderColor))
            {
                // Combo active
                if (!mouseIsOver)
                {
                    var g = Graphics.FromHwnd(Handle);
                    DrawBorder(g, m_NormalBorderColor);
                    DrawNormalArrow(ref g, true);
                    currentColor = m_NormalBorderColor;
                    g.Dispose();
                    MouseOver = false;
                }
            }
            else if (currentColor.Equals(m_NormalBorderColor))
            {
                // Combo disactive
                if (mouseIsOver && MouseOver)
                {
                    var g = Graphics.FromHwnd(Handle);
                    if (HighlightBorderOnMouseEvents == true)
                    {
                        if (!Highlighted)
                        {
                            currentColor = HighlightBorderColor;
                            DrawBorder(g, currentColor);
                            DrawHighlightedArrow(ref g, false);
                        }
                    }
                    g.Dispose();
                }
            }
        }
    }

    // Calculate the location of the Arrow Box
    private void ArrowBoxPosition(ref int left, ref int top, ref int width, ref int height)
    {
        var rc = ClientRectangle;
        width = arrowWidth;
        left = rc.Right - width - 2;
        top = rc.Top + 2;
        height = rc.Height - 4;
    }

    // Draw the Flat Arrow Box when not highlighted
    private void DrawNormalArrow(ref Graphics g, bool disable)
    {
        if ((int)BorderStyle == (int)TipiBordi.FlatXP)
        {
            int left = default(int), top = default(int), arrowWidth = default(int), height = default(int);
            ArrowBoxPosition(ref left, ref top, ref arrowWidth, ref height);

            Brush stripeColorBrush = new SolidBrush(SystemColors.Control);
            int Larghezza = SystemInformation.VerticalScrollBarWidth - arrowWidth;
            if (Enabled)
            {
                Brush b = new SolidBrush(SystemColors.Control);
                g.FillRectangle(b, new Rectangle(left - Larghezza, top - 2, SystemInformation.VerticalScrollBarWidth, height + 4));
            }

            if (Enabled)
            {
                var p = new Pen(m_NormalBorderColor);
                g.DrawLine(p, new Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Top), new Point(ClientRectangle.Right, ClientRectangle.Top));
                g.DrawLine(p, new Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Bottom - 1), new Point(ClientRectangle.Right, ClientRectangle.Bottom - 1));

                if (!disable)
                {
                    DrawHighlightedArrow(ref g, true);
                    g.FillRectangle(stripeColorBrush, left, top - 1, arrowWidth + 1, height + 2);
                }
                else
                    g.FillRectangle(stripeColorBrush, left - 4, top - 1, arrowWidth + 5, height + 2);

                DrawArrow(g, false);
            }
            else
            {
                var p = new Pen(SystemColors.InactiveBorder);
                g.DrawLine(p, new Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Top), new Point(ClientRectangle.Right, ClientRectangle.Top));
                g.DrawLine(p, new Point(ClientRectangle.Right - SystemInformation.VerticalScrollBarWidth - 2, ClientRectangle.Bottom - 1), new Point(ClientRectangle.Right, ClientRectangle.Bottom - 1));

                // Now draw the unselected background
                g.FillRectangle(stripeColorBrush, left - 5, top - 1, arrowWidth + 6, height + 2);

                DrawArrow(g, true);
            }
            Highlighted = false;
        }
    }

    // Draw the Flat Arrow Box when highlighted
    private void DrawHighlightedArrow(ref Graphics g, bool Delete)
    {
        if ((int)BorderStyle == (int)TipiBordi.FlatXP)
        {
            int left = default(int), top = default(int), arrowWidth = default(int), height = default(int);
            ArrowBoxPosition(ref left, ref top, ref arrowWidth, ref height);

            if (Enabled)
            {
                int comboTextWidth = SystemInformation.VerticalScrollBarWidth - arrowWidth;
                if (comboTextWidth < 0)
                    comboTextWidth = 1;
                Brush b = new SolidBrush(HighlightBorderColor);
            }

            if (!Delete)
            {
                if (DroppedDown)
                {
                    var cbg = CreateGraphics();
                    Brush pressedColorBrush = new SolidBrush(m_DropDownArrowBackColor);
                    int Larghezza = SystemInformation.VerticalScrollBarWidth - arrowWidth;
                    cbg.FillRectangle(pressedColorBrush, new Rectangle(left - Larghezza, top - 1, SystemInformation.VerticalScrollBarWidth + 1, height + 2));
                    var p = new Pen(HighlightBorderColor);
                    cbg.DrawRectangle(p, left - Larghezza - 1, top - 2, SystemInformation.VerticalScrollBarWidth + 2, height + 4);
                    DrawArrow(cbg, false);
                    cbg.Dispose();
                    return;
                }
                else if (Enabled)
                {
                    Brush b = new SolidBrush(m_DropDownBackColor);
                    int Larghezza = SystemInformation.VerticalScrollBarWidth - arrowWidth;
                    g.FillRectangle(b, new Rectangle(left - Larghezza, top - 1, SystemInformation.VerticalScrollBarWidth + 1, height + 2));

                    var pencolor = customBorderColor;
                    if (pencolor.Equals(Color.Empty))
                        pencolor = BackColor;
                }
            }
            else
            {
                Brush b = new SolidBrush(BackColor);
                g.FillRectangle(b, left - 1, top - 1, arrowWidth + 2, height + 2);
            }
            if (Enabled)
                DrawArrow(g, false);
            Highlighted = true;
        }
    }

    private void DrawArrow(Graphics g, bool Disable)
    {
        if ((int)BorderStyle == (int)TipiBordi.FlatXP)
        {
            int left = default(int), top = default(int), arrowWidth = default(int), height = default(int);
            ArrowBoxPosition(ref left, ref top, ref arrowWidth, ref height);

            int extra = 1;
            if (bUsingLargeFont)
                extra = 2;

            // triangle vertex of the arrow
            var pts = new Point[3];
            pts[0] = new Point(Conversions.ToInteger(left + arrowWidth / (double)2 - 2 - extra - 2), Conversions.ToInteger(top + height / (double)2 - 1));
            pts[1] = new Point(Conversions.ToInteger(left + arrowWidth / (double)2 + 3 + extra - 1), Conversions.ToInteger(top + height / (double)2 - 1));
            pts[2] = new Point(Conversions.ToInteger(left + arrowWidth / (double)2 - 1), Conversions.ToInteger(top + height / (double)2 - 1 + 3 + extra));

            // draw the arrow as a polygon
            if (Disable)
            {
                Brush b = new SolidBrush(arrowDisableColor);
                g.FillPolygon(b, pts);
            }
            else
            {
                Brush b = new SolidBrush(arrowColor);
                g.FillPolygon(b, pts);
            }
        }
    }

    private void DrawBorder(Graphics g, Color DrawColor)
    {
        if ((int)BorderStyle == (int)TipiBordi.FlatXP)
        {
            g.DrawRectangle(new Pen(BackColor, 1), ClientRectangle.Left + 1, ClientRectangle.Top + 1, ClientRectangle.Width - 1, ClientRectangle.Height - 3);

            // Draw the Border
            if (Enabled == false)
                DrawColor = SystemColors.InactiveBorder;

            var pen = new Pen(DrawColor, 1);
            // Border Rectangle
            g.DrawRectangle(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            // Button Rectangle
            g.DrawRectangle(pen, ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - 3, ClientRectangle.Height - 1);
        }
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if ((int)BorderStyle == (int)TipiBordi.FlatXP)
        {
            switch (m.Msg)
            {
                case 0xF:
                case 0x133:
                    {
                        // WM_PAINT

                        // We have to find if the Mouse is Over the combo
                        bool mouseIsOver;
                        var mousePosition = MousePosition;
                        mousePosition = PointToClient(mousePosition);
                        mouseIsOver = ClientRectangle.Contains(mousePosition);

                        if (HighlightBorderOnMouseEvents && (mouseIsOver || Focused))
                        {
                            var g = Graphics.FromHwnd(Handle);

                            DrawBorder(g, HighlightBorderColor);
                            DrawHighlightedArrow(ref g, false);
                        }
                        else
                        {
                            var g = Graphics.FromHwnd(Handle);

                            DrawBorder(g, m_NormalBorderColor);
                            DrawNormalArrow(ref g, true);
                        }

                        break;
                    }

                case 0x2A3:
                    {
                        // WM_MOUSELEAVE
                        if (Focused)
                            return;
                        if (currentColor.Equals(m_HighlightBorderColor))
                        {
                            bool mouseIsOver;
                            var mousePosition = MousePosition;
                            mousePosition = PointToClient(mousePosition);
                            mouseIsOver = ClientRectangle.Contains(mousePosition);

                            if (!mouseIsOver)
                            {
                                var g = Graphics.FromHwnd(Handle);
                                DrawBorder(g, m_NormalBorderColor);
                                DrawNormalArrow(ref g, true);
                                g.Dispose();
                            }
                        }

                        break;
                    }

                case 0x200:
                    {
                        // WM_MOUSEMOVE
                        if (HighlightBorderOnMouseEvents == true && !Highlighted)
                        {
                            currentColor = HighlightBorderColor;
                            var g = Graphics.FromHwnd(Handle);
                            DrawBorder(g, currentColor);
                            DrawHighlightedArrow(ref g, false);
                            g.Dispose();
                        }

                        break;
                    }

                case 0x46:
                    {
                        // WM_WINDOWPOSCHANGING 
                        if ((int)BorderStyle == (int)TipiBordi.FlatXP)
                        {
                            // Repaint the arrow when pressed
                            if (HighlightBorderOnMouseEvents)
                            {
                                var g = Graphics.FromHwnd(Handle);
                                Brush pressedColorBrush = new SolidBrush(m_DropDownBackColor);
                                int Larghezza = SystemInformation.VerticalScrollBarWidth - arrowWidth;
                                g.FillRectangle(pressedColorBrush, new Rectangle(Left - Larghezza, Top - 1, SystemInformation.VerticalScrollBarWidth + 1, Height + 2));
                                var p = new Pen(HighlightBorderColor);
                                g.DrawRectangle(p, Left - Larghezza - 1, Top - 2, SystemInformation.VerticalScrollBarWidth + 2, Height + 4);
                                DrawArrow(g, false);
                                g.Dispose();
                                Invalidate();
                            }
                        }

                        break;
                    }

                default:
                    {
                        break;
                        break;
                    }
            }
        }
    }

    // Custom painting of the DropDownList
    private void mComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
        var g = e.Graphics;
        var r = e.Bounds;
        SolidBrush b;
        int i;
        if (e.Index >= 0)
        {
            var rd = r;
            rd.Width = rd.Left + 100;
            b = new SolidBrush((Color)sender.ForeColor);
            g.FillRectangle(b, rd);
            var sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            // ******************* WINDOWS 98 **********************
            if ((int)e.State == (int)DrawItemState.Selected)
            {
                // item selected
                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), r);
                switch (ColumnNum)
                {
                    case 1:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                                if (m_GridLineHorizontal)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            }

                            break;
                        }

                    case 2:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 3:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol3) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) + 1, r.Height);
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col3.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2)), (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[2]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2), rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 4:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol3) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) + 1, r.Height);
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col3.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2)), (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[2]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2), rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol4) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) - Conversions.ToInteger(wcol3) + 1, r.Height);
                                if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col4.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3)), (float)rd.Y, sf);
                                else if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[3]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3), rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }
                }
                if ((int)BorderStyle == (int)TipiBordi.FlatXP)
                {
                    // Use the border color to highlight the selected item 
                    if (GridLineHorizontal)
                        e.Graphics.DrawRectangle(new Pen(HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 2);
                    else
                        e.Graphics.DrawRectangle(new Pen(HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 1);
                }
                e.DrawFocusRectangle();
            }
            else if ((int)e.State == (int)(DrawItemState.NoAccelerator | DrawItemState.Selected) | (int)e.State == (int)(DrawItemState.Selected | DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
            {
                // item selected
                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), r);
                switch (ColumnNum)
                {
                    case 1:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                                if (m_GridLineHorizontal)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            }

                            break;
                        }

                    case 2:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 3:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol3) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[2]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col3.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2)), (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 4:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush(DropDownForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol3) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[2]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col3.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2)), (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol4) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush(DropDownBackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) - Conversions.ToInteger(wcol3) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[3]]).ToString(), Font, new SolidBrush(DropDownForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col4.ToString(), Font, new SolidBrush(DropDownForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3)), (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }
                }
                if ((int)BorderStyle == (int)TipiBordi.FlatXP)
                {
                    // Use the border color to highlight the selected item 
                    if (GridLineHorizontal)
                        e.Graphics.DrawRectangle(new Pen(HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 2);
                    else
                        e.Graphics.DrawRectangle(new Pen(HighlightBorderColor, 1), r.X, r.Y, r.Width - 1, r.Height - 1);
                }
                e.DrawFocusRectangle();
            }
            else
            {
                // items NOT selected
                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), r);
                if ((int)BorderStyle == (int)TipiBordi.FlatXP)
                {
                    if (GridLineHorizontal)
                        e.Graphics.DrawRectangle(new Pen(BackColor, 1), r.X, r.Y, r.Width, r.Height - 1);
                    else
                        e.Graphics.DrawRectangle(new Pen(BackColor, 1), r.X, r.Y, r.Width, r.Height);
                }
                switch (ColumnNum)
                {
                    case 1:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 2:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 3:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol3) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[2]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col3.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2)), (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }

                    case 4:
                        {
                            if (Conversions.ToDouble(wcol1) > 0)
                            {
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[0]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X, rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col1.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)rd.X, (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol2) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), rd.X + Conversions.ToInteger(wcol1) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[1]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X + Conversions.ToInteger(wcol1), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col2.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)(rd.X + Conversions.ToInteger(wcol1)), (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol3) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[2]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col3.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2)), (float)rd.Y, sf);
                            }
                            if (Conversions.ToDouble(wcol4) > 0)
                            {
                                if (m_GridLineVertical)
                                    e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 2, rd.Y, rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 2, rd.Y + 15);
                                e.Graphics.FillRectangle(new SolidBrush((Color)sender.BackColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3) - 1, rd.Y, r.Width - Conversions.ToInteger(wcol1) - Conversions.ToInteger(wcol2) - Conversions.ToInteger(wcol3) + 1, r.Height);
                                if ((int)LoadingType == (int)CaricamentoCombo.DataTable)
                                    e.Graphics.DrawString(Assegna(m_DataTable.Rows[e.Index][Indice[3]]).ToString(), Font, new SolidBrush((Color)sender.ForeColor), rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3), rd.Y, sf);
                                else if (LoadingType == (int)CaricamentoCombo.ComboBoxItem)
                                    e.Graphics.DrawString(Items[e.Index].Col4.ToString(), Font, new SolidBrush((Color)sender.ForeColor), (float)(rd.X + Conversions.ToInteger(wcol1) + Conversions.ToInteger(wcol2) + Conversions.ToInteger(wcol3)), (float)rd.Y, sf);
                            }
                            if (m_GridLineHorizontal)
                                e.Graphics.DrawLine(new Pen(GridLineColor, 1), rd.X, rd.Y + rd.Height - 1, rd.X + DropDownWidth, rd.Y + rd.Height - 1);
                            break;
                        }
                }
                e.DrawFocusRectangle();
            }
        }
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        // AUTOCOMPLETE: we have to know when a key has been really pressed

        if (DropDownStyle == (int)CustomDropDownStyle.DropDown)
            PressedKey = true;
        else
        {
            // ReadOnly AutoComplete Management
            string sTypedText;
            int iFoundIndex;
            string currentText;
            int Start, selLength;

            if (Strings.Asc(e.KeyChar) == 8)
            {
                if ((SelectedText ?? "") == (Text ?? ""))
                {
                    PressedKey = true;
                    return;
                }
            }
            if (SelectionLength > 0)
            {
                Start = SelectionStart;
                selLength = SelectionLength;

                // This is equivalent to Me.Text, but sometimes using Me.Text it doesn't work
                currentText = AccessibilityObject.Value;

                currentText = currentText.Remove(Start, selLength);
                currentText = currentText.Insert(Start, Conversions.ToString(e.KeyChar));
                sTypedText = currentText;
            }
            else
            {
                Start = SelectionStart;
                sTypedText = Text.Insert(Start, Conversions.ToString(e.KeyChar));
            }
            iFoundIndex = FindString(sTypedText);
            if (iFoundIndex >= 0)
                PressedKey = true;
            else
                e.Handled = true;
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if ((int)DropDownStyle == (int)CustomDropDownStyle.DropDownList && (int)e.KeyCode == (int)Keys.Delete)
        {
            if ((Text ?? "") != (SelectedText ?? ""))
                e.Handled = true;
        }

        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        // AUTOCOMPLETING

        // WARNING: With VB.Net 2003 there is a strange behaviour. This event is raised not just when any key is pressed
        // but also when the Me.Text property changes. Particularly, it happens when you write in a fast way (for example you
        // you press 2 keys and the event is raised 3 times). To manage this we have added a boolean variable PressedKey that
        // is set to true in the OnKeyPress Event

        string sTypedText;
        int iFoundIndex;
        object oFoundItem;
        string sFoundText;
        string sAppendText;

        if (PressedKey)
        {
            // Ignoring alphanumeric chars
            switch (e.KeyCode)
            {
                case Keys.Back:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Delete:
                case Keys.Down:
                case Keys.End:
                case Keys.Home:
                    {
                        return;
                    }
            }

            // Get the Typed Text and Find it in the list
            sTypedText = Text;
            iFoundIndex = FindString(sTypedText);

            // If we found the Typed Text in the list then Autocomplete
            if (iFoundIndex >= 0)
            {

                // Get the Item from the list (Return Type depends if Datasource was bound 
                // or List Created)
                oFoundItem = Items[iFoundIndex];

                // Use the ListControl.GetItemText to resolve the Name in case the Combo 
                // was Data bound
                sFoundText = GetItemText(oFoundItem);

                // Append then found text to the typed text to preserve case
                sAppendText = sFoundText.Substring(sTypedText.Length);
                Text = sTypedText + sAppendText;

                // Select the Appended Text
                SelectionStart = sTypedText.Length;
                SelectionLength = sAppendText.Length;

                if ((int)e.KeyCode == (int)Keys.Enter)
                {
                    iFoundIndex = FindStringExact(Text);
                    SelectedIndex = iFoundIndex;
                    SendKeys.Send(Constants.vbTab);
                    e.Handled = true;
                }
            }
        }
        PressedKey = false;
    }

    protected override void OnLeave(EventArgs e)
    {
        // Selecting the item which text is showed in the text area of the ComboBox
        int iFoundIndex;
        // The Me.AccessibilityObject.Value is used instead of Me.Text to manage
        // the event when you write in the combobox text and the DropDownList
        // is open. In this case, if you click outside the combo, Me.Text mantains 
        // the old value and not the current one
        iFoundIndex = FindStringExact(AccessibilityObject.Value);
        SelectedIndex = iFoundIndex;
    }

    protected override void OnCreateControl()
    {
        DisplayMember = "Text";
        DrawMode = DrawMode.OwnerDrawFixed;
        currentColor = m_NormalBorderColor;
        Invalidate();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        if (ManagingFastMouseMoving)
        {
            myTimer.Interval = m_ManagingFastMouseMovingInterval;
            myTimer.Start();
        }
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        var m = default(Message);
        m.Msg = 0xF;
        if (Enabled)
            currentColor = NormalBorderColor;
        else
            currentColor = SystemColors.InactiveBorder;
        // Generate a PAINT event
        WndProc(ref m);
    }


    // Override mouse and focus events to draw
    // proper borders. Basically, set the color and Invalidate(),
    // In general, Invalidate causes a control to redraw itself.
    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        if (HighlightBorderOnMouseEvents == true && !Highlighted)
        {
            currentColor = HighlightBorderColor;
            var g = Graphics.FromHwnd(Handle);
            DrawBorder(g, currentColor);
            DrawHighlightedArrow(ref g, false);
            g.Dispose();
        }
        MouseOver = true;
    }

    protected override void OnMouseHover(EventArgs e)
    {
        base.OnMouseHover(e);
        MouseOver = true;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        if (Focused)
            return;
        if (HighlightBorderOnMouseEvents == true && Highlighted)
        {
            var g = Graphics.FromHwnd(Handle);
            DrawBorder(g, m_NormalBorderColor);
            DrawNormalArrow(ref g, true);
            g.Dispose();
        }
        MouseOver = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        MouseOver = true;
    }

    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        if (HighlightBorderOnMouseEvents == true && Highlighted)
        {
            currentColor = NormalBorderColor;
            var g = Graphics.FromHwnd(Handle);
            DrawBorder(g, m_NormalBorderColor);
            DrawNormalArrow(ref g, true);
            g.Dispose();
        }
    }

    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        if (HighlightBorderOnMouseEvents == true && !Highlighted)
        {
            currentColor = HighlightBorderColor;
            var g = Graphics.FromHwnd(Handle);
            DrawBorder(g, currentColor);
            DrawHighlightedArrow(ref g, false);
            g.Dispose();
        }
    }
}



public class MTGCComboBoxItem : ListViewItem, IComparable
{

    // each of the below public declarations will be "visible" to the outside
    // You may add as many of these declarations using whatever types you desire
    public string Col1;
    public string Col2;
    public string Col3;
    public string Col4;

    // every value of MyInfo you want to store, get's added to the NEW declaration 
    public MTGCComboBoxItem(string C1, string C2 = "", string C3 = "", string C4 = "") : base()
    {
        // transfer all incoming parameters to your local storage
        Col1 = C1;
        Col2 = C2;
        Col3 = C3;
        Col4 = C4;
        // and finally, pass back the Text property
        Text = C1;
    }

    // Function used to sort the items on first element Col1
    private int CompareTo(object obj)
    {
        // every not nothing object is greater than nothing
        if (obj == null)
            return 1;

        // this is used to take care of late binding
        var other = (MTGCComboBoxItem)obj;

        // comparing strings
        return Strings.StrComp(Col1, other.Col1, CompareMethod.Text);
    }
}


