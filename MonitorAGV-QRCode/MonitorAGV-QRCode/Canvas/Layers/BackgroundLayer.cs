using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Canvas.Layers
{
	public class BackgroundLayer : ICanvasLayer, ISerialize
	{
		private Font m_font = new Font("Arial Black", 25f, FontStyle.Regular, GraphicsUnit.Point, 0);

		private SolidBrush m_brush = new SolidBrush(Color.FromArgb(50, 200, 200, 200));

		private SolidBrush m_backgroundBrush;

		private Color m_color = Color.White;

		[XmlSerializable]
		public Color Color
		{
			get
			{
				return this.m_color;
			}
			set
			{
				this.m_color = value;
				this.m_backgroundBrush = new SolidBrush(this.m_color);
			}
		}

		public string Id
		{
			get
			{
				return "BackGround";
			}
		}

		public IEnumerable<IDrawObject> Objects
		{
			get
			{
				return null;
			}
		}

		public bool Enabled
		{
			get;
			set;
		}

		public bool Visible
		{
			get;
			set;
		}

		public BackgroundLayer()
		{
			this.m_backgroundBrush = new SolidBrush(this.m_color);
			this.Enabled = true;
			this.Visible = true;
		}

		public void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				RectangleF rect = ScreenUtils.ToScreenNormalized(canvas, unitrect);
				bool flag = !float.IsNaN(rect.X) || !float.IsInfinity(rect.X) || !float.IsNaN(rect.Y) || !float.IsInfinity(rect.Y) || !float.IsNaN(rect.Width) || !float.IsInfinity(rect.Width) || !float.IsNaN(rect.Height) || !float.IsInfinity(rect.Height);
				if (flag)
				{
					canvas.Graphics.FillRectangle(this.m_backgroundBrush, rect);
				}
				canvas.Graphics.ResetTransform();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public PointF SnapPoint(PointF unitmousepoint)
		{
			return PointF.Empty;
		}

		ISnapPoint ICanvasLayer.SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj)
		{
			throw new Exception("背景布局不实现该方法!");
		}

		public void GetObjectData(XmlWriter wr)
		{
			wr.WriteStartElement("BackGroundLayer");
			XmlUtil.WriteProperties(this, wr);
			wr.WriteEndElement();
		}

		public void AfterSerializedIn()
		{
		}


      
    }
}
