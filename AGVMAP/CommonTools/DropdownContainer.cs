using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class DropdownContainer<T> : Form
    {
        private int m_captionHeight = 18;

        private int m_buttonAreaHeight = 24;

        private int m_frameMargin = 4;

        private Button m_acceptButton;

        private Button m_cancelButton;

        private DropdownContainerControl<T> m_owner;

        private Hook m_hook = new Hook();

        public virtual Rectangle WindowRectangle
        {
            get
            {
                return base.ClientRectangle;
            }
        }

        public new virtual Rectangle ClientRectangle
        {
            get
            {
                Rectangle windowRectangle = this.WindowRectangle;
                windowRectangle.Y += this.m_frameMargin + this.m_captionHeight;
                windowRectangle.Height -= this.m_frameMargin * 2 + this.m_captionHeight + this.m_buttonAreaHeight;
                windowRectangle.X += this.m_frameMargin;
                windowRectangle.Width -= this.m_frameMargin * 2;
                return windowRectangle;
            }
        }

        protected virtual Rectangle CancelButtonRect
        {
            get
            {
                Rectangle windowRectangle = this.WindowRectangle;
                windowRectangle.Y += 2;
                windowRectangle.Height = this.m_captionHeight;
                windowRectangle.X = windowRectangle.Width - (this.m_captionHeight + 2);
                windowRectangle.Width = this.m_captionHeight;
                return windowRectangle;
            }
        }

        protected virtual Rectangle AcceptButtonRect
        {
            get
            {
                Rectangle cancelButtonRect = this.CancelButtonRect;
                cancelButtonRect.X -= this.m_captionHeight + 2;
                return cancelButtonRect;
            }
        }

        public DropdownContainer(DropdownContainerControl<T> owner)
        {
            this.m_owner = owner;
            WinUtil.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 16);
            base.FormBorderStyle = FormBorderStyle.None;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            base.SetTopLevel(true);
            base.ShowInTaskbar = false;
            base.Hide();
            Rectangle windowRectangle = this.WindowRectangle;
            windowRectangle.Inflate(-2, -2);
            int y = windowRectangle.Y;
            windowRectangle.Y = y - 1;
            this.m_cancelButton = new Button();
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.Size = new Size(60, this.m_buttonAreaHeight - 2);
            this.m_cancelButton.Location = new Point(windowRectangle.Right - this.m_cancelButton.Size.Width, windowRectangle.Bottom - this.m_cancelButton.Height);
            this.m_cancelButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_cancelButton.Click += new EventHandler(this.OnCancelClick);
            base.Controls.Add(this.m_cancelButton);
            this.m_acceptButton = new Button();
            this.m_acceptButton.Text = "Accept";
            this.m_acceptButton.Size = new Size(60, this.m_buttonAreaHeight - 2);
            this.m_acceptButton.Location = new Point(this.m_cancelButton.Left - this.m_acceptButton.Size.Width - 1, windowRectangle.Bottom - this.m_acceptButton.Height);
            this.m_acceptButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_acceptButton.Click += new EventHandler(this.OnAcceptClick);
            base.Controls.Add(this.m_acceptButton);
            Hook hook = this.m_hook;
            hook.OnKeyDown = (Hook.KeyboardDelegate)Delegate.Combine(hook.OnKeyDown, new Hook.KeyboardDelegate(this.OnHookKeyDown));
        }

        public void SetControl(Control ctrl)
        {
            base.Controls.Clear();
            bool flag = this.m_acceptButton != null;
            if (flag)
            {
                base.Controls.Add(this.m_acceptButton);
            }
            bool flag2 = this.m_cancelButton != null;
            if (flag2)
            {
                base.Controls.Add(this.m_cancelButton);
            }
            Rectangle clientRectangle = this.ClientRectangle;
            base.Width += ctrl.Width - clientRectangle.Width;
            base.Height += ctrl.Height - clientRectangle.Height;
            ctrl.Location = this.ClientRectangle.Location;
            base.Controls.Add(ctrl);
            ctrl.TabIndex = 0;
            this.m_acceptButton.TabIndex = 1;
            this.m_cancelButton.TabIndex = 2;
        }

        public virtual void Cancel()
        {
            this.m_hook.SetHook(false);
            base.Hide();
            bool flag = this.m_owner != null;
            if (flag)
            {
                this.m_owner.CloseDropdown(false);
            }
        }

        public virtual void Accept()
        {
            this.m_hook.SetHook(false);
            base.Hide();
            bool flag = this.m_owner != null;
            if (flag)
            {
                this.m_owner.CloseDropdown(true);
            }
        }

        public virtual void ShowDropdown(DropdownContainerControl<T> owner)
        {
            bool flag = owner != null;
            if (flag)
            {
                this.m_owner = owner;
            }
            base.Show();
            this.m_hook.SetHook(true);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Cancel();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle windowRectangle = this.WindowRectangle;
            int num = windowRectangle.Width;
            windowRectangle.Width = num - 1;
            num = windowRectangle.Height;
            windowRectangle.Height = num - 1;
            Util.DrawFrame(e.Graphics, windowRectangle, 6f, Color.CadetBlue);
            e.Graphics.DrawImage(PopupContainerImages.Image(PopupContainerImages.eIndexes.Close), this.CancelButtonRect);
            e.Graphics.DrawImage(PopupContainerImages.Image(PopupContainerImages.eIndexes.Check), this.AcceptButtonRect);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Point pt = new Point(e.X, e.Y);
            bool flag = this.AcceptButtonRect.Contains(pt);
            if (flag)
            {
                this.Accept();
            }
            bool flag2 = this.CancelButtonRect.Contains(pt);
            if (flag2)
            {
                this.Cancel();
            }
        }

        private void OnHookKeyDown(KeyEventArgs e)
        {
            this.OnKeyDown(e);
            bool handled = e.Handled;
            if (!handled)
            {
                bool flag = e.KeyCode == Keys.Escape;
                if (flag)
                {
                    this.Cancel();
                    e.Handled = true;
                }
                bool flag2 = e.KeyCode == Keys.Return;
                if (flag2)
                {
                    this.Accept();
                    e.Handled = true;
                }
            }
        }

        private void OnAcceptClick(object sender, EventArgs e)
        {
            this.Accept();
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.Cancel();
        }
    }
}
