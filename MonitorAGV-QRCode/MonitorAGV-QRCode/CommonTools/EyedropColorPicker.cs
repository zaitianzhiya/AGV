using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    internal class EyedropColorPicker : Control
    {
        private Bitmap m_snapshot;

        private Bitmap m_icon;

        private Color m_selectedColor;

        private float m_zoom = 4f;

        private bool iscapturing = false;

        [method: CompilerGenerated]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        public event EventHandler SelectedColorChanged;

        public int Zoom
        {
            get
            {
                return (int)this.m_zoom;
            }
            set
            {
                this.m_zoom = (float)value;
                this.RecalcSnapshotSize();
            }
        }

        public Color SelectedColor
        {
            get
            {
                return this.m_selectedColor;
            }
            set
            {
                bool flag = this.m_selectedColor == value;
                if (flag)
                {
                }
            }
        }

        private RectangleF ImageRect
        {
            get
            {
                return Util.Rect(base.ClientRectangle);
            }
        }

        public EyedropColorPicker()
        {
            this.DoubleBuffered = true;
            this.m_icon = new Bitmap(typeof(EyedropColorPicker), "ColorPickerCtrl.Resources.eyedropper.bmp");
            this.m_icon.MakeTransparent(Color.Magenta);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.RecalcSnapshotSize();
        }

        private void RecalcSnapshotSize()
        {
            bool flag = this.m_snapshot != null;
            if (flag)
            {
                this.m_snapshot.Dispose();
            }
            RectangleF imageRect = this.ImageRect;
            int width = (int)Math.Floor((double)(imageRect.Width / (float)this.Zoom));
            int height = (int)Math.Floor((double)(imageRect.Height / (float)this.Zoom));
            this.m_snapshot = new Bitmap(width, height);
        }

        private void GetSnapshot()
        {
            Point mousePosition = Control.MousePosition;
            mousePosition.X -= this.m_snapshot.Width / 2;
            mousePosition.Y -= this.m_snapshot.Height / 2;
            using (Graphics graphics = Graphics.FromImage(this.m_snapshot))
            {
                graphics.CopyFromScreen(mousePosition, new Point(0, 0), this.m_snapshot.Size);
                this.Refresh();
                PointF pointF = Util.Center(new RectangleF(0f, 0f, (float)this.m_snapshot.Size.Width, (float)this.m_snapshot.Size.Height));
                Color pixel = this.m_snapshot.GetPixel((int)Math.Round((double)pointF.X), (int)Math.Round((double)pointF.Y));
                bool flag = pixel != this.m_selectedColor;
                if (flag)
                {
                    this.m_selectedColor = pixel;
                    bool flag2 = this.SelectedColorChanged != null;
                    if (flag2)
                    {
                        this.SelectedColorChanged(this, null);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle clientRectangle = base.ClientRectangle;
            bool flag = this.m_snapshot != null;
            if (flag)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                RectangleF empty = RectangleF.Empty;
                empty.Width = (float)(this.m_snapshot.Size.Width * this.Zoom);
                empty.Height = (float)(this.m_snapshot.Size.Height * this.Zoom);
                empty.X = 0f;
                empty.Y = 0f;
                e.Graphics.DrawImage(this.m_snapshot, empty);
                bool flag2 = this.iscapturing;
                if (flag2)
                {
                    PointF pf = Util.Center(empty);
                    Rectangle rect = new Rectangle(Util.Point(pf), new Size(0, 0));
                    rect.X -= this.Zoom / 2 - 1;
                    rect.Y -= this.Zoom / 2 - 1;
                    rect.Width = this.Zoom;
                    rect.Height = this.Zoom;
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                }
                else
                {
                    int num = 3;
                    e.Graphics.FillRectangle(SystemBrushes.Control, new Rectangle(new Point(num, num), this.m_icon.Size));
                    e.Graphics.DrawImage(this.m_icon, num, num);
                }
            }
            Pen pen = new Pen(this.BackColor, 3f);
            clientRectangle.Inflate(-1, -1);
            e.Graphics.DrawRectangle(pen, clientRectangle);
            Util.DrawFrame(e.Graphics, clientRectangle, 6f, Color.CadetBlue);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            bool flag = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            if (flag)
            {
                this.Cursor = Cursors.Cross;
                this.iscapturing = true;
                base.Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool flag = (e.Button & MouseButtons.Left) == MouseButtons.Left;
            if (flag)
            {
                this.GetSnapshot();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Cursor = Cursors.Arrow;
            this.iscapturing = false;
            base.Invalidate();
        }
    }
}
