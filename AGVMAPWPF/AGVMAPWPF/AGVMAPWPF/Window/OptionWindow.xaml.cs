using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;
using MessageBox = System.Windows.MessageBox;

namespace AGVMAPWPF
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow()
        {
            InitializeComponent();
        }

        private void OptionWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            txtR.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorR",
            System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            txtG.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorG",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            txtB.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorB",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            string chk = FileControl.SetFileControl.ReadIniValue("OPTION", "UseCoordinate",
           System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            chkCoor.IsChecked = bool.Parse(chk);

            string rab = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateType",
           System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            if (rab == "L")
            {
                rabL.IsChecked = true;
            }
            else
            {
                rabP.IsChecked = true;
            }
            txtCoorR.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateR",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            txtCoorG.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateG",
              System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            txtCoorB.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateB",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            txtPenR.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "PenR",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            txtPenG.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "PenG",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            txtPenB.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "PenB",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            txtPenSize.Text = FileControl.SetFileControl.ReadIniValue("OPTION", "PenSize",
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
        }

        /// 确定单击事件
        /// <summary>
        /// 确定单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtR.Text.Trim()))
            {
                MessageBox.Show("背景颜色R值不能为空");
                txtR.SelectAll();
                txtR.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtG.Text.Trim()))
            {
                MessageBox.Show("背景颜色G值不能为空");
                txtG.SelectAll();
                txtG.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtB.Text.Trim()))
            {
                MessageBox.Show("背景颜色B值不能为空");
                txtB.SelectAll();
                txtB.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtCoorR.Text.Trim()))
            {
                MessageBox.Show("栅格颜色R值不能为空");
                txtCoorR.SelectAll();
                txtCoorR.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCoorG.Text.Trim()))
            {
                MessageBox.Show("栅格颜色G值不能为空");
                txtCoorG.SelectAll();
                txtCoorG.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCoorB.Text.Trim()))
            {
                MessageBox.Show("栅格颜色B值不能为空");
                txtCoorB.SelectAll();
                txtCoorB.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPenR.Text.Trim()))
            {
                MessageBox.Show("画笔颜色R值不能为空");
                txtPenR.SelectAll();
                txtPenR.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPenG.Text.Trim()))
            {
                MessageBox.Show("画笔颜色G值不能为空");
                txtPenG.SelectAll();
                txtPenG.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPenB.Text.Trim()))
            {
                MessageBox.Show("画笔颜色B值不能为空");
                txtPenB.SelectAll();
                txtPenB.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPenSize.Text.Trim()))
            {
                MessageBox.Show("画笔大小不能为空");
                txtPenSize.SelectAll();
                txtPenSize.Focus();
                return;
            }

            if (int.Parse(txtR.Text.Trim()) > 255)
            {
                MessageBox.Show("背景颜色R值不能大于255");
                txtR.SelectAll();
                txtR.Focus();
                return;
            }
            if (int.Parse(txtG.Text.Trim()) > 255)
            {
                MessageBox.Show("背景颜色G值不能大于255");
                txtG.SelectAll();
                txtG.Focus();
                return;
            }
            if (int.Parse(txtB.Text.Trim()) > 255)
            {
                MessageBox.Show("背景颜色B值不能大于255");
                txtB.SelectAll();
                txtB.Focus();
                return;
            }
            if (int.Parse(txtCoorR.Text.Trim()) > 255)
            {
                MessageBox.Show("栅格颜色R值不能大于255");
                txtCoorR.SelectAll();
                txtCoorR.Focus();
                return;
            }
            if (int.Parse(txtCoorG.Text.Trim()) > 255)
            {
                MessageBox.Show("栅格颜色G值不能大于255");
                txtCoorG.SelectAll();
                txtCoorG.Focus();
                return;
            }
            if (int.Parse(txtCoorB.Text.Trim()) > 255)
            {
                MessageBox.Show("栅格颜色B值不能大于255");
                txtCoorB.SelectAll();
                txtCoorB.Focus();
                return;
            }
            if (int.Parse(txtPenR.Text.Trim()) > 255)
            {
                MessageBox.Show("画笔颜色R值不能大于255");
                txtPenR.SelectAll();
                txtPenR.Focus();
                return;
            }
            if (int.Parse(txtPenG.Text.Trim()) > 255)
            {
                MessageBox.Show("画笔颜色G值不能大于255");
                txtPenG.SelectAll();
                txtPenG.Focus();
                return;
            }
            if (int.Parse(txtPenB.Text.Trim()) > 255)
            {
                MessageBox.Show("画笔颜色B值不能大于255");
                txtPenB.SelectAll();
                txtPenB.Focus();
                return;
            }

            FileControl.SetFileControl.WriteIniValue("OPTION", "BackgroundColorR", txtR.Text.Trim(),
                System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "BackgroundColorG", txtG.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "BackgroundColorB", txtB.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            FileControl.SetFileControl.WriteIniValue("OPTION", "UseCoordinate", chkCoor.IsChecked.ToString(),
           System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "CoordinateType", ((bool)rabL.IsChecked ? "L" : "P"),
           System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "CoordinateR", txtCoorR.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "CoordinateG", txtCoorG.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "CoordinateB", txtCoorB.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            FileControl.SetFileControl.WriteIniValue("OPTION", "PenR", txtPenR.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "PenG", txtPenG.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");
            FileControl.SetFileControl.WriteIniValue("OPTION", "PenB", txtPenB.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            FileControl.SetFileControl.WriteIniValue("OPTION", "PenSize", txtPenSize.Text.Trim(),
               System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini");

            DialogResult = true;
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

        private void TxtR_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void TxtR_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtB.Text.Trim()) && !string.IsNullOrEmpty(txtR.Text.Trim()) &&
                !string.IsNullOrEmpty(txtG.Text.Trim()))
            {
                int r = int.Parse(txtR.Text.Trim());
                int g = int.Parse(txtG.Text.Trim());
                int b = int.Parse(txtB.Text.Trim());
                if (r <= 255 && g <= 255 && b <= 255)
                {
                    txtColor.Background = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
                }
            }
        }

        private void TxtCoorR_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCoorB.Text.Trim()) && !string.IsNullOrEmpty(txtCoorR.Text.Trim()) &&
                !string.IsNullOrEmpty(txtCoorG.Text.Trim()))
            {
                int r = int.Parse(txtCoorR.Text.Trim());
                int g = int.Parse(txtCoorG.Text.Trim());
                int b = int.Parse(txtCoorB.Text.Trim());
                if (r <= 255 && g <= 255 && b <= 255)
                {
                    txtCoorColor.Background = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
                }
            }
        }

        private void TxtPenR_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPenB.Text.Trim()) && !string.IsNullOrEmpty(txtPenR.Text.Trim()) &&
               !string.IsNullOrEmpty(txtPenG.Text.Trim()))
            {
                int r = int.Parse(txtPenR.Text.Trim());
                int g = int.Parse(txtPenG.Text.Trim());
                int b = int.Parse(txtPenB.Text.Trim());
                if (r <= 255 && g <= 255 && b <= 255)
                {
                    txtPenColor.Background = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
                }
            }
        }
    }
}
