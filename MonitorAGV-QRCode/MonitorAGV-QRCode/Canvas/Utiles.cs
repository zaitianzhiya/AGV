using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;

namespace Canvas
{
	public class Utiles
	{
		public static IDrawObject FindObjectTypeInList(IDrawObject caller, List<IDrawObject> list, Type type)
		{
			IDrawObject result;
			foreach (IDrawObject current in list)
			{
				bool flag = caller == current;
				if (!flag)
				{
					bool flag2 = current.GetType() == type;
					if (flag2)
					{
						result = current;
						return result;
					}
				}
			}
			result = null;
			return result;
		}
	}
}
