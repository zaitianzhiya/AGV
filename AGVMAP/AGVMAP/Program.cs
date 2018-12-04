using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;

namespace AGVMAP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            string str1 = "DevExpress Dark Style";
            if (File.Exists(Application.StartupPath + @"\AGV_Set.ini"))
            {
                str1 = FileControl.SetFileControl.ReadIniValue("STYLE", "Style",
                Application.StartupPath + @"\AGV_Set.ini");
            }
            UserLookAndFeel.Default.SetSkinStyle(str1);
            Application.Run(new FrmMain2());
        }
    }
}
