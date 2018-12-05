using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using Canvas.Utils;

namespace Canvas.DrawTools
{
    public class AGVTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        public int MapNo { get; set; }

        //小车编号
        protected string agvNo;

        public string AgvNo
        {
            get
            {
                return agvNo;
            }
            set
            {
                agvNo = value;
            }
        }

        //小车颜色
        protected Color agvColor;

        public Color AgvColor
        {
            get
            {
                return agvColor;
            }
            set
            {
                agvColor = value;
            }
        }

        //小车角度
        protected float angle;

        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }


        protected UnitPoint location = UnitPoint.Empty;

        public UnitPoint Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
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
                return ObjectType;
            }
        }

        public UnitPoint RepeatStartingPoint
        {
            get
            {
                return location;
            }
        }

        public IDrawObject Clone()
        {
            AGVTool agv = new AGVTool();
            agv.Copy(this);
            return agv;
        }

        public bool PointInObject(ICanvas canvas, UnitPoint point)
        {
             return GetBoundingRect(canvas).Contains(point.Point);
        }

        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            throw new NotImplementedException();
            //bool result;
            //try
            //{
            //    RectangleF boundingRect = GetBoundingRect(canvas);
            //    if (anyPoint)
            //    {
            //        result = HitUtil.LineIntersectWithRect(location, this.aqlocation, rect);
            //    }
            //    else
            //    {
            //        result = rect.Contains(boundingRect);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return result;
        }

        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            CanvasWrapper canvasWrapper = (CanvasWrapper)canvas;
            DataModel model = (DataModel)canvasWrapper.DataModel;

            float xStart = unitrect.X;
            float yStart = unitrect.Y;
            float xEnd = (unitrect.X + unitrect.Width);
            float yEnd = (unitrect.Y + unitrect.Height);

            if (location.X * model.Zoom + model.Zoom * (float)model.Distance / 2 >= xStart && location.X * model.Zoom - model.Zoom * (float)model.Distance / 2 <= xEnd && location.Y * model.Zoom + model.Zoom * (float)model.Distance / 2 >= yStart && location.Y * model.Zoom - model.Zoom * (float)model.Distance / 2 <= yEnd)
            {
                canvas.DrawAgv(canvas, agvNo, agvColor, Angle,location);
            }
        }

        public void Draw(ICanvas canvas, RectangleF unitrect, Graphics g)
        {
            CanvasWrapper canvasWrapper = (CanvasWrapper)canvas;
            DataModel model = (DataModel)canvasWrapper.DataModel;

            float xStart = unitrect.X;
            float yStart = unitrect.Y;
            float xEnd = (unitrect.X + unitrect.Width);
            float yEnd = (unitrect.Y + unitrect.Height);

            if (location.X * model.Zoom + model.Zoom * (float)model.Distance / 2 >= xStart && location.X * model.Zoom - model.Zoom * (float)model.Distance / 2 <= xEnd && location.Y * model.Zoom + model.Zoom * (float)model.Distance / 2 >= yStart && location.Y * model.Zoom - model.Zoom * (float)model.Distance / 2 <= yEnd)
            {
                canvas.DrawAgv(canvas, agvNo, agvColor, Angle, location,g);
            }
        }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            CanvasWrapper canvasWrapper = (CanvasWrapper)canvas;
            DataModel model = (DataModel)canvasWrapper.DataModel;
            return ScreenUtils.GetRect(location, location, (double)model.Distance/2);
        }

        public void OnMouseMove(ICanvas canvas, UnitPoint point)
        {
        }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            Selected = false;
            return eDrawObjectMouseDownEnum.Done;
        }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
        }

        public void OnKeyDown(ICanvas canvas, System.Windows.Forms.KeyEventArgs e)
        {
        }

        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            throw new NotImplementedException();
        }

        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
        {
            throw new NotImplementedException();
        }

        public void Move(UnitPoint offset)
        {
        }

        public string GetInfoAsString()
        {
            throw new NotImplementedException();
        }

        public IDrawObject GetClone()
        {
            throw new NotImplementedException();
        }

        public IDrawObject GetOriginal()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
        }

        public void Finish()
        {
        }

        public void SetPosition(UnitPoint pos)
        {
        }

        public void Undo()
        {
        }

        public void Redo()
        {
        }

        public void GetObjectData(System.Xml.XmlWriter wr)
        {
            wr.WriteStartElement("AGVTool");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public void AfterSerializedIn()
        {
            throw new NotImplementedException();
        }

        public override void InitializeFromModel(Canvas.UnitPoint point, Canvas.Layers.DrawingLayer layer, Canvas.CanvasInterfaces.ISnapPoint snap)
        {
            base.Width = layer.Width;
            base.Color = layer.Color;
            this.Selected = true;
        }

        public override void InitializeFromModel(CanvasCtrller cc,Canvas.UnitPoint point, Canvas.Layers.DrawingLayer layer, Canvas.CanvasInterfaces.ISnapPoint snap)
        {
         
        }
    }
}
