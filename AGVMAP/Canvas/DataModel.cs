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

		public bool IsDirty
		{
			get
			{
				return this.m_undoBuffer.Dirty;
			}
		}

		public UnitPoint CenterPoint
		{
			get
			{
				return this.m_centerPoint;
			}
			set
			{
				this.m_centerPoint = value;
			}
		}

		public float Zoom
		{
			get
			{
				return this.m_zoom;
			}
			set
			{
				this.m_zoom = value;
			}
		}

		public ICanvasLayer BackgroundLayer
		{
			get
			{
				return this.m_backgroundLayer;
			}
		}

		public ICanvasLayer GridLayer
		{
			get
			{
				return this.m_gridLayer;
			}
		}

		public ICanvasLayer[] Layers
		{
			get
			{
				return this.m_layers.ToArray();
			}
		}

		public ICanvasLayer ActiveLayer
		{
			get
			{
				bool flag = this.m_activeLayer == null;
				if (flag)
				{
					this.m_activeLayer = this.m_layers[0];
				}
				return this.m_activeLayer;
			}
			set
			{
				this.m_activeLayer = value;
			}
		}

		public IEnumerable<IDrawObject> SelectedObjects
		{
			get
			{
				return this.m_selection.Keys;
			}
		}

		public int SelectedCount
		{
			get
			{
				return this.m_selection.Count;
			}
		}

		public static IDrawObject NewDrawObject(string objecttype)
		{
			bool flag = DataModel.m_toolTypes.ContainsKey(objecttype);
			IDrawObject result;
			if (flag)
			{
				string typeName = DataModel.m_toolTypes[objecttype].ToString();
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
			bool flag = this.m_drawObjectTypes.ContainsKey(objecttype);
			DrawObjectBase result;
			if (flag)
			{
				result = (this.m_drawObjectTypes[objecttype].Clone() as DrawObjectBase);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void AddEditTool(string key, IEditTool tool)
		{
			this.m_editTools.Add(key, tool);
		}

		public DataModel()
		{
			DataModel.m_toolTypes.Clear();
			DataModel.m_toolTypes[LineTool.ObjectType] = typeof(LineTool);
			DataModel.m_toolTypes[LandMarkTool.ObjectType] = typeof(LandMarkTool);
			DataModel.m_toolTypes[ImgeTool.ObjectType] = typeof(ImgeTool);
			DataModel.m_toolTypes[TextTool.ObjectType] = typeof(TextTool);
			DataModel.m_toolTypes[BezierTool.ObjectType] = typeof(BezierTool);
			DataModel.m_toolTypes[StorageTool.ObjectType] = typeof(StorageTool);
			DataModel.m_toolTypes[ButtonTool.ObjectType] = typeof(ButtonTool);
			this.DefaultLayer();
			this.m_centerPoint = new UnitPoint(0.0, 0.0);
		}

		public void AddDrawTool(string key, IDrawObject drawtool)
		{
			this.m_drawObjectTypes[key] = drawtool;
		}

		public void Save(string filename)
		{
			try
			{
				bool flag = File.Exists(filename);
				if (flag)
				{
					File.Delete(filename);
				}
				XmlTextWriter xmlTextWriter = new XmlTextWriter(filename, null);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartElement("CanvasDataModel");
				this.m_backgroundLayer.GetObjectData(xmlTextWriter);
				this.m_gridLayer.GetObjectData(xmlTextWriter);
				foreach (ICanvasLayer current in this.m_layers)
				{
					bool flag2 = current is ISerialize;
					if (flag2)
					{
						((ISerialize)current).GetObjectData(xmlTextWriter);
					}
				}
				XmlUtil.WriteProperties(this, xmlTextWriter);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.Close();
				this.m_undoBuffer.Dirty = false;
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
				bool flag = documentElement.Name != "CanvasDataModel";
				if (flag)
				{
					result = false;
					return result;
				}
				this.m_layers.Clear();
				this.m_undoBuffer.Clear();
				this.m_undoBuffer.Dirty = false;
				foreach (XmlElement xmlElement in documentElement.ChildNodes)
				{
					bool flag2 = xmlElement.Name == "backgroundlayer";
					if (flag2)
					{
						XmlUtil.ParseProperties(xmlElement, this.m_backgroundLayer);
					}
					else
					{
						bool flag3 = xmlElement.Name == "gridlayer";
						if (flag3)
						{
							XmlUtil.ParseProperties(xmlElement, this.m_gridLayer);
						}
						else
						{
							bool flag4 = xmlElement.Name == "layer";
							if (flag4)
							{
								DrawingLayer item = DrawingLayer.NewLayer(xmlElement);
								this.m_layers.Add(item);
							}
							bool flag5 = xmlElement.Name == "property";
							if (flag5)
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
				this.DefaultLayer();
				Console.WriteLine("Load exception - {0}", ex.Message);
			}
			result = false;
			return result;
		}

		private void DefaultLayer()
		{
			this.m_layers.Clear();
			this.m_layers.Add(new DrawingLayer("1", Color.Green, 0.1f));
		}

		public IDrawObject GetFirstSelected()
		{
			bool flag = this.m_selection.Count > 0;
			IDrawObject result;
			if (flag)
			{
				Dictionary<IDrawObject, bool>.KeyCollection.Enumerator enumerator = this.m_selection.Keys.GetEnumerator();
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
			foreach (ICanvasLayer current in this.m_layers)
			{
				bool flag = current.Id == id;
				if (flag)
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
			DrawingLayer drawingLayer = this.ActiveLayer as DrawingLayer;
			bool flag = !drawingLayer.Enabled;
			IDrawObject result;
			if (flag)
			{
				result = null;
			}
			else
			{
				DrawObjectBase drawObjectBase = this.CreateObject(type);
				bool flag2 = drawObjectBase != null;
				if (flag2)
				{
					drawObjectBase.Layer = drawingLayer;
					drawObjectBase.InitializeFromModel(point, drawingLayer, snappoint);
				}
				result = (drawObjectBase as IDrawObject);
			}
			return result;
		}

		public void AddObject(ICanvasLayer layer, IDrawObject drawobject)
		{
			bool flag = drawobject is IObjectEditInstance;
			if (flag)
			{
				drawobject = ((IObjectEditInstance)drawobject).GetDrawObject();
			}
			bool canCapture = this.m_undoBuffer.CanCapture;
			if (canCapture)
			{
				this.m_undoBuffer.AddCommand(new EditCommandAdd(layer, drawobject));
			}
			((DrawingLayer)layer).AddObject(drawobject);
		}

		public void DeleteObjects(IEnumerable<IDrawObject> objects)
		{
			EditCommandRemove editCommandRemove = null;
			bool canCapture = this.m_undoBuffer.CanCapture;
			if (canCapture)
			{
				editCommandRemove = new EditCommandRemove();
			}
			foreach (ICanvasLayer current in this.m_layers)
			{
				List<IDrawObject> list = ((DrawingLayer)current).DeleteObjects(objects);
				bool flag = list != null && editCommandRemove != null;
				if (flag)
				{
					editCommandRemove.AddLayerObjects(current, list);
				}
			}
			bool flag2 = editCommandRemove != null;
			if (flag2)
			{
				this.m_undoBuffer.AddCommand(editCommandRemove);
			}
		}

		public void MoveObjects(UnitPoint offset, IEnumerable<IDrawObject> objects)
		{
			bool canCapture = this.m_undoBuffer.CanCapture;
			if (canCapture)
			{
				this.m_undoBuffer.AddCommand(new EditCommandMove(offset, objects));
			}
			foreach (IDrawObject current in objects)
			{
				current.Move(offset);
			}
		}

		public void CopyObjects(UnitPoint offset, IEnumerable<IDrawObject> objects)
		{
			this.ClearSelectedObjects();
			List<IDrawObject> list = new List<IDrawObject>();
			foreach (IDrawObject current in objects)
			{
				IDrawObject drawObject = current.Clone();
				list.Add(drawObject);
				drawObject.Move(offset);
				((DrawingLayer)this.ActiveLayer).AddObject(drawObject);
				this.AddSelectedObject(drawObject);
			}
			bool canCapture = this.m_undoBuffer.CanCapture;
			if (canCapture)
			{
				this.m_undoBuffer.AddCommand(new EditCommandAdd(this.ActiveLayer, list));
			}
		}

		public void AfterEditObjects(IEditTool edittool)
		{
			edittool.Finished();
			bool canCapture = this.m_undoBuffer.CanCapture;
			if (canCapture)
			{
				this.m_undoBuffer.AddCommand(new EditCommandEditTool(edittool));
			}
		}

		public IEditTool GetEditTool(string edittoolid)
		{
			bool flag = this.m_editTools.ContainsKey(edittoolid);
			IEditTool result;
			if (flag)
			{
				result = this.m_editTools[edittoolid].Clone();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void MoveNodes(UnitPoint position, IEnumerable<INodePoint> nodes)
		{
			bool canCapture = this.m_undoBuffer.CanCapture;
			if (canCapture)
			{
				this.m_undoBuffer.AddCommand(new EditCommandNodeMove(nodes));
			}
			foreach (INodePoint current in nodes)
			{
				IDrawObject original = current.GetOriginal();
				bool flag = (original is LineTool && (original as LineTool).Type == LineType.PointLine) || (original is BezierTool && (original as BezierTool).Type == BezierType.PointBezier);
				if (!flag)
				{
					current.SetPosition(position);
					current.Finish();
				}
			}
		}

		public List<IDrawObject> GetHitObjects(ICanvas canvas, RectangleF selection, bool anyPoint)
		{
			List<IDrawObject> list = new List<IDrawObject>();
			foreach (ICanvasLayer current in this.m_layers)
			{
				bool flag = !current.Visible;
				if (!flag)
				{
					foreach (IDrawObject current2 in current.Objects)
					{
						bool flag2 = current2.ObjectInRectangle(canvas, selection, anyPoint);
						if (flag2)
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
			foreach (ICanvasLayer current in this.m_layers)
			{
				bool flag = !current.Visible;
				if (!flag)
				{
					foreach (IDrawObject current2 in current.Objects)
					{
						bool flag2 = current2.PointInObject(canvas, point);
						if (flag2)
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
			return this.m_selection.ContainsKey(drawobject);
		}

		public void AddSelectedObject(IDrawObject drawobject)
		{
			DrawObjectBase drawObjectBase = drawobject as DrawObjectBase;
			this.RemoveSelectedObject(drawobject);
		    if (drawObjectBase is ImgeTool)
		    {
		        if (m_selection.Count <= 0)
		        {
                    this.m_selection[drawobject] = true;
                    drawObjectBase.Selected = true;
		        }
		    }
		    else
		    {
		        m_selection.Clear();
                this.m_selection[drawobject] = true;
                drawObjectBase.Selected = true;
		    }
		}

		public void RemoveSelectedObject(IDrawObject drawobject)
		{
			bool flag = this.m_selection.ContainsKey(drawobject);
			if (flag)
			{
				DrawObjectBase drawObjectBase = drawobject as DrawObjectBase;
				drawObjectBase.Selected = false;
				this.m_selection.Remove(drawobject);
			}
		}

		public void ClearSelectedObjects()
		{
			IEnumerable<IDrawObject> selectedObjects = this.SelectedObjects;
			foreach (IDrawObject current in selectedObjects)
			{
				DrawObjectBase drawObjectBase = current as DrawObjectBase;
				drawObjectBase.Selected = false;
			}
			this.m_selection.Clear();
		}

		public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, Type[] runningsnaptypes, Type usersnaptype)
		{
			List<IDrawObject> hitObjects = this.GetHitObjects(canvas, point);
			bool flag = hitObjects.Count == 0;
			ISnapPoint result;
			if (flag)
			{
				result = null;
			}
			else
			{
				foreach (IDrawObject current in hitObjects)
				{
					ISnapPoint snapPoint = current.SnapPoint(canvas, point, hitObjects, runningsnaptypes, usersnaptype);
					bool flag2 = snapPoint != null;
					if (flag2)
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
			return this.m_undoBuffer.CanUndo;
		}

		public bool DoUndo()
		{
			return this.m_undoBuffer.DoUndo(this);
		}

		public bool CanRedo()
		{
			return this.m_undoBuffer.CanRedo;
		}

		public bool DoRedo()
		{
			return this.m_undoBuffer.DoRedo(this);
		}
	}
}
