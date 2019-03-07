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
    public partial class frm_Monitor_Car_Manual : Form
    {
        frm_Main us_frmmain;
        string us_CarNo;
        int us_CarIndex;
        byte[] SendMess;
        int RGV_AC;
        public frm_Monitor_Car_Manual(frm_Main frmmain, string CarNo, int CarIndex,int CarAC)
        {
            InitializeComponent();
            us_frmmain = frmmain;
            us_CarNo = CarNo;
            us_CarIndex = CarIndex;
            lb_CarNo.Text = CarNo;
            SendMess = new byte[13];
            RGV_AC = CarAC;
            ShowLb();
        }

        private void ShowLb()
        {
            if (RGV_AC == 1)
            {
                lb_CarAC.BackColor = Color.Lime;
                lb_CarAC.Text="自动中";
                bt_Down.Enabled = false;
                bt_Up.Enabled = false;
                bt_M_Auto.Text = "手动";
            }
            else if (RGV_AC == 2)
            {
                lb_CarAC.BackColor = Color.Yellow;
                lb_CarAC.Text = "手动中";
                bt_Down.Enabled = true;
                bt_Up.Enabled = true;
                bt_M_Auto.Text = "自动";
            }

        }

        private void bt_Up_Click(object sender, EventArgs e)
        {
            SendMess[0] = 0x08;
            SendMess[1] = 0x00;
            SendMess[2] = 0x00;
            SendMess[3] = 0x00;
            SendMess[4] = 0x00;
            SendMess[5] = (byte)int.Parse(us_CarNo);
            SendMess[6] = 0x01;
            SendMess[7] = 0x02;
            SendMess[8] = 0x00;
            SendMess[9] = 0x00;
            SendMess[10] = 0x00;
            SendMess[11] = 0x00;
            SendMess[12] = 0x00;
            //us_frmmain.SendData(SendMess);
        }

        private void bt_Down_Click(object sender, EventArgs e)
        {
             SendMess[0] = 0x08;
            SendMess[1] = 0x00;
            SendMess[2] = 0x00;
            SendMess[3] = 0x00;
            SendMess[4] = 0x00;
            SendMess[5] = (byte)int.Parse(us_CarNo);
            SendMess[6] = 0x01;
            SendMess[7] = 0x01;
            SendMess[8] = 0x00;
            SendMess[9] = 0x00;
            SendMess[10] = 0x00;
            SendMess[11] = 0x00;
            SendMess[12] = 0x00;
            //us_frmmain.SendData(SendMess);
        }

        private void bt_Stop_Click(object sender, EventArgs e)
        {
            SendMess[0] = 0x08;
            SendMess[1] = 0x00;
            SendMess[2] = 0x00;
            SendMess[3] = 0x00;
            SendMess[4] = 0x00;
            SendMess[5] = (byte)int.Parse(us_CarNo);
            SendMess[6] = 0x01;
            SendMess[7] = 0x03;
            SendMess[8] = 0x00;
            SendMess[9] = 0x00;
            SendMess[10] = 0x00;
            SendMess[11] = 0x00;
            SendMess[12] = 0x00;
            //us_frmmain.SendData(SendMess);
        }

        private void bt_M_Auto_Click(object sender, EventArgs e)
        {
            if (RGV_AC == 1)//变手动
            {
                SendMess[0] = 0x08;
                SendMess[1] = 0x00;
                SendMess[2] = 0x00;
                SendMess[3] = 0x00;
                SendMess[4] = 0x00;
                SendMess[5] = (byte)int.Parse(us_CarNo);
                SendMess[6] = 0x01;
                SendMess[7] = 0x00;
                SendMess[8] = 0x00;
                SendMess[9] = 0x00;
                SendMess[10] = 0x00;
                SendMess[11] = 0x00;
                SendMess[12] = 0x00;
                //us_frmmain.SendData(SendMess);
                RGV_AC = 2;
            }
            else if (RGV_AC == 2)//变自动
            {
                SendMess[0] = 0x08;
                SendMess[1] = 0x00;
                SendMess[2] = 0x00;
                SendMess[3] = 0x00;
                SendMess[4] = 0x00;
                SendMess[5] = (byte)int.Parse(us_CarNo);
                SendMess[6] = 0x02;
                SendMess[7] = 0x00;
                SendMess[8] = 0x00;
                SendMess[9] = 0x00;
                SendMess[10] = 0x00;
                SendMess[11] = 0x00;
                SendMess[12] = 0x00;
                //us_frmmain.SendData(SendMess);
                RGV_AC = 1;
            }
            ShowLb();
        }
    }
}
