using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms; 

namespace TS_RGB
{
    /// <summary> 
    /// Summary description for cilpButton. 
    /// </summary> 
    public class cilpButton : System.Windows.Forms.Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.DrawEllipse(new Pen(Color.Red), 0, 0, this.Width, this.Height);
            g.Dispose();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            g.DrawEllipse(new Pen(this.BackColor), 0, 0, this.Width, this.Height);
            g.Dispose();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Pen pen = new Pen(this.BackColor);
            Graphics g = this.CreateGraphics();
            g.Clear(Color.Goldenrod);
            g.FillEllipse(Brushes.DarkKhaki, new Rectangle(0, 0, this.Width, this.Height));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnPaintBackground(null);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.OnClick(e);//响应click事件. 
        }
    } 
}
