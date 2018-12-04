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
    public partial class FrmTaskDetail : BaseForm
    {
        private string mode = "";//模式,新增N，编辑M
        private string taskConditonCode = "";
        private DataTable dtSource;
        private DataTable dtSource2;
        private DataTable dtMaterial;
        private DataTable dtArea;

        public FrmTaskDetail()
        {
            InitializeComponent();
            mode = "N";
        }

        public FrmTaskDetail(string taskConditonCode, string name)
        {
            InitializeComponent();
            this.taskConditonCode = taskConditonCode;
            mode = "M";
            txtTaskConditonCode.Text = taskConditonCode;
            txtTaskConditonName.Text = name;
        }

        private void FrmTaskDetail_Load(object sender, EventArgs e)
        {
            dtArea = Function.GetDataInfo("PR_SELECT_AREA_INFO");
            repositoryItemLookUpEdit1.DataSource = dtArea;
            repositoryItemLookUpEdit1.DisplayMember = "AreaName";
            repositoryItemLookUpEdit1.ValueMember = "OwnArea";
       
            dtMaterial = Function.GetDataInfo("PR_SELECT_MATERIAL_INFO");
            repositoryItemLookUpEdit2.DataSource = dtMaterial;
            repositoryItemLookUpEdit2.DisplayMember = "MaterialName";
            repositoryItemLookUpEdit2.ValueMember = "MaterialType";


            dtSource = Function.GettbTaskConfigDetail(string.IsNullOrEmpty(taskConditonCode) ? "-99" : taskConditonCode);
            gc1.DataSource = dtSource;

            if (dtSource.Rows.Count > 0)
            {
                dtSource2 = Function.GettbTaskMustPass(dtSource.Rows[0]["TaskConditonCode"].ToString(),
                    dtSource.Rows[0]["DetailID"].ToString());
                gc2.DataSource = dtSource2;
            }

            if (mode == "N")
            {
                string maxCallId = Function.GetMaxValue("PR_SELECT_MAX_TASKDETAILID");
                txtTaskConditonCode.Text = int.Parse(maxCallId).ToString("0000");
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow drNew = dtSource.NewRow();
            string dtMax = dtSource.Compute("Max(DetailID)", null).ToString();
            if (string.IsNullOrEmpty(dtMax))
            {
                dtMax = "0";
            }
            string dbMax = Function.GetMaxDetailID(txtTaskConditonCode.Text.Trim());
            drNew["DetailID"] = Math.Max(int.Parse(dtMax) + 1, int.Parse(dbMax));
            if (dtArea.Rows.Count > 0)
            {
                drNew["ArmOwnArea"] = dtArea.Rows[0]["OwnArea"];
            }
            drNew["StorageState"] = "0";
            if (dtMaterial.Rows.Count > 0)
            {
                drNew["MaterialType"] = dtMaterial.Rows[0]["MaterialType"];
            }
            drNew["Action"] = "0";
            drNew["IsWaitPass"] = "1";
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

            if (string.IsNullOrEmpty(txtTaskConditonCode.Text.Trim()))
            {
                MessageBoxShow.Alert("任务条件配置编号不能为空", MessageBoxIcon.Exclamation);
                txtTaskConditonCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtTaskConditonName.Text.Trim()))
            {
                MessageBoxShow.Alert("任务条件配置名称不能为空", MessageBoxIcon.Exclamation);
                txtTaskConditonName.Focus();
                return;
            }
            if (dtSource.Rows.Count == 0 || dtSource.AsEnumerable().Where(p => p.RowState != DataRowState.Deleted).Count() <= 0)
            {
                MessageBoxShow.Alert("请先维护明细数据", MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["TaskConditonCode"] = txtTaskConditonCode.Text.Trim();
                }
            }
            Function.Update_tbTaskConfigDetail(mode, txtTaskConditonCode.Text.Trim(), txtTaskConditonName.Text.Trim(), dtSource);
            dtSource.AcceptChanges();
            MessageBoxShow.Alert("保存成功", MessageBoxIcon.Asterisk);
            DialogResult = DialogResult.OK;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void gv1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gv1.FocusedRowHandle >= 0)
            {
                dtSource2 =
                    Function.GettbTaskMustPass(
                        gv1.GetRowCellValue(gv1.FocusedRowHandle, "TaskConditonCode").ToString(),
                        gv1.GetRowCellValue(gv1.FocusedRowHandle, "DetailID").ToString());
                gc2.DataSource = dtSource2;
            }
        }

        private void gv1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks==2&&gv1.FocusedRowHandle >= 0 && e.Column.FieldName == "DetailID")
            {
                string code = gv1.GetRowCellValue(gv1.FocusedRowHandle, "TaskConditonCode").ToString();
                string detailId = gv1.GetRowCellValue(gv1.FocusedRowHandle, "DetailID").ToString();
                FrmMustPass frm = new FrmMustPass(txtTaskConditonCode.Text.Trim(), detailId);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    dtSource2 = Function.GettbTaskMustPass((string.IsNullOrEmpty(code) ? txtTaskConditonCode.Text.Trim() : code), detailId);
                    gc2.DataSource = dtSource2;
                }
            }
        }

        private void repositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gv1.FocusedRowHandle > -1)
            {
                ComboBoxEdit cmb = sender as ComboBoxEdit;
                if (cmb != null)
                {
                    gv1.SetRowCellValue(gv1.FocusedRowHandle, "StorageState", cmb.SelectedIndex);
                }
            }
        }

        private void repositoryItemComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gv1.FocusedRowHandle > -1)
            {
                ComboBoxEdit cmb = sender as ComboBoxEdit;
                if (cmb != null)
                {
                    gv1.SetRowCellValue(gv1.FocusedRowHandle, "Action", cmb.SelectedIndex);
                }
            }
        }

        private void repositoryItemComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gv1.FocusedRowHandle > -1)
            {
                ComboBoxEdit cmb = sender as ComboBoxEdit;
                if (cmb != null)
                {
                    gv1.SetRowCellValue(gv1.FocusedRowHandle, "IsWaitPass", cmb.SelectedIndex);
                }
            }
        }
    }
}
