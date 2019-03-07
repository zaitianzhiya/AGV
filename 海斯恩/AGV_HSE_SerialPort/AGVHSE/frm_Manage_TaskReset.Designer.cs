namespace TS_RGB
{
    partial class frm_Manage_TaskReset
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
            this.dg_OptionInfo = new System.Windows.Forms.DataGridView();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cm_Fw = new System.Windows.Forms.ComboBox();
            this.button_fresh = new System.Windows.Forms.Button();
            this.button_del = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_update = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_carNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_RFID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dg_OptionInfo)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dg_OptionInfo
            // 
            this.dg_OptionInfo.AllowUserToAddRows = false;
            this.dg_OptionInfo.AllowUserToDeleteRows = false;
            this.dg_OptionInfo.AllowUserToResizeColumns = false;
            this.dg_OptionInfo.AllowUserToResizeRows = false;
            this.dg_OptionInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_OptionInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dg_OptionInfo.ColumnHeadersHeight = 30;
            this.dg_OptionInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_OptionInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AGV_ID,
            this.Column2,
            this.RFID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_OptionInfo.DefaultCellStyle = dataGridViewCellStyle1;
            this.dg_OptionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dg_OptionInfo.Location = new System.Drawing.Point(0, 0);
            this.dg_OptionInfo.Name = "dg_OptionInfo";
            this.dg_OptionInfo.ReadOnly = true;
            this.dg_OptionInfo.RowHeadersWidth = 50;
            this.dg_OptionInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_OptionInfo.RowTemplate.Height = 30;
            this.dg_OptionInfo.Size = new System.Drawing.Size(414, 502);
            this.dg_OptionInfo.TabIndex = 6;
            this.dg_OptionInfo.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_OptionInfo_CellMouseDoubleClick);
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "重置地标";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.ReadOnly = true;
            this.AGV_ID.Width = 120;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "重置方向";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 120;
            // 
            // RFID
            // 
            this.RFID.FillWeight = 300F;
            this.RFID.HeaderText = "重置AGV编号";
            this.RFID.Name = "RFID";
            this.RFID.ReadOnly = true;
            this.RFID.Width = 120;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cm_Fw);
            this.panel2.Controls.Add(this.button_fresh);
            this.panel2.Controls.Add(this.button_del);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.button_update);
            this.panel2.Controls.Add(this.button_add);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox_carNo);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.textBox_RFID);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 331);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(414, 171);
            this.panel2.TabIndex = 8;
            // 
            // cm_Fw
            // 
            this.cm_Fw.FormattingEnabled = true;
            this.cm_Fw.Items.AddRange(new object[] {
            "前进",
            "后退"});
            this.cm_Fw.Location = new System.Drawing.Point(154, 56);
            this.cm_Fw.Name = "cm_Fw";
            this.cm_Fw.Size = new System.Drawing.Size(178, 20);
            this.cm_Fw.TabIndex = 29;
            this.cm_Fw.Text = "前进";
            // 
            // button_fresh
            // 
            this.button_fresh.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_fresh.Location = new System.Drawing.Point(307, 132);
            this.button_fresh.Name = "button_fresh";
            this.button_fresh.Size = new System.Drawing.Size(75, 23);
            this.button_fresh.TabIndex = 14;
            this.button_fresh.Text = "刷新";
            this.button_fresh.UseVisualStyleBackColor = true;
            this.button_fresh.Click += new System.EventHandler(this.button_fresh_Click);
            // 
            // button_del
            // 
            this.button_del.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_del.Location = new System.Drawing.Point(123, 132);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(75, 23);
            this.button_del.TabIndex = 12;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(83, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "重置地标：";
            // 
            // button_update
            // 
            this.button_update.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_update.Location = new System.Drawing.Point(215, 132);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(75, 23);
            this.button_update.TabIndex = 10;
            this.button_update.Text = "更新";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // button_add
            // 
            this.button_add.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_add.Location = new System.Drawing.Point(31, 132);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 9;
            this.button_add.Text = "添加";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "重置方向：";
            // 
            // textBox_carNo
            // 
            this.textBox_carNo.Location = new System.Drawing.Point(154, 89);
            this.textBox_carNo.Name = "textBox_carNo";
            this.textBox_carNo.Size = new System.Drawing.Size(178, 21);
            this.textBox_carNo.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "重置AGV编号：";
            // 
            // textBox_RFID
            // 
            this.textBox_RFID.Location = new System.Drawing.Point(154, 22);
            this.textBox_RFID.Name = "textBox_RFID";
            this.textBox_RFID.Size = new System.Drawing.Size(178, 21);
            this.textBox_RFID.TabIndex = 16;
            // 
            // frm_Manage_TaskReset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 502);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dg_OptionInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Manage_TaskReset";
            this.Text = "呼叫盒任务重置条件设置";
            ((System.ComponentModel.ISupportInitialize)(this.dg_OptionInfo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_OptionInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_fresh;
        private System.Windows.Forms.Button button_del;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_carNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_RFID;
        private System.Windows.Forms.ComboBox cm_Fw;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;
    }
}