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
    public partial class FrmAreaInfo : BaseForm
    {
        private DataTable dtSource;
        public FrmAreaInfo()
        {
            InitializeComponent();
        }

        private void FrmAreaInfo_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_AREA_INFO");
            gc1.DataSource = dtSource;
        }

        private void btnAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckIsNull())
            {
                return;
            }
            string dtMax = dtSource.Compute("Max(OwnArea)", null).ToString();
            dtMax = string.IsNullOrEmpty(dtMax) ? "0" : dtMax;
            string dbMax = Function.GetMaxValue("PR_SELECT_MAX_AREACODE");
            DataRow drNew = dtSource.NewRow();
            drNew["OwnArea"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax));
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
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "OwnArea" });
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxShow.Alert("区域编码重复", MessageBoxIcon.Exclamation);
                return;
            }
            Function.Update_tbAreaInfo(dtSource);
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
                    if (string.IsNullOrEmpty(dr["OwnArea"].ToString()))
                    {
                        MessageBoxShow.Alert("区域编码不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["AreaName"].ToString()))
                    {
                        MessageBoxShow.Alert("区域名称不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
