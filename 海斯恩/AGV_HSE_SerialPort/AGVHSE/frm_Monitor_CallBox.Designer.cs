namespace TS_RGB
{
    partial class frm_Monitor_CallBox
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
            this.dg_callBoxInfo = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.按钮编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_callBoxInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_callBoxInfo
            // 
            this.dg_callBoxInfo.AllowUserToAddRows = false;
            this.dg_callBoxInfo.AllowUserToDeleteRows = false;
            this.dg_callBoxInfo.AllowUserToResizeColumns = false;
            this.dg_callBoxInfo.AllowUserToResizeRows = false;
            this.dg_callBoxInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_callBoxInfo.ColumnHeadersHeight = 30;
            this.dg_callBoxInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_callBoxInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.AGV_ID,
            this.按钮编号,
            this.AGV_IP,
            this.AGV_AC,
            this.RFID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_callBoxInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_callBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_callBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_callBoxInfo.Name = "dg_callBoxInfo";
            this.dg_callBoxInfo.ReadOnly = true;
            this.dg_callBoxInfo.RowHeadersWidth = 50;
            this.dg_callBoxInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_callBoxInfo.RowTemplate.Height = 30;
            this.dg_callBoxInfo.Size = new System.Drawing.Size(609, 612);
            this.dg_callBoxInfo.TabIndex = 3;
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "呼叫盒编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Width = 80;
            // 
            // 按钮编号
            // 
            this.按钮编号.HeaderText = "按钮编号";
            this.按钮编号.Name = "按钮编号";
            this.按钮编号.ReadOnly = true;
            // 
            // AGV_IP
            // 
            this.AGV_IP.HeaderText = "目标车辆IP";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            this.AGV_IP.Width = 150;
            // 
            // AGV_AC
            // 
            this.AGV_AC.HeaderText = "状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            // 
            // RFID
            // 
            this.RFID.HeaderText = "任务";
            this.RFID.Name = "RFID";
            this.RFID.ReadOnly = true;
            this.RFID.Width = 125;
            // 
            // frm_Monitor_CallBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 612);
            this.Controls.Add(this.dg_callBoxInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frm_Monitor_CallBox";
            this.Text = "呼叫盒监控";
            ((System.ComponentModel.ISupportInitialize)(this.dg_callBoxInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_callBoxInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 按钮编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;

    }
}