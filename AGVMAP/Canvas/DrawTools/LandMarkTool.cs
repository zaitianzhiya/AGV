using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class LandMarkTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
        public enum IsWork
        {
            否 = 0,
            是 = 1
        }

        public enum WorkDir
        {
            无方向=0,
            正向=1,
            反向=2
        }

        public enum ActionDir
        {
            上升 = 0,
            下降 = 1
        }

        protected IsWork isWork = IsWork.否;

        protected WorkDir workDir = WorkDir.无方向;

        protected ActionDir actionDir = ActionDir.上升;
       
		protected UnitPoint location = UnitPoint.Empty;

		protected UnitPoint aqlocation = UnitPoint.Empty;

		protected UnitPoint midpoint = UnitPoint.Empty;

		protected string landcode = "1";

		protected string landname = "";

		protected static int ThresholdPixel = 0;

		protected bool allowModify = true;

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

		[XmlSerializable, Description("地标编码")]
		public string LandCode
		{
			get
			{
				return this.landcode;
			}
			set
			{
				this.landcode = value;
			}
		}

		[XmlSerializable, Description("地标名称")]
		public string LandName
		{
			get
			{
				return this.landname;
			}
			set
			{
				this.landname = value;
			}
		}

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "LandMark";
			}
		}

		[Browsable(false)]
		public virtual string Id
		{
			get
			{
				return LandMarkTool.ObjectType;
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

		[Description("地标中心点")]
		public UnitPoint MidPoint
		{
			get
			{
				this.midpoint = new UnitPoint(this.Location.X + 0.1, this.Location.Y - 0.1);
				return this.midpoint;
			}
		}

        [XmlSerializable, Description("是否工位")]
        public IsWork IsWorkStation
        {
            get { return isWork; }
            set { isWork = value; }
        }

        [XmlSerializable, Description("工位")]
        public WorkDir WorkDirect
        {
            get { return workDir; }
            set { workDir = value; }
        }

        [XmlSerializable, Description("上升、下降")]
        public ActionDir ActionDirect
        {
            get { return actionDir; }
            set { actionDir = value; }
        }

		public LandMarkTool()
		{
			base.Width = 0.2f;
		}

		public LandMarkTool(string code)
		{
			this.landcode = code;
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
			wr.WriteStartElement("LandMark");
			XmlUtil.WriteProperties(this, wr);
			wr.WriteEndElement();
		}

		public void AfterSerializedIn()
		{
		}

		public virtual IDrawObject Clone()
		{
			LandMarkTool landMarkTool = new LandMarkTool();
			landMarkTool.Copy(this);
			return landMarkTool;
		}

		public virtual void Copy(LandMarkTool acopy)
		{
			base.Copy(acopy);
			this.Location = acopy.Location;
			this.LandCode = acopy.LandCode;
			this.LandName = acopy.LandName;
			this.Selected = acopy.Selected;
		}

		public RectangleF GetBoundingRect(ICanvas canvas)
		{
			RectangleF rect;
			try
			{
				double width = canvas.ToUnit((float)LandMarkTool.ThresholdPixel);
				float screenvalue = canvas.ToScreen(0.20000000298023224);
				this.aqlocation = new UnitPoint(this.Location.X + canvas.ToUnit(screenvalue), this.Location.Y - canvas.ToUnit(screenvalue));
				rect = ScreenUtils.GetRect(this.Location, this.aqlocation, width);
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
					result = HitUtil.LineIntersectWithRect(this.Location, this.aqlocation, rect);
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
				canvas.DrawLandMark(canvas, darkRed, this.LandCode, this.Location);
				bool selected = this.Selected;
				if (selected)
				{
					Brush magenta = Brushes.Magenta;
					canvas.DrawLandMark(canvas, magenta, this.LandCode, this.Location);
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
                if (canvas.DataModel.ActiveLayer.Objects.Any())
				{
					if ((from p in canvas.DataModel.ActiveLayer.Objects
                             where p.Id == "LandMark"
                             select p).Any())
					{
					    int num = (from p in canvas.DataModel.ActiveLayer.Objects
					        where p.Id == "LandMark"
					        select p).Max(p => Convert.ToInt32((p as LandMarkTool).LandCode));
						this.LandCode = (num + 1).ToString();
					}
				}
				this.Selected = false;
				this.location = point;
				Brush darkRed = Brushes.DarkRed;
				UnitPoint offset = new UnitPoint(-0.1, 0.1);
				this.Move(offset);
				canvas.DrawLandMark(canvas, darkRed, this.LandCode, this.Location);
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
			return new MidpointSnapPoint(canvas, this, new UnitPoint(this.Location.X + 0.1, this.Location.Y - 0.1));
		}

		public void Move(UnitPoint offset)
		{
			this.location.X = this.location.X + offset.X;
			this.location.Y = this.location.Y + offset.Y;
		}

		public string GetInfoAsString()
		{
			return string.Format("LandMark:@{0}", this.LandCode);
		}
	}
}
