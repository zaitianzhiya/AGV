using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.CodeParser;
using DevExpress.Xpf.Grid.LookUp;

namespace AGVMAPWPF
{
    /// <summary>
    /// MachineInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TaskSetUpDetailWindow : Window
    {
        private string mode = "";//模式,新增N，编辑M
        private string taskConditonCode = "";
        private DataTable dtSource;
        private DataTable dtSource2;
        private DataTable dtMaterial;
        private DataTable dtArea;
        private static ObservableCollection<ComboxSource> observableCollection11 = new ObservableCollection<ComboxSource>();//目的区域编码
        private static ObservableCollection<ComboxSource> observableCollection22 = new ObservableCollection<ComboxSource>();//目的储位状态
        private static ObservableCollection<ComboxSource> observableCollection33 = new ObservableCollection<ComboxSource>();//目的储位物料类型
        private static ObservableCollection<ComboxSource> observableCollection44 = new ObservableCollection<ComboxSource>();//目的点动作
        private static ObservableCollection<ComboxSource> observableCollection55 = new ObservableCollection<ComboxSource>();//是否等待放行

        public static ObservableCollection<ComboxSource> ObservableCollection11
        {
            get { return observableCollection11; }
            set { observableCollection11 = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollection22
        {
            get { return observableCollection22; }
            set { observableCollection22 = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollection33
        {
            get { return observableCollection33; }
            set { observableCollection33 = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollection44
        {
            get { return observableCollection44; }
            set { observableCollection44 = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollection55
        {
            get { return observableCollection55; }
            set { observableCollection55 = value; }
        }

        public TaskSetUpDetailWindow()
        {
            InitializeComponent();
            mode = "N";
        }

        public TaskSetUpDetailWindow(string taskConditonCode,string name)
        {
            InitializeComponent();
            this.taskConditonCode = taskConditonCode;
            mode = "M";
            TextBoxID.Text = taskConditonCode;
            TextBoxName.Text = name;
        }

        private void TaskSetUpDetailWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            observableCollection11.Clear();
            observableCollection22.Clear();
            observableCollection33.Clear();
            observableCollection44.Clear();
            observableCollection55.Clear();

            dtArea = Function.GetDataInfo("PR_SELECT_AREA_INFO");

            foreach (DataRow dr in dtArea.Rows)
            {
                observableCollection11.Add(new ComboxSource(dr["AreaName"].ToString(), dr["OwnArea"].ToString()));
            }

            observableCollection22.Add(new ComboxSource("空位置", "0"));
            observableCollection22.Add(new ComboxSource("空料架", "1"));
            observableCollection22.Add(new ComboxSource("满料架", "2"));

            dtMaterial = Function.GetDataInfo("PR_SELECT_MATERIAL_INFO");

            foreach (DataRow dr in dtMaterial.Rows)
            {
                observableCollection33.Add(new ComboxSource(dr["MaterialName"].ToString(), dr["MaterialType"].ToString()));
            }

            observableCollection44.Add(new ComboxSource("取料", "0"));
            observableCollection44.Add(new ComboxSource("放料", "1"));

            observableCollection55.Add(new ComboxSource("是", "1"));
            observableCollection55.Add(new ComboxSource("否", "0"));

            dtSource = Function.GettbTaskConfigDetail(string.IsNullOrEmpty(taskConditonCode) ? "-99" : taskConditonCode);
            DataGrid.ItemsSource = dtSource.DefaultView;

            if (dtSource.Rows.Count > 0)
            {
                dtSource2 = Function.GettbTaskMustPass(dtSource.Rows[0]["TaskConditonCode"].ToString(),
                    dtSource.Rows[0]["DetailID"].ToString());
                DataGrid2.ItemsSource = dtSource2.DefaultView;
            }

            if (mode == "N")
            {
                string maxCallId = Function.GetMaxValue("PR_SELECT_MAX_TASKDETAILID");
                TextBoxID.Text =int.Parse(maxCallId).ToString("0000");
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

        private void RabExist_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void RabAdd_OnClick(object sender, RoutedEventArgs e)
        {
            DataRow drNew = dtSource.NewRow();
            string dtMax = dtSource.Compute("Max(DetailID)", null).ToString();
            if (string.IsNullOrEmpty(dtMax))
            {
                dtMax = "0";
            }
            string dbMax = Function.GetMaxDetailID(TextBoxID.Text.Trim());
            drNew["DetailID"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax));
            if (dtArea.Rows.Count > 0)
            {
                drNew["ArmOwnArea"] = dtArea.Rows[0]["OwnArea"];
            }
            drNew["StorageState"] = "0";
            if (dtMaterial.Rows.Count > 0)
            {
                drNew["MaterialType"] = dtMaterial.Rows[0]["MaterialType"];
            }
            drNew["Action"] = "0";
            drNew["IsWaitPass"] = "1";
            dtSource.Rows.Add(drNew);
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
                    string detailID = selectRow.Row["DetailID"].ToString();
                    dtSource.Select(string.Format("DetailID='{0}'", detailID))[0].Delete();
                }
            }
        }

        private void RabSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxID.Text.Trim()))
            {
                MessageBoxAlert.Show("任务条件配置编号不能为空", MessageBoxImage.Exclamation);
                TextBoxName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(TextBoxName.Text.Trim()))
            {
                MessageBoxAlert.Show("任务条件配置名称不能为空", MessageBoxImage.Exclamation);
                TextBoxName.Focus();
                return;
            }
            if (dtSource.Rows.Count == 0 || dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count() <= 0)
            {
                MessageBoxAlert.Show("请先维护明细数据", MessageBoxImage.Exclamation);
                return;
            }
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["TaskConditonCode"] = TextBoxID.Text.Trim();
                }
            }
            Function.Update_tbTaskConfigDetail(mode, TextBoxID.Text.Trim(), TextBoxName.Text.Trim(),dtSource);
            dtSource.AcceptChanges();
            MessageBoxAlert.Show("保存成功", MessageBoxImage.Asterisk);
            DialogResult = true;
        }

        private void DataGrid_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            DataRowView currentRow = e.Row.Item as DataRowView;
            if (currentRow != null)
            {
                string detailID = currentRow.Row["DetailID"].ToString();
                DataRow drCurrentRow =dtSource.Select(string.Format("DetailID='{0}'", detailID))[0];
                if (drCurrentRow.RowState == DataRowState.Unchanged)
                {
                    drCurrentRow.AcceptChanges();
                    drCurrentRow.SetModified();
                }
            }
        }

        private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IList<DataGridCellInfo> cells = DataGrid.SelectedCells;
            if (cells.Count > 0)
            {
                DataGridCellInfo cell = cells[0];
                DataGridColumn dc = cell.Column;
                if (dc.Header.ToString() == "明细ID")
                {
                    DataRowView currentRow = cell.Item as DataRowView;
                    TaskMustPassWindow taskMustPassWindow = new TaskMustPassWindow(TextBoxID.Text.Trim(), currentRow["DetailID"].ToString());
                    if (taskMustPassWindow.ShowDialog() == true)
                    {
                        dtSource2 = Function.GettbTaskMustPass((string.IsNullOrEmpty(currentRow["TaskConditonCode"].ToString()) ? TextBoxID.Text.Trim() : currentRow["TaskConditonCode"].ToString()), currentRow["DetailID"].ToString());
                        DataGrid2.ItemsSource = dtSource2.DefaultView;
                    }
                }
            }
        }

        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid.BeginEdit();
            IList<DataGridCellInfo> cells = DataGrid.SelectedCells;
            if (cells.Count > 0)
            {
                DataGridCellInfo cell = cells[0];
                DataRowView currentRow = cell.Item as DataRowView;
                dtSource2 = Function.GettbTaskMustPass(currentRow["TaskConditonCode"].ToString(), currentRow["DetailID"].ToString());
                DataGrid2.ItemsSource = dtSource2.DefaultView;
            }
        }
    }
}
