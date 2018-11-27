using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class Win32Defines
    {
        public const int SC_SIZE = 61440;

        public const int SC_MOVE = 61456;

        public const int SC_MINIMIZE = 61472;

        public const int SC_MAXIMIZE = 61488;

        public const int SC_NEXTWINDOW = 61504;

        public const int SC_PREVWINDOW = 61520;

        public const int SC_CLOSE = 61536;

        public const int SC_VSCROLL = 61552;

        public const int SC_HSCROLL = 61568;

        public const int SC_MOUSEMENU = 61584;

        public const int SC_KEYMENU = 61696;

        public const int SC_ARRANGE = 61712;

        public const int SC_RESTORE = 61728;

        public const int SC_TASKLIST = 61744;

        public const int SC_SCREENSAVE = 61760;

        public const int SC_HOTKEY = 61776;

        public const int GWL_WNDPROC = -4;

        public const int GWL_HINSTANCE = -6;

        public const int GWL_HWNDPARENT = -8;

        public const int GWL_STYLE = -16;

        public const int GWL_EXSTYLE = -20;

        public const int GWL_USERDATA = -21;

        public const int GWL_ID = -12;

        public const int WS_OVERLAPPED = 0;

        public const uint WS_POPUP = 2147483648u;

        public const int WS_CHILD = 1073741824;

        public const int WS_VISIBLE = 268435456;

        public const int WS_DISABLED = 134217728;

        public const int WS_EX_DLGMODALFRAME = 1;

        public const int WS_EX_NOPARENTNOTIFY = 4;

        public const int WS_EX_TOPMOST = 8;

        public const int WS_EX_ACCEPTFILES = 16;

        public const int WS_EX_TRANSPARENT = 32;

        public const int WS_EX_MDICHILD = 64;

        public const int WS_EX_TOOLWINDOW = 128;

        public const int WS_EX_WINDOWEDGE = 256;

        public const int WS_EX_CLIENTEDGE = 512;

        public const int WS_EX_CONTEXTHELP = 1024;

        public const int WS_EX_RIGHT = 4096;

        public const int WS_EX_LEFT = 0;

        public const int WS_EX_RTLREADING = 8192;

        public const int WS_EX_LTRREADING = 0;

        public const int WS_EX_LEFTSCROLLBAR = 16384;

        public const int WS_EX_RIGHTSCROLLBAR = 0;

        public const int WS_EX_CONTROLPARENT = 65536;

        public const int WS_EX_STATICEDGE = 131072;

        public const int WS_EX_APPWINDOW = 262144;

        public const int HTNOWHERE = 0;

        public const int HTCLIENT = 1;

        public const int HTCAPTION = 2;

        public const int HTSYSMENU = 3;

        public const int HTGROWBOX = 4;

        public const int HTSIZE = 4;

        public const int HTMENU = 5;

        public const int HTHSCROLL = 6;

        public const int HTVSCROLL = 7;

        public const int HTMINBUTTON = 8;

        public const int HTMAXBUTTON = 9;

        public const int HTLEFT = 10;

        public const int HTRIGHT = 11;

        public const int HTTOP = 12;

        public const int HTTOPLEFT = 13;

        public const int HTTOPRIGHT = 14;

        public const int HTBOTTOM = 15;

        public const int HTBOTTOMLEFT = 16;

        public const int HTBOTTOMRIGHT = 17;

        public const int HTBORDER = 18;

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

        public const int HWND_TOP = 0;

        public const int HWND_BOTTOM = 1;

        public const int HWND_TOPMOST = -1;

        public const int HWND_NOTOPMOST = -2;

        public const int HH_DISPLAY_TOPIC = 0;

        public const int HH_DISPLAY_TOC = 1;

        public const int HH_DISPLAY_INDEX = 2;

        public const int HH_DISPLAY_SEARCH = 3;

        public const int HH_HELP_CONTEXT = 15;

        public const int HH_CLOSE_ALL = 18;
    }
}
