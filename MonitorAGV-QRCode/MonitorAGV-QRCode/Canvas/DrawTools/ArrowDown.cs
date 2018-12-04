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
    public class ArrowDown : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        protected int mapNo;

        protected int x;

        protected int y;

        protected UnitPoint m_p1;

        protected UnitPoint m_p2;

        protected UnitPoint location = UnitPoint.Empty;

        public UnitPoint P1
        {
            get
            {
                return m_p1;
            }
            set
            {
                m_p1 = value;
            }
        }

        public UnitPoint P2
        {
            get
            {
                return m_p2;
            }
            set
            {
                m_p2 = value;
            }
        }

        public int MapNo
        {
            get
            {
                return mapNo;
            }
            set
            {
                mapNo = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

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
                return "ArrowDown";
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
                return m_p1;
            }
        }

        public IDrawObject Clone()
        {
            ArrowDown arrow = new ArrowDown();
            arrow.Copy(this);
            return arrow;
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
            Pen pen = new Pen(Color.Green);
            AdjustableArrowCap cap = new AdjustableArrowCap(2f, 2f);
            pen.CustomEndCap = cap;
            pen.Width = 4 * model.Zoom;

            float xStart = unitrect.X;
            float yStart = unitrect.Y;
            float xEnd = (unitrect.X + unitrect.Width);
            float yEnd = (unitrect.Y + unitrect.Height);

            if (location.X * model.Zoom + model.Zoom * (float)model.Distance / 2 >= xStart && location.X * model.Zoom - model.Zoom * (float)model.Distance / 2 <= xEnd && location.Y * model.Zoom + model.Zoom * (float)model.Distance / 2 >= yStart && location.Y * model.Zoom - model.Zoom * (float)model.Distance / 2 <= yEnd)
            {
                P1 = new UnitPoint(location.X, location.Y + 10);
                P2 = new UnitPoint(location.X, location.Y + 20);
                canvas.DrawLine(canvas, pen, P1, P2);
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
            wr.WriteStartElement("ArrowDownTool");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public void AfterSerializedIn()
        {
        }

        public override void InitializeFromModel(Canvas.UnitPoint point, Canvas.Layers.DrawingLayer layer, Canvas.CanvasInterfaces.ISnapPoint snap)
        {
            this.P2 = point;
            this.P1 = point;
            base.Width = layer.Width;
            base.Color = layer.Color;
            this.Selected = true;
        }

        public override void InitializeFromModel(CanvasCtrller cc, Canvas.UnitPoint point, Canvas.Layers.DrawingLayer layer, Canvas.CanvasInterfaces.ISnapPoint snap)
        {
            IModel model = cc.m_model;
            x = (int)((point.X - 20) / model.Distance);
            y = model.YCount - (int)((point.Y - 20) / model.Distance) - 1;
            mapNo = y * model.XCount + x;
            location = new UnitPoint(20 + X * model.Distance + (float)model.Distance / 2, 20 + (model.YCount - Y) * model.Distance - (float)model.Distance / 2);
            base.Width = layer.Width;
            base.Color = layer.Color;
            this.Selected = true;
        }
    }
}
