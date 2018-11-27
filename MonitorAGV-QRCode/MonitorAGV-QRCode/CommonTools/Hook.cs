using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class Hook
    {
        public delegate void KeyboardDelegate(KeyEventArgs e);

        public Hook.KeyboardDelegate OnKeyDown;

        private int m_hHook = 0;

        private WinUtil.HookProc m_HookCallback;

        public void SetHook(bool enable)
        {
            bool flag = enable && this.m_hHook == 0;
            if (flag)
            {
                this.m_HookCallback = new WinUtil.HookProc(this.HookCallbackProc);
                Module m = Assembly.GetExecutingAssembly().GetModules()[0];
                this.m_hHook = WinUtil.SetWindowsHookEx(13, this.m_HookCallback, Marshal.GetHINSTANCE(m), 0);
                bool flag2 = this.m_hHook == 0;
                if (flag2)
                {
                    MessageBox.Show("SetHook Failed. Please make sure the 'Visual Studio Host Process' on the debug setting page is disabled");
                }
            }
            else
            {
                bool flag3 = !enable && this.m_hHook != 0;
                if (flag3)
                {
                    WinUtil.UnhookWindowsHookEx(this.m_hHook);
                    this.m_hHook = 0;
                }
            }
        }

        private int HookCallbackProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool flag = nCode < 0;
            int result;
            if (flag)
            {
                result = WinUtil.CallNextHookEx(this.m_hHook, nCode, wParam, lParam);
            }
            else
            {
                WinUtil.KeyboardHookStruct keyboardHookStruct = (WinUtil.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(WinUtil.KeyboardHookStruct));
                bool flag2 = this.OnKeyDown != null && wParam.ToInt32() == 256;
                if (flag2)
                {
                    Keys keys = (Keys)keyboardHookStruct.vkCode;
                    bool flag3 = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
                    if (flag3)
                    {
                        keys |= Keys.Shift;
                    }
                    bool flag4 = (Control.ModifierKeys & Keys.Control) == Keys.Control;
                    if (flag4)
                    {
                        keys |= Keys.Control;
                    }
                    KeyEventArgs keyEventArgs = new KeyEventArgs(keys);
                    keyEventArgs.Handled = false;
                    this.OnKeyDown(keyEventArgs);
                    bool handled = keyEventArgs.Handled;
                    if (handled)
                    {
                        result = 1;
                        return result;
                    }
                }
                int num = 0;
                bool flag5 = this.m_hHook != 0;
                if (flag5)
                {
                    num = WinUtil.CallNextHookEx(this.m_hHook, nCode, wParam, lParam);
                }
                result = num;
            }
            return result;
        }
    }
}
