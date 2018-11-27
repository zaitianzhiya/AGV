using System;
using System.Collections.Generic;

namespace Model.MDM
{
	[Serializable]
	public class CallBoxInfo
	{
		public bool IsNew
		{
			get;
			set;
		}

		public int CallBoxID
		{
			get;
			set;
		}

		public string CallBoxName
		{
			get;
			set;
		}

		public int CallBoxType
		{
			get;
			set;
		}

		public string CallBoxTypeUI
		{
			get
			{
				int callBoxType = this.CallBoxType;
				string result;
				if (callBoxType != 0)
				{
					if (callBoxType != 1)
					{
						result = "";
					}
					else
					{
						result = "监控";
					}
				}
				else
				{
					result = "呼叫";
				}
				return result;
			}
			set
			{
				if (!(value == "呼叫"))
				{
					if (!(value == "监控"))
					{
						this.CallBoxType = -1;
					}
					else
					{
						this.CallBoxType = 1;
					}
				}
				else
				{
					this.CallBoxType = 0;
				}
			}
		}

		public IList<CallBoxDetail> CallBoxDetails
		{
			get;
			set;
		}

		public CallBoxInfo()
		{
			this.CallBoxDetails = new List<CallBoxDetail>();
			this.CallBoxType = -1;
			this.CallBoxName = "";
			this.IsNew = false;
		}
	}
}
