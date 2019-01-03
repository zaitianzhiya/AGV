using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fuction;

namespace TS_RGB
{
    public partial class frm_Car : Form
    {
        frm_Main us_frmmain;
        int Lent = 650;
        int Map_X = 60;
        public frm_Car(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            Map_X = us_frmmain.Map_X;
            GetInfo();
            //取消选中行
            dg_CarInfo.Rows[0].Selected = false;
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
            //if (e.ColumnIndex == 3)
            //{
            //    if (dg_CarInfo.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Lime)
            //    {
            //        dg_CarInfo.Rows[e.RowIndex].Selected = false;
            //        dg_CarInfo.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.LightGray;
            //    }
            //    else
            //    {
            //        dg_CarInfo.Rows[e.RowIndex].Selected = false;
            //        dg_CarInfo.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Lime;
            //    }
            //}
            //else if (e.ColumnIndex == 2) 
            //{
            //    dg_CarInfo.Rows[e.RowIndex].Selected = false;
            //    frm_CarManual frm_carmanual = new frm_CarManual(us_frmmain, dg_CarInfo.Rows[e.RowIndex].Cells[1].Value.ToString().Trim(), e.RowIndex, ((dg_CarInfo.Rows[e.RowIndex].Cells[3].Value.ToString().Trim()).Equals("在线") ? 1 : 2));
            //    frm_carmanual.ShowDialog();
            //}
        }

        private void DG_CARMESSAGE_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dg_CarInfo.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dg_CarInfo.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        public void GetInfo()
        {
            DataTable AGV_Info = Fuction.Function.SELETE_AGV_INFO();
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
                        dg_CarInfo.Rows[i].Cells[0].Value = int.Parse(AGV_Info.Rows[i][0].ToString());
                        dg_CarInfo.Rows[i].Cells[1].Value = (int.Parse(AGV_Info.Rows[i][1].ToString().Trim().Substring(10, AGV_Info.Rows[i][1].ToString().Trim().Length - 10)) - 200).ToString();

                        dg_CarInfo.Rows[i].Cells[2].Value = AGV_Info.Rows[i][1].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[3].Value = AGV_Info.Rows[i][2].ToString().Trim() == "1" ? "在线" : "离线";
                        dg_CarInfo.Rows[i].Cells[4].Value = ((int.Parse(AGV_Info.Rows[i][4].ToString().Trim()) / Lent + (int.Parse(AGV_Info.Rows[i][4].ToString().Trim()) % Lent > (Lent / 2) ? 1 : 0)) * Map_X + int.Parse(AGV_Info.Rows[i][3].ToString().Trim()) / Lent + (int.Parse(AGV_Info.Rows[i][3].ToString().Trim()) % Lent > (Lent / 2) ? 1 : 0)).ToString()
                                                            + ":(" + (int.Parse(AGV_Info.Rows[i][3].ToString().Trim()) / Lent + (int.Parse(AGV_Info.Rows[i][3].ToString().Trim()) % Lent > (Lent/2) ? 1 : 0)).ToString()
                                                            + ","
                                                            + (int.Parse(AGV_Info.Rows[i][4].ToString().Trim()) / Lent + (int.Parse(AGV_Info.Rows[i][4].ToString().Trim()) % Lent > (Lent/2) ? 1 : 0)).ToString()
                                                            + ")";
                        dg_CarInfo.Rows[i].Cells[5].Value = AGV_Info.Rows[i][6].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[6].Value = Function.retErr(AGV_Info.Rows[i][14].ToString().Trim());
                        dg_CarInfo.Rows[i].Cells[7].Value = AGV_Info.Rows[i][9].ToString().Trim() + "mA";
                        dg_CarInfo.Rows[i].Cells[8].Value = AGV_Info.Rows[i][8].ToString().Trim() + "mV";
                        dg_CarInfo.Rows[i].Cells[9].Value = (Math.Abs(int.Parse(AGV_Info.Rows[i][10].ToString().Trim())) + Math.Abs(int.Parse(AGV_Info.Rows[i][11].ToString().Trim()))) > 1500 ? "行驶中>>>>>" : "停止中";
                        dg_CarInfo.Rows[i].Cells[10].Value = AGV_Info.Rows[i][15].ToString().Trim();// == "1" ? "锁定" : "未锁"
                        dg_CarInfo.Rows[i].Cells[11].Value = AGV_Info.Rows[i][24].ToString().Trim() == "1" ? "在二维码上" : "不在...";
                        dg_CarInfo.Rows[i].Cells[12].Value = AGV_Info.Rows[i][16].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[13].Value = AGV_Info.Rows[i][17].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[14].Value = AGV_Info.Rows[i][6].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[15].Value = AGV_Info.Rows[i][7].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[16].Value = AGV_Info.Rows[i][19].ToString().Trim();
                        dg_CarInfo.Rows[i].Cells[17].Value = AGV_Info.Rows[i][20].ToString().Trim();

                        if(AGV_Info.Rows[i][2].ToString().Trim()=="1")
                        {
                            if (dg_CarInfo.Rows[i].Cells[3].Style.BackColor != Color.Lime)
                                dg_CarInfo.Rows[i].Cells[3].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (dg_CarInfo.Rows[i].Cells[3].Style.BackColor != Color.LightGray)
                                dg_CarInfo.Rows[i].Cells[3].Style.BackColor = Color.LightGray;
                        }
                        //错误代码行颜色显示
                        if (AGV_Info.Rows[i][14].ToString().Trim() != "0")
                        {
                            if (dg_CarInfo.Rows[i].Cells[6].Style.BackColor != Color.Red)
                                dg_CarInfo.Rows[i].Cells[6].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (dg_CarInfo.Rows[i].Cells[6].Style.BackColor != Color.Lime)
                                dg_CarInfo.Rows[i].Cells[6].Style.BackColor = Color.Lime;
                        }
                        if (dg_CarInfo.Rows[i].Cells[3].Value.ToString() == "离线")
                        {
                            if (dg_CarInfo.Rows[i].Cells[6].Style.BackColor != Color.LightGray)
                                dg_CarInfo.Rows[i].Cells[6].Style.BackColor = Color.LightGray;
                        }
                        //电压颜色提示
                        if (int.Parse(AGV_Info.Rows[i][8].ToString().Trim()) < 50000)
                        {
                            if (dg_CarInfo.Rows[i].Cells[8].Style.BackColor != Color.Yellow)
                                dg_CarInfo.Rows[i].Cells[8].Style.BackColor = Color.Yellow;
                        }
                        else if (int.Parse(AGV_Info.Rows[i][8].ToString().Trim()) >= 50000)
                        {
                            if (dg_CarInfo.Rows[i].Cells[8].Style.BackColor != Color.Lime)
                                dg_CarInfo.Rows[i].Cells[8].Style.BackColor = Color.Lime;
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
            if (dg_CarInfo.SelectedRows != null && dg_CarInfo.SelectedRows.Count > 0)
            {
                us_frmmain.KIVA_SendTo_AGV_Initialize(dg_CarInfo.SelectedRows[0].Cells[2].Value.ToString().Trim());
            }
        }

        //down
        private void 装载料架ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dg_CarInfo.SelectedRows != null && dg_CarInfo.SelectedRows.Count > 0)
            {
                us_frmmain.KIVA_SendTo_AGV_SkipDown(dg_CarInfo.SelectedRows[0].Cells[2].Value.ToString().Trim()
                    , dg_CarInfo.SelectedRows[0].Cells[4].Value.ToString().Trim().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries)[1]
                    , dg_CarInfo.SelectedRows[0].Cells[4].Value.ToString().Trim().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries)[2]
                    , GetAng(dg_CarInfo.SelectedRows[0].Cells[16].Value.ToString().Trim()).ToString()
                    , GetAng(dg_CarInfo.SelectedRows[0].Cells[17].Value.ToString().Trim()).ToString());

            }
        }

        private void 举升托盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dg_CarInfo.SelectedRows != null && dg_CarInfo.SelectedRows.Count > 0)
            {
                us_frmmain.KIVA_SendTo_AGV_SkipUp(dg_CarInfo.SelectedRows[0].Cells[2].Value.ToString().Trim()
                        , dg_CarInfo.SelectedRows[0].Cells[4].Value.ToString().Trim().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries)[1]
                        , dg_CarInfo.SelectedRows[0].Cells[4].Value.ToString().Trim().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries)[2]
                        , GetAng(dg_CarInfo.SelectedRows[0].Cells[16].Value.ToString().Trim()).ToString()
                        , GetAng(dg_CarInfo.SelectedRows[0].Cells[17].Value.ToString().Trim()).ToString());
            }
        }

        public int GetAng(string ang)
        {
            int AGV_angle_A = int.Parse(ang.Split('.')[0]);
            if (AGV_angle_A < 45 | AGV_angle_A > 315)
                return 0;
            else if (AGV_angle_A > 45 && AGV_angle_A < 135)
                return 90;
            else if (AGV_angle_A > 135 && AGV_angle_A < 225)
                return 180;
            else if (AGV_angle_A > 225 && AGV_angle_A < 315)
                return 270;
            else
                return -1;
        }
    }
}
