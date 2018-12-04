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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace WpfApplication1
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        public void newMission()
        {
            ObservableCollection<Opreation> OpreationData = new ObservableCollection<Opreation>();
            OpreationData.Add(new Opreation() { MissionNumber = "1", MissionIdentity = mIdele.顶盘归零, MissionOrder = "1", PositonX = "40", PositionY = "40", AGVAngle = "45", ShellAngle = "90", Obligatie = "0" });
            one.DataContext = OpreationData;


        }
        private void NunberUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.textBlock1 == null) return;
            this.textBlock1.Text = this.NunberUpDown.Value.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.NunberUpDown.Value = 100;
        }

        private void NunberUpDown_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            newMission();
        }
    }
}