using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class BindingWithNotify : Binding
    {
        public BindingWithNotify(string propertyName, object dataSource, string dataMember)
            : base(propertyName, dataSource, dataMember, true)
        {
        }

        protected override void OnBindingComplete(BindingCompleteEventArgs e)
        {
            base.OnBindingComplete(e);
            base.Control.Validating += new CancelEventHandler(this.Control_Validating);
        }

        private void Control_Validating(object sender, CancelEventArgs e)
        {
            this.WriteNotifyIfChanged();
        }

        public void WriteNotify()
        {
            base.WriteValue();
            this.NotifyChanged();
        }

        public void WriteNotifyIfChanged()
        {
            object obj = base.DataSource;
            bool flag = obj is ICurrencyManagerProvider;
            if (flag)
            {
                obj = ((ICurrencyManagerProvider)obj).CurrencyManager.Current;
            }
            PropertyInfo nestedProperty = PropertyUtil.GetNestedProperty(ref obj, base.BindingMemberInfo.BindingMember);
            bool flag2 = nestedProperty != null;
            if (flag2)
            {
                object value = nestedProperty.GetValue(obj, null);
                base.WriteValue();
                object value2 = nestedProperty.GetValue(obj, null);
                bool flag3 = value != null && value2 != null && !value.Equals(value2);
                if (flag3)
                {
                    this.NotifyChanged();
                }
                base.ReadValue();
            }
        }

        protected virtual void NotifyChanged()
        {
            MyBindingSource myBindingSource = base.DataSource as MyBindingSource;
            bool flag = myBindingSource != null;
            if (flag)
            {
                myBindingSource.RaiseValueChanged(this);
            }
        }

        protected override void OnParse(ConvertEventArgs cevent)
        {
        }
    }
}
