namespace TS_RGB
{
    partial class frm_Manage_Area
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dg_AreaInfo = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_fresh = new System.Windows.Forms.Button();
            this.textBox_RFIDs = new System.Windows.Forms.TextBox();
            this.button_del = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_update = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button_add = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_note = new System.Windows.Forms.TextBox();
            this.textBox_AreaName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_AreaNo = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AreaInfo)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dg_AreaInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 517);
            this.panel1.TabIndex = 0;
            // 
            // dg_AreaInfo
            // 
            this.dg_AreaInfo.AllowUserToAddRows = false;
            this.dg_AreaInfo.AllowUserToDeleteRows = false;
            this.dg_AreaInfo.AllowUserToResizeColumns = false;
            this.dg_AreaInfo.AllowUserToResizeRows = false;
            this.dg_AreaInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_AreaInfo.ColumnHeadersHeight = 30;
            this.dg_AreaInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_AreaInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.AGV_ID,
            this.Column2,
            this.RFID,
            this.Column1});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_AreaInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_AreaInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_AreaInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_AreaInfo.Name = "dg_AreaInfo";
            this.dg_AreaInfo.ReadOnly = true;
            this.dg_AreaInfo.RowHeadersWidth = 50;
            this.dg_AreaInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_AreaInfo.RowTemplate.Height = 30;
            this.dg_AreaInfo.Size = new System.Drawing.Size(698, 515);
            this.dg_AreaInfo.TabIndex = 5;
            this.dg_AreaInfo.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_AreaInfo_CellMouseDoubleClick);
            // 
            // Column3
            // 
            this.Column3.HeaderText = "id";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Visible = false;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "区域编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Width = 75;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "区域名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 160;
            // 
            // RFID
            // 
            this.RFID.FillWeight = 300F;
            this.RFID.HeaderText = "所含RFID";
            this.RFID.Name = "RFID";
            this.RFID.ReadOnly = true;
            this.RFID.Width = 260;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "备注";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 505);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 107);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_fresh);
            this.panel3.Controls.Add(this.textBox_RFIDs);
            this.panel3.Controls.Add(this.button_del);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.button_update);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.button_add);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.textBox_note);
            this.panel3.Controls.Add(this.textBox_AreaName);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.textBox_AreaNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(698, 105);
            this.panel3.TabIndex = 8;
            // 
            // button_fresh
            // 
            this.button_fresh.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_fresh.Location = new System.Drawing.Point(539, 73);
            this.button_fresh.Name = "button_fresh";
            this.button_fresh.Size = new System.Drawing.Size(75, 23);
            this.button_fresh.TabIndex = 3;
            this.button_fresh.Text = "刷新";
            this.button_fresh.UseVisualStyleBackColor = true;
            this.button_fresh.Click += new System.EventHandler(this.button_fresh_Click);
            // 
            // textBox_RFIDs
            // 
            this.textBox_RFIDs.Location = new System.Drawing.Point(104, 44);
            this.textBox_RFIDs.Name = "textBox_RFIDs";
            this.textBox_RFIDs.Size = new System.Drawing.Size(560, 21);
            this.textBox_RFIDs.TabIndex = 7;
            // 
            // button_del
            // 
            this.button_del.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_del.Location = new System.Drawing.Point(394, 73);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(75, 23);
            this.button_del.TabIndex = 2;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "区域编号：";
            // 
            // button_update
            // 
            this.button_update.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_update.Location = new System.Drawing.Point(249, 73);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(75, 23);
            this.button_update.TabIndex = 1;
            this.button_update.Text = "更新";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "所含RFID：";
            // 
            // button_add
            // 
            this.button_add.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_add.Location = new System.Drawing.Point(104, 73);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 0;
            this.button_add.Text = "添加";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "区域名称：";
            // 
            // textBox_note
            // 
            this.textBox_note.Location = new System.Drawing.Point(496, 17);
            this.textBox_note.Name = "textBox_note";
            this.textBox_note.Size = new System.Drawing.Size(168, 21);
            this.textBox_note.TabIndex = 6;
            // 
            // textBox_AreaName
            // 
            this.textBox_AreaName.Location = new System.Drawing.Point(260, 17);
            this.textBox_AreaName.Name = "textBox_AreaName";
            this.textBox_AreaName.Size = new System.Drawing.Size(187, 21);
            this.textBox_AreaName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(462, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "备注：";
            // 
            // textBox_AreaNo
            // 
            this.textBox_AreaNo.Location = new System.Drawing.Point(104, 17);
            this.textBox_AreaNo.Name = "textBox_AreaNo";
            this.textBox_AreaNo.Size = new System.Drawing.Size(73, 21);
            this.textBox_AreaNo.TabIndex = 4;
            // 
            // frm_Manage_Area
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 612);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Manage_Area";
            this.Text = "区域管理";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg_AreaInfo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dg_AreaInfo;
        private System.Windows.Forms.TextBox textBox_RFIDs;
        private System.Windows.Forms.TextBox textBox_note;
        private System.Windows.Forms.TextBox textBox_AreaName;
        private System.Windows.Forms.TextBox textBox_AreaNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_fresh;
        private System.Windows.Forms.Button button_del;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}