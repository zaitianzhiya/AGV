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
    public partial class FrmAction : BaseForm
    {
        private DataTable dtSource;
        public FrmAction()
        {
            InitializeComponent();
        }

        private void FrmAction_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_ACTION");
            gc1.DataSource = dtSource;
        }

        private void btnAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "ActionID" });
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxShow.Alert("动作ID重复", MessageBoxIcon.Exclamation);
                return;
            }
            Function.Update_tbActionInfo(dtSource);
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
                    if (string.IsNullOrEmpty(dr["ActionID"].ToString()))
                    {
                        MessageBoxShow.Alert("动作ID不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["ActionName"].ToString()))
                    {
                        MessageBoxShow.Alert("动作名称不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["WaitTime"].ToString()))
                    {
                        MessageBoxShow.Alert("等待时间不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["CommondText"].ToString()))
                    {
                        MessageBoxShow.Alert("指令不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
