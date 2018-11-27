using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.ComponentModel;

namespace Canvas.DrawTools
{
	public class LineEdit : LineTool, IObjectEditInstance
	{
		protected PerpendicularSnapPoint m_perSnap;

		protected TangentSnapPoint m_tanSnap;

		protected bool m_tanReverse = false;

		protected bool m_singleLineSegment = true;

		protected new LineType Type;

		[Browsable(false)]
		public override string Id
		{
			get
			{
				bool singleLineSegment = this.m_singleLineSegment;
				string result;
				if (singleLineSegment)
				{
					result = "Line";
				}
				else
				{
					result = "Lines";
				}
				return result;
			}
		}

		public LineEdit(bool singleLine)
		{
			this.m_singleLineSegment = singleLine;
		}

		public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
		{
			base.InitializeFromModel(point, layer, snap);
			this.m_perSnap = (snap as PerpendicularSnapPoint);
			this.m_tanSnap = (snap as TangentSnapPoint);
		}

		public override void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
			base.OnMouseMove(canvas, point);
		}

		public override eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			bool flag = this.m_perSnap != null || this.m_tanSnap != null;
			eDrawObjectMouseDownEnum result;
			if (flag)
			{
				bool flag2 = snappoint != null;
				if (flag2)
				{
					point = snappoint.SnapPoint;
				}
				this.OnMouseMove(canvas, point);
				bool singleLineSegment = this.m_singleLineSegment;
				if (singleLineSegment)
				{
					result = eDrawObjectMouseDownEnum.Done;
				}
				else
				{
					result = eDrawObjectMouseDownEnum.DoneRepeat;
				}
			}
			else
			{
				eDrawObjectMouseDownEnum eDrawObjectMouseDownEnum = base.OnMouseDown(canvas, point, snappoint);
				bool singleLineSegment2 = this.m_singleLineSegment;
				if (singleLineSegment2)
				{
					result = eDrawObjectMouseDownEnum.Done;
				}
				else
				{
					result = eDrawObjectMouseDownEnum.DoneRepeat;
				}
			}
			return result;
		}

		protected virtual void MouseMovePerpendicular(ICanvas canvas, UnitPoint point)
		{
			bool flag = this.m_perSnap.Owner is LineTool;
			if (flag)
			{
				LineTool lineTool = this.m_perSnap.Owner as LineTool;
				this.m_p1 = HitUtil.NearestPointOnLine(lineTool.P1, lineTool.P2, point, true);
				this.m_p2 = point;
			}
		}

		public void Copy(LineEdit acopy)
		{
			base.Copy(acopy);
			this.m_perSnap = acopy.m_perSnap;
			this.m_tanSnap = acopy.m_tanSnap;
			this.m_tanReverse = acopy.m_tanReverse;
			this.m_singleLineSegment = acopy.m_singleLineSegment;
		}

		public override IDrawObject Clone()
		{
			LineEdit lineEdit = new LineEdit(false);
			lineEdit.Copy(this);
			return lineEdit;
		}

		public IDrawObject GetDrawObject()
		{
			return new LineTool(base.P1, base.P2, base.Width, base.Color);
		}
	}
}
