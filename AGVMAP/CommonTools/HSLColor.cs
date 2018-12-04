using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public struct HSLColor
    {
        private double m_hue;

        private double m_saturation;

        private double m_lightness;

        public double Hue
        {
            get
            {
                return this.m_hue;
            }
            set
            {
                this.m_hue = value;
            }
        }

        public double Saturation
        {
            get
            {
                return this.m_saturation;
            }
            set
            {
                this.m_saturation = value;
            }
        }

        public double Lightness
        {
            get
            {
                return this.m_lightness;
            }
            set
            {
                this.m_lightness = value;
                bool flag = this.m_lightness < 0.0;
                if (flag)
                {
                    this.m_lightness = 0.0;
                }
                bool flag2 = this.m_lightness > 1.0;
                if (flag2)
                {
                    this.m_lightness = 1.0;
                }
            }
        }

        public Color Color
        {
            get
            {
                return this.ToRGB();
            }
            set
            {
                this.FromRGB(value);
            }
        }

        public HSLColor(double hue, double saturation, double lightness)
        {
            this.m_hue = Math.Min(360.0, hue);
            this.m_saturation = Math.Min(1.0, saturation);
            this.m_lightness = Math.Min(1.0, lightness);
        }

        public HSLColor(Color color)
        {
            this.m_hue = 0.0;
            this.m_saturation = 1.0;
            this.m_lightness = 1.0;
            this.FromRGB(color);
        }

        private void FromRGB(Color cc)
        {
            double num = (double)cc.R / 255.0;
            double num2 = (double)cc.G / 255.0;
            double num3 = (double)cc.B / 255.0;
            double num4 = Math.Min(Math.Min(num, num2), num3);
            double num5 = Math.Max(Math.Max(num, num2), num3);
            this.m_hue = 0.0;
            bool flag = num4 != num5;
            if (flag)
            {
                bool flag2 = num == num5 && num2 >= num3;
                if (flag2)
                {
                    this.m_hue = 60.0 * ((num2 - num3) / (num5 - num4)) + 0.0;
                }
                else
                {
                    bool flag3 = num == num5 && num2 < num3;
                    if (flag3)
                    {
                        this.m_hue = 60.0 * ((num2 - num3) / (num5 - num4)) + 360.0;
                    }
                    else
                    {
                        bool flag4 = num2 == num5;
                        if (flag4)
                        {
                            this.m_hue = 60.0 * ((num3 - num) / (num5 - num4)) + 120.0;
                        }
                        else
                        {
                            bool flag5 = num3 == num5;
                            if (flag5)
                            {
                                this.m_hue = 60.0 * ((num - num2) / (num5 - num4)) + 240.0;
                            }
                        }
                    }
                }
            }
            this.m_lightness = (num4 + num5) / 2.0;
            bool flag6 = this.m_lightness == 0.0 || num4 == num5;
            if (flag6)
            {
                this.m_saturation = 0.0;
            }
            else
            {
                bool flag7 = this.m_lightness > 0.0 && this.m_lightness <= 0.5;
                if (flag7)
                {
                    this.m_saturation = (num5 - num4) / (2.0 * this.m_lightness);
                }
                else
                {
                    bool flag8 = this.m_lightness > 0.5;
                    if (flag8)
                    {
                        this.m_saturation = (num5 - num4) / (2.0 - 2.0 * this.m_lightness);
                    }
                }
            }
        }

        private Color ToRGB()
        {
            double lightness = this.m_lightness;
            double lightness2 = this.m_lightness;
            double lightness3 = this.m_lightness;
            bool flag = this.m_saturation == 0.0;
            Color result;
            if (flag)
            {
                result = Color.FromArgb(255, (int)(lightness * 255.0), (int)(lightness2 * 255.0), (int)(lightness3 * 255.0));
            }
            else
            {
                bool flag2 = this.m_lightness < 0.5;
                double num;
                if (flag2)
                {
                    num = this.m_lightness * (1.0 + this.m_saturation);
                }
                else
                {
                    num = this.m_lightness + this.m_saturation - this.m_lightness * this.m_saturation;
                }
                double num2 = 2.0 * this.m_lightness - num;
                double num3 = this.m_hue / 360.0;
                double[] array = new double[]
				{
					num3 + 0.33333333333333331,
					num3,
					num3 - 0.33333333333333331
				};
                double[] array2 = new double[3];
                int num4;
                for (int i = 0; i < array2.Length; i = num4 + 1)
                {
                    bool flag3 = array[i] < 0.0;
                    if (flag3)
                    {
                        array[i] += 1.0;
                    }
                    bool flag4 = array[i] > 1.0;
                    if (flag4)
                    {
                        array[i] -= 1.0;
                    }
                    bool flag5 = array[i] < 0.16666666666666666;
                    if (flag5)
                    {
                        array2[i] = num2 + (num - num2) * 6.0 * array[i];
                    }
                    else
                    {
                        bool flag6 = array[i] >= 0.16666666666666666 && array[i] < 0.5;
                        if (flag6)
                        {
                            array2[i] = num;
                        }
                        else
                        {
                            bool flag7 = array[i] >= 0.5 && array[i] < 0.66666666666666663;
                            if (flag7)
                            {
                                array2[i] = num2 + (num - num2) * 6.0 * (0.66666666666666663 - array[i]);
                            }
                            else
                            {
                                array2[i] = num2;
                            }
                        }
                    }
                    array2[i] *= 255.0;
                    num4 = i;
                }
                result = Color.FromArgb(255, (int)array2[0], (int)array2[1], (int)array2[2]);
            }
            return result;
        }

        public static bool operator !=(HSLColor left, HSLColor right)
        {
            return !(left == right);
        }

        public static bool operator ==(HSLColor left, HSLColor right)
        {
            return left.Hue == right.Hue && left.Lightness == right.Lightness && left.Saturation == right.Saturation;
        }

        public override string ToString()
        {
            return string.Format("HSL({0:f2}, {1:f2}, {2:f2})", this.Hue, this.Saturation, this.Lightness);
        }
    }
}
