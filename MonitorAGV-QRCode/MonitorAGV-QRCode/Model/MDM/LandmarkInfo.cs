using System;
using System.Collections.Generic;

namespace Model.MDM
{
	[Serializable]
	public class LandmarkInfo
	{
		public int LandmarkID
		{
			get;
			set;
		}

		public string LandmarkCode
		{
			get;
			set;
		}

		public string LandmarkName
		{
			get;
			set;
		}

		public double LandX
		{
			get;
			set;
		}

		public double LandY
		{
			get;
			set;
		}

		public double LandMidX
		{
			get;
			set;
		}

		public double LandMidY
		{
			get;
			set;
		}

		public int IsWaitmark
		{
			get;
			set;
		}

		public int AreaType
		{
			get;
			set;
		}

		public int IndexX
		{
			get;
			set;
		}

		public int IndexY
		{
			get;
			set;
		}

		public int IsDelivery
		{
			get;
			set;
		}

		public int DeliverCode
		{
			get;
			set;
		}

		public int IsSorting
		{
			get;
			set;
		}

		public int SortingCode
		{
			get;
			set;
		}

		public int IsInflectionPoint
		{
			get;
			set;
		}

		public int IsWaitPoint
		{
			get;
			set;
		}

		public IList<CmdInfo> Acts
		{
			get;
			set;
		}

		public string LandMarkName
		{
			get;
			set;
		}

		public SwayEnum sway
		{
			get;
			set;
		}

		public int Angle
		{
			get;
			set;
		}

		public bool IsRotateLand
		{
			get;
			set;
		}

		public bool IsBack
		{
			get;
			set;
		}

		public LandmarkInfo()
		{
			this.LandmarkCode = "";
			this.Acts = new List<CmdInfo>();
			this.LandMarkName = "";
			this.IsBack = false;
			this.IsRotateLand = false;
			this.sway = SwayEnum.None;
		}
	}
}
