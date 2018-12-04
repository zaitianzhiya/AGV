using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AGVMAPWPF
{
    /// <summary>
    /// DbSetUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DbSetUpWindow : Window
    {
        string path = System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini";
        public DbSetUpWindow()
        {
            InitializeComponent();
        }

        private void DbSetUpWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            string conStr = File.ReadAllText(System.Windows.Forms.Application.StartupPath + @"\ConnectionString").Trim();
            string[] array = conStr.Split(';');
            foreach (string s in array)
            {
                if (s.Trim().ToLower().StartsWith("database"))
                {
                    TxtDbName.Text = s.Split('=')[1].Trim();
                }
                if (s.Trim().ToLower().StartsWith("server"))
                {
                    TxtDbAddress.Text = s.Split('=')[1].Trim();
                }
                if (s.Trim().ToLower().StartsWith("uid"))
                {
                    TxtUser.Text = s.Split('=')[1].Trim();
                }
                if (s.Trim().ToLower().StartsWith("pwd"))
                {
                    TxtPwd.Password = s.Split('=')[1].Trim();
                }
            }

            TxtPcAddress.Text = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTIP", path);
            TxtPcPort.Text = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTPORT", path);
        }

        /// 测试单击事件
        /// <summary>
        /// 测试单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTest_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckIsNull())
            {
                //验证数据库是否OK
                string conStr = string.Format(
                "database={0};server={1};Max Pool Size=30;Min Pool Size=1;uid={2};pwd={3}", TxtDbName.Text.Trim(),
                TxtDbAddress.Text.Trim(), TxtUser.Text.Trim(), TxtPwd.Password);
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
                        MessageBox.Show("数据库连接失败", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                    MessageBoxResult.OK);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库连接失败", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                        MessageBoxResult.OK);
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
                    IPAddress ip = IPAddress.Parse(TxtPcAddress.Text.Trim());
                    IPEndPoint point = new IPEndPoint(ip, int.Parse(TxtPcPort.Text.Trim()));
                    Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(point);

                    MessageBox.Show("测试成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk,MessageBoxResult.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("上位机连接失败", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation,
                         MessageBoxResult.OK);
                }
            }
        }

        /// 确定单击事件
        /// <summary>
        /// 确定单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckIsNull())
            {
                string conStr = string.Format(
                    "database={0};server={1};Max Pool Size=30;Min Pool Size=1;uid={2};pwd={3}", TxtDbName.Text.Trim(),
                    TxtDbAddress.Text.Trim(), TxtUser.Text.Trim(), TxtPwd.Password);
                File.WriteAllText(System.Windows.Forms.Application.StartupPath + @"\ConnectionString", conStr);

                FileControl.SetFileControl.WriteIniValue("HOST", "HOSTIP", TxtPcAddress.Text.Trim(), path);
                FileControl.SetFileControl.WriteIniValue("HOST", "HOSTPORT", TxtPcPort.Text.Trim(), path);
                DialogResult = true;
            }
        }

        /// 取消单击事件
        /// <summary>
        /// 取消单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// 验证是否都不为空
        /// <summary>
        /// 验证是否都不为空
        /// </summary>
        bool CheckIsNull()
        {
            if (string.IsNullOrEmpty(TxtDbAddress.Text.Trim()))
            {
                MessageBox.Show("数据库地址不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                TxtDbAddress.Focus();
                TxtDbAddress.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(TxtDbName.Text.Trim()))
            {
                MessageBox.Show("数据库名称不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                TxtDbName.Focus();
                TxtDbName.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(TxtUser.Text.Trim()))
            {
                MessageBox.Show("用户名不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                TxtUser.Focus();
                TxtUser.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(TxtPwd.Password.Trim()))
            {
                MessageBox.Show("密码不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                TxtPwd.Focus();
                TxtPwd.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(TxtPcAddress.Text.Trim()))
            {
                MessageBox.Show("上位机地址不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                TxtPcAddress.Focus();
                TxtPcAddress.SelectAll();
                return false;
            }
            if (string.IsNullOrEmpty(TxtPcPort.Text.Trim()))
            {
                MessageBox.Show("上位机端口不能为空", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                TxtPcPort.Focus();
                TxtPcPort.SelectAll();
                return false;
            }
            return true;
        }
    }
}
