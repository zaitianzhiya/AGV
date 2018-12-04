using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGVMAP.Dialog
{
    public partial class FrmCoorCompare : BaseForm
    {
        public FrmCoorCompare()
        {
            InitializeComponent();
        }

        private void FrmCoorCompare_Load(object sender, EventArgs e)
        {
            DataTable dtCoor = Function.GetDataInfo("PR_SELECT_COOR_INFO");
            if (dtCoor.Rows.Count > 0)
            {
                cmbEast.Text = dtCoor.Select("DIRECTION='1'")[0]["ANGLE"].ToString();
                cmbSouth.Text = dtCoor.Select("DIRECTION='2'")[0]["ANGLE"].ToString();
                cmbWest.Text = dtCoor.Select("DIRECTION='3'")[0]["ANGLE"].ToString();
                cmbNorth.Text = dtCoor.Select("DIRECTION='0'")[0]["ANGLE"].ToString();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cmbEast.Text == cmbSouth.Text || cmbEast.Text == cmbWest.Text ||
               cmbEast.Text == cmbNorth.Text || cmbSouth.Text == cmbWest.Text ||
               cmbSouth.Text == cmbNorth.Text || cmbWest.Text == cmbNorth.Text)
            {
                MessageBoxShow.Alert("角度有重复", MessageBoxIcon.Exclamation);
                return;
            }
            Function.Update_CoorInfo(cmbEast.Text, cmbSouth.Text, cmbWest.Text, cmbNorth.Text);
            DialogResult = DialogResult.OK;
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult=DialogResult.Cancel;
        }
    }
}
