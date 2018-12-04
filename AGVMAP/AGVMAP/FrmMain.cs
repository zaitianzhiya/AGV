using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AGVMAP.Dialog;
using AGVMAP.HelpClass;
using DevExpress.LookAndFeel;

namespace AGVMAP
{
    public partial class FrmMain : Form
    {
        static string Path = System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini";
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Global.path = System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini";
            UserLookAndFeel.Default.SetSkinStyle(FileControl.SetFileControl.ReadIniValue("STYLE", "Style", Path));
            string dataBase = FileControl.SetFileControl.ReadIniValue("DBSETUP", "DATABASE", Path);
            string server = FileControl.SetFileControl.ReadIniValue("DBSETUP", "SERVER", Path);
            string maxPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MaxPoolSize", Path);
            string minPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MinPoolSize", Path);
            string uid = FileControl.SetFileControl.ReadIniValue("DBSETUP", "UID", Path);
            string pwd = FileControl.SetFileControl.ReadIniValue("DBSETUP", "PWD", Path);
            SqlDBControl._defultConnectionString = string.Format(
                "database={0};server={1};Max Pool Size={2};Min Pool Size={3};uid={4};pwd={5}", dataBase, server,
                maxPool, minPool, uid, pwd);
        }

        private void btnOpenMap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnNewMap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnCoorCompare_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmCoorCompare frm=new FrmCoorCompare())
            {
                frm.ShowDialog();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBoxShow.Alert("确定退出当前系统?", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.ExitThread();
                Application.Exit();
            }
        }

        private void btnExchangeInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmDbSetup frm = new FrmDbSetup())
            {
                frm.ShowDialog();
            }
        }

        private void btnAgvSetUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAgvInfo frm = new FrmAgvInfo())
            {
                frm.ShowDialog();
            }
        }

        private void btnSysInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmSysPara frm = new FrmSysPara())
            {
                frm.ShowDialog();
            }
        }

        private void btnCallBox_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnTask_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAreaInfo frm = new FrmAreaInfo())
            {
                frm.ShowDialog();
            }
        }

        private void btnAction_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAction frm = new FrmAction())
            {
                frm.ShowDialog();
            }
        }

        private void btnMaterial_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmMaterial frm = new FrmMaterial())
            {
                frm.ShowDialog();
            }
        }

        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnOption_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBoxShow.Alert("确定退出当前系统?", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.ExitThread();
                Application.Exit();
            }
        }

        /// 取得INI档数据
        /// <summary>
        /// 取得INI档数据
        /// </summary>
        void GetIniData()
        {
            //string path = System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini";
            //string r = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorR", path);
            //string g = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorG", path);
            //string b = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorB", path);
            //canvasBrush = new SolidColorBrush(Color.FromRgb((byte)int.Parse(r), (byte)int.Parse(g), (byte)int.Parse(b)));

            //r = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateR", path);
            //g = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateG", path);
            //b = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateB", path);
            //coorBrush = new SolidColorBrush(Color.FromRgb((byte)int.Parse(r), (byte)int.Parse(g), (byte)int.Parse(b)));

            //r = FileControl.SetFileControl.ReadIniValue("OPTION", "PenR", path);
            //g = FileControl.SetFileControl.ReadIniValue("OPTION", "PenG", path);
            //b = FileControl.SetFileControl.ReadIniValue("OPTION", "PenB", path);
            //penBrush = new SolidColorBrush(Color.FromRgb((byte)int.Parse(r), (byte)int.Parse(g), (byte)int.Parse(b)));

            ////CanvasMain.Background = canvasBrush;

            //string chk = FileControl.SetFileControl.ReadIniValue("OPTION", "UseCoordinate", path);
            //IsShowCoordinate = bool.Parse(chk);

            //displayMode = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateType", path);

            //GridMain.Background = canvasBrush;

            //penSize = double.Parse(FileControl.SetFileControl.ReadIniValue("OPTION", "PenSize", path));
        }
    }
}
