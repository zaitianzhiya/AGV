using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimulationModel;

namespace AGVMAP.Dialog
{
    public partial class FrmSimulationCallBox : BaseForm
    {
        public int CallBoxID = -1;

        private Simulator Simula = null;

        public FrmSimulationCallBox()
        {
            InitializeComponent();
        }

        public FrmSimulationCallBox(int BoxID, Simulator Simulator)
        {
            InitializeComponent();
            CallBoxID = BoxID;
            Simula = Simulator;
            lblBoxID.Text = CallBoxID+ "号按钮盒";
        }


        private void pictureBox5_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Simula != null)
            {
                int btnID = Convert.ToInt16(button1.Tag);
                string text = Simula.CreatTask(CallBoxID, btnID);
                txtMesWarn.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Simula != null)
            {
                int btnID = Convert.ToInt16(button2.Tag);
                string text = Simula.CreatTask(CallBoxID, btnID);
                txtMesWarn.Text = text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Simula != null)
            {
                int btnID = Convert.ToInt16(button3.Tag);
                string text = Simula.CreatTask(CallBoxID, btnID);
                txtMesWarn.Text = text;
            }
        }
    }
}
