using Canvas.CanvasCtrl;
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
        //public enum eStyle
        //{
        //    Dots,
        //    Lines
        //}

        public SizeF m_spacing = new SizeF(1f, 1f);

        private bool m_enabled = true;

        private int m_minSize = 5;

        //private eStyle m_gridStyle = GridLayer.eStyle.Dots;

        private Color m_color = Color.FromArgb(255, 255, 255);

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

        //[XmlSerializable]
        //public eStyle GridStyle
        //{
        //    get
        //    {
        //        return this.m_gridStyle;
        //    }
        //    set
        //    {
        //        this.m_gridStyle = value;
        //    }
        //}

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
            //this.m_gridStyle = acopy.m_gridStyle;
            this.m_color = acopy.m_color;
        }

        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                if (Enabled)
                {
                    DataModel dataModel = canvas.DataModel as DataModel;
            
                    if (dataModel != null)
                    {
                        float xStart = unitrect.X ;
                        float yStart = unitrect.Y ;
                        float xEnd = (unitrect.X + unitrect.Width);
                        float yEnd = (unitrect.Y + unitrect.Height);

                        Graphics g = canvas.Graphics;
                        g.SmoothingMode = SmoothingMode.HighSpeed;
                        Pen pen = new Pen(Color.White);
                        pen.Width = 2;
                        float[] dashArray =
                            {
                                1*dataModel.Zoom, //线长5个像素
                                2*dataModel.Zoom //间断2个像素
                            };
                        pen.DashPattern = dashArray;
                        Pen pen2 = new Pen(Color.Green);
                        float[] dashArray2 =
                            {
                                4*dataModel.Zoom, //线长5个像素
                                16*dataModel.Zoom //间断2个像素
                            };
                        pen2.DashPattern = dashArray2;
                        pen2.Width = 2;
                        Font font = new Font("Tahoma", 6 * dataModel.Zoom, FontStyle.Regular);
                        GraphicsPath graphicsPath = new GraphicsPath();
                        GraphicsPath graphicsPath2 = new GraphicsPath();
                        float x1, y1, x2, y2;
                        int xC, yC, cC;
                        xC = yC = cC = 0;
                        int xMin, yMin, xMax, yMax;
                        xMin = yMin = xMax = yMax = 0;
                        bool isFirst = true;

                        for (int i = 0; i <= dataModel.XCount; i++)
                        {
                            x1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                            if (x1 >= xStart && x1 <= xEnd)
                            {
                                y1 = 20 * dataModel.Zoom;
                                y2 = (20 + dataModel.YCount * dataModel.Distance) * dataModel.Zoom;
                                if (y2 > yEnd)
                                {
                                    y2 = yEnd;
                                }
                                xMax = i;
                                graphicsPath.AddLine(x1, y1, x1, y2);
                                graphicsPath.CloseFigure();
                                xC++;
                            }
                            if (i != dataModel.XCount)
                            {
                                x1 = (20 + i * dataModel.Distance + dataModel.Distance / 2) * dataModel.Zoom;
                                if (x1 >= xStart && x1 <= xEnd)
                                {
                                    if (isFirst)
                                    {
                                        xMin = i;
                                        isFirst = false;
                                    }
                                    y1 = 20 * dataModel.Zoom;
                                    y2 = (20 + dataModel.YCount * dataModel.Distance) * dataModel.Zoom;
                                    graphicsPath2.AddLine(x1, y1, x1, y2);
                                    graphicsPath2.CloseFigure();
                                }
                            }
                        }

                        isFirst = true;
                        for (int i = 0; i <= dataModel.YCount; i++)
                        {
                            y1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                            if (y1 >= yStart && y1 <= yEnd)
                            {
                                x1 = 20 * dataModel.Zoom;
                                x2 = (20 + dataModel.XCount * dataModel.Distance) * dataModel.Zoom;
                                if (x2 > xEnd)
                                {
                                    x2 = xEnd;
                                }
                                yMax = i;
                                graphicsPath.AddLine(x1, y1, x2, y1);
                                graphicsPath.CloseFigure();
                                yC++;
                            }
                            if (i != dataModel.YCount)
                            {
                                y1 = (20 + i * dataModel.Distance + dataModel.Distance / 2) * dataModel.Zoom;
                                if (y1 >= yStart && y1 <= yEnd)
                                {
                                    if (isFirst)
                                    {
                                        yMin = i;
                                        isFirst = false;
                                    }
                                    x1 = 20 * dataModel.Zoom;
                                    x2 = (20 + dataModel.XCount * dataModel.Distance) * dataModel.Zoom;
                                    graphicsPath2.AddLine(x1, y1, x2, y1);
                                    graphicsPath2.CloseFigure();
                                }
                            }
                        }

                        //g.FillRectangle(Brushes.Red, unitrect);
                        for (int i = xMin; i <= xMax; i++)
                        {
                            for (int j = yMin; j <= yMax; j++)
                            {
                                x1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                                y1 = (20 + j * dataModel.Distance) * dataModel.Zoom;
                                if (i != dataModel.XCount && j != dataModel.YCount && x1 + dataModel.Distance * dataModel.Zoom / 2 >= xStart && x1 + dataModel.Distance * dataModel.Zoom / 2 <= xEnd && y1 + dataModel.Distance * dataModel.Zoom / 2 >= yStart && y1 + dataModel.Distance * dataModel.Zoom / 2 <= yEnd)
                                {
                                    g.DrawString(i.ToString().PadLeft(3,' ') + "," + (dataModel.YCount - j - 1), font, Brushes.White, (float)(x1 + dataModel.Distance * dataModel.Zoom / 8), (float)(y1 + dataModel.Distance * dataModel.Zoom * 0.4));
                                    cC++;
                                }
                            }
                        }

                        g.DrawPath(pen, graphicsPath);
                        g.DrawPath(pen2, graphicsPath2);
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
            return null;
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
