using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class SelectorImages
    {
        public enum eIndexes
        {
            Right,
            Left,
            Up,
            Down,
            Donut
        }

        private static ImageList m_imageList = null;

        public static ImageList ImageList()
        {
            Type typeFromHandle = typeof(SelectorImages);
            bool flag = SelectorImages.m_imageList == null;
            if (flag)
            {
                SelectorImages.m_imageList = ImagesUtil.GetToolbarImageList(typeFromHandle, "ColorPickerCtrl.Resources.colorbarIndicators.bmp", new Size(12, 12), Color.Magenta);
            }
            return SelectorImages.m_imageList;
        }

        public static Image Image(SelectorImages.eIndexes index)
        {
            return SelectorImages.ImageList().Images[(int)index];
        }
    }
}
