using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;

namespace MonitorAGV_QRCode.Windows
{
    public partial class FrmSetTask : BaseForm
    {
        private DataTable dtCars;
        public FrmSetTask()
        {
            InitializeComponent();
        }

        private void FrmSetTask_Load(object sender, EventArgs e)
        {
            dtCars = Function.ReadDB_tb_AGV_Info();
            foreach (DataRow dr in dtCars.Rows)
            {
                cmbCar.Properties.Items.Add(dr["AGV_Ip"].ToString());
            }
            if (dtCars.Rows.Count > 0)
            {
                cmbCar.SelectedIndex = 0;
            }
        }

        private void btnAdd_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.Add, new RectangleF(new PointF((float)((btnAdd.Width - btnAdd.Height) * 0.5), 2), new SizeF((float)(btnAdd.Height-4), (float)(btnAdd.Height-4))));
        }

        private void btnMul_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.Remove, new RectangleF(new PointF((float)((btnMul.Width - btnMul.Height) * 0.5), 2), new SizeF((float)(btnMul.Height - 4), (float)(btnMul.Height - 4))));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnMul_Click(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {

        }
    }
}
