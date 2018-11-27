using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ColorPickerCombobox : DropdownContainerControl<Color>
    {
        private static ColorPickerCtrl m_colorPicker;

        private static DropdownContainer<Color> m_container;

        public new Color SelectedItem
        {
            get
            {
                return base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
            }
        }

        public ColorPickerCombobox()
        {
            this.SelectedItem = Color.Wheat;
        }

        protected void CreateDropdown()
        {
            bool flag = ColorPickerCombobox.m_container == null;
            if (flag)
            {
                ColorPickerCombobox.m_container = new DropdownContainer<Color>(this);
                ColorPickerCombobox.m_colorPicker = new ColorPickerCtrl();
                ColorPickerCombobox.m_container.SetControl(ColorPickerCombobox.m_colorPicker);
            }
            base.DropdownContainer = ColorPickerCombobox.m_container;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void ShowDropdown()
        {
            this.CreateDropdown();
            ColorPickerCombobox.m_colorPicker.SelectedColor = this.SelectedItem;
            base.DropdownContainer.KeyDown += new KeyEventHandler(this.OnDropdownKeyDown);
            base.ShowDropdown();
        }

        public override void CloseDropdown(bool acceptValue)
        {
            base.DropdownContainer.KeyDown -= new KeyEventHandler(this.OnDropdownKeyDown);
            base.CloseDropdown(acceptValue);
            if (acceptValue)
            {
                this.SelectedItem = ColorPickerCombobox.m_colorPicker.SelectedColor;
            }
        }

        protected override void DrawItem(Graphics dc, Rectangle itemrect)
        {
            Brush brush = new SolidBrush(this.SelectedItem);
            dc.FillRectangle(brush, itemrect);
        }

        private void OnDropdownKeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Space;
            if (flag)
            {
                e.Handled = true;
                this.CloseDropdown(true);
            }
        }
    }
}
