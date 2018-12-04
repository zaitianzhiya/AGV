namespace AGVMAP.Dialog
{
    partial class FrmDbSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDbSetup));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lc1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtPwd = new DevExpress.XtraEditors.TextEdit();
            this.txtUid = new DevExpress.XtraEditors.TextEdit();
            this.txtDbName = new DevExpress.XtraEditors.TextEdit();
            this.txtDbAddress = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lc2 = new DevExpress.XtraLayout.LayoutControl();
            this.txtHostPort = new DevExpress.XtraEditors.TextEdit();
            this.txtHostIp = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.btnTest = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lc1)).BeginInit();
            this.lc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDbName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDbAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lc2)).BeginInit();
            this.lc2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHostPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHostIp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lc1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 160);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库设置";
            // 
            // lc1
            // 
            this.lc1.Controls.Add(this.txtPwd);
            this.lc1.Controls.Add(this.txtUid);
            this.lc1.Controls.Add(this.txtDbName);
            this.lc1.Controls.Add(this.txtDbAddress);
            this.lc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lc1.Font = new System.Drawing.Font("宋体", 12F);
            this.lc1.Location = new System.Drawing.Point(3, 22);
            this.lc1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lc1.Name = "lc1";
            this.lc1.Root = this.layoutControlGroup1;
            this.lc1.Size = new System.Drawing.Size(374, 135);
            this.lc1.TabIndex = 0;
            this.lc1.Text = "layoutControl1";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(96, 97);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtPwd.Properties.Appearance.Options.UseFont = true;
            this.txtPwd.Properties.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(266, 26);
            this.txtPwd.StyleController = this.lc1;
            this.txtPwd.TabIndex = 7;
            // 
            // txtUid
            // 
            this.txtUid.Location = new System.Drawing.Point(96, 67);
            this.txtUid.Name = "txtUid";
            this.txtUid.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtUid.Properties.Appearance.Options.UseFont = true;
            this.txtUid.Size = new System.Drawing.Size(266, 26);
            this.txtUid.StyleController = this.lc1;
            this.txtUid.TabIndex = 6;
            // 
            // txtDbName
            // 
            this.txtDbName.Location = new System.Drawing.Point(96, 37);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtDbName.Properties.Appearance.Options.UseFont = true;
            this.txtDbName.Size = new System.Drawing.Size(266, 26);
            this.txtDbName.StyleController = this.lc1;
            this.txtDbName.TabIndex = 5;
            // 
            // txtDbAddress
            // 
            this.txtDbAddress.Location = new System.Drawing.Point(96, 7);
            this.txtDbAddress.Name = "txtDbAddress";
            this.txtDbAddress.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtDbAddress.Properties.Appearance.Options.UseFont = true;
            this.txtDbAddress.Size = new System.Drawing.Size(266, 26);
            this.txtDbAddress.StyleController = this.lc1;
            this.txtDbAddress.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 10, 5, 10);
            this.layoutControlGroup1.Size = new System.Drawing.Size(374, 135);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtDbAddress;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(359, 30);
            this.layoutControlItem1.Text = "数据库地址:";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(86, 19);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem2.Control = this.txtDbName;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(359, 30);
            this.layoutControlItem2.Text = "数据库名称:";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(86, 19);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem3.Control = this.txtUid;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(359, 30);
            this.layoutControlItem3.Text = "用户名:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(86, 19);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem4.Control = this.txtPwd;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(359, 30);
            this.layoutControlItem4.Text = "密码:";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(86, 19);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lc2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox2.Location = new System.Drawing.Point(389, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 160);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "上位机通讯设置";
            // 
            // lc2
            // 
            this.lc2.Controls.Add(this.txtHostPort);
            this.lc2.Controls.Add(this.txtHostIp);
            this.lc2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lc2.Location = new System.Drawing.Point(3, 22);
            this.lc2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lc2.Name = "lc2";
            this.lc2.Root = this.layoutControlGroup2;
            this.lc2.Size = new System.Drawing.Size(374, 135);
            this.lc2.TabIndex = 0;
            this.lc2.Text = "layoutControl1";
            // 
            // txtHostPort
            // 
            this.txtHostPort.Location = new System.Drawing.Point(96, 37);
            this.txtHostPort.Name = "txtHostPort";
            this.txtHostPort.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtHostPort.Properties.Appearance.Options.UseFont = true;
            this.txtHostPort.Size = new System.Drawing.Size(266, 26);
            this.txtHostPort.StyleController = this.lc2;
            this.txtHostPort.TabIndex = 5;
            // 
            // txtHostIp
            // 
            this.txtHostIp.Location = new System.Drawing.Point(96, 7);
            this.txtHostIp.Name = "txtHostIp";
            this.txtHostIp.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtHostIp.Properties.Appearance.Options.UseFont = true;
            this.txtHostIp.Size = new System.Drawing.Size(266, 26);
            this.txtHostIp.StyleController = this.lc2;
            this.txtHostIp.TabIndex = 4;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.emptySpaceItem1,
            this.layoutControlItem6});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 10, 5, 10);
            this.layoutControlGroup2.Size = new System.Drawing.Size(374, 135);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem5.Control = this.txtHostIp;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(359, 30);
            this.layoutControlItem5.Text = "上位机地址:";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(86, 19);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 60);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(359, 60);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem6.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem6.Control = this.txtHostPort;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(359, 30);
            this.layoutControlItem6.Text = "上位机端口:";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(86, 19);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 230);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnConfirm);
            this.panel2.Controls.Add(this.btnTest);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 160);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(769, 70);
            this.panel2.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(503, 15);
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 40);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnConfirm.Appearance.Options.UseFont = true;
            this.btnConfirm.Location = new System.Drawing.Point(318, 15);
            this.btnConfirm.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(133, 40);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnTest
            // 
            this.btnTest.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.btnTest.Appearance.Options.UseFont = true;
            this.btnTest.Location = new System.Drawing.Point(134, 15);
            this.btnTest.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(133, 40);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "测试";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // FrmDbSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 242);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmDbSetup";
            this.Padding = new System.Windows.Forms.Padding(12, 12, 12, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "调度通信设置";
            this.Load += new System.EventHandler(this.FrmDbSetup_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lc1)).EndInit();
            this.lc1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPwd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDbName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDbAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lc2)).EndInit();
            this.lc2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtHostPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHostIp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraLayout.LayoutControl lc1;
        private DevExpress.XtraEditors.TextEdit txtPwd;
        private DevExpress.XtraEditors.TextEdit txtUid;
        private DevExpress.XtraEditors.TextEdit txtDbName;
        private DevExpress.XtraEditors.TextEdit txtDbAddress;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControl lc2;
        private DevExpress.XtraEditors.TextEdit txtHostPort;
        private DevExpress.XtraEditors.TextEdit txtHostIp;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.SimpleButton btnTest;
    }
}