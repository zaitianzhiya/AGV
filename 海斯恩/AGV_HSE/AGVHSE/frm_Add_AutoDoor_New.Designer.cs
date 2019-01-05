namespace TS_RGB
{
    partial class frm_Add_AutoDoor_New
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_AutoDoorfo = new System.Windows.Forms.DataGridView();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.bt_AddRFID = new System.Windows.Forms.Button();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AGV_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dg_AutoDoorfo)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_AutoDoorfo
            // 
            this.dg_AutoDoorfo.AllowUserToResizeColumns = false;
            this.dg_AutoDoorfo.AllowUserToResizeRows = false;
            this.dg_AutoDoorfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dg_AutoDoorfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dg_AutoDoorfo.ColumnHeadersHeight = 30;
            this.dg_AutoDoorfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dg_AutoDoorfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.AGV_ID,
            this.Column2,
            this.RFID,
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_AutoDoorfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dg_AutoDoorfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.dg_AutoDoorfo.Location = new System.Drawing.Point(0, 0);
            this.dg_AutoDoorfo.Name = "dg_AutoDoorfo";
            this.dg_AutoDoorfo.RowHeadersWidth = 50;
            this.dg_AutoDoorfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dg_AutoDoorfo.RowTemplate.Height = 30;
            this.dg_AutoDoorfo.Size = new System.Drawing.Size(577, 210);
            this.dg_AutoDoorfo.TabIndex = 7;
            this.dg_AutoDoorfo.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dg_AutoDoorfo_EditingControlShowing);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Location = new System.Drawing.Point(478, 227);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cancel.TabIndex = 9;
            this.bt_Cancel.Text = "清空";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Cancel_Click);
            // 
            // bt_AddRFID
            // 
            this.bt_AddRFID.Location = new System.Drawing.Point(377, 227);
            this.bt_AddRFID.Name = "bt_AddRFID";
            this.bt_AddRFID.Size = new System.Drawing.Size(75, 23);
            this.bt_AddRFID.TabIndex = 8;
            this.bt_AddRFID.Text = "添加";
            this.bt_AddRFID.UseVisualStyleBackColor = true;
            this.bt_AddRFID.Click += new System.EventHandler(this.bt_AddRFID_Click);
            // 
            // Column7
            // 
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column7.HeaderText = "id";
            this.Column7.Name = "Column7";
            this.Column7.Visible = false;
            // 
            // AGV_ID
            // 
            this.AGV_ID.HeaderText = "自动门编号";
            this.AGV_ID.Name = "AGV_ID";
            this.AGV_ID.Width = 75;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "IP地址";
            this.Column2.Name = "Column2";
            this.Column2.Width = 90;
            // 
            // RFID
            // 
            this.RFID.FillWeight = 300F;
            this.RFID.HeaderText = "路线编号";
            this.RFID.Name = "RFID";
            this.RFID.Width = 60;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "入RFID值";
            this.Column1.Name = "Column1";
            this.Column1.Width = 60;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "出RFID值";
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "方向";
            this.Column4.Items.AddRange(new object[] {
            "前进",
            "后退"});
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "X坐标";
            this.Column5.Name = "Column5";
            this.Column5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Y坐标";
            this.Column6.Name = "Column6";
            this.Column6.Width = 60;
            // 
            // frm_Add_AutoDoor_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 266);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.bt_AddRFID);
            this.Controls.Add(this.dg_AutoDoorfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Add_AutoDoor_New";
            this.Text = "自动门";
            ((System.ComponentModel.ISupportInitialize)(this.dg_AutoDoorfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dg_AutoDoorfo;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.Button bt_AddRFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn AGV_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn RFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}