using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public abstract class BaseEditor<T> : TextBox
    {
        protected string m_filterString = string.Empty;

        protected T m_lastValue;

        public T Value
        {
            get
            {
                return this.GetValue();
            }
            set
            {
                this.SetValue(value);
            }
        }

        protected virtual bool IsValidChar(char ch)
        {
            return this.m_filterString.Length == 0 || this.m_filterString.IndexOf(ch) >= 0;
        }

        protected override void OnLeave(EventArgs e)
        {
            this.SetValue(this.GetValue());
            base.OnLeave(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            bool flag = !this.IsValidChar(e.KeyChar);
            if (flag)
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyPress(e);
            }
        }

        protected virtual T GetValue()
        {
            string validatedText = this.GetValidatedText();
            T result;
            try
            {
                result = (T)((object)Convert.ChangeType(validatedText, typeof(T)));
                return result;
            }
            catch
            {
            }
            result = this.m_lastValue;
            return result;
        }

        protected virtual void SetValue(T value)
        {
            this.Text = value.ToString();
            this.m_lastValue = value;
        }

        protected virtual string GetValidatedText()
        {
            return this.Text;
        }
    }
}
