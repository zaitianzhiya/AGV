using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class IntEditor : BaseEditor<int>
    {
        public bool AllowNegativeNumber
        {
            set
            {
                if (value)
                {
                    this.m_filterString = "-0123456789";
                }
                else
                {
                    this.m_filterString = "0123456789";
                }
            }
        }

        public IntEditor()
        {
            this.AllowNegativeNumber = true;
            base.TextAlign = HorizontalAlignment.Right;
        }
    }
}
