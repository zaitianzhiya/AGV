using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TS_RGB.Fuction;

namespace TS_RGB
{
    public partial class frm_Manage_Param : Form
    {
        frm_Login frm_login = null;
        public frm_Manage_Param()
        {
            InitializeComponent();
            label_msg.Text = "";
        }

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
            System.Windows.Forms.OpenFileDialog folder = new OpenFileDialog();
            folder.Filter = "图片文件png|*.png|图片文件jpg|*.jpg|所有文件(*.*)|*.*";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                textBox_mapPath.Text = folder.FileName;
                try
                {
                    if (MessageBox.Show("确定更新地图吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        File.Copy(folder.FileName, Application.StartupPath + "\\img\\map.png", true);
                        label_msg.Text = "地图更新成功！";
                        label_msg.ForeColor = Color.Green;
                    }
                    return;
                }
                catch (IOException ioexp)
                {
                    if (ioexp.GetType().Name == "IOException")
                    {
                        MessageBox.Show("地图页面已打开，请重启软件后再设置！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void frm_Manage_Param_Load(object sender, EventArgs e)
        {
            DataTable proj = Function.SELECT_Proj();
            if (proj != null && proj.Rows.Count > 0)
            {
                textBox_PName.Text = proj.Rows[0][0].ToString().Trim();
                textBox_PNo.Text = proj.Rows[0][1].ToString().Trim();
                textBox_PNote.Text = proj.Rows[0][2].ToString().Trim();
            }
            checkBox_power_log.Checked = Params.powerLog;
            checkBox_landmark.Checked = Params.rfidLog;
            checkBox_send.Checked = Params.sendLog;
            checkBox_rec.Checked = Params.recLog;
            checkBox_warn.Checked = Params.errLog;
            checkBox_Vol.Checked = Params.volLog;


        }

        private void checkBox_power_log_CheckedChanged(object sender, EventArgs e)
        {
            Params.powerLog = checkBox_power_log.Checked;
        }

        private void checkBox_landmark_CheckedChanged(object sender, EventArgs e)
        {
            Params.rfidLog = checkBox_landmark.Checked;
        }

        private void checkBox_send_CheckedChanged(object sender, EventArgs e)
        {
            Params.sendLog = checkBox_send.Checked;
        }

        private void checkBox_rec_CheckedChanged(object sender, EventArgs e)
        {
            Params.recLog = checkBox_rec.Checked;
        }

        private void checkBox_warn_CheckedChanged(object sender, EventArgs e)
        {
            Params.errLog = checkBox_warn.Checked;
        }

        private void checkBox_Vol_CheckedChanged(object sender, EventArgs e)
        {
            Params.volLog = checkBox_Vol.Checked;
        }


    }
}
