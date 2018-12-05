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

        public void DrawForbid(ICanvas canvas, Pen pen, UnitPoint p)
        {
            m_canvas.DrawForbid(canvas, pen, p);
        }

	    public void DrawCharge(ICanvas canvas, Pen pen, UnitPoint p)
	    {
            m_canvas.DrawCharge(canvas, pen, p);
	    }

        public void DrawImage(ICanvas canvas, UnitPoint p)
        {
            m_canvas.DrawImage(canvas, p);
        }

        public void DrawTxt(ICanvas canvas, string code, UnitPoint Point)
        {
            m_canvas.DrawTxt(canvas, code,Point);
        }

        public void DrawAgv(ICanvas canvas, string no, Color color, float angel, UnitPoint Point)
        {
            m_canvas.DrawAgv(canvas, no, color, angel, Point);
        }


        public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, Graphics g)
        {
            m_canvas.DrawLine(canvas, pen, p1, p2, g);
        }

        public void DrawForbid(ICanvas canvas, Pen pen, UnitPoint p, Graphics g)
        {
            m_canvas.DrawForbid(canvas,pen,p,g);
        }

        public void DrawCharge(ICanvas canvas, Pen pen, UnitPoint p, Graphics g)
        {
            m_canvas.DrawCharge(canvas,pen,p,g);
        }

        public void DrawImage(ICanvas canvas, UnitPoint p, Graphics g)
        {
            m_canvas.DrawImage(canvas,p,g);
        }

        public void DrawTxt(ICanvas canvas, string code, UnitPoint Point, Graphics g)
        {
            m_canvas.DrawTxt(canvas,code,Point,g);
        }

        public void DrawAgv(ICanvas canvas, string no, Color color, float angel, UnitPoint Point, Graphics g)
        {
            m_canvas.DrawAgv(canvas,no,color,angel,Point,g);
        }
    }
}
