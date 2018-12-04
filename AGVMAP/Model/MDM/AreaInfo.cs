using System;

namespace Model.MDM
{
	[Serializable]
	public class AreaInfo
	{
		public int OwnArea
		{
			get;
			set;
		}

		public string AreaName
		{
			get;
			set;
		}

		public AreaInfo()
		{
			this.AreaName = "";
		}
	}
}
