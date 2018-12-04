using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CommonTools
{
    public class MyBindingSource : BindingSource
    {
        [method: CompilerGenerated]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        public event EventHandler ValueChanged;

        public void RaiseValueChanged(object sender)
        {
            bool flag = this.ValueChanged != null;
            if (flag)
            {
                this.ValueChanged(sender, null);
            }
        }
    }
}
