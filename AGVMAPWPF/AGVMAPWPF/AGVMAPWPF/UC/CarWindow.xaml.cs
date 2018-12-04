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
    /// CarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CarWindow : UserControl
    {
        public static readonly DependencyProperty HeightValueProperty =
DependencyProperty.Register("HeightValue", typeof(double), typeof(CarWindow), null);

        public static readonly DependencyProperty WidthValueProperty =
DependencyProperty.Register("WidthValue", typeof(double), typeof(CarWindow), null);

        //封装依赖属性
        public double WidthValue
        {
            get
            {
                return this.MaxWidth;
            }
            set
            {
                this.Width = this.MaxWidth = value;
                SetValue(WidthValueProperty, value);
            }
        }

        //封装依赖属性
        public double HeightValue
        {
            get
            {
                return this.MaxHeight;
            }
            set
            {
                this.Height = this.MaxHeight = value;
                SetValue(HeightValueProperty, value);
            }
        }

        public CarWindow()
        {
            InitializeComponent();
        }

        public CarWindow(string num,double height,double width)
        {
            InitializeComponent();
            txt.Text = num;
            this.MaxHeight=this.Height = height;
            this.MaxWidth=this.Width = width;
        }

        private void CarWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            L1.Point = new Point(0,this.Height-10);
            A2.Point=new Point(10,this.Height);
            L2.Point = new Point(this.Width - 10, this.Height);
            A3.Point = new Point(this.Width, this.Height-10);
            L3.Point = new Point(this.Width, 10);
            A4.Point = new Point(this.Width-10, 0);
        }
    }
}
