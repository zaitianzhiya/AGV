using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGVMAP.HelpClass;
using DevExpress.Utils;

namespace AGVMAP.Dialog
{
    public partial class FrmDbSetup : BaseForm
    {
        public FrmDbSetup()
        {
            InitializeComponent();
        }

        private void FrmDbSetup_Load(object sender, EventArgs e)
        {
            groupBox1.ForeColor = groupBox2.ForeColor = this.ForeColor;
            txtDbName.Text = FileControl.SetFileControl.ReadIniValue("DBSETUP", "DATABASE", Global.path);
            txtDbAddress.Text = FileControl.SetFileControl.ReadIniValue("DBSETUP", "SERVER", Global.path);
            txtUid.Text = FileControl.SetFileControl.ReadIniValue("DBSETUP", "UID", Global.path);
            txtPwd.Text = FileControl.SetFileControl.ReadIniValue("DBSETUP", "PWD", Global.path);
            txtHostIp.Text = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTIP", Global.path);
            txtHostPort.Text = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTPORT", Global.path);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (CheckIsNull())
            {
                using (new WaitDialogForm("正在测试连接,请稍后...", "提示"))
                {
                    //验证数据库是否OK
                    string conStr = string.Format(
                        "database={0};server={1};Max Pool Size=30;Min Pool Size=1;uid={2};pwd={3}",
                        txtDbName.Text.Trim(),
                        txtDbAddress.Text.Trim(), txtUid.Text.Trim(), txtPwd.Text.Trim());
                    SqlConnection con = null;
                    try
                    {
                        con = new SqlConnection(conStr);
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {

                        }
                        else
                        {
                            MessageBoxShow.Alert("数据库连接失败", MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxShow.Alert("数据库连接失败", MessageBoxIcon.Exclamation);
                        return;
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }

                    //验证上位机是否OK
                    try
                    {
                        IPAddress ip = IPAddress.Parse(txtHostIp.Text.Trim());
                        IPEndPoint point = new IPEndPoint(ip, int.Parse(txtHostPort.Text.Trim()));
                        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        client.Connect(point);
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBoxShow.Alert("上位机连接失败", MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (CheckIsNull())
            {
                FileControl.SetFileControl.WriteIniValue("DBSETUP", "DATABASE", txtDbName.Text.Trim(), Global.path);
                FileControl.SetFileControl.WriteIniValue("DBSETUP", "SERVER", txtDbAddress.Text.Trim(), Global.path);
                FileControl.SetFileControl.WriteIniValue("DBSETUP", "UID", txtUid.Text.Trim(), Global.path);
                FileControl.SetFileControl.WriteIniValue("DBSETUP", "PWD", txtPwd.Text.Trim(), Global.path);

                FileControl.SetFileControl.WriteIniValue("HOST", "HOSTIP", txtHostIp.Text.Trim(), Global.path);
                FileControl.SetFileControl.WriteIniValue("HOST", "HOSTPORT", txtHostPort.Text.Trim(), Global.path);
                string maxPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MaxPoolSize", Global.path);
                string minPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MinPoolSize", Global.path);
                SqlDBControl._defultConnectionString = string.Format(
               "database={0};server={1};Max Pool Size={2};Min Pool Size={3};uid={4};pwd={5}", txtDbName.Text.Trim(), txtDbAddress.Text.Trim(),
               maxPool, minPool, txtUid.Text.Trim(), txtPwd.Text.Trim());

                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// 验证是否都不为空
        /// <summary>
        /// 验证是否都不为空
        /// </summary>
        bool CheckIsNull()
        {
            if (string.IsNullOrEmpty(txtDbAddress.Text.Trim()))
            {
                MessageBoxShow.Alert("数据库地址不能为空",MessageBoxIcon.Exclamation);
                txtDbAddress.Focus();
                txtDbAddress.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(txtDbName.Text.Trim()))
            {
                MessageBoxShow.Alert("数据库名称不能为空", MessageBoxIcon.Exclamation);
                txtDbName.Focus();
                txtDbName.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(txtUid.Text.Trim()))
            {
                MessageBoxShow.Alert("用户名不能为空", MessageBoxIcon.Exclamation);
                txtUid.Focus();
                txtUid.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(txtPwd.Text.Trim()))
            {
                MessageBoxShow.Alert("密码不能为空", MessageBoxIcon.Exclamation);
                txtPwd.Focus();
                txtPwd.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(txtHostIp.Text.Trim()))
            {
                MessageBoxShow.Alert("上位机地址不能为空", MessageBoxIcon.Exclamation);
                txtHostIp.Focus();
                txtHostIp.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(txtHostPort.Text.Trim()))
            {
                MessageBoxShow.Alert("上位机端口不能为空", MessageBoxIcon.Exclamation);
                txtHostPort.Focus();
                txtHostPort.SelectAll();
                return false;
            }
            return true;
        }
    }
}
