namespace TS_RGB
{
    partial class frm_Add_CallBox
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
            this.txt_CallBoxNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_CallBox_IP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Info = new System.Windows.Forms.Label();
            this.lb_RFID_Location_Y = new System.Windows.Forms.TextBox();
            this.lb_RFID_Location_X = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_Ent = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dgv_CallBox = new System.Windows.Forms.DataGridView();
            this.S_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_CallBoxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_AnJianNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_count = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_InRFID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CallBox)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_CallBoxNo
            // 
            this.txt_CallBoxNo.Location = new System.Drawing.Point(103, 61);
            this.txt_CallBoxNo.Name = "txt_CallBoxNo";
            this.txt_CallBoxNo.Size = new System.Drawing.Size(100, 21);
            this.txt_CallBoxNo.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "呼叫盒编号：";
            // 
            // txt_CallBox_IP
            // 
            this.txt_CallBox_IP.Location = new System.Drawing.Point(336, 61);
            this.txt_CallBox_IP.Name = "txt_CallBox_IP";
            this.txt_CallBox_IP.Size = new System.Drawing.Size(131, 21);
            this.txt_CallBox_IP.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(256, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "IP地址：";
            // 
            // lb_Info
            // 
            this.lb_Info.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Info.ForeColor = System.Drawing.Color.Green;
            this.lb_Info.Location = new System.Drawing.Point(135, 9);
            this.lb_Info.Name = "lb_Info";
            this.lb_Info.Size = new System.Drawing.Size(206, 23);
            this.lb_Info.TabIndex = 23;
            this.lb_Info.Text = "添加成功！";
            this.lb_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_Info.Visible = false;
            // 
            // lb_RFID_Location_Y
            // 
            this.lb_RFID_Location_Y.Location = new System.Drawing.Point(336, 141);
            this.lb_RFID_Location_Y.Name = "lb_RFID_Location_Y";
            this.lb_RFID_Location_Y.Size = new System.Drawing.Size(100, 21);
            this.lb_RFID_Location_Y.TabIndex = 28;
            // 
            // lb_RFID_Location_X
            // 
            this.lb_RFID_Location_X.Location = new System.Drawing.Point(103, 141);
            this.lb_RFID_Location_X.Name = "lb_RFID_Location_X";
            this.lb_RFID_Location_X.Size = new System.Drawing.Size(100, 21);
            this.lb_RFID_Location_X.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "Y坐标：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "X坐标：";
            // 
            // bt_Ent
            // 
            this.bt_Ent.Location = new System.Drawing.Point(430, 465);
            this.bt_Ent.Name = "bt_Ent";
            this.bt_Ent.Size = new System.Drawing.Size(75, 23);
            this.bt_Ent.TabIndex = 35;
            this.bt_Ent.Text = "确认";
            this.bt_Ent.UseVisualStyleBackColor = true;
            this.bt_Ent.Click += new System.EventHandler(this.bt_Ent_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 34;
            this.label5.Text = "按钮逻辑：";
            // 
            // dgv_CallBox
            // 
            this.dgv_CallBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_CallBox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.S_No,
            this.S_CallBoxName,
            this.S_Name,
            this.S_IP,
            this.S_AnJianNo,
            this.S_Task});
            this.dgv_CallBox.Location = new System.Drawing.Point(12, 209);
            this.dgv_CallBox.Name = "dgv_CallBox";
            this.dgv_CallBox.RowTemplate.Height = 23;
            this.dgv_CallBox.Size = new System.Drawing.Size(521, 240);
            this.dgv_CallBox.TabIndex = 33;
            // 
            // S_No
            // 
            this.S_No.HeaderText = "逻辑编号";
            this.S_No.Name = "S_No";
            this.S_No.Visible = false;
            this.S_No.Width = 50;
            // 
            // S_CallBoxName
            // 
            this.S_CallBoxName.HeaderText = "呼叫盒编号";
            this.S_CallBoxName.Name = "S_CallBoxName";
            this.S_CallBoxName.Width = 90;
            // 
            // S_Name
            // 
            this.S_Name.HeaderText = "按钮编号";
            this.S_Name.Name = "S_Name";
            this.S_Name.Width = 80;
            // 
            // S_IP
            // 
            this.S_IP.HeaderText = "目的IP地址";
            this.S_IP.Name = "S_IP";
            this.S_IP.Width = 120;
            // 
            // S_AnJianNo
            // 
            this.S_AnJianNo.HeaderText = "是否按下";
            this.S_AnJianNo.Name = "S_AnJianNo";
            this.S_AnJianNo.Width = 90;
            // 
            // S_Task
            // 
            this.S_Task.HeaderText = "任务";
            this.S_Task.Name = "S_Task";
            this.S_Task.Width = 97;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 36;
            this.label6.Text = "按键数量：";
            // 
            // txt_count
            // 
            this.txt_count.Location = new System.Drawing.Point(103, 102);
            this.txt_count.Name = "txt_count";
            this.txt_count.Size = new System.Drawing.Size(100, 21);
            this.txt_count.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(256, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "生效RFID：";
            // 
            // textBox_InRFID
            // 
            this.textBox_InRFID.Location = new System.Drawing.Point(336, 102);
            this.textBox_InRFID.Name = "textBox_InRFID";
            this.textBox_InRFID.Size = new System.Drawing.Size(100, 21);
            this.textBox_InRFID.TabIndex = 39;
            // 
            // frm_AddCallBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 502);
            this.Controls.Add(this.textBox_InRFID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_count);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bt_Ent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgv_CallBox);
            this.Controls.Add(this.lb_RFID_Location_Y);
            this.Controls.Add(this.lb_RFID_Location_X);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_Info);
            this.Controls.Add(this.txt_CallBoxNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_CallBox_IP);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_AddCallBox";
            this.Text = "呼叫盒";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_CallBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_CallBoxNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_CallBox_IP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Info;
        private System.Windows.Forms.TextBox lb_RFID_Location_Y;
        private System.Windows.Forms.TextBox lb_RFID_Location_X;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_Ent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgv_CallBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_count;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_InRFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_CallBoxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_AnJianNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_Task;
    }
}