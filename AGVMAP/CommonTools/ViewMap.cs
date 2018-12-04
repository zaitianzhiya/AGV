using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ViewMap : Panel
    {
        private object m_curKey = null;

        private Dictionary<object, Control> m_views = new Dictionary<object, Control>();

        public object CurKey
        {
            get
            {
                return this.m_curKey;
            }
            set
            {
                this.SelectView(value);
            }
        }

        public void AddView(object key, Control view)
        {
            view.Dock = DockStyle.Fill;
            view.Visible = false;
            Form form = view as Form;
            bool flag = form != null;
            if (flag)
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
            }
            this.m_views[key] = view;
            bool flag2 = !base.Controls.Contains(view);
            if (flag2)
            {
                base.Controls.Add(view);
            }
        }

        public Control GetView(object key)
        {
            bool flag = key == null;
            Control result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = this.m_views.ContainsKey(key);
                if (flag2)
                {
                    result = this.m_views[key];
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public void SelectView(object key)
        {
            Control view = this.GetView(key);
            foreach (Control current in this.m_views.Values)
            {
                bool flag = view != current;
                if (flag)
                {
                    current.Hide();
                }
            }
            bool flag2 = view != null;
            if (flag2)
            {
                view.Show();
            }
            this.m_curKey = key;
        }
    }
}
