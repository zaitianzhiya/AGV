namespace TS_RGB
{
    partial class frm_Car
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_CarInfo = new System.Windows.Forms.DataGridView();
            this.Cm_Del = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tm_St = new System.Windows.Forms.ToolStripMenuItem();
            this.tm_Del = new System.Windows.Forms.ToolStripMenuItem();
            this.装载料架ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.举升托盘ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_Famart = new System.Windows.Forms.Button();
            this.bt_Manager = new System.Windows.Forms.Button();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_RFID_Now = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_LineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ErrorCord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Power = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Ang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKIP_Ang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_CarInfo)).BeginInit();
            this.Cm_Del.SuspendLayout();
            this.SuspendLayout();
            // 
            // dg_CarInfo
            // 
            this.dg_CarInfo.AllowUserToAddRows = false;
            this.dg_CarInfo.AllowUserToDeleteRows = false;
            this.dg_CarInfo.AllowUserToResizeColumns = false;
            this.dg_CarInfo.AllowUserToResizeRows = false;
            this.dg_CarInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_CarInfo.ColumnHeadersHeight = 30;
            this.dg_CarInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_CarInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.AGV_Remark,
            this.AGV_IP,
            this.AGV_AC,
            this.AGV_RFID_Now,
            this.AGV_LineNo,
            this.AGV_ErrorCord,
            this.Column1,
            this.AGV_Power,
            this.AGV_Speed,
            this.Column3,
            this.Column2,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.AGV_Ang,
            this.SKIP_Ang});
            this.dg_CarInfo.ContextMenuStrip = this.Cm_Del;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_CarInfo.DefaultCellStyle = dataGridViewCellStyle18;
            this.dg_CarInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_CarInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_CarInfo.Name = "dg_CarInfo";
            this.dg_CarInfo.ReadOnly = true;
            this.dg_CarInfo.RowHeadersWidth = 50;
            this.dg_CarInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_CarInfo.RowTemplate.Height = 30;
            this.dg_CarInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_CarInfo.Size = new System.Drawing.Size(1362, 596);
            this.dg_CarInfo.TabIndex = 1;
            this.dg_CarInfo.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DG_CARMESSAGE_CellContentDoubleClick);
            this.dg_CarInfo.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DG_CARMESSAGE_RowPostPaint);
            // 
            // Cm_Del
            // 
            this.Cm_Del.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tm_St,
            this.tm_Del,
            this.装载料架ToolStripMenuItem,
            this.举升托盘ToolStripMenuItem});
            this.Cm_Del.Name = "Cm_Del";
            this.Cm_Del.Size = new System.Drawing.Size(125, 92);
            // 
            // tm_St
            // 
            this.tm_St.Name = "tm_St";
            this.tm_St.Size = new System.Drawing.Size(124, 22);
            this.tm_St.Text = "初始化";
            this.tm_St.Click += new System.EventHandler(this.tm_St_Click);
            // 
            // tm_Del
            // 
            this.tm_Del.Name = "tm_Del";
            this.tm_Del.Size = new System.Drawing.Size(124, 22);
            this.tm_Del.Text = "删除";
            this.tm_Del.Visible = false;
            this.tm_Del.Click += new System.EventHandler(this.tm_Del_Click);
            // 
            // 装载料架ToolStripMenuItem
            // 
            this.装载料架ToolStripMenuItem.Name = "装载料架ToolStripMenuItem";
            this.装载料架ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.装载料架ToolStripMenuItem.Text = "下降托盘";
            this.装载料架ToolStripMenuItem.Click += new System.EventHandler(this.装载料架ToolStripMenuItem_Click);
            // 
            // 举升托盘ToolStripMenuItem
            // 
            this.举升托盘ToolStripMenuItem.Name = "举升托盘ToolStripMenuItem";
            this.举升托盘ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.举升托盘ToolStripMenuItem.Text = "举升托盘";
            this.举升托盘ToolStripMenuItem.Click += new System.EventHandler(this.举升托盘ToolStripMenuItem_Click);
            // 
            // bt_Famart
            // 
            this.bt_Famart.Location = new System.Drawing.Point(858, 577);
            this.bt_Famart.Name = "bt_Famart";
            this.bt_Famart.Size = new System.Drawing.Size(75, 23);
            this.bt_Famart.TabIndex = 2;
            this.bt_Famart.Text = "初始化";
            this.bt_Famart.UseVisualStyleBackColor = true;
            this.bt_Famart.Visible = false;
            // 
            // bt_Manager
            // 
            this.bt_Manager.Location = new System.Drawing.Point(749, 577);
            this.bt_Manager.Name = "bt_Manager";
            this.bt_Manager.Size = new System.Drawing.Size(75, 23);
            this.bt_Manager.TabIndex = 3;
            this.bt_Manager.Text = "功能选择";
            this.bt_Manager.UseVisualStyleBackColor = true;
            this.bt_Manager.Visible = false;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "ID";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Visible = false;
            // 
            // AGV_Remark
            // 
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_Remark.DefaultCellStyle = dataGridViewCellStyle1;
            this.AGV_Remark.HeaderText = "AGV编号";
            this.AGV_Remark.Name = "AGV_Remark";
            this.AGV_Remark.ReadOnly = true;
            this.AGV_Remark.Visible = false;
            this.AGV_Remark.Width = 80;
            // 
            // AGV_IP
            // 
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_IP.DefaultCellStyle = dataGridViewCellStyle2;
            this.AGV_IP.HeaderText = "IP地址";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            // 
            // AGV_AC
            // 
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_AC.DefaultCellStyle = dataGridViewCellStyle3;
            this.AGV_AC.HeaderText = "网络状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            this.AGV_AC.Width = 80;
            // 
            // AGV_RFID_Now
            // 
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_RFID_Now.DefaultCellStyle = dataGridViewCellStyle4;
            this.AGV_RFID_Now.HeaderText = "当前位置";
            this.AGV_RFID_Now.Name = "AGV_RFID_Now";
            this.AGV_RFID_Now.ReadOnly = true;
            this.AGV_RFID_Now.Width = 80;
            // 
            // AGV_LineNo
            // 
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_LineNo.DefaultCellStyle = dataGridViewCellStyle5;
            this.AGV_LineNo.HeaderText = "路线编号";
            this.AGV_LineNo.Name = "AGV_LineNo";
            this.AGV_LineNo.ReadOnly = true;
            this.AGV_LineNo.Visible = false;
            this.AGV_LineNo.Width = 80;
            // 
            // AGV_ErrorCord
            // 
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_ErrorCord.DefaultCellStyle = dataGridViewCellStyle6;
            this.AGV_ErrorCord.HeaderText = "故障信息";
            this.AGV_ErrorCord.Name = "AGV_ErrorCord";
            this.AGV_ErrorCord.ReadOnly = true;
            this.AGV_ErrorCord.Width = 130;
            // 
            // Column1
            // 
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column1.HeaderText = "当前电流";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // AGV_Power
            // 
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_Power.DefaultCellStyle = dataGridViewCellStyle8;
            this.AGV_Power.HeaderText = "当前电压";
            this.AGV_Power.Name = "AGV_Power";
            this.AGV_Power.ReadOnly = true;
            this.AGV_Power.Width = 80;
            // 
            // AGV_Speed
            // 
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_Speed.DefaultCellStyle = dataGridViewCellStyle9;
            this.AGV_Speed.HeaderText = "运动状态";
            this.AGV_Speed.Name = "AGV_Speed";
            this.AGV_Speed.ReadOnly = true;
            this.AGV_Speed.Width = 80;
            // 
            // Column3
            // 
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column3.DefaultCellStyle = dataGridViewCellStyle10;
            this.Column3.HeaderText = "是否锁定";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // Column2
            // 
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column2.HeaderText = "是否在二维码上";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 110;
            // 
            // Column4
            // 
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle12;
            this.Column4.HeaderText = "当前任务";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column5.DefaultCellStyle = dataGridViewCellStyle13;
            this.Column5.HeaderText = "总任务数";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 80;
            // 
            // Column6
            // 
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column6.DefaultCellStyle = dataGridViewCellStyle14;
            this.Column6.HeaderText = "起点";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 80;
            // 
            // Column7
            // 
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Column7.DefaultCellStyle = dataGridViewCellStyle15;
            this.Column7.HeaderText = "终点";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 80;
            // 
            // AGV_Ang
            // 
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.AGV_Ang.DefaultCellStyle = dataGridViewCellStyle16;
            this.AGV_Ang.HeaderText = "AGV角度";
            this.AGV_Ang.Name = "AGV_Ang";
            this.AGV_Ang.ReadOnly = true;
            // 
            // SKIP_Ang
            // 
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.SKIP_Ang.DefaultCellStyle = dataGridViewCellStyle17;
            this.SKIP_Ang.HeaderText = "料架角度";
            this.SKIP_Ang.Name = "SKIP_Ang";
            this.SKIP_Ang.ReadOnly = true;
            // 
            // frm_Car
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 596);
            this.Controls.Add(this.bt_Manager);
            this.Controls.Add(this.bt_Famart);
            this.Controls.Add(this.dg_CarInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frm_Car";
            this.ShowIcon = false;
            this.Text = "车辆一览";
            ((System.ComponentModel.ISupportInitialize)(this.dg_CarInfo)).EndInit();
            this.Cm_Del.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_CarInfo;
        private System.Windows.Forms.ContextMenuStrip Cm_Del;
        private System.Windows.Forms.ToolStripMenuItem tm_St;
        private System.Windows.Forms.ToolStripMenuItem tm_Del;
        private System.Windows.Forms.Button bt_Famart;
        private System.Windows.Forms.Button bt_Manager;
        private System.Windows.Forms.ToolStripMenuItem 装载料架ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 举升托盘ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_RFID_Now;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_LineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ErrorCord;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Power;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Ang;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKIP_Ang;
    }
}