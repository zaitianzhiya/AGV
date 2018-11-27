using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ColorTable : LabelRotate
    {
        private int m_cols = 0;

        private int m_rows = 0;

        private Size m_fieldSize = new Size(12, 12);

        private List<Color> m_colors = new List<Color>();

        private int m_spacing = 3;

        private int m_selindex = 0;

        private int m_initialColorCount = 0;

        [method: CompilerGenerated]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        public event EventHandler SelectedIndexChanged;

        public Color SelectedItem
        {
            get
            {
                bool flag = this.m_selindex < 0 || this.m_selindex >= this.m_colors.Count;
                Color result;
                if (flag)
                {
                    result = Color.White;
                }
                else
                {
                    result = this.m_colors[this.m_selindex];
                }
                return result;
            }
            set
            {
                bool flag = this.m_selindex < this.m_colors.Count && value == this.m_colors[this.m_selindex];
                if (!flag)
                {
                    int num = this.m_colors.IndexOf(value);
                    bool flag2 = num < 0;
                    if (!flag2)
                    {
                        this.SetIndex(num);
                    }
                }
            }
        }

        public int Cols
        {
            get
            {
                return this.m_cols;
            }
            set
            {
                this.m_cols = value;
                this.m_rows = this.m_colors.Count / this.m_cols;
                bool flag = this.m_colors.Count % this.m_cols != 0;
                if (flag)
                {
                    int rows = this.m_rows;
                    this.m_rows = rows + 1;
                }
            }
        }

        public Size FieldSize
        {
            get
            {
                return this.m_fieldSize;
            }
            set
            {
                this.m_fieldSize = value;
            }
        }

        public Color[] Colors
        {
            get
            {
                return this.m_colors.ToArray();
            }
            set
            {
                this.m_colors = new List<Color>(value);
                this.Cols = 16;
                this.m_initialColorCount = this.m_colors.Count;
            }
        }

        public bool ColorExist(Color c)
        {
            int num = this.m_colors.IndexOf(c);
            return num >= 0;
        }

        private int CompareColorByValue(Color c1, Color c2)
        {
            int num = (int)c1.R << 16 | (int)c1.G << 8 | (int)c1.B;
            int num2 = (int)c2.R << 16 | (int)c2.G << 8 | (int)c2.B;
            bool flag = num > num2;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                bool flag2 = num < num2;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        private int CompareColorByHue(Color c1, Color c2)
        {
            float hue = c1.GetHue();
            float hue2 = c2.GetHue();
            bool flag = hue < hue2;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                bool flag2 = hue > hue2;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        private int CompareColorByBrightness(Color c1, Color c2)
        {
            float brightness = c1.GetBrightness();
            float brightness2 = c2.GetBrightness();
            bool flag = brightness < brightness2;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                bool flag2 = brightness > brightness2;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        public void SortColorByValue()
        {
            this.m_colors.Sort(new Comparison<Color>(this.CompareColorByValue));
            base.Invalidate();
        }

        public void SortColorByHue()
        {
            this.m_colors.Sort(new Comparison<Color>(this.CompareColorByHue));
            base.Invalidate();
        }

        public void SortColorByBrightness()
        {
            this.m_colors.Sort(new Comparison<Color>(this.CompareColorByBrightness));
            base.Invalidate();
        }

        public ColorTable(Color[] colors)
        {
            this.DoubleBuffered = true;
            bool flag = colors != null;
            if (flag)
            {
                this.m_colors = new List<Color>(colors);
            }
            this.Cols = 16;
            this.m_initialColorCount = this.m_colors.Count;
            base.Padding = new Padding(8, 8, 0, 0);
        }

        public ColorTable()
        {
            this.DoubleBuffered = true;
            PropertyInfo[] properties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public);
            PropertyInfo[] array = properties;
            int i = 0;
            while (i < array.Length)
            {
                PropertyInfo propertyInfo = array[i];
                bool flag = propertyInfo.PropertyType == typeof(Color);
                if (flag)
                {
                    Color item = (Color)propertyInfo.GetValue(typeof(Color), null);
                    bool flag2 = item.A == 0;
                    if (!flag2)
                    {
                        this.m_colors.Add(item);
                    }
                }
            IL_C1:
                i++;
                continue;
                goto IL_C1;
            }
            this.m_colors.Sort(new Comparison<Color>(this.CompareColorByBrightness));
            this.m_initialColorCount = this.m_colors.Count;
            this.Cols = 16;
        }

        public void RemoveCustomColor()
        {
            bool flag = this.m_colors.Count > this.m_initialColorCount;
            if (flag)
            {
                this.m_colors.RemoveAt(this.m_colors.Count - 1);
            }
        }

        public void SetCustomColor(Color col)
        {
            this.RemoveCustomColor();
            bool flag = !this.m_colors.Contains(col);
            if (flag)
            {
                int rows = this.m_rows;
                this.m_colors.Add(col);
                this.Cols = this.Cols;
                bool flag2 = this.m_rows != rows;
                if (flag2)
                {
                    base.Invalidate();
                }
                else
                {
                    base.Invalidate(this.GetRectangle(this.m_colors.Count - 1));
                }
            }
        }

        private Rectangle GetSelectedItemRect()
        {
            Rectangle rectangle = this.GetRectangle(this.m_selindex);
            rectangle.Inflate(this.m_fieldSize.Width / 2, this.m_fieldSize.Height / 2);
            return rectangle;
        }

        private Rectangle GetRectangle(int index)
        {
            int row = 0;
            int col = 0;
            this.GetRowCol(index, ref row, ref col);
            return this.GetRectangle(row, col);
        }

        private void GetRowCol(int index, ref int row, ref int col)
        {
            row = index / this.m_cols;
            col = index - row * this.m_cols;
        }

        private Rectangle GetRectangle(int row, int col)
        {
            int x = base.Padding.Left + col * (this.m_fieldSize.Width + this.m_spacing);
            int y = base.Padding.Top + row * (this.m_fieldSize.Height + this.m_spacing);
            return new Rectangle(x, y, this.m_fieldSize.Width, this.m_fieldSize.Height);
        }

        private int GetIndexFromMousePos(int x, int y)
        {
            int col = (x - base.Padding.Left) / (this.m_fieldSize.Width + this.m_spacing);
            int row = (y - base.Padding.Top) / (this.m_fieldSize.Height + this.m_spacing);
            return this.GetIndex(row, col);
        }

        private int GetIndex(int row, int col)
        {
            bool flag = col < 0 || col >= this.m_cols;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                bool flag2 = row < 0 || row >= this.m_rows;
                if (flag2)
                {
                    result = -1;
                }
                else
                {
                    result = row * this.m_cols + col;
                }
            }
            return result;
        }

        private void SetIndex(int index)
        {
            bool flag = index == this.m_selindex;
            if (!flag)
            {
                base.Invalidate(this.GetSelectedItemRect());
                this.m_selindex = index;
                bool flag2 = this.SelectedIndexChanged != null;
                if (flag2)
                {
                    this.SelectedIndexChanged(this, null);
                }
                base.Invalidate(this.GetSelectedItemRect());
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();
            bool flag = this.GetSelectedItemRect().Contains(new Point(e.X, e.Y));
            if (!flag)
            {
                int indexFromMousePos = this.GetIndexFromMousePos(e.X, e.Y);
                bool flag2 = indexFromMousePos != -1;
                if (flag2)
                {
                    this.SetIndex(indexFromMousePos);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int num = 0;
            int width = this.m_cols * (this.m_fieldSize.Width + this.m_spacing);
            int height = this.m_rows * (this.m_fieldSize.Height + this.m_spacing);
            int num2 = this.m_spacing / 2 + 1;
            Rectangle rect = new Rectangle(0, 0, width, height);
            rect.X += base.Padding.Left - num2;
            rect.Y += base.Padding.Top - num2;
            e.Graphics.DrawRectangle(Pens.CadetBlue, rect);
            int num3 = rect.X;
            rect.X = num3 + 1;
            num3 = rect.Y;
            rect.Y = num3 + 1;
            num3 = rect.Width;
            rect.Width = num3 - 1;
            num3 = rect.Height;
            rect.Height = num3 - 1;
            e.Graphics.FillRectangle(Brushes.White, rect);
            for (int i = 1; i < this.m_cols; i = num3 + 1)
            {
                int num4 = base.Padding.Left - num2 + i * (this.m_fieldSize.Width + this.m_spacing);
                e.Graphics.DrawLine(Pens.CadetBlue, num4, rect.Y, num4, rect.Bottom - 1);
                num3 = i;
            }
            for (int j = 1; j < this.m_rows; j = num3 + 1)
            {
                int num5 = base.Padding.Top - num2 + j * (this.m_fieldSize.Height + this.m_spacing);
                e.Graphics.DrawLine(Pens.CadetBlue, rect.X, num5, rect.Right - 1, num5);
                num3 = j;
            }
            for (int k = 0; k < this.m_rows; k = num3 + 1)
            {
                for (int l = 0; l < this.m_cols; l = num3 + 1)
                {
                    bool flag = num >= this.m_colors.Count;
                    if (flag)
                    {
                        break;
                    }
                    Rectangle rectangle = this.GetRectangle(k, l);
                    List<Color> arg_24B_0 = this.m_colors;
                    num3 = num;
                    num = num3 + 1;
                    using (SolidBrush solidBrush = new SolidBrush(arg_24B_0[num3]))
                    {
                        e.Graphics.FillRectangle(solidBrush, rectangle);
                    }
                    num3 = l;
                }
                num3 = k;
            }
            bool flag2 = this.m_selindex >= 0;
            if (flag2)
            {
                Rectangle selectedItemRect = this.GetSelectedItemRect();
                e.Graphics.FillRectangle(Brushes.White, selectedItemRect);
                selectedItemRect.Inflate(-3, -3);
                using (SolidBrush solidBrush2 = new SolidBrush(this.SelectedItem))
                {
                    e.Graphics.FillRectangle(solidBrush2, selectedItemRect);
                }
                bool focused = this.Focused;
                if (focused)
                {
                    selectedItemRect.Inflate(2, 2);
                    ControlPaint.DrawFocusRectangle(e.Graphics, selectedItemRect);
                }
                else
                {
                    selectedItemRect.X -= 2;
                    selectedItemRect.Y -= 2;
                    selectedItemRect.Width += 3;
                    selectedItemRect.Height += 3;
                    e.Graphics.DrawRectangle(Pens.CadetBlue, selectedItemRect);
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            base.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            base.Invalidate();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool flag = false;
            int num = 0;
            int num2 = 0;
            this.GetRowCol(this.m_selindex, ref num, ref num2);
            switch (keyData)
            {
                case Keys.Left:
                    {
                        int num3 = num2;
                        num2 = num3 - 1;
                        bool flag2 = num2 < 0;
                        if (flag2)
                        {
                            num2 = this.m_cols - 1;
                            num3 = num;
                            num = num3 - 1;
                        }
                        flag = true;
                        break;
                    }
                case Keys.Up:
                    {
                        int num3 = num;
                        num = num3 - 1;
                        flag = true;
                        break;
                    }
                case Keys.Right:
                    {
                        int num3 = num2;
                        num2 = num3 + 1;
                        bool flag3 = num2 >= this.m_cols;
                        if (flag3)
                        {
                            num2 = 0;
                            num3 = num;
                            num = num3 + 1;
                        }
                        flag = true;
                        break;
                    }
                case Keys.Down:
                    {
                        int num3 = num;
                        num = num3 + 1;
                        flag = true;
                        break;
                    }
            }
            bool flag4 = flag;
            bool result;
            if (flag4)
            {
                int index = this.GetIndex(num, num2);
                bool flag5 = index != -1;
                if (flag5)
                {
                    this.SetIndex(index);
                }
                result = false;
            }
            else
            {
                result = base.ProcessDialogKey(keyData);
            }
            return result;
        }
    }
}
