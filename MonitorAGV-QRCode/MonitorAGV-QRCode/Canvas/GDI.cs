using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Canvas
{
	public class GDI
	{
		private IntPtr hdc;

		private Graphics grp;

		public void BeginGDI(Graphics g)
		{
			this.grp = g;
			this.hdc = this.grp.GetHdc();
		}

		public void EndGDI()
		{
			this.grp.ReleaseHdc(this.hdc);
		}

		public IntPtr CreatePEN(PenStyles fnPenStyle, int nWidth, int crColor)
		{
			return GDI.CreatePen(fnPenStyle, nWidth, crColor);
		}

		public bool DeleteOBJECT(IntPtr hObject)
		{
			return GDI.DeleteObject(hObject);
		}

		public IntPtr SelectObject(IntPtr hgdiobj)
		{
			return GDI.SelectObject(this.hdc, hgdiobj);
		}

		public void MoveTo(int X, int Y)
		{
			GDI.MoveToEx(this.hdc, X, Y, 0);
		}

		public void LineTo(int X, int Y)
		{
			GDI.LineTo(this.hdc, X, Y);
		}

		public int SetROP2(drawingMode fnDrawMode)
		{
			return GDI.SetROP2(this.hdc, fnDrawMode);
		}

		public void SetPixel(int x, int y, int color)
		{
			GDI.SetPixelV(this.hdc, x, y, color & 16777215);
		}

		[DllImport("gdi32.dll")]
		public static extern void SetPixelV(IntPtr hdc, int x, int y, int color);

		[DllImport("gdi32.dll")]
		public static extern int SetROP2(IntPtr hdc, drawingMode fnDrawMode);

		[DllImport("gdi32.dll")]
		public static extern bool MoveToEx(IntPtr hdc, int X, int Y, int oldp);

		[DllImport("gdi32.dll")]
		public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreatePen(PenStyles fnPenStyle, int nWidth, int crColor);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		public static extern void Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

		[DllImport("gdi32.dll")]
		public static extern IntPtr GetStockObject(int brStyle);

		[DllImport("gdi32.dll")]
		public static extern int SetBkMode(IntPtr hdc, int iBkMode);

		public static int RGB(int R, int G, int B)
		{
			return R | G << 8 | B << 16;
		}
	}
}
