using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Canvas.CanvasInterfaces
{
	public interface IEditTool
	{
		bool SupportSelection
		{
			get;
		}

		IEditTool Clone();

		void SetHitObjects(UnitPoint mousepoint, List<IDrawObject> list);

		void OnMouseMove(ICanvas canvas, UnitPoint point);

		eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);

		void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);

		void OnKeyDown(ICanvas canvas, KeyEventArgs e);

		void Finished();

		void Undo();

		void Redo();
	}
}
