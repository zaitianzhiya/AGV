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
    public partial class frm_Monitor_CallBox : Form
    {
        frm_Main us_frmmain;
        public frm_Monitor_CallBox(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable callBox_Info = Fuction.Function.SELECT_CallBoxLogic_ALL();
            if (callBox_Info != null && callBox_Info.Rows.Count > 0)
            {
                if (dg_callBoxInfo.Rows.Count < callBox_Info.Rows.Count)
                {
                    dg_callBoxInfo.Rows.Add(callBox_Info.Rows.Count - dg_callBoxInfo.Rows.Count);
                }
                int GetCount = dg_callBoxInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < callBox_Info.Rows.Count)
                    {
                        dg_callBoxInfo.Rows[i].Cells[0].Value = callBox_Info.Rows[i][0].ToString().Trim();
                        dg_callBoxInfo.Rows[i].Cells[1].Value = callBox_Info.Rows[i][1].ToString().Trim();
                        dg_callBoxInfo.Rows[i].Cells[2].Value = callBox_Info.Rows[i][2].ToString().Trim();
                        dg_callBoxInfo.Rows[i].Cells[3].Value = callBox_Info.Rows[i][3].ToString().Trim();
                        dg_callBoxInfo.Rows[i].Cells[4].Value = callBox_Info.Rows[i][4].ToString().Trim() == "0" ? "未按下" : "已按下";
                        dg_callBoxInfo.Rows[i].Cells[5].Value = callBox_Info.Rows[i][5].ToString().Trim();


                        if (callBox_Info.Rows[i][4].ToString().Trim() == "0")
                        {

                            if (dg_callBoxInfo.Rows[i].Cells[4].Style.BackColor != Color.LightGray)
                                dg_callBoxInfo.Rows[i].Cells[4].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (dg_callBoxInfo.Rows[i].Cells[4].Style.BackColor != Color.Lime)
                                dg_callBoxInfo.Rows[i].Cells[4].Style.BackColor = Color.Lime;
                        }
                    }
                    else
                    {
                        dg_callBoxInfo.Rows.RemoveAt(callBox_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_callBoxInfo.Rows.Clear();
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
