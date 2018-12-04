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
    public partial class TaskMustPassWindow : Window
    {
        private string Code = "";
        private string DetailId = "";
        private DataTable dtSource;

        public TaskMustPassWindow()
        {
            InitializeComponent();
        }

        public TaskMustPassWindow(string code,string detailId)
        {
            InitializeComponent();
            this.Code =TextBoxID.Text=  code;
            this.DetailId=TextBoxName.Text = detailId;
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dtSource = Function.GettbTaskMustPass(Code, DetailId);
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
            DataRow drNew = dtSource.NewRow();
            string dtMax = dtSource.Compute("Max(DetailID)", null).ToString();
            if (string.IsNullOrEmpty(dtMax))
            {
                dtMax = "0";
            }
            string dbMax = Function.GetMaxDetailID(TextBoxID.Text.Trim(),TextBoxName.Text.Trim());
            drNew["TaskConditonCode"] = TextBoxID.Text;
            drNew["TaskConfigDetailID"] = TextBoxName.Text;
            drNew["DetailID"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax)); ;
            drNew["Action"] = "0";
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
          
            if (dtSource.Rows.Count == 0 || dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count() <= 0)
            {
                MessageBoxAlert.Show("请先维护明细数据", MessageBoxImage.Exclamation);
                return;
            }
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["DetailID"].ToString()))
                    {
                        MessageBoxAlert.Show("明细ID不能为空", MessageBoxImage.Exclamation);
                        return;
                    }
                    if (string.IsNullOrEmpty(dr["MustPassLandCode"].ToString()))
                    {
                        MessageBoxAlert.Show("必经地表号不能为空", MessageBoxImage.Exclamation);
                        return;
                    }
                    if (string.IsNullOrEmpty(dr["Action"].ToString()))
                    {
                        MessageBoxAlert.Show("动作不能为空", MessageBoxImage.Exclamation);
                        return;
                    }
                }
            }
            Function.Update_tbTaskConfigMustPass(dtSource);
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
                string detailID = currentRow.Row["DetailID"].ToString();
                DataRow drCurrentRow =dtSource.Select(string.Format("DetailID='{0}'", detailID))[0];
                if (drCurrentRow.RowState == DataRowState.Unchanged)
                {
                    drCurrentRow.AcceptChanges();
                    drCurrentRow.SetModified();
                }
            }
        }
    }
}
