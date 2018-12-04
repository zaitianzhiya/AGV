using Canvas.CanvasInterfaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Canvas.Utils
{
	public class DrawUtils
	{
		private static Pen m_selectedPen = null;

		public static Pen SelectedPen
		{
			get
			{
				bool flag = DrawUtils.m_selectedPen == null;
				if (flag)
				{
					DrawUtils.m_selectedPen = new Pen(Color.Magenta, 1f);
					DrawUtils.m_selectedPen.DashStyle = DashStyle.Dash;
				}
				return DrawUtils.m_selectedPen;
			}
		}

		public static void DrawNode(ICanvas canvas, UnitPoint nodepoint)
		{
			try
			{
				RectangleF rectangleF = new RectangleF(canvas.ToScreen(nodepoint), new SizeF(0f, 0f));
				rectangleF.Inflate(3f, 3f);
				bool flag = rectangleF.Right < 0f || rectangleF.Left > (float)canvas.ClientRectangle.Width;
				if (!flag)
				{
					bool flag2 = rectangleF.Top < 0f || rectangleF.Bottom > (float)canvas.ClientRectangle.Height;
					if (!flag2)
					{
						canvas.Graphics.FillRectangle(Brushes.White, rectangleF);
						rectangleF.Inflate(1f, 1f);
						canvas.Graphics.DrawRectangle(Pens.Black, ScreenUtils.ConvertRect(rectangleF));
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void DrawTriangleNode(ICanvas canvas, UnitPoint nodepoint)
		{
			try
			{
				PointF pointF = canvas.ToScreen(nodepoint);
				float num = 4f;
				PointF[] points = new PointF[]
				{
					new PointF(pointF.X - num, pointF.Y),
					new PointF(pointF.X, pointF.Y + num),
					new PointF(pointF.X + num, pointF.Y),
					new PointF(pointF.X, pointF.Y - num)
				};
				canvas.Graphics.FillPolygon(Brushes.White, points);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
