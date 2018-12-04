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
    public partial class FrmTaskSet : BaseForm
    {
        private DataTable dtSource;
        public FrmTaskSet()
        {
            InitializeComponent();
        }

        private void FrmTaskSet_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_TASKCONFIGINFO");
            gc1.DataSource = dtSource;
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmTaskDetail frmTaskDetail = new FrmTaskDetail();
            if (frmTaskDetail.ShowDialog() == DialogResult.OK)
            {
                dtSource = Function.GetDataInfo("PR_SELECT_TASKCONFIGINFO");
                gc1.DataSource = dtSource;
            }
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gv1.FocusedRowHandle < 0)
            {
                MessageBoxShow.Alert("请选择要编辑的项", MessageBoxIcon.Exclamation);
                return;
            }
            FrmTaskDetail frmTaskDetail = new FrmTaskDetail(gv1.GetRowCellValue(gv1.FocusedRowHandle, "TaskConditonCode").ToString(), gv1.GetRowCellValue(gv1.FocusedRowHandle, "TaskConditonName").ToString());
            if (frmTaskDetail.ShowDialog() == DialogResult.OK)
            {
                dtSource = Function.GetDataInfo("PR_SELECT_TASKCONFIGINFO");
                gc1.DataSource = dtSource;
            }
        }

        private void btnDele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBoxShow.Alert("确定删除当前项?", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (gv1.FocusedRowHandle < 0)
                {
                    MessageBoxShow.Alert("请选择要删除的项", MessageBoxIcon.Exclamation);
                    return;
                }
                string taskConditonCode = gv1.GetRowCellValue(gv1.FocusedRowHandle, "TaskConditonCode").ToString();
                int i = Function.Del_DataByPk("tbTaskConfigInfo", taskConditonCode);
                dtSource = Function.GetDataInfo("PR_SELECT_TASKCONFIGINFO");
                gc1.DataSource = dtSource;
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void gv1_DoubleClick(object sender, EventArgs e)
        {
            if (gv1.FocusedRowHandle > -1)
            {
                btnEdit_ItemClick(sender, null);
            }
        }
    }
}
