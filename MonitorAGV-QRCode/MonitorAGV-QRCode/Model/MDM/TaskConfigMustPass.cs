using System;

namespace Model.MDM
{
	[Serializable]
	public class TaskConfigMustPass
	{
		public string TaskConditonCode
		{
			get;
			set;
		}

		public int TaskConfigDetailID
		{
			get;
			set;
		}

		public int DetailID
		{
			get;
			set;
		}

		public string MustPassLandCode
		{
			get;
			set;
		}

		public int Action
		{
			get;
			set;
		}

		public TaskConfigMustPass()
		{
			this.TaskConditonCode = "";
			this.MustPassLandCode = "";
		}
	}
}
