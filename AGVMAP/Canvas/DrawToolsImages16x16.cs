using System;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
{
	public class DrawToolsImages16x16
	{
		public enum eIndexes
		{
			Select,
			Pan,
			Move,
			Line,
			CircleCR,
			Circle2P,
			ArcCR,
			Arc2P,
			Arc3P132,
			Arc3P123
		}

		private static ImageList m_imageList = null;

		public static ImageList ImageList()
		{
			Type typeFromHandle = typeof(MenuImages16x16);
			bool flag = DrawToolsImages16x16.m_imageList == null;
			if (flag)
			{
				DrawToolsImages16x16.m_imageList = ImagesUtil.GetToolbarImageList(typeFromHandle, "Resources.drawtoolimages.bmp", new Size(16, 16), Color.White);
			}
			return DrawToolsImages16x16.m_imageList;
		}

		public static Image Image(DrawToolsImages16x16.eIndexes index)
		{
			return DrawToolsImages16x16.ImageList().Images[(int)index];
		}
	}
}
