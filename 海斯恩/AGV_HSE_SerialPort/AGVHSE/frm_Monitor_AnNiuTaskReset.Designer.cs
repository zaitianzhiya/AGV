namespace TS_RGB
{
    partial class frm_Monitor_AnNiuTaskReset
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
            this.dg_AnNiuTaskReset = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.按钮编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AnNiuTaskReset)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_AnNiuTaskReset
            // 
            this.dg_AnNiuTaskReset.AllowUserToAddRows = false;
            this.dg_AnNiuTaskReset.AllowUserToDeleteRows = false;
            this.dg_AnNiuTaskReset.AllowUserToResizeColumns = false;
            this.dg_AnNiuTaskReset.AllowUserToResizeRows = false;
            this.dg_AnNiuTaskReset.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_AnNiuTaskReset.ColumnHeadersHeight = 30;
            this.dg_AnNiuTaskReset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_AnNiuTaskReset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.AGV_ID,
            this.按钮编号,
            this.AGV_AC});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_AnNiuTaskReset.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_AnNiuTaskReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_AnNiuTaskReset.Location = new System.Drawing.Point(0, 0);
            this.dg_AnNiuTaskReset.Name = "dg_AnNiuTaskReset";
            this.dg_AnNiuTaskReset.ReadOnly = true;
            this.dg_AnNiuTaskReset.RowHeadersWidth = 50;
            this.dg_AnNiuTaskReset.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_AnNiuTaskReset.RowTemplate.Height = 30;
            this.dg_AnNiuTaskReset.Size = new System.Drawing.Size(412, 612);
            this.dg_AnNiuTaskReset.TabIndex = 4;
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
            this.AGV_ID.HeaderText = "AGV编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Width = 120;
            // 
            // 按钮编号
            // 
            this.按钮编号.HeaderText = "IP地址";
            this.按钮编号.Name = "按钮编号";
            this.按钮编号.ReadOnly = true;
            this.按钮编号.Width = 120;
            // 
            // AGV_AC
            // 
            this.AGV_AC.HeaderText = "任务状态";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            this.AGV_AC.Width = 120;
            // 
            // frm_Monitor_AnNiuTaskReset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 612);
            this.Controls.Add(this.dg_AnNiuTaskReset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frm_Monitor_AnNiuTaskReset";
            this.Text = "按钮任务监控";
            ((System.ComponentModel.ISupportInitialize)(this.dg_AnNiuTaskReset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_AnNiuTaskReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 按钮编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
    }
}