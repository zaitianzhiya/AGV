using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class NameObjectComboBox<T> : ComboBox
    {
        private NameObjectCollection<T> m_items = new NameObjectCollection<T>();

        public new NameObjectCollection<T> Items
        {
            get
            {
                return this.m_items;
            }
            set
            {
                this.m_items = value;
                base.DataSource = this.m_items;
            }
        }

        public new NameObject<T> SelectedItem
        {
            get
            {
                return base.SelectedItem as NameObject<T>;
            }
            set
            {
                base.SelectedItem = value;
            }
        }

        public NameObjectComboBox()
        {
            base.DisplayMember = "Name";
            base.ValueMember = "Object";
        }

        protected override void OnLeave(EventArgs e)
        {
            bool flag = base.DataBindings.Count > 0;
            if (flag)
            {
                base.DataBindings[0].WriteValue();
            }
            base.OnLeave(e);
        }
    }
}
