using System;

namespace Model.MDM
{
	[Serializable]
	public class CmdInfo
	{
		public int CmdCode
		{
			get;
			set;
		}

		public string CmdName
		{
			get;
			set;
		}

		public string CmdOrder
		{
			get;
			set;
		}

		public CmdInfo()
		{
			this.CmdName = "";
		}
	}
}
