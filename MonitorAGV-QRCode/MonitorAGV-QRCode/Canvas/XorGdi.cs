using System;
using System.Drawing;

namespace Canvas
{
	public static class XorGdi
	{
		private static int NULL_BRUSH = 5;

		private static int TRANSPARENT = 1;

		public static void DrawLine(PenStyles penStyle, int penWidth, Color col, Graphics grp, int X1, int Y1, int X2, int Y2)
		{
			IntPtr hdc = grp.GetHdc();
			IntPtr intPtr = GDI.CreatePen(penStyle, penWidth, GDI.RGB((int)col.R, (int)col.G, (int)col.B));
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);
			GDI.SetBkMode(hdc, XorGdi.TRANSPARENT);
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);
			IntPtr hgdiobj = GDI.SelectObject(hdc, intPtr);
			GDI.MoveToEx(hdc, X1, Y1, 0);
			GDI.LineTo(hdc, X2, Y2);
			GDI.SelectObject(hdc, hgdiobj);
			GDI.DeleteObject(intPtr);
			grp.ReleaseHdc(hdc);
		}

		public static void DrawRectangle(Graphics dc, PenStyles penStyle, int penWidth, Color col, int X1, int Y1, int X2, int Y2)
		{
			IntPtr hdc = dc.GetHdc();
			IntPtr intPtr = GDI.CreatePen(penStyle, penWidth, GDI.RGB((int)col.R, (int)col.G, (int)col.B));
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);
			GDI.SetBkMode(hdc, XorGdi.TRANSPARENT);
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);
			IntPtr hgdiobj = GDI.SelectObject(hdc, intPtr);
			IntPtr hgdiobj2 = GDI.SelectObject(hdc, GDI.GetStockObject(XorGdi.NULL_BRUSH));
			GDI.Rectangle(hdc, X1, Y1, X2, Y2);
			GDI.SelectObject(hdc, hgdiobj2);
			GDI.SelectObject(hdc, hgdiobj);
			GDI.DeleteObject(intPtr);
			dc.ReleaseHdc(hdc);
		}

		public static void DrawRectangle(Graphics dc, PenStyles penStyle, int penWidth, Color col, PointF topleft, PointF bottomright)
		{
			XorGdi.DrawRectangle(dc, penStyle, penWidth, col, (int)topleft.X, (int)topleft.Y, (int)bottomright.X, (int)bottomright.Y);
		}
	}
}
