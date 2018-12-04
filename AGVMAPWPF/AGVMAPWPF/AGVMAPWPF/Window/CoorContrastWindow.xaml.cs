using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// CoorContrastWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CoorContrastWindow : Window
    {
        public CoorContrastWindow()
        {
            InitializeComponent();
        }

        /// 窗体加载事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoorContrastWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Angle");
            DataRow drNew = dtSource.NewRow();
            drNew["Angle"] = "0";
            dtSource.Rows.Add(drNew);
            drNew = dtSource.NewRow();
            drNew["Angle"] = "90";
            dtSource.Rows.Add(drNew);
            drNew = dtSource.NewRow();
            drNew["Angle"] = "180";
            dtSource.Rows.Add(drNew);
            drNew = dtSource.NewRow();
            drNew["Angle"] = "270";
            dtSource.Rows.Add(drNew);
            cmbEast.ItemsSource = cmbSouth.ItemsSource = cmbWest.ItemsSource = cmbNorth.ItemsSource = dtSource.DefaultView;

            DataTable dtCoor = Function.GetDataInfo("PR_SELECT_COOR_INFO");
            if (dtCoor.Rows.Count > 0)
            {
                cmbEast.SelectedValue = dtCoor.Select("DIRECTION='1'")[0]["ANGLE"].ToString();
                cmbSouth.SelectedValue = dtCoor.Select("DIRECTION='2'")[0]["ANGLE"].ToString();
                cmbWest.SelectedValue = dtCoor.Select("DIRECTION='3'")[0]["ANGLE"].ToString();
                cmbNorth.SelectedValue = dtCoor.Select("DIRECTION='0'")[0]["ANGLE"].ToString();
            }
        }

        /// ToolBar_OnLoaded事件（隐藏溢出箭头）
        /// <summary>
        /// ToolBar_OnLoaded事件（隐藏溢出箭头）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (cmbEast.Text == cmbSouth.Text || cmbEast.Text == cmbWest.Text ||
                cmbEast.Text == cmbNorth.Text || cmbSouth.Text == cmbWest.Text ||
                cmbSouth.Text == cmbNorth.Text || cmbWest.Text == cmbNorth.Text)
            {
                MessageBox.Show("角度有重复", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                return;
            }
            Function.Update_CoorInfo(cmbEast.Text,cmbSouth.Text,cmbWest.Text,cmbNorth.Text);
            DialogResult = true;
        }

        private void BtnExit_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
