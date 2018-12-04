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
using DevExpress.Xpf.Grid.LookUp;

namespace AGVMAPWPF
{
    /// <summary>
    /// MachineInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CallDetailsInfoWindow : Window
    {
        private string mode = "";//模式,新增N，编辑M
        private string callBoxID = "";
        private DataTable dtSource;
        private static ObservableCollection<ComboxSource> observableCollectionType1 = new ObservableCollection<ComboxSource>();
        private static ObservableCollection<ComboxSource> observableCollectionType2 = new ObservableCollection<ComboxSource>();
        private static ObservableCollection<ComboxSource> observableCollectionStore = new ObservableCollection<ComboxSource>();
        private static ObservableCollection<ComboxSource> lookUpSource = new ObservableCollection<ComboxSource>();

        public static ObservableCollection<ComboxSource> LookUpSource
        {
            get { return lookUpSource; }
            set { lookUpSource = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollectionType1
        {
            get { return observableCollectionType1; }
            set { observableCollectionType1 = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollectionType2
        {
            get { return observableCollectionType2; }
            set { observableCollectionType2 = value; }
        }

        public static ObservableCollection<ComboxSource> ObservableCollectionStore
        {
            get { return observableCollectionStore; }
            set { observableCollectionStore = value; }
        }

        public CallDetailsInfoWindow()
        {
            InitializeComponent();
            mode = "N";
        }

        public CallDetailsInfoWindow(string callBoxID)
        {
            InitializeComponent();
            this.callBoxID = callBoxID;
            mode = "M";
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LookUpSource.Clear();
            observableCollectionType1.Clear();
            observableCollectionType2.Clear();
            observableCollectionStore.Clear();

            observableCollectionType1.Add(new ComboxSource("呼叫","0"));
            observableCollectionType1.Add(new ComboxSource("监控", "1"));

            observableCollectionType2.Add(new ComboxSource("呼叫或监控", "0"));
            observableCollectionType2.Add(new ComboxSource("放行", "1"));
            observableCollectionType2.Add(new ComboxSource("取消最近一次呼叫", "2"));

            observableCollectionStore.Add(new ComboxSource("空位置", "0"));
            observableCollectionStore.Add(new ComboxSource("空料架", "1"));
            observableCollectionStore.Add(new ComboxSource("满料架", "2"));

            LookUpSource.Add(new ComboxSource("托盘周转", "0001"));
            LookUpSource.Add(new ComboxSource("下线", "0002"));

            ComboBoxType.ItemsSource = observableCollectionType1;

            dtSource = Function.GetCallBox(string.IsNullOrEmpty(callBoxID) ? "-99" : callBoxID);
            DataGrid.ItemsSource = dtSource.DefaultView;

            if (dtSource.Rows.Count > 0)
            {
                TextBoxID.Text = callBoxID;
                TextBoxName.Text = dtSource.Rows[0]["CallBoxName"].ToString();
                ComboBoxType.SelectedValue = dtSource.Rows[0]["CallBoxType"].ToString();
            }
            if (mode == "N")
            {
                string maxCallId=Function.GetMaxValue("PR_SELECT_MAX_CALLBOXID");
                TextBoxID.Text = maxCallId;
                ComboBoxType.SelectedIndex = 0;
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
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] {"CallBoxID", "ButtonID"});
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p=>p.RowState!=DataRowState.Deleted).Count())
            {
                MessageBoxAlert.Show("当前按钮号重复", MessageBoxImage.Exclamation);
                return;
            }
            DataRow drNew = dtSource.NewRow();
            drNew["CallBoxID"] = TextBoxID.Text;
            drNew["ButtonID"] = "0";
            drNew["TaskConditonCode"] = "0001";
            drNew["OperaType"] ="0";
            drNew["LocationID"] = "0";
            drNew["LocationState"] = "0";
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
                    string buttonID = selectRow.Row["ButtonID"].ToString();
                    dtSource.Select(string.Format("CallBoxID='{0}' and ButtonID='{1}'", TextBoxID.Text.Trim(), buttonID))
                        [0].Delete();
                }
            }
        }

        private void RabSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxName.Text.Trim()))
            {
                MessageBoxAlert.Show("呼叫器名称不能为空", MessageBoxImage.Exclamation);
                TextBoxName.Focus();
                return;
            }
            if (dtSource.Rows.Count == 0 || dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count() <= 0)
            {
                //DataRow drNew = dtSource.NewRow();
                //drNew["CallBoxID"] = TextBoxID.Text.Trim();
                //drNew["CallBoxName"] = TextBoxName.Text.Trim();
                //drNew["CallBoxType"] = ComboBoxType.SelectedValue.ToString();
                //dtSource.Rows.Add(drNew);
                MessageBoxAlert.Show("请先维护明细数据", MessageBoxImage.Exclamation);
                return;
            }
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "CallBoxID", "ButtonID" });
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxAlert.Show("当前按钮号重复", MessageBoxImage.Exclamation);
                return;
            }
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["ButtonID"].ToString()))
                    {
                        MessageBoxAlert.Show("按钮号不能为空", MessageBoxImage.Exclamation);
                        return;
                    }
                    if (string.IsNullOrEmpty(dr["LocationID"].ToString()))
                    {
                        MessageBoxAlert.Show("监控储位ID不能为空", MessageBoxImage.Exclamation);
                        return;
                    }
                    dr["CallBoxName"] = TextBoxName.Text.Trim();
                    dr["CallBoxType"] = ComboBoxType.SelectedValue.ToString();
                }
            }
            Function.Update_tbCallBox(mode, TextBoxID.Text.Trim(), TextBoxName.Text.Trim(), ComboBoxType.SelectedValue.ToString(), dtSource);
            //dtSource.AcceptChanges();
            MessageBoxAlert.Show("保存成功", MessageBoxImage.Asterisk);
            DialogResult = true;
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void DataGrid_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            DataRowView currentRow = e.Row.Item as DataRowView;
            if (currentRow != null)
            {
                string buttonID = currentRow.Row["ButtonID"].ToString();
                DataRow drCurrentRow =
                    dtSource.Select(string.Format("CallBoxID='{0}' and ButtonID='{1}'", TextBoxID.Text.Trim(), buttonID))
                        [0];
                if (drCurrentRow.RowState == DataRowState.Unchanged)
                {
                    drCurrentRow.AcceptChanges();
                    drCurrentRow.SetModified();
                }
            }
        }
    }
}
