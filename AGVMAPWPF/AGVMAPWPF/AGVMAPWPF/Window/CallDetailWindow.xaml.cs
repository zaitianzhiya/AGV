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
    public partial class CallDetailWindow : Window
    {
        private DataTable dtSource;

        public CallDetailWindow()
        {
            InitializeComponent();
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_CALLBOXINFO");
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

        private void RabExist_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void RabAdd_OnClick(object sender, RoutedEventArgs e)
        {
            CallDetailsInfoWindow callDetailsInfoWindow = new CallDetailsInfoWindow();
            if (callDetailsInfoWindow.ShowDialog() == true)
            {
                dtSource = Function.GetDataInfo("PR_SELECT_CALLBOXINFO");
                DataGrid.ItemsSource = dtSource.DefaultView;
            }
        }

        private void RabEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedCells.Count <= 0)
            {
                MessageBoxAlert.Show("请选择要编辑的项", MessageBoxImage.Exclamation);
                return;
            }
            DataRowView selectRow = DataGrid.SelectedCells[0].Item as DataRowView;
            if (selectRow != null)
            {
                string callBoxID = selectRow.Row["CallBoxID"].ToString();
                CallDetailsInfoWindow callDetailsInfoWindow = new CallDetailsInfoWindow(callBoxID);
                if (callDetailsInfoWindow.ShowDialog() == true)
                {
                    dtSource = Function.GetDataInfo("PR_SELECT_CALLBOXINFO");
                    DataGrid.ItemsSource = dtSource.DefaultView;
                }
            }
        }

        private void RabDel_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBoxAlert.Show("确定删除当前项?", MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (DataGrid.SelectedCells.Count <= 0)
                {
                    MessageBoxAlert.Show("请选择要删除的项", MessageBoxImage.Exclamation);
                    return;
                }
                DataRowView selectRow = DataGrid.SelectedCells[0].Item as DataRowView;
                if (selectRow != null)
                {
                    string callBoxID = selectRow.Row["CallBoxID"].ToString();
                    int i=Function.Del_DataByPk("tbCallBoxInfo", callBoxID);
                    dtSource = Function.GetDataInfo("PR_SELECT_CALLBOXINFO");
                    DataGrid.ItemsSource = dtSource.DefaultView;
                }
            }
        }

        private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView selectRow = DataGrid.SelectedCells[0].Item as DataRowView;
            if (selectRow != null)
            {
                string callBoxID = selectRow.Row["CallBoxID"].ToString();
                CallDetailsInfoWindow callDetailsInfoWindow = new CallDetailsInfoWindow(callBoxID);
                if (callDetailsInfoWindow.ShowDialog() == true)
                {
                    dtSource = Function.GetDataInfo("PR_SELECT_CALLBOXINFO");
                    DataGrid.ItemsSource = dtSource.DefaultView;
                }
            }
        }
    }
}
