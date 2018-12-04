using System;
using System.Drawing;

namespace Canvas.CanvasInterfaces
{
	public interface ISnapPoint
	{
		IDrawObject Owner
		{
			get;
		}

		UnitPoint SnapPoint
		{
			get;
		}

		RectangleF BoundingRect
		{
			get;
		}

		void Draw(ICanvas canvas);
	}
}
