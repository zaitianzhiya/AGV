using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class NameObjectBinder : BindingWithNotify
    {
        private int cnt = 0;

        public NameObjectBinder(string propertyName, object dataSource, string dataMember)
            : base(propertyName, dataSource, dataMember)
        {
        }

        protected override void OnFormat(ConvertEventArgs cevent)
        {
            this.cnt = 0;
            base.OnFormat(cevent);
        }

        protected override void OnParse(ConvertEventArgs cevent)
        {
            Console.WriteLine("OnParse ({0},{1})", cevent.DesiredType, cevent.Value);
            bool flag = cevent.Value == DBNull.Value;
            if (flag)
            {
            }
            cevent.Value = 1;
        }
    }
}
