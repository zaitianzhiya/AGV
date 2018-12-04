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
    /// ImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageWindow : UserControl
    {
        public event EventHandler mouseDown;

        public static readonly DependencyProperty BackgroundValueProperty =
      DependencyProperty.Register("BackgroundValue", typeof(Brush), typeof(ImageWindow), null);

        public static readonly DependencyProperty WidthValueProperty =
    DependencyProperty.Register("WidthValue", typeof(double), typeof(ImageWindow), null);

        public static readonly DependencyProperty HeightValueProperty =
   DependencyProperty.Register("HeightValue", typeof(double), typeof(ImageWindow), null);

        public static readonly DependencyProperty SourceValueProperty =
 DependencyProperty.Register("SourceValue", typeof(string), typeof(ImageWindow), null);

        public static readonly DependencyProperty TransColorProperty =
DependencyProperty.Register("TransColor", typeof(string), typeof(ImageWindow), null);

        //封装依赖属性
        public string TransColor
        {
            get
            {
                return (string)GetValue(TransColorProperty);
            }
            set
            {
                SetValue(TransColorProperty, value);
            }
        }

        //封装依赖属性
        public string SourceValue
        {
            get
            {
                return image.Source.ToString();
            }
            set
            {
                image.Source=new BitmapImage(new Uri(value,UriKind.Absolute));
                SetValue(SourceValueProperty, value);
            }
        }

        //封装依赖属性
        public Brush BackgroundValue
        {
            get
            {
                return grid.Background;
            }
            set
            {
                grid.Background= value;
                SetValue(BackgroundValueProperty, value);
            }
        }

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

        public ImageWindow(double heigth,double width)
        {
            InitializeComponent();
            this.Height = this.MaxHeight = heigth;
            this.Width = this.MaxWidth = width;
        }

        private void ImageWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mouseDown != null)
            {
                mouseDown(sender, e);
            }
        }
    }
}
