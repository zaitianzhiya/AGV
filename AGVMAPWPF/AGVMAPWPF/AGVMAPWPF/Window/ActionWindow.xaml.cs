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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ToolBar = System.Windows.Controls.ToolBar;

namespace AGVMAPWPF
{
    /// <summary>
    /// MachineInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ActionWindow : Window
    {
        private DataTable dtSource;

        public ActionWindow()
        {
            InitializeComponent();
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_ACTION");
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
            if (!CheckIsNull())
            {
                return;
            }
            string dtMax = dtSource.Compute("Max(ActionID)", null).ToString();
            if (string.IsNullOrEmpty(dtMax))
            {
                dtMax = "0";
            }
            string dbMax = Function.GetMaxValue("PR_SELECT_MAX_ACTIONID");
            DataRow drNew = dtSource.NewRow();
            drNew["ActionID"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax));
            dtSource.Rows.Add(drNew);
        }

        private void RabSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckIsNull())
            {
                return;
            }
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "ActionID" });
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxAlert.Show("动作ID重复", MessageBoxImage.Exclamation);
                return;
            }
            Function.Update_tbActionInfo(dtSource);
            dtSource.AcceptChanges();
            MessageBoxAlert.Show("保存成功", MessageBoxImage.Asterisk);
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
                    string actionID = selectRow.Row["ActionID"].ToString();
                    dtSource.Select(string.Format("ActionID='{0}'", actionID))[0].Delete();
                }
            }
        }

        private void DataGrid_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            DataRowView currentRow = e.Row.Item as DataRowView;
            if (currentRow != null)
            {
                string actionID = currentRow.Row["ActionID"].ToString();
                DataRow drCurrentRow = dtSource.Select(string.Format("ActionID='{0}'", actionID))[0];
                if (drCurrentRow.RowState == DataRowState.Unchanged)
                {
                    drCurrentRow.AcceptChanges();
                    drCurrentRow.SetModified();
                }
            }
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void UIElement_OnPreviewTextInput2(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.]+");
            e.Handled = re.IsMatch(e.Text);
        }

        bool CheckIsNull()
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["ActionID"].ToString()))
                    {
                        MessageBoxAlert.Show("动作ID不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["ActionName"].ToString()))
                    {
                        MessageBoxAlert.Show("动作名称不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["WaitTime"].ToString()))
                    {
                        MessageBoxAlert.Show("等待时间不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["CommondText"].ToString()))
                    {
                        MessageBoxAlert.Show("指令不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
