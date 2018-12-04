using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AGVMAP.Dialog
{
    public partial class FrmMaterial : BaseForm
    {
        private DataTable dtSource;
        public FrmMaterial()
        {
            InitializeComponent();
        }

        private void FrmMaterial_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_MATERIAL_INFO");
            gc1.DataSource = dtSource;
        }

        private void btnAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckIsNull())
            {
                return;
            }
            string dtMax = dtSource.Compute("Max(MaterialType)", null).ToString();
            if (string.IsNullOrEmpty(dtMax))
            {
                dtMax = "0";
            }
            string dbMax = Function.GetMaxValue("PR_SELECT_MAX_MaterialType");
            DataRow drNew = dtSource.NewRow();
            drNew["MaterialType"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax));
            dtSource.Rows.Add(drNew);
            gv1.MoveLastVisible();
        }

        private void btnDelRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBoxShow.Alert("确定删除当前项?", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (gv1.FocusedRowHandle < 0)
                {
                    MessageBoxShow.Alert("请先选择要删除的行", MessageBoxIcon.Exclamation);
                    return;
                }
                gv1.GetFocusedDataRow().Delete();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //gv1.PostEditor();
            gv1.CloseEditor();
            gv1.UpdateCurrentRow();
            if (!CheckIsNull())
            {
                return;
            }
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "MaterialType" });
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxShow.Alert("物料类型重复", MessageBoxIcon.Exclamation);
                return;
            }
            Function.Update_tbMaterialInfo(dtSource);
            dtSource.AcceptChanges();
            MessageBoxShow.Alert("保存成功", MessageBoxIcon.Asterisk);
        }

        private void btnQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        bool CheckIsNull()
        {
            gv1.PostEditor();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["MaterialType"].ToString()))
                    {
                        MessageBoxShow.Alert("物料类型不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["MaterialName"].ToString()))
                    {
                        MessageBoxShow.Alert("物料名称不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
