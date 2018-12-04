using System;
using System.Drawing;
using System.Xml;

namespace Canvas
{
	public struct UnitPoint
	{
		public static UnitPoint Empty;

		private double m_x;

		private double m_y;

		public bool IsEmpty
		{
			get
			{
				return double.IsNaN(this.X) && double.IsNaN(this.Y);
			}
		}

		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		public PointF Point
		{
			get
			{
				return new PointF((float)this.m_x, (float)this.m_y);
			}
		}

		public static bool operator !=(UnitPoint left, UnitPoint right)
		{
			return !(left == right);
		}

		public static bool operator ==(UnitPoint left, UnitPoint right)
		{
			bool flag = left.X == right.X;
			bool result;
			if (flag)
			{
				result = (left.Y == right.Y);
			}
			else
			{
				bool flag2 = left.IsEmpty && right.IsEmpty;
				result = flag2;
			}
			return result;
		}

		public static UnitPoint operator +(UnitPoint left, UnitPoint right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		static UnitPoint()
		{
			UnitPoint.Empty = default(UnitPoint);
			UnitPoint.Empty.m_x = double.NaN;
			UnitPoint.Empty.m_y = double.NaN;
		}

		public UnitPoint(double x, double y)
		{
			this.m_x = x;
			this.m_y = y;
		}

		public UnitPoint(PointF p)
		{
			this.m_x = (double)p.X;
			this.m_y = (double)p.Y;
		}

		public override string ToString()
		{
			return string.Format("{{X={0}, Y={1}}}", XmlConvert.ToString(Math.Round(this.X, 8)), XmlConvert.ToString(Math.Round(this.Y, 8)));
		}

		public override bool Equals(object obj)
		{
			bool flag = obj is UnitPoint;
			return flag && this == (UnitPoint)obj;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public string PosAsString()
		{
			return string.Format("[{0:f4}, {1:f4}]", this.X, this.Y);
		}
	}
}
