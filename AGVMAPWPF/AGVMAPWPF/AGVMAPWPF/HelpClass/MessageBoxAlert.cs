using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AGVMAPWPF
{
    class MessageBoxAlert
    {
        public static MessageBoxResult Show(string msg,MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Exclamation:
                    return MessageBox.Show(msg, "提示", MessageBoxButton.OK, image, MessageBoxResult.OK);
                case MessageBoxImage.Asterisk:
                    return MessageBox.Show(msg, "提示", MessageBoxButton.OK, image, MessageBoxResult.OK);
                case MessageBoxImage.Question:
                    return MessageBox.Show(msg, "询问", MessageBoxButton.YesNo, image, MessageBoxResult.Yes);
                default:
                    return MessageBox.Show(msg);
            }
        }
    }
}
