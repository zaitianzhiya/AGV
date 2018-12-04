using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Canvas
{
	public class CursorCollection
	{
		private Dictionary<object, Cursor> m_map = new Dictionary<object, Cursor>();

		public void AddCursor(object key, Cursor cursor)
		{
			this.m_map[key] = cursor;
		}

		public void AddCursor(object key, string resourcename)
		{
			string resource = "Resources." + resourcename;
			Type type = base.GetType();
			Cursor value = new Cursor(base.GetType(), resource);
			this.m_map[key] = value;
		}

		public Cursor GetCursor(object key)
		{
			bool flag = this.m_map.ContainsKey(key);
			Cursor result;
			if (flag)
			{
				result = this.m_map[key];
			}
			else
			{
				result = Cursors.Arrow;
			}
			return result;
		}
	}
}
