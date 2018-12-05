using System;
using System.Drawing;

namespace Canvas.CanvasInterfaces
{
	public interface ICanvas
	{
		IModel DataModel
		{
			get;
		}

		IDrawObject CurrentObject
		{
			get;
		}

		Rectangle ClientRectangle
		{
			get;
		}

		Graphics Graphics
		{
			get;
		}

		UnitPoint ScreenTopLeftToUnitPoint();

		UnitPoint ScreenBottomRightToUnitPoint();

		PointF ToScreen(UnitPoint unitpoint);

		float ToScreen(double unitvalue);

		double ToUnit(float screenvalue);

		UnitPoint ToUnit(PointF screenpoint);

		void Invalidate();

		Pen CreatePen(Color color);

		void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2);

	    void DrawForbid(ICanvas canvas, Pen pen, UnitPoint p);

        void DrawCharge(ICanvas canvas, Pen pen, UnitPoint p);

        void DrawImage(ICanvas canvas, UnitPoint p);

	    void DrawTxt(ICanvas canvas, string code, UnitPoint Point);

	    void DrawAgv(ICanvas canvas, string no, Color color, float angel, UnitPoint Point);

        void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2,Graphics g);

        void DrawForbid(ICanvas canvas, Pen pen, UnitPoint p, Graphics g);

        void DrawCharge(ICanvas canvas, Pen pen, UnitPoint p, Graphics g);

        void DrawImage(ICanvas canvas, UnitPoint p, Graphics g);

        void DrawTxt(ICanvas canvas, string code, UnitPoint Point, Graphics g);

        void DrawAgv(ICanvas canvas, string no, Color color, float angel, UnitPoint Point, Graphics g);
	}
}
