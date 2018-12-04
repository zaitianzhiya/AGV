using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    internal class HSLColorSlider : ColorSlider
    {
        private HSLColor m_selectedColor = default(HSLColor);

        public HSLColor SelectedHSLColor
        {
            get
            {
                return this.m_selectedColor;
            }
            set
            {
                bool flag = this.m_selectedColor == value;
                if (!flag)
                {
                    this.m_selectedColor = value;
                    value.Lightness = 0.5;
                    base.Color2 = Color.FromArgb(255, value.Color);
                    base.Percent = (float)this.m_selectedColor.Lightness;
                    this.Refresh();
                }
            }
        }

        protected override void SetPercent(PointF mousepoint)
        {
            base.SetPercent(mousepoint);
            this.m_selectedColor.Lightness = (double)base.Percent;
            this.Refresh();
        }

        protected override void SetPercent(float percent)
        {
            base.SetPercent(percent);
            this.m_selectedColor.Lightness = (double)(percent / 100f);
            this.SelectedHSLColor = this.m_selectedColor;
        }
    }
}
