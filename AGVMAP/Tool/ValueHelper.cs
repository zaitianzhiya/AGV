using System;

namespace Tools
{
	public class ValueHelper
	{
		public static bool LittleEndian;

		public static ValueHelper _Instance;

		public static ValueHelper Instance
		{
			get
			{
				bool flag = ValueHelper._Instance == null;
				if (flag)
				{
					ValueHelper._Instance = (ValueHelper.LittleEndian ? new LittleEndianValueHelper() : new ValueHelper());
				}
				return ValueHelper._Instance;
			}
		}

		static ValueHelper()
		{
			ValueHelper.LittleEndian = false;
			ValueHelper._Instance = null;
		}

		protected ValueHelper()
		{
		}

		public virtual byte[] GetBytes(short value)
		{
			return BitConverter.GetBytes(value);
		}

		public virtual byte[] GetBytes(int value)
		{
			return BitConverter.GetBytes(value);
		}

		public virtual byte[] GetBytes(float value)
		{
			return BitConverter.GetBytes(value);
		}

		public virtual byte[] GetBytes(double value)
		{
			return BitConverter.GetBytes(value);
		}

		public virtual short GetShort(byte[] data)
		{
			return BitConverter.ToInt16(data, 0);
		}

		public virtual int GetInt(byte[] data)
		{
			return BitConverter.ToInt32(data, 0);
		}

		public virtual float GetFloat(byte[] data)
		{
			return BitConverter.ToSingle(data, 0);
		}

		public virtual double GetDouble(byte[] data)
		{
			return BitConverter.ToDouble(data, 0);
		}
	}
}
