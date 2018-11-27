using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;

namespace Canvas
{
	internal class EditCommandNodeMove : EditCommandBase
	{
		private List<INodePoint> m_objects = new List<INodePoint>();

		public EditCommandNodeMove(IEnumerable<INodePoint> objects)
		{
			this.m_objects = new List<INodePoint>(objects);
		}

		public override bool DoUndo(IModel data)
		{
			foreach (INodePoint current in this.m_objects)
			{
				current.Undo();
			}
			return true;
		}

		public override bool DoRedo(IModel data)
		{
			foreach (INodePoint current in this.m_objects)
			{
				current.Redo();
			}
			return true;
		}
	}
}
