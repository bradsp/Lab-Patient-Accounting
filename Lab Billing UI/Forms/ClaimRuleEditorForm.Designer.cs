namespace LabBilling.Forms
{
    partial class ClaimRuleEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.tvRuleHierarchy = new System.Windows.Forms.TreeView();
            this.menuTVRuleHierarchy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripAddCriteria = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDeleteCriteria = new System.Windows.Forms.ToolStripMenuItem();
            this.listRules = new MetroFramework.Controls.MetroListView();
            this.tbRuleDescription = new MetroFramework.Controls.MetroTextBox();
            this.tbErrorText = new MetroFramework.Controls.MetroTextBox();
            this.tbRuleName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.effectiveDate = new MetroFramework.Controls.MetroDateTime();
            this.endEffectiveDate = new MetroFramework.Controls.MetroDateTime();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.cbLineType = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.tbTargetValue = new MetroFramework.Controls.MetroTextBox();
            this.cbOperator = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel9 = new MetroFramework.Controls.MetroLabel();
            this.addRuleButton = new MetroFramework.Controls.MetroButton();
            this.saveRuleButton = new MetroFramework.Controls.MetroButton();
            this.cbMemberName = new MetroFramework.Controls.MetroComboBox();
            this.comboBoxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDetailButton = new MetroFramework.Controls.MetroButton();
            this.removeDetailButton = new MetroFramework.Controls.MetroButton();
            this.saveCriteraButton = new MetroFramework.Controls.MetroButton();
            this.menuTVRuleHierarchy.SuspendLayout();
            this.comboBoxMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvRuleHierarchy
            // 
            this.tvRuleHierarchy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvRuleHierarchy.ContextMenuStrip = this.menuTVRuleHierarchy;
            this.tvRuleHierarchy.Location = new System.Drawing.Point(229, 66);
            this.tvRuleHierarchy.Name = "tvRuleHierarchy";
            this.tvRuleHierarchy.Size = new System.Drawing.Size(296, 420);
            this.tvRuleHierarchy.TabIndex = 20;
            this.tvRuleHierarchy.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvRuleHierarchy_AfterSelect);
            // 
            // menuTVRuleHierarchy
            // 
            this.menuTVRuleHierarchy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddCriteria,
            this.toolStripAddGroup,
            this.toolStripDeleteCriteria});
            this.menuTVRuleHierarchy.Name = "contextMenuStrip1";
            this.menuTVRuleHierarchy.Size = new System.Drawing.Size(138, 70);
            // 
            // toolStripAddCriteria
            // 
            this.toolStripAddCriteria.Name = "toolStripAddCriteria";
            this.toolStripAddCriteria.Size = new System.Drawing.Size(137, 22);
            this.toolStripAddCriteria.Text = "Add Criteria";
            // 
            // toolStripAddGroup
            // 
            this.toolStripAddGroup.Name = "toolStripAddGroup";
            this.toolStripAddGroup.Size = new System.Drawing.Size(137, 22);
            this.toolStripAddGroup.Text = "Add Group";
            // 
            // toolStripDeleteCriteria
            // 
            this.toolStripDeleteCriteria.Name = "toolStripDeleteCriteria";
            this.toolStripDeleteCriteria.Size = new System.Drawing.Size(137, 22);
            this.toolStripDeleteCriteria.Text = "Delete Item";
            // 
            // listRules
            // 
            this.listRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listRules.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.listRules.FullRowSelect = true;
            this.listRules.Location = new System.Drawing.Point(18, 66);
            this.listRules.MultiSelect = false;
            this.listRules.Name = "listRules";
            this.listRules.OwnerDraw = true;
            this.listRules.Size = new System.Drawing.Size(205, 392);
            this.listRules.TabIndex = 19;
            this.listRules.UseCompatibleStateImageBehavior = false;
            this.listRules.UseSelectable = true;
            this.listRules.SelectedIndexChanged += new System.EventHandler(this.listRules_SelectedIndexChanged);
            // 
            // tbRuleDescription
            // 
            // 
            // 
            // 
            this.tbRuleDescription.CustomButton.Image = null;
            this.tbRuleDescription.CustomButton.Location = new System.Drawing.Point(293, 1);
            this.tbRuleDescription.CustomButton.Name = "";
            this.tbRuleDescription.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.tbRuleDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbRuleDescription.CustomButton.TabIndex = 1;
            this.tbRuleDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbRuleDescription.CustomButton.UseSelectable = true;
            this.tbRuleDescription.CustomButton.Visible = false;
            this.tbRuleDescription.Lines = new string[0];
            this.tbRuleDescription.Location = new System.Drawing.Point(618, 106);
            this.tbRuleDescription.MaxLength = 32767;
            this.tbRuleDescription.Name = "tbRuleDescription";
            this.tbRuleDescription.PasswordChar = '\0';
            this.tbRuleDescription.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbRuleDescription.SelectedText = "";
            this.tbRuleDescription.SelectionLength = 0;
            this.tbRuleDescription.SelectionStart = 0;
            this.tbRuleDescription.ShortcutsEnabled = true;
            this.tbRuleDescription.Size = new System.Drawing.Size(315, 23);
            this.tbRuleDescription.TabIndex = 3;
            this.tbRuleDescription.UseSelectable = true;
            this.tbRuleDescription.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbRuleDescription.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // tbErrorText
            // 
            // 
            // 
            // 
            this.tbErrorText.CustomButton.Image = null;
            this.tbErrorText.CustomButton.Location = new System.Drawing.Point(293, 1);
            this.tbErrorText.CustomButton.Name = "";
            this.tbErrorText.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.tbErrorText.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbErrorText.CustomButton.TabIndex = 1;
            this.tbErrorText.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbErrorText.CustomButton.UseSelectable = true;
            this.tbErrorText.CustomButton.Visible = false;
            this.tbErrorText.Lines = new string[0];
            this.tbErrorText.Location = new System.Drawing.Point(618, 135);
            this.tbErrorText.MaxLength = 32767;
            this.tbErrorText.Name = "tbErrorText";
            this.tbErrorText.PasswordChar = '\0';
            this.tbErrorText.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbErrorText.SelectedText = "";
            this.tbErrorText.SelectionLength = 0;
            this.tbErrorText.SelectionStart = 0;
            this.tbErrorText.ShortcutsEnabled = true;
            this.tbErrorText.Size = new System.Drawing.Size(315, 23);
            this.tbErrorText.TabIndex = 5;
            this.tbErrorText.UseSelectable = true;
            this.tbErrorText.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbErrorText.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // tbRuleName
            // 
            // 
            // 
            // 
            this.tbRuleName.CustomButton.Image = null;
            this.tbRuleName.CustomButton.Location = new System.Drawing.Point(293, 1);
            this.tbRuleName.CustomButton.Name = "";
            this.tbRuleName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.tbRuleName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbRuleName.CustomButton.TabIndex = 1;
            this.tbRuleName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbRuleName.CustomButton.UseSelectable = true;
            this.tbRuleName.CustomButton.Visible = false;
            this.tbRuleName.Lines = new string[0];
            this.tbRuleName.Location = new System.Drawing.Point(618, 77);
            this.tbRuleName.MaxLength = 32767;
            this.tbRuleName.Name = "tbRuleName";
            this.tbRuleName.PasswordChar = '\0';
            this.tbRuleName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbRuleName.SelectedText = "";
            this.tbRuleName.SelectionLength = 0;
            this.tbRuleName.SelectionStart = 0;
            this.tbRuleName.ShortcutsEnabled = true;
            this.tbRuleName.Size = new System.Drawing.Size(315, 23);
            this.tbRuleName.TabIndex = 1;
            this.tbRuleName.UseSelectable = true;
            this.tbRuleName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbRuleName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(531, 81);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(74, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Rule Name";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(531, 110);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(74, 19);
            this.metroLabel2.TabIndex = 2;
            this.metroLabel2.Text = "Description";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(540, 139);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(65, 19);
            this.metroLabel3.TabIndex = 4;
            this.metroLabel3.Text = "Error Text";
            // 
            // effectiveDate
            // 
            this.effectiveDate.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.effectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.effectiveDate.Location = new System.Drawing.Point(939, 77);
            this.effectiveDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.effectiveDate.Name = "effectiveDate";
            this.effectiveDate.Size = new System.Drawing.Size(114, 25);
            this.effectiveDate.TabIndex = 7;
            // 
            // endEffectiveDate
            // 
            this.endEffectiveDate.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.endEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endEffectiveDate.Location = new System.Drawing.Point(939, 134);
            this.endEffectiveDate.MinimumSize = new System.Drawing.Size(0, 25);
            this.endEffectiveDate.Name = "endEffectiveDate";
            this.endEffectiveDate.Size = new System.Drawing.Size(114, 25);
            this.endEffectiveDate.TabIndex = 9;
            this.endEffectiveDate.Value = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(939, 55);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(88, 19);
            this.metroLabel4.TabIndex = 6;
            this.metroLabel4.Text = "Effective Date";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(939, 112);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(114, 19);
            this.metroLabel5.TabIndex = 8;
            this.metroLabel5.Text = "End Effective Date";
            // 
            // cbLineType
            // 
            this.cbLineType.FormattingEnabled = true;
            this.cbLineType.ItemHeight = 23;
            this.cbLineType.Items.AddRange(new object[] {
            "Header",
            "Group",
            "Detail",
            "SubGroup"});
            this.cbLineType.Location = new System.Drawing.Point(540, 250);
            this.cbLineType.Name = "cbLineType";
            this.cbLineType.Size = new System.Drawing.Size(133, 29);
            this.cbLineType.TabIndex = 11;
            this.cbLineType.UseSelectable = true;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(540, 228);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(63, 19);
            this.metroLabel6.TabIndex = 10;
            this.metroLabel6.Text = "Line Type";
            // 
            // tbTargetValue
            // 
            // 
            // 
            // 
            this.tbTargetValue.CustomButton.Image = null;
            this.tbTargetValue.CustomButton.Location = new System.Drawing.Point(155, 1);
            this.tbTargetValue.CustomButton.Name = "";
            this.tbTargetValue.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.tbTargetValue.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbTargetValue.CustomButton.TabIndex = 1;
            this.tbTargetValue.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbTargetValue.CustomButton.UseSelectable = true;
            this.tbTargetValue.CustomButton.Visible = false;
            this.tbTargetValue.Lines = new string[0];
            this.tbTargetValue.Location = new System.Drawing.Point(891, 311);
            this.tbTargetValue.MaxLength = 32767;
            this.tbTargetValue.Name = "tbTargetValue";
            this.tbTargetValue.PasswordChar = '\0';
            this.tbTargetValue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbTargetValue.SelectedText = "";
            this.tbTargetValue.SelectionLength = 0;
            this.tbTargetValue.SelectionStart = 0;
            this.tbTargetValue.ShortcutsEnabled = true;
            this.tbTargetValue.Size = new System.Drawing.Size(177, 23);
            this.tbTargetValue.TabIndex = 17;
            this.tbTargetValue.UseSelectable = true;
            this.tbTargetValue.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbTargetValue.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cbOperator
            // 
            this.cbOperator.FormattingEnabled = true;
            this.cbOperator.ItemHeight = 23;
            this.cbOperator.Items.AddRange(new object[] {
            "AndAlso",
            "Any",
            "Equal",
            "GreaterThan",
            "LessThan",
            "Or"});
            this.cbOperator.Location = new System.Drawing.Point(761, 311);
            this.cbOperator.Name = "cbOperator";
            this.cbOperator.Size = new System.Drawing.Size(124, 29);
            this.cbOperator.TabIndex = 15;
            this.cbOperator.UseSelectable = true;
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(540, 289);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(100, 19);
            this.metroLabel7.TabIndex = 12;
            this.metroLabel7.Text = "Member Name";
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.Location = new System.Drawing.Point(761, 289);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(64, 19);
            this.metroLabel8.TabIndex = 14;
            this.metroLabel8.Text = "Operator";
            // 
            // metroLabel9
            // 
            this.metroLabel9.AutoSize = true;
            this.metroLabel9.Location = new System.Drawing.Point(891, 289);
            this.metroLabel9.Name = "metroLabel9";
            this.metroLabel9.Size = new System.Drawing.Size(79, 19);
            this.metroLabel9.TabIndex = 16;
            this.metroLabel9.Text = "Target Value";
            // 
            // addRuleButton
            // 
            this.addRuleButton.Location = new System.Drawing.Point(18, 463);
            this.addRuleButton.Name = "addRuleButton";
            this.addRuleButton.Size = new System.Drawing.Size(75, 23);
            this.addRuleButton.TabIndex = 23;
            this.addRuleButton.Text = "Add Rule";
            this.addRuleButton.UseSelectable = true;
            this.addRuleButton.Click += new System.EventHandler(this.addRuleButton_Click);
            // 
            // saveRuleButton
            // 
            this.saveRuleButton.Location = new System.Drawing.Point(148, 464);
            this.saveRuleButton.Name = "saveRuleButton";
            this.saveRuleButton.Size = new System.Drawing.Size(75, 23);
            this.saveRuleButton.TabIndex = 24;
            this.saveRuleButton.Text = "Save Rule";
            this.saveRuleButton.UseSelectable = true;
            this.saveRuleButton.Click += new System.EventHandler(this.saveRuleButton_Click);
            // 
            // cbMemberName
            // 
            this.cbMemberName.ContextMenuStrip = this.comboBoxMenuStrip;
            this.cbMemberName.FormattingEnabled = true;
            this.cbMemberName.ItemHeight = 23;
            this.cbMemberName.Location = new System.Drawing.Point(540, 311);
            this.cbMemberName.Name = "cbMemberName";
            this.cbMemberName.Size = new System.Drawing.Size(215, 29);
            this.cbMemberName.TabIndex = 13;
            this.cbMemberName.UseSelectable = true;
            // 
            // comboBoxMenuStrip
            // 
            this.comboBoxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetSelectionToolStripMenuItem});
            this.comboBoxMenuStrip.Name = "comboBoxMenuStrip";
            this.comboBoxMenuStrip.Size = new System.Drawing.Size(154, 26);
            // 
            // resetSelectionToolStripMenuItem
            // 
            this.resetSelectionToolStripMenuItem.Name = "resetSelectionToolStripMenuItem";
            this.resetSelectionToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.resetSelectionToolStripMenuItem.Text = "Reset Selection";
            this.resetSelectionToolStripMenuItem.Click += new System.EventHandler(this.resetSelectionToolStripMenuItem_Click);
            // 
            // addDetailButton
            // 
            this.addDetailButton.ContextMenuStrip = this.menuTVRuleHierarchy;
            this.addDetailButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.addDetailButton.Location = new System.Drawing.Point(455, 37);
            this.addDetailButton.Name = "addDetailButton";
            this.addDetailButton.Size = new System.Drawing.Size(32, 23);
            this.addDetailButton.TabIndex = 21;
            this.addDetailButton.Text = "+";
            this.addDetailButton.UseSelectable = true;
            this.addDetailButton.Click += new System.EventHandler(this.addDetailButton_Click);
            // 
            // removeDetailButton
            // 
            this.removeDetailButton.ContextMenuStrip = this.menuTVRuleHierarchy;
            this.removeDetailButton.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.removeDetailButton.Location = new System.Drawing.Point(493, 37);
            this.removeDetailButton.Name = "removeDetailButton";
            this.removeDetailButton.Size = new System.Drawing.Size(32, 23);
            this.removeDetailButton.TabIndex = 22;
            this.removeDetailButton.Text = "-";
            this.removeDetailButton.UseSelectable = true;
            this.removeDetailButton.Click += new System.EventHandler(this.removeDetailButton_Click);
            // 
            // saveCriteraButton
            // 
            this.saveCriteraButton.Location = new System.Drawing.Point(993, 464);
            this.saveCriteraButton.Name = "saveCriteraButton";
            this.saveCriteraButton.Size = new System.Drawing.Size(75, 23);
            this.saveCriteraButton.TabIndex = 18;
            this.saveCriteraButton.Text = "Save Criteria";
            this.saveCriteraButton.UseSelectable = true;
            this.saveCriteraButton.Click += new System.EventHandler(this.saveCriteraButton_Click);
            // 
            // ClaimRuleEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 509);
            this.Controls.Add(this.saveCriteraButton);
            this.Controls.Add(this.removeDetailButton);
            this.Controls.Add(this.addDetailButton);
            this.Controls.Add(this.cbMemberName);
            this.Controls.Add(this.saveRuleButton);
            this.Controls.Add(this.addRuleButton);
            this.Controls.Add(this.tbTargetValue);
            this.Controls.Add(this.cbOperator);
            this.Controls.Add(this.cbLineType);
            this.Controls.Add(this.endEffectiveDate);
            this.Controls.Add(this.effectiveDate);
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel9);
            this.Controls.Add(this.metroLabel8);
            this.Controls.Add(this.metroLabel7);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.tbRuleName);
            this.Controls.Add(this.tbErrorText);
            this.Controls.Add(this.tbRuleDescription);
            this.Controls.Add(this.listRules);
            this.Controls.Add(this.tvRuleHierarchy);
            this.Name = "ClaimRuleEditorForm";
            this.Text = "Claim Rule Editor";
            this.Load += new System.EventHandler(this.ClaimRuleEditorForm_Load);
            this.menuTVRuleHierarchy.ResumeLayout(false);
            this.comboBoxMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvRuleHierarchy;
        private MetroFramework.Controls.MetroListView listRules;
        private MetroFramework.Controls.MetroTextBox tbRuleDescription;
        private MetroFramework.Controls.MetroTextBox tbErrorText;
        private MetroFramework.Controls.MetroTextBox tbRuleName;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroDateTime effectiveDate;
        private MetroFramework.Controls.MetroDateTime endEffectiveDate;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroComboBox cbLineType;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroTextBox tbTargetValue;
        private MetroFramework.Controls.MetroComboBox cbOperator;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroLabel metroLabel9;
        private MetroFramework.Controls.MetroButton addRuleButton;
        private System.Windows.Forms.ContextMenuStrip menuTVRuleHierarchy;
        private System.Windows.Forms.ToolStripMenuItem toolStripAddCriteria;
        private System.Windows.Forms.ToolStripMenuItem toolStripAddGroup;
        private System.Windows.Forms.ToolStripMenuItem toolStripDeleteCriteria;
        private MetroFramework.Controls.MetroButton saveRuleButton;
        private MetroFramework.Controls.MetroComboBox cbMemberName;
        private MetroFramework.Controls.MetroButton addDetailButton;
        private MetroFramework.Controls.MetroButton removeDetailButton;
        private MetroFramework.Controls.MetroButton saveCriteraButton;
        private System.Windows.Forms.ContextMenuStrip comboBoxMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem resetSelectionToolStripMenuItem;
    }
}