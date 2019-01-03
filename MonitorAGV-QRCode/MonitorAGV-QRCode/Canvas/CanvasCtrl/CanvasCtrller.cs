using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DAL;
using HelpClass;

namespace Canvas.CanvasCtrl
{
    public class CanvasCtrller : UserControl
    {
        public DevExpress.XtraEditors.HScrollBar hScrollBar;

        public DevExpress.XtraEditors.VScrollBar vScrollBar;

        public bool isMonitor;//是否监控中

        public int OffsetX, OffsetY;//滚动条坐标偏移量

        private int mapNo;//右击时选中的地图编号

        private int X, Y;//右击时选中的地图X,Y下标编号

        private string FromPoint, EndPoint;//起点，终点

        private bool IsExistFrom = false;//起点是否存在 

        public event EventHandler TestEvent;

        public delegate void ZoomDelegate(Point p1, UnitPoint p2,int hsVal,int vsVal);

        public event ZoomDelegate ZoomEvent;

        private UnitPoint? panStartPoint = null;//拖动起始点

        //private int scrollPointX, scrollPointY;//拖动起始点滚动条数值

        private UnitPoint? lockStartPoint = null;//单方向添加箭头起始点

        private UnitPoint? recStartPoint = null;//矩形框内绘制控件起始点

        private string arrowType;//单方向添加箭头箭头类型

        private int x1, y1;//单方向添加箭头起点X,Y

        public Panel parentPanel;

        public event EventHandler MoveEvent;

        private Rectangle canvasRectangle;//显示区域

        public float stepZoom = 1f;//单次放大倍数

        private eCommandType? beforeECommandType = null;

        public eCommandType m_commandType = eCommandType.select;

        public bool IsChooseSpecial = false;

        public ICanvasOwner m_owner;

        public CursorCollection m_cursors = new CursorCollection();

        public IModel m_model;

        private MoveHelper m_moveHelper = null;

        private NodeMoveHelper m_nodeMoveHelper = null;

        private CanvasWrapper m_canvaswrapper;

        private bool m_runningSnaps = true;

        private Type[] m_runningSnapTypes = null;

        private PointF m_mousedownPoint;

        private IDrawObject m_newObject = null;

        private IEditTool m_editTool = null;

        private SelectionRectangle m_selection = null;

        private string m_drawObjectId = string.Empty;

        private string m_editToolId = string.Empty;

        private Bitmap m_staticImage = null;

        private bool m_staticDirty = true;

        private UnitPoint m_lastCenterPoint;

        private PointF m_panOffset = new PointF(0f, 0f);

        private PointF m_dragOffset = new PointF(0f, 0f);

        //private float m_screenResolution = 100f;
        private float m_screenResolution = 1f;

        private ISnapPoint m_snappoint = null;

        private SmoothingMode m_smoothingMode = SmoothingMode.HighQuality;

        private Dictionary<Keys, Type> m_QuickSnap = new Dictionary<Keys, Type>();
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem tsmRemove;
        private ToolStripMenuItem tsmAddFrom;
        private ToolStripMenuItem tsmAddEnd;

        private IContainer components = null;

        public Type[] RunningSnaps
        {
            get
            {
                return m_runningSnapTypes;
            }
            set
            {
                m_runningSnapTypes = value;
            }
        }

        public bool RunningSnapsEnabled
        {
            get
            {
                return m_runningSnaps;
            }
            set
            {
                m_runningSnaps = value;
            }
        }

        public SmoothingMode SmoothingMode
        {
            get
            {
                return m_smoothingMode;
            }
            set
            {
                m_smoothingMode = value;
            }
        }

        public IModel Model
        {
            get
            {
                return m_model;
            }
            set
            {
                m_model = value;
            }
        }

        public IDrawObject NewObject
        {
            get
            {
                return m_newObject;
            }
        }

        public CanvasCtrller(ICanvasOwner owner, IModel datamodel)
        {
            m_canvaswrapper = new CanvasWrapper(this);
            m_owner = owner;
            m_model = datamodel;
            InitializeComponent();
            //Width = (int)((m_model.XCount * m_model.Distance + 100) * m_model.Zoom);
            //Height = (int)((m_model.YCount * m_model.Distance + 100) * m_model.Zoom);
            //int width = (int)((m_model.XCount * m_model.Distance + 100) * m_model.Zoom);
            //int height = (int)((m_model.YCount * m_model.Distance + 100) * m_model.Zoom);
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //m_commandType = eCommandType.select;
            //m_cursors.AddCursor(eCommandType.select, Cursors.Arrow);
            //m_cursors.AddCursor(eCommandType.draw, Cursors.Cross);
            //m_cursors.AddCursor(eCommandType.pan, "hmove.cur");
            //m_cursors.AddCursor(eCommandType.move, Cursors.SizeAll);
            //m_cursors.AddCursor(eCommandType.edit, Cursors.Cross);
            //UpdateCursor();
            m_moveHelper = new MoveHelper(this);
            m_nodeMoveHelper = new NodeMoveHelper(m_canvaswrapper);
        }

        public UnitPoint GetMousePoint()
        {
            Point p = base.PointToClient(Control.MousePosition);
            return ToUnit(p);
        }

        public void SetCenter(UnitPoint unitPoint)
        {
            PointF screenPoint = ToScreen(unitPoint);
            m_lastCenterPoint = unitPoint;
            SetCenterScreen(screenPoint, false);
        }

        public void SetCenter()
        {
            Point p = base.PointToClient(Control.MousePosition);
            SetCenterScreen(p, true);
        }

        public UnitPoint GetCenter()
        {
            //return ToUnit(new PointF((float)(base.ClientRectangle.Width / 2), (float)(base.ClientRectangle.Height / 2)));
            return ToUnit(new PointF((float)(canvasRectangle.Width / 2), (float)(canvasRectangle.Height / 2)));
        }

        protected void SetCenterScreen(PointF screenPoint, bool setCursor)
        {
            //float num = (float)(base.ClientRectangle.Width / 2);
            float num = (float)(canvasRectangle.Width / 2);
            m_panOffset.X = m_panOffset.X + (num - screenPoint.X);
            //float num2 = (float)(base.ClientRectangle.Height / 2);
            float num2 = (float)(canvasRectangle.Height / 2);
            m_panOffset.Y = m_panOffset.Y + (num2 - screenPoint.Y);
            m_panOffset.X = m_panOffset.Y = 0;
            if (setCursor)
            {
                //Cursor.Position = base.PointToScreen(new Point((int)num, (int)num2));
                //Point pZoom = new Point((int) screenPoint.X, (int) screenPoint.Y);
                //Cursor.Position = base.PointToScreen(pZoom);
                //Panel plMain = (Panel)this.Parent;
                //plMain.AutoScrollPosition = new Point((int)num, (int)num2);
            }
            DoInvalidate(true);
        }

        public double ToUnit(float screenvalue)
        {
            return Math.Round((double)screenvalue / (double)(m_screenResolution * m_model.Zoom), 5);
        }

        public UnitPoint ToUnit(PointF screenpoint)
        {
            float num = m_panOffset.X + m_dragOffset.X;
            float num2 = m_panOffset.Y + m_dragOffset.Y;
            float num3 = (screenpoint.X - num) / (m_screenResolution * m_model.Zoom);
            //float num4 = ScreenHeight() - (screenpoint.Y - num2) / (m_screenResolution * m_model.Zoom);
            float num4 = (screenpoint.Y - num2) / (m_screenResolution * m_model.Zoom);
            return new UnitPoint((double)num3, (double)num4);
        }

        public UnitPoint CenterPointUnit()
        {
            UnitPoint result;
            try
            {
                UnitPoint unitPoint = ScreenTopLeftToUnitPoint();
                UnitPoint unitPoint2 = ScreenBottomRightToUnitPoint();
                result = new UnitPoint
                {
                    X = (unitPoint.X + unitPoint2.X) / 2.0,
                    Y = (unitPoint.Y + unitPoint2.Y) / 2.0
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public UnitPoint ScreenTopLeftToUnitPoint()
        {
            return ToUnit(new PointF(0f, 0f));
        }

        public UnitPoint ScreenBottomRightToUnitPoint()
        {
            //return ToUnit(new PointF((float)base.ClientRectangle.Width, (float)base.ClientRectangle.Height));
            return ToUnit(new PointF((float)canvasRectangle.Width, (float)canvasRectangle.Height));
        }

        public float ToScreen(double value)
        {
            return (float)Math.Round(value * (double)m_screenResolution * (double)m_model.Zoom, 5);
        }

        public PointF ToScreen(UnitPoint point)
        {
            PointF pointF = Translate(point);
            //pointF.Y = ScreenHeight() - pointF.Y;
            pointF.Y *= m_screenResolution * m_model.Zoom;
            pointF.X *= m_screenResolution * m_model.Zoom;
            //pointF.X += m_panOffset.X + m_dragOffset.X;
            //pointF.Y += m_panOffset.Y + m_dragOffset.Y;
            return pointF;
        }

        private PointF Translate(UnitPoint point)
        {
            return point.Point;
        }

        private float ScreenHeight()
        {
            //return (float)(ToUnit((float)base.ClientRectangle.Height) / (double)m_model.Zoom);
            return (float)(ToUnit((float)canvasRectangle.Height));
        }

        public Pen CreatePen(Color color, float unitWidth)
        {
            return new Pen(color, unitWidth);
        }

        public void DoInvalidate(bool dostatic, RectangleF rect)
        {
            if (dostatic)
            {
                m_staticDirty = true;
            }
            base.Invalidate(ScreenUtils.ConvertRect(rect));
        }

        public void DoInvalidate(bool dostatic)
        {
            if (dostatic)
            {
                m_staticDirty = true;
            }
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = m_smoothingMode;
            CanvasWrapper canvasWrapper = new CanvasWrapper(this, e.Graphics, base.ClientRectangle);
            Rectangle rectangle = e.ClipRectangle;
            rectangle.Location = new Point(hScrollBar.Value, vScrollBar.Value);
            m_dragOffset = new PointF(-hScrollBar.Value, -vScrollBar.Value);
            canvasRectangle = this.Parent.ClientRectangle;
            if (!(float.IsNaN(rectangle.Width) || float.IsInfinity(rectangle.Width)))
            {
                //CanvasWrapper canvasWrapper2 = new CanvasWrapper(this, Graphics.FromImage(m_staticImage), base.ClientRectangle);
                canvasWrapper.Graphics.SmoothingMode = m_smoothingMode;

                if (m_model.GridLayer.Enabled)
                {
                    m_model.GridLayer.Draw(canvasWrapper, rectangle);
                }

                if (m_model.ActiveLayer != null)
                {
                    m_model.ActiveLayer.Draw(canvasWrapper, rectangle);
                }

                ICanvasLayer[] layers = m_model.Layers;
                for (int i = layers.Length - 1; i >= 0; i--)
                {
                    if (layers[i] != m_model.ActiveLayer && layers[i].Visible)
                    {
                        layers[i].Draw(canvasWrapper, rectangle);
                    }
                }

                float xStart = rectangle.X;
                float yStart = rectangle.Y;
                float xEnd = (rectangle.X + rectangle.Width);
                float yEnd = (rectangle.Y + rectangle.Height);
                float x1, y1, x2, y2;
                x1 = 20 * m_model.Zoom;
                bool isEnd;
                CanvasWrapper canvasWrapper3 = new CanvasWrapper(this, e.Graphics, base.ClientRectangle);
                Pen penCoor = new Pen(Color.White);
                AdjustableArrowCap cap = new AdjustableArrowCap(6 * m_model.Zoom, 6 * m_model.Zoom);
                penCoor.CustomEndCap = cap;
                penCoor.Width = 2;
                Pen penCoor2 = new Pen(Color.White);
                penCoor2.Width = 2;
                Font fontCoor = new Font("Tahoma", 9 * m_model.Zoom);
                if (x1 >= xStart && x1 <= xEnd)
                {
                    y1 = yStart;
                    if (y1 <= 10 * m_model.Zoom)
                    {
                        y1 = 0;
                    }
                    y2 = (20 + m_model.YCount * m_model.Distance) * m_model.Zoom;
                    if (y2 > yEnd)
                    {
                        y2 = yEnd;
                    }
                    if (y1 == 0)
                    {
                        canvasWrapper3.Graphics.DrawLine(penCoor, x1, y2, x1, y1);
                        canvasWrapper3.Graphics.DrawString("Y", fontCoor, Brushes.White, (float)(5 * m_model.Zoom),
                            5 * m_model.Zoom);
                    }
                    else
                    {
                        canvasWrapper3.Graphics.DrawLine(penCoor2, x1, y2, x1, y1);
                    }
                }
                y2 = (20 + m_model.YCount * m_model.Distance) * m_model.Zoom;
                if (y2 >= yStart && y2 <= yEnd)
                {
                    isEnd = false;
                    x1 = 20 * m_model.Zoom;
                    if (xStart > x1)
                    {
                        x1 = xStart;
                    }
                    x2 = (20 + m_model.XCount * m_model.Distance + 20) * m_model.Zoom;
                    if (x2 > xEnd)
                    {
                        if (x2 - xEnd <= 10 * m_model.Zoom)
                        {
                            isEnd = true;
                        }
                        else
                        {
                            x2 = xEnd;
                        }
                    }
                    else
                    {
                        isEnd = true;
                    }
                    if (isEnd)
                    {
                        canvasWrapper3.Graphics.DrawLine(penCoor, x1, y2, x2, y2);
                        canvasWrapper3.Graphics.DrawString("X", fontCoor, Brushes.White, x2 - 20 * m_model.Zoom,
                            y2 + 5 * m_model.Zoom);
                    }
                    else
                    {
                        canvasWrapper3.Graphics.DrawLine(penCoor2, x1, y2, x2, y2);
                    }
                }
                canvasWrapper3.Dispose();
                if (m_selection != null)
                {
                    m_selection.Reset();
                    m_selection.SetMousePoint(e.Graphics, base.PointToClient(Control.MousePosition));
                }
                canvasWrapper.Dispose();
            }
        }

        protected virtual void HandleMouseDownWhenDrawing(UnitPoint mouseunitpoint, ISnapPoint snappoint) 
        {
            if (m_commandType == eCommandType.draw)
            {
                int x = (int)((mouseunitpoint.X - 20) / m_model.Distance);
                int y = m_model.YCount - (int)((mouseunitpoint.Y - 20) / m_model.Distance) - 1;
                int mapNo = y * m_model.XCount + x;
                int RealNo=0;
                switch (m_drawObjectId)
                {
                    case "ArrowUp":
                        RealNo = mapNo + 1 + m_model.YCount;
                        break;
                    case "ArrowDown":
                        RealNo = mapNo - m_model.YCount+1;
                        break;
                    case "ArrowLeft":
                        RealNo = mapNo;
                        break;
                    case "ArrowRight":
                        RealNo = mapNo + 2;
                        break;
                }
                List<IDrawObject> lst = (from p in m_model.ActiveLayer.Objects
                                         where p.Id == m_drawObjectId && p.MapNo == mapNo
                                         select p).ToList();
                if (lst.Any())
                {
                    m_model.DeleteObjects(lst);
                    switch (m_drawObjectId)
                    {
                        case "Forbid":
                        case "Charge":
                        case "Elevator":
                            Function.PR_Write_Map_FQ(mapNo.ToString(), 0);
                            break;
                        case "ArrowUp":
                        case "ArrowDown":
                        case "ArrowLeft":
                        case "ArrowRight":
                            Function.PR_UPDATE_Map_Info_Real(mapNo, 0 - RealNo);
                            break;
                    }
                    DoInvalidate(true);
                }
                else
                {
                    if (m_newObject == null)
                    {
                        m_newObject = m_model.CreateObject(this, m_drawObjectId, mouseunitpoint, snappoint);
                        if (m_newObject!=null&&(m_newObject.Id == "ArrowLeft" || m_newObject.Id == "ArrowRight" ||
                            m_newObject.Id == "ArrowDown" || m_newObject.Id == "ArrowUp" ||
                            m_newObject.Id == "AGVTool" || m_newObject.Id == "Charge" || m_newObject.Id == "Forbid" || m_newObject.Id == "Shelf" || m_newObject.Id == "Elevator"))
                        {
                            if (m_newObject.Id == "Charge" || m_newObject.Id == "Forbid" || m_newObject.Id == "Shelf" || m_newObject.Id == "Elevator")
                            {
                                if ((from p in m_model.ActiveLayer.Objects
                                     where (p.Id == "Charge" || p.Id == "Forbid" || p.Id == "Shelf" || p.Id == "Elevator") && p.MapNo == mapNo
                                    select p).Any())
                                {
                                    XtrMsg.Show("单一地图编号只能是充电桩，禁止区域，货架，电梯其中一种类型", MessageBoxIcon.Exclamation);
                                    m_newObject = null;
                                    return;
                                }
                            }
                            m_newObject.OnMouseDown(m_canvaswrapper, mouseunitpoint, snappoint);
                            m_model.AddObject(m_model.ActiveLayer, m_newObject);
                            m_newObject = null;

                            switch (m_drawObjectId)
                            {
                                case "Forbid":
                                    Function.PR_Write_Map_FQ(mapNo.ToString(), 2);
                                    break;
                                case "Charge":
                                    Function.PR_Write_Map_FQ(mapNo.ToString(), 10);
                                    break;
                                case "Elevator":
                                    Function.PR_Write_Map_FQ(mapNo.ToString(), 11);
                                    break;
                                case "ArrowUp":
                                case "ArrowDown":
                                case "ArrowLeft":
                                case "ArrowRight":
                                    Function.PR_UPDATE_Map_Info_Real(mapNo, RealNo);
                                    break;
                            }
                            DoInvalidate(true);
                        }
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            m_mousedownPoint = new PointF((float)e.X, (float)e.Y);
            UnitPoint unitPoint = ToUnit(m_mousedownPoint);
            //int x = (int)((unitPoint.X - 20) / m_model.Distance);
            //int y = m_model.YCount - (int)((unitPoint.Y - 20) / m_model.Distance) - 1;
            //int mapNo = y * m_model.XCount + x;
            //TestEvent(x + "," + y+"@"+mapNo, null);
            if (m_commandType == eCommandType.draw && e.Button == MouseButtons.Left && !isMonitor)
            {
                m_selection = new SelectionRectangle(m_mousedownPoint);
                recStartPoint = unitPoint;
                x1 = (int)((unitPoint.X - 20) / m_model.Distance);
                y1 = m_model.YCount - (int)((unitPoint.Y - 20) / m_model.Distance) - 1;
            }
            if (m_commandType == eCommandType.pan && e.Button == MouseButtons.Left)
            {
                panStartPoint = unitPoint;
                OffsetX = hScrollBar.Value;
                OffsetY = vScrollBar.Value;
                Cursor.Current = Cursors.Hand;
            }

            #region 单一方向绘制箭头
            if (m_commandType == eCommandType.lockdir && e.Button == MouseButtons.Left && !isMonitor)
            {
                int x = (int)((unitPoint.X - 20) / m_model.Distance);
                int y = m_model.YCount - (int)((unitPoint.Y - 20) / m_model.Distance) - 1;

                if (unitPoint.X > 20 + x * m_model.Distance && unitPoint.X < 20 + x * m_model.Distance + m_model.Distance / 4 &&
                    unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.25 &&
                    unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.75)
                {
                    lockStartPoint = unitPoint;
                    x1 = x;
                    y1 = y;
                    arrowType = "L";
                }
                else if (unitPoint.X > 20 + x * m_model.Distance + 0.75 * m_model.Distance && unitPoint.X < 20 + x * m_model.Distance + m_model.Distance &&
                    unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.25 &&
                    unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.75)
                {
                    lockStartPoint = unitPoint;
                    x1 = x;
                    y1 = y;
                    arrowType = "R";
                }
                else if (unitPoint.X > 20 + x * m_model.Distance + 0.25 * m_model.Distance && unitPoint.X < 20 + x * m_model.Distance + m_model.Distance * 0.75 &&
                  unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance &&
                  unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.25)
                {
                    lockStartPoint = unitPoint;
                    x1 = x;
                    y1 = y;
                    arrowType = "U";
                }
                else if (unitPoint.X > 20 + x * m_model.Distance + 0.25 * m_model.Distance && unitPoint.X < 20 + x * m_model.Distance + m_model.Distance * 0.75 &&
                  unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.75 &&
                  unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance)
                {
                    lockStartPoint = unitPoint;
                    x1 = x;
                    y1 = y;
                    arrowType = "D";
                }
            }
            #endregion

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_mousedownPoint = new PointF((float)e.X, (float)e.Y);
            UnitPoint unitPoint = ToUnit(m_mousedownPoint);
            if (m_commandType == eCommandType.pan && e.Button == MouseButtons.Left && panStartPoint != null)
            {
                double xDistance = (((UnitPoint)panStartPoint).X - unitPoint.X) * m_model.Zoom;
                double yDistance = (((UnitPoint)panStartPoint).Y - unitPoint.Y) * m_model.Zoom;
                hScrollBar.Value = (int)(OffsetX + xDistance);
                vScrollBar.Value = (int)(OffsetY + yDistance);
                panStartPoint = null;
                DoInvalidate(true);
                Cursor.Current = Cursors.Default;
            }

            if (m_commandType == eCommandType.draw && e.Button == MouseButtons.Left && recStartPoint != null && !isMonitor)
            {
                int x = (int)((unitPoint.X - 20) / m_model.Distance);
                int y = m_model.YCount - (int)((unitPoint.Y - 20) / m_model.Distance) - 1;
                if (x == x1 && y == y1)
                {
                    if (unitPoint.X >= 20 && unitPoint.Y >= 20 && unitPoint.X <= m_model.XCount * m_model.Distance + 20 &&
                        unitPoint.Y <= m_model.YCount * m_model.Distance + 20)
                    {
                        HandleMouseDownWhenDrawing(unitPoint, null);
                    }
                }
                else
                {
                    int xMin = Math.Min(x, x1);
                    int xMax = Math.Max(x, x1);
                    int yMin = Math.Min(y, y1);
                    int yMax = Math.Max(y, y1);
                    if (xMin < 0)
                    {
                        xMin = 0;
                    }
                    if (yMin < 0)
                    {
                        yMin = 0;
                    }
                    if (xMax > m_model.XCount - 1)
                    {
                        xMax = m_model.XCount - 1;
                    }
                    if (yMax > m_model.YCount - 1)
                    {
                        yMax = m_model.YCount - 1;
                    }
                    for (int i = xMin; i <= xMax; i++)
                    {
                        for (int j = yMin; j <= yMax; j++)
                        {
                            HandleMouseDownWhenDrawing(new UnitPoint(20 + i * m_model.Distance + 0.5 * m_model.Distance, 20 + (m_model.YCount - j) * m_model.Distance - 0.5 * m_model.Distance), null);
                        }
                    }
                    DoInvalidate(true);
                }
            }

            #region 单一方向绘制箭头
            if (m_commandType == eCommandType.lockdir && e.Button == MouseButtons.Left && lockStartPoint != null && !isMonitor)
            {
                int x = (int)((unitPoint.X - 20) / m_model.Distance);
                int y = m_model.YCount - (int)((unitPoint.Y - 20) / m_model.Distance) - 1;

                if (x1 == x || y1 == y)
                {
                    if ((arrowType == "L" || arrowType == "R") && y1 == y)
                    {
                        if (arrowType == "L")
                        {
                            if (unitPoint.X > 20 + x * m_model.Distance &&
                                unitPoint.X < 20 + x * m_model.Distance + m_model.Distance / 4 &&
                                unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.25 &&
                                unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.75)
                            {
                                int xMax = Math.Max(x1, x);
                                int xMin = Math.Min(x1, x);
                                int mapNo, RealNo;
                                List<IDrawObject> lst;
                                ArrowLeft arrow;
                                for (int i = xMin; i <= xMax; i++)
                                {
                                    RealNo=mapNo = y * m_model.XCount + i;
                                    lst = (from p in m_model.ActiveLayer.Objects
                                           where p.Id == "ArrowLeft" && p.MapNo == mapNo
                                           select p).ToList();
                                    if (lst.Any())
                                    {
                                        m_model.DeleteObjects(lst);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, 0 - RealNo);
                                    }
                                    else
                                    {
                                        arrow = new ArrowLeft();
                                        arrow.MapNo = mapNo;
                                        arrow.X = i;
                                        arrow.Y = y;
                                        arrow.Location = new UnitPoint(20 + arrow.X * m_model.Distance + (float)m_model.Distance / 2, 20 + (m_model.YCount - arrow.Y) * m_model.Distance - (float)m_model.Distance / 2);
                                        m_model.AddObject(m_model.ActiveLayer, arrow);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, RealNo);
                                    }
                                }
                                DoInvalidate(true);
                            }
                        }
                        else
                        {
                            if (unitPoint.X > 20 + x * m_model.Distance + 0.75 * m_model.Distance &&
                                unitPoint.X < 20 + x * m_model.Distance + m_model.Distance &&
                                unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.25 &&
                                unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.75)
                            {
                                int xMax = Math.Max(x1, x);
                                int xMin = Math.Min(x1, x);
                                int mapNo, RealNo;
                                List<IDrawObject> lst;
                                ArrowRight arrow;
                                for (int i = xMin; i <= xMax; i++)
                                {
                                    mapNo = y * m_model.XCount + i;
                                    RealNo = mapNo + 2;
                                    lst = (from p in m_model.ActiveLayer.Objects
                                           where p.Id == "ArrowRight" && p.MapNo == mapNo
                                           select p).ToList();
                                    if (lst.Any())
                                    {
                                        m_model.DeleteObjects(lst);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, 0 - RealNo);
                                    }
                                    else
                                    {
                                        arrow = new ArrowRight();
                                        arrow.MapNo = mapNo;
                                        arrow.X = i;
                                        arrow.Y = y;
                                        arrow.Location = new UnitPoint(20 + arrow.X * m_model.Distance + (float)m_model.Distance / 2, 20 + (m_model.YCount - arrow.Y) * m_model.Distance - (float)m_model.Distance / 2);
                                        m_model.AddObject(m_model.ActiveLayer, arrow);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, RealNo);
                                    }
                                }
                                DoInvalidate(true);
                            }
                        }
                    }
                    else if ((arrowType == "U" || arrowType == "D") && x1 == x)
                    {
                        if (arrowType == "U")
                        {
                            if (unitPoint.X > 20 + x * m_model.Distance + 0.25 * m_model.Distance &&
                                unitPoint.X < 20 + x * m_model.Distance + m_model.Distance * 0.75 &&
                                unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance &&
                                unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.25)
                            {
                                int yMax = Math.Max(y1, y);
                                int yMin = Math.Min(y1, y);
                                int mapNo, RealNo;
                                List<IDrawObject> lst;
                                ArrowUp arrow;
                                for (int i = yMin; i <= yMax; i++)
                                {
                                    mapNo = i * m_model.XCount + x;
                                    RealNo = mapNo + 1 + m_model.YCount;
                                    lst = (from p in m_model.ActiveLayer.Objects
                                           where p.Id == "ArrowUp" && p.MapNo == mapNo
                                           select p).ToList();
                                    if (lst.Any())
                                    {
                                        m_model.DeleteObjects(lst);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, 0 - RealNo);
                                    }
                                    else
                                    {
                                        arrow = new ArrowUp();
                                        arrow.MapNo = mapNo;
                                        arrow.X = x;
                                        arrow.Y = i;
                                        arrow.Location = new UnitPoint(20 + arrow.X * m_model.Distance + (float)m_model.Distance / 2, 20 + (m_model.YCount - arrow.Y) * m_model.Distance - (float)m_model.Distance / 2);
                                        m_model.AddObject(m_model.ActiveLayer, arrow);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, RealNo);
                                    }
                                }
                                DoInvalidate(true);
                            }
                        }
                        else
                        {
                            if (unitPoint.X > 20 + x * m_model.Distance + 0.25 * m_model.Distance &&
                                unitPoint.X < 20 + x * m_model.Distance + m_model.Distance * 0.75 &&
                                unitPoint.Y > 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance * 0.75 &&
                                unitPoint.Y < 20 + (m_model.YCount - y - 1) * m_model.Distance + m_model.Distance)
                            {
                                int yMax = Math.Max(y1, y);
                                int yMin = Math.Min(y1, y);
                                int mapNo, RealNo;
                                List<IDrawObject> lst;
                                ArrowDown arrow;
                                for (int i = yMin; i <= yMax; i++)
                                {
                                    mapNo = i * m_model.XCount + x;
                                    RealNo = mapNo - m_model.YCount+1;
                                    lst = (from p in m_model.ActiveLayer.Objects
                                           where p.Id == "ArrowDown" && p.MapNo == mapNo
                                           select p).ToList();
                                    if (lst.Any())
                                    {
                                        m_model.DeleteObjects(lst);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, 0 - RealNo);
                                    }
                                    else
                                    {
                                        arrow = new ArrowDown();
                                        arrow.MapNo = mapNo;
                                        arrow.X = x;
                                        arrow.Y = i;
                                        arrow.Location = new UnitPoint(20 + arrow.X * m_model.Distance + (float)m_model.Distance / 2, 20 + (m_model.YCount - arrow.Y) * m_model.Distance - (float)m_model.Distance / 2);
                                        m_model.AddObject(m_model.ActiveLayer, arrow);
                                        Function.PR_UPDATE_Map_Info_Real(mapNo, RealNo);
                                    }
                                }
                                DoInvalidate(true);
                            }
                        }
                    }
                }

                lockStartPoint = null;
                arrowType = "";
            }
            #endregion

            if (m_selection != null)
            {
                RectangleF rectangleF = m_selection.Selection(m_canvaswrapper);
                if (rectangleF != RectangleF.Empty)
                {
                    DoInvalidate(true);
                }
                m_selection = null;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_selection != null)
            {
                Graphics graphics = Graphics.FromHwnd(base.Handle);
                m_selection.SetMousePoint(graphics, new PointF((float)e.X, (float)e.Y));
                graphics.Dispose();
            }
            else
            {
                UnitPoint mousePoint = GetMousePoint();
                //string msg = string.Format("Virtual:({0},{1}) || Real:({2}mm,{3}mm)", (float)((mousePoint.X - 20) / m_model.Distance),  m_model.YCount - (float)((mousePoint.Y - 20) / m_model.Distance) - 1,
                //    Math.Round(mousePoint.X*m_model.Zoom, 2), Math.Round(mousePoint.Y*m_model.Zoom, 2));
                string msg = string.Format("Virtual:({0},{1}) || Unit:({2}mm,{3}mm)||Real:({4}mm,{5}mm)", Math.Round((float)((mousePoint.X - 20) / m_model.Distance), 2), Math.Round(m_model.YCount - (float)((mousePoint.Y - 20) / m_model.Distance), 2), Math.Round(mousePoint.X, 2), Math.Round(mousePoint.Y, 2),
              Math.Round(mousePoint.X * m_model.Zoom, 2), Math.Round(mousePoint.Y * m_model.Zoom, 2));
                MoveEvent(msg, null);
                base.OnMouseMove(e);

                if (m_commandType == eCommandType.pan && e.Button == MouseButtons.Left && panStartPoint != null)
                {
                    m_mousedownPoint = new PointF((float)e.X, (float)e.Y);
                    UnitPoint panEndPoint = ToUnit(m_mousedownPoint);
                    double xDistance = (((UnitPoint)panStartPoint).X - panEndPoint.X) * m_model.Zoom;
                    double yDistance = (((UnitPoint)panStartPoint).Y - panEndPoint.Y) * m_model.Zoom;
                    hScrollBar.Value = (int)(OffsetX + xDistance);
                    vScrollBar.Value = (int)(OffsetY + yDistance);
                    OffsetX = hScrollBar.Value;
                    OffsetY = vScrollBar.Value;
                    DoInvalidate(true);
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                UnitPoint mousePoint = GetMousePoint();
                if (Control.ModifierKeys == Keys.Control)
                {
                    Point p = base.PointToClient(Control.MousePosition);
                    //string msg = string.Format("Mouse:({0},{1})",p.X, p.Y);
                    //TestEvent(mousePoint, null);
                    ZoomEvent(p, mousePoint,hScrollBar.Value,vScrollBar.Value);
                    //float num = 120f;
                    //float num2 = 1.5f * ((float)Math.Abs(e.Delta) / num);
                    float num3;
                    if (e.Delta < 0)
                    {
                        num3 = m_model.Zoom / stepZoom;
                    }
                    else
                    {
                        num3 = m_model.Zoom * stepZoom;
                    }
                    int width = (int)((m_model.XCount * m_model.Distance + 100) * num3);
                    int height = (int)((m_model.YCount * m_model.Distance + 100) * num3);

                    //if (num3 > 10 || num3 < 0.2 || width >= 30759 || height >= 30759)
                    if (40 / num3 < 5 || 40 / num3 > 100)
                    {
                        DoInvalidate(true);
                        base.OnMouseWheel(e);
                        return;
                    }
                    bool flag2 = float.IsNaN(num3) || float.IsInfinity(num3) || Math.Round((double)num3, 6) <= 1E-06 ||
                                 num3 > 20000f;
                    if (!flag2)
                    {
                        m_model.Zoom = num3;
                        //Width = width;
                        //Height = height;
                        ScrollSetUp(height, width,false);

                        //SetCenterScreen(ToScreen(mousePoint), true);
                    }
                }
                //OffsetY = parentPanel.VerticalScroll.Value - e.Delta;
                //if (OffsetY < 0)
                //{
                //    OffsetY = 0;
                //}
                DoInvalidate(true);
                base.OnMouseWheel(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            try
            {
                base.OnResize(e);
                SetCenterScreen(ToScreen(m_lastCenterPoint), false);
                m_lastCenterPoint = CenterPointUnit();
                m_staticImage = null;
                DoInvalidate(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && components != null;
            if (flag)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2)
        {
            PointF pt = ToScreen(p1);
            PointF pt2 = ToScreen(p2);
            canvas.Graphics.DrawLine(pen, pt, pt2);
        }

        public void DrawImage(ICanvas canvas, UnitPoint p)
        {
            if (!p.IsEmpty)
            {
                if (canvas.Graphics != null)
                {
                    PointF pointF = ToScreen(p);
                    RectangleF rect = new RectangleF(pointF.X - m_model.Distance * m_model.Zoom / 4, pointF.Y - m_model.Distance * m_model.Zoom / 4, (float)(m_model.Distance * m_model.Zoom / 2), (float)(m_model.Distance * m_model.Zoom / 2));
                    Image image = Properties.Resources.goods;
                    canvas.Graphics.DrawImage(image, rect);
                }
            }
        }

        public void DrawForbid(ICanvas canvas, Pen pen, UnitPoint p)
        {
            if (!p.IsEmpty)
            {
                if (canvas.Graphics != null)
                {
                    PointF pointF = ToScreen(p);
                    Rectangle rect = new Rectangle((int)(pointF.X + pen.Width / 2 + m_model.Zoom), (int)(pointF.Y + pen.Width / 2 + m_model.Zoom), (int)(m_model.Distance * m_model.Zoom - pen.Width - 2 * m_model.Zoom), (int)(m_model.Distance * m_model.Zoom - pen.Width - 2 * m_model.Zoom));
                    canvas.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        public void DrawCharge(ICanvas canvas, Pen pen, UnitPoint p)
        {
            if (!p.IsEmpty)
            {
                if (canvas.Graphics != null)
                {
                    PointF pointF = ToScreen(p);
                    Rectangle rect = new Rectangle((int)(pointF.X + pen.Width / 2 + m_model.Zoom), (int)(pointF.Y + pen.Width / 2 + m_model.Zoom), (int)(m_model.Distance * m_model.Zoom - pen.Width - 2 * m_model.Zoom), (int)(m_model.Distance * m_model.Zoom - pen.Width - 2 * m_model.Zoom));
                    canvas.Graphics.DrawEllipse(pen, rect);
                }
            }
        }

        public void DrawAgv(ICanvas canvas, string no, Color color, float angel, UnitPoint Point)
        {
            if (!Point.IsEmpty)
            {
                if (canvas.Graphics != null)
                {
                    PointF pointF = this.ToScreen(Point);
                    Pen pen = new Pen(color);
                    pen.Width = 2;

                    Rectangle rect = new Rectangle((int)(pointF.X - m_model.Distance * m_model.Zoom / 2),(int) (pointF.Y - m_model.Distance * m_model.Zoom / 2), (int)(m_model.Distance * m_model.Zoom), (int)(m_model.Distance * m_model.Zoom));

                    Bitmap image = new Bitmap(rect.Width, rect.Height);
                    Graphics imageGraphics = Graphics.FromImage(image);
                    imageGraphics.TranslateTransform((float)(rect.Width * 0.5), (float)(rect.Height * 0.5));

                    int width = (int)(m_model.Distance * m_model.Zoom * 0.4);
                    int height = (int)(m_model.Distance * m_model.Zoom * 0.3);

                    float emSize = ToScreen(6);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    Font font = new Font("宋体", emSize);
                    if (no.Length > 1)
                    {
                        imageGraphics.DrawString(no, font, Brushes.Yellow,
                            new PointF((float) (-emSize*0.9), (float) (-emSize*0.7)));
                    }
                    else
                    {
                        imageGraphics.DrawString(no, font, Brushes.Yellow,
                            new PointF((float)(-emSize * 0.4), (float)(-emSize * 0.7)));
                    }
                    Rectangle rectEli = new Rectangle((int)(-height * 0.5), (int)(-height * 0.5), height, height);
                    imageGraphics.DrawEllipse(pen, rectEli);

                    Rectangle rect1 = new Rectangle((int)(-height * 0.5), (int)(-height * 0.5), width, height);
                    imageGraphics.RotateTransform(angel);
                    imageGraphics.DrawRectangle(pen, rect1);

                    double r = Math.Sqrt(height * height * 0.25 + (width - height * 0.5) * (width - height * 0.5));
                    Rectangle rectArc = new Rectangle((int)(-r), (int)(-r), (int)(2 * r), (int)(2 * r));
                    imageGraphics.DrawArc(pen, rectArc, -30, 60);
                    imageGraphics.ResetTransform();

                    canvas.Graphics.DrawImage(image, rect);
                }
            }
        }

        public void DrawTxt(ICanvas canvas, string code, UnitPoint Point)
        {
            if (!Point.IsEmpty)
            {
                if (canvas.Graphics != null)
                {
                    PointF pointF = this.ToScreen(Point);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Near;
                    Font font = new Font("Arial Black", 9, FontStyle.Regular, GraphicsUnit.Point, 0);
                    canvas.Graphics.DrawString(code, font, Brushes.Red, pointF.X, pointF.Y, stringFormat);
                }
            }
        }

        public void DrawElevator(ICanvas canvas, Pen pen, UnitPoint p)
        {
            if (!p.IsEmpty)
            {
                if (canvas.Graphics != null)
                {
                    PointF pointF = ToScreen(p);
                    Rectangle rect = new Rectangle((int)(pointF.X + pen.Width / 2 + m_model.Zoom), (int)(pointF.Y + pen.Width / 2 + m_model.Zoom), (int)(m_model.Distance * m_model.Zoom - pen.Width - 2 * m_model.Zoom), (int)(m_model.Distance * m_model.Zoom - pen.Width - 2 * m_model.Zoom));
                    canvas.Graphics.DrawRectangle(pen, rect);
                    canvas.Graphics.DrawLine(pen, new Point((int)(pointF.X + pen.Width / 2 + m_model.Zoom), (int)(pointF.Y + 20 * m_model.Zoom)), new Point((int)(pointF.X - pen.Width / 2 - m_model.Zoom + m_model.Distance * m_model.Zoom), (int)(pointF.Y + 20 * m_model.Zoom)));
                    Pen pen2 = new Pen(pen.Color);
                    AdjustableArrowCap cap = new AdjustableArrowCap(3f, 1.5f);
                    pen2.CustomEndCap = cap;
                    pen2.Width = pen.Width;
                    canvas.Graphics.DrawLine(pen2, new Point((int)(pointF.X + 20 * m_model.Zoom), (int)(pointF.Y + 20 * m_model.Zoom)), new Point((int)(pointF.X + 20 * m_model.Zoom), (int)(pointF.Y + pen.Width / 2 + m_model.Zoom)));
                    canvas.Graphics.DrawLine(pen2, new Point((int)(pointF.X + 20 * m_model.Zoom), (int)(pointF.Y + 20 * m_model.Zoom)), new Point((int)(pointF.X + 20 * m_model.Zoom), (int)(pointF.Y - pen.Width / 2 - m_model.Zoom + m_model.Distance * m_model.Zoom)));
                }
            }
        }

        public void AddQuickSnapType(Keys key, Type snaptype)
        {
            m_QuickSnap.Add(key, snaptype);
        }

        public void CommandSelectDrawTool(string drawobjectid)
        {
            CommandEscape();
            m_model.ClearSelectedObjects();
            m_commandType = eCommandType.draw;
            m_drawObjectId = drawobjectid;
        }

        public void CommandEscape()
        {
            bool dostatic = m_newObject != null || m_snappoint != null;
            m_newObject = null;
            m_snappoint = null;
            if (m_editTool != null)
            {
                m_editTool.Finished();
            }
            m_editTool = null;
            m_commandType = eCommandType.select;
            m_moveHelper.HandleCancelMove();
            m_nodeMoveHelper.HandleCancelMove();
            DoInvalidate(dostatic);
        }

        public void CommandPan()
        {
            m_commandType = eCommandType.pan;
        }

        public void CommandMove(bool handleImmediately)
        {
            try
            {
                if (m_model.SelectedCount > 0)
                {
                    if (handleImmediately && m_commandType == eCommandType.move)
                    {
                        m_moveHelper.HandleMouseDownForMove(GetMousePoint(), m_snappoint);
                    }
                    m_commandType = eCommandType.move;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommandDeleteSelected()
        {
            m_model.DeleteObjects(m_model.SelectedObjects);
            m_model.ClearSelectedObjects();
            DoInvalidate(true);
        }

        public void CommandEdit(string editid)
        {
            CommandEscape();
            m_model.ClearSelectedObjects();
            m_commandType = eCommandType.edit;
            m_editToolId = editid;
            m_editTool = m_model.GetEditTool(m_editToolId);
        }

        private void InitializeComponent()
        {
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.tsmRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAddFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAddEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRemove,
            this.tsmAddFrom,
            this.tsmAddEnd});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // tsmRemove
            // 
            this.tsmRemove.Name = "tsmRemove";
            this.tsmRemove.Size = new System.Drawing.Size(159, 22);
            this.tsmRemove.Text = "Remove";
            this.tsmRemove.Click += new System.EventHandler(this.tsmRemove_Click);
            // 
            // tsmAddFrom
            // 
            this.tsmAddFrom.Name = "tsmAddFrom";
            this.tsmAddFrom.Size = new System.Drawing.Size(159, 22);
            this.tsmAddFrom.Text = "AddFromPoint";
            this.tsmAddFrom.Click += new System.EventHandler(this.tsmAddFrom_Click);
            // 
            // tsmAddEnd
            // 
            this.tsmAddEnd.Name = "tsmAddEnd";
            this.tsmAddEnd.Size = new System.Drawing.Size(159, 22);
            this.tsmAddEnd.Text = "AddEndPoint";
            this.tsmAddEnd.Click += new System.EventHandler(this.tsmAddEnd_Click);
            // 
            // CanvasCtrller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CanvasCtrller";
            this.Size = new System.Drawing.Size(500, 500);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            UnitPoint unitPoint = GetMousePoint();
            X = (int)((unitPoint.X - 20) / m_model.Distance);
            Y = m_model.YCount - (int)((unitPoint.Y - 20) / m_model.Distance) - 1;
            mapNo = Y * m_model.XCount + X;
            tsmAddFrom.Visible = !IsExistFrom;
            tsmAddEnd.Visible = IsExistFrom;
            IList<IDrawObject> lst = (from p in m_model.ActiveLayer.Objects
                                      where (p.Id == "Charge" || p.Id == "Forbid" || p.Id == "Shelf" || p.Id == "Elevator") && p.MapNo == mapNo
                                      select p).ToList();
            tsmRemove.Visible = lst.Any();
        }

        private void tsmRemove_Click(object sender, EventArgs e)
        {
            IList<IDrawObject> lst = (from p in m_model.ActiveLayer.Objects
                                      where (p.Id == "Charge" || p.Id == "Forbid" || p.Id == "Shelf" || p.Id == "Elevator") && p.MapNo == mapNo
                                      select p).ToList();
            if (lst.Any())
            {
                IList<IDrawObject> lstFirst = new List<IDrawObject>();
                lstFirst.Add(lst.LastOrDefault());
                m_model.DeleteObjects(lstFirst);
                if (lstFirst[0].Id == "Forbid")
                {
                    Function.PR_Write_Map_FQ(lstFirst[0].MapNo.ToString(), 0);
                }
                DoInvalidate(true);
            }
        }

        private void tsmAddFrom_Click(object sender, EventArgs e)
        {
            if (!Function.IsExist_Map_Info())
            {
                HelpClass.XtrMsg.Show("请先保存地图信息", MessageBoxIcon.Exclamation);
                return;
            }

            FromPoint = Function.GetMapNoByXY(X, Y);
            if (!string.IsNullOrEmpty(FromPoint))
            {
                IsExistFrom = true;
            }
            else
            {
                HelpClass.XtrMsg.Show("数据库中查无对应地图编号", MessageBoxIcon.Exclamation);
            }
        }

        private void tsmAddEnd_Click(object sender, EventArgs e)
        {
            EndPoint = Function.GetMapNoByXY(X, Y);

            if (string.IsNullOrEmpty(EndPoint))
            {
                HelpClass.XtrMsg.Show("数据库中查无对应地图编号", MessageBoxIcon.Exclamation);
                return;
            }

            if (int.Parse(FromPoint) == int.Parse(EndPoint))
            {
                HelpClass.XtrMsg.Show("起点不能与终点相同", MessageBoxIcon.Exclamation);
                return;
            }
            DialogResult dialogResult = HelpClass.XtrMsg.Show("确定创建任务：" + FromPoint + "(起点)," + EndPoint + "(终点)?", MessageBoxIcon.Question); 
            if (dialogResult == DialogResult.Yes)
            {
                if (Function.PR_Insert_Task(FromPoint, EndPoint) > 0)
                {
                    HelpClass.XtrMsg.Show("创建任务成功", MessageBoxIcon.Asterisk);
                    IsExistFrom = false;
                }
                else
                {
                    HelpClass.XtrMsg.Show("创建任务失败", MessageBoxIcon.Asterisk);
                }
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                IsExistFrom = false;
            }
        }

        public void SizeChange()
        {
            int width = (int)((m_model.XCount * m_model.Distance + 100) * m_model.Zoom);
            int height = (int)((m_model.YCount * m_model.Distance + 100) * m_model.Zoom);
            ScrollSetUp(height,width,false);
            DoInvalidate(true);
        }

        /// 设置滚动条最大值
        /// <summary>
        /// 设置滚动条最大值
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void ScrollSetUp(int height, int width,bool isInitial)
        {
            if (isInitial)
            {
                width = (int) ((m_model.XCount*m_model.Distance + 100)*m_model.Zoom);
                height = (int) ((m_model.YCount*m_model.Distance + 100)*m_model.Zoom);
            }

            hScrollBar.Maximum = width;
            vScrollBar.Maximum =height;

            if (hScrollBar.Maximum <= parentPanel.Width)
            {
                hScrollBar.Visible = false;
                hScrollBar.Maximum = 0;
                hScrollBar.Value = 0;
            }
            else
            {
                hScrollBar.Maximum = hScrollBar.Maximum - parentPanel.Width;
                hScrollBar.Visible = true;
            }
            if (vScrollBar.Maximum <= parentPanel.Height)
            {
                vScrollBar.Visible = false;
                vScrollBar.Maximum = 0;
                vScrollBar.Value = 0;
            }
            else
            {
                vScrollBar.Maximum = vScrollBar.Maximum - parentPanel.Height;
                vScrollBar.Visible = true;
            }
        }
    }
}
