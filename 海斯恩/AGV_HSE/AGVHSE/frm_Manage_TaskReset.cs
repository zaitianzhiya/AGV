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
    public partial class frm_Manage_TaskReset : Form
    {
        frm_Login frm_login = null;

        public frm_Manage_TaskReset()
        {
            InitializeComponent();
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable taskReset_Info = Function.SELECT_RESETTASK_ALL();
            if (taskReset_Info != null && taskReset_Info.Rows.Count > 0)
            {
                if (dg_OptionInfo.Rows.Count < taskReset_Info.Rows.Count)
                {
                    dg_OptionInfo.Rows.Add(taskReset_Info.Rows.Count - dg_OptionInfo.Rows.Count);
                }
                int GetCount = dg_OptionInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < taskReset_Info.Rows.Count)
                    {
                        dg_OptionInfo.Rows[i].Cells[0].Value = taskReset_Info.Rows[i][0].ToString().Trim();
                        dg_OptionInfo.Rows[i].Cells[1].Value = taskReset_Info.Rows[i][1].ToString().Trim() == "1" ? "前进" : "后退";
                        dg_OptionInfo.Rows[i].Cells[2].Value = taskReset_Info.Rows[i][2].ToString().Trim();
                    }
                    else
                    {
                        dg_OptionInfo.Rows.RemoveAt(taskReset_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_OptionInfo.Rows.Clear();
            }
        }

        private void button_fresh_Click(object sender, EventArgs e)
        {
            textBox_RFID.Text = "";
            cm_Fw.SelectedIndex = 0;
            textBox_carNo.Text = "";
            GetInfo();
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
            if (!string.IsNullOrEmpty(textBox_RFID.Text.ToString().Trim())
                && !string.IsNullOrEmpty(textBox_carNo.Text.ToString().Trim()))
            {
                int res = Function.INSERT_TaskReset_All(textBox_RFID.Text.ToString().Trim(), (cm_Fw.SelectedIndex + 1).ToString(), textBox_carNo.Text.ToString().Trim());
                if (res > 0)
                {
                    MessageBox.Show("信息添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("添加失败，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                GetInfo();
            }
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
            if (!string.IsNullOrEmpty(textBox_RFID.Text.ToString().Trim())
                && !string.IsNullOrEmpty(textBox_carNo.Text.ToString().Trim()))
            {
                int res = Function.INSERT_TaskReset_All(textBox_RFID.Text.ToString().Trim(), (cm_Fw.SelectedIndex + 1).ToString(), textBox_carNo.Text.ToString().Trim());
                if (res > 0)
                {
                    MessageBox.Show("信息更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("更新失败，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                GetInfo();
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
            if (!string.IsNullOrEmpty(textBox_RFID.Text.ToString().Trim())
                && !string.IsNullOrEmpty(textBox_carNo.Text.ToString().Trim()))
            {
                if (MessageBox.Show("确定删除该条信息吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    int res = Function.DELETE_TaskReset(textBox_RFID.Text.ToString().Trim(), (cm_Fw.SelectedIndex + 1).ToString(), textBox_carNo.Text.ToString().Trim());
                    if (res > 0)
                    {
                        MessageBox.Show("信息删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("删除失败，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    dg_OptionInfo.Rows.Clear();
                    GetInfo();
                }
            }
        }

        private void dg_OptionInfo_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dg_OptionInfo.RowCount > 0)
            {
                textBox_RFID.Text = dg_OptionInfo.Rows[dg_OptionInfo.CurrentRow.Index].Cells[0].Value.ToString().Trim();
                cm_Fw.SelectedIndex = dg_OptionInfo.Rows[dg_OptionInfo.CurrentRow.Index].Cells[1].Value.ToString().Trim() == "前进" ? 0 : 1;
                textBox_carNo.Text = dg_OptionInfo.Rows[dg_OptionInfo.CurrentRow.Index].Cells[2].Value.ToString().Trim();
            }
        }
    }
}
