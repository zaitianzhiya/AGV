using System;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
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
