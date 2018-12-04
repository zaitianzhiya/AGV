namespace AGVMAP.Dialog
{
    partial class FrmOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOption));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clorPk_grid = new DevExpress.XtraEditors.ColorPickEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.rdoPoints = new System.Windows.Forms.RadioButton();
            this.rdoLines = new System.Windows.Forms.RadioButton();
            this.ckeEnableGrid = new DevExpress.XtraEditors.CheckEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPenSize = new DevExpress.XtraEditors.TextEdit();
            this.clorPk_pen = new DevExpress.XtraEditors.ColorPickEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clorPk_bg = new DevExpress.XtraEditors.ColorPickEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clorPk_grid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeEnableGrid.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPenSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clorPk_pen.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clorPk_bg.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnOK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 277);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(518, 44);
            this.panelControl1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(419, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(322, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupBox2);
            this.panelControl2.Controls.Add(this.groupBox3);
            this.panelControl2.Controls.Add(this.groupBox1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Padding = new System.Windows.Forms.Padding(10);
            this.panelControl2.Size = new System.Drawing.Size(518, 277);
            this.panelControl2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clorPk_grid);
            this.groupBox2.Controls.Add(this.labelControl2);
            this.groupBox2.Controls.Add(this.rdoPoints);
            this.groupBox2.Controls.Add(this.rdoLines);
            this.groupBox2.Controls.Add(this.ckeEnableGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(12, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(494, 125);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "栅格";
            // 
            // clorPk_grid
            // 
            this.clorPk_grid.EditValue = System.Drawing.Color.Empty;
            this.clorPk_grid.Location = new System.Drawing.Point(138, 95);
            this.clorPk_grid.Name = "clorPk_grid";
            this.clorPk_grid.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.clorPk_grid.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.clorPk_grid.Size = new System.Drawing.Size(159, 20);
            this.clorPk_grid.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(75, 95);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "格栅颜色:";
            // 
            // rdoPoints
            // 
            this.rdoPoints.AutoSize = true;
            this.rdoPoints.Location = new System.Drawing.Point(188, 59);
            this.rdoPoints.Name = "rdoPoints";
            this.rdoPoints.Size = new System.Drawing.Size(73, 18);
            this.rdoPoints.TabIndex = 2;
            this.rdoPoints.TabStop = true;
            this.rdoPoints.Text = "点阵网格";
            this.rdoPoints.UseVisualStyleBackColor = true;
            // 
            // rdoLines
            // 
            this.rdoLines.AutoSize = true;
            this.rdoLines.Location = new System.Drawing.Point(75, 59);
            this.rdoLines.Name = "rdoLines";
            this.rdoLines.Size = new System.Drawing.Size(73, 18);
            this.rdoLines.TabIndex = 1;
            this.rdoLines.TabStop = true;
            this.rdoLines.Text = "直线网格";
            this.rdoLines.UseVisualStyleBackColor = true;
            // 
            // ckeEnableGrid
            // 
            this.ckeEnableGrid.Location = new System.Drawing.Point(74, 22);
            this.ckeEnableGrid.Name = "ckeEnableGrid";
            this.ckeEnableGrid.Properties.Caption = "启用栅格";
            this.ckeEnableGrid.Size = new System.Drawing.Size(75, 19);
            this.ckeEnableGrid.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPenSize);
            this.groupBox3.Controls.Add(this.clorPk_pen);
            this.groupBox3.Controls.Add(this.labelControl4);
            this.groupBox3.Controls.Add(this.labelControl3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(12, 185);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(494, 80);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "画布";
            // 
            // txtPenSize
            // 
            this.txtPenSize.Location = new System.Drawing.Point(138, 48);
            this.txtPenSize.Name = "txtPenSize";
            this.txtPenSize.Properties.Mask.EditMask = "(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))";
            this.txtPenSize.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtPenSize.Properties.Mask.ShowPlaceHolders = false;
            this.txtPenSize.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPenSize.Size = new System.Drawing.Size(159, 20);
            this.txtPenSize.TabIndex = 3;
            // 
            // clorPk_pen
            // 
            this.clorPk_pen.EditValue = System.Drawing.Color.Empty;
            this.clorPk_pen.Location = new System.Drawing.Point(138, 21);
            this.clorPk_pen.Name = "clorPk_pen";
            this.clorPk_pen.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.clorPk_pen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.clorPk_pen.Size = new System.Drawing.Size(159, 20);
            this.clorPk_pen.TabIndex = 2;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(75, 49);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(52, 14);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "画笔大小:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(75, 22);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 14);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "画笔颜色:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clorPk_bg);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "背景";
            // 
            // clorPk_bg
            // 
            this.clorPk_bg.EditValue = System.Drawing.Color.Empty;
            this.clorPk_bg.Location = new System.Drawing.Point(138, 18);
            this.clorPk_bg.Name = "clorPk_bg";
            this.clorPk_bg.Properties.AutomaticColor = System.Drawing.Color.Black;
            this.clorPk_bg.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.clorPk_bg.Size = new System.Drawing.Size(159, 20);
            this.clorPk_bg.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(87, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "背景色:";
            // 
            // FrmOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 321);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmOption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选项";
            this.Load += new System.EventHandler(this.FrmOption_Load);
            this.Shown += new System.EventHandler(this.FrmOption_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clorPk_grid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeEnableGrid.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPenSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clorPk_pen.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clorPk_bg.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.ColorPickEdit clorPk_grid;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.RadioButton rdoPoints;
        private System.Windows.Forms.RadioButton rdoLines;
        private DevExpress.XtraEditors.CheckEdit ckeEnableGrid;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.ColorPickEdit clorPk_bg;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtPenSize;
        private DevExpress.XtraEditors.ColorPickEdit clorPk_pen;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}