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
    public partial class frm_Monitor_Car : Form
    {
        frm_Login frm_login = null;
        frm_Main us_frmmain;
        int[] Car_AC;
        public frm_Monitor_Car(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
           // ControlBox = false;
            GetInfo();
        }
        /// <summary>
        /// 禁用Close按钮
        /// </summary>
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;

                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;

                return myCp;

            }
        }
        private void DG_CARMESSAGE_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if (dg_CarInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Lime)
                {
                    dg_CarInfo.Rows[e.RowIndex].Selected = false;
                    dg_CarInfo.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightGray;
                }
                else
                {
                    dg_CarInfo.Rows[e.RowIndex].Selected = false;
                    dg_CarInfo.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Lime;
                }
            }
            else if (e.ColumnIndex == 2) 
            {
                dg_CarInfo.Rows[e.RowIndex].Selected = false;
                frm_Monitor_Car_Manual frm_carmanual = new frm_Monitor_Car_Manual(us_frmmain, dg_CarInfo.Rows[e.RowIndex].Cells[1].Value.ToString().Trim(), e.RowIndex, ((dg_CarInfo.Rows[e.RowIndex].Cells[3].Value.ToString().Trim()).Equals("在线") ? 1 : 2));
                frm_carmanual.ShowDialog();
            }
        }

        private void DG_CARMESSAGE_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dg_CarInfo.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dg_CarInfo.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        public void GetInfo()
        {
            DataTable AGV_Info = Fuction.Function.SELETE_AGV_INFO(us_frmmain.lineNo);
            if (AGV_Info != null && AGV_Info.Rows.Count > 0)
            {
                if (dg_CarInfo.Rows.Count < AGV_Info.Rows.Count)
                {
                    dg_CarInfo.Rows.Add(AGV_Info.Rows.Count - dg_CarInfo.Rows.Count);
                }
                int GetCount = dg_CarInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < AGV_Info.Rows.Count)
                    {
                        dg_CarInfo.Rows[i].Cells[0].Value = AGV_Info.Rows[i][0].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[1].Value = AGV_Info.Rows[i][7].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[2].Value = AGV_Info.Rows[i][1].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[3].Value = AGV_Info.Rows[i][2].ToString().Trim() == "1" ? "在线" : "离线";
                        dg_CarInfo.Rows[i].Cells[4].Value = AGV_Info.Rows[i][3].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[5].Value = AGV_Info.Rows[i][5].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[6].Value = AGV_Info.Rows[i][6].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[7].Value = Function.retErr(AGV_Info.Rows[i][8].ToString().Trim());
                        //dg_CarInfo.Rows[i].Cells[7].Value = (Math.Round(((int.Parse(AGV_Info.Rows[i][9].ToString().Trim()) - 200) / 60.00f), 2) * 100.00f).ToString() + "%:" + AGV_Info.Rows[i][9].ToString().Trim() + "";
                        dg_CarInfo.Rows[i].Cells[8].Value = GetVoltageString(AGV_Info.Rows[i][9].ToString().Trim());

                        dg_CarInfo.Rows[i].Cells[9].Value = AGV_Info.Rows[i][10].ToString().Trim() == "1" ? "行驶中>>>" : "停止中";
                        if(AGV_Info.Rows[i][2].ToString().Trim()=="-1")
                        {
                            if(dg_CarInfo.Rows[i].Cells[3].Style.BackColor != Color.LightGray)
                            dg_CarInfo.Rows[i].Cells[3].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (dg_CarInfo.Rows[i].Cells[3].Style.BackColor != Color.Lime)
                            dg_CarInfo.Rows[i].Cells[3].Style.BackColor = Color.Lime;
                        }
                        //错误代码行颜色显示
                        if (AGV_Info.Rows[i][8].ToString().Trim() != "0")
                        {
                            if (dg_CarInfo.Rows[i].Cells[7].Style.BackColor != Color.Red)
                                dg_CarInfo.Rows[i].Cells[7].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (dg_CarInfo.Rows[i].Cells[7].Style.BackColor != Color.Lime)
                                dg_CarInfo.Rows[i].Cells[7].Style.BackColor = Color.Lime;
                        }
                        //电压颜色提示
                        if (int.Parse(AGV_Info.Rows[i][9].ToString().Trim()) < 240 && int.Parse(AGV_Info.Rows[i][9].ToString().Trim()) > 225)
                        {
                            if (dg_CarInfo.Rows[i].Cells[8].Style.BackColor != Color.Yellow)
                                dg_CarInfo.Rows[i].Cells[8].Style.BackColor = Color.Yellow;
                        }
                        else if(int.Parse(AGV_Info.Rows[i][9].ToString().Trim()) >=240)
                        {
                            if (dg_CarInfo.Rows[i].Cells[8].Style.BackColor != Color.Lime)
                                dg_CarInfo.Rows[i].Cells[8].Style.BackColor = Color.Lime;
                        }
                        else if(int.Parse(AGV_Info.Rows[i][9].ToString().Trim()) <= 225)
                        {
                            if (dg_CarInfo.Rows[i].Cells[8].Style.BackColor != Color.Red)
                                dg_CarInfo.Rows[i].Cells[8].Style.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        dg_CarInfo.Rows.RemoveAt(AGV_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_CarInfo.Rows.Clear();
            }
        }    
        private void tm_Del_Click(object sender, EventArgs e)
        {
            if (dg_CarInfo.SelectedRows != null && dg_CarInfo.SelectedRows.Count > 0 && dg_CarInfo.SelectedRows[0].Cells[2].Value.ToString().Trim()=="-1")
            {
                Fuction.Function.DELETE_AGV_INFO(int.Parse(dg_CarInfo.SelectedRows[0].Cells[0].Value.ToString().Trim()));
            }
        }

        private void tm_St_Click(object sender, EventArgs e)
        {

        }

        private void 清除电梯占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            if (MessageBox.Show("确定清除电梯占用吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (dg_CarInfo.SelectedRows != null && dg_CarInfo.SelectedRows.Count > 0)
                {
                    int res_elc = Function.UPDATE_ELC_InAGVNo(int.Parse(dg_CarInfo.SelectedRows[0].Cells[1].Value.ToString().Trim()));

                    if (res_elc != 0)
                    {
                        MessageBox.Show("电梯占用已清除，请关注车辆及电梯状况！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("清除失败或不存在电梯占用，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("请先选中该行.....", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                }
            }
        }

        private void 清除全部管控占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            if (MessageBox.Show("确定清除管控占用吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (dg_CarInfo.SelectedRows != null && dg_CarInfo.SelectedRows.Count > 0)
                {
                    int res_cross = Function.UPDATE_Cross_cac_agv(int.Parse(dg_CarInfo.SelectedRows[0].Cells[1].Value.ToString().Trim()));
                    if (res_cross != 0)
                    {
                        MessageBox.Show("管控占用已清除，请关注车辆及路口状况！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("清除失败或不存在管控占用，请稍后重试...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("请先选中该行.....", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        public string GetVoltageString(string vol)
        {
            double temp = (Math.Round(((int.Parse(vol) - 200) / 60.00f), 2) * 100.00f);
            if (temp < 0)
                return "电量过低！";
            else
                return temp.ToString() + "%: " + vol;
        }
        
    }
}
