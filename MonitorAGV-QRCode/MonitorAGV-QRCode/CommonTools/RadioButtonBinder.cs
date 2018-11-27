using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class RadioButtonBinder : BindingWithNotify
    {
        public RadioButtonBinder(string propertyName, object dataSource, string dataMember)
            : base(propertyName, dataSource, dataMember)
        {
        }

        protected override void OnFormat(ConvertEventArgs cevent)
        {
            MyRadioButton myRadioButton = base.Control as MyRadioButton;
            myRadioButton.Checked = myRadioButton.CheckedValue.Equals(cevent.Value);
        }

        protected override void OnParse(ConvertEventArgs cevent)
        {
            MyRadioButton myRadioButton = base.Control as MyRadioButton;
            bool @checked = myRadioButton.Checked;
            if (@checked)
            {
                cevent.Value = myRadioButton.CheckedValue;
            }
        }
    }
}
