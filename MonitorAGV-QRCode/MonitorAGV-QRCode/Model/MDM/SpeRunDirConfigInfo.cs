using System;

namespace Model.MDM
{
	public class SpeRunDirConfigInfo
	{
		public string Fragment
		{
			get;
			set;
		}

		public int Dir
		{
			get;
			set;
		}

		public SpeRunDirConfigInfo()
		{
			this.Fragment = "";
			this.Dir = 0;
		}
	}
}
