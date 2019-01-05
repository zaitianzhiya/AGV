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
    public partial class frm_Manage_CrossInfo : Form
    {
        frm_Login frm_login = null;

        public frm_Manage_CrossInfo()
        {
            InitializeComponent();
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable Cross_Info = Fuction.Function.SELECT_Cross_ALL();
            if (Cross_Info != null && Cross_Info.Rows.Count > 0)
            {
                label1.Text = "当前系统共设置了 " + Cross_Info.Rows.Count.ToString() + " 条路口管控";
                if (dg_CrossInfo.Rows.Count < Cross_Info.Rows.Count)
                {
                    dg_CrossInfo.Rows.Add(Cross_Info.Rows.Count - dg_CrossInfo.Rows.Count);
                }
                int GetCount = dg_CrossInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < Cross_Info.Rows.Count)
                    {
                        dg_CrossInfo.Rows[i].Cells[0].Value = Cross_Info.Rows[i][0].ToString().Trim();
                        dg_CrossInfo.Rows[i].Cells[1].Value = Function.reArea(Cross_Info.Rows[i][1].ToString().Trim());
                        dg_CrossInfo.Rows[i].Cells[2].Value = Cross_Info.Rows[i][2].ToString().Trim();
                        dg_CrossInfo.Rows[i].Cells[3].Value = Cross_Info.Rows[i][3].ToString().Trim() == "1" ? "正方向" : "负方向";
                        dg_CrossInfo.Rows[i].Cells[4].Value = Cross_Info.Rows[i][4].ToString().Trim();
                        dg_CrossInfo.Rows[i].Cells[5].Value = Cross_Info.Rows[i][5].ToString().Trim() == "1" ? "正方向" : "负方向";
                        dg_CrossInfo.Rows[i].Cells[6].Value = Cross_Info.Rows[i][6].ToString().Trim();
                    }
                    else
                    {
                        dg_CrossInfo.Rows.RemoveAt(Cross_Info.Rows.Count);
                    }
                }

            }
            else
            {
                dg_CrossInfo.Rows.Clear();
            }
        }
        

        private void dg_CrossInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //清除占用
            if (e.ColumnIndex == 7)
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
                if (e.RowIndex >= 0)
                {
                    DataGridViewColumn column = dg_CrossInfo.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        //update cac=0
                        if (MessageBox.Show("确定清除此条交管占用吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            int cid = int.Parse(dg_CrossInfo.Rows[e.RowIndex].Cells[0].Value.ToString().Trim());
                            //MessageBox.Show(cid.ToString());
                            int res = Function.UPDATE_Cross_cac(cid);
                            if (res != 0)
                            {
                                MessageBox.Show("该条占用已清除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("占用未能清除，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            GetInfo();
                        }
                    }
                }
            }
            //删除交管
            if (e.ColumnIndex == 8)
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
                    DataGridViewColumn column = dg_CrossInfo.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        //update cac=0
                        if (MessageBox.Show("确定删除此条交管信息吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            int cid = int.Parse(dg_CrossInfo.Rows[e.RowIndex].Cells[0].Value.ToString().Trim());

                            int res = Function.DELETE_CROSS_INFO(cid);
                            if (res != 0)
                            {
                                MessageBox.Show("该条管控信息已删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("管控未能删除，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
    }

}
