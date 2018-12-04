using System;
using System.Collections.Generic;

namespace Model.Comoon
{
	[Serializable]
	public class DetailQueryInfo
	{
		public string QueryCode
		{
			get;
			set;
		}

		public string QueryName
		{
			get;
			set;
		}

		public string sqlStr
		{
			get;
			set;
		}

		public IList<DetailCondition> Condition
		{
			get;
			set;
		}

		public IList<DetailQueryFiled> Fileds
		{
			get;
			set;
		}

		public DetailQueryInfo()
		{
			this.sqlStr = "";
			this.QueryCode = "";
			this.QueryName = "";
			this.Condition = new List<DetailCondition>();
			this.Fileds = new List<DetailQueryFiled>();
		}
	}
}
