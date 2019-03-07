namespace TS_RGB
{
    partial class frm_Monitor_Car
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_CarInfo = new System.Windows.Forms.DataGridView();
            this.Cm_Del = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tm_St = new System.Windows.Forms.ToolStripMenuItem();
            this.tm_Del = new System.Windows.Forms.ToolStripMenuItem();
            this.清除电梯占用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除全部管控占用ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bt_Famart = new System.Windows.Forms.Button();
            this.bt_Manager = new System.Windows.Forms.Button();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_RFID_Now = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_LineNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ErrorCord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Power = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dg_CarInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_CarInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_CarInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_CarInfo.ColumnHeadersHeight = 30;
            this.dg_CarInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_CarInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.AGV_Remark,
            this.AGV_IP,
            this.AGV_AC,
            this.AGV_RFID_Now,
            this.Column1,
            this.AGV_LineNo,
            this.AGV_ErrorCord,
            this.AGV_Power,
            this.AGV_Speed});
            this.dg_CarInfo.ContextMenuStrip = this.Cm_Del;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_CarInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dg_CarInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_CarInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_CarInfo.Name = "dg_CarInfo";
            this.dg_CarInfo.ReadOnly = true;
            this.dg_CarInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_CarInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dg_CarInfo.RowHeadersWidth = 50;
            this.dg_CarInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_CarInfo.RowTemplate.Height = 30;
            this.dg_CarInfo.Size = new System.Drawing.Size(1245, 362);
            this.dg_CarInfo.TabIndex = 1;
            this.dg_CarInfo.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DG_CARMESSAGE_CellContentDoubleClick);
            this.dg_CarInfo.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DG_CARMESSAGE_RowPostPaint);
            // 
            // Cm_Del
            // 
            this.Cm_Del.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tm_St,
            this.tm_Del,
            this.清除电梯占用ToolStripMenuItem,
            this.清除全部管控占用ToolStripMenuItem});
            this.Cm_Del.Name = "Cm_Del";
            this.Cm_Del.Size = new System.Drawing.Size(173, 92);
            // 
            // tm_St
            // 
            this.tm_St.Name = "tm_St";
            this.tm_St.Size = new System.Drawing.Size(172, 22);
            this.tm_St.Text = "初始化";
            this.tm_St.Visible = false;
            this.tm_St.Click += new System.EventHandler(this.tm_St_Click);
            // 
            // tm_Del
            // 
            this.tm_Del.Name = "tm_Del";
            this.tm_Del.Size = new System.Drawing.Size(172, 22);
            this.tm_Del.Text = "删除";
            this.tm_Del.Visible = false;
            this.tm_Del.Click += new System.EventHandler(this.tm_Del_Click);
            // 
            // 清除电梯占用ToolStripMenuItem
            // 
            this.清除电梯占用ToolStripMenuItem.Name = "清除电梯占用ToolStripMenuItem";
            this.清除电梯占用ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.清除电梯占用ToolStripMenuItem.Text = "清除电梯占用";
            this.清除电梯占用ToolStripMenuItem.Click += new System.EventHandler(this.清除电梯占用ToolStripMenuItem_Click);
            // 
            // 清除全部管控占用ToolStripMenuItem
            // 
            this.清除全部管控占用ToolStripMenuItem.Name = "清除全部管控占用ToolStripMenuItem";
            this.清除全部管控占用ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.清除全部管控占用ToolStripMenuItem.Text = "清除全部管控占用";
            this.清除全部管控占用ToolStripMenuItem.Click += new System.EventHandler(this.清除全部管控占用ToolStripMenuItem_Click);
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
            this.AGV_Remark.FillWeight = 92.978F;
            this.AGV_Remark.HeaderText = "AGV编号";
            this.AGV_Remark.Name = "AGV_Remark";
            this.AGV_Remark.ReadOnly = true;
            // 
            // AGV_IP
            // 
            this.AGV_IP.FillWeight = 142.132F;
            this.AGV_IP.HeaderText = "IP地址";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            // 
            // AGV_AC
            // 
            this.AGV_AC.FillWeight = 92.978F;
            this.AGV_AC.HeaderText = "网络状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            // 
            // AGV_RFID_Now
            // 
            this.AGV_RFID_Now.FillWeight = 92.978F;
            this.AGV_RFID_Now.HeaderText = "当前RFID";
            this.AGV_RFID_Now.Name = "AGV_RFID_Now";
            this.AGV_RFID_Now.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "目标RFID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // AGV_LineNo
            // 
            this.AGV_LineNo.HeaderText = "AGV路线编号";
            this.AGV_LineNo.Name = "AGV_LineNo";
            this.AGV_LineNo.ReadOnly = true;
            this.AGV_LineNo.Visible = false;
            // 
            // AGV_ErrorCord
            // 
            this.AGV_ErrorCord.FillWeight = 92.978F;
            this.AGV_ErrorCord.HeaderText = "AGV故障信息";
            this.AGV_ErrorCord.Name = "AGV_ErrorCord";
            this.AGV_ErrorCord.ReadOnly = true;
            // 
            // AGV_Power
            // 
            this.AGV_Power.FillWeight = 92.978F;
            this.AGV_Power.HeaderText = "AGV电量";
            this.AGV_Power.Name = "AGV_Power";
            this.AGV_Power.ReadOnly = true;
            // 
            // AGV_Speed
            // 
            this.AGV_Speed.FillWeight = 92.978F;
            this.AGV_Speed.HeaderText = "AGV运动状态";
            this.AGV_Speed.Name = "AGV_Speed";
            this.AGV_Speed.ReadOnly = true;
            // 
            // frm_Monitor_Car
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 362);
            this.Controls.Add(this.bt_Manager);
            this.Controls.Add(this.bt_Famart);
            this.Controls.Add(this.dg_CarInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frm_Monitor_Car";
            this.ShowIcon = false;
            this.Text = "车辆监控";
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
        private System.Windows.Forms.ToolStripMenuItem 清除电梯占用ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除全部管控占用ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_RFID_Now;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_LineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ErrorCord;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Power;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Speed;
    }
}