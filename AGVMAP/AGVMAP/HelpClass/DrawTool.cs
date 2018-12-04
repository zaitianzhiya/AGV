using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVMAP.HelpClass
{
    public class DrawTool
    {
        public static void DrawLine(Graphics graphics, Pen pen, PointF p1, PointF p2)
        {
            graphics.DrawLine(pen, p1, p2);
        }
    }
}
