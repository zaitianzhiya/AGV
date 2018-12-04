using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class LineTool : DrawObjectBase, IDrawObject, ISerialize
	{
		protected UnitPoint m_p1;

		protected UnitPoint m_p2;

		protected int excutAngle = -1;

		private static int ThresholdPixel = 6;

		[XmlSerializable, Description("长度")]
		public double Lenth
		{
			get;
			set;
		}

		[Browsable(false)]
		public bool UseRoute
		{
			get;
			set;
		}

		[XmlSerializable]
		public UnitPoint P1
		{
			get
			{
				return this.m_p1;
			}
			set
			{
				this.m_p1 = value;
			}
		}

		[XmlSerializable]
		public UnitPoint P2
		{
			get
			{
				return this.m_p2;
			}
			set
			{
				this.m_p2 = value;
			}
		}

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "LineTool";
			}
		}

		[XmlSerializable, Browsable(false)]
		public LineType Type
		{
			get;
			set;
		}

		[XmlSerializable, Description("强制车头方向,不强制旋转请设置为:-1")]
		public int ExcuteAngle
		{
			get
			{
				return this.excutAngle;
			}
			set
			{
				this.excutAngle = value;
			}
		}

		[Browsable(false)]
		public UnitPoint RepeatStartingPoint
		{
			get
			{
				return this.m_p2;
			}
		}

		[Browsable(false)]
		public virtual string Id
		{
			get
			{
				return LineTool.ObjectType;
			}
		}

		public LineTool(LineType type)
		{
			this.Type = type;
			this.Lenth = 1.0;
			this.UseRoute = false;
		}

		public LineTool()
		{
			this.UseRoute = false;
			this.Lenth = 1.0;
		}

		public LineTool(UnitPoint point, UnitPoint endpoint, float width, Color color)
		{
			this.P1 = point;
			this.P2 = endpoint;
			base.Width = width;
			base.Color = color;
			this.Selected = false;
			this.UseRoute = false;
			this.Lenth = 1.0;
		}

		public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
		{
			this.P2 = point;
			this.P1 = point;
			base.Width = layer.Width;
			base.Color = layer.Color;
			this.Selected = true;
		}

		public static float ThresholdWidth(ICanvas canvas, float objectwidth)
		{
			return LineTool.ThresholdWidth(canvas, objectwidth, (float)LineTool.ThresholdPixel);
		}

		public static float ThresholdWidth(ICanvas canvas, float objectwidth, float pixelwidth)
		{
			double val = canvas.ToUnit(pixelwidth);
			double num = Math.Max((double)(objectwidth / 2f), val);
			return (float)num;
		}

		public virtual void Copy(LineTool acopy)
		{
			base.Copy(acopy);
			this.m_p1 = acopy.m_p1;
			this.m_p2 = acopy.m_p2;
			this.Selected = acopy.Selected;
			this.Lenth = acopy.Lenth;
		}

		public virtual IDrawObject Clone()
		{
			LineTool lineTool = new LineTool(this.Type);
			lineTool.Copy(this);
			return lineTool;
		}

		public RectangleF GetBoundingRect(ICanvas canvas)
		{
			RectangleF rect;
			try
			{
				float num = LineTool.ThresholdWidth(canvas, base.Width);
				rect = ScreenUtils.GetRect(this.m_p1, this.m_p2, (double)num);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return rect;
		}

		private UnitPoint MidPoint(ICanvas canvas, UnitPoint p1, UnitPoint p2, UnitPoint hitpoint)
		{
			UnitPoint empty;
			try
			{
				empty = UnitPoint.Empty;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return empty;
		}

		public bool PointInObject(ICanvas canvas, UnitPoint point)
		{
			bool result;
			try
			{
				float halflinewidth = LineTool.ThresholdWidth(canvas, base.Width);
				result = HitUtil.IsPointInLine(this.m_p1, this.m_p2, point, halflinewidth);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
		{
			bool result;
			try
			{
				RectangleF boundingRect = this.GetBoundingRect(canvas);
				if (anyPoint)
				{
					result = HitUtil.LineIntersectWithRect(this.m_p1, this.m_p2, rect);
				}
				else
				{
					result = rect.Contains(boundingRect);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public virtual void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				Pen pen;
				if (this.Type == LineType.PointLine)
				{
					pen = new Pen(base.Color, base.Width);
				}
				else
				{
					pen = new Pen(base.Color, base.Width);
				}
				if (this.UseRoute)
				{
					pen.Color = Color.DeepPink;
					pen.DashStyle = DashStyle.Custom;
					pen.DashPattern = new float[]
					{
						1f,
						1f
					};
				}
				if (this.Type == LineType.PointLine)
				{
					GraphicsPath graphicsPath = new GraphicsPath();
					float y = canvas.ToScreen(0.0);
					float x = canvas.ToScreen(0.0);
					float x2 = canvas.ToScreen(0.03);
					float y2 = canvas.ToScreen(-0.08);
					float x3 = canvas.ToScreen(-0.03);
					float y3 = canvas.ToScreen(-0.08);
					graphicsPath.AddLine(new PointF(x, y3), new PointF(x2, y2));
					graphicsPath.AddLine(new PointF(x2, y2), new PointF(x, y));
					graphicsPath.AddLine(new PointF(x, y), new PointF(x3, y3));
					graphicsPath.CloseFigure();
					CustomLineCap customEndCap = new CustomLineCap(graphicsPath, null);
					pen.CustomEndCap = customEndCap;
				}
				else
				{
					pen.EndCap = LineCap.Round;
				}
				pen.StartCap = LineCap.Round;
				canvas.DrawLine(canvas, pen, this.m_p1, this.m_p2);
				if (this.Highlighted)
				{
					canvas.DrawLine(canvas, DrawUtils.SelectedPen, this.m_p1, this.m_p2);
				}
				if (this.Selected)
				{
					canvas.DrawLine(canvas, DrawUtils.SelectedPen, this.m_p1, this.m_p2);
					if (!this.m_p1.IsEmpty)
					{
						DrawUtils.DrawNode(canvas, this.m_p1);
					}
					if (!this.m_p2.IsEmpty)
					{
						DrawUtils.DrawNode(canvas, this.m_p2);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
			try
			{
				bool flag = Control.ModifierKeys == Keys.Control;
				if (flag)
				{
					point = HitUtil.OrthoPointD(this.m_p1, point, 45.0);
				}
				this.m_p2 = point;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public virtual eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			eDrawObjectMouseDownEnum result;
			try
			{
				this.Selected = false;
				bool flag = snappoint is PerpendicularSnapPoint && snappoint.Owner is LineTool;
				if (flag)
				{
					LineTool lineTool = snappoint.Owner as LineTool;
					this.m_p2 = HitUtil.NearestPointOnLine(lineTool.P1, lineTool.P2, this.m_p1, true);
					result = eDrawObjectMouseDownEnum.DoneRepeat;
				}
				else
				{
					bool flag2 = Control.ModifierKeys == Keys.Control;
					if (flag2)
					{
						point = HitUtil.OrthoPointD(this.m_p1, point, 45.0);
					}
					this.m_p2 = point;
					result = eDrawObjectMouseDownEnum.DoneRepeat;
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

		public virtual void OnKeyDown(ICanvas canvas, KeyEventArgs e)
		{
		}

		public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
		{
			INodePoint result;
			try
			{
				float radius = LineTool.ThresholdWidth(canvas, base.Width);
				bool flag = HitUtil.CircleHitPoint(this.m_p1, radius, point);
				if (flag)
				{
					result = new NodePointLine(this, NodePointLine.ePoint.P1);
				}
				else
				{
					bool flag2 = HitUtil.CircleHitPoint(this.m_p2, radius, point);
					if (flag2)
					{
						result = new NodePointLine(this, NodePointLine.ePoint.P2);
					}
					else
					{
						result = null;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobjs, Type[] runningsnaptypes, Type usersnaptype)
		{
			ISnapPoint result;
			try
			{
				bool flag = this.Type == LineType.Line;
				if (flag)
				{
					float radius = LineTool.ThresholdWidth(canvas, base.Width);
					bool flag2 = runningsnaptypes != null;
					if (flag2)
					{
						int i = 0;
						while (i < runningsnaptypes.Length)
						{
							Type left = runningsnaptypes[i];
							bool flag3 = left == typeof(VertextSnapPoint);
							if (flag3)
							{
								bool flag4 = HitUtil.CircleHitPoint(this.m_p1, radius, point);
								if (flag4)
								{
									result = new VertextSnapPoint(canvas, this, this.m_p1);
									return result;
								}
								bool flag5 = HitUtil.CircleHitPoint(this.m_p2, radius, point);
								if (flag5)
								{
									result = new VertextSnapPoint(canvas, this, this.m_p2);
									return result;
								}
							}
							bool flag6 = left == typeof(IntersectSnapPoint);
							if (flag6)
							{
								LineTool lineTool = Utiles.FindObjectTypeInList(this, otherobjs, typeof(LineTool)) as LineTool;
								bool flag7 = lineTool == null;
								if (!flag7)
								{
									UnitPoint unitPoint = HitUtil.LinesIntersectPoint(this.m_p1, this.m_p2, lineTool.m_p1, lineTool.m_p2);
									bool flag8 = unitPoint != UnitPoint.Empty;
									if (flag8)
									{
										result = new IntersectSnapPoint(canvas, this, unitPoint);
										return result;
									}
								}
							}
							IL_129:
							i++;
							continue;
							goto IL_129;
						}
						result = null;
						return result;
					}
					bool flag9 = usersnaptype == typeof(IntersectSnapPoint);
					if (flag9)
					{
						LineTool lineTool2 = Utiles.FindObjectTypeInList(this, otherobjs, typeof(LineTool)) as LineTool;
						bool flag10 = lineTool2 == null;
						if (flag10)
						{
							result = null;
							return result;
						}
						UnitPoint unitPoint2 = HitUtil.LinesIntersectPoint(this.m_p1, this.m_p2, lineTool2.m_p1, lineTool2.m_p2);
						bool flag11 = unitPoint2 != UnitPoint.Empty;
						if (flag11)
						{
							result = new IntersectSnapPoint(canvas, this, unitPoint2);
							return result;
						}
					}
					bool flag12 = usersnaptype == typeof(VertextSnapPoint);
					if (flag12)
					{
						double num = HitUtil.Distance(point, this.m_p1);
						double num2 = HitUtil.Distance(point, this.m_p2);
						bool flag13 = num <= num2;
						if (flag13)
						{
							result = new VertextSnapPoint(canvas, this, this.m_p1);
							return result;
						}
						result = new VertextSnapPoint(canvas, this, this.m_p2);
						return result;
					}
					else
					{
						bool flag14 = usersnaptype == typeof(NearestSnapPoint);
						if (flag14)
						{
							UnitPoint unitPoint3 = HitUtil.NearestPointOnLine(this.m_p1, this.m_p2, point);
							bool flag15 = unitPoint3 != UnitPoint.Empty;
							if (flag15)
							{
								result = new NearestSnapPoint(canvas, this, unitPoint3);
								return result;
							}
						}
						bool flag16 = usersnaptype == typeof(PerpendicularSnapPoint);
						if (flag16)
						{
							UnitPoint unitPoint4 = HitUtil.NearestPointOnLine(this.m_p1, this.m_p2, point);
							bool flag17 = unitPoint4 != UnitPoint.Empty;
							if (flag17)
							{
								result = new PerpendicularSnapPoint(canvas, this, unitPoint4);
								return result;
							}
						}
					}
				}
				result = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void Move(UnitPoint offset)
		{
			try
			{
				this.m_p1.X = this.m_p1.X + offset.X;
				this.m_p1.Y = this.m_p1.Y + offset.Y;
				this.m_p2.X = this.m_p2.X + offset.X;
				this.m_p2.Y = this.m_p2.Y + offset.Y;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetInfoAsString()
		{
			string result;
			try
			{
				result = string.Format("Line@{0},{1} - L={2:f4}<{3:f4}", new object[]
				{
					this.P1.PosAsString(),
					this.P2.PosAsString(),
					HitUtil.Distance(this.P1, this.P2),
					HitUtil.RadiansToDegrees(HitUtil.LineAngleR(this.P1, this.P2, 0.0))
				});
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void GetObjectData(XmlWriter wr)
		{
			try
			{
				wr.WriteStartElement("LineTool");
				XmlUtil.WriteProperties(this, wr);
				wr.WriteEndElement();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void AfterSerializedIn()
		{
		}

		public void ExtendLineToPoint(UnitPoint newpoint)
		{
			try
			{
				UnitPoint unitPoint = HitUtil.NearestPointOnLine(this.P1, this.P2, newpoint, true);
				bool flag = HitUtil.Distance(unitPoint, this.P1) < HitUtil.Distance(unitPoint, this.P2);
				if (flag)
				{
					this.P1 = unitPoint;
				}
				else
				{
					this.P2 = unitPoint;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
