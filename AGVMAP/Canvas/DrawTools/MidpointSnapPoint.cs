using Canvas.CanvasInterfaces;
using System;
using System.Drawing;

namespace Canvas.DrawTools
{
	public class MidpointSnapPoint : SnapPointBase
	{
		public MidpointSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint) : base(canvas, owner, snappoint)
		{
		}

		public override void Draw(ICanvas canvas)
		{
			base.DrawPoint(canvas, Pens.White, Brushes.YellowGreen);
		}
	}
}
