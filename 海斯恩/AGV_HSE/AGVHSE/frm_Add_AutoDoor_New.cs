using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TS_RGB.Fuction;

namespace TS_RGB
{
    public partial class frm_Add_AutoDoor_New : Form
    {
        public frm_Add_AutoDoor_New()
        {
            InitializeComponent();
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            dg_AutoDoorfo.Rows.Clear();
        }

        private void bt_AddRFID_Click(object sender, EventArgs e)
        {
            int dg_count = dg_AutoDoorfo.Rows.Count - 1;
            int res = 0;
            string D_no, D_ip, D_LineNo, D_In_RFID, D_Out_RFID;
            int D_FW, D_X, D_Y;
            for (int i = 0; i < dg_count; i++)
            {
                if (dg_AutoDoorfo.Rows[i].Cells[1].Value != null && dg_AutoDoorfo.Rows[i].Cells[2].Value != null
                    && dg_AutoDoorfo.Rows[i].Cells[3].Value != null && dg_AutoDoorfo.Rows[i].Cells[4].Value != null
                    && dg_AutoDoorfo.Rows[i].Cells[5].Value != null && dg_AutoDoorfo.Rows[i].Cells[6].Value != null
                    && dg_AutoDoorfo.Rows[i].Cells[7].Value != null && dg_AutoDoorfo.Rows[i].Cells[8].Value != null)
                {
                    D_no = dg_AutoDoorfo.Rows[i].Cells[1].Value.ToString().Trim();
                    D_ip = dg_AutoDoorfo.Rows[i].Cells[2].Value.ToString().Trim();
                    D_LineNo = dg_AutoDoorfo.Rows[i].Cells[3].Value.ToString().Trim();
                    D_In_RFID = dg_AutoDoorfo.Rows[i].Cells[4].Value.ToString().Trim();
                    D_Out_RFID = dg_AutoDoorfo.Rows[i].Cells[5].Value.ToString().Trim();
                    D_FW = dg_AutoDoorfo.Rows[i].Cells[6].Value.ToString().Trim() == "前进" ? 1 : 2;
                    D_X = int.Parse(dg_AutoDoorfo.Rows[i].Cells[7].Value.ToString().Trim());
                    D_Y = int.Parse(dg_AutoDoorfo.Rows[i].Cells[8].Value.ToString().Trim());

                    //逐条添加到:tb_AutoDoor_INFO
                    res += Function.INSERT_AutoDoor_INFO_New(D_no, D_ip, D_LineNo, D_X, D_Y, D_In_RFID, D_FW, D_Out_RFID);
                }
                else
                {
                    MessageBox.Show("参数错误或缺失，请填写完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (res > 0)
            {
                MessageBox.Show("自动门添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("添加失败，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public DataGridViewTextBoxEditingControl CellEdit = null;
        private void dg_AutoDoorfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.dg_AutoDoorfo.CurrentCellAddress.X == 7 || this.dg_AutoDoorfo.CurrentCellAddress.X == 8)//获取当前处于活动状态的单元格索引
            {
                CellEdit = (DataGridViewTextBoxEditingControl)e.Control;
                CellEdit.SelectAll();
                CellEdit.KeyPress += Cells_KeyPress; //绑定事件
            }
        }
        private void Cells_KeyPress(object sender, KeyPressEventArgs e) //自定义事件
        {
            if ((this.dg_AutoDoorfo.CurrentCellAddress.X == 7) || (this.dg_AutoDoorfo.CurrentCellAddress.X == 8))//获取当前处于活动状态的单元格索引
            {
                if (!(e.KeyChar >= '0' && e.KeyChar <= '9')) e.Handled = true;
                if (e.KeyChar == '\b') e.Handled = false;
            }
        }
    }
}
