namespace TS_RGB
{
    partial class frm_Add_Elevator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Elec_IP = new System.Windows.Forms.TextBox();
            this.lb_Info = new System.Windows.Forms.Label();
            this.txt_RFID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_Out_Menu = new System.Windows.Forms.ComboBox();
            this.txt_ElecNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dgv_elec = new System.Windows.Forms.DataGridView();
            this.S_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_LineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_FromRFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_FROM_C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_From_F = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.S_To_C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_ToRFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.bt_Ent = new System.Windows.Forms.Button();
            this.lb_RFID_Location_X = new System.Windows.Forms.TextBox();
            this.lb_RFID_Location_Y = new System.Windows.Forms.TextBox();
            this.bt_Frmart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_elec)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(238, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "X坐标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y坐标：";
            // 
            // txt_Elec_IP
            // 
            this.txt_Elec_IP.Location = new System.Drawing.Point(327, 61);
            this.txt_Elec_IP.Name = "txt_Elec_IP";
            this.txt_Elec_IP.Size = new System.Drawing.Size(168, 21);
            this.txt_Elec_IP.TabIndex = 3;
            // 
            // lb_Info
            // 
            this.lb_Info.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Info.ForeColor = System.Drawing.Color.Green;
            this.lb_Info.Location = new System.Drawing.Point(154, 9);
            this.lb_Info.Name = "lb_Info";
            this.lb_Info.Size = new System.Drawing.Size(206, 23);
            this.lb_Info.TabIndex = 8;
            this.lb_Info.Text = "添加成功！";
            this.lb_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_Info.Visible = false;
            // 
            // txt_RFID
            // 
            this.txt_RFID.Location = new System.Drawing.Point(98, 169);
            this.txt_RFID.Name = "txt_RFID";
            this.txt_RFID.Size = new System.Drawing.Size(100, 21);
            this.txt_RFID.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "内部RFID：";
            // 
            // cb_Out_Menu
            // 
            this.cb_Out_Menu.FormattingEnabled = true;
            this.cb_Out_Menu.Items.AddRange(new object[] {
            "前启动",
            "后启动"});
            this.cb_Out_Menu.Location = new System.Drawing.Point(324, 169);
            this.cb_Out_Menu.Name = "cb_Out_Menu";
            this.cb_Out_Menu.Size = new System.Drawing.Size(101, 20);
            this.cb_Out_Menu.TabIndex = 12;
            // 
            // txt_ElecNo
            // 
            this.txt_ElecNo.Location = new System.Drawing.Point(98, 61);
            this.txt_ElecNo.Name = "txt_ElecNo";
            this.txt_ElecNo.Size = new System.Drawing.Size(100, 21);
            this.txt_ElecNo.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "电梯编号：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 172);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "出电梯方向：";
            // 
            // dgv_elec
            // 
            this.dgv_elec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_elec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.S_Name,
            this.S_LineName,
            this.S_FromRFID,
            this.S_FROM_C,
            this.S_From_F,
            this.S_To_C,
            this.S_ToRFID});
            this.dgv_elec.Location = new System.Drawing.Point(6, 255);
            this.dgv_elec.Name = "dgv_elec";
            this.dgv_elec.RowTemplate.Height = 23;
            this.dgv_elec.Size = new System.Drawing.Size(495, 240);
            this.dgv_elec.TabIndex = 20;
            // 
            // S_Name
            // 
            this.S_Name.HeaderText = "逻辑名称";
            this.S_Name.Name = "S_Name";
            // 
            // S_LineName
            // 
            this.S_LineName.HeaderText = "主线编号";
            this.S_LineName.Name = "S_LineName";
            // 
            // S_FromRFID
            // 
            this.S_FromRFID.HeaderText = "入RFID";
            this.S_FromRFID.Name = "S_FromRFID";
            // 
            // S_FROM_C
            // 
            this.S_FROM_C.HeaderText = "入楼层";
            this.S_FROM_C.Name = "S_FROM_C";
            // 
            // S_From_F
            // 
            this.S_From_F.HeaderText = "入方向";
            this.S_From_F.Items.AddRange(new object[] {
            "前启",
            "后启"});
            this.S_From_F.Name = "S_From_F";
            // 
            // S_To_C
            // 
            this.S_To_C.HeaderText = "出楼层";
            this.S_To_C.Name = "S_To_C";
            // 
            // S_ToRFID
            // 
            this.S_ToRFID.HeaderText = "出RFID";
            this.S_ToRFID.Name = "S_ToRFID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "电梯逻辑：";
            // 
            // bt_Ent
            // 
            this.bt_Ent.Location = new System.Drawing.Point(420, 521);
            this.bt_Ent.Name = "bt_Ent";
            this.bt_Ent.Size = new System.Drawing.Size(75, 23);
            this.bt_Ent.TabIndex = 22;
            this.bt_Ent.Text = "确认";
            this.bt_Ent.UseVisualStyleBackColor = true;
            this.bt_Ent.Click += new System.EventHandler(this.bt_Ent_Click);
            // 
            // lb_RFID_Location_X
            // 
            this.lb_RFID_Location_X.Location = new System.Drawing.Point(98, 107);
            this.lb_RFID_Location_X.Name = "lb_RFID_Location_X";
            this.lb_RFID_Location_X.Size = new System.Drawing.Size(100, 21);
            this.lb_RFID_Location_X.TabIndex = 23;
            // 
            // lb_RFID_Location_Y
            // 
            this.lb_RFID_Location_Y.Location = new System.Drawing.Point(327, 107);
            this.lb_RFID_Location_Y.Name = "lb_RFID_Location_Y";
            this.lb_RFID_Location_Y.Size = new System.Drawing.Size(100, 21);
            this.lb_RFID_Location_Y.TabIndex = 24;
            // 
            // bt_Frmart
            // 
            this.bt_Frmart.Location = new System.Drawing.Point(285, 521);
            this.bt_Frmart.Name = "bt_Frmart";
            this.bt_Frmart.Size = new System.Drawing.Size(75, 23);
            this.bt_Frmart.TabIndex = 25;
            this.bt_Frmart.Text = "初始化";
            this.bt_Frmart.UseVisualStyleBackColor = true;
            this.bt_Frmart.Visible = false;
            this.bt_Frmart.Click += new System.EventHandler(this.bt_Frmart_Click);
            // 
            // frm_AddELEC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 556);
            this.Controls.Add(this.bt_Frmart);
            this.Controls.Add(this.lb_RFID_Location_Y);
            this.Controls.Add(this.lb_RFID_Location_X);
            this.Controls.Add(this.bt_Ent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgv_elec);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_ElecNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cb_Out_Menu);
            this.Controls.Add(this.txt_RFID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_Info);
            this.Controls.Add(this.txt_Elec_IP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frm_AddELEC";
            this.Text = "电梯";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_elec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Elec_IP;
        private System.Windows.Forms.Label lb_Info;
        private System.Windows.Forms.TextBox txt_RFID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_Out_Menu;
        private System.Windows.Forms.TextBox txt_ElecNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgv_elec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bt_Ent;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_LineName;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_FromRFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_FROM_C;
        private System.Windows.Forms.DataGridViewComboBoxColumn S_From_F;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_To_C;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_ToRFID;
        private System.Windows.Forms.TextBox lb_RFID_Location_X;
        private System.Windows.Forms.TextBox lb_RFID_Location_Y;
        private System.Windows.Forms.Button bt_Frmart;
    }
}