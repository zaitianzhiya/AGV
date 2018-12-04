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

namespace AGVMAPWPF
{
    /// <summary>
    /// MachineInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialWindow : Window
    {
        private DataTable dtSource;

        public MaterialWindow()
        {
            InitializeComponent();
        }

        private void MachineInfoWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_MATERIAL_INFO");
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
            string dtMax = dtSource.Compute("Max(MaterialType)", null).ToString();
            string dbMax = Function.GetMaxValue("PR_SELECT_MAX_MaterialType");
            DataRow drNew = dtSource.NewRow();
            drNew["MaterialType"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax));
            dtSource.Rows.Add(drNew);
        }

        private void RabSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckIsNull())
            {
                return;
            }
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "MaterialType" });
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxAlert.Show("物料类型重复", MessageBoxImage.Exclamation);
                return;
            }
            Function.Update_tbMaterialInfo(dtSource);
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
                    string materialType = selectRow.Row["MaterialType"].ToString();
                    dtSource.Select(string.Format("MaterialType='{0}'", materialType))[0].Delete();
                }
            }
        }

        private void DataGrid_OnPreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            DataRowView currentRow = e.Row.Item as DataRowView;
            if (currentRow != null)
            {
                string materialType = currentRow.Row["MaterialType"].ToString();
                DataRow drCurrentRow = dtSource.Select(string.Format("MaterialType='{0}'", materialType))[0];
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

        bool CheckIsNull()
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["MaterialType"].ToString()))
                    {
                        MessageBoxAlert.Show("物料类型不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["MaterialName"].ToString()))
                    {
                        MessageBoxAlert.Show("物料名称不能为空", MessageBoxImage.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
