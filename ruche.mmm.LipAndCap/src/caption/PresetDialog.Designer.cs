namespace ruche.mmm.caption
{
    partial class PresetDialog
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
            this.panelRoot = new System.Windows.Forms.TableLayoutPanel();
            this.panelOkCancel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelRight = new System.Windows.Forms.TableLayoutPanel();
            this.panelEditApply = new System.Windows.Forms.TableLayoutPanel();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.panelProp = new System.Windows.Forms.TableLayoutPanel();
            this.panelPresetName = new System.Windows.Forms.TableLayoutPanel();
            this.labelPresetName = new System.Windows.Forms.Label();
            this.textPresetName = new System.Windows.Forms.TextBox();
            this.propCaption = new System.Windows.Forms.PropertyGrid();
            this.panelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.panelUpDown = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.listPreset = new System.Windows.Forms.ListBox();
            this.panelRoot.SuspendLayout();
            this.panelOkCancel.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelEditApply.SuspendLayout();
            this.panelProp.SuspendLayout();
            this.panelPresetName.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelUpDown.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.ColumnCount = 1;
            this.panelRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRoot.Controls.Add(this.panelOkCancel, 0, 2);
            this.panelRoot.Controls.Add(this.panelMain, 0, 0);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.panelRoot.RowCount = 3;
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.panelRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelRoot.Size = new System.Drawing.Size(524, 390);
            this.panelRoot.TabIndex = 1;
            // 
            // panelOkCancel
            // 
            this.panelOkCancel.ColumnCount = 5;
            this.panelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.panelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.panelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.panelOkCancel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.panelOkCancel.Controls.Add(this.buttonOk, 1, 0);
            this.panelOkCancel.Controls.Add(this.buttonCancel, 3, 0);
            this.panelOkCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOkCancel.Location = new System.Drawing.Point(4, 354);
            this.panelOkCancel.Margin = new System.Windows.Forms.Padding(0);
            this.panelOkCancel.Name = "panelOkCancel";
            this.panelOkCancel.RowCount = 1;
            this.panelOkCancel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelOkCancel.Size = new System.Drawing.Size(516, 28);
            this.panelOkCancel.TabIndex = 1;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(307, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(94, 22);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK(&O)";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(415, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 22);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "キャンセル(Esc)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panelMain
            // 
            this.panelMain.ColumnCount = 2;
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.panelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.panelMain.Controls.Add(this.panelRight, 1, 0);
            this.panelMain.Controls.Add(this.panelLeft, 0, 0);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(4, 8);
            this.panelMain.Margin = new System.Windows.Forms.Padding(0);
            this.panelMain.Name = "panelMain";
            this.panelMain.RowCount = 1;
            this.panelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelMain.Size = new System.Drawing.Size(516, 338);
            this.panelMain.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.ColumnCount = 4;
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRight.Controls.Add(this.panelEditApply, 1, 0);
            this.panelRight.Controls.Add(this.panelProp, 3, 0);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(154, 0);
            this.panelRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelRight.Name = "panelRight";
            this.panelRight.RowCount = 1;
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRight.Size = new System.Drawing.Size(362, 338);
            this.panelRight.TabIndex = 1;
            // 
            // panelEditApply
            // 
            this.panelEditApply.ColumnCount = 1;
            this.panelEditApply.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelEditApply.Controls.Add(this.buttonEdit, 0, 1);
            this.panelEditApply.Controls.Add(this.buttonApply, 0, 3);
            this.panelEditApply.Controls.Add(this.buttonRemove, 0, 5);
            this.panelEditApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditApply.Location = new System.Drawing.Point(4, 0);
            this.panelEditApply.Margin = new System.Windows.Forms.Padding(0);
            this.panelEditApply.Name = "panelEditApply";
            this.panelEditApply.RowCount = 7;
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelEditApply.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelEditApply.Size = new System.Drawing.Size(100, 338);
            this.panelEditApply.TabIndex = 0;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEdit.Location = new System.Drawing.Point(3, 59);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(94, 22);
            this.buttonEdit.TabIndex = 0;
            this.buttonEdit.Text = "編集(&E) >>";
            this.buttonEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonApply.Location = new System.Drawing.Point(3, 95);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(94, 22);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "<< 適用(&A)";
            this.buttonApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemove.Location = new System.Drawing.Point(3, 151);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(94, 22);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "×削除(Del)";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // panelProp
            // 
            this.panelProp.ColumnCount = 1;
            this.panelProp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelProp.Controls.Add(this.panelPresetName, 0, 0);
            this.panelProp.Controls.Add(this.propCaption, 0, 2);
            this.panelProp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProp.Location = new System.Drawing.Point(108, 0);
            this.panelProp.Margin = new System.Windows.Forms.Padding(0);
            this.panelProp.Name = "panelProp";
            this.panelProp.RowCount = 3;
            this.panelProp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.panelProp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.panelProp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelProp.Size = new System.Drawing.Size(254, 338);
            this.panelProp.TabIndex = 1;
            // 
            // panelPresetName
            // 
            this.panelPresetName.ColumnCount = 2;
            this.panelPresetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.panelPresetName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelPresetName.Controls.Add(this.labelPresetName, 0, 0);
            this.panelPresetName.Controls.Add(this.textPresetName, 1, 0);
            this.panelPresetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPresetName.Location = new System.Drawing.Point(0, 0);
            this.panelPresetName.Margin = new System.Windows.Forms.Padding(0);
            this.panelPresetName.Name = "panelPresetName";
            this.panelPresetName.RowCount = 1;
            this.panelPresetName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelPresetName.Size = new System.Drawing.Size(254, 24);
            this.panelPresetName.TabIndex = 0;
            // 
            // labelPresetName
            // 
            this.labelPresetName.AutoSize = true;
            this.labelPresetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPresetName.Location = new System.Drawing.Point(3, 0);
            this.labelPresetName.Name = "labelPresetName";
            this.labelPresetName.Size = new System.Drawing.Size(78, 24);
            this.labelPresetName.TabIndex = 0;
            this.labelPresetName.Text = "プリセット名(&N):";
            this.labelPresetName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textPresetName
            // 
            this.textPresetName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPresetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPresetName.Location = new System.Drawing.Point(87, 3);
            this.textPresetName.Name = "textPresetName";
            this.textPresetName.Size = new System.Drawing.Size(164, 19);
            this.textPresetName.TabIndex = 1;
            this.textPresetName.Text = "My Preset";
            // 
            // propCaption
            // 
            this.propCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propCaption.Location = new System.Drawing.Point(3, 31);
            this.propCaption.Name = "propCaption";
            this.propCaption.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propCaption.Size = new System.Drawing.Size(248, 304);
            this.propCaption.TabIndex = 1;
            this.propCaption.ToolbarVisible = false;
            // 
            // panelLeft
            // 
            this.panelLeft.ColumnCount = 1;
            this.panelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelLeft.Controls.Add(this.panelUpDown, 0, 1);
            this.panelLeft.Controls.Add(this.listPreset, 0, 0);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.RowCount = 2;
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.panelLeft.Size = new System.Drawing.Size(154, 338);
            this.panelLeft.TabIndex = 0;
            // 
            // panelUpDown
            // 
            this.panelUpDown.ColumnCount = 2;
            this.panelUpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelUpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelUpDown.Controls.Add(this.buttonUp, 0, 0);
            this.panelUpDown.Controls.Add(this.buttonDown, 1, 0);
            this.panelUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUpDown.Location = new System.Drawing.Point(0, 310);
            this.panelUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.panelUpDown.Name = "panelUpDown";
            this.panelUpDown.RowCount = 1;
            this.panelUpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelUpDown.Size = new System.Drawing.Size(154, 28);
            this.panelUpDown.TabIndex = 1;
            // 
            // buttonUp
            // 
            this.buttonUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUp.Location = new System.Drawing.Point(3, 3);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(71, 22);
            this.buttonUp.TabIndex = 0;
            this.buttonUp.Text = "↑(&U)";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDown.Location = new System.Drawing.Point(80, 3);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(71, 22);
            this.buttonDown.TabIndex = 1;
            this.buttonDown.Text = "↓(&D)";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // listPreset
            // 
            this.listPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPreset.FormattingEnabled = true;
            this.listPreset.ItemHeight = 12;
            this.listPreset.Location = new System.Drawing.Point(3, 3);
            this.listPreset.Name = "listPreset";
            this.listPreset.Size = new System.Drawing.Size(148, 304);
            this.listPreset.TabIndex = 0;
            this.listPreset.SelectedIndexChanged += new System.EventHandler(this.listPreset_SelectedIndexChanged);
            // 
            // PresetDialog
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(524, 390);
            this.Controls.Add(this.panelRoot);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 428);
            this.Name = "PresetDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "字幕プリセット編集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PresetDialog_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PresetDialog_FormClosed);
            this.Load += new System.EventHandler(this.PresetDialog_Load);
            this.panelRoot.ResumeLayout(false);
            this.panelOkCancel.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelEditApply.ResumeLayout(false);
            this.panelProp.ResumeLayout(false);
            this.panelPresetName.ResumeLayout(false);
            this.panelPresetName.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelUpDown.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel panelRoot;
        private System.Windows.Forms.TableLayoutPanel panelOkCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TableLayoutPanel panelMain;
        private System.Windows.Forms.TableLayoutPanel panelRight;
        private System.Windows.Forms.TableLayoutPanel panelEditApply;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TableLayoutPanel panelProp;
        private System.Windows.Forms.TableLayoutPanel panelPresetName;
        private System.Windows.Forms.Label labelPresetName;
        private System.Windows.Forms.TextBox textPresetName;
        private System.Windows.Forms.TableLayoutPanel panelLeft;
        private System.Windows.Forms.TableLayoutPanel panelUpDown;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.ListBox listPreset;
        private System.Windows.Forms.PropertyGrid propCaption;

    }
}