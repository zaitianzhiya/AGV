using System;

namespace Model.MSM
{
	public class SysParameter
	{
		public string ParameterCode
		{
			get;
			set;
		}

		public string ParameterValue
		{
			get;
			set;
		}

		public string ParameterName
		{
			get;
			set;
		}

		public SysParameter()
		{
			this.ParameterCode = "";
			this.ParameterValue = "";
			this.ParameterName = "";
		}
	}
}
