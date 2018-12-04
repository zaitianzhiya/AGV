using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Data;
using System.Collections.ObjectModel;
using MonitorAGV;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    //public enum SexOpt { Male, Female ,}
    public class Member
    {
        public string id { get; set; }
        public string Shuxing { get; set; }
        public string Shuxing1 { get; set; }
        public string Zhi { get; set; }
        public string Zhi1 { get; set; }
    }
    public partial class MainWindow : Window
    {
        Point? lastDragPoint;
        Point? lastMousePositionOnTarget;
        Point? lastCenterPositionOnTarget;

        int xA = new int();
        int yA = new int();
        int xAA = new int();
        int yAA = new int();
        int generateClickCount = new int();

        List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
        List<DataGrid> dataGrids = new List<DataGrid>();
        List<CPosition> positions = new List<CPosition>();

        int rectangleSize = new int();
        int ellipseSize = new int();
        int imageSize = new int();

        private System.Windows.Threading.DispatcherTimer mm = null;
        int Ms = 0;
        int num;
        int[,] matrix;
        int[,] matrix2;
        int[,] matrix3;
        int[,] matrix4;
        int[,] matrix5;
        int[,] matrix6;
        bool ST = false;
        double timeslice;
        double timesliceRotate;
        double rotateUnit;
        double angleFrom, angleTo;
        double angleFrom2, angleTo2;
        double angleFrom3, angleTo3;
        double angleFrom4, angleTo4;
        double angleFrom5, angleTo5;
        double angleFrom6, angleTo6;
        bool isCleared;

        int pointStartX, pointStartY, pointEndX, pointEndY;//记录鼠标起点与终点数值
        Point locationStart, locationEnd;//记录鼠标起点与终点
        int rowStartX, rowStartY, rowEndX, rowEndY;//记录鼠标点击起点与终点的行号，列号
        const int imageHeight = 30;//AGV小车图片高度
        const int imageWidth = 46;//AGV小车图片宽度

        private string path;//"c"-禁行  “h”-充电区
        ContextMenu contextMenu = new ContextMenu();
        Rectangle shadowRectangle;
        BitmapImage pic;//AGV正常图片
        BitmapImage picError;//AGV非正常图片
        private MenuItem removeMenuItem, addFromMenuItem, addEndMenuItem;
        private bool IsExistFrom = false;//是否添加了起点
        private string FromPoint, EndPoint;//起点，终点
        //private Point pointStart, pointEnd;//起点，终点对应的右击点位置
        private Point lastPoint;//最后右击点的位置
        private Rectangle rectMousePress;//鼠标拖动时范围外框
        private Point rectPoint;//鼠标拖动时范围外框起点

        public MainWindow()
        {
            InitializeComponent();
            buttonGenerateClick();

            #region treeViewItems and dataGrids
            TreeViewItem node;
            DataGrid grid;
            foreach (var item in this.myTreeView.Items)
            {
                node = item as TreeViewItem;
                if (node != null)
                {
                    treeViewItems.Add(node);
                    foreach (var gd in node.Items)
                    {
                        grid = gd as DataGrid;
                        if (grid != null)
                        {
                            dataGrids.Add(grid);
                        }
                    }
                }
            }

            #region old
            //treeViewItems.Add(TreeViewItem0);
            //treeViewItems.Add(TreeViewItem1);
            //treeViewItems.Add(TreeViewItem2);
            //treeViewItems.Add(TreeViewItem3);
            //treeViewItems.Add(TreeViewItem4);
            //treeViewItems.Add(TreeViewItem5);
            //treeViewItems.Add(TreeViewItem6);
            //treeViewItems.Add(TreeViewItem7);
            //treeViewItems.Add(TreeViewItem8);
            //treeViewItems.Add(TreeViewItem9);
            //treeViewItems.Add(TreeViewItem10);
            //treeViewItems.Add(TreeViewItem11);
            //treeViewItems.Add(TreeViewItem12);
            //treeViewItems.Add(TreeViewItem13);
            //treeViewItems.Add(TreeViewItem14);
            //treeViewItems.Add(TreeViewItem15);
            //treeViewItems.Add(TreeViewItem16);
            //treeViewItems.Add(TreeViewItem17);
            //treeViewItems.Add(TreeViewItem18);
            //treeViewItems.Add(TreeViewItem19);
            //treeViewItems.Add(TreeViewItem20);
            //treeViewItems.Add(TreeViewItem21);
            //treeViewItems.Add(TreeViewItem22);
            //treeViewItems.Add(TreeViewItem23);
            //treeViewItems.Add(TreeViewItem24);

            //dataGrids.Add(datagrid0);
            //dataGrids.Add(datagrid0_0);
            //dataGrids.Add(datagrid1);
            //dataGrids.Add(datagrid1_1);
            //dataGrids.Add(datagrid2);
            //dataGrids.Add(datagrid2_2);
            //dataGrids.Add(datagrid3);
            //dataGrids.Add(datagrid3_3);
            //dataGrids.Add(datagrid4);
            //dataGrids.Add(datagrid4_4);
            //dataGrids.Add(datagrid5);
            //dataGrids.Add(datagrid5_5);
            //dataGrids.Add(datagrid6);
            //dataGrids.Add(datagrid6_6);
            //dataGrids.Add(datagrid7);
            //dataGrids.Add(datagrid7_7);
            //dataGrids.Add(datagrid8);
            //dataGrids.Add(datagrid8_8);
            //dataGrids.Add(datagrid9);
            //dataGrids.Add(datagrid9_9);
            //dataGrids.Add(datagrid10);
            //dataGrids.Add(datagrid10_1);
            //dataGrids.Add(datagrid11);
            //dataGrids.Add(datagrid11_1);
            //dataGrids.Add(datagrid12);
            //dataGrids.Add(datagrid12_1);
            //dataGrids.Add(datagrid13);
            //dataGrids.Add(datagrid13_1);
            //dataGrids.Add(datagrid14);
            //dataGrids.Add(datagrid14_1);
            //dataGrids.Add(datagrid15);
            //dataGrids.Add(datagrid15_1);
            //dataGrids.Add(datagrid16);
            //dataGrids.Add(datagrid16_1);
            //dataGrids.Add(datagrid17);
            //dataGrids.Add(datagrid17_1);
            //dataGrids.Add(datagrid18);
            //dataGrids.Add(datagrid18_1);
            //dataGrids.Add(datagrid19);
            //dataGrids.Add(datagrid19_1);
            //dataGrids.Add(datagrid20);
            //dataGrids.Add(datagrid20_1);
            //dataGrids.Add(datagrid21);
            //dataGrids.Add(datagrid21_1);
            //dataGrids.Add(datagrid22);
            //dataGrids.Add(datagrid22_1);
            //dataGrids.Add(datagrid23);
            //dataGrids.Add(datagrid23_1);
            //dataGrids.Add(datagrid24);
            //dataGrids.Add(datagrid24_1);
            #endregion
            #endregion

            //string ConDB = "database=db_BBY_KIVA;server=www.kwell-tech.cn;uid=sa;pwd=qazwsx12!@";
            string ConDB = FileControl.SetFileControl.ReadIniValue("DB", "DBSTRING", System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini").Trim();
            Function.getConn(ConDB);

            rectangleSize = 41;
            ellipseSize = 40;
            imageSize = 50;

            generateClickCount = 0;

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;
            scrollViewer.PreviewMouseRightButtonDown += scrollViewer_PreviewMouseRightButtonDown;
            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.PreviewMouseLeftButtonUp += scrollViewer_PreviewMouseLeftButtonUp;
            scrollViewer.MouseMove += OnMouseMove;

            #region Width, Height, and Opacity of Image
            //Image image;
            //foreach (var item in chartCanvas.Children)
            //{
            //    image = item as Image;
            //    if (image != null && image.Name.StartsWith("mytextbox"))
            //    {
            //        image.Height = imageHeight;
            //        image.Width = imageWidth;
            //        image.Opacity = 0;
            //    }
            //}
            #region old
            //mytextbox.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox3.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox3.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox4.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox4.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox5.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox5.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox6.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox6.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox7.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox7.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox8.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox8.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox9.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox9.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox10.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox10.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox11.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox11.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox12.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox12.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox13.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox13.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox14.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox14.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox15.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox15.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox16.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox16.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox17.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox17.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox18.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox18.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox19.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox19.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox20.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox20.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox21.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox21.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox22.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox22.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox23.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox23.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox24.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox24.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox25.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox25.Height = 72;//XG2017110716：14 {HeightOrigin:50}

            //mytextbox_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox2_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox2_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox3_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox3_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox4_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox4_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox5_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox5_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox6_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox6_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox7_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox7_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox8_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox8_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox9_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox9_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox10_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox10_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox11_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox11_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox12_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox12_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox13_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox13_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox14_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox14_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox15_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox15_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox16_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox16_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox17_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox17_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox18_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox18_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox19_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox19_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox20_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox20_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox21_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox21_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox22_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox22_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox23_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox23_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox24_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox24_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}
            //mytextbox25_2.Width = 72;//XG2017110716：14 {WidthOrigin:70}
            //mytextbox25_2.Height = 72;//XG2017110716：14 {HeightOrigin:50}

            ////Canvas.SetLeft(mytextbox, 0);
            ////Canvas.SetTop(mytextbox, 0);
            //mytextbox.Opacity = 0;
            //mytextbox2.Opacity = 0;
            //mytextbox3.Opacity = 0;
            //mytextbox4.Opacity = 0;
            //mytextbox5.Opacity = 0;
            //mytextbox6.Opacity = 0;
            //mytextbox7.Opacity = 0;
            //mytextbox8.Opacity = 0;
            //mytextbox9.Opacity = 0;
            //mytextbox10.Opacity = 0;
            //mytextbox11.Opacity = 0;
            //mytextbox12.Opacity = 0;
            //mytextbox13.Opacity = 0;
            //mytextbox14.Opacity = 0;
            //mytextbox15.Opacity = 0;
            //mytextbox16.Opacity = 0;
            //mytextbox17.Opacity = 0;
            //mytextbox18.Opacity = 0;
            //mytextbox19.Opacity = 0;
            //mytextbox20.Opacity = 0;
            //mytextbox21.Opacity = 0;
            //mytextbox22.Opacity = 0;
            //mytextbox23.Opacity = 0;
            //mytextbox24.Opacity = 0;
            //mytextbox25.Opacity = 0;

            //mytextbox_2.Opacity = 0;
            //mytextbox2_2.Opacity = 0;
            //mytextbox3_2.Opacity = 0;
            //mytextbox4_2.Opacity = 0;
            //mytextbox5_2.Opacity = 0;
            //mytextbox6_2.Opacity = 0;
            //mytextbox7_2.Opacity = 0;
            //mytextbox8_2.Opacity = 0;
            //mytextbox9_2.Opacity = 0;
            //mytextbox10_2.Opacity = 0;
            //mytextbox11_2.Opacity = 0;
            //mytextbox12_2.Opacity = 0;
            //mytextbox13_2.Opacity = 0;
            //mytextbox14_2.Opacity = 0;
            //mytextbox15_2.Opacity = 0;
            //mytextbox16_2.Opacity = 0;
            //mytextbox17_2.Opacity = 0;
            //mytextbox18_2.Opacity = 0;
            //mytextbox19_2.Opacity = 0;
            //mytextbox20_2.Opacity = 0;
            //mytextbox21_2.Opacity = 0;
            //mytextbox22_2.Opacity = 0;
            //mytextbox23_2.Opacity = 0;
            //mytextbox24_2.Opacity = 0;
            //mytextbox25_2.Opacity = 0;
            #endregion
            #endregion

            mm = new System.Windows.Threading.DispatcherTimer();
            mm.Tick += new EventHandler(OntimedEvent);
            mm.Interval = new TimeSpan(1000000);
            mm.Start();
            num = 0;
            timeslice = 2000;
            timesliceRotate = 1;
            rotateUnit = 15;
            angleFrom = 0;
            angleTo = 0;
            angleFrom2 = 0;
            angleTo2 = 0;
            angleFrom3 = 0;
            angleTo3 = 0;
            angleFrom4 = 0;
            angleTo4 = 0;
            angleFrom5 = 0;
            angleTo5 = 0;
            angleFrom6 = 0;
            angleTo6 = 0;
            isCleared = false;

            //mytextboxR.Opacity = 0;
            //mytextbox2R.Opacity = 0;
            //mytextbox3R.Opacity = 0;

            //CanvasAGV agvCanvas = new CanvasAGV();
            //Canvas can = new Canvas();
            //can.Height = 343;
            //can.Width = 319;
            //agvCanvas.GetAGVCanvas(25, 0, "1号", can);
            //ConvertCanvasToImage(can);
            // mytextbox.Source = Image.

            #region Creat Image
            pic = new BitmapImage();
            pic.BeginInit();
            pic.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car1.png", UriKind.RelativeOrAbsolute);
            pic.EndInit();

            picError = new BitmapImage();
            picError.BeginInit();
            picError.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car1_2.png", UriKind.RelativeOrAbsolute);
            picError.EndInit();
            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();
            //bi.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car1.png", UriKind.RelativeOrAbsolute);
            //bi.EndInit();
            //mytextbox.Source = bi;

            //BitmapImage bi2 = new BitmapImage();
            //bi2.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car2.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi2.EndInit();
            //mytextbox2.Source = bi2;

            //BitmapImage bi3 = new BitmapImage();
            //bi3.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi3.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car3.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi3.EndInit();
            //mytextbox3.Source = bi3;

            //BitmapImage bi4 = new BitmapImage();
            //bi4.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi4.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car4.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi4.EndInit();
            //mytextbox4.Source = bi4;

            //BitmapImage bi5 = new BitmapImage();
            //bi5.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi5.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car5.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi5.EndInit();
            //mytextbox5.Source = bi5;

            //BitmapImage bi6 = new BitmapImage();
            //bi6.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi6.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car6.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi6.EndInit();
            //mytextbox6.Source = bi6;

            //BitmapImage bi7 = new BitmapImage();
            //bi7.BeginInit();
            //bi7.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car7.png", UriKind.RelativeOrAbsolute);
            //bi7.EndInit();
            //mytextbox7.Source = bi7;

            //BitmapImage bi8 = new BitmapImage();
            //bi8.BeginInit();
            //bi8.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car8.png", UriKind.RelativeOrAbsolute);
            //bi8.EndInit();
            //mytextbox8.Source = bi8;

            //BitmapImage bi9 = new BitmapImage();
            //bi9.BeginInit();
            //bi9.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car9.png", UriKind.RelativeOrAbsolute);
            //bi9.EndInit();
            //mytextbox9.Source = bi9;

            //BitmapImage bi10 = new BitmapImage();
            //bi10.BeginInit();
            //bi10.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car10.png", UriKind.RelativeOrAbsolute);
            //bi10.EndInit();
            //mytextbox10.Source = bi10;

            //BitmapImage bi11 = new BitmapImage();
            //bi11.BeginInit();
            //bi11.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car11.png", UriKind.RelativeOrAbsolute);
            //bi11.EndInit();
            //mytextbox11.Source = bi11;

            //BitmapImage bi12 = new BitmapImage();
            //bi12.BeginInit();
            //bi12.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car12.png", UriKind.RelativeOrAbsolute);
            //bi12.EndInit();
            //mytextbox12.Source = bi12;

            //BitmapImage bi13 = new BitmapImage();
            //bi13.BeginInit();
            //bi13.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car13.png", UriKind.RelativeOrAbsolute);
            //bi13.EndInit();
            //mytextbox13.Source = bi13;

            //BitmapImage bi14 = new BitmapImage();
            //bi14.BeginInit();
            //bi14.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car14.png", UriKind.RelativeOrAbsolute);
            //bi14.EndInit();
            //mytextbox14.Source = bi14;

            //BitmapImage bi15 = new BitmapImage();
            //bi15.BeginInit();
            //bi15.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car15.png", UriKind.RelativeOrAbsolute);
            //bi15.EndInit();
            //mytextbox15.Source = bi15;

            //BitmapImage bi16 = new BitmapImage();
            //bi16.BeginInit();
            //bi16.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car16.png", UriKind.RelativeOrAbsolute);
            //bi16.EndInit();
            //mytextbox16.Source = bi16;

            //BitmapImage bi17 = new BitmapImage();
            //bi17.BeginInit();
            //bi17.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car17.png", UriKind.RelativeOrAbsolute);
            //bi17.EndInit();
            //mytextbox17.Source = bi17;

            //BitmapImage bi18 = new BitmapImage();
            //bi18.BeginInit();
            //bi18.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car18.png", UriKind.RelativeOrAbsolute);
            //bi18.EndInit();
            //mytextbox18.Source = bi18;

            //BitmapImage bi19 = new BitmapImage();
            //bi19.BeginInit();
            //bi19.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car19.png", UriKind.RelativeOrAbsolute);
            //bi19.EndInit();
            //mytextbox19.Source = bi19;

            //BitmapImage bi20 = new BitmapImage();
            //bi20.BeginInit();
            //bi20.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car20.png", UriKind.RelativeOrAbsolute);
            //bi20.EndInit();
            //mytextbox20.Source = bi20;

            //BitmapImage bi21 = new BitmapImage();
            //bi21.BeginInit();
            //bi21.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car21.png", UriKind.RelativeOrAbsolute);
            //bi21.EndInit();
            //mytextbox21.Source = bi21;

            //BitmapImage bi22 = new BitmapImage();
            //bi22.BeginInit();
            //bi22.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car22.png", UriKind.RelativeOrAbsolute);
            //bi22.EndInit();
            //mytextbox22.Source = bi22;

            //BitmapImage bi23 = new BitmapImage();
            //bi23.BeginInit();
            //bi23.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car23.png", UriKind.RelativeOrAbsolute);
            //bi23.EndInit();
            //mytextbox23.Source = bi23;

            //BitmapImage bi24 = new BitmapImage();
            //bi24.BeginInit();
            //bi24.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car24.png", UriKind.RelativeOrAbsolute);
            //bi24.EndInit();
            //mytextbox24.Source = bi24;

            //BitmapImage bi25 = new BitmapImage();
            //bi25.BeginInit();
            //bi25.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car25.png", UriKind.RelativeOrAbsolute);
            //bi25.EndInit();
            //mytextbox25.Source = bi25;

            //BitmapImage bi_2 = new BitmapImage();
            //bi_2.BeginInit();
            //bi_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car1_2.png", UriKind.RelativeOrAbsolute);
            //bi_2.EndInit();
            //mytextbox_2.Source = bi_2;

            //BitmapImage bi2_2 = new BitmapImage();
            //bi2_2.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi2_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car2_2.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi2_2.EndInit();
            //mytextbox2_2.Source = bi2_2;

            //BitmapImage bi3_2 = new BitmapImage();
            //bi3_2.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi3_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car3_2.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi3_2.EndInit();
            //mytextbox3_2.Source = bi3_2;

            //BitmapImage bi4_2 = new BitmapImage();
            //bi4_2.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi4_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car4_2.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi4_2.EndInit();
            //mytextbox4_2.Source = bi4_2;

            //BitmapImage bi5_2 = new BitmapImage();
            //bi5_2.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi5_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car5_2.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi5_2.EndInit();
            //mytextbox5_2.Source = bi5_2;

            //BitmapImage bi6_2 = new BitmapImage();
            //bi6_2.BeginInit();
            ////StreamResourceInfo info = Application.GetRemoteStream(new Uri("Test.jpg", UriKind.Relaltive));
            //bi6_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car6_2.png", UriKind.RelativeOrAbsolute);
            ////bi.StreamSource = info.Stream;
            //bi6_2.EndInit();
            //mytextbox6_2.Source = bi6_2;

            //BitmapImage bi7_2 = new BitmapImage();
            //bi7_2.BeginInit();
            //bi7_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car7_2.png", UriKind.RelativeOrAbsolute);
            //bi7_2.EndInit();
            //mytextbox7_2.Source = bi7_2;

            //BitmapImage bi8_2 = new BitmapImage();
            //bi8_2.BeginInit();
            //bi8_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car8_2.png", UriKind.RelativeOrAbsolute);
            //bi8_2.EndInit();
            //mytextbox8_2.Source = bi8_2;

            //BitmapImage bi9_2 = new BitmapImage();
            //bi9_2.BeginInit();
            //bi9_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car9_2.png", UriKind.RelativeOrAbsolute);
            //bi9_2.EndInit();
            //mytextbox9_2.Source = bi9_2;

            //BitmapImage bi10_2 = new BitmapImage();
            //bi10_2.BeginInit();
            //bi10_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car10_2.png", UriKind.RelativeOrAbsolute);
            //bi10_2.EndInit();
            //mytextbox10_2.Source = bi10_2;

            //BitmapImage bi11_2 = new BitmapImage();
            //bi11_2.BeginInit();
            //bi11_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car11_2.png", UriKind.RelativeOrAbsolute);
            //bi11_2.EndInit();
            //mytextbox11_2.Source = bi11_2;

            //BitmapImage bi12_2 = new BitmapImage();
            //bi12_2.BeginInit();
            //bi12_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car12_2.png", UriKind.RelativeOrAbsolute);
            //bi12_2.EndInit();
            //mytextbox12_2.Source = bi12_2;

            //BitmapImage bi13_2 = new BitmapImage();
            //bi13_2.BeginInit();
            //bi13_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car13_2.png", UriKind.RelativeOrAbsolute);
            //bi13_2.EndInit();
            //mytextbox13_2.Source = bi13_2;

            //BitmapImage bi14_2 = new BitmapImage();
            //bi14_2.BeginInit();
            //bi14_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car14_2.png", UriKind.RelativeOrAbsolute);
            //bi14_2.EndInit();
            //mytextbox14_2.Source = bi14_2;

            //BitmapImage bi15_2 = new BitmapImage();
            //bi15_2.BeginInit();
            //bi15_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car15_2.png", UriKind.RelativeOrAbsolute);
            //bi15_2.EndInit();
            //mytextbox15_2.Source = bi15_2;

            //BitmapImage bi16_2 = new BitmapImage();
            //bi16_2.BeginInit();
            //bi16_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car16_2.png", UriKind.RelativeOrAbsolute);
            //bi16_2.EndInit();
            //mytextbox16_2.Source = bi16_2;

            //BitmapImage bi17_2 = new BitmapImage();
            //bi17_2.BeginInit();
            //bi17_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car17_2.png", UriKind.RelativeOrAbsolute);
            //bi17_2.EndInit();
            //mytextbox17_2.Source = bi17_2;

            //BitmapImage bi18_2 = new BitmapImage();
            //bi18_2.BeginInit();
            //bi18_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car18_2.png", UriKind.RelativeOrAbsolute);
            //bi18_2.EndInit();
            //mytextbox18_2.Source = bi18_2;

            //BitmapImage bi19_2 = new BitmapImage();
            //bi19_2.BeginInit();
            //bi19_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car19_2.png", UriKind.RelativeOrAbsolute);
            //bi19_2.EndInit();
            //mytextbox19_2.Source = bi19_2;

            //BitmapImage bi20_2 = new BitmapImage();
            //bi20_2.BeginInit();
            //bi20_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car20_2.png", UriKind.RelativeOrAbsolute);
            //bi20_2.EndInit();
            //mytextbox20_2.Source = bi20_2;

            //BitmapImage bi21_2 = new BitmapImage();
            //bi21_2.BeginInit();
            //bi21_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car21_2.png", UriKind.RelativeOrAbsolute);
            //bi21_2.EndInit();
            //mytextbox21_2.Source = bi21_2;

            //BitmapImage bi22_2 = new BitmapImage();
            //bi22_2.BeginInit();
            //bi22_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car22_2.png", UriKind.RelativeOrAbsolute);
            //bi22_2.EndInit();
            //mytextbox22_2.Source = bi22_2;

            //BitmapImage bi23_2 = new BitmapImage();
            //bi23_2.BeginInit();
            //bi23_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car23_2.png", UriKind.RelativeOrAbsolute);
            //bi23_2.EndInit();
            //mytextbox23_2.Source = bi23_2;

            //BitmapImage bi24_2 = new BitmapImage();
            //bi24_2.BeginInit();
            //bi24_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car24_2.png", UriKind.RelativeOrAbsolute);
            //bi24_2.EndInit();
            //mytextbox24_2.Source = bi24_2;

            //BitmapImage bi25_2 = new BitmapImage();
            //bi25_2.BeginInit();
            //bi25_2.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"AGV_Image\car25_2.png", UriKind.RelativeOrAbsolute);
            //bi25_2.EndInit();
            //mytextbox25_2.Source = bi25_2;
            #endregion

            matrix = new int[9, 3] { { 2, 3, 0 }, { 5, 3, 0 }, { 5, 3, 90 }, { 5, 5, 90 }, { 5, 5, 180 }, { 3, 5, 180 }, { 3, 5, 270 }, { 3, 1, 270 }, { 3, 1, 0 } };
            matrix2 = new int[9, 3] { { 4, 3, 0 }, { 4, 3, 180 }, { 3, 3, 180 }, { 3, 3, 90 }, { 3, 4, 90 }, { 3, 4, 0 }, { 5, 4, 0 }, { 5, 4, 270 }, { 5, 3, 270 } };
            matrix3 = new int[9, 3] { { 4, 4, 270 }, { 4, 2, 270 }, { 4, 2, 180 }, { 2, 2, 180 }, { 2, 2, 90 }, { 2, 5, 90 }, { 2, 5, 0 }, { 4, 5, 0 }, { 4, 5, 90 } };

            matrix4 = new int[9, 3] { { 6, 6, 90 }, { 6, 6, 180 }, { 0, 6, 180 }, { 0, 6, 270 }, { 0, 4, 270 }, { 0, 4, 0 }, { 5, 4, 0 }, { 5, 4, 270 }, { 5, 2, 270 } };
            matrix5 = new int[9, 3] { { 0, 3, 0 }, { 2, 3, 0 }, { 2, 3, 270 }, { 2, 1, 270 }, { 5, 1, 180 }, { 3, 1, 180 }, { 3, 1, 90 }, { 3, 4, 90 }, { 3, 4, 0 } };
            matrix6 = new int[9, 3] { { 5, 4, 270 }, { 5, 2, 270 }, { 5, 2, 90 }, { 5, 4, 90 }, { 5, 4, 180 }, { 2, 4, 180 }, { 2, 4, 90 }, { 2, 6, 90 }, { 2, 6, 270 } };

            removeMenuItem = new MenuItem() { Header = "Remove" };
            contextMenu.Items.Add(removeMenuItem);
            addFromMenuItem = new MenuItem() { Header = "AddFromPoint" };
            contextMenu.Items.Add(addFromMenuItem);
            addEndMenuItem = new MenuItem() { Header = "AddEndPoint" };
            contextMenu.Items.Add(addEndMenuItem);
            addFromMenuItem.Click += addFromMenuItem_Click;
            addEndMenuItem.Click += addEndMenuItem_Click;

            removeMenuItem.Click += (sender, e) =>
            {
                double xtemp, ytemp, x, y;
                xtemp = Canvas.GetLeft(contextMenu.PlacementTarget);
                ytemp = Canvas.GetTop(contextMenu.PlacementTarget);
                x = (xtemp - 40) / 80;
                y = (ytemp - 40) / 80;
                int temp = (int)x + (yA - (int)y) * (xA + 1);
                string map_no = temp.ToString();
                int gap = ((int)x * 80 + 80) - (int)xtemp;
                switch (gap)
                {
                    case 39:
                        this.chartCanvas.Children.Remove(contextMenu.PlacementTarget);
                        positions[temp].posFlag[0] = 0;
                        positions[temp].forbiddenCount--;
                        positions[temp].isLocked = false;
                        int k = Function.PR_Write_Map_FQ(map_no, 0);
                        break;
                    case 40:
                        this.chartCanvas.Children.Remove(contextMenu.PlacementTarget);
                        positions[temp].posFlag[1] = 0;
                        positions[temp].chargeCount--;
                        break;
                    case 30:
                        this.chartCanvas.Children.Remove(contextMenu.PlacementTarget);
                        positions[temp].posFlag[2] = 0;
                        positions[temp].shellCount--;
                        break;
                }
            };

            addForbidFromDB();

        }

        private void OntimedEvent(object sender, EventArgs e)
        {
            Ms++;
            if (Ms == 2)
            {
                DataGridAdd();
                Ms = 0;
            }
        }

        private void OntimedEventBackUp(object sender, EventArgs e)
        {
            if (ST)
            {
                Ms++;
                if (Ms == 30)
                {
                    if (num < 9)
                    {
                        //#region  Get real-time message
                        ////Random rd = new Random();
                        ////RotateTransform rotateTransform = new RotateTransform(360-matrix[num, 2]);
                        ////TJA2017110223:27
                        //mytextbox.Opacity = 100;
                        //mytextbox2.Opacity = 100;
                        //mytextbox3.Opacity = 100;
                        //mytextbox4.Opacity = 100;
                        //mytextbox5.Opacity = 100;
                        //mytextbox6.Opacity = 100;
                        //if (num == 0)
                        //{
                        //    double realX_1 = (double)(matrix[num, 0] * 80 + 80) - (mytextbox.Width) / 2;
                        //    double realY_1 = (double)((yA - matrix[num, 1]) * 80 + 80) - (mytextbox.Height) / 2;
                        //    double realX2_1 = (double)(matrix2[num, 0] * 80 + 80) - (mytextbox2.Width) / 2;
                        //    double realY2_1 = (double)((yA - matrix2[num, 1]) * 80 + 80) - (mytextbox2.Height) / 2;
                        //    double realX3_1 = (double)(matrix3[num, 0] * 80 + 80) - (mytextbox3.Width) / 2;
                        //    double realY3_1 = (double)((yA - matrix3[num, 1]) * 80 + 80) - (mytextbox3.Height) / 2;
                        //    double realX4_1 = (double)(matrix4[num, 0] * 80 + 80) - (mytextbox4.Width) / 2;
                        //    double realY4_1 = (double)((yA - matrix4[num, 1]) * 80 + 80) - (mytextbox4.Height) / 2;
                        //    double realX5_1 = (double)(matrix5[num, 0] * 80 + 80) - (mytextbox5.Width) / 2;
                        //    double realY5_1 = (double)((yA - matrix5[num, 1]) * 80 + 80) - (mytextbox5.Height) / 2;
                        //    double realX6_1 = (double)(matrix6[num, 0] * 80 + 80) - (mytextbox6.Width) / 2;
                        //    double realY6_1 = (double)((yA - matrix6[num, 1]) * 80 + 80) - (mytextbox6.Height) / 2;

                        //    Canvas.SetLeft(mytextbox, realX_1);
                        //    Canvas.SetTop(mytextbox, realY_1);
                        //    Canvas.SetLeft(mytextbox2, realX2_1);
                        //    Canvas.SetTop(mytextbox2, realY2_1);
                        //    Canvas.SetLeft(mytextbox3, realX3_1);
                        //    Canvas.SetTop(mytextbox3, realY3_1);
                        //    Canvas.SetLeft(mytextbox4, realX4_1);
                        //    Canvas.SetTop(mytextbox4, realY4_1);
                        //    Canvas.SetLeft(mytextbox5, realX5_1);
                        //    Canvas.SetTop(mytextbox5, realY5_1);
                        //    Canvas.SetLeft(mytextbox6, realX6_1);
                        //    Canvas.SetTop(mytextbox6, realY6_1);
                        //    RotateTransform rotateTransform_1 = new RotateTransform((matrix[num, 2] == 0) ? 0 : (360 - matrix[num, 2])); //XGS2017110522:41
                        //    RotateTransform rotateTransform2_1 = new RotateTransform((matrix2[num, 2] == 0) ? 0 : (360 - matrix2[num, 2]));//XGS2017110522:41
                        //    RotateTransform rotateTransform3_1 = new RotateTransform((matrix3[num, 2] == 0) ? 0 : (360 - matrix3[num, 2]));//XGS2017110522:41
                        //    RotateTransform rotateTransform4_1 = new RotateTransform((matrix4[num, 2] == 0) ? 0 : (360 - matrix4[num, 2])); //XGS2017110522:41
                        //    RotateTransform rotateTransform5_1 = new RotateTransform((matrix5[num, 2] == 0) ? 0 : (360 - matrix5[num, 2]));//XGS2017110522:41
                        //    RotateTransform rotateTransform6_1 = new RotateTransform((matrix6[num, 2] == 0) ? 0 : (360 - matrix6[num, 2]));//XGS2017110522:41
                        //    mytextbox.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox2.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox3.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox.RenderTransform = rotateTransform_1;
                        //    mytextbox2.RenderTransform = rotateTransform2_1;
                        //    mytextbox3.RenderTransform = rotateTransform3_1;
                        //    mytextbox4.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox5.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox6.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox4.RenderTransform = rotateTransform4_1;
                        //    mytextbox5.RenderTransform = rotateTransform5_1;
                        //    mytextbox6.RenderTransform = rotateTransform6_1;
                        //}
                        //else
                        //{

                        //    angleFrom = ((matrix[num - 1, 2] == 0) ? 0 : (360 - matrix[num - 1, 2]));
                        //    angleTo = ((matrix[num, 2] == 0) ? 0 : (360 - matrix[num, 2]));
                        //    //rotateToByUnit(angleFrom, angleTo, mytextbox, rotateUnit);
                        //    rotateToByAnimation(angleFrom, angleTo, mytextbox, timesliceRotate);
                        //    angleFrom2 = ((matrix2[num - 1, 2] == 0) ? 0 : (360 - matrix2[num - 1, 2]));
                        //    angleTo2 = ((matrix2[num, 2] == 0) ? 0 : (360 - matrix2[num, 2]));
                        //    //rotateToByUnit(angleFrom2, angleTo2, mytextbox2, rotateUnit);
                        //    rotateToByAnimation(angleFrom2, angleTo2, mytextbox2, timesliceRotate);
                        //    angleFrom3 = ((matrix3[num - 1, 2] == 0) ? 0 : (360 - matrix3[num - 1, 2]));
                        //    angleTo3 = ((matrix3[num, 2] == 0) ? 0 : (360 - matrix3[num, 2]));
                        //    //rotateToByUnit(angleFrom3, angleTo3, mytextbox3, rotateUnit);
                        //    rotateToByAnimation(angleFrom3, angleTo3, mytextbox3, timesliceRotate);
                        //    angleFrom4 = ((matrix4[num - 1, 2] == 0) ? 0 : (360 - matrix4[num - 1, 2]));
                        //    angleTo4 = ((matrix4[num, 2] == 0) ? 0 : (360 - matrix4[num, 2]));
                        //    //rotateToByUnit(angleFrom4, angleTo4, mytextbox4, rotateUnit);
                        //    rotateToByAnimation(angleFrom4, angleTo4, mytextbox4, timesliceRotate);
                        //    angleFrom5 = ((matrix5[num - 1, 2] == 0) ? 0 : (360 - matrix5[num - 1, 2]));
                        //    angleTo5 = ((matrix5[num, 2] == 0) ? 0 : (360 - matrix5[num, 2]));
                        //    //rotateToByUnit(angleFrom5, angleTo5, mytextbox5, rotateUnit);
                        //    rotateToByAnimation(angleFrom5, angleTo5, mytextbox5, timesliceRotate);
                        //    angleFrom6 = ((matrix6[num - 1, 2] == 0) ? 0 : (360 - matrix6[num - 1, 2]));
                        //    angleTo6 = ((matrix6[num, 2] == 0) ? 0 : (360 - matrix6[num, 2]));
                        //    //rotateToByUnit(angleFrom6, angleTo6, mytextbox6, rotateUnit);
                        //    rotateToByAnimation(angleFrom6, angleTo6, mytextbox6, timesliceRotate);

                        //    /*测试代码，未带旋转效果和调用旋转函数一样，时间不同 //TJAZ2017110612:43
                        //    RotateTransform rotateTransform_2 = new RotateTransform((matrix[num, 2] == 0) ? 0 : (360 - matrix[num, 2])); //XGS2017110522:41
                        //    RotateTransform rotateTransform2_2 = new RotateTransform((matrix2[num, 2] == 0) ? 0 : (360 - matrix2[num, 2]));//XGS2017110522:41
                        //    RotateTransform rotateTransform3_2 = new RotateTransform((matrix3[num, 2] == 0) ? 0 : (360 - matrix3[num, 2]));//XGS2017110522:41
                        //    mytextbox.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox2.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox3.RenderTransformOrigin = new Point(0.5, 0.5);
                        //    mytextbox.RenderTransform = rotateTransform_2;
                        //    mytextbox2.RenderTransform = rotateTransform2_2;
                        //    mytextbox3.RenderTransform = rotateTransform3_2;
                        //     */
                        //    double realX = (double)(matrix[num, 0] * 80 + 80) - (mytextbox.Width) / 2;
                        //    double realY = (double)((yA - matrix[num, 1]) * 80 + 80) - (mytextbox.Height) / 2;
                        //    double realX2 = (double)(matrix2[num, 0] * 80 + 80) - (mytextbox2.Width) / 2;
                        //    double realY2 = (double)((yA - matrix2[num, 1]) * 80 + 80) - (mytextbox2.Height) / 2;
                        //    double realX3 = (double)(matrix3[num, 0] * 80 + 80) - (mytextbox3.Width) / 2;
                        //    double realY3 = (double)((yA - matrix3[num, 1]) * 80 + 80) - (mytextbox3.Height) / 2;
                        //    double realX4 = (double)(matrix4[num, 0] * 80 + 80) - (mytextbox4.Width) / 2;
                        //    double realY4 = (double)((yA - matrix4[num, 1]) * 80 + 80) - (mytextbox4.Height) / 2;
                        //    double realX5 = (double)(matrix5[num, 0] * 80 + 80) - (mytextbox5.Width) / 2;
                        //    double realY5 = (double)((yA - matrix5[num, 1]) * 80 + 80) - (mytextbox5.Height) / 2;
                        //    double realX6 = (double)(matrix6[num, 0] * 80 + 80) - (mytextbox6.Width) / 2;
                        //    double realY6 = (double)((yA - matrix6[num, 1]) * 80 + 80) - (mytextbox6.Height) / 2;
                        //    Point destinationPoint = new Point();
                        //    Point destinationPoint2 = new Point();
                        //    Point destinationPoint3 = new Point();
                        //    Point destinationPoint4 = new Point();
                        //    Point destinationPoint5 = new Point();
                        //    Point destinationPoint6 = new Point();
                        //    destinationPoint.X = realX;
                        //    destinationPoint.Y = realY;
                        //    destinationPoint2.X = realX2;
                        //    destinationPoint2.Y = realY2;
                        //    destinationPoint3.X = realX3;
                        //    destinationPoint3.Y = realY3;
                        //    destinationPoint4.X = realX4;
                        //    destinationPoint4.Y = realY4;
                        //    destinationPoint5.X = realX5;
                        //    destinationPoint5.Y = realY5;
                        //    destinationPoint6.X = realX6;
                        //    destinationPoint6.Y = realY6;
                        //    moveTo(destinationPoint, mytextbox, timeslice);
                        //    moveTo(destinationPoint2, mytextbox2, timeslice);
                        //    moveTo(destinationPoint3, mytextbox3, timeslice);
                        //    moveTo(destinationPoint4, mytextbox4, timeslice);
                        //    moveTo(destinationPoint5, mytextbox5, timeslice);
                        //    moveTo(destinationPoint6, mytextbox6, timeslice);
                        //    //Canvas.SetTop(mytextbox, realY);//?用于确定小车位置，不要亦可
                        //    //Canvas.SetLeft(mytextbox, realX);//?用于确定小车位置，不要亦可
                        //    //Canvas.SetTop(mytextbox2, realY2);//?用于确定小车位置，不要亦可
                        //    //Canvas.SetLeft(mytextbox2, realX2);//?用于确定小车位置，不要亦可
                        //    //Canvas.SetTop(mytextbox3, realY3);//?用于确定小车位置，不要亦可
                        //    //Canvas.SetLeft(mytextbox3, realX3);//?用于确定小车位置，不要亦可
                        //}
                        ////TJZ2017110223:27

                        ////showtime.Text = DateTime.Now.ToLongTimeString();
                        ////int curx = (int)Canvas.GetLeft(mytextbox);
                        ////int cury = (int)Canvas.GetTop(mytextbox);
                        ////string x = Canvas.GetLeft(mytextbox).ToString();
                        ////string y = Canvas.GetTop(mytextbox).ToString();
                        ////xtextbox.Text = x;
                        ////ytextbox.Text = y;
                        ////DbReadCount.Text = DBReadCount.ToString();
                        //num++;
                        //#endregion
                    }
                    else
                    {
                        num = 0;
                    }
                    Ms = 0;

                }
            }

        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point coordinate = Mouse.GetPosition(chartCanvas); 
            double x = (coordinate.X - 80) / 80;
            double y = (coordinate.Y - 80) / 80;
            double q = Math.Round(x, 2);
            double w = Math.Round(yA - y, 2);
            this.textBlock.Text = string.Format("Virtual:({0},{1}) || Real:({2}mm,{3}mm)", q, w, Math.Round(x * 600, 2), Math.Round((yA - y) * 600, 2));
            //TJZ2017102411:53
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);
                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;
                lastDragPoint = posNow;
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // if (path == "a")
            // {
            if (!lockDirection && path != "c" && path != "d" && path != "e" && path != "f" && path != "g" && path != "h" && path != "i")
            {
                var mousePos = e.GetPosition(scrollViewer);
                if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y < scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
                {
                    scrollViewer.Cursor = Cursors.Hand;
                    lastDragPoint = mousePos;
                    Mouse.Capture(scrollViewer);
                }
                //addShell();
                //addCharge();
                //addForbid();
            }
            //}        

            //if (lockDirection)
            //    DrawArrowByLocation();

            if (lockDirection || path == "c" || path == "d" || path == "e" || path == "f" || path == "g" || path == "h" || path == "i")
            {
                Point location = Mouse.GetPosition(chartCanvas);
                double x = (location.X - 40) / 80;
                double y = (location.Y - 40) / 80;

                int temp = (int)x + (yA - (int)y) * (xA + 1);
                int virtual_x = rowStartX = temp % (int.Parse(ynumber.Text) + 1);
                int virtual_y = rowStartY = int.Parse(ynumber.Text) - ((temp - virtual_x) / (int.Parse(ynumber.Text) + 1));

                pointStartX = (virtual_x * 80) + 40;
                pointStartY = (virtual_y * 80) + 40;

                locationStart = location;

                rectPoint = location;
            }
        }

        void scrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point location = Mouse.GetPosition(chartCanvas);
            double x = (location.X - 40) / 80;
            double y = (location.Y - 40) / 80;

            int temp = (int)x + (yA - (int)y) * (xA + 1);
            int virtual_x = rowEndX = temp % (int.Parse(ynumber.Text) + 1);
            int virtual_y = rowEndY = int.Parse(ynumber.Text) - ((temp - virtual_x) / (int.Parse(ynumber.Text) + 1));

            pointEndX = (virtual_x * 80) + 40;
            pointEndY = (virtual_y * 80) + 40;
            locationEnd = location;

            string str = "";
            int temp_x, temp_y;
            Path arrow;
            PathFigure pathFigure;
            PathGeometry pathGeometry;

            if (lockDirection)
            {
                #region UP
                if (locationStart.X - pointStartX > 20 && locationStart.X - pointStartX < 60 && locationStart.Y - pointStartY > 0 && locationStart.Y - pointStartY < 20 &&
                    locationEnd.X - pointEndX > 20 && locationEnd.X - pointEndX < 60 && locationEnd.Y - pointEndY > 0 && locationEnd.Y - pointEndY < 20)
                {
                    for (int i = rowEndY; i <= rowStartY; i++)
                    {
                        str = "UP" + virtual_x.ToString() + "_" + i.ToString();
                        temp_x = (virtual_x * 80) + 40;
                        temp_y = (i * 80) + 40;
                        if (ps.ContainsKey(str))
                        {
                            ps[str].Visibility = Visibility.Collapsed;
                            ps.Remove(str);
                        }
                        else
                        {
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);
                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 0);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;
                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                    }
                }
                #endregion

                #region DOWN
                else if (locationStart.X - pointStartX > 20 && locationStart.X - pointStartX < 60 && locationStart.Y - pointStartY > 60 && locationStart.Y - pointStartY < 80 &&
                    locationEnd.X - pointEndX > 20 && locationEnd.X - pointEndX < 60 && locationEnd.Y - pointEndY > 60 && locationEnd.Y - pointEndY < 80)
                {
                    for (int i = rowStartY; i <= rowEndY; i++)
                    {
                        str = "DOWN" + virtual_x.ToString() + "_" + i.ToString();
                        temp_x = (virtual_x * 80) + 40;
                        temp_y = (i * 80) + 40;
                        if (ps.ContainsKey(str))
                        {
                            ps[str].Visibility = Visibility.Collapsed;
                            ps.Remove(str);
                        }
                        else
                        {
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);

                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 80);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10 + 60), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10 + 60), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20 + 40), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20 + 40), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10 + 60), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10 + 60), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                    }
                }
                #endregion

                #region LEFT
                else if (locationStart.X - pointStartX > 0 && locationStart.X - pointStartX < 20 && locationStart.Y - pointStartY > 20 && locationStart.Y - pointStartY < 60 &&
                    locationEnd.X - pointEndX > 0 && locationEnd.X - pointEndX < 20 && locationEnd.Y - pointEndY > 20 && locationEnd.Y - pointEndY < 60)
                {
                    for (int i = rowEndX; i <= rowStartX; i++)
                    {
                        str = "LEFT" + i.ToString() + "_" + virtual_y.ToString();
                        temp_x = (i * 80) + 40;
                        temp_y = (virtual_y * 80) + 40;
                        if (ps.ContainsKey(str))
                        {
                            ps[str].Visibility = Visibility.Collapsed;
                            ps.Remove(str);
                        }
                        else
                        {
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);

                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 0, temp_y + 40);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 33), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 47), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                    }
                }
                #endregion

                #region RIGHT
                else if (locationStart.X - pointStartX > 60 && locationStart.X - pointStartX < 80 && locationStart.Y - pointStartY > 20 && locationStart.Y - pointStartY < 60 &&
                    locationEnd.X - pointEndX > 60 && locationEnd.X - pointEndX < 80 && locationEnd.Y - pointEndY > 20 && locationEnd.Y - pointEndY < 60)
                {
                    for (int i = rowStartX; i <= rowEndX; i++)
                    {
                        str = "RIGHT" + i.ToString() + "_" + virtual_y.ToString();
                        temp_x = (i * 80) + 40;
                        temp_y = (virtual_y * 80) + 40;
                        if (ps.ContainsKey(str))
                        {
                            ps[str].Visibility = Visibility.Collapsed;
                            ps.Remove(str);
                        }
                        else
                        {
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);

                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 80, temp_y + 40);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 33), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 47), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                    }
                }
                #endregion
            }

            #region 左箭头
            else if (path == "d")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);

                for (int i = minX; i <= maxX; i++)
                {
                    //多行
                    if (minY != maxY)
                    {
                        for (int j = minY; j <= maxY; j++)
                        {
                            str = "LEFT" + i.ToString() + "_" + j.ToString();
                            temp_x = (i * 80) + 40;
                            temp_y = (j * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 0, temp_y + 40);//路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 33), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 47), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;

                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                    }
                    // 单行
                    else
                    {
                        str = "LEFT" + i.ToString() + "_" + virtual_y.ToString();
                        if (i - 1 >= 0)
                        {
                            int MapNo = (int.Parse(ynumber.Text) - virtual_y) * (int.Parse(ynumber.Text) + 1) + i;
                            int RealNo = MapNo;


                            temp_x = (i * 80) + 40;
                            temp_y = (virtual_y * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                                Function.PR_UPDATE_Map_Info_Real(MapNo, 0 - RealNo);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 0, temp_y + 40);//路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 33), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 47), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;
                                    Function.PR_UPDATE_Map_Info_Real(MapNo, RealNo);
                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                        //temp_x = (i * 80) + 40;
                        //temp_y = (virtual_y * 80) + 40;
                        //if (ps.ContainsKey(str))
                        //{
                        //    ps[str].Visibility = Visibility.Collapsed;
                        //    ps.Remove(str);
                        //}
                        //else
                        //{
                        //    arrow = new Path();
                        //    arrow.Fill = new SolidColorBrush(Colors.Green);

                        //    pathFigure = new PathFigure();
                        //    pathFigure.IsClosed = true;
                        //    arrow.Name = str;
                        //    if (!ps.ContainsKey(str))
                        //    {
                        //        ps.Add(str, arrow);
                        //        pathFigure.StartPoint = new Point(temp_x + 0, temp_y + 40);//路径的起点
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 33), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 38), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 38), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 42), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 42), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 47), false));
                        //        pathGeometry = new PathGeometry();
                        //        pathGeometry.Figures.Add(pathFigure);
                        //        arrow.Data = pathGeometry;

                        //        this.chartCanvas.Children.Add(arrow);
                        //    }
                        //}
                    }
                }
            }
            #endregion

            #region 上箭头
            else if (path == "e")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);
                for (int i = minY; i <= maxY; i++)
                {
                    //多行
                    if (minX != maxX)
                    {
                        for (int j = minX; j <= maxX; j++)
                        {
                            str = "UP" + j + "_" + i.ToString();
                            temp_x = (j * 80) + 40;
                            temp_y = (i * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 0); //路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;

                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                    }
                    //单行
                    else
                    {
                        str = "UP" + virtual_x.ToString() + "_" + i.ToString();
                        if (i - 1 >= 0)
                        {
                            int MapNo = (int.Parse(ynumber.Text) - i) * (int.Parse(ynumber.Text) + 1) + virtual_x;
                            int RealNo = MapNo + 2 + int.Parse(ynumber.Text);
                            temp_x = (virtual_x * 80) + 40;
                            temp_y = (i * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                                Function.PR_UPDATE_Map_Info_Real(MapNo, 0 - RealNo);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 0); //路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;
                                    Function.PR_UPDATE_Map_Info_Real(MapNo, RealNo);
                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                        //temp_x = (virtual_x * 80) + 40;
                        //temp_y = (i * 80) + 40;
                        //if (ps.ContainsKey(str))
                        //{
                        //    ps[str].Visibility = Visibility.Collapsed;
                        //    ps.Remove(str);
                        //}
                        //else
                        //{
                        //    arrow = new Path();
                        //    arrow.Fill = new SolidColorBrush(Colors.Green);

                        //    pathFigure = new PathFigure();
                        //    pathFigure.IsClosed = true;
                        //    arrow.Name = str;
                        //    if (!ps.ContainsKey(str))
                        //    {
                        //        ps.Add(str, arrow);
                        //        pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 0); //路径的起点
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10), false));
                        //        pathGeometry = new PathGeometry();
                        //        pathGeometry.Figures.Add(pathFigure);
                        //        arrow.Data = pathGeometry;

                        //        this.chartCanvas.Children.Add(arrow);
                        //    }
                        //}
                    }
                }
            }
            #endregion

            #region 右箭头
            else if (path == "f")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);
                for (int i = minX; i <= maxX; i++)
                {
                    //多行
                    if (minY != maxY)
                    {
                        for (int j = minY; j <= maxY; j++)
                        {
                            str = "RIGHT" + i.ToString() + "_" + j.ToString();
                            temp_x = (i * 80) + 40;
                            temp_y = (j * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 80, temp_y + 40);//路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 33), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 47), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;

                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                    }
                    // 单行
                    else
                    {
                        str = "RIGHT" + i.ToString() + "_" + virtual_y.ToString();
                        if (i + 1 <= int.Parse(xnumber.Text))
                        {
                            int MapNo = (int.Parse(ynumber.Text) - virtual_y) * (int.Parse(ynumber.Text) + 1) + i;
                            int RealNo = MapNo + 2;
                            temp_x = (i * 80) + 40;
                            temp_y = (virtual_y * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                                Function.PR_UPDATE_Map_Info_Real(MapNo, 0 - RealNo);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 80, temp_y + 40);//路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 33), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 38), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 42), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 47), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;
                                    Function.PR_UPDATE_Map_Info_Real(MapNo, RealNo);
                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                        //temp_x = (i * 80) + 40;
                        //temp_y = (virtual_y * 80) + 40;
                        //if (ps.ContainsKey(str))
                        //{
                        //    ps[str].Visibility = Visibility.Collapsed;
                        //    ps.Remove(str);
                        //}
                        //else
                        //{
                        //    arrow = new Path();
                        //    arrow.Fill = new SolidColorBrush(Colors.Green);

                        //    pathFigure = new PathFigure();
                        //    pathFigure.IsClosed = true;
                        //    arrow.Name = str;
                        //    if (!ps.ContainsKey(str))
                        //    {
                        //        ps.Add(str, arrow);
                        //        pathFigure.StartPoint = new Point(temp_x + 80, temp_y + 40);//路径的起点
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 33), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 38), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 38), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 42), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 42), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 47), false));
                        //        pathGeometry = new PathGeometry();
                        //        pathGeometry.Figures.Add(pathFigure);
                        //        arrow.Data = pathGeometry;

                        //        this.chartCanvas.Children.Add(arrow);
                        //    }
                        //}
                    }
                }
            }
            #endregion

            #region 下箭头
            else if (path == "g")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);
                for (int i = minY; i <= maxY; i++)
                {
                    //多行
                    if (minX != maxX)
                    {
                        for (int j = minX; j <= maxX; j++)
                        {
                            str = "DOWN" + j.ToString() + "_" + i.ToString();
                            temp_x = (j * 80) + 40;
                            temp_y = (i * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 80);//路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10 + 60), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10 + 60), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20 + 40), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20 + 40), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10 + 60), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10 + 60), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;

                                    this.chartCanvas.Children.Add(arrow);
                                }
                            }
                        }
                    }
                    //单行
                    else
                    {
                        str = "DOWN" + virtual_x.ToString() + "_" + i.ToString();
                        if (i + 1 <= (int.Parse(ynumber.Text)))
                        {
                            int MapNo = (int.Parse(ynumber.Text) - i) * (int.Parse(ynumber.Text) + 1) + virtual_x;
                            int RealNo = MapNo - int.Parse(ynumber.Text);
                            temp_x = (virtual_x * 80) + 40;
                            temp_y = (i * 80) + 40;
                            if (ps.ContainsKey(str))
                            {
                                ps[str].Visibility = Visibility.Collapsed;
                                ps.Remove(str);
                                //PR_UPDATE_Map_Info_Real
                                Function.PR_UPDATE_Map_Info_Real(MapNo, 0 - RealNo);
                            }
                            else
                            {
                                arrow = new Path();
                                arrow.Fill = new SolidColorBrush(Colors.Green);

                                pathFigure = new PathFigure();
                                pathFigure.IsClosed = true;
                                arrow.Name = str;
                                if (!ps.ContainsKey(str))
                                {
                                    ps.Add(str, arrow);
                                    pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 80);//路径的起点
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10 + 60), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10 + 60), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20 + 40), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20 + 40), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10 + 60), false));
                                    pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10 + 60), false));
                                    pathGeometry = new PathGeometry();
                                    pathGeometry.Figures.Add(pathFigure);
                                    arrow.Data = pathGeometry;
                                    this.chartCanvas.Children.Add(arrow);
                                    Function.PR_UPDATE_Map_Info_Real(MapNo, RealNo);
                                }
                            }
                        }
                        //temp_x = (virtual_x * 80) + 40;
                        //temp_y = (i * 80) + 40;
                        //if (ps.ContainsKey(str))
                        //{
                        //    ps[str].Visibility = Visibility.Collapsed;
                        //    ps.Remove(str);
                        //}
                        //else
                        //{
                        //    arrow = new Path();
                        //    arrow.Fill = new SolidColorBrush(Colors.Green);

                        //    pathFigure = new PathFigure();
                        //    pathFigure.IsClosed = true;
                        //    arrow.Name = str;
                        //    if (!ps.ContainsKey(str))
                        //    {
                        //        ps.Add(str, arrow);
                        //        pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 80);//路径的起点
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10 + 60), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10 + 60), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20 + 40), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20 + 40), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10 + 60), false));
                        //        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10 + 60), false));
                        //        pathGeometry = new PathGeometry();
                        //        pathGeometry.Figures.Add(pathFigure);
                        //        arrow.Data = pathGeometry;

                        //        this.chartCanvas.Children.Add(arrow);
                        //    }
                        //}
                    }
                }
            }
            #endregion

            #region 禁止区域
            else if (path == "c")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);

                Rectangle rec;
                Rectangle recSelectLeft = null;
                Rectangle recSelectRight = null;
                int temp2;
                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        shadowRectangle = new Rectangle() { ContextMenu = contextMenu };
                        shadowRectangle.Height = 80 - 2 * (rectangleSize - 40);
                        shadowRectangle.Width = 80 - 2 * (rectangleSize - 40);
                        shadowRectangle.Stroke = new SolidColorBrush(Colors.Red);
                        shadowRectangle.StrokeThickness = 5;
                        shadowRectangle.Fill = new SolidColorBrush(Colors.Transparent);

                        Canvas.SetLeft(shadowRectangle, i * 80 + rectangleSize);
                        Canvas.SetTop(shadowRectangle, j * 80 + rectangleSize);

                        temp2 = i + (yA - j) * (xA + 1);
                        shadowRectangle.Name = "LEFT_" + temp2;

                        foreach (UIElement u in this.chartCanvas.Children)
                        {
                            rec = u as Rectangle;
                            if (rec != null)
                            {
                                if (Canvas.GetLeft(rec) == Canvas.GetLeft(shadowRectangle) && Canvas.GetTop(rec) == Canvas.GetTop(shadowRectangle) && rec.Name.StartsWith("LEFT_"))
                                {
                                    recSelectLeft = rec;
                                }
                                if (Canvas.GetLeft(rec) == Canvas.GetLeft(shadowRectangle) && Canvas.GetTop(rec) == Canvas.GetTop(shadowRectangle) && rec.Name.StartsWith("RIGHT_"))
                                {
                                    recSelectRight = rec;
                                }
                            }
                        }
                        if (positions[temp2].posFlag[0] == 0)
                        {
                            if (recSelectRight != null)
                            {
                                this.chartCanvas.Children.Remove(recSelectRight);
                            }
                            this.chartCanvas.Children.Add(shadowRectangle);
                            positions[temp2].posFlag[0] = 1;
                            positions[temp2].forbiddenCount++;
                            positions[temp2].isLocked = true; // isLocked may be used in the future.
                            string map_no = temp2.ToString();
                            int k = Function.PR_Write_Map_FQ(map_no, 2);
                        }
                        else
                        {
                            if (recSelectLeft != null)
                            {
                                this.chartCanvas.Children.Remove(recSelectLeft);
                                positions[temp2].posFlag[0] = 0;
                                positions[temp2].forbiddenCount--;
                                positions[temp2].isLocked = false;
                                int k = Function.PR_Write_Map_FQ(temp2.ToString(), 0);
                            }
                        }
                    }
                }
            }
            #endregion

            #region 充电区
            else if (path == "h")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);
                Ellipse charge;
                Ellipse ellipse;
                Ellipse ellipseSelect = null;
                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        charge = new Ellipse() { ContextMenu = contextMenu };

                        charge.Height = 2 * ellipseSize;
                        charge.Width = 2 * ellipseSize;
                        charge.StrokeDashArray = new DoubleCollection() { 1, 1 };
                        charge.Stroke = new SolidColorBrush(Colors.Blue);
                        charge.StrokeThickness = 3;
                        charge.Fill = new SolidColorBrush(Colors.Transparent);

                        Canvas.SetLeft(charge, i * 80 + ellipseSize);
                        Canvas.SetTop(charge, j * 80 + ellipseSize);

                        temp = i + (yA - j) * (xA + 1);
                        if (positions[temp].posFlag[1] == 0)
                        {
                            charge.Name = "ELEC_" + temp;
                            chartCanvas.Children.Add(charge);
                            positions[temp].posFlag[1] = 1;
                            positions[temp].chargeCount++;
                        }
                        else
                        {
                            foreach (UIElement u in this.chartCanvas.Children)
                            {
                                ellipse = u as Ellipse;
                                if (ellipse != null)
                                {
                                    if (Canvas.GetLeft(ellipse) == Canvas.GetLeft(charge) && Canvas.GetTop(ellipse) == Canvas.GetTop(charge) && ellipse.Name.StartsWith("ELEC_"))
                                    {
                                        ellipseSelect = ellipse;
                                        break;
                                    }
                                }
                            }
                            if (ellipseSelect != null)
                            {
                                this.chartCanvas.Children.Remove(ellipseSelect);
                                positions[temp].posFlag[1] = 0;
                                positions[temp].chargeCount--;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 货架区域
            else if (path == "i")
            {
                int minX = Math.Min(rowEndX, rowStartX);
                int maxX = Math.Max(rowEndX, rowStartX);

                int minY = Math.Min(rowEndY, rowStartY);
                int maxY = Math.Max(rowEndY, rowStartY);
                Image simpleImage;
                Image image;
                Image imageSelect = null;
                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        simpleImage = new Image() { ContextMenu = contextMenu };
                        simpleImage.Height = 80 - 2 * (imageSize - 40);
                        simpleImage.Width = 80 - 2 * (imageSize - 40);

                        simpleImage.Source = new BitmapImage(new Uri(@"/Images/goods.png", UriKind.RelativeOrAbsolute));

                        Canvas.SetLeft(simpleImage, i * 80 + imageSize);
                        Canvas.SetTop(simpleImage, j * 80 + imageSize);

                        temp = i + (yA - j) * (xA + 1);
                        if (positions[temp].posFlag[2] == 0)
                        {
                            simpleImage.Name = "SHELF_" + temp;
                            this.chartCanvas.Children.Add(simpleImage);
                            positions[temp].posFlag[2] = 1;
                            positions[temp].shellCount++;
                        }
                        else
                        {
                            foreach (UIElement u in this.chartCanvas.Children)
                            {
                                image = u as Image;
                                if (image != null)
                                {
                                    if (Canvas.GetLeft(image) == Canvas.GetLeft(simpleImage) && Canvas.GetTop(image) == Canvas.GetTop(simpleImage) && image.Name.StartsWith("SHELF_"))
                                    {
                                        imageSelect = image;
                                        break;
                                    }
                                }
                            }
                            if (imageSelect != null)
                            {
                                this.chartCanvas.Children.Remove(imageSelect);
                                positions[temp].posFlag[2] = 0;
                                positions[temp].shellCount--;
                            }
                        }
                    }
                }
            }
            #endregion

            if (rectMousePress != null)
            {
                this.chartCanvas.Children.Remove(rectMousePress);
                this.rectMousePress = null;
            }
        }

        void scrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point location = Mouse.GetPosition(chartCanvas);
            double x1 = (location.X - 80) / 80;
            double y1 = (location.Y - 80) / 80;
            double q = Math.Round(x1, 2);
            double w = Math.Round(yA - y1, 2);
            if (q > yA || q < -0.5 || w < -0.5)
            {
                return;
            }
            shadowRectangle = new Rectangle() { ContextMenu = contextMenu };
            shadowRectangle.Height = 80 - 2 * (rectangleSize - 40);
            shadowRectangle.Width = 80 - 2 * (rectangleSize - 40);
            shadowRectangle.Stroke = new SolidColorBrush(Colors.Transparent);
            shadowRectangle.StrokeThickness = 1;
            shadowRectangle.Fill = new SolidColorBrush(Colors.Transparent);
            double x;
            double y;
            x = (location.X - 40) / 80;
            y = (location.Y - 40) / 80;
            Canvas.SetLeft(shadowRectangle, (int)x * 80 + rectangleSize);
            Canvas.SetTop(shadowRectangle, (int)y * 80 + rectangleSize);
            lastPoint = location;

            int temp = (int)x + (yA - (int)y) * (xA + 1);
            shadowRectangle.Name = "RIGHT_" + temp;
            Rectangle rec;
            Rectangle recSelect = null;
            Image image;
            Image imageSelect = null;
            Ellipse ellipse;
            Ellipse ellipseSelect = null;

            Ellipse charge = new Ellipse() { ContextMenu = contextMenu };
            charge.Height = 2 * ellipseSize;
            charge.Width = 2 * ellipseSize;
            charge.StrokeDashArray = new DoubleCollection() { 1, 1 };
            charge.Stroke = new SolidColorBrush(Colors.Blue);
            charge.StrokeThickness = 3;
            charge.Fill = new SolidColorBrush(Colors.Transparent);

            Canvas.SetLeft(charge, (int)x * 80 + ellipseSize);
            Canvas.SetTop(charge, (int)y * 80 + ellipseSize);

            Image simpleImage = new Image() { ContextMenu = contextMenu };
            simpleImage.Height = 80 - 2 * (imageSize - 40);
            simpleImage.Width = 80 - 2 * (imageSize - 40);

            simpleImage.Source = new BitmapImage(new Uri(@"/Images/goods.png", UriKind.RelativeOrAbsolute));

            Canvas.SetLeft(simpleImage, (int)x * 80 + imageSize);
            Canvas.SetTop(simpleImage, (int)y * 80 + imageSize);

            foreach (UIElement u in this.chartCanvas.Children)
            {
                rec = u as Rectangle;
                if (rec != null)
                {
                    if (Canvas.GetLeft(rec) == Canvas.GetLeft(shadowRectangle) && Canvas.GetTop(rec) == Canvas.GetTop(shadowRectangle))
                    {
                        recSelect = rec;
                        break;
                    }
                }
                image = u as Image;
                if (image != null)
                {
                    if (Canvas.GetLeft(image) == Canvas.GetLeft(simpleImage) && Canvas.GetTop(image) == Canvas.GetTop(simpleImage) && image.Name.StartsWith("SHELF_"))
                    {
                        imageSelect = image;
                        break;
                    }
                }
                ellipse = u as Ellipse;
                if (ellipse != null)
                {
                    if (Canvas.GetLeft(ellipse) == Canvas.GetLeft(charge) && Canvas.GetTop(ellipse) == Canvas.GetTop(charge) && ellipse.Name.StartsWith("ELEC_"))
                    {
                        ellipseSelect = ellipse;
                        break;
                    }
                }
            }
            if (recSelect == null&&imageSelect==null&&ellipseSelect==null)
            {
                this.chartCanvas.Children.Add(shadowRectangle);
                if (contextMenu.Items.Contains(removeMenuItem))
                {
                    contextMenu.Items.Remove(removeMenuItem);
                }
                if (IsExistFrom)
                {
                    if (contextMenu.Items.Contains(addFromMenuItem))
                    {
                        contextMenu.Items.Remove(addFromMenuItem);
                    }
                    if (!contextMenu.Items.Contains(addEndMenuItem))
                    {
                        contextMenu.Items.Add(addEndMenuItem);
                    }
                }
                else
                {
                    if (contextMenu.Items.Contains(addEndMenuItem))
                    {
                        contextMenu.Items.Remove(addEndMenuItem);
                    }
                    if (!contextMenu.Items.Contains(addFromMenuItem))
                    {
                        contextMenu.Items.Add(addFromMenuItem);
                    }
                }
            }
            else
            {
                if (recSelect!=null&&recSelect.Name.StartsWith("RIGHT"))
                {
                    if (contextMenu.Items.Contains(removeMenuItem))
                    {
                        contextMenu.Items.Remove(removeMenuItem);
                    }
                    if (IsExistFrom)
                    {
                        if (contextMenu.Items.Contains(addFromMenuItem))
                        {
                            contextMenu.Items.Remove(addFromMenuItem);
                        }
                        if (!contextMenu.Items.Contains(addEndMenuItem))
                        {
                            contextMenu.Items.Add(addEndMenuItem);
                        }
                    }
                    else
                    {
                        if (contextMenu.Items.Contains(addEndMenuItem))
                        {
                            contextMenu.Items.Remove(addEndMenuItem);
                        }
                        if (!contextMenu.Items.Contains(addFromMenuItem))
                        {
                            contextMenu.Items.Add(addFromMenuItem);
                        }
                    }
                }
                else
                {
                    if (!contextMenu.Items.Contains(removeMenuItem))
                    {
                        contextMenu.Items.Insert(0, removeMenuItem);
                    }
                    if (IsExistFrom)
                    {
                        if (contextMenu.Items.Contains(addFromMenuItem))
                        {
                            contextMenu.Items.Remove(addFromMenuItem);
                        }
                        if (!contextMenu.Items.Contains(addEndMenuItem))
                        {
                            contextMenu.Items.Add(addEndMenuItem);
                        }
                    }
                    else
                    {
                        if (contextMenu.Items.Contains(addEndMenuItem))
                        {
                            contextMenu.Items.Remove(addEndMenuItem);
                        }
                        if (!contextMenu.Items.Contains(addFromMenuItem))
                        {
                            contextMenu.Items.Add(addFromMenuItem);
                        }
                    }
                }
            }
        }

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || (Keyboard.IsKeyDown(Key.LeftCtrl)))
            {
                lastMousePositionOnTarget = Mouse.GetPosition(grid);
                if (scaleTransform.ScaleX < 0.2 && scaleTransform.ScaleY < 0.2 && e.Delta < 0)
                {
                    return;
                }
                scaleTransform.ScaleX += (double)e.Delta / 600;
                scaleTransform.ScaleY += (double)e.Delta / 600;
                var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
                lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);

                e.Handled = true;
            }
        }

        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Point? targetBefore = null;
            Point? targetNow = null;
            //if (!lastMousePositionOnTarget.HasValue)
            //{
            //    if (lastCenterPositionOnTarget.HasValue)
            //    {
            //        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            //        Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, grid);

            //        targetBefore = lastCenterPositionOnTarget;
            //        targetNow = centerOfTargetNow;
            //    }
            //}
            //else
            //{
            //    targetBefore = lastMousePositionOnTarget;
            //    targetNow = Mouse.GetPosition(grid);

            //    lastMousePositionOnTarget = null;
            //}
            targetBefore = lastMousePositionOnTarget;
            targetNow = Mouse.GetPosition(grid);

            lastMousePositionOnTarget = null;
            if (targetBefore.HasValue)
            {
                double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                double multiplicatorX = e.ExtentWidth / grid.Width;
                double multiplicatorY = e.ExtentHeight / grid.Height;

                double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                {
                    return;
                }

                scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                scrollViewer.ScrollToVerticalOffset(newOffsetY);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //TJA2017102618:42
            if (generateClickCount > 0)
            {
                isCleared = true;
                for (int j = 0; j < positions.Count(); j++)
                {
                    positions[j].posFlag[0] = 0;
                    positions[j].forbiddenCount--;
                    chartCanvas.Children.Clear();
                }
            }
            else
            {
                isCleared = false;
            }
            //TJZ2017102618:42
            if (!string.IsNullOrEmpty(xnumber.Text) && !string.IsNullOrEmpty(ynumber.Text))
            {
                try
                {
                    xA = int.Parse(xnumber.Text);
                    yA = int.Parse(ynumber.Text);
                    xAA = 2 * xA;
                    yAA = 2 * yA;
                    //CreateImageView(xAA, yAA);
                    DrawScale(xAA, yAA);
                    //DrawScaleLabel(xA, yA);
                    DrawAxis(xA, yA);
                    DrawArrow(xA, yA);
                    //TJA2017102510:55
                    for (int i = 0; i < (xA + 1) * (yA + 1); i++)
                    {
                        CPosition temposition = new CPosition(0, 0, 0, 0, 0, 0, false);
                        positions.Add(temposition);
                    }
                    //TJZ2017102510:55

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");
                }
            }
            else
            {
                MessageBox.Show("Plese Insert Right Number!!!", "Error");
            }
            generateClickCount++; //TJS2017102622:40


        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            chartCanvas.Children.Clear();
            isCleared = true; //TJS2017110618:18
            //TJA2017102618:00
            for (int i = 0; i < (xA + 1) * (yA + 1); i++)
            {
                positions[i].posFlag[0] = 0;
                positions[i].forbiddenCount--;
            }
            //TJZ2017102618:00
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            path = "a";
        }

        private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            path = "b";
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            path = "c";
        }

        private void RadioButton_Checked_33(object sender, RoutedEventArgs e)
        {
            path = "dir";
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            path = "d";
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            path = "e";
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            path = "f";
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            path = "g";
        }

        private void RadioButton_Checked_8(object sender, RoutedEventArgs e)
        {
            path = "h";
        }

        private void RadioButton_Checked_9(object sender, RoutedEventArgs e)
        {
            path = "i";
        }

        private void RadioButton_Checked_10(object sender, RoutedEventArgs e)
        {
            path = "j";
        }

        private void RadioButton_Checked_11(object sender, RoutedEventArgs e)
        {
            path = "k";
        }

        private void RadioButton_Checked_12(object sender, RoutedEventArgs e)
        {
            path = "l";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window1 me = new Window1();
            me.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Window2 me = new Window2();
            me.ShowDialog();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //Window3 me = new Window3();
            //me.ShowDialog();

            try
            {
                if (isCleared == true)
                {
                    //this.chartCanvas.Children.Add(mytextbox);
                    //this.chartCanvas.Children.Add(mytextbox2);
                    //this.chartCanvas.Children.Add(mytextbox3);
                    //this.chartCanvas.Children.Add(mytextbox4);
                    //this.chartCanvas.Children.Add(mytextbox5);
                    //this.chartCanvas.Children.Add(mytextbox6);
                    ////mytextbox.Opacity = 100;
                    ////mytextbox2.Opacity = 100;
                    ////mytextbox3.Opacity = 100;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ST = !ST;
            if (ST == true)
            {
                button5.Content = "暂停";
            }
            else
            {
                button5.Content = "监控小车";
            }

            //num = 0; //TJS2017110313:49{ this command can decide whether cars go to their initial positons when the button is clicked }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // Window4 me = new Window4();
            //me.ShowDialog();
            addForbidFromDB();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Window5 me = new Window5();
            me.ShowDialog();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Window6 me = new Window6();
            me.ShowDialog();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            Window7 me = new Window7();
            me.ShowDialog();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            Window8 me = new Window8();
            me.ShowDialog();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            Window9 me = new Window9();
            me.ShowDialog();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            Window10 me = new Window10();
            me.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".xaml";
            ofd.Filter = "xml file|*.xaml";

            if (ofd.ShowDialog() == true)
            {
                //此处做你想做的事 ...=ofd.FileName; 
            }
        }

        private void Lock_Direction_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lockDirection = !lockDirection;
            if (lockDirection)
            {
                Lock_Direction.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                Lock_Direction.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        private void open_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window12 w12 = new Window12();
            w12.ShowDialog();
        }

        /// 右击添加终点事件
        /// <summary>
        /// 右击添加终点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addEndMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Point location = lastPoint;
            double x = (location.X - 80) / 80;
            double y = (location.Y - 80) / 80;
            double q = Math.Round(x, 2);
            double w = Math.Round(yA - y, 2);

            int mapX = int.Parse(Math.Round(q - 0.01, 0).ToString());
            int mapY = int.Parse(Math.Round(w - 0.01, 0).ToString());

            EndPoint = Function.GetMapNoByXY(mapX, mapY);

            if (!string.IsNullOrEmpty(EndPoint))
            {
                if (int.Parse(FromPoint) == int.Parse(EndPoint))
                {
                    MessageBox.Show("起点不能与终点相同", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                    return;
                }
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "确定创建任务：" + FromPoint + "(起点)," + EndPoint + "(终点)?", "询问",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question, MessageBoxResult.Yes);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (Function.PR_Insert_Task(FromPoint, EndPoint) > 0)
                    {
                        MessageBox.Show("创建任务成功");
                        IsExistFrom = false;
                    }
                    else
                    {
                        MessageBox.Show("创建任务失败");
                    }
                }
                else if (messageBoxResult == MessageBoxResult.Cancel)
                {
                    IsExistFrom = false;
                }
            }
        }

        /// 右击添加起点事件
        /// <summary>
        /// 右击添加起点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addFromMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!Function.IsExist_Map_Info())
            {
                MessageBox.Show("请先保存地图信息");
                return;
            }
            Point location = lastPoint;
            double x = (location.X - 80) / 80;
            double y = (location.Y - 80) / 80;
            double q = Math.Round(x, 2);
            double w = Math.Round(yA - y, 2);

            int mapX = int.Parse(Math.Round(q - 0.01, 0).ToString());
            int mapY = int.Parse(Math.Round(w - 0.01, 0).ToString());

            FromPoint = Function.GetMapNoByXY(mapX, mapY);
            if (!string.IsNullOrEmpty(FromPoint))
            {
                IsExistFrom = true;
            }
        }

        /// 保存地图单击事件
        /// <summary>
        /// 保存地图单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("是否确定要保存此地图信息？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.Yes);
            if (messageBoxResult != MessageBoxResult.Yes)
            {
                return;
            }
            Function.PR_Update_Map_Info(xA, yA, 0);
            MessageBox.Show("地图信息保存成功");
        }

        /// 鼠标拖动时阴影外框显示
        /// <summary>
        /// 鼠标拖动时阴影外框显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(this.chartCanvas);
                if (p != this.rectPoint && rectMousePress == null)
                {
                    rectMousePress = new Rectangle();
                    rectMousePress.Stroke = new SolidColorBrush(Colors.Black);
                    rectMousePress.StrokeThickness = 1;

                    rectMousePress.Fill = Brushes.LightGray;
                    this.chartCanvas.Children.Add(rectMousePress);
                }
                rectMousePress.Width = Math.Abs(p.X - rectPoint.X);
                rectMousePress.Height = Math.Abs(p.Y - rectPoint.Y);
                Canvas.SetLeft(rectMousePress, Math.Min(p.X, rectPoint.X));
                Canvas.SetTop(rectMousePress, Math.Min(p.Y, rectPoint.Y));
                Canvas.SetZIndex(rectMousePress, -1);
            }
        }

        //private void adddDrag(){
        //    scrollViewer.Cursor = Cursors.Hand;
        //}
        //private void addCar()
        //{
        //    if (path == "b") {
        //        Ellipse charge = new Ellipse() { ContextMenu = contextMenu };
        //        charge.Height = 40;
        //        charge.Width = 40;
        //        charge.StrokeDashArray = new DoubleCollection() { 3, 1 };
        //        charge.Stroke = new SolidColorBrush(Colors.Blue);
        //        charge.Fill = new SolidColorBrush(Colors.Transparent);
        //        Canvas.SetLeft(charge, 140);
        //        Canvas.SetTop(charge, 140);
        //        chartCanvas.Children.Add(charge);
        //    }
        //}

        /// 定义充电区方法
        /// <summary>
        /// 定义充电区方法
        /// </summary>
        private void addCharge()
        {
            if (path == "h")
            {
                Ellipse charge = new Ellipse() { ContextMenu = contextMenu };
                //charge.Height = 80; 
                //charge.Width = 80; 
                charge.Height = 2 * ellipseSize; //XGS2017102516:30 {80=>2*ellipseSize}
                charge.Width = 2 * ellipseSize; //XGS2017102516:30 {80=>2*ellipseSize}
                charge.StrokeDashArray = new DoubleCollection() { 1, 1 };
                charge.Stroke = new SolidColorBrush(Colors.Blue);
                charge.StrokeThickness = 3;
                charge.Fill = new SolidColorBrush(Colors.Transparent);
                Point location = Mouse.GetPosition(chartCanvas);
                double x = (location.X - 40) / 80;
                double y = (location.Y - 40) / 80;
                Canvas.SetLeft(charge, (int)x * 80 + ellipseSize);
                Canvas.SetTop(charge, (int)y * 80 + ellipseSize);

                try
                {
                    int MapNo = (int)x + (yA - (int)y) * (xA + 1);
                    if (positions[MapNo].posFlag[1] == 0)
                    {
                        chartCanvas.Children.Add(charge);
                        positions[MapNo].posFlag[1] = 1;
                        positions[MapNo].chargeCount++;
                    }
                    else
                    {
                        Ellipse ellipse;
                        Ellipse ellipseSelect = null;
                        foreach (UIElement u in this.chartCanvas.Children)
                        {
                            ellipse = u as Ellipse;
                            if (ellipse != null)
                            {
                                if (Canvas.GetLeft(ellipse) == Canvas.GetLeft(charge) && Canvas.GetTop(ellipse) == Canvas.GetTop(charge))
                                {
                                    ellipseSelect = ellipse;
                                    break;
                                }
                            }
                        }
                        if (ellipseSelect != null)
                        {
                            this.chartCanvas.Children.Remove(ellipseSelect);
                            positions[MapNo].posFlag[1] = 0;
                            positions[MapNo].chargeCount--;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// 定义货架区域方法
        /// <summary>
        /// 定义货架区域方法
        /// </summary>
        private void addShell()
        {
            if (path == "i")
            {
                Image simpleImage = new Image() { ContextMenu = contextMenu };

                //simpleImage.Height = 60; 
                //simpleImage.Width = 60; 
                simpleImage.Height = 80 - 2 * (imageSize - 40); //XGS2017102516:30 {60=>80-2*(imagesize-40)}
                simpleImage.Width = 80 - 2 * (imageSize - 40); //XGS2017102516:30 {60=>80-2*(imagesize-40)}

                simpleImage.Source = new BitmapImage(new Uri(@"/Images/goods.png", UriKind.RelativeOrAbsolute));
                Point location = Mouse.GetPosition(chartCanvas);
                double x;
                double y;
                x = (location.X - 40) / 80;
                y = (location.Y - 40) / 80;
                Canvas.SetLeft(simpleImage, (int)x * 80 + imageSize);//40是原点的坐标，-12是为了让标签看的位置剧中一点
                Canvas.SetTop(simpleImage, (int)y * 80 + imageSize);

                //TJA2017102511:48
                try
                {
                    int MapNo = (int)x + (yA - (int)y) * (xA + 1);
                    if (positions[MapNo].posFlag[2] == 0)
                    {
                        this.chartCanvas.Children.Add(simpleImage);
                        positions[MapNo].posFlag[2] = 1;
                        positions[MapNo].shellCount++;
                    }
                    else
                    {
                        Image image;
                        Image imageSelect = null;
                        foreach (UIElement u in this.chartCanvas.Children)
                        {
                            image = u as Image;
                            if (image != null)
                            {
                                if (Canvas.GetLeft(image) == Canvas.GetLeft(simpleImage) && Canvas.GetTop(image) == Canvas.GetTop(simpleImage))
                                {
                                    imageSelect = image;
                                    break;
                                }
                            }
                        }
                        if (imageSelect != null)
                        {
                            this.chartCanvas.Children.Remove(imageSelect);
                            positions[MapNo].posFlag[2] = 0;
                            positions[MapNo].shellCount--;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //TJZ2017102511:48
            }
        }

        /// 定义禁止区域方法
        /// <summary>
        /// 定义禁止区域方法
        /// </summary>
        private void addForbid()
        {
            if (path == "c")
            {
                shadowRectangle = new Rectangle() { ContextMenu = contextMenu };
                //shadowRectangle.Height = 78;
                //shadowRectangle.Width = 78;
                shadowRectangle.Height = 80 - 2 * (rectangleSize - 40);//XGS2017102516:30 {80=>80-2*(rectangleSize-40)}
                shadowRectangle.Width = 80 - 2 * (rectangleSize - 40);//XGS2017102516:30 {80=>80-2*(rectangleSize-40)}
                shadowRectangle.Stroke = new SolidColorBrush(Colors.Red);
                shadowRectangle.StrokeThickness = 5;
                shadowRectangle.Fill = new SolidColorBrush(Colors.Transparent);
                Point location = Mouse.GetPosition(chartCanvas);
                double x;
                double y;
                x = (location.X - 40) / 80;
                y = (location.Y - 40) / 80;
                Canvas.SetLeft(shadowRectangle, (int)x * 80 + rectangleSize);
                Canvas.SetTop(shadowRectangle, (int)y * 80 + rectangleSize);

                //TJA2017102015:27
                try
                {
                    int temp = (int)x + (yA - (int)y) * (xA + 1);  //TJS2017102410:55
                    shadowRectangle.Name = "LEFT_" + temp;
                    Rectangle rec;
                    Rectangle recSelectLeft = null;
                    Rectangle recSelectRight = null;
                    foreach (UIElement u in this.chartCanvas.Children)
                    {
                        rec = u as Rectangle;
                        if (rec != null)
                        {
                            if (Canvas.GetLeft(rec) == Canvas.GetLeft(shadowRectangle) && Canvas.GetTop(rec) == Canvas.GetTop(shadowRectangle) && rec.Name.StartsWith("LEFT_"))
                            {
                                recSelectLeft = rec;
                            }
                            if (Canvas.GetLeft(rec) == Canvas.GetLeft(shadowRectangle) && Canvas.GetTop(rec) == Canvas.GetTop(shadowRectangle) && rec.Name.StartsWith("RIGHT_"))
                            {
                                recSelectRight = rec;
                            }
                        }
                    }
                    if (positions[temp].posFlag[0] == 0)
                    {
                        if (recSelectRight != null)
                        {
                            this.chartCanvas.Children.Remove(recSelectRight);
                        }
                        this.chartCanvas.Children.Add(shadowRectangle);
                        positions[temp].posFlag[0] = 1;
                        positions[temp].forbiddenCount++;
                        positions[temp].isLocked = true; // isLocked may be used in the future.
                        string map_no = temp.ToString();
                        int k = Function.PR_Write_Map_FQ(map_no, 2);
                    }
                    else
                    {
                        if (recSelectLeft != null)
                        {
                            this.chartCanvas.Children.Remove(recSelectLeft);
                            positions[temp].posFlag[0] = 0;
                            positions[temp].forbiddenCount--;
                            positions[temp].isLocked = false;
                            int k = Function.PR_Write_Map_FQ(temp.ToString(), 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //TJZ2017102015:27
            }
        }

        private void addForbidFromDB()
        {             
            DataTable MapTable = Function.PR_Read_Map_FQ();
            if (MapTable != null && MapTable.Rows.Count > 0)
            {
                for (int j = 0; j < MapTable.Rows.Count; j++)
                {
                    int MapNo = int.Parse(MapTable.Rows[j][0].ToString().Trim());
                    int MapUsed = int.Parse(MapTable.Rows[j][1].ToString().Trim());
                    int Map_A = int.Parse(MapTable.Rows[j][2].ToString().Trim());
                    int Map_B = int.Parse(MapTable.Rows[j][3].ToString().Trim());
                    int Map_C = int.Parse(MapTable.Rows[j][4].ToString().Trim());
                    int Map_D = int.Parse(MapTable.Rows[j][5].ToString().Trim());
                    int Real_A = MapNo - int.Parse(xnumber.Text);
                    int Real_B = MapNo;
                    int Real_C = MapNo + 2;
                    int Real_D = MapNo + 2 + int.Parse(xnumber.Text);
                    if (MapUsed == 2)
                    {
                        shadowRectangle = new Rectangle() { ContextMenu = contextMenu };
                        shadowRectangle.Height = 80 - 2 * (rectangleSize - 40);
                        shadowRectangle.Width = 80 - 2 * (rectangleSize - 40);
                        shadowRectangle.Stroke = new SolidColorBrush(Colors.Red);
                        shadowRectangle.StrokeThickness = 5;
                        shadowRectangle.Fill = new SolidColorBrush(Colors.Transparent);
                        Canvas.SetLeft(shadowRectangle, (MapNo % (xA + 1)) * 80 + rectangleSize);//40是原点的坐标，-12是为了让标签看的位置剧中一点
                        Canvas.SetTop(shadowRectangle, (yA - MapNo / (xA + 1)) * 80 + rectangleSize); //TJS2017102410:55
                        try
                        {

                            if (positions[MapNo].posFlag[0] == 0)  //TJS2017102512:22
                            {
                                this.chartCanvas.Children.Add(shadowRectangle); //TJS2017102512:22   
                                positions[MapNo].posFlag[0] = 1;//TJS2017102511:37
                                positions[MapNo].forbiddenCount++;//TJS2017102511:37
                                positions[MapNo].isLocked = true;//TJS2017102511:37
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {

                        int virtual_x = MapNo % (int.Parse(xnumber.Text) + 1);
                        int virtual_y = int.Parse(ynumber.Text) - MapNo / (int.Parse(xnumber.Text) + 1);
                        int temp_x = (MapNo % (xA + 1)) * 80 + rectangleSize;
                        int temp_y = (yA - MapNo / (xA + 1)) * 80 + rectangleSize;
                        Path arrow;
                        PathFigure pathFigure;
                        PathGeometry pathGeometry;
                        string str = "";
                        //left//B
                        if (Real_B > 0 && (Real_B == Map_A|Real_B == Map_B|Real_B == Map_C|Real_B == Map_D))
                        {
                            str = "LEFT" + virtual_x.ToString() + "_" + virtual_y.ToString();
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);
                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 0, temp_y + 40);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 33), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 47), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                        //right//B
                        if (Real_C > 0 && (Real_C == Map_A | Real_C == Map_B | Real_C == Map_C | Real_C == Map_D))
                        {
                            str = "RIGHT" + virtual_x.ToString() + "_" + virtual_y.ToString();
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);

                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 80, temp_y + 40);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 33), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 38), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 42), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 47), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                        //top//D
                        if (Real_D > 0 && (Real_D == Map_A|Real_D == Map_B|Real_D == Map_C|Real_D == Map_D))
                        {
                            str = "UP" + virtual_x.ToString() + "_" + virtual_y.ToString();
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);
                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 0); //路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                        //down//A
                        if (Real_A > 0 && (Real_A == Map_A | Real_A == Map_B | Real_A == Map_C | Real_A == Map_D))
                        {
                            str = "DOWN" + virtual_x.ToString() + "_" + virtual_y.ToString();
                            arrow = new Path();
                            arrow.Fill = new SolidColorBrush(Colors.Green);

                            pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            arrow.Name = str;
                            if (!ps.ContainsKey(str))
                            {
                                ps.Add(str, arrow);
                                pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 80);//路径的起点
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10 + 60), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10 + 60), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20 + 40), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20 + 40), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10 + 60), false));
                                pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10 + 60), false));
                                pathGeometry = new PathGeometry();
                                pathGeometry.Figures.Add(pathFigure);
                                arrow.Data = pathGeometry;

                                this.chartCanvas.Children.Add(arrow);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  加载AGV车辆信息方法
        /// </summary>
        public void DataGridAdd()
        {
            //TJA2017102509:00
            for (int i = 0; i < 50; i++) //XG2017110808:38{25=>50}
            {
                dataGrids[i].DataContext = null;
            }
            UIElement ui;
            for (int i = chartCanvas.Children.Count - 1; i >= 0; i--)
            {
                ui = chartCanvas.Children[i];

                if (ui.Uid.StartsWith("AGV"))
                {
                    chartCanvas.Children.RemoveAt(i);
                }
            }
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
                RotateTransform rotateTransform;
                //CanvasAGV agv = null;
                SolidColorBrush color;
                AGV agvUc;
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
                    realX = (double)(carNowX * 80 + 80) - (imageWidth) / 2;
                    realY = (double)((yA - carNowY) * 80 + 80) - (imageHeight) / 2;

                    //agv = new CanvasAGV();

                    //rotateTransform = new RotateTransform(((carAngle == 0) ? 0 : (360 - carAngle)), realX + (imageWidth) / 2, realY + (imageHeight) / 2); //XGS2017110522:41

                    if ((agv_Ac == 0) || (agv_ErrorCord != "0") || (agv_WarningCord != "0"))
                    {
                        color = Brushes.Red;
                    }
                    else
                    {
                        color = Brushes.Blue;
                    }
                    //agv.GetAGVCanvas(realX, realY, imageHeight, imageWidth, carID.ToString(), this.chartCanvas, color, rotateTransform);
                    agvUc = new AGV(carID.ToString(), color, ((carAngle == 0) ? 0 : (360 - carAngle)));
                    agvUc.Uid = "AGV" + carID;
                    Canvas.SetLeft(agvUc, realX);
                    Canvas.SetTop(agvUc, realY);
                    this.chartCanvas.Children.Add(agvUc);
                   

                    if (j < 25)
                        treeViewItems[j].Header = agv_Ip;

                    ObservableCollection<Member> memberData = new ObservableCollection<Member>();
                    ObservableCollection<Member> memberData1 = new ObservableCollection<Member>();

                    memberData.Add(new Member() { id = 1.ToString(), Shuxing = "坐标", Zhi = coordinates }); //TJS2017102417:00                   
                    memberData.Add(new Member() { id = 2.ToString(), Shuxing = "AGV当前角度值", Zhi = agv_Angle });
                    memberData.Add(new Member() { id = 3.ToString(), Shuxing = "料架当前角度值", Zhi = agv_Skip_Angle });
                    memberData.Add(new Member() { id = 4.ToString(), Shuxing = "当前任务执行编号", Zhi = agv_Now_Ord });
                    memberData.Add(new Member() { id = 5.ToString(), Shuxing = "当前任务数", Zhi = agv_Now_Ord_Count.ToString() });
                    memberData.Add(new Member() { id = 6.ToString(), Shuxing = "剩余行程总和", Zhi = agv_Remaining_Trip });
                    memberData.Add(new Member() { id = 7.ToString(), Shuxing = "左轮转速", Zhi = agv_L_Speed });
                    memberData.Add(new Member() { id = 8.ToString(), Shuxing = "右轮转速", Zhi = agv_R_Speed });
                    memberData.Add(new Member() { id = 9.ToString(), Shuxing = "举升电机速度", Zhi = agv_Lifting_Speed });
                    memberData.Add(new Member() { id = 10.ToString(), Shuxing = "旋转电机速度", Zhi = agv_Rotating_Speed });
                    memberData.Add(new Member() { id = 11.ToString(), Shuxing = "电池电压值", Zhi = agv_Voltage });
                    memberData.Add(new Member() { id = 12.ToString(), Shuxing = "系统电流值", Zhi = agv_Electricity });
                    memberData.Add(new Member() { id = 13.ToString(), Shuxing = "错误代码", Zhi = agv_ErrorCord });
                    memberData.Add(new Member() { id = 14.ToString(), Shuxing = "警告代码", Zhi = agv_WarningCord });
                    memberData.Add(new Member() { id = 15.ToString(), Shuxing = "当前货架编号", Zhi = agv_LineNo });

                    memberData1.Add(new Member() { id = 1.ToString(), Shuxing1 = "执行任务状态", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 2.ToString(), Shuxing1 = "顶升复位标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 3.ToString(), Shuxing1 = "转盘复位标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 4.ToString(), Shuxing1 = "陀螺仪零偏纠正标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 5.ToString(), Shuxing1 = "携带料架信息", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 6.ToString(), Shuxing1 = "位置确定标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 7.ToString(), Shuxing1 = "小车当前故障标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 8.ToString(), Shuxing1 = "小车当前警告标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 9.ToString(), Shuxing1 = "激光感应器远距离标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 10.ToString(), Shuxing1 = "激光感应器中距离标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 11.ToString(), Shuxing1 = "激光感应器近距离标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 12.ToString(), Shuxing1 = "前端料架感应器标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 13.ToString(), Shuxing1 = "后端料架感应器标志", Zhi1 = "" });
                    memberData1.Add(new Member() { id = 13.ToString(), Shuxing1 = "休眠状态指示", Zhi1 = "" });
                    if (j < 25)
                    {
                        dataGrids[j * 2].DataContext = memberData;
                        dataGrids[j * 2 + 1].DataContext = memberData1;
                    }
                }
            }
        }

        /// 坐标箭头方法
        /// <summary>
        /// 坐标箭头方法
        /// </summary>
        /// <param name="xA"></param>
        /// <param name="yA"></param>
        private void DrawArrow(int xA, int yA)
        {
            Path x_axisArrow = new Path();//x轴箭头
            Path y_axisArrow = new Path();//y轴箭头

            x_axisArrow.Fill = new SolidColorBrush(Color.FromRgb(23, 23, 43));
            y_axisArrow.Fill = new SolidColorBrush(Color.FromRgb(23, 23, 43));

            PathFigure x_axisFigure = new PathFigure();
            x_axisFigure.IsClosed = true;
            x_axisFigure.StartPoint = new Point(100 + (xA + 1) * 80, (yA + 1) * 80 + 40 + 1 - 4);                          //路径的起点
            //路径的起点
            x_axisFigure.Segments.Add(new LineSegment(new Point(100 + (xA + 1) * 80, (yA + 1) * 80 + 40 + 1 + 4), false)); //第2个点
            x_axisFigure.Segments.Add(new LineSegment(new Point(110 + (xA + 1) * 80, (yA + 1) * 80 + 40 + 1), false)); //第3个点

            PathFigure y_axisFigure = new PathFigure();
            y_axisFigure.IsClosed = true;
            y_axisFigure.StartPoint = new Point(35, 10);                          //路径的起点
            y_axisFigure.Segments.Add(new LineSegment(new Point(43, 10), false)); //第2个点
            y_axisFigure.Segments.Add(new LineSegment(new Point(39, 0), false)); //第3个点

            PathGeometry x_axisGeometry = new PathGeometry();
            PathGeometry y_axisGeometry = new PathGeometry();

            x_axisGeometry.Figures.Add(x_axisFigure);
            y_axisGeometry.Figures.Add(y_axisFigure);

            x_axisArrow.Data = x_axisGeometry;
            y_axisArrow.Data = y_axisGeometry;

            this.chartCanvas.Children.Add(x_axisArrow);
            this.chartCanvas.Children.Add(y_axisArrow);
        }

        /// 绘制X轴、Y轴方法
        /// <summary>
        /// 绘制X轴、Y轴方法
        /// </summary>
        /// <param name="xA"></param>
        /// <param name="yA"></param>
        private void DrawAxis(int xA, int yA)
        {
            Line x_Axis = new Line();
            Line y_Axis = new Line();
            x_Axis.StrokeThickness = 1;
            y_Axis.StrokeThickness = 1;
            x_Axis.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            y_Axis.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            x_Axis.X1 = 39;
            x_Axis.Y1 = 41 + (yA + 1) * 80;
            x_Axis.X2 = 100 + (xA + 1) * 80;
            x_Axis.Y2 = x_Axis.Y1;
            y_Axis.X1 = 39;
            y_Axis.Y1 = 41 + (yA + 1) * 80;
            y_Axis.X2 = y_Axis.X1;
            y_Axis.Y2 = 10;
            this.chartCanvas.Children.Add(x_Axis);
            this.chartCanvas.Children.Add(y_Axis);

            TextBlock x_Label = new TextBlock();
            TextBlock y_Label = new TextBlock();
            x_Label.Text = "X";
            y_Label.Text = "Y";
            Canvas.SetLeft(x_Label, 90 + (xA + 1) * 80);
            Canvas.SetTop(x_Label, 43 + (yA + 1) * 80);
            Canvas.SetLeft(y_Label, 30);
            Canvas.SetTop(y_Label, 10);
            this.chartCanvas.Children.Add(x_Label);
            this.chartCanvas.Children.Add(y_Label);
        }

        /// 绘制坐标网格线方法
        /// <summary>
        /// 绘制坐标网格线方法
        /// </summary>
        /// <param name="xAA"></param>
        /// <param name="yAA"></param>
        private void DrawScale(int xAA, int yAA)
        {
            for (int i = 0; i <= xAA + 2; i += 1)
            {
                //原点 O=(40,40)
                Line x_scale = new Line();
                if (i % 2 == 0)
                {
                    x_scale.StrokeEndLineCap = PenLineCap.Triangle;
                    x_scale.StrokeThickness = 1;
                    x_scale.StrokeDashArray = new DoubleCollection() { 2 };
                    x_scale.StrokeDashCap = PenLineCap.Flat;
                    x_scale.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    x_scale.X1 = 40 + i * 40;   //原点x=40,每40px作1个刻度
                    x_scale.X2 = x_scale.X1;    //在x轴上的刻度线，起点和终点相同
                    x_scale.Y1 = 40;           //与原点坐标的y=280，相同 
                    x_scale.Y2 = 40 + 40 * (yAA + 2);//刻度线长度为4px 
                }
                else
                {
                    x_scale.StrokeEndLineCap = PenLineCap.Triangle;
                    x_scale.StrokeThickness = 1;
                    x_scale.StrokeDashArray = new DoubleCollection() { 30, 50 };
                    x_scale.StrokeDashCap = PenLineCap.Flat;
                    x_scale.Stroke = new SolidColorBrush(Color.FromRgb(54, 141, 24));

                    x_scale.X1 = 40 + i * 40;   //原点x=40,每10px作1个刻度
                    x_scale.X2 = x_scale.X1;    //在x轴上的刻度线，起点和终点相同

                    x_scale.Y1 = 105;           //与原点坐标的y=280，相同 
                    x_scale.Y2 = 40 + 40 * (yAA + 1);//刻度线长度为4px 
                }
                this.chartCanvas.Children.Add(x_scale);
            }

            //作出Y轴的刻度
            for (int j = 0; j <= yAA + 2; j += 1)
            {
                Line y_scale = new Line();
                if (j % 2 == 0)
                {
                    y_scale.StrokeEndLineCap = PenLineCap.Triangle;
                    y_scale.StrokeThickness = 1;
                    y_scale.StrokeDashArray = new DoubleCollection() { 2 };
                    y_scale.StrokeDashCap = PenLineCap.Flat;
                    y_scale.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    y_scale.X1 = 40;            //原点x=40，在y轴上的刻度线的起点与原点相同
                    y_scale.X2 = 40 + 40 * (xAA + 2);//刻度线长度为8px 
                    y_scale.Y1 = 40 + 40 * j;  //每10px作一个刻度 
                    y_scale.Y2 = y_scale.Y1;    //起点和终点y坐标相同 
                }
                else
                {
                    y_scale.StrokeEndLineCap = PenLineCap.Triangle;
                    y_scale.StrokeThickness = 1;
                    y_scale.StrokeDashArray = new DoubleCollection() { 30, 50 };
                    y_scale.StrokeDashCap = PenLineCap.Flat;
                    y_scale.Stroke = new SolidColorBrush(Color.FromRgb(54, 141, 24));
                    y_scale.X1 = 105;            //原点x=40，在y轴上的刻度线的起点与原点相同
                    y_scale.X2 = 40 + 40 * (xAA + 1);//刻度线长度为8px 
                    y_scale.Y1 = 40 + 40 * j;  //每10px作一个刻度 
                    y_scale.Y2 = y_scale.Y1;    //起点和终点y坐标相同 

                }
                this.chartCanvas.Children.Add(y_scale);
            }
        }

        /// <summary>
        /// 绘制坐标QRCODE方法
        /// </summary>
        /// <param name="xA"></param>
        /// <param name="yA"></param>
        private void DrawScaleLabel(int xA, int yA)
        {
            UCQrCode uc1;
            for (int i = 0; i <= xA; i++)
            {
                for (int j = 0; j <= yA; j++)
                {
                    //uc1 = new UCQrCode(i + "," + (yA - j));
                    //Canvas.SetLeft(uc1, 55 + 80 * i);
                    //Canvas.SetTop(uc1, 72 + 80 * j);
                    ////Canvas.SetZIndex(uc1, 99);
                    //this.chartCanvas.Children.Add(uc1);

                    TextBlock x_ScaleLabel = new TextBlock();
                    x_ScaleLabel.Text = (i * 1).ToString() + "," + (yA - j).ToString();
                    x_ScaleLabel.FontSize = 19;
                    Canvas.SetLeft(x_ScaleLabel, 55 + 80 * i + 4);//40是原点的坐标，-12是为了让标签看的位置剧中一点
                    Canvas.SetTop(x_ScaleLabel, 68 + 80 * j);
                    this.chartCanvas.Children.Add(x_ScaleLabel);

                    //Image simpleImage = new Image();
                    //simpleImage.Height = 16;
                    //simpleImage.Width = 16;
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block.                   
                    //simpleImage.Source = new BitmapImage(new Uri(@"/Images/QRCode.png", UriKind.RelativeOrAbsolute));
                    // Set the image source.
                    //Canvas.SetLeft(simpleImage, 72 + 80 * i);//40是原点的坐标，-12是为了让标签看的位置剧中一点
                    //Canvas.SetTop(simpleImage, 72 + 80 * j);
                    //this.chartCanvas.Children.Add(simpleImage);
                    //TextBringToFront(x_ScaleLabel);
                    ////BringToFront(simpleImage);
                    ////TJA201710221030
                    ////TextBlock x_ScaleLabel_No = new TextBlock();
                    ////x_ScaleLabel_No.FontSize = 26;
                    ////x_ScaleLabel_No.Foreground = Brushes.Red;
                    ////x_ScaleLabel_No.Text = (i + j * (xA+1)).ToString();
                    ////Canvas.SetLeft(x_ScaleLabel_No, 45 + 80 * i);
                    ////Canvas.SetTop(x_ScaleLabel_No, 45 + 80 * j);
                    ////this.chartCanvas.Children.Add(x_ScaleLabel_No);
                    ////TextBringToFront(x_ScaleLabel_No);
                    ////TJZ201710221030
                }
            }
        }

        public static void TextBringToFront(TextBlock element)//图片置于最顶层显示
        {
            if (element == null) return;


            Canvas parent = element.Parent as Canvas;
            if (parent == null) return;


            var maxZ = parent.Children.OfType<UIElement>()//linq语句，取Zindex的最大值
              .Where(x => x != element)
              .Select(x => Canvas.GetZIndex(x))
              .Max();
            Canvas.SetZIndex(element, maxZ + 1);
        }

        public static void BringToFront(Image element)//图片置于最顶层显示
        {
            if (element == null) return;


            Canvas parent = element.Parent as Canvas;
            if (parent == null) return;


            var maxZ = parent.Children.OfType<UIElement>()//linq语句，取Zindex的最大值
              .Where(x => x != element)
              .Select(x => Canvas.GetZIndex(x))
              .Max();
            Canvas.SetZIndex(element, maxZ + 1);
        }

        private void buttonGenerateClick() //TJBlock2017110814:16
        {
            buttonGenerate.IsEnabled = false;//TJS2017110814:24
            buttonClear.IsEnabled = false;//TJS2017110814:24
            if (generateClickCount > 0)
            {
                isCleared = true;
                for (int j = 0; j < positions.Count(); j++)
                {
                    positions[j].posFlag[0] = 0;
                    positions[j].forbiddenCount--;
                    chartCanvas.Children.Clear();
                }
            }
            else
            {
                isCleared = false;
            }
            string X = FileControl.SetFileControl.ReadIniValue("X_Y", "X", System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini").Trim();
            string Y = FileControl.SetFileControl.ReadIniValue("X_Y", "Y", System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini").Trim();
            //TJZ2017102618:42
            xnumber.Text = X;
            ynumber.Text = Y;

            if (!string.IsNullOrEmpty(xnumber.Text) && !string.IsNullOrEmpty(ynumber.Text))
            {
                try
                {
                    xA = int.Parse(xnumber.Text);
                    yA = int.Parse(ynumber.Text);
                    xAA = 2 * xA;
                    yAA = 2 * yA;
                    //CreateImageView(xAA, yAA);
                    DrawScale(xAA, yAA);
                    DrawScaleLabel(xA, yA);
                    DrawAxis(xA, yA);
                    DrawArrow(xA, yA);
                    //TJA2017102510:55
                    for (int i = 0; i < (xA + 1) * (yA + 1); i++)
                    {
                        CPosition temposition = new CPosition(0, 0, 0, 0, 0, 0, false);
                        positions.Add(temposition);
                    }
                    //TJZ2017102510:55

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");
                }
            }
            else
            {
                MessageBox.Show("Plese Insert Right Number!!!", "Error");
            }
            generateClickCount++; //TJS2017102622:40

        }

        #region DrawArrow 2018年9月19日
        private bool lockDirection = false;
        Dictionary<string, Path> ps = new Dictionary<string, Path>();
        private void DrawArrowByLocation()
        {
            Point location = Mouse.GetPosition(chartCanvas);
            double x = (location.X - 40) / 80;
            double y = (location.Y - 40) / 80;

            int temp = (int)x + (yA - (int)y) * (xA + 1);
            int virtual_x = temp % (int.Parse(ynumber.Text) + 1);
            int virtual_y = int.Parse(ynumber.Text) - ((temp - virtual_x) / (int.Parse(ynumber.Text) + 1));

            int temp_x = (virtual_x * 80) + 40;
            int temp_y = (virtual_y * 80) + 40;

            #region UP
            if (location.X - temp_x > 20 && location.X - temp_x < 60 && location.Y - temp_y > 0 && location.Y - temp_y < 20)
            {
                string str = "UP" + virtual_x.ToString() + "_" + virtual_y.ToString();
                if (ps.ContainsKey(str))
                {
                    ps[str].Visibility = Visibility.Collapsed;
                    ps.Remove(str);

                }
                else
                {
                    Path arrow = new Path();
                    arrow.Fill = new SolidColorBrush(Colors.Green);

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.IsClosed = true;
                    arrow.Name = str;
                    if (!ps.ContainsKey(str))
                    {
                        ps.Add(str, arrow);
                        pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 0);//路径的起点
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10), false));
                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);
                        arrow.Data = pathGeometry;

                        this.chartCanvas.Children.Add(arrow);
                    }
                }
            }
            #endregion
            #region DOWN
            else if (location.X - temp_x > 20 && location.X - temp_x < 60 && location.Y - temp_y > 60 && location.Y - temp_y < 80)
            {
                string str = "DOWN" + virtual_x.ToString() + "_" + virtual_y.ToString();
                if (ps.ContainsKey(str))
                {
                    ps[str].Visibility = Visibility.Collapsed;
                    ps.Remove(str);

                }
                else
                {
                    Path arrow = new Path();
                    arrow.Fill = new SolidColorBrush(Colors.Green);

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.IsClosed = true;
                    arrow.Name = str;
                    if (!ps.ContainsKey(str))
                    {
                        ps.Add(str, arrow);
                        pathFigure.StartPoint = new Point(temp_x + 40, temp_y + 80);//路径的起点
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 33, temp_y + 10 + 60), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 10 + 60), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 38, temp_y + 20 + 40), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 20 + 40), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 42, temp_y + 10 + 60), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 47, temp_y + 10 + 60), false));

                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);
                        arrow.Data = pathGeometry;

                        this.chartCanvas.Children.Add(arrow);
                    }
                }
            }
            #endregion
            #region LEFT
            else if (location.X - temp_x > 0 && location.X - temp_x < 20 && location.Y - temp_y > 20 && location.Y - temp_y < 60)
            {
                string str = "LEFT" + virtual_x.ToString() + "_" + virtual_y.ToString();
                if (ps.ContainsKey(str))
                {
                    ps[str].Visibility = Visibility.Collapsed;
                    ps.Remove(str);

                }
                else
                {
                    Path arrow = new Path();
                    arrow.Fill = new SolidColorBrush(Colors.Green);

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.IsClosed = true;
                    arrow.Name = str;
                    if (!ps.ContainsKey(str))
                    {
                        ps.Add(str, arrow);
                        pathFigure.StartPoint = new Point(temp_x + 0, temp_y + 40);//路径的起点
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 33), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 38), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 38), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20, temp_y + 42), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 42), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10, temp_y + 47), false));

                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);
                        arrow.Data = pathGeometry;

                        this.chartCanvas.Children.Add(arrow);
                    }
                }
            }
            #endregion
            #region RIGHT
            else if (location.X - temp_x > 60 && location.X - temp_x < 80 && location.Y - temp_y > 20 && location.Y - temp_y < 60)
            {
                string str = "RIGHT" + virtual_x.ToString() + "_" + virtual_y.ToString();
                if (ps.ContainsKey(str))
                {
                    ps[str].Visibility = Visibility.Collapsed;
                    ps.Remove(str);

                }
                else
                {
                    Path arrow = new Path();
                    arrow.Fill = new SolidColorBrush(Colors.Green);

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.IsClosed = true;
                    arrow.Name = str;
                    if (!ps.ContainsKey(str))
                    {
                        ps.Add(str, arrow);
                        pathFigure.StartPoint = new Point(temp_x + 80, temp_y + 40);//路径的起点
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 33), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 38), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 38), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 20 + 40, temp_y + 42), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 42), false));
                        pathFigure.Segments.Add(new LineSegment(new Point(temp_x + 10 + 60, temp_y + 47), false));


                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);
                        arrow.Data = pathGeometry;

                        this.chartCanvas.Children.Add(arrow);
                    }
                }
            }
            #endregion
        }
        #endregion

        public void ConvertCanvasToImage(Canvas can)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(343, 319, this.contextMenu.Width, this.contextMenu.Width, PixelFormats.Default);
            rtb.Render(can);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            string file = "d:\\xxx.jpg";
            using (System.IO.Stream stm = System.IO.File.Create(file))
            {
                encoder.Save(stm);
            }
        }

        private void moveTo(Point deskPoint, Image ell, double space)
        {
            Point curPoint = new Point();
            curPoint.X = Canvas.GetLeft(ell);
            curPoint.Y = Canvas.GetTop(ell);
            Storyboard storyboard = new Storyboard();
            double lxspeed = space, lyspeed = space;
            DoubleAnimation doubleAnimation = new DoubleAnimation(
              Canvas.GetLeft(ell),
              deskPoint.X,
              new Duration(TimeSpan.FromMilliseconds(lxspeed))
            );
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTarget(doubleAnimation, ell);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(doubleAnimation);
            doubleAnimation = new DoubleAnimation(
              Canvas.GetTop(ell),
              deskPoint.Y,
              new Duration(TimeSpan.FromMilliseconds(lyspeed))
            );
            //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTarget(doubleAnimation, ell);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        //旋转动画
        private void rotateToByAnimation(double angleFromAnimation, double angleToAnimation, Image ell, double space)
        {
            #region rotating conditions 2017110616:40
            double gap = System.Math.Abs(angleFromAnimation - angleToAnimation);
            double reverseGap = 360 - gap;
            double angleFromAnimationFinal = 0;
            double angleToAnimationFinal = 0;
            switch ((int)gap)
            {
                case 0:
                    angleFromAnimationFinal = angleToAnimation;
                    angleToAnimationFinal = angleToAnimation;
                    //RotateTransform rtf = new RotateTransform(angleToAnimation);//TJS2017110611:46{去掉转向紊乱，小车朝向初始化方向0}
                    //ell.RenderTransform = rtf;//TJS2017110611:46{去掉转向紊乱，小车朝向初始化方向0}
                    /* 测试代码，旋转效果不能显示//TJAZ2017110612:47
                    for (int m = 1; m <= 135 / rotateUnitTemp; m++)
                    {
                        rtf = new RotateTransform(m*rotateUnitTemp);
                        ell.RenderTransform = rtf;
                    }   
                     */
                    break; //return ;???
                case 180:
                    if (angleFromAnimation >= 0 && angleFromAnimation < 180)
                    {
                        angleFromAnimationFinal = angleFromAnimation;
                        angleToAnimationFinal = angleToAnimation;
                    }
                    if (angleFromAnimation >= 180 && angleFromAnimation < 360)
                    {
                        angleFromAnimationFinal = angleFromAnimation;
                        angleToAnimationFinal = (angleToAnimation + 360);
                    }
                    break;
                default:
                    {
                        if (angleFromAnimation < angleToAnimation)
                        {
                            if (gap < 180)
                            {
                                angleFromAnimationFinal = angleFromAnimation;
                                angleToAnimationFinal = angleToAnimation;
                            }
                            else
                            {
                                angleFromAnimationFinal = (angleFromAnimation + 360);
                                angleToAnimationFinal = angleToAnimation;
                            }
                        }

                        if (angleFromAnimation > angleToAnimation)
                        {
                            if (gap < 180)
                            {
                                angleFromAnimationFinal = angleFromAnimation;
                                angleToAnimationFinal = angleToAnimation;
                            }
                            else
                            {
                                angleFromAnimationFinal = angleFromAnimation;
                                angleToAnimationFinal = (angleToAnimation + 360);
                            }
                        }
                        break;
                    }
            }
            #endregion

            RotateTransform rtf = new RotateTransform();
            ell.RenderTransform = rtf;
            ell.RenderTransformOrigin = new Point(0.5, 0.5);
            DoubleAnimation dbAscending = new DoubleAnimation(angleFromAnimationFinal, angleToAnimationFinal, new Duration(TimeSpan.FromSeconds(space)));
            Storyboard story = new Storyboard();
            //dbAscending.RepeatBehavior = RepeatBehavior.Forever;
            story.Children.Add(dbAscending);
            Storyboard.SetTarget(dbAscending, ell);
            Storyboard.SetTargetProperty(dbAscending, new PropertyPath("RenderTransform.Angle"));
            story.Begin();
        }

        private void rotateToByUnit(double angleFromUnit, double angleToUnit, Control ell, double rotateUnitTemp)
        {
            RotateTransform rtf = new RotateTransform();
            ell.RenderTransform = rtf;
            ell.RenderTransformOrigin = new Point(0.5, 0.5);

            //TJA2017110517:11
            double gap = System.Math.Abs(angleFromUnit - angleToUnit);
            double reverseGap = 360 - gap;
            double steps = gap / rotateUnitTemp;
            double reverseSteps = reverseGap / rotateUnitTemp;
            switch ((int)gap)
            {
                case 0:
                    rtf = new RotateTransform(angleToUnit);//TJS2017110611:46{去掉转向紊乱，小车朝向初始化方向0}
                    ell.RenderTransform = rtf;//TJS2017110611:46{去掉转向紊乱，小车朝向初始化方向0}
                    /* 测试代码，旋转效果不能显示//TJAZ2017110612:47
                    for (int m = 1; m <= 135 / rotateUnitTemp; m++)
                    {
                        rtf = new RotateTransform(m*rotateUnitTemp);
                        ell.RenderTransform = rtf;
                    }   
                     */
                    break;
                case 180:
                    if (angleFromUnit >= 0 && angleFromUnit < 180)
                    {
                        for (int i = 1; (i > 0) && (i <= (int)steps); i++)
                        {
                            rtf = new RotateTransform(angleFromUnit + i * rotateUnitTemp);
                            ell.RenderTransform = rtf;
                        }
                        rtf = new RotateTransform(angleToUnit);
                        ell.RenderTransform = rtf;
                    }
                    if (angleFromUnit >= 180 && angleFromUnit < 360)
                    {
                        for (int i = 1; (i > 0) && (i <= (int)steps); i++)
                        {
                            rtf = new RotateTransform(angleFromUnit + i * rotateUnitTemp);
                            ell.RenderTransform = rtf;
                        }
                        rtf = new RotateTransform(angleToUnit + 360);
                        ell.RenderTransform = rtf;
                    }
                    break;
                default:
                    {
                        if (angleFromUnit < angleToUnit)
                        {
                            if (gap < 180)
                            {
                                for (int i = 1; (i > 0) && (i <= (int)steps); i++)
                                {
                                    rtf = new RotateTransform(angleFromUnit + i * rotateUnitTemp);
                                    ell.RenderTransform = rtf;
                                }
                                rtf = new RotateTransform(angleToUnit);
                                ell.RenderTransform = rtf;
                            }
                            else
                            {
                                for (int i = 1; (i > 0) && (i <= (int)reverseSteps); i++)
                                {
                                    rtf = new RotateTransform(angleFromUnit + 360 - i * rotateUnitTemp);
                                    ell.RenderTransform = rtf;
                                }
                                rtf = new RotateTransform(angleToUnit);
                                ell.RenderTransform = rtf;
                            }

                        }

                        if (angleFromUnit > angleToUnit)
                        {
                            if (gap < 180)
                            {
                                for (int i = 1; (i > 0) && (i <= (int)steps); i++)
                                {
                                    rtf = new RotateTransform(angleFromUnit - i * rotateUnitTemp);
                                    ell.RenderTransform = rtf;
                                }
                                rtf = new RotateTransform(angleToUnit);
                                ell.RenderTransform = rtf;
                            }
                            else
                            {
                                for (int i = 1; (i > 0) && (i <= (int)reverseSteps); i++)
                                {
                                    rtf = new RotateTransform(angleFromUnit + i * rotateUnitTemp);
                                    ell.RenderTransform = rtf;
                                }
                                rtf = new RotateTransform(angleToUnit);
                                ell.RenderTransform = rtf;
                            }
                        }
                        break;
                    }
            }
            //TJZ2017110517:11
        }

        private void rotateToByUnitSimple(double angleFromSimple, double angleToSimple, Control ell, double rotateUnitSimple)
        {
            RotateTransform rtf = new RotateTransform();
            ell.RenderTransform = rtf;//???
            ell.RenderTransformOrigin = new Point(0.5, 0.5);
            if (angleFromSimple < angleToSimple)
            {
                int icount = (int)((angleToSimple - angleFromSimple) / rotateUnitSimple);
                for (int i = 1; (i > 0) && (i <= icount); i++)
                {
                    rtf = new RotateTransform(angleFromSimple + i * rotateUnitSimple);
                    ell.RenderTransform = rtf;
                }
                rtf = new RotateTransform(angleToSimple);
                ell.RenderTransform = rtf;
            }
            else
            {
                int jcount = (int)((angleFromSimple - angleToSimple) / rotateUnitSimple);
                for (int j = 1; (j > 0) && (j <= jcount); j++)
                {
                    rtf = new RotateTransform(angleFromSimple - j * rotateUnitSimple);
                    ell.RenderTransform = rtf;
                }
                rtf = new RotateTransform(angleToSimple);
                ell.RenderTransform = rtf;
            }
        }

     
    }
}

