using Canvas.CanvasInterfaces;
using System;
using System.Drawing;

namespace Canvas
{
	public class SelectionRectangle
	{
		private PointF m_point1;

		private PointF m_point2;

		public SelectionRectangle(PointF mousedownpoint)
		{
			this.m_point1 = mousedownpoint;
			this.m_point2 = PointF.Empty;
		}

		public void Reset()
		{
			this.m_point2 = PointF.Empty;
		}

		public void SetMousePoint(Graphics dc, PointF mousepoint)
		{
			bool flag = this.m_point2 != PointF.Empty;
			if (flag)
			{
				XorGdi.DrawRectangle(dc, PenStyles.PS_DOT, 1, this.GetColor(), this.m_point1, this.m_point2);
			}
			this.m_point2 = mousepoint;
			XorGdi.DrawRectangle(dc, PenStyles.PS_DOT, 1, this.GetColor(), this.m_point1, this.m_point2);
		}

		public Rectangle ScreenRect()
		{
			float num = Math.Min(this.m_point1.X, this.m_point2.X);
			float num2 = Math.Min(this.m_point1.Y, this.m_point2.Y);
			float num3 = Math.Abs(this.m_point1.X - this.m_point2.X);
			float num4 = Math.Abs(this.m_point1.Y - this.m_point2.Y);
			bool flag = this.m_point2 == PointF.Empty;
			Rectangle result;
			if (flag)
			{
				result = Rectangle.Empty;
			}
			else
			{
				bool flag2 = num3 < 4f || num4 < 4f;
				if (flag2)
				{
					result = Rectangle.Empty;
				}
				else
				{
					result = new Rectangle((int)num, (int)num2, (int)num3, (int)num4);
				}
			}
			return result;
		}

		public RectangleF Selection(ICanvas canvas)
		{
			Rectangle screenrect = this.ScreenRect();
			bool isEmpty = screenrect.IsEmpty;
			RectangleF result;
			if (isEmpty)
			{
				result = RectangleF.Empty;
			}
			else
			{
				result = ScreenUtils.ToUnitNormalized(canvas, screenrect);
			}
			return result;
		}

		public bool AnyPoint()
		{
			return this.m_point1.X > this.m_point2.X;
		}

		private Color GetColor()
		{
			Color result;
			if (AnyPoint())
			{
				result = Color.Blue;
			}
			else
			{
				result = Color.Cyan;
			}
			return result;
		}
	}
}
