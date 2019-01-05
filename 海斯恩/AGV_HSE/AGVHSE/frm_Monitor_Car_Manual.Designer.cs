namespace TS_RGB
{
    partial class frm_Monitor_Car_Manual
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
            this.bt_Stop = new System.Windows.Forms.Button();
            this.bt_Down = new System.Windows.Forms.Button();
            this.lb_CarAC = new System.Windows.Forms.Label();
            this.bt_Up = new System.Windows.Forms.Button();
            this.lb_CarNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_M_Auto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_Stop
            // 
            this.bt_Stop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Stop.Location = new System.Drawing.Point(284, 276);
            this.bt_Stop.Name = "bt_Stop";
            this.bt_Stop.Size = new System.Drawing.Size(80, 50);
            this.bt_Stop.TabIndex = 19;
            this.bt_Stop.Text = "停止";
            this.bt_Stop.UseVisualStyleBackColor = true;
            this.bt_Stop.Click += new System.EventHandler(this.bt_Stop_Click);
            // 
            // bt_Down
            // 
            this.bt_Down.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Down.Location = new System.Drawing.Point(158, 276);
            this.bt_Down.Name = "bt_Down";
            this.bt_Down.Size = new System.Drawing.Size(80, 50);
            this.bt_Down.TabIndex = 18;
            this.bt_Down.Text = "右移动-->";
            this.bt_Down.UseVisualStyleBackColor = true;
            this.bt_Down.Click += new System.EventHandler(this.bt_Down_Click);
            // 
            // lb_CarAC
            // 
            this.lb_CarAC.BackColor = System.Drawing.Color.Yellow;
            this.lb_CarAC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_CarAC.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lb_CarAC.ForeColor = System.Drawing.Color.Black;
            this.lb_CarAC.Location = new System.Drawing.Point(284, 37);
            this.lb_CarAC.Name = "lb_CarAC";
            this.lb_CarAC.Size = new System.Drawing.Size(80, 23);
            this.lb_CarAC.TabIndex = 14;
            this.lb_CarAC.Text = "手动中";
            this.lb_CarAC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bt_Up
            // 
            this.bt_Up.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Up.Location = new System.Drawing.Point(41, 276);
            this.bt_Up.Name = "bt_Up";
            this.bt_Up.Size = new System.Drawing.Size(80, 50);
            this.bt_Up.TabIndex = 13;
            this.bt_Up.Text = "左移动<--";
            this.bt_Up.UseVisualStyleBackColor = true;
            this.bt_Up.Click += new System.EventHandler(this.bt_Up_Click);
            // 
            // lb_CarNo
            // 
            this.lb_CarNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_CarNo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lb_CarNo.ForeColor = System.Drawing.Color.OrangeRed;
            this.lb_CarNo.Location = new System.Drawing.Point(158, 37);
            this.lb_CarNo.Name = "lb_CarNo";
            this.lb_CarNo.Size = new System.Drawing.Size(80, 23);
            this.lb_CarNo.TabIndex = 12;
            this.lb_CarNo.Text = "001";
            this.lb_CarNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.Location = new System.Drawing.Point(37, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 11;
            this.label1.Text = "小车编号：";
            // 
            // bt_M_Auto
            // 
            this.bt_M_Auto.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_M_Auto.Location = new System.Drawing.Point(158, 136);
            this.bt_M_Auto.Name = "bt_M_Auto";
            this.bt_M_Auto.Size = new System.Drawing.Size(80, 50);
            this.bt_M_Auto.TabIndex = 20;
            this.bt_M_Auto.Text = "手/自动";
            this.bt_M_Auto.UseVisualStyleBackColor = true;
            this.bt_M_Auto.Click += new System.EventHandler(this.bt_M_Auto_Click);
            // 
            // frm_CarManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 378);
            this.Controls.Add(this.bt_M_Auto);
            this.Controls.Add(this.bt_Stop);
            this.Controls.Add(this.bt_Down);
            this.Controls.Add(this.lb_CarAC);
            this.Controls.Add(this.bt_Up);
            this.Controls.Add(this.lb_CarNo);
            this.Controls.Add(this.label1);
            this.Name = "frm_CarManual";
            this.Text = "手动操作";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Stop;
        private System.Windows.Forms.Button bt_Down;
        private System.Windows.Forms.Label lb_CarAC;
        private System.Windows.Forms.Button bt_Up;
        private System.Windows.Forms.Label lb_CarNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_M_Auto;
    }
}