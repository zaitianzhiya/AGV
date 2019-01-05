namespace TS_RGB
{
    partial class frm_Monitor_AutoDoor
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
            this.dg_AutoDoorInfo = new System.Windows.Forms.DataGridView();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AutoDoorInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_AutoDoorInfo
            // 
            this.dg_AutoDoorInfo.AllowUserToAddRows = false;
            this.dg_AutoDoorInfo.AllowUserToDeleteRows = false;
            this.dg_AutoDoorInfo.AllowUserToResizeColumns = false;
            this.dg_AutoDoorInfo.AllowUserToResizeRows = false;
            this.dg_AutoDoorInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_AutoDoorInfo.ColumnHeadersHeight = 30;
            this.dg_AutoDoorInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_AutoDoorInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.AGV_IP,
            this.AGV_AC});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_AutoDoorInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_AutoDoorInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_AutoDoorInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_AutoDoorInfo.Name = "dg_AutoDoorInfo";
            this.dg_AutoDoorInfo.ReadOnly = true;
            this.dg_AutoDoorInfo.RowHeadersWidth = 50;
            this.dg_AutoDoorInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_AutoDoorInfo.RowTemplate.Height = 30;
            this.dg_AutoDoorInfo.Size = new System.Drawing.Size(450, 612);
            this.dg_AutoDoorInfo.TabIndex = 2;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "自动门编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Width = 80;
            // 
            // AGV_IP
            // 
            this.AGV_IP.HeaderText = "IP地址";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            this.AGV_IP.Width = 200;
            // 
            // AGV_AC
            // 
            this.AGV_AC.HeaderText = "网络状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            this.AGV_AC.Width = 118;
            // 
            // frm_Monitor_AutoDoor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 612);
            this.Controls.Add(this.dg_AutoDoorInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frm_Monitor_AutoDoor";
            this.Text = "自动门状态";
            ((System.ComponentModel.ISupportInitialize)(this.dg_AutoDoorInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_AutoDoorInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
    }
}