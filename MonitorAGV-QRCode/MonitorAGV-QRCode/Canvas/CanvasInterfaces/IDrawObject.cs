using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas.CanvasInterfaces
{
	public interface IDrawObject
	{
		string Id
		{
			get;
		}

        int MapNo { get; set; }

		UnitPoint RepeatStartingPoint
		{
			get;
		}

		IDrawObject Clone();

		bool PointInObject(ICanvas canvas, UnitPoint point);

		bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint);

		void Draw(ICanvas canvas, RectangleF unitrect);

        void Draw(ICanvas canvas, RectangleF unitrect, Graphics g);

		RectangleF GetBoundingRect(ICanvas canvas);

		void OnMouseMove(ICanvas canvas, UnitPoint point);

		eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);

		void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);

		void OnKeyDown(ICanvas canvas, KeyEventArgs e);

		INodePoint NodePoint(ICanvas canvas, UnitPoint point);

		ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype);

		void Move(UnitPoint offset);

		string GetInfoAsString();
	}
}
