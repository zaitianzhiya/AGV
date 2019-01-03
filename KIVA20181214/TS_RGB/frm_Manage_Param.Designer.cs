namespace TS_RGB
{
    partial class frm_Manage_Param
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_Vol = new System.Windows.Forms.CheckBox();
            this.checkBox_landmark = new System.Windows.Forms.CheckBox();
            this.checkBox_warn = new System.Windows.Forms.CheckBox();
            this.checkBox_task = new System.Windows.Forms.CheckBox();
            this.checkBox_rec = new System.Windows.Forms.CheckBox();
            this.checkBox_send = new System.Windows.Forms.CheckBox();
            this.checkBox_power_log = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_msg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_mapPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_PNote = new System.Windows.Forms.TextBox();
            this.textBox_PName = new System.Windows.Forms.TextBox();
            this.textBox_PNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(468, 439);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "系统参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_Vol);
            this.groupBox3.Controls.Add(this.checkBox_landmark);
            this.groupBox3.Controls.Add(this.checkBox_warn);
            this.groupBox3.Controls.Add(this.checkBox_task);
            this.groupBox3.Controls.Add(this.checkBox_rec);
            this.groupBox3.Controls.Add(this.checkBox_send);
            this.groupBox3.Controls.Add(this.checkBox_power_log);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 207);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(462, 229);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "日志开关";
            // 
            // checkBox_Vol
            // 
            this.checkBox_Vol.AutoSize = true;
            this.checkBox_Vol.Location = new System.Drawing.Point(323, 64);
            this.checkBox_Vol.Name = "checkBox_Vol";
            this.checkBox_Vol.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Vol.TabIndex = 10;
            this.checkBox_Vol.Text = "电压记录";
            this.checkBox_Vol.UseVisualStyleBackColor = true;
            this.checkBox_Vol.CheckedChanged += new System.EventHandler(this.checkBox_Vol_CheckedChanged);
            // 
            // checkBox_landmark
            // 
            this.checkBox_landmark.AutoSize = true;
            this.checkBox_landmark.Location = new System.Drawing.Point(323, 33);
            this.checkBox_landmark.Name = "checkBox_landmark";
            this.checkBox_landmark.Size = new System.Drawing.Size(96, 16);
            this.checkBox_landmark.TabIndex = 9;
            this.checkBox_landmark.Text = "地标读取日志";
            this.checkBox_landmark.UseVisualStyleBackColor = true;
            this.checkBox_landmark.CheckedChanged += new System.EventHandler(this.checkBox_landmark_CheckedChanged);
            // 
            // checkBox_warn
            // 
            this.checkBox_warn.AutoSize = true;
            this.checkBox_warn.Location = new System.Drawing.Point(31, 64);
            this.checkBox_warn.Name = "checkBox_warn";
            this.checkBox_warn.Size = new System.Drawing.Size(90, 16);
            this.checkBox_warn.TabIndex = 8;
            this.checkBox_warn.Text = "AGV故障日志";
            this.checkBox_warn.UseVisualStyleBackColor = true;
            this.checkBox_warn.CheckedChanged += new System.EventHandler(this.checkBox_warn_CheckedChanged);
            // 
            // checkBox_task
            // 
            this.checkBox_task.AutoSize = true;
            this.checkBox_task.Location = new System.Drawing.Point(31, 101);
            this.checkBox_task.Name = "checkBox_task";
            this.checkBox_task.Size = new System.Drawing.Size(72, 16);
            this.checkBox_task.TabIndex = 7;
            this.checkBox_task.Text = "任务日志";
            this.checkBox_task.UseVisualStyleBackColor = true;
            this.checkBox_task.Visible = false;
            // 
            // checkBox_rec
            // 
            this.checkBox_rec.AutoSize = true;
            this.checkBox_rec.Location = new System.Drawing.Point(177, 33);
            this.checkBox_rec.Name = "checkBox_rec";
            this.checkBox_rec.Size = new System.Drawing.Size(96, 16);
            this.checkBox_rec.TabIndex = 6;
            this.checkBox_rec.Text = "数据接收日志";
            this.checkBox_rec.UseVisualStyleBackColor = true;
            this.checkBox_rec.CheckedChanged += new System.EventHandler(this.checkBox_rec_CheckedChanged);
            // 
            // checkBox_send
            // 
            this.checkBox_send.AutoSize = true;
            this.checkBox_send.Location = new System.Drawing.Point(31, 33);
            this.checkBox_send.Name = "checkBox_send";
            this.checkBox_send.Size = new System.Drawing.Size(96, 16);
            this.checkBox_send.TabIndex = 5;
            this.checkBox_send.Text = "数据发送日志";
            this.checkBox_send.UseVisualStyleBackColor = true;
            this.checkBox_send.CheckedChanged += new System.EventHandler(this.checkBox_send_CheckedChanged);
            // 
            // checkBox_power_log
            // 
            this.checkBox_power_log.AutoSize = true;
            this.checkBox_power_log.Location = new System.Drawing.Point(177, 64);
            this.checkBox_power_log.Name = "checkBox_power_log";
            this.checkBox_power_log.Size = new System.Drawing.Size(72, 16);
            this.checkBox_power_log.TabIndex = 0;
            this.checkBox_power_log.Text = "充电日志";
            this.checkBox_power_log.UseVisualStyleBackColor = true;
            this.checkBox_power_log.CheckedChanged += new System.EventHandler(this.checkBox_power_log_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_msg);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox_mapPath);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(462, 85);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "地图信息";
            // 
            // label_msg
            // 
            this.label_msg.AutoSize = true;
            this.label_msg.Location = new System.Drawing.Point(182, 58);
            this.label_msg.Name = "label_msg";
            this.label_msg.Size = new System.Drawing.Size(89, 12);
            this.label_msg.TabIndex = 5;
            this.label_msg.Text = "地图更新成功！";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "地图选择：";
            // 
            // textBox_mapPath
            // 
            this.textBox_mapPath.Location = new System.Drawing.Point(96, 25);
            this.textBox_mapPath.Name = "textBox_mapPath";
            this.textBox_mapPath.Size = new System.Drawing.Size(268, 21);
            this.textBox_mapPath.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(373, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "选择地图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_PNote);
            this.groupBox1.Controls.Add(this.textBox_PName);
            this.groupBox1.Controls.Add(this.textBox_PNo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(462, 119);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "项目信息";
            // 
            // textBox_PNote
            // 
            this.textBox_PNote.Location = new System.Drawing.Point(96, 77);
            this.textBox_PNote.Name = "textBox_PNote";
            this.textBox_PNote.ReadOnly = true;
            this.textBox_PNote.Size = new System.Drawing.Size(353, 21);
            this.textBox_PNote.TabIndex = 7;
            // 
            // textBox_PName
            // 
            this.textBox_PName.Location = new System.Drawing.Point(96, 49);
            this.textBox_PName.Name = "textBox_PName";
            this.textBox_PName.ReadOnly = true;
            this.textBox_PName.Size = new System.Drawing.Size(353, 21);
            this.textBox_PName.TabIndex = 6;
            // 
            // textBox_PNo
            // 
            this.textBox_PNo.Location = new System.Drawing.Point(96, 21);
            this.textBox_PNo.Name = "textBox_PNo";
            this.textBox_PNo.ReadOnly = true;
            this.textBox_PNo.Size = new System.Drawing.Size(353, 21);
            this.textBox_PNo.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "其他信息：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "项目名称：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "项目编号：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(476, 465);
            this.tabControl1.TabIndex = 0;
            // 
            // frm_Manage_Param
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 465);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Manage_Param";
            this.Text = "参数设置";
            this.Load += new System.EventHandler(this.frm_Manage_Param_Load);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_mapPath;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_PNote;
        private System.Windows.Forms.TextBox textBox_PName;
        private System.Windows.Forms.TextBox textBox_PNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_power_log;
        private System.Windows.Forms.Label label_msg;
        private System.Windows.Forms.CheckBox checkBox_send;
        private System.Windows.Forms.CheckBox checkBox_landmark;
        private System.Windows.Forms.CheckBox checkBox_warn;
        private System.Windows.Forms.CheckBox checkBox_task;
        private System.Windows.Forms.CheckBox checkBox_rec;
        private System.Windows.Forms.CheckBox checkBox_Vol;

    }
}