using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;

namespace Canvas
{
	internal class EditCommandAdd : EditCommandBase
	{
		private List<IDrawObject> m_objects = null;

		private IDrawObject m_object;

		private ICanvasLayer m_layer;

		public EditCommandAdd(ICanvasLayer layer, IDrawObject obj)
		{
			this.m_object = obj;
			this.m_layer = layer;
		}

		public EditCommandAdd(ICanvasLayer layer, List<IDrawObject> objects)
		{
			this.m_objects = new List<IDrawObject>(objects);
			this.m_layer = layer;
		}

		public override bool DoUndo(IModel data)
		{
			bool flag = this.m_object != null;
			if (flag)
			{
				data.DeleteObjects(new IDrawObject[]
				{
					this.m_object
				});
			}
			bool flag2 = this.m_objects != null;
			if (flag2)
			{
				data.DeleteObjects(this.m_objects);
			}
			return true;
		}

		public override bool DoRedo(IModel data)
		{
			bool flag = this.m_object != null;
			if (flag)
			{
				data.AddObject(this.m_layer, this.m_object);
			}
			bool flag2 = this.m_objects != null;
			if (flag2)
			{
				foreach (IDrawObject current in this.m_objects)
				{
					data.AddObject(this.m_layer, current);
				}
			}
			return true;
		}
	}
}
