namespace TS_RGB
{
    partial class frm_Monitor_Elevator
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
            this.dg_ELECInfo = new System.Windows.Forms.DataGridView();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.当前楼层 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.开合状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.电梯内障碍物 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_ELECInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_ELECInfo
            // 
            this.dg_ELECInfo.AllowUserToAddRows = false;
            this.dg_ELECInfo.AllowUserToDeleteRows = false;
            this.dg_ELECInfo.AllowUserToResizeColumns = false;
            this.dg_ELECInfo.AllowUserToResizeRows = false;
            this.dg_ELECInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_ELECInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_ELECInfo.ColumnHeadersHeight = 30;
            this.dg_ELECInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_ELECInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.AGV_IP,
            this.AGV_AC,
            this.当前楼层,
            this.开合状态,
            this.Column1,
            this.电梯内障碍物});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_ELECInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_ELECInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_ELECInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_ELECInfo.Name = "dg_ELECInfo";
            this.dg_ELECInfo.ReadOnly = true;
            this.dg_ELECInfo.RowHeadersWidth = 50;
            this.dg_ELECInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_ELECInfo.RowTemplate.Height = 30;
            this.dg_ELECInfo.Size = new System.Drawing.Size(1245, 152);
            this.dg_ELECInfo.TabIndex = 4;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "电梯编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            // 
            // AGV_IP
            // 
            this.AGV_IP.HeaderText = "IP地址";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            // 
            // AGV_AC
            // 
            this.AGV_AC.HeaderText = "网络状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            // 
            // 当前楼层
            // 
            this.当前楼层.HeaderText = "当前楼层";
            this.当前楼层.Name = "当前楼层";
            this.当前楼层.ReadOnly = true;
            // 
            // 开合状态
            // 
            this.开合状态.HeaderText = "开合状态";
            this.开合状态.Name = "开合状态";
            this.开合状态.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "占用电梯的AGV编号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // 电梯内障碍物
            // 
            this.电梯内障碍物.HeaderText = "电梯内障碍物";
            this.电梯内障碍物.Name = "电梯内障碍物";
            this.电梯内障碍物.ReadOnly = true;
            this.电梯内障碍物.Visible = false;
            // 
            // frm_Monitor_Elevator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 152);
            this.Controls.Add(this.dg_ELECInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frm_Monitor_Elevator";
            this.Text = "电梯监控";
            ((System.ComponentModel.ISupportInitialize)(this.dg_ELECInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_ELECInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当前楼层;
        private System.Windows.Forms.DataGridViewTextBoxColumn 开合状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 电梯内障碍物;
    }
}