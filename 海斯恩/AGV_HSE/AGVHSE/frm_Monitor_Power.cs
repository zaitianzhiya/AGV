using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TS_RGB
{
    public partial class frm_Monitor_Power : Form
    {
        frm_Main us_frmmain;
        public frm_Monitor_Power(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable power_Info = Fuction.Function.SELECT_Power_INFO(us_frmmain.lineNo);
            if (power_Info != null && power_Info.Rows.Count > 0)
            {
                if (dg_PowerInfo.Rows.Count < power_Info.Rows.Count)
                {
                    dg_PowerInfo.Rows.Add(power_Info.Rows.Count - dg_PowerInfo.Rows.Count);
                }
                int GetCount = dg_PowerInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < power_Info.Rows.Count)
                    {
                        dg_PowerInfo.Rows[i].Cells[0].Value = power_Info.Rows[i][1].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[1].Value = power_Info.Rows[i][2].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[2].Value = power_Info.Rows[i][15].ToString().Trim() == "1" ? "在线" : "离线";
                        dg_PowerInfo.Rows[i].Cells[3].Value = power_Info.Rows[i][12].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[4].Value = power_Info.Rows[i][13].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[5].Value = power_Info.Rows[i][16].ToString().Trim() == "1" ? "正在充电" : "停止充电";
                        dg_PowerInfo.Rows[i].Cells[6].Value = power_Info.Rows[i][17].ToString().Trim();

                        if (power_Info.Rows[i][15].ToString().Trim() == "1")
                        {

                            if (dg_PowerInfo.Rows[i].Cells[2].Style.BackColor != Color.Lime)
                                dg_PowerInfo.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                        }                                  
                        else                               
                        {                                  
                            if (dg_PowerInfo.Rows[i].Cells[2].Style.BackColor != Color.LightGray)
                                dg_PowerInfo.Rows[i].Cells[2].Style.BackColor = Color.LightGray;
                        }

                        if (power_Info.Rows[i][16].ToString().Trim() == "1")
                        {

                            if (dg_PowerInfo.Rows[i].Cells[5].Style.BackColor != Color.Lime)
                                dg_PowerInfo.Rows[i].Cells[5].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (dg_PowerInfo.Rows[i].Cells[5].Style.BackColor != Color.LightGray)
                                dg_PowerInfo.Rows[i].Cells[5].Style.BackColor = Color.LightGray;
                        }
                    }
                    else
                    {
                        dg_PowerInfo.Rows.RemoveAt(power_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_PowerInfo.Rows.Clear();
            }
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
    }
}
