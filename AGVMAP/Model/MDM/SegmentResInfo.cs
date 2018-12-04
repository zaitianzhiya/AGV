using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.MDM
{
	[Serializable]
	public class SegmentResInfo
	{
		public int RouteID
		{
			get;
			set;
		}

		public int SegmentResID
		{
			get;
			set;
		}

		public int StartLandMarkID
		{
			get;
			set;
		}

		public int EndLandMarkID
		{
			get;
			set;
		}

		public LandmarkInfo StartLandMark
		{
			get;
			set;
		}

		public LandmarkInfo EndLandMark
		{
			get;
			set;
		}

		public List<LandmarkInfo> LinkLandMarks
		{
			get;
			set;
		}

		public List<LandmarkInfo> AllLandmarks
		{
			get
			{
				return this.LinkLandMarks.Union(new List<LandmarkInfo>
				{
					this.StartLandMark,
					this.EndLandMark
				}).ToList<LandmarkInfo>();
			}
		}

		public bool IsNullOrEmpty
		{
			get
			{
				bool flag = this.StartLandMark == null;
				return flag || string.IsNullOrEmpty(this.StartLandMark.LandmarkCode);
			}
		}

		public int ORD
		{
			get;
			set;
		}

		public SegmentResInfo()
		{
			this.StartLandMark = new LandmarkInfo();
			this.EndLandMark = new LandmarkInfo();
			this.LinkLandMarks = new List<LandmarkInfo>();
		}
	}
}
