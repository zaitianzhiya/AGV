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
        public SizeF m_spacing = new SizeF(1f, 1f);

        private bool m_enabled = true;

        private int m_minSize = 5;

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
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        Pen pen = new Pen(Color.White);
                        pen.Width = 2;
                        float[] dashArray =
                            {
                                1*dataModel.Zoom,
                                2*dataModel.Zoom
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

                        float oneDistance = dataModel.Distance*dataModel.Zoom;

                        int startIndexX = (int)((xStart - 20 * dataModel.Zoom) / oneDistance) - 1;
                        int endIndexX = (int)((xEnd - 20 * dataModel.Zoom) / oneDistance) + 1;
                        int startIndexY = (int)((yStart - 20 * dataModel.Zoom) / oneDistance) - 1;
                        int endIndexY = (int)((yEnd - 20 * dataModel.Zoom) / oneDistance) + 1;

                        if (startIndexX < 0)
                        {
                            startIndexX = 0;
                        }
                        if (endIndexX > dataModel.XCount)
                        {
                            endIndexX = dataModel.XCount;
                        }
                        if (startIndexY < 0)
                        {
                            startIndexY = 0;
                        }
                        if (endIndexY > dataModel.YCount)
                        {
                            endIndexY = dataModel.YCount;
                        }

                        for (int i = startIndexX; i <= endIndexX; i++)
                        {
                            x1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                            if (x1 >= xStart && x1 <= xEnd&&i!=0)
                            {
                                y1 = 20 * dataModel.Zoom;
                                if (y1 < yStart)
                                {
                                    y1 = yStart;
                                }
                                y2 = (20 + dataModel.YCount * dataModel.Distance) * dataModel.Zoom;
                                if (y2 > yEnd)
                                {
                                    y2 = yEnd;
                                }
                                graphicsPath.AddLine(x1, y1, x1, y2);
                                graphicsPath.CloseFigure();
                                xC++;
                            }

                            if (i != dataModel.XCount)
                            {
                                x1 = (20 + i * dataModel.Distance + dataModel.Distance/2) * dataModel.Zoom;
                                if (x1 >= xStart && x1 <= xEnd && x1 > 20 * dataModel.Zoom)
                                {
                                    //y1 = 20 * dataModel.Zoom;
                                    //y2 = (20 + dataModel.YCount * dataModel.Distance) * dataModel.Zoom;
                                    y1 = (20 + startIndexY * dataModel.Distance) * dataModel.Zoom;
                                    y2 = (20 + endIndexY * dataModel.Distance) * dataModel.Zoom;
                                    graphicsPath2.AddLine(x1, y1, x1, y2);
                                    graphicsPath2.CloseFigure();
                                }
                            }
                        }

                        for (int i = startIndexY; i <= endIndexY; i++)
                        {
                            y1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                            if (y1 >= yStart && y1 <= yEnd && i != dataModel.YCount)
                            {
                                x1 = 20 * dataModel.Zoom;
                                if (x1 < xStart)
                                {
                                    x1 = xStart;
                                }
                                x2 = (20 + dataModel.XCount * dataModel.Distance) * dataModel.Zoom;
                                if (x2 > xEnd)
                                {
                                    x2 = xEnd;
                                }
                                graphicsPath.AddLine(x1, y1, x2, y1);
                                graphicsPath.CloseFigure();
                                yC++;
                            }
                            if (i != dataModel.YCount)
                            {
                                y1 = (20 + i * dataModel.Distance + dataModel.Distance / 2) * dataModel.Zoom;
                                if (y1 >= yStart && y1 <= yEnd && y1 > 20 * dataModel.Zoom)
                                {
                                    //x1 = 20 * dataModel.Zoom;
                                    //x2 = (20 + dataModel.XCount * dataModel.Distance) * dataModel.Zoom;
                                    x1 = (20 + startIndexX * dataModel.Distance) * dataModel.Zoom;
                                    x2 = (20 + endIndexX * dataModel.Distance) * dataModel.Zoom;
                                    graphicsPath2.AddLine(x1, y1, x2, y1);
                                    graphicsPath2.CloseFigure();
                                }
                            }
                        }

                        g.TranslateTransform(-unitrect.X, -unitrect.Y);
                        //for (int i = startIndexX; i <= endIndexX; i++)
                        //{
                        //    for (int j = startIndexY; j <= endIndexY; j++)
                        //    {
                        //        x1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                        //        y1 = (20 + j * dataModel.Distance) * dataModel.Zoom;
                        //        if (i != dataModel.XCount && j != dataModel.YCount && x1 + dataModel.Distance * dataModel.Zoom * 0.75 >= xStart && x1 + dataModel.Distance * dataModel.Zoom * 0.25 <= xEnd && y1 + dataModel.Distance * dataModel.Zoom * 0.75 >= yStart && y1 + dataModel.Distance * dataModel.Zoom * 0.25 <= yEnd)
                        //        {
                        //            g.DrawString(i.ToString().PadLeft(3, ' ') + "," + (dataModel.YCount - j - 1), font, Brushes.White, (float)(x1 + dataModel.Distance * dataModel.Zoom / 8), (float)(y1 + dataModel.Distance * dataModel.Zoom * 0.38));
                        //            cC++;
                        //        }
                        //    }
                        //}

                        RectangleF rect,rectStr;
                        //Image image = Properties.Resources.erweima2;
                        Image image = Properties.Resources.qrBluee;
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        for (int i = startIndexX; i <= endIndexX; i++)
                        {
                            for (int j = startIndexY; j <= endIndexY; j++)
                            {
                                x1 = (20 + i * dataModel.Distance) * dataModel.Zoom;
                                y1 = (20 + j * dataModel.Distance) * dataModel.Zoom;
                                if (i != dataModel.XCount && j != dataModel.YCount && x1 + dataModel.Distance * dataModel.Zoom * 0.75 >= xStart && x1 + dataModel.Distance * dataModel.Zoom * 0.25 <= xEnd && y1 + dataModel.Distance * dataModel.Zoom * 0.75 >= yStart && y1 + dataModel.Distance * dataModel.Zoom * 0.25 <= yEnd)
                                {
                                    rect = new RectangleF((float)(x1 + dataModel.Distance * dataModel.Zoom * 0.25), (float)(y1 + dataModel.Distance * dataModel.Zoom * 0.25), (float)(dataModel.Distance * dataModel.Zoom * 0.5), (float)(dataModel.Distance * dataModel.Zoom * 0.5));
                                    g.DrawImage(image, rect);
                                    rectStr = new RectangleF((float)(x1), (float)(y1 + dataModel.Distance * dataModel.Zoom * 0.25), (float)(dataModel.Distance * dataModel.Zoom), (float)(dataModel.Distance * dataModel.Zoom * 0.5));
                                    g.DrawString(i + "," + (dataModel.YCount - j - 1), font, Brushes.White, rectStr, stringFormat);
                                    //g.DrawString(i.ToString().PadLeft(3, ' ') + "," + (dataModel.YCount - j - 1), font, Brushes.White, (float)(x1 + dataModel.Distance * dataModel.Zoom / 8), (float)(y1 + dataModel.Distance * dataModel.Zoom * 0.38));
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
