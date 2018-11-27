using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class Util
    {
        public static Rectangle Rect(RectangleF rf)
        {
            return new Rectangle
            {
                X = (int)rf.X,
                Y = (int)rf.Y,
                Width = (int)rf.Width,
                Height = (int)rf.Height
            };
        }

        public static RectangleF Rect(Rectangle r)
        {
            return new RectangleF
            {
                X = (float)r.X,
                Y = (float)r.Y,
                Width = (float)r.Width,
                Height = (float)r.Height
            };
        }

        public static Point Point(PointF pf)
        {
            return new Point((int)pf.X, (int)pf.Y);
        }

        public static PointF Center(RectangleF r)
        {
            PointF location = r.Location;
            location.X += r.Width / 2f;
            location.Y += r.Height / 2f;
            return location;
        }

        public static void DrawFrame(Graphics dc, RectangleF r, float cornerRadius, Color color)
        {
            Pen pen = new Pen(color);
            bool flag = cornerRadius <= 0f;
            if (flag)
            {
                dc.DrawRectangle(pen, Util.Rect(r));
            }
            else
            {
                cornerRadius = (float)Math.Min((double)cornerRadius, Math.Floor((double)r.Width) - 2.0);
                cornerRadius = (float)Math.Min((double)cornerRadius, Math.Floor((double)r.Height) - 2.0);
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddArc(r.X, r.Y, cornerRadius, cornerRadius, 180f, 90f);
                graphicsPath.AddArc(r.Right - cornerRadius, r.Y, cornerRadius, cornerRadius, 270f, 90f);
                graphicsPath.AddArc(r.Right - cornerRadius, r.Bottom - cornerRadius, cornerRadius, cornerRadius, 0f, 90f);
                graphicsPath.AddArc(r.X, r.Bottom - cornerRadius, cornerRadius, cornerRadius, 90f, 90f);
                graphicsPath.CloseAllFigures();
                dc.DrawPath(pen, graphicsPath);
            }
        }

        public static void Draw2ColorBar(Graphics dc, RectangleF r, Orientation orientation, Color c1, Color c2)
        {
            RectangleF rect = r;
            float angle = 0f;
            bool flag = orientation == Orientation.Vertical;
            if (flag)
            {
                angle = 270f;
            }
            bool flag2 = orientation == Orientation.Horizontal;
            if (flag2)
            {
                angle = 0f;
            }
            bool flag3 = rect.Height > 0f && rect.Width > 0f;
            if (flag3)
            {
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, c1, c2, angle, false);
                dc.FillRectangle(linearGradientBrush, rect);
                linearGradientBrush.Dispose();
            }
        }

        public static void Draw3ColorBar(Graphics dc, RectangleF r, Orientation orientation, Color c1, Color c2, Color c3)
        {
            RectangleF rect = r;
            RectangleF rect2 = r;
            float angle = 0f;
            bool flag = orientation == Orientation.Vertical;
            if (flag)
            {
                angle = 270f;
                rect.Height /= 2f;
                rect2.Height = r.Height - rect.Height;
                rect2.Y += rect.Height;
            }
            bool flag2 = orientation == Orientation.Horizontal;
            if (flag2)
            {
                angle = 0f;
                rect.Width /= 2f;
                rect2.Width = r.Width - rect.Width;
                rect.X = rect2.Right;
            }
            bool flag3 = rect.Height > 0f && rect.Width > 0f;
            if (flag3)
            {
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect2, c1, c2, angle, false);
                LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect, c2, c3, angle, false);
                dc.FillRectangle(linearGradientBrush2, rect);
                dc.FillRectangle(linearGradientBrush, rect2);
                linearGradientBrush2.Dispose();
                linearGradientBrush.Dispose();
            }
            bool flag4 = orientation == Orientation.Vertical;
            if (flag4)
            {
                Pen pen = new Pen(c2, 1f);
                Pen pen2 = new Pen(c3, 1f);
                dc.DrawLine(pen2, rect.Left, rect.Top, rect.Right - 1f, rect.Top);
                dc.DrawLine(pen, rect2.Left, rect2.Top, rect2.Right - 1f, rect2.Top);
                pen.Dispose();
                pen2.Dispose();
            }
            bool flag5 = orientation == Orientation.Horizontal;
            if (flag5)
            {
                Pen pen3 = new Pen(c1, 1f);
                Pen pen4 = new Pen(c2, 1f);
                Pen pen5 = new Pen(c3, 1f);
                dc.DrawLine(pen3, rect2.Left, rect2.Top, rect2.Left, rect2.Bottom - 1f);
                dc.DrawLine(pen4, rect2.Right, rect2.Top, rect2.Right, rect2.Bottom - 1f);
                dc.DrawLine(pen5, rect.Right, rect.Top, rect.Right, rect.Bottom - 1f);
                pen3.Dispose();
                pen4.Dispose();
                pen5.Dispose();
            }
        }
    }
}
