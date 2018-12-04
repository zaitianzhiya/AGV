using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    internal class WinUtil
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;

            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public WinUtil.POINT pt;

            public int hwnd;

            public int wHitTestCode;

            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;

            public int scanCode;

            public int flags;

            public int time;

            public int dwExtraInfo;
        }

        public const int WM_KEYDOWN = 256;

        public const int WM_KEYUP = 257;

        public const int WM_CHAR = 258;

        public const int SWP_NOSIZE = 1;

        public const int SWP_NOMOVE = 2;

        public const int SWP_NOZORDER = 4;

        public const int SWP_NOREDRAW = 8;

        public const int SWP_NOACTIVATE = 16;

        public const int SWP_FRAMECHANGED = 32;

        public const int SWP_SHOWWINDOW = 64;

        public const int SWP_HIDEWINDOW = 128;

        public const int SWP_NOCOPYBITS = 256;

        public const int SWP_NOOWNERZORDER = 512;

        public const int SWP_NOSENDCHANGING = 1024;

        public const uint WS_OVERLAPPED = 12582912u;

        public const uint WS_CLIPSIBLINGS = 67108864u;

        public const uint WS_CLIPCHILDREN = 33554432u;

        public const uint WS_CAPTION = 12582912u;

        public const uint WS_BORDER = 8388608u;

        public const uint WS_DLGFRAME = 4194304u;

        public const uint WS_VSCROLL = 2097152u;

        public const uint WS_HSCROLL = 1048576u;

        public const uint WS_SYSMENU = 524288u;

        public const uint WS_THICKFRAME = 262144u;

        public const uint WS_MAXIMIZEBOX = 131072u;

        public const uint WS_MINIMIZEBOX = 65536u;

        public const uint WS_SIZEBOX = 262144u;

        public const uint WS_POPUP = 2147483648u;

        public const uint WS_CHILD = 1073741824u;

        public const uint WS_VISIBLE = 268435456u;

        public const uint WS_DISABLED = 134217728u;

        public const uint WS_EX_DLGMODALFRAME = 1u;

        public const uint WS_EX_TOPMOST = 8u;

        public const uint WS_EX_TOOLWINDOW = 128u;

        public const uint WS_EX_WINDOWEDGE = 256u;

        public const uint WS_EX_CLIENTEDGE = 512u;

        public const uint WS_EX_CONTEXTHELP = 1024u;

        public const uint WS_EX_STATICEDGE = 131072u;

        public const uint WS_EX_OVERLAPPEDWINDOW = 768u;

        public const int GWL_STYLE = -16;

        public const int GWL_EXSTYLE = -20;

        public const int WH_KEYBOARD = 2;

        public const int WH_MOUSE = 7;

        public const int WH_KEYBOARD_LL = 13;

        public const int WH_MOUSE_LL = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int SetWindowsHookEx(int idHook, WinUtil.HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
