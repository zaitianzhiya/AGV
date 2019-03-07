using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TS_RGB.Fuction;

namespace TS_RGB
{
    public partial class frm_Add_CallBox : Form
    {
        public frm_Add_CallBox()
        {
            InitializeComponent();
        }
        public frm_Add_CallBox(string C_No, int RFIDLX, int RFIDLY)
        {
            InitializeComponent();
            txt_CallBox_IP.Text = C_No;
            lb_RFID_Location_X.Text = RFIDLX.ToString();
            lb_RFID_Location_Y.Text = RFIDLY.ToString();
            if (C_No != "")
            {
                ShowCallBox(C_No);
            }
        }

        private void ShowCallBox(string C_No)
        {
            //显示上部分（呼叫盒信息）
            DataTable C_Table = Function.SELECT_CallBox_IP(C_No.Substring(1, C_No.Trim().Length - 1));
            if (C_Table != null && C_Table.Rows.Count > 0)
            {
                txt_CallBox_IP.Enabled = false;
                txt_CallBoxNo.Text = C_Table.Rows[0][1].ToString().Trim();
                txt_CallBox_IP.Text = C_Table.Rows[0][2].ToString().Trim();
                txt_count.Text = C_Table.Rows[0][3].ToString().Trim();
                lb_RFID_Location_X.Text = C_Table.Rows[0][4].ToString().Trim();
                lb_RFID_Location_Y.Text = C_Table.Rows[0][5].ToString().Trim();
                textBox_InRFID.Text = C_Table.Rows[0][7].ToString().Trim();
            }

            //显示下部分（按钮逻辑）
            DataTable c_Table = Function.SELECT_CallBoxLogic_IP(C_No.Substring(1, C_No.Trim().Length - 1));
            if (c_Table != null && c_Table.Rows.Count > 0)
            {
                dgv_CallBox.Rows.Add(c_Table.Rows.Count);
                for (int i = 0; i < c_Table.Rows.Count; i++)
                {
                    dgv_CallBox.Rows[i].Cells[0].Value = c_Table.Rows[i][0].ToString().Trim();
                    dgv_CallBox.Rows[i].Cells[1].Value = c_Table.Rows[i][1].ToString().Trim();
                    dgv_CallBox.Rows[i].Cells[2].Value = c_Table.Rows[i][2].ToString().Trim();
                    dgv_CallBox.Rows[i].Cells[3].Value = c_Table.Rows[i][3].ToString().Trim();
                    dgv_CallBox.Rows[i].Cells[4].Value = c_Table.Rows[i][4].ToString().Trim();
                    dgv_CallBox.Rows[i].Cells[5].Value = c_Table.Rows[i][5].ToString().Trim();
                }
            }
        }

        private void bt_Ent_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (txt_count.Text.Trim() != string.Empty)
            {
                count = Convert.ToInt32(txt_count.Text.Trim());
            }
            int dg_count = dgv_CallBox.Rows.Count - 1;
            if (dg_count <= count)
            {
                if (!string.IsNullOrEmpty(txt_CallBoxNo.Text) && !string.IsNullOrEmpty(txt_CallBox_IP.Text)
                   && !string.IsNullOrEmpty(txt_count.Text) && !string.IsNullOrEmpty(lb_RFID_Location_X.Text) && !string.IsNullOrEmpty(lb_RFID_Location_Y.Text))
                {
                    Function.INSERT_CALLBOX(txt_CallBoxNo.Text.ToString().Trim(), txt_CallBox_IP.Text.ToString().Trim(), int.Parse(txt_count.Text), int.Parse(lb_RFID_Location_X.Text), int.Parse(lb_RFID_Location_Y.Text),textBox_InRFID.Text.ToString().Trim());
                    if (dgv_CallBox != null && dgv_CallBox.Rows.Count > 1)
                    {
                        try
                        {
                            for (int i = 0; i < dgv_CallBox.Rows.Count - 1; i++)
                            {
                                Function.INSERT_CallBoxLogic(
                                    dgv_CallBox.Rows[i].Cells[1].Value.ToString(),
                                    dgv_CallBox.Rows[i].Cells[2].Value.ToString(),
                                    dgv_CallBox.Rows[i].Cells[3].Value.ToString(),
                                    dgv_CallBox.Rows[i].Cells[4].Value.ToString(),
                                    dgv_CallBox.Rows[i].Cells[5].Value.ToString());
                            }
                            this.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("参数", "错误");
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else { MessageBox.Show("参数缺失，请填写完整!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
            {
                MessageBox.Show("按钮数量与该呼叫盒所定义的数量不一致，请确认！","警告");
                return;
            }
        }
    }
}
