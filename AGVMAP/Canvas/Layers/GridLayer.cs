using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace Canvas.Layers
{
	public class GridLayer : ICanvasLayer, ISerialize
	{
		public enum eStyle
		{
			Dots,
			Lines
		}

		public SizeF m_spacing = new SizeF(0.5f, 0.5f);

		private bool m_enabled = true;

		private int m_minSize = 1;

		private GridLayer.eStyle m_gridStyle = GridLayer.eStyle.Dots;

		private Color m_color = Color.FromArgb(216, 216, 216);

		public string Id
		{
			get
			{
				return "grid";
			}
		}

		[XmlSerializable]
		public SizeF Spacing
		{
			get
			{
				return this.m_spacing;
			}
			set
			{
				this.m_spacing = value;
			}
		}

		[XmlSerializable]
		public int MinSize
		{
			get
			{
				return this.m_minSize;
			}
			set
			{
				this.m_minSize = value;
			}
		}

		[XmlSerializable]
		public GridLayer.eStyle GridStyle
		{
			get
			{
				return this.m_gridStyle;
			}
			set
			{
				this.m_gridStyle = value;
			}
		}

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
			}
		}

		[XmlSerializable]
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
			}
		}

		public bool Visible
		{
			get
			{
				return true;
			}
		}

		public IEnumerable<IDrawObject> Objects
		{
			get
			{
				return null;
			}
		}

		public void Copy(GridLayer acopy)
		{
			this.m_enabled = acopy.m_enabled;
			this.m_spacing = acopy.m_spacing;
			this.m_minSize = acopy.m_minSize;
			this.m_gridStyle = acopy.m_gridStyle;
			this.m_color = acopy.m_color;
		}

		public void Draw(ICanvas canvas, RectangleF unitrect)
		{
			try
			{
				bool flag = !this.Enabled;
				if (!flag)
				{
					float width = this.Spacing.Width;
					float height = this.Spacing.Height;
					float num = canvas.ToScreen((double)width);
					float num2 = canvas.ToScreen((double)height);
					bool flag2 = num < (float)this.MinSize || num2 < (float)this.MinSize;
					if (!flag2)
					{
						PointF location = unitrect.Location;
						PointF pointF = ScreenUtils.RightPoint(canvas, unitrect);
						float num3 = (float)Math.Round((double)(location.X / width)) * width;
						float num4 = unitrect.Height + unitrect.Y;
						float x = pointF.X;
						float num5 = (float)Math.Round((double)(location.Y / height)) * height;
						bool flag3 = this.GridStyle == GridLayer.eStyle.Dots;
						if (flag3)
						{
							GDI gDI = new GDI();
							gDI.BeginGDI(canvas.Graphics);
							for (float num6 = num3; num6 <= x; num6 += width)
							{
								for (float num7 = num5; num7 <= num4; num7 += height)
								{
									PointF pointF2 = canvas.ToScreen(new UnitPoint((double)num6, (double)num7));
									gDI.SetPixel((int)pointF2.X, (int)pointF2.Y, this.m_color.ToArgb());
								}
							}
							gDI.EndGDI();
						}
						bool flag4 = this.GridStyle == GridLayer.eStyle.Lines;
						if (flag4)
						{
							Pen pen = new Pen(this.m_color);
							GraphicsPath graphicsPath = new GraphicsPath();
							while (num3 < x)
							{
								PointF pt = canvas.ToScreen(new UnitPoint((double)num3, (double)location.Y));
								PointF pt2 = canvas.ToScreen(new UnitPoint((double)num3, (double)pointF.Y));
								graphicsPath.AddLine(pt, pt2);
								graphicsPath.CloseFigure();
								num3 += width;
							}
							while (num5 < num4)
							{
								PointF pt3 = canvas.ToScreen(new UnitPoint((double)location.X, (double)num5));
								PointF pt4 = canvas.ToScreen(new UnitPoint((double)pointF.X, (double)num5));
								graphicsPath.AddLine(pt3, pt4);
								graphicsPath.CloseFigure();
								num5 += height;
							}
							canvas.Graphics.DrawPath(pen, graphicsPath);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj)
		{
			ISnapPoint result;
			try
			{
				bool flag = !this.Enabled;
				if (flag)
				{
					result = null;
				}
				else
				{
					UnitPoint snappoint = default(UnitPoint);
					UnitPoint unitPoint = point;
					float width = this.Spacing.Width;
					float height = this.Spacing.Height;
					snappoint.X = (double)((float)Math.Round(unitPoint.X / (double)width) * width);
					snappoint.Y = (double)((float)Math.Round(unitPoint.Y / (double)height) * height);
					double num = canvas.ToUnit(6f);
					bool flag2 = snappoint.X < point.X - num || snappoint.X > point.X + num;
					if (flag2)
					{
						result = null;
					}
					else
					{
						bool flag3 = snappoint.Y < point.Y - num || snappoint.Y > point.Y + num;
						if (flag3)
						{
							result = null;
						}
						else
						{
							result = new GridSnapPoint(canvas, snappoint);
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

		public void GetObjectData(XmlWriter wr)
		{
			wr.WriteStartElement("gridlayer");
			XmlUtil.WriteProperties(this, wr);
			wr.WriteEndElement();
		}

		public void AfterSerializedIn()
		{
		}
	}
}
