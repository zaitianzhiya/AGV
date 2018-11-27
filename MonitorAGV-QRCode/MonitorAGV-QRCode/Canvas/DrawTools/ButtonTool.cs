using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class ButtonTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
		protected UnitPoint location = UnitPoint.Empty;

		protected UnitPoint centerpoint = UnitPoint.Empty;

		protected int ThresholdPixel = 0;

		protected float radius = 1f;

		protected int BoxID = 0;

		[XmlSerializable]
		public int CallBoxID
		{
			get
			{
				return this.BoxID;
			}
			set
			{
				this.BoxID = value;
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
		public UnitPoint Centerpoint
		{
			get
			{
				return this.centerpoint;
			}
			set
			{
				this.centerpoint = value;
			}
		}

		[XmlSerializable]
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius = value;
			}
		}

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "ButtonTool";
			}
		}

		[Browsable(false)]
		public virtual string Id
		{
			get
			{
				return ButtonTool.ObjectType;
			}
		}

		[Browsable(false)]
		public UnitPoint RepeatStartingPoint
		{
			get
			{
				return this.Location;
			}
		}

		public ButtonTool()
		{
			base.Width = 0.1f;
			this.Selected = false;
		}

		public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
		{
			base.Width = layer.Width;
			base.Color = layer.Color;
			this.Selected = true;
		}

		public void SetPosition(UnitPoint pos)
		{
			this.Location = pos;
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

		public void GetObjectData(XmlWriter wr)
		{
			wr.WriteStartElement("ButtonTool");
			XmlUtil.WriteProperties(this, wr);
			wr.WriteEndElement();
		}

		public void AfterSerializedIn()
		{
		}

		public virtual IDrawObject Clone()
		{
			ButtonTool buttonTool = new ButtonTool();
			buttonTool.Copy(this);
			return buttonTool;
		}

		public virtual void Copy(ButtonTool acopy)
		{
			base.Copy(acopy);
			this.Location = acopy.Location;
			this.Radius = acopy.Radius;
			this.Selected = acopy.Selected;
		}

		public RectangleF GetBoundingRect(ICanvas canvas)
		{
			RectangleF rect;
			try
			{
				double width = canvas.ToUnit((float)this.ThresholdPixel);
				float screenvalue = canvas.ToScreen((double)this.radius);
				UnitPoint p = new UnitPoint(this.Location.X + canvas.ToUnit(screenvalue), this.Location.Y - canvas.ToUnit(screenvalue));
				rect = ScreenUtils.GetRect(this.Location, p, width);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return rect;
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
					float screenvalue = canvas.ToScreen((double)this.radius);
					UnitPoint lp = new UnitPoint(this.Location.X + canvas.ToUnit(screenvalue), this.Location.Y - canvas.ToUnit(screenvalue));
					result = HitUtil.LineIntersectWithRect(this.Location, lp, rect);
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

		public void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				Brush darkRed = Brushes.DarkRed;
				canvas.DrawBtnBox(canvas, this.Radius, this.Location, this.Selected);
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
				Brush darkRed = Brushes.DarkRed;
				float num = canvas.ToScreen((double)this.radius);
				UnitPoint offset = new UnitPoint((double)(-(double)this.radius / 2f), (double)(this.radius / 2f));
				this.Move(offset);
				canvas.DrawBtnBox(canvas, this.radius, this.Location, this.Selected);
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
			this.location.X = this.location.X + offset.X;
			this.location.Y = this.location.Y + offset.Y;
		}

		public string GetInfoAsString()
		{
			return "";
		}
	}
}
