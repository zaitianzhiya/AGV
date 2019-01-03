using Fuction;
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
    public partial class frm_AGV_Order : Form
    {
        frm_Main us_frmmain;
        public frm_AGV_Order(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            GetInfo();
            //取消选中行
            dg_OrderInfo.Rows[0].Selected = false;
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
            DataTable order_Info = Fuction.Function.KIVA_SELECT_AGV_Order_show();
            if (order_Info != null && order_Info.Rows.Count > 0)
            {
                if (dg_OrderInfo.Rows.Count < order_Info.Rows.Count)
                {
                    dg_OrderInfo.Rows.Add(order_Info.Rows.Count - dg_OrderInfo.Rows.Count);
                }
                int GetCount = dg_OrderInfo.Rows.Count;
                for (int i = 0; i < GetCount; i++)
                {
                    if (i < order_Info.Rows.Count)
                    {
                        dg_OrderInfo.Rows[i].Cells[0].Value = order_Info.Rows[i][0].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[1].Value = order_Info.Rows[i][1].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[2].Value = order_Info.Rows[i][2].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[3].Value = order_Info.Rows[i][3].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[4].Value = order_Info.Rows[i][4].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[5].Value = order_Info.Rows[i][5].ToString().Trim();
                        dg_OrderInfo.Rows[i].Cells[6].Value = order_Info.Rows[i][6].ToString().Trim();
                        //dg_OrderInfo.Rows[i].Cells[7].Value = order_Info.Rows[i][7].ToString().Trim() == "1" ? "锁定" : "未锁";
                        //dg_OrderInfo.Rows[i].Cells[8].Value = order_Info.Rows[i][8].ToString().Trim();
                    }
                    else
                    {
                        dg_OrderInfo.Rows.RemoveAt(order_Info.Rows.Count - 1);
                    }
                }

            }
            else
            {
                dg_OrderInfo.Rows.Clear();
            }

        }    

    }
}
