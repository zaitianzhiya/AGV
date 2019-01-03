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
    public partial class frm_Charge_ByHand : Form
    {
        frm_Main us_frmmain;
        public frm_Charge_ByHand(frm_Main frmmain)
        {
            InitializeComponent();
            us_frmmain = frmmain;
        }

        /// <summary>
        /// 打开充电回路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string ip = "192.168.1." + (int.Parse(textBox_no.Text.Trim()) + 200).ToString();
            us_frmmain.KIVA_SendTo_AGV_OpenHuiLu(ip);
        }
        /// <summary>
        /// 关闭充电回路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string ip = "192.168.1." + (int.Parse(textBox_no.Text.Trim()) + 200).ToString();
            us_frmmain.KIVA_SendTo_AGV_CloseHuiLu(ip);
        }
        /// <summary>
        /// 伸出开始充电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string ip = "192.168.1." + (int.Parse(textBox_c.Text.Trim()) + 160).ToString();
            us_frmmain.KIVA_SendTo_Charge_AskorControl(ip, 2);
        }
        /// <summary>
        /// 缩回停止充电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string ip = "192.168.1." + (int.Parse(textBox_c.Text.Trim()) + 160).ToString();
            us_frmmain.KIVA_SendTo_Charge_AskorControl(ip, 1);
        }
    }
}
