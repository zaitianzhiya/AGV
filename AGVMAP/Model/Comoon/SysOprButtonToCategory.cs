using System;
using System.Collections.Generic;

namespace Model.Comoon
{
	public class SysOprButtonToCategory
	{
		public string CategoryCode
		{
			get;
			set;
		}

		public int ButtonType
		{
			get;
			set;
		}

		public string ButtonName
		{
			get;
			set;
		}

		public IList<UserCategory> ButtonOfCategory
		{
			get;
			set;
		}

		public SysOprButtonToCategory()
		{
			this.CategoryCode = "";
			this.ButtonType = 0;
			this.ButtonName = "";
		}
	}
}
