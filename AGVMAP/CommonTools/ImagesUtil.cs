using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class ImagesUtil
    {
        public static ImageList GetToolbarImageList(Type type, string resourceName, Size imageSize, Color transparentColor)
        {
            Bitmap value = new Bitmap(type, resourceName);
            ImageList imageList = new ImageList();
            imageList.ImageSize = imageSize;
            imageList.TransparentColor = transparentColor;
            imageList.Images.AddStrip(value);
            imageList.ColorDepth = ColorDepth.Depth24Bit;
            return imageList;
        }
    }
}
