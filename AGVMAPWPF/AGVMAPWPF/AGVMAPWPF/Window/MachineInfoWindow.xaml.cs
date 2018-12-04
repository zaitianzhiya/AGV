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
    public partial class MachineInfoWindow : Window
    {
        private DataTable dtSource;
        static ObservableCollection<ComboxSource> selectionList = new ObservableCollection<ComboxSource>();
        public static ObservableCollection<ComboxSource> SelectionList
        {
            get { return selectionList; }
            set { selectionList = value; }
        }

        public MachineInfoWindow()
        {
            InitializeComponent();
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_TBCAR");
            DataGrid.ItemsSource = dtSource.DefaultView;

            selectionList.Add(new ComboxSource("单向","0"));
            selectionList.Add(new ComboxSource("双向", "1"));
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

        private void RabAdd_OnClick(object sender, RoutedEventArgs e)
        {
            //if (dtSource.AsEnumerable().Where(p => p.RowState == DataRowState.Added).Count() > 0)
            //{
            //    MessageBoxAlert.Show("有新增数据未保存", MessageBoxImage.Exclamation);
            //    return;
            //}
            if (!CheckIsNull())
            {
                return;
            }
            string dbMax = Function.GetMaxValue("PR_SELECT_CARCODE");
            string dtMax = dtSource.Compute("MAX(CarCode)", "").ToString();
            dtMax = string.IsNullOrEmpty(dtMax) ? "1" : (int.Parse(dtMax)+1).ToString();
            int max = Math.Max(int.Parse(dbMax), int.Parse(dtMax));
            DataRow drNew = dtSource.NewRow();
            drNew["CarCode"] = max.ToString();
            drNew["CarType"] = "0";
            dtSource.Rows.Add(drNew);
        }

        private void RabDel_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedCells.Count <= 0)
            {
                MessageBoxAlert.Show("请先选择要删除的行", MessageBoxImage.Exclamation);
                return;
            }
            DataRowView selectRow = DataGrid.SelectedCells[0].Item as DataRowView;
            if (selectRow != null)
            {
                string carCode = selectRow.Row["CarCode"].ToString();
                dtSource.Select(string.Format("CarCode='{0}'",carCode))[0].Delete();
            }
        }

        private void RabSave_OnClick(object sender, RoutedEventArgs e)
        {
            DataGrid.CommitEdit();
            if (!CheckIsNull())
            {
                return;
            }
            Function.Update_tbCar(dtSource);
            dtSource.AcceptChanges();
            MessageBoxAlert.Show("保存成功", MessageBoxImage.Asterisk);
        }

        private void RabExist_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DataGrid_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            DataRowView currentRow = e.Row.Item as DataRowView;
            if (currentRow != null)
            {
                string carCode = currentRow.Row["CarCode"].ToString();
                DataRow drCurrentRow = dtSource.Select(string.Format("CarCode='{0}'", carCode))[0];
                if (drCurrentRow.RowState == DataRowState.Unchanged)
                {
                    drCurrentRow.AcceptChanges();
                    drCurrentRow.SetModified();
                }
            }
        }

        bool CheckIsNull()
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["CarName"].ToString()))
                    {
                        MessageBoxAlert.Show("设备名称不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["StandbyLandMark"].ToString()))
                    {
                        MessageBoxAlert.Show("待命点不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["CarIP"].ToString()))
                    {
                        MessageBoxAlert.Show("IP地址不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["CarPort"].ToString()))
                    {
                        MessageBoxAlert.Show("端口号不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
