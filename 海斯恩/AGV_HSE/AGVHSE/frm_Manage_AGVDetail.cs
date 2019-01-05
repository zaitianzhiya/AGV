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
    public partial class frm_Manage_AGVDetail : Form
    {
        frm_Manage_CrossInfo frm_crossInfo = new frm_Manage_CrossInfo();
        //路口：1
        string agvNo1 = "";
        string areaNo1 = "";
        string inRFID1 = "";
        int inDir1 = 0;
        string outRFID1 = "";
        int outDir1 = 0;
        //电梯：2
        string oagv = "";
        //int otype = 0;
        string ostring = "";

        frm_Login frm_login = null;

        public frm_Manage_AGVDetail(string no, string ip, string rfid, string err, string ac, string power, string speed, string dir)
        {
            InitializeComponent();
            textBox_BagvNo.Text = no;
            textBox_BIP.Text = ip;
            textBox_BnowRFID.Text = rfid;
            textBox_Berr.Text = err;
            textBox_Bac.Text = ac;
            textBox_Bpower.Text = power;
            textBox_Bspeed.Text = speed;
            textBox_Bdir.Text = dir;

            textBox_ORD_agvNo.Text = no;
            textBox_AD_agvNo.Text = no;
            textBox_E_agvNo.Text = no;
            textBox_P_AgvNo.Text = no;
        }

        //清除管控占用
        private void button_down_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            if (MessageBox.Show("确定清除管控占用吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                int res_cross = Function.UPDATE_Cross_cac_agv(int.Parse(textBox_BagvNo.Text));
                if (res_cross != 0)
                {
                    MessageBox.Show("管控占用已清除，请关注车辆及路口状况！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("清除失败或不存在管控占用，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        //清除电梯占用
        private void button11_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            if (MessageBox.Show("确定清除电梯占用吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                int res_elc = Function.UPDATE_ELC_InAGVNo(int.Parse(textBox_BagvNo.Text));

                if (res_elc != 0)
                {
                    MessageBox.Show("电梯占用已清除，请关注车辆及电梯状况！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("清除失败或不存在电梯占用，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        //添加交叉路口管控和任务
        private void button1_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            agvNo1 = textBox_ORD_agvNo.Text.ToString().Trim();
            areaNo1 = textBox_ORD_AreaNo.Text.ToString().Trim();
            inRFID1 = textBox_ORD_INrfid.Text.ToString().Trim();
            inDir1 = comboBox_ORD_INdir.SelectedIndex;
            outRFID1 = textBox_ORD_OUTrfid.Text.ToString().Trim();
            outDir1 = comboBox_ORD_OUTdir.SelectedIndex;

            //cross  order
            if (!string.IsNullOrEmpty(agvNo1) && !string.IsNullOrEmpty(areaNo1))
            {
                try
                {
                    int res = Function.INSERT_CrossingANDOrder_hand(agvNo1, areaNo1, inRFID1, inDir1, outRFID1, outDir1);
                    if (res > 0)
                    { MessageBox.Show("交叉路口管控添加成功！", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    { MessageBox.Show("交叉路口管控添加失败...", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("参数", "错误");
                }
            }
            else { MessageBox.Show("参数缺失，请填写完整...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //private void textBox_AD_INRFID_Validated(object sender, EventArgs e)
        //{
        //    string outRFID = textBox_AD_INRFID.Text.ToString().Trim();
        //    if (!string.IsNullOrEmpty(outRFID))
        //    {
        //        outRFID = "," + outRFID + ",";
        //    }
        //    else
        //    {
        //        MessageBox.Show("进区域地标不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    textBox_AD_OUTRFID.Text = outRFID;
        //}

        private void button4_Click(object sender, EventArgs e)
        {
            frm_crossInfo = new frm_Manage_CrossInfo();
            frm_crossInfo.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            frm_Manage_OrderInfo f_M_O = new frm_Manage_OrderInfo();
            f_M_O.Show();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            frm_Manage_OrderInfo f_M_O = new frm_Manage_OrderInfo();
            f_M_O.Show();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            frm_Manage_OrderInfo f_M_O = new frm_Manage_OrderInfo();
            f_M_O.Show();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            frm_Manage_OrderInfo f_M_O = new frm_Manage_OrderInfo();
            f_M_O.Show();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string res = "已使用的区域号有：" + "\r\n";
            DataTable cno = Function.SELECT_USED_CNO_INCROSSING();
            if (cno != null && cno.Rows.Count > 0)
            {
                for (int i = 0; i < cno.Rows.Count; i++)
                {
                    if (i < cno.Rows.Count - 1)
                    { res += cno.Rows[i][0] + ","; }
                    else
                    { res += cno.Rows[i][0]; }
                }
                MessageBox.Show(res, "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("查询无结果！", "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }
        private void linkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }

        //添加电梯交互任务
        private void button3_Click(object sender, EventArgs e)
        {
            oagv=textBox_E_agvNo.Text.ToString().Trim();
            ostring = textBox_E_AreaNo.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(ostring))
            {
                try
                {
                    int res = Function.INSERT_ORDER_Info_FROM_AGV(oagv, 2, ostring);
                    if (res > 0)
                    { MessageBox.Show("电梯交互任务添加成功！", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    { MessageBox.Show("电梯交互任务添加失败...", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("参数", "错误");
                }
            }
            else { MessageBox.Show("参数缺失，请填写完整...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //自动门交互任务
        private void button7_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_AD_ADoorIP.Text.ToString().Trim()))
            {
                try
                {
                    int res = Function.INSERT_ORDER_Info_FROM_AGV(textBox_AD_agvNo.Text.ToString().Trim(), 3, textBox_AD_ADoorIP.Text.ToString().Trim());
                    if (res > 0)
                    { MessageBox.Show("自动门交互任务添加成功！", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    { MessageBox.Show("自动门交互任务添加失败...", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("参数", "错误");
                }
            }
            else { MessageBox.Show("参数缺失，请填写完整...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //充电站交互任务
        private void button8_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_P_PowerIP.Text.ToString().Trim()))
            {
                try
                {
                    int res = Function.INSERT_ORDER_Info_FROM_AGV(textBox_P_AgvNo.Text.ToString().Trim(), 4, textBox_P_PowerIP.Text.ToString().Trim());
                    if (res > 0)
                    { MessageBox.Show("充电站交互任务添加成功！", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    { MessageBox.Show("充电站交互任务添加失败...", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("参数", "错误");
                }
            }
            else { MessageBox.Show("参数缺失，请填写完整...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        
        }

        //只添加任务
        private void button9_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_ORD_AreaNo.Text.ToString().Trim()))
            {
                try
                {
                    int res = Function.INSERT_ORDER_Info_FROM_AGV(textBox_ORD_agvNo.Text.ToString().Trim(), 1, textBox_ORD_AreaNo.Text.ToString().Trim());
                    if (res > 0)
                    { MessageBox.Show("AGV交互任务添加成功！", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    { MessageBox.Show("AGV交互任务添加失败...", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("参数", "错误");
                }
            }
            else { MessageBox.Show("参数缺失，请填写完整...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //只添加交叉路口管控
        private void button12_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            areaNo1 = textBox_ORD_AreaNo.Text.ToString().Trim();
            inRFID1 = textBox_ORD_INrfid.Text.ToString().Trim();
            inDir1 = comboBox_ORD_INdir.SelectedIndex;
            outRFID1 = textBox_ORD_OUTrfid.Text.ToString().Trim();
            outDir1 = comboBox_ORD_OUTdir.SelectedIndex;
            if (!string.IsNullOrEmpty(areaNo1))
            {
                try
                {
                    int res = Function.INSERT_Crossing_hand(areaNo1, inRFID1, inDir1, outRFID1, outDir1);
                    if (res > 0)
                    { MessageBox.Show("路口管控添加成功！", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    { MessageBox.Show("路口管控添加失败...", "添加失败", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception)
                {
                    MessageBox.Show("参数", "错误");
                }
            }
            else { MessageBox.Show("参数缺失，请填写完整...", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        

        
    }
}
