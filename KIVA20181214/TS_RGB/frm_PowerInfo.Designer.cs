namespace TS_RGB
{
    partial class frm_PowerInfo
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
            this.dg_PowerInfo = new System.Windows.Forms.DataGridView();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.电压下限 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_PowerInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_PowerInfo
            // 
            this.dg_PowerInfo.AllowUserToAddRows = false;
            this.dg_PowerInfo.AllowUserToDeleteRows = false;
            this.dg_PowerInfo.AllowUserToResizeColumns = false;
            this.dg_PowerInfo.AllowUserToResizeRows = false;
            this.dg_PowerInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_PowerInfo.ColumnHeadersHeight = 30;
            this.dg_PowerInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_PowerInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.AGV_IP,
            this.RFID,
            this.电压下限,
            this.Column1,
            this.Column2,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_PowerInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_PowerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_PowerInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_PowerInfo.Name = "dg_PowerInfo";
            this.dg_PowerInfo.ReadOnly = true;
            this.dg_PowerInfo.RowHeadersWidth = 50;
            this.dg_PowerInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_PowerInfo.RowTemplate.Height = 30;
            this.dg_PowerInfo.Size = new System.Drawing.Size(763, 596);
            this.dg_PowerInfo.TabIndex = 4;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Visible = false;
            this.AGV_ID.Width = 80;
            // 
            // AGV_IP
            // 
            this.AGV_IP.HeaderText = "IP地址";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            this.AGV_IP.Width = 150;
            // 
            // RFID
            // 
            this.RFID.HeaderText = "电压上限";
            this.RFID.Name = "RFID";
            this.RFID.ReadOnly = true;
            this.RFID.Width = 80;
            // 
            // 电压下限
            // 
            this.电压下限.HeaderText = "电压下限";
            this.电压下限.Name = "电压下限";
            this.电压下限.ReadOnly = true;
            this.电压下限.Width = 80;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "工作状态";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "充电站坐标";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "充电AGV源坐标";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "充电AGV IP";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // frm_PowerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 596);
            this.Controls.Add(this.dg_PowerInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frm_PowerInfo";
            this.Text = "充电站一览";
            ((System.ComponentModel.ISupportInitialize)(this.dg_PowerInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_PowerInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 电压下限;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}