using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using DevExpress.XtraEditors;

namespace AGVMAP.HelpClass
{
    public class GraphicsFunction
    {
        public static TranslateTransform TranslateTransform;
        public static ScaleTransform ScaleTransform;

        /// 重绘坐标系
        /// <summary>
        /// 重绘坐标系
        /// </summary>
        /// <param name="pcCanvas">画板</param>
        /// <param name="coorCount">坐标数量*2+1</param>
        /// <param name="coorDistance">坐标间距</param>
        /// <param name="isShowCoordinate">是否显示栅格</param>
        /// <param name="displayMode">栅格显示方式  L-线  P-点</param>
        public static void PaintCoordinate(Control pcCanvas, int coorCount, float coorDistance, bool isShowCoordinate, string displayMode,Graphics graphics)
        {
            //Graphics graphics = pcCanvas.CreateGraphics();
            //DateTime dt1 = DateTime.Now;
            float totalWidth = (float)(pcCanvas.Width * ScaleTransform.ScaleX);
            float totalHeight = (float)(pcCanvas.Height * ScaleTransform.ScaleX);
            float lastCoorDis = (float)(coorDistance * ScaleTransform.ScaleX);//当前间距
            float offX = (float)(TranslateTransform.X * ScaleTransform.ScaleX);
            float offY = (float)(TranslateTransform.Y * ScaleTransform.ScaleX);

            PointF centerPoint = new PointF(totalWidth / 2, totalHeight / 2);
            centerPoint.X = centerPoint.X + offX;
            centerPoint.Y = centerPoint.Y + offY;

            graphics.SmoothingMode = SmoothingMode.HighSpeed;
            if (isShowCoordinate)
            {
                PointF p1, p2;
                if (displayMode == "P")
                {
                    //Bitmap bitmap = new Bitmap(pcCanvas.Width, pcCanvas.Height);
                    //Graphics g = Graphics.FromImage(bitmap);

                    //g.Clear(System.Drawing.Color.Black);

                    GDI gdi = new GDI();
                    gdi.BeginGDI(graphics);
                    for (int i = 0; i <= coorCount; i++)
                    {
                        //if (pcCanvas.Width >= pcCanvas.Height && centerPoint.X + coorDistance * i >= pcCanvas.Width)
                        //{
                        //    break;
                        //}
                        for (int j = 0; j <= coorCount; j++)
                        {
                            //if (pcCanvas.Width < pcCanvas.Height && centerPoint.Y + coorDistance * j >= pcCanvas.Height)
                            //{
                            //    break;
                            //}
                            if (centerPoint.X + lastCoorDis * i <= pcCanvas.Width &&
                                centerPoint.Y + lastCoorDis * j <= pcCanvas.Height)
                            {
                                p1 = new PointF(centerPoint.X + lastCoorDis * i, centerPoint.Y + lastCoorDis * j);
                                gdi.SetPixel((int)p1.X, (int)p1.Y, System.Drawing.Color.White.ToArgb());
                            }
                            if (j > 0)
                            {
                                if (centerPoint.X + lastCoorDis * i <= pcCanvas.Width &&
                                    centerPoint.Y - lastCoorDis * j >= 0)
                                {
                                    p1 = new PointF(centerPoint.X + lastCoorDis * i, centerPoint.Y - lastCoorDis * j);
                                    gdi.SetPixel((int)p1.X, (int)p1.Y, System.Drawing.Color.White.ToArgb());
                                }
                            }

                            if (i > 0)
                            {
                                if (centerPoint.X - lastCoorDis * i >= 0 &&
                                    centerPoint.Y - lastCoorDis * j >= 0)
                                {
                                    p1 = new PointF(centerPoint.X - lastCoorDis * i, centerPoint.Y - lastCoorDis * j);
                                    gdi.SetPixel((int)p1.X, (int)p1.Y, System.Drawing.Color.White.ToArgb());
                                }
                            }

                            if (i > 0 && j > 0)
                            {
                                if (centerPoint.X - lastCoorDis * i >= 0 &&
                                    centerPoint.Y + lastCoorDis * j <= pcCanvas.Height)
                                {
                                    p1 = new PointF(centerPoint.X - lastCoorDis * i, centerPoint.Y + lastCoorDis * j);
                                    gdi.SetPixel((int)p1.X, (int)p1.Y, System.Drawing.Color.White.ToArgb());
                                }
                            }
                        }
                    }

                    gdi.EndGDI();
                    //RectangleF rectangleF = new RectangleF(new PointF(0, 0), new SizeF(pcCanvas.Width, pcCanvas.Height));
                    //e.Graphics.DrawImage(bitmap, rectangleF, rectangleF, GraphicsUnit.Pixel);

                    //DateTime dt2 = DateTime.Now;
                    //TimeSpan timeSpan = dt2 - dt1;
                }
                else
                {
                    GraphicsPath path = new GraphicsPath();
                    float length = (float)coorCount * lastCoorDis;

                    p1 = new PointF((float)(centerPoint.X), (float)(centerPoint.Y - length));
                    p2 = new PointF((float)(centerPoint.X), (float)(centerPoint.Y + length));
                    path.AddLine(p1, p2);
                    path.CloseFigure();

                    p1 = new PointF((float)(centerPoint.X - length), (float)(centerPoint.Y));
                    p2 = new PointF((float)(centerPoint.X + length), (float)(centerPoint.Y));
                    path.AddLine(p1, p2);
                    path.CloseFigure();

                    int count = 1;
                    // draw vertical lines
                    while (true)
                    {
                        p1 = new PointF((float)(centerPoint.X + lastCoorDis * count), (float)(centerPoint.Y - length));
                        p2 = new PointF((float)(centerPoint.X + lastCoorDis * count), (float)(centerPoint.Y + length));
                        path.AddLine(p1, p2);
                        path.CloseFigure();

                        p1 = new PointF((float)(centerPoint.X - lastCoorDis * count), (float)(centerPoint.Y - length));
                        p2 = new PointF((float)(centerPoint.X - lastCoorDis * count), (float)(centerPoint.Y + length));
                        path.AddLine(p1, p2);
                        path.CloseFigure();

                        ++count;
                        if (count > coorCount)
                        {
                            break;
                        }
                    }

                    count = 1;
                    // draw horizontal lines
                    while (true)
                    {
                        p1 = new PointF((float)(centerPoint.X - length), (float)(centerPoint.Y - lastCoorDis * count));
                        p2 = new PointF((float)(centerPoint.X + length), (float)(centerPoint.Y - lastCoorDis * count));
                        path.AddLine(p1, p2);
                        path.CloseFigure();

                        p1 = new PointF((float)(centerPoint.X - length), (float)(centerPoint.Y + lastCoorDis * count));
                        p2 = new PointF((float)(centerPoint.X + length), (float)(centerPoint.Y + lastCoorDis * count));
                        path.AddLine(p1, p2);
                        path.CloseFigure();

                        ++count;
                        if (count > coorCount)
                        {
                            break;
                        }
                    }
                    graphics.DrawPath(Pens.White, path);
                }

                p1 = new PointF(centerPoint.X - 30, centerPoint.Y);
                p2 = new PointF(centerPoint.X + 30, centerPoint.Y);
                graphics.DrawLine(Pens.Blue, p1, p2);
                p1 = new PointF(centerPoint.X, (centerPoint.Y - 30));
                p2 = new PointF(centerPoint.X, (centerPoint.Y + 30));
                graphics.DrawLine(Pens.Blue, p1, p2);
            }
        }

        /// 重绘画布
        /// <summary>
        /// 重绘画布
        /// </summary>
        /// <param name="pcCanvas">画板</param>
        /// <param name="coorCount">坐标数量*2+1</param>
        /// <param name="coorDistance">坐标间距</param>
        /// <param name="isShowCoordinate">是否显示栅格</param>
        /// <param name="displayMode">栅格显示方式  L-线  P-点</param>
        /// <param name="graphics"></param>
        public static void PaintPccanvas(PanelControl pcCanvas)
        {
           
        }
    }
}
