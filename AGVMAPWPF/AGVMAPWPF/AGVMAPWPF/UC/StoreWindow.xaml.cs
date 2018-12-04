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
    /// StoreWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StoreWindow : UserControl
    {
        public event EventHandler mouseDown;

        public static readonly DependencyProperty BackgroundValueProperty =DependencyProperty.Register("BackgroundValue", typeof(Brush), typeof(StoreWindow), null);

        public static readonly DependencyProperty NumValueProperty =DependencyProperty.Register("NumValue", typeof(string), typeof(StoreWindow), null);

        public static readonly DependencyProperty LankMarkCodeProperty =DependencyProperty.Register("LankMarkCode", typeof(string), typeof(StoreWindow), null);

        public static readonly DependencyProperty StorageStateProperty =DependencyProperty.Register("StorageState", typeof(string), typeof(StoreWindow), null);

        //封装依赖属性
        public Brush BackgroundValue
        {
            get
            {
                return rect.Fill;
            }
            set
            {
                rect.Fill = value;
                SetValue(BackgroundValueProperty, value);
            }
        }

        //封装依赖属性
        public string NumValue
        {
            get
            {
                return txtContent.Text.Trim();
            }
            set
            {
                txtContent.Text = value;
                SetValue(NumValueProperty, value);
            }
        }

        //封装依赖属性
        public string LankMarkCode
        {
            get
            {
                return (string)GetValue(LankMarkCodeProperty);
            }
            set
            {
                SetValue(LankMarkCodeProperty, value);
            }
        }

        //封装依赖属性
        public string StorageState
        {
            get
            {
                return (string)GetValue(StorageStateProperty);
            }
            set
            {
                SetValue(StorageStateProperty, value);
            }
        }

        public StoreWindow(string num, string lankMarkCode, string storageName, double width)
        {
            InitializeComponent();
            txtContent.Text = num;
            this.Width = this.Height = this.MaxHeight = this.MaxWidth = width;
            this.LankMarkCode = lankMarkCode;
            this.Name = storageName;
        }

        private void StoreWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mouseDown != null)
            {
                mouseDown(sender, e);
            }
        }
    }
}
