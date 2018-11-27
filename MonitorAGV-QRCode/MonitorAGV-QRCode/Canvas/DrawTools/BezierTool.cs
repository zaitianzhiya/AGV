using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class BezierTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
		public enum eCurrentPoint
		{
			p1,
			p2,
			p3,
			p4,
			done
		}

		protected static int ThresholdPixel = 0;

		private UnitPoint m_p1 = UnitPoint.Empty;

		private UnitPoint m_p2 = UnitPoint.Empty;

		private UnitPoint m_p3 = UnitPoint.Empty;

		private UnitPoint m_p4 = UnitPoint.Empty;

		protected BezierTool.eCurrentPoint m_curPoint = BezierTool.eCurrentPoint.p1;

		private BezierTool m_owner;

		private BezierTool m_clone;

		private BezierTool.eCurrentPoint m_pointId;

		private UnitPoint m_originalPoint;

		private UnitPoint m_endPoint;

		[XmlSerializable, Description("长度")]
		public double Lenth
		{
			get;
			set;
		}

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "BezierTool";
			}
		}

		[Browsable(false)]
		public bool UseRoute
		{
			get;
			set;
		}

		[Browsable(false)]
		public virtual string Id
		{
			get
			{
				return BezierTool.ObjectType;
			}
		}

		[XmlSerializable, Browsable(false)]
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

		[XmlSerializable, Browsable(false)]
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

		[XmlSerializable, Browsable(false)]
		public UnitPoint P3
		{
			get
			{
				return this.m_p3;
			}
			set
			{
				this.m_p3 = value;
			}
		}

		[XmlSerializable, Browsable(false)]
		public UnitPoint P4
		{
			get
			{
				return this.m_p4;
			}
			set
			{
				this.m_p4 = value;
			}
		}

		[Browsable(false)]
		public BezierTool.eCurrentPoint CurrentPoint
		{
			get
			{
				return this.m_curPoint;
			}
			set
			{
				this.m_curPoint = value;
			}
		}

		[Browsable(false)]
		public UnitPoint RepeatStartingPoint
		{
			get
			{
				return this.m_p4;
			}
		}

		public BezierTool.eCurrentPoint CurrSelectPoint
		{
			get
			{
				return this.m_pointId;
			}
		}

		[XmlSerializable, Browsable(false)]
		public BezierType Type
		{
			get;
			set;
		}

		public BezierTool()
		{
			this.UseRoute = false;
		}

		public BezierTool(BezierType type)
		{
			this.Type = type;
			this.UseRoute = false;
		}

		public BezierTool(BezierTool owner, BezierTool.eCurrentPoint id)
		{
			try
			{
				this.m_owner = owner;
				this.m_clone = (this.m_owner.Clone() as BezierTool);
				this.m_pointId = id;
				this.Type = owner.Type;
				this.m_originalPoint = this.GetPoint(this.m_pointId);
				this.UseRoute = false;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
		{
			try
			{
				base.Width = layer.Width;
				base.Color = layer.Color;
				this.m_p4 = point;
				this.m_p3 = point;
				this.m_p2 = point;
				this.m_p1 = point;
				this.Selected = true;
				this.OnMouseDown(null, point, snap);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected UnitPoint GetPoint(BezierTool.eCurrentPoint pointid)
		{
			UnitPoint result;
			try
			{
				bool flag = pointid == BezierTool.eCurrentPoint.p1;
				if (flag)
				{
					result = this.m_clone.P1;
				}
				else
				{
					bool flag2 = pointid == BezierTool.eCurrentPoint.p1;
					if (flag2)
					{
						result = this.m_clone.P2;
					}
					else
					{
						bool flag3 = pointid == BezierTool.eCurrentPoint.p1;
						if (flag3)
						{
							result = this.m_clone.P3;
						}
						else
						{
							bool flag4 = pointid == BezierTool.eCurrentPoint.p1;
							if (flag4)
							{
								result = this.m_clone.P4;
							}
							else
							{
								result = this.m_owner.P1;
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

		public void AfterSerializedIn()
		{
		}

		public void Cancel()
		{
		}

		public virtual void Copy(BezierTool acopy)
		{
			try
			{
				base.Copy(acopy);
				this.m_p1 = acopy.m_p1;
				this.m_p2 = acopy.m_p2;
				this.m_p3 = acopy.m_p3;
				this.m_p4 = acopy.m_p4;
				this.Selected = acopy.Selected;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IDrawObject Clone()
		{
			IDrawObject result;
			try
			{
				BezierTool bezierTool = new BezierTool(this.Type);
				bezierTool.Copy(this);
				result = bezierTool;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				Pen pen = new Pen(base.Color, base.Width);
				bool useRoute = this.UseRoute;
				if (useRoute)
				{
					pen.Color = Color.DeepPink;
					pen.DashStyle = DashStyle.Custom;
					pen.DashPattern = new float[]
					{
						1f,
						1f
					};
				}
				bool flag = this.Type == BezierType.PointBezier;
				if (flag)
				{
					GraphicsPath graphicsPath = new GraphicsPath();
					float num = canvas.ToScreen(0.0);
					float num2 = canvas.ToScreen(0.0);
					float num3 = canvas.ToScreen(0.03);
					float num4 = canvas.ToScreen(-0.08);
					float num5 = canvas.ToScreen(-0.03);
					float num6 = canvas.ToScreen(-0.08);
					bool flag2 = float.IsNaN(num) || float.IsInfinity(num) || float.IsNaN(num2) || float.IsInfinity(num2) || float.IsNaN(num3) || float.IsInfinity(num3) || float.IsNaN(num4) || float.IsInfinity(num4) || float.IsNaN(num5) || float.IsInfinity(num5) || float.IsNaN(num6) || float.IsInfinity(num6);
					if (flag2)
					{
						return;
					}
					graphicsPath.AddLine(new PointF(num2, num6), new PointF(num3, num4));
					graphicsPath.AddLine(new PointF(num3, num4), new PointF(num2, num));
					graphicsPath.AddLine(new PointF(num2, num), new PointF(num5, num6));
					graphicsPath.CloseFigure();
					CustomLineCap customEndCap;
					try
					{
						customEndCap = new CustomLineCap(graphicsPath, null);
					}
					catch
					{
						return;
					}
					pen.CustomEndCap = customEndCap;
				}
				else
				{
					pen.EndCap = LineCap.Round;
				}
				pen.StartCap = LineCap.Round;
				bool highlighted = this.Highlighted;
				if (highlighted)
				{
					Pen pen2 = (Pen)DrawUtils.SelectedPen.Clone();
					canvas.DrawBizer(canvas, pen2, this.m_p1, this.m_p4, this.m_p3, this.m_p2);
				}
				else
				{
					bool selected = this.Selected;
					if (selected)
					{
						Pen pen3 = (Pen)DrawUtils.SelectedPen.Clone();
						canvas.DrawBizer(canvas, pen3, this.m_p1, this.m_p4, this.m_p3, this.m_p2);
						canvas.DrawLine(canvas, pen3, this.m_p1, this.m_p4);
						canvas.DrawLine(canvas, pen3, this.m_p2, this.m_p3);
						bool flag3 = !this.m_p2.IsEmpty;
						if (flag3)
						{
							DrawUtils.DrawNode(canvas, this.m_p2);
						}
						bool flag4 = !this.m_p3.IsEmpty;
						if (flag4)
						{
							DrawUtils.DrawNode(canvas, this.m_p3);
						}
						bool flag5 = !this.m_p1.IsEmpty;
						if (flag5)
						{
							DrawUtils.DrawNode(canvas, this.m_p1);
						}
						bool flag6 = !this.m_p4.IsEmpty;
						if (flag6)
						{
							DrawUtils.DrawNode(canvas, this.m_p4);
						}
					}
					else
					{
						canvas.DrawBizer(canvas, pen, this.m_p1, this.m_p4, this.m_p3, this.m_p2);
					}
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
				this.m_owner.P3 = this.m_clone.P3;
				this.m_owner.P4 = this.m_clone.P4;
				this.m_clone = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public RectangleF GetBoundingRect(ICanvas canvas)
		{
			RectangleF result;
			try
			{
				double num = Math.Min(this.m_p1.X, this.m_p2.X);
				double num2 = Math.Max(this.m_p1.X, this.m_p2.X);
				double num3 = Math.Min(this.m_p1.Y, this.m_p2.Y);
				double num4 = Math.Max(this.m_p1.Y, this.m_p2.Y);
				num = Math.Min(num, this.m_p3.X);
				num2 = Math.Max(num2, this.m_p3.X);
				num3 = Math.Min(num3, this.m_p3.Y);
				num4 = Math.Max(num4, this.m_p3.Y);
				num = Math.Min(num, this.m_p4.X);
				num2 = Math.Max(num2, this.m_p4.X);
				num3 = Math.Min(num3, this.m_p4.Y);
				num4 = Math.Max(num4, this.m_p4.Y);
				result = new RectangleF(new PointF((float)num, (float)num3), new SizeF((float)Math.Abs(num2 - num), (float)Math.Abs(num4 - num3)));
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

		public string GetInfoAsString()
		{
			return "贝塞尔曲线";
		}

		public void GetObjectData(XmlWriter wr)
		{
			try
			{
				wr.WriteStartElement("BezierTool");
				XmlUtil.WriteProperties(this, wr);
				wr.WriteEndElement();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IDrawObject GetOriginal()
		{
			return this.m_owner;
		}

		public void Move(UnitPoint offset)
		{
			try
			{
				this.m_p1.X = this.m_p1.X + offset.X;
				this.m_p1.Y = this.m_p1.Y + offset.Y;
				this.m_p2.X = this.m_p2.X + offset.X;
				this.m_p2.Y = this.m_p2.Y + offset.Y;
				this.m_p3.X = this.m_p3.X + offset.X;
				this.m_p3.Y = this.m_p3.Y + offset.Y;
				this.m_p4.X = this.m_p4.X + offset.X;
				this.m_p4.Y = this.m_p4.Y + offset.Y;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
		{
			INodePoint result;
			try
			{
				float radius = LineTool.ThresholdWidth(canvas, base.Width, (float)BezierTool.ThresholdPixel);
				bool flag = HitUtil.CircleHitPoint(this.m_p1, radius, point);
				if (flag)
				{
					result = new BezierTool(this, BezierTool.eCurrentPoint.p1);
				}
				else
				{
					bool flag2 = HitUtil.CircleHitPoint(this.m_p2, radius, point);
					if (flag2)
					{
						result = new BezierTool(this, BezierTool.eCurrentPoint.p2);
					}
					else
					{
						bool flag3 = HitUtil.CircleHitPoint(this.m_p3, radius, point);
						if (flag3)
						{
							result = new BezierTool(this, BezierTool.eCurrentPoint.p3);
						}
						else
						{
							bool flag4 = HitUtil.CircleHitPoint(this.m_p4, radius, point);
							if (flag4)
							{
								result = new BezierTool(this, BezierTool.eCurrentPoint.p4);
							}
							else
							{
								result = null;
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

		public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
		{
		}

		public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			eDrawObjectMouseDownEnum result;
			try
			{
				this.Selected = false;
				this.OnMouseMove(canvas, point);
				bool flag = this.m_curPoint == BezierTool.eCurrentPoint.p1;
				if (flag)
				{
					this.m_curPoint = BezierTool.eCurrentPoint.p2;
					result = eDrawObjectMouseDownEnum.Continue;
				}
				else
				{
					bool flag2 = this.m_curPoint == BezierTool.eCurrentPoint.p2;
					if (flag2)
					{
						this.m_curPoint = BezierTool.eCurrentPoint.p3;
						result = eDrawObjectMouseDownEnum.Continue;
					}
					else
					{
						bool flag3 = this.m_curPoint == BezierTool.eCurrentPoint.p3;
						if (flag3)
						{
							this.m_curPoint = BezierTool.eCurrentPoint.p4;
							result = eDrawObjectMouseDownEnum.Continue;
						}
						else
						{
							bool flag4 = this.m_curPoint == BezierTool.eCurrentPoint.p4;
							if (flag4)
							{
								this.m_curPoint = BezierTool.eCurrentPoint.done;
								result = eDrawObjectMouseDownEnum.Done;
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

		public void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
			try
			{
				bool flag = this.m_curPoint == BezierTool.eCurrentPoint.p1;
				if (flag)
				{
					this.m_p1 = point;
				}
				else
				{
					bool flag2 = this.m_curPoint == BezierTool.eCurrentPoint.p2;
					if (flag2)
					{
						this.m_p2 = point;
					}
					else
					{
						bool flag3 = this.m_curPoint == BezierTool.eCurrentPoint.p3;
						if (flag3)
						{
							this.m_p3 = point;
						}
						else
						{
							bool flag4 = this.m_curPoint == BezierTool.eCurrentPoint.p4;
							if (flag4)
							{
								this.m_p4 = point;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
		}

		public bool PointInObject(ICanvas canvas, UnitPoint point)
		{
			bool result;
			try
			{
				PointF[] array = new PointF[]
				{
					this.P1.Point,
					this.P4.Point,
					this.P3.Point,
					this.P2.Point
				};
				PointF[] source = HitUtil.draw_bezier_curves(array, array.Length, 0.001f);
				bool flag = source.Count((PointF p) => (double)Math.Abs(p.X - point.Point.X) <= 0.05 && (double)Math.Abs(p.Y - point.Point.Y) <= 0.05) > 0;
				if (flag)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
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

		public void SetPosition(UnitPoint pos)
		{
			try
			{
				this.SetPoint(this.m_pointId, pos, this.m_clone);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected void SetPoint(BezierTool.eCurrentPoint pointid, UnitPoint point, BezierTool Bezier)
		{
			try
			{
				bool flag = pointid == BezierTool.eCurrentPoint.p1;
				if (flag)
				{
					Bezier.P1 = point;
				}
				bool flag2 = pointid == BezierTool.eCurrentPoint.p2;
				if (flag2)
				{
					Bezier.P2 = point;
				}
				bool flag3 = pointid == BezierTool.eCurrentPoint.p3;
				if (flag3)
				{
					Bezier.P3 = point;
				}
				bool flag4 = pointid == BezierTool.eCurrentPoint.p4;
				if (flag4)
				{
					Bezier.P4 = point;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
		{
			ISnapPoint result;
			try
			{
				bool flag = this.Type == BezierType.Bezier;
				if (flag)
				{
					bool flag2 = HitUtil.Distance(point, this.m_p1, true) <= 0.2;
					if (flag2)
					{
						result = new VertextSnapPoint(canvas, this, this.m_p1);
						return result;
					}
					bool flag3 = HitUtil.Distance(point, this.m_p2, true) <= 0.2;
					if (flag3)
					{
						result = new VertextSnapPoint(canvas, this, this.m_p2);
						return result;
					}
					bool flag4 = HitUtil.Distance(point, this.m_p3, true) <= 0.2;
					if (flag4)
					{
						result = new VertextSnapPoint(canvas, this, this.m_p3);
						return result;
					}
					bool flag5 = HitUtil.Distance(point, this.m_p4, true) <= 0.2;
					if (flag5)
					{
						result = new VertextSnapPoint(canvas, this, this.m_p4);
						return result;
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
	}
}
