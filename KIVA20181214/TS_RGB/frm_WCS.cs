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
    public partial class frm_WCS : Form
    {
        frm_Main us_frmmain;
        public frm_WCS(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
            //取消选中行
            dg_WcsInfo.Rows[0].Selected = false;
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
        public void GetInfo()
        {
            DataTable order_Info = Fuction.Function.KIVA_SELECT_WCS_To_KIVA_all();
            if (order_Info != null && order_Info.Rows.Count > 0)
            {
                if (dg_WcsInfo.Rows.Count < order_Info.Rows.Count)
                {
                    dg_WcsInfo.Rows.Add(order_Info.Rows.Count - dg_WcsInfo.Rows.Count);
                }
                int GetCount = dg_WcsInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < order_Info.Rows.Count)
                    {
                        dg_WcsInfo.Rows[i].Cells[0].Value = order_Info.Rows[i][0].ToString().Trim();
                        dg_WcsInfo.Rows[i].Cells[1].Value = order_Info.Rows[i][1].ToString().Trim();
                        dg_WcsInfo.Rows[i].Cells[2].Value = order_Info.Rows[i][2].ToString().Trim();
                        dg_WcsInfo.Rows[i].Cells[3].Value = order_Info.Rows[i][4].ToString().Trim();
                        dg_WcsInfo.Rows[i].Cells[4].Value = order_Info.Rows[i][5].ToString().Trim();
                        dg_WcsInfo.Rows[i].Cells[5].Value = order_Info.Rows[i][7].ToString().Trim();
                        dg_WcsInfo.Rows[i].Cells[6].Value = order_Info.Rows[i][9].ToString().Trim();
                        
                    }
                    else
                    {
                        dg_WcsInfo.Rows.RemoveAt(order_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_WcsInfo.Rows.Clear();
            }

        }    

    }
}
