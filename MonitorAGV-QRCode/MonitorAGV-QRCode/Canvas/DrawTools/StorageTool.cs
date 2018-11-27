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
	public class StorageTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
		protected string lankmarkcode = "";

		protected string storagename = "";

		protected UnitPoint location = UnitPoint.Empty;

		protected static int ThresholdPixel = 0;

		protected bool allowModify = true;

		protected UnitPoint aqlocation = UnitPoint.Empty;

		protected int stcokid = 1;

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

		[XmlSerializable]
		public int StcokID
		{
			get
			{
				return this.stcokid;
			}
			set
			{
				this.stcokid = value;
			}
		}

		[XmlSerializable, Description("储位地标号")]
		public string LankMarkCode
		{
			get
			{
				return this.lankmarkcode;
			}
			set
			{
				bool flag = this.allowModify;
				if (flag)
				{
					this.lankmarkcode = value;
				}
			}
		}

		[XmlSerializable, Description("储位名称")]
		public string StorageName
		{
			get
			{
				return this.storagename;
			}
			set
			{
				this.storagename = value;
			}
		}

		[Browsable(false)]
		public int OwnArea
		{
			get;
			set;
		}

		[Browsable(false)]
		public int SubOwnArea
		{
			get;
			set;
		}

		[Browsable(false)]
		public int matterType
		{
			get;
			set;
		}

		[Browsable(false)]
		public int MaterielType
		{
			get;
			set;
		}

		[Description("储位状态(0空|1有空料车|2有满料车)")]
		public int StorageState
		{
			get;
			set;
		}

		[Browsable(false)]
		public int LockState
		{
			get;
			set;
		}

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "StorageTool";
			}
		}

		[Browsable(false)]
		public string Id
		{
			get
			{
				return StorageTool.ObjectType;
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

		public void AfterSerializedIn()
		{
		}

		public void Cancel()
		{
		}

		public virtual IDrawObject Clone()
		{
			IDrawObject result;
			try
			{
				StorageTool storageTool = new StorageTool();
				storageTool.Copy(this);
				result = storageTool;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public virtual void Copy(StorageTool acopy)
		{
			try
			{
				base.Copy(acopy);
				this.location = acopy.location;
				this.StorageState = acopy.StorageState;
				this.LankMarkCode = acopy.LankMarkCode;
				this.StorageName = acopy.StorageName;
				this.StcokID = acopy.StcokID;
				this.LockState = acopy.LockState;
				this.OwnArea = acopy.OwnArea;
				this.SubOwnArea = acopy.SubOwnArea;
				this.matterType = acopy.matterType;
				this.Selected = acopy.Selected;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				Brush pen;
                if (this.StorageState == 2)
				{
					pen = Brushes.Blue;
				}
				else
				{
					if (this.StorageState == 1)
					{
						pen = Brushes.BlueViolet;
					}
					else
					{
                        //pen =base.Color.A!=0?new SolidBrush(base.Color): Brushes.White;
                        pen =  Brushes.White;
					}
				}
				if (this.LockState == 1)
				{
					pen = Brushes.Red;
				}
				if (this.Selected)
				{
					pen = Brushes.Magenta;
				}
				canvas.DrawStorage(canvas, pen, this.StcokID.ToString(), this.Location);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Finish()
		{
		}

		public RectangleF GetBoundingRect(ICanvas canvas)
		{
			RectangleF rect;
			try
			{
				float num = LineTool.ThresholdWidth(canvas, base.Width, (float)StorageTool.ThresholdPixel);
				bool flag = num < base.Width;
				if (flag)
				{
					num = base.Width;
				}
				float screenvalue = canvas.ToScreen(0.5);
				this.aqlocation = new UnitPoint(this.location.X + canvas.ToUnit(screenvalue), this.location.Y - canvas.ToUnit(screenvalue));
				rect = ScreenUtils.GetRect(this.location, this.aqlocation, (double)num);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return rect;
		}

		public IDrawObject GetClone()
		{
			return null;
		}

		public string GetInfoAsString()
		{
			return "";
		}

		public void GetObjectData(XmlWriter wr)
		{
			try
			{
				wr.WriteStartElement("StorageTool");
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
			return null;
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

		public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
		{
			return null;
		}

		public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
		{
			bool result;
			try
			{
				RectangleF boundingRect = this.GetBoundingRect(canvas);
				if (anyPoint)
				{
					result = HitUtil.LineIntersectWithRect(this.location, this.aqlocation, rect);
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
                if (canvas.DataModel.ActiveLayer.Objects.Any())
				{
                    if ((from p in canvas.DataModel.ActiveLayer.Objects
                             where p.Id == "StorageTool"
                             select p).Any())
					{
					    int num = (from p in canvas.DataModel.ActiveLayer.Objects
					        where p.Id == "StorageTool"
					        select p).Max(p => (p as StorageTool).StcokID);
						this.StcokID = num + 1;
					}
				}
				this.Selected = false;
				this.location = point;
				Brush white = Brushes.White;
				canvas.DrawStorage(canvas, white, this.StcokID.ToString(), this.Location);
				result = eDrawObjectMouseDownEnum.Done;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
		}

		public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
		}

		public bool PointInObject(ICanvas canvas, UnitPoint point)
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

		public void Redo()
		{
		}

		public void SetPosition(UnitPoint pos)
		{
			this.Location = pos;
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
		{
			return null;
		}

		public void Undo()
		{
		}
	}
}
