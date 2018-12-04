using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// MachineInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SysParameterWindow : Window
    {
        private DataTable dtSource;

        public SysParameterWindow()
        {
            InitializeComponent();
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_SYSPARAMETER");
            DataGrid.ItemsSource = dtSource.DefaultView;
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

        private void RabSave_OnClick(object sender, RoutedEventArgs e)
        {
            DataGrid.CommitEdit();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (string.IsNullOrEmpty(dr["ParameterValue"].ToString()))
                {
                    MessageBoxAlert.Show("参数值不能为空", MessageBoxImage.Exclamation);
                    return;
                }
            }
            Function.Update_tbSysParameter(dtSource);
            dtSource.AcceptChanges();
            MessageBoxAlert.Show("保存成功", MessageBoxImage.Asterisk);
        }

        private void RabExist_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
