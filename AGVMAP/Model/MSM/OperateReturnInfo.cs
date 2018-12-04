using System;

namespace Model.MSM
{
	public class OperateReturnInfo
	{
		private OperateCodeEnum returnCode;

		private object returnInfo;

		private string adviceInfo;

		public OperateCodeEnum ReturnCode
		{
			get
			{
				return this.returnCode;
			}
			set
			{
				this.returnCode = value;
			}
		}

		public object ReturnInfo
		{
			get
			{
				bool flag = this.returnInfo == null;
				object empty;
				if (flag)
				{
					empty = string.Empty;
				}
				else
				{
					empty = this.returnInfo;
				}
				return empty;
			}
			set
			{
				this.returnInfo = value;
			}
		}

		public string AdviceInfo
		{
			get
			{
				return this.adviceInfo;
			}
			set
			{
				this.adviceInfo = value;
			}
		}

		public OperateReturnInfo()
		{
		}

		public OperateReturnInfo(OperateCodeEnum returnCode)
		{
			this.returnCode = returnCode;
		}

		public OperateReturnInfo(OperateCodeEnum returnCode, object returnInfo)
		{
			this.returnCode = returnCode;
			this.returnInfo = returnInfo;
		}
	}
}
