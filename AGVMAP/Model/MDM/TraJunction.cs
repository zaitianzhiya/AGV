using System;

namespace Model.MDM
{
	[Serializable]
	public class TraJunction
	{
		public int TraJunctionID
		{
			get;
			set;
		}

		public int Carnumber
		{
			get;
			set;
		}

		public string JunctionLandMarkCodes
		{
			get;
			set;
		}

		public TraJunction()
		{
			this.TraJunctionID = 1;
			this.Carnumber = 1;
			this.JunctionLandMarkCodes = "";
		}
	}
}
