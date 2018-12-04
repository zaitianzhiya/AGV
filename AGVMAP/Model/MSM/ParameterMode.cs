using System;

namespace Model.MSM
{
	public class ParameterMode
	{
		public string ParameterCode
		{
			get;
			set;
		}

		public string ParameterName
		{
			get;
			set;
		}

		public string DefaultValue
		{
			get;
			set;
		}

		public int ExitType
		{
			get;
			set;
		}

		public string ChooseValues
		{
			get;
			set;
		}

		public ParameterMode()
		{
			this.ParameterCode = "";
			this.ParameterName = "";
			this.DefaultValue = "";
			this.ChooseValues = "";
		}
	}
}
