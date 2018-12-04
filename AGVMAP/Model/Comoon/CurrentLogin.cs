using System;
using System.Collections.Generic;

namespace Model.Comoon
{
	public static class CurrentLogin
	{
		public static UserInfo CurrentUser
		{
			get;
			set;
		}

		public static IList<SysOprButtonToCategory> SysOprButtons
		{
			get;
			set;
		}
	}
}
