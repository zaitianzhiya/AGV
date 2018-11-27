using Canvas.CanvasInterfaces;
using System;
using System.Drawing;

namespace Canvas.DrawTools
{
	public class SnapPointBase : ISnapPoint
	{
		protected UnitPoint m_snappoint;

		protected RectangleF m_boundingRect;

		protected IDrawObject m_owner;

		public IDrawObject Owner
		{
			get
			{
				return this.m_owner;
			}
		}

		public virtual UnitPoint SnapPoint
		{
			get
			{
				return this.m_snappoint;
			}
		}

		public virtual RectangleF BoundingRect
		{
			get
			{
				return this.m_boundingRect;
			}
		}

		public SnapPointBase(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
		{
			this.m_owner = owner;
			this.m_snappoint = snappoint;
			float num = (float)canvas.ToUnit(14f);
			this.m_boundingRect.X = (float)(snappoint.X - (double)(num / 2f));
			this.m_boundingRect.Y = (float)(snappoint.Y - (double)(num / 2f));
			this.m_boundingRect.Width = num;
			this.m_boundingRect.Height = num;
		}

		public virtual void Draw(ICanvas canvas)
		{
		}

		protected void DrawPoint(ICanvas canvas, Pen pen, Brush fillBrush)
		{
			try
			{
				Rectangle rect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(canvas, this.m_boundingRect));
				canvas.Graphics.DrawRectangle(pen, rect);
				int num = rect.X;
				rect.X = num + 1;
				num = rect.Y;
				rect.Y = num + 1;
				num = rect.Width;
				rect.Width = num - 1;
				num = rect.Height;
				rect.Height = num - 1;
				bool flag = fillBrush != null;
				if (flag)
				{
					canvas.Graphics.FillRectangle(fillBrush, rect);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
