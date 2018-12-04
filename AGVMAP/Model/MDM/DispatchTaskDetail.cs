using System;

namespace Model.MDM
{
	[Serializable]
	public class DispatchTaskDetail
	{
		public bool IsSelect
		{
			get;
			set;
		}

		public string dispatchNo
		{
			get;
			set;
		}

		public int DetailID
		{
			get;
			set;
		}

		public string LandCode
		{
			get;
			set;
		}

		public int OperType
		{
			get;
			set;
		}

		public string OperTypeStr
		{
			get
			{
				string result;
				switch (this.OperType)
				{
				case 0:
					result = "降平台";
					break;
				case 1:
					result = "升平台";
					break;
				case 2:
					result = "自动充电";
					break;
				default:
					result = "";
					break;
				}
				return result;
			}
		}

		public int IsAllowExcute
		{
			get;
			set;
		}

		public int State
		{
			get;
			set;
		}

		public int PutType
		{
			get;
			set;
		}

		public DispatchTaskDetail()
		{
			this.dispatchNo = "";
			this.LandCode = "";
		}
	}
}
