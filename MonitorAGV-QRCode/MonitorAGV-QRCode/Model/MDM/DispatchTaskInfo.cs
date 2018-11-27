using System;
using System.Collections.Generic;

namespace Model.MDM
{
	[Serializable]
	public class DispatchTaskInfo
	{
		public int Site
		{
			get;
			set;
		}

		public string dispatchNo
		{
			get;
			set;
		}

		public int stationNo
		{
			get;
			set;
		}

		public int taskType
		{
			get;
			set;
		}

		public int matterType
		{
			get;
			set;
		}

		public int ExeAgvID
		{
			get;
			set;
		}

		public string BuildTime
		{
			get;
			set;
		}

		public int TaskState
		{
			get;
			set;
		}

		public string TaskStateStr
		{
			get
			{
				string result;
				switch (this.TaskState)
				{
				case 0:
					result = "未执行";
					break;
				case 1:
					result = "执行中";
					break;
				case 2:
					result = "已完成";
					break;
				default:
					result = "已超时";
					break;
				}
				return result;
			}
		}

		public string FinishTime
		{
			get;
			set;
		}

		public string taskTypeStr
		{
			get
			{
				bool flag = this.taskType == 0;
				string result;
				if (flag)
				{
					result = "叫料";
				}
				else
				{
					result = "下料";
				}
				return result;
			}
		}

		public string ExeAgv
		{
			get
			{
				bool flag = this.ExeAgvID == 0;
				string result;
				if (flag)
				{
					result = "";
				}
				else
				{
					result = this.ExeAgvID.ToString() + "号AGV";
				}
				return result;
			}
		}

		public string CallLand
		{
			get;
			set;
		}

		public bool IsSelect
		{
			get;
			set;
		}

		public IList<DispatchTaskDetail> TaskDetail
		{
			get;
			set;
		}

		public DispatchTaskInfo()
		{
			this.dispatchNo = "";
			this.stationNo = 0;
			this.matterType = 0;
			this.ExeAgvID = 0;
			this.BuildTime = "";
			this.TaskState = 0;
			this.FinishTime = "";
			this.CallLand = "";
			this.TaskDetail = new List<DispatchTaskDetail>();
		}
	}
}
