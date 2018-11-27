using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraNavBar;
using SimulationModel;

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
        private DataTable dtSourceProperty;
        private Dictionary<string, NavBarGroup> dicNavBarGroups = new Dictionary<string, NavBarGroup>();
        private Dictionary<string, GridControl> dicGrid = new Dictionary<string, GridControl>();

        private DataModel m_data;

        private Simulator simula = null;

        public FrmMain()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            foreach (NavBarGroup g in navBarControl1.Groups)
            {
                dicNavBarGroups[g.Caption] = g;
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

            //navBarControl1.Groups.Clear();
            foreach (NavBarGroup n in navBarControl1.Groups)
            {
                n.Visible = false;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            SqlDBControl._defultConnectionString = SetFileControl.ReadIniValue("DB", "DBSTRING", Path);
            Xcount = int.Parse(SetFileControl.ReadIniValue("X_Y", "X", Path));
            Ycount = int.Parse(SetFileControl.ReadIniValue("X_Y", "Y", Path));
            txtX.Text = Xcount.ToString();
            txtY.Text = Ycount.ToString();
            using (new WaitDialogForm("正在启动,请稍后...", "提示"))
            {
                InitCanvas();
            }
            dtSourceProperty = new DataTable();
            dtSourceProperty.Columns.Add("Property");
            dtSourceProperty.Columns.Add("VAL");
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DataGridAdd();
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

        public void SetPositionInfo(UnitPoint unitpos)
        {
        }

        public void SetSnapInfo(ISnapPoint snap)
        {
        }

        public void SetHint(string text)
        {
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

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            lock (obj)
            {
                Rectangle rec = e.ClipRectangle;
                float xStart = rec.X;
                float yStart = rec.Y;
                float xEnd = (rec.X + rec.Width);
                float yEnd = (rec.Y + rec.Height);

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                Pen pen = new Pen(Color.White);
                float[] dashArray =
                            {
                                2*Zoom, //线长5个像素
                                4*Zoom //间断2个像素
                            };
                pen.DashPattern = dashArray;
                Pen pen2 = new Pen(Color.Green);
                float[] dashArray2 =
                            {
                                8*Zoom, //线长5个像素
                                32*Zoom //间断2个像素
                            };
                pen2.DashPattern = dashArray2;
                Font font = new Font("Tahoma", 6 * Zoom, FontStyle.Regular);
                GraphicsPath graphicsPath = new GraphicsPath();
                GraphicsPath graphicsPath2 = new GraphicsPath();
                float x1, y1, x2, y2;
                int xC, yC, cC;
                xC = yC = cC = 0;
                for (int i = 0; i <= Xcount; i++)
                {
                    x1 = (20 + i * Distance) * Zoom;
                    if (x1 >= xStart && x1 <= xEnd)
                    {
                        y1 = 20 * Zoom;
                        y2 = (20 + Ycount * Distance) * Zoom;
                        graphicsPath.AddLine(x1, y1, x1, y2);
                        graphicsPath.CloseFigure();
                        xC++;
                    }
                    if (i != Xcount)
                    {
                        x1 = (20 + i * Distance + Distance / 2) * Zoom;
                        if (x1 >= xStart && x1 <= xEnd)
                        {
                            y1 = 20 * Zoom;
                            y2 = (20 + Ycount * Distance) * Zoom;
                            graphicsPath2.AddLine(x1, y1, x1, y2);
                            graphicsPath2.CloseFigure();
                        }
                    }
                }

                for (int i = 0; i < Ycount; i++)
                {
                    y1 = (20 + i * Distance) * Zoom;
                    if (y1 >= yStart && y1 <= yEnd)
                    {
                        x1 = 20 * Zoom;
                        x2 = (20 + Xcount * Distance) * Zoom;
                        graphicsPath.AddLine(x1, y1, x2, y1);
                        graphicsPath.CloseFigure();
                        yC++;
                    }

                    if (i != Ycount)
                    {
                        y1 = (20 + i * Distance + Distance / 2) * Zoom;
                        if (y1 >= yStart && y1 <= yEnd)
                        {
                            x1 = 20 * Zoom;
                            x2 = (20 + Xcount * Distance) * Zoom;
                            graphicsPath2.AddLine(x1, y1, x2, y1);
                            graphicsPath2.CloseFigure();
                        }
                    }
                }

                for (int i = 0; i <= Xcount; i++)
                {
                    for (int j = 0; j <= Ycount; j++)
                    {
                        x1 = (20 + i * Distance) * Zoom;
                        y1 = (20 + j * Distance) * Zoom;
                        if (i != Xcount && j != Ycount && x1 + Distance * Zoom / 2 >= xStart && x1 + Distance * Zoom / 2 <= xEnd && y1 + Distance * Zoom / 2 >= yStart && y1 + Distance * Zoom / 2 <= yEnd)
                        {
                            g.DrawString(i + "," + (Ycount - j - 1), font, Brushes.White, (float)(x1 + Distance * Zoom / 4), (float)(y1 + Distance * Zoom * 0.4));
                            cC++;
                        }
                    }
                }

                g.DrawPath(pen, graphicsPath);
                g.DrawPath(pen2, graphicsPath2);

                //坐标
                x1 = 20 * Zoom;
                y1 = 0;
                y2 = (20 + Ycount * Distance) * Zoom;
                Pen penCoor = new Pen(Color.White);
                AdjustableArrowCap cap = new AdjustableArrowCap(3 * Zoom, 3 * Zoom);
                penCoor.CustomEndCap = cap;
                Font fontCoor = new Font("Tahoma", 9 * Zoom);
                if (x1 >= xStart && x1 <= xEnd)
                {
                    g.DrawLine(penCoor, x1, y2, x1, y1);
                    g.DrawString("Y", fontCoor, Brushes.White, (float)(5 * Zoom), 5 * Zoom);
                }

                x1 = 20 * Zoom;
                x2 = (20 + Xcount * Distance + 20) * Zoom;
                y2 = (20 + Ycount * Distance) * Zoom;
                if (y2 >= yStart && y2 <= yEnd)
                {
                    g.DrawLine(penCoor, x1, y2, x2, y2);
                    g.DrawString("X", fontCoor, Brushes.White, x2 - 20 * Zoom, y2 + 5 * Zoom);
                }
            }
        }

        private void pbMain_MouseMove(object sender, MouseEventArgs e)
        {
            Point coordinate = new Point(MousePosition.X, MousePosition.Y);
            double x = (coordinate.X - 80) / 80;
            double y = (coordinate.Y - 80) / 80;
            lblMsg.Caption = string.Format("Virtual:({0},{1}) || Real:({2}mm,{3}mm)", x, y, Math.Round(x * 600, 2), Math.Round((y) * 600, 2));
        }

        private void InitCanvas()
        {
            m_data = new DataModel();
            m_data.XCount = Xcount;
            m_data.YCount = Ycount;
            m_data.Distance = Distance;
            m_data.Zoom = Zoom;
            m_canvas = new CanvasCtrller(this, m_data);
            m_canvas.Location = new Point(0, 0);
            //m_canvas.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(m_canvas);
            m_canvas.SetCenter(new UnitPoint(0.0, 0.0));
            m_canvas.RunningSnaps = new Type[]
				{
					typeof(VertextSnapPoint),
					typeof(MidpointSnapPoint),
					typeof(IntersectSnapPoint),
					typeof(QuadrantSnapPoint),
					typeof(CenterSnapPoint),
					typeof(DivisionSnapPoint)
				};
            m_canvas.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
            m_canvas.MouseUp += new MouseEventHandler(OnCanvasMouseUp);
            m_canvas.MouseDoubleClick += new MouseEventHandler(OnCanvasMouseDoubleClick);
            m_canvas.TestEvent += m_canvas_TestEvent;
        }

        void m_canvas_TestEvent(object sender, EventArgs e)
        {
            this.Text = sender.ToString() + "@" + Control.MousePosition.X + "," + Control.MousePosition.Y;
        }

        private void pnlMain_Scroll(object sender, ScrollEventArgs e)
        {
            m_canvas.DoInvalidate(true);
        }

        private void pnlMain_SizeChanged(object sender, EventArgs e)
        {
            if (m_canvas != null)
            {
                m_canvas.DoInvalidate(true);
            }
        }

        /// <summary>
        ///  加载AGV车辆信息方法
        /// </summary>
        public void DataGridAdd()
        {
            foreach (NavBarGroup n in navBarControl1.Groups)
            {
                n.Visible = false;
            }
            IEnumerable<IDrawObject> objects = from p in m_data.ActiveLayer.Objects
                                               where p.Id == "AGVTool"
                                               select p;
            m_data.DeleteObjects(objects);

            DataTable table = Function.ReadDB_tb_AGV_Info();

            if (table != null && table.Rows.Count > 0)
            {
                //int agv_Id;
                int agv_Ac, agv_Now_Ord_Count;
                String agv_Ip, agv_Now_X, agv_Now_Y, agv_Skip_No, agv_From, agv_To, agv_Voltage, agv_Electricity;
                String agv_L_Speed, agv_R_Speed, agv_LineNo, agv_LineString, agv_ErrorCord, agv_WarningCord, agv_Now_Ord;
                String agv_Remaining_Trip, agv_Angle, agv_Skip_Angle, agv_Lifting_Speed, agv_Rotating_Speed, agv_OrderNo;
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
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    agv_Ip = table.Rows[j][1].ToString().Trim();
                    agv_Ac = int.Parse(table.Rows[j][2].ToString().Trim());
                    agv_Now_X = table.Rows[j][3].ToString().Trim();
                    agv_Now_Y = table.Rows[j][4].ToString().Trim();

                    agv_Skip_No = table.Rows[j][5].ToString().Trim();
                    agv_From = table.Rows[j][6].ToString().Trim();
                    agv_To = table.Rows[j][7].ToString().Trim();
                    agv_Voltage = table.Rows[j][8].ToString().Trim();
                    agv_Electricity = table.Rows[j][9].ToString().Trim();
                    agv_L_Speed = table.Rows[j][10].ToString().Trim();
                    agv_R_Speed = table.Rows[j][11].ToString().Trim();
                    agv_LineNo = table.Rows[j][12].ToString().Trim();
                    agv_LineString = table.Rows[j][13].ToString().Trim();
                    agv_ErrorCord = table.Rows[j][14].ToString().Trim();
                    agv_WarningCord = table.Rows[j][15].ToString().Trim();
                    agv_Now_Ord = table.Rows[j][16].ToString().Trim();
                    agv_Now_Ord_Count = int.Parse(table.Rows[j][17].ToString().Trim());
                    agv_Remaining_Trip = table.Rows[j][18].ToString().Trim();
                    agv_Angle = table.Rows[j][19].ToString().Trim();
                    agv_Skip_Angle = table.Rows[j][20].ToString().Trim();
                    agv_Lifting_Speed = table.Rows[j][21].ToString().Trim();
                    agv_Rotating_Speed = table.Rows[j][22].ToString().Trim();
                    agv_OrderNo = table.Rows[j][23].ToString().Trim();

                    //TJA2017110811:11
                    carNowX = int.Parse(agv_Now_X) / (double)600;
                    carNowY = int.Parse(agv_Now_Y) / (double)600;
                    carAngle = (double)(int.Parse(table.Rows[j][19].ToString().Trim()));
                    coordinates = "(" + carNowX.ToString() + "," + carNowY.ToString() + ")";
                    //int carNo = int.Parse(agv_Ip.Substring(9,3))-200;
                    carIpEnd = agv_Ip.Substring(10, 3);
                    carID = 0;

                    carID = int.Parse(carIpEnd.Substring(1));
                    //realX = (double)(carNowX * 80 + 80) - (imageWidth) / 2;
                    //realY = (double)((yA - carNowY) * 80 + 80) - (imageHeight) / 2;

                    ////agv = new CanvasAGV();

                    ////rotateTransform = new RotateTransform(((carAngle == 0) ? 0 : (360 - carAngle)), realX + (imageWidth) / 2, realY + (imageHeight) / 2); //XGS2017110522:41

                    //if ((agv_Ac == 0) || (agv_ErrorCord != "0") || (agv_WarningCord != "0"))
                    //{
                    //    color = Brushes.Red;
                    //}
                    //else
                    //{
                    //    color = Brushes.Blue;
                    //}
                    ////agv.GetAGVCanvas(realX, realY, imageHeight, imageWidth, carID.ToString(), this.chartCanvas, color, rotateTransform);
                    //agvUc = new AGV(carID.ToString(), color, ((carAngle == 0) ? 0 : (360 - carAngle)));
                    //agvUc.Uid = "AGV" + carID;
                    //Canvas.SetLeft(agvUc, realX);
                    //Canvas.SetTop(agvUc, realY);
                    //this.chartCanvas.Children.Add(agvUc);


                    if (j < 25)
                    {
                        navBarGroup = dicNavBarGroups[j.ToString()];
                        navBarGroup.Caption = agv_Ip;
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

                        dicGrid["gridControl" + (j + 1)].DataSource = dtTemp;
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
                        
                        dicGrid["gridControl" + (j + 2)].DataSource = dtTemp;

                        navBarGroup.Visible = true;
                    }
                }
            }
        }
    }
}
