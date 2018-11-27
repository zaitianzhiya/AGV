using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class PopupContainerImages
    {
        public enum eIndexes
        {
            Close,
            Check
        }

        private static ImageList m_imageList = null;

        public static ImageList ImageList()
        {
            Type typeFromHandle = typeof(SelectorImages);
            bool flag = PopupContainerImages.m_imageList == null;
            if (flag)
            {
                PopupContainerImages.m_imageList = ImagesUtil.GetToolbarImageList(typeFromHandle, "ColorPickerCtrl.Resources.popupcontainerbuttons.bmp", new Size(16, 16), Color.Magenta);
            }
            return PopupContainerImages.m_imageList;
        }

        public static Image Image(PopupContainerImages.eIndexes index)
        {
            return PopupContainerImages.ImageList().Images[(int)index];
        }
    }
}
