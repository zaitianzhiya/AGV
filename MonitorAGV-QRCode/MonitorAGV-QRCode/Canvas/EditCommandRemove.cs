using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;

namespace Canvas
{
	internal class EditCommandRemove : EditCommandBase
	{
		private Dictionary<ICanvasLayer, List<IDrawObject>> m_objects = new Dictionary<ICanvasLayer, List<IDrawObject>>();

		public void AddLayerObjects(ICanvasLayer layer, List<IDrawObject> objects)
		{
			this.m_objects.Add(layer, objects);
		}

		public override bool DoUndo(IModel data)
		{
			foreach (ICanvasLayer current in this.m_objects.Keys)
			{
				foreach (IDrawObject current2 in this.m_objects[current])
				{
					data.AddObject(current, current2);
				}
			}
			return true;
		}

		public override bool DoRedo(IModel data)
		{
			foreach (ICanvasLayer current in this.m_objects.Keys)
			{
				data.DeleteObjects(this.m_objects[current]);
			}
			return true;
		}
	}
}
