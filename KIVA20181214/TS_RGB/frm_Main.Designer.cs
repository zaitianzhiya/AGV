namespace TS_RGB
{
    partial class frm_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///// <summary>
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            this.mu_RGBST = new System.Windows.Forms.MenuStrip();
            this.mItems_SystemMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.mItems_RGVMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.任务监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wCS监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.充电站监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mItems_OrdMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.数据传输日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.接收日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发送日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mItems_RGV_Alams = new System.Windows.Forms.ToolStripMenuItem();
            this.aGV电压日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.充电日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.软件启停日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mItems_ = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.手松充电ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ts_Lable = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mu_RGBST.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mu_RGBST
            // 
            this.mu_RGBST.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.mu_RGBST.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mu_RGBST.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItems_SystemMonitor,
            this.日志ToolStripMenuItem,
            this.mItems_,
            this.toolStripMenuItem1,
            this.帮助HToolStripMenuItem});
            this.mu_RGBST.Location = new System.Drawing.Point(0, 0);
            this.mu_RGBST.Name = "mu_RGBST";
            this.mu_RGBST.Size = new System.Drawing.Size(980, 28);
            this.mu_RGBST.TabIndex = 0;
            this.mu_RGBST.Text = "menuStrip1";
            // 
            // mItems_SystemMonitor
            // 
            this.mItems_SystemMonitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItems_RGVMonitor,
            this.任务监控ToolStripMenuItem,
            this.wCS监控ToolStripMenuItem,
            this.充电站监控ToolStripMenuItem});
            this.mItems_SystemMonitor.Image = ((System.Drawing.Image)(resources.GetObject("mItems_SystemMonitor.Image")));
            this.mItems_SystemMonitor.Name = "mItems_SystemMonitor";
            this.mItems_SystemMonitor.Size = new System.Drawing.Size(92, 24);
            this.mItems_SystemMonitor.Text = "监控(&M)";
            // 
            // mItems_RGVMonitor
            // 
            this.mItems_RGVMonitor.Name = "mItems_RGVMonitor";
            this.mItems_RGVMonitor.Size = new System.Drawing.Size(153, 24);
            this.mItems_RGVMonitor.Text = "车辆监控";
            this.mItems_RGVMonitor.Click += new System.EventHandler(this.mItems_RGVMonitor_Click);
            // 
            // 任务监控ToolStripMenuItem
            // 
            this.任务监控ToolStripMenuItem.Name = "任务监控ToolStripMenuItem";
            this.任务监控ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.任务监控ToolStripMenuItem.Text = "任务监控";
            this.任务监控ToolStripMenuItem.Click += new System.EventHandler(this.任务监控ToolStripMenuItem_Click);
            // 
            // wCS监控ToolStripMenuItem
            // 
            this.wCS监控ToolStripMenuItem.Name = "wCS监控ToolStripMenuItem";
            this.wCS监控ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.wCS监控ToolStripMenuItem.Text = "WCS监控";
            this.wCS监控ToolStripMenuItem.Click += new System.EventHandler(this.wCS监控ToolStripMenuItem_Click);
            // 
            // 充电站监控ToolStripMenuItem
            // 
            this.充电站监控ToolStripMenuItem.Name = "充电站监控ToolStripMenuItem";
            this.充电站监控ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.充电站监控ToolStripMenuItem.Text = "充电站监控";
            this.充电站监控ToolStripMenuItem.Click += new System.EventHandler(this.充电站监控ToolStripMenuItem_Click);
            // 
            // 日志ToolStripMenuItem
            // 
            this.日志ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mItems_OrdMessage,
            this.数据传输日志ToolStripMenuItem,
            this.mItems_RGV_Alams,
            this.aGV电压日志ToolStripMenuItem,
            this.充电日志ToolStripMenuItem,
            this.软件启停日志ToolStripMenuItem});
            this.日志ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("日志ToolStripMenuItem.Image")));
            this.日志ToolStripMenuItem.Name = "日志ToolStripMenuItem";
            this.日志ToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.日志ToolStripMenuItem.Text = "日志(&L)";
            // 
            // mItems_OrdMessage
            // 
            this.mItems_OrdMessage.Name = "mItems_OrdMessage";
            this.mItems_OrdMessage.Size = new System.Drawing.Size(170, 24);
            this.mItems_OrdMessage.Text = "任务日志";
            this.mItems_OrdMessage.Visible = false;
            this.mItems_OrdMessage.Click += new System.EventHandler(this.mItems_OrdMessage_Click);
            // 
            // 数据传输日志ToolStripMenuItem
            // 
            this.数据传输日志ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.接收日志ToolStripMenuItem,
            this.发送日志ToolStripMenuItem});
            this.数据传输日志ToolStripMenuItem.Name = "数据传输日志ToolStripMenuItem";
            this.数据传输日志ToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.数据传输日志ToolStripMenuItem.Text = "数据传输日志";
            // 
            // 接收日志ToolStripMenuItem
            // 
            this.接收日志ToolStripMenuItem.Name = "接收日志ToolStripMenuItem";
            this.接收日志ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.接收日志ToolStripMenuItem.Text = "接收日志";
            this.接收日志ToolStripMenuItem.Click += new System.EventHandler(this.接收日志ToolStripMenuItem_Click);
            // 
            // 发送日志ToolStripMenuItem
            // 
            this.发送日志ToolStripMenuItem.Name = "发送日志ToolStripMenuItem";
            this.发送日志ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.发送日志ToolStripMenuItem.Text = "发送日志";
            this.发送日志ToolStripMenuItem.Click += new System.EventHandler(this.发送日志ToolStripMenuItem_Click);
            // 
            // mItems_RGV_Alams
            // 
            this.mItems_RGV_Alams.Name = "mItems_RGV_Alams";
            this.mItems_RGV_Alams.Size = new System.Drawing.Size(170, 24);
            this.mItems_RGV_Alams.Text = "AGV故障日志";
            this.mItems_RGV_Alams.Click += new System.EventHandler(this.mItems_RGV_Alams_Click);
            // 
            // aGV电压日志ToolStripMenuItem
            // 
            this.aGV电压日志ToolStripMenuItem.Name = "aGV电压日志ToolStripMenuItem";
            this.aGV电压日志ToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.aGV电压日志ToolStripMenuItem.Text = "AGV电压日志";
            this.aGV电压日志ToolStripMenuItem.Visible = false;
            this.aGV电压日志ToolStripMenuItem.Click += new System.EventHandler(this.aGV电压日志ToolStripMenuItem_Click);
            // 
            // 充电日志ToolStripMenuItem
            // 
            this.充电日志ToolStripMenuItem.Name = "充电日志ToolStripMenuItem";
            this.充电日志ToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.充电日志ToolStripMenuItem.Text = "AGV充电日志";
            this.充电日志ToolStripMenuItem.Click += new System.EventHandler(this.充电日志ToolStripMenuItem_Click);
            // 
            // 软件启停日志ToolStripMenuItem
            // 
            this.软件启停日志ToolStripMenuItem.Name = "软件启停日志ToolStripMenuItem";
            this.软件启停日志ToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.软件启停日志ToolStripMenuItem.Text = "软件启停日志";
            this.软件启停日志ToolStripMenuItem.Click += new System.EventHandler(this.软件启停日志ToolStripMenuItem_Click);
            // 
            // mItems_
            // 
            this.mItems_.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.toolStripSeparator1,
            this.手松充电ToolStripMenuItem});
            this.mItems_.Image = ((System.Drawing.Image)(resources.GetObject("mItems_.Image")));
            this.mItems_.Name = "mItems_";
            this.mItems_.Size = new System.Drawing.Size(86, 24);
            this.mItems_.Text = "系统(&S)";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // 手松充电ToolStripMenuItem
            // 
            this.手松充电ToolStripMenuItem.Name = "手松充电ToolStripMenuItem";
            this.手松充电ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.手松充电ToolStripMenuItem.Text = "手动充电";
            this.手松充电ToolStripMenuItem.Click += new System.EventHandler(this.手松充电ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 24);
            this.toolStripMenuItem1.Text = "登录(&O)";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.帮助文档ToolStripMenuItem});
            this.帮助HToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("帮助HToolStripMenuItem.Image")));
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 帮助文档ToolStripMenuItem
            // 
            this.帮助文档ToolStripMenuItem.Name = "帮助文档ToolStripMenuItem";
            this.帮助文档ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.帮助文档ToolStripMenuItem.Text = "SOP";
            this.帮助文档ToolStripMenuItem.Click += new System.EventHandler(this.帮助文档ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.ts_Lable,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel8});
            this.statusStrip1.Location = new System.Drawing.Point(0, 510);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(980, 30);
            this.statusStrip1.TabIndex = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 25);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripStatusLabel2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel2.Image")));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabel2.Text = "苏州快捷机器人有限公司";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(11, 25);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(40, 25);
            this.toolStripStatusLabel4.Text = "时间:";
            // 
            // ts_Lable
            // 
            this.ts_Lable.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ts_Lable.Name = "ts_Lable";
            this.ts_Lable.Size = new System.Drawing.Size(159, 25);
            this.ts_Lable.Text = "yyyy-dd-mm hh:mm:ss";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(122, 25);
            this.toolStripStatusLabel5.Text = " |  当前用户:匿名用户";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(11, 25);
            this.toolStripStatusLabel7.Text = "|";
            this.toolStripStatusLabel7.Visible = false;
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.ForeColor = System.Drawing.Color.Green;
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(92, 25);
            this.toolStripStatusLabel6.Text = " AGV总数：0/0";
            this.toolStripStatusLabel6.Visible = false;
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(131, 25);
            this.toolStripStatusLabel8.Text = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(12, 24);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(980, 540);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mu_RGBST);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mu_RGBST;
            this.Name = "frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AGV控制系统 RV_K1.3 20181214";
            this.TransparencyKey = System.Drawing.Color.Wheat;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mu_RGBST.ResumeLayout(false);
            this.mu_RGBST.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mu_RGBST;
        private System.Windows.Forms.ToolStripMenuItem mItems_;
        private System.Windows.Forms.ToolStripMenuItem mItems_SystemMonitor;
        private System.Windows.Forms.ToolStripMenuItem mItems_RGVMonitor;
        private System.Windows.Forms.ToolStripMenuItem 日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mItems_OrdMessage;
        private System.Windows.Forms.ToolStripMenuItem mItems_RGV_Alams;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel ts_Lable;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 充电站监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripMenuItem 数据传输日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 接收日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发送日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 充电日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripMenuItem aGV电压日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripMenuItem 软件启停日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripMenuItem 任务监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wCS监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 手松充电ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}