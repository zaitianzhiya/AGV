using System;

namespace Model.Comoon
{
	public class SysOperButton
	{
		public string Button
		{
			get
			{
				bool flag = this.ButtonType == 1;
				string result;
				if (flag)
				{
					result = "服务端";
				}
				else
				{
					bool flag2 = this.ButtonType == 0;
					if (flag2)
					{
						result = "客户端";
					}
					else
					{
						result = "";
					}
				}
				return result;
			}
		}

		public int ButtonType
		{
			get;
			set;
		}

		public string ButtonTypeStr
		{
			get
			{
				bool flag = this.ButtonType == 0;
				string result;
				if (flag)
				{
					result = "客户端";
				}
				else
				{
					result = "服务端";
				}
				return result;
			}
		}

		public bool IsSelect
		{
			get;
			set;
		}

		public string ButtonName
		{
			get;
			set;
		}

		public string ButtonCaption
		{
			get;
			set;
		}

		public SysOperButton()
		{
			this.ButtonName = "";
			this.ButtonCaption = "";
		}
	}
}
