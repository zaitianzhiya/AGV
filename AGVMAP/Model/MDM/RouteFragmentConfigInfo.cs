using System;

namespace Model.MDM
{
	[Serializable]
	public class RouteFragmentConfigInfo
	{
		public string Fragment
		{
			get;
			set;
		}

		public string ActionLandMark
		{
			get;
			set;
		}

		public int CmdCode
		{
			get;
			set;
		}

		public int CmdPara
		{
			get;
			set;
		}

		public int CmdIndex
		{
			get;
			set;
		}

		public string CmdName
		{
			get;
			set;
		}

		public RouteFragmentConfigInfo()
		{
			this.CmdName = "";
			this.Fragment = "";
			this.ActionLandMark = "";
			this.CmdCode = 0;
			this.CmdIndex = 0;
			this.CmdPara = 0;
		}
	}
}
