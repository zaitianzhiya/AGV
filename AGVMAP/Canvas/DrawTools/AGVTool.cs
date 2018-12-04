using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class AGVTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
		protected UnitPoint position;

		protected string agv_id = "";

		private bool isviewable = true;

		protected static int ThresholdPixel = 15;

		public string Agv_id
		{
			get
			{
				return this.agv_id;
			}
			set
			{
				this.agv_id = value;
			}
		}

		public static string ObjectType
		{
			get
			{
				return "AGVTool";
			}
		}

		public string Id
		{
			get
			{
				return AGVTool.ObjectType;
			}
		}

		public UnitPoint Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		public UnitPoint RepeatStartingPoint
		{
			get
			{
				return this.position;
			}
		}

		public bool IsViewable
		{
			get
			{
				return this.isviewable;
			}
			set
			{
				this.isviewable = value;
			}
		}

		public void AfterSerializedIn()
		{
		}

		public void Cancel()
		{
		}

		public IDrawObject Clone()
		{
			AGVTool aGVTool = new AGVTool();
			aGVTool.Copy(this);
			return aGVTool;
		}

		public void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				bool flag = !this.isviewable;
				if (!flag)
				{
					canvas.DrawAGV(canvas, new Pen(Color.Red, canvas.ToScreen(0.05))
					{
						DashStyle = DashStyle.Custom
					}, this.position, this.Agv_id);
				}
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
			float num = LineTool.ThresholdWidth(canvas, base.Width, (float)AGVTool.ThresholdPixel);
			return ScreenUtils.GetRect(new UnitPoint(this.position.X - 0.2, this.position.Y + 0.2), new UnitPoint(this.position.X + 0.2, this.position.Y - 0.2), (double)num);
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
		}

		public IDrawObject GetOriginal()
		{
			return null;
		}

		public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
		{
			base.Width = layer.Width;
			base.Color = layer.Color;
			this.Selected = true;
		}

		public void Move(UnitPoint offset)
		{
		}

		public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
		{
			return null;
		}

		public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
		{
			RectangleF boundingRect = this.GetBoundingRect(canvas);
			bool result;
			if (anyPoint)
			{
				result = HitUtil.LineIntersectWithRect(new UnitPoint(this.position.X - 0.2, this.position.Y + 0.2), new UnitPoint(this.position.X + 0.2, this.position.Y - 0.2), rect);
			}
			else
			{
				result = rect.Contains(boundingRect);
			}
			return result;
		}

		public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
		{
		}

		public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			return eDrawObjectMouseDownEnum.Done;
		}

		public void OnMouseMove(ICanvas canvas, UnitPoint point)
		{
		}

		public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
		}

		public bool PointInObject(ICanvas canvas, UnitPoint point)
		{
			bool flag = !this.GetBoundingRect(canvas).Contains(point.Point);
			return !flag;
		}

		public void Redo()
		{
		}

		public void SetPosition(UnitPoint pos)
		{
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
