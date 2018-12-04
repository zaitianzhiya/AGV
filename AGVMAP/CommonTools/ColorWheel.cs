using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class ColorWheel : Control
    {
        private Color m_frameColor = Color.CadetBlue;

        private HSLColor m_selectedColor = new HSLColor(Color.BlanchedAlmond);

        private PathGradientBrush m_brush = null;

        private List<PointF> m_path = new List<PointF>();

        private List<Color> m_colors = new List<Color>();

        private double m_wheelLightness = 0.5;

        [method: CompilerGenerated]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        public event EventHandler SelectedColorChanged;

        public HSLColor SelectedHSLColor
        {
            get
            {
                return this.m_selectedColor;
            }
            set
            {
                bool flag = this.m_selectedColor == value;
                if (!flag)
                {
                    base.Invalidate(Util.Rect(this.ColorSelectorRectangle));
                    this.m_selectedColor = value;
                    bool flag2 = this.SelectedColorChanged != null;
                    if (flag2)
                    {
                        this.SelectedColorChanged(this, null);
                    }
                    this.Refresh();
                }
            }
        }

        private RectangleF ColorSelectorRectangle
        {
            get
            {
                HSLColor selectedColor = this.m_selectedColor;
                double num = selectedColor.Hue * 3.1415926535897931 / 180.0;
                PointF pointF = Util.Center(this.ColorWheelRectangle);
                double num2 = (double)this.Radius(this.ColorWheelRectangle);
                num2 *= selectedColor.Saturation;
                double num3 = (double)pointF.X + Math.Cos(num) * num2;
                double num4 = (double)pointF.Y - Math.Sin(num) * num2;
                Rectangle r = new Rectangle(new Point((int)num3, (int)num4), new Size(0, 0));
                r.Inflate(12, 12);
                return r;
            }
        }

        private RectangleF WheelRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                clientRectangle.Width--;
                clientRectangle.Height--;
                return clientRectangle;
            }
        }

        private RectangleF ColorWheelRectangle
        {
            get
            {
                RectangleF wheelRectangle = this.WheelRectangle;
                wheelRectangle.Inflate(-5f, -5f);
                return wheelRectangle;
            }
        }

        public ColorWheel()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
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
            using (SolidBrush solidBrush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(solidBrush, base.ClientRectangle);
            }
            RectangleF rectangleF = this.WheelRectangle;
            Util.DrawFrame(e.Graphics, rectangleF, 6f, this.m_frameColor);
            rectangleF = this.ColorWheelRectangle;
            PointF centerPoint = Util.Center(rectangleF);
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            bool flag = this.m_brush == null;
            if (flag)
            {
                this.m_brush = new PathGradientBrush(this.m_path.ToArray(), WrapMode.Clamp);
                this.m_brush.CenterPoint = centerPoint;
                this.m_brush.CenterColor = Color.White;
                this.m_brush.SurroundColors = this.m_colors.ToArray();
            }
            e.Graphics.FillPie(this.m_brush, Util.Rect(rectangleF), 0f, 360f);
            this.DrawColorSelector(e.Graphics);
            bool focused = this.Focused;
            if (focused)
            {
                RectangleF wheelRectangle = this.WheelRectangle;
                wheelRectangle.Inflate(-2f, -2f);
                ControlPaint.DrawFocusRectangle(e.Graphics, Util.Rect(wheelRectangle));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bool flag = this.m_brush != null;
            if (flag)
            {
                this.m_brush.Dispose();
            }
            this.m_brush = null;
            this.RecalcWheelPoints();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            PointF color = new PointF((float)e.X, (float)e.Y);
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                this.SetColor(color);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();
            PointF color = new PointF((float)e.X, (float)e.Y);
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                this.SetColor(color);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            HSLColor selectedHSLColor = this.SelectedHSLColor;
            double num = selectedHSLColor.Hue;
            int num2 = 1;
            bool flag = (keyData & Keys.Control) == Keys.Control;
            if (flag)
            {
                num2 = 5;
            }
            bool flag2 = (keyData & Keys.Up) == Keys.Up;
            if (flag2)
            {
                num += (double)num2;
            }
            bool flag3 = (keyData & Keys.Down) == Keys.Down;
            if (flag3)
            {
                num -= (double)num2;
            }
            bool flag4 = num >= 360.0;
            if (flag4)
            {
                num = 0.0;
            }
            bool flag5 = num < 0.0;
            if (flag5)
            {
                num = 359.0;
            }
            bool flag6 = num != selectedHSLColor.Hue;
            bool result;
            if (flag6)
            {
                selectedHSLColor.Hue = num;
                this.SelectedHSLColor = selectedHSLColor;
                result = true;
            }
            else
            {
                result = base.ProcessDialogKey(keyData);
            }
            return result;
        }

        private void DrawColorSelector(Graphics dc)
        {
            Rectangle r = Util.Rect(this.ColorSelectorRectangle);
            PointF pointF = Util.Center(r);
            Image image = SelectorImages.Image(SelectorImages.eIndexes.Donut);
            dc.DrawImageUnscaled(image, (int)(pointF.X - (float)(image.Width / 2)), (int)(pointF.Y - (float)(image.Height / 2)));
        }

        private float Radius(RectangleF r)
        {
            PointF pointF = Util.Center(r);
            return Math.Min(r.Width / 2f, r.Height / 2f);
        }

        private void RecalcWheelPoints()
        {
            this.m_path.Clear();
            this.m_colors.Clear();
            PointF pointF = Util.Center(this.ColorWheelRectangle);
            float num = this.Radius(this.ColorWheelRectangle);
            double num2 = 0.0;
            double num3 = 360.0;
            double num4 = 5.0;
            while (num2 < num3)
            {
                double num5 = num2 * 0.017453292519943295;
                double num6 = (double)pointF.X + Math.Cos(num5) * (double)num;
                double num7 = (double)pointF.Y - Math.Sin(num5) * (double)num;
                this.m_path.Add(new PointF((float)num6, (float)num7));
                this.m_colors.Add(new HSLColor(num2, 1.0, this.m_wheelLightness).Color);
                num2 += num4;
            }
        }

        private void SetColor(PointF mousepoint)
        {
            bool flag = !this.WheelRectangle.Contains(mousepoint);
            if (!flag)
            {
                PointF pointF = Util.Center(this.ColorWheelRectangle);
                double num = (double)this.Radius(this.ColorWheelRectangle);
                double num2 = (double)Math.Abs(mousepoint.X - pointF.X);
                double num3 = (double)Math.Abs(mousepoint.Y - pointF.Y);
                double num4 = Math.Atan(num3 / num2) / 3.1415926535897931 * 180.0;
                double num5 = Math.Pow(Math.Pow(num2, 2.0) + Math.Pow(num3, 2.0), 0.5);
                double saturation = num5 / num;
                bool flag2 = num5 < 6.0;
                if (flag2)
                {
                    saturation = 0.0;
                }
                bool flag3 = mousepoint.X < pointF.X;
                if (flag3)
                {
                    num4 = 180.0 - num4;
                }
                bool flag4 = mousepoint.Y > pointF.Y;
                if (flag4)
                {
                    num4 = 360.0 - num4;
                }
                this.SelectedHSLColor = new HSLColor(num4, saturation, this.SelectedHSLColor.Lightness);
            }
        }
    }
}
