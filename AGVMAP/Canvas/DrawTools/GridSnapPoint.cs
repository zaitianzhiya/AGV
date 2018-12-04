using Canvas.CanvasInterfaces;
using System;
using System.Drawing;

namespace Canvas.DrawTools
{
	public class GridSnapPoint : SnapPointBase
	{
		public GridSnapPoint(ICanvas canvas, UnitPoint snappoint) : base(canvas, null, snappoint)
		{
		}

		public override void Draw(ICanvas canvas)
		{
			base.DrawPoint(canvas, Pens.Gray, null);
		}
	}
}
