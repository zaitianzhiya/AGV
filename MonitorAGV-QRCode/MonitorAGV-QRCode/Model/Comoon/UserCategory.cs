using System;
using System.Collections.Generic;

namespace Model.Comoon
{
	[Serializable]
	public class UserCategory
	{
		public IList<SysOprButtonToCategory> CategoryList
		{
			get;
			set;
		}

		public bool IsNew
		{
			get;
			set;
		}

		public string CategoryCode
		{
			get;
			set;
		}

		public string CategoryName
		{
			get;
			set;
		}

		public UserCategory()
		{
			this.CategoryCode = "";
			this.CategoryName = "";
			this.IsNew = true;
			this.CategoryList = new List<SysOprButtonToCategory>();
		}
	}
}
