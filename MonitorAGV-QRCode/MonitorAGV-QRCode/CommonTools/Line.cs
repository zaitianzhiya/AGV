using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class Line : Control
    {
        private AnchorStyles m_linePositions = AnchorStyles.Bottom;

        public AnchorStyles LinePositions
        {
            get
            {
                return this.m_linePositions;
            }
            set
            {
                this.m_linePositions = value;
                base.Invalidate();
            }
        }

        public Line()
        {
            base.TabStop = false;
            base.SetStyle(ControlStyles.Selectable, false);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle.Inflate(-2, -2);
            Pen pen = new Pen(this.ForeColor, 1f);
            bool flag = (this.LinePositions & AnchorStyles.Left) > AnchorStyles.None;
            if (flag)
            {
                e.Graphics.DrawLine(pen, clientRectangle.Left - 1, clientRectangle.Top, clientRectangle.Left - 1, clientRectangle.Bottom - 1);
                e.Graphics.DrawLine(Pens.WhiteSmoke, clientRectangle.Left, clientRectangle.Top, clientRectangle.Left, clientRectangle.Bottom - 1);
            }
            bool flag2 = (this.LinePositions & AnchorStyles.Right) > AnchorStyles.None;
            if (flag2)
            {
                e.Graphics.DrawLine(pen, clientRectangle.Right, clientRectangle.Top, clientRectangle.Right, clientRectangle.Bottom - 1);
                e.Graphics.DrawLine(Pens.WhiteSmoke, clientRectangle.Right + 1, clientRectangle.Top, clientRectangle.Right + 1, clientRectangle.Bottom - 1);
            }
            bool flag3 = (this.LinePositions & AnchorStyles.Top) > AnchorStyles.None;
            if (flag3)
            {
                e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Top - 1, clientRectangle.Right - 1, clientRectangle.Top - 1);
                e.Graphics.DrawLine(Pens.WhiteSmoke, clientRectangle.Left, clientRectangle.Top, clientRectangle.Right - 1, clientRectangle.Top);
            }
            bool flag4 = (this.LinePositions & AnchorStyles.Bottom) > AnchorStyles.None;
            if (flag4)
            {
                e.Graphics.DrawLine(pen, clientRectangle.Left, clientRectangle.Bottom, clientRectangle.Right - 1, clientRectangle.Bottom);
                e.Graphics.DrawLine(Pens.WhiteSmoke, clientRectangle.Left, clientRectangle.Bottom + 1, clientRectangle.Right - 1, clientRectangle.Bottom + 1);
            }
            pen.Dispose();
        }
    }
}
