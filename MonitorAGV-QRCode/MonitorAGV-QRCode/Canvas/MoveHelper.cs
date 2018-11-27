using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
{
	public class MoveHelper
	{
		private List<IDrawObject> m_originals = new List<IDrawObject>();

		private List<IDrawObject> m_copies = new List<IDrawObject>();

		private UnitPoint m_originPoint = UnitPoint.Empty;

		private UnitPoint m_lastPoint = UnitPoint.Empty;

		private CanvasCtrller m_canvas;

		public UnitPoint OriginPoint
		{
			get
			{
				return m_originPoint;
			}
		}

		public UnitPoint LastPoint
		{
			get
			{
				return m_lastPoint;
			}
		}

		public IEnumerable<IDrawObject> Copies
		{
			get
			{
				return m_copies;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return m_copies.Count == 0;
			}
		}

		public MoveHelper(CanvasCtrller canvas)
		{
			m_canvas = canvas;
		}

		public bool HandleMouseMoveForMove(UnitPoint mouseunitpoint)
		{
			bool flag = m_originals.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				double x = mouseunitpoint.X - m_lastPoint.X;
				double y = mouseunitpoint.Y - m_lastPoint.Y;
				UnitPoint offset = new UnitPoint(x, y);
				m_lastPoint = mouseunitpoint;
				foreach (IDrawObject current in m_copies)
				{
					current.Move(offset);
				}
				m_canvas.DoInvalidate(true);
				result = true;
			}
			return result;
		}

		public void HandleCancelMove()
		{
			foreach (IDrawObject current in m_originals)
			{
				m_canvas.Model.AddSelectedObject(current);
			}
			m_originals.Clear();
			m_copies.Clear();
			m_canvas.DoInvalidate(true);
		}

		public void HandleMouseDownForMove(UnitPoint mouseunitpoint, ISnapPoint snappoint)
		{
			UnitPoint unitPoint = mouseunitpoint;
			if (snappoint != null)
			{
				unitPoint = snappoint.SnapPoint;
			}
			if (m_originals.Count == 0)
			{
				foreach (IDrawObject current in m_canvas.Model.SelectedObjects)
				{
					m_originals.Add(current);
					m_copies.Add(current.Clone());
				}
				m_canvas.Model.ClearSelectedObjects();
				m_originPoint = unitPoint;
				m_lastPoint = unitPoint;
			}
			else
			{
				double x = unitPoint.X - m_originPoint.X;
				double y = unitPoint.Y - m_originPoint.Y;
				UnitPoint offset = new UnitPoint(x, y);
				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					m_canvas.Model.CopyObjects(offset, m_originals);
				}
				else
				{
					m_canvas.Model.MoveObjects(offset, m_originals);
					foreach (IDrawObject current2 in m_originals)
					{
						m_canvas.Model.AddSelectedObject(current2);
					}
				}
				m_originals.Clear();
				m_copies.Clear();
			}
			m_canvas.DoInvalidate(true);
		}

		public void DrawObjects(ICanvas canvas, RectangleF r)
		{
            if (m_copies.Count != 0)
			{
				canvas.Graphics.DrawLine(Pens.Wheat, canvas.ToScreen(OriginPoint), canvas.ToScreen(LastPoint));
				foreach (IDrawObject current in Copies)
				{
					current.Draw(canvas, r);
				}
			}
		}
	}
}
