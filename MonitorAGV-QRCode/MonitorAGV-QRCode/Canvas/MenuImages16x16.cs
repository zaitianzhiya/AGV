using System;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
{
	public class MenuImages16x16
	{
		public enum eIndexes
		{
			Undo,
			Redo,
			NewDocument,
			OpenDocument,
			SaveDocument
		}

		private static ImageList m_imageList = null;

		public static ImageList ImageList()
		{
			Type typeFromHandle = typeof(MenuImages16x16);
			bool flag = MenuImages16x16.m_imageList == null;
			if (flag)
			{
				MenuImages16x16.m_imageList = ImagesUtil.GetToolbarImageList(typeFromHandle, "Resources.menuimages.bmp", new Size(16, 16), Color.White);
			}
			return MenuImages16x16.m_imageList;
		}

		public static Image Image(MenuImages16x16.eIndexes index)
		{
			return MenuImages16x16.ImageList().Images[(int)index];
		}
	}
}
