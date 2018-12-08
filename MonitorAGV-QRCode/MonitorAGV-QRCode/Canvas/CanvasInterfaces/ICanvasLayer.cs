using System;
using System.Collections.Generic;
using System.Drawing;

namespace Canvas.CanvasInterfaces
{
	public interface ICanvasLayer
	{
		string Id
		{
			get;
		}

		IEnumerable<IDrawObject> Objects
		{
			get;
		}

		bool Enabled
		{
			get;
			set;
		}

		bool Visible
		{
			get;
		}

		void Draw(ICanvas canvas, RectangleF unitrect);

		ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj);
	}
}
