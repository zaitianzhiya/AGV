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
    public partial class frm_Add_RFID : Form
    {
        
        public frm_Add_RFID(string RFIDno,int RFIDLX,int RFIDLY)
        {
            InitializeComponent();
            txt_RFIDNo.Text = RFIDno;
            lb_RFID_Location_X.Text = RFIDLX.ToString();
            lb_RFID_Location_Y.Text = RFIDLY.ToString();
            rb_Road.Checked = true;
        }

        private void bt_AddRFID_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_LineNo.Text) && !string.IsNullOrEmpty(txt_RFIDNo.Text) && !string.IsNullOrEmpty(lb_RFID_Location_X.Text) && !string.IsNullOrEmpty(lb_RFID_Location_Y.Text))
            {
                try
                {
                    try
                    {
                        string RFID_No = txt_RFIDNo.Text.Trim();
                        int RFID_Location_X = Convert.ToInt32(lb_RFID_Location_X.Text);
                        int RFID_Location_Y = Convert.ToInt32(lb_RFID_Location_Y.Text);
                        if (RFID_Location_X > 0 && RFID_Location_Y > 0)
                        {
                            int Type = 2;
                            if (rb_Manager.Checked)
                            {
                                Type = 1;
                            }
                            Function.INSERET_RFID_MAP(RFID_No, RFID_Location_X.ToString(), RFID_Location_Y.ToString(), Type, txt_LastRFID.Text.Trim(), cb_ManagerMenu.SelectedIndex, txt_PassNo.Text.Trim(), txt_LineNo.Text.Trim());
                            //MessageBox.Show("添加成功！", "提示");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("参数错误！", "提示");
                        }
                    }
                    catch (Exception)
                    {
                        lb_Info.Visible = true;
                        lb_Info.Text = "添加失败！";
                        lb_Info.ForeColor = Color.Red;
                    }
                }
                catch (Exception)
                {
                    lb_Info.Visible = true;
                    lb_Info.Text = "坐标值错误！";
                    lb_Info.ForeColor = Color.Red;
                }
           }
        }

        private void rb_Road_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Road.Checked)
            {
                cb_ManagerMenu.Items.Clear();
                cb_ManagerMenu.Items.Add("直行");
                cb_ManagerMenu.Items.Add("左转");
                cb_ManagerMenu.Items.Add("右转");
                cb_ManagerMenu.Items.Add("后退");
                cb_ManagerMenu.SelectedIndex = 0;
            }
        }

        private void rb_Manager_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Manager.Checked)
            {
                cb_ManagerMenu.Items.Clear();
                cb_ManagerMenu.Items.Add("待机点");
                cb_ManagerMenu.Items.Add("交叉开始");
                cb_ManagerMenu.Items.Add("交叉结束");
                cb_ManagerMenu.SelectedIndex = 0;
            }

        }

        private void cb_ManagerMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cb_ManagerMenu.SelectedIndex > 0)
                {
                    label6.Visible = true;
                    txt_PassNo.Visible = true;
                }
                else
                {
                    label6.Visible = false;
                    txt_PassNo.Visible = false;
                }
            }
            catch (Exception)
            {
              
            }
        }
    }
}
