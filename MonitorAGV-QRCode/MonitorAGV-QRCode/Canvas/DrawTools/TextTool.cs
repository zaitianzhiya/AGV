using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class TextTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
		private int font_size =17;

		private string str_value = "输入文字...";

		private UnitPoint location = UnitPoint.Empty;

		protected static int ThresholdPixel = 0;

		[XmlSerializable, Description("字体大小")]
		public int FontSize
		{
			get
			{
				return this.font_size;
			}
			set
			{
				this.font_size = value;
			}
		}

		[XmlSerializable, Description("显示文字")]
		public string StrValue
		{
			get
			{
				return this.str_value;
			}
			set
			{
				this.str_value = value;
			}
		}

		[XmlSerializable, Browsable(false)]
		public UnitPoint Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "TextTool";
			}
		}

		[Browsable(false)]
		public virtual string Id
		{
			get
			{
				return TextTool.ObjectType;
			}
		}

		[Browsable(false)]
		public UnitPoint RepeatStartingPoint
		{
			get
			{
				return this.location;
			}
		}

		public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
		{
			try
			{
				base.Width = layer.Width;
				base.Color = layer.Color;
				this.Selected = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void GetObjectData(XmlWriter wr)
		{
			try
			{
				wr.WriteStartElement("TextTool");
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

		public virtual IDrawObject Clone()
		{
			IDrawObject result;
			try
			{
				TextTool textTool = new TextTool();
				textTool.Copy(this);
				result = textTool;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public virtual void Copy(TextTool acopy)
		{
			try
			{
				base.Copy(acopy);
				this.Location = acopy.Location;
				this.FontSize = acopy.FontSize;
				this.StrValue = acopy.StrValue;
				this.Selected = acopy.Selected;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public RectangleF GetBoundingRect(ICanvas canvas)
		{
			RectangleF rect;
			try
			{
				float num = LineTool.ThresholdWidth(canvas, base.Width, (float)TextTool.ThresholdPixel);
				if (num < base.Width)
				{
					num = base.Width;
				}
				UnitPoint p = new UnitPoint(this.location.X + canvas.ToUnit((float)(this.FontSize * 2 * this.str_value.Length)), this.location.Y - canvas.ToUnit((float)(this.FontSize * 2)));
				rect = ScreenUtils.GetRect(this.location, p, (double)num);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return rect;
		}

		private UnitPoint MidPoint(ICanvas canvas, UnitPoint p1, UnitPoint p2, UnitPoint hitpoint)
		{
			return UnitPoint.Empty;
		}

		public virtual bool PointInObject(ICanvas canvas, UnitPoint point)
		{
			bool result;
			try
			{
				bool flag = !this.GetBoundingRect(canvas).Contains(point.Point);
				if (flag)
				{
					result = false;
				}
				else
				{
					result = true;
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
					UnitPoint lp = new UnitPoint(this.location.X + canvas.ToUnit(10f * (float)this.str_value.Length), this.location.Y);
					bool flag = HitUtil.LineIntersectWithRect(this.location, lp, rect);
					if (flag)
					{
						result = true;
					}
					else
					{
						lp = new UnitPoint(this.location.X, this.location.Y - canvas.ToUnit(10f));
						result = HitUtil.LineIntersectWithRect(this.location, lp, rect);
					}
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
				canvas.DrawTxt(canvas, this.StrValue, this.Location, this.FontSize, base.Color);
                if (this.Selected)
				{
					canvas.DrawTxt(canvas, this.StrValue, this.Location, this.FontSize, DrawUtils.SelectedPen.Color);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
		}

		public virtual eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			eDrawObjectMouseDownEnum result;
			try
			{
				this.Selected = false;
				this.location = point;
				canvas.DrawTxt(canvas, this.StrValue, this.location, this.FontSize, base.Color);
                if (this.Selected)
				{
					canvas.DrawTxt(canvas, this.StrValue, this.location, this.FontSize, DrawUtils.SelectedPen.Color);
				}
				result = eDrawObjectMouseDownEnum.Done;
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

		public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
		{
			return null;
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobjs, Type[] runningsnaptypes, Type usersnaptype)
		{
			return null;
		}

		public void Move(UnitPoint offset)
		{
			try
			{
				this.location.X = this.location.X + offset.X;
				this.location.Y = this.location.Y + offset.Y;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetInfoAsString()
		{
			return string.Format("TextTool:@{0}", this.StrValue);
		}

		public void SetPosition(UnitPoint pos)
		{
			try
			{
				this.Location = pos;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public IDrawObject GetClone()
		{
			return null;
		}

		public IDrawObject GetOriginal()
		{
			return null;
		}

		public void Finish()
		{
		}

		public void Cancel()
		{
		}

		public void Undo()
		{
		}

		public void Redo()
		{
		}

		public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
		{
		}
	}
}
