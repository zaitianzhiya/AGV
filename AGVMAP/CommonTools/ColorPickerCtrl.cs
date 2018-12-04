using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ColorPickerCtrl : UserControl
    {
        private Color m_selectedColor = Color.AntiqueWhite;

        private float m_opacity = 1f;

        private bool lockColorTable = false;

        private IContainer components = null;

        private ColorWheelCtrl m_colorWheel;

        private ColorSlider m_opacitySlider;

        private Panel panel1;

        private EyedropColorPicker m_eyedropColorPicker;

        private ToolTip m_tooltip;

        private ColorTable m_colorTable;

        private LabelRotate m_infoLabel;

        private LabelRotate m_colorSample;

        public Color SelectedColor
        {
            get
            {
                return Color.FromArgb((int)Math.Floor((double)(255f * this.m_opacity)), this.m_selectedColor);
            }
            set
            {
                this.m_opacity = (float)value.A / 255f;
                value = Color.FromArgb(255, value);
                this.m_colorWheel.SelectedColor = value;
                bool flag = !this.m_colorTable.ColorExist(value);
                if (flag)
                {
                    this.m_colorTable.SetCustomColor(value);
                }
                this.m_colorTable.SelectedItem = value;
                this.m_opacitySlider.Percent = this.m_opacity;
            }
        }

        public ColorPickerCtrl()
        {
            this.InitializeComponent();
            List<Color> list = new List<Color>();
            float num = 12f;
            for (float num2 = 0f; num2 <= 100f; num2 += num)
            {
                float num3 = 255f * num2 / 100f;
                list.Add(Color.FromArgb(255, (int)num3, (int)num3, (int)num3));
            }
            list.Add(Color.White);
            list.Add(Color.FromArgb(255, 255, 0, 0));
            list.Add(Color.FromArgb(255, 255, 255, 0));
            list.Add(Color.FromArgb(255, 0, 255, 0));
            list.Add(Color.FromArgb(255, 0, 255, 255));
            list.Add(Color.FromArgb(255, 0, 0, 255));
            list.Add(Color.FromArgb(255, 255, 0, 255));
            int num4 = 16;
            int num5 = 3;
            float num6 = (float)(num5 * num4);
            float num7 = 360f / num6;
            for (float num8 = 0f; num8 < 360f; num8 += num7)
            {
                list.Add(new HSLColor((double)num8, 1.0, 0.5).Color);
            }
            for (float num8 = 0f; num8 < 360f; num8 += num7)
            {
                list.Add(new HSLColor((double)num8, 0.5, 0.5).Color);
            }
            this.m_colorTable.Colors = list.ToArray();
            this.m_colorTable.Cols = num4;
            this.m_colorTable.SelectedIndexChanged += new EventHandler(this.OnColorTableSelectionChanged);
            this.m_colorWheel.SelectedColorChanged += new EventHandler(this.OnColorWheelSelectionChanged);
            this.m_opacitySlider.SelectedValueChanged += new EventHandler(this.OnOpacityValueChanged);
            this.m_eyedropColorPicker.SelectedColorChanged += new EventHandler(this.OnEyeDropperSelectionChanged);
            this.m_colorSample.Paint += new PaintEventHandler(this.OnColorSamplePaint);
        }

        private void OnEyeDropperSelectionChanged(object sender, EventArgs e)
        {
            this.m_colorWheel.SelectedColor = this.m_eyedropColorPicker.SelectedColor;
        }

        private void OnOpacityValueChanged(object sender, EventArgs e)
        {
            this.m_opacity = Math.Max(0f, this.m_opacitySlider.Percent);
            this.m_opacity = Math.Min(1f, this.m_opacitySlider.Percent);
            this.m_colorSample.Refresh();
            this.UpdateInfo();
        }

        private void OnColorWheelSelectionChanged(object sender, EventArgs e)
        {
            Color selectedColor = this.m_colorWheel.SelectedColor;
            bool flag = selectedColor != this.m_selectedColor;
            if (flag)
            {
                this.m_selectedColor = selectedColor;
                this.m_colorSample.Refresh();
                bool flag2 = !this.lockColorTable && selectedColor != this.m_colorTable.SelectedItem;
                if (flag2)
                {
                    this.m_colorTable.SetCustomColor(selectedColor);
                }
            }
            this.UpdateInfo();
        }

        private void UpdateInfo()
        {
            Color color = Color.FromArgb((int)Math.Floor((double)(255f * this.m_opacity)), this.m_selectedColor);
            string text = string.Format("{0} aRGB({1}, {2}, {3}, {4})", new object[]
			{
				this.m_colorWheel.SelectedHSLColor.ToString(),
				color.A,
				color.R,
				color.G,
				color.B
			});
            this.m_infoLabel.Text = text;
        }

        private void OnColorSamplePaint(object sender, PaintEventArgs e)
        {
            Rectangle clientRectangle = this.m_colorSample.ClientRectangle;
            clientRectangle.Inflate(-4, -4);
            int width = clientRectangle.Width;
            clientRectangle.Width /= 2;
            Color color = Color.FromArgb((int)Math.Floor((double)(255f * this.m_opacity)), this.m_selectedColor);
            SolidBrush brush = new SolidBrush(color);
            e.Graphics.FillRectangle(brush, clientRectangle);
            clientRectangle.X += clientRectangle.Width;
            e.Graphics.FillRectangle(Brushes.White, clientRectangle);
            color = Color.FromArgb(255, this.m_selectedColor);
            brush = new SolidBrush(color);
            e.Graphics.FillRectangle(brush, clientRectangle);
        }

        private void OnColorTableSelectionChanged(object sender, EventArgs e)
        {
            Color selectedItem = this.m_colorTable.SelectedItem;
            bool flag = selectedItem != this.m_selectedColor;
            if (flag)
            {
                this.lockColorTable = true;
                this.m_colorWheel.SelectedColor = selectedItem;
                this.lockColorTable = false;
                this.m_colorSample.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.panel1 = new Panel();
            this.m_tooltip = new ToolTip(this.components);
            this.m_colorSample = new LabelRotate();
            this.m_infoLabel = new LabelRotate();
            this.m_colorTable = new ColorTable();
            this.m_eyedropColorPicker = new EyedropColorPicker();
            this.m_colorWheel = new ColorWheelCtrl();
            this.m_opacitySlider = new ColorSlider();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.Transparent;
            this.panel1.Controls.Add(this.m_colorWheel);
            this.panel1.Controls.Add(this.m_opacitySlider);
            this.panel1.Location = new Point(257, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(254, 242);
            this.panel1.TabIndex = 9;
            this.m_colorSample.Location = new Point(1, 150);
            this.m_colorSample.Name = "m_colorSample";
            this.m_colorSample.RotatePointAlignment = ContentAlignment.MiddleCenter;
            this.m_colorSample.Size = new Size(186, 60);
            this.m_colorSample.TabIndex = 1;
            this.m_colorSample.TabStop = false;
            this.m_colorSample.TextAlign = ContentAlignment.MiddleLeft;
            this.m_colorSample.TextAngle = 0f;
            this.m_infoLabel.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.m_infoLabel.Location = new Point(1, 217);
            this.m_infoLabel.Name = "m_infoLabel";
            this.m_infoLabel.RotatePointAlignment = ContentAlignment.MiddleCenter;
            this.m_infoLabel.Size = new Size(252, 28);
            this.m_infoLabel.TabIndex = 3;
            this.m_infoLabel.TabStop = false;
            this.m_infoLabel.Text = "This is some sample text";
            this.m_infoLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.m_infoLabel.TextAngle = 0f;
            this.m_colorTable.Cols = 16;
            this.m_colorTable.FieldSize = new Size(12, 12);
            this.m_colorTable.Location = new Point(1, 7);
            this.m_colorTable.Name = "m_colorTable";
            this.m_colorTable.Padding = new Padding(8, 8, 0, 0);
            this.m_colorTable.RotatePointAlignment = ContentAlignment.MiddleCenter;
            this.m_colorTable.SelectedItem = Color.Black;
            this.m_colorTable.Size = new Size(252, 138);
            this.m_colorTable.TabIndex = 0;
            this.m_colorTable.Text = "m_colorTable";
            this.m_colorTable.TextAlign = ContentAlignment.MiddleLeft;
            this.m_colorTable.TextAngle = 0f;
            this.m_eyedropColorPicker.BackColor = SystemColors.Control;
            this.m_eyedropColorPicker.Location = new Point(193, 150);
            this.m_eyedropColorPicker.Name = "m_eyedropColorPicker";
            this.m_eyedropColorPicker.SelectedColor = Color.Empty;
            this.m_eyedropColorPicker.Size = new Size(60, 60);
            this.m_eyedropColorPicker.TabIndex = 2;
            this.m_eyedropColorPicker.TabStop = false;
            this.m_tooltip.SetToolTip(this.m_eyedropColorPicker, "Color Selector. Click and Drag to pick a color from the screen");
            this.m_eyedropColorPicker.Zoom = 4;
            this.m_colorWheel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.m_colorWheel.BackColor = Color.Transparent;
            this.m_colorWheel.Location = new Point(-1, 0);
            this.m_colorWheel.Name = "m_colorWheel";
            this.m_colorWheel.SelectedColor = Color.FromArgb(252, 235, 205);
            this.m_colorWheel.Size = new Size(254, 209);
            this.m_colorWheel.TabIndex = 0;
            this.m_opacitySlider.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.m_opacitySlider.BackColor = Color.Transparent;
            this.m_opacitySlider.BarPadding = new Padding(60, 12, 80, 25);
            this.m_opacitySlider.Color1 = Color.White;
            this.m_opacitySlider.Color2 = Color.Black;
            this.m_opacitySlider.Color3 = Color.Black;
            this.m_opacitySlider.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.m_opacitySlider.ForeColor = Color.Black;
            this.m_opacitySlider.Location = new Point(2, 213);
            this.m_opacitySlider.Name = "m_opacitySlider";
            this.m_opacitySlider.NumberOfColors = ColorSlider.eNumberOfColors.Use2Colors;
            this.m_opacitySlider.Orientation = Orientation.Horizontal;
            this.m_opacitySlider.Padding = new Padding(5, 0, 0, 0);
            this.m_opacitySlider.Percent = 1f;
            this.m_opacitySlider.RotatePointAlignment = ContentAlignment.MiddleCenter;
            this.m_opacitySlider.Size = new Size(248, 28);
            this.m_opacitySlider.TabIndex = 1;
            this.m_opacitySlider.Text = "Opacity";
            this.m_opacitySlider.TextAlign = ContentAlignment.MiddleLeft;
            this.m_opacitySlider.TextAngle = 0f;
            this.m_opacitySlider.ValueOrientation = ColorSlider.eValueOrientation.MinToMax;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.m_colorSample);
            base.Controls.Add(this.m_infoLabel);
            base.Controls.Add(this.m_colorTable);
            base.Controls.Add(this.m_eyedropColorPicker);
            base.Controls.Add(this.panel1);
            base.Name = "ColorPickerCtrl";
            base.Padding = new Padding(3, 3, 0, 0);
            base.Size = new Size(507, 250);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
