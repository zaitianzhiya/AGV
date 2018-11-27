using System;

namespace Model.MDM
{
	[Serializable]
	public class ActionInfo
	{
		public int ActionID
		{
			get;
			set;
		}

		public string ActionName
		{
			get;
			set;
		}

		public double WaitTime
		{
			get;
			set;
		}

		public string CommondText
		{
			get;
			set;
		}

		public ActionInfo()
		{
			this.ActionName = "";
			this.WaitTime = 0.0;
			this.CommondText = "";
		}
	}
}
