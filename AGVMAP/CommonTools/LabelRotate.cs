using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class LabelRotate : Control
    {
        private float m_textAngle = 0f;

        private ContentAlignment m_rotatePointAlignment = ContentAlignment.MiddleCenter;

        private ContentAlignment m_textAlignment = ContentAlignment.MiddleLeft;

        private Color m_frameColor = Color.CadetBlue;

        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Refresh();
            }
        }

        public float TextAngle
        {
            get
            {
                return this.m_textAngle;
            }
            set
            {
                this.m_textAngle = value;
                base.Invalidate();
            }
        }

        public ContentAlignment TextAlign
        {
            get
            {
                return this.m_textAlignment;
            }
            set
            {
                this.m_textAlignment = value;
                base.Invalidate();
            }
        }

        public ContentAlignment RotatePointAlignment
        {
            get
            {
                return this.m_rotatePointAlignment;
            }
            set
            {
                this.m_rotatePointAlignment = value;
                base.Invalidate();
            }
        }

        protected RectangleF ClientRectangleF
        {
            get
            {
                RectangleF result = base.ClientRectangle;
                result.Width -= 1f;
                result.Height -= 1f;
                return result;
            }
        }

        public LabelRotate()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Text = string.Empty;
            this.DoubleBuffered = true;
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (SolidBrush solidBrush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(solidBrush, base.ClientRectangle);
            }
            RectangleF clientRectangleF = this.ClientRectangleF;
            Pen pen = new Pen(this.m_frameColor, 1f);
            Util.DrawFrame(e.Graphics, clientRectangleF, 6f, this.m_frameColor);
            bool flag = this.Text.Length > 0;
            if (flag)
            {
                StringFormat stringFormat = new StringFormat();
                string text = this.TextAlign.ToString();
                bool flag2 = (this.TextAlign & (ContentAlignment)273) > (ContentAlignment)0;
                if (flag2)
                {
                    stringFormat.Alignment = StringAlignment.Near;
                }
                bool flag3 = (this.TextAlign & (ContentAlignment)546) > (ContentAlignment)0;
                if (flag3)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                }
                bool flag4 = (this.TextAlign & (ContentAlignment)1092) > (ContentAlignment)0;
                if (flag4)
                {
                    stringFormat.Alignment = StringAlignment.Far;
                }
                bool flag5 = (this.TextAlign & (ContentAlignment)1792) > (ContentAlignment)0;
                if (flag5)
                {
                    stringFormat.LineAlignment = StringAlignment.Far;
                }
                bool flag6 = (this.TextAlign & (ContentAlignment)112) > (ContentAlignment)0;
                if (flag6)
                {
                    stringFormat.LineAlignment = StringAlignment.Center;
                }
                bool flag7 = (this.TextAlign & (ContentAlignment)7) > (ContentAlignment)0;
                if (flag7)
                {
                    stringFormat.LineAlignment = StringAlignment.Near;
                }
                Rectangle clientRectangle = base.ClientRectangle;
                clientRectangle.X += base.Padding.Left;
                clientRectangle.Y += base.Padding.Top;
                clientRectangle.Width -= base.Padding.Right;
                clientRectangle.Height -= base.Padding.Bottom;
                using (SolidBrush solidBrush2 = new SolidBrush(this.ForeColor))
                {
                    bool flag8 = this.TextAngle == 0f;
                    if (flag8)
                    {
                        e.Graphics.DrawString(this.Text, this.Font, solidBrush2, clientRectangle, stringFormat);
                    }
                    else
                    {
                        PointF pointF = Util.Center(base.ClientRectangle);
                        ContentAlignment rotatePointAlignment = this.RotatePointAlignment;
                        if (rotatePointAlignment <= ContentAlignment.MiddleCenter)
                        {
                            switch (rotatePointAlignment)
                            {
                                case ContentAlignment.TopLeft:
                                    pointF.X = (float)clientRectangle.Left;
                                    pointF.Y = (float)clientRectangle.Top;
                                    break;
                                case ContentAlignment.TopCenter:
                                    pointF.Y = (float)clientRectangle.Top;
                                    break;
                                case (ContentAlignment)3:
                                    break;
                                case ContentAlignment.TopRight:
                                    pointF.X = (float)clientRectangle.Right;
                                    pointF.Y = (float)clientRectangle.Top;
                                    break;
                                default:
                                    if (rotatePointAlignment != ContentAlignment.MiddleLeft)
                                    {
                                        if (rotatePointAlignment != ContentAlignment.MiddleCenter)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        pointF.X = (float)clientRectangle.Left;
                                    }
                                    break;
                            }
                        }
                        else if (rotatePointAlignment <= ContentAlignment.BottomLeft)
                        {
                            if (rotatePointAlignment != ContentAlignment.MiddleRight)
                            {
                                if (rotatePointAlignment == ContentAlignment.BottomLeft)
                                {
                                    pointF.X = (float)clientRectangle.Left;
                                    pointF.Y = (float)clientRectangle.Bottom;
                                }
                            }
                            else
                            {
                                pointF.X = (float)clientRectangle.Right;
                            }
                        }
                        else if (rotatePointAlignment != ContentAlignment.BottomCenter)
                        {
                            if (rotatePointAlignment == ContentAlignment.BottomRight)
                            {
                                pointF.X = (float)clientRectangle.Right;
                                pointF.Y = (float)clientRectangle.Bottom;
                            }
                        }
                        else
                        {
                            pointF.Y = (float)clientRectangle.Bottom;
                        }
                        pointF.X += (float)base.Padding.Left;
                        pointF.Y += (float)base.Padding.Top;
                        pointF.X -= (float)base.Padding.Right;
                        pointF.Y -= (float)base.Padding.Bottom;
                        e.Graphics.TranslateTransform(pointF.X, pointF.Y);
                        e.Graphics.RotateTransform(this.TextAngle);
                        e.Graphics.DrawString(this.Text, this.Font, solidBrush2, new PointF(0f, 0f), stringFormat);
                        e.Graphics.ResetTransform();
                    }
                }
            }
            base.RaisePaintEvent(this, e);
        }
    }
}
