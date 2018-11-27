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
				return m_canvas.Model;
			}
		}

		public IModel DataModel
		{
			get
			{
				return m_canvas.Model;
			}
		}

		public CanvasCtrller CanvasCtrl
		{
			get
			{
				return m_canvas;
			}
		}

		public Graphics Graphics
		{
			get
			{
				return m_graphics;
			}
		}

		public Rectangle ClientRectangle
		{
			get
			{
				return m_rect;
			}
			set
			{
				m_rect = value;
			}
		}

		public IDrawObject CurrentObject
		{
			get
			{
				return m_canvas.NewObject;
			}
		}

		public UnitPoint ScreenTopLeftToUnitPoint()
		{
			return m_canvas.ScreenTopLeftToUnitPoint();
		}

		public UnitPoint ScreenBottomRightToUnitPoint()
		{
			return m_canvas.ScreenBottomRightToUnitPoint();
		}

		public CanvasWrapper(CanvasCtrller canvas)
		{
			m_canvas = canvas;
			m_graphics = null;
			m_rect = default(Rectangle);
		}

		public CanvasWrapper(CanvasCtrller canvas, Graphics graphics, Rectangle clientrect)
		{
			m_canvas = canvas;
			m_graphics = graphics;
			m_rect = clientrect;
		}

		public PointF ToScreen(UnitPoint unitpoint)
		{
			return m_canvas.ToScreen(unitpoint);
		}

		public float ToScreen(double unitvalue)
		{
			return m_canvas.ToScreen(unitvalue);
		}

		public double ToUnit(float screenvalue)
		{
			return m_canvas.ToUnit(screenvalue);
		}

		public UnitPoint ToUnit(PointF screenpoint)
		{
			return m_canvas.ToUnit(screenpoint);
		}

		public Pen CreatePen(Color color)
		{
			return m_canvas.CreatePen(color, 0.05f);
		}

		public void Invalidate()
		{
			m_canvas.DoInvalidate(false);
		}

		public void Dispose()
		{
			m_graphics = null;
		}

		public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2)
		{
			try
			{
				m_canvas.DrawLine(canvas, pen, p1, p2);
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
				m_canvas.DrawLandMark(canvas, pen, code, Point);
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
				m_canvas.DrawImge(canvas, pen, Location, Widht, Hight, img, values);
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
				m_canvas.DrawTxt(canvas, code, Point, FontSize, fontColor);
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
				m_canvas.DrawBizer(canvas, pen, p1, p2, p3, p4);
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
				m_canvas.DrawStorage(canvas, Pen, code, Point);
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
				m_canvas.DrawBtnBox(canvas, Radius, Point, Selected);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code)
		{
			m_canvas.DrawAGV(canvas, pen, p, Code);
		}
	}
}
