using System;
using System.Collections.Generic;
using System.Data;
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
    /// Window11.xaml 的交互逻辑
    /// </summary>
    public partial class Window11 : Window
    {
        public static bool now_A = true;
        public static bool now_B = true;
        public static bool now_C = true;
        public static bool now_D = true;
        public static bool now_E = true;
        public static bool now_F = true;
        public static bool now_G = true;
        public static bool now_H = true;

        private double X {set;get;}
        private double Y { set; get; }


        public Window11(double x,double y)
        {
            InitializeComponent();
            this.X = x;
            this.Y = y;

           // this.Name = "Direction: Point(" + X + "," + Y + ")";

            //获取数据库中当前方向禁用情况
            DataTable dt_map = Function.PR_Read_Map_FQ(2);//pr
            if (dt_map != null && dt_map.Rows.Count > 0)
            {
                for (int j = 0; j < dt_map.Rows.Count; j++)
                {
                    now_A = true;//
                    now_B = true;
                    now_C = true;
                    now_D = true;
                    now_E = true;
                    now_F = true;
                    now_G = true;
                    now_H = true;
                }
            }

            DrawArrowPath(5, 5);
        }

        //private void A_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_A = !now_A;
        //    if (now_A)
        //    {
        //        A.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        A.Source = new BitmapImage(new Uri(@"/Images/5.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void B_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_B = !now_B;
        //    if (now_B)
        //    {
        //        B.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        B.Source = new BitmapImage(new Uri(@"/Images/6.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void C_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_C = !now_C;
        //    if (now_C)
        //    {
        //        C.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        C.Source = new BitmapImage(new Uri(@"/Images/7.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void D_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_D = !now_D;
        //    if (now_D)
        //    {
        //        D.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        D.Source = new BitmapImage(new Uri(@"/Images/4.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void E_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_E = !now_E;
        //    if (now_E)
        //    {
        //        E.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        E.Source = new BitmapImage(new Uri(@"/Images/4.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void F_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_F = !now_F;
        //    if (now_F)
        //    {
        //        F.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        F.Source = new BitmapImage(new Uri(@"/Images/4.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void G_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_G = !now_G;
        //    if (now_G)
        //    {
        //        G.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        G.Source = new BitmapImage(new Uri(@"/Images/4.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}

        //private void H_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    now_H = !now_H;
        //    if (now_H)
        //    {
        //        H.Source = new BitmapImage(new Uri(@"/Images/forbidArea.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //    else
        //    {
        //        H.Source = new BitmapImage(new Uri(@"/Images/4.png", UriKind.RelativeOrAbsolute));
        //        //更新数据库
        //    }
        //}


        private void DrawArrowPath(int x, int y)
        {
            Path arrow = new Path();
            arrow.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            PathFigure pathFigure = new PathFigure();
            pathFigure.IsClosed = true;

            pathFigure.StartPoint = new Point(40, 0);//路径的起点

            pathFigure.Segments.Add(new LineSegment(new Point(35, 5), false));
            pathFigure.Segments.Add(new LineSegment(new Point(39, 5), false));
            pathFigure.Segments.Add(new LineSegment(new Point(39, 15), false));
            pathFigure.Segments.Add(new LineSegment(new Point(41, 15), false));
            pathFigure.Segments.Add(new LineSegment(new Point(41, 5), false));
            pathFigure.Segments.Add(new LineSegment(new Point(45, 5), false));


            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            arrow.Data = pathGeometry;

            this.cvs.Children.Add(arrow);
        }
    }
}
