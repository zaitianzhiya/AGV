using CommonTools;
using System;

namespace Canvas
{
	internal class PropertyUtil
	{
		public static object ChangeType(object value, Type type)
		{
			bool flag = type == typeof(UnitPoint);
			object result;
			if (flag)
			{
                result = PropertyUtil.Parse(value.ToString(), type);
			}
			else
			{
                result = CommonTools.PropertyUtil.ChangeType(value, type);
			}
			return result;
		}

		public static object Parse(string value, Type type)
		{
			bool flag = type == typeof(UnitPoint);
			object result;
			if (flag)
			{
                result = CommonTools.PropertyUtil.Parse(new UnitPoint(0.0, 0.0), value);
			}
			else
			{
                result = CommonTools.PropertyUtil.Parse(value, type);
			}
			return result;
		}
	}
}
