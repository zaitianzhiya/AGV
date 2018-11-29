﻿using System;
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
    public class Forbid : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        protected int mapNo;

        protected int x;

        protected int y;

        protected UnitPoint location = UnitPoint.Empty;

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
                return "Forbid";
            }
        }

        public string Id
        {
            get
            {
                return ObjectType;
            }
        }

        public IDrawObject Clone()
        {
            Forbid forbid = new Forbid();
            forbid.Copy(this);
            return forbid;
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
            Pen pen = new Pen(Color.Red);
            pen.Width = 4 * model.Zoom;

            float xStart = unitrect.X;
            float yStart = unitrect.Y;
            float xEnd = (unitrect.X + unitrect.Width);
            float yEnd = (unitrect.Y + unitrect.Height);

            if (location.X * model.Zoom + model.Zoom * model.Distance >= xStart && location.X * model.Zoom <= xEnd && location.Y * model.Zoom + model.Zoom * model.Distance >= yStart && location.Y * model.Zoom <= yEnd)
            {
                canvas.DrawForbid(canvas,pen,location);
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
            throw new NotImplementedException();
        }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            Selected = false;
            return eDrawObjectMouseDownEnum.Done;
        }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            throw new NotImplementedException();
        }

        public void OnKeyDown(ICanvas canvas, System.Windows.Forms.KeyEventArgs e)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Finish()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(UnitPoint pos)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void GetObjectData(System.Xml.XmlWriter wr)
        {
            wr.WriteStartElement("ForbidTool");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public void AfterSerializedIn()
        {
            throw new NotImplementedException();
        }

        public override void InitializeFromModel(Canvas.UnitPoint point, Canvas.Layers.DrawingLayer layer, Canvas.CanvasInterfaces.ISnapPoint snap)
        {
            this.location = point;
            base.Width = layer.Width;
            base.Color = layer.Color;
            this.Selected = true;
        }

        public UnitPoint RepeatStartingPoint
        {
            get { throw new NotImplementedException(); }
        }

        public override void InitializeFromModel(CanvasCtrller cc, Canvas.UnitPoint point, Canvas.Layers.DrawingLayer layer, Canvas.CanvasInterfaces.ISnapPoint snap)
        {
            IModel model = cc.m_model;
            x = (int)((point.X - 20) / model.Distance);
            y = model.YCount - (int)((point.Y - 20) / model.Distance) - 1;
            mapNo = y * model.XCount + x + 1;
            location = new UnitPoint(20 + X * model.Distance + (float)model.Distance / 2, 20 + (model.YCount - Y) * model.Distance - (float)model.Distance / 2);
            base.Width = layer.Width;
            base.Color = layer.Color;
            this.Selected = true;
        }
    }
}