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
    public partial class frm_Manage_AGV : Form
    {
        frm_Main us_frmmain;
        public frm_Manage_AGV(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }
        public void GetInfo()
        {
            DataTable AGV_Info = Fuction.Function.SELETE_AGV_INFO();
            if (AGV_Info != null && AGV_Info.Rows.Count > 0)
            {
                if (dg_AGVInfo.Rows.Count < AGV_Info.Rows.Count)
                {
                    dg_AGVInfo.Rows.Add(AGV_Info.Rows.Count - dg_AGVInfo.Rows.Count);
                }
                int GetCount = dg_AGVInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < AGV_Info.Rows.Count)
                    {
                        dg_AGVInfo.Rows[i].Cells[0].Value = AGV_Info.Rows[i][0].ToString().Trim();
                        dg_AGVInfo.Rows[i].Cells[1].Value = AGV_Info.Rows[i][7].ToString().Trim();
                        dg_AGVInfo.Rows[i].Cells[2].Value = AGV_Info.Rows[i][1].ToString().Trim();
                        dg_AGVInfo.Rows[i].Cells[3].Value = AGV_Info.Rows[i][2].ToString().Trim() == "1" ? "在线" : "离线";
                        dg_AGVInfo.Rows[i].Cells[4].Value = AGV_Info.Rows[i][3].ToString().Trim();
                        dg_AGVInfo.Rows[i].Cells[5].Value = Function.retErr(AGV_Info.Rows[i][8].ToString().Trim());
                        dg_AGVInfo.Rows[i].Cells[6].Value = AGV_Info.Rows[i][9].ToString().Trim();
                        dg_AGVInfo.Rows[i].Cells[7].Value = AGV_Info.Rows[i][10].ToString().Trim();
                        dg_AGVInfo.Rows[i].Cells[8].Value = AGV_Info.Rows[i][11].ToString().Trim() == "1" ? "前启" : "后启";

                        if (AGV_Info.Rows[i][2].ToString().Trim() == "-1")
                        {
                            if (dg_AGVInfo.Rows[i].Cells[3].Style.BackColor != Color.LightGray)
                                dg_AGVInfo.Rows[i].Cells[3].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (dg_AGVInfo.Rows[i].Cells[3].Style.BackColor != Color.Lime)
                                dg_AGVInfo.Rows[i].Cells[3].Style.BackColor = Color.Lime;
                        }
                        //错误代码行颜色显示
                        if (AGV_Info.Rows[i][8].ToString().Trim() != "0")
                        {
                            if (dg_AGVInfo.Rows[i].Cells[5].Style.BackColor != Color.Red)
                                dg_AGVInfo.Rows[i].Cells[5].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            if (dg_AGVInfo.Rows[i].Cells[5].Style.BackColor != Color.Lime)
                                dg_AGVInfo.Rows[i].Cells[5].Style.BackColor = Color.Lime;
                        }
                    }
                    else
                    {
                        dg_AGVInfo.Rows.RemoveAt(AGV_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_AGVInfo.Rows.Clear();
            }
        }

        private void dg_CarInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewColumn column = dg_AGVInfo.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        frm_Manage_AGVDetail frm_m_a_d = new frm_Manage_AGVDetail(
                            dg_AGVInfo.Rows[e.RowIndex].Cells[1].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[2].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[4].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[5].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[3].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[6].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[7].Value.ToString().Trim(),
                                dg_AGVInfo.Rows[e.RowIndex].Cells[8].Value.ToString().Trim());

                        frm_m_a_d.ShowDialog();
                        
                    }
                }
            }
        }    

    }
}
