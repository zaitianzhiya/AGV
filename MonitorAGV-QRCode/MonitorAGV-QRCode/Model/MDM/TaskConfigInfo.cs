using System;
using System.Collections.Generic;

namespace Model.MDM
{
	[Serializable]
	public class TaskConfigInfo
	{
		public string TaskConditonCode
		{
			get;
			set;
		}

		public string TaskConditonName
		{
			get;
			set;
		}

		public bool IsNew
		{
			get;
			set;
		}

		public IList<TaskConfigDetail> TaskConfigDetail
		{
			get;
			set;
		}

		public TaskConfigInfo()
		{
			this.TaskConditonCode = "";
			this.TaskConditonName = "";
			this.IsNew = false;
			this.TaskConfigDetail = new List<TaskConfigDetail>();
		}
	}
}
