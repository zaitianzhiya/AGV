using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Canvas;
using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using Canvas.Layers;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Frames;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraNavBar;
using DAL;
using MonitorAGV_QRCode.Windows;

namespace MonitorAGV_QRCode
{
    public partial class FrmMain : BaseForm, ICanvasOwner, IEditToolOwner
    {
        private static string Path = Application.StartupPath + @"\AGV_Set.ini";
        private static object obj = new object();
        private int Xcount, Ycount;
        private CanvasCtrller m_canvas;
        private int Distance = 40;
        private float Zoom = 1;
        private float StepZoom = 2f;
        private DataTable dtSourceProperty;
        private DataTable dtSourcePropertyOld;
        private Dictionary<string, NavBarGroup> dicNavBarGroups = new Dictionary<string, NavBarGroup>();
        private Dictionary<string, GridControl> dicGrid = new Dictionary<string, GridControl>();
        private bool lockDirection = false;
        private Color lockColor;
        private bool isMonitor = false;
        private UnitPoint p2Before;
        private Point p1Before;

        private DataModel m_data;

        public FrmMain()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            foreach (NavBarGroup g in navBarControl1.Groups)
            {
                dicNavBarGroups[g.Caption] = g;
                g.Visible = false;
            }
            dicGrid[gridControl1.Name] = gridControl1;
            dicGrid[gridControl2.Name] = gridControl2;
            dicGrid[gridControl3.Name] = gridControl3;
            dicGrid[gridControl4.Name] = gridControl4;
            dicGrid[gridControl5.Name] = gridControl5;
            dicGrid[gridControl6.Name] = gridControl6;
            dicGrid[gridControl7.Name] = gridControl7;
            dicGrid[gridControl8.Name] = gridControl8;
            dicGrid[gridControl9.Name] = gridControl9;
            dicGrid[gridControl10.Name] = gridControl10;
            dicGrid[gridControl11.Name] = gridControl11;
            dicGrid[gridControl12.Name] = gridControl12;
            dicGrid[gridControl13.Name] = gridControl13;
            dicGrid[gridControl14.Name] = gridControl14;
            dicGrid[gridControl15.Name] = gridControl15;
            dicGrid[gridControl16.Name] = gridControl16;
            dicGrid[gridControl17.Name] = gridControl17;
            dicGrid[gridControl18.Name] = gridControl18;
            dicGrid[gridControl19.Name] = gridControl19;
            dicGrid[gridControl20.Name] = gridControl20;
            dicGrid[gridControl21.Name] = gridControl21;
            dicGrid[gridControl22.Name] = gridControl22;
            dicGrid[gridControl23.Name] = gridControl23;
            dicGrid[gridControl24.Name] = gridControl24;
            dicGrid[gridControl25.Name] = gridControl25;
            dicGrid[gridControl26.Name] = gridControl26;
            dicGrid[gridControl27.Name] = gridControl27;
            dicGrid[gridControl28.Name] = gridControl28;
            dicGrid[gridControl29.Name] = gridControl29;
            dicGrid[gridControl30.Name] = gridControl30;
            dicGrid[gridControl31.Name] = gridControl31;
            dicGrid[gridControl32.Name] = gridControl32;
            dicGrid[gridControl33.Name] = gridControl33;
            dicGrid[gridControl34.Name] = gridControl34;
            dicGrid[gridControl35.Name] = gridControl35;
            dicGrid[gridControl36.Name] = gridControl36;
            dicGrid[gridControl37.Name] = gridControl37;
            dicGrid[gridControl38.Name] = gridControl38;
            dicGrid[gridControl39.Name] = gridControl39;
            dicGrid[gridControl40.Name] = gridControl40;
            dicGrid[gridControl41.Name] = gridControl41;
            dicGrid[gridControl42.Name] = gridControl42;
            dicGrid[gridControl43.Name] = gridControl43;
            dicGrid[gridControl44.Name] = gridControl44;
            dicGrid[gridControl45.Name] = gridControl45;
            dicGrid[gridControl46.Name] = gridControl46;
            dicGrid[gridControl47.Name] = gridControl47;
            dicGrid[gridControl48.Name] = gridControl48;
            dicGrid[gridControl49.Name] = gridControl49;
            dicGrid[gridControl50.Name] = gridControl50;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lockColor = btnLock.Appearance.BackColor;
            SqlDBControl._defultConnectionString = SetFileControl.ReadIniValue("DB", "DBSTRING", Path);
            Xcount = int.Parse(SetFileControl.ReadIniValue("X_Y", "X", Path));
            Ycount = int.Parse(SetFileControl.ReadIniValue("X_Y", "Y", Path));
            txtX.Text = Xcount.ToString();
            txtY.Text = Ycount.ToString();
            dtSourceProperty = new DataTable();
            dtSourceProperty.Columns.Add("Property");
            dtSourceProperty.Columns.Add("VAL");
            using (new WaitDialogForm("正在启动,请稍后...", "提示"))
            {
                InitCanvas();
                //DataGridAdd();
            }
            //timer1.Enabled = true;

            pnlMain.MouseWheel += pnlMain_MouseWheel;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }

        void pnlMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                UnitPoint oldUnitPoint = p2Before;
                UnitPoint nowUnitPoint = m_canvas.ToUnit(p1Before);
                m_canvas.OffsetX += (int)(Math.Abs(nowUnitPoint.X - oldUnitPoint.X)*m_data.Zoom);
                m_canvas.OffsetY += (int)(Math.Abs(nowUnitPoint.Y - oldUnitPoint.Y) * m_data.Zoom);
                m_canvas.DoInvalidate(true);
                return;
            }
            pnlMain_Scroll(null, null);
        }

        private void pnlMain_Scroll(object sender, ScrollEventArgs e)
        {
            m_canvas.OffsetX = pnlMain.HorizontalScroll.Value;
            m_canvas.OffsetY = pnlMain.VerticalScroll.Value;
            m_canvas.DoInvalidate(true);
        }

        private void pnlMain_SizeChanged(object sender, EventArgs e)
        {
            if (m_canvas != null)
            {
                m_canvas.DoInvalidate(true);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                GridView gv = sender as GridView;
                if (gv != null)
                {
                    e.Info.Appearance.ForeColor = this.ForeColor;
                    e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task t= Task.Run(() => { DataGridAdd(); });
            //t.Wait();
        }

        private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        {

        }

        private void OnCanvasKeyDown(object sender, KeyEventArgs e)
        {

        }

        void m_canvas_MoveEvent(object sender, EventArgs e)
        {
            lblMsg.Caption = sender.ToString();
        }

        void m_canvas_TestEvent(object sender, EventArgs e)
        {
            //pBefore = (UnitPoint)sender;
            //pnlMain.Scroll -= pnlMain_Scroll;
            //pnlMain.MouseWheel -= pnlMain_MouseWheel;
            //this.Text = sender.ToString();
            //m_canvas.OffsetX = (int)(int.Parse(sender.ToString().Split('@')[0])*m_data.Zoom);
            //m_canvas.OffsetY = (int)(int.Parse(sender.ToString().Split('@')[1]) * m_data.Zoom);
            //m_canvas.DoInvalidate(true);
            //pnlMain.Scroll += pnlMain_Scroll;
            //pnlMain.MouseWheel += pnlMain_MouseWheel;
        }

        void m_canvas_ZoomEvent(Point p1, UnitPoint p2)
        {
            p2Before=p2;
            p1Before = p1;
        }

        private void dockPanel2_Collapsed(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            pnlMain.SizeChanged -= pnlMain_SizeChanged;
            if (m_canvas != null)
            {
                m_canvas.DoInvalidate(true);
            }
            pnlMain.SizeChanged += pnlMain_SizeChanged;
        }

        #region 绘图事件
        private void btnHand_ItemClick(object sender, ItemClickEventArgs e)
        {
            string text = string.Empty;

            if (e.Item is BarButtonItem)
            {
                text = ((BarButtonItem)e.Item).Tag.ToString();
            }
            if (text == string.Empty)
            {
                return;
            }
            btnLock.ItemAppearance.Hovered.BackColor = btnLock.ItemAppearance.Normal.BackColor = lockColor;
            if (text == "pan")
            {
                m_canvas.CommandPan();
            }
            else
            {
                m_canvas.CommandSelectDrawTool(text);
            }
        }

        private void btnLock_ItemClick(object sender, ItemClickEventArgs e)
        {
            m_canvas.m_commandType = eCommandType.lockdir;
            lockDirection = !lockDirection;
            if (lockDirection)
            {
                btnLock.ItemAppearance.Hovered.BackColor = btnLock.ItemAppearance.Normal.BackColor = Color.Green;
            }
            else
            {
                btnLock.ItemAppearance.Hovered.BackColor = btnLock.ItemAppearance.Normal.BackColor = lockColor;
            }
        }

        private void btnOpen_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        #endregion 

        #region Button事件
        /// Generate单击事件
        /// <summary>
        /// Generate单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtX.Text.Trim()) <= 0)
                {
                    Msg.Show("X不能小于等于0", MessageBoxIcon.Exclamation);
                    txtX.Focus();
                    txtX.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                Msg.Show("X格式不正确", MessageBoxIcon.Exclamation);
                txtX.Focus();
                txtX.SelectAll();
                return;
            }
            try
            {
                if (int.Parse(txtY.Text.Trim()) <= 0)
                {
                    Msg.Show("Y不能小于等于0", MessageBoxIcon.Exclamation);
                    txtY.Focus();
                    txtY.SelectAll();
                    return;
                }
            }
            catch (Exception ex)
            {
                Msg.Show("Y格式不正确", MessageBoxIcon.Exclamation);
                txtY.Focus();
                txtY.SelectAll();
                return;
            }
            SetFileControl.WriteIniValue("X_Y", "X",txtX.Text.Trim(), Path);
            SetFileControl.WriteIniValue("X_Y", "Y",txtY.Text.Trim(), Path);

            m_data.XCount = Xcount = int.Parse(txtX.Text.Trim());
            m_data.YCount = Ycount = int.Parse(txtY.Text.Trim());
            m_data.Zoom = 1;

            m_canvas.OffsetX=m_canvas.OffsetY = 0;
            m_data.DeleteObjects((from p in m_data.ActiveLayer.Objects select p).ToList());
            m_canvas.SizeChange();
        }

        /// Clear单击事件
        /// <summary>
        /// Clear单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            m_data.DeleteObjects(
             (from p in m_data.ActiveLayer.Objects
              select p).ToList());
            m_canvas.DoInvalidate(true);
        }

        /// 设置任务单击事件
        /// <summary>
        /// 设置任务单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetTask_Click(object sender, EventArgs e)
        {
            using (FrmSetTask frm = new FrmSetTask())
            {
                frm.ShowDialog();
            }
        }

        /// 查询小车状态单击事件
        /// <summary>
        /// 查询小车状态单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchSta_Click(object sender, EventArgs e)
        {

        }

        /// 监控小车单击事件
        /// <summary>
        /// 监控小车单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonitor_Click(object sender, EventArgs e)
        {
            isMonitor = !isMonitor;
            if (isMonitor)
            {
                timer1.Enabled = true;
                btnMonitor.Text = "暂停";
            }
            else
            {
                timer1.Enabled = false;
                btnMonitor.Text = "监控小车";
                Thread.Sleep(500);
                m_data.DeleteObjects((from p in m_data.ActiveLayer.Objects where p.Id=="AGVTool" select p).ToList());
                m_canvas.DoInvalidate(true);
            }
            MonitorStaChange();
        }

        /// 显示状态单击事件
        /// <summary>
        /// 显示状态单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisplaySta_Click(object sender, EventArgs e)
        {
            AddForbidFromDB();
            m_canvas.DoInvalidate(true);
        }

        /// 读取错误信息单击事件
        /// <summary>
        /// 读取错误信息单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadError_Click(object sender, EventArgs e)
        {

        }

        /// 停止小车单击事件
        /// <summary>
        /// 停止小车单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopCar_Click(object sender, EventArgs e)
        {

        }

        /// 打开充电回路单击事件
        /// <summary>
        /// 打开充电回路单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenCharge_Click(object sender, EventArgs e)
        {

        }

        /// 关闭充电回路单击事件
        /// <summary>
        /// 关闭充电回路单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseCharge_Click(object sender, EventArgs e)
        {

        }

        /// 打开二维码报警单击事件
        /// <summary>
        /// 打开二维码报警单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenQRError_Click(object sender, EventArgs e)
        {

        }

        /// 关闭二维码报警单击事件
        /// <summary>
        /// 关闭二维码报警单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCloseQRError_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region MenuItem
        /// 根据配置文件打开地图
        /// <summary>
        /// 根据配置文件打开地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiOpenMapByOption_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 输入参数打开地图
        /// <summary>
        /// 输入参数打开地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiOpenMapByInput_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 打开地图文件
        /// <summary>
        /// 打开地图文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiOpenMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xaml";
            ofd.Filter = "xml file|*.xaml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //此处做你想做的事 ...=ofd.FileName; 
            }
        }

        /// 保存地图文件
        /// <summary>
        /// 保存地图文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiSaveMap_ItemClick(object sender, ItemClickEventArgs e)
        {
          
        }

        /// 显示AGV列表
        /// <summary>
        /// 显示AGV列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiDisplayAgv_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 显示功能按键列表
        /// <summary>
        /// 显示功能按键列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiDisplayFunc_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 显示AGV状态
        /// <summary>
        /// 显示AGV状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiDisplayAgvSta_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 设置地图
        /// <summary>
        /// 设置地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiSetupMap_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 保存地图信息
        /// <summary>
        /// 保存地图信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiSaveMapInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Msg.Show("是否确定要保存此地图信息？", MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            Function.PR_Update_Map_Info(Xcount, Ycount, 0);
            Msg.Show("地图信息保存成功",MessageBoxIcon.Asterisk);
        }

        /// while
        /// <summary>
        /// while
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiWhile_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 文件配置
        /// <summary>
        /// 文件配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiFileSet_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// 手动输入
        /// <summary>
        /// 手动输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiInput_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        #endregion

        #region Function
        public void SetPositionInfo(UnitPoint unitpos)
        {
        }

        public void SetSnapInfo(ISnapPoint snap)
        {
        }

        public void SetHint(string text)
        {
        }

        /// 初始画布方法
        /// <summary>
        /// 初始画布方法
        /// </summary>
        private void InitCanvas()
        {
            m_data = new DataModel();
            m_data.XCount = Xcount;
            m_data.YCount = Ycount;
            m_data.Distance = Distance;
            m_data.Zoom = Zoom;
            m_data.AddDrawTool(btnLeft.Tag.ToString(), new ArrowLeft());
            m_data.AddDrawTool(btnRight.Tag.ToString(), new ArrowRight());
            m_data.AddDrawTool(btnUp.Tag.ToString(), new ArrowUp());
            m_data.AddDrawTool(btnDown.Tag.ToString(), new ArrowDown());
            m_data.AddDrawTool(btnCharge.Tag.ToString(), new ChargeTool());
            m_data.AddDrawTool(btnForbid.Tag.ToString(), new Forbid());
            m_data.AddDrawTool(btnShelf.Tag.ToString(), new Shelf());
            m_canvas = new CanvasCtrller(this, m_data);
            m_canvas.stepZoom = StepZoom;
            m_canvas.Location = new Point(0, 0);
            m_canvas.parentPanel = pnlMain;
            AddForbidFromDB();
            //m_canvas.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(m_canvas);
            m_canvas.SetCenter(new UnitPoint(0.0, 0.0));
            //m_canvas.RunningSnaps = new Type[]
            //    {
            //        typeof(VertextSnapPoint),
            //        typeof(MidpointSnapPoint),
            //        typeof(IntersectSnapPoint),
            //        typeof(QuadrantSnapPoint),
            //        typeof(CenterSnapPoint),
            //        typeof(DivisionSnapPoint)
            //    };
            m_canvas.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
            m_canvas.MouseUp += new MouseEventHandler(OnCanvasMouseUp);
            m_canvas.MouseDoubleClick += new MouseEventHandler(OnCanvasMouseDoubleClick);
            m_canvas.MoveEvent += m_canvas_MoveEvent;

            m_canvas.TestEvent += m_canvas_TestEvent;

            m_canvas.ZoomEvent += m_canvas_ZoomEvent;
        }

        ///  加载AGV车辆信息方法
        /// <summary>
        ///  加载AGV车辆信息方法
        /// </summary>
        private void DataGridAdd()
        {
            //try
            //{
                Thread.Sleep(500);
                lock (obj)
                {
                    List<AGVTool> lstAgv = (from p in m_data.ActiveLayer.Objects
                                            where p.Id == "AGVTool"
                                            select (AGVTool)p).ToList<AGVTool>();

                    List<string> lstAgvNo = (from p in m_data.ActiveLayer.Objects
                                             where p.Id == "AGVTool"
                                             select ((AGVTool)p).AgvNo).ToList();

                    //m_data.DeleteObjects(objects);

                    DataTable table = Function.ReadDB_tb_AGV_Info();

                    if (table != null && table.Rows.Count > 0)
                    {
                        //int agv_Id;
                        int agv_Ac, agv_Now_Ord_Count;
                        String agv_Ip, agv_Now_X, agv_Now_Y, agv_Skip_No, agv_From, agv_To, agv_Voltage, agv_Electricity;
                        String agv_L_Speed,
                            agv_R_Speed,
                            agv_LineNo,
                            agv_LineString,
                            agv_ErrorCord,
                            agv_WarningCord,
                            agv_Now_Ord;
                        String agv_Remaining_Trip,
                            agv_Angle,
                            agv_Skip_Angle,
                            agv_Lifting_Speed,
                            agv_Rotating_Speed,
                            agv_OrderNo;
                        String coordinates;
                        double carNowX;
                        double carNowY;
                        double carAngle;
                        string carIpEnd;
                        int carID = 0;
                        double realX;
                        double realY;
                        NavBarGroup navBarGroup;
                        DataRow drNew;
                        DataTable dtTemp;
                        bool isSame;
                        bool isSameAgv = true;

                        DrawingLayer lay = (DrawingLayer)m_data.ActiveLayer;
                        AGVTool agv;
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            isSame = true;
                            if (dtSourcePropertyOld != null && dtSourcePropertyOld.Rows.Count > 0)
                            {
                                if (dtSourcePropertyOld.Rows.Count >= j)
                                {
                                    foreach (DataColumn dc in table.Columns)
                                    {
                                        if (table.Rows[j][dc.ColumnName].ToString() !=
                                            dtSourcePropertyOld.Rows[j][dc.ColumnName].ToString())
                                        {
                                            isSame = false;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    isSame = false;
                                }
                            }
                            else
                            {
                                isSame = false;
                            }

                            if (j < 25)
                            {
                                navBarGroup = dicNavBarGroups[j.ToString()];
                                agv_Ip = table.Rows[j][1].ToString().Trim();
                                agv_Ac = int.Parse(table.Rows[j][2].ToString().Trim());
                                agv_Now_X = table.Rows[j][3].ToString().Trim();
                                agv_Now_Y = table.Rows[j][4].ToString().Trim();
                                agv_ErrorCord = table.Rows[j][14].ToString().Trim();
                                agv_WarningCord = table.Rows[j][15].ToString().Trim();

                                carIpEnd = agv_Ip.Substring(10, 3);
                                carID = int.Parse(carIpEnd.Substring(1));

                                carNowX = int.Parse(agv_Now_X) / (double)600;
                                carNowY = int.Parse(agv_Now_Y) / (double)600;
                                carAngle = (double)(int.Parse(table.Rows[j][19].ToString().Trim()));

                                if (lstAgvNo.Contains(carID.ToString()))
                                {
                                    agv = lstAgv.AsEnumerable().Where(p => p.AgvNo == carID.ToString()).Single();
                                    UnitPoint location = new UnitPoint(20 + carNowX * Distance, 20 + (Ycount - carNowY) * Distance);
                                    if (location != agv.Location)
                                    {
                                        isSameAgv = false;
                                        agv.Location = location;
                                    }
                                    Color color;
                                    if ((agv_Ac == 0) || (agv_ErrorCord != "0") || (agv_WarningCord != "0"))
                                    {
                                        color = Color.Red;
                                    }
                                    else
                                    {
                                        color = Color.Blue;
                                    }
                                    if (agv.AgvColor != color)
                                    {
                                        isSameAgv = false;
                                        agv.AgvColor = color;
                                    }
                                    float angle = carAngle == 0 ? 0f : (float)(360 - carAngle);
                                    if (angle != agv.Angle)
                                    {
                                        isSameAgv = false;
                                        agv.Angle = angle;
                                    }
                                    lstAgvNo.Remove(carID.ToString());
                                }
                                else
                                {
                                    agv = new AGVTool();
                                    agv.AgvNo = carID.ToString();
                                    agv.Location = new UnitPoint(20 + carNowX * Distance, 20 + (Ycount - carNowY) * Distance);
                                    if ((agv_Ac == 0) || (agv_ErrorCord != "0") || (agv_WarningCord != "0"))
                                    {
                                        agv.AgvColor = Color.Red;
                                    }
                                    else
                                    {
                                        agv.AgvColor = Color.Blue;
                                    }
                                    agv.Angle = carAngle == 0 ? 0f : (float)(360 - carAngle);
                                    lay.AddObject(agv);
                                    isSameAgv = false;
                                }

                                if (!isSame)
                                {
                                    agv_Skip_No = table.Rows[j][5].ToString().Trim();
                                    agv_From = table.Rows[j][6].ToString().Trim();
                                    agv_To = table.Rows[j][7].ToString().Trim();
                                    agv_Voltage = table.Rows[j][8].ToString().Trim();
                                    agv_Electricity = table.Rows[j][9].ToString().Trim();
                                    agv_L_Speed = table.Rows[j][10].ToString().Trim();
                                    agv_R_Speed = table.Rows[j][11].ToString().Trim();
                                    agv_LineNo = table.Rows[j][12].ToString().Trim();
                                    agv_LineString = table.Rows[j][13].ToString().Trim();

                                    agv_Now_Ord = table.Rows[j][16].ToString().Trim();
                                    agv_Now_Ord_Count = int.Parse(table.Rows[j][17].ToString().Trim());
                                    agv_Remaining_Trip = table.Rows[j][18].ToString().Trim();
                                    agv_Angle = table.Rows[j][19].ToString().Trim();
                                    agv_Skip_Angle = table.Rows[j][20].ToString().Trim();
                                    agv_Lifting_Speed = table.Rows[j][21].ToString().Trim();
                                    agv_Rotating_Speed = table.Rows[j][22].ToString().Trim();
                                    agv_OrderNo = table.Rows[j][23].ToString().Trim();

                                    coordinates = "(" + carNowX.ToString() + "," + carNowY.ToString() + ")";

                                    this.Invoke(new Action(()=> {navBarGroup.Caption = agv_Ip;}));
                                    
                                    dtTemp = dtSourceProperty.Clone();
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "坐标";
                                    drNew["VAL"] = coordinates;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "AGV当前角度值";
                                    drNew["VAL"] = agv_Angle;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "料架当前角度值";
                                    drNew["VAL"] = agv_Skip_Angle;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "当前任务执行编号";
                                    drNew["VAL"] = agv_Now_Ord;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "当前任务数";
                                    drNew["VAL"] = agv_Now_Ord_Count;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "剩余行程总和";
                                    drNew["VAL"] = agv_Remaining_Trip;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "左轮转速";
                                    drNew["VAL"] = agv_L_Speed;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "右轮转速";
                                    drNew["VAL"] = agv_R_Speed;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "举升电机速度";
                                    drNew["VAL"] = agv_Lifting_Speed;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "旋转电机速度";
                                    drNew["VAL"] = agv_Rotating_Speed;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "电池电压值";
                                    drNew["VAL"] = agv_Voltage;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "系统电流值";
                                    drNew["VAL"] = agv_Electricity;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "错误代码";
                                    drNew["VAL"] = agv_ErrorCord;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "警告代码";
                                    drNew["VAL"] = agv_WarningCord;
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "当前货架编号";
                                    drNew["VAL"] = agv_LineNo;
                                    dtTemp.Rows.Add(drNew);

                                    this.Invoke(new Action(() => { dicGrid["gridControl" + (j * 2 + 1)].DataSource = dtTemp; }));
                                    dtTemp = dtSourceProperty.Clone();
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "执行任务状态";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "顶升复位标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "转盘复位标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "陀螺仪零偏纠正标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "携带料架信息";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "位置确定标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "小车当前故障标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "小车当前警告标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "激光感应器远距离标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "激光感应器中距离标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "激光感应器近距离标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "前端料架感应器标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "后端料架感应器标志";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);
                                    drNew = dtTemp.NewRow();
                                    drNew["Property"] = "休眠状态指示";
                                    drNew["VAL"] = "";
                                    dtTemp.Rows.Add(drNew);

                                    this.Invoke(new Action(() => { dicGrid["gridControl" + (j * 2 + 2)].DataSource = dtTemp; }));
                                }
                                this.Invoke(new Action(() => { navBarGroup.Visible = true; }));
                            }
                        }
                        if (lstAgvNo.Count > 0)
                        {
                            m_data.DeleteObjects(lstAgv.AsEnumerable().Where(p => lstAgvNo.Contains(p.AgvNo)).ToList());
                        }

                        foreach (string key in dicNavBarGroups.Keys)
                        {
                            if (int.Parse(key) >= table.Rows.Count)
                            {
                                this.Invoke(new Action(() => { dicNavBarGroups[key].Visible = false; }));
                            }
                        }
                        dtSourcePropertyOld = table;
                        if (!isSameAgv)
                        {
                            m_canvas.DoInvalidate(true);
                        }
                    }
                }
            //}
            //catch (InvalidOperationException ex)
            //{

            //}
        }

        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        private void AddForbidFromDB()
        {
            DataTable MapTable = Function.PR_Read_Map_FQ();
            if (MapTable != null && MapTable.Rows.Count > 0)
            {
                DrawingLayer lay = (DrawingLayer)m_data.ActiveLayer;
                for (int j = 0; j < MapTable.Rows.Count; j++)
                {
                    int MapNo = int.Parse(MapTable.Rows[j][0].ToString().Trim());
                    int MapUsed = int.Parse(MapTable.Rows[j][1].ToString().Trim());
                    int Map_A = int.Parse(MapTable.Rows[j][2].ToString().Trim());
                    int Map_B = int.Parse(MapTable.Rows[j][3].ToString().Trim());
                    int Map_C = int.Parse(MapTable.Rows[j][4].ToString().Trim());
                    int Map_D = int.Parse(MapTable.Rows[j][5].ToString().Trim());
                    int Real_A = MapNo - int.Parse(txtX.Text);
                    int Real_B = MapNo;
                    int Real_C = MapNo + 2;
                    int Real_D = MapNo + 2 + int.Parse(txtX.Text);
                    if (MapUsed == 2)
                    {
                        Forbid forbid = new Forbid();
                        forbid.MapNo = MapNo;
                        forbid.X = MapNo % Xcount;
                        forbid.Y = MapNo / Xcount;
                        forbid.Location = new UnitPoint(20 + forbid.X * Distance, 20 + (Ycount - forbid.Y) * Distance - (float)Distance);
                        lay.AddObject(forbid);
                    }
                    else
                    {
                        //Left
                        if (Real_B > 0 && (Real_B == Map_A | Real_B == Map_B | Real_B == Map_C | Real_B == Map_D))
                        {
                            ArrowLeft arrow = new ArrowLeft();
                            arrow.MapNo = MapNo;
                            arrow.X = MapNo % Xcount;
                            arrow.Y = MapNo / Xcount;
                            arrow.Location = new UnitPoint(20 + arrow.X * Distance + (float)Distance / 2, 20 + (Ycount - arrow.Y) * Distance - (float)Distance / 2);
                            lay.AddObject(arrow);
                        }
                        //right//B
                        if (Real_C > 0 && (Real_C == Map_A | Real_C == Map_B | Real_C == Map_C | Real_C == Map_D))
                        {
                            ArrowRight arrow = new ArrowRight();
                            arrow.MapNo = MapNo;
                            arrow.X = MapNo % Xcount;
                            arrow.Y = MapNo / Xcount;
                            arrow.Location = new UnitPoint(20 + arrow.X * Distance + (float)Distance / 2, 20 + (Ycount - arrow.Y) * Distance - (float)Distance / 2);
                            lay.AddObject(arrow);
                        }
                        //top//D
                        if (Real_D > 0 && (Real_D == Map_A | Real_D == Map_B | Real_D == Map_C | Real_D == Map_D))
                        {
                            ArrowUp arrow = new ArrowUp();
                            arrow.MapNo = MapNo;
                            arrow.X = MapNo % Xcount;
                            arrow.Y = MapNo / Xcount;
                            arrow.Location = new UnitPoint(20 + arrow.X * Distance + (float)Distance / 2, 20 + (Ycount - arrow.Y) * Distance - (float)Distance / 2);
                            lay.AddObject(arrow);
                        }
                        //down//A
                        if (Real_A > 0 && (Real_A == Map_A | Real_A == Map_B | Real_A == Map_C | Real_A == Map_D))
                        {
                            ArrowDown arrow = new ArrowDown();
                            arrow.MapNo = MapNo;
                            arrow.X = MapNo % Xcount;
                            arrow.Y = MapNo / Xcount;
                            arrow.Location = new UnitPoint(20 + arrow.X * Distance + (float)Distance / 2, 20 + (Ycount - arrow.Y) * Distance - (float)Distance / 2);
                            lay.AddObject(arrow);
                        }
                    }
                }
            }
        }

        /// 监控小车时控件状态变更方法
        /// <summary>
        /// 监控小车时控件状态变更方法
        /// </summary>
        private void MonitorStaChange()
        {
            bsiOpen.Enabled =
                bsiFile.Enabled =
                    bsiView.Enabled = bsiSetup.Enabled = bsiDebug.Enabled = bsiOptionTask.Enabled = !isMonitor;
            txtX.Enabled =
                txtY.Enabled =
                    btnSetTask.Enabled =
                        btnSearchSta.Enabled =
                            btnDisplaySta.Enabled =
                                btnReadError.Enabled =
                                    btnStopCar.Enabled =
                                        btnOpenCharge.Enabled =
                                            btnCloseCharge.Enabled =
                                                btnOpenQRError.Enabled = btnCloseQRError.Enabled = !isMonitor;
            btnHand.Enabled =
                btnPath.Enabled =
                    btnForbid.Enabled =
                        btnCharge.Enabled =
                            btnShelf.Enabled =
                                btnLeft.Enabled =
                                    btnRight.Enabled = btnUp.Enabled = btnDown.Enabled = btnLock.Enabled = btnAdvice.Enabled
                                        = btnDistance.Enabled = btnDesignPath.Enabled = btnOpen.Enabled = !isMonitor;
        }
        #endregion
    }
}
