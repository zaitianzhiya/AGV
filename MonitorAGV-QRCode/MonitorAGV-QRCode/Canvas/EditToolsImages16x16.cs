using System;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
{
	public class EditToolsImages16x16
	{
		public enum eIndexes
		{
			Meet2Lines,
			LineSrhinkExtend
		}

		private static ImageList m_imageList = null;

		public static ImageList ImageList()
		{
			Type typeFromHandle = typeof(MenuImages16x16);
			bool flag = EditToolsImages16x16.m_imageList == null;
			if (flag)
			{
				EditToolsImages16x16.m_imageList = ImagesUtil.GetToolbarImageList(typeFromHandle, "Resources.edittoolimages.bmp", new Size(16, 16), Color.White);
			}
			return EditToolsImages16x16.m_imageList;
		}

		public static Image Image(EditToolsImages16x16.eIndexes index)
		{
			return EditToolsImages16x16.ImageList().Images[(int)index];
		}
	}
}
