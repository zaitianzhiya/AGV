namespace TS_RGB
{
    partial class frm_map
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MI_RFIDAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_AddELEC = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_AddAuto = new System.Windows.Forms.ToolStripMenuItem();
            this.ts_AddPower = new System.Windows.Forms.ToolStripMenuItem();
            this.增加呼叫盒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除电梯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除充电站ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除呼叫盒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.img_Map = new System.Windows.Forms.PictureBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img_Map)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_RFIDAdd,
            this.ts_AddELEC,
            this.ts_AddAuto,
            this.ts_AddPower,
            this.增加呼叫盒ToolStripMenuItem,
            this.xToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 136);
            // 
            // MI_RFIDAdd
            // 
            this.MI_RFIDAdd.Name = "MI_RFIDAdd";
            this.MI_RFIDAdd.Size = new System.Drawing.Size(153, 22);
            this.MI_RFIDAdd.Text = "增加RFID";
            this.MI_RFIDAdd.Click += new System.EventHandler(this.MI_RFIDAdd_Click);
            // 
            // ts_AddELEC
            // 
            this.ts_AddELEC.Name = "ts_AddELEC";
            this.ts_AddELEC.Size = new System.Drawing.Size(153, 22);
            this.ts_AddELEC.Text = "增加电梯";
            this.ts_AddELEC.Visible = false;
            this.ts_AddELEC.Click += new System.EventHandler(this.ts_AddELEC_Click);
            // 
            // ts_AddAuto
            // 
            this.ts_AddAuto.Name = "ts_AddAuto";
            this.ts_AddAuto.Size = new System.Drawing.Size(153, 22);
            this.ts_AddAuto.Text = "增加自动门";
            this.ts_AddAuto.Visible = false;
            this.ts_AddAuto.Click += new System.EventHandler(this.ts_AddAuto_Click);
            // 
            // ts_AddPower
            // 
            this.ts_AddPower.Name = "ts_AddPower";
            this.ts_AddPower.Size = new System.Drawing.Size(153, 22);
            this.ts_AddPower.Text = "增加充电点";
            this.ts_AddPower.Click += new System.EventHandler(this.ts_AddPower_Click);
            // 
            // 增加呼叫盒ToolStripMenuItem
            // 
            this.增加呼叫盒ToolStripMenuItem.Name = "增加呼叫盒ToolStripMenuItem";
            this.增加呼叫盒ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.增加呼叫盒ToolStripMenuItem.Text = "增加呼叫盒";
            this.增加呼叫盒ToolStripMenuItem.Click += new System.EventHandler(this.增加呼叫盒ToolStripMenuItem_Click);
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.xToolStripMenuItem.Text = "当前坐标:(X,Y)";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.xToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.删除电梯ToolStripMenuItem,
            this.删除充电站ToolStripMenuItem,
            this.删除呼叫盒ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(137, 92);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem1.Text = "删除RFID";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 删除电梯ToolStripMenuItem
            // 
            this.删除电梯ToolStripMenuItem.Name = "删除电梯ToolStripMenuItem";
            this.删除电梯ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除电梯ToolStripMenuItem.Text = "删除电梯";
            this.删除电梯ToolStripMenuItem.Visible = false;
            this.删除电梯ToolStripMenuItem.Click += new System.EventHandler(this.删除电梯ToolStripMenuItem_Click);
            // 
            // 删除充电站ToolStripMenuItem
            // 
            this.删除充电站ToolStripMenuItem.Name = "删除充电站ToolStripMenuItem";
            this.删除充电站ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除充电站ToolStripMenuItem.Text = "删除充电站";
            this.删除充电站ToolStripMenuItem.Click += new System.EventHandler(this.删除充电站ToolStripMenuItem_Click);
            // 
            // 删除呼叫盒ToolStripMenuItem
            // 
            this.删除呼叫盒ToolStripMenuItem.Name = "删除呼叫盒ToolStripMenuItem";
            this.删除呼叫盒ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.删除呼叫盒ToolStripMenuItem.Text = "删除呼叫盒";
            this.删除呼叫盒ToolStripMenuItem.Click += new System.EventHandler(this.删除呼叫盒ToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 989;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // img_Map
            // 
            this.img_Map.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.img_Map.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.img_Map.ContextMenuStrip = this.contextMenuStrip1;
            this.img_Map.Location = new System.Drawing.Point(1, 0);
            this.img_Map.Name = "img_Map";
            this.img_Map.Size = new System.Drawing.Size(1115, 306);
            this.img_Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.img_Map.TabIndex = 0;
            this.img_Map.TabStop = false;
            this.img_Map.MouseDown += new System.Windows.Forms.MouseEventHandler(this.img_Map_MouseDown);
            this.img_Map.MouseMove += new System.Windows.Forms.MouseEventHandler(this.img_Map_MouseMove);
            this.img_Map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.img_Map_MouseUp);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1234;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // frm_map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1050, 474);
            this.Controls.Add(this.img_Map);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frm_map";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "地图";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_map_FormClosing);
            this.Load += new System.EventHandler(this.frm_map_Load);
            this.Resize += new System.EventHandler(this.frm_map_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.img_Map)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MI_RFIDAdd;
        private System.Windows.Forms.PictureBox img_Map;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem ts_AddELEC;
        private System.Windows.Forms.ToolStripMenuItem ts_AddAuto;
        private System.Windows.Forms.ToolStripMenuItem ts_AddPower;
        private System.Windows.Forms.ToolStripMenuItem 删除电梯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 增加呼叫盒ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除呼叫盒ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除充电站ToolStripMenuItem;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;


    }
}