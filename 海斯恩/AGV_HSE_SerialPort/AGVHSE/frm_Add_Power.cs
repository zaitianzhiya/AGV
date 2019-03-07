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
    public partial class frm_Add_Power : Form
    {

        public frm_Add_Power(string RFIDno, int RFIDLX, int RFIDLY)
        {
            InitializeComponent();
            txt_RFIDNo.Text = RFIDno;
            lb_RFID_Location_X.Text = RFIDLX.ToString();
            lb_RFID_Location_Y.Text = RFIDLY.ToString();
            if (RFIDno != "")
            {
                ShowP(RFIDno);
            }
        }

        private void ShowP(string E_IP)
        {
            DataTable E_Table = Function.SELECT_Power_INFO_IP(E_IP);
            if (E_Table != null && E_Table.Rows.Count > 0)
            {
                txt_IP.Enabled = false;
                txt_PowerNo.Text = E_Table.Rows[0][1].ToString().Trim();
                txt_IP.Text = E_Table.Rows[0][2].ToString().Trim();
                txt_LineNo.Text = E_Table.Rows[0][3].ToString().Trim();
                txt_RFIDNo.Text = E_Table.Rows[0][4].ToString().Trim();
                lb_RFID_Location_X.Text = E_Table.Rows[0][5].ToString().Trim();
                lb_RFID_Location_Y.Text = E_Table.Rows[0][6].ToString().Trim();
                txt_InRFID.Text = E_Table.Rows[0][7].ToString().Trim();
                cm_IN_F.SelectedIndex = int.Parse(E_Table.Rows[0][8].ToString().Trim()) - 1;
                txt_Out_RFID.Text = E_Table.Rows[0][9].ToString().Trim();
                cm_OUT_F.SelectedIndex = int.Parse(E_Table.Rows[0][10].ToString().Trim()) - 1;
                txt_InLineNo.Text = E_Table.Rows[0][11].ToString().Trim();
                txt_PowerHigh.Text = E_Table.Rows[0][12].ToString().Trim();
                txt_PowerLow.Text = E_Table.Rows[0][13].ToString().Trim();
                txt_OutLineNo.Text = E_Table.Rows[0][14].ToString().Trim();
               
            }         
        }

        private void bt_AddRFID_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_PowerNo.Text) && !string.IsNullOrEmpty(txt_RFIDNo.Text) && !string.IsNullOrEmpty(lb_RFID_Location_X.Text) && !string.IsNullOrEmpty(lb_RFID_Location_Y.Text))
            {
                try
                {
                    Function.INSERT_Power_INFO(txt_PowerNo.Text.Trim(),
                                               txt_IP.Text.Trim(),
                                               txt_LineNo.Text.Trim(),
                                               txt_RFIDNo.Text.Trim(),
                                               int.Parse(lb_RFID_Location_X.Text.Trim()),
                                               int.Parse(lb_RFID_Location_Y.Text.Trim()),
                                               txt_InRFID.Text.Trim(),
                                               cm_IN_F.SelectedIndex + 1,
                                               txt_InLineNo.Text.Trim(),
                                               int.Parse(txt_PowerHigh.Text.Trim()),
                                               int.Parse(txt_PowerLow.Text.Trim()),
                                               txt_OutLineNo.Text.Trim(),
                                               txt_Out_RFID.Text.Trim(),
                                               cm_OUT_F.SelectedIndex + 1
                                               );
                    this.Close();

                }
                catch (Exception)
                {
                    MessageBox.Show("参数设定错误", "错误");
                }
            }
        }


       
    }
}
