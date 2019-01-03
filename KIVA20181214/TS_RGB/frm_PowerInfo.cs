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
    public partial class frm_PowerInfo : Form
    {
        frm_Main us_frmmain;
        public frm_PowerInfo(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable power_Info = Fuction.Function.KIVA_SELECT_Charge();
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
                        dg_PowerInfo.Rows[i].Cells[0].Value = power_Info.Rows[i][0].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[1].Value = power_Info.Rows[i][1].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[2].Value = power_Info.Rows[i][2].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[3].Value = power_Info.Rows[i][3].ToString().Trim();
                        if (power_Info.Rows[i][4].ToString().Trim() == "1")
                            dg_PowerInfo.Rows[i].Cells[4].Value = "停止充电";
                        if (power_Info.Rows[i][4].ToString().Trim() == "2")
                            dg_PowerInfo.Rows[i].Cells[4].Value = "正在充电";
                        if (power_Info.Rows[i][4].ToString().Trim() == "0")
                            dg_PowerInfo.Rows[i].Cells[4].Value = "掉线...";
                        if (power_Info.Rows[i][4].ToString().Trim() == "3")
                            dg_PowerInfo.Rows[i].Cells[4].Value = "小车返回中";
                        dg_PowerInfo.Rows[i].Cells[5].Value = power_Info.Rows[i][5].ToString().Trim() + "," + power_Info.Rows[i][6].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[6].Value = power_Info.Rows[i][7].ToString().Trim();
                        dg_PowerInfo.Rows[i].Cells[7].Value = power_Info.Rows[i][8].ToString().Trim();


                        if (power_Info.Rows[i][4].ToString().Trim() == "2")
                        {

                            if (dg_PowerInfo.Rows[i].Cells[4].Style.BackColor != Color.Lime)
                                dg_PowerInfo.Rows[i].Cells[4].Style.BackColor = Color.Lime;
                        }
                        else if (power_Info.Rows[i][4].ToString().Trim() == "1")
                        {
                            if (dg_PowerInfo.Rows[i].Cells[4].Style.BackColor != Color.Yellow)
                                dg_PowerInfo.Rows[i].Cells[4].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            if (dg_PowerInfo.Rows[i].Cells[4].Style.BackColor != Color.LightGray)
                                dg_PowerInfo.Rows[i].Cells[4].Style.BackColor = Color.LightGray;
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
