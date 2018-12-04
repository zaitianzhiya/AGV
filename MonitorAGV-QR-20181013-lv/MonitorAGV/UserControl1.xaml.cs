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

namespace WpfApplication1
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
       
    public partial class UserControl1 : UserControl
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("value", typeof(double), typeof(UserControl1), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnValueChanged)));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                this.Txt_MainBox.Text = value.ToString();
            }
        }
        public static readonly DependencyProperty PlusValueProperty = DependencyProperty.Register("PlusValue", typeof(double), typeof(UserControl1), new FrameworkPropertyMetadata());
        public double PlusValue
        {
            get { return (double)GetValue(PlusValueProperty); }
            set
            {
                SetValue(PlusValueProperty, value);
            }
        }
        public static readonly DependencyProperty BackGroundProperty = DependencyProperty.Register("BackGround", typeof(Brush), typeof(UserControl1));
        public Brush BackGround
        {
            get { return (Brush)GetValue(BackGroundProperty); }
            set
            {
                SetValue(BackGroundProperty, value);
            }
        }
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(UserControl1));
        public event RoutedPropertyChangedEventHandler<double> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj != null && obj is UserControl1)
            {
                UserControl1 control = obj as UserControl1;
                RoutedPropertyChangedEventArgs<double> e = new RoutedPropertyChangedEventArgs<double>((double)args.OldValue, (double)args.NewValue, ValueChangedEvent);
                control.OnValueChange(e);
            }
        }

        private void OnValueChange(RoutedPropertyChangedEventArgs<double> args)
        {
            RaiseEvent(args);
        }
 


        public UserControl1()
        {
            InitializeComponent();
        }

        private void img_Minus_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Value -= this.PlusValue;
            this.Txt_MainBox.Text = Convert.ToString(this.Value);
        }

        private void img_Plus_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Value += this.PlusValue;
            this.Txt_MainBox.Text = Convert.ToString(this.Value);
        }

        private void Txt_MainBox_KeyUp_1(object sender, KeyEventArgs e)
        {
            try
            {
                this.Value = Convert.ToDouble(this.Txt_MainBox.Text);
            }
            catch
            {
                MessageBox.Show("输入数据格式不正确");
                this.Txt_MainBox.Text = this.Txt_MainBox.Text.ToString().Remove(Txt_MainBox.Text.ToString().Length - 1);
                this.Txt_MainBox.CaretIndex = this.Txt_MainBox.Text.ToString().Length;

            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Txt_MainBox.Text = Value.ToString();
        }
       
    }
}
