using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TS_RGB
{
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
            Params.userName = FileControl.SetFileControl.ReadIniValue("USERS", "USERNAME", Application.StartupPath + @"\AGV_Set.ini").Trim();
            Params.passWord = FileControl.SetFileControl.ReadIniValue("USERS", "PASSWORD", Application.StartupPath + @"\AGV_Set.ini").Trim();
            txtName.Text = Params.userName_now;
            txtPW.Text = Params.passWord_now;
            if (txtName.Text != "")
                button1.Text = "注销";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "注销")
            {
                Params.userName_now = "";
                Params.passWord_now = "";
                frm_Main.toolStripStatusLabel_forPublic.Text = " |  当前用户：匿名用户";
                //权限设置
                frm_Main.ToolStripMenuItemAGVManage.Visible = false;
                frm_Main.ToolStripMenuItemAGV_orderManage.Visible = false;
                frm_Main.ToolStripMenuItemRFIDManage.Visible = false;
                frm_Main.ToolStripMenuItemAreaManage.Visible = false;

                this.Close();
            }
            else
            {
                //普通用户
                if (radioButton_user.Checked)
                {
                    if (Params.userName == txtName.Text.Trim() && Params.passWord == txtPW.Text.Trim())
                    {
                        Params.userName_now = Params.userName;
                        Params.passWord_now = Params.passWord;
                        frm_Main.toolStripStatusLabel_forPublic.Text = " |  当前用户：" + Params.userName_now;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //管理员权限
                if (radioButton_admin.Checked)
                {
                    if ("123" == txtName.Text.Trim() && "123" == txtPW.Text.Trim())
                    {
                        Params.userName_now = "123";
                        Params.passWord_now = "123";
                        frm_Main.toolStripStatusLabel_forPublic.Text = " |  当前用户：" + Params.userName_now;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误，请重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //权限设置
                    frm_Main.ToolStripMenuItemAGVManage.Visible = true;
                    frm_Main.ToolStripMenuItemAGV_orderManage.Visible = true;
                    frm_Main.ToolStripMenuItemRFIDManage.Visible = true;
                    frm_Main.ToolStripMenuItemAreaManage.Visible = true;
                }
            }
        }
    }
}
