using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace AGVMAPWPF
{
    /// <summary>
    /// PointWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PointWindow : UserControl
    {
        public static readonly DependencyProperty BackgroundValueProperty =
   DependencyProperty.Register("BackgroundValue", typeof(Brush), typeof(StoreWindow), null);

        //封装依赖属性
        public Brush BackgroundValue
        {
            get
            {
                return (Brush)GetValue(BackgroundValueProperty);
            }
            set
            {
                this.Ellipse.Fill = this.Ellipse.Stroke = value;
                SetValue(BackgroundValueProperty, value);
            }
        }

        public PointWindow()
        {
            InitializeComponent();
        }

        public PointWindow(Brush brush,double width)
        {
            InitializeComponent();
            this.Height = this.Width = width;
            this.Ellipse.Fill = this.Ellipse.Stroke = brush;
        }
    }
}
