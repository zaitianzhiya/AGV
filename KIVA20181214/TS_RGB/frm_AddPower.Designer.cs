namespace TS_RGB
{
    partial class frm_AddPower
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
            this.txt_InRFID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_PowerLow = new System.Windows.Forms.TextBox();
            this.txt_PowerNo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_PowerHigh = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_LineNo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_IP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cm_IN_F = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_OutLineNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_InLineNo = new System.Windows.Forms.TextBox();
            this.txt_Out_RFID = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cm_OUT_F = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "RFID编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(102, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "X坐标：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y坐标：";
            // 
            // txt_RFIDNo
            // 
            this.txt_RFIDNo.Location = new System.Drawing.Point(172, 156);
            this.txt_RFIDNo.Name = "txt_RFIDNo";
            this.txt_RFIDNo.Size = new System.Drawing.Size(100, 21);
            this.txt_RFIDNo.TabIndex = 3;
            // 
            // lb_RFID_Location_X
            // 
            this.lb_RFID_Location_X.BackColor = System.Drawing.Color.Gainsboro;
            this.lb_RFID_Location_X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_RFID_Location_X.Location = new System.Drawing.Point(172, 194);
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
            this.lb_RFID_Location_Y.Location = new System.Drawing.Point(172, 232);
            this.lb_RFID_Location_Y.Name = "lb_RFID_Location_Y";
            this.lb_RFID_Location_Y.Size = new System.Drawing.Size(102, 21);
            this.lb_RFID_Location_Y.TabIndex = 5;
            this.lb_RFID_Location_Y.Text = "label5";
            this.lb_RFID_Location_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_AddRFID
            // 
            this.bt_AddRFID.Location = new System.Drawing.Point(74, 578);
            this.bt_AddRFID.Name = "bt_AddRFID";
            this.bt_AddRFID.Size = new System.Drawing.Size(75, 23);
            this.bt_AddRFID.TabIndex = 6;
            this.bt_AddRFID.Text = "确认";
            this.bt_AddRFID.UseVisualStyleBackColor = true;
            this.bt_AddRFID.Click += new System.EventHandler(this.bt_AddRFID_Click);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Location = new System.Drawing.Point(200, 578);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cancel.TabIndex = 7;
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
            // txt_InRFID
            // 
            this.txt_InRFID.Location = new System.Drawing.Point(172, 274);
            this.txt_InRFID.Name = "txt_InRFID";
            this.txt_InRFID.Size = new System.Drawing.Size(100, 21);
            this.txt_InRFID.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "入RFID编号：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(70, 362);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "充电路径号：";
            // 
            // txt_PowerLow
            // 
            this.txt_PowerLow.Location = new System.Drawing.Point(174, 421);
            this.txt_PowerLow.Name = "txt_PowerLow";
            this.txt_PowerLow.Size = new System.Drawing.Size(100, 21);
            this.txt_PowerLow.TabIndex = 16;
            // 
            // txt_PowerNo
            // 
            this.txt_PowerNo.Location = new System.Drawing.Point(172, 51);
            this.txt_PowerNo.Name = "txt_PowerNo";
            this.txt_PowerNo.Size = new System.Drawing.Size(100, 21);
            this.txt_PowerNo.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(72, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "充电站编号：";
            // 
            // txt_PowerHigh
            // 
            this.txt_PowerHigh.Location = new System.Drawing.Point(174, 394);
            this.txt_PowerHigh.Name = "txt_PowerHigh";
            this.txt_PowerHigh.Size = new System.Drawing.Size(100, 21);
            this.txt_PowerHigh.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(82, 394);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "电量上值：";
            // 
            // txt_LineNo
            // 
            this.txt_LineNo.Location = new System.Drawing.Point(172, 121);
            this.txt_LineNo.Name = "txt_LineNo";
            this.txt_LineNo.Size = new System.Drawing.Size(100, 21);
            this.txt_LineNo.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(82, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 21;
            this.label8.Text = "路线编号：";
            // 
            // txt_IP
            // 
            this.txt_IP.Location = new System.Drawing.Point(172, 87);
            this.txt_IP.Name = "txt_IP";
            this.txt_IP.Size = new System.Drawing.Size(100, 21);
            this.txt_IP.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(82, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "IP 地址：";
            // 
            // cm_IN_F
            // 
            this.cm_IN_F.FormattingEnabled = true;
            this.cm_IN_F.Items.AddRange(new object[] {
            "前进",
            "后退"});
            this.cm_IN_F.Location = new System.Drawing.Point(174, 321);
            this.cm_IN_F.Name = "cm_IN_F";
            this.cm_IN_F.Size = new System.Drawing.Size(100, 20);
            this.cm_IN_F.TabIndex = 28;
            this.cm_IN_F.Text = "前进";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(96, 324);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 27;
            this.label10.Text = "入方向：";
            // 
            // txt_OutLineNo
            // 
            this.txt_OutLineNo.Location = new System.Drawing.Point(174, 455);
            this.txt_OutLineNo.Name = "txt_OutLineNo";
            this.txt_OutLineNo.Size = new System.Drawing.Size(100, 21);
            this.txt_OutLineNo.TabIndex = 32;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(48, 464);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 12);
            this.label12.TabIndex = 31;
            this.label12.Text = "充电完成路径号：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(84, 424);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 33;
            this.label13.Text = "电量下值：";
            // 
            // txt_InLineNo
            // 
            this.txt_InLineNo.Location = new System.Drawing.Point(174, 359);
            this.txt_InLineNo.Name = "txt_InLineNo";
            this.txt_InLineNo.Size = new System.Drawing.Size(100, 21);
            this.txt_InLineNo.TabIndex = 34;
            // 
            // txt_Out_RFID
            // 
            this.txt_Out_RFID.Location = new System.Drawing.Point(174, 490);
            this.txt_Out_RFID.Name = "txt_Out_RFID";
            this.txt_Out_RFID.Size = new System.Drawing.Size(100, 21);
            this.txt_Out_RFID.TabIndex = 36;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(60, 499);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 35;
            this.label11.Text = "出充电区RFID：";
            // 
            // cm_OUT_F
            // 
            this.cm_OUT_F.FormattingEnabled = true;
            this.cm_OUT_F.Items.AddRange(new object[] {
            "前进",
            "后退"});
            this.cm_OUT_F.Location = new System.Drawing.Point(172, 528);
            this.cm_OUT_F.Name = "cm_OUT_F";
            this.cm_OUT_F.Size = new System.Drawing.Size(100, 20);
            this.cm_OUT_F.TabIndex = 38;
            this.cm_OUT_F.Text = "前进";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(88, 531);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 37;
            this.label14.Text = "出方向：";
            // 
            // frm_AddPower
            // 
            this.AcceptButton = this.bt_AddRFID;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(344, 628);
            this.Controls.Add(this.cm_OUT_F);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txt_Out_RFID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_InLineNo);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txt_OutLineNo);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cm_IN_F);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txt_IP);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txt_LineNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_PowerHigh);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_PowerNo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_PowerLow);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_InRFID);
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
            this.Name = "frm_AddPower";
            this.Text = "增加充电点";
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
        private System.Windows.Forms.TextBox txt_InRFID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_PowerLow;
        private System.Windows.Forms.TextBox txt_PowerNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_PowerHigh;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_LineNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_IP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cm_IN_F;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_OutLineNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_InLineNo;
        private System.Windows.Forms.TextBox txt_Out_RFID;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cm_OUT_F;
        private System.Windows.Forms.Label label14;
    }
}