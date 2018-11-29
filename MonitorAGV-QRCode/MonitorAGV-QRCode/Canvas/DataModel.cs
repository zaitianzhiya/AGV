using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Canvas
{
	public class DataModel : IModel
	{
		private static Dictionary<string, Type> m_toolTypes = new Dictionary<string, Type>();

		private Dictionary<string, IDrawObject> m_drawObjectTypes = new Dictionary<string, IDrawObject>();

		private Dictionary<string, IEditTool> m_editTools = new Dictionary<string, IEditTool>();

		private UndoRedoBuffer m_undoBuffer = new UndoRedoBuffer();

		private UnitPoint m_centerPoint = UnitPoint.Empty;

		private float m_zoom = 0.5f;

		private GridLayer m_gridLayer = new GridLayer();

		private BackgroundLayer m_backgroundLayer = new BackgroundLayer();

		private List<ICanvasLayer> m_layers = new List<ICanvasLayer>();

		private ICanvasLayer m_activeLayer;

		private Dictionary<IDrawObject, bool> m_selection = new Dictionary<IDrawObject, bool>();

        public int XCount { get; set; }
        public int YCount { get; set; }
        public int Distance { get; set; }
		public bool IsDirty
		{
			get
			{
				return m_undoBuffer.Dirty;
			}
		}

		public UnitPoint CenterPoint
		{
			get
			{
				return m_centerPoint;
			}
			set
			{
				m_centerPoint = value;
			}
		}

		public float Zoom
		{
			get
			{
				return m_zoom;
			}
			set
			{
				m_zoom = value;
			}
		}

		public ICanvasLayer BackgroundLayer
		{
			get
			{
				return m_backgroundLayer;
			}
		}

		public ICanvasLayer GridLayer
		{
			get
			{
				return m_gridLayer;
			}
		}

		public ICanvasLayer[] Layers
		{
			get
			{
				return m_layers.ToArray();
			}
		}

		public ICanvasLayer ActiveLayer
		{
			get
			{
				bool flag = m_activeLayer == null;
				if (flag)
				{
					m_activeLayer = m_layers[0];
				}
				return m_activeLayer;
			}
			set
			{
				m_activeLayer = value;
			}
		}

		public IEnumerable<IDrawObject> SelectedObjects
		{
			get
			{
				return m_selection.Keys;
			}
		}

		public int SelectedCount
		{
			get
			{
				return m_selection.Count;
			}
		}

		public static IDrawObject NewDrawObject(string objecttype)
		{
			IDrawObject result;
			if (m_toolTypes.ContainsKey(objecttype))
			{
				string typeName = m_toolTypes[objecttype].ToString();
				result = (Assembly.GetExecutingAssembly().CreateInstance(typeName) as IDrawObject);
			}
			else
			{
				result = null;
			}
			return result;
		}

		private DrawObjectBase CreateObject(string objecttype)
		{
			DrawObjectBase result;
			if (m_drawObjectTypes.ContainsKey(objecttype))
			{
				result = (m_drawObjectTypes[objecttype].Clone() as DrawObjectBase);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void AddEditTool(string key, IEditTool tool)
		{
			m_editTools.Add(key, tool);
		}

		public DataModel()
		{
			m_toolTypes.Clear();
            //m_toolTypes[LineTool.ObjectType] = typeof(LineTool);
            //m_toolTypes[LandMarkTool.ObjectType] = typeof(LandMarkTool);
            //m_toolTypes[ImgeTool.ObjectType] = typeof(ImgeTool);
            //m_toolTypes[TextTool.ObjectType] = typeof(TextTool);
            //m_toolTypes[BezierTool.ObjectType] = typeof(BezierTool);
            //m_toolTypes[StorageTool.ObjectType] = typeof(StorageTool);
            //m_toolTypes[ButtonTool.ObjectType] = typeof(ButtonTool);
            m_toolTypes[ArrowLeft.ObjectType] = typeof(ArrowLeft);
            m_toolTypes[ArrowRight.ObjectType] = typeof(ArrowRight);
            m_toolTypes[ArrowUp.ObjectType] = typeof(ArrowUp);
            m_toolTypes[ArrowDown.ObjectType] = typeof(ArrowDown);
            m_toolTypes[ChargeTool.ObjectType] = typeof(ChargeTool);
            m_toolTypes[Forbid.ObjectType] = typeof(Forbid);
			DefaultLayer();
			m_centerPoint = new UnitPoint(0.0, 0.0);
		}

		public void AddDrawTool(string key, IDrawObject drawtool)
		{
			m_drawObjectTypes[key] = drawtool;
		}

		public void Save(string filename)
		{
			try
			{
				if (File.Exists(filename))
				{
					File.Delete(filename);
				}
				XmlTextWriter xmlTextWriter = new XmlTextWriter(filename, null);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartElement("CanvasDataModel");
				m_backgroundLayer.GetObjectData(xmlTextWriter);
				m_gridLayer.GetObjectData(xmlTextWriter);
				foreach (ICanvasLayer current in m_layers)
				{
					if (current is ISerialize)
					{
						((ISerialize)current).GetObjectData(xmlTextWriter);
					}
				}
				XmlUtil.WriteProperties(this, xmlTextWriter);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.Close();
				m_undoBuffer.Dirty = false;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool Load(string filename)
		{
			bool result;
			try
			{
				StreamReader streamReader = new StreamReader(filename);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(streamReader);
				streamReader.Dispose();
				XmlElement documentElement = xmlDocument.DocumentElement;
				if (documentElement.Name != "CanvasDataModel")
				{
					result = false;
					return result;
				}
				m_layers.Clear();
				m_undoBuffer.Clear();
				m_undoBuffer.Dirty = false;
				foreach (XmlElement xmlElement in documentElement.ChildNodes)
				{
					if (xmlElement.Name == "backgroundlayer")
					{
						XmlUtil.ParseProperties(xmlElement, m_backgroundLayer);
					}
					else
					{
						if (xmlElement.Name == "gridlayer")
						{
							XmlUtil.ParseProperties(xmlElement, m_gridLayer);
						}
						else
						{
							if (xmlElement.Name == "layer")
							{
								DrawingLayer item = DrawingLayer.NewLayer(xmlElement);
								m_layers.Add(item);
							}
							if (xmlElement.Name == "property")
							{
								XmlUtil.ParseProperty(xmlElement, this);
							}
						}
					}
				}
				result = true;
				return result;
			}
			catch (Exception ex)
			{
				DefaultLayer();
				Console.WriteLine("Load exception - {0}", ex.Message);
			}
			result = false;
			return result;
		}

		private void DefaultLayer()
		{
			m_layers.Clear();
			m_layers.Add(new DrawingLayer("1", Color.Green, 0.1f));
		}

		public IDrawObject GetFirstSelected()
		{
			IDrawObject result;
			if (m_selection.Count > 0)
			{
				Dictionary<IDrawObject, bool>.KeyCollection.Enumerator enumerator = m_selection.Keys.GetEnumerator();
				enumerator.MoveNext();
				result = enumerator.Current;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public ICanvasLayer GetLayer(string id)
		{
			ICanvasLayer result;
			foreach (ICanvasLayer current in m_layers)
			{
				if (current.Id == id)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public IDrawObject CreateObject(string type, UnitPoint point, ISnapPoint snappoint)
		{
			DrawingLayer drawingLayer = ActiveLayer as DrawingLayer;
			IDrawObject result;
			if (!drawingLayer.Enabled)
			{
				result = null;
			}
			else
			{
				DrawObjectBase drawObjectBase = CreateObject(type);
				if (drawObjectBase != null)
				{
					drawObjectBase.Layer = drawingLayer;
					drawObjectBase.InitializeFromModel(point, drawingLayer, snappoint);
				}
				result = (drawObjectBase as IDrawObject);
			}
			return result;
		}

        public IDrawObject CreateObject(CanvasCtrller cc, string type, UnitPoint point, ISnapPoint snappoint)
        {
            DrawingLayer drawingLayer = ActiveLayer as DrawingLayer;
            IDrawObject result;
            if (!drawingLayer.Enabled)
            {
                result = null;
            }
            else
            {
                DrawObjectBase drawObjectBase = CreateObject(type);
                if (drawObjectBase != null)
                {
                    drawObjectBase.Layer = drawingLayer;
                    drawObjectBase.InitializeFromModel(cc,point, drawingLayer, snappoint);
                }
                result = (drawObjectBase as IDrawObject);
            }
            return result;
        }

		public void AddObject(ICanvasLayer layer, IDrawObject drawobject)
		{
			if (drawobject is IObjectEditInstance)
			{
				drawobject = ((IObjectEditInstance)drawobject).GetDrawObject();
			}
			if (m_undoBuffer.CanCapture)
			{
				m_undoBuffer.AddCommand(new EditCommandAdd(layer, drawobject));
			}
			((DrawingLayer)layer).AddObject(drawobject);
		}

		public void DeleteObjects(IEnumerable<IDrawObject> objects)
		{
			EditCommandRemove editCommandRemove = null;
			if (m_undoBuffer.CanCapture)
			{
				editCommandRemove = new EditCommandRemove();
			}
			foreach (ICanvasLayer current in this.m_layers)
			{
				List<IDrawObject> list = ((DrawingLayer)current).DeleteObjects(objects);
				if (list != null && editCommandRemove != null)
				{
					editCommandRemove.AddLayerObjects(current, list);
				}
			}
			if (editCommandRemove != null)
			{
				m_undoBuffer.AddCommand(editCommandRemove);
			}
		}

		public void MoveObjects(UnitPoint offset, IEnumerable<IDrawObject> objects)
		{
			if (m_undoBuffer.CanCapture)
			{
				m_undoBuffer.AddCommand(new EditCommandMove(offset, objects));
			}
			foreach (IDrawObject current in objects)
			{
				current.Move(offset);
			}
		}

		public void CopyObjects(UnitPoint offset, IEnumerable<IDrawObject> objects)
		{
			ClearSelectedObjects();
			List<IDrawObject> list = new List<IDrawObject>();
			foreach (IDrawObject current in objects)
			{
				IDrawObject drawObject = current.Clone();
				list.Add(drawObject);
				drawObject.Move(offset);
				((DrawingLayer)ActiveLayer).AddObject(drawObject);
				AddSelectedObject(drawObject);
			}
			if ( m_undoBuffer.CanCapture)
			{
				m_undoBuffer.AddCommand(new EditCommandAdd(ActiveLayer, list));
			}
		}

		public void AfterEditObjects(IEditTool edittool)
		{
			edittool.Finished();
			if (m_undoBuffer.CanCapture)
			{
				m_undoBuffer.AddCommand(new EditCommandEditTool(edittool));
			}
		}

		public IEditTool GetEditTool(string edittoolid)
		{
			IEditTool result;
			if (m_editTools.ContainsKey(edittoolid))
			{
				result = m_editTools[edittoolid].Clone();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void MoveNodes(UnitPoint position, IEnumerable<INodePoint> nodes)
		{

		}

		public List<IDrawObject> GetHitObjects(ICanvas canvas, RectangleF selection, bool anyPoint)
		{
			List<IDrawObject> list = new List<IDrawObject>();
			foreach (ICanvasLayer current in this.m_layers)
			{
				if (current.Visible)
				{
					foreach (IDrawObject current2 in current.Objects)
					{
						if (current2.ObjectInRectangle(canvas, selection, anyPoint))
						{
							list.Add(current2);
						}
					}
				}
			}
			return list;
		}

		public List<IDrawObject> GetHitObjects(ICanvas canvas, UnitPoint point)
		{
			List<IDrawObject> list = new List<IDrawObject>();
			foreach (ICanvasLayer current in m_layers)
			{
				if (current.Visible)
				{
					foreach (IDrawObject current2 in current.Objects)
					{
						if (current2.PointInObject(canvas, point))
						{
							list.Add(current2);
						}
					}
				}
			}
			return list;
		}

		public bool IsSelected(IDrawObject drawobject)
		{
			return m_selection.ContainsKey(drawobject);
		}

		public void AddSelectedObject(IDrawObject drawobject)
		{
			
		}

		public void RemoveSelectedObject(IDrawObject drawobject)
		{
			if (m_selection.ContainsKey(drawobject))
			{
				DrawObjectBase drawObjectBase = drawobject as DrawObjectBase;
				drawObjectBase.Selected = false;
				m_selection.Remove(drawobject);
			}
		}

		public void ClearSelectedObjects()
		{
			IEnumerable<IDrawObject> selectedObjects = SelectedObjects;
			foreach (IDrawObject current in selectedObjects)
			{
				DrawObjectBase drawObjectBase = current as DrawObjectBase;
				drawObjectBase.Selected = false;
			}
			m_selection.Clear();
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, Type[] runningsnaptypes, Type usersnaptype)
		{
			List<IDrawObject> hitObjects = GetHitObjects(canvas, point);
			ISnapPoint result;
			if (hitObjects.Count == 0)
			{
				result = null;
			}
			else
			{
				foreach (IDrawObject current in hitObjects)
				{
					ISnapPoint snapPoint = current.SnapPoint(canvas, point, hitObjects, runningsnaptypes, usersnaptype);
					if (snapPoint != null)
					{
						result = snapPoint;
						return result;
					}
				}
				result = null;
			}
			return result;
		}

		public bool CanUndo()
		{
			return m_undoBuffer.CanUndo;
		}

		public bool DoUndo()
		{
			return m_undoBuffer.DoUndo(this);
		}

		public bool CanRedo()
		{
			return m_undoBuffer.CanRedo;
		}

		public bool DoRedo()
		{
			return m_undoBuffer.DoRedo(this);
		}
        }
}
