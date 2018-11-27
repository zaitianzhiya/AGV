using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
{
	public class NodeMoveHelper
	{
		private List<INodePoint> m_nodes = new List<INodePoint>();

		private UnitPoint m_originPoint = UnitPoint.Empty;

		private UnitPoint m_lastPoint = UnitPoint.Empty;

		private CanvasWrapper m_canvas;

		public bool IsEmpty
		{
			get
			{
				return this.m_nodes.Count == 0;
			}
		}

		public NodeMoveHelper(CanvasWrapper canvas)
		{
			this.m_canvas = canvas;
		}

		public RectangleF HandleMouseMoveForNode(UnitPoint mouseunitpoint)
		{
			RectangleF rectangleF = RectangleF.Empty;
			bool flag = this.m_nodes.Count == 0;
			RectangleF result;
			if (flag)
			{
				result = rectangleF;
			}
			else
			{
				rectangleF = new RectangleF(this.m_originPoint.Point, new Size(0, 0));
				rectangleF = RectangleF.Union(rectangleF, new RectangleF(mouseunitpoint.Point, new SizeF(0f, 0f)));
				bool flag2 = this.m_lastPoint != UnitPoint.Empty;
				if (flag2)
				{
					rectangleF = RectangleF.Union(rectangleF, new RectangleF(this.m_lastPoint.Point, new SizeF(0f, 0f)));
				}
				this.m_lastPoint = mouseunitpoint;
				foreach (INodePoint current in this.m_nodes)
				{
					bool flag3 = rectangleF == RectangleF.Empty;
					if (flag3)
					{
						rectangleF = current.GetClone().GetBoundingRect(this.m_canvas);
					}
					else
					{
						rectangleF = RectangleF.Union(rectangleF, current.GetClone().GetBoundingRect(this.m_canvas));
					}
					current.SetPosition(mouseunitpoint);
				}
				result = rectangleF;
			}
			return result;
		}

		public bool HandleMouseDown(UnitPoint mouseunitpoint, ref bool handled)
		{
			handled = false;
			bool flag = this.m_nodes.Count == 0;
			bool result;
			if (flag)
			{
				bool flag2 = this.m_canvas.Model.SelectedCount > 0;
				if (flag2)
				{
					foreach (IDrawObject current in this.m_canvas.Model.SelectedObjects)
					{
						INodePoint nodePoint = current.NodePoint(this.m_canvas, mouseunitpoint);
						bool flag3 = nodePoint != null;
						if (flag3)
						{
							this.m_nodes.Add(nodePoint);
						}
					}
				}
				handled = (this.m_nodes.Count > 0);
				bool flag4 = handled;
				if (flag4)
				{
					this.m_originPoint = mouseunitpoint;
				}
				result = handled;
			}
			else
			{
				this.m_canvas.Model.MoveNodes(mouseunitpoint, this.m_nodes);
				this.m_nodes.Clear();
				handled = true;
				this.m_canvas.CanvasCtrl.DoInvalidate(true);
				result = handled;
			}
			return result;
		}

		public void HandleCancelMove()
		{
			foreach (INodePoint current in this.m_nodes)
			{
				current.Cancel();
			}
			this.m_nodes.Clear();
		}

		public void DrawOriginalObjects(ICanvas canvas, RectangleF r)
		{
			foreach (INodePoint current in this.m_nodes)
			{
				current.GetOriginal().Draw(canvas, r);
			}
		}

		public void DrawObjects(ICanvas canvas, RectangleF r)
		{
			bool flag = this.m_nodes.Count == 0;
			if (!flag)
			{
				foreach (INodePoint current in this.m_nodes)
				{
					current.GetClone().Draw(canvas, r);
				}
			}
		}

		public void OnKeyDown(ICanvas canvas, KeyEventArgs keyevent)
		{
			foreach (INodePoint current in this.m_nodes)
			{
				current.OnKeyDown(canvas, keyevent);
				bool handled = keyevent.Handled;
				if (handled)
				{
					break;
				}
				IDrawObject clone = current.GetClone();
				bool flag = clone == null;
				if (!flag)
				{
					clone.OnKeyDown(canvas, keyevent);
				}
			}
		}
	}
}
