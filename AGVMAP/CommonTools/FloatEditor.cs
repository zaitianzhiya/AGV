using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class FloatEditor : BaseEditor<float>
    {
        public bool AllowNegativeNumber
        {
            set
            {
                if (value)
                {
                    this.m_filterString = "-.0123456789";
                }
                else
                {
                    this.m_filterString = ".0123456789";
                }
            }
        }

        public FloatEditor()
        {
            this.AllowNegativeNumber = false;
            base.TextAlign = HorizontalAlignment.Right;
        }
    }
}
