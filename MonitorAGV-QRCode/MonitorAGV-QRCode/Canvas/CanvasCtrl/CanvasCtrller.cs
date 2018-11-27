using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using CommonTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Canvas.CanvasCtrl
{
    public class CanvasCtrller : UserControl
    {
        public object obj;
        public event EventHandler TestEvent;

        private Rectangle canvasRectangle;//显示区域

        private float stepZoom = 2f;

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
            Width = (int)((m_model.XCount * m_model.Distance + 100) * m_model.Zoom);
            Height = (int)((m_model.YCount * m_model.Distance + 100) * m_model.Zoom);
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            m_commandType = eCommandType.select;
            m_cursors.AddCursor(eCommandType.select, Cursors.Arrow);
            m_cursors.AddCursor(eCommandType.draw, Cursors.Cross);
            m_cursors.AddCursor(eCommandType.pan, "hmove.cur");
            m_cursors.AddCursor(eCommandType.move, Cursors.SizeAll);
            m_cursors.AddCursor(eCommandType.edit, Cursors.Cross);
            UpdateCursor();
            m_moveHelper = new MoveHelper(this);
            m_nodeMoveHelper = new NodeMoveHelper(m_canvaswrapper);
        }

        private void UpdateCursor()
        {
            Cursor = m_cursors.GetCursor(m_commandType);
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
            //m_panOffset.X = m_panOffset.Y = 0;
            if (setCursor)
            {
                //Cursor.Position = base.PointToScreen(new Point((int)num, (int)num2));
                //Point pZoom = new Point((int) screenPoint.X, (int) screenPoint.Y);
                //Cursor.Position = base.PointToScreen(pZoom);
                Panel plMain = (Panel)this.Parent;
                plMain.AutoScrollPosition = new Point((int)num, (int)num2);
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
            float num4 =(screenpoint.Y - num2) / (m_screenResolution * m_model.Zoom);
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
            pointF.X += m_panOffset.X + m_dragOffset.X;
            pointF.Y += m_panOffset.Y + m_dragOffset.Y;
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

        private void RepaintStatic(Rectangle r)
        {
            if (m_staticImage!=null)
            {
                Graphics graphics = Graphics.FromHwnd(base.Handle);
                if (r.X < 0)
                {
                    r.X = 0;
                }
                if ( r.X > m_staticImage.Width)
                {
                    r.X = 0;
                }
                if ( r.Y < 0)
                {
                    r.Y = 0;
                }
                if ( r.Y > m_staticImage.Height)
                {
                    r.Y = 0;
                }
                if (r.Width > m_staticImage.Width || r.Width < 0)
                {
                    r.Width = m_staticImage.Width;
                }
                if ( r.Height > m_staticImage.Height || r.Height < 0)
                {
                    r.Height = m_staticImage.Height;
                }
                graphics.DrawImage(m_staticImage, r, r, GraphicsUnit.Pixel);
                graphics.Dispose();
            }
        }

        private void RepaintSnappoint(ISnapPoint snappoint)
        {
            bool flag = snappoint == null;
            if (!flag)
            {
                CanvasWrapper canvasWrapper = new CanvasWrapper(this, Graphics.FromHwnd(base.Handle), base.ClientRectangle);
                snappoint.Draw(canvasWrapper);
                canvasWrapper.Graphics.Dispose();
                canvasWrapper.Dispose();
            }
        }

        private void RepaintObject(IDrawObject obj)
        {
            bool flag = obj == null;
            if (!flag)
            {
                CanvasWrapper canvasWrapper = new CanvasWrapper(this, Graphics.FromHwnd(base.Handle), base.ClientRectangle);
                RectangleF unitrect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(canvasWrapper, obj.GetBoundingRect(canvasWrapper)));
                obj.Draw(canvasWrapper, unitrect);
                canvasWrapper.Graphics.Dispose();
                canvasWrapper.Dispose();
            }
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

        protected void HandleSelection(List<IDrawObject> selected)
        {
            bool flag = Control.ModifierKeys == Keys.Shift;
            bool flag2 = Control.ModifierKeys == Keys.Control;
            bool flag3 = false;
            bool flag4 = false;
            int num = 0;
            bool flag5 = selected != null;
            if (flag5)
            {
                num = selected.Count;
            }
            bool flag6 = m_model.SelectedObjects.Count<IDrawObject>() > 0;
            if (flag6)
            {
                flag4 = true;
            }
            bool flag7 = flag2 && num > 0;
            if (flag7)
            {
                flag3 = true;
                foreach (IDrawObject current in selected)
                {
                    bool isChooseSpecial = IsChooseSpecial;
                    if (isChooseSpecial)
                    {
                        bool flag8 = current.Id != "ButtonTool";
                        if (flag8)
                        {
                            m_model.RemoveSelectedObject(current);
                        }
                        else
                        {
                            m_model.AddSelectedObject(current);
                        }
                    }
                    else
                    {
                        bool flag9 = m_model.IsSelected(current);
                        if (flag9)
                        {
                            m_model.RemoveSelectedObject(current);
                        }
                        else
                        {
                            m_model.AddSelectedObject(current);
                        }
                    }
                }
            }
            bool flag10 = flag && num > 0;
            if (flag10)
            {
                flag3 = true;
                foreach (IDrawObject current2 in selected)
                {
                    bool isChooseSpecial2 = IsChooseSpecial;
                    if (isChooseSpecial2)
                    {
                        bool flag11 = current2.Id != "ButtonTool";
                        if (flag11)
                        {
                            m_model.RemoveSelectedObject(current2);
                        }
                        else
                        {
                            m_model.AddSelectedObject(current2);
                        }
                    }
                    else
                    {
                        m_model.AddSelectedObject(current2);
                    }
                }
            }
            bool flag12 = !flag && !flag2 && num > 0;
            if (flag12)
            {
                flag3 = true;
                m_model.ClearSelectedObjects();
                foreach (IDrawObject current3 in selected)
                {
                    bool isChooseSpecial3 = IsChooseSpecial;
                    if (isChooseSpecial3)
                    {
                        bool flag13 = current3.Id != "ButtonTool";
                        if (flag13)
                        {
                            m_model.RemoveSelectedObject(current3);
                        }
                        else
                        {
                            m_model.AddSelectedObject(current3);
                        }
                    }
                    else
                    {
                        m_model.AddSelectedObject(current3);
                    }
                }
            }
            bool flag14 = (!flag && !flag2 && num == 0) & flag4;
            if (flag14)
            {
                flag3 = true;
                m_model.ClearSelectedObjects();
            }
            bool flag15 = flag3;
            if (flag15)
            {
                DoInvalidate(false);
            }
        }

        private void FinishNodeEdit()
        {
            m_commandType = eCommandType.select;
            m_snappoint = null;
        }

        protected virtual void HandleMouseDownWhenDrawing(UnitPoint mouseunitpoint, ISnapPoint snappoint)
        {
            try
            {
                if (m_commandType == eCommandType.draw)
                {
                    if (m_newObject == null)
                    {
                        m_newObject = m_model.CreateObject(m_drawObjectId, mouseunitpoint, snappoint);
                        DoInvalidate(false, m_newObject.GetBoundingRect(m_canvaswrapper));
                        if (m_newObject.Id == "LandMark" || m_newObject.Id == "ImgeTool" || m_newObject.Id == "TextTool" || m_newObject.Id == "StorageTool" || m_newObject.Id == "AGVTool" || m_newObject.Id == "ButtonTool")
                        {
                            m_newObject = m_model.CreateObject(m_drawObjectId, mouseunitpoint, snappoint);
                            m_newObject.OnMouseDown(m_canvaswrapper, mouseunitpoint, snappoint);
                            m_model.AddObject(m_model.ActiveLayer, m_newObject);
                            m_newObject = null;
                            DoInvalidate(true);
                        }
                        if (m_drawObjectId == "PointBezier")
                        {
                            if ((m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p1 || (m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p2)
                            {
                                if (m_snappoint == null || (m_snappoint != null && ((m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark") || m_snappoint.Owner == null)))
                                {
                                    m_newObject = null;
                                    DoInvalidate(true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_drawObjectId == "PointBezier")
                        {
                            if ((m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p1 || (m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p2)
                            {
                                if (m_snappoint == null || (m_snappoint != null && ((m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark") || m_snappoint.Owner == null)))
                                {
                                    m_newObject = null;
                                    DoInvalidate(true);
                                    return;
                                }
                            }
                        }
                        switch (m_newObject.OnMouseDown(m_canvaswrapper, mouseunitpoint, snappoint))
                        {
                            case eDrawObjectMouseDownEnum.Done:
                                m_model.AddObject(m_model.ActiveLayer, m_newObject);
                                m_newObject = null;
                                DoInvalidate(true);
                                break;
                            case eDrawObjectMouseDownEnum.DoneRepeat:
                                m_model.AddObject(m_model.ActiveLayer, m_newObject);
                                m_newObject = m_model.CreateObject(m_newObject.Id, m_newObject.RepeatStartingPoint, null);
                                DoInvalidate(true);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Tracing.StartTrack(Tracing.TracePaint);
            e.Graphics.SmoothingMode = m_smoothingMode;
            //CanvasWrapper canvasWrapper = new CanvasWrapper(this, e.Graphics, base.ClientRectangle);
            Rectangle rectangle = e.ClipRectangle;
            canvasRectangle = this.Parent.ClientRectangle;
            //if (m_staticImage == null)
            //{
                //m_staticImage = new Bitmap(rectangle.Width, rectangle.Height);
                //m_staticDirty = true;
            //}
            if (!(float.IsNaN(rectangle.Width) || float.IsInfinity(rectangle.Width)))
            {
                if (m_staticDirty)
                {
                    m_staticDirty = false;
                    //CanvasWrapper canvasWrapper2 = new CanvasWrapper(this, Graphics.FromImage(m_staticImage), base.ClientRectangle);
                    CanvasWrapper canvasWrapper2 = new CanvasWrapper(this, e.Graphics, base.ClientRectangle);
                    canvasWrapper2.Graphics.SmoothingMode = m_smoothingMode;

                    if (m_model.GridLayer.Enabled)
                    {
                        m_model.GridLayer.Draw(canvasWrapper2, rectangle);
                    }

                    if (m_model.ActiveLayer != null)
                    {
                        m_model.ActiveLayer.Draw(canvasWrapper2, rectangle);
                    }
                    canvasWrapper2.Dispose();
                    GC.Collect();

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
                        //y1 = 0;
                        //y2 = (20 + m_model.YCount * m_model.Distance) * m_model.Zoom;
                        y1 = yStart;
                        if (y1 <= 10*m_model.Zoom)
                        {
                            y1 = 0;
                        }
                        y2 = (20 + m_model.YCount*m_model.Distance)*m_model.Zoom;
                        if (y2 > yEnd)
                        {
                            y2 = yEnd;
                        }
                        if (y1 == 0)
                        {
                            canvasWrapper3.Graphics.DrawLine(penCoor, x1, y2, x1, y1);
                            canvasWrapper3.Graphics.DrawString("Y", fontCoor, Brushes.White, (float) (5*m_model.Zoom),
                                5*m_model.Zoom);
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
                        //x1 = 20 * m_model.Zoom;
                        //x2 = (20 + m_model.XCount * m_model.Distance + 20) * m_model.Zoom;
                        x1 = 20 * m_model.Zoom;
                        if (xStart > x1)
                        {
                            x1 = xStart;
                        }
                        x2 = (20 + m_model.XCount * m_model.Distance + 20) * m_model.Zoom;
                        if (x2 > xEnd)
                        {
                            if (x2 - xEnd <=10*m_model.Zoom)
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
                            canvasWrapper3.Graphics.DrawString("X", fontCoor, Brushes.White, x2 - 20*m_model.Zoom,
                                y2 + 5*m_model.Zoom);
                        }
                        else
                        {
                            canvasWrapper3.Graphics.DrawLine(penCoor2, x1, y2, x2, y2);
                        }
                    }
                    canvasWrapper3.Dispose();
                }
                //Rectangle rectangle2 = new Rectangle();
                //rectangle2.X = 0;
                //rectangle2.Y = 0;
                //rectangle2.Width = rectangle.Width;
                //rectangle2.Height = rectangle.Height;
                //e.Graphics.DrawImage(m_staticImage, rectangle, rectangle2, GraphicsUnit.Pixel);
                //m_staticImage.Save(Application.StartupPath+@"\1.bmp",ImageFormat.Bmp);
                //obj = rectangle.X + "@" + rectangle.Y + "@" + rectangle.Width + "@" + rectangle.Height;
                //TestEvent(null, null);
                //foreach (IDrawObject current in m_model.SelectedObjects)
                //{
                //    current.Draw(canvasWrapper, rectangle);
                //}

                //if (m_newObject != null)
                //{
                //    m_newObject.Draw(canvasWrapper, rectangle);
                //}

                //if (m_snappoint != null)
                //{
                //    m_snappoint.Draw(canvasWrapper);
                //}
                //if (m_selection != null)
                //{
                //    m_selection.Reset();
                //    m_selection.SetMousePoint(e.Graphics, base.PointToClient(Control.MousePosition));
                //}
                //if (!m_moveHelper.IsEmpty)
                //{
                //    m_moveHelper.DrawObjects(canvasWrapper, rectangle);
                //}
                //if (!m_nodeMoveHelper.IsEmpty)
                //{
                //    m_nodeMoveHelper.DrawObjects(canvasWrapper, rectangle);
                //}
                //canvasWrapper.Dispose();
                Tracing.EndTrack(Tracing.TracePaint, "OnPaint complete");
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    beforeECommandType = m_commandType;
                    m_commandType = eCommandType.pan;
                    UpdateCursor();
                }
                else
                {
                    beforeECommandType = null;
                }
                if (m_commandType == eCommandType.select && e.Button == MouseButtons.Right)
                {
                    m_commandType = eCommandType.pan;
                    UpdateCursor();
                }
                m_mousedownPoint = new PointF((float)e.X, (float)e.Y);
                m_dragOffset = new PointF(0f, 0f);
                UnitPoint unitPoint = ToUnit(m_mousedownPoint);
                if (m_snappoint != null)
                {
                    unitPoint = m_snappoint.SnapPoint;
                }
                if (m_commandType == eCommandType.editNode)
                {
                    bool flag4 = false;
                    if (m_nodeMoveHelper.HandleMouseDown(unitPoint, ref flag4))
                    {
                        if (m_drawObjectId == "PointLine")
                        {
                            if (m_snappoint == null || (m_snappoint != null && ((m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark") || m_snappoint.Owner == null)))
                            {
                                m_newObject = null;
                                DoInvalidate(true);
                                return;
                            }
                        }
                        FinishNodeEdit();
                        base.OnMouseDown(e);
                        return;
                    }
                }
                if (m_commandType == eCommandType.select && e.Button == MouseButtons.Left)
                {
                    bool flag9 = false;
                    if (m_nodeMoveHelper.HandleMouseDown(unitPoint, ref flag9))
                    {
                        m_commandType = eCommandType.editNode;
                        m_snappoint = null;
                        base.OnMouseDown(e);
                        return;
                    }
                    m_selection = new SelectionRectangle(m_mousedownPoint);
                }
                if (m_commandType == eCommandType.move)
                {
                    m_moveHelper.HandleMouseDownForMove(unitPoint, m_snappoint);
                }
                if (m_commandType == eCommandType.draw)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        m_newObject = null;
                        DoInvalidate(true);
                        return;
                    }
                    if (m_drawObjectId == "PointLine")
                    {
                        if (m_snappoint == null || (m_snappoint != null && ((m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark") || m_snappoint.Owner == null)))
                        {
                            m_newObject = null;
                            DoInvalidate(true);
                            return;
                        }
                    }
                    HandleMouseDownWhenDrawing(unitPoint, null);
                    DoInvalidate(true);
                }
                if (m_commandType == eCommandType.edit)
                {
                    if (m_editTool == null)
                    {
                        m_editTool = m_model.GetEditTool(m_editToolId);
                    }
                    if (m_editTool != null)
                    {
                        if (m_editTool.SupportSelection)
                        {
                            m_selection = new SelectionRectangle(m_mousedownPoint);
                        }
                        eDrawObjectMouseDownEnum eDrawObjectMouseDownEnum = m_editTool.OnMouseDown(m_canvaswrapper, unitPoint, m_snappoint);
                        if (eDrawObjectMouseDownEnum == eDrawObjectMouseDownEnum.Done)
                        {
                            m_editTool.Finished();
                            m_editTool = m_model.GetEditTool(m_editToolId);
                            bool supportSelection2 = m_editTool.SupportSelection;
                            if (supportSelection2)
                            {
                                m_selection = new SelectionRectangle(m_mousedownPoint);
                            }
                        }
                    }
                    DoInvalidate(true);
                    UpdateCursor();
                }
                base.OnMouseDown(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                if (m_commandType == eCommandType.pan && e.Button == MouseButtons.Right)
                {
                    //m_commandType = eCommandType.select;
                    if (beforeECommandType != null)
                    {
                        m_commandType = (eCommandType)beforeECommandType;
                    }
                    else
                    {
                        m_commandType = eCommandType.select;
                    }
                    UpdateCursor();
                }
                if (m_commandType == eCommandType.pan || e.Button == MouseButtons.Right)
                {
                    m_panOffset.X = m_panOffset.X + m_dragOffset.X;
                    m_panOffset.Y = m_panOffset.Y + m_dragOffset.Y;
                    m_dragOffset = new PointF(0f, 0f);
                }
                List<IDrawObject> list = null;
                Rectangle left = Rectangle.Empty;
                if (m_selection != null)
                {
                    left = m_selection.ScreenRect();
                    RectangleF rectangleF = m_selection.Selection(m_canvaswrapper);
                    if (rectangleF != RectangleF.Empty)
                    {
                        list = m_model.GetHitObjects(m_canvaswrapper, rectangleF, m_selection.AnyPoint());
                        DoInvalidate(true);
                    }
                    else
                    {
                        UnitPoint point = ToUnit(new PointF((float)e.X, (float)e.Y));
                        list = m_model.GetHitObjects(m_canvaswrapper, point);
                    }
                    m_selection = null;
                }
                if (m_commandType == eCommandType.select && e.Button == MouseButtons.Left)
                {
                    bool flag6 = list != null;
                    if (flag6)
                    {
                        HandleSelection(list);
                    }
                }
                if (m_commandType == eCommandType.edit && m_editTool != null)
                {
                    UnitPoint unitPoint = ToUnit(m_mousedownPoint);
                    if (m_snappoint != null)
                    {
                        unitPoint = m_snappoint.SnapPoint;
                    }
                    if (left != Rectangle.Empty)
                    {
                        m_editTool.SetHitObjects(unitPoint, list);
                    }
                    m_editTool.OnMouseUp(m_canvaswrapper, unitPoint, m_snappoint);
                }
                if (m_commandType == eCommandType.draw && m_newObject != null)
                {
                    UnitPoint point2 = ToUnit(m_mousedownPoint);
                    if (m_snappoint != null)
                    {
                        point2 = m_snappoint.SnapPoint;
                    }
                    m_newObject.OnMouseUp(m_canvaswrapper, point2, m_snappoint);
                }
                base.OnMouseUp(e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                UnitPoint mousePoint = GetMousePoint();
                string msg = string.Format("Mouse:({0},{1}),Unit:({2},{3})", MousePosition.X, MousePosition.Y,
                      mousePoint.X, mousePoint.Y);
                TestEvent(msg, null);
                base.OnMouseMove(e);
                if (m_selection != null)
                {
                    Graphics graphics = Graphics.FromHwnd(base.Handle);
                    m_selection.SetMousePoint(graphics, new PointF((float)e.X, (float)e.Y));
                    graphics.Dispose();
                }
                else
                {
                    if ((m_commandType == eCommandType.pan && e.Button == MouseButtons.Left) || e.Button == MouseButtons.Right)
                    {
                        m_dragOffset.X = -(m_mousedownPoint.X - (float)e.X);
                        m_dragOffset.Y = -(m_mousedownPoint.Y - (float)e.Y);
                        m_lastCenterPoint = CenterPointUnit();
                        DoInvalidate(true);
                    }
                    UnitPoint positionInfo = ToUnit(new PointF((float)e.X, (float)e.Y));
                    UnitPoint unitPoint;
                    if (m_commandType == eCommandType.draw || m_commandType == eCommandType.move || !m_nodeMoveHelper.IsEmpty)
                    {
                        Rectangle rectangle = Rectangle.Empty;
                        ISnapPoint snapPoint = null;
                        unitPoint = GetMousePoint();
                        if (RunningSnapsEnabled)
                        {
                            snapPoint = m_model.SnapPoint(m_canvaswrapper, unitPoint, m_runningSnapTypes, null);
                        }
                        if (snapPoint == null)
                        {
                            snapPoint = m_model.GridLayer.SnapPoint(m_canvaswrapper, unitPoint, null);
                        }
                        if (m_snappoint != null && (snapPoint == null || snapPoint.SnapPoint != m_snappoint.SnapPoint || m_snappoint.GetType() != snapPoint.GetType()))
                        {
                            rectangle = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(m_canvaswrapper, m_snappoint.BoundingRect));
                            rectangle.Inflate(2, 2);
                            RepaintStatic(rectangle);
                            m_snappoint = snapPoint;
                        }
                        if (m_commandType == eCommandType.move)
                        {
                            base.Invalidate(rectangle);
                        }
                        if (m_snappoint == null)
                        {
                            m_snappoint = snapPoint;
                        }
                    }
                    m_owner.SetPositionInfo(positionInfo);
                    m_owner.SetSnapInfo(m_snappoint);
                    if (m_snappoint != null)
                    {
                        unitPoint = m_snappoint.SnapPoint;
                    }
                    else
                    {
                        unitPoint = GetMousePoint();
                    }
                    if (m_newObject != null)
                    {
                        Rectangle r = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(m_canvaswrapper, m_newObject.GetBoundingRect(m_canvaswrapper)));
                        r.Inflate(2, 2);
                        RepaintStatic(r);
                        m_newObject.OnMouseMove(m_canvaswrapper, unitPoint);
                        RepaintObject(m_newObject);
                    }
                    if (m_snappoint != null)
                    {
                        RepaintSnappoint(m_snappoint);
                    }
                    if (m_moveHelper.HandleMouseMoveForMove(unitPoint))
                    {
                        Refresh();
                    }
                    RectangleF rectangleF = m_nodeMoveHelper.HandleMouseMoveForNode(unitPoint);
                    if (rectangleF != RectangleF.Empty)
                    {
                        Rectangle r2 = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(m_canvaswrapper, rectangleF));
                        RepaintStatic(r2);
                        CanvasWrapper canvasWrapper = new CanvasWrapper(this, Graphics.FromHwnd(base.Handle), base.ClientRectangle);
                        canvasWrapper.Graphics.Clip = new Region(base.ClientRectangle);
                        m_nodeMoveHelper.DrawObjects(canvasWrapper, rectangleF);
                        if (m_snappoint != null)
                        {
                            RepaintSnappoint(m_snappoint);
                        }
                        canvasWrapper.Graphics.Dispose();
                        canvasWrapper.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    UnitPoint mousePoint = GetMousePoint();
                    //string msg = string.Format("Mouse:({0},{1}),Unit:({2},{3})", canvasRectangle.Width, canvasRectangle.Height,
                    //    mousePoint.X, mousePoint.Y);
                    //TestEvent(msg, null);
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
                    if (num3 > 10 || num3 < 0.2)
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
                        Width = (int)((m_model.XCount * m_model.Distance + 100) * num3);
                        Height = (int)((m_model.YCount * m_model.Distance + 100) * num3);
                        SetCenterScreen(ToScreen(mousePoint), true);
                        //Panel plMain = (Panel) this.Parent;
                        //plMain.AutoScrollPosition = new Point(plMain.HorizontalScroll.Value, plMain.VerticalScroll.Value+e.Delta);
                    }
                }
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            bool isChooseSpecial = IsChooseSpecial;
            if (!isChooseSpecial)
            {
                HandleQuickSnap(e);
                bool flag = !m_nodeMoveHelper.IsEmpty;
                if (flag)
                {
                    m_nodeMoveHelper.OnKeyDown(m_canvaswrapper, e);
                    bool handled = e.Handled;
                    if (handled)
                    {
                        return;
                    }
                }
                base.OnKeyDown(e);
                bool handled2 = e.Handled;
                if (handled2)
                {
                    UpdateCursor();
                }
                else
                {
                    bool flag2 = m_editTool != null;
                    if (flag2)
                    {
                        m_editTool.OnKeyDown(m_canvaswrapper, e);
                        bool handled3 = e.Handled;
                        if (handled3)
                        {
                            return;
                        }
                    }
                    bool flag3 = m_newObject != null;
                    if (flag3)
                    {
                        m_newObject.OnKeyDown(m_canvaswrapper, e);
                        bool handled4 = e.Handled;
                        if (handled4)
                        {
                            return;
                        }
                    }
                    foreach (IDrawObject current in m_model.SelectedObjects)
                    {
                        current.OnKeyDown(m_canvaswrapper, e);
                        bool handled5 = e.Handled;
                        if (handled5)
                        {
                            return;
                        }
                    }
                    bool flag4 = (Control.ModifierKeys & Keys.Control) == Keys.Control;
                    if (flag4)
                    {
                        bool flag5 = e.KeyCode == Keys.G;
                        if (flag5)
                        {
                            m_model.GridLayer.Enabled = !m_model.GridLayer.Enabled;
                            DoInvalidate(true);
                        }
                        bool flag6 = e.KeyCode == Keys.C;
                        if (flag6)
                        {
                            RunningSnapsEnabled = !RunningSnapsEnabled;
                            bool flag7 = !RunningSnapsEnabled;
                            if (flag7)
                            {
                                m_snappoint = null;
                            }
                            DoInvalidate(false);
                        }
                    }
                    else
                    {
                        bool flag8 = e.KeyCode == Keys.Escape;
                        if (flag8)
                        {
                            CommandEscape();
                        }
                        bool flag9 = e.KeyCode == Keys.P;
                        if (flag9)
                        {
                            CommandPan();
                        }
                        bool flag10 = e.KeyCode == Keys.S;
                        if (flag10)
                        {
                            CommandEscape();
                        }
                        bool flag11 = e.KeyCode == Keys.M;
                        if (flag11)
                        {
                            CommandMove(true);
                        }
                        bool flag12 = e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9;
                        if (flag12)
                        {
                            int num = e.KeyCode - Keys.D1;
                            bool flag13 = num >= 0 && num < m_model.Layers.Length;
                            if (flag13)
                            {
                                m_model.ActiveLayer = m_model.Layers[num];
                                DoInvalidate(true);
                            }
                        }
                        bool flag14 = e.KeyCode == Keys.Delete;
                        if (flag14)
                        {
                            CommandDeleteSelected();
                        }
                        bool flag15 = e.KeyCode == Keys.O;
                        if (flag15)
                        {
                            CommandEdit("linesmeet");
                        }
                        UpdateCursor();
                    }
                }
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
            try
            {
                PointF pt = ToScreen(p1);
                PointF pt2 = ToScreen(p2);
                canvas.Graphics.DrawLine(pen, pt, pt2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawLandMark(ICanvas canvas, Brush pen, string code, UnitPoint Point)
        {
            try
            {
                bool isEmpty = Point.IsEmpty;
                if (!isEmpty)
                {
                    bool flag = canvas.Graphics == null;
                    if (!flag)
                    {
                        PointF pointF = ToScreen(Point);
                        float num = ToScreen(0.2);
                        canvas.Graphics.FillEllipse(pen, pointF.X, pointF.Y, num, num);
                        pen = Brushes.White;
                        float num2 = ToScreen(0.10000000149011612);
                        float num3 = num2 * (float)(code.Length + 1);
                        float num4 = ToScreen(0.15000000596046448);
                        float num5 = num3 / 2f - ToScreen(0.1);
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        Rectangle r = new Rectangle((int)(pointF.X - num5), (int)(pointF.Y - num4), (int)num3, (int)num4);
                        Font font = new Font("宋体", num2);
                        canvas.Graphics.DrawString(code, font, pen, r, stringFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawImge(ICanvas canvas, Pen pen, UnitPoint Location, float Widht, float Hight, Image img, string values)
        {
            try
            {
                bool isEmpty = Location.IsEmpty;
                if (!isEmpty)
                {
                    bool flag = canvas.Graphics != null;
                    if (flag)
                    {
                        PointF location = ToScreen(Location);
                        float num = ToScreen((double)(Widht / 96f));
                        float num2 = ToScreen((double)(Hight / 96f));
                        canvas.Graphics.DrawRectangle(pen, location.X, location.Y, num, num2);
                        canvas.Graphics.DrawImage(img, new RectangleF(location, new Size((int)num, (int)num2)));
                        bool flag2 = !string.IsNullOrEmpty(values);
                        if (flag2)
                        {
                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            float num3 = 0f;
                            bool flag3 = values.Length == 1;
                            float emSize;
                            float num4;
                            if (flag3)
                            {
                                emSize = ToScreen(0.3125);
                                num4 = -ToScreen(0.26041666666666669);
                            }
                            else
                            {
                                bool flag4 = values.Length == 2;
                                if (flag4)
                                {
                                    emSize = ToScreen(0.3125);
                                    num4 = -ToScreen(0.28125);
                                }
                                else
                                {
                                    emSize = ToScreen(0.20833333333333334);
                                    num4 = -ToScreen(0.28125);
                                    num3 = -ToScreen(0.10416666666666667);
                                }
                            }
                            Font font = new Font("Arial Black", emSize, FontStyle.Regular, GraphicsUnit.Point, 0);
                            SolidBrush brush = new SolidBrush(Color.White);
                            canvas.Graphics.DrawString(values, font, brush, location.X - num4, location.Y - num3, stringFormat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawTxt(ICanvas canvas, string code, UnitPoint Point, int FontSize, Color fontColor)
        {
            try
            {
                bool isEmpty = Point.IsEmpty;
                if (!isEmpty)
                {
                    bool flag = canvas.Graphics != null;
                    if (flag)
                    {
                        PointF pointF = ToScreen(Point);
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Near;
                        float emSize = ToScreen((double)((float)FontSize / 96f));
                        Font font = new Font("Arial Black", emSize, FontStyle.Regular, GraphicsUnit.Point, 0);
                        SolidBrush brush = new SolidBrush(fontColor);
                        canvas.Graphics.DrawString(code, font, brush, pointF.X, pointF.Y, stringFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawBizer(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, UnitPoint p3, UnitPoint p4)
        {
            try
            {
                PointF pt = ToScreen(p1);
                PointF pt2 = ToScreen(p2);
                PointF pt3 = ToScreen(p3);
                PointF pt4 = ToScreen(p4);
                bool flag = canvas.Graphics != null;
                if (flag)
                {
                    canvas.Graphics.DrawBezier(pen, pt, pt2, pt3, pt4);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawStorage(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
        {
            try
            {
                if (!Point.IsEmpty)
                {
                    if (canvas.Graphics != null)
                    {
                        PointF pointF = ToScreen(Point);
                        float num = ToScreen(0.5);
                        Rectangle rect = new Rectangle((int)pointF.X, (int)pointF.Y, (int)num, (int)num);
                        canvas.Graphics.FillRectangle(Pen, rect);
                        //float num2 = ToScreen(0.10000000149011612);
                        float num2 = ToScreen(0.20000000149011612);
                        float num3 = num2 * (float)code.Length + num2;
                        //float num4 = ToScreen(0.15000000596046448);
                        float num4 = ToScreen(0.30000000596046448);
                        float num5 = num / 2f - num3 / 2f;
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        Font font = new Font("宋体", num2);
                        Brush gray = Brushes.Gray;
                        //Rectangle r = new Rectangle((int)(pointF.X + num5), (int)(pointF.Y + ToScreen(0.15000000596046448)), (int)num3, (int)num4);
                        Rectangle r = new Rectangle((int)(pointF.X + num5), (int)(pointF.Y + ToScreen(0.10000000596046448)), (int)num3, (int)num4);
                        //canvas.Graphics.FillRectangle(Brushes.Red,r);
                        canvas.Graphics.DrawString(code, font, gray, r, stringFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawBtnBox(ICanvas canvas, float Radius, UnitPoint Point, bool Selected)
        {
            try
            {
                if (!Point.IsEmpty)
                {
                    if (canvas.Graphics != null)
                    {
                        PointF pointF = ToScreen(Point);
                        float num = ToScreen((double)Radius);
                        Color color = Color.FromArgb(47, 79, 79);
                        Brush brush = new SolidBrush(color);
                        canvas.Graphics.FillEllipse(brush, pointF.X, pointF.Y, num, num);
                        color = Color.FromArgb(105, 105, 105);
                        brush = new SolidBrush(color);
                        if (Selected)
                        {
                            brush = Brushes.Magenta;
                        }
                        num -= ToScreen(0.20000000298023224);
                        bool flag2 = num > 0f;
                        if (flag2)
                        {
                            canvas.Graphics.FillEllipse(brush, pointF.X + ToScreen(0.10000000149011612), pointF.Y + ToScreen(0.10000000149011612), num, num);
                        }
                        brush = Brushes.DarkGray;
                        float emSize = ToScreen((double)(((Radius >= 1f) ? (0.2f * Radius) : 0.14f) - 0.05f));
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;
                        Rectangle r = new Rectangle((int)(ToScreen(Point).X + ToScreen((double)(Radius / 3f))), (int)(ToScreen(Point).Y + ToScreen((double)(Radius / 3f))), (int)ToScreen((double)(Radius / 3f)), (int)ToScreen((double)(Radius / 3f * 2f)));
                        Font font = new Font("宋体", emSize);
                        canvas.Graphics.DrawString("呼叫", font, brush, r, stringFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code)
        {
            try
            {
                PointF pointF = ToScreen(p);
                SolidBrush brush = new SolidBrush(pen.Color);
                RectangleF rect = new RectangleF(pointF.X - ToScreen(0.15), pointF.Y - ToScreen(0.15), ToScreen(0.5), ToScreen(0.5));
                bool flag = !rect.Location.IsEmpty && !float.IsInfinity(rect.X) && !float.IsInfinity(rect.Y);
                if (flag)
                {
                    using (GraphicsPath graphicsPath = CreateRoundedRectanglePath(rect, (int)ToScreen(0.06)))
                    {
                        bool flag2 = graphicsPath != null;
                        if (flag2)
                        {
                            canvas.Graphics.FillPath(brush, graphicsPath);
                        }
                    }
                    float emSize = ToScreen(0.30000001192092896);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    Font font = new Font("宋体", emSize);
                    Brush yellow = Brushes.Yellow;
                    float num = ToScreen(0.05) + (float)(Code.Length - 1) * ToScreen(0.1);
                    float num2 = ToScreen(0.1);
                    canvas.Graphics.DrawString(Code, font, yellow, new PointF((float)((int)(pointF.X - num)), (float)((int)(pointF.Y - num2))));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            GraphicsPath result;
            try
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddArc(rect.X, rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 180f, 90f);
                graphicsPath.AddLine(rect.X + (float)cornerRadius, rect.Y, rect.Right - (float)(cornerRadius * 2), rect.Y);
                graphicsPath.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 270f, 90f);
                graphicsPath.AddLine(rect.Right, rect.Y + (float)(cornerRadius * 2), rect.Right, rect.Y + rect.Height - (float)(cornerRadius * 2));
                graphicsPath.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y + rect.Height - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 0f, 90f);
                graphicsPath.AddLine(rect.Right - (float)(cornerRadius * 2), rect.Bottom, rect.X + (float)(cornerRadius * 2), rect.Bottom);
                graphicsPath.AddArc(rect.X, rect.Bottom - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 90f, 90f);
                graphicsPath.AddLine(rect.X, rect.Bottom - (float)(cornerRadius * 2), rect.X, rect.Y + (float)(cornerRadius * 2));
                graphicsPath.CloseFigure();
                result = graphicsPath;
                return result;
            }
            catch
            {
            }
            result = null;
            return result;
        }

        public void AddQuickSnapType(Keys key, Type snaptype)
        {
            m_QuickSnap.Add(key, snaptype);
        }

        public void CommandSelectDrawTool(string drawobjectid)
        {
            try
            {
                CommandEscape();
                m_model.ClearSelectedObjects();
                m_commandType = eCommandType.draw;
                m_drawObjectId = drawobjectid;
                UpdateCursor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommandEscape()
        {
            try
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
                UpdateCursor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommandPan()
        {
            try
            {
                m_commandType = eCommandType.pan;
                UpdateCursor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommandMove(bool handleImmediately)
        {
            try
            {
                bool flag = m_model.SelectedCount > 0;
                if (flag)
                {
                    bool flag2 = handleImmediately && m_commandType == eCommandType.move;
                    if (flag2)
                    {
                        m_moveHelper.HandleMouseDownForMove(GetMousePoint(), m_snappoint);
                    }
                    m_commandType = eCommandType.move;
                    UpdateCursor();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommandDeleteSelected()
        {
            try
            {
                m_model.DeleteObjects(m_model.SelectedObjects);
                m_model.ClearSelectedObjects();
                DoInvalidate(true);
                UpdateCursor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CommandEdit(string editid)
        {
            try
            {
                CommandEscape();
                m_model.ClearSelectedObjects();
                m_commandType = eCommandType.edit;
                m_editToolId = editid;
                m_editTool = m_model.GetEditTool(m_editToolId);
                UpdateCursor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HandleQuickSnap(KeyEventArgs e)
        {
            try
            {
                bool flag = m_commandType == eCommandType.select || m_commandType == eCommandType.pan;
                if (!flag)
                {
                    ISnapPoint snapPoint = null;
                    UnitPoint mousePoint = GetMousePoint();
                    bool flag2 = m_QuickSnap.ContainsKey(e.KeyCode);
                    if (flag2)
                    {
                        snapPoint = m_model.SnapPoint(m_canvaswrapper, mousePoint, null, m_QuickSnap[e.KeyCode]);
                    }
                    bool flag3 = snapPoint != null;
                    if (flag3)
                    {
                        bool flag4 = m_commandType == eCommandType.draw;
                        if (flag4)
                        {
                            HandleMouseDownWhenDrawing(snapPoint.SnapPoint, snapPoint);
                            bool flag5 = m_newObject != null;
                            if (flag5)
                            {
                                m_newObject.OnMouseMove(m_canvaswrapper, GetMousePoint());
                            }
                            DoInvalidate(true);
                            e.Handled = true;
                        }
                        bool flag6 = m_commandType == eCommandType.move;
                        if (flag6)
                        {
                            m_moveHelper.HandleMouseDownForMove(snapPoint.SnapPoint, snapPoint);
                            e.Handled = true;
                        }
                        bool flag7 = !m_nodeMoveHelper.IsEmpty;
                        if (flag7)
                        {
                            bool flag8 = false;
                            m_nodeMoveHelper.HandleMouseDown(snapPoint.SnapPoint, ref flag8);
                            FinishNodeEdit();
                            e.Handled = true;
                        }
                        bool flag9 = m_commandType == eCommandType.edit;
                        if (flag9)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CanvasCtrller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CanvasCtrller";
            this.Size = new System.Drawing.Size(500, 500);
            this.ResumeLayout(false);

        }
    }
}
