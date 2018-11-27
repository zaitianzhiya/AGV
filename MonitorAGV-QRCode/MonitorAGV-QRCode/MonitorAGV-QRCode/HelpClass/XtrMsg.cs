using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MonitorAGV_QRCode
{
    public class XtrMsg
    {
        public static DialogResult Show(string msg, MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.Exclamation:
                    return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.None:
                    return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.Question:
                    return XtraMessageBox.Show(msg, "询问", MessageBoxButtons.YesNo, icon, MessageBoxDefaultButton.Button1);
                case MessageBoxIcon.Error:
                    return XtraMessageBox.Show(msg, "异常", MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
                default:
                    return XtraMessageBox.Show(msg);
            }
        }

        public static DialogResult ShowAsterisk(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowError(string msg)
        {
            return XtraMessageBox.Show(msg, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowExclamation(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowHand(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowInformation(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowNone(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowQuestion(string msg)
        {
            return XtraMessageBox.Show(msg, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowStop(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult ShowWarning(string msg)
        {
            return XtraMessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }
    }
}
