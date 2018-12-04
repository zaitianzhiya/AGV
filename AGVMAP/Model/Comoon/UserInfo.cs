using System;

namespace Model.Comoon
{
	[Serializable]
	public class UserInfo
	{
		public string UserID
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string PassWord
		{
			get;
			set;
		}

		public bool IsNew
		{
			get;
			set;
		}

		public UserInfo()
		{
			this.UserID = "";
			this.UserName = "";
			this.PassWord = "";
			this.IsNew = true;
		}
	}
}
