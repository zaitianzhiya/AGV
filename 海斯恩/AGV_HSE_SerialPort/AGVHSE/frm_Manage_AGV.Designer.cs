namespace TS_RGB
{
    partial class frm_Manage_AGV
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_AGVInfo = new System.Windows.Forms.DataGridView();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_RFID_Now = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ErrorCord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Power = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AGVInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_AGVInfo
            // 
            this.dg_AGVInfo.AllowUserToAddRows = false;
            this.dg_AGVInfo.AllowUserToDeleteRows = false;
            this.dg_AGVInfo.AllowUserToResizeColumns = false;
            this.dg_AGVInfo.AllowUserToResizeRows = false;
            this.dg_AGVInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_AGVInfo.ColumnHeadersHeight = 30;
            this.dg_AGVInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_AGVInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.AGV_Remark,
            this.AGV_IP,
            this.AGV_AC,
            this.AGV_RFID_Now,
            this.AGV_ErrorCord,
            this.AGV_Power,
            this.AGV_Speed,
            this.Column2,
            this.Column1});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_AGVInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_AGVInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_AGVInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_AGVInfo.Name = "dg_AGVInfo";
            this.dg_AGVInfo.ReadOnly = true;
            this.dg_AGVInfo.RowHeadersWidth = 50;
            this.dg_AGVInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_AGVInfo.RowTemplate.Height = 30;
            this.dg_AGVInfo.Size = new System.Drawing.Size(673, 612);
            this.dg_AGVInfo.TabIndex = 2;
            this.dg_AGVInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_CarInfo_CellContentClick);
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
            this.AGV_Remark.HeaderText = "AGV编号";
            this.AGV_Remark.Name = "AGV_Remark";
            this.AGV_Remark.ReadOnly = true;
            // 
            // AGV_IP
            // 
            this.AGV_IP.HeaderText = "IP地址";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            this.AGV_IP.Width = 120;
            // 
            // AGV_AC
            // 
            this.AGV_AC.HeaderText = "网络状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            // 
            // AGV_RFID_Now
            // 
            this.AGV_RFID_Now.HeaderText = "当前RFID";
            this.AGV_RFID_Now.Name = "AGV_RFID_Now";
            this.AGV_RFID_Now.ReadOnly = true;
            this.AGV_RFID_Now.Visible = false;
            // 
            // AGV_ErrorCord
            // 
            this.AGV_ErrorCord.HeaderText = "AGV故障信息";
            this.AGV_ErrorCord.Name = "AGV_ErrorCord";
            this.AGV_ErrorCord.ReadOnly = true;
            // 
            // AGV_Power
            // 
            this.AGV_Power.HeaderText = "AGV电量";
            this.AGV_Power.Name = "AGV_Power";
            this.AGV_Power.ReadOnly = true;
            // 
            // AGV_Speed
            // 
            this.AGV_Speed.HeaderText = "AGV速度(0~8)";
            this.AGV_Speed.Name = "AGV_Speed";
            this.AGV_Speed.ReadOnly = true;
            this.AGV_Speed.Visible = false;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "方向";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            this.Column2.Width = 80;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "详情";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Text = "详情";
            this.Column1.UseColumnTextForButtonValue = true;
            // 
            // frm_Manage_AGV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 612);
            this.Controls.Add(this.dg_AGVInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Manage_AGV";
            this.Text = "AGV管理";
            ((System.ComponentModel.ISupportInitialize)(this.dg_AGVInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_AGVInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_RFID_Now;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ErrorCord;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Power;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_Speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
    }
}