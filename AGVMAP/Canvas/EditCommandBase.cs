using Canvas.CanvasInterfaces;
using System;

namespace Canvas
{
	internal class EditCommandBase
	{
		public virtual bool DoUndo(IModel data)
		{
			return false;
		}

		public virtual bool DoRedo(IModel data)
		{
			return false;
		}
	}
}
