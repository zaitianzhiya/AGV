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
    public partial class FrmCallDetailsInfo : BaseForm
    {
        private string mode = "";//模式,新增N，编辑M
        private string callBoxID = "";
        private DataTable dtSource;
        private DataTable dtLook;

        public FrmCallDetailsInfo()
        {
            InitializeComponent();
            mode = "N";
        }

        public FrmCallDetailsInfo(string callBoxID)
        {
            InitializeComponent();
            this.callBoxID = callBoxID;
            mode = "M";
        }

        private void FrmCallDetailsInfo_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetCallBox(string.IsNullOrEmpty(callBoxID) ? "-99" : callBoxID);
            gc1.DataSource = dtSource;

            if (dtSource.Rows.Count > 0)
            {
                txtCallBoxID.Text = callBoxID;
                txtCallBoxName.Text = dtSource.Rows[0]["CallBoxName"].ToString();
                cbxCallBoxType.SelectedIndex = int.Parse(dtSource.Rows[0]["CallBoxType"].ToString());
            }
            if (mode == "N")
            {
                string maxCallId = Function.GetMaxValue("PR_SELECT_MAX_CALLBOXID");
                txtCallBoxID.Text = maxCallId;
                cbxCallBoxType.SelectedIndex = 0;
            }

            dtLook = Function.GetDataInfo("PR_SELECT_TASKCONFIGINFO");
            repositoryItemLookUpEdit1.DataSource = dtLook;
            repositoryItemLookUpEdit1.DisplayMember = "TaskConditonName";
            repositoryItemLookUpEdit1.ValueMember = "TaskConditonCode";
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gv1.CloseEditor();
            gv1.UpdateCurrentRow();
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "CallBoxID", "ButtonID" });
            if (dtTemp.AsEnumerable().Where(p => string.IsNullOrEmpty(p["ButtonID"].ToString()) || p["ButtonID"].ToString()=="0").Any())
            {
                MessageBoxShow.Alert("按钮号不能为空或0", MessageBoxIcon.Exclamation);
                return;
            }
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxShow.Alert("当前按钮号重复", MessageBoxIcon.Exclamation);
                return;
            }
            DataRow drNew = dtSource.NewRow();
            drNew["CallBoxID"] = txtCallBoxID.Text;
            drNew["ButtonID"] = "0";
            if (dtLook != null && dtLook.Rows.Count > 0)
            {
                drNew["TaskConditonCode"] = dtLook.Rows[0]["TaskConditonCode"];
                drNew["TaskConditonName"] = dtLook.Rows[0]["TaskConditonName"];
            }
            drNew["OperaType"] = "0";
            drNew["OperaTypeName"] = "呼叫或监控";
            drNew["LocationID"] = "0";
            drNew["LocationState"] = "0";
            drNew["LocationStateName"] = "空位置";
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
                dtSource.Select(string.Format("CallBoxID='{0}' and ButtonID='{1}'", txtCallBoxID.Text.Trim(), gv1.GetRowCellValue(gv1.FocusedRowHandle, "ButtonID")))[0].Delete();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gv1.CloseEditor();
            gv1.UpdateCurrentRow();
            if (string.IsNullOrEmpty(txtCallBoxName.Text.Trim()))
            {
                MessageBoxShow.Alert("呼叫器名称不能为空", MessageBoxIcon.Exclamation);
                txtCallBoxName.Focus();
                return;
            }
            if (dtSource.Rows.Count == 0 || dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count() <= 0)
            {
                MessageBoxShow.Alert("请先维护明细数据", MessageBoxIcon.Exclamation);
                return;
            }
            DataView dv = dtSource.DefaultView;
            DataTable dtTemp = dv.ToTable("dt", true, new string[] { "CallBoxID", "ButtonID" });
            if (dtTemp.AsEnumerable().Where(p => string.IsNullOrEmpty(p["ButtonID"].ToString()) || p["ButtonID"].ToString() == "0").Any())
            {
                MessageBoxShow.Alert("按钮号不能为空或0", MessageBoxIcon.Exclamation);
                return;
            }
            if (dtTemp.Rows.Count != dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count())
            {
                MessageBoxShow.Alert("当前按钮号重复", MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["ButtonID"].ToString()))
                    {
                        MessageBoxShow.Alert("按钮号不能为空", MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (string.IsNullOrEmpty(dr["LocationID"].ToString()))
                    {
                        MessageBoxShow.Alert("监控储位ID不能为空", MessageBoxIcon.Exclamation);
                        return;
                    }
                    dr["CallBoxName"] = txtCallBoxName.Text.Trim();
                    dr["CallBoxType"] = cbxCallBoxType.SelectedIndex.ToString();
                }
            }
            Function.Update_tbCallBox(mode, txtCallBoxID.Text.Trim(), txtCallBoxName.Text.Trim(), cbxCallBoxType.SelectedIndex.ToString(), dtSource);
            //dtSource.AcceptChanges();
            MessageBoxShow.Alert("保存成功", MessageBoxIcon.Asterisk);
            DialogResult = DialogResult.OK;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void repositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit cmb = sender as ComboBoxEdit;
            if (cmb != null)
            {
                gv1.SetRowCellValue(gv1.FocusedRowHandle, "OperaType", cmb.SelectedIndex);
            }
        }

        private void repositoryItemComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit cmb = sender as ComboBoxEdit;
            if (cmb != null)
            {
                gv1.SetRowCellValue(gv1.FocusedRowHandle, "LocationState", cmb.SelectedIndex);
            }
        }
    }
}
