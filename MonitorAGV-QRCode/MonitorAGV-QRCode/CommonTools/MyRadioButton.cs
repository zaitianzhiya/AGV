using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class MyRadioButton : RadioButton
    {
        private object m_checkedValue = null;

        public object CheckedValue
        {
            get
            {
                return this.m_checkedValue;
            }
            set
            {
            }
        }

        public void AddDatabinding(MyBindingSource datasource, string datamember, object controlValue)
        {
            this.m_checkedValue = controlValue;
            base.DataBindings.Add(new RadioButtonBinder("CheckedValue", datasource, datamember));
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            bool flag = base.Checked && base.DataBindings != null && base.DataBindings.Count > 0;
            if (flag)
            {
                BindingWithNotify bindingWithNotify = base.DataBindings[0] as BindingWithNotify;
                bool flag2 = bindingWithNotify != null;
                if (flag2)
                {
                    bindingWithNotify.WriteNotify();
                }
            }
        }
    }
}
