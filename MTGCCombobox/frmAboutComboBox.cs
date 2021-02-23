using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

public class frmAboutComboBox : Form
{
    public frmAboutComboBox() : base()
    {

        // Chiamata richiesta da Progettazione Windows Form.
        InitializeComponent();
    }

    // Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    protected new override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!(components == null))
                components.Dispose();
        }
        base.Dispose(disposing);
    }

    // Richiesto da Progettazione Windows Form
    private System.ComponentModel.IContainer components;

    // NOTA: la procedura che segue è richiesta da Progettazione Windows Form.
    // Può essere modificata in Progettazione Windows Form.  
    // Non modificarla nell'editor del codice.
    private LinkLabel _lblMTGC;

    internal LinkLabel lblMTGC
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _lblMTGC;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_lblMTGC != null)
            {
                _lblMTGC.LinkClicked -= lblMTGC_LinkClicked;
            }

            _lblMTGC = value;
            if (_lblMTGC != null)
            {
                _lblMTGC.LinkClicked += lblMTGC_LinkClicked;
            }
        }
    }

    private PictureBox _pct;

    internal PictureBox pct
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _pct;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_pct != null)
            {
            }

            _pct = value;
            if (_pct != null)
            {
            }
        }
    }

    private Label _lblVersione;

    internal Label lblVersione
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _lblVersione;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_lblVersione != null)
            {
            }

            _lblVersione = value;
            if (_lblVersione != null)
            {
            }
        }
    }

    private Label _lblFramework;

    internal Label lblFramework
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _lblFramework;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_lblFramework != null)
            {
            }

            _lblFramework = value;
            if (_lblFramework != null)
            {
            }
        }
    }

    private Panel _Panel1;

    internal Panel Panel1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Panel1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Panel1 != null)
            {
            }

            _Panel1 = value;
            if (_Panel1 != null)
            {
            }
        }
    }

    private GroupBox _GroupBox1;

    internal GroupBox GroupBox1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _GroupBox1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_GroupBox1 != null)
            {
            }

            _GroupBox1 = value;
            if (_GroupBox1 != null)
            {
            }
        }
    }

    private Label _Label1;

    internal Label Label1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Label1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Label1 != null)
            {
            }

            _Label1 = value;
            if (_Label1 != null)
            {
            }
        }
    }

    private Label _Label2;

    internal Label Label2
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Label2;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Label2 != null)
            {
            }

            _Label2 = value;
            if (_Label2 != null)
            {
            }
        }
    }

    private Label _Label3;

    internal Label Label3
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Label3;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Label3 != null)
            {
            }

            _Label3 = value;
            if (_Label3 != null)
            {
            }
        }
    }

    [DebuggerStepThrough()]
    private void InitializeComponent()
    {
        var resources = new System.Resources.ResourceManager(typeof(frmAboutComboBox));
        _pct = new PictureBox();
        _lblMTGC = new LinkLabel();
        _lblMTGC.LinkClicked += lblMTGC_LinkClicked;
        _lblVersione = new Label();
        _lblFramework = new Label();
        _Panel1 = new Panel();
        _GroupBox1 = new GroupBox();
        _Label1 = new Label();
        _Label2 = new Label();
        _Label3 = new Label();
        _Panel1.SuspendLayout();
        _GroupBox1.SuspendLayout();
        SuspendLayout();
        // 
        // pct
        // 
        _pct.Image = (Image)resources.GetObject("pct.Image");
        _pct.Location = new Point(56, 8);
        _pct.Name = "pct";
        _pct.Size = new Size(185, 35);
        _pct.TabIndex = 0;
        _pct.TabStop = false;
        // 
        // lblMTGC
        // 
        _lblMTGC.AutoSize = true;
        _lblMTGC.Font = new Font("Verdana", 8.25F);
        _lblMTGC.Location = new Point(80, 66);
        _lblMTGC.Name = "lblMTGC";
        _lblMTGC.Size = new Size(126, 17);
        _lblMTGC.TabIndex = 1;
        _lblMTGC.TabStop = true;
        _lblMTGC.Text = "http://www.mtgc.net";
        // 
        // lblVersione
        // 
        _lblVersione.AutoSize = true;
        _lblVersione.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
        _lblVersione.Location = new Point(8, 56);
        _lblVersione.Name = "lblVersione";
        _lblVersione.Size = new Size(215, 17);
        _lblVersione.TabIndex = 2;
        _lblVersione.Text = "MTGCComboBox for .NET, Version 1.0.0.0";
        // 
        // lblFramework
        // 
        _lblFramework.AutoSize = true;
        _lblFramework.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
        _lblFramework.Location = new Point(8, 72);
        _lblFramework.Name = "lblFramework";
        _lblFramework.Size = new Size(140, 17);
        _lblFramework.TabIndex = 3;
        _lblFramework.Text = "[.NET Framework, Version]";
        // 
        // Panel1
        // 
        _Panel1.BackColor = Color.FromArgb(Conversions.ToByte(215), Conversions.ToByte(222), Conversions.ToByte(238));
        _Panel1.Controls.Add(_pct);
        _Panel1.Location = new Point(0, 0);
        _Panel1.Name = "Panel1";
        _Panel1.Size = new Size(304, 48);
        _Panel1.TabIndex = 4;
        // 
        // GroupBox1
        // 
        _GroupBox1.Controls.Add(_Label3);
        _GroupBox1.Controls.Add(_Label2);
        _GroupBox1.Controls.Add(_Label1);
        _GroupBox1.Controls.Add(_lblMTGC);
        _GroupBox1.Location = new Point(8, 88);
        _GroupBox1.Name = "GroupBox1";
        _GroupBox1.Size = new Size(280, 88);
        _GroupBox1.TabIndex = 5;
        _GroupBox1.TabStop = false;
        // 
        // Label1
        // 
        _Label1.AutoSize = true;
        _Label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
        _Label1.Location = new Point(8, 14);
        _Label1.Name = "Label1";
        _Label1.Size = new Size(242, 17);
        _Label1.TabIndex = 3;
        _Label1.Text = "MT Global Consulting Srl - Via Roccasparvera, 4";
        // 
        // Label2
        // 
        _Label2.AutoSize = true;
        _Label2.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
        _Label2.Location = new Point(8, 29);
        _Label2.Name = "Label2";
        _Label2.Size = new Size(189, 17);
        _Label2.TabIndex = 4;
        _Label2.Text = "12010 - S.Rocco Castagnaretta  (CN)";
        // 
        // Label3
        // 
        _Label3.AutoSize = true;
        _Label3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
        _Label3.Location = new Point(8, 45);
        _Label3.Name = "Label3";
        _Label3.Size = new Size(241, 17);
        _Label3.TabIndex = 5;
        _Label3.Text = "Tel. +39.0171.491274 - Fax. +39.0171.494516";
        // 
        // frmAboutComboBox
        // 
        AutoScaleBaseSize = new Size(5, 13);
        BackColor = Color.FromArgb(Conversions.ToByte(235), Conversions.ToByte(192), Conversions.ToByte(44));
        ClientSize = new Size(298, 184);
        Controls.Add(_GroupBox1);
        Controls.Add(_Panel1);
        Controls.Add(_lblFramework);
        Controls.Add(_lblVersione);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAboutComboBox";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "About: MTGCComboBox";
        base.Load += frmAboutComboBox_Load;
        _Panel1.ResumeLayout(false);
        base.Load += frmAboutComboBox_Load;
        _GroupBox1.ResumeLayout(false);
        base.Load += frmAboutComboBox_Load;
        ResumeLayout(false);
    }


    private void lblMTGC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("http://www.mtgc.net");
    }

    private void frmAboutComboBox_Load(object sender, EventArgs e)
    {
        lblVersione.Text = "MTGCComboBox for .NET, Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        lblFramework.Text = "[.NET Framework, Version & " + System.Reflection.Assembly.GetExecutingAssembly().ImageRuntimeVersion.ToString() + "]";
    }
}

