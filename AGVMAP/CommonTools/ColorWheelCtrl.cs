using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonTools
{
    public class ColorWheelCtrl : UserControl
    {
        private HSLColor m_selectedColor = new HSLColor(Color.Wheat);

        private IContainer components = null;

        private HSLColorSlider m_colorBar;

        private ColorWheel m_colorWheel;

        [method: CompilerGenerated]
        //[DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        public event EventHandler SelectedColorChanged;

        public Color SelectedColor
        {
            get
            {
                return this.m_selectedColor.Color;
            }
            set
            {
                bool flag = this.m_selectedColor.Color != value;
                if (flag)
                {
                    this.SelectedHSLColor = new HSLColor(value);
                }
            }
        }

        public HSLColor SelectedHSLColor
        {
            get
            {
                return this.m_selectedColor;
            }
            set
            {
                this.m_colorBar.SelectedHSLColor = value;
                this.m_colorWheel.SelectedHSLColor = value;
                this.m_selectedColor = value;
                bool flag = this.SelectedColorChanged != null;
                if (flag)
                {
                    this.SelectedColorChanged(this, null);
                }
            }
        }

        public ColorWheelCtrl()
        {
            this.InitializeComponent();
            this.m_colorWheel.SelectedColorChanged += new EventHandler(this.OnWheelColorChanged);
            this.m_colorBar.SelectedValueChanged += new EventHandler(this.OnLightnessColorChanged);
            this.m_colorBar.ValueOrientation = ColorSlider.eValueOrientation.MaxToMin;
        }

        private void OnLightnessColorChanged(object sender, EventArgs e)
        {
            this.m_selectedColor.Lightness = this.m_colorBar.SelectedHSLColor.Lightness;
            this.SelectedHSLColor = this.m_selectedColor;
        }

        private void OnWheelColorChanged(object sender, EventArgs e)
        {
            this.m_selectedColor.Hue = this.m_colorWheel.SelectedHSLColor.Hue;
            this.m_selectedColor.Saturation = this.m_colorWheel.SelectedHSLColor.Saturation;
            this.SelectedHSLColor = this.m_selectedColor;
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
            this.m_colorWheel = new ColorWheel();
            this.m_colorBar = new HSLColorSlider();
            base.SuspendLayout();
            this.m_colorWheel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.m_colorWheel.Location = new Point(3, 3);
            this.m_colorWheel.Name = "m_colorWheel";
            this.m_colorWheel.Size = new Size(236, 200);
            this.m_colorWheel.TabIndex = 0;
            this.m_colorWheel.Text = "colorWheel1";
            this.m_colorBar.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
            this.m_colorBar.BarPadding = new Padding(12, 5, 32, 10);
            this.m_colorBar.Color1 = Color.Black;
            this.m_colorBar.Color2 = Color.FromArgb(127, 127, 127);
            this.m_colorBar.Color3 = Color.White;
            this.m_colorBar.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.m_colorBar.Location = new Point(246, 3);
            this.m_colorBar.Name = "m_colorBar";
            this.m_colorBar.Orientation = Orientation.Vertical;
            this.m_colorBar.Padding = new Padding(0, 0, 1, 0);
            this.m_colorBar.Percent = 0f;
            this.m_colorBar.RotatePointAlignment = ContentAlignment.MiddleRight;
            this.m_colorBar.Size = new Size(46, 200);
            this.m_colorBar.TabIndex = 1;
            this.m_colorBar.Text = "Lightness";
            this.m_colorBar.TextAlign = ContentAlignment.BottomCenter;
            this.m_colorBar.TextAngle = 270f;
            this.m_colorBar.ValueOrientation = ColorSlider.eValueOrientation.MinToMax;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.m_colorWheel);
            base.Controls.Add(this.m_colorBar);
            base.Name = "ColorWheelCtrl";
            base.Size = new Size(296, 206);
            base.ResumeLayout(false);
        }
    }
}
