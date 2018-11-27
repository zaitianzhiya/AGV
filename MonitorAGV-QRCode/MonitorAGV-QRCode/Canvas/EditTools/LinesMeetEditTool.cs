using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Canvas.EditTools
{
	public class LinesMeetEditTool : IEditTool
	{
		private IEditToolOwner m_owner;

		private LinePoints m_l1Original = new LinePoints();

		private LinePoints m_l2Original = new LinePoints();

		private LinePoints m_l1NewPoint = new LinePoints();

		private LinePoints m_l2NewPoint = new LinePoints();

		public bool SupportSelection
		{
			get
			{
				return false;
			}
		}

		public LinesMeetEditTool(IEditToolOwner owner)
		{
			this.m_owner = owner;
			this.SetHint("选择第一个直线");
		}

		private void SetHint(string text)
		{
			try
			{
				bool flag = this.m_owner != null;
				if (flag)
				{
					bool flag2 = text.Length > 0;
					if (flag2)
					{
						this.m_owner.SetHint("提示:" + text);
					}
					else
					{
						this.m_owner.SetHint("");
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IEditTool Clone()
		{
			IEditTool result;
			try
			{
				LinesMeetEditTool linesMeetEditTool = new LinesMeetEditTool(this.m_owner);
				result = linesMeetEditTool;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void SetHitObjects(UnitPoint point, List<IDrawObject> list)
		{
		}

		public void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
		}

		public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			eDrawObjectMouseDownEnum result;
			try
			{
				List<IDrawObject> hitObjects = canvas.DataModel.GetHitObjects(canvas, point);
				LineTool lineTool = null;
				foreach (IDrawObject current in hitObjects)
				{
					bool flag = current is LineTool;
					if (flag)
					{
						lineTool = (current as LineTool);
						bool flag2 = lineTool.Type == LineType.PointLine;
						if (flag2)
						{
							result = eDrawObjectMouseDownEnum.Done;
							return result;
						}
						bool flag3 = lineTool != this.m_l1Original.Line;
						if (flag3)
						{
							break;
						}
					}
				}
				bool flag4 = lineTool == null;
				if (flag4)
				{
					bool flag5 = this.m_l1Original.Line == null;
					if (flag5)
					{
						this.SetHint("请选择第一个直线");
					}
					else
					{
						this.SetHint("请选择第二个直线");
					}
					result = eDrawObjectMouseDownEnum.Continue;
				}
				else
				{
					bool flag6 = this.m_l1Original.Line == null;
					if (flag6)
					{
						lineTool.Highlighted = true;
						this.m_l1Original.SetLine(lineTool);
						this.m_l1Original.MousePoint = point;
						this.SetHint("请选择第二个直线");
						result = eDrawObjectMouseDownEnum.Continue;
					}
					else
					{
						bool flag7 = this.m_l2Original.Line == null;
						if (flag7)
						{
							lineTool.Highlighted = true;
							this.m_l2Original.SetLine(lineTool);
							this.m_l2Original.MousePoint = point;
							UnitPoint unitPoint = HitUtil.LinesIntersectPoint(this.m_l1Original.Line.P1, this.m_l1Original.Line.P2, this.m_l2Original.Line.P1, this.m_l2Original.Line.P2);
							bool flag8 = unitPoint == UnitPoint.Empty;
							if (flag8)
							{
								UnitPoint unitPoint2 = HitUtil.FindApparentIntersectPoint(this.m_l1Original.Line.P1, this.m_l1Original.Line.P2, this.m_l2Original.Line.P1, this.m_l2Original.Line.P2);
								bool flag9 = unitPoint2 == UnitPoint.Empty;
								if (flag9)
								{
									result = eDrawObjectMouseDownEnum.Done;
								}
								else
								{
									this.m_l1Original.Line.ExtendLineToPoint(unitPoint2);
									this.m_l2Original.Line.ExtendLineToPoint(unitPoint2);
									this.m_l1NewPoint.SetLine(this.m_l1Original.Line);
									this.m_l2NewPoint.SetLine(this.m_l2Original.Line);
									canvas.DataModel.AfterEditObjects(this);
									result = eDrawObjectMouseDownEnum.Done;
								}
							}
							else
							{
								this.m_l1NewPoint.SetNewPoints(this.m_l1Original.Line, this.m_l1Original.MousePoint, unitPoint);
								this.m_l2NewPoint.SetNewPoints(this.m_l2Original.Line, this.m_l2Original.MousePoint, unitPoint);
								canvas.DataModel.AfterEditObjects(this);
								result = eDrawObjectMouseDownEnum.Done;
							}
						}
						else
						{
							result = eDrawObjectMouseDownEnum.Done;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
		}

		public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
		{
		}

		public void Finished()
		{
			try
			{
				this.SetHint("");
				bool flag = this.m_l1Original.Line != null;
				if (flag)
				{
					this.m_l1Original.Line.Highlighted = false;
				}
				bool flag2 = this.m_l2Original.Line != null;
				if (flag2)
				{
					this.m_l2Original.Line.Highlighted = false;
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
				this.m_l1Original.ResetLine();
				this.m_l2Original.ResetLine();
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
				this.m_l1NewPoint.ResetLine();
				this.m_l2NewPoint.ResetLine();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
