using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AGVMAP
{
    public class MessageBoxShow
    {
        public static DialogResult Alert(string msg,MessageBoxIcon messageBoxIcon)
        {
            switch (messageBoxIcon)
            {
                case  MessageBoxIcon.Exclamation:
                    return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, messageBoxIcon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.Asterisk:
                    return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, messageBoxIcon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.Question:
                    return XtraMessageBox.Show(msg, "询问", MessageBoxButtons.YesNo, messageBoxIcon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.Error:
                    return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, messageBoxIcon, MessageBoxDefaultButton.Button1);
               default:
                    return XtraMessageBox.Show(msg);
            }
        }
    }
}
