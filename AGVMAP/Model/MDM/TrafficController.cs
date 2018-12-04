using System;

namespace Model.MDM
{
	[Serializable]
	public class TrafficController
	{
		public int JunctionID
		{
			get;
			set;
		}

		public string EnterP
		{
			get;
			set;
		}

		public string EnterLandCode
		{
			get;
			set;
		}

		public string JunctionP
		{
			get;
			set;
		}

		public string JunctionLandMarkCodes
		{
			get;
			set;
		}

		public string RealseLandMarkCode
		{
			get;
			set;
		}

		public TrafficController()
		{
			this.EnterP = "";
			this.EnterLandCode = "";
			this.JunctionP = "";
			this.JunctionLandMarkCodes = "";
			this.RealseLandMarkCode = "";
		}
	}
}
