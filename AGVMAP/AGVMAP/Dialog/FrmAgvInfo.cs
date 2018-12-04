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
    public partial class FrmAgvInfo : BaseForm
    {
        private DataTable dtSource;
        public FrmAgvInfo()
        {
            InitializeComponent();
        }

        private void FrmAgvInfo_Load(object sender, EventArgs e)
        {
            dtSource = Function.GetDataInfo("PR_SELECT_TBCAR");
            gc1.DataSource = dtSource;
        }

        private void btnAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckIsNull())
            {
                return;
            }
            string dbMax = Function.GetMaxValue("PR_SELECT_CARCODE");
            string dtMax = dtSource.Compute("MAX(CarCode)", "").ToString();
            dtMax = string.IsNullOrEmpty(dtMax) ? "1" : (int.Parse(dtMax) + 1).ToString();
            int max = Math.Max(int.Parse(dbMax), int.Parse(dtMax));
            DataRow drNew = dtSource.NewRow();
            drNew["CarCode"] = max.ToString();
            drNew["CarType"] = "0";
            drNew["CarTypeName"] = "单向";
            dtSource.Rows.Add(drNew);
            gv1.MoveLastVisible();
        }

        private void btnDelRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gv1.FocusedRowHandle<0)
            {
                MessageBoxShow.Alert("请先选择要删除的行", MessageBoxIcon.Exclamation);
                return;
            }
            gv1.GetFocusedDataRow().Delete();
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
            Function.Update_tbCar(dtSource);
            dtSource.AcceptChanges();
            MessageBoxShow.Alert("保存成功", MessageBoxIcon.Asterisk);
        }

        private void btnQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void repositoryItemComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit cmb = sender as ComboBoxEdit;
            if (cmb != null)
            {
                gv1.SetRowCellValue(gv1.FocusedRowHandle,"CarType",cmb.SelectedIndex);
            }
        }

        bool CheckIsNull()
        {
            gv1.PostEditor();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (string.IsNullOrEmpty(dr["CarName"].ToString()))
                    {
                        MessageBoxShow.Alert("设备名称不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["StandbyLandMark"].ToString()))
                    {
                        MessageBoxShow.Alert("待命点不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["CarIP"].ToString()))
                    {
                        MessageBoxShow.Alert("IP地址不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (string.IsNullOrEmpty(dr["CarPort"].ToString()))
                    {
                        MessageBoxShow.Alert("端口号不能为空", MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
