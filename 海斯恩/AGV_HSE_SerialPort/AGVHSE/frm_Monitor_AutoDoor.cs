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
    public partial class frm_Monitor_AutoDoor : Form
    {
        frm_Main us_frmmain;
        public frm_Monitor_AutoDoor(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
        }

        public void GetInfo()
        {
            DataTable AutoDoor_Info = Fuction.Function.SELECT_AuotDoor_INFO();
            if (AutoDoor_Info != null && AutoDoor_Info.Rows.Count > 0)
            {
                if (dg_AutoDoorInfo.Rows.Count < AutoDoor_Info.Rows.Count)
                {
                    dg_AutoDoorInfo.Rows.Add(AutoDoor_Info.Rows.Count - dg_AutoDoorInfo.Rows.Count);
                }
                int GetCount = dg_AutoDoorInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < AutoDoor_Info.Rows.Count)
                    {
                        dg_AutoDoorInfo.Rows[i].Cells[0].Value = AutoDoor_Info.Rows[i][1].ToString().Trim();
                        dg_AutoDoorInfo.Rows[i].Cells[1].Value = AutoDoor_Info.Rows[i][2].ToString().Trim();
                        dg_AutoDoorInfo.Rows[i].Cells[2].Value = AutoDoor_Info.Rows[i][9].ToString().Trim() == "1" ? "在线" : "离线";

                        
                        if (AutoDoor_Info.Rows[i][9].ToString().Trim() == "-1")
                        {
                            if (dg_AutoDoorInfo.Rows[i].Cells[2].Style.BackColor != Color.LightGray)
                                dg_AutoDoorInfo.Rows[i].Cells[2].Style.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (dg_AutoDoorInfo.Rows[i].Cells[2].Style.BackColor != Color.Lime)
                                dg_AutoDoorInfo.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                        }
                    }
                    else
                    {
                        dg_AutoDoorInfo.Rows.RemoveAt(AutoDoor_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_AutoDoorInfo.Rows.Clear();
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
