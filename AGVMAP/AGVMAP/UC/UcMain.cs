using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using AGVMAP.HelpClass;
using DevExpress.Utils.Drawing.Animation;
using DevExpress.XtraNavBar;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using Colors = System.Drawing.Color;
using Color = System.Drawing.Color;

namespace AGVMAP.UC
{
    public partial class UcMain : UserControl
    {
        private TranslateTransform TranslateTransform;
        private ScaleTransform ScaleTransform;
        //private float ScaleX = 1;//比例X轴
        //private float ScaleY = 1;//比例Y轴
        //private float TranslateX = 0;//偏移X轴
        //private float TranslateY = 0;//偏移Y轴
        private int CoorCount = 450;//坐标数量*2+1
        private float CoorDistance = 10;//坐标间距
        private float Zoom = 1;//缩放比例
        private float OneZoom = 2;//单次缩放比例
        private bool IsShowCoordinate = true;//是否显示栅格
        private string DisplayMode = "P";//栅格显示方式  L-线  P-点
        private PointF? p1,p2,p3;
        private string navType = "";//操作选择

        public UcMain()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private void UcMain_Load(object sender, EventArgs e)
        {
            TranslateTransform = new TranslateTransform();
            ScaleTransform = new ScaleTransform();
            ScaleTransform.ScaleX =ScaleTransform.ScaleY = 1;
            TranslateTransform.X =TranslateTransform.Y = 0;
            GraphicsFunction.TranslateTransform = TranslateTransform;
            GraphicsFunction.ScaleTransform = ScaleTransform;
            pcCanvas.MouseWheel += new MouseEventHandler(pcCanvas_MouseWheel);
        }

        void pcCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            //if (e.Delta < 0)
            //{
            //    if (Zoom < 0.125)
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    if (Zoom > 20000)
            //    {
            //        return;
            //    }
            //}
            Point screenPoint = Control.MousePosition;

            System.Windows.Forms.Cursor.Position = screenPoint;

            Zoom = e.Delta < 0 ? Zoom / OneZoom : Zoom * OneZoom;
            ScaleTransform.ScaleX = ScaleTransform.ScaleY = Zoom;
            GraphicsFunction.ScaleTransform = ScaleTransform;

            if (e.Delta < 0)
            {
                TranslateTransform.X += e.X / Zoom / OneZoom;
                TranslateTransform.Y += e.Y / Zoom / OneZoom;
            }
            else
            {
                TranslateTransform.X -= e.X / Zoom;
                TranslateTransform.Y -= e.Y / Zoom;
            }
            GraphicsFunction.TranslateTransform = TranslateTransform;

            pcCanvas.Refresh();
            base.OnMouseWheel(e);
        }

        private void pcCanvas_Paint(object sender, PaintEventArgs e)
        {
            GraphicsFunction.PaintCoordinate(pcCanvas, CoorCount, CoorDistance, IsShowCoordinate, DisplayMode, e.Graphics);
            //GraphicsFunction.PaintPccanvas(pcCanvas, CoorCount, CoorDistance, IsShowCoordinate, DisplayMode);
        }

        private void pcCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (p1 != null)
                {
                    p2 = new PointF((float) (e.X/Zoom - TranslateTransform.X), (float) (e.Y/Zoom - TranslateTransform.Y));
                    TranslateTransform.X += p2.Value.X - p1.Value.X;
                    TranslateTransform.Y += p2.Value.Y - p1.Value.Y;
                    pcCanvas.Refresh();
                }
            }
            else
            {
                switch (navType)
                {
                    case "D":
                        if (p1 != null)
                        {
                            //pcCanvas.Refresh();
                            p2 = GetUnitPoint(new PointF(e.X, e.Y));

                            Graphics graphics = pcCanvas.CreateGraphics();
                            //if (p3 != null)
                            //{
                            //    DrawTool.DrawLine(graphics, Pens.Black, (PointF) p1, (PointF) p3);
                            //}
                            DrawTool.DrawLine(graphics, Pens.White, (PointF)p1, (PointF)p2);
                            p3 = p2;
                            //ControlPaint.DrawReversibleLine(pStart, pEnd, Colors.White);
                        }
                        break;
                }
            }


            textEdit1.Text = e.X + "@" + e.Y;
            //Point point = this.PointToClient(Control.MousePosition);
            Point screenPoint = Control.MousePosition;//鼠标相对于屏幕左上角的坐标
            textEdit2.Text = screenPoint.X + "@" + screenPoint.Y;
            Point formPoint = this.PointToClient(Control.MousePosition);//鼠标相对于窗体左上角的坐标
            textEdit3.Text = formPoint.X + "@" + formPoint.Y;
            //PointF point = new PointF((float)(e.X/Zoom-TranslateTransform.X),(float)(e.Y/Zoom-TranslateTransform.Y));
            Point point = pcCanvas.PointToScreen(Control.MousePosition);
            textEdit4.Text = point.X + "@" + point.Y;

            float xOld = (float)(e.X / Zoom - TranslateTransform.X);
            float yOld = (float)(e.Y / Zoom - TranslateTransform.Y);
            PointF point2 = new PointF(xOld, yOld);
            textEdit5.Text = point2.X + "@" + point2.Y;
        }

        private void pcCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (p1 != null)
                {
                    p2 = new PointF((float)(e.X / Zoom - TranslateTransform.X), (float)(e.Y / Zoom - TranslateTransform.Y));
                    TranslateTransform.X += p2.Value.X - p1.Value.X;
                    TranslateTransform.Y += p2.Value.Y - p1.Value.Y;
                    p1=p2 = null;
                    pcCanvas.Refresh();
                }
            }
        }

        private void pcCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                p1 = new PointF((float)(e.X / Zoom - TranslateTransform.X), (float)(e.Y / Zoom - TranslateTransform.Y));
            }
            else
            {
                switch (navType)
                {
                    case "D":
                        if (p1 == null)
                        {
                            p1 = GetUnitPoint(new PointF(e.X, e.Y));
                        }
                        break;
                }
            }
        }

        /// 绘图，编辑工具单击事件
        /// <summary>
        /// 绘图，编辑工具单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            NavBarItem navBarItem = sender as NavBarItem;
            if (navBarItem != null)
            {
                navType = navBarItem.Tag.ToString();
            }
            //if (pathSel1 != null)
            //{
            //    pathSel1.Stroke = Brushes.Blue;
            //}
            //if (rabType != "N")
            //{
            //    BackUielementStatus();
            //}
            //InitNull();
        }

        #region Function
        /// 根据当前点取得标准坐标系点坐标
        /// <summary>
        /// 根据当前点取得标准坐标系点坐标
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private PointF GetUnitPoint(PointF point)
        {
            return new PointF((float)(point.X / Zoom - TranslateTransform.X), (float)(point.Y / Zoom - TranslateTransform.Y));
        }
        #endregion
    }
}
