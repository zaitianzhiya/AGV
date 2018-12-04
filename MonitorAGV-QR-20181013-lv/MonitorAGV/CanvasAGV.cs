using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MonitorAGV
{
    public class CanvasAGV
    {
        public Canvas GetAGVCanvas(int init_x, int init_y, string remark, Canvas myCanvas)
        {
            RectangleGeometry myRectangleGeometry = new RectangleGeometry();
            myRectangleGeometry.Rect = new Rect(init_x + 0, init_y + 0, 60, 40);
            Path myPath = new Path();
            myPath.Fill = Brushes.LemonChiffon;
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myRectangleGeometry;
            myCanvas.Children.Add(myPath);


            EllipseGeometry myEllipseGeometry = new EllipseGeometry(new Point(init_x + 30, 20), 20, 18);
            Path myPath2 = new Path();
            myPath2.Fill = Brushes.LemonChiffon;
            myPath2.Stroke = Brushes.Black;
            myPath2.StrokeThickness = 1;
            myPath2.Data = myEllipseGeometry;
            myCanvas.Children.Add(myPath2);


            ArcSegment arc = new ArcSegment(new Point(init_x, 40), new Size(15, 25), 0, false, SweepDirection.Counterclockwise, true);
            Path myPath3 = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(init_x, init_y);
            figure.Segments.Add(arc);
            pathGeometry.Figures.Add(figure);
            myPath3.Data = pathGeometry;
            myPath3.Stroke = Brushes.Black;
            myPath3.Fill = Brushes.LemonChiffon;
            myPath2.StrokeThickness = 1;
            myCanvas.Children.Add(myPath3);


            ArcSegment arc_right = new ArcSegment(new Point(init_x + 60, 0), new Size(15, 25), 0, false, SweepDirection.Counterclockwise, true);
            Path myPath4 = new Path();
            PathGeometry pathGeometry2 = new PathGeometry();
            PathFigure figure2 = new PathFigure();
            figure2.StartPoint = new Point(init_x + 60, init_y + 40);
            figure2.Segments.Add(arc_right);
            pathGeometry2.Figures.Add(figure2);
            myPath4.Data = pathGeometry2;
            myPath4.Stroke = Brushes.Black;
            myPath4.Fill = Brushes.LemonChiffon;
            myPath2.StrokeThickness = 1;
            myCanvas.Children.Add(myPath4);


            TextBlock textBlock = new TextBlock();
            textBlock.Text = remark;
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(textBlock, init_x + 30 - 9);
            Canvas.SetTop(textBlock, init_y + 20 - 7);
            myCanvas.Children.Add(textBlock);

            return myCanvas;
        }

        public Canvas GetAGVCanvas(double init_x, double init_y,double height,double width, string remark, Canvas myCanvas, SolidColorBrush color, RotateTransform rotateTransform)
        {
            RectangleGeometry myRectangleGeometry = new RectangleGeometry();
            myRectangleGeometry.Rect = new Rect(init_x, init_y, width, height);
            Path myPath = new Path();
            myPath.Uid = "AGV_" + remark+"_01";
            myPath.Fill = Brushes.LemonChiffon;
            myPath.Stroke = color;
            myPath.StrokeThickness = 1;
            myPath.Data = myRectangleGeometry;
            myPath.RenderTransform = rotateTransform; 
            myCanvas.Children.Add(myPath);

            EllipseGeometry myEllipseGeometry = new EllipseGeometry(new Point(init_x + width / 2, init_y + height / 2), height / 2, height / 2 - 1);
            Path myPath2 = new Path();
            myPath2.Uid = "AGV_" + remark + "_02";
            myPath2.Fill = Brushes.LemonChiffon;
            myPath2.Stroke = color;
            myPath2.StrokeThickness = 1;
            myPath2.Data = myEllipseGeometry;
            myCanvas.Children.Add(myPath2);

            ArcSegment arc = new ArcSegment(new Point(init_x, init_y + height), new Size(7.5, 12.5), 0, false, SweepDirection.Counterclockwise, true);
            Path myPath3 = new Path();
            myPath3.Uid = "AGV_" + remark + "_03";
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(init_x, init_y);
            figure.Segments.Add(arc);
            pathGeometry.Figures.Add(figure);
            myPath3.Data = pathGeometry;
            myPath3.Stroke = color;
            myPath3.Fill = Brushes.LemonChiffon;
            myPath3.StrokeThickness = 1;
            myPath3.RenderTransform = rotateTransform;
            myCanvas.Children.Add(myPath3);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = remark;
            textBlock.Uid = "AGV_" + remark + "_05";
            textBlock.Foreground = new SolidColorBrush(Colors.Red);
            if (remark.Length == 1)
            {
                Canvas.SetLeft(textBlock, init_x + width / 2 - 4);
            }
            else
            {
                Canvas.SetLeft(textBlock, init_x + width / 2 - 7);
            }
            Canvas.SetTop(textBlock, init_y + height / 2 - 7);
            myCanvas.Children.Add(textBlock);

            return myCanvas;
        }
    }
}
