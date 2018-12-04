using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;

namespace Canvas
{
	internal class EditCommandMove : EditCommandBase
	{
		private List<IDrawObject> m_objects = new List<IDrawObject>();

		private UnitPoint m_offset;

		public EditCommandMove(UnitPoint offset, IEnumerable<IDrawObject> objects)
		{
			this.m_objects = new List<IDrawObject>(objects);
			this.m_offset = offset;
		}

		public override bool DoUndo(IModel data)
		{
			foreach (IDrawObject current in this.m_objects)
			{
				UnitPoint offset = new UnitPoint(-this.m_offset.X, -this.m_offset.Y);
				current.Move(offset);
			}
			return true;
		}

		public override bool DoRedo(IModel data)
		{
			foreach (IDrawObject current in this.m_objects)
			{
				current.Move(this.m_offset);
			}
			return true;
		}
	}
}
