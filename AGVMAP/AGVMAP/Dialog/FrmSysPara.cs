using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGVMAP.Dialog
{
    public partial class FrmSysPara : BaseForm
    {
        private DataTable dtSource;
        public FrmSysPara()
        {
            InitializeComponent();
        }

        private void FrmSysPara_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_SYSPARAMETER");
            gridControl1.DataSource = dtSource;
            bindingSource1.DataSource = dtSource;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.PostEditor();
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (string.IsNullOrEmpty(dr["ParameterValue"].ToString()))
                {
                    MessageBoxShow.Alert("参数值不能为空", MessageBoxIcon.Exclamation);
                    return;
                }
            }
            Function.Update_tbSysParameter(dtSource);
            dtSource.AcceptChanges();
            MessageBoxShow.Alert("保存成功", MessageBoxIcon.Asterisk);
        }

        private void btnQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            gridView1.MoveFirst();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            gridView1.MovePrev();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            gridView1.MoveNext();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            gridView1.MoveLast();
        }

        private void bindingNavigatorPositionItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar ==(char)Keys.Enter)
            {
                if (int.Parse(bindingNavigatorPositionItem.Text) > dtSource.Rows.Count)
                {
                    bindingSource1.MoveLast();
                    gridView1.MoveLast();
                }
                else
                {
                    bindingSource1.Position = int.Parse(bindingNavigatorPositionItem.Text)-1;
                    gridView1.MoveFirst();
                    gridView1.MoveBy(int.Parse(bindingNavigatorPositionItem.Text)-1);
                }
            }
            else if (e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)Keys.Back)
            {

            }
            else
            {
                e.Handled = !Char.IsNumber(e.KeyChar);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle > -1)
            {
                bindingSource1.Position = e.FocusedRowHandle;
            }
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //是否导入路径长度
            if (gridView1.GetRowCellValue(e.RowHandle, "ParameterCode").ToString() == "IsImportLenth")
            {
                e.RepositoryItem = repositoryItemComboBox1;
            }
            //路线生成方式
            else if (gridView1.GetRowCellValue(e.RowHandle, "ParameterCode").ToString() == "RouteCountMode")
            {
                e.RepositoryItem = repositoryItemComboBox2;
            }
            //路线维护方式
            else if (gridView1.GetRowCellValue(e.RowHandle, "ParameterCode").ToString() == "RouteSetMode")
            {
                e.RepositoryItem = repositoryItemComboBox3;
            }
            else
            {
                e.RepositoryItem = repositoryItemTextEdit1;
            }
        }
    }
}
