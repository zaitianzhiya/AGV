using Canvas.CanvasInterfaces;
using System;
using System.Drawing;

namespace Canvas.CanvasCtrl
{
	public class CanvasWrapper : ICanvas
	{
		private CanvasCtrller m_canvas;

		private Graphics m_graphics;

		private Rectangle m_rect;

		public IModel Model
		{
			get
			{
				return this.m_canvas.Model;
			}
		}

		public IModel DataModel
		{
			get
			{
				return this.m_canvas.Model;
			}
		}

		public CanvasCtrller CanvasCtrl
		{
			get
			{
				return this.m_canvas;
			}
		}

		public Graphics Graphics
		{
			get
			{
				return this.m_graphics;
			}
		}

		public Rectangle ClientRectangle
		{
			get
			{
				return this.m_rect;
			}
			set
			{
				this.m_rect = value;
			}
		}

		public IDrawObject CurrentObject
		{
			get
			{
				return this.m_canvas.NewObject;
			}
		}

		public UnitPoint ScreenTopLeftToUnitPoint()
		{
			return this.m_canvas.ScreenTopLeftToUnitPoint();
		}

		public UnitPoint ScreenBottomRightToUnitPoint()
		{
			return this.m_canvas.ScreenBottomRightToUnitPoint();
		}

		public CanvasWrapper(CanvasCtrller canvas)
		{
			this.m_canvas = canvas;
			this.m_graphics = null;
			this.m_rect = default(Rectangle);
		}

		public CanvasWrapper(CanvasCtrller canvas, Graphics graphics, Rectangle clientrect)
		{
			this.m_canvas = canvas;
			this.m_graphics = graphics;
			this.m_rect = clientrect;
		}

		public PointF ToScreen(UnitPoint unitpoint)
		{
			return this.m_canvas.ToScreen(unitpoint);
		}

		public float ToScreen(double unitvalue)
		{
			return this.m_canvas.ToScreen(unitvalue);
		}

		public double ToUnit(float screenvalue)
		{
			return this.m_canvas.ToUnit(screenvalue);
		}

		public UnitPoint ToUnit(PointF screenpoint)
		{
			return this.m_canvas.ToUnit(screenpoint);
		}

		public Pen CreatePen(Color color)
		{
			return this.m_canvas.CreatePen(color, 0.05f);
		}

		public void Invalidate()
		{
			this.m_canvas.DoInvalidate(false);
		}

		public void Dispose()
		{
			this.m_graphics = null;
		}

		public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2)
		{
			try
			{
				this.m_canvas.DrawLine(canvas, pen, p1, p2);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawLandMark(ICanvas canvas, Brush pen, string code, UnitPoint Point)
		{
			try
			{
				this.m_canvas.DrawLandMark(canvas, pen, code, Point);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawImge(ICanvas canvas, Pen pen, UnitPoint Location, float Widht, float Hight, Image img, string values)
		{
			try
			{
				this.m_canvas.DrawImge(canvas, pen, Location, Widht, Hight, img, values);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawTxt(ICanvas canvas, string code, UnitPoint Point, int FontSize, Color fontColor)
		{
			try
			{
				this.m_canvas.DrawTxt(canvas, code, Point, FontSize, fontColor);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawBizer(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, UnitPoint p3, UnitPoint p4)
		{
			try
			{
				this.m_canvas.DrawBizer(canvas, pen, p1, p2, p3, p4);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawStorage(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
		{
			try
			{
				this.m_canvas.DrawStorage(canvas, Pen, code, Point);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawBtnBox(ICanvas canvas, float Radius, UnitPoint Point, bool Selected)
		{
			try
			{
				this.m_canvas.DrawBtnBox(canvas, Radius, Point, Selected);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code)
		{
			this.m_canvas.DrawAGV(canvas, pen, p, Code);
		}
	}
}
