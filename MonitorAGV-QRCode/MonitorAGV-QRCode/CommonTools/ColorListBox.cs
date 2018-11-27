using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ColorListBox : ListBox
    {
        private int m_knownColorCount = 0;

        public ColorListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            PropertyInfo[] properties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);
            PropertyInfo[] array = properties;
            int i = 0;
            while (i < array.Length)
            {
                PropertyInfo propertyInfo = array[i];
                bool flag = propertyInfo.PropertyType == typeof(Color);
                if (flag)
                {
                    Color color = (Color)propertyInfo.GetValue(typeof(Color), null);
                    bool flag2 = color.A == 0;
                    if (!flag2)
                    {
                        base.Items.Add(color);
                    }
                }
            IL_90:
                i++;
                continue;
                goto IL_90;
            }
            this.m_knownColorCount = base.Items.Count;
        }

        private int ColorIndex(Color color)
        {
            int num = color.ToArgb();
            int result;
            int num2;
            for (int i = 0; i < base.Items.Count; i = num2 + 1)
            {
                bool flag = ((Color)base.Items[i]).ToArgb() == num;
                if (flag)
                {
                    result = i;
                    return result;
                }
                num2 = i;
            }
            result = -1;
            return result;
        }

        public void SelectColor(Color color)
        {
            int num = this.ColorIndex(color);
            bool flag = num < 0;
            if (flag)
            {
                num = this.SetCustomColor(color);
            }
            base.SelectedItem = base.Items[num];
        }

        private void RemoveCustomColor()
        {
            bool flag = base.Items.Count > this.m_knownColorCount;
            if (flag)
            {
                base.Items.RemoveAt(base.Items.Count - 1);
            }
        }

        private int SetCustomColor(Color col)
        {
            this.RemoveCustomColor();
            base.Items.Add(col);
            return base.Items.Count - 1;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            bool flag = e.Index > -1;
            if (flag)
            {
                e.DrawBackground();
                Rectangle bounds = e.Bounds;
                bounds.Inflate(-2, -2);
                bounds.Width = 50;
                Color color = Color.Empty;
                bool flag2 = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                if (flag2)
                {
                    color = SystemColors.HighlightText;
                }
                else
                {
                    color = this.ForeColor;
                }
                Color color2 = (Color)base.Items[e.Index];
                using (Brush brush = new SolidBrush(color2))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                }
                bool flag3 = (e.State & DrawItemState.Selected) != DrawItemState.Selected;
                if (flag3)
                {
                    using (Pen pen = new Pen(e.ForeColor))
                    {
                        e.Graphics.DrawRectangle(pen, bounds);
                    }
                }
                using (Brush brush2 = new SolidBrush(e.ForeColor))
                {
                    string s = color2.Name + string.Format("({0})", e.Index);
                    bool flag4 = !color2.IsKnownColor;
                    if (flag4)
                    {
                        s = "<custom>";
                    }
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.LineAlignment = StringAlignment.Center;
                    bounds = e.Bounds;
                    bounds.X += 60;
                    e.Graphics.DrawString(s, this.Font, brush2, bounds, stringFormat);
                }
                e.DrawFocusRectangle();
            }
        }
    }
}
