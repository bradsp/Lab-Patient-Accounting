using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Core
{
    #region InputBox return result

    /// <summary>
    /// Class used to store the result of an InputBox.Show message.
    /// </summary>
    public class InputBoxResult
    {
        public DialogResult ReturnCode;
        public string Text;
    }

    #endregion

    /// <summary>
    /// Summary description for InputBox.
    /// </summary>
    public sealed class InputBox
    {

        #region Private Windows Contols and Constructor

        // Create a new instance of the form.
        private static Form frmInputDialog;
        private static Label lblPrompt;
        private static Button btnOK;
        private static Button btnCancel;
        private static TextBox txtInput;
        private static ContextMenuStrip contextMenu;

        public InputBox()
        {
        }

        #endregion

        #region Private Variables

        private static string _formCaption = string.Empty;
        private static string _formPrompt = string.Empty;
        private static InputBoxResult _outputResponse = new InputBoxResult();
        private static string _defaultValue = string.Empty;
        private static int _xPos = -1;
        private static int _yPos = -1;
        private static bool _multiline = false;
        private static bool _passwordMask = false;

        #endregion

        #region Properties

        public int InputBoxHeight { get; set; } = 20;

        #endregion

        #region Windows Form code

        private static void InitializeComponent()
        {
            // Create a new instance of the form.
            frmInputDialog = new Form();
            lblPrompt = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            txtInput = new TextBox();
            contextMenu = new ContextMenuStrip();
            frmInputDialog.SuspendLayout();
            //
            // contextMenu
            //
            ToolStripMenuItem pasteToolStripItem = new ToolStripMenuItem("Paste", null, pasteToolStripItem_click);
            contextMenu.Items.Add(pasteToolStripItem);
            // 
            // lblPrompt
            // 
            lblPrompt.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            lblPrompt.BackColor = SystemColors.Control;
            lblPrompt.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            lblPrompt.Location = new Point(12, 9);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(302, 82);
            lblPrompt.TabIndex = 3;
            // 
            // btnOK
            // 
            btnOK.DialogResult = DialogResult.OK;
            btnOK.FlatStyle = FlatStyle.Popup;
            btnOK.Location = new Point(326, 8);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(64, 24);
            btnOK.TabIndex = 1;
            btnOK.Text = "&OK";
            btnOK.Click += new EventHandler(btnOK_Click);
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Location = new Point(326, 40);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(64, 24);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // txtInput
            // 
            txtInput.Location = new Point(8, 30);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(280, 20);
            txtInput.TabIndex = 0;
            txtInput.Text = "";
            txtInput.ContextMenuStrip = contextMenu;
            // 
            // InputBoxDialog
            // 
            frmInputDialog.AutoScaleBaseSize = new Size(5, 13);
            frmInputDialog.ClientSize = new Size(398, 80);
            frmInputDialog.Controls.Add(txtInput);
            frmInputDialog.Controls.Add(btnCancel);
            frmInputDialog.Controls.Add(btnOK);
            frmInputDialog.Controls.Add(lblPrompt);
            frmInputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            frmInputDialog.MaximizeBox = false;
            frmInputDialog.MinimizeBox = false;
            frmInputDialog.Name = "InputBoxDialog";
            frmInputDialog.ResumeLayout(false);
        }

        private static void cutToolStripItem_click(object sender, EventArgs e)
        {
            // Ensure that text is currently selected in the text box.   
            if (txtInput.SelectedText != "")
                // Cut the selected text in the control and paste it into the Clipboard.
                txtInput.Cut();
        }

        private static void pasteToolStripItem_click(object sender, EventArgs e)
        {
            // Determine if there is any text in the Clipboard to paste into the text box.
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                // Determine if any text is selected in the text box.
                if (txtInput.SelectionLength > 0)
                {
                    // Ask user if they want to paste over currently selected text.
                    if (MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) == DialogResult.No)
                        // Move selection to the point after the current selection and paste.
                        txtInput.SelectionStart = txtInput.SelectionStart + txtInput.SelectionLength;
                }
                // Paste current text in Clipboard into text box.
                txtInput.Paste();
            }
        }

        private static void copyToolStripItem_click(object sender, EventArgs e)
        {
            // Ensure that text is selected in the text box.   
            if (txtInput.SelectionLength > 0)
                // Copy the selected text to the Clipboard.
                txtInput.Copy();
        }

        #endregion

        #region Private function, InputBox Form move and change size

        static private void LoadForm()
        {

            OutputResponse.ReturnCode = DialogResult.Ignore;
            OutputResponse.Text = string.Empty;

            txtInput.Text = _defaultValue;
            lblPrompt.Text = _formPrompt;
            frmInputDialog.Text = _formCaption;

            // Retrieve the working rectangle from the Screen class
            // using the PrimaryScreen and the WorkingArea properties.
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            if ((_xPos >= 0 && _xPos < workingRectangle.Width - 100) && (_yPos >= 0 && _yPos < workingRectangle.Height - 100))
            {
                frmInputDialog.StartPosition = FormStartPosition.Manual;
                frmInputDialog.Location = new System.Drawing.Point(_xPos, _yPos);
            }
            else
                frmInputDialog.StartPosition = FormStartPosition.CenterScreen;


            string PrompText = lblPrompt.Text;

            int n = 0;
            int Index = 0;
            while (PrompText.IndexOf("\n", Index) > -1)
            {
                Index = PrompText.IndexOf("\n", Index) + 1;
                n++;
            }

            if (n == 0)
                n = 1;

            System.Drawing.Point Txt = txtInput.Location;
            Txt.Y += (n * 4);
            txtInput.Location = Txt;
            System.Drawing.Size form = frmInputDialog.Size;
            form.Height += (n * 4);
            frmInputDialog.Size = form;

            txtInput.SelectionStart = 0;
            txtInput.SelectionLength = txtInput.Text.Length;
            if (_passwordMask)
                txtInput.PasswordChar = '*';
            if (_multiline)
            {
                txtInput.Multiline = true;
                txtInput.Height += 60;
                frmInputDialog.Height += 60;
            }
            txtInput.Focus();
        }

        #endregion

        #region Button control click event

        static private void btnOK_Click(object sender, System.EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.OK;
            OutputResponse.Text = txtInput.Text;
            frmInputDialog.Dispose();
        }

        static private void btnCancel_Click(object sender, System.EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.Cancel;
            OutputResponse.Text = string.Empty; //Clean output response
            frmInputDialog.Dispose();
        }

        #endregion

        #region Public Static Show functions

        static public InputBoxResult Show(string Prompt, bool multiline = false)
        {
            InitializeComponent();
            Multiline = multiline;
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult ShowPassword(string Prompt, string Title, bool multiline = false)
        {
            InitializeComponent();
            Multiline = multiline;
            FormCaption = Title;
            FormPrompt = Prompt;
            PasswordMask = true;

            //Display the form as a modal dialog box
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult Show(string Prompt, string Title, bool multiline = false)
        {
            InitializeComponent();
            Multiline = multiline;
            FormCaption = Title;
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult Show(string Prompt, string Title, string Default, bool multiline = false)
        {
            InitializeComponent();
            Multiline = multiline;
            FormCaption = Title;
            FormPrompt = Prompt;
            DefaultValue = Default;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult Show(string Prompt, string Title, string Default, int XPos, int YPos, bool multiline = false)
        {
            InitializeComponent();
            Multiline = multiline;
            FormCaption = Title;
            FormPrompt = Prompt;
            DefaultValue = Default;
            XPosition = XPos;
            YPosition = YPos;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        #endregion

        #region Private Properties

        static private string FormCaption
        {
            set
            {
                _formCaption = value;
            }
        } // property FormCaption

        static private string FormPrompt
        {
            set
            {
                _formPrompt = value;
            }
        } // property FormPrompt

        static private InputBoxResult OutputResponse
        {
            get
            {
                return _outputResponse;
            }
            set
            {
                _outputResponse = value;
            }
        } // property InputResponse

        static private string DefaultValue
        {
            set
            {
                _defaultValue = value;
            }
        } // property DefaultValue

        static private int XPosition
        {
            set
            {
                if (value >= 0)
                    _xPos = value;
            }
        } // property XPos

        static private int YPosition
        {
            set
            {
                if (value >= 0)
                    _yPos = value;
            }
        } // property YPos

        static private bool PasswordMask
        {
            set
            {
                _passwordMask = value;
            }
        }

        static private bool Multiline
        {
            set
            {
                _multiline = value;
            }
        }

        #endregion
    }
}
