using System;
using System.Windows.Forms;

namespace Canvas.CanvasInterfaces
{
	public interface INodePoint
	{
		IDrawObject GetClone();

		IDrawObject GetOriginal();

		void Cancel();

		void Finish();

		void SetPosition(UnitPoint pos);

		void Undo();

		void Redo();

		void OnKeyDown(ICanvas canvas, KeyEventArgs e);
	}
}
