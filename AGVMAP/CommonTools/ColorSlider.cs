using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class ColorSlider : LabelRotate
    {
        public enum eNumberOfColors
        {
            Use2Colors,
            Use3Colors
        }

        public enum eValueOrientation
        {
            MinToMax,
            MaxToMin
        }

        private Orientation m_orientation = Orientation.Vertical;

        private ColorSlider.eNumberOfColors m_numberOfColors = ColorSlider.eNumberOfColors.Use3Colors;

        private ColorSlider.eValueOrientation m_valueOrientation = ColorSlider.eValueOrientation.MinToMax;

        private float m_percent = 0f;

        private Color m_color1 = Color.Black;

        private Color m_color2 = Color.FromArgb(255, 127, 127, 127);

        private Color m_color3 = Color.White;

        private Padding m_barPadding = new Padding(12, 5, 24, 10);

        [method: CompilerGenerated]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        public event EventHandler SelectedValueChanged;

        public Orientation Orientation
        {
            get
            {
                return this.m_orientation;
            }
            set
            {
                this.m_orientation = value;
            }
        }

        public ColorSlider.eNumberOfColors NumberOfColors
        {
            get
            {
                return this.m_numberOfColors;
            }
            set
            {
                this.m_numberOfColors = value;
            }
        }

        public ColorSlider.eValueOrientation ValueOrientation
        {
            get
            {
                return this.m_valueOrientation;
            }
            set
            {
                this.m_valueOrientation = value;
            }
        }

        public float Percent
        {
            get
            {
                return this.m_percent;
            }
            set
            {
                bool flag = value < 0f;
                if (flag)
                {
                    value = 0f;
                }
                bool flag2 = value > 1f;
                if (flag2)
                {
                    value = 1f;
                }
                bool flag3 = value != this.m_percent;
                if (flag3)
                {
                    this.m_percent = value;
                    bool flag4 = this.SelectedValueChanged != null;
                    if (flag4)
                    {
                        this.SelectedValueChanged(this, null);
                    }
                    base.Invalidate();
                }
            }
        }

        public Color Color1
        {
            get
            {
                return this.m_color1;
            }
            set
            {
                this.m_color1 = value;
            }
        }

        public Color Color2
        {
            get
            {
                return this.m_color2;
            }
            set
            {
                this.m_color2 = value;
            }
        }

        public Color Color3
        {
            get
            {
                return this.m_color3;
            }
            set
            {
                this.m_color3 = value;
            }
        }

        public Padding BarPadding
        {
            get
            {
                return this.m_barPadding;
            }
            set
            {
                this.m_barPadding = value;
                base.Invalidate();
            }
        }

        protected RectangleF BarRectangle
        {
            get
            {
                RectangleF result = base.ClientRectangle;
                result.X += (float)this.BarPadding.Left;
                result.Width -= (float)this.BarPadding.Right;
                result.Y += (float)this.BarPadding.Top;
                result.Height -= (float)this.BarPadding.Bottom;
                return result;
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DrawColorBar(e.Graphics);
            bool focused = this.Focused;
            if (focused)
            {
                RectangleF clientRectangleF = base.ClientRectangleF;
                clientRectangleF.Inflate(-2f, -2f);
                ControlPaint.DrawFocusRectangle(e.Graphics, Util.Rect(clientRectangleF));
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            PointF percent = new PointF((float)e.X, (float)e.Y);
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                this.SetPercent(percent);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();
            PointF percent = new PointF((float)e.X, (float)e.Y);
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                this.SetPercent(percent);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            float num = this.Percent * 100f;
            int num2 = 0;
            bool flag = (keyData & Keys.Up) == Keys.Up;
            if (flag)
            {
                num2 = 1;
            }
            bool flag2 = (keyData & Keys.Down) == Keys.Down;
            if (flag2)
            {
                num2 = -1;
            }
            bool flag3 = (keyData & Keys.Control) == Keys.Control;
            if (flag3)
            {
                num2 *= 5;
            }
            bool flag4 = num2 != 0;
            bool result;
            if (flag4)
            {
                this.SetPercent((float)Math.Round((double)(num + (float)num2)));
                result = true;
            }
            else
            {
                result = base.ProcessDialogKey(keyData);
            }
            return result;
        }

        protected virtual void SetPercent(float percent)
        {
            this.Percent = percent / 100f;
        }

        protected virtual void SetPercent(PointF mousepoint)
        {
            RectangleF clientRectangleF = base.ClientRectangleF;
            RectangleF barRectangle = this.BarRectangle;
            mousepoint.X += clientRectangleF.X - barRectangle.X;
            mousepoint.Y += clientRectangleF.Y - barRectangle.Y;
            this.Percent = this.GetPercentSet(this.BarRectangle, this.Orientation, mousepoint);
            this.Refresh();
        }

        protected float GetPercentSet(RectangleF r, Orientation orientation, PointF mousepoint)
        {
            float num = 0f;
            bool flag = orientation == Orientation.Vertical;
            if (flag)
            {
                bool flag2 = this.m_valueOrientation == ColorSlider.eValueOrientation.MaxToMin;
                if (flag2)
                {
                    num = 1f - (mousepoint.Y - r.Y / r.Height) / r.Height;
                }
                else
                {
                    num = mousepoint.Y / r.Height;
                }
            }
            bool flag3 = orientation == Orientation.Horizontal;
            if (flag3)
            {
                bool flag4 = this.m_valueOrientation == ColorSlider.eValueOrientation.MaxToMin;
                if (flag4)
                {
                    num = 1f - (mousepoint.X - r.X / r.Width) / r.Width;
                }
                else
                {
                    num = mousepoint.X / r.Width;
                }
            }
            bool flag5 = num < 0f;
            if (flag5)
            {
                num = 0f;
            }
            bool flag6 = num > 100f;
            if (flag6)
            {
                num = 100f;
            }
            return num;
        }

        protected void DrawSelector(Graphics dc, RectangleF r, Orientation orientation, float percentSet)
        {
            Pen pen = new Pen(Color.CadetBlue);
            percentSet = Math.Max(0f, percentSet);
            percentSet = Math.Min(1f, percentSet);
            bool flag = orientation == Orientation.Vertical;
            if (flag)
            {
                float num = (float)Math.Floor((double)(r.Top + (r.Height - r.Height * percentSet)));
                bool flag2 = this.m_valueOrientation == ColorSlider.eValueOrientation.MaxToMin;
                if (flag2)
                {
                    num = (float)Math.Floor((double)(r.Top + (r.Height - r.Height * percentSet)));
                }
                else
                {
                    num = (float)Math.Floor((double)(r.Top + r.Height * percentSet));
                }
                dc.DrawLine(pen, r.X, num, r.Right, num);
                Image image = SelectorImages.Image(SelectorImages.eIndexes.Right);
                float num2 = r.Right;
                float num3 = num - (float)(image.Height / 2);
                dc.DrawImageUnscaled(image, (int)num2, (int)num3);
                image = SelectorImages.Image(SelectorImages.eIndexes.Left);
                num2 = r.Left - (float)image.Width;
                dc.DrawImageUnscaled(image, (int)num2, (int)num3);
            }
            bool flag3 = orientation == Orientation.Horizontal;
            if (flag3)
            {
                bool flag4 = this.m_valueOrientation == ColorSlider.eValueOrientation.MaxToMin;
                float num4;
                if (flag4)
                {
                    num4 = (float)Math.Floor((double)(r.Left + (r.Width - r.Width * percentSet)));
                }
                else
                {
                    num4 = (float)Math.Floor((double)(r.Left + r.Width * percentSet));
                }
                dc.DrawLine(pen, num4, r.Top, num4, r.Bottom);
                Image image2 = SelectorImages.Image(SelectorImages.eIndexes.Up);
                float num5 = num4 - (float)(image2.Width / 2);
                float num6 = r.Bottom;
                dc.DrawImageUnscaled(image2, (int)num5, (int)num6);
                image2 = SelectorImages.Image(SelectorImages.eIndexes.Down);
                num6 = r.Top - (float)image2.Height;
                dc.DrawImageUnscaled(image2, (int)num5, (int)num6);
            }
        }

        protected void DrawColorBar(Graphics dc)
        {
            RectangleF barRectangle = this.BarRectangle;
            bool flag = this.m_numberOfColors == ColorSlider.eNumberOfColors.Use2Colors;
            if (flag)
            {
                Util.Draw2ColorBar(dc, barRectangle, this.Orientation, this.m_color1, this.m_color2);
            }
            else
            {
                Util.Draw3ColorBar(dc, barRectangle, this.Orientation, this.m_color1, this.m_color2, this.m_color3);
            }
            this.DrawSelector(dc, barRectangle, this.Orientation, this.Percent);
        }
    }
}
