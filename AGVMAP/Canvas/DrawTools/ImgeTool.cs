using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.Properties;
using Canvas.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
	public class ImgeTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
	{
		private UnitPoint location = UnitPoint.Empty;

		private string imageStr = "";

		private Image img;

		private Color transcolor = Color.Transparent;

		private string upValue = "";

		private float height = 100f;

		private float width = 100f;

		private bool isviewable = true;

		protected static int ThresholdPixel = 0;

		[XmlSerializable, Description("位置坐标")]
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

		[XmlSerializable, Browsable(false)]
		public string ImageStr
		{
			get
			{
				return this.imageStr;
			}
			set
			{
				this.imageStr = value;
				bool flag = !string.IsNullOrEmpty(this.ImageStr);
				if (flag)
				{
					byte[] array = Convert.FromBase64String(this.ImageStr);
					MemoryStream memoryStream = new MemoryStream(array, 0, array.Length);
					memoryStream.Write(array, 0, array.Length);
					Image image = Image.FromStream(memoryStream);
					this.img = image;
				}
			}
		}

		[Description("图片")]
		public Image Imge
		{
			get
			{
				return this.img;
			}
			set
			{
				this.img = value;
				bool flag = this.img != null;
				if (flag)
				{
					MemoryStream memoryStream = new MemoryStream();
					this.img.Save(memoryStream, ImageFormat.Bmp);
					byte[] array = new byte[memoryStream.Length];
					memoryStream.Position = 0L;
					memoryStream.Read(array, 0, array.Length);
					memoryStream.Close();
					this.imageStr = Convert.ToBase64String(array);
				}
			}
		}

		[XmlSerializable, Description("图片透明背景色")]
		public Color TransColor
		{
			get
			{
				return this.transcolor;
			}
			set
			{
				this.transcolor = value;
			}
		}

		[Browsable(false)]
		public string UpValue
		{
			get
			{
				return this.upValue;
			}
			set
			{
				this.upValue = value;
			}
		}

		[XmlSerializable, Description("高度")]
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		[XmlSerializable, Description("宽度")]
		public float Wideth
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		[Browsable(false)]
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

		[Browsable(false)]
		public static string ObjectType
		{
			get
			{
				return "ImgeTool";
			}
		}

		[Browsable(false)]
		public virtual string Id
		{
			get
			{
				return ImgeTool.ObjectType;
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
				wr.WriteStartElement("ImgeTool");
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
				ImgeTool imgeTool = new ImgeTool();
				imgeTool.Copy(this);
				result = imgeTool;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public virtual void Copy(ImgeTool acopy)
		{
			try
			{
				base.Copy(acopy);
				this.location = acopy.location;
				this.img = acopy.img;
				this.imageStr = acopy.imageStr;
				this.width = acopy.width;
				this.height = acopy.height;
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
				float num = LineTool.ThresholdWidth(canvas, base.Width, (float)ImgeTool.ThresholdPixel);
				bool flag = num < base.Width;
				if (flag)
				{
					num = base.Width;
				}
				float screenvalue = canvas.ToScreen((double)(this.width / 96f));
				float screenvalue2 = canvas.ToScreen((double)(this.height / 96f));
				UnitPoint p = new UnitPoint(this.location.X + canvas.ToUnit(screenvalue), this.location.Y - canvas.ToUnit(screenvalue2));
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
					float screenvalue = canvas.ToScreen((double)(this.width / 96f));
					float screenvalue2 = canvas.ToScreen((double)(this.height / 96f));
					UnitPoint lp = new UnitPoint(this.location.X + canvas.ToUnit(screenvalue), this.location.Y - canvas.ToUnit(screenvalue2));
					result = HitUtil.LineIntersectWithRect(this.location, lp, rect);
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
				bool flag = !this.isviewable;
				if (!flag)
				{
					Pen pen = new Pen(Color.Transparent, 1f);
					bool flag2 = this.img == null;
					if (flag2)
					{
						this.img = Resources._default;
					}
					else
					{
						(this.img as Bitmap).MakeTransparent(this.transcolor);
						canvas.DrawImge(canvas, pen, this.location, this.Wideth, this.height, this.img, this.UpValue);
						bool selected = this.Selected;
						if (selected)
						{
							canvas.DrawImge(canvas, DrawUtils.SelectedPen, this.location, this.Wideth, this.height, this.img, this.UpValue);
						}
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
		}

		public virtual eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
		{
			eDrawObjectMouseDownEnum result;
			try
			{
				bool flag = !this.isviewable;
				if (flag)
				{
					result = eDrawObjectMouseDownEnum.Done;
				}
				else
				{
					this.Selected = false;
					this.location = point;
					Pen pen = new Pen(Color.Transparent, 1f);
					bool flag2 = this.img == null;
					if (flag2)
					{
						this.img = Resources._default;
						result = eDrawObjectMouseDownEnum.Done;
					}
					else
					{
						(this.img as Bitmap).MakeTransparent(this.transcolor);
						canvas.DrawImge(canvas, pen, this.location, this.Wideth, this.height, this.img, this.UpValue);
						result = eDrawObjectMouseDownEnum.Done;
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
			return "ImageTool";
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
	}
}
