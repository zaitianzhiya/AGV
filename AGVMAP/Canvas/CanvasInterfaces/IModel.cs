using System;
using System.Collections.Generic;
using System.Drawing;

namespace Canvas.CanvasInterfaces
{
	public interface IModel
	{
		float Zoom
		{
			get;
			set;
		}

		ICanvasLayer BackgroundLayer
		{
			get;
		}

		ICanvasLayer GridLayer
		{
			get;
		}

		ICanvasLayer[] Layers
		{
			get;
		}

		ICanvasLayer ActiveLayer
		{
			get;
			set;
		}

		IEnumerable<IDrawObject> SelectedObjects
		{
			get;
		}

		int SelectedCount
		{
			get;
		}

		ICanvasLayer GetLayer(string id);

		IDrawObject CreateObject(string type, UnitPoint point, ISnapPoint snappoint);

		void AddObject(ICanvasLayer layer, IDrawObject drawobject);

		void DeleteObjects(IEnumerable<IDrawObject> objects);

		void MoveObjects(UnitPoint offset, IEnumerable<IDrawObject> objects);

		void CopyObjects(UnitPoint offset, IEnumerable<IDrawObject> objects);

		void MoveNodes(UnitPoint position, IEnumerable<INodePoint> nodes);

		IEditTool GetEditTool(string id);

		void AfterEditObjects(IEditTool edittool);

		List<IDrawObject> GetHitObjects(ICanvas canvas, RectangleF selection, bool anyPoint);

		List<IDrawObject> GetHitObjects(ICanvas canvas, UnitPoint point);

		bool IsSelected(IDrawObject drawobject);

		void AddSelectedObject(IDrawObject drawobject);

		void RemoveSelectedObject(IDrawObject drawobject);

		void ClearSelectedObjects();

		ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, Type[] runningsnaptypes, Type usersnaptype);
	}
}
