using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Model.MDM
{
	[Serializable]
	public class RouteInfo
	{
		public bool IsNew
		{
			get;
			set;
		}

		public int RouteID
		{
			get;
			set;
		}

		public string RouteName
		{
			get;
			set;
		}

		public RouteTypeEnum RouteType
		{
			get;
			set;
		}

		public string RouteTypeStr
		{
			get
			{
				return (this.RouteTypeInt == 0) ? "工作路线" : "充电路线";
			}
			set
			{
				this.RouteTypeInt = (value.Equals("工作路线") ? 0 : 1);
			}
		}

		public int RouteTypeInt
		{
			get;
			set;
		}

		public List<SegmentResInfo> SegmentResList
		{
			get;
			set;
		}

		public LandmarkInfo LastLandmark
		{
			get
			{
				bool flag = this.LandMarkList != null && this.LandMarkList.Count > 0;
				LandmarkInfo result;
				if (flag)
				{
					result = this.LandMarkList[this.LandMarkList.Count];
				}
				else
				{
					result = new LandmarkInfo();
				}
				return result;
			}
		}

		public LandmarkInfo FistLandmark
		{
			get
			{
				bool flag = this.LandMarkList != null && this.LandMarkList.Count > 0;
				LandmarkInfo result;
				if (flag)
				{
					result = this.LandMarkList[0];
				}
				else
				{
					result = new LandmarkInfo();
				}
				return result;
			}
		}

		public SegmentResInfo LastSegmentRes
		{
			get
			{
                return (
                   from k in SegmentResList
                   orderby k.ORD descending
                   select k).FirstOrDefault<SegmentResInfo>();
			}
		}

		public SegmentResInfo FirstSegmentRes
		{
			get
			{
                return (
                  from k in this.SegmentResList
                  orderby k.ORD
                  select k).FirstOrDefault<SegmentResInfo>();
			}
		}

		public string LandCodeStr
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public IList<LandmarkInfo> LandMarkList
		{
			get;
			set;
		}

		public IList<string> LandmarkListStr
		{
			get;
			set;
		}

		public int StationNo
		{
			get;
			set;
		}

		public int DirectionForStation
		{
			get;
			set;
		}

		public string DirectionUI
		{
			get
			{
				return (this.DirectionForStation == 0) ? "往站点去" : "从站点出";
			}
			set
			{
				this.DirectionForStation = ((value == "往站点去") ? 0 : 1);
			}
		}

		public IList<CarInfo> ToCars
		{
			get;
			set;
		}

		public RouteInfo()
		{
			this.RouteName = "";
			this.LandCodeStr = "";
			this.FileName = "";
			this.RouteID = 1;
			this.LandMarkList = new List<LandmarkInfo>();
			this.LandmarkListStr = new List<string>();
			this.ToCars = new List<CarInfo>();
			this.IsNew = true;
		}
	}
}
