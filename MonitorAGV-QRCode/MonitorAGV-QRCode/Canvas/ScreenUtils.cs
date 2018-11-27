using Canvas.CanvasInterfaces;
using System;
using System.Drawing;

namespace Canvas
{
	public class ScreenUtils
	{
		public static PointF RightPoint(ICanvas canvas, RectangleF unitrect)
		{
			PointF location = unitrect.Location;
			float x = location.X + unitrect.Width;
			float y = location.Y + unitrect.Height;
			return new PointF(x, y);
		}

		public static RectangleF ToScreen(ICanvas canvas, RectangleF unitrect)
		{
			return new RectangleF
			{
				Location = canvas.ToScreen(new UnitPoint(unitrect.Location)),
				Width = (float)Math.Round((double)canvas.ToScreen((double)unitrect.Width)),
				Height = (float)Math.Round((double)canvas.ToScreen((double)unitrect.Height))
			};
		}

		public static RectangleF ToUnit(ICanvas canvas, Rectangle screenrect)
		{
			UnitPoint unitPoint = canvas.ToUnit(screenrect.Location);
			SizeF size = new SizeF((float)canvas.ToUnit((float)screenrect.Width), (float)canvas.ToUnit((float)screenrect.Height));
			RectangleF result = new RectangleF(unitPoint.Point, size);
			return result;
		}

		public static RectangleF ToScreenNormalized(ICanvas canvas, RectangleF unitrect)
		{
			RectangleF result = ScreenUtils.ToScreen(canvas, unitrect);
			result.Y -= result.Height;
			return result;
		}

		public static RectangleF ToUnitNormalized(ICanvas canvas, Rectangle screenrect)
		{
			UnitPoint unitPoint = canvas.ToUnit(screenrect.Location);
			SizeF size = new SizeF((float)canvas.ToUnit((float)screenrect.Width), (float)canvas.ToUnit((float)screenrect.Height));
			RectangleF result = new RectangleF(unitPoint.Point, size);
			result.Y -= result.Height;
			return result;
		}

		public static Rectangle ConvertRect(RectangleF r)
		{
			return new Rectangle((int)r.Left, (int)r.Top, (int)r.Width, (int)r.Height);
		}

		public static RectangleF GetRect(UnitPoint p1, UnitPoint p2, double width)
		{
			double x = Math.Min(p1.X, p2.X);
			double y = Math.Min(p1.Y, p2.Y);
			double w = Math.Abs(p1.X - p2.X);
			double h = Math.Abs(p1.Y - p2.Y);
			RectangleF rect = ScreenUtils.GetRect(x, y, w, h);
			rect.Inflate((float)width, (float)width);
			return rect;
		}

		public static RectangleF GetRect(double x, double y, double w, double h)
		{
			return new RectangleF((float)x, (float)y, (float)w, (float)h);
		}

		public static Point ConvertPoint(PointF p)
		{
			return new Point((int)p.X, (int)p.Y);
		}
	}
}
