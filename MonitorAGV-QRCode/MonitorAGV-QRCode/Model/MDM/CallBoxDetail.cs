using System;

namespace Model.MDM
{
	[Serializable]
	public class CallBoxDetail
	{
		public int CallBoxID
		{
			get;
			set;
		}

		public int ButtonID
		{
			get;
			set;
		}

		public string TaskConditonCode
		{
			get;
			set;
		}

		public int OperaType
		{
			get;
			set;
		}

		public int LocationID
		{
			get;
			set;
		}

		public int LocationState
		{
			get;
			set;
		}

		public string OperTypeStr
		{
			get
			{
				string result;
				switch (this.OperaType)
				{
				case 0:
					result = "呼叫或监控";
					break;
				case 1:
					result = "放行";
					break;
				case 2:
					result = "取消最近一次呼叫";
					break;
				default:
					result = "";
					break;
				}
				return result;
			}
			set
			{
				if (!(value == "呼叫或监控"))
				{
					if (!(value == "放行"))
					{
						if (!(value == "取消最近一次呼叫"))
						{
							this.OperaType = -1;
						}
						else
						{
							this.OperaType = 2;
						}
					}
					else
					{
						this.OperaType = 1;
					}
				}
				else
				{
					this.OperaType = 0;
				}
			}
		}

		public string LocationStateStr
		{
			get
			{
				string result;
				switch (this.LocationState)
				{
				case 0:
					result = "空位置";
					break;
				case 1:
					result = "空料架";
					break;
				case 2:
					result = "满料架";
					break;
				default:
					result = "";
					break;
				}
				return result;
			}
			set
			{
				if (!(value == "空位置"))
				{
					if (!(value == "空料架"))
					{
						if (!(value == "满料架"))
						{
							this.LocationState = -1;
						}
						else
						{
							this.LocationState = 2;
						}
					}
					else
					{
						this.LocationState = 1;
					}
				}
				else
				{
					this.LocationState = 0;
				}
			}
		}

		public CallBoxDetail()
		{
			this.TaskConditonCode = "";
		}
	}
}
