using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CommonTools
{
    public class DropdownContainerControl<T> : Control
    {
        private T m_selectedItem;

        private bool m_mouseIn = false;

        private DropdownContainer<T> m_container = null;

        public T SelectedItem
        {
            get
            {
                return this.m_selectedItem;
            }
            set
            {
                this.m_selectedItem = value;
                base.Invalidate();
            }
        }

        public DropdownContainer<T> DropdownContainer
        {
            get
            {
                return this.m_container;
            }
            set
            {
                this.m_container = value;
            }
        }

        public bool DroppedDown
        {
            get
            {
                return this.m_container.Visible;
            }
        }

        public Rectangle ItemRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                clientRectangle.Y += 2;
                clientRectangle.Height -= 4;
                clientRectangle.X += 2;
                clientRectangle.Width = this.ButtonRectangle.Left - 4;
                return clientRectangle;
            }
        }

        public Rectangle ButtonRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                clientRectangle.Y++;
                clientRectangle.Height -= 2;
                clientRectangle.X = clientRectangle.Right - 18;
                clientRectangle.Width = 17;
                return clientRectangle;
            }
        }

        public DropdownContainerControl()
        {
            this.DoubleBuffered = true;
            this.m_container = new DropdownContainer<T>(this);
        }

        public virtual void CloseDropdown(bool acceptValue)
        {
            this.HideDropdown();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle bounds = base.ClientRectangle;
            ComboBoxRenderer.DrawTextBox(e.Graphics, bounds, ComboBoxState.Normal);
            bounds = this.ButtonRectangle;
            bool mouseIn = this.m_mouseIn;
            if (mouseIn)
            {
                ComboBoxRenderer.DrawDropDownButton(e.Graphics, bounds, ComboBoxState.Hot);
            }
            else
            {
                ComboBoxRenderer.DrawDropDownButton(e.Graphics, bounds, ComboBoxState.Normal);
            }
            this.ItemRectangle.Inflate(-1, -1);
            this.DrawItem(e.Graphics, this.ItemRectangle);
            bool focused = this.Focused;
            if (focused)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, this.ItemRectangle);
            }
            base.RaisePaintEvent(this, e);
        }

        protected virtual void DrawItem(Graphics dc, Rectangle itemrect)
        {
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            base.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                base.Focus();
                bool droppedDown = this.DroppedDown;
                if (droppedDown)
                {
                    this.HideDropdown();
                }
                else
                {
                    this.ShowDropdown();
                }
            }
        }

        protected virtual void ShowDropdown()
        {
            Point location = base.Parent.PointToScreen(base.Location);
            location.Y += base.Height + 2;
            Rectangle rectangle = base.Parent.RectangleToScreen(base.Bounds);
            Rectangle workingArea = Screen.GetWorkingArea(this);
            bool flag = location.X + this.m_container.Width > workingArea.Right;
            if (flag)
            {
                location.X = rectangle.Right - this.m_container.Width;
            }
            bool flag2 = location.X < 0;
            if (flag2)
            {
                location.X = 0;
            }
            bool flag3 = location.Y + this.m_container.Height > workingArea.Bottom;
            if (flag3)
            {
                location.Y = rectangle.Top - this.m_container.Height;
            }
            bool flag4 = location.Y < 0;
            if (flag4)
            {
                location.Y = 0;
            }
            this.m_container.Location = location;
            this.m_container.ShowDropdown(this);
        }

        protected virtual void HideDropdown()
        {
            this.m_container.Hide();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.m_mouseIn = true;
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.m_mouseIn = false;
            base.Invalidate();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            bool flag = keyData == Keys.Down && !this.DroppedDown;
            bool result;
            if (flag)
            {
                this.ShowDropdown();
                result = true;
            }
            else
            {
                result = base.ProcessDialogKey(keyData);
            }
            return result;
        }
    }
}
