namespace TS_RGB
{
    partial class frm_Add_RFID
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
            this.txt_RFIDNo = new System.Windows.Forms.TextBox();
            this.lb_RFID_Location_X = new System.Windows.Forms.Label();
            this.lb_RFID_Location_Y = new System.Windows.Forms.Label();
            this.bt_AddRFID = new System.Windows.Forms.Button();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.lb_Info = new System.Windows.Forms.Label();
            this.txt_LastRFID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_Road = new System.Windows.Forms.RadioButton();
            this.rb_Manager = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_PassNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_LineNo = new System.Windows.Forms.TextBox();
            this.cb_ManagerMenu = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "RFID编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "X坐标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y坐标：";
            // 
            // txt_RFIDNo
            // 
            this.txt_RFIDNo.Location = new System.Drawing.Point(172, 94);
            this.txt_RFIDNo.Name = "txt_RFIDNo";
            this.txt_RFIDNo.Size = new System.Drawing.Size(100, 21);
            this.txt_RFIDNo.TabIndex = 2;
            // 
            // lb_RFID_Location_X
            // 
            this.lb_RFID_Location_X.BackColor = System.Drawing.Color.Gainsboro;
            this.lb_RFID_Location_X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_RFID_Location_X.Location = new System.Drawing.Point(170, 136);
            this.lb_RFID_Location_X.Name = "lb_RFID_Location_X";
            this.lb_RFID_Location_X.Size = new System.Drawing.Size(102, 23);
            this.lb_RFID_Location_X.TabIndex = 4;
            this.lb_RFID_Location_X.Text = "label4";
            this.lb_RFID_Location_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_RFID_Location_Y
            // 
            this.lb_RFID_Location_Y.BackColor = System.Drawing.Color.Gainsboro;
            this.lb_RFID_Location_Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_RFID_Location_Y.Location = new System.Drawing.Point(170, 173);
            this.lb_RFID_Location_Y.Name = "lb_RFID_Location_Y";
            this.lb_RFID_Location_Y.Size = new System.Drawing.Size(102, 21);
            this.lb_RFID_Location_Y.TabIndex = 5;
            this.lb_RFID_Location_Y.Text = "label5";
            this.lb_RFID_Location_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_AddRFID
            // 
            this.bt_AddRFID.Location = new System.Drawing.Point(86, 379);
            this.bt_AddRFID.Name = "bt_AddRFID";
            this.bt_AddRFID.Size = new System.Drawing.Size(75, 23);
            this.bt_AddRFID.TabIndex = 8;
            this.bt_AddRFID.Text = "确认";
            this.bt_AddRFID.UseVisualStyleBackColor = true;
            this.bt_AddRFID.Click += new System.EventHandler(this.bt_AddRFID_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Location = new System.Drawing.Point(198, 379);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cancel.TabIndex = 9;
            this.bt_Cancel.Text = "取消";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            // 
            // lb_Info
            // 
            this.lb_Info.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_Info.ForeColor = System.Drawing.Color.Green;
            this.lb_Info.Location = new System.Drawing.Point(88, 9);
            this.lb_Info.Name = "lb_Info";
            this.lb_Info.Size = new System.Drawing.Size(206, 23);
            this.lb_Info.TabIndex = 8;
            this.lb_Info.Text = "添加成功！";
            this.lb_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_Info.Visible = false;
            // 
            // txt_LastRFID
            // 
            this.txt_LastRFID.Location = new System.Drawing.Point(171, 209);
            this.txt_LastRFID.Name = "txt_LastRFID";
            this.txt_LastRFID.Size = new System.Drawing.Size(100, 21);
            this.txt_LastRFID.TabIndex = 3;
            this.txt_LastRFID.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "上一个RFID编号：";
            this.label4.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rb_Road);
            this.panel1.Controls.Add(this.rb_Manager);
            this.panel1.Location = new System.Drawing.Point(107, 236);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 36);
            this.panel1.TabIndex = 11;
            // 
            // rb_Road
            // 
            this.rb_Road.AutoSize = true;
            this.rb_Road.Location = new System.Drawing.Point(103, 12);
            this.rb_Road.Name = "rb_Road";
            this.rb_Road.Size = new System.Drawing.Size(47, 16);
            this.rb_Road.TabIndex = 5;
            this.rb_Road.TabStop = true;
            this.rb_Road.Text = "路径";
            this.rb_Road.UseVisualStyleBackColor = true;
            this.rb_Road.CheckedChanged += new System.EventHandler(this.rb_Road_CheckedChanged);
            // 
            // rb_Manager
            // 
            this.rb_Manager.AutoSize = true;
            this.rb_Manager.Location = new System.Drawing.Point(10, 12);
            this.rb_Manager.Name = "rb_Manager";
            this.rb_Manager.Size = new System.Drawing.Size(47, 16);
            this.rb_Manager.TabIndex = 4;
            this.rb_Manager.TabStop = true;
            this.rb_Manager.Text = "功能";
            this.rb_Manager.UseVisualStyleBackColor = true;
            this.rb_Manager.CheckedChanged += new System.EventHandler(this.rb_Manager_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 293);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "功能选择：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(84, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "交管编号：";
            this.label6.Visible = false;
            // 
            // txt_PassNo
            // 
            this.txt_PassNo.Location = new System.Drawing.Point(170, 325);
            this.txt_PassNo.Name = "txt_PassNo";
            this.txt_PassNo.Size = new System.Drawing.Size(100, 21);
            this.txt_PassNo.TabIndex = 7;
            this.txt_PassNo.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(86, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "路线编号：";
            // 
            // txt_LineNo
            // 
            this.txt_LineNo.Location = new System.Drawing.Point(173, 51);
            this.txt_LineNo.Name = "txt_LineNo";
            this.txt_LineNo.Size = new System.Drawing.Size(100, 21);
            this.txt_LineNo.TabIndex = 1;
            // 
            // cb_ManagerMenu
            // 
            this.cb_ManagerMenu.FormattingEnabled = true;
            this.cb_ManagerMenu.Location = new System.Drawing.Point(171, 290);
            this.cb_ManagerMenu.Name = "cb_ManagerMenu";
            this.cb_ManagerMenu.Size = new System.Drawing.Size(101, 20);
            this.cb_ManagerMenu.TabIndex = 6;
            this.cb_ManagerMenu.SelectedIndexChanged += new System.EventHandler(this.cb_ManagerMenu_SelectedIndexChanged);
            // 
            // frm_AddRFID
            // 
            this.AcceptButton = this.bt_AddRFID;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(359, 445);
            this.Controls.Add(this.cb_ManagerMenu);
            this.Controls.Add(this.txt_LineNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_PassNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txt_LastRFID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_Info);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_AddRFID);
            this.Controls.Add(this.lb_RFID_Location_Y);
            this.Controls.Add(this.lb_RFID_Location_X);
            this.Controls.Add(this.txt_RFIDNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frm_AddRFID";
            this.Text = "RFID";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_RFIDNo;
        private System.Windows.Forms.Label lb_RFID_Location_X;
        private System.Windows.Forms.Label lb_RFID_Location_Y;
        private System.Windows.Forms.Button bt_AddRFID;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.Label lb_Info;
        private System.Windows.Forms.TextBox txt_LastRFID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rb_Road;
        private System.Windows.Forms.RadioButton rb_Manager;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_PassNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_LineNo;
        private System.Windows.Forms.ComboBox cb_ManagerMenu;
    }
}