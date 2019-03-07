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
    public partial class frm_Manage_Area : Form
    {
        frm_Login frm_login = null;
        public frm_Manage_Area()
        {
            InitializeComponent();
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable Area_Info = Function.SELECT_Area_All();
            if (Area_Info != null && Area_Info.Rows.Count > 0)
            {
                if (dg_AreaInfo.Rows.Count < Area_Info.Rows.Count)
                {
                    dg_AreaInfo.Rows.Add(Area_Info.Rows.Count - dg_AreaInfo.Rows.Count);
                }
                int GetCount = dg_AreaInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < Area_Info.Rows.Count)
                    {
                        dg_AreaInfo.Rows[i].Cells[0].Value = Area_Info.Rows[i][0].ToString().Trim();
                        dg_AreaInfo.Rows[i].Cells[1].Value = Area_Info.Rows[i][1].ToString().Trim();
                        dg_AreaInfo.Rows[i].Cells[2].Value = Area_Info.Rows[i][2].ToString().Trim();
                        dg_AreaInfo.Rows[i].Cells[3].Value = Area_Info.Rows[i][3].ToString().Trim();
                        dg_AreaInfo.Rows[i].Cells[4].Value = Area_Info.Rows[i][4].ToString().Trim();
                    }
                    else
                    {
                        dg_AreaInfo.Rows.RemoveAt(Area_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_AreaInfo.Rows.Clear();
            }
        }
        private void button_add_Click(object sender, EventArgs e)
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
            string areaNo = textBox_AreaNo.Text.ToString().Trim();
            string areaname = textBox_AreaName.Text.ToString().Trim();
            string RFIDs = textBox_RFIDs.Text.ToString().Trim();
            string note = textBox_note.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(areaNo) && !string.IsNullOrEmpty(areaname)
                && !string.IsNullOrEmpty(RFIDs) && !string.IsNullOrEmpty(note))
            {
                int res = Function.Insert_Area_All(areaNo, areaname, RFIDs, note);
                if (res > 0)
                {
                    MessageBox.Show("区域信息添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("添加失败，请稍后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                GetInfo();
            }
        }

        private void button_fresh_Click(object sender, EventArgs e)
        {
            GetInfo();
            textBox_AreaNo.Text = "";
            textBox_AreaName.Text = "";
            textBox_RFIDs.Text = "";
            textBox_note.Text = "";
        }

        private void button_update_Click(object sender, EventArgs e)
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
            string areaNo = textBox_AreaNo.Text.ToString().Trim();
            string areaname = textBox_AreaName.Text.ToString().Trim();
            string RFIDs = textBox_RFIDs.Text.ToString().Trim();
            string note = textBox_note.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(areaNo) && !string.IsNullOrEmpty(areaname)
                && !string.IsNullOrEmpty(RFIDs) && !string.IsNullOrEmpty(note))
            {
                int res = Function.Insert_Area_All(areaNo, areaname, RFIDs, note);
                if (res > 0)
                {
                    MessageBox.Show("区域信息更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("更新失败，请稍后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                GetInfo();
            }
        }

        private void dg_AreaInfo_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dg_AreaInfo.RowCount > 0)
            {
                textBox_AreaNo.Text = dg_AreaInfo.Rows[dg_AreaInfo.CurrentRow.Index].Cells[1].Value.ToString().Trim();
                textBox_AreaName.Text = dg_AreaInfo.Rows[dg_AreaInfo.CurrentRow.Index].Cells[2].Value.ToString().Trim();
                textBox_RFIDs.Text = dg_AreaInfo.Rows[dg_AreaInfo.CurrentRow.Index].Cells[3].Value.ToString().Trim();
                textBox_note.Text = dg_AreaInfo.Rows[dg_AreaInfo.CurrentRow.Index].Cells[4].Value.ToString().Trim();
            }
        }

        private void button_del_Click(object sender, EventArgs e)
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
            string areaNo = textBox_AreaNo.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(areaNo))
            {
                if (MessageBox.Show("确定删除该区域信息吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    int res = Function.DELETE_Area_AreaNo(areaNo);
                    if (res > 0)
                    {
                        MessageBox.Show("区域信息删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("删除失败，请稍后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    dg_AreaInfo.Rows.Clear();
                    GetInfo();
                }
            }       
        }
    }
}
