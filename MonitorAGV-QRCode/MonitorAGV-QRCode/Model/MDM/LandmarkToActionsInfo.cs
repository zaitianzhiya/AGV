using System;
using System.Collections.Generic;

namespace Model.MDM
{
	public class LandmarkToActionsInfo
	{
		public string LandCode
		{
			get;
			set;
		}

		public IList<RouteFragmentConfigInfo> ActionList
		{
			get;
			set;
		}

		public LandmarkToActionsInfo()
		{
			this.LandCode = "";
			this.ActionList = new List<RouteFragmentConfigInfo>();
		}
	}
}
