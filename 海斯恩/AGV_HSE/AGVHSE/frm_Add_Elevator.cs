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
    public partial class frm_Add_Elevator : Form
    {

        public frm_Add_Elevator(string RFIDno, int RFIDLX, int RFIDLY)
        {
            InitializeComponent();
            txt_Elec_IP.Text = RFIDno;
            lb_RFID_Location_X.Text = RFIDLX.ToString();
            lb_RFID_Location_Y.Text = RFIDLY.ToString();
            if (RFIDno != "")
            {
                ShowE(RFIDno);
            }
        }

        private void ShowE(string E_IP)
        {
            DataTable E_Table=Function.SELECT_ELC_IP(E_IP);
            if (E_Table != null && E_Table.Rows.Count > 0)
            {
                txt_Elec_IP.Enabled = false;
                txt_ElecNo.Text = E_Table.Rows[0][1].ToString().Trim();
                txt_Elec_IP.Text = E_Table.Rows[0][2].ToString().Trim();
                txt_RFID.Text = E_Table.Rows[0][6].ToString().Trim();
                lb_RFID_Location_X.Text = E_Table.Rows[0][3].ToString().Trim();
                lb_RFID_Location_Y.Text = E_Table.Rows[0][4].ToString().Trim();
                cb_Out_Menu.SelectedIndex = int.Parse(E_Table.Rows[0][5].ToString().Trim()) - 1;
            }

            DataTable e_Table = Function.SELECT_Elec_IP(E_IP);
            if (e_Table != null && e_Table.Rows.Count > 0)
            {
                dgv_elec.Rows.Add(e_Table.Rows.Count);
                for (int i = 0; i < e_Table.Rows.Count; i++)
                {
                    dgv_elec.Rows[i].Cells[0].Value = e_Table.Rows[i][2].ToString().Trim();
                    dgv_elec.Rows[i].Cells[1].Value = e_Table.Rows[i][3].ToString().Trim();
                    dgv_elec.Rows[i].Cells[2].Value = e_Table.Rows[i][4].ToString().Trim();
                    dgv_elec.Rows[i].Cells[3].Value = e_Table.Rows[i][5].ToString().Trim();
                    dgv_elec.Rows[i].Cells[4].Value = e_Table.Rows[i][6].ToString().Trim() == "1" ? "前启" : "后启";
                    dgv_elec.Rows[i].Cells[5].Value = e_Table.Rows[i][7].ToString().Trim();
                    dgv_elec.Rows[i].Cells[6].Value = e_Table.Rows[i][8].ToString().Trim();
                }
            }
        }

        private void bt_Ent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_ElecNo.Text) && !string.IsNullOrEmpty(txt_Elec_IP.Text) && !string.IsNullOrEmpty(lb_RFID_Location_X.Text) && !string.IsNullOrEmpty(lb_RFID_Location_Y.Text))
            {
                Function.INSERT_ELC(txt_ElecNo.Text.Trim(), txt_Elec_IP.Text.Trim(), int.Parse(lb_RFID_Location_X.Text), int.Parse(lb_RFID_Location_Y.Text), txt_RFID.Text.Trim(), cb_Out_Menu.SelectedIndex + 1);
                if (dgv_elec != null && dgv_elec.Rows.Count > 1)
                {
                    try
                    {
                        for (int i = 0; i < dgv_elec.Rows.Count - 1; i++)
                        {
                            Function.INSERT_Elec(txt_Elec_IP.Text.Trim(), dgv_elec.Rows[i].Cells[0].Value.ToString(), dgv_elec.Rows[i].Cells[1].Value.ToString(), dgv_elec.Rows[i].Cells[2].Value.ToString(), int.Parse(dgv_elec.Rows[i].Cells[3].Value.ToString()), dgv_elec.Rows[i].Cells[4].Value.ToString().Trim() == "后启" ? 2 : 1, int.Parse(dgv_elec.Rows[i].Cells[5].Value.ToString()), dgv_elec.Rows[i].Cells[6].Value.ToString());
                        }
                        this.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("参数", "错误");
                    }
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void bt_Frmart_Click(object sender, EventArgs e)
        {
            if (!txt_Elec_IP.Enabled)
            {
 
            }
        }
    }
}
