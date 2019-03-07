namespace TS_RGB
{
    partial class frm_Manage_CrossInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_CrossInfo = new System.Windows.Forms.DataGridView();
            this.cid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_AC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_clear = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dg_CrossInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_CrossInfo
            // 
            this.dg_CrossInfo.AllowUserToAddRows = false;
            this.dg_CrossInfo.AllowUserToDeleteRows = false;
            this.dg_CrossInfo.AllowUserToResizeColumns = false;
            this.dg_CrossInfo.AllowUserToResizeRows = false;
            this.dg_CrossInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_CrossInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_CrossInfo.ColumnHeadersHeight = 30;
            this.dg_CrossInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_CrossInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cid,
            this.AGV_ID,
            this.AGV_IP,
            this.AGV_AC,
            this.RFID,
            this.Column1,
            this.Column2,
            this.Column_clear,
            this.Column3});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_CrossInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.dg_CrossInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.dg_CrossInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_CrossInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dg_CrossInfo.Name = "dg_CrossInfo";
            this.dg_CrossInfo.ReadOnly = true;
            this.dg_CrossInfo.RowHeadersWidth = 50;
            this.dg_CrossInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_CrossInfo.RowTemplate.Height = 30;
            this.dg_CrossInfo.Size = new System.Drawing.Size(1081, 954);
            this.dg_CrossInfo.TabIndex = 4;
            this.dg_CrossInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_CrossInfo_CellContentClick);
            // 
            // cid
            // 
            this.cid.HeaderText = "cid";
            this.cid.Name = "cid";
            this.cid.ReadOnly = true;
            this.cid.Visible = false;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "区域编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            // 
            // AGV_IP
            // 
            this.AGV_IP.HeaderText = "进区域RFID";
            this.AGV_IP.Name = "AGV_IP";
            this.AGV_IP.ReadOnly = true;
            // 
            // AGV_AC
            // 
            this.AGV_AC.HeaderText = "进区域方向";
            this.AGV_AC.Name = "AGV_AC";
            this.AGV_AC.ReadOnly = true;
            // 
            // RFID
            // 
            this.RFID.HeaderText = "出区域RFID";
            this.RFID.Name = "RFID";
            this.RFID.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "出区域方向";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column2.HeaderText = "区域内车辆编号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column_clear
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.Column_clear.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_clear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Column_clear.HeaderText = "清除此条交管占用";
            this.Column_clear.Name = "Column_clear";
            this.Column_clear.ReadOnly = true;
            this.Column_clear.Text = "清除占用";
            this.Column_clear.UseColumnTextForButtonValue = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "删除此条交管信息";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Text = "删除交管";
            this.Column3.UseColumnTextForButtonValue = true;
            this.Column3.Visible = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(367, 961);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 35);
            this.button1.TabIndex = 5;
            this.button1.Text = "刷新数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(488, 970);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "当前系统共设置了N条路口管控";
            // 
            // frm_Manage_CrossInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1081, 1005);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dg_CrossInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Manage_CrossInfo";
            this.Text = "路口交通管理";
            ((System.ComponentModel.ISupportInitialize)(this.dg_CrossInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_CrossInfo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cid;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_AC;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewButtonColumn Column_clear;
        private System.Windows.Forms.DataGridViewButtonColumn Column3;
    }
}