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
    public partial class frm_Add_AutoDoor : Form
    {

        public frm_Add_AutoDoor(string RFIDno, int RFIDLX, int RFIDLY)
        {
            InitializeComponent();
            txt_OutRFID.Text = RFIDno;
            lb_RFID_Location_X.Text = RFIDLX.ToString();
            lb_RFID_Location_Y.Text = RFIDLY.ToString();
        }

        private void bt_AddRFID_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_AutoNo.Text) && !string.IsNullOrEmpty(txt_OutRFID.Text) && !string.IsNullOrEmpty(lb_RFID_Location_X.Text) && !string.IsNullOrEmpty(lb_RFID_Location_Y.Text))
            {
                Function.INSERT_AutoDoor_INFO(txt_AutoNo.Text.Trim(),
                                              txt_IP.Text.Trim(),
                                              txt_LineNo.Text.Trim(),
                                              int.Parse(lb_RFID_Location_X.Text),
                                              int.Parse(lb_RFID_Location_Y.Text),
                                              txt_InRFID.Text.Trim(),
                                              cm_F.SelectedIndex + 1,
                                              txt_OutRFID.Text.Trim());

            }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {

        }
       
    }
}
