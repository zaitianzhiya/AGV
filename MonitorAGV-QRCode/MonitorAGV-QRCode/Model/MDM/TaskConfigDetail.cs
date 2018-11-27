using System;
using System.Collections.Generic;

namespace Model.MDM
{
	[Serializable]
	public class TaskConfigDetail
	{
		public string TaskConditonCode
		{
			get;
			set;
		}

		public int DetailID
		{
			get;
			set;
		}

		public int ArmOwnArea
		{
			get;
			set;
		}

		public int StorageState
		{
			get;
			set;
		}

		public string StorageStateStr
		{
			get
			{
				int storageState = this.StorageState;
				string result;
				if (storageState != 0)
				{
					if (storageState != 1)
					{
						result = "满料架";
					}
					else
					{
						result = "空料架";
					}
				}
				else
				{
					result = "空储位";
				}
				return result;
			}
			set
			{
				if (!(value == "空储位"))
				{
					if (!(value == "空料架"))
					{
						this.StorageState = 2;
					}
					else
					{
						this.StorageState = 1;
					}
				}
				else
				{
					this.StorageState = 0;
				}
			}
		}

		public int MaterialType
		{
			get;
			set;
		}

		public int Action
		{
			get;
			set;
		}

		public int IsWaitPass
		{
			get;
			set;
		}

		public string ActionUI
		{
			get
			{
				int action = this.Action;
				string result;
				if (action != 0)
				{
					if (action != 1)
					{
						result = "";
					}
					else
					{
						result = "放料";
					}
				}
				else
				{
					result = "取料";
				}
				return result;
			}
			set
			{
				if (!(value == "取料"))
				{
					if (!(value == "放料"))
					{
						this.Action = -1;
					}
					else
					{
						this.Action = 1;
					}
				}
				else
				{
					this.Action = 0;
				}
			}
		}

		public string IsWaitPassUI
		{
			get
			{
				int isWaitPass = this.IsWaitPass;
				string result;
				if (isWaitPass != 0)
				{
					if (isWaitPass != 1)
					{
						result = "";
					}
					else
					{
						result = "是";
					}
				}
				else
				{
					result = "否";
				}
				return result;
			}
			set
			{
				if (!(value == "否"))
				{
					if (!(value == "是"))
					{
						this.IsWaitPass = -1;
					}
					else
					{
						this.IsWaitPass = 1;
					}
				}
				else
				{
					this.IsWaitPass = 0;
				}
			}
		}

		public IList<TaskConfigMustPass> TaskConfigMustPass
		{
			get;
			set;
		}

		public TaskConfigDetail()
		{
			this.TaskConditonCode = "";
			this.TaskConfigMustPass = new List<TaskConfigMustPass>();
			this.ArmOwnArea = -1;
		}
	}
}
