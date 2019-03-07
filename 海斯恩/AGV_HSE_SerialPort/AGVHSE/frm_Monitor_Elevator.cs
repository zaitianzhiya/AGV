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
    public partial class frm_Monitor_Elevator : Form
    {
        frm_Main us_frmmain;
        public frm_Monitor_Elevator(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable elec_Info = Fuction.Function.PSELECT_ELC();
            if (elec_Info != null && elec_Info.Rows.Count > 0)
            {
                if (dg_ELECInfo.Rows.Count < elec_Info.Rows.Count)
                {
                    dg_ELECInfo.Rows.Add(elec_Info.Rows.Count - dg_ELECInfo.Rows.Count);
                }
                int GetCount = dg_ELECInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < elec_Info.Rows.Count)
                    {
                        dg_ELECInfo.Rows[i].Cells[0].Value = elec_Info.Rows[i][1].ToString().Trim();
                        dg_ELECInfo.Rows[i].Cells[1].Value = elec_Info.Rows[i][2].ToString().Trim();
                        dg_ELECInfo.Rows[i].Cells[2].Value = elec_Info.Rows[i][10].ToString().Trim() == "1" ? "在线" : "离线";
                        dg_ELECInfo.Rows[i].Cells[3].Value = elec_Info.Rows[i][8].ToString().Trim();
                        dg_ELECInfo.Rows[i].Cells[4].Value = elec_Info.Rows[i][9].ToString().Trim() == "1" ? "打开" : "闭合";
                        dg_ELECInfo.Rows[i].Cells[5].Value = elec_Info.Rows[i][11].ToString().Trim();
                        dg_ELECInfo.Rows[i].Cells[6].Value = elec_Info.Rows[i][12].ToString().Trim() == "0" ? "空：可入..." : "非空：不可入";



                        if (elec_Info.Rows[i][10].ToString().Trim() == "1")
                        {

                            if (dg_ELECInfo.Rows[i].Cells[2].Style.BackColor != Color.Lime)
                                dg_ELECInfo.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (dg_ELECInfo.Rows[i].Cells[2].Style.BackColor != Color.LightGray)
                                dg_ELECInfo.Rows[i].Cells[2].Style.BackColor = Color.LightGray;
                        }

                        if (elec_Info.Rows[i][12].ToString().Trim() == "0")
                        {

                            if (dg_ELECInfo.Rows[i].Cells[6].Style.BackColor != Color.Lime)
                                dg_ELECInfo.Rows[i].Cells[6].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (dg_ELECInfo.Rows[i].Cells[6].Style.BackColor != Color.Red)
                                dg_ELECInfo.Rows[i].Cells[6].Style.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        dg_ELECInfo.Rows.RemoveAt(elec_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_ELECInfo.Rows.Clear();
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
