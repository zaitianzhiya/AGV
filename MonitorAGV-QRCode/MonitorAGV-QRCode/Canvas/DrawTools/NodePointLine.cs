using Canvas.CanvasInterfaces;
using System;
using System.Windows.Forms;

namespace Canvas.DrawTools
{
	public class NodePointLine : INodePoint
	{
		public enum ePoint
		{
			P1,
			P2
		}

		private static bool m_angleLocked = false;

		private LineTool m_owner;

		private LineTool m_clone;

		private UnitPoint m_originalPoint;

		private UnitPoint m_endPoint;

		private NodePointLine.ePoint m_pointId;

		public NodePointLine(LineTool owner, NodePointLine.ePoint id)
		{
			try
			{
				this.m_owner = owner;
				this.m_clone = (this.m_owner.Clone() as LineTool);
				this.m_pointId = id;
				this.m_originalPoint = this.GetPoint(this.m_pointId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected UnitPoint GetPoint(NodePointLine.ePoint pointid)
		{
			UnitPoint result;
			try
			{
				bool flag = pointid == NodePointLine.ePoint.P1;
				if (flag)
				{
					result = this.m_clone.P1;
				}
				else
				{
					bool flag2 = pointid == NodePointLine.ePoint.P2;
					if (flag2)
					{
						result = this.m_clone.P2;
					}
					else
					{
						result = this.m_owner.P1;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public IDrawObject GetClone()
		{
			return this.m_clone;
		}

		public IDrawObject GetOriginal()
		{
			return this.m_owner;
		}

		public void SetPosition(UnitPoint pos)
		{
			try
			{
				bool flag = Control.ModifierKeys == Keys.Control;
				if (flag)
				{
					pos = HitUtil.OrthoPointD(this.OtherPoint(this.m_pointId), pos, 45.0);
				}
				bool flag2 = NodePointLine.m_angleLocked || Control.ModifierKeys == (Keys.Shift | Keys.Control);
				if (flag2)
				{
					pos = HitUtil.NearestPointOnLine(this.m_owner.P1, this.m_owner.P2, pos, true);
				}
				this.SetPoint(this.m_pointId, pos, this.m_clone);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected UnitPoint OtherPoint(NodePointLine.ePoint currentpointid)
		{
			UnitPoint point;
			try
			{
				bool flag = currentpointid == NodePointLine.ePoint.P1;
				if (flag)
				{
					point = this.GetPoint(NodePointLine.ePoint.P2);
				}
				else
				{
					point = this.GetPoint(NodePointLine.ePoint.P1);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return point;
		}

		protected void SetPoint(NodePointLine.ePoint pointid, UnitPoint point, LineTool line)
		{
			try
			{
				bool flag = pointid == NodePointLine.ePoint.P1;
				if (flag)
				{
					line.P1 = point;
				}
				bool flag2 = pointid == NodePointLine.ePoint.P2;
				if (flag2)
				{
					line.P2 = point;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Finish()
		{
			try
			{
				this.m_endPoint = this.GetPoint(this.m_pointId);
				this.m_owner.P1 = this.m_clone.P1;
				this.m_owner.P2 = this.m_clone.P2;
				this.m_clone = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Cancel()
		{
		}

		public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
		{
			try
			{
				bool flag = e.KeyCode == Keys.L;
				if (flag)
				{
					NodePointLine.m_angleLocked = !NodePointLine.m_angleLocked;
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Undo()
		{
			try
			{
				this.SetPoint(this.m_pointId, this.m_originalPoint, this.m_owner);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Redo()
		{
			try
			{
				this.SetPoint(this.m_pointId, this.m_endPoint, this.m_owner);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
