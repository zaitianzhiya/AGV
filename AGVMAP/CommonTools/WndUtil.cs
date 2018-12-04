using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class WndUtil
    {
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
    }
}
