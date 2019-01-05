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
    public partial class frm_Monitor_AnNiuTaskReset : Form
    {
        frm_Main us_frmmain;

        public frm_Monitor_AnNiuTaskReset(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable Buffer_temp_Info = Fuction.Function.SELECT_Buffer_temp_ALL();
            if (Buffer_temp_Info != null && Buffer_temp_Info.Rows.Count > 0)
            {
                if (dg_AnNiuTaskReset.Rows.Count < Buffer_temp_Info.Rows.Count)
                {
                    dg_AnNiuTaskReset.Rows.Add(Buffer_temp_Info.Rows.Count - dg_AnNiuTaskReset.Rows.Count);
                }
                int GetCount = dg_AnNiuTaskReset.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < Buffer_temp_Info.Rows.Count)
                    {
                        dg_AnNiuTaskReset.Rows[i].Cells[0].Value = Buffer_temp_Info.Rows[i][0].ToString().Trim();
                        dg_AnNiuTaskReset.Rows[i].Cells[1].Value = (int.Parse(Buffer_temp_Info.Rows[i][2].ToString().Trim().Split('.')[3]) - 50).ToString().Trim();
                        dg_AnNiuTaskReset.Rows[i].Cells[2].Value = Buffer_temp_Info.Rows[i][2].ToString().Trim();
                        dg_AnNiuTaskReset.Rows[i].Cells[3].Value = Buffer_temp_Info.Rows[i][4].ToString().Trim() == "1" ? "任务进行中>>>>>" : "无任务空闲.....";


                        if (Buffer_temp_Info.Rows[i][4].ToString().Trim() == "1")
                        {
                            if (dg_AnNiuTaskReset.Rows[i].Cells[3].Style.BackColor != Color.Lime)
                                dg_AnNiuTaskReset.Rows[i].Cells[3].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (dg_AnNiuTaskReset.Rows[i].Cells[3].Style.BackColor != Color.White)
                                dg_AnNiuTaskReset.Rows[i].Cells[3].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        dg_AnNiuTaskReset.Rows.RemoveAt(Buffer_temp_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_AnNiuTaskReset.Rows.Clear();
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
