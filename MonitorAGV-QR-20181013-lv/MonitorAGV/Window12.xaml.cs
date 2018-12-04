using MonitorAGV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Window12.xaml 的交互逻辑
    /// </summary>
    public partial class Window12 : Window
    {
        public Window12()
        {
            InitializeComponent();
            CanvasAGV agv = new CanvasAGV();
            agv.GetAGVCanvas(25, 0, "1号", this.myCanvas);
        }
        ContextMenu contextMenu = new ContextMenu();
        //public void DrawAGV(int init_x, int init_y, string AGVNo)
        //{
        //    RectangleGeometry myRectangleGeometry = new RectangleGeometry();
        //    myRectangleGeometry.Rect = new Rect(init_x + 0, init_y + 0, 60, 40);
        //    Path myPath = new Path();
        //    myPath.Fill = Brushes.LemonChiffon;
        //    myPath.Stroke = Brushes.Black;
        //    myPath.StrokeThickness = 1;
        //    myPath.Data = myRectangleGeometry;
        //    this.myCanvas.Children.Add(myPath);


        //    EllipseGeometry myEllipseGeometry = new EllipseGeometry(new Point(init_x + 30, 20), 20, 18);
        //    Path myPath2 = new Path();
        //    myPath2.Fill = Brushes.LemonChiffon;
        //    myPath2.Stroke = Brushes.Black;
        //    myPath2.StrokeThickness = 1;
        //    myPath2.Data = myEllipseGeometry;
        //    this.myCanvas.Children.Add(myPath2);


        //    ArcSegment arc = new ArcSegment(new Point(init_x, 40), new Size(15, 25), 0, false, SweepDirection.Counterclockwise, true);
        //    Path myPath3 = new Path();
        //    PathGeometry pathGeometry = new PathGeometry();
        //    PathFigure figure = new PathFigure();
        //    figure.StartPoint = new Point(init_x, init_y);
        //    figure.Segments.Add(arc);
        //    pathGeometry.Figures.Add(figure);
        //    myPath3.Data = pathGeometry;
        //    myPath3.Stroke = Brushes.Black;
        //    myPath3.Fill = Brushes.LemonChiffon;
        //    myPath2.StrokeThickness = 1;
        //    myCanvas.Children.Add(myPath3);


        //    ArcSegment arc_right = new ArcSegment(new Point(init_x + 60, 0), new Size(15, 25), 0, false, SweepDirection.Counterclockwise, true);
        //    Path myPath4 = new Path();
        //    PathGeometry pathGeometry2 = new PathGeometry();
        //    PathFigure figure2 = new PathFigure();
        //    figure2.StartPoint = new Point(init_x + 60, init_y + 40);
        //    figure2.Segments.Add(arc_right);
        //    pathGeometry2.Figures.Add(figure2);
        //    myPath4.Data = pathGeometry2;
        //    myPath4.Stroke = Brushes.Black;
        //    myPath4.Fill = Brushes.LemonChiffon;
        //    myPath2.StrokeThickness = 1;
        //    myCanvas.Children.Add(myPath4);


        //    TextBlock textBlock = new TextBlock();
        //    textBlock.Text = AGVNo;
        //    textBlock.Foreground = new SolidColorBrush(Colors.Red);
        //    Canvas.SetLeft(textBlock, init_x + 30 - 9);
        //    Canvas.SetTop(textBlock, init_y + 20 - 7);
        //    myCanvas.Children.Add(textBlock);




        //    PathGeometry p = PathGeometry.Combine(myRectangleGeometry, myEllipseGeometry, GeometryCombineMode.Union, null);
        //    PathGeometry p2 = PathGeometry.Combine(p, pathGeometry, GeometryCombineMode.Union, null);
        //    PathGeometry p_final = PathGeometry.Combine(p2, pathGeometry2, GeometryCombineMode.Union, null);




        //    //Rectangle AGV;

        //    //AGV = new Rectangle() { ContextMenu = contextMenu };
        //    //AGV.Height = 24;
        //    //AGV.Width = 28;
        //    //AGV.Stroke = new SolidColorBrush(Colors.Black);
        //    //AGV.StrokeThickness = 5;
        //    //AGV.Fill = new SolidColorBrush(Colors.Transparent);
        //    //Canvas.SetLeft(AGV, 10);
        //    //Canvas.SetTop(AGV, 10);
        //    //try
        //    //{

        //    //    this.myCanvas.Children.Add(AGV);


        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}



        //    //Path arrow = new Path();
        //    //arrow.Fill = Brushes.White;

        //    //PathFigure pathFigure = new PathFigure();
        //    //pathFigure.IsClosed = true;

        //    //pathFigure.StartPoint = new Point(10, 10);//路径的起点
        //    //pathFigure.Segments.Add(new LineSegment(new Point(10, 40), false));
        //    //pathFigure.Segments.Add(new LineSegment(new Point(60, 40), false));
        //    //pathFigure.Segments.Add(new LineSegment(new Point(60, 10), false));

        //    //PathGeometry pathGeometry = new PathGeometry();
        //    //pathGeometry.Figures.Add(pathFigure);
        //    //arrow.Data = pathGeometry;

        //    //this.myCanvas.Children.Add(arrow);
        //}
    }
}
