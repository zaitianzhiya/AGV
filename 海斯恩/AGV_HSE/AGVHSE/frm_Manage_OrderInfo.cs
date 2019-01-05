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
    public partial class frm_Manage_OrderInfo : Form
    {
        frm_Login frm_login = null;
        public frm_Manage_OrderInfo()
        {
            InitializeComponent();
            GetInfo();
        }
        public void GetInfo()
        {
            DataTable Cross_Info = Function.SELECT_ORDER_INFO();
            if (Cross_Info != null && Cross_Info.Rows.Count > 0)
            {
                label2.Text = "当前共配置了 " + Cross_Info.Rows.Count.ToString() + " 条AGV与其他设备的交互任务";
                if (dg_OrderInfo.Rows.Count < Cross_Info.Rows.Count)
                {
                    dg_OrderInfo.Rows.Add(Cross_Info.Rows.Count - dg_OrderInfo.Rows.Count);
                }
                int GetCount = dg_OrderInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < Cross_Info.Rows.Count)
                    {
                        dg_OrderInfo.Rows[i].Cells[0].Value = Cross_Info.Rows[i][0].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[1].Value = Cross_Info.Rows[i][1].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[2].Value = Cross_Info.Rows[i][2].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[3].Value = Cross_Info.Rows[i][3].ToString().Trim();

                        
                    }
                    else
                    {
                        dg_OrderInfo.Rows.RemoveAt(Cross_Info.Rows.Count);
                    }
                }

            }
            else
            {
                dg_OrderInfo.Rows.Clear();
            }
        }

        private void dg_OrderInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //删除
            if (e.ColumnIndex == 4)
            {
                if (Params.userName_now != Params.root_name)
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
                if (e.RowIndex >= 0)
                {
                    DataGridViewColumn column = dg_OrderInfo.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        if (MessageBox.Show("确定删除此条任务信息吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            int cid = int.Parse(dg_OrderInfo.Rows[e.RowIndex].Cells[0].Value.ToString().Trim());

                            int res = Function.DELETE_ORDEF_INFO(cid);
                            if (res != 0)
                            {
                                MessageBox.Show("该任务已删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("任务未能删除，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            GetInfo();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetInfo();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

        }
        private void radioButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = Function.SELECT_ORDER_TYPE(int.Parse(radioButton1.Tag.ToString().Trim()));
            showalone(dt);
        }
        private void radioButton2_Click(object sender, EventArgs e)
        {
            DataTable dt = Function.SELECT_ORDER_TYPE(int.Parse(radioButton2.Tag.ToString().Trim()));
            showalone(dt);
        }
        private void radioButton3_Click(object sender, EventArgs e)
        {
            DataTable dt = Function.SELECT_ORDER_TYPE(int.Parse(radioButton3.Tag.ToString().Trim()));
            showalone(dt);
        }
        private void radioButton4_Click(object sender, EventArgs e)
        {
            DataTable dt = Function.SELECT_ORDER_TYPE(int.Parse(radioButton4.Tag.ToString().Trim()));
            showalone(dt);
        }

        public void showalone(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                label2.Text = "当前共配置了 " + dt.Rows.Count.ToString() + " 条AGV与其他设备的交互任务";
                if (dg_OrderInfo.Rows.Count < dt.Rows.Count)
                {
                    dg_OrderInfo.Rows.Add(dt.Rows.Count - dg_OrderInfo.Rows.Count);
                }
                int GetCount = dg_OrderInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < dt.Rows.Count)
                    {
                        dg_OrderInfo.Rows[i].Cells[0].Value = dt.Rows[i][0].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[1].Value = dt.Rows[i][1].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[2].Value = dt.Rows[i][2].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[3].Value = dt.Rows[i][3].ToString().Trim();
                    }
                    else
                    {
                        dg_OrderInfo.Rows.RemoveAt(dt.Rows.Count);
                    }
                }

            }
            else
            {
                dg_OrderInfo.Rows.Clear();
            }
        }
    }
}
