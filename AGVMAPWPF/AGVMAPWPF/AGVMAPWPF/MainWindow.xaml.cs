using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.XtraExport;
using DevExpress.XtraPrinting.Native;
using Microsoft.Win32;
using Cursors = System.Windows.Input.Cursors;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Path = System.Windows.Shapes.Path;
using RadioButton = System.Windows.Controls.RadioButton;
using ToolBar = System.Windows.Controls.ToolBar;
using System.Windows.Threading;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Drawing.Drawing2D;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace AGVMAPWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 自定义 变量
        /*未解决问题：
         1.坐标系矢量放大缩小到一定程度后消失
         2.使两直线相交，打断直线未考虑为0的情况
         */

        private Brush canvasBrush, coorBrush, penBrush;//画布背景色，栅格背景色，画笔颜色
        private double penSize;//画笔大小
        private bool IsShowCoordinate;//是否显示栅格
        private string displayMode = "L";//栅格显示方式  L-线  P-点
        private int Scale = 50;//最小刻度尺寸
        private int coordinateCount = 50;//坐标轴线数
        double oneScale = 2;

        private double imageHeight = 50;//图片高度
        private double imageWidth = 100;//图片宽度
        private double callRadius = 70;//呼叫器直径大小
        private double storeWidth = 50;//储位宽度
        private double ellipseRad = 10;//地标半径
        private double arrowAngle = 28;//箭头角度
        private double arrowDistance = 20;//箭头与顶点坐标距离
        private double pointWidth = 1;//点阵坐标点直径大小
        private double centerLineWidth = 70;//中心坐标线一半大小
        private double carHeight = 30;//小车高度
        private double carWidth = 30;//小车宽度

        Point CenterPoint = new Point();//中心点坐标
        private Point CenterPointTrans;//line1相对中心点坐标
        private Line line1, line2;

        //移动标志
        bool isMoving = false;
        //鼠标按下去的位置
        Point startMovePosition;
        TranslateTransform totalTranslate = new TranslateTransform();
        TranslateTransform tempTranslate = new TranslateTransform();
        ScaleTransform totalScale = new ScaleTransform();
        TransformGroup tfGroup;
        Double scaleLevel = 1;//缩放比例
        private string rabType = "";//操作选择
        private int lineArrowCount = 1;//地标直线数量
        private int imageCount = 1;//图片数量
        private int callCount = 1;//呼叫器数量
        private int storeCount = 1;//储位数量
        private int stationCount = 1;//地标数量
        private int fontCount = 1;//文字数量
        private int lineCount = 1;//直线数量
        private int curveCount = 1;//曲线数量
        private int stationLineCount = 1;//地标直线数量
        private int stationCurveCount = 1;//地标曲线数量

        Point? startLine;//地标直线起点，终点
        //private Dictionary<string, Path> dicLineArrow = new Dictionary<string, Path>();//地标直线集合
        private Path lastPath;//显示的最后一个地标直线
        private Polygon lastPolygon;//显示的最后一个地标直线箭头
        private Rectangle firstStation;//第一个地标
        private Rectangle secondStation;//第二个地标
        private Rectangle stationCurrent;//当前正在操作的地标内接正方形

        private Point? lineStart = null;//直线起点
        private Path lastLine = null;//显示的最后一个直线

        private Point? curveP1, curveP2, curveP3;//曲线/地标曲线经过的点 curveP1(起点)，curveP2(结束点),curveP3(二次方程式贝塞尔曲线最后一个点),curveP4(三次方程式贝塞尔曲线最后一个点)
        private Path lastCurve = null;//显示的最后一个曲线/地标曲线

        private bool isOneClick = false;//是否一次事件，mouseleftbuttondown一次点击两次触发

        //private Dictionary<string, Point> dicPoint = new Dictionary<string, Point>();//点阵坐标点集合 

        private Dictionary<string, Point> dicImage = new Dictionary<string, Point>();//图片中心点集合 
        private Dictionary<string, UIElement> dicImageUc = new Dictionary<string, UIElement>();//图片集合 

        private Dictionary<string, Point> dicCall = new Dictionary<string, Point>();//呼叫器中心点集合 
        private Dictionary<string, UIElement> dicCallUc = new Dictionary<string, UIElement>();//呼叫器集合 

        private Dictionary<string, Point> dicStore = new Dictionary<string, Point>();//储位中心点集合 
        private Dictionary<string, UIElement> dicStoreUc = new Dictionary<string, UIElement>();//储位集合 

        private Dictionary<string, Point> dicStation = new Dictionary<string, Point>();//地标中心点集合 
        private Dictionary<string, UIElement> dicStationUc = new Dictionary<string, UIElement>();//地标集合 

        private Dictionary<string, Point> dicFont = new Dictionary<string, Point>();//文字集合 
        private Dictionary<string, UIElement> dicFontUc = new Dictionary<string, UIElement>();//文字集合 

        private Dictionary<string, UIElement> dicLineUc = new Dictionary<string, UIElement>();//直线集合 

        private Dictionary<string, UIElement> dicCurveUc = new Dictionary<string, UIElement>();//曲线集合 

        private Dictionary<string, Path> dicStationLineUc = new Dictionary<string, Path>();//地标直线集合 
        private Dictionary<string, Polygon> dicStationLineArrowUc = new Dictionary<string, Polygon>();//地标直线箭头集合

        private Dictionary<string, Path> dicstationCurveUc = new Dictionary<string, Path>();//地标曲线集合 
        private Dictionary<string, Polygon> dicstationCurveArrowUc = new Dictionary<string, Polygon>();//地标曲线箭头集合

        private Dictionary<string, Point> dicCar = new Dictionary<string, Point>();//小车中心点集合 
        private Dictionary<string, UIElement> dicCarUc = new Dictionary<string, UIElement>();//小车集合

        private string selectType = "";//上一个选择元素的类型 A-地标 B-地标直线 C-地标曲线 D-直线 E-曲线 F-储位 G-文字 H-图片 I-呼叫器
        private UIElement selectUiElement = null;//选择的元素
        private UIElement selectUiElement2 = null;//选择的元素2  地标直线、地标曲线需要

        internal static Brush selectBrush = Brushes.BlueViolet;//选中元素颜色

        private Path pathSel1;//选中的两条直线 直线相交，打断时使用
        private Point pointClick1, pointClick2;//选中的两条直线单击点 直线相交，打断时使用
        private Brush brushOld;//选中直线原来颜色 直线相交，打断时使用

        private double left, top, left2, top2;//移动时原始的Canvas.Left,Canvas.Top值
        private Dictionary<string, Point> dicOldPoint = new Dictionary<string, Point>();//移动时原始的绘制坐标点

        private Brush brushPre;//选中项原来的颜色，文字，直线，曲线，地标直线，地标曲线使用

        ObservableCollection<Member> itemSource = new ObservableCollection<Member>();//DataGrid数据源

        private DataTable dtSource;//所有已选择的更改后的数据源

        private DispatcherTimer timer = new DispatcherTimer();

        private DataTable dtCarInfo, dtCarInfoOld;//小车最新信息与上次获取的信息

        private bool isStart = false;//是否启动

        List<string> lstStation = new List<string>();//地标指向地标集合
        private StationWindow station1, station2;//地标直线，曲线关联的地标
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        /// 加载事件
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SqlDBControl._defultConnectionString = File.ReadAllText(System.Windows.Forms.Application.StartupPath + @"\ConnectionString").Trim();
            DataTable dtArea = Function.GetDataInfo("PR_SELECT_AREA_INFO");
            dataGridArea.ItemsSource = dtArea.DefaultView;

            cmbArea.ItemsSource = dtArea.DefaultView;

            dtSource = new DataTable();
            dtSource.Columns.Add("Uid");
            dtSource.Columns.Add("Color");
            dtSource.Columns.Add("LandCode");
            dtSource.Columns.Add("LandName");
            dtSource.Columns.Add("MidPoint");
            dtSource.Columns.Add("ExcuteAngle");
            dtSource.Columns.Add("Length");
            dtSource.Columns.Add("P1");
            dtSource.Columns.Add("P2");

            dtSource.Columns.Add("CurrSelectPoint");
            dtSource.Columns.Add("LankMarkCode");
            dtSource.Columns.Add("StcokID");
            dtSource.Columns.Add("StorageName");
            dtSource.Columns.Add("StorageState");
            dtSource.Columns.Add("FontSize");
            dtSource.Columns.Add("StrValue");
            dtSource.Columns.Add("Height");
            dtSource.Columns.Add("Image");
            dtSource.Columns.Add("Location");
            dtSource.Columns.Add("TransColor");
            dtSource.Columns.Add("Width");
            dtSource.Columns.Add("CallBoxID");
            dtSource.Columns.Add("Radius");
            dtSource.Columns.Add("Area");
            dtSource.Columns.Add("ImageUrl");

            //取得ini档数据
            GetIniData();

            InitNull();
            this.SizeChanged += MainWindow_SizeChanged;
            CanvasMain.Focusable = true;
            CanvasMain.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            CanvasMain.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            CanvasMain.Background = Brushes.Transparent;

            this.KeyDown += MainWindow_KeyDown;

            CanvasMain.PreviewMouseLeftButtonUp += CanvasMain_PreviewMouseLeftButtonUp;
            CanvasMain.MouseWheel += CanvasMain_OnMouseWheel;
            CanvasMain.MouseLeftButtonDown += CanvasMain_OnMouseLeftButtonDown;
            CanvasMain.PreviewMouseMove += CanvasMain_PreviewMouseMove;
            CanvasMain.MouseRightButtonDown += CanvasMain_MouseRightButtonDown;
            CanvasMain.MouseRightButtonUp += CanvasMain_MouseRightButtonUp;
            //X = int.Parse(FileControl.SetFileControl.ReadIniValue("X_Y", "X", System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini").Trim());
            //Y = int.Parse(FileControl.SetFileControl.ReadIniValue("X_Y", "Y", System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini").Trim());
          

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            string fileName = System.Windows.Forms.Application.StartupPath + @"\AGVINFO.agv";
            DataTable dtMap = Function.GetDataInfo("PR_SELECT_TBPLANSET");
            if (dtMap.Rows.Count > 0)
            {
                DataRow[] drs = dtMap.Select("fileName='AGVINFO.agv'");
                if (drs.Length > 0)
                {
                    byte[] bytes = (byte[])drs[0]["FileContent"];
                    int size = bytes.GetUpperBound(0); //获得数据库中存储的位流数组的维度上限，用作读取流的上限

                    using (
                        FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write,
                            FileShare.ReadWrite))
                    {
                        fs.Write(bytes, 0, size + 1);
                    }
                }
            }
            if (File.Exists(fileName))
            {
                OpenMap(fileName);
            }
            else
            {
                InitCenter();
            }

            //InitCenter();

            CenterPointTrans = line1.TranslatePoint(CenterPoint, CanvasMain);
        }

        /// timer定时执行事件
        /// <summary>
        /// timer定时执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            GeneralCar();
        }

        /// 窗体KEYDOWN事件
        /// <summary>
        /// 窗体KEYDOWN事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                if (rabType == "B" && lastPath != null)
                {
                    CanvasMain.Children.Remove(lastPath);
                    CanvasMain.Children.Remove(lastPolygon);
                    InitNull();
                }
                if (rabType == "D" && lastLine != null)
                {
                    CanvasMain.Children.Remove(lastLine);
                    InitNull();
                }
                if ((rabType == "E" || rabType == "C") && lastCurve != null)
                {
                    CanvasMain.Children.Remove(lastCurve);
                    if (rabType == "C")
                    {
                        if (lastPolygon != null)
                        {
                            CanvasMain.Children.Remove(lastPolygon);
                        }
                    }
                    InitNull();
                }
                //移动取消
                if (rabType == "N")
                {
                    if (lastLine != null)
                    {
                        if (selectType == "A" || selectType == "F" || selectType == "G" || selectType == "H" ||
                            selectType == "I")
                        {
                            if (selectUiElement != null)
                            {
                                Canvas.SetLeft(selectUiElement, left);
                                Canvas.SetTop(selectUiElement, top);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(selectType))
                            {
                                LineGeometry lineGeometryTemp;
                                Polygon polygonTemp;
                                PathFigure pathFigureTemp;
                                BezierSegment bezierSegmentTemp;
                                switch (selectType)
                                {
                                    case "B":
                                        lineGeometryTemp = (LineGeometry)(((Path)selectUiElement).Data);
                                        lineGeometryTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X,
                                            dicOldPoint["StartPoint"].Y);
                                        lineGeometryTemp.EndPoint = new Point(dicOldPoint["EndPoint"].X,
                                            dicOldPoint["EndPoint"].Y);

                                        polygonTemp = (Polygon)selectUiElement2;
                                        polygonTemp.Points[0] = new Point(dicOldPoint["Point1"].X,
                                            dicOldPoint["Point1"].Y);
                                        polygonTemp.Points[1] = new Point(dicOldPoint["Point2"].X,
                                            dicOldPoint["Point2"].Y);
                                        polygonTemp.Points[2] = new Point(dicOldPoint["Point3"].X,
                                            dicOldPoint["Point3"].Y);
                                        break;
                                    case "C":
                                        pathFigureTemp = ((PathGeometry)(((Path)selectUiElement).Data)).Figures[0];
                                        pathFigureTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X,
                                            dicOldPoint["StartPoint"].Y);
                                        bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                                        bezierSegmentTemp.Point1 = new Point(dicOldPoint["Point1"].X,
                                            dicOldPoint["Point1"].Y);
                                        bezierSegmentTemp.Point2 = new Point(dicOldPoint["Point2"].X,
                                            dicOldPoint["Point2"].Y);
                                        bezierSegmentTemp.Point3 = new Point(dicOldPoint["Point3"].X,
                                            dicOldPoint["Point3"].Y);

                                        polygonTemp = (Polygon)selectUiElement2;
                                        polygonTemp.Points[0] = new Point(dicOldPoint["Point11"].X,
                                            dicOldPoint["Point11"].Y);
                                        polygonTemp.Points[1] = new Point(dicOldPoint["Point21"].X,
                                            dicOldPoint["Point21"].Y);
                                        polygonTemp.Points[2] = new Point(dicOldPoint["Point31"].X,
                                            dicOldPoint["Point31"].Y);
                                        break;
                                    case "D":
                                        lineGeometryTemp = (LineGeometry)(((Path)selectUiElement).Data);
                                        lineGeometryTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X,
                                            dicOldPoint["StartPoint"].Y);
                                        lineGeometryTemp.EndPoint = new Point(dicOldPoint["EndPoint"].X,
                                            dicOldPoint["EndPoint"].Y);
                                        break;

                                    case "E":
                                        pathFigureTemp = ((PathGeometry)(((Path)selectUiElement).Data)).Figures[0];
                                        pathFigureTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X,
                                            dicOldPoint["StartPoint"].Y);
                                        bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                                        bezierSegmentTemp.Point1 = new Point(dicOldPoint["Point1"].X,
                                            dicOldPoint["Point1"].Y);
                                        bezierSegmentTemp.Point2 = new Point(dicOldPoint["Point2"].X,
                                            dicOldPoint["Point2"].Y);
                                        bezierSegmentTemp.Point3 = new Point(dicOldPoint["Point3"].X,
                                            dicOldPoint["Point3"].Y);
                                        break;
                                }
                            }
                        }
                        CanvasMain.Children.Remove(lastLine);
                        InitNull();
                    }
                }
            }
            //删除
            if (rabType == "L" && Keyboard.IsKeyDown(Key.Delete) && selectUiElement != null)
            {
                switch (selectType)
                {
                    case "A":
                        dicStation.Remove(selectUiElement.Uid);
                        dicStationUc.Remove(selectUiElement.Uid);
                        break;
                    case "B":
                        dicStationLineUc.Remove(selectUiElement.Uid);
                        dicStationLineArrowUc.Remove(selectUiElement2.Uid);
                        break;
                    case "C":
                        dicstationCurveUc.Remove(selectUiElement.Uid);
                        dicstationCurveArrowUc.Remove(selectUiElement2.Uid);
                        break;
                    case "D":
                        dicLineUc.Remove(selectUiElement.Uid);
                        break;
                    case "E":
                        dicCurveUc.Remove(selectUiElement.Uid);
                        break;
                    case "F":
                        dicStore.Remove(selectUiElement.Uid);
                        dicStoreUc.Remove(selectUiElement.Uid);
                        break;
                    case "G":
                        dicFont.Remove(selectUiElement.Uid);
                        dicFontUc.Remove(selectUiElement.Uid);
                        break;
                    case "H":
                        dicImage.Remove(selectUiElement.Uid);
                        dicImageUc.Remove(selectUiElement.Uid);
                        break;
                    case "I":
                        dicCall.Remove(selectUiElement.Uid);
                        dicCallUc.Remove(selectUiElement.Uid);
                        break;
                }
                CanvasMain.Children.Remove(selectUiElement);
                if (selectUiElement2 != null)
                {
                    CanvasMain.Children.Remove(selectUiElement2);
                }
            }
        }

        /// 窗体尺寸变更事件
        /// <summary>
        /// 窗体尺寸变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //InitCenter();
            //InitialCanvas();
        }

        /// ToolBar_OnLoaded事件（隐藏溢出箭头）
        /// <summary>
        /// ToolBar_OnLoaded事件（隐藏溢出箭头）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        /// 按钮功能单击事件
        /// <summary>
        /// 按钮功能单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RabPoint_OnClick(object sender, RoutedEventArgs e)
        {
            RadioButton rab = sender as RadioButton;
            if (rab != null)
            {
                rabType = rab.Tag.ToString();
            }
            if (pathSel1 != null)
            {
                pathSel1.Stroke = Brushes.Blue;
            }
            if (rabType != "N")
            {
                BackUielementStatus();
            }
            InitNull();
        }

        /// 鼠标滚轮滚动事件
        /// <summary>
        /// 鼠标滚轮滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMain_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point scaleCenter = e.GetPosition((Canvas)sender);
            //double scale = 2;
            bool isExtend = false;
            if (e.Delta > 0)
            {
                scaleLevel *= oneScale;
                isExtend = true;
            }
            else
            {
                scaleLevel /= oneScale;
            }

            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            //totalScale.CenterX = CenterPoint.X;
            //totalScale.CenterY = CenterPoint.Y;
            //totalScale.CenterX = CenterPoint.X + tempTranslate.X;
            //totalScale.CenterY = CenterPoint.Y + tempTranslate.Y;
            //totalScale.CenterX = 0;
            //totalScale.CenterY = 0;
            //Point oldPoint = GetOldPointByNowPoint(scaleCenter);
            //totalScale.CenterX = oldPoint.X;
            //totalScale.CenterY = oldPoint.Y;

            totalScale.CenterX = scaleCenter.X;
            totalScale.CenterY = scaleCenter.Y;

            AdjustGraph();
            //Line line;
            //PointWindow pw;
            //Path path;
            //EllipseGeometry ellipseGeometry;
            foreach (UIElement ue in CanvasMain.Children)
            {
                if (ue.Uid.StartsWith("STATION_"))
                {
                    Canvas.SetLeft(ue, dicStation[ue.Uid].X * scaleLevel);
                    Canvas.SetTop(ue, dicStation[ue.Uid].Y * scaleLevel);
                }
                if (ue.Uid.StartsWith("IMAGE_"))
                {
                    Canvas.SetLeft(ue, dicImage[ue.Uid].X * scaleLevel);
                    Canvas.SetTop(ue, dicImage[ue.Uid].Y * scaleLevel);
                }
                if (ue.Uid.StartsWith("CALL_"))
                {
                    Canvas.SetLeft(ue, dicCall[ue.Uid].X * scaleLevel);
                    Canvas.SetTop(ue, dicCall[ue.Uid].Y * scaleLevel);
                }
                if (ue.Uid.StartsWith("STORE_"))
                {
                    Canvas.SetLeft(ue, dicStore[ue.Uid].X * scaleLevel);
                    Canvas.SetTop(ue, dicStore[ue.Uid].Y * scaleLevel);
                }
                if (ue.Uid.StartsWith("FONT_"))
                {
                    Canvas.SetLeft(ue, dicFont[ue.Uid].X * scaleLevel);
                    Canvas.SetTop(ue, dicFont[ue.Uid].Y * scaleLevel);
                }
                if (ue.Uid.StartsWith("CAR_"))
                {
                    Canvas.SetLeft(ue, dicCar[ue.Uid].X * scaleLevel);
                    Canvas.SetTop(ue, dicCar[ue.Uid].Y * scaleLevel);
                }
                ////直线坐标
                //if (ue.Uid.StartsWith("COORLINE"))
                //{
                //    line = ue as Line;
                //    if (line != null)
                //    {
                //        line.StrokeThickness = isExtend ? (line.StrokeThickness / oneScale) : (line.StrokeThickness * oneScale);
                //    }
                //}
                ////点阵坐标
                //if (ue.Uid.StartsWith("COORPOINT_"))
                //{
                //    //Canvas.SetLeft(ue, dicPoint[ue.Uid].X * scaleLevel);
                //    //Canvas.SetTop(ue, dicPoint[ue.Uid].Y * scaleLevel);
                //    //pw = ue as PointWindow;
                //    //if (pw != null)
                //    //{
                //    //    pw.Height = pw.Width = isExtend ? (pw.Width / oneScale) : (pw.Width * oneScale);
                //    //}
                //    path = ue as Path;
                //    if (path != null)
                //    {
                //        ellipseGeometry = path.Data as EllipseGeometry;
                //        if (ellipseGeometry != null)
                //        {
                //            ellipseGeometry.RadiusX = ellipseGeometry.RadiusY = isExtend ? (ellipseGeometry.RadiusX / oneScale) : (ellipseGeometry.RadiusX * oneScale);
                //        }
                //    }
                //}
            }

            Path pathTemp;
            //直线
            foreach (UIElement ue in dicLineUc.Values)
            {
                pathTemp = ue as Path;
                if (pathTemp != null)
                {
                    pathTemp.StrokeThickness = isExtend ? (pathTemp.StrokeThickness / oneScale) : (pathTemp.StrokeThickness * oneScale);
                }
            }

            //曲线
            foreach (UIElement ue in dicCurveUc.Values)
            {
                pathTemp = ue as Path;
                if (pathTemp != null)
                {
                    pathTemp.StrokeThickness = isExtend ? (pathTemp.StrokeThickness / oneScale) : (pathTemp.StrokeThickness * oneScale);
                }
            }

            //地标直线
            foreach (UIElement ue in dicStationLineUc.Values)
            {
                pathTemp = ue as Path;
                if (pathTemp != null)
                {
                    pathTemp.StrokeThickness = isExtend ? (pathTemp.StrokeThickness / oneScale) : (pathTemp.StrokeThickness * oneScale);
                }
            }

            //地标曲线
            foreach (UIElement ue in dicstationCurveUc.Values)
            {
                pathTemp = ue as Path;
                if (pathTemp != null)
                {
                    pathTemp.StrokeThickness = isExtend ? (pathTemp.StrokeThickness / oneScale) : (pathTemp.StrokeThickness * oneScale);
                }
            }

            line1.StrokeThickness = isExtend ? (line1.StrokeThickness / oneScale) : (line1.StrokeThickness * oneScale);
            line1.X1 = CenterPoint.X - centerLineWidth / scaleLevel;
            line1.X2 = CenterPoint.X + centerLineWidth / scaleLevel;
            line2.StrokeThickness = isExtend ? (line2.StrokeThickness / oneScale) : (line2.StrokeThickness * oneScale);
            line2.Y1 = CenterPoint.Y - centerLineWidth / scaleLevel;
            line2.Y2 = CenterPoint.Y + centerLineWidth / scaleLevel;
        }

        /// 画布鼠标移动事件
        /// <summary>
        /// 画布鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMain_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point c = e.GetPosition((Canvas)sender);//当前鼠标位置
            //Point test = GetOldPointByNowPoint(c);
            ////this.Title = c.X + "," + c.Y + "|" + test.X + "," + test.Y;
            //Point test1 = GetNowPointByOldPoint(test);
            //this.Title = c.X + "," + c.Y + "|" + test.X + "," + test.Y + "|" + test1.X + "," + test1.Y;

            if (rabType == "B" || rabType == "C" || rabType == "D" || rabType == "E" || rabType == "N")
            {
                if (stationCurrent != null)
                {
                    Point pointOld = GetOldPointByNowPoint(c);
                    string name = "STATION_" + stationCurrent.Uid;
                    if (pointOld.X > dicStation[name].X + 10 + ellipseRad || pointOld.X < dicStation[name].X + 10 - ellipseRad || pointOld.Y > dicStation[name].Y + 30 + ellipseRad || pointOld.Y < dicStation[name].Y + 30 - ellipseRad)
                    {
                        stationCurrent.Fill = Brushes.Brown;
                        stationCurrent.Stroke = Brushes.Brown;
                        stationCurrent = null;
                    }
                    else
                    {
                        stationCurrent.Fill = Brushes.Green;
                        stationCurrent.Stroke = Brushes.White;
                        if (rabType == "C")
                        {
                            if (curveP1!=null&&curveP2 == null)
                            {
                                station2 = dicStationUc[name] as StationWindow;
                            }
                        }
                        else
                        {
                            station2 = dicStationUc[name] as StationWindow;
                        }
                    }
                }
            }

            switch (rabType)
            {
                #region 地标直线
                case "B":
                    if (startLine != null)
                    {
                        LineGeometry lineGeometry = new LineGeometry();
                        Point pointStart = (Point)startLine;
                        lineGeometry.StartPoint = pointStart;
                        Point pointEnd = GetOldPointByNowPoint(c);
                        if (stationCurrent != null)
                        {
                            pointEnd.X = dicStation["STATION_" + stationCurrent.Uid].X + 10;
                            pointEnd.Y = dicStation["STATION_" + stationCurrent.Uid].Y + 30;
                        }
                        lineGeometry.EndPoint = pointEnd;

                        Path myPath = new Path();
                        myPath.Fill = penBrush;
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = lineGeometry;
                        if (lastPath != null)
                        {
                            CanvasMain.Children.Remove(lastPath);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastPath = myPath;

                        Polygon polygon = new Polygon();
                        polygon.Fill = penBrush;
                        polygon.Stroke = penBrush;
                        polygon.StrokeThickness = penSize / scaleLevel;

                        polygon.Points.Add(pointEnd);
                        List<Point> lst = GetSecondPoint((Point)startLine, pointEnd);

                        polygon.Points.Add(lst[0]);
                        polygon.Points.Add(lst[1]);
                        if (lastPolygon != null)
                        {
                            CanvasMain.Children.Remove(lastPolygon);
                        }
                        if (tfGroup != null)
                        {
                            polygon.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(polygon);
                        lastPolygon = polygon;
                    }

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (startLine != null)
                        {
                            if (stationCurrent == null)
                            {
                                if (lastPath != null)
                                {
                                    CanvasMain.Children.Remove(lastPath);
                                }
                                if (lastPolygon != null)
                                {
                                    CanvasMain.Children.Remove(lastPolygon);
                                }
                            }
                            else
                            {
                                if (firstStation != null && secondStation != null)
                                {
                                    if (firstStation.Uid == secondStation.Uid)
                                    {
                                        if (lastPath != null)
                                        {
                                            CanvasMain.Children.Remove(lastPath);
                                        }
                                        if (lastPolygon != null)
                                        {
                                            CanvasMain.Children.Remove(lastPolygon);
                                        }
                                    }
                                    else
                                    {
                                        lastPath.Uid = "STATIONLINE_" + stationLineCount;
                                        lastPolygon.Uid = "STATIONARROWLINE_" + stationLineCount++;
                                        lastPath.MouseDown += lastPath_MouseDown;
                                        lastPolygon.MouseDown += lastPath_MouseDown;

                                        if (station1 != null && station2 != null && !lstStation.Contains(station1.Uid + "@" + station2.Uid))
                                        {
                                            lstStation.Add(station1.Uid + "@" + station2.Uid);
                                        }
                                        station1 = station2 = null;
                                        dicStationLineUc[lastPath.Uid] = lastPath;
                                        dicStationLineArrowUc[lastPolygon.Uid] = lastPolygon;
                                    }
                                }
                            }
                            startLine = null;
                            lastPath = null;
                            lastPolygon = null;
                            firstStation = null;
                            secondStation = null;
                        }
                    }
                    break;
                #endregion

                #region 地标曲线
                case "C":
                    //直线
                    if (curveP1 != null && curveP2 == null && curveP3 == null)
                    {
                        LineGeometry lineGeometry = new LineGeometry();
                        lineGeometry.StartPoint = (Point)curveP1;

                        Point pointEnd = GetOldPointByNowPoint(c);
                        if (stationCurrent != null)
                        {
                            pointEnd.X = dicStation["STATION_" + stationCurrent.Uid].X + 10;
                            pointEnd.Y = dicStation["STATION_" + stationCurrent.Uid].Y + 30;
                        }
                        lineGeometry.EndPoint = pointEnd;

                        Path myPath = new Path();
                        myPath.Fill = penBrush;
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = lineGeometry;
                        if (lastCurve != null)
                        {
                            CanvasMain.Children.Remove(lastCurve);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastCurve = myPath;

                        Polygon polygon = new Polygon();
                        polygon.Fill = penBrush;
                        polygon.Stroke = penBrush;
                        polygon.StrokeThickness = penSize / scaleLevel;

                        polygon.Points.Add(pointEnd);
                        List<Point> lst = GetSecondPoint((Point)curveP1, pointEnd);

                        polygon.Points.Add(lst[0]);
                        polygon.Points.Add(lst[1]);
                        if (lastPolygon != null)
                        {
                            CanvasMain.Children.Remove(lastPolygon);
                        }
                        if (tfGroup != null)
                        {
                            polygon.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(polygon);
                        lastPolygon = polygon;
                    }
                    //二次方程式贝塞尔曲线
                    if (curveP1 != null && curveP2 != null && curveP3 == null)
                    {
                        QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment(GetOldPointByNowPoint(c), (Point)curveP2, true);
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = (Point)curveP1;
                        pathFigure.Segments.Add(quadraticBezierSegment);
                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);

                        Path myPath = new Path();
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = pathGeometry;

                        if (lastCurve != null)
                        {
                            CanvasMain.Children.Remove(lastCurve);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastCurve = myPath;

                        Polygon polygon = new Polygon();
                        polygon.Fill = penBrush;
                        polygon.Stroke = penBrush;
                        polygon.StrokeThickness = penSize / scaleLevel;

                        polygon.Points.Add((Point)curveP2);
                        List<Point> lst = GetSecondPoint(GetOldPointByNowPoint(c), (Point)curveP2);

                        polygon.Points.Add(lst[0]);
                        polygon.Points.Add(lst[1]);
                        if (lastPolygon != null)
                        {
                            CanvasMain.Children.Remove(lastPolygon);
                        }
                        if (tfGroup != null)
                        {
                            polygon.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(polygon);
                        lastPolygon = polygon;
                    }
                    //三次方程式贝塞尔曲线
                    if (curveP1 != null && curveP2 != null && curveP3 != null)
                    {
                        BezierSegment bezierSegment = new BezierSegment((Point)curveP3, GetOldPointByNowPoint(c), (Point)curveP2, true);
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = (Point)curveP1;
                        pathFigure.Segments.Add(bezierSegment);
                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);

                        Path myPath = new Path();
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = pathGeometry;

                        if (lastCurve != null)
                        {
                            CanvasMain.Children.Remove(lastCurve);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastCurve = myPath;

                        Polygon polygon = new Polygon();
                        polygon.Fill = penBrush;
                        polygon.Stroke = penBrush;
                        polygon.StrokeThickness = penSize / scaleLevel;

                        polygon.Points.Add((Point)curveP2);
                        List<Point> lst = GetSecondPoint(GetOldPointByNowPoint(c), (Point)curveP2);

                        polygon.Points.Add(lst[0]);
                        polygon.Points.Add(lst[1]);
                        if (lastPolygon != null)
                        {
                            CanvasMain.Children.Remove(lastPolygon);
                        }
                        if (tfGroup != null)
                        {
                            polygon.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(polygon);
                        lastPolygon = polygon;
                    }

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (curveP1 != null)
                        {
                            if (curveP2 == null && stationCurrent != null)
                            {
                                if (((Point)curveP1).X != dicStation["STATION_" + stationCurrent.Uid].X + 10 &&
                                    ((Point)curveP1).Y != dicStation["STATION_" + stationCurrent.Uid].X + 30)
                                {
                                    curveP2 = new Point(dicStation["STATION_" + stationCurrent.Uid].X + 10,
                                        dicStation["STATION_" + stationCurrent.Uid].Y + 30);
                                    curveP3 = null;
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region 直线
                case "D":
                    if (lineStart != null)
                    {
                        LineGeometry lineGeometry = new LineGeometry();
                        lineGeometry.StartPoint = (Point)lineStart;
                        Point pointEnd = GetOldPointByNowPoint(c);
                        if (stationCurrent != null)
                        {
                            pointEnd.X = dicStation["STATION_" + stationCurrent.Uid].X + 10;
                            pointEnd.Y = dicStation["STATION_" + stationCurrent.Uid].Y + 30;
                        }
                        lineGeometry.EndPoint = pointEnd;

                        Path myPath = new Path();
                        myPath.Fill = penBrush;
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = lineGeometry;
                        if (lastLine != null)
                        {
                            CanvasMain.Children.Remove(lastLine);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastLine = myPath;
                    }

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (lastLine != null)
                        {
                            LineGeometry lineGeometry = lastLine.Data as LineGeometry;
                            if (lineGeometry != null)
                            {
                                if (lineGeometry.StartPoint == lineGeometry.EndPoint)
                                {
                                    if (lastLine != null)
                                    {
                                        CanvasMain.Children.Remove(lastLine);
                                    }
                                }
                                else
                                {
                                    lastLine.Uid = "LINENEW_" + lineCount++;
                                    lastLine.MouseLeftButtonDown += lastLine_MouseLeftButtonDown;
                                    dicLineUc[lastLine.Uid] = lastLine;
                                }
                                lineStart = null;
                                lastLine = null;
                            }
                        }
                    }
                    break;
                #endregion

                #region 曲线
                case "E":
                    //直线
                    if (curveP1 != null && curveP2 == null && curveP3 == null)
                    {
                        LineGeometry lineGeometry = new LineGeometry();
                        lineGeometry.StartPoint = (Point)curveP1;

                        Point pointEnd = GetOldPointByNowPoint(c);
                        if (stationCurrent != null)
                        {
                            pointEnd.X = dicStation["STATION_" + stationCurrent.Uid].X + 10;
                            pointEnd.Y = dicStation["STATION_" + stationCurrent.Uid].Y + 30;
                        }
                        lineGeometry.EndPoint = pointEnd;

                        Path myPath = new Path();
                        myPath.Fill = penBrush;
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = lineGeometry;
                        if (lastCurve != null)
                        {
                            CanvasMain.Children.Remove(lastCurve);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastCurve = myPath;
                    }
                    //二次方程式贝塞尔曲线
                    if (curveP1 != null && curveP2 != null && curveP3 == null)
                    {
                        QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment(GetOldPointByNowPoint(c), (Point)curveP2, true);
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = (Point)curveP1;
                        pathFigure.Segments.Add(quadraticBezierSegment);
                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);

                        Path myPath = new Path();
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = pathGeometry;

                        if (lastCurve != null)
                        {
                            CanvasMain.Children.Remove(lastCurve);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastCurve = myPath;
                    }
                    //三次方程式贝塞尔曲线
                    if (curveP1 != null && curveP2 != null && curveP3 != null)
                    {
                        BezierSegment bezierSegment = new BezierSegment((Point)curveP3, GetOldPointByNowPoint(c), (Point)curveP2, true);
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = (Point)curveP1;
                        pathFigure.Segments.Add(bezierSegment);
                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);

                        Path myPath = new Path();
                        myPath.Stroke = penBrush;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = pathGeometry;

                        if (lastCurve != null)
                        {
                            CanvasMain.Children.Remove(lastCurve);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastCurve = myPath;
                    }

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (curveP1 != null)
                        {
                            if (curveP2 == null && stationCurrent == null)
                            {
                                curveP2 = GetOldPointByNowPoint(c);
                                curveP3 = null;
                            }
                        }
                    }
                    break;
                #endregion

                #region 抓手
                case "M":
                    if (isMoving && (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed))
                    {
                        Point currentMousePosition = e.GetPosition((Canvas)sender); //当前鼠标位置

                        Point deltaPt = new Point(0, 0);
                        deltaPt.X = (currentMousePosition.X - startMovePosition.X) / scaleLevel;
                        deltaPt.Y = (currentMousePosition.Y - startMovePosition.Y) / scaleLevel;

                        tempTranslate.X = totalTranslate.X + deltaPt.X;
                        tempTranslate.Y = totalTranslate.Y + deltaPt.Y;

                        AdjustGraph();
                    }
                    break;
                #endregion

                #region 移动
                case "N":
                    if (lineStart != null && selectUiElement != null)
                    {
                        LineGeometry lineGeometry = new LineGeometry();
                        lineGeometry.StartPoint = (Point)lineStart;
                        Point pointEnd = GetOldPointByNowPoint(c);
                        if (stationCurrent != null)
                        {
                            pointEnd.X = dicStation["STATION_" + stationCurrent.Uid].X + 10;
                            pointEnd.Y = dicStation["STATION_" + stationCurrent.Uid].Y + 30;
                        }
                        lineGeometry.EndPoint = pointEnd;

                        Path myPath = new Path();
                        myPath.Fill = Brushes.Blue;
                        myPath.Stroke = Brushes.Blue;
                        myPath.StrokeThickness = penSize / scaleLevel;
                        myPath.Data = lineGeometry;
                        if (lastLine != null)
                        {
                            CanvasMain.Children.Remove(lastLine);
                        }
                        if (tfGroup != null)
                        {
                            myPath.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(myPath);
                        lastLine = myPath;

                        //移动元素(地标，图片，呼叫器，文字，储位)
                        if (selectType == "A" || selectType == "F" || selectType == "G" || selectType == "H" ||
                            selectType == "I")
                        {
                            Canvas.SetLeft(selectUiElement,
                                left + (lineGeometry.EndPoint.X - lineGeometry.StartPoint.X) * scaleLevel);
                            Canvas.SetTop(selectUiElement, top + (lineGeometry.EndPoint.Y - lineGeometry.StartPoint.Y) * scaleLevel);
                        }
                        //地标直线，地标曲线，直线，曲线
                        else
                        {
                            double distinctX = lineGeometry.EndPoint.X - lineGeometry.StartPoint.X;
                            double distinctY = lineGeometry.EndPoint.Y - lineGeometry.StartPoint.Y;
                            LineGeometry lineGeometryTemp;
                            Polygon polygonTemp;
                            PathFigure pathFigureTemp;
                            BezierSegment bezierSegmentTemp;
                            switch (selectType)
                            {
                                case "B":
                                    lineGeometryTemp = (LineGeometry)(((Path)selectUiElement).Data);
                                    lineGeometryTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X + distinctX, dicOldPoint["StartPoint"].Y + distinctY);
                                    lineGeometryTemp.EndPoint = new Point(dicOldPoint["EndPoint"].X + distinctX, dicOldPoint["EndPoint"].Y + distinctY);

                                    polygonTemp = (Polygon)selectUiElement2;
                                    polygonTemp.Points[0] = new Point(dicOldPoint["Point1"].X + distinctX, dicOldPoint["Point1"].Y + distinctY);
                                    polygonTemp.Points[1] = new Point(dicOldPoint["Point2"].X + distinctX, dicOldPoint["Point2"].Y + distinctY);
                                    polygonTemp.Points[2] = new Point(dicOldPoint["Point3"].X + distinctX, dicOldPoint["Point3"].Y + distinctY);
                                    break;

                                case "C":
                                    pathFigureTemp = ((PathGeometry)(((Path)selectUiElement).Data)).Figures[0];
                                    pathFigureTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X + distinctX, dicOldPoint["StartPoint"].Y + distinctY);
                                    bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                                    bezierSegmentTemp.Point1 = new Point(dicOldPoint["Point1"].X + distinctX, dicOldPoint["Point1"].Y + distinctY);
                                    bezierSegmentTemp.Point2 = new Point(dicOldPoint["Point2"].X + distinctX, dicOldPoint["Point2"].Y + distinctY);
                                    bezierSegmentTemp.Point3 = new Point(dicOldPoint["Point3"].X + distinctX, dicOldPoint["Point3"].Y + distinctY);

                                    polygonTemp = (Polygon)selectUiElement2;
                                    polygonTemp.Points[0] = new Point(dicOldPoint["Point11"].X + distinctX, dicOldPoint["Point11"].Y + distinctY);
                                    polygonTemp.Points[1] = new Point(dicOldPoint["Point21"].X + distinctX, dicOldPoint["Point21"].Y + distinctY);
                                    polygonTemp.Points[2] = new Point(dicOldPoint["Point31"].X + distinctX, dicOldPoint["Point31"].Y + distinctY);
                                    break;

                                case "D":
                                    lineGeometryTemp = (LineGeometry)(((Path)selectUiElement).Data);
                                    lineGeometryTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X + distinctX, dicOldPoint["StartPoint"].Y + distinctY);
                                    lineGeometryTemp.EndPoint = new Point(dicOldPoint["EndPoint"].X + distinctX, dicOldPoint["EndPoint"].Y + distinctY);
                                    break;

                                case "E":
                                    pathFigureTemp = ((PathGeometry)(((Path)selectUiElement).Data)).Figures[0];
                                    pathFigureTemp.StartPoint = new Point(dicOldPoint["StartPoint"].X + distinctX, dicOldPoint["StartPoint"].Y + distinctY);
                                    bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                                    bezierSegmentTemp.Point1 = new Point(dicOldPoint["Point1"].X + distinctX, dicOldPoint["Point1"].Y + distinctY);
                                    bezierSegmentTemp.Point2 = new Point(dicOldPoint["Point2"].X + distinctX, dicOldPoint["Point2"].Y + distinctY);
                                    bezierSegmentTemp.Point3 = new Point(dicOldPoint["Point3"].X + distinctX, dicOldPoint["Point3"].Y + distinctY);
                                    break;
                            }
                        }
                    }

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (lastLine != null)
                        {
                            CanvasMain.Children.Remove(lastLine);
                            if (selectType == "A" || selectType == "F" || selectType == "G" || selectType == "H" ||
                                selectType == "I")
                            {
                                Dictionary<string, Point> dic = GetDicPoint(selectType);
                                if (dic != null)
                                {
                                    dic[selectUiElement.Uid] = new Point(Canvas.GetLeft(selectUiElement) / scaleLevel,
                                        Canvas.GetTop(selectUiElement) / scaleLevel);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(selectType))
                                {
                                    LineGeometry lineGeometryTemp = (LineGeometry)lastLine.Data;
                                    double distinctX = lineGeometryTemp.EndPoint.X - lineGeometryTemp.StartPoint.X;
                                    double distinctY = lineGeometryTemp.EndPoint.Y - lineGeometryTemp.StartPoint.Y;

                                    List<string> lstKey = new List<string>();
                                    lstKey.AddRange(dicOldPoint.Keys);
                                    foreach (string key in lstKey)
                                    {
                                        dicOldPoint[key] = new Point(dicOldPoint[key].X + distinctX, dicOldPoint[key].Y + distinctY);
                                    }
                                }
                            }
                            lineStart = null;
                            lastLine = null;
                        }
                    }

                    break;
                #endregion

                default:
                    if (isMoving && e.RightButton == MouseButtonState.Pressed)
                    {
                        Point currentMousePosition = e.GetPosition((Canvas)sender); //当前鼠标位置

                        Point deltaPt = new Point(0, 0);
                        deltaPt.X = (currentMousePosition.X - startMovePosition.X) / scaleLevel;
                        deltaPt.Y = (currentMousePosition.Y - startMovePosition.Y) / scaleLevel;

                        tempTranslate.X = totalTranslate.X + deltaPt.X;
                        tempTranslate.Y = totalTranslate.Y + deltaPt.Y;

                        AdjustGraph();
                    }
                    break;
            }
            e.Handled = true;
        }

        /// CanvasMain鼠标左键按下事件
        /// <summary>
        /// CanvasMain鼠标左键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMain_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startMovePosition = e.GetPosition((Canvas)sender);
            isMoving = true;

            //未选择控件时
            if (rabType == "L" && !isOneClick)
            {
                if (itemSource.Count > 0)
                {
                    //dataGrid.CommitEdit();
                    ChoiceDataChange();
                    itemSource.Clear();
                    dataGrid.ItemsSource = itemSource;
                }
            }
            switch (rabType)
            {
                #region 地标直线
                case "B":
                    if (startLine != null && secondStation != null)
                    {
                        if (lastPath != null)
                        {
                            lastPath.Uid = "STATIONLINE_" + stationLineCount;
                            lastPolygon.Uid = "STATIONARROWLINE_" + stationLineCount++;
                            lastPath.MouseDown += lastPath_MouseDown;
                            lastPolygon.MouseDown += lastPath_MouseDown;
                            if (station1 != null && station2 != null && !lstStation.Contains(station1.Uid + "@" + station2.Uid))
                            {
                                lstStation.Add(station1.Uid + "@" + station2.Uid);
                            }
                            station1 = station2 = null;
                            dicStationLineUc[lastPath.Uid] = lastPath;
                            dicStationLineArrowUc[lastPolygon.Uid] = lastPolygon;
                        }

                        startLine = null;
                        lastPath = null;
                        lastPolygon = null;
                        firstStation = null;
                        secondStation = null;
                    }
                    break;
                #endregion

                #region 地标曲线
                case "C":
                    if (curveP1 == null && stationCurrent != null)
                    {
                        curveP1 = new Point(dicStation["STATION_" + stationCurrent.Uid].X + 10, dicStation["STATION_" + stationCurrent.Uid].Y + 30);
                        curveP2 = curveP3 = null;
                    }
                    else
                    {
                        if (curveP3 != null)
                        {
                            if (lastCurve != null)
                            {
                                lastCurve.Uid = "STATIONCURVE_" + stationCurveCount;
                                lastPolygon.Uid = "STATIONCURVEARROW_" + stationCurveCount++;
                                lastCurve.MouseDown += lastCurve_MouseDown;
                                lastPolygon.MouseDown += lastCurve_MouseDown;
                                if (station1 != null && station2 != null && !lstStation.Contains(station1.Uid + "@" + station2.Uid))
                                {
                                    lstStation.Add(station1.Uid + "@" + station2.Uid);
                                }
                                station1 = station2 = null;
                                dicstationCurveUc[lastCurve.Uid] = lastCurve;
                                dicstationCurveArrowUc[lastPolygon.Uid] = lastPolygon;
                                lastCurve = null;
                                lastPolygon = null;
                                curveP1 = curveP2 = curveP3 = null;
                            }
                        }
                        else
                        {
                            if (curveP1 != null)
                            {
                                if (curveP2 == null && stationCurrent != null && !isOneClick)
                                {
                                    if (((Point)curveP1).X != dicStation["STATION_" + stationCurrent.Uid].X + 10 &&
                                        ((Point)curveP1).Y != dicStation["STATION_" + stationCurrent.Uid].X + 30)
                                    {
                                        curveP2 = new Point(dicStation["STATION_" + stationCurrent.Uid].X + 10,
                                            dicStation["STATION_" + stationCurrent.Uid].Y + 30);
                                        curveP3 = null;
                                    }
                                }
                                else if (curveP2 != null && curveP3 == null && !isOneClick)
                                {
                                    curveP3 = GetOldPointByNowPoint(startMovePosition);
                                }
                            }
                        }
                    }
                    //isOneClick = false;
                    break;
                #endregion

                #region 直线
                case "D":
                    if (lineStart != null)
                    {
                        if (lastLine != null)
                        {
                            lastLine.Uid = "LINENEW_" + lineCount++;
                            lastLine.MouseLeftButtonDown += lastLine_MouseLeftButtonDown;
                            dicLineUc[lastLine.Uid] = lastLine;
                            lastLine = null;
                            lineStart = null;
                        }
                    }
                    else
                    {
                        if (lineStart == null && stationCurrent == null)
                        {
                            lineStart = GetOldPointByNowPoint(startMovePosition);
                        }
                    }
                    break;
                #endregion

                #region 曲线
                case "E":
                    if (curveP1 == null && stationCurrent == null)
                    {
                        curveP1 = GetOldPointByNowPoint(startMovePosition);
                        curveP2 = curveP3 = null;
                    }
                    else
                    {
                        if (curveP3 != null)
                        {
                            if (lastCurve != null)
                            {
                                lastCurve.Uid = "CURVENEW_" + curveCount++;
                                lastCurve.MouseDown += lastLine_MouseLeftButtonDown;
                                dicCurveUc[lastCurve.Uid] = lastCurve;

                                lastCurve = null;
                                curveP1 = curveP2 = curveP3 = null;
                            }
                        }
                        else
                        {
                            if (curveP1 != null)
                            {
                                if (curveP2 == null && stationCurrent == null && !isOneClick)
                                {
                                    curveP2 = GetOldPointByNowPoint(startMovePosition);
                                    curveP3 = null;
                                }
                                else if (curveP3 == null && !isOneClick)
                                {
                                    curveP3 = GetOldPointByNowPoint(startMovePosition);
                                }
                            }
                        }
                    }
                    //isOneClick = false;
                    break;
                #endregion

                #region 移动
                case "N":
                    if (lineStart != null)
                    {
                        if (lastLine != null)
                        {
                            CanvasMain.Children.Remove(lastLine);
                            if (selectType == "A" || selectType == "F" || selectType == "G" || selectType == "H" ||
                                selectType == "I")
                            {
                                Dictionary<string, Point> dic = GetDicPoint(selectType);
                                if (dic != null)
                                {
                                    dic[selectUiElement.Uid] = new Point(Canvas.GetLeft(selectUiElement) / scaleLevel,
                                        Canvas.GetTop(selectUiElement) / scaleLevel);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(selectType))
                                {
                                    LineGeometry lineGeometryTemp = (LineGeometry)lastLine.Data;
                                    double distinctX = lineGeometryTemp.EndPoint.X - lineGeometryTemp.StartPoint.X;
                                    double distinctY = lineGeometryTemp.EndPoint.Y - lineGeometryTemp.StartPoint.Y;

                                    foreach (string key in dicOldPoint.Keys)
                                    {
                                        dicOldPoint[key] = new Point(dicOldPoint[key].X + distinctX, dicOldPoint[key].Y + distinctY);
                                    }
                                }
                            }
                            lastLine = null;
                            lineStart = null;
                        }
                    }
                    else
                    {
                        if (lineStart == null && stationCurrent == null)
                        {
                            lineStart = GetOldPointByNowPoint(startMovePosition);
                            if (selectType == "A" || selectType == "F" || selectType == "G" || selectType == "H" ||
                                    selectType == "I")
                            {
                                if (selectUiElement != null)
                                {
                                    left = Canvas.GetLeft(selectUiElement);
                                    top = Canvas.GetTop(selectUiElement);
                                }
                            }
                        }
                    }
                    break;
                #endregion
            }

            isOneClick = false;
        }

        /// CanvasMain鼠标左键抬起事件
        /// <summary>
        /// CanvasMain鼠标左键抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMain_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endMovePosition = e.GetPosition((Canvas)sender);
            switch (rabType)
            {
                case "A":
                    GeneralStation(endMovePosition,null,null,null);
                    break;
                case "F":
                    GeneralStore(endMovePosition,null,null,null,null);
                    break;
                case "G":
                    GeneralFont(endMovePosition,null,penBrush,null);
                    break;
                case "H":
                    GeneralImage(endMovePosition,null,null,null);
                    break;
                case "I":
                    GeneralCall(endMovePosition,null,null,null);
                    break;
                case "M":
                    if (isMoving)
                    {
                        isMoving = false;
                        //为了避免跳跃式的变换，单次有效变化 累加入 totalTranslate中。           
                        totalTranslate.X += (endMovePosition.X - startMovePosition.X) / scaleLevel;
                        totalTranslate.Y += (endMovePosition.Y - startMovePosition.Y) / scaleLevel;
                    }
                    break;
            }

            e.Handled = true;
        }

        /// 鼠标右键抬起事件
        /// <summary>
        /// 鼠标右键抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CanvasMain_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endMovePosition = e.GetPosition((Canvas)sender);
            if (isMoving)
            {
                isMoving = false;
                //为了避免跳跃式的变换，单次有效变化 累加入 totalTranslate中。           
                totalTranslate.X += (endMovePosition.X - startMovePosition.X) / scaleLevel;
                totalTranslate.Y += (endMovePosition.Y - startMovePosition.Y) / scaleLevel;
            }
        }

        /// 鼠标右键按下事件
        /// <summary>
        /// 鼠标右键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CanvasMain_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            startMovePosition = e.GetPosition((Canvas)sender);
            isMoving = true;
        }

        /// CanvasMain鼠标进入事件
        /// <summary>
        /// CanvasMain鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMain_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (rabType == "M")
            {
                Cursor = Cursors.Hand;
            }
            else if (rabType == "L")
            {
                Cursor = Cursors.Arrow;
            }
            else if (rabType == "N")
            {
                if (selectUiElement != null)
                {
                    Cursor = Cursors.SizeAll;
                }
            }
            else
            {
                Cursor = Cursors.Cross;
            }
        }

        /// CanvasMain鼠标离开事件
        /// <summary>
        /// CanvasMain鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMain_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        /// 直/曲线单击事件
        /// <summary>
        /// 直/曲线单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lastLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Path pathSelect = (Path)sender;
            switch (rabType)
            {
                #region 选择
                case "L":
                    if (!isStart)
                    {
                        gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                        BackUielementStatus();
                        brushPre = pathSelect.Stroke;
                        pathSelect.Stroke = selectBrush;

                        selectUiElement = pathSelect;
                        selectUiElement2 = null;

                        dicOldPoint.Clear();
                        if (pathSelect.Uid.StartsWith("LINENEW_"))
                        {
                            selectType = "D";
                            LineGeometry lineGeometryTemp = (LineGeometry)(pathSelect.Data);
                            dicOldPoint["StartPoint"] = lineGeometryTemp.StartPoint;
                            dicOldPoint["EndPoint"] = lineGeometryTemp.EndPoint;
                        }
                        else
                        {
                            selectType = "E";
                            PathFigure pathFigureTemp = ((PathGeometry)(pathSelect.Data)).Figures[0];
                            dicOldPoint["StartPoint"] = pathFigureTemp.StartPoint;
                            BezierSegment bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                            dicOldPoint["Point1"] = bezierSegmentTemp.Point1;
                            dicOldPoint["Point2"] = bezierSegmentTemp.Point2;
                            dicOldPoint["Point3"] = bezierSegmentTemp.Point3;
                        }

                        GetItemSource(selectType, pathSelect.Uid);
                        dataGrid.ItemsSource = itemSource;
                        isOneClick = true;
                    }
                    break;
                #endregion

                #region 使两直线相交
                case "J":
                    if (pathSel1 == null)
                    {
                        pathSel1 = pathSelect;
                        brushOld = pathSel1.Stroke;
                        pathSel1.Stroke = Brushes.Red;

                        pointClick1 = GetOldPointByNowPoint(Mouse.GetPosition(CanvasMain));
                    }
                    else
                    {
                        pointClick2 = GetOldPointByNowPoint(Mouse.GetPosition(CanvasMain));
                        //处理相交
                        LineGeometry lineGeometry1 = (LineGeometry)pathSel1.Data;
                        LineGeometry lineGeometry2 = (LineGeometry)pathSelect.Data;

                        Point p1Start = lineGeometry1.StartPoint;
                        Point p1End = lineGeometry1.EndPoint;

                        Point p2Start = lineGeometry2.StartPoint;
                        Point p2End = lineGeometry2.EndPoint;

                        decimal x1 = (decimal)p1Start.X;
                        decimal x2 = (decimal)p1End.X;
                        decimal x3 = (decimal)p2Start.X;
                        decimal x4 = (decimal)p2End.X;

                        decimal y1 = (decimal)p1Start.Y;
                        decimal y2 = (decimal)p1End.Y;
                        decimal y3 = (decimal)p2Start.Y;
                        decimal y4 = (decimal)p2End.Y;

                        decimal y, x;

                        //未考虑除数为0情况
                        x = (x1 * (y2 - y1) * (x4 - x3) + (y3 - y1) * (x4 - x3) * (x2 - x1) - x3 * (y4 - y3) * (x2 - x1)) / ((y2 - y1) * (x4 - x3) - (y4 - y3) * (x2 - x1));

                        y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;

                        //交点坐标
                        Point pointSame = new Point((double)x, (double)y);

                        double[] arrX = new[] { p1Start.X, pointSame.X, p1End.X };
                        Array.Sort(arrX);
                        //交点在线外
                        if (arrX[0] == pointSame.X || arrX[2] == pointSame.X)
                        {
                            if (arrX[0] == p1Start.X || arrX[2] == p1Start.X)
                            {
                                lineGeometry1.StartPoint = p1Start;
                                lineGeometry1.EndPoint = pointSame;
                            }
                            else
                            {
                                lineGeometry1.StartPoint = p1End;
                                lineGeometry1.EndPoint = pointSame;
                            }
                        }
                        //交点在线上
                        else
                        {
                            lineGeometry1.StartPoint = pointSame;
                            if (pointClick1.X >= pointSame.X)
                            {
                                if (pointClick1.X <= p1Start.X)
                                {
                                    lineGeometry1.EndPoint = p1Start;
                                }
                                else
                                {
                                    lineGeometry1.EndPoint = p1End;
                                }
                            }
                            else
                            {
                                if (pointClick1.X >= p1Start.X)
                                {
                                    lineGeometry1.EndPoint = p1Start;
                                }
                                else
                                {
                                    lineGeometry1.EndPoint = p1End;
                                }
                            }
                        }

                        arrX = new[] { p2Start.X, pointSame.X, p2End.X };
                        Array.Sort(arrX);
                        //交点在线外
                        if (arrX[0] == pointSame.X || arrX[2] == pointSame.X)
                        {
                            if (arrX[0] == p2Start.X || arrX[2] == p2Start.X)
                            {
                                lineGeometry2.StartPoint = p2Start;
                                lineGeometry2.EndPoint = pointSame;
                            }
                            else
                            {
                                lineGeometry2.StartPoint = p2End;
                                lineGeometry2.EndPoint = pointSame;
                            }
                        }
                        //交点在线上
                        else
                        {
                            lineGeometry2.StartPoint = pointSame;
                            if (pointClick2.X >= pointSame.X)
                            {
                                if (pointClick2.X <= p2Start.X)
                                {
                                    lineGeometry2.EndPoint = p2Start;
                                }
                                else
                                {
                                    lineGeometry2.EndPoint = p2End;
                                }
                            }
                            else
                            {
                                if (pointClick2.X >= p2Start.X)
                                {
                                    lineGeometry2.EndPoint = p2Start;
                                }
                                else
                                {
                                    lineGeometry2.EndPoint = p2End;
                                }
                            }
                        }

                        pathSel1.Stroke = brushOld;
                        pathSel1 = null;
                    }
                    break;
                #endregion

                #region 打断直线
                case "K":
                    if (pathSel1 == null)
                    {
                        pathSel1 = pathSelect;
                        brushOld = pathSel1.Stroke;
                        pathSel1.Stroke = Brushes.Red;

                        pointClick1 = GetOldPointByNowPoint(Mouse.GetPosition(CanvasMain));
                    }
                    else
                    {
                        pointClick2 = GetOldPointByNowPoint(Mouse.GetPosition(CanvasMain));
                        //处理相交
                        LineGeometry lineGeometry1 = (LineGeometry)pathSel1.Data;
                        LineGeometry lineGeometry2 = (LineGeometry)pathSelect.Data;

                        Point p1Start = lineGeometry1.StartPoint;
                        Point p1End = lineGeometry1.EndPoint;

                        Point p2Start = lineGeometry2.StartPoint;
                        Point p2End = lineGeometry2.EndPoint;

                        decimal x1 = (decimal)p1Start.X;
                        decimal x2 = (decimal)p1End.X;
                        decimal x3 = (decimal)p2Start.X;
                        decimal x4 = (decimal)p2End.X;

                        decimal y1 = (decimal)p1Start.Y;
                        decimal y2 = (decimal)p1End.Y;
                        decimal y3 = (decimal)p2Start.Y;
                        decimal y4 = (decimal)p2End.Y;

                        decimal y, x;

                        //未考虑除数为0情况
                        x = (x1 * (y2 - y1) * (x4 - x3) + (y3 - y1) * (x4 - x3) * (x2 - x1) - x3 * (y4 - y3) * (x2 - x1)) / ((y2 - y1) * (x4 - x3) - (y4 - y3) * (x2 - x1));

                        y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;

                        //交点坐标
                        Point pointSame = new Point((double)x, (double)y);

                        //两直线相交才允许打断直线
                        if (pointSame.X <= Math.Max(p1Start.X, p1End.X) && pointSame.X >= Math.Min(p1Start.X, p1End.X) &&
                            pointSame.X <= Math.Max(p2Start.X, p2End.X) && pointSame.X >= Math.Min(p2Start.X, p2End.X))
                        {
                            lineGeometry1.StartPoint = pointSame;
                            if (pointClick1.X >= pointSame.X)
                            {
                                if (pointClick1.X <= p1Start.X)
                                {
                                    lineGeometry1.EndPoint = p1Start;
                                }
                                else
                                {
                                    lineGeometry1.EndPoint = p1End;
                                }
                            }
                            else
                            {
                                if (pointClick1.X >= p1Start.X)
                                {
                                    lineGeometry1.EndPoint = p1Start;
                                }
                                else
                                {
                                    lineGeometry1.EndPoint = p1End;
                                }
                            }
                        }
                        pathSel1.Stroke = brushOld;
                        pathSel1 = null;
                    }
                    break;
                #endregion
            }
        }

        /// 地标鼠标按下事件
        /// <summary>
        /// 地标鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void stationWindow_mouseDown(object sender, EventArgs e)
        {
            string num = sender.ToString();
            switch (rabType)
            {
                case "B":
                    if (startLine == null)
                    {
                        startLine = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                        station1 = dicStationUc["STATION_" + num] as StationWindow;
                    }
                    break;
                case "C":
                    if (curveP1 == null)
                    {
                        curveP1 = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                        station1 = dicStationUc["STATION_" + num] as StationWindow;
                        curveP2 = curveP3 = null;
                    }
                    else if (curveP2 == null)
                    {
                        if (((Point)curveP1).X != dicStation["STATION_" + num].X + 10 &&
                            ((Point)curveP1).Y != dicStation["STATION_" + num].X + 30)
                        {
                            curveP2 = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                            curveP3 = null;
                        }
                    }
                    isOneClick = true;
                    break;
                case "D":
                    if (lineStart == null)
                    {
                        lineStart = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                    }
                    break;
                case "E":
                    if (curveP1 == null)
                    {
                        curveP1 = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                        curveP2 = curveP3 = null;
                    }
                    else if (curveP2 == null)
                    {
                        if (((Point)curveP1).X != dicStation["STATION_" + num].X + 10 &&
                            ((Point)curveP1).Y != dicStation["STATION_" + num].X + 30)
                        {
                            curveP2 = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                            curveP3 = null;
                        }
                    }
                    isOneClick = true;
                    break;
                case "L":
                    if (!isStart)
                    {
                        gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                        BackUielementStatus();
                        StationWindow station = (StationWindow)dicStationUc["STATION_" + num];
                        station.BackgroundValue = selectBrush;
                        selectType = "A";
                        selectUiElement = station;
                        selectUiElement2 = null;

                        GetItemSource(selectType, station.Uid);
                        dataGrid.ItemsSource = itemSource;
                        isOneClick = true;
                    }
                    break;
                case "N":
                    if (lineStart == null)
                    {
                        lineStart = new Point(dicStation["STATION_" + num].X + 10, dicStation["STATION_" + num].Y + 30);
                        if (selectUiElement != null)
                        {
                            left = Canvas.GetLeft(selectUiElement);
                            top = Canvas.GetTop(selectUiElement);
                        }
                    }
                    break;
            }
        }

        /// 地标鼠标进入事件
        /// <summary>
        /// 地标鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void stationWindow_mouseEnter(object sender, EventArgs e)
        {
            stationCurrent = sender as Rectangle;
            switch (rabType)
            {
                case "B":
                    //System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new MouseEnterAndLeaveFunc(MouseEnter), path);
                    if (stationCurrent != null)
                    {
                        if (firstStation == null)
                        {
                            firstStation = stationCurrent;
                        }
                        else
                        {
                            secondStation = stationCurrent;
                        }
                    }
                    break;
            }
        }

        /// 呼叫器单击事件
        /// <summary>
        /// 呼叫器单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void callWindow_mouseDown(object sender, EventArgs e)
        {
            if (rabType == "L" && !isStart)
            {
                gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                BackUielementStatus();
                CallWindow callWindow = (CallWindow)sender;
                callWindow.BackgroundValue = selectBrush;
                selectType = "I";
                selectUiElement = callWindow;
                selectUiElement2 = null;

                GetItemSource(selectType, callWindow.Uid);
                dataGrid.ItemsSource = itemSource;
                isOneClick = true;
            }
        }

        /// 图片单击事件
        /// <summary>
        /// 图片单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageWindow_mouseDown(object sender, EventArgs e)
        {
            if (rabType == "L" && !isStart)
            {
                gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                BackUielementStatus();
                ImageWindow window = (ImageWindow)sender;
                window.BackgroundValue = selectBrush;
                selectType = "H";
                selectUiElement = window;
                selectUiElement2 = null;

                GetItemSource(selectType, window.Uid);
                dataGrid.ItemsSource = itemSource;
                isOneClick = true;
            }
        }

        /// 储位单击事件
        /// <summary>
        /// 储位单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void storeWindow_mouseDown(object sender, EventArgs e)
        {
            if (rabType == "L" && !isStart)
            {
                BackUielementStatus();
                StoreWindow window = (StoreWindow)sender;
                window.BackgroundValue = selectBrush;
                selectType = "F";
                selectUiElement = window;
                selectUiElement2 = null;

                GetItemSource(selectType, window.Uid);
                dataGrid.ItemsSource = itemSource;

                gridRow1.MaxHeight = 26;
                gridRow2.MaxHeight = 0;
                isOneClick = true;
            }
        }

        /// 地标直线单击事件
        /// <summary>
        /// 地标直线单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lastPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rabType == "L" && !isStart)
            {
                gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                BackUielementStatus();

                Path path = sender as Path;
                Polygon polygon;
                dicOldPoint.Clear();
                isOneClick = true;
                if (path != null)
                {
                    brushPre = path.Stroke;
                    path.Stroke = selectBrush;
                    polygon = dicStationLineArrowUc[path.Uid.Replace("STATIONLINE_", "STATIONARROWLINE_")];
                    polygon.Stroke = polygon.Fill = selectBrush;
                    selectType = "B";
                    selectUiElement = path;
                    selectUiElement2 = polygon;

                    LineGeometry lineGeometryTemp = (LineGeometry)(path.Data);
                    dicOldPoint["StartPoint"] = lineGeometryTemp.StartPoint;
                    dicOldPoint["EndPoint"] = lineGeometryTemp.EndPoint;

                    dicOldPoint["Point1"] = polygon.Points[0];
                    dicOldPoint["Point2"] = polygon.Points[1];
                    dicOldPoint["Point3"] = polygon.Points[2];

                    GetItemSource(selectType, path.Uid);
                    dataGrid.ItemsSource = itemSource;
                    return;
                }
                polygon = sender as Polygon;
                if (polygon != null)
                {
                    brushPre = polygon.Stroke;
                    polygon.Stroke = polygon.Fill = selectBrush;
                    path = dicStationLineUc[polygon.Uid.Replace("STATIONARROWLINE_", "STATIONLINE_")];
                    path.Stroke = selectBrush;
                    selectType = "B";
                    selectUiElement = path;
                    selectUiElement2 = polygon;

                    LineGeometry lineGeometryTemp = (LineGeometry)(path.Data);
                    dicOldPoint["StartPoint"] = lineGeometryTemp.StartPoint;
                    dicOldPoint["EndPoint"] = lineGeometryTemp.EndPoint;

                    dicOldPoint["Point1"] = polygon.Points[0];
                    dicOldPoint["Point2"] = polygon.Points[1];
                    dicOldPoint["Point3"] = polygon.Points[2];

                    GetItemSource(selectType, path.Uid);
                    dataGrid.ItemsSource = itemSource;
                }
            }
        }

        /// 地标曲线单击事件
        /// <summary>
        /// 地标曲线单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lastCurve_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rabType == "L" && !isStart)
            {
                gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                BackUielementStatus();
                Path path = sender as Path;
                Polygon polygon;
                dicOldPoint.Clear();
                isOneClick = true;
                if (path != null)
                {
                    brushPre = path.Stroke;
                    path.Stroke = selectBrush;
                    polygon = dicstationCurveArrowUc[path.Uid.Replace("STATIONCURVE_", "STATIONCURVEARROW_")];
                    polygon.Stroke = polygon.Fill = selectBrush;
                    selectType = "C";
                    selectUiElement = path;
                    selectUiElement2 = polygon;

                    PathFigure pathFigureTemp = ((PathGeometry)(((Path)selectUiElement).Data)).Figures[0];
                    dicOldPoint["StartPoint"] = pathFigureTemp.StartPoint;
                    BezierSegment bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                    dicOldPoint["Point1"] = bezierSegmentTemp.Point1;
                    dicOldPoint["Point2"] = bezierSegmentTemp.Point2;
                    dicOldPoint["Point3"] = bezierSegmentTemp.Point3;

                    dicOldPoint["Point11"] = polygon.Points[0];
                    dicOldPoint["Point21"] = polygon.Points[1];
                    dicOldPoint["Point31"] = polygon.Points[2];

                    GetItemSource(selectType, path.Uid);
                    dataGrid.ItemsSource = itemSource;
                    return;
                }
                polygon = sender as Polygon;
                if (polygon != null)
                {
                    brushPre = polygon.Stroke;
                    polygon.Stroke = polygon.Fill = selectBrush;
                    path = dicstationCurveUc[polygon.Uid.Replace("STATIONCURVEARROW_", "STATIONCURVE_")];
                    path.Stroke = selectBrush;
                    selectType = "C";
                    selectUiElement = path;
                    selectUiElement2 = polygon;

                    PathFigure pathFigureTemp = ((PathGeometry)(((Path)selectUiElement).Data)).Figures[0];
                    dicOldPoint["StartPoint"] = pathFigureTemp.StartPoint;
                    BezierSegment bezierSegmentTemp = (BezierSegment)pathFigureTemp.Segments[0];
                    dicOldPoint["Point1"] = bezierSegmentTemp.Point1;
                    dicOldPoint["Point2"] = bezierSegmentTemp.Point2;
                    dicOldPoint["Point3"] = bezierSegmentTemp.Point3;

                    dicOldPoint["Point11"] = polygon.Points[0];
                    dicOldPoint["Point21"] = polygon.Points[1];
                    dicOldPoint["Point31"] = polygon.Points[2];

                    GetItemSource(selectType, path.Uid);
                    dataGrid.ItemsSource = itemSource;
                }
            }
        }

        /// 文字单击事件
        /// <summary>
        /// 文字单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rabType == "L" && !isStart)
            {
                gridRow1.MaxHeight = gridRow2.MaxHeight = 0;
                BackUielementStatus();
                TextBlock txt = (TextBlock)sender;
                brushPre = txt.Foreground;
                txt.Foreground = selectBrush;
                selectType = "G";
                selectUiElement = txt;
                selectUiElement2 = null;

                GetItemSource(selectType, txt.Uid);
                dataGrid.ItemsSource = itemSource;
                isOneClick = true;
            }
        }

        /// 所属区域COMBOX单击事件
        /// <summary>
        /// 所属区域COMBOX单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbArea_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridRow2.MaxHeight = gridRow2.MaxHeight == 0 ? 1000 : 0;
            e.Handled = true;
        }

        /// 属性行数据加载事件
        /// <summary>
        /// 属性行数据加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var drv = e.Row.Item as Member;
            if (drv.property == "P1" || drv.property == "P2" || drv.property == "MidPoint" || drv.property == "CurrSelectPoint" || drv.property == "Location")
            {
                //e.Row.Foreground = Brushes.LightGray;
                e.Row.IsEnabled = false;
            }
        }

        /// 所属区域DATAGRID单击事件
        /// <summary>
        /// 所属区域DATAGRID单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridArea_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                gridRow2.MaxHeight = gridRow2.MaxHeight == 0 ? 1000 : 0;
            }
        }

        /// 所属区域DATAGRID选项变更事件
        /// <summary>
        /// 所属区域DATAGRID选项变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridArea_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = dataGridArea.SelectedItem as DataRowView;
            if (row != null)
            {
                //cmbArea.Text=row.Row.ItemArray[1].ToString();
                cmbArea.SelectedValue = row.Row.ItemArray[0].ToString();
                gridRow2.MaxHeight = gridRow2.MaxHeight == 0 ? 1000 : 0;
            }
        }

        /// datagrid结束编辑事件
        /// <summary>
        /// datagrid结束编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //ChoiceDataChange();
        }

        /// 属性值KEYDOWN事件
        /// <summary>
        /// 属性值KEYDOWN事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBoxVal_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChoiceDataChange();
            }
        }

        /// 图片路径单击事件
        /// <summary>
        /// 图片路径单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditImage_OnDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "所有图像文件|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.ico;*.emp;*.wmf;|位图文件|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.ico;|图元文件|*.emp;*.wmf;";
            if (openFileDialog.ShowDialog() == true)
            {
                string uid = itemSource[0].uid;
                ImageWindow imageWindow = (ImageWindow)dicImageUc[uid];
                imageWindow.SourceValue = openFileDialog.FileName;
                dtSource.Select(string.Format("Uid='{0}'", uid))[0]["ImageUrl"] = openFileDialog.FileName;
            }
        }

        /// 颜色变更事件
        /// <summary>
        /// 颜色变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupColorEdit_OnColorChanged(object sender, RoutedEventArgs e)
        {
            ChoiceDataChange();
        }

        /// 属性值变更事件
        /// <summary>
        /// 属性值变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBoxVal_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ChoiceDataChange();
        }

        #region MenuItem事件
        /// 调度通信设置单击事件
        /// <summary>
        /// 调度通信设置单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiExchange_OnClick(object sender, RoutedEventArgs e)
        {
            DbSetUpWindow dbSetUpWindow = new DbSetUpWindow();
            bool? result = dbSetUpWindow.ShowDialog();
            if (result == true)
            {
                SqlDBControl._defultConnectionString = File.ReadAllText(System.Windows.Forms.Application.StartupPath + @"\ConnectionString").Trim();
            }
        }

        /// 退出系统单击事件
        /// <summary>
        /// 退出系统单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiExist1_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定退出当前系统?", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question,
                    MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        /// 选项单击事件
        /// <summary>
        /// 选项单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiOption_OnClick(object sender, RoutedEventArgs e)
        {
            OptionWindow optionWindow = new OptionWindow();
            bool? result = optionWindow.ShowDialog();
            if (result == true)
            {
                string displayModeOld = displayMode;
                Brush brushTemp = coorBrush;
                bool IsShowCoordinateOld = IsShowCoordinate;
                GetIniData();
                //if (!IsShowCoordinate)
                //{
                //    for (int i = CanvasMain.Children.Count - 1; i >= 0; i--)
                //    {
                //        if (CanvasMain.Children[i].Uid.StartsWith("COOR"))
                //        {
                //            CanvasMain.Children.RemoveAt(i);
                //        }
                //    }
                //    return;
                //}
                //if (displayModeOld != displayMode)
                //{
                //    for (int i = CanvasMain.Children.Count - 1; i >= 0; i--)
                //    {
                //        if (CanvasMain.Children[i].Uid.StartsWith("COOR"))
                //        {
                //            CanvasMain.Children.RemoveAt(i);
                //        }
                //    }
                //    InitialCanvas3();
                //}
                //else
                //{
                //    if (!IsShowCoordinateOld)
                //    {
                //        InitialCanvas3();
                //        return;
                //    }
                //    if (brushTemp.ToString() == coorBrush.ToString())
                //    {
                //        return;
                //    }
                //    Line line;
                //    Path path;
                //    for (int i = CanvasMain.Children.Count - 1; i >= 0; i--)
                //    {
                //        if (CanvasMain.Children[i].Uid.StartsWith("COORLINE"))
                //        {
                //            line = (Line)CanvasMain.Children[i];
                //            line.Stroke = coorBrush;
                //            //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                //        }
                //        if (CanvasMain.Children[i].Uid.StartsWith("COORPOINT_"))
                //        {
                //            path = (Path)CanvasMain.Children[i];
                //            path.Stroke = coorBrush;
                //            //path.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                //        }
                //    }
                //}

                if (!IsShowCoordinate)
                {
                    CanvasCoordinate.ClearVisual();
                    return;
                }
                if (displayModeOld != displayMode)
                {
                    CanvasCoordinate.ClearVisual();
                    InitialCanvas3();
                }
                else
                {
                    if (!IsShowCoordinateOld)
                    {
                        InitialCanvas3();
                        return;
                    }
                    if (brushTemp.ToString() == coorBrush.ToString())
                    {
                        return;
                    }
                    CanvasCoordinate.ClearVisual();
                    InitialCanvas3();
                }
            }
        }

        /// 退出单击事件
        /// <summary>
        /// 退出单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiExist_OnClick(object sender, RoutedEventArgs e)
        {
            MeiExist1_OnClick(null, null);
        }

        /// 打开地图单击事件
        /// <summary>
        /// 打开地图单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiOpen_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "AGV文件|*.agv";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                if (!File.Exists(openFileDialog.FileName))
                {
                    MessageBox.Show("文件不存在", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                    return;
                }
                scaleLevel = 1;
                totalScale.ScaleX = scaleLevel;
                totalScale.ScaleY = scaleLevel;
                totalScale.CenterX = CenterPoint.X;
                totalScale.CenterY = CenterPoint.Y;

                tempTranslate.X = tempTranslate.Y = 0;
                totalTranslate.X = totalTranslate.Y = 0;

                tfGroup = new TransformGroup();
                tfGroup.Children.Add(tempTranslate);
                tfGroup.Children.Add(totalScale);

                CanvasMain.Children.Clear();
                CanvasCoordinate.ClearVisual();
                ClearData();

                if (OpenMap(openFileDialog.FileName))
                {
                    //MessageBox.Show("打开成功");
                }
            }
        }

        /// 新建地图单击事件
        /// <summary>
        /// 新建地图单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiCreate_OnClick(object sender, RoutedEventArgs e)
        {
            scaleLevel = 1;
            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            totalScale.CenterX = CenterPoint.X;
            totalScale.CenterY = CenterPoint.Y;

            tempTranslate.X = tempTranslate.Y = 0;
            totalTranslate.X = totalTranslate.Y = 0;

            tfGroup = new TransformGroup();
            tfGroup.Children.Add(tempTranslate);
            tfGroup.Children.Add(totalScale);

            CanvasMain.Children.Clear();
            CanvasCoordinate.ClearVisual();
            InitCenter();
            ClearData();
        }

        /// 坐标系对照单击事件
        /// <summary>
        /// 坐标系对照单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiXY_OnClick(object sender, RoutedEventArgs e)
        {
            CoorContrastWindow coorContrastWindow = new CoorContrastWindow();
            coorContrastWindow.ShowDialog();
        }

        /// 导入路径长度单击事件
        /// <summary>
        /// 导入路径长度单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiImport_OnClick(object sender, RoutedEventArgs e)
        {
        }

        /// 保存单击事件
        /// <summary>
        /// 保存单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认保存当前地图?", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question,
                    MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                string fileName = System.Windows.Forms.Application.StartupPath + @"\AGVINFO.agv";
                if (SaveAs(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(fs))
                        {
                            byte[] bytes = binaryReader.ReadBytes((int) fs.Length);

                            Function.Update_tbPlanset("AGVINFO.agv", bytes, 0, scaleLevel, 0);

                            MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk,
                                MessageBoxResult.OK);
                        }
                    }
                }
            }
        }

        /// 另存为文件单击事件
        /// <summary>
        /// 另存为文件单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiSaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.FileName = "AGVINFO";
            saveFileDialog.Filter = "AGV文件|.agv";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (SaveAs(saveFileDialog.FileName))
                {
                    MessageBox.Show("另存为成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
                }
            }
        }

        /// AGV档案配置单击事件
        /// <summary>
        /// AGV档案配置单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiFile_OnClick(object sender, RoutedEventArgs e)
        {
            MachineInfoWindow machineInfoWindow = new MachineInfoWindow();
            machineInfoWindow.ShowDialog();
        }

        /// 系统参数单击事件
        /// <summary>
        /// 系统参数单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiSysInfo_OnClick(object sender, RoutedEventArgs e)
        {
            SysParameterWindow sysParameterWindow = new SysParameterWindow();
            sysParameterWindow.ShowDialog();
        }

        /// 呼叫器档案单击事件
        /// <summary>
        /// 呼叫器档案单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiLoud_OnClick(object sender, RoutedEventArgs e)
        {
            CallDetailWindow callDetailWindow = new CallDetailWindow();
            callDetailWindow.ShowDialog();
        }

        /// 任务条件配置单击事件
        /// <summary>
        /// 任务条件配置单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiTaskSetUp_OnClick(object sender, RoutedEventArgs e)
        {
            TaskSetUpWindow taskSetUpWindow = new TaskSetUpWindow();
            taskSetUpWindow.ShowDialog();
        }

        /// 区域档案单击事件
        /// <summary>
        /// 区域档案单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiAreaFile_OnClick(object sender, RoutedEventArgs e)
        {
            AreaInfoWindow areaInfoWindow = new AreaInfoWindow();
            areaInfoWindow.ShowDialog();

            DataTable dtArea = Function.GetDataInfo("PR_SELECT_AREA_INFO");
            dataGridArea.ItemsSource = dtArea.DefaultView;

            cmbArea.ItemsSource = dtArea.DefaultView;
        }

        /// 动作档案单击事件
        /// <summary>
        /// 动作档案单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiActionFile_OnClick(object sender, RoutedEventArgs e)
        {
            ActionWindow actionWindow = new ActionWindow();
            actionWindow.ShowDialog();
        }

        /// 物料档案单击事件
        /// <summary>
        /// 物料档案单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiMaterialFile_OnClick(object sender, RoutedEventArgs e)
        {
            MaterialWindow materialWindow = new MaterialWindow();
            materialWindow.ShowDialog();
        }

        /// 启动单击事件
        /// <summary>
        /// 启动单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiStart_OnClick(object sender, RoutedEventArgs e)
        {
            miFile.IsEnabled = MeSysSetUp.IsEnabled = MeOutSetUp.IsEnabled = MeOutBasic.IsEnabled = GridLeft.IsEnabled = GridRight.IsEnabled = false;

            startLine = null;
            stationCurrent = null;
            firstStation = secondStation = null;
            lineStart = null;
            curveP1 = curveP2 = curveP3 = null;
            pathSel1 = null;

            timer.Start();
            isStart = true;
        }

        /// 停止单击事件
        /// <summary>
        /// 停止单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiStop_OnClick(object sender, RoutedEventArgs e)
        {
            miFile.IsEnabled = MeSysSetUp.IsEnabled = MeOutSetUp.IsEnabled = MeOutBasic.IsEnabled = GridLeft.IsEnabled = GridRight.IsEnabled = true;
            timer.Stop();
            CarWindow carWindow;
            foreach (UIElement ue in dicCarUc.Values)
            {
                carWindow = ue as CarWindow;
                if (carWindow != null)
                {
                    CanvasMain.Children.Remove(carWindow);
                }
            }

            dicCarUc.Clear();
            dicCar.Clear();
            dtCarInfoOld.Clear();
            isStart = false;
        }

        /// 保存任务单击事件
        /// <summary>
        /// 保存任务单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiSaveTask_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectUiElement != null && selectType == "A")
            {
                StationWindow stationWindow = selectUiElement as StationWindow;
                if (stationWindow != null)
                {
                    Function.Insert_tbOrder(stationWindow.numValue);
                    MessageBoxAlert.Show("保存任务成功", MessageBoxImage.Asterisk);
                }
            }
            else
            {
                MessageBoxAlert.Show("请选择地标", MessageBoxImage.Exclamation);
            }
        }

        /// 保存路径单击事件
        /// <summary>
        /// 保存路径单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeiSaveLoad_OnClick(object sender, RoutedEventArgs e)
        {
            if (dicStationUc.Count <= 0||lstStation.Count==0)
            {
                MessageBoxAlert.Show("请先维护数据", MessageBoxImage.Exclamation);
                return;
            }
            if (Function.Update_tbMapInfo(dicStationUc, lstStation,dicStation))
            {
                MessageBoxAlert.Show("保存路径成功", MessageBoxImage.Asterisk);
            }
        }
        #endregion

        #region Func
        /// 画布是所有元素设置移动，缩放比例方法
        /// <summary>
        /// 画布是所有元素设置移动，缩放比例方法
        /// </summary>
        private void AdjustGraph()
        {
            tfGroup = new TransformGroup();
            tfGroup.Children.Add(tempTranslate);
            tfGroup.Children.Add(totalScale);

            foreach (UIElement ue in CanvasMain.Children)
            {
                ue.RenderTransform = tfGroup;
            }

            //DrawingVisual dv;
            //foreach (Visual v in CanvasCoordinate.AllVisuals)
            //{
            //    dv = v as DrawingVisual;
            //    if (dv != null)
            //    {
            //        dv.Transform = tfGroup;
            //    }
            //}

            CanvasCoordinate.ClearVisual();
            InitialCanvas3();
        }

        /// 初始NULL值方法
        /// <summary>
        /// 初始NULL值方法
        /// </summary>
        void InitNull()
        {
            startLine = null;
            stationCurrent = null;
            firstStation = secondStation = null;
            lineStart = null;
            curveP1 = curveP2 = curveP3 = null;
            if (rabType != "N")
            {
                selectType = "";
                selectUiElement = selectUiElement2 = null;
            }
            pathSel1 = null;
        }

        /// 初始化中心点，中心原点坐标方法
        /// <summary>
        /// 初始化中心点，中心原点坐标方法
        /// </summary>
        void InitCenter()
        {
            if (line1 != null)
            {
                CanvasMain.Children.Remove(line1);
                CanvasMain.Children.Remove(line2);
            }
            CenterPoint.X = CanvasCoordinate.ActualWidth / 2;
            CenterPoint.Y = CanvasCoordinate.ActualHeight / 2;

            line1 = new Line();
            line1.Uid = "LINECENTERH";
            line1.X1 = CenterPoint.X - centerLineWidth;
            line1.Y1 = CenterPoint.Y;
            line1.X2 = CenterPoint.X + centerLineWidth;
            line1.Y2 = CenterPoint.Y;
            line1.StrokeThickness = penSize;
            line1.Stroke = Brushes.Red;
            CanvasMain.Children.Add(line1);
            Canvas.SetZIndex(line1, 50);

            line2 = new Line();
            line2.Uid = "LINEDCENTERV";
            line2.X1 = CenterPoint.X;
            line2.Y1 = CenterPoint.Y - centerLineWidth;
            line2.X2 = CenterPoint.X;
            line2.Y2 = CenterPoint.Y + centerLineWidth;
            line2.StrokeThickness = penSize;
            line2.Stroke = Brushes.Red;
            CanvasMain.Children.Add(line2);
            Canvas.SetZIndex(line2, 50);

            InitialCanvas3();
        }

        /// 初始化画布方法2
        /// <summary>
        /// 初始化画布方法2
        /// </summary>
        void InitialCanvas2()
        {
            DateTime d1 = DateTime.Now;
            //CanvasMain.BeginInit();
            if (!IsShowCoordinate)
            {
                return;
            }
            if (displayMode == "L")
            {
                #region 线

                Line line;
                int index = 1;
                double temp = 0;
                double length = coordinateCount * Scale;
                //竖线
                line = new Line();
                line.Uid = "COORLINECENTERRL";
                line.X1 = CenterPoint.X;
                line.Y1 = -length + CenterPoint.Y;
                line.X2 = CenterPoint.X;
                line.Y2 = length + CenterPoint.Y;
                line.StrokeThickness = 1 / scaleLevel;
                line.Stroke = coorBrush;
                //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                if (tfGroup != null)
                {
                    line.RenderTransform = tfGroup;
                }
                CanvasMain.Children.Add(line);

                while (true)
                {
                    line = new Line();
                    line.Uid = "COORLINERIGHT" + index;
                    temp = CenterPoint.X + index * Scale;
                    line.X1 = temp;
                    line.Y1 = -length + CenterPoint.Y;
                    line.X2 = temp;
                    line.Y2 = length + CenterPoint.Y;
                    line.StrokeThickness = 1 / scaleLevel;
                    line.Stroke = coorBrush;
                    //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                    if (tfGroup != null)
                    {
                        line.RenderTransform = tfGroup;
                    }
                    CanvasMain.Children.Add(line);

                    line = new Line();
                    line.Uid = "COORLINELEFT" + index;
                    temp = CenterPoint.X - index * Scale;
                    line.X1 = temp;
                    line.Y1 = -length + CenterPoint.Y;
                    line.X2 = temp;
                    line.Y2 = length + CenterPoint.Y;
                    line.StrokeThickness = 1 / scaleLevel;
                    line.Stroke = coorBrush;
                    //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                    if (tfGroup != null)
                    {
                        line.RenderTransform = tfGroup;
                    }
                    CanvasMain.Children.Add(line);

                    if (index >= coordinateCount)
                    {
                        break;
                    }
                    index++;
                }

                //横线
                index = 1;
                line = new Line();
                line.Uid = "COORLINECENTERUD";
                line.X1 = -length + CenterPoint.X;
                line.Y1 = CenterPoint.Y;
                line.X2 = length + CenterPoint.X;
                line.Y2 = CenterPoint.Y;
                line.StrokeThickness = 1 / scaleLevel;
                line.Stroke = coorBrush;
                //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                if (tfGroup != null)
                {
                    line.RenderTransform = tfGroup;
                }
                CanvasMain.Children.Add(line);

                while (true)
                {
                    line = new Line();
                    line.Uid = "COORLINEUP" + index;
                    temp = CenterPoint.Y - index * Scale;
                    line.X1 = -length + CenterPoint.X;
                    line.Y1 = temp;
                    line.X2 = length + CenterPoint.X;
                    line.Y2 = temp;
                    line.StrokeThickness = 1 / scaleLevel;
                    line.Stroke = coorBrush;
                    //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                    if (tfGroup != null)
                    {
                        line.RenderTransform = tfGroup;
                    }
                    CanvasMain.Children.Add(line);

                    line = new Line();
                    line.Uid = "COORLINEDOWN" + index;
                    temp = CenterPoint.Y + index * Scale;
                    line.X1 = -length + CenterPoint.X;
                    line.Y1 = temp;
                    line.X2 = length + CenterPoint.X;
                    line.Y2 = temp;
                    line.StrokeThickness = 1 / scaleLevel;
                    line.Stroke = coorBrush;
                    //line.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                    if (tfGroup != null)
                    {
                        line.RenderTransform = tfGroup;
                    }
                    CanvasMain.Children.Add(line);
                    if (index >= coordinateCount)
                    {
                        break;
                    }
                    index++;
                }

                #endregion
            }
            else
            {
                #region 用户控件方式

                //for (int i = 0; i <= coordinateCount; i++)
                //{
                //    for (int j = 0; j <= coordinateCount; j++)
                //    {
                //        pointWindow = new PointWindow(coorBrush,pointWidth);
                //        pointWindow.Uid = "COORPOINT_" + index++;
                //        pointWindow.SetValue(Canvas.LeftProperty, CenterPoint.X - pointWidth/2 + Scale * i);
                //        pointWindow.SetValue(Canvas.TopProperty, CenterPoint.Y - pointWidth / 2 + Scale * j);
                ////        pointWindow.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                //        dicPoint[pointWindow.Uid] = new Point(Canvas.GetLeft(pointWindow), Canvas.GetTop(pointWindow));
                //        CanvasMain.Children.Add(pointWindow);

                //        if (j > 0)
                //        {
                //            pointWindow = new PointWindow(coorBrush, pointWidth);
                //            pointWindow.Uid = "COORPOINT_" + index++;
                //            pointWindow.SetValue(Canvas.LeftProperty, CenterPoint.X - pointWidth / 2 + Scale * i);
                //            pointWindow.SetValue(Canvas.TopProperty, CenterPoint.Y - pointWidth / 2 - Scale * j);
                ////            pointWindow.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                //            dicPoint[pointWindow.Uid] = new Point(Canvas.GetLeft(pointWindow), Canvas.GetTop(pointWindow));
                //            CanvasMain.Children.Add(pointWindow);
                //        }

                //        if (i > 0)
                //        {
                //            pointWindow = new PointWindow(coorBrush, pointWidth);
                //            pointWindow.Uid = "COORPOINT_" + index++;
                //            pointWindow.SetValue(Canvas.LeftProperty, CenterPoint.X - pointWidth / 2 - Scale * i);
                //            pointWindow.SetValue(Canvas.TopProperty, CenterPoint.Y - pointWidth / 2 - Scale * j);
                ////            pointWindow.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                //            dicPoint[pointWindow.Uid] = new Point(Canvas.GetLeft(pointWindow), Canvas.GetTop(pointWindow));
                //            CanvasMain.Children.Add(pointWindow);
                //        }

                //        if (i > 0&&j>0)
                //        {
                //            pointWindow = new PointWindow(coorBrush, pointWidth);
                //            pointWindow.Uid = "COORPOINT_" + index++;
                //            pointWindow.SetValue(Canvas.LeftProperty, CenterPoint.X - pointWidth / 2 - Scale * i);
                //            pointWindow.SetValue(Canvas.TopProperty, CenterPoint.Y - pointWidth / 2 + Scale * j);
                ////            pointWindow.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                //            dicPoint[pointWindow.Uid] = new Point(Canvas.GetLeft(pointWindow), Canvas.GetTop(pointWindow));
                //            CanvasMain.Children.Add(pointWindow);
                //        }
                //    }
                //}

                #endregion

                #region 点

                //PointWindow pointWindow;
                EllipseGeometry ellipseGeometry;
                Path path;
                int index = 1;

                for (int i = 0; i <= coordinateCount; i++)
                {
                    for (int j = 0; j <= coordinateCount; j++)
                    {
                        ellipseGeometry =
                            new EllipseGeometry(new Point(CenterPoint.X + Scale * i, CenterPoint.Y + Scale * j),
                                pointWidth / 2, pointWidth / 2);
                        path = new Path();
                        path.Stroke = coorBrush;
                        path.Fill = coorBrush;
                        path.StrokeThickness = 1 / scaleLevel;
                        path.Data = ellipseGeometry;
                        path.Uid = "COORPOINT_" + index++;
                        //path.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                        if (tfGroup != null)
                        {
                            path.RenderTransform = tfGroup;
                        }
                        CanvasMain.Children.Add(path);

                        if (j > 0)
                        {
                            ellipseGeometry =
                                new EllipseGeometry(new Point(CenterPoint.X + Scale * i, CenterPoint.Y - Scale * j),
                                    pointWidth / 2, pointWidth / 2);
                            path = new Path();
                            path.Stroke = coorBrush;
                            path.Fill = coorBrush;
                            path.StrokeThickness = 1 / scaleLevel;
                            path.Data = ellipseGeometry;
                            path.Uid = "COORPOINT_" + index++;
                            //path.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                            if (tfGroup != null)
                            {
                                path.RenderTransform = tfGroup;
                            }
                            CanvasMain.Children.Add(path);
                        }

                        if (i > 0)
                        {
                            ellipseGeometry =
                                new EllipseGeometry(new Point(CenterPoint.X - Scale * i, CenterPoint.Y - Scale * j),
                                    pointWidth / 2, pointWidth / 2);
                            path = new Path();
                            path.Stroke = coorBrush;
                            path.Fill = coorBrush;
                            path.StrokeThickness = 1 / scaleLevel;
                            path.Data = ellipseGeometry;
                            path.Uid = "COORPOINT_" + index++;
                            //path.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                            if (tfGroup != null)
                            {
                                path.RenderTransform = tfGroup;
                            }
                            CanvasMain.Children.Add(path);
                        }

                        if (i > 0 && j > 0)
                        {
                            ellipseGeometry =
                                new EllipseGeometry(new Point(CenterPoint.X - Scale * i, CenterPoint.Y + Scale * j),
                                    pointWidth / 2, pointWidth / 2);
                            path = new Path();
                            path.Stroke = coorBrush;
                            path.Fill = coorBrush;
                            path.StrokeThickness = 1 / scaleLevel;
                            path.Data = ellipseGeometry;
                            path.Uid = "COORPOINT_" + index++;
                            //path.Visibility = IsShowCoordinate ? Visibility.Visible : Visibility.Hidden;
                            if (tfGroup != null)
                            {
                                path.RenderTransform = tfGroup;
                            }
                            CanvasMain.Children.Add(path);
                        }
                    }
                }
                #endregion
            }

            //CanvasMain.EndInit();
            CanvasMain.UpdateLayout();
            TimeSpan span = DateTime.Now - d1;
            this.Title = span.TotalSeconds.ToString();
        }

        /// 初始化画布方法3
        /// <summary>
        /// 初始化画布方法3
        /// </summary>
        void InitialCanvas3()
        {
            //DateTime d1 = DateTime.Now;
            //CanvasMain.BeginInit();
            if (!IsShowCoordinate)
            {
                return;
            }
            DrawingVisual visual = new DrawingVisual();
            //Pen pen = new Pen(coorBrush, 1 / scaleLevel);
            Pen pen = new Pen(coorBrush, 1);

            pen.Freeze(); //冻结画笔，这样能加快绘图速度
            if (displayMode == "L")
            {
                #region 线
                int index = 1;
                double temp = 0;
                double length = coordinateCount * Scale;

                using (DrawingContext dc = visual.RenderOpen())
                {
                    dc.DrawLine(pen, new Point(CenterPoint.X, -length + CenterPoint.Y), new Point(CenterPoint.X, length + CenterPoint.Y));

                    while (true)
                    {
                        temp = CenterPoint.X + index * Scale;
                        dc.DrawLine(pen, new Point(temp, -length + CenterPoint.Y), new Point(temp, length + CenterPoint.Y));

                        temp = CenterPoint.X - index * Scale;
                        dc.DrawLine(pen, new Point(temp, -length + CenterPoint.Y), new Point(temp, length + CenterPoint.Y));

                        if (index >= coordinateCount)
                        {
                            break;
                        }
                        index++;
                    }

                    //横线
                    index = 1;

                    dc.DrawLine(pen, new Point(-length + CenterPoint.X, CenterPoint.Y), new Point(length + CenterPoint.X, CenterPoint.Y));

                    while (true)
                    {
                        temp = CenterPoint.Y - index * Scale;
                        dc.DrawLine(pen, new Point(-length + CenterPoint.X, temp), new Point(length + CenterPoint.X, temp));

                        temp = CenterPoint.Y + index * Scale;
                        dc.DrawLine(pen, new Point(-length + CenterPoint.X, temp), new Point(length + CenterPoint.X, temp));

                        if (index >= coordinateCount)
                        {
                            break;
                        }
                        index++;
                    }
                }
                #endregion
            }
            else
            {
                #region 点
                using (DrawingContext dc = visual.RenderOpen())
                {
                    for (int i = 0; i <= coordinateCount; i++)
                    {
                        for (int j = 0; j <= coordinateCount; j++)
                        {
                            //pen.Freeze(); //冻结画笔，这样能加快绘图速度
                            dc.DrawEllipse(coorBrush, pen, new Point(CenterPoint.X + Scale * i, CenterPoint.Y + Scale * j), pointWidth / (2 * scaleLevel), pointWidth / (2 * scaleLevel));

                            if (j > 0)
                            {
                                //pen.Freeze(); //冻结画笔，这样能加快绘图速度
                                dc.DrawEllipse(coorBrush, pen, new Point(CenterPoint.X + Scale * i, CenterPoint.Y - Scale * j), pointWidth / (2 * scaleLevel), pointWidth / (2 * scaleLevel));
                            }

                            if (i > 0)
                            {
                                //pen.Freeze(); //冻结画笔，这样能加快绘图速度
                                dc.DrawEllipse(coorBrush, pen, new Point(CenterPoint.X - Scale * i, CenterPoint.Y - Scale * j), pointWidth / (2 * scaleLevel), pointWidth / (2 * scaleLevel));
                            }

                            if (i > 0 && j > 0)
                            {
                                //pen.Freeze(); //冻结画笔，这样能加快绘图速度
                                dc.DrawEllipse(coorBrush, pen, new Point(CenterPoint.X - Scale * i, CenterPoint.Y + Scale * j), pointWidth / (2 * scaleLevel), pointWidth / (2 * scaleLevel));
                            }
                        }
                    }
                }
                #endregion
            }

            if (tfGroup != null)
            {
                visual.Transform = tfGroup;
            }
            CanvasCoordinate.AddVisual(visual);
            //CanvasMain.EndInit();
            //CanvasCoordinate.UpdateLayout();
            //TimeSpan span = DateTime.Now - d1;
            //this.Title = span.TotalSeconds.ToString();
        }

        /// 产生地标方法
        /// <summary>
        /// 产生地标方法
        /// </summary>
        void GeneralStation(Point point,string landCode,string landName,Brush color)
        {
            Point location = GetOldPointByNowPoint(point);
            StationWindow stationWindow = new StationWindow(landCode ?? stationCount.ToString(), landName??"");
            stationWindow.Uid = "STATION_" + stationCount++;
            dicStation[stationWindow.Uid] = new Point(location.X - 10, location.Y - 30);
            stationWindow.mouseDown += stationWindow_mouseDown;
            stationWindow.mouseEnter += stationWindow_mouseEnter;
            if (tfGroup != null)
            {
                stationWindow.RenderTransform = tfGroup;
            }
            stationWindow.SetValue(Canvas.LeftProperty, (location.X - 10) * scaleLevel);
            stationWindow.SetValue(Canvas.TopProperty, (location.Y - 30) * scaleLevel);
            CanvasMain.Children.Add(stationWindow);
            dicStationUc[stationWindow.Uid] = stationWindow;

            Canvas.SetZIndex(stationWindow, 100);
            StatusBarItem.Content = "当前最大地标:" + (stationCount - 1);
        }

        /// 产生储位方法
        /// <summary>
        /// 产生储位方法
        /// </summary>
        void GeneralStore(Point point, string stcokID, string lankMarkCode, string storageName,Brush color)
        {
            Point location = GetOldPointByNowPoint(point);
            StoreWindow storeWindow = new StoreWindow(stcokID??storeCount.ToString(),lankMarkCode??"",storageName??"", storeWidth);
            storeWindow.Uid = "STORE_" + storeCount++;
            storeWindow.mouseDown += storeWindow_mouseDown;
            if (color != null)
            {
                storeWindow.BackgroundValue = color;
            }
            dicStore[storeWindow.Uid] = new Point(location.X, location.Y);
            
            if (tfGroup != null)
            {
                storeWindow.RenderTransform = tfGroup;
            }
            storeWindow.SetValue(Canvas.LeftProperty, location.X * scaleLevel);
            storeWindow.SetValue(Canvas.TopProperty, location.Y * scaleLevel);
            CanvasMain.Children.Add(storeWindow);
            dicStoreUc[storeWindow.Uid] = storeWindow;
            Canvas.SetZIndex(storeWindow, 10);
        }

        /// 产生文字方法
        /// <summary>
        /// 产生文字方法
        /// </summary>
        void GeneralFont(Point point,string text,Brush color,double? fontSize)
        {
            Point location = GetOldPointByNowPoint(point);
            TextBlock txt = new TextBlock();
            txt.Foreground = color;
            if (fontSize != null)
            {
                txt.FontSize = (double)fontSize;
            }
            txt.Text =text?? "输入文字...";
            txt.Uid = "FONT_" + fontCount++;
            txt.MouseDown += txt_MouseDown;
            dicFont[txt.Uid] = new Point(location.X, location.Y);

            if (tfGroup != null)
            {
                txt.RenderTransform = tfGroup;
            }
            txt.SetValue(Canvas.LeftProperty, location.X * scaleLevel);
            txt.SetValue(Canvas.TopProperty, location.Y * scaleLevel);

            CanvasMain.Children.Add(txt);
            dicFontUc[txt.Uid] = txt;

            Canvas.SetZIndex(txt, 10);
        }

        /// 产生图片方法
        /// <summary>
        /// 产生图片方法
        /// </summary>
        void GeneralImage(Point point,double? height,double? width,string image)
        {
            Point location = GetOldPointByNowPoint(point);
            ImageWindow imageWindow = new ImageWindow(height ?? imageHeight, width??imageWidth);
            imageWindow.Uid = "IMAGE_" + imageCount++;
            if (image != null)
            {
                imageWindow.SourceValue = image;
            }
            imageWindow.mouseDown += imageWindow_mouseDown;
            dicImage[imageWindow.Uid] = new Point(location.X, location.Y);

            if (tfGroup != null)
            {
                imageWindow.RenderTransform = tfGroup;
            }
            imageWindow.SetValue(Canvas.LeftProperty, location.X * scaleLevel);
            imageWindow.SetValue(Canvas.TopProperty, location.Y * scaleLevel);
            CanvasMain.Children.Add(imageWindow);
            dicImageUc[imageWindow.Uid] = imageWindow;

            Canvas.SetZIndex(imageWindow, 10);
        }

        /// 产生呼叫器方法
        /// <summary>
        /// 产生呼叫器方法
        /// </summary>
        void GeneralCall(Point point,string num,double? width,Brush color)
        {
            Point location = GetOldPointByNowPoint(point);
            CallWindow callWindow = new CallWindow(width??callRadius, num ?? callCount.ToString());
            if (color != null)
            {
                callWindow.BackgroundValue = color;
            }
            callWindow.Uid = "CALL_" + callCount++;
            dicCall[callWindow.Uid] = new Point(location.X, location.Y);
            callWindow.mouseDown += callWindow_mouseDown;
            if (tfGroup != null)
            {
                callWindow.RenderTransform = tfGroup;
            }
            callWindow.SetValue(Canvas.LeftProperty, location.X * scaleLevel);
            callWindow.SetValue(Canvas.TopProperty, location.Y * scaleLevel);
            CanvasMain.Children.Add(callWindow);
            dicCallUc[callWindow.Uid] = callWindow;
            Canvas.SetZIndex(callWindow, 10);
        }

        /// 产生小车方法
        /// <summary>
        /// 产生小车方法
        /// </summary>
        void GeneralCar()
        {
            dtCarInfo = Function.GetDataInfo("PR_SELECT_TBCAR");
            CarWindow carWindow;
            if (dtCarInfoOld == null || dtCarInfoOld.Rows.Count == 0)
            {
                foreach (DataRow dr in dtCarInfo.Rows)
                {
                    carWindow = new CarWindow(dr["CarCode"].ToString(), carHeight, carWidth);
                    carWindow.Uid = "CAR_" + dr["CarCode"].ToString();
                    if (tfGroup != null)
                    {
                        carWindow.RenderTransform = tfGroup;
                    }
                    carWindow.SetValue(Canvas.LeftProperty, (dicStation["STATION_" + dr["StandbyLandMark"]].X + 10 - carWidth / 2) * scaleLevel);
                    carWindow.SetValue(Canvas.TopProperty, (dicStation["STATION_" + dr["StandbyLandMark"]].Y + 30 - carHeight / 2) * scaleLevel);
                    CanvasMain.Children.Add(carWindow);
                    dicCar[carWindow.Uid] = new Point(dicStation["STATION_" + dr["StandbyLandMark"]].X + 10 - carWidth / 2, dicStation["STATION_" + dr["StandbyLandMark"]].Y + 30 - carHeight / 2);
                    dicCarUc[carWindow.Uid] = carWindow;
                    Canvas.SetZIndex(carWindow, 200);
                }
            }
            else
            {
                DataRow[] dataRows;
                foreach (DataRow dr in dtCarInfo.Rows)
                {
                    dataRows = dtCarInfoOld.Select(string.Format("CarCode='{0}'", dr["CarCode"].ToString()));
                    if (dataRows.Length > 0)
                    {
                        if (dr["StandbyLandMark"].ToString() != dataRows[0]["StandbyLandMark"].ToString())
                        {
                            Canvas.SetLeft(dicCarUc["CAR_" + dr["CarCode"]], (dicStation["STATION_" + dr["StandbyLandMark"]].X + 10 - carWidth / 2) * scaleLevel);
                            Canvas.SetTop(dicCarUc["CAR_" + dr["CarCode"]], (dicStation["STATION_" + dr["StandbyLandMark"]].Y + 30 - carHeight / 2) * scaleLevel);
                            dicCar["CAR_" + dr["CarCode"]] = new Point(dicStation["STATION_" + dr["StandbyLandMark"]].X + 10 - carWidth / 2, dicStation["STATION_" + dr["StandbyLandMark"]].Y + 30 - carHeight / 2);
                        }
                    }
                    else
                    {
                        carWindow = new CarWindow(dr["CarCode"].ToString(), carHeight, carWidth);
                        carWindow.Uid = "CAR_" + dr["CarCode"].ToString();
                        if (tfGroup != null)
                        {
                            carWindow.RenderTransform = tfGroup;
                        }
                        carWindow.SetValue(Canvas.LeftProperty, (dicStation["STATION_" + dr["StandbyLandMark"]].X + 10 - carWidth / 2) * scaleLevel);
                        carWindow.SetValue(Canvas.TopProperty, (dicStation["STATION_" + dr["StandbyLandMark"]].Y + 30 - carHeight / 2) * scaleLevel);
                        CanvasMain.Children.Add(carWindow);
                        dicCarUc[carWindow.Uid] = carWindow;
                        dicCar[carWindow.Uid] = new Point(dicStation["STATION_" + dr["StandbyLandMark"]].X + 10 - carWidth / 2, dicStation["STATION_" + dr["StandbyLandMark"]].Y + 30 - carHeight / 2);
                        Canvas.SetZIndex(carWindow, 200);
                    }
                }
            }
            dtCarInfoOld = dtCarInfo;
        }

        /// 根据现在鼠标坐标得到最初点坐标
        /// <summary>
        /// 根据现在鼠标坐标得到最初点坐标
        /// </summary>
        /// <param name="point"></param>
        private Point GetOldPointByNowPoint(Point point)
        {
            Point finallyPoint = new Point();
            if (scaleLevel == 1)
            {
                finallyPoint.X = point.X - totalTranslate.X;
                finallyPoint.Y = point.Y - totalTranslate.Y;
            }
            else
            {
                Point centerCurrentTran = line1.TranslatePoint(CenterPoint, CanvasMain);
                finallyPoint.X = (double)(((decimal)point.X - (decimal)centerCurrentTran.X) / (decimal)scaleLevel + (decimal)CenterPointTrans.X);
                finallyPoint.Y = (double)(((decimal)point.Y - (decimal)centerCurrentTran.Y) / (decimal)scaleLevel + (decimal)CenterPointTrans.Y);
            }
            return finallyPoint;
        }

        /// 根据最初点坐标得到现在鼠标坐标
        /// <summary>
        /// 根据最初点坐标得到现在鼠标坐标
        /// </summary>
        /// <param name="point">最初点坐标</param>
        private Point GetNowPointByOldPoint(Point point)
        {
            Point finallyPoint = new Point();
            if (scaleLevel == 1)
            {
                finallyPoint.X = point.X + totalTranslate.X;
                finallyPoint.Y = point.Y + totalTranslate.Y;
            }
            else
            {
                Point centerCurrentTran = line1.TranslatePoint(CenterPoint, CanvasMain);
                finallyPoint.X = (double)((decimal)centerCurrentTran.X + ((decimal)point.X - (decimal)CenterPoint.X) * (decimal)scaleLevel);
                finallyPoint.Y = (double)((decimal)centerCurrentTran.Y + ((decimal)point.Y - (decimal)CenterPoint.Y) * (decimal)scaleLevel);
            }
            return finallyPoint;
        }

        /// 根据直线起点终点取得箭头第二,三点
        /// <summary>
        /// 根据直线起点终点取得箭头第二,三点
        /// </summary>
        /// <param name="pointStart">起点</param>
        /// <param name="pointEnd">终点</param>
        /// <returns></returns>
        List<Point> GetSecondPoint(Point pointStart, Point pointEnd)
        {
            Point pointReturn2 = new Point();
            Point pointReturn3 = new Point();
            double lenX = Math.Abs(pointStart.X - pointEnd.X);
            double lenY = Math.Abs(pointStart.Y - pointEnd.Y);
            double arrowMaxLen = arrowDistance / Math.Cos(arrowAngle);//斜边
            double arrowOtherLen = arrowDistance * Math.Tan(arrowAngle);//另一直角边
            double angleTemp = Math.Atan(lenX / lenY);
            if (pointEnd.X >= pointStart.X)
            {
                if (pointEnd.Y >= pointStart.Y)
                {
                    pointReturn2.X = pointEnd.X - Math.Sin(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn2.Y = pointEnd.Y - Math.Cos(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn3.X = pointEnd.X - Math.Sin(angleTemp - arrowAngle) * arrowMaxLen - 2 * arrowOtherLen * Math.Cos(angleTemp);
                    pointReturn3.Y = pointEnd.Y - Math.Cos(angleTemp - arrowAngle) * arrowMaxLen + 2 * arrowOtherLen * Math.Sin(angleTemp);
                }
                else
                {
                    pointReturn2.X = pointEnd.X - Math.Sin(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn2.Y = pointEnd.Y + Math.Cos(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn3.X = pointEnd.X - Math.Sin(angleTemp - arrowAngle) * arrowMaxLen - 2 * arrowOtherLen * Math.Cos(angleTemp);
                    pointReturn3.Y = pointEnd.Y + Math.Cos(angleTemp - arrowAngle) * arrowMaxLen - 2 * arrowOtherLen * Math.Sin(angleTemp);
                }
            }
            else
            {
                if (pointEnd.Y >= pointStart.Y)
                {
                    pointReturn2.X = pointEnd.X + Math.Sin(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn2.Y = pointEnd.Y - Math.Cos(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn3.X = pointEnd.X + Math.Sin(angleTemp - arrowAngle) * arrowMaxLen + 2 * arrowOtherLen * Math.Cos(angleTemp);
                    pointReturn3.Y = pointEnd.Y - Math.Cos(angleTemp - arrowAngle) * arrowMaxLen + 2 * arrowOtherLen * Math.Sin(angleTemp);
                }
                else
                {
                    pointReturn2.X = pointEnd.X + Math.Sin(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn2.Y = pointEnd.Y + Math.Cos(angleTemp - arrowAngle) * arrowMaxLen;
                    pointReturn3.X = pointEnd.X + Math.Sin(angleTemp - arrowAngle) * arrowMaxLen + 2 * arrowOtherLen * Math.Cos(angleTemp);
                    pointReturn3.Y = pointEnd.Y + Math.Cos(angleTemp - arrowAngle) * arrowMaxLen - 2 * arrowOtherLen * Math.Sin(angleTemp);
                }
            }

            List<Point> lst = new List<Point>();
            lst.Add(pointReturn2);
            lst.Add(pointReturn3);
            return lst;
        }

        /// 还原上一个选择控件颜色状态
        /// <summary>
        /// 还原上一个选择控件颜色状态
        /// </summary>
        void BackUielementStatus()
        {
            if (string.IsNullOrEmpty(selectType))
            {
                return;
            }
            switch (selectType)
            {
                case "A":
                    ((StationWindow)selectUiElement).BackgroundValue = Brushes.Brown;
                    break;
                case "B":
                case "C":
                    ((Path)selectUiElement).Stroke = brushPre;
                    ((Polygon)selectUiElement2).Stroke = ((Polygon)selectUiElement2).Fill = brushPre;
                    break;
                case "D":
                case "E":
                    ((Path)selectUiElement).Stroke = brushPre;
                    break;
                case "F":
                    ((StoreWindow)selectUiElement).BackgroundValue = Brushes.White;
                    break;
                case "G":
                    ((TextBlock)selectUiElement).Foreground = brushPre;
                    break;
                case "H":
                    ((ImageWindow)selectUiElement).BackgroundValue = Brushes.White;
                    break;
                case "I":
                    ((CallWindow)selectUiElement).BackgroundValue = Brushes.DarkGray;
                    break;
            }
        }

        /// 根据类型返回中心点集合
        /// <summary>
        /// 根据类型返回中心点集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Dictionary<string, Point> GetDicPoint(string type)
        {
            switch (type)
            {
                case "A":
                    return dicStation;
                case "F":
                    return dicStore;
                case "G":
                    return dicFont;
                case "H":
                    return dicImage;
                case "I":
                    return dicCall;
                default:
                    return null;
            }
        }

        /// 取得INI档数据
        /// <summary>
        /// 取得INI档数据
        /// </summary>
        void GetIniData()
        {
            string path = System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini";
            string r = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorR", path);
            string g = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorG", path);
            string b = FileControl.SetFileControl.ReadIniValue("OPTION", "BackgroundColorB", path);
            canvasBrush = new SolidColorBrush(Color.FromRgb((byte)int.Parse(r), (byte)int.Parse(g), (byte)int.Parse(b)));

            r = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateR", path);
            g = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateG", path);
            b = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateB", path);
            coorBrush = new SolidColorBrush(Color.FromRgb((byte)int.Parse(r), (byte)int.Parse(g), (byte)int.Parse(b)));

            r = FileControl.SetFileControl.ReadIniValue("OPTION", "PenR", path);
            g = FileControl.SetFileControl.ReadIniValue("OPTION", "PenG", path);
            b = FileControl.SetFileControl.ReadIniValue("OPTION", "PenB", path);
            penBrush = new SolidColorBrush(Color.FromRgb((byte)int.Parse(r), (byte)int.Parse(g), (byte)int.Parse(b)));

            //CanvasMain.Background = canvasBrush;

            string chk = FileControl.SetFileControl.ReadIniValue("OPTION", "UseCoordinate", path);
            IsShowCoordinate = bool.Parse(chk);

            displayMode = FileControl.SetFileControl.ReadIniValue("OPTION", "CoordinateType", path);

            GridMain.Background = canvasBrush;

            penSize = double.Parse(FileControl.SetFileControl.ReadIniValue("OPTION", "PenSize", path));
        }

        /// 根据元素类型初始化数据源
        /// <summary>
        /// 根据元素类型初始化数据源
        /// </summary>
        /// <param name="type"></param>
        void GetItemSource(string type, string uid)
        {
            itemSource.Clear();
            DataRow[] dataRows = dtSource.Select(string.Format("Uid='{0}'", uid));
            if (dataRows.Length > 0)
            {
                switch (type)
                {
                    case "A":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "LandCode", dataRows[0]["LandCode"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "LandName", dataRows[0]["LandName"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "MidPoint", dataRows[0]["MidPoint"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "B":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "ExcuteAngle", dataRows[0]["ExcuteAngle"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Length", dataRows[0]["Length"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "P1", dataRows[0]["P1"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "P2", dataRows[0]["P2"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "C":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "CurrSelectPoint", dataRows[0]["CurrSelectPoint"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Length", dataRows[0]["Length"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "D":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "ExcuteAngle", dataRows[0]["ExcuteAngle"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Length", dataRows[0]["Length"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "P1", dataRows[0]["P1"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "P2", dataRows[0]["P2"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "E":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "CurrSelectPoint", dataRows[0]["CurrSelectPoint"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Length", dataRows[0]["Length"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "F":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "LankMarkCode", dataRows[0]["LankMarkCode"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "StcokID", dataRows[0]["StcokID"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "StorageName", dataRows[0]["StorageName"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "StorageState", dataRows[0]["StorageState"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        if (!string.IsNullOrEmpty(dataRows[0]["Area"].ToString()))
                        {
                            dataGridArea.SelectedValue = cmbArea.SelectedValue = dataRows[0]["Area"].ToString();
                        }
                        break;
                    case "G":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "FontSize", dataRows[0]["FontSize"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "StrValue", dataRows[0]["StrValue"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "H":
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "Height", dataRows[0]["Height"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Image", dataRows[0]["Image"].ToString(), Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Location", dataRows[0]["Location"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "TransColor", dataRows[0]["TransColor"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Width", dataRows[0]["Width"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                    case "I":
                        itemSource.Add(new Member(uid, "CallBoxID", dataRows[0]["CallBoxID"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        itemSource.Add(new Member(uid, "Color", dataRows[0]["Color"].ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                        itemSource.Add(new Member(uid, "Radius", dataRows[0]["Radius"].ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                        break;
                }
                return;
            }
            Path path;
            LineGeometry lineGeometry;
            DataRow drNew = dtSource.NewRow();
            drNew["Uid"] = uid;
            Color color;
            string cStr = "";
            switch (type)
            {
                case "A":
                    StationWindow window = (StationWindow)dicStationUc[uid];
                    //cStr = Brushes.Coral.Color.R + "," + Brushes.Coral.Color.G + "," + Brushes.Coral.Color.B;
                    itemSource.Add(new Member(uid, "Color", Brushes.Brown.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "LandCode", window.numValue, Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "LandName", window.Name, Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "MidPoint", "{X=" + dicStation[uid].X + ",Y=" + dicStation[uid].Y + "}", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = Brushes.Brown.ToString();
                    drNew["LandCode"] = window.numValue;
                    drNew["LandName"] = window.Name;
                    drNew["MidPoint"] = "{X=" + dicStation[uid].X + ",Y=" + dicStation[uid].Y + "}";
                    break;
                case "B":
                    path = dicStationLineUc[uid];
                    color = (Color)ColorConverter.ConvertFromString(brushPre.ToString());
                    //cStr = color.R + "," + color.G + "," + color.B;
                    lineGeometry = (LineGeometry)path.Data;
                    itemSource.Add(new Member(uid, "Color", color.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "ExcuteAngle", "-1", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Length", "1", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "P1", "{X=" + lineGeometry.StartPoint.X + ",Y=" + lineGeometry.StartPoint.Y + "}", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "P2", "{X=" + lineGeometry.EndPoint.X + ",Y=" + lineGeometry.EndPoint.Y + "}", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = color.ToString();
                    drNew["ExcuteAngle"] = "-1";
                    drNew["Length"] = "1";
                    drNew["P1"] = "{X=" + lineGeometry.StartPoint.X + ",Y=" + lineGeometry.StartPoint.Y + "}";
                    drNew["P2"] = "{X=" + lineGeometry.EndPoint.X + ",Y=" + lineGeometry.EndPoint.Y + "}";
                    break;
                case "C":
                    path = dicstationCurveUc[uid];
                    color = (Color)ColorConverter.ConvertFromString(brushPre.ToString());
                    //cStr = color.R + "," + color.G + "," + color.B;
                    itemSource.Add(new Member(uid, "Color", color.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "CurrSelectPoint", "p1", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Length", "0", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = color.ToString();;
                    drNew["CurrSelectPoint"] = "p1";
                    drNew["Length"] = "0";
                    break;
                case "D":
                    path = (Path)dicLineUc[uid];
                    lineGeometry = (LineGeometry)path.Data;
                    color = (Color)ColorConverter.ConvertFromString(brushPre.ToString());
                    //cStr = color.R + "," + color.G + "," + color.B;
                    itemSource.Add(new Member(uid, "Color", color.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "ExcuteAngle", "-1", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Length", "1", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "P1", "{X=" + lineGeometry.StartPoint.X + ",Y=" + lineGeometry.StartPoint.Y + "}", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "P2", "{X=" + lineGeometry.EndPoint.X + ",Y=" + lineGeometry.EndPoint.Y + "}", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = color.ToString();;
                    drNew["ExcuteAngle"] = "-1";
                    drNew["Length"] = "1";
                    drNew["P1"] = "{X=" + lineGeometry.StartPoint.X + ",Y=" + lineGeometry.StartPoint.Y + "}";
                    drNew["P2"] = "{X=" + lineGeometry.EndPoint.X + ",Y=" + lineGeometry.EndPoint.Y + "}";
                    break;
                case "E":
                    path = (Path)dicCurveUc[uid];
                    color = (Color)ColorConverter.ConvertFromString(brushPre.ToString());
                    //cStr = color.R + "," + color.G + "," + color.B;
                    itemSource.Add(new Member(uid, "Color", color.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "CurrSelectPoint", "p1", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Length", "0", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = color.ToString();;
                    drNew["CurrSelectPoint"] = "p1";
                    drNew["Length"] = "0";
                    break;
                case "F":
                    StoreWindow storeWindow = (StoreWindow)dicStoreUc[uid];
                    //cStr = Brushes.White.Color.R + "," + Brushes.White.Color.G + "," + Brushes.White.Color.B;
                    itemSource.Add(new Member(uid, "Color", Brushes.White.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "LankMarkCode", "", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "StcokID", storeWindow.NumValue, Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "StorageName", storeWindow.Name, Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "StorageState", "0", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = Brushes.White.ToString();
                    drNew["LankMarkCode"] = "";
                    drNew["StcokID"] = storeWindow.NumValue;
                    drNew["StorageName"] = storeWindow.Name;
                    drNew["StorageState"] = "0";
                    break;
                case "G":
                    TextBlock txt = (TextBlock)dicFontUc[uid];
                    color = (Color)ColorConverter.ConvertFromString(brushPre.ToString());
                    //cStr = color.R + "," + color.G + "," + color.B;
                    itemSource.Add(new Member(uid, "Color", color.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "FontSize", txt.FontSize.ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "StrValue", txt.Text, Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = color.ToString();
                    drNew["FontSize"] = txt.FontSize.ToString();
                    drNew["StrValue"] = txt.Text;
                    break;
                case "H":
                    ImageWindow imageWindow = (ImageWindow)dicImageUc[uid];
                    //cStr = Brushes.White.Color.R + "," + Brushes.White.Color.G + "," + Brushes.White.Color.B;
                    itemSource.Add(new Member(uid, "Color", Brushes.White.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "Height", imageWindow.HeightValue.ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Image", "System.Drawing.Bitmap", Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Location", "{X=" + dicImage[uid].X + ",Y=" + dicImage[uid].Y + "}", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "TransColor", "Transparent", Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Width", imageWindow.WidthValue.ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["Color"] = Brushes.White.ToString();
                    drNew["Height"] = imageWindow.HeightValue.ToString();
                    drNew["Image"] = "System.Drawing.Bitmap";
                    drNew["Location"] = "{X=" + dicImage[uid].X + ",Y=" + dicImage[uid].Y + "}";
                    drNew["TransColor"] = "Transparent";
                    drNew["Width"] = imageWindow.WidthValue.ToString();
                    break;
                case "I":
                    CallWindow callWindow = (CallWindow)dicCallUc[uid];
                    //cStr = Brushes.DarkGray.Color.R + "," + Brushes.DarkGray.Color.G + "," + Brushes.DarkGray.Color.B;
                    itemSource.Add(new Member(uid, "CallBoxID", uid.Replace("CALL_", ""), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));
                    itemSource.Add(new Member(uid, "Color", Brushes.DarkGray.ToString(), Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible));
                    itemSource.Add(new Member(uid, "Radius", callWindow.HeightValue.ToString(), Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed));

                    drNew["CallBoxID"] = uid.Replace("CALL_", "");
                    drNew["Color"] = Brushes.DarkGray.ToString();
                    drNew["Radius"] = callWindow.HeightValue.ToString();
                    break;
            }
            dtSource.Rows.Add(drNew);
        }

        /// 根据传入值取得颜色
        /// <summary>
        /// 根据传入值取得颜色
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        Color? GetColor(string val)
        {
            if (val.StartsWith("#"))
            {
                try
                {
                    if (val.Length == 9)
                    {
                        return Color.FromRgb((byte)int.Parse(val.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), (byte)int.Parse(val.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier), (byte)int.Parse(val.Substring(7, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                    return Color.FromRgb((byte)int.Parse(val.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), (byte)int.Parse(val.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), (byte)int.Parse(val.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else if (val.IndexOf(',') > 0)
            {
                try
                {
                    string[] array = val.Split(',');
                    return Color.FromRgb((byte)int.Parse(array[0]), (byte)int.Parse(array[1]), (byte)int.Parse(array[2]));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            try
            {
                System.Drawing.ColorConverter convert = new System.Drawing.ColorConverter();
                System.Drawing.Color color = (System.Drawing.Color)convert.ConvertFromString(val);
                return Color.FromRgb(color.R, color.G, color.B);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// 选择元素数据变更更新方法
        /// <summary>
        /// 选择元素数据变更更新方法
        /// </summary>
        void ChoiceDataChange()
        {
            string uid = itemSource[0].uid;
            Path path;
            Polygon polygon;
            Color? color;
            if (selectUiElement != null)
            {
                switch (selectType)
                {
                    case "A":
                        StationWindow stationWindow = (StationWindow)dicStationUc[uid];
                        stationWindow.numValue = itemSource[1].val;
                        break;
                    case "B":
                        path = dicStationLineUc[uid];
                        polygon = dicStationLineArrowUc[uid.Replace("STATIONLINE_", "STATIONARROWLINE_")];
                        color = GetColor(itemSource[0].val);
                        if (color != null)
                        {
                            brushPre = polygon.Stroke =
                                polygon.Fill =
                                    path.Stroke = new SolidColorBrush((Color)color);
                        }
                        break;
                    case "C":
                        path = dicstationCurveUc[uid];
                        polygon = dicstationCurveArrowUc[uid.Replace("STATIONCURVE_", "STATIONCURVEARROW_")];
                        color = GetColor(itemSource[0].val);
                        if (color != null)
                        {
                            brushPre = polygon.Stroke =
                                polygon.Fill =
                                    path.Stroke = new SolidColorBrush((Color)color);
                        }
                        break;
                    case "D":
                        path = (Path)dicLineUc[uid];
                        color = GetColor(itemSource[0].val);
                        if (color != null)
                        {
                            brushPre = path.Stroke = new SolidColorBrush((Color)color);
                        }
                        break;
                    case "E":
                        path = (Path)dicCurveUc[uid];
                        color = GetColor(itemSource[0].val);
                        if (color != null)
                        {
                            brushPre = path.Stroke = new SolidColorBrush((Color)color);
                        }
                        break;
                    case "F":
                        StoreWindow storeWindow = (StoreWindow)dicStoreUc[uid];
                        storeWindow.NumValue = itemSource[2].val;
                        storeWindow.Name = itemSource[3].val;
                        storeWindow.LankMarkCode = itemSource[1].val;
                        break;
                    case "G":
                        TextBlock txt = (TextBlock)dicFontUc[uid];
                        txt.Text = itemSource[2].val;
                        if (!string.IsNullOrEmpty(itemSource[1].val))
                        {
                            try
                            {
                                txt.FontSize = double.Parse(itemSource[1].val);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        color = GetColor(itemSource[0].val);
                        if (color != null)
                        {
                            brushPre = txt.Foreground = new SolidColorBrush((Color)color);
                        }
                        break;
                    case "H":
                        ImageWindow imageWindow = (ImageWindow)dicImageUc[uid];
                        if (!string.IsNullOrEmpty(itemSource[1].val))
                        {
                            try
                            {
                                imageWindow.HeightValue = double.Parse(itemSource[1].val);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        if (!string.IsNullOrEmpty(itemSource[5].val))
                        {
                            try
                            {
                                imageWindow.WidthValue = double.Parse(itemSource[5].val);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    case "I":
                        CallWindow callWindow = (CallWindow)dicCallUc[uid];
                        if (!string.IsNullOrEmpty(itemSource[2].val))
                        {
                            try
                            {
                                callWindow.HeightValue = double.Parse(itemSource[2].val);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (!string.IsNullOrEmpty(itemSource[0].val))
                        {
                            try
                            {
                                callWindow.CallBoxIdValue = itemSource[0].val;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                }
            }

            DataRow dataRows = dtSource.AsEnumerable().Where(p => p["Uid"].ToString() == uid).SingleOrDefault();
            if (dataRows != null)
            {
                foreach (Member m in itemSource)
                {
                    dataRows[m.property] = m.val;
                }
            }
        }

        /// 根据字符串得到点坐标
        /// <summary>
        /// 根据字符串得到点坐标
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        Point GetPoint(string val)
        {
            string[] array = val.Split(',');
            return new Point(double.Parse(array[0].Split('=')[1]), double.Parse(array[1].Split('=')[1].TrimEnd('}')));
        }

        /// 新建地图，打开地图时清除数据
        /// <summary>
        /// 新建地图，打开地图时清除数据
        /// </summary>
        void ClearData()
        {
            scaleLevel = 1;
            rabType = "";
            lineArrowCount = 1;
            imageCount = 1;
            callCount = 1;
            storeCount = 1;
            stationCount = 1;
            fontCount = 1;
            lineCount = 1;
            curveCount = 1;
            stationLineCount = 1;
            stationCurveCount = 1;
            isOneClick = false;
            dicImage.Clear();
            dicImageUc.Clear();
            dicCall.Clear();
            dicCallUc.Clear();
            dicStore.Clear();
            dicStoreUc.Clear();
            dicStation.Clear();
            dicStationUc.Clear();
            dicFont.Clear();
            dicFontUc.Clear();
            dicLineUc.Clear();
            dicCurveUc.Clear();
            dicStationLineUc.Clear();
            dicStationLineArrowUc.Clear();
            dicstationCurveUc.Clear();
            dicstationCurveArrowUc.Clear();
            dicOldPoint.Clear();
            dicCar.Clear();
            dicCarUc.Clear();
            lstStation.Clear();
            station1 = station2 = null;
            selectType = "";
            selectUiElement = selectUiElement2 = null;
            itemSource.Clear();
            if (dtSource != null && dtSource.Rows.Count > 0)
            {
                dtSource.Clear();
            }
            if (dtCarInfo != null && dtCarInfo.Rows.Count > 0)
            {
                dtCarInfo.Clear();
            }
            if (dtCarInfoOld != null && dtCarInfoOld.Rows.Count > 0)
            {
                dtCarInfoOld.Clear();
            }
        }

        /// 另存为方法
        /// <summary>
        /// 另存为方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool SaveAs(string fileName)
        {
            XmlElement xmlelem;
            XmlDocument xmldoc = new XmlDocument();
            ////加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            //XmlDeclaration xmldecl;
            //xmldecl = xml.CreateXmlDeclaration("1.0", "gb2312", null);
            //xml.AppendChild(xmldecl);

            //加入一个根元素
            xmlelem = xmldoc.CreateElement("", "CanvasDataModel", "");
            xmldoc.AppendChild(xmlelem);

            XmlNode root = xmldoc.SelectSingleNode("CanvasDataModel");
            //背景信息
            Color color = (Color)ColorConverter.ConvertFromString(canvasBrush.ToString());
            XmlElement xe1 = xmldoc.CreateElement("BackGroundLayer");
            XmlElement xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "Color");
            xe2.SetAttribute("value", color.R + "," + color.G + "," + color.B);
            xe1.AppendChild(xe2);
            root.AppendChild(xe1);

            //坐标信息
            xe1 = xmldoc.CreateElement("gridlayer");
            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "Spacing");
            xe2.SetAttribute("value", Scale.ToString());
            xe1.AppendChild(xe2);

            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "CoorCount");
            xe2.SetAttribute("value", coordinateCount.ToString());
            xe1.AppendChild(xe2);

            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "GridStyle");
            xe2.SetAttribute("value", displayMode);
            xe1.AppendChild(xe2);

            color = (Color)ColorConverter.ConvertFromString(coorBrush.ToString());
            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "Color");
            xe2.SetAttribute("value", color.R + "," + color.G + "," + color.B);
            xe1.AppendChild(xe2);

            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "Enabled");
            xe2.SetAttribute("value", IsShowCoordinate.ToString());
            xe1.AppendChild(xe2);
            root.AppendChild(xe1);

            //数据层
            color = (Color)ColorConverter.ConvertFromString(penBrush.ToString());
            xe1 = xmldoc.CreateElement("layer");
            xe1.SetAttribute("Id", "1");
            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "Color");
            xe2.SetAttribute("value", color.R + "," + color.G + "," + color.B);
            xe1.AppendChild(xe2);

            xe2 = xmldoc.CreateElement("property");
            xe2.SetAttribute("name", "PenSize");
            xe2.SetAttribute("value", penSize.ToString());
            xe1.AppendChild(xe2);

            xe2 = xmldoc.CreateElement("items");
            XmlElement xe3, xe4;

            #region 文字
            TextBlock txt;
            foreach (string key in dicFontUc.Keys)
            {
                txt = dicFontUc[key] as TextBlock;
                if (txt != null)
                {
                    xe3 = xmldoc.CreateElement("TextTool");
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "FontSize");
                    xe4.SetAttribute("value", txt.FontSize.ToString());
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "StrValue");
                    xe4.SetAttribute("value", txt.Text.ToString());
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Location");
                    xe4.SetAttribute("value", "{X=" + dicFont[txt.Uid].X + ",Y=" + dicFont[txt.Uid].Y + "}");
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(txt.Foreground.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 呼叫器
            CallWindow callWindow;
            foreach (string key in dicCallUc.Keys)
            {
                callWindow = dicCallUc[key] as CallWindow;
                if (callWindow != null)
                {
                    xe3 = xmldoc.CreateElement("CallWindow");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "CallBoxID");
                    xe4.SetAttribute("value", callWindow.CallBoxIdValue);
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Location");
                    xe4.SetAttribute("value", "{X=" + dicCall[callWindow.Uid].X + ",Y=" + dicCall[callWindow.Uid].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Radius");
                    xe4.SetAttribute("value", callWindow.HeightValue.ToString());
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(callWindow.BackgroundValue.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 地标
            StationWindow stationWindow;
            DataRow[] dataRows;
            foreach (string key in dicStationUc.Keys)
            {
                stationWindow = dicStationUc[key] as StationWindow;
                if (stationWindow != null)
                {
                    dataRows = dtSource.Select(string.Format("Uid='{0}'", stationWindow.Uid));
                    xe3 = xmldoc.CreateElement("LandMark");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Location");
                    xe4.SetAttribute("value", "{X=" + dicStation[stationWindow.Uid].X + ",Y=" + dicStation[stationWindow.Uid].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "LandCode");
                    xe4.SetAttribute("value", stationWindow.numValue);
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "LandName");
                    xe4.SetAttribute("value", dataRows.Length > 0 ? dataRows[0]["LandName"].ToString() : stationWindow.Name);
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(Brushes.Coral.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 储位
            StoreWindow storeWindow;
            foreach (string key in dicStoreUc.Keys)
            {
                storeWindow = dicStoreUc[key] as StoreWindow;
                if (storeWindow != null)
                {
                    dataRows = dtSource.Select(string.Format("Uid='{0}'", storeWindow.Uid));
                    xe3 = xmldoc.CreateElement("StorageTool");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Location");
                    xe4.SetAttribute("value", "{X=" + dicStore[storeWindow.Uid].X + ",Y=" + dicStore[storeWindow.Uid].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "StcokID");
                    xe4.SetAttribute("value", storeWindow.NumValue);
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "LankMarkCode");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["LankMarkCode"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "");
                    }

                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "StorageName");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["StorageName"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", storeWindow.Name);
                    }
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(storeWindow.BackgroundValue.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 图片
            ImageWindow imageWindow;
            FileStream fs;
            BinaryReader br;
            byte[] imageBuffer;
            FileInfo file;
            foreach (string key in dicImageUc.Keys)
            {
                imageWindow = dicImageUc[key] as ImageWindow;
                if (imageWindow != null)
                {
                    dataRows = dtSource.Select(string.Format("Uid='{0}'", imageWindow.Uid));
                    xe3 = xmldoc.CreateElement("ImgeTool");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Location");
                    xe4.SetAttribute("value", "{X=" + dicImage[imageWindow.Uid].X + ",Y=" + dicImage[imageWindow.Uid].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ImageStr");
                    if (dataRows.Length > 0 && !string.IsNullOrEmpty(dataRows[0]["ImageUrl"].ToString()))
                    {
                        fs = new FileStream(dataRows[0]["ImageUrl"].ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        file = new FileInfo(dataRows[0]["ImageUrl"].ToString());
                        xe4.SetAttribute("imageName", file.Name);
                    }
                    else
                    {
                        fs = new FileStream(System.Windows.Forms.Application.StartupPath + @"\Image\goods.png", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        xe4.SetAttribute("imageName", "goods.png");
                    }
                    br = new BinaryReader(fs);
                    imageBuffer = new byte[br.BaseStream.Length];
                    br.Read(imageBuffer, 0, Convert.ToInt32(br.BaseStream.Length));
                    xe4.SetAttribute("value", System.Convert.ToBase64String(imageBuffer));
                    fs.Close();
                    br.Close();
                    fs.Dispose();
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "TransColor");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["TransColor"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "Transparent");
                    }

                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Height");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["Height"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", imageWindow.HeightValue.ToString());
                    }
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Width");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["Width"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", imageWindow.WidthValue.ToString());
                    }
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(imageWindow.BackgroundValue.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 直线
            Path path;
            LineGeometry lineGeometry;
            foreach (string key in dicLineUc.Keys)
            {
                path = dicLineUc[key] as Path;
                if (path != null)
                {
                    lineGeometry = path.Data as LineGeometry;
                    dataRows = dtSource.Select(string.Format("Uid='{0}'", path.Uid));

                    xe3 = xmldoc.CreateElement("Line");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Lenth");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["Length"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "1");
                    }
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Thickness");
                    xe4.SetAttribute("value", path.StrokeThickness.ToString());
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P1");
                    xe4.SetAttribute("value", "{X=" + lineGeometry.StartPoint.X + ",Y=" + lineGeometry.StartPoint.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P2");
                    xe4.SetAttribute("value", "{X=" + lineGeometry.EndPoint.X + ",Y=" + lineGeometry.EndPoint.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ExcuteAngle");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["ExcuteAngle"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "1");
                    }
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(path.Stroke.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 曲线
            BezierSegment bezierSegment;
            PathFigure pathFigure;
            foreach (string key in dicCurveUc.Keys)
            {
                path = dicCurveUc[key] as Path;
                if (path != null)
                {
                    pathFigure = ((PathGeometry)path.Data).Figures[0];
                    bezierSegment = (BezierSegment)pathFigure.Segments[0];

                    dataRows = dtSource.Select(string.Format("Uid='{0}'", path.Uid));

                    xe3 = xmldoc.CreateElement("CurveLine");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Lenth");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["Length"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "0");
                    }
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Thickness");
                    xe4.SetAttribute("value", path.StrokeThickness.ToString());
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P1");
                    xe4.SetAttribute("value", "{X=" + pathFigure.StartPoint.X + ",Y=" + pathFigure.StartPoint.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P2");
                    xe4.SetAttribute("value", "{X=" + bezierSegment.Point3.X + ",Y=" + bezierSegment.Point3.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P3");
                    xe4.SetAttribute("value", "{X=" + bezierSegment.Point1.X + ",Y=" + bezierSegment.Point1.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P4");
                    xe4.SetAttribute("value", "{X=" + bezierSegment.Point2.X + ",Y=" + bezierSegment.Point2.Y + "}");
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(path.Stroke.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 地标直线
            Polygon polygon;
            foreach (string key in dicStationLineUc.Keys)
            {
                path = dicStationLineUc[key] as Path;
                if (path != null)
                {
                    lineGeometry = path.Data as LineGeometry;
                    dataRows = dtSource.Select(string.Format("Uid='{0}'", path.Uid));

                    xe3 = xmldoc.CreateElement("LandLine");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Lenth");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["Length"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "1");
                    }
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Thickness");
                    xe4.SetAttribute("value", path.StrokeThickness.ToString());
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P1");
                    xe4.SetAttribute("value", "{X=" + lineGeometry.StartPoint.X + ",Y=" + lineGeometry.StartPoint.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P2");
                    xe4.SetAttribute("value", "{X=" + lineGeometry.EndPoint.X + ",Y=" + lineGeometry.EndPoint.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ExcuteAngle");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["ExcuteAngle"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "1");
                    }
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(path.Stroke.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    polygon = (Polygon)dicStationLineArrowUc[key.Replace("STATIONLINE_", "STATIONARROWLINE_")];
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ArrowP1");
                    xe4.SetAttribute("value", "{X=" + polygon.Points[0].X + ",Y=" + polygon.Points[0].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ArrowP2");
                    xe4.SetAttribute("value", "{X=" + polygon.Points[1].X + ",Y=" + polygon.Points[1].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ArrowP3");
                    xe4.SetAttribute("value", "{X=" + polygon.Points[2].X + ",Y=" + polygon.Points[2].Y + "}");
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            #region 地标曲线
            foreach (string key in dicstationCurveUc.Keys)
            {
                path = dicstationCurveUc[key] as Path;
                if (path != null)
                {
                    pathFigure = ((PathGeometry)path.Data).Figures[0];
                    bezierSegment = (BezierSegment)pathFigure.Segments[0];

                    dataRows = dtSource.Select(string.Format("Uid='{0}'", path.Uid));

                    xe3 = xmldoc.CreateElement("LandCurveLine");

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Lenth");
                    if (dataRows.Length > 0)
                    {
                        xe4.SetAttribute("value", dataRows[0]["Length"].ToString());
                    }
                    else
                    {
                        xe4.SetAttribute("value", "0");
                    }
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Thickness");
                    xe4.SetAttribute("value", path.StrokeThickness.ToString());
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P1");
                    xe4.SetAttribute("value", "{X=" + pathFigure.StartPoint.X + ",Y=" + pathFigure.StartPoint.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P2");
                    xe4.SetAttribute("value", "{X=" + bezierSegment.Point3.X + ",Y=" + bezierSegment.Point3.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P3");
                    xe4.SetAttribute("value", "{X=" + bezierSegment.Point1.X + ",Y=" + bezierSegment.Point1.Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "P4");
                    xe4.SetAttribute("value", "{X=" + bezierSegment.Point2.X + ",Y=" + bezierSegment.Point2.Y + "}");
                    xe3.AppendChild(xe4);

                    color = (Color)ColorConverter.ConvertFromString(path.Stroke.ToString());
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "Color");
                    xe4.SetAttribute("value", color.R + "," + color.G + "," + color.B);
                    xe3.AppendChild(xe4);

                    polygon = (Polygon)dicstationCurveArrowUc[key.Replace("STATIONCURVE_", "STATIONCURVEARROW_")];
                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ArrowP1");
                    xe4.SetAttribute("value", "{X=" + polygon.Points[0].X + ",Y=" + polygon.Points[0].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ArrowP2");
                    xe4.SetAttribute("value", "{X=" + polygon.Points[1].X + ",Y=" + polygon.Points[1].Y + "}");
                    xe3.AppendChild(xe4);

                    xe4 = xmldoc.CreateElement("property");
                    xe4.SetAttribute("name", "ArrowP3");
                    xe4.SetAttribute("value", "{X=" + polygon.Points[2].X + ",Y=" + polygon.Points[2].Y + "}");
                    xe3.AppendChild(xe4);

                    xe2.AppendChild(xe3);
                }
            }
            #endregion

            xe1.AppendChild(xe2);
            root.AppendChild(xe1);

            //保存创建好的XML文档
            xmldoc.Save(fileName);
            return true;
        }

        /// 打开地图方法
        /// <summary>
        /// 打开地图方法
        /// </summary>
        /// <returns></returns>
        bool OpenMap(string filePathName)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filePathName);
            XmlElement root = xmldoc.DocumentElement;

            if (root.HasChildNodes)
            {
                XmlNodeList childlist = root.ChildNodes;
                XmlNodeList childNodeList;
                XmlNodeList ueNodeList;
                string fontSize = "";
                string text = "";
                Point point = new Point(0, 0);
                Brush color = penBrush;
                string val;
                double height = 0;
                double width = 0;
                string landCode = "";
                string landName = "";
                string tranColor = "";
                string imageStr = "";
                byte[] picByte = null;
                System.IO.MemoryStream ms = null;
                System.Drawing.Image image = null;
                string imageName = "";
                string fileName = "";
                LineGeometry lineGeometry;
                Path path;
                BezierSegment bezierSegment;
                PathFigure pathFigure;
                PathGeometry pathGeometry;
                Polygon polygon;

                Dictionary<Point, string> dicStationTemp = new Dictionary<Point, string>();//点与地标uid关系集合

                foreach (XmlNode node in childlist)
                {
                    if (node.Name == "BackGroundLayer")
                    {
                        if (node.HasChildNodes)
                        {
                            childNodeList = node.ChildNodes;
                            foreach (XmlNode nd in childNodeList)
                            {
                                if (((XmlElement)nd).GetAttribute("name") == "Color")
                                {
                                    val = ((XmlElement)nd).GetAttribute("value");
                                    GridMain.Background = canvasBrush = new SolidColorBrush((Color)GetColor(val));
                                    break;
                                }
                            }
                        }
                    }
                    else if (node.Name == "gridlayer")
                    {
                        if (node.HasChildNodes)
                        {
                            childNodeList = node.ChildNodes;
                            foreach (XmlNode nd in childNodeList)
                            {
                                if (((XmlElement)nd).GetAttribute("name") == "Color")
                                {
                                    val = ((XmlElement)nd).GetAttribute("value");
                                    coorBrush = new SolidColorBrush((Color)GetColor(val));
                                }
                                if (((XmlElement)nd).GetAttribute("name") == "Spacing")
                                {
                                    val = ((XmlElement)nd).GetAttribute("value");
                                    Scale = int.Parse(val);
                                }
                                if (((XmlElement)nd).GetAttribute("name") == "CoorCount")
                                {
                                    val = ((XmlElement)nd).GetAttribute("value");
                                    coordinateCount = int.Parse(val);
                                }
                                if (((XmlElement)nd).GetAttribute("name") == "GridStyle")
                                {
                                    val = ((XmlElement)nd).GetAttribute("value");
                                    displayMode = val;
                                }
                                if (((XmlElement)nd).GetAttribute("name") == "Enabled")
                                {
                                    val = ((XmlElement)nd).GetAttribute("value");
                                    IsShowCoordinate = bool.Parse(val);
                                }
                            }
                        }
                        InitCenter();
                    }
                    else if (node.Name == "layer")
                    {
                        if (node.HasChildNodes)
                        {
                            childNodeList = node.SelectSingleNode("items").ChildNodes;
                            foreach (XmlNode nd in childNodeList)
                            {
                                #region 文字
                                if (nd.Name == "TextTool")
                                {
                                    ueNodeList = nd.ChildNodes;

                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        if (((XmlElement)u).GetAttribute("name") == "FontSize")
                                        {
                                            fontSize = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "StrValue")
                                        {
                                            text = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Location")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            point = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            color = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    GeneralFont(point, text, color, double.Parse(fontSize));
                                }
                                #endregion

                                #region 呼叫器
                                if (nd.Name == "CallWindow")
                                {
                                    ueNodeList = nd.ChildNodes;
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        if (((XmlElement)u).GetAttribute("name") == "CallBoxID")
                                        {
                                            text = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Radius")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            width = double.Parse(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Location")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            point = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            color = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    GeneralCall(point, text, width, color);
                                }
                                #endregion

                                #region 地标
                                if (nd.Name == "LandMark")
                                {
                                    ueNodeList = nd.ChildNodes;
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        if (((XmlElement)u).GetAttribute("name") == "LandCode")
                                        {
                                            landCode = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "LandName")
                                        {
                                            landName = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Location")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            point = GetPoint(val);
                                            point = new Point(point.X + 10, point.Y + 30);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            color = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    dicStationTemp[point] = "STATION_" + stationCount;
                                    GeneralStation(point, landCode, landName, color);
                                }
                                #endregion

                                #region 储位
                                if (nd.Name == "StorageTool")
                                {
                                    ueNodeList = nd.ChildNodes;
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        if (((XmlElement)u).GetAttribute("name") == "StcokID")
                                        {
                                            landCode = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "LankMarkCode")
                                        {
                                            landName = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "StorageName")
                                        {
                                            text = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Location")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            point = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            color = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    GeneralStore(point, landCode, landName, text, null);
                                }
                                #endregion

                                #region 图片
                                if (nd.Name == "ImgeTool")
                                {
                                    ueNodeList = nd.ChildNodes;
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        if (((XmlElement)u).GetAttribute("name") == "ImageStr")
                                        {
                                            imageName = ((XmlElement)u).GetAttribute("imageName");
                                            imageStr = ((XmlElement)u).GetAttribute("value");
                                            picByte = Convert.FromBase64String(imageStr);//转化为byte[]
                                            ms = new MemoryStream();
                                            ms.Write(picByte, 0, picByte.Length);//写到流中
                                            image = System.Drawing.Image.FromStream(ms, true);
                                            ms.Close();
                                            fileName = System.Windows.Forms.Application.StartupPath +
                                                        @"\Image\" + imageName;
                                            if (!File.Exists(fileName))
                                            {
                                                image.Save(fileName);
                                                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                                            }
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "TransColor")
                                        {
                                            tranColor = ((XmlElement)u).GetAttribute("value");
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Height")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            height = double.Parse(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Width")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            width = double.Parse(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Location")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            point = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            color = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    GeneralImage(point, height, width, imageName == "goods.png" ? null : fileName);
                                }
                                #endregion

                                #region 直线
                                if (nd.Name == "Line")
                                {
                                    ueNodeList = nd.ChildNodes;

                                    lineGeometry = new LineGeometry();
                                    path = new Path();
                                    path.Data = lineGeometry;
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        //if (((XmlElement)u).GetAttribute("name") == "Lenth")
                                        //{
                                        //    fontSize = ((XmlElement)u).GetAttribute("value");
                                        //}
                                        if (((XmlElement)u).GetAttribute("name") == "Thickness")
                                        {
                                            path.StrokeThickness = double.Parse(((XmlElement)u).GetAttribute("value"));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P1")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            lineGeometry.StartPoint = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P2")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            lineGeometry.EndPoint = GetPoint(val);
                                        }
                                        //if (((XmlElement)u).GetAttribute("name") == "ExcuteAngle")
                                        //{
                                        //    val = ((XmlElement)u).GetAttribute("value");
                                        //    point = GetPoint(val);
                                        //}
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            path.Fill = path.Stroke = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    //if (tfGroup != null)
                                    //{
                                    //    path.RenderTransform = tfGroup;
                                    //}
                                    CanvasMain.Children.Add(path);
                                    path.Uid = "LINENEW_" + lineCount++;
                                    path.MouseLeftButtonDown += lastLine_MouseLeftButtonDown;
                                    dicLineUc[path.Uid] = path;
                                }
                                #endregion

                                #region 曲线
                                if (nd.Name == "CurveLine")
                                {
                                    ueNodeList = nd.ChildNodes;

                                    bezierSegment = new BezierSegment();
                                    pathGeometry = new PathGeometry();
                                    pathFigure = new PathFigure();
                                    path = new Path();
                                    path.Data = pathGeometry;
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        //if (((XmlElement)u).GetAttribute("name") == "Lenth")
                                        //{
                                        //    fontSize = ((XmlElement)u).GetAttribute("value");
                                        //}
                                        if (((XmlElement)u).GetAttribute("name") == "Thickness")
                                        {
                                            path.StrokeThickness = double.Parse(((XmlElement)u).GetAttribute("value"));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P1")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            pathFigure.StartPoint = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P2")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            bezierSegment.Point3 = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P3")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            bezierSegment.Point1 = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P4")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            bezierSegment.Point2 = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            path.Stroke = new SolidColorBrush((Color)GetColor(val));
                                        }
                                    }
                                    bezierSegment.IsStroked = true;
                                    pathFigure.Segments.Add(bezierSegment);
                                    pathGeometry.Figures.Add(pathFigure);
                                    //if (tfGroup != null)
                                    //{
                                    //    path.RenderTransform = tfGroup;
                                    //}
                                    CanvasMain.Children.Add(path);
                                    path.Uid = "CURVENEW_" + curveCount++;
                                    path.MouseDown += lastLine_MouseLeftButtonDown;
                                    dicCurveUc[path.Uid] = path;
                                }
                                #endregion

                                #region 地标直线
                                if (nd.Name == "LandLine")
                                {
                                    ueNodeList = nd.ChildNodes;

                                    lineGeometry = new LineGeometry();
                                    path = new Path();
                                    path.Data = lineGeometry;
                                    polygon = new Polygon();
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        //if (((XmlElement)u).GetAttribute("name") == "Lenth")
                                        //{
                                        //    fontSize = ((XmlElement)u).GetAttribute("value");
                                        //}
                                        if (((XmlElement)u).GetAttribute("name") == "Thickness")
                                        {
                                            polygon.StrokeThickness = path.StrokeThickness = double.Parse(((XmlElement)u).GetAttribute("value"));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P1")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            lineGeometry.StartPoint = GetPoint(val);
                                            if (dicStationTemp.Keys.Contains(lineGeometry.StartPoint))
                                            {
                                                if (dicStationUc.Keys.Contains(dicStationTemp[lineGeometry.StartPoint]))
                                                {
                                                    station1 =
                                                        dicStationUc[dicStationTemp[lineGeometry.StartPoint]] as
                                                            StationWindow;
                                                }
                                            }
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P2")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            lineGeometry.EndPoint = GetPoint(val);
                                            if (dicStationTemp.Keys.Contains(lineGeometry.EndPoint))
                                            {
                                                if (dicStationUc.Keys.Contains(dicStationTemp[lineGeometry.EndPoint]))
                                                {
                                                    station2 =
                                                        dicStationUc[dicStationTemp[lineGeometry.EndPoint]] as
                                                            StationWindow;
                                                }
                                            }
                                        }
                                        //if (((XmlElement)u).GetAttribute("name") == "ExcuteAngle")
                                        //{
                                        //    val = ((XmlElement)u).GetAttribute("value");
                                        //    point = GetPoint(val);
                                        //}
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Fill = polygon.Stroke = path.Fill = path.Stroke = new SolidColorBrush((Color)GetColor(val));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "ArrowP1")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Points.Add(GetPoint(val));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "ArrowP2")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Points.Add(GetPoint(val));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "ArrowP3")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Points.Add(GetPoint(val));
                                        }
                                    }
                                    //if (tfGroup != null)
                                    //{
                                    //    polygon.RenderTransform =path.RenderTransform = tfGroup;
                                    //}
                                    CanvasMain.Children.Add(path);
                                    CanvasMain.Children.Add(polygon);

                                    path.Uid = "STATIONLINE_" + stationLineCount;
                                    polygon.Uid = "STATIONARROWLINE_" + stationLineCount++;
                                    path.MouseDown += lastPath_MouseDown;
                                    polygon.MouseDown += lastPath_MouseDown;
                                    
                                    if (station1 != null && station2 != null && !lstStation.Contains(station1.Uid + "@" + station2.Uid))
                                    {
                                        lstStation.Add(station1.Uid + "@" + station2.Uid);
                                    }
                                    station1 = station2 = null;

                                    dicStationLineUc[path.Uid] = path;
                                    dicStationLineArrowUc[polygon.Uid] = polygon;
                                }
                                #endregion

                                #region 地标曲线
                                if (nd.Name == "LandCurveLine")
                                {
                                    ueNodeList = nd.ChildNodes;

                                    bezierSegment = new BezierSegment();
                                    pathGeometry = new PathGeometry();
                                    pathFigure = new PathFigure();
                                    path = new Path();
                                    path.Data = pathGeometry;
                                    polygon = new Polygon();
                                    foreach (XmlNode u in ueNodeList)
                                    {
                                        //if (((XmlElement)u).GetAttribute("name") == "Lenth")
                                        //{
                                        //    fontSize = ((XmlElement)u).GetAttribute("value");
                                        //}
                                        if (((XmlElement)u).GetAttribute("name") == "Thickness")
                                        {
                                            polygon.StrokeThickness = path.StrokeThickness = double.Parse(((XmlElement)u).GetAttribute("value"));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P1")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            pathFigure.StartPoint = GetPoint(val);
                                            if (dicStationTemp.Keys.Contains(pathFigure.StartPoint))
                                            {
                                                if (dicStationUc.Keys.Contains(dicStationTemp[pathFigure.StartPoint]))
                                                {
                                                    station1 =
                                                        dicStationUc[dicStationTemp[pathFigure.StartPoint]] as
                                                            StationWindow;
                                                }
                                            }
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P2")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            bezierSegment.Point3 = GetPoint(val);
                                            if (dicStationTemp.Keys.Contains(bezierSegment.Point3))
                                            {
                                                if (dicStationUc.Keys.Contains(dicStationTemp[bezierSegment.Point3]))
                                                {
                                                    station2 =
                                                        dicStationUc[dicStationTemp[bezierSegment.Point3]] as
                                                            StationWindow;
                                                }
                                            }
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P3")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            bezierSegment.Point1 = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "P4")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            bezierSegment.Point2 = GetPoint(val);
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "Color")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Fill = polygon.Stroke = path.Stroke = new SolidColorBrush((Color)GetColor(val));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "ArrowP1")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Points.Add(GetPoint(val));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "ArrowP2")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Points.Add(GetPoint(val));
                                        }
                                        if (((XmlElement)u).GetAttribute("name") == "ArrowP3")
                                        {
                                            val = ((XmlElement)u).GetAttribute("value");
                                            polygon.Points.Add(GetPoint(val));
                                        }
                                    }
                                    bezierSegment.IsStroked = true;
                                    pathFigure.Segments.Add(bezierSegment);
                                    pathGeometry.Figures.Add(pathFigure);
                                    //if (tfGroup != null)
                                    //{
                                    //    polygon.RenderTransform=path.RenderTransform = tfGroup;
                                    //}
                                    CanvasMain.Children.Add(path);
                                    CanvasMain.Children.Add(polygon);

                                    path.Uid = "STATIONCURVE_" + stationCurveCount;
                                    polygon.Uid = "STATIONCURVEARROW_" + stationCurveCount++;
                                    path.MouseDown += lastCurve_MouseDown;
                                    polygon.MouseDown += lastCurve_MouseDown;

                                    if (station1 != null && station2 != null && !lstStation.Contains(station1.Uid + "@" + station2.Uid))
                                    {
                                        lstStation.Add(station1.Uid + "@" + station2.Uid);
                                    }
                                    station1 = station2 = null;

                                    dicstationCurveUc[path.Uid] = path;
                                    dicstationCurveArrowUc[polygon.Uid] = polygon;
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
