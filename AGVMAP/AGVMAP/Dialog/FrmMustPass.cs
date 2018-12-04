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
    public partial class FrmMustPass : BaseForm
    {
        private string Code = "";
        private string DetailId = "";
        private DataTable dtSource;

        public FrmMustPass()
        {
            InitializeComponent();
        }

        public FrmMustPass(string code, string detailId)
        {
            InitializeComponent();
            this.Code = txtTaskConditonCode.Text = code;
            this.DetailId = txtDetailID.Text = detailId;
        }

        private void FrmMustPass_Load(object sender, EventArgs e)
        {
            dtSource = Function.GettbTaskMustPass(Code, DetailId);
            gc1.DataSource = dtSource;
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow drNew = dtSource.NewRow();
            string dtMax = dtSource.Compute("Max(DetailID)", null).ToString();
            if (string.IsNullOrEmpty(dtMax))
            {
                dtMax = "0";
            }
            string dbMax = Function.GetMaxDetailID(txtTaskConditonCode.Text.Trim(), txtDetailID.Text.Trim());
            drNew["TaskConditonCode"] = txtTaskConditonCode.Text;
            drNew["TaskConfigDetailID"] = txtDetailID.Text;
            drNew["DetailID"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax)); ;
            drNew["Action"] = "0";
            dtSource.Rows.Add(drNew);
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
                dtSource.Select(string.Format("DetailID='{0}'", gv1.GetRowCellValue(gv1.FocusedRowHandle, "DetailID")))[0].Delete();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gv1.CloseEditor();
            gv1.UpdateCurrentRow();
            if (dtSource.Rows.Count == 0 || dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count() <= 0)
            {
                MessageBoxShow.Alert("请先维护明细数据", MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["DetailID"].ToString()))
                    {
                        MessageBoxShow.Alert("明细ID不能为空", MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (string.IsNullOrEmpty(dr["MustPassLandCode"].ToString()))
                    {
                        MessageBoxShow.Alert("必经地表号不能为空", MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (string.IsNullOrEmpty(dr["Action"].ToString()))
                    {
                        MessageBoxShow.Alert("动作不能为空", MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            Function.Update_tbTaskConfigMustPass(dtSource);
            MessageBoxShow.Alert("保存成功", MessageBoxIcon.Asterisk);
            DialogResult = DialogResult.OK;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
