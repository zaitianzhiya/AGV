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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// AGV.xaml 的交互逻辑
    /// </summary>
    public partial class AGV : UserControl
    {
        public AGV()
        {
            InitializeComponent();
        }

        public AGV(string remark, SolidColorBrush color, double angle)
        {
            InitializeComponent();

            RotateTransform rotateTransform = new RotateTransform(angle, 23, 15);

            //myPath.Uid = "AGV_" + remark + "_01";
            myPath.Stroke = color;

            myPath.RenderTransform = rotateTransform;

            //myPath2.Uid = "AGV_" + remark + "_02";
            myPath2.Stroke = color;

            //myPath3.Uid = "AGV_" + remark + "_03";
            myPath3.Stroke = color;
            myPath3.RenderTransform = rotateTransform;

            textBlock.Text = remark;
            //textBlock.Uid = "AGV_" + remark + "_05";
        }
    }
}
