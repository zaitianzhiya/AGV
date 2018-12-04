using System;

namespace Tools
{
	internal class LittleEndianValueHelper : ValueHelper
	{
		public override byte[] GetBytes(short value)
		{
			return this.Reverse(BitConverter.GetBytes(value));
		}

		public override byte[] GetBytes(int value)
		{
			return this.Reverse(BitConverter.GetBytes(value));
		}

		public override byte[] GetBytes(float value)
		{
			return this.Reverse(BitConverter.GetBytes(value));
		}

		public override byte[] GetBytes(double value)
		{
			return this.Reverse(BitConverter.GetBytes(value));
		}

		public new virtual short GetShort(byte[] data)
		{
			return BitConverter.ToInt16(this.Reverse(data), 0);
		}

		public new virtual int GetInt(byte[] data)
		{
			return BitConverter.ToInt32(this.Reverse(data), 0);
		}

		public new virtual float GetFloat(byte[] data)
		{
			return BitConverter.ToSingle(this.Reverse(data), 0);
		}

		public new virtual double GetDouble(byte[] data)
		{
			return BitConverter.ToDouble(this.Reverse(data), 0);
		}

		private byte[] Reverse(byte[] data)
		{
			Array.Reverse(data);
			return data;
		}
	}
}
