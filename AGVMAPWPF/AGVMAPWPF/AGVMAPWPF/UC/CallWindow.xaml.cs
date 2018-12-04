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
    /// CallWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CallWindow : UserControl
    {
        public event EventHandler mouseDown;

        public static readonly DependencyProperty BackgroundValueProperty =
      DependencyProperty.Register("BackgroundValue", typeof(Brush), typeof(CallWindow), null);

        public static readonly DependencyProperty HeightValueProperty =
    DependencyProperty.Register("HeightValue", typeof(double), typeof(CallWindow), null);

        public static readonly DependencyProperty CallBoxIdValueProperty =
  DependencyProperty.Register("CallBoxIdValue", typeof(string), typeof(CallWindow), null);

        //封装依赖属性
        public string CallBoxIdValue
        {
            get
            {
                return (string)GetValue(CallBoxIdValueProperty);
            }
            set
            {
                SetValue(CallBoxIdValueProperty, value);
            }
        }

        //封装依赖属性
        public Brush BackgroundValue
        {
            get
            {
                return ellipseInside.Fill;
            }
            set
            {
                ellipseInside.Fill = value;
                SetValue(BackgroundValueProperty, value);
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
                this.Height = this.Width = this.MaxHeight = this.MaxWidth = value;
                if (value < 100)
                {
                    txtContent.FontSize = 10;
                }
                else if (value < 200)
                {
                    txtContent.FontSize = 25;
                }
                else if (value < 300)
                {
                    txtContent.FontSize = 50;
                }
                else
                {
                    txtContent.FontSize =80;
                }
                SetValue(HeightValueProperty, value);
            }
        }

        public CallWindow(double width,string callBoxId)
        {
            InitializeComponent();
            txtContent.Text = "呼\n叫";
            this.Height = this.Width = this.MaxHeight = this.MaxWidth = width;
            this.CallBoxIdValue = callBoxId;
            if (this.Height < 100)
            {
                txtContent.FontSize = 10;
            }
            else if (this.Height < 200)
            {
                txtContent.FontSize = 25;
            }
            else if (this.Height < 300)
            {
                txtContent.FontSize = 50;
            }
            else
            {
                txtContent.FontSize = 80;
            }
        }

        private void CallWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mouseDown != null)
            {
                mouseDown(sender, e);
            }
        }
    }
}
