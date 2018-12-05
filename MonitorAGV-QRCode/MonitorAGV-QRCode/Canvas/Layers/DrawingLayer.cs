using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Canvas.Layers
{
	public class DrawingLayer : ICanvasLayer, ISerialize
	{
		private string m_id;

		private Color m_color;

		private float m_width = 0f;

		private bool m_enabled = true;

		private bool m_visible = true;

		private List<IDrawObject> m_objects = new List<IDrawObject>();

		private Dictionary<IDrawObject, bool> m_objectMap = new Dictionary<IDrawObject, bool>();

		public string Id
		{
			get
			{
				return this.m_id;
			}
		}

		public IEnumerable<IDrawObject> Objects
		{
			get
			{
				return this.m_objects;
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
		public float Width
		{
			get
			{
				return this.m_width;
			}
			set
			{
				this.m_width = value;
			}
		}

		[XmlSerializable]
		public bool Enabled
		{
			get
			{
				return this.m_enabled && this.m_visible;
			}
			set
			{
				this.m_enabled = value;
			}
		}

		[XmlSerializable]
		public bool Visible
		{
			get
			{
				return this.m_visible;
			}
			set
			{
				this.m_visible = value;
			}
		}

		public int Count
		{
			get
			{
				return this.m_objects.Count;
			}
		}

		public DrawingLayer(string id, Color color, float width)
		{
			this.m_id = id;
			this.m_color = color;
			this.m_width = width;
		}

		public void AddObject(IDrawObject drawobject)
		{
			try
			{
				bool flag = this.m_objectMap.ContainsKey(drawobject);
				if (!flag)
				{
					bool flag2 = drawobject is DrawObjectBase;
					if (flag2)
					{
						((DrawObjectBase)drawobject).Layer = this;
					}
					this.m_objects.Add(drawobject);
					this.m_objectMap[drawobject] = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<IDrawObject> DeleteObjects(IEnumerable<IDrawObject> objects)
		{
			List<IDrawObject> result;
			try
			{
				bool flag = !this.Enabled;
				if (flag)
				{
					result = null;
				}
				else
				{
					List<IDrawObject> list = new List<IDrawObject>();
					foreach (IDrawObject current in objects)
					{
						bool flag2 = this.m_objectMap.ContainsKey(current);
						if (flag2)
						{
							list.Add(current);
							this.m_objectMap.Remove(current);
						}
					}
					bool flag3 = list.Count == 0;
					if (flag3)
					{
						result = null;
					}
					else
					{
						bool flag4 = list.Count < 10;
						if (flag4)
						{
							foreach (IDrawObject current2 in list)
							{
								this.m_objects.Remove(current2);
							}
						}
						else
						{
							List<IDrawObject> list2 = new List<IDrawObject>();
							foreach (IDrawObject current3 in this.m_objects)
							{
								bool flag5 = this.m_objectMap.ContainsKey(current3);
								if (flag5)
								{
									list2.Add(current3);
								}
							}
							this.m_objects.Clear();
							this.m_objects = list2;
						}
						result = list;
					}
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
				int num = 0;
				foreach (IDrawObject current in m_objects)
				{
                    DrawObjectBase drawObjectBase = current as DrawObjectBase;
                    //bool flag = drawObjectBase is IDrawObject && !((IDrawObject)drawObjectBase).ObjectInRectangle(canvas, unitrect, true);
                    //if (!flag)
                    if (drawObjectBase is IDrawObject)
                    {
                        //bool selected = drawObjectBase.Selected;
                        //bool highlighted = drawObjectBase.Highlighted;
                        //drawObjectBase.Selected = false;
                        //try
                        //{
                            current.Draw(canvas, unitrect);
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex;
                        //}
                        //drawObjectBase.Selected = selected;
                        //drawObjectBase.Highlighted = highlighted;
                        int num2 = num;
                        num = num2 + 1;
                    }
				}
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

        public void Draw(ICanvas canvas, RectangleF unitrect, BufferedGraphics myBuffer, Rectangle rect, Graphics g)
        {
            foreach (IDrawObject current in m_objects)
            {
                DrawObjectBase drawObjectBase = current as DrawObjectBase;
                if (drawObjectBase is IDrawObject)
                {
                    current.Draw(canvas, unitrect,g);
                }
            }
        }

		public PointF SnapPoint(PointF unitmousepoint)
		{
			return PointF.Empty;
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj)
		{
			ISnapPoint result;
			foreach (IDrawObject current in this.m_objects)
			{
				ISnapPoint snapPoint = current.SnapPoint(canvas, point, otherobj, null, null);
				bool flag = snapPoint != null;
				if (flag)
				{
					result = snapPoint;
					return result;
				}
			}
			result = null;
			return result;
		}

		public void GetObjectData(XmlWriter wr)
		{
			try
			{
				wr.WriteStartElement("layer");
				wr.WriteAttributeString("Id", this.m_id);
				XmlUtil.WriteProperties(this, wr);
				wr.WriteStartElement("items");
				foreach (IDrawObject current in this.m_objects)
				{
					bool flag = current is ISerialize;
					if (flag)
					{
						((ISerialize)current).GetObjectData(wr);
					}
				}
				wr.WriteEndElement();
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

		public static DrawingLayer NewLayer(XmlElement xmlelement)
		{
			string text = xmlelement.GetAttribute("Id");
			bool flag = text.Length == 0;
			if (flag)
			{
				text = Guid.NewGuid().ToString();
			}
			DrawingLayer drawingLayer = new DrawingLayer(text, Color.White, 0f);
			foreach (XmlElement xmlElement in xmlelement.ChildNodes)
			{
				XmlUtil.ParseProperty(xmlElement, drawingLayer);
				bool flag2 = xmlElement.Name == "items";
				if (flag2)
				{
					foreach (XmlElement xmlElement2 in xmlElement.ChildNodes)
					{
						object obj = DataModel.NewDrawObject(xmlElement2.Name);
						bool flag3 = obj == null;
						if (!flag3)
						{
							bool flag4 = obj is IDrawObject;
							if (flag4)
							{
								drawingLayer.AddObject(obj as IDrawObject);
							}
							bool flag5 = obj != null;
							if (flag5)
							{
								XmlUtil.ParseProperties(xmlElement2, obj);
							}
							bool flag6 = obj is ISerialize;
							if (flag6)
							{
								((ISerialize)obj).AfterSerializedIn();
							}
						}
					}
				}
			}
			return drawingLayer;
		}
    }
}
