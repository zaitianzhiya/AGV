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
    /// Ellipse.xaml 的交互逻辑
    /// </summary>
    public partial class StationWindow : UserControl
    {
        public event EventHandler mouseDown;
        public event EventHandler mouseEnter;

        //public static readonly DependencyProperty BackgroundValueProperty =
        //  DependencyProperty.Register("BackgroundValue", typeof(Brush), typeof(StationWindow), new PropertyMetadata(OnBackgroudValueChanged));
        public static readonly DependencyProperty BackgroundValueProperty =
         DependencyProperty.Register("BackgroundValue", typeof(Brush), typeof(StationWindow), null);

        public static readonly DependencyProperty numProperty =
       DependencyProperty.Register("numValue", typeof(string), typeof(StationWindow), null);

        //封装依赖属性
        public Brush BackgroundValue
        {
            get
            {
                return (Brush)GetValue(BackgroundValueProperty);
            }
            set
            {
                if (value.ToString() == MainWindow.selectBrush.ToString())
                {
                    rectOut.MouseEnter -= rect_OnMouseEnter;
                    rectOut.MouseLeave -= rect_OnMouseLeave;
                }
                else
                {
                    rectOut.MouseEnter += rect_OnMouseEnter;
                    rectOut.MouseLeave += rect_OnMouseLeave;
                }
                ellipse.Fill = ellipse.Stroke = rect.Fill = rect.Stroke = value;

                SetValue(BackgroundValueProperty, value);
            }
        }

        //封装依赖属性
        public string numValue
        {
            get
            {
                return txtBlock.Text.Trim();
            }
            set
            {
                txtBlock.Text = value;
                SetValue(numProperty, value);
            }
        }

        //回调函数
        static void OnBackgroudValueChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            StationWindow uc = (StationWindow)sender;
            uc.ellipse.Fill = (Brush)args.NewValue;
        }

        public StationWindow()
        {
            InitializeComponent();
        }

        public StationWindow(string num, string landName)
        {
            InitializeComponent();
            txtBlock.Text = num;
            rect.Uid = num;
            this.Name = landName;
        }

        private void rect_OnMouseEnter(object sender, MouseEventArgs e)
        {
            rect.Fill = Brushes.Green;
            rect.Stroke = Brushes.White;
            if (mouseDown != null)
            {
                mouseEnter(rect, e);
            }
        }

        private void rect_OnMouseLeave(object sender, MouseEventArgs e)
        {
            rect.Fill = Brushes.Brown;
            rect.Stroke = Brushes.Brown;
        }

        private void RectOut_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mouseDown != null)
            {
                mouseDown(txtBlock.Text.Trim(), e);
            }
        }
    }
}
