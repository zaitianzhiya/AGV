using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Canvas.EditTools
{
	public class LineShrinkExtendEditTool : IEditTool
	{
		private IEditToolOwner m_owner;

		private Dictionary<LineTool, LinePoints> m_originalLines = new Dictionary<LineTool, LinePoints>();

		private Dictionary<LineTool, LinePoints> m_modifiedLines = new Dictionary<LineTool, LinePoints>();

		public bool SupportSelection
		{
			get
			{
				return true;
			}
		}

		public LineShrinkExtendEditTool(IEditToolOwner owner)
		{
			this.m_owner = owner;
			this.SetHint("选择一条直线延伸");
		}

		private void SetHint(string text)
		{
			bool flag = this.m_owner != null;
			if (flag)
			{
				bool flag2 = text.Length > 0;
				if (flag2)
				{
					this.m_owner.SetHint("延长直线: " + text);
				}
				else
				{
					this.m_owner.SetHint("");
				}
			}
		}

		public IEditTool Clone()
		{
			return new LineShrinkExtendEditTool(this.m_owner);
		}

		public void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
		}

		private void ClearAll()
		{
			try
			{
				foreach (LinePoints current in this.m_originalLines.Values)
				{
					current.Line.Highlighted = false;
				}
				this.m_originalLines.Clear();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AddLine(UnitPoint point, LineTool line)
		{
			try
			{
				bool flag = !this.m_originalLines.ContainsKey(line);
				if (flag)
				{
					line.Highlighted = true;
					LinePoints linePoints = new LinePoints();
					linePoints.SetLine(line);
					linePoints.MousePoint = point;
					this.m_originalLines.Add(line, linePoints);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void RemoveLine(LineTool line)
		{
			try
			{
				bool flag = this.m_originalLines.ContainsKey(line);
				if (flag)
				{
					this.m_originalLines[line].Line.Highlighted = false;
					this.m_originalLines.Remove(line);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void SetHitObjects(UnitPoint point, List<IDrawObject> list)
		{
			try
			{
				bool flag = list == null;
				if (!flag)
				{
					List<LineTool> lines = this.GetLines(list);
					bool flag2 = lines.Count == 0;
					if (!flag2)
					{
						bool flag3 = Control.ModifierKeys == Keys.Shift;
						bool flag4 = Control.ModifierKeys == Keys.Control;
						bool flag5 = !flag3 && !flag4;
						if (flag5)
						{
							this.ClearAll();
						}
						bool flag6 = !flag4;
						if (flag6)
						{
							foreach (LineTool current in lines)
							{
								this.AddLine(point, current);
							}
						}
						bool flag7 = flag4;
						if (flag7)
						{
							foreach (LineTool current2 in lines)
							{
								bool flag8 = this.m_originalLines.ContainsKey(current2);
								if (flag8)
								{
									this.RemoveLine(current2);
								}
								else
								{
									this.AddLine(point, current2);
								}
							}
						}
						this.SetSelectHint();
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void SetSelectHint()
		{
			bool flag = this.m_originalLines.Count == 0;
			if (flag)
			{
				this.SetHint("选择延伸的直线");
			}
			else
			{
				this.SetHint("Control+单击可以多选");
			}
		}

		private List<LineTool> GetLines(List<IDrawObject> objs)
		{
			List<LineTool> result;
			try
			{
				List<LineTool> list = new List<LineTool>();
				foreach (IDrawObject current in objs)
				{
					bool flag = current is LineTool;
					if (flag)
					{
						list.Add((LineTool)current);
					}
				}
				result = list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			eDrawObjectMouseDownEnum result;
			try
			{
				List<IDrawObject> hitObjects = canvas.DataModel.GetHitObjects(canvas, point);
				List<LineTool> lines = this.GetLines(hitObjects);
				bool flag = this.m_originalLines.Count == 0 || Control.ModifierKeys == Keys.Shift;
				if (flag)
				{
					foreach (LineTool current in lines)
					{
						this.AddLine(point, current);
					}
					this.SetSelectHint();
					result = eDrawObjectMouseDownEnum.Continue;
				}
				else
				{
					bool flag2 = this.m_originalLines.Count == 0 || Control.ModifierKeys == Keys.Control;
					if (flag2)
					{
						foreach (LineTool current2 in lines)
						{
							bool flag3 = this.m_originalLines.ContainsKey(current2);
							if (flag3)
							{
								this.RemoveLine(current2);
							}
							else
							{
								this.AddLine(point, current2);
							}
						}
						this.SetSelectHint();
						result = eDrawObjectMouseDownEnum.Continue;
					}
					else
					{
						bool flag4 = hitObjects.Count == 0;
						if (flag4)
						{
							result = eDrawObjectMouseDownEnum.Continue;
						}
						else
						{
							bool flag5 = hitObjects[0] is LineTool;
							if (flag5)
							{
								LineTool lineTool = (LineTool)hitObjects[0];
								bool flag6 = lineTool.Type == LineType.PointLine;
								if (flag6)
								{
									result = eDrawObjectMouseDownEnum.Done;
								}
								else
								{
									bool flag7 = false;
									foreach (LinePoints current3 in this.m_originalLines.Values)
									{
										UnitPoint unitPoint = HitUtil.LinesIntersectPoint(lineTool.P1, lineTool.P2, current3.Line.P1, current3.Line.P2);
										bool flag8 = unitPoint != UnitPoint.Empty;
										if (flag8)
										{
											LinePoints linePoints = new LinePoints();
											linePoints.SetLine(current3.Line);
											linePoints.MousePoint = current3.MousePoint;
											this.m_modifiedLines.Add(linePoints.Line, linePoints);
											linePoints.SetNewPoints(linePoints.Line, linePoints.MousePoint, unitPoint);
											flag7 = true;
										}
										else
										{
											bool flag9 = unitPoint == UnitPoint.Empty;
											if (flag9)
											{
												UnitPoint unitPoint2 = HitUtil.FindApparentIntersectPoint(lineTool.P1, lineTool.P2, current3.Line.P1, current3.Line.P2, false, true);
												bool flag10 = unitPoint2 == UnitPoint.Empty;
												if (!flag10)
												{
													flag7 = true;
													current3.Line.ExtendLineToPoint(unitPoint2);
													LinePoints linePoints2 = new LinePoints();
													linePoints2.SetLine(current3.Line);
													linePoints2.MousePoint = point;
													this.m_modifiedLines.Add(linePoints2.Line, linePoints2);
												}
											}
										}
									}
									bool flag11 = flag7;
									if (flag11)
									{
										canvas.DataModel.AfterEditObjects(this);
									}
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
				foreach (LinePoints current in this.m_originalLines.Values)
				{
					current.Line.Highlighted = false;
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
				foreach (LinePoints current in this.m_originalLines.Values)
				{
					current.ResetLine();
				}
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
				foreach (LinePoints current in this.m_modifiedLines.Values)
				{
					current.ResetLine();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
